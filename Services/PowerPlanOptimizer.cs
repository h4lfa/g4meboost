using G4meBoost.Helpers;
using System.Text.RegularExpressions;

namespace G4meBoost.Services
{
    public static class PowerPlanOptimizer
    {
        // Well-known GUIDs
        private const string BalancedGuid   = "381b4222-f694-41f0-9685-ff5bb260df2e";
        private const string HighPerfGuid   = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c";
        private const string UltimateGuid   = "e9a42b02-d5df-448d-aa00-03f14749eb61";

        public static void SetHighPerformance()
        {
            // Ensure High Performance plan exists and activate it
            CmdHelper.Run("powercfg.exe", $"/setactive {HighPerfGuid}");
        }

        public static void SetUltimatePerformance()
        {
            // Check if Ultimate Performance plan exists
            var (_, output) = CmdHelper.Run("powercfg.exe", "/list");
            if (!output.Contains(UltimateGuid))
            {
                // Create it by duplicating the hidden scheme
                CmdHelper.Run("powercfg.exe", $"/duplicatescheme {UltimateGuid}");
            }
            CmdHelper.Run("powercfg.exe", $"/setactive {UltimateGuid}");
        }

        public static void SetBalanced()
        {
            CmdHelper.Run("powercfg.exe", $"/setactive {BalancedGuid}");
        }

        public static string GetActivePlanGuid()
        {
            var (_, output) = CmdHelper.Run("powercfg.exe", "/getactivescheme");
            // Output: "Power Scheme GUID: 381b4222-... (Balanced)"
            var match = Regex.Match(output, @"([0-9a-fA-F\-]{36})");
            return match.Success ? match.Groups[1].Value.ToLowerInvariant() : BalancedGuid;
        }

        public static bool IsHighPerformance()
        {
            var guid = GetActivePlanGuid();
            return guid == HighPerfGuid || guid == UltimateGuid;
        }
    }
}
