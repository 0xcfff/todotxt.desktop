using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TodoTxt.Avalonia.Models;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Cross-platform settings service for persisting application settings
    /// </summary>
    public class SettingsService
    {
        private readonly string _settingsFilePath;
        private readonly JsonSerializerOptions _jsonOptions;
        private ApplicationSettings? _cachedSettings;

        public SettingsService()
        {
            // Get the appropriate settings directory for the current platform
            var settingsDirectory = GetSettingsDirectory();
            Directory.CreateDirectory(settingsDirectory);
            
            _settingsFilePath = Path.Combine(settingsDirectory, "settings.json");
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <summary>
        /// Gets the platform-specific settings directory
        /// </summary>
        private static string GetSettingsDirectory()
        {
            var appName = "TodoTxt.Desktop";
            
            return Environment.OSVersion.Platform switch
            {
                PlatformID.Win32NT => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    appName),
                PlatformID.Unix => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    ".config", appName.ToLower()),
                PlatformID.MacOSX => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Library", "Application Support", appName),
                _ => Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    ".config", appName.ToLower())
            };
        }

        /// <summary>
        /// Loads settings from disk
        /// </summary>
        public async Task<ApplicationSettings> LoadSettingsAsync()
        {
            if (_cachedSettings != null)
                return _cachedSettings;

            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var json = await File.ReadAllTextAsync(_settingsFilePath);
                    _cachedSettings = JsonSerializer.Deserialize<ApplicationSettings>(json, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
            }

            _cachedSettings ??= new ApplicationSettings();
            return _cachedSettings;
        }

        /// <summary>
        /// Saves settings to disk
        /// </summary>
        public async Task SaveSettingsAsync(ApplicationSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, _jsonOptions);
                await File.WriteAllTextAsync(_settingsFilePath, json);
                _cachedSettings = settings;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the current settings (cached or loaded)
        /// </summary>
        public ApplicationSettings GetCurrentSettings()
        {
            return _cachedSettings ?? new ApplicationSettings();
        }

        /// <summary>
        /// Resets settings to default values
        /// </summary>
        public async Task ResetToDefaultsAsync()
        {
            var defaultSettings = new ApplicationSettings();
            await SaveSettingsAsync(defaultSettings);
        }

        /// <summary>
        /// Migrates settings from WPF version if they exist
        /// </summary>
        public async Task<bool> MigrateFromWpfAsync()
        {
            try
            {
                // Look for WPF settings file in the same directory as the executable
                var wpfSettingsPath = Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "",
                    "Client.settings");

                if (!File.Exists(wpfSettingsPath))
                    return false;

                // Parse WPF settings XML and convert to our settings model
                var wpfSettings = await ParseWpfSettingsAsync(wpfSettingsPath);
                if (wpfSettings != null)
                {
                    await SaveSettingsAsync(wpfSettings);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to migrate WPF settings: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Parses WPF settings XML file
        /// </summary>
        private async Task<ApplicationSettings?> ParseWpfSettingsAsync(string wpfSettingsPath)
        {
            try
            {
                var xml = await File.ReadAllTextAsync(wpfSettingsPath);
                var settings = new ApplicationSettings();

                // Simple XML parsing for the settings we need
                // This is a basic implementation - in production you might want to use a proper XML parser
                var lines = xml.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("setting[@name='"))
                    {
                        var nameStart = line.IndexOf("setting[@name='") + 15;
                        var nameEnd = line.IndexOf("']", nameStart);
                        if (nameStart > 14 && nameEnd > nameStart)
                        {
                            var settingName = line.Substring(nameStart, nameEnd - nameStart);
                            var valueStart = line.IndexOf(">", nameEnd) + 1;
                            var valueEnd = line.IndexOf("</", valueStart);
                            if (valueStart > 0 && valueEnd > valueStart)
                            {
                                var value = line.Substring(valueStart, valueEnd - valueStart);
                                SetSettingValue(settings, settingName, value);
                            }
                        }
                    }
                }

                return settings;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to parse WPF settings: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Sets a setting value by name and string value
        /// </summary>
        private void SetSettingValue(ApplicationSettings settings, string name, string value)
        {
            switch (name)
            {
                case "FilePath":
                    settings.FilePath = value;
                    break;
                case "ArchiveFilePath":
                    settings.ArchiveFilePath = value;
                    break;
                case "WindowHeight":
                    if (double.TryParse(value, out var height))
                        settings.WindowHeight = height;
                    break;
                case "WindowWidth":
                    if (double.TryParse(value, out var width))
                        settings.WindowWidth = width;
                    break;
                case "WindowLeft":
                    if (double.TryParse(value, out var left))
                        settings.WindowLeft = left;
                    break;
                case "WindowTop":
                    if (double.TryParse(value, out var top))
                        settings.WindowTop = top;
                    break;
                case "FilterText":
                    settings.FilterText = value;
                    break;
                case "CurrentSort":
                    if (int.TryParse(value, out var sort))
                        settings.CurrentSort = sort;
                    break;
                case "AutoArchive":
                    if (bool.TryParse(value, out var autoArchive))
                        settings.AutoArchive = autoArchive;
                    break;
                case "AutoRefresh":
                    if (bool.TryParse(value, out var autoRefresh))
                        settings.AutoRefresh = autoRefresh;
                    break;
                case "FilterCaseSensitive":
                    if (bool.TryParse(value, out var caseSensitive))
                        settings.FilterCaseSensitive = caseSensitive;
                    break;
                case "FirstRun":
                    if (bool.TryParse(value, out var firstRun))
                        settings.FirstRun = firstRun;
                    break;
                case "AddCreationDate":
                    if (bool.TryParse(value, out var addCreationDate))
                        settings.AddCreationDate = addCreationDate;
                    break;
                case "DebugLoggingOn":
                    if (bool.TryParse(value, out var debugLogging))
                        settings.DebugLoggingOn = debugLogging;
                    break;
                case "MinimiseToSystemTray":
                    if (bool.TryParse(value, out var minToTray))
                        settings.MinimiseToSystemTray = minToTray;
                    break;
                case "MinimiseOnClose":
                    if (bool.TryParse(value, out var minOnClose))
                        settings.MinimiseOnClose = minOnClose;
                    break;
                case "RequireCtrlEnter":
                    if (bool.TryParse(value, out var requireCtrlEnter))
                        settings.RequireCtrlEnter = requireCtrlEnter;
                    break;
                case "TaskListFontFamily":
                    settings.TaskListFontFamily = value;
                    break;
                case "TaskListFontSize":
                    if (double.TryParse(value, out var fontSize))
                        settings.TaskListFontSize = fontSize;
                    break;
                case "TaskListFontStyle":
                    settings.TaskListFontStyle = value;
                    break;
                case "TaskListFontStretch":
                    settings.TaskListFontStretch = value;
                    break;
                case "TaskListFontWeight":
                    settings.TaskListFontWeight = value;
                    break;
                case "TaskListFontBrushColor":
                    settings.TaskListFontBrushColor = value;
                    break;
                case "AllowGrouping":
                    if (bool.TryParse(value, out var allowGrouping))
                        settings.AllowGrouping = allowGrouping;
                    break;
                case "MoveFocusToTaskListAfterAddingNewTask":
                    if (bool.TryParse(value, out var moveFocus))
                        settings.MoveFocusToTaskListAfterAddingNewTask = moveFocus;
                    break;
                case "PreserveWhiteSpace":
                    if (bool.TryParse(value, out var preserveWhiteSpace))
                        settings.PreserveWhiteSpace = preserveWhiteSpace;
                    break;
                case "WordWrap":
                    if (bool.TryParse(value, out var wordWrap))
                        settings.WordWrap = wordWrap;
                    break;
                case "IntellisenseCaseSensitive":
                    if (bool.TryParse(value, out var intellisenseCaseSensitive))
                        settings.IntellisenseCaseSensitive = intellisenseCaseSensitive;
                    break;
                case "AutoSelectArchivePath":
                    if (bool.TryParse(value, out var autoSelectArchive))
                        settings.AutoSelectArchivePath = autoSelectArchive;
                    break;
                case "DisplayStatusBar":
                    if (bool.TryParse(value, out var displayStatusBar))
                        settings.DisplayStatusBar = displayStatusBar;
                    break;
                case "FilterFutureTasks":
                    if (bool.TryParse(value, out var filterFutureTasks))
                        settings.FilterFutureTasks = filterFutureTasks;
                    break;
                case "ShowHidenTasks":
                    if (bool.TryParse(value, out var showHiddenTasks))
                        settings.ShowHiddenTasks = showHiddenTasks;
                    break;
                case "CheckForUpdates":
                    if (bool.TryParse(value, out var checkForUpdates))
                        settings.CheckForUpdates = checkForUpdates;
                    break;
                case "FilterTextPreset1":
                    settings.FilterTextPreset1 = value;
                    break;
                case "FilterTextPreset2":
                    settings.FilterTextPreset2 = value;
                    break;
                case "FilterTextPreset3":
                    settings.FilterTextPreset3 = value;
                    break;
                case "FilterTextPreset4":
                    settings.FilterTextPreset4 = value;
                    break;
                case "FilterTextPreset5":
                    settings.FilterTextPreset5 = value;
                    break;
                case "FilterTextPreset6":
                    settings.FilterTextPreset6 = value;
                    break;
                case "FilterTextPreset7":
                    settings.FilterTextPreset7 = value;
                    break;
                case "FilterTextPreset8":
                    settings.FilterTextPreset8 = value;
                    break;
                case "FilterTextPreset9":
                    settings.FilterTextPreset9 = value;
                    break;
            }
        }
    }
}

