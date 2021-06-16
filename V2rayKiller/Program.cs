using System.Diagnostics;

namespace V2rayKiller
{
    class Program
    {
        private static string PNAME_V2RAY = "v2ray";
        private static string PNAME_V2RAYRUNNER = "V2rayRunner";

        static void Main(string[] args)
        {
            foreach (var process in Process.GetProcessesByName(PNAME_V2RAY))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }
            foreach (var process in Process.GetProcessesByName(PNAME_V2RAYRUNNER))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }
        }
    }
}
