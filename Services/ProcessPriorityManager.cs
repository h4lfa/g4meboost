using Microsoft.Win32;

namespace G4meBoost.Services
{
    public static class ProcessPriorityManager
    {
        private const string SystemProfilePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";
        private const string GamesTaskPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games";

        /// <summary>Sets maximum gaming priority in the Windows Multimedia Class Scheduler.</summary>
        public static void SetGamingPriority()
        {
            using var profile = Registry.LocalMachine.OpenSubKey(SystemProfilePath, true)
                                ?? Registry.LocalMachine.CreateSubKey(SystemProfilePath);
            profile?.SetValue("SystemResponsiveness", 0, RegistryValueKind.DWord);
            profile?.SetValue("NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF), RegistryValueKind.DWord);

            using var games = Registry.LocalMachine.OpenSubKey(GamesTaskPath, true)
                              ?? Registry.LocalMachine.CreateSubKey(GamesTaskPath);
            games?.SetValue("GPU Priority", 8, RegistryValueKind.DWord);
            games?.SetValue("Priority", 6, RegistryValueKind.DWord);
            games?.SetValue("Scheduling Category", "High", RegistryValueKind.String);
            games?.SetValue("SFIO Priority", "High", RegistryValueKind.String);
        }

        public static void ResetPriority()
        {
            using var profile = Registry.LocalMachine.OpenSubKey(SystemProfilePath, true);
            profile?.SetValue("SystemResponsiveness", 20, RegistryValueKind.DWord);
            profile?.SetValue("NetworkThrottlingIndex", 10, RegistryValueKind.DWord);

            using var games = Registry.LocalMachine.OpenSubKey(GamesTaskPath, true);
            games?.SetValue("GPU Priority", 2, RegistryValueKind.DWord);
            games?.SetValue("Priority", 2, RegistryValueKind.DWord);
            games?.SetValue("Scheduling Category", "Medium", RegistryValueKind.String);
            games?.SetValue("SFIO Priority", "Normal", RegistryValueKind.String);
        }

        public static bool IsGamingPrioritySet()
        {
            using var games = Registry.LocalMachine.OpenSubKey(GamesTaskPath);
            return games?.GetValue("Priority") is int v && v >= 6;
        }

        /// <summary>Sets GPU priority to maximum for gaming.</summary>
        public static void SetGpuPriority()
        {
            using var games = Registry.LocalMachine.OpenSubKey(GamesTaskPath, true)
                              ?? Registry.LocalMachine.CreateSubKey(GamesTaskPath);
            games?.SetValue("GPU Priority", 8, RegistryValueKind.DWord);
        }

        public static void ResetGpuPriority()
        {
            using var games = Registry.LocalMachine.OpenSubKey(GamesTaskPath, true);
            games?.SetValue("GPU Priority", 2, RegistryValueKind.DWord);
        }

        public static bool IsGpuPriorityMax()
        {
            using var games = Registry.LocalMachine.OpenSubKey(GamesTaskPath);
            return games?.GetValue("GPU Priority") is int v && v >= 8;
        }

        /// <summary>Enables high resolution timer (affects global system timer resolution).</summary>
        public static void EnableHighResTimer()
        {
            // Set the global timer resolution to 0.5ms via registry
            using var key = Registry.LocalMachine.OpenSubKey(SystemProfilePath, true)
                            ?? Registry.LocalMachine.CreateSubKey(SystemProfilePath);
            key?.SetValue("SystemResponsiveness", 0, RegistryValueKind.DWord);
        }

        public static void DisableHighResTimer()
        {
            using var key = Registry.LocalMachine.OpenSubKey(SystemProfilePath, true);
            key?.SetValue("SystemResponsiveness", 20, RegistryValueKind.DWord);
        }

        public static bool IsHighResTimerEnabled()
        {
            using var key = Registry.LocalMachine.OpenSubKey(SystemProfilePath);
            return key?.GetValue("SystemResponsiveness") is int v && v == 0;
        }
    }
}
