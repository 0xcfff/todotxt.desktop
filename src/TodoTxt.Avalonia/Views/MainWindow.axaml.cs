using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TodoTxt.Avalonia.ViewModels;

namespace TodoTxt.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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
}