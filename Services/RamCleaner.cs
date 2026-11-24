using System;
using System.IO;
using System.Runtime.InteropServices;
using G4meBoost.Helpers;

namespace G4meBoost.Services
{
    public static class RamCleaner
    {
        // ── P/Invoke for memory cleanup ──
        [DllImport("psapi.dll", SetLastError = true)]
        private static extern bool EmptyWorkingSet(IntPtr hProcess);

        [DllImport("ntdll.dll")]
        private static extern int NtSetSystemInformation(int infoClass, ref int info, int length);

        private const int SystemMemoryListInfo = 0x0050; // MemoryPurgeStandbyList

        public static void ClearStandbyList()
        {
            // Requires admin. Purge standby pages.
            int command = 4; // MemoryPurgeStandbyList
            NtSetSystemInformation(SystemMemoryListInfo, ref command, sizeof(int));
        }

        public static void ClearWorkingSets()
        {
            foreach (var proc in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    EmptyWorkingSet(proc.Handle);
                }
                catch
                {
                    // Some processes are protected — skip
                }
            }
        }

        public static long ClearTempFiles()
        {
            long totalFreed = 0;
            string[] tempPaths =
            {
                Path.GetTempPath(),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp")
            };

            foreach (var tempPath in tempPaths)
            {
                if (!Directory.Exists(tempPath)) continue;
                try
                {
                    foreach (var file in Directory.EnumerateFiles(tempPath, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            var info = new FileInfo(file);
                            totalFreed += info.Length;
                            info.Delete();
                        }
                        catch { /* locked file — skip */ }
                    }

                    foreach (var dir in Directory.EnumerateDirectories(tempPath))
                    {
                        try { Directory.Delete(dir, true); }
                        catch { }
                    }
                }
                catch { }
            }

            return totalFreed;
        }

        public static void FlushDns()
        {
            CmdHelper.RunCmd("ipconfig /flushdns");
        }

        public static void CleanAll()
        {
            ClearStandbyList();
            ClearWorkingSets();
            ClearTempFiles();
            FlushDns();
        }
    }
}
