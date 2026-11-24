using Microsoft.Win32;
using System.Collections.Generic;

namespace G4meBoost.Services
{
    public class StartupItem
    {
        public string Name { get; set; } = "";
        public string Command { get; set; } = "";
        public string Location { get; set; } = ""; // "HKLM" or "HKCU"
        public bool IsEnabled { get; set; }
    }

    public static class StartupManager
    {
        private const string HklmRunPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string HkcuRunPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string HklmDisabledPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        private const string HkcuDisabledPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

        public static List<StartupItem> GetStartupItems()
        {
            var items = new List<StartupItem>();

            // HKCU
            using (var key = Registry.CurrentUser.OpenSubKey(HkcuRunPath))
            {
                if (key != null)
                {
                    foreach (var name in key.GetValueNames())
                    {
                        items.Add(new StartupItem
                        {
                            Name = name,
                            Command = key.GetValue(name)?.ToString() ?? "",
                            Location = "HKCU",
                            IsEnabled = !IsStartupDisabled("HKCU", name)
                        });
                    }
                }
            }

            // HKLM
            using (var key = Registry.LocalMachine.OpenSubKey(HklmRunPath))
            {
                if (key != null)
                {
                    foreach (var name in key.GetValueNames())
                    {
                        items.Add(new StartupItem
                        {
                            Name = name,
                            Command = key.GetValue(name)?.ToString() ?? "",
                            Location = "HKLM",
                            IsEnabled = !IsStartupDisabled("HKLM", name)
                        });
                    }
                }
            }

            return items;
        }

        private static bool IsStartupDisabled(string hive, string name)
        {
            var approvedPath = hive == "HKCU" ? HkcuDisabledPath : HklmDisabledPath;
            var root = hive == "HKCU" ? Registry.CurrentUser : Registry.LocalMachine;

            using var key = root.OpenSubKey(approvedPath);
            if (key?.GetValue(name) is byte[] data && data.Length >= 4)
            {
                // If first byte has bit 0 clear and bit 1 set → disabled
                return (data[0] & 0x02) != 0 || (data[0] & 0x01) == 0 && data[0] != 0;
            }
            return false;
        }

        public static void DisableStartupItem(string hive, string name)
        {
            var approvedPath = hive == "HKCU" ? HkcuDisabledPath : HklmDisabledPath;
            var root = hive == "HKCU" ? Registry.CurrentUser : Registry.LocalMachine;

            using var key = root.OpenSubKey(approvedPath, true)
                            ?? root.CreateSubKey(approvedPath);
            // Set disabled flag: 03 00 00 ... (12 bytes)
            var data = new byte[12];
            data[0] = 0x03;
            key?.SetValue(name, data, RegistryValueKind.Binary);
        }

        public static void EnableStartupItem(string hive, string name)
        {
            var approvedPath = hive == "HKCU" ? HkcuDisabledPath : HklmDisabledPath;
            var root = hive == "HKCU" ? Registry.CurrentUser : Registry.LocalMachine;

            using var key = root.OpenSubKey(approvedPath, true);
            // Set enabled flag: 02 00 00 ... (12 bytes)
            var data = new byte[12];
            data[0] = 0x02;
            key?.SetValue(name, data, RegistryValueKind.Binary);
        }
    }
}
