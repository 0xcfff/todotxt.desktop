using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TodoTxt.Avalonia.ViewModels;

namespace TodoTxt.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        System.Diagnostics.Debug.WriteLine("MainWindow constructor started");
        
        InitializeComponent();
        
        System.Diagnostics.Debug.WriteLine("InitializeComponent completed");
        
        // Debug: Check if the TaskTextBox is found
        if (TaskTextBox != null)
        {
            System.Diagnostics.Debug.WriteLine("TaskTextBox found and initialized");
            System.Diagnostics.Debug.WriteLine($"TaskTextBox IsVisible: {TaskTextBox.IsVisible}");
            System.Diagnostics.Debug.WriteLine($"TaskTextBox Opacity: {TaskTextBox.Opacity}");
            System.Diagnostics.Debug.WriteLine($"TaskTextBox Height: {TaskTextBox.Height}");
            System.Diagnostics.Debug.WriteLine($"TaskTextBox Background: {TaskTextBox.Background}");
            System.Diagnostics.Debug.WriteLine($"TaskTextBox Grid.Row: {Grid.GetRow(TaskTextBox)}");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("TaskTextBox is null!");
        }
        
        // Also check the Grid
        if (MainGrid != null)
        {
            System.Diagnostics.Debug.WriteLine($"MainGrid found with {MainGrid.RowDefinitions.Count} rows");
            for (int i = 0; i < MainGrid.RowDefinitions.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"Row {i}: Height = {MainGrid.RowDefinitions[i].Height}");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("MainGrid is null!");
        }
        
        System.Diagnostics.Debug.WriteLine("MainWindow constructor completed");
        
        // Add Loaded event handler
        this.Loaded += MainWindow_Loaded;
        
        // Add KeyDown event handler for global shortcuts
        this.KeyDown += MainWindow_KeyDown;
    }
    
    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("MainWindow Loaded event fired");
        
        if (TaskTextBox != null)
        {
            System.Diagnostics.Debug.WriteLine("TaskTextBox in Loaded event:");
            System.Diagnostics.Debug.WriteLine($"  IsVisible: {TaskTextBox.IsVisible}");
            System.Diagnostics.Debug.WriteLine($"  Opacity: {TaskTextBox.Opacity}");
            System.Diagnostics.Debug.WriteLine($"  Height: {TaskTextBox.Height}");
            System.Diagnostics.Debug.WriteLine($"  ActualHeight: {TaskTextBox.Bounds.Height}");
            System.Diagnostics.Debug.WriteLine($"  Background: {TaskTextBox.Background}");
            System.Diagnostics.Debug.WriteLine($"  Bounds: {TaskTextBox.Bounds}");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("TaskTextBox is null in Loaded event!");
        }
    }

    private void TaskTextBox_KeyUp(object? sender, KeyEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.HandleTaskInputKeyPress(e);
        }
    }

    private void TaskListBox_DoubleTapped(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.EditSelectedTaskCommand.Execute(null);
        }
    }

    private void TaskListBox_KeyUp(object? sender, KeyEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            switch (e.Key)
            {
                case Key.Delete:
                case Key.Back:
                    viewModel.DeleteSelectedTaskCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.X:
                    viewModel.ToggleTaskCompletionCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Key.F2:
                case Key.Enter:
                    viewModel.EditSelectedTaskCommand.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
    }

    private void SearchTextBox_KeyUp(object? sender, KeyEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            if (e.Key == Key.Enter)
            {
                viewModel.SearchTasksCommand.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                viewModel.ClearSearchCommand.Execute(null);
                e.Handled = true;
            }
        }
    }

    private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            // Handle Ctrl+key combinations
            if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                switch (e.Key)
                {
                    case Key.F:
                        // Focus search box
                        if (SearchTextBox != null)
                        {
                            SearchTextBox.Focus();
                            e.Handled = true;
                        }
                        break;
                    case Key.G:
                        // Toggle grouping
                        viewModel.ToggleGroupingCommand.Execute(null);
                        e.Handled = true;
                        break;
                    case Key.H:
                        // Toggle hidden tasks
                        viewModel.ToggleShowHiddenTasksCommand.Execute(null);
                        e.Handled = true;
                        break;
                    case Key.T:
                        // Toggle future tasks filter
                        viewModel.ToggleFilterFutureTasksCommand.Execute(null);
                        e.Handled = true;
                        break;
                    case Key.C:
                        // Toggle case sensitivity
                        viewModel.ToggleFilterCaseSensitivityCommand.Execute(null);
                        e.Handled = true;
                        break;
                }
            }
            // Handle F-key shortcuts
            else
            {
                switch (e.Key)
                {
                    case Key.F3:
                        // Clear search
                        viewModel.ClearSearchCommand.Execute(null);
                        e.Handled = true;
                        break;
                    case Key.F4:
                        // Remove filter
                        viewModel.RemoveFilterCommand.Execute(null);
                        e.Handled = true;
                        break;
                }
            }
        }
    }
}