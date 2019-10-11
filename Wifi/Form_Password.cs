using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WifiTool
{
    public partial class Form_Password : Form
    {
        public string Password { get; set; }
        Process kbpr;
        public Form_Password()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.PasswordChar = '\0';//明文显示
            }
            else
            {
                textBox1.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Password = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 8)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        //启动软键盘
        private void StartOSK(bool OpenOrKill)
        {
            string windir = System.AppDomain.CurrentDomain.BaseDirectory;
            string osk = null;

            if (osk == null)
            {
                osk = Path.Combine(windir, "osk.exe");
                if (!File.Exists(osk))
                    osk = null;
            }

            if (osk == null)
            {
                osk = Path.Combine(Path.Combine(windir, "system32"), "osk.exe");
                if (!File.Exists(osk))
                {
                    osk = null;
                }
            }

            if (osk == null)
                osk = "osk.exe";

            try
            {
                if (kbpr == null)
                {
                    kbpr = System.Diagnostics.Process.Start(osk); // 打开系统键盘
                    return;
                }

                if (OpenOrKill)
                {
                    if (kbpr.HasExited)
                        kbpr = System.Diagnostics.Process.Start(osk); // 打开系统键盘
                    return;
                }

                if (!kbpr.HasExited)
                    kbpr.Kill();
            }
            catch (Exception)
            {

            }

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            StartOSK(true);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            StartOSK(false);
        }
    }
}
