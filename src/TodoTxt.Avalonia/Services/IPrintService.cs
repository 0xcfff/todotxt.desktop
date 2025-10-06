using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Represents print settings for a print job
    /// </summary>
    public class PrintSettings
    {
        public string? PrinterName { get; set; }
        public string? DocumentName { get; set; }
        public int Copies { get; set; } = 1;
        public bool Collate { get; set; } = true;
        public PrintOrientation Orientation { get; set; } = PrintOrientation.Portrait;
        public PrintPaperSize PaperSize { get; set; } = PrintPaperSize.Letter;
        public PrintQuality Quality { get; set; } = PrintQuality.Normal;
        public bool Color { get; set; } = false;
        public double MarginTop { get; set; } = 1.0;
        public double MarginBottom { get; set; } = 1.0;
        public double MarginLeft { get; set; } = 1.0;
        public double MarginRight { get; set; } = 1.0;
    }

    /// <summary>
    /// Print orientation options
    /// </summary>
    public enum PrintOrientation
    {
        Portrait,
        Landscape
    }

    /// <summary>
    /// Print paper size options
    /// </summary>
    public enum PrintPaperSize
    {
        Letter,
        Legal,
        A4,
        A3,
        Tabloid,
        Custom
    }

    /// <summary>
    /// Print quality options
    /// </summary>
    public enum PrintQuality
    {
        Draft,
        Normal,
        High
    }

    /// <summary>
    /// Represents a printer
    /// </summary>
    public class PrinterInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public bool IsNetwork { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Interface for printing functionality across different platforms
    /// </summary>
    public interface IPrintService
    {
        /// <summary>
        /// Gets whether printing is supported on the current platform
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Gets a list of available printers
        /// </summary>
        /// <returns>List of available printers</returns>
        Task<IEnumerable<PrinterInfo>> GetAvailablePrintersAsync();

        /// <summary>
        /// Gets the default printer
        /// </summary>
        /// <returns>The default printer, or null if none available</returns>
        Task<PrinterInfo?> GetDefaultPrinterAsync();

        /// <summary>
        /// Prints text content
        /// </summary>
        /// <param name="content">The text content to print</param>
        /// <param name="settings">Print settings</param>
        /// <returns>True if printing was successful</returns>
        Task<bool> PrintTextAsync(string content, PrintSettings? settings = null);

        /// <summary>
        /// Prints HTML content
        /// </summary>
        /// <param name="htmlContent">The HTML content to print</param>
        /// <param name="settings">Print settings</param>
        /// <returns>True if printing was successful</returns>
        Task<bool> PrintHtmlAsync(string htmlContent, PrintSettings? settings = null);

        /// <summary>
        /// Shows a print preview dialog
        /// </summary>
        /// <param name="content">The content to preview</param>
        /// <param name="settings">Print settings</param>
        /// <returns>True if the user confirmed printing</returns>
        Task<bool> ShowPrintPreviewAsync(string content, PrintSettings? settings = null);

        /// <summary>
        /// Shows a print dialog and returns the selected settings
        /// </summary>
        /// <param name="defaultSettings">Default print settings</param>
        /// <returns>The selected print settings, or null if cancelled</returns>
        Task<PrintSettings?> ShowPrintDialogAsync(PrintSettings? defaultSettings = null);

        /// <summary>
        /// Checks if a specific printer is available
        /// </summary>
        /// <param name="printerName">The name of the printer to check</param>
        /// <returns>True if the printer is available</returns>
        Task<bool> IsPrinterAvailableAsync(string printerName);
    }
}
