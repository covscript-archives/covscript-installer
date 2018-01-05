using Microsoft.Win32;
using System;
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
            textBox2.Text="http://covariant.cn/cs";
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
            textBox2.Text = "http://covariant.cn/cs";
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
            Installation inst = new Installation(form.label1, form.progressBar1)
            {
                installation_path = textBox1.Text,
                repo_url = textBox2.Text
            };
            try
            {
                form.Show();
                inst.install();
                Registry.ClassesRoot.CreateSubKey(".csc").SetValue("", "CovScriptGUI.Code", RegistryValueKind.String);
                Registry.ClassesRoot.CreateSubKey("CovScriptGUI.Code").CreateSubKey("Shell\\Open\\Command").SetValue("", "\"" + inst.installation_path + "\\bin\\cs_gui.exe" + "\" \"%1\"", RegistryValueKind.ExpandString);
                Registry.ClassesRoot.CreateSubKey(".csp").SetValue("", "CovScriptGUI.Package", RegistryValueKind.String);
                Registry.ClassesRoot.CreateSubKey("CovScriptGUI.Package").CreateSubKey("Shell\\Open\\Command").SetValue("", "\"" + inst.installation_path + "\\bin\\cs_gui.exe" + "\" \"%1\"", RegistryValueKind.ExpandString);
                Registry.ClassesRoot.CreateSubKey(".cse").SetValue("", "CovScriptGUI.Extension", RegistryValueKind.String);
                Registry.ClassesRoot.CreateSubKey("CovScriptGUI.Extension").CreateSubKey("Shell\\Open\\Command").SetValue("", "\"" + inst.installation_path + "\\bin\\cs_gui.exe" + "\" \"%1\"", RegistryValueKind.ExpandString);
                if (checkBox7.Checked)
                {
                    Environment.SetEnvironmentVariable("COVSCRIPT_HOME", inst.installation_path, EnvironmentVariableTarget.User);
                    Environment.SetEnvironmentVariable("CS_IMPORT_PATH", inst.installation_path + "\\imports", EnvironmentVariableTarget.User);
                    bool exist = false;
                    string value = inst.installation_path + "\\bin";
                    string env = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                    if (env != null)
                    {
                        foreach (string val in env.Split(';'))
                        {
                            if (val == value)
                            {
                                exist = true;
                                break;
                            }
                        }
                    }
                    if (!exist)
                        Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) + ";" + inst.installation_path + "\\bin", EnvironmentVariableTarget.User);
                }
                if(checkBox8.Checked)
                {
                    create_shotcut(inst.installation_path + "\\bin\\cs_inst.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript Installer.lnk", "CovScript安装程序", "");
                    create_shotcut(inst.installation_path + "\\bin\\cs_gui.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript GUI.lnk", "CovScript GUI", "");
                    create_shotcut(inst.installation_path + "\\bin\\cs_repl.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript REPL.lnk", "CovScript交互式解释器", "--wait-before-exit --import-path \"" + inst.installation_path + "\\imports\" --log-path \"" + inst.installation_path + "\\logs\\cs_repl_runtime.log\"");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("安装失败", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.Close();
                return;
            }
            MessageBox.Show("安装完毕", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form.Close();
        }
    }
}
