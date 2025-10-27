using Avalonia;
using Avalonia.Controls;

namespace TodoTxt.UI.App;

/// <summary>
/// Interface for the application
/// </summary>
public interface IApp
{
    int Run(string[] args);
}

public class App : IApp
{
    private readonly Func<Avalonia.Application> _createApp;

    public App(Func<Avalonia.Application> createApp)
    {
        _createApp = createApp;
    }

    public int Run(string[] args)
    {
        var appBuilder = AppBuilder.Configure(_createApp)
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
        return appBuilder.StartWithClassicDesktopLifetime(args);
    }
}