using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Interface for native file dialog functionality across different platforms
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Shows an open file dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="initialDirectory">The initial directory to open</param>
        /// <param name="filter">File filter string (e.g., "Text files (*.txt)|*.txt|All files (*.*)|*.*")</param>
        /// <param name="defaultExtension">Default file extension</param>
        /// <returns>The selected file path, or null if cancelled</returns>
        Task<string?> ShowOpenFileDialogAsync(string title = "Open File", 
            string? initialDirectory = null, 
            string? filter = null, 
            string? defaultExtension = null);

        /// <summary>
        /// Shows a save file dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="initialDirectory">The initial directory to open</param>
        /// <param name="initialFileName">The initial file name</param>
        /// <param name="filter">File filter string (e.g., "Text files (*.txt)|*.txt|All files (*.*)|*.*")</param>
        /// <param name="defaultExtension">Default file extension</param>
        /// <returns>The selected file path, or null if cancelled</returns>
        Task<string?> ShowSaveFileDialogAsync(string title = "Save File", 
            string? initialDirectory = null, 
            string? initialFileName = null, 
            string? filter = null, 
            string? defaultExtension = null);

        /// <summary>
        /// Shows an open folder dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="initialDirectory">The initial directory to open</param>
        /// <returns>The selected folder path, or null if cancelled</returns>
        Task<string?> ShowOpenFolderDialogAsync(string title = "Select Folder", 
            string? initialDirectory = null);

        /// <summary>
        /// Shows a multi-select open file dialog
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="initialDirectory">The initial directory to open</param>
        /// <param name="filter">File filter string</param>
        /// <param name="defaultExtension">Default file extension</param>
        /// <returns>A list of selected file paths, or null if cancelled</returns>
        Task<IEnumerable<string>?> ShowOpenMultipleFilesDialogAsync(string title = "Open Files", 
            string? initialDirectory = null, 
            string? filter = null, 
            string? defaultExtension = null);
    }
}

