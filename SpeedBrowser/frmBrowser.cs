using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading;
using System.Net;
using System.IO; 

namespace SpeedBrowser
{
    public partial class frmBrowser : Form
    {
        [DllImport("SpeedHook.dll")]
        public static extern void SetSpeed(double speed);

        public IniFile config = new IniFile();
        public string indexpage;
        public int tabindex;
        public const int TAB_TEXT_MAXLENGTH=10;
        public string BROWSER_TITLE = "变速浏览器";
        public const string CONFIG_FILE = "SpeedBrowser.cfg";
        public bool tabmiddle = false;

        public static string cutstring(string s)
        {
            if (s.Length <= TAB_TEXT_MAXLENGTH)
                return s;
            else
                return s.Substring(0,TAB_TEXT_MAXLENGTH - 3) + "...";
        }
        public frmBrowser()
        {
            InitializeComponent();
        }

        public void title(string t)
        {
            if (t == null || t=="") t = "Loading...";
            this.Text=t+" - "+BROWSER_TITLE;

        }
        public WebBrowser newtab(string url = "")
        {

            webBrowser1.Add(new WebBrowser());

            tabPage1.Add(new TabPage());
            int wbindex = webBrowser1.Count - 1;
            int tbindex=tabPage1.Count - 1;
            tabindex = tbindex;

            WebBrowser twb = webBrowser1[wbindex];

            TabPage tpage = tabPage1[tbindex];

            twb.Tag = tbindex;
            twb.Dock = System.Windows.Forms.DockStyle.Fill;
            twb.Name = "webBrowser" + wbindex.ToString();
            twb.DocumentTitleChanged += new EventHandler(this.webBrowser1_DocumentTitleChanged);
            twb.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);

            twb.ScriptErrorsSuppressed = true;
            if(url!="null")
                twb.Navigate(url == "" ? indexpage : url);

            twb.TabIndex = tbindex;
            
            tpage.Tag = wbindex;
            tpage.Controls.Add(twb);
            tpage.Dock = System.Windows.Forms.DockStyle.Fill;
            tpage.Location = new System.Drawing.Point(4, 25);
            tpage.Name = "tabPage" + tbindex.ToString();
            tpage.Padding = new System.Windows.Forms.Padding(3);
            tpage.Size = new System.Drawing.Size(994, 365);
            tpage.TabIndex = tbindex;


            tpage.Text = "Loading...";
            tpage.UseVisualStyleBackColor = true;
            tabControl1.Controls.Add(tpage);
            tabControl1.SelectedTab = tpage;
            (twb.ActiveXInstance as SHDocVw.WebBrowser).NewWindow2 += new SHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(webBrowser1_NewWindow2);
            return twb;

        }
        public void closetab(int tbindex)
        {
            int newindex = tbindex-1;
            while (newindex >= 0 && tabPage1[newindex] == null)
                newindex--;
            if (newindex < 0)
            {
                newindex = tbindex + 1;
                while (newindex < tabPage1.Count && tabPage1[newindex] == null)
                    newindex++;
                if (newindex == tabPage1.Count) return;

            }
            tabControl1.TabPages.Remove(tabPage1[tbindex]);
            tabControl1.SelectedTab = tabPage1[newindex];
            tabPage1[tbindex].Dispose();
            tabPage1[tbindex] = null;
            webBrowser1[tbindex].Dispose();
            webBrowser1[tbindex] = null;
        }

        private bool mysetspeed(string s)
        {
            double ds=0;
            try
            {
                ds = double.Parse(s);
            }catch{
                return false;
            }
            if (ds != 0)
            {
                SetSpeed(ds);
                config.SetKeyValue("main", "speed", s);
                return true;
            }
            return false;
        }
        private void webBrowser1_NewWindow2(ref object ppDisp, ref bool Cancel)
        {
            ppDisp = newtab("null").ActiveXInstance;
        }
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            int mytag = (int)(sender as WebBrowser).Tag;
            if (tabindex == mytag)
                toolStripTextBox1.Text = (sender as WebBrowser).Url.OriginalString;
        }

        public string getcurpage()
        {
            return toolStripTextBox1.Text;
        }
        private void webBrowser1_DocumentTitleChanged(object sender, EventArgs e)
        {
            int mytag=(int)(sender as WebBrowser).Tag;
            string title = (sender as WebBrowser).DocumentTitle;
            TabPage tab = tabPage1[mytag];
            tab.Text = cutstring(title);
            tab.ToolTipText = title;
            if (tabindex == mytag)
                this.title(title);
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                Rectangle rect;
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    rect = tabControl1.GetTabRect(i);
                    if (e.X <= rect.Right && e.X >= rect.Left && e.Y <= rect.Bottom && e.Y >= rect.Top)
                    {
                        closetab((int)tabControl1.TabPages[i].Tag);
                        return;
                    }
                }
                    
            }
  
        }
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            tabindex = (int)e.TabPage.Tag;

            WebBrowser wb = webBrowser1[tabindex];
            if (wb.Url != null) toolStripTextBox1.Text = wb.Url.OriginalString;

            this.title(wb.DocumentTitle);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1[tabindex].GoForward();
        }
        private static RegistryKey regopen(string regpath)
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(regpath, true);
            if(reg!=null)return reg;
            string root1=regpath.Substring(0, regpath.LastIndexOf('\\') );
            string sub1=regpath.Substring(regpath.LastIndexOf('\\') + 1);
            reg=regopen(root1);
            reg.CreateSubKey(sub1);
            return reg.OpenSubKey(sub1, true);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(Register.GetRegCode(Register.GetMcode()));
            RegistryKey reg = regopen(frmSetting.strRegPath);
            if (reg.GetValue(frmSetting.strRegKey) == null)
                reg.SetValue(frmSetting.strRegKey, 0);
            try
            {
                config.Load(CONFIG_FILE);
            }
            catch { }
            indexpage = config.GetKeyValue("main", "index");
            if (indexpage == "") indexpage = "http://www.baidu.com/";
            string speed = config.GetKeyValue("main", "speed");
            if (mysetspeed(speed))toolStripComboBox1.Text = speed;
            Form1_Resize(sender, e);
            newtab();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            toolStripTextBox1.Size = new System.Drawing.Size(this.Width-122,toolStripTextBox1.Height);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            webBrowser1[tabindex].Navigate(toolStripTextBox1.Text);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            webBrowser1[tabindex].Refresh();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            webBrowser1[tabindex].Stop();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1[tabindex].GoBack();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            webBrowser1[tabindex].Navigate(indexpage);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            newtab();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            closetab(tabindex);
        }


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mysetspeed((sender as ToolStripComboBox).Text);
        }



        private void frmBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            config.Save(CONFIG_FILE);
        }
        public void setComboxText(string s)
        {
            toolStripComboBox1.Text = s;
        }
        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(Keys.Enter))
            {
                mysetspeed((sender as ToolStripComboBox).Text);
                e.Handled = true;
            }
        }
        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(Keys.Enter))
            {
                toolStripButton7_Click(sender, e);
                e.Handled = true;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            (new frmSetting()).ShowDialog(this);
        }


    }
}
