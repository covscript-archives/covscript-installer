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
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = Path.GetTempPath(),
                        FileName = daemon_exe_path,
                        Arguments = "--daemon"
                    };
                    Process.Start(startInfo);
                }
                else
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        WorkingDirectory = Path.GetTempPath(),
                        FileName = daemon_exe_path,
                        Arguments = "--daemon",
                        Verb = "runas"
                    };
                    Process.Start(startInfo);
                }
                Environment.Exit(0);
            }
        }
    }
}