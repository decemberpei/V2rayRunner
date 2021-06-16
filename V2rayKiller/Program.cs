using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2rayKiller
{
    class Program
    {
        private static string V2RAY = "v2ray";
        private static string V2RAY_RUNNER = "V2rayRunner";

        static void Main(string[] args)
        {
            foreach (var process in Process.GetProcessesByName(V2RAY))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }
            foreach (var process in Process.GetProcessesByName(V2RAY_RUNNER))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill();
                }
            }
        }
    }
}
