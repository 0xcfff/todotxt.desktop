using Avalonia;
using System;
using TodoTxt.Desktop.Bootstrap.Application;

namespace TodoTxt.Desktop;

sealed class Program
{
    private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args) {
        try {
            return new ClientApp().Run(args);
        } catch (Exception ex) {
            Log.Error(ex, "Error starting application");
            return -1;
        }
        finally {
            NLog.LogManager.Shutdown();
        }
    }

}

