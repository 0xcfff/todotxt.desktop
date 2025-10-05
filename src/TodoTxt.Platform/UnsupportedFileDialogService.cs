using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Unsupported file dialog service implementation that provides no functionality
    /// </summary>
    public class UnsupportedFileDialogService : IFileDialogService
    {
        public Task<string?> ShowOpenFileDialogAsync(string title = "Open File", 
            string? initialDirectory = null, 
            string? filter = null, 
            string? defaultExtension = null)
        {
            return Task.FromResult<string?>(null);
        }

        public Task<string?> ShowSaveFileDialogAsync(string title = "Save File", 
            string? initialDirectory = null, 
            string? initialFileName = null, 
            string? filter = null, 
            string? defaultExtension = null)
        {
            return Task.FromResult<string?>(null);
        }

        public Task<string?> ShowOpenFolderDialogAsync(string title = "Select Folder", 
            string? initialDirectory = null)
        {
            return Task.FromResult<string?>(null);
        }

        public Task<IEnumerable<string>?> ShowOpenMultipleFilesDialogAsync(string title = "Open Files", 
            string? initialDirectory = null, 
            string? filter = null, 
            string? defaultExtension = null)
        {
            return Task.FromResult<IEnumerable<string>?>(null);
        }
    }
}

