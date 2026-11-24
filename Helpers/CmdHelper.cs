using System;
using System.Diagnostics;

namespace G4meBoost.Helpers
{
    public static class CmdHelper
    {
        public static (int exitCode, string output) Run(string fileName, string arguments, int timeoutMs = 15000)
        {
            try
            {
                using var proc = new Process();
                proc.StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                proc.Start();
                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();
                proc.WaitForExit(timeoutMs);
                return (proc.ExitCode, string.IsNullOrWhiteSpace(output) ? error : output);
            }
            catch (Exception ex)
            {
                return (-1, ex.Message);
            }
        }

        public static (int exitCode, string output) RunCmd(string command) =>
            Run("cmd.exe", $"/c {command}");

        public static (int exitCode, string output) RunPowerShell(string command) =>
            Run("powershell.exe", $"-NoProfile -NonInteractive -Command \"{command}\"");

        public static void RunSc(string serviceName, string action) =>
            Run("sc.exe", $"{action} \"{serviceName}\"");

        public static void SetServiceStartup(string serviceName, string startType) =>
            Run("sc.exe", $"config \"{serviceName}\" start= {startType}");
    }
}
