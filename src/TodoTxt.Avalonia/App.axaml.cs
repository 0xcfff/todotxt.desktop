using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using TodoTxt.Avalonia.ViewModels;
using TodoTxt.Avalonia.Views;
using TodoTxt.Avalonia.Services;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using Avalonia.Media.Imaging;

namespace TodoTxt.Avalonia;

public partial class App : Application
{
    private TrayIcon? _trayIcon;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // Create the main window first to show UI immediately
            desktop.MainWindow = new MainWindow();
            
            // Initialize tray icon
            InitializeTrayIcon(desktop);
            
            // Initialize settings and view model asynchronously
            _ = InitializeAsync((MainWindow)desktop.MainWindow);
            
            // Clean up tray icon when application shuts down
            desktop.ShutdownRequested += (s, e) => 
            {
                System.Diagnostics.Debug.WriteLine("Application shutting down, disposing tray icon");
                _trayIcon?.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private async System.Threading.Tasks.Task InitializeAsync(MainWindow mainWindow)
    {
        try
        {
            // Initialize settings service
            await ServiceLocator.InitializeAsync();
            
            // Create and set the view model after settings are loaded
            var viewModel = new MainWindowViewModel();
            mainWindow.DataContext = viewModel;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize application: {ex.Message}");
            // Fall back to a basic view model
            mainWindow.DataContext = new MainWindowViewModel();
        }
    }

    private void InitializeTrayIcon(IClassicDesktopStyleApplicationLifetime desktop)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("Initializing tray icon...");
            _trayIcon = new TrayIcon();
            
            // Load custom icon from assets
            try
            {
                var iconUri = new Uri("avares://TodoTxt.Avalonia/Assets/todotxt-icon.ico");
                _trayIcon.Icon = new WindowIcon(AssetLoader.Open(iconUri));
                System.Diagnostics.Debug.WriteLine("Using custom TodoTxt icon");
            }
            catch (Exception iconEx)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load custom icon: {iconEx.Message}");
                // Continue without custom icon - Avalonia will use default
            }
            
            _trayIcon.ToolTipText = "TodoTxt";
            
            // Set up the menu
            _trayIcon.Menu = CreateTrayMenu(desktop);
            
            // Subscribe to events
            _trayIcon.Clicked += (s, e) => 
            {
                System.Diagnostics.Debug.WriteLine("Tray icon clicked");
                ShowMainWindow(desktop);
            };
            
            // Show the tray icon
            _trayIcon.IsVisible = true;
            
            System.Diagnostics.Debug.WriteLine($"Tray icon initialized successfully. IsVisible: {_trayIcon.IsVisible}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize tray icon: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Exception details: {ex}");
        }
    }

    private NativeMenu CreateTrayMenu(IClassicDesktopStyleApplicationLifetime desktop)
    {
        var menu = new NativeMenu();
        
        var showItem = new NativeMenuItem("Show TodoTxt");
        showItem.Click += (s, e) => ShowMainWindow(desktop);
        menu.Add(showItem);
        
        var hideItem = new NativeMenuItem("Hide TodoTxt");
        hideItem.Click += (s, e) => HideMainWindow(desktop);
        menu.Add(hideItem);
        
        menu.Add(new NativeMenuItemSeparator());
        
        var exitItem = new NativeMenuItem("Exit");
        exitItem.Click += (s, e) => desktop.Shutdown();
        menu.Add(exitItem);
        
        return menu;
    }

    private void ShowMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
    {
        if (desktop.MainWindow != null)
        {
            desktop.MainWindow.Show();
            desktop.MainWindow.WindowState = WindowState.Normal;
            desktop.MainWindow.Activate();
        }
    }

    private void HideMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
    {
        if (desktop.MainWindow != null)
        {
            desktop.MainWindow.Hide();
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}