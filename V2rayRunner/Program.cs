using System;
using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace V2rayRunner
{
    static class Program
    {
        private static string PNAME_V2RAY = "v2ray";
        private static string FILE_ERROR = "error.txt";
        private static string CURRENT_DIR = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            // get current dir
            CURRENT_DIR = Path.GetDirectoryName(Application.ExecutablePath);
            log(CURRENT_DIR);

            // kill PNAME_V2RAY instances
            foreach (var process in Process.GetProcessesByName(PNAME_V2RAY))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }

            // kill other instances of myself
            foreach (var process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }

            // start v2ray in sub processes
            string[] configs = Directory.GetFiles(CURRENT_DIR, "*.json");
            foreach (string c in configs)
            {
                log(c);
                Thread t = new Thread(new ParameterizedThreadStart(runV2ray));
                t.Start(c);
            }

            // auto start with windows
            AutoStart();

            // infinite sleep
            while (true)
            {
                try
                {
                    Thread.Sleep(1000000000);
                }
                catch (Exception)
                {

                }
            }
        }

        static void runV2ray(object c)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = CURRENT_DIR + "\\" + PNAME_V2RAY + ".exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-c " + (string)c;
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
                log(ex.Message + Environment.NewLine + ex.StackTrace);
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

        static void log(string msg)
        {
            if (CURRENT_DIR != null)
            {
                try
                {
                    System.IO.File.AppendAllText(CURRENT_DIR + "\\" + FILE_ERROR, Environment.NewLine + msg);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
