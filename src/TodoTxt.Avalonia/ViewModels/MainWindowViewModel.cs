using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLib;
using TodoTxt.Avalonia.Models;

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
    
    [ObservableProperty]
    private string _taskInputText = string.Empty;
    
    private ToDoLib.Task? _updatingTask;
    private string? _currentFilePath;
    
    [ObservableProperty]
    private SortType _currentSortType = SortType.None;

    /// <summary>
    /// Exposes the TaskList for IntellisenseTextBox autocompletion
    /// </summary>
    public TaskList? TaskList => _taskList;

    public MainWindowViewModel()
    {
        // Initialize with some sample tasks for demonstration
        LoadSampleTasks();
    }

    private void LoadSampleTasks()
    {
        Tasks.Clear();
        
        // Create a temporary TaskList for Intellisense functionality
        var tempFilePath = Path.Combine(Path.GetTempPath(), "temp_todo.txt");
        
        // Create the temporary file if it doesn't exist
        if (!File.Exists(tempFilePath))
        {
            File.WriteAllText(tempFilePath, "");
        }
        
        _taskList = new TaskList(tempFilePath);
        _taskList.Add(new ToDoLib.Task("(A) Call Mom +family @home"));
        _taskList.Add(new ToDoLib.Task("Buy groceries @errands"));
        _taskList.Add(new ToDoLib.Task("x 2024-01-15 Complete project documentation +work"));
        
        // Add tasks to the observable collection for display
        foreach (var task in _taskList.Tasks)
        {
            Tasks.Add(task);
        }
        
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

    /// <summary>
    /// Adds a new task from the input text box
    /// </summary>
    [RelayCommand]
    public void AddTask()
    {
        if (string.IsNullOrWhiteSpace(TaskInputText))
            return;

        try
        {
            var newTask = new ToDoLib.Task(TaskInputText.Trim());
            
            if (_taskList != null)
            {
                _taskList.Add(newTask);
            }
            
            Tasks.Add(newTask);
            UpdateTaskCounts();
            
            // Clear the input text
            TaskInputText = string.Empty;
        }
        catch (TaskException ex)
        {
            // TODO: Show error dialog
            System.Diagnostics.Debug.WriteLine($"Error adding task: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates the currently selected task with text from the input box
    /// </summary>
    [RelayCommand]
    public void UpdateTask()
    {
        if (string.IsNullOrWhiteSpace(TaskInputText) || _updatingTask == null)
            return;

        try
        {
            var updatedTask = new ToDoLib.Task(TaskInputText.Trim());
            
            if (_taskList != null)
            {
                _taskList.Update(_updatingTask, updatedTask);
            }
            
            // Find and replace the task in the collection
            var index = Tasks.IndexOf(_updatingTask);
            if (index >= 0)
            {
                Tasks[index] = updatedTask;
            }
            
            UpdateTaskCounts();
            
            // Clear the input text and reset updating state
            TaskInputText = string.Empty;
            _updatingTask = null;
        }
        catch (TaskException ex)
        {
            // TODO: Show error dialog
            System.Diagnostics.Debug.WriteLine($"Error updating task: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles key press events from the task input TextBox
    /// </summary>
    public void HandleTaskInputKeyPress(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (_updatingTask != null)
            {
                UpdateTask();
            }
            else
            {
                AddTask();
            }
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            // Cancel editing
            TaskInputText = string.Empty;
            _updatingTask = null;
            e.Handled = true;
        }
    }

    /// <summary>
    /// Starts editing the selected task
    /// </summary>
    [RelayCommand]
    public void EditSelectedTask()
    {
        if (SelectedItem != null)
        {
            _updatingTask = SelectedItem;
            TaskInputText = SelectedItem.Raw;
        }
    }

    /// <summary>
    /// Deletes the selected task
    /// </summary>
    [RelayCommand]
    public void DeleteSelectedTask()
    {
        if (SelectedItem != null)
        {
            if (_taskList != null)
            {
                _taskList.Delete(SelectedItem);
            }
            
            Tasks.Remove(SelectedItem);
            UpdateTaskCounts();
        }
    }

    /// <summary>
    /// Toggles completion status of the selected task
    /// </summary>
    [RelayCommand]
    public void ToggleTaskCompletion()
    {
        if (SelectedItem != null)
        {
            var updatedTask = new ToDoLib.Task(SelectedItem.Raw);
            updatedTask.Completed = !updatedTask.Completed;
            
            if (_taskList != null)
            {
                _taskList.Update(SelectedItem, updatedTask);
            }
            
            // Find and replace the task in the collection
            var index = Tasks.IndexOf(SelectedItem);
            if (index >= 0)
            {
                Tasks[index] = updatedTask;
            }
            
            UpdateTaskCounts();
        }
    }

    #region File Operations

    /// <summary>
    /// Creates a new empty task list
    /// </summary>
    [RelayCommand]
    public void NewFile()
    {
        Tasks.Clear();
        _taskList = null;
        _currentFilePath = null;
        UpdateTaskCounts();
    }

    /// <summary>
    /// Opens a todo.txt file
    /// </summary>
    [RelayCommand]
    public void OpenFile()
    {
        // TODO: Implement file dialog
        // For now, we'll use a hardcoded path for testing
        var testFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "todo.txt");
        
        if (File.Exists(testFilePath))
        {
            LoadTasks(testFilePath);
            _currentFilePath = testFilePath;
        }
        else
        {
            // Create a new file if it doesn't exist
            File.WriteAllText(testFilePath, "");
            LoadTasks(testFilePath);
            _currentFilePath = testFilePath;
        }
    }

    /// <summary>
    /// Reloads the current file
    /// </summary>
    [RelayCommand]
    public void ReloadFile()
    {
        if (!string.IsNullOrEmpty(_currentFilePath) && File.Exists(_currentFilePath))
        {
            LoadTasks(_currentFilePath);
        }
    }

    /// <summary>
    /// Saves the current task list to file
    /// </summary>
    [RelayCommand]
    public void SaveFile()
    {
        if (_taskList != null && !string.IsNullOrEmpty(_currentFilePath))
        {
            try
            {
                // The TaskList automatically saves when tasks are modified
                // This method is here for explicit save operations
                System.Diagnostics.Debug.WriteLine($"Tasks saved to {_currentFilePath}");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving file: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Archives completed tasks
    /// </summary>
    [RelayCommand]
    public void ArchiveCompletedTasks()
    {
        if (_taskList == null) return;

        var completedTasks = Tasks.Where(t => t.Completed).ToList();
        
        if (completedTasks.Count == 0)
        {
            System.Diagnostics.Debug.WriteLine("No completed tasks to archive");
            return;
        }

        try
        {
            // Create archive file path
            var archivePath = _currentFilePath?.Replace(".txt", "_archive.txt") ?? "todo_archive.txt";
            
            // Append completed tasks to archive file
            using (var writer = new StreamWriter(archivePath, true))
            {
                foreach (var task in completedTasks)
                {
                    writer.WriteLine(task.Raw);
                }
            }

            // Remove completed tasks from current list
            foreach (var task in completedTasks)
            {
                _taskList.Delete(task);
                Tasks.Remove(task);
            }

            UpdateTaskCounts();
            System.Diagnostics.Debug.WriteLine($"Archived {completedTasks.Count} completed tasks");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error archiving tasks: {ex.Message}");
        }
    }

    #endregion

    #region Edit Operations

    /// <summary>
    /// Copies the selected task to clipboard
    /// </summary>
    [RelayCommand]
    public void CopySelectedTask()
    {
        if (SelectedItem != null)
        {
            // TODO: Implement clipboard functionality
            // For now, just log the action
            System.Diagnostics.Debug.WriteLine($"Copying task: {SelectedItem.Raw}");
        }
    }

    /// <summary>
    /// Cuts the selected task (copies and deletes)
    /// </summary>
    [RelayCommand]
    public void CutSelectedTask()
    {
        if (SelectedItem != null)
        {
            // TODO: Implement clipboard functionality
            // For now, just copy and delete
            System.Diagnostics.Debug.WriteLine($"Cutting task: {SelectedItem.Raw}");
            DeleteSelectedTask();
        }
    }

    /// <summary>
    /// Pastes tasks from clipboard
    /// </summary>
    [RelayCommand]
    public void PasteTasks()
    {
        // TODO: Implement clipboard functionality
        // For now, just log the action
        System.Diagnostics.Debug.WriteLine("Pasting tasks from clipboard");
    }

    #endregion

    #region Sorting Operations

    /// <summary>
    /// Sorts the task list by the specified sort type
    /// </summary>
    [RelayCommand]
    public void SortByOrderInFile()
    {
        CurrentSortType = SortType.None;
        ApplySorting();
    }

    /// <summary>
    /// Sorts the task list alphabetically
    /// </summary>
    [RelayCommand]
    public void SortByAlphabetical()
    {
        CurrentSortType = SortType.Alphabetical;
        ApplySorting();
    }

    /// <summary>
    /// Sorts the task list by priority
    /// </summary>
    [RelayCommand]
    public void SortByPriority()
    {
        CurrentSortType = SortType.Priority;
        ApplySorting();
    }

    /// <summary>
    /// Applies the current sorting to the task list
    /// </summary>
    private void ApplySorting()
    {
        if (_taskList == null) return;

        var sortedTasks = SortTasks(_taskList.Tasks, CurrentSortType);
        
        Tasks.Clear();
        foreach (var task in sortedTasks)
        {
            Tasks.Add(task);
        }
        
        UpdateTaskCounts();
    }

    /// <summary>
    /// Sorts a collection of tasks based on the specified sort type
    /// </summary>
    private IEnumerable<ToDoLib.Task> SortTasks(IEnumerable<ToDoLib.Task> tasks, SortType sortType)
    {
        return sortType switch
        {
            SortType.None => tasks,
            SortType.Alphabetical => tasks.OrderBy(t => t.Raw),
            SortType.Priority => tasks.OrderBy(t => string.IsNullOrEmpty(t.Priority) ? "(zzz)" : t.Priority)
                .ThenBy(t => t.Completed)
                .ThenBy(t => string.IsNullOrEmpty(t.DueDate) ? "9999-99-99" : t.DueDate)
                .ThenBy(t => string.IsNullOrEmpty(t.CreationDate) ? "0000-00-00" : t.CreationDate),
            SortType.DueDate => tasks.OrderBy(t => string.IsNullOrEmpty(t.DueDate) ? "9999-99-99" : t.DueDate)
                .ThenBy(t => t.Completed)
                .ThenBy(t => string.IsNullOrEmpty(t.Priority) ? "(zzz)" : t.Priority)
                .ThenBy(t => string.IsNullOrEmpty(t.CreationDate) ? "0000-00-00" : t.CreationDate),
            SortType.Created => tasks.OrderBy(t => string.IsNullOrEmpty(t.CreationDate) ? "0000-00-00" : t.CreationDate)
                .ThenBy(t => t.Completed)
                .ThenBy(t => string.IsNullOrEmpty(t.Priority) ? "(zzz)" : t.Priority)
                .ThenBy(t => string.IsNullOrEmpty(t.DueDate) ? "9999-99-99" : t.DueDate),
            SortType.Project => tasks.OrderBy(t =>
                {
                    var s = "";
                    if (t.Projects != null && t.Projects.Count > 0)
                        s += t.PrimaryProject;
                    else
                        s += "zzz";
                    return s;
                })
                .ThenBy(t => t.Completed)
                .ThenBy(t => string.IsNullOrEmpty(t.Priority) ? "(zzz)" : t.Priority)
                .ThenBy(t => string.IsNullOrEmpty(t.DueDate) ? "9999-99-99" : t.DueDate)
                .ThenBy(t => string.IsNullOrEmpty(t.CreationDate) ? "0000-00-00" : t.CreationDate),
            SortType.Context => tasks.OrderBy(t =>
                {
                    var s = "";
                    if (t.Contexts != null && t.Contexts.Count > 0)
                        s += t.PrimaryContext;
                    else
                        s += "zzz";
                    return s;
                })
                .ThenBy(t => t.Completed)
                .ThenBy(t => string.IsNullOrEmpty(t.Priority) ? "(zzz)" : t.Priority)
                .ThenBy(t => string.IsNullOrEmpty(t.DueDate) ? "9999-99-99" : t.DueDate)
                .ThenBy(t => string.IsNullOrEmpty(t.CreationDate) ? "0000-00-00" : t.CreationDate),
            SortType.Completed => tasks.OrderBy(t => t.Completed)
                .ThenBy(t => string.IsNullOrEmpty(t.Priority) ? "(zzz)" : t.Priority)
                .ThenBy(t => string.IsNullOrEmpty(t.DueDate) ? "9999-99-99" : t.DueDate)
                .ThenBy(t => string.IsNullOrEmpty(t.CreationDate) ? "0000-00-00" : t.CreationDate),
            _ => tasks
        };
    }

    #endregion
}
