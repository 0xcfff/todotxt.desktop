using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace TodoTxt.Avalonia.Controls
{
    public partial class HelpDialog : BaseDialog
    {
        public HelpDialog()
        {
            InitializeComponent();
        }

        private void WebsiteLink_PointerPressed(object? sender, global::Avalonia.Input.PointerPressedEventArgs e)
        {
            // Open the website in the default browser
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://benrhughes.com/todotxt.net",
                    UseShellExecute = true
                });
            }
            catch
            {
                // If we can't open the browser, just ignore the error
            }
        }
    }
}
