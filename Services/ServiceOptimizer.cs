using G4meBoost.Helpers;

namespace G4meBoost.Services
{
    public static class ServiceOptimizer
    {
        private static readonly string[] GamingDisableServices =
        {
            "SysMain",           // Superfetch — caches apps in RAM
            "WSearch",           // Windows Search indexer
            "DiagTrack",         // Telemetry
            "dmwappushservice",  // WAP Push
            "WerSvc",           // Error reporting
            "MapsBroker",       // Downloaded Maps Manager
            "Fax",              // Fax service
        };

        public static void DisableService(string serviceName)
        {
            CmdHelper.RunSc(serviceName, "stop");
            CmdHelper.SetServiceStartup(serviceName, "disabled");
        }

        public static void EnableService(string serviceName)
        {
            CmdHelper.SetServiceStartup(serviceName, "auto");
            CmdHelper.RunSc(serviceName, "start");
        }

        public static bool IsServiceDisabled(string serviceName)
        {
            var (_, output) = CmdHelper.Run("sc.exe", $"qc \"{serviceName}\"");
            // START_TYPE : 4 = DISABLED
            return output.Contains("DISABLED") || output.Contains("START_TYPE         : 4");
        }

        public static void DisableAll()
        {
            foreach (var svc in GamingDisableServices)
                DisableService(svc);
        }

        public static void EnableAll()
        {
            foreach (var svc in GamingDisableServices)
                EnableService(svc);
        }
    }
}
