using Avalonia;
using Avalonia.Controls;
using Autofac;
using TodoTxt.UI.MarkupExtensions;

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
    private readonly ILifetimeScope _lifetimeScope;
    public App(Func<Avalonia.Application> createApp, ILifetimeScope lifetimeScope)
    {
        _createApp = createApp;
        _lifetimeScope = lifetimeScope;
    }

    public int Run(string[] args)
    {
        using var appScope = _lifetimeScope.BeginLifetimeScope();
        using var xamlScope = appScope.BeginXamlDIScope();

        var appBuilder = AppBuilder.Configure(_createApp)
            // TODO: Add lifetime scope to app builder to resolve some services from the container
            //  This can be used to empower xaml parser extensions to resolve services from the container
            .With<ILifetimeScope>(_lifetimeScope)
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

        return appBuilder.StartWithClassicDesktopLifetime(args);
    }
}