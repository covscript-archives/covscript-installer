using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Covariant_Script_Installer
{
    public partial class Form1 : Form
    {
        bool exist = false;
        public Form1()
        {
            InitializeComponent();
            if (Environment.GetEnvironmentVariable("COVSCRIPT_HOME") != null)
            {
                textBox1.Text = Environment.GetEnvironmentVariable("COVSCRIPT_HOME");
                button1.Text = "更新";
                textBox1.Enabled = false;
                button5.Enabled = false;
                exist = true;
            }
            else
                textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\CovScript";
            comboBox1.Text = "http://mirrors.covscript.org.cn/";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://covscript.org.cn");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (exist ? true : (new Form2(this)).ShowDialog() == DialogResult.OK)
            {
                this.Hide();
                this.install();
                button1.Text = "更新";
                textBox1.Enabled = false;
                button5.Enabled = false;
                exist = true;
                this.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "http://mirrors.covscript.org.cn/";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fix();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath.TrimEnd('\\') + "\\CovScript";
        }

        private void create_shotcut(string path, string link, string description, string args)
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

        public void fix()
        {
            Installation inst = new Installation(null, null)
            {
                installation_path = textBox1.Text,
                repo_url = comboBox1.Text
            };
            try
            {
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
                if (checkBox8.Checked)
                {
                    create_shotcut(inst.installation_path + "\\bin\\cs_inst.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript Installer.lnk", "CovScript安装程序", "");
                    create_shotcut(inst.installation_path + "\\bin\\cs_gui.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript GUI.lnk", "CovScript GUI", "");
                    create_shotcut(inst.installation_path + "\\bin\\cs.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript REPL.lnk", "CovScript交互式解释器", "--wait-before-exit --import-path \"" + inst.installation_path + "\\imports\" --log-path \"" + inst.installation_path + "\\logs\\cs_repl_runtime.log\"");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("修复失败", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("修复完毕", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void install()
        {
            if (checkBox1.Checked)
            {
                if (MessageBox.Show("确定要清理安装目录？\n这将会删除其中的所有文件，包括自定义扩展和日志。", "Covariant Script Installer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    return;
            }
            Form3 form = new Form3();
            Installation inst = new Installation(form.label1, form.progressBar1)
            {
                installation_path = textBox1.Text,
                repo_url = comboBox1.Text
            };
            try
            {
                form.Show();
                if (!inst.Install(checkBox1.Checked, checkBox2.Checked, exist))
                {
                    form.Close();
                    return;
                }
                if (!exist)
                {
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
                    if (checkBox8.Checked)
                    {
                        create_shotcut(inst.installation_path + "\\bin\\cs_inst.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript Installer.lnk", "CovScript安装程序", "");
                        create_shotcut(inst.installation_path + "\\bin\\cs_gui.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript GUI.lnk", "CovScript GUI", "");
                        create_shotcut(inst.installation_path + "\\bin\\cs.exe", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript REPL.lnk", "CovScript交互式解释器", "--wait-before-exit --import-path \"" + inst.installation_path + "\\imports\" --log-path \"" + inst.installation_path + "\\logs\\cs_repl_runtime.log\"");
                    }
                }
            }
            catch (Exception e)
            {
                if (exist)
                    MessageBox.Show("更新失败:" + e.ToString(), "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("安装失败:" + e.ToString(), "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.Close();
                return;
            }
            if (exist)
                MessageBox.Show("更新完毕", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("安装完毕", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form.Close();
        }

        private void Remove_dir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        private void Remove_file(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要卸载Covariant Script?\n这会清除所有数据，你可能需要重新下载才能重新使用。", "Covariant Script Installer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Remove_dir(textBox1.Text);
                    Remove_file(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript Installer.lnk");
                    Remove_file(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript GUI.lnk");
                    Remove_file(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CovScript REPL.lnk");
                    Registry.CurrentUser.CreateSubKey("Environment").DeleteValue("COVSCRIPT_HOME", false);
                    Registry.CurrentUser.CreateSubKey("Environment").DeleteValue("CS_IMPORT_PATH", false);
                    Registry.CurrentUser.CreateSubKey("Software").DeleteSubKeyTree("CovScriptGUI", false);
                    Registry.ClassesRoot.DeleteSubKeyTree(".csc", false);
                    Registry.ClassesRoot.DeleteSubKeyTree("CovScriptGUI.Code", false);
                    Registry.ClassesRoot.DeleteSubKeyTree(".csp", false);
                    Registry.ClassesRoot.DeleteSubKeyTree("CovScriptGUI.Package", false);
                    Registry.ClassesRoot.DeleteSubKeyTree(".cse", false);
                    Registry.ClassesRoot.DeleteSubKeyTree("CovScriptGUI.Extension", false);
                    string env = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                    string new_env = "";
                    foreach (string value in env.Split(';'))
                    {
                        if (value.Length != 0 && value != textBox1.Text + "\\bin")
                            new_env += value + ";";
                    }
                    Environment.SetEnvironmentVariable("PATH", new_env.TrimEnd(';'), EnvironmentVariableTarget.User);
                }
                catch (Exception)
                {
                    MessageBox.Show("卸载失败。", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Hide();
                MessageBox.Show("卸载完毕，感谢您的使用。", "Covariant Script Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }
        }
    }
}