using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ToDoLib;

namespace TodoTxt.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private TaskList? _taskList;
    
    [ObservableProperty]
    private ObservableCollection<ToDoLib.Task> _tasks = new();
    
    [ObservableProperty]
    private ToDoLib.Task? _selectedItem;
    
    [ObservableProperty]
    private int _totalTasks;
    
    [ObservableProperty]
    private int _filteredTasks;
    
    [ObservableProperty]
    private int _incompleteTasks;

    public MainWindowViewModel()
    {
        // Initialize with some sample tasks for demonstration
        LoadSampleTasks();
    }

    private void LoadSampleTasks()
    {
        Tasks.Clear();
        Tasks.Add(new ToDoLib.Task("(A) Call Mom +family @home"));
        Tasks.Add(new ToDoLib.Task("Buy groceries @errands"));
        Tasks.Add(new ToDoLib.Task("x 2024-01-15 Complete project documentation +work"));
        
        UpdateTaskCounts();
    }

    public void LoadTasks(string filePath)
    {
        try
        {
            _taskList = new TaskList(filePath);
            Tasks.Clear();
            
            foreach (var task in _taskList.Tasks)
            {
                Tasks.Add(task);
            }
            
            UpdateTaskCounts();
        }
        catch (System.Exception ex)
        {
            // Handle error - in a real implementation, show a dialog
            System.Diagnostics.Debug.WriteLine($"Error loading tasks: {ex.Message}");
        }
    }

    private void UpdateTaskCounts()
    {
        TotalTasks = Tasks.Count;
        FilteredTasks = Tasks.Count; // For now, no filtering
        IncompleteTasks = Tasks.Count(t => !t.Completed);
    }
}
