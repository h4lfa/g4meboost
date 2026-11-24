using Microsoft.Win32;

namespace G4meBoost.Services
{
    public static class GameBarOptimizer
    {
        private const string GameBarPath = @"SOFTWARE\Microsoft\GameBar";
        private const string GameDvrPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR";
        private const string GameStorePath = @"System\GameConfigStore";

        // ── Game Bar ──
        public static void DisableGameBar()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameBarPath, true)
                            ?? Registry.CurrentUser.CreateSubKey(GameBarPath);
            key?.SetValue("AllowAutoGameMode", 0, RegistryValueKind.DWord);
            key?.SetValue("AutoGameModeEnabled", 0, RegistryValueKind.DWord);

            using var dvr = Registry.CurrentUser.OpenSubKey(GameDvrPath, true)
                            ?? Registry.CurrentUser.CreateSubKey(GameDvrPath);
            dvr?.SetValue("AppCaptureEnabled", 0, RegistryValueKind.DWord);

            // Also disable via HKLM policy
            using var policy = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR", true)
                               ?? Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR");
            policy?.SetValue("AllowGameDVR", 0, RegistryValueKind.DWord);
        }

        public static void EnableGameBar()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameBarPath, true);
            key?.SetValue("AllowAutoGameMode", 1, RegistryValueKind.DWord);
            key?.SetValue("AutoGameModeEnabled", 1, RegistryValueKind.DWord);

            using var dvr = Registry.CurrentUser.OpenSubKey(GameDvrPath, true);
            dvr?.SetValue("AppCaptureEnabled", 1, RegistryValueKind.DWord);

            using var policy = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR", true);
            policy?.DeleteValue("AllowGameDVR", false);
        }

        public static bool IsGameBarDisabled()
        {
            using var dvr = Registry.CurrentUser.OpenSubKey(GameDvrPath);
            return dvr?.GetValue("AppCaptureEnabled") is int v && v == 0;
        }

        // ── Game DVR ──
        public static void DisableGameDvr()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameStorePath, true)
                            ?? Registry.CurrentUser.CreateSubKey(GameStorePath);
            key?.SetValue("GameDVR_Enabled", 0, RegistryValueKind.DWord);
            key?.SetValue("GameDVR_FSEBehavior", 2, RegistryValueKind.DWord);
        }

        public static void EnableGameDvr()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameStorePath, true);
            key?.SetValue("GameDVR_Enabled", 1, RegistryValueKind.DWord);
            key?.DeleteValue("GameDVR_FSEBehavior", false);
        }

        public static bool IsGameDvrDisabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameStorePath);
            return key?.GetValue("GameDVR_Enabled") is int v && v == 0;
        }

        // ── Fullscreen Optimizations ──
        public static void DisableFullscreenOptimizations()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameStorePath, true)
                            ?? Registry.CurrentUser.CreateSubKey(GameStorePath);
            key?.SetValue("GameDVR_FSEBehaviorMode", 2, RegistryValueKind.DWord);
            key?.SetValue("GameDVR_HonorUserFSEBehaviorMode", 1, RegistryValueKind.DWord);
            key?.SetValue("GameDVR_DXGIHonorFSEWindowsCompatible", 1, RegistryValueKind.DWord);
        }

        public static void EnableFullscreenOptimizations()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameStorePath, true);
            key?.DeleteValue("GameDVR_FSEBehaviorMode", false);
            key?.DeleteValue("GameDVR_HonorUserFSEBehaviorMode", false);
            key?.DeleteValue("GameDVR_DXGIHonorFSEWindowsCompatible", false);
        }

        public static bool AreFullscreenOptimizationsDisabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameStorePath);
            return key?.GetValue("GameDVR_FSEBehaviorMode") is int v && v == 2;
        }

        // ── Game Bar Tips ──
        public static void DisableTips()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameBarPath, true)
                            ?? Registry.CurrentUser.CreateSubKey(GameBarPath);
            key?.SetValue("ShowStartupPanel", 0, RegistryValueKind.DWord);
        }

        public static void EnableTips()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameBarPath, true);
            key?.SetValue("ShowStartupPanel", 1, RegistryValueKind.DWord);
        }

        public static bool AreTipsDisabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(GameBarPath);
            return key?.GetValue("ShowStartupPanel") is int v && v == 0;
        }
    }
}
