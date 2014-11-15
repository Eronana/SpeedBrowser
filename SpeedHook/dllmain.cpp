// dllmain.cpp : 定义 DLL 应用程序的入口点。
#define DETOURS_X86
#include "windows.h"
#include "detours\detours.cpp"
#include "detours\disasm.cpp"
#include "detours\modules.cpp"
#include "detours\creatwth.cpp"
#include "detours\image.cpp"

#pragma   comment(lib, "winmm.lib")
#define DLLEXPORT_API extern "C" _declspec(dllexport)

double SPEED=1;

DWORD x_GetTickCount,h_GetTickCount;
LONG x_GetMessageTime,h_GetMessageTime;
DWORD x_timeGetTime,h_timeGetTime;
LARGE_INTEGER x_QueryPerformanceCounter,h_QueryPerformanceCounter;

DWORD (WINAPI*Sys_GetTickCount)(VOID)=GetTickCount;
DWORD WINAPI Hook_GetTickCount(VOID)
{
	DWORD t=Sys_GetTickCount();
	x_GetTickCount+=(t-h_GetTickCount)*SPEED;
	h_GetTickCount=t;
	return x_GetTickCount;
}
VOID (WINAPI *Sys_Sleep)(_In_ DWORD dwMilliseconds)=Sleep;
VOID WINAPI Hook_Sleep(_In_ DWORD dwMilliseconds)
{
	Sys_Sleep(dwMilliseconds/SPEED);
}
LONG (WINAPI *Sys_GetMessageTime)(VOID)=GetMessageTime;
LONG WINAPI Hook_GetMessageTime(VOID)
{
	LONG t=Sys_GetMessageTime();
	x_GetMessageTime+=(t-h_GetMessageTime)*SPEED;
	h_GetMessageTime=t;
	return x_GetMessageTime;
}
UINT_PTR (WINAPI *Sys_SetTimer)(_In_opt_ HWND hWnd, _In_ UINT_PTR nIDEvent,_In_ UINT uElapse,_In_opt_ TIMERPROC lpTimerFunc)=SetTimer;
UINT_PTR WINAPI Hook_SetTimer(_In_opt_ HWND hWnd, _In_ UINT_PTR nIDEvent,_In_ UINT uElapse,_In_opt_ TIMERPROC lpTimerFunc)
{
	return Sys_SetTimer(hWnd,nIDEvent,uElapse/SPEED,lpTimerFunc);
}

MMRESULT (WINAPI *Sys_timeSetEvent)(_In_ UINT uDelay,_In_ UINT uResolution, _In_ LPTIMECALLBACK fptc,_In_ DWORD_PTR dwUser,_In_ UINT fuEvent)=timeSetEvent;
MMRESULT WINAPI Hook_timeSetEvent(_In_ UINT uDelay,_In_ UINT uResolution, _In_ LPTIMECALLBACK fptc,_In_ DWORD_PTR dwUser,_In_ UINT fuEvent)
{
	return Sys_timeSetEvent(uDelay/SPEED,uResolution,fptc,dwUser,fuEvent);
}

DWORD (WINAPI *Sys_timeGetTime)(void)=timeGetTime;
DWORD WINAPI Hook_timeGetTime(void)
{
	DWORD t=Sys_timeGetTime();
	x_timeGetTime+=(t-h_timeGetTime)*SPEED;
	h_timeGetTime=t;
	return x_timeGetTime;
}

BOOL (WINAPI *Sys_QueryPerformanceCounter)( _Out_ LARGE_INTEGER * lpPerformanceCount)=QueryPerformanceCounter;
BOOL WINAPI Hook_QueryPerformanceCounter( _Out_ LARGE_INTEGER * lpPerformanceCount)
{
	LARGE_INTEGER tmp;
	BOOL result=Sys_QueryPerformanceCounter(&tmp);
	(*lpPerformanceCount).QuadPart=x_QueryPerformanceCounter.QuadPart+=(tmp.QuadPart-h_QueryPerformanceCounter.QuadPart)*SPEED;
	h_QueryPerformanceCounter=tmp;
	return result;
}
void Hook()
{
	DetourRestoreAfterWith();
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());

	//初始化时间
	x_GetTickCount=h_GetTickCount=GetTickCount();
	x_GetMessageTime=h_GetMessageTime=GetMessageTime();
	x_timeGetTime=h_timeGetTime=timeGetTime();
	QueryPerformanceCounter(&x_QueryPerformanceCounter);
	h_QueryPerformanceCounter=x_QueryPerformanceCounter;

	//HOOK 函数列表
	DetourAttach(&(PVOID&)Sys_GetTickCount, Hook_GetTickCount);
	DetourAttach(&(PVOID&)Sys_Sleep, Hook_Sleep);
	DetourAttach(&(PVOID&)Sys_GetMessageTime, Hook_GetMessageTime);
	DetourAttach(&(PVOID&)Sys_SetTimer, Hook_SetTimer);
	DetourAttach(&(PVOID&)Sys_timeSetEvent, Hook_timeSetEvent);
	DetourAttach(&(PVOID&)Sys_timeGetTime, Hook_timeGetTime);
	DetourAttach(&(PVOID&)Sys_QueryPerformanceCounter, Hook_QueryPerformanceCounter);

	DetourTransactionCommit();
}

void UnHook()  
{
    DetourTransactionBegin();  
    DetourUpdateThread(GetCurrentThread());  

	DetourDetach(&(PVOID&)Sys_GetTickCount, Hook_GetTickCount);
	DetourDetach(&(PVOID&)Sys_Sleep, Hook_Sleep);
	DetourDetach(&(PVOID&)Sys_GetMessageTime, Hook_GetMessageTime);
	DetourDetach(&(PVOID&)Sys_SetTimer, Hook_SetTimer);
	DetourDetach(&(PVOID&)Sys_timeSetEvent, Hook_timeSetEvent);
	DetourDetach(&(PVOID&)Sys_timeGetTime, Hook_timeGetTime);
	DetourDetach(&(PVOID&)Sys_QueryPerformanceCounter, Hook_QueryPerformanceCounter);
  
	DetourTransactionCommit();  
}

/*
DLLEXPORT_API double* GetSpeedAddr()
{
	return &SPEED;
}
*/

void __stdcall SetSpeed(double s)
{
		SPEED=s;
}
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		Hook();
		break;
	case DLL_PROCESS_DETACH:
		UnHook();
	}
	return TRUE;
}

