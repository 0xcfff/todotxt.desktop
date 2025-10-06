using System;
using System.Runtime.InteropServices;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Factory for creating platform-specific service implementations
    /// </summary>
    public static class PlatformServiceFactory
    {
        /// <summary>
        /// Gets the current operating system
        /// </summary>
        public static OperatingSystem OS => GetOperatingSystem();

        /// <summary>
        /// Creates a platform-specific tray service implementation
        /// </summary>
        /// <returns>The appropriate tray service for the current platform</returns>
        public static ITrayService CreateTrayService()
        {
            return OS switch
            {
                OperatingSystem.Windows => new WindowsTrayService(),
                OperatingSystem.macOS => new MacOSTrayService(),
                OperatingSystem.Linux => new LinuxTrayService(),
                _ => new UnsupportedTrayService()
            };
        }

        /// <summary>
        /// Creates a platform-specific file dialog service implementation
        /// </summary>
        /// <returns>The appropriate file dialog service for the current platform</returns>
        public static IFileDialogService CreateFileDialogService()
        {
            return OS switch
            {
                OperatingSystem.Windows => new WindowsFileDialogService(),
                OperatingSystem.macOS => new MacOSFileDialogService(),
                OperatingSystem.Linux => new LinuxFileDialogService(),
                _ => new UnsupportedFileDialogService()
            };
        }

        /// <summary>
        /// Creates a platform-specific hotkey service implementation
        /// </summary>
        /// <returns>The appropriate hotkey service for the current platform</returns>
        public static IHotkeyService CreateHotkeyService()
        {
            return OS switch
            {
                OperatingSystem.Windows => new WindowsHotkeyService(),
                OperatingSystem.macOS => new MacOSHotkeyService(),
                OperatingSystem.Linux => new LinuxHotkeyService(),
                _ => new UnsupportedHotkeyService()
            };
        }

        /// <summary>
        /// Creates a platform-specific print service implementation
        /// </summary>
        /// <returns>The appropriate print service for the current platform</returns>
        public static IPrintService CreatePrintService()
        {
            return OS switch
            {
                OperatingSystem.Windows => new WindowsPrintService(),
                OperatingSystem.macOS => new MacOSPrintService(),
                OperatingSystem.Linux => new LinuxPrintService(),
                _ => new UnsupportedPrintService()
            };
        }

        /// <summary>
        /// Registers all platform services with the service provider
        /// </summary>
        /// <param name="serviceProvider">The service provider to register services with</param>
        public static void RegisterPlatformServices(IServiceProvider serviceProvider)
        {
            serviceProvider.RegisterInstance(CreateTrayService());
            serviceProvider.RegisterInstance(CreateFileDialogService());
            serviceProvider.RegisterInstance(CreateHotkeyService());
            serviceProvider.RegisterInstance(CreatePrintService());
        }

        private static OperatingSystem GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OperatingSystem.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OperatingSystem.macOS;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OperatingSystem.Linux;
            
            return OperatingSystem.Unknown;
        }
    }

    /// <summary>
    /// Enumeration of supported operating systems
    /// </summary>
    public enum OperatingSystem
    {
        Windows,
        macOS,
        Linux,
        Unknown
    }
}
