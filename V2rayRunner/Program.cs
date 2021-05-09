using System;
using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace V2rayRunner
{
    static class Program
    {
        private static string V2RAY = "v2ray";
        private static string CFG_IP = "config-ip-10810.json";
        private static string CFG_DOMAIN = "config-domain-10809.json";
        private static string ERR_FILE = "error.txt";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            // kill other instances first
            foreach (var process in Process.GetProcessesByName(V2RAY))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }
            foreach (var process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }

            // start v2ray in sub processes
            new Thread(new ThreadStart(runV2rayDomain)).Start();
            new Thread(new ThreadStart(runV2rayIp)).Start();

            AutoStart();

            // infinite sleep
            while (true)
            {
                try
                {
                    Thread.Sleep(1000000000);
                }
                catch (Exception ex)
                {

                }
            }
        }

        static void runV2rayDomain()
        {
            string current_dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = current_dir + "\\" + V2RAY + ".exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-c " + current_dir + "\\" + CFG_DOMAIN;

            string logTxt = startInfo.FileName + Environment.NewLine + startInfo.Arguments;
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(current_dir + "\\" + ERR_FILE, ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine + logTxt);
            }
        }

        static void runV2rayIp()
        {
            string current_dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = current_dir + "\\" + V2RAY + ".exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-c " + current_dir + "\\" + CFG_IP;

            string logTxt = startInfo.FileName + Environment.NewLine + startInfo.Arguments;
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(current_dir + "\\" + ERR_FILE, ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine + logTxt);
            }
        }

        public static void AutoStart()
        {
            string link = "C:\\Users\\" + Environment.UserName
                + "\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\"
                    + AppDomain.CurrentDomain.FriendlyName + ".lnk";
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(link) as IWshShortcut;
            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            //shortcut...
            shortcut.Save();
        }
    }
}
