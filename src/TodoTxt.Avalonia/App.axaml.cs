using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using TodoTxt.Avalonia.ViewModels;
using TodoTxt.Avalonia.Views;
using TodoTxt.Avalonia.Services;
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Layout;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

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
            
            // Set application icon for macOS dock/taskbar
            try
            {
                var iconUri = new Uri("avares://TodoTxt.Avalonia/Assets/todotxt-icon.ico");
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.Icon = new WindowIcon(AssetLoader.Open(iconUri));
                System.Diagnostics.Debug.WriteLine("Application icon set successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to set application icon: {ex.Message}");
                // Fallback: create window without custom icon
                desktop.MainWindow = new MainWindow();
            }
            
            // Initialize native menu for macOS (delayed to override defaults)
            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                await System.Threading.Tasks.Task.Delay(100); // Small delay to ensure app is fully initialized
                Dispatcher.UIThread.Post(() => InitializeNativeMenu(desktop));
            });
            
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

    private void InitializeNativeMenu(IClassicDesktopStyleApplicationLifetime desktop)
    {
        try
        {
            // Clear any existing native menu first
            if (desktop.MainWindow != null)
            {
                NativeMenu.SetMenu(desktop.MainWindow, null);
            }

            // Create the native menu for macOS
            var nativeMenu = new NativeMenu();
            
            // File Menu
            var fileMenu = new NativeMenuItem("File");
            var fileSubMenu = new NativeMenu();
            
            var newItem = new NativeMenuItem("New");
            newItem.Click += (s, e) => ExecuteCommand(desktop, "NewFileCommand");
            fileSubMenu.Add(newItem);
            
            var openItem = new NativeMenuItem("Open...");
            openItem.Click += (s, e) => ExecuteCommand(desktop, "OpenFileCommand");
            fileSubMenu.Add(openItem);
            
            var printItem = new NativeMenuItem("Print...");
            printItem.Click += (s, e) => ExecuteCommand(desktop, "PrintTasksCommand");
            fileSubMenu.Add(printItem);
            
            fileSubMenu.Add(new NativeMenuItemSeparator());
            
            var archiveItem = new NativeMenuItem("Archive Completed Tasks");
            archiveItem.Click += (s, e) => ExecuteCommand(desktop, "ArchiveCompletedTasksCommand");
            fileSubMenu.Add(archiveItem);
            
            var reloadItem = new NativeMenuItem("Reload File");
            reloadItem.Click += (s, e) => ExecuteCommand(desktop, "ReloadFileCommand");
            fileSubMenu.Add(reloadItem);
            
            fileSubMenu.Add(new NativeMenuItemSeparator());
            
            var optionsItem = new NativeMenuItem("Options...");
            optionsItem.Click += (s, e) => ExecuteCommand(desktop, "ShowOptionsDialogCommand");
            fileSubMenu.Add(optionsItem);
            
            fileSubMenu.Add(new NativeMenuItemSeparator());
            
            var quitItem = new NativeMenuItem("Quit TodoTxt");
            quitItem.Click += (s, e) => ExecuteCommand(desktop, "ExitCommand");
            fileSubMenu.Add(quitItem);
            
            fileMenu.Menu = fileSubMenu;
            nativeMenu.Add(fileMenu);
            
            // Edit Menu
            var editMenu = new NativeMenuItem("Edit");
            var editSubMenu = new NativeMenu();
            
            var cutItem = new NativeMenuItem("Cut");
            cutItem.Click += (s, e) => ExecuteCommand(desktop, "CutSelectedTaskCommand");
            editSubMenu.Add(cutItem);
            
            var copyItem = new NativeMenuItem("Copy");
            copyItem.Click += (s, e) => ExecuteCommand(desktop, "CopySelectedTaskCommand");
            editSubMenu.Add(copyItem);
            
            var pasteItem = new NativeMenuItem("Paste");
            pasteItem.Click += (s, e) => ExecuteCommand(desktop, "PasteTasksCommand");
            editSubMenu.Add(pasteItem);
            
            editMenu.Menu = editSubMenu;
            nativeMenu.Add(editMenu);
            
            // Task Menu
            var taskMenu = new NativeMenuItem("Task");
            var taskSubMenu = new NativeMenu();
            
            var updateItem = new NativeMenuItem("Update Task");
            updateItem.Click += (s, e) => ExecuteCommand(desktop, "EditSelectedTaskCommand");
            taskSubMenu.Add(updateItem);
            
            var deleteItem = new NativeMenuItem("Delete Task");
            deleteItem.Click += (s, e) => ExecuteCommand(desktop, "DeleteSelectedTaskCommand");
            taskSubMenu.Add(deleteItem);
            
            taskSubMenu.Add(new NativeMenuItemSeparator());
            
            var toggleItem = new NativeMenuItem("Toggle Completion");
            toggleItem.Click += (s, e) => ExecuteCommand(desktop, "ToggleTaskCompletionCommand");
            taskSubMenu.Add(toggleItem);
            
            taskMenu.Menu = taskSubMenu;
            nativeMenu.Add(taskMenu);
            
            // Sort Menu
            var sortMenu = new NativeMenuItem("Sort");
            var sortSubMenu = new NativeMenu();
            
            var orderItem = new NativeMenuItem("Order in file");
            orderItem.Click += (s, e) => ExecuteCommand(desktop, "SortByOrderInFileCommand");
            sortSubMenu.Add(orderItem);
            
            var alphaItem = new NativeMenuItem("Alphabetical");
            alphaItem.Click += (s, e) => ExecuteCommand(desktop, "SortByAlphabeticalCommand");
            sortSubMenu.Add(alphaItem);
            
            var priorityItem = new NativeMenuItem("Priority");
            priorityItem.Click += (s, e) => ExecuteCommand(desktop, "SortByPriorityCommand");
            sortSubMenu.Add(priorityItem);
            
            sortMenu.Menu = sortSubMenu;
            nativeMenu.Add(sortMenu);
            
            // Filter Menu
            var filterMenu = new NativeMenuItem("Filter");
            var filterSubMenu = new NativeMenu();
            
            var defineItem = new NativeMenuItem("Define Filters");
            defineItem.Click += (s, e) => ExecuteCommand(desktop, "ShowFilterDialogCommand");
            filterSubMenu.Add(defineItem);
            
            var removeItem = new NativeMenuItem("Remove Filter");
            removeItem.Click += (s, e) => ExecuteCommand(desktop, "RemoveFilterCommand");
            filterSubMenu.Add(removeItem);
            
            filterMenu.Menu = filterSubMenu;
            nativeMenu.Add(filterMenu);
            
            // Help Menu
            var helpMenu = new NativeMenuItem("Help");
            var helpSubMenu = new NativeMenu();
            
            var aboutItem = new NativeMenuItem("About TodoTxt");
            aboutItem.Click += (s, e) => ExecuteCommand(desktop, "ShowHelpDialogCommand");
            helpSubMenu.Add(aboutItem);
            
            helpMenu.Menu = helpSubMenu;
            nativeMenu.Add(helpMenu);
            
            // Set the native menu
            if (desktop.MainWindow != null)
            {
                NativeMenu.SetMenu(desktop.MainWindow, nativeMenu);
            }
            
            System.Diagnostics.Debug.WriteLine("Native menu initialized successfully");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize native menu: {ex.Message}");
        }
    }
    
    private void ExecuteCommand(IClassicDesktopStyleApplicationLifetime desktop, string commandName)
    {
        try
        {
            if (desktop.MainWindow?.DataContext is MainWindowViewModel viewModel)
            {
                // Use reflection to find and execute the command
                var commandProperty = typeof(MainWindowViewModel).GetProperty($"{commandName}");
                if (commandProperty?.GetValue(viewModel) is IRelayCommand command)
                {
                    command.Execute(null);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error executing command {commandName}: {ex.Message}");
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