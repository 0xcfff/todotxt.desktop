using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Linq;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Windows-specific file dialog service implementation using Avalonia's built-in file dialogs
    /// </summary>
    public class WindowsFileDialogService : IFileDialogService
    {
        public async Task<string?> ShowOpenFileDialogAsync(string title = "Open File", 
            string? initialDirectory = null, 
            string? filter = null, 
            string? defaultExtension = null)
        {
            try
            {
                var mainWindow = GetMainWindow();
                if (mainWindow == null)
                    return null;

                var options = new FilePickerOpenOptions
                {
                    Title = title,
                    AllowMultiple = false
                };

                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    options.SuggestedStartLocation = await mainWindow.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    options.FileTypeFilter = ParseFileTypeFilter(filter);
                }

                var files = await mainWindow.StorageProvider.OpenFilePickerAsync(options);
                return files.Count > 0 ? files[0].Path.LocalPath : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> ShowSaveFileDialogAsync(string title = "Save File", 
            string? initialDirectory = null, 
            string? initialFileName = null, 
            string? filter = null, 
            string? defaultExtension = null)
        {
            try
            {
                var mainWindow = GetMainWindow();
                if (mainWindow == null)
                    return null;

                var options = new FilePickerSaveOptions
                {
                    Title = title,
                    SuggestedFileName = initialFileName ?? "untitled"
                };

                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    options.SuggestedStartLocation = await mainWindow.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    options.FileTypeChoices = ParseFileTypeChoices(filter);
                }

                var file = await mainWindow.StorageProvider.SaveFilePickerAsync(options);
                return file?.Path.LocalPath;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> ShowOpenFolderDialogAsync(string title = "Select Folder", 
            string? initialDirectory = null)
        {
            try
            {
                var mainWindow = GetMainWindow();
                if (mainWindow == null)
                    return null;

                var options = new FolderPickerOpenOptions
                {
                    Title = title,
                    AllowMultiple = false
                };

                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    options.SuggestedStartLocation = await mainWindow.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                }

                var folders = await mainWindow.StorageProvider.OpenFolderPickerAsync(options);
                return folders.Count > 0 ? folders[0].Path.LocalPath : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<string>?> ShowOpenMultipleFilesDialogAsync(string title = "Open Files", 
            string? initialDirectory = null, 
            string? filter = null, 
            string? defaultExtension = null)
        {
            try
            {
                var mainWindow = GetMainWindow();
                if (mainWindow == null)
                    return null;

                var options = new FilePickerOpenOptions
                {
                    Title = title,
                    AllowMultiple = true
                };

                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    options.SuggestedStartLocation = await mainWindow.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    options.FileTypeFilter = ParseFileTypeFilter(filter);
                }

                var files = await mainWindow.StorageProvider.OpenFilePickerAsync(options);
                return files.Select(f => f.Path.LocalPath);
            }
            catch
            {
                return null;
            }
        }

        private static Window? GetMainWindow()
        {
            if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            return null;
        }

        private static List<FilePickerFileType> ParseFileTypeFilter(string filter)
        {
            var fileTypes = new List<FilePickerFileType>();
            var parts = filter.Split('|');
            
            for (int i = 0; i < parts.Length; i += 2)
            {
                if (i + 1 < parts.Length)
                {
                    var name = parts[i].Trim();
                    var extensions = parts[i + 1].Split(';')
                        .Select(ext => ext.Trim().TrimStart('*'))
                        .Where(ext => !string.IsNullOrEmpty(ext))
                        .ToArray();
                    
                    if (extensions.Length > 0)
                    {
                        fileTypes.Add(new FilePickerFileType(name) { Patterns = extensions });
                    }
                }
            }
            
            return fileTypes;
        }

        private static List<FilePickerFileType> ParseFileTypeChoices(string filter)
        {
            return ParseFileTypeFilter(filter);
        }
    }
}