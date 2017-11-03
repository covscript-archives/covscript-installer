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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
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
            form.Show();
            Installation inst = new Installation(form.label1, form.progressBar1);
            inst.installation_path = textBox1.Text;
            List < Pair < string, string>> field = new List<Pair<string, string>>();
            if (checkBox1.Checked)
                field.Add(new Pair<string, string>("http://ldc.atd3.cn/cs.exe", "\\Bin\\cs.exe"));
            if (checkBox2.Checked)
                field.Add(new Pair<string, string>("http://ldc.atd3.cn/cs_gui.exe", "\\Bin\\cs_gui.exe"));
            if (checkBox3.Checked)
                field.Add(new Pair<string, string>("http://ldc.atd3.cn/cs_repl.exe", "\\Bin\\cs_repl.exe"));
            if (checkBox4.Checked)
                field.Add(new Pair<string, string>("http://ldc.atd3.cn/darwin.cse", "\\Imports\\darwin.cse"));
            if (checkBox5.Checked)
                field.Add(new Pair<string, string>("http://ldc.atd3.cn/sqlite.cse", "\\Imports\\sqlite.cse"));
            inst.installation_field = field;
            try
            {
                inst.install();
                if (checkBox6.Checked)
                {
                    Environment.SetEnvironmentVariable("CS_IMPORT_PATH", inst.installation_path + "\\Imports", EnvironmentVariableTarget.User);
                    Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) + ";" + inst.installation_path + "\\Bin", EnvironmentVariableTarget.User);
                }
                if(checkBox7.Checked)
                {
                    if (checkBox2.Checked)
                        create_shotcut(inst.installation_path + "\\Bin\\cs_gui.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript GUI.lnk", "Start CovScript GUI", "");
                    if (checkBox3.Checked)
                        create_shotcut(inst.installation_path + "\\Bin\\cs_repl.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript REPL.lnk", "Start CovScript REPL", "--wait-before-exit --import-path \"" + inst.installation_path + "\\Imports\" --log-path \"" + inst.installation_path + "\\Logs\\cs_repl_runtime.log\"");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Installation Failed", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Installation Finished", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form.Close();
        }
    }
}
