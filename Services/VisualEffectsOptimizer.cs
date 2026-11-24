using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace G4meBoost.Services
{
    public static class VisualEffectsOptimizer
    {
        private const string DesktopPath = @"Control Panel\Desktop";
        private const string DwmPath = @"SOFTWARE\Microsoft\Windows\DWM";
        private const string PersonalizePath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string ExplorerAdvPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string VisualFxPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects";

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

        private const uint SPI_SETCLIENTAREAANIMATION = 0x1043;
        private const uint SPI_SETANIMATION = 0x0049;
        private const uint SPIF_SENDCHANGE = 0x02;

        public static void DisableAnimations()
        {
            using var key = Registry.CurrentUser.OpenSubKey(DesktopPath, true);
            if (key != null)
            {
                key.SetValue("MenuShowDelay", "0", RegistryValueKind.String);
            }

            using var explorer = Registry.CurrentUser.OpenSubKey(ExplorerAdvPath, true);
            explorer?.SetValue("TaskbarAnimations", 0, RegistryValueKind.DWord);

            // Disable client area animation
            SystemParametersInfo(SPI_SETCLIENTAREAANIMATION, 0, 0, SPIF_SENDCHANGE);

            using var vfx = Registry.CurrentUser.OpenSubKey(VisualFxPath, true)
                            ?? Registry.CurrentUser.CreateSubKey(VisualFxPath);
            vfx?.SetValue("VisualFXSetting", 2, RegistryValueKind.DWord); // 2 = Best Performance
        }

        public static void EnableAnimations()
        {
            using var key = Registry.CurrentUser.OpenSubKey(DesktopPath, true);
            key?.SetValue("MenuShowDelay", "400", RegistryValueKind.String);

            using var explorer = Registry.CurrentUser.OpenSubKey(ExplorerAdvPath, true);
            explorer?.SetValue("TaskbarAnimations", 1, RegistryValueKind.DWord);

            SystemParametersInfo(SPI_SETCLIENTAREAANIMATION, 0, 1, SPIF_SENDCHANGE);

            using var vfx = Registry.CurrentUser.OpenSubKey(VisualFxPath, true);
            vfx?.SetValue("VisualFXSetting", 0, RegistryValueKind.DWord); // 0 = Let Windows choose
        }

        public static bool AreAnimationsDisabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(DesktopPath);
            return key?.GetValue("MenuShowDelay") as string == "0";
        }

        public static void DisableTransparency()
        {
            using var key = Registry.CurrentUser.OpenSubKey(PersonalizePath, true);
            key?.SetValue("EnableTransparency", 0, RegistryValueKind.DWord);
        }

        public static void EnableTransparency()
        {
            using var key = Registry.CurrentUser.OpenSubKey(PersonalizePath, true);
            key?.SetValue("EnableTransparency", 1, RegistryValueKind.DWord);
        }

        public static bool IsTransparencyDisabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(PersonalizePath);
            return key?.GetValue("EnableTransparency") is int v && v == 0;
        }

        public static void DisableShadows()
        {
            using var dwm = Registry.CurrentUser.OpenSubKey(DwmPath, true)
                            ?? Registry.CurrentUser.CreateSubKey(DwmPath);
            dwm?.SetValue("EnableAeroPeek", 0, RegistryValueKind.DWord);

            using var explorer = Registry.CurrentUser.OpenSubKey(ExplorerAdvPath, true);
            explorer?.SetValue("ListviewShadow", 0, RegistryValueKind.DWord);
        }

        public static void EnableShadows()
        {
            using var dwm = Registry.CurrentUser.OpenSubKey(DwmPath, true);
            dwm?.SetValue("EnableAeroPeek", 1, RegistryValueKind.DWord);

            using var explorer = Registry.CurrentUser.OpenSubKey(ExplorerAdvPath, true);
            explorer?.SetValue("ListviewShadow", 1, RegistryValueKind.DWord);
        }

        public static bool AreShadowsDisabled()
        {
            using var dwm = Registry.CurrentUser.OpenSubKey(DwmPath);
            return dwm?.GetValue("EnableAeroPeek") is int v && v == 0;
        }
    }
}
