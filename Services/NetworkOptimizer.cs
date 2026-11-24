using Microsoft.Win32;
using System.Linq;

namespace G4meBoost.Services
{
    public static class NetworkOptimizer
    {
        private const string InterfacesPath = @"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces";
        private const string MultimediaPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";

        /// <summary>Disables Nagle's algorithm on all network interfaces.</summary>
        public static void DisableNagle()
        {
            using var interfaces = Registry.LocalMachine.OpenSubKey(InterfacesPath);
            if (interfaces == null) return;

            foreach (var subKeyName in interfaces.GetSubKeyNames())
            {
                using var iface = interfaces.OpenSubKey(subKeyName, true);
                if (iface == null) continue;

                // Only modify interfaces that have an IP address assigned
                var ip = iface.GetValue("DhcpIPAddress") as string ?? iface.GetValue("IPAddress") as string;
                if (string.IsNullOrEmpty(ip) || ip == "0.0.0.0") continue;

                iface.SetValue("TcpNoDelay", 1, RegistryValueKind.DWord);
                iface.SetValue("TcpAckFrequency", 1, RegistryValueKind.DWord);
            }
        }

        public static void EnableNagle()
        {
            using var interfaces = Registry.LocalMachine.OpenSubKey(InterfacesPath);
            if (interfaces == null) return;

            foreach (var subKeyName in interfaces.GetSubKeyNames())
            {
                using var iface = interfaces.OpenSubKey(subKeyName, true);
                iface?.DeleteValue("TcpNoDelay", false);
                iface?.DeleteValue("TcpAckFrequency", false);
            }
        }

        public static bool IsNagleDisabled()
        {
            using var interfaces = Registry.LocalMachine.OpenSubKey(InterfacesPath);
            if (interfaces == null) return false;

            return interfaces.GetSubKeyNames().Any(name =>
            {
                using var iface = interfaces.OpenSubKey(name);
                return iface?.GetValue("TcpNoDelay") is int v && v == 1;
            });
        }

        /// <summary>Disables network throttling for multimedia applications.</summary>
        public static void DisableThrottling()
        {
            using var key = Registry.LocalMachine.OpenSubKey(MultimediaPath, true);
            if (key == null) return;
            key.SetValue("NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF), RegistryValueKind.DWord);
            key.SetValue("SystemResponsiveness", 0, RegistryValueKind.DWord);
        }

        public static void EnableThrottling()
        {
            using var key = Registry.LocalMachine.OpenSubKey(MultimediaPath, true);
            if (key == null) return;
            key.SetValue("NetworkThrottlingIndex", 10, RegistryValueKind.DWord);
            key.SetValue("SystemResponsiveness", 20, RegistryValueKind.DWord);
        }

        public static bool IsThrottlingDisabled()
        {
            using var key = Registry.LocalMachine.OpenSubKey(MultimediaPath);
            return key?.GetValue("NetworkThrottlingIndex") is int v && v == unchecked((int)0xFFFFFFFF);
        }

        /// <summary>Sets TcpAckFrequency=1 for immediate ACKs.</summary>
        public static void EnableFastAck()
        {
            using var interfaces = Registry.LocalMachine.OpenSubKey(InterfacesPath);
            if (interfaces == null) return;

            foreach (var subKeyName in interfaces.GetSubKeyNames())
            {
                using var iface = interfaces.OpenSubKey(subKeyName, true);
                if (iface == null) continue;
                var ip = iface.GetValue("DhcpIPAddress") as string ?? iface.GetValue("IPAddress") as string;
                if (string.IsNullOrEmpty(ip) || ip == "0.0.0.0") continue;
                iface.SetValue("TcpAckFrequency", 1, RegistryValueKind.DWord);
            }
        }

        public static void DisableFastAck()
        {
            using var interfaces = Registry.LocalMachine.OpenSubKey(InterfacesPath);
            if (interfaces == null) return;
            foreach (var subKeyName in interfaces.GetSubKeyNames())
            {
                using var iface = interfaces.OpenSubKey(subKeyName, true);
                iface?.DeleteValue("TcpAckFrequency", false);
            }
        }

        public static bool IsFastAckEnabled()
        {
            using var interfaces = Registry.LocalMachine.OpenSubKey(InterfacesPath);
            if (interfaces == null) return false;
            return interfaces.GetSubKeyNames().Any(name =>
            {
                using var iface = interfaces.OpenSubKey(name);
                return iface?.GetValue("TcpAckFrequency") is int v && v == 1;
            });
        }
    }
}
