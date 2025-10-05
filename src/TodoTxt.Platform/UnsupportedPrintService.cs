using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Unsupported print service implementation that provides no functionality
    /// </summary>
    public class UnsupportedPrintService : IPrintService
    {
        public bool IsSupported => false;

        public Task<IEnumerable<PrinterInfo>> GetAvailablePrintersAsync()
        {
            return Task.FromResult<IEnumerable<PrinterInfo>>(Array.Empty<PrinterInfo>());
        }

        public Task<PrinterInfo?> GetDefaultPrinterAsync()
        {
            return Task.FromResult<PrinterInfo?>(null);
        }

        public Task<bool> PrintTextAsync(string content, PrintSettings? settings = null)
        {
            return Task.FromResult(false);
        }

        public Task<bool> PrintHtmlAsync(string htmlContent, PrintSettings? settings = null)
        {
            return Task.FromResult(false);
        }

        public Task<bool> ShowPrintPreviewAsync(string content, PrintSettings? settings = null)
        {
            return Task.FromResult(false);
        }

        public Task<PrintSettings?> ShowPrintDialogAsync(PrintSettings? defaultSettings = null)
        {
            return Task.FromResult<PrintSettings?>(null);
        }

        public Task<bool> IsPrinterAvailableAsync(string printerName)
        {
            return Task.FromResult(false);
        }
    }
}

