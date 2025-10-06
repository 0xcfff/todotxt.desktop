using System;
using TodoTxt.Avalonia.Models;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Simple service locator for dependency injection
    /// </summary>
    public static class ServiceLocator
    {
        private static SettingsService? _settingsService;
        private static ApplicationSettings? _applicationSettings;
        private static IServiceProvider? _platformServiceProvider;

        /// <summary>
        /// Gets the settings service instance
        /// </summary>
        public static SettingsService SettingsService
        {
            get
            {
                if (_settingsService == null)
                {
                    _settingsService = new SettingsService();
                }
                return _settingsService;
            }
        }

        /// <summary>
        /// Gets the current application settings
        /// </summary>
        public static ApplicationSettings ApplicationSettings
        {
            get
            {
                if (_applicationSettings == null)
                {
                    _applicationSettings = new ApplicationSettings();
                }
                return _applicationSettings;
            }
            set
            {
                _applicationSettings = value;
            }
        }

        /// <summary>
        /// Gets the platform service provider
        /// </summary>
        public static IServiceProvider PlatformServiceProvider
        {
            get
            {
                if (_platformServiceProvider == null)
                {
                    _platformServiceProvider = new ServiceProvider();
                    PlatformServiceFactory.RegisterPlatformServices(_platformServiceProvider);
                }
                return _platformServiceProvider;
            }
        }

        /// <summary>
        /// Gets the tray service
        /// </summary>
        public static ITrayService TrayService => PlatformServiceProvider.GetRequiredService<ITrayService>();

        /// <summary>
        /// Gets the file dialog service
        /// </summary>
        public static IFileDialogService FileDialogService => PlatformServiceProvider.GetRequiredService<IFileDialogService>();

        /// <summary>
        /// Gets the hotkey service
        /// </summary>
        public static IHotkeyService HotkeyService => PlatformServiceProvider.GetRequiredService<IHotkeyService>();

        /// <summary>
        /// Gets the print service
        /// </summary>
        public static IPrintService PrintService => PlatformServiceProvider.GetRequiredService<IPrintService>();

        /// <summary>
        /// Initializes the service locator and loads settings
        /// </summary>
        public static async System.Threading.Tasks.Task InitializeAsync()
        {
            try
            {
                // Initialize platform services first
                _ = PlatformServiceProvider; // This will create and register platform services
                
                // Try to migrate from WPF settings first
                var migrated = await SettingsService.MigrateFromWpfAsync();
                
                if (!migrated)
                {
                    // Load existing settings or create new ones
                    ApplicationSettings = await SettingsService.LoadSettingsAsync();
                }
                else
                {
                    // Settings were migrated, get the current settings
                    ApplicationSettings = SettingsService.GetCurrentSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize settings: {ex.Message}");
                // Fall back to default settings
                ApplicationSettings = new ApplicationSettings();
            }
        }

        /// <summary>
        /// Saves the current application settings
        /// </summary>
        public static async System.Threading.Tasks.Task SaveSettingsAsync()
        {
            try
            {
                await SettingsService.SaveSettingsAsync(ApplicationSettings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Disposes of all services and cleans up resources
        /// </summary>
        public static void Dispose()
        {
            try
            {
                if (_platformServiceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                _platformServiceProvider = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to dispose services: {ex.Message}");
            }
        }
    }
}

