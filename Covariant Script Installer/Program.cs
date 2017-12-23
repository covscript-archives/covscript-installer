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
                string daemon_exe_path = Path.GetTempFileName() + ".exe";
                File.Copy(Application.ExecutablePath, daemon_exe_path, true);
                Process.Start(daemon_exe_path, "--daemon");
            }
        }
    }
}
