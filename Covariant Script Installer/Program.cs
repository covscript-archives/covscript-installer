using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Covariant_Script_Installer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "--daemon")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                string daemon_exe_path = Path.GetTempPath() + "\\CovScript_Installer[Daemon_Process_" + Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + "].exe";
                File.Copy(Application.ExecutablePath, daemon_exe_path, true);
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = daemon_exe_path,
                    Arguments = "--daemon",
                    Verb = "runas"
                };
                Process.Start(startInfo);
            }
        }
    }
}
