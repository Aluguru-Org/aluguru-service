using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Aluguru.Marketplace.Infrastructure.SysInfo
{
    public static class AssemblyExtensions
    {
        public static SystemInfo GetSystemInfo(this Assembly assembly)
        {
            var basePath = assembly.Location;

            var appName = assembly.GetName().Name;
            var assemblyVersion = assembly.GetName().Version.ToString();
            var runtimeFramework = RuntimeInformation.FrameworkDescription;

            var envs = Environment.GetEnvironmentVariables() as Hashtable;

            var osArchitecture = GetOperationalSystemArchitecture();
            var osDescription = RuntimeInformation.OSDescription;
            var processArchitecture = GetProcessArchitecture();

            var model = new SystemInfo(
                osArchitecture,
                osDescription,
                processArchitecture,
                basePath,
                appName,
                assemblyVersion,
                runtimeFramework,
                envs);

            return model;
        }

        private static string GetOperationalSystemArchitecture()
        {
            return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                            ? ((RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                                ? "Linux or OSX"
                                : "Others")
                            : "Windows";
        }

        private static string GetProcessArchitecture()
        {
            return RuntimeInformation.ProcessArchitecture == Architecture.Arm
                            ? "Arm"
                            : RuntimeInformation.ProcessArchitecture == Architecture.Arm64
                                ? "Arm64"
                                : RuntimeInformation.ProcessArchitecture == Architecture.X64
                                    ? "x64"
                                    : RuntimeInformation.ProcessArchitecture == Architecture.X86
                                        ? "x86"
                                        : "Others";
        }
    }
}