using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Covariant_Script_Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\CovScript";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://covscript.org");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((new Form2(this)).ShowDialog() == DialogResult.OK)
            {
                this.Hide();
                this.install();
                this.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void create_shotcut(string path,string link,string description,string args)
        {
            File.Delete(link);
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(link);
            shortcut.TargetPath = path;
            shortcut.Arguments = args;
            shortcut.Description = description;
            shortcut.WorkingDirectory = Path.GetDirectoryName(path);
            shortcut.IconLocation = path;
            shortcut.WindowStyle = 1;
            shortcut.Save();
        }

        public void install()
        {
            Form3 form = new Form3();
            Installation inst = new Installation(form.label1, form.progressBar1);
            inst.installation_path = textBox1.Text;
            List < Pair < string, string>> field = new List<Pair<string, string>>();
            if (checkBox1.Checked)
                field.Add(new Pair<string, string>(Environment.Is64BitOperatingSystem ? "http://ldc.atd3.cn/build_x64/bin/cs.exe" : "http://ldc.atd3.cn/build_x86/bin/cs.exe", "\\Bin\\cs.exe"));
            if (checkBox2.Checked)
                field.Add(new Pair<string, string>("http://ldc.atd3.cn/cs_gui.exe", "\\Bin\\cs_gui.exe"));
            if (checkBox3.Checked)
                field.Add(new Pair<string, string>(Environment.Is64BitOperatingSystem ? "http://ldc.atd3.cn/build_x64/bin/cs_repl.exe" : "http://ldc.atd3.cn/build_x86/bin/cs_repl.exe", "\\Bin\\cs_repl.exe"));
            if (checkBox4.Checked)
                field.Add(new Pair<string, string>(Environment.Is64BitOperatingSystem ? "http://ldc.atd3.cn/build_x64/imports/regex.cse" : "http://ldc.atd3.cn/build_x86/imports/regex.cse", "\\Imports\\regex.cse"));
            if (checkBox5.Checked)
                field.Add(new Pair<string, string>(Environment.Is64BitOperatingSystem ? "http://ldc.atd3.cn/build_x64/imports/darwin.cse" : "http://ldc.atd3.cn/build_x86/imports/darwin.cse", "\\Imports\\darwin.cse"));
            if (checkBox6.Checked)
                field.Add(new Pair<string, string>(Environment.Is64BitOperatingSystem ? "http://ldc.atd3.cn/build_x64/imports/sqlite.cse" : "http://ldc.atd3.cn/build_x86/imports/sqlite.cse", "\\Imports\\sqlite.cse"));
            inst.installation_field = field;
            try
            {
                form.Show();
                inst.install();
                File.Copy(Application.ExecutablePath, inst.installation_path + "\\Bin\\cs_inst.exe", true);
                if (checkBox7.Checked)
                {
                    Environment.SetEnvironmentVariable("CS_IMPORT_PATH", inst.installation_path + "\\Imports", EnvironmentVariableTarget.User);
                    bool exist = false;
                    string value = inst.installation_path + "\\Bin";
                    foreach(string val in Environment.GetEnvironmentVariable("PATH",EnvironmentVariableTarget.User).Split(';'))
                    {
                        if(val==value)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) + ";" + inst.installation_path + "\\Bin", EnvironmentVariableTarget.User);
                }
                if(checkBox8.Checked)
                {
                    create_shotcut(inst.installation_path + "\\Bin\\cs_inst.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript Installer.lnk", "CovScript安装程序", "");
                    if (checkBox2.Checked)
                        create_shotcut(inst.installation_path + "\\Bin\\cs_gui.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript GUI.lnk", "CovScript GUI", "");
                    if (checkBox3.Checked)
                        create_shotcut(inst.installation_path + "\\Bin\\cs_repl.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript REPL.lnk", "CovScript交互式解释器", "--wait-before-exit --import-path \"" + inst.installation_path + "\\Imports\" --log-path \"" + inst.installation_path + "\\Logs\\cs_repl_runtime.log\"");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("安装失败", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("安装完毕", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form.Close();
        }
    }
}
