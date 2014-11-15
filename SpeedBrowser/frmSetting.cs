using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;


namespace SpeedBrowser
{
    public partial class frmSetting : Form
    {
        IniFile config;
        public const string strRegPath = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        public const string strRegKey = "SpeedBrowser.exe";
        private RegistryKey reg;
        private int shitver;
        private int[] vers = {0,7000, 8000, 8888, 9000, 9999, 10000, 10001, 11000, 11001 };
        public frmSetting()
        {
            InitializeComponent();
        }

        private void trSpeed_Scroll(object sender, EventArgs e)
        {
            int v = (sender as TrackBar).Value;
            txtSpeed.Text = (v >= 0 ? v : (100.0+v)/100).ToString();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            config = (this.Owner as frmBrowser).config;
            txtIndex.Text = config.GetKeyValue("main", "index");
            double speed = 1;
            try
            {
                speed = double.Parse(config.GetKeyValue("main", "speed"));
            }
            catch { }
            txtSpeed.Text = speed.ToString();
            if (speed >= 0.01 && speed <= 100)
                trSpeed.Value = (int)((speed >= 0) ? speed : speed * 100 - 100);
            reg = Registry.CurrentUser.OpenSubKey(strRegPath,true);
            int a = 0;
            try
            {
                a = (int)reg.GetValue(strRegKey);
            }
            catch { }
            int x=(int)Array.IndexOf<int>(vers, a);
            if (x == -1) x = 0;
            comboIE.SelectedIndex = x;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnon_Click(object sender, EventArgs e)
        {
            double speed = 0;
            try
            {
                speed = double.Parse(txtSpeed.Text);
            }
            catch { }
            if (speed != 0)
            {
                config.SetKeyValue("main", "speed", speed.ToString());
                frmBrowser.SetSpeed(speed);
                (this.Owner as frmBrowser).setComboxText(speed.ToString());
            }
            config.SetKeyValue("main", "index", txtIndex.Text);
            (this.Owner as frmBrowser).indexpage = txtIndex.Text;
            config.Save(frmBrowser.CONFIG_FILE);

            if (shitver != comboIE.SelectedIndex)
            {
                shitver = vers[comboIE.SelectedIndex];
                reg.SetValue(strRegKey, shitver);
                MessageBox.Show("您更改了浏览器版本,新的浏览器版本在下次运行本软件时才会生效!");
            }
            this.Close();
        }

        private void btnCurpage_Click(object sender, EventArgs e)
        {
            txtIndex.Text = (this.Owner as frmBrowser).getcurpage();
        }
    }
}
