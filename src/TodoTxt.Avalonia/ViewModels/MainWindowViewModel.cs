using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoLib;
using TodoTxt.Avalonia.Models;
using TodoTxt.Avalonia.Controls;
using TodoTxt.Avalonia.Services;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
    
    [ObservableProperty]
    private bool _allowGrouping = true;
    
    [ObservableProperty]
    private string _filterText = string.Empty;
    
    [ObservableProperty]
    private bool _filterCaseSensitive = false;
    
    [ObservableProperty]
    private bool _showHiddenTasks = false;
    
    [ObservableProperty]
    private bool _filterFutureTasks = false;
    
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    [ObservableProperty]
    private int _tasksOverdue = 0;
    
    [ObservableProperty]
    private int _tasksDueToday = 0;
    
    [ObservableProperty]
    private int _tasksDueThisWeek = 0;
    
    private List<ToDoLib.Task> _allTasks = new();
    private List<ToDoLib.Task> _currentFilteredTasks = new();

    /// <summary>
    /// Exposes the TaskList for IntellisenseTextBox autocompletion
    /// </summary>
    public TaskList? TaskList => _taskList;

    public MainWindowViewModel()
    {
        // Subscribe to property changes to auto-save settings
        PropertyChanged += OnPropertyChanged;
        
        // Initialize asynchronously to avoid blocking the UI thread
        _ = InitializeAsync();
    }
    
    private async System.Threading.Tasks.Task InitializeAsync()
    {
        try
        {
            await System.Threading.Tasks.Task.Run(() => LoadSettings());
            
            // Initialize with some sample tasks for demonstration if no file is loaded
            if (_taskList == null || _taskList.Tasks.Count == 0)
            {
                await System.Threading.Tasks.Task.Run(() => LoadSampleTasks());
            }
            
            // Initialize hotkeys
            await System.Threading.Tasks.Task.Run(() => InitializeHotkeys());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize MainWindowViewModel: {ex.Message}");
            // Fall back to sample tasks
            await System.Threading.Tasks.Task.Run(() => LoadSampleTasks());
        }
    }

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Auto-save certain settings when they change
        if (e.PropertyName == nameof(AllowGrouping) ||
            e.PropertyName == nameof(FilterCaseSensitive) ||
            e.PropertyName == nameof(ShowHiddenTasks) ||
            e.PropertyName == nameof(FilterFutureTasks) ||
            e.PropertyName == nameof(FilterText) ||
            e.PropertyName == nameof(CurrentSortType))
        {
            await SaveSettingsAsync();
        }
    }

    /// <summary>
    /// Saves current settings to the settings service (async version)
    /// </summary>
    private async System.Threading.Tasks.Task SaveSettingsAsync()
    {
        var settings = ServiceLocator.ApplicationSettings;
        
        // Save current property values to settings
        settings.AllowGrouping = AllowGrouping;
        settings.FilterCaseSensitive = FilterCaseSensitive;
        settings.ShowHiddenTasks = ShowHiddenTasks;
        settings.FilterFutureTasks = FilterFutureTasks;
        settings.FilterText = FilterText;
        settings.CurrentSort = (int)CurrentSortType;
        
        if (!string.IsNullOrEmpty(_currentFilePath))
        {
            settings.FilePath = _currentFilePath;
        }
        
        await ServiceLocator.SaveSettingsAsync();
    }

    /// <summary>
    /// Loads settings from the settings service
    /// </summary>
    private void LoadSettings()
    {
        var settings = ServiceLocator.ApplicationSettings;
        
        // Apply settings to properties
        AllowGrouping = settings.AllowGrouping;
        FilterCaseSensitive = settings.FilterCaseSensitive;
        ShowHiddenTasks = settings.ShowHiddenTasks;
        FilterFutureTasks = settings.FilterFutureTasks;
        
        // Load file path if available
        if (!string.IsNullOrEmpty(settings.FilePath) && File.Exists(settings.FilePath))
        {
            LoadTasks(settings.FilePath);
        }
    }


    private void LoadSampleTasks()
    {
        Tasks.Clear();
        _allTasks.Clear();
        
        // Create a temporary TaskList for Intellisense functionality
        var tempFilePath = Path.Combine(Path.GetTempPath(), "temp_todo.txt");
        
        // Create the temporary file if it doesn't exist
        if (!File.Exists(tempFilePath))
        {
            File.WriteAllText(tempFilePath, "");
        }
        
        _taskList = new TaskList(tempFilePath);
        _taskList.Add(new ToDoLib.Task("(A) Call Mom +family @home due:2024-01-20"));
        _taskList.Add(new ToDoLib.Task("(B) Buy groceries @errands due:2024-01-18"));
        _taskList.Add(new ToDoLib.Task("x 2024-01-15 Complete project documentation +work"));
        _taskList.Add(new ToDoLib.Task("(C) Review code +work @office due:2024-01-19"));
        _taskList.Add(new ToDoLib.Task("h:1 Private task +personal"));
        
        // Store all tasks in the internal list
        _allTasks.AddRange(_taskList.Tasks);
        
        // Apply current filtering and sorting
        ApplyFiltersAndSorting();
        
        UpdateTaskCounts();
    }

    public async void LoadTasks(string filePath)
    {
        try
        {
            _currentFilePath = filePath;
            _taskList = new TaskList(filePath);
            Tasks.Clear();
            _allTasks.Clear();
            
            // Store all tasks in the internal list
            _allTasks.AddRange(_taskList.Tasks);
            
            // Apply current filtering and sorting
            ApplyFiltersAndSorting();
            
            UpdateTaskCounts();
            
            // Save the file path to settings
            var settings = ServiceLocator.ApplicationSettings;
            settings.FilePath = filePath;
            await ServiceLocator.SaveSettingsAsync();
        }
        catch (System.Exception ex)
        {
            // Handle error - in a real implementation, show a dialog
            System.Diagnostics.Debug.WriteLine($"Error loading tasks: {ex.Message}");
        }
    }

    /// <summary>
    /// Applies current filters and sorting to the task list
    /// </summary>
    private void ApplyFiltersAndSorting()
    {
        // Start with all tasks
        var tasks = new List<ToDoLib.Task>(_allTasks);
        
        // Apply filters
        tasks = ApplyFilters(tasks);
        
        // Apply search
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            tasks = ApplySearch(tasks, SearchText);
        }
        
        // Store filtered tasks
        _currentFilteredTasks = tasks;
        
        // Apply sorting
        var sortedTasks = SortTasks(tasks, CurrentSortType);
        
        // Update the observable collection
        Tasks.Clear();
        foreach (var task in sortedTasks)
        {
            Tasks.Add(task);
        }
        
        UpdateTaskCounts();
    }
    
    /// <summary>
    /// Applies filters to the task list
    /// </summary>
    private List<ToDoLib.Task> ApplyFilters(List<ToDoLib.Task> tasks)
    {
        var filteredTasks = new List<ToDoLib.Task>();
        var comparer = FilterCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
        
        foreach (var task in tasks)
        {
            bool include = true;
            
            // Hide hidden tasks if not showing them
            if (!ShowHiddenTasks && task.Raw.Contains("h:1"))
                include = false;
            
            // Filter future tasks if enabled
            if (include && FilterFutureTasks)
            {
                if (!string.IsNullOrEmpty(task.ThresholdDate) && 
                    DateTime.TryParse(task.ThresholdDate, out var thresholdDate) &&
                    thresholdDate > DateTime.Now.AddDays(1))
                {
                    include = false;
                }
            }
            
            // Apply text filters
            if (include && !string.IsNullOrWhiteSpace(FilterText))
            {
                var filters = FilterText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var filter in filters)
                {
                    if (!ApplySingleFilter(task, filter, comparer))
                    {
                        include = false;
                        break;
                    }
                }
            }
            
            if (include)
                filteredTasks.Add(task);
        }
        
        return filteredTasks;
    }
    
    /// <summary>
    /// Applies a single filter to a task
    /// </summary>
    private bool ApplySingleFilter(ToDoLib.Task task, string filter, StringComparison comparer)
    {
        // Handle special date filters
        if (filter.Equals("due:today", StringComparison.OrdinalIgnoreCase))
        {
            return task.DueDate == DateTime.Now.ToString("yyyy-MM-dd");
        }
        if (filter.Equals("due:future", StringComparison.OrdinalIgnoreCase))
        {
            return !string.IsNullOrEmpty(task.DueDate) && 
                   DateTime.TryParse(task.DueDate, out var dueDate) && 
                   dueDate > DateTime.Now;
        }
        if (filter.Equals("due:past", StringComparison.OrdinalIgnoreCase))
        {
            return !string.IsNullOrEmpty(task.DueDate) && 
                   DateTime.TryParse(task.DueDate, out var dueDate) && 
                   dueDate < DateTime.Now;
        }
        if (filter.Equals("due:active", StringComparison.OrdinalIgnoreCase))
        {
            return !string.IsNullOrEmpty(task.DueDate) && 
                   DateTime.TryParse(task.DueDate, out var dueDate) && 
                   dueDate <= DateTime.Now;
        }
        
        // Handle negative date filters
        if (filter.Equals("-due:today", StringComparison.OrdinalIgnoreCase))
        {
            return task.DueDate != DateTime.Now.ToString("yyyy-MM-dd");
        }
        if (filter.Equals("-due:future", StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrEmpty(task.DueDate) || 
                   !DateTime.TryParse(task.DueDate, out var dueDate) || 
                   dueDate <= DateTime.Now;
        }
        if (filter.Equals("-due:past", StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrEmpty(task.DueDate) || 
                   !DateTime.TryParse(task.DueDate, out var dueDate) || 
                   dueDate >= DateTime.Now;
        }
        if (filter.Equals("-due:active", StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrEmpty(task.DueDate) || 
                   !DateTime.TryParse(task.DueDate, out var dueDate) || 
                   dueDate > DateTime.Now;
        }
        
        // Handle completion filters
        if (filter.Equals("DONE", StringComparison.Ordinal))
        {
            return task.Completed;
        }
        if (filter.Equals("-DONE", StringComparison.Ordinal))
        {
            return !task.Completed;
        }
        
        // Handle text filters
        if (filter.StartsWith("-"))
        {
            // Negative filter - exclude if contains
            return !task.Raw.Contains(filter.Substring(1), comparer);
        }
        else
        {
            // Positive filter - include if contains
            return task.Raw.Contains(filter, comparer);
        }
    }
    
    /// <summary>
    /// Applies search to the task list
    /// </summary>
    private List<ToDoLib.Task> ApplySearch(List<ToDoLib.Task> tasks, string searchText)
    {
        var comparer = FilterCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
        return tasks.Where(t => t.Raw.Contains(searchText, comparer)).ToList();
    }
    
    /// <summary>
    /// Updates task statistics
    /// </summary>
    private void UpdateTaskCounts()
    {
        TotalTasks = _allTasks.Count;
        FilteredTasks = _currentFilteredTasks.Count;
        IncompleteTasks = _currentFilteredTasks.Count(t => !t.Completed);
        
        // Calculate due date statistics
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        var weekFromNow = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        
        TasksOverdue = _currentFilteredTasks.Count(t => !t.Completed && 
            !string.IsNullOrEmpty(t.DueDate) && 
            DateTime.TryParse(t.DueDate, out var dueDate) && 
            dueDate < DateTime.Now);
            
        TasksDueToday = _currentFilteredTasks.Count(t => !t.Completed && t.DueDate == today);
        
        TasksDueThisWeek = _currentFilteredTasks.Count(t => !t.Completed && 
            !string.IsNullOrEmpty(t.DueDate) && 
            DateTime.TryParse(t.DueDate, out var dueDate) && 
            dueDate >= DateTime.Now && dueDate <= DateTime.Now.AddDays(7));
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
            
            // Add to the internal list
            _allTasks.Add(newTask);
            
            // Apply current filtering and sorting
            ApplyFiltersAndSorting();
            
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
            
            // Find and replace the task in the internal list
            var index = _allTasks.IndexOf(_updatingTask);
            if (index >= 0)
            {
                _allTasks[index] = updatedTask;
            }
            
            // Apply current filtering and sorting
            ApplyFiltersAndSorting();
            
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
    public async System.Threading.Tasks.Task DeleteSelectedTask()
    {
        if (SelectedItem != null)
        {
            // Show confirmation dialog
            var confirmed = await ShowDeleteConfirmationDialog();
            if (confirmed)
            {
                if (_taskList != null)
                {
                    _taskList.Delete(SelectedItem);
                }
                
                // Remove from internal list
                _allTasks.Remove(SelectedItem);
                
                // Apply current filtering and sorting
                ApplyFiltersAndSorting();
            }
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
            
            // Find and replace the task in the internal list
            var index = _allTasks.IndexOf(SelectedItem);
            if (index >= 0)
            {
                _allTasks[index] = updatedTask;
            }
            
            // Apply current filtering and sorting
            ApplyFiltersAndSorting();
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
        _allTasks.Clear();
        _currentFilteredTasks.Clear();
        _taskList = null;
        _currentFilePath = null;
        UpdateTaskCounts();
    }

    /// <summary>
    /// Opens a todo.txt file
    /// </summary>
    [RelayCommand]
    public async System.Threading.Tasks.Task OpenFile()
    {
        try
        {
            var filePath = await ServiceLocator.FileDialogService.ShowOpenFileDialogAsync(
                title: "Open Todo File",
                initialDirectory: Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                filter: "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                defaultExtension: ".txt"
            );

            if (!string.IsNullOrEmpty(filePath))
            {
                LoadTasks(filePath);
                _currentFilePath = filePath;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error opening file dialog: {ex.Message}");
            // Fall back to default behavior
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
    /// Saves the current task list to a new file
    /// </summary>
    [RelayCommand]
    public async System.Threading.Tasks.Task SaveAsFile()
    {
        if (_taskList == null) return;

        try
        {
            var filePath = await ServiceLocator.FileDialogService.ShowSaveFileDialogAsync(
                title: "Save Todo File As",
                initialDirectory: Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                initialFileName: "todo.txt",
                filter: "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                defaultExtension: ".txt"
            );

            if (!string.IsNullOrEmpty(filePath))
            {
                // Save tasks to the new file
                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var task in _allTasks)
                    {
                        writer.WriteLine(task.Raw);
                    }
                }
                
                _currentFilePath = filePath;
                System.Diagnostics.Debug.WriteLine($"Tasks saved to {filePath}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving file as: {ex.Message}");
        }
    }

    /// <summary>
    /// Prints the current task list
    /// </summary>
    [RelayCommand]
    public async System.Threading.Tasks.Task PrintTasks()
    {
        if (_allTasks.Count == 0)
        {
            System.Diagnostics.Debug.WriteLine("No tasks to print");
            return;
        }

        try
        {
            // Create a formatted text representation of the tasks
            var printContent = FormatTasksForPrinting();
            
            // Use the platform print service
            var success = await ServiceLocator.PrintService.PrintTextAsync(printContent);
            
            if (success)
            {
                System.Diagnostics.Debug.WriteLine("Tasks printed successfully");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Printing failed or was cancelled");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error printing tasks: {ex.Message}");
        }
    }

    /// <summary>
    /// Formats tasks for printing
    /// </summary>
    private string FormatTasksForPrinting()
    {
        var content = new System.Text.StringBuilder();
        
        // Add header
        content.AppendLine("Todo List");
        content.AppendLine("=========");
        content.AppendLine();
        
        // Add file info
        if (!string.IsNullOrEmpty(_currentFilePath))
        {
            content.AppendLine($"File: {_currentFilePath}");
            content.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            content.AppendLine();
        }
        
        // Add task counts
        content.AppendLine($"Total Tasks: {TotalTasks}");
        content.AppendLine($"Incomplete: {IncompleteTasks}");
        content.AppendLine($"Completed: {TotalTasks - IncompleteTasks}");
        content.AppendLine();
        
        // Add tasks
        if (_allTasks.Count > 0)
        {
            content.AppendLine("Tasks:");
            content.AppendLine("------");
            
            foreach (var task in _allTasks)
            {
                var status = task.Completed ? "[X]" : "[ ]";
                var priority = !string.IsNullOrEmpty(task.Priority) ? $"({task.Priority})" : "";
                var dueDate = !string.IsNullOrEmpty(task.DueDate) ? $" due:{task.DueDate}" : "";
                var context = task.Contexts.Count > 0 ? $" @{string.Join(" @", task.Contexts)}" : "";
                var project = task.Projects.Count > 0 ? $" +{string.Join(" +", task.Projects)}" : "";
                
                content.AppendLine($"{status} {priority}{task.Body}{dueDate}{context}{project}");
            }
        }
        
        return content.ToString();
    }

    /// <summary>
    /// Archives completed tasks
    /// </summary>
    [RelayCommand]
    public void ArchiveCompletedTasks()
    {
        if (_taskList == null) return;

        var completedTasks = _allTasks.Where(t => t.Completed).ToList();
        
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
                _allTasks.Remove(task);
            }

            // Apply current filtering and sorting
            ApplyFiltersAndSorting();
            
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
            _ = DeleteSelectedTask();
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
        // Apply current filtering and sorting
        ApplyFiltersAndSorting();
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

    #region Filtering and Search Operations

    /// <summary>
    /// Applies a filter preset
    /// </summary>
    [RelayCommand]
    public void ApplyFilterPreset(int presetNumber)
    {
        // TODO: Load filter presets from settings
        // For now, just clear the filter
        FilterText = string.Empty;
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Removes the current filter
    /// </summary>
    [RelayCommand]
    public void RemoveFilter()
    {
        FilterText = string.Empty;
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Toggles case sensitivity for filtering
    /// </summary>
    [RelayCommand]
    public void ToggleFilterCaseSensitivity()
    {
        FilterCaseSensitive = !FilterCaseSensitive;
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Toggles showing hidden tasks
    /// </summary>
    [RelayCommand]
    public void ToggleShowHiddenTasks()
    {
        ShowHiddenTasks = !ShowHiddenTasks;
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Toggles filtering future tasks
    /// </summary>
    [RelayCommand]
    public void ToggleFilterFutureTasks()
    {
        FilterFutureTasks = !FilterFutureTasks;
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Performs a search on the task list
    /// </summary>
    [RelayCommand]
    public void SearchTasks()
    {
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Clears the search text
    /// </summary>
    [RelayCommand]
    public void ClearSearch()
    {
        SearchText = string.Empty;
        ApplyFiltersAndSorting();
    }

    /// <summary>
    /// Toggles grouping of tasks
    /// </summary>
    [RelayCommand]
    public void ToggleGrouping()
    {
        AllowGrouping = !AllowGrouping;
        ApplyFiltersAndSorting();
    }

    #endregion

    #region Dialog Operations

    /// <summary>
    /// Shows the Options dialog
    /// </summary>
    [RelayCommand]
    public async System.Threading.Tasks.Task ShowOptionsDialog()
    {
        var dialog = new OptionsDialog();
        var settings = ServiceLocator.ApplicationSettings;
        
        // Load current settings into dialog
        dialog.ArchiveFile = settings.ArchiveFilePath;
        dialog.AutoArchive = settings.AutoArchive;
        dialog.AutoSelectArchivePath = settings.AutoSelectArchivePath;
        dialog.CurrentFont = $"{settings.TaskListFontFamily}, {settings.TaskListFontSize}pt";
        dialog.AddCreationDate = settings.AddCreationDate;
        dialog.MoveFocusToTaskListAfterAddingNewTask = settings.MoveFocusToTaskListAfterAddingNewTask;
        dialog.AutoRefresh = settings.AutoRefresh;
        dialog.CaseSensitiveFilter = settings.FilterCaseSensitive;
        dialog.IntellisenseCaseSensitive = settings.IntellisenseCaseSensitive;
        dialog.MinToSysTray = settings.MinimiseToSystemTray;
        dialog.MinOnClose = settings.MinimiseOnClose;
        dialog.DebugOn = settings.DebugLoggingOn;
        dialog.RequireCtrlEnter = settings.RequireCtrlEnter;
        dialog.AllowGrouping = settings.AllowGrouping;
        dialog.PreserveWhiteSpace = settings.PreserveWhiteSpace;
        dialog.WordWrap = settings.WordWrap;
        dialog.DisplayStatusBar = settings.DisplayStatusBar;
        dialog.CheckForUpdates = settings.CheckForUpdates;
        
        var result = await dialog.ShowDialog();
        if (result == true)
        {
            // Save settings from dialog
            settings.ArchiveFilePath = dialog.ArchiveFile;
            settings.AutoArchive = dialog.AutoArchive;
            settings.AutoSelectArchivePath = dialog.AutoSelectArchivePath;
            settings.AddCreationDate = dialog.AddCreationDate;
            settings.MoveFocusToTaskListAfterAddingNewTask = dialog.MoveFocusToTaskListAfterAddingNewTask;
            settings.AutoRefresh = dialog.AutoRefresh;
            settings.FilterCaseSensitive = dialog.CaseSensitiveFilter;
            settings.IntellisenseCaseSensitive = dialog.IntellisenseCaseSensitive;
            settings.MinimiseToSystemTray = dialog.MinToSysTray;
            settings.MinimiseOnClose = dialog.MinOnClose;
            settings.DebugLoggingOn = dialog.DebugOn;
            settings.RequireCtrlEnter = dialog.RequireCtrlEnter;
            settings.AllowGrouping = dialog.AllowGrouping;
            settings.PreserveWhiteSpace = dialog.PreserveWhiteSpace;
            settings.WordWrap = dialog.WordWrap;
            settings.DisplayStatusBar = dialog.DisplayStatusBar;
            settings.CheckForUpdates = dialog.CheckForUpdates;
            
            // Apply settings to current view model
            AllowGrouping = settings.AllowGrouping;
            FilterCaseSensitive = settings.FilterCaseSensitive;
            
            await ServiceLocator.SaveSettingsAsync();
            System.Diagnostics.Debug.WriteLine("Options saved");
        }
    }

    /// <summary>
    /// Shows the Filter dialog
    /// </summary>
    [RelayCommand]
    public async System.Threading.Tasks.Task ShowFilterDialog()
    {
        var dialog = new FilterDialog();
        // TODO: Load current filter settings
        var result = await dialog.ShowDialog();
        if (result == true)
        {
            // TODO: Apply filter settings
            System.Diagnostics.Debug.WriteLine("Filter settings applied");
        }
    }

    /// <summary>
    /// Shows the Help dialog
    /// </summary>
    [RelayCommand]
    public async System.Threading.Tasks.Task ShowHelpDialog()
    {
        var dialog = new HelpDialog();
        await dialog.ShowDialog();
    }

    /// <summary>
    /// Shows the Delete Confirmation dialog
    /// </summary>
    public async System.Threading.Tasks.Task<bool> ShowDeleteConfirmationDialog()
    {
        var dialog = new DeleteConfirmationDialog();
        var result = await dialog.ShowDialog();
        return result == true;
    }

    /// <summary>
    /// Shows the Append Text dialog
    /// </summary>
    public async System.Threading.Tasks.Task<string?> ShowAppendTextDialog()
    {
        var dialog = new AppendTextDialog();
        var result = await dialog.ShowDialog();
        if (result == true)
        {
            return dialog.TextToAppend;
        }
        return null;
    }

    /// <summary>
    /// Shows the Set Priority dialog
    /// </summary>
    public async System.Threading.Tasks.Task<string?> ShowSetPriorityDialog()
    {
        var dialog = new SetPriorityDialog();
        var result = await dialog.ShowDialog();
        if (result == true)
        {
            return dialog.Priority;
        }
        return null;
    }

    /// <summary>
    /// Shows the Set Due Date dialog
    /// </summary>
    public async System.Threading.Tasks.Task<DateTime?> ShowSetDueDateDialog()
    {
        var dialog = new SetDueDateDialog();
        var result = await dialog.ShowDialog();
        if (result == true)
        {
            return dialog.DueDate;
        }
        return null;
    }

    /// <summary>
    /// Shows the Postpone dialog
    /// </summary>
    public async System.Threading.Tasks.Task<int?> ShowPostponeDialog()
    {
        var dialog = new PostponeDialog();
        var result = await dialog.ShowDialog();
        if (result == true)
        {
            return dialog.DaysToPostpone;
        }
        return null;
    }

    #endregion

    #region Hotkey Management

    /// <summary>
    /// Initializes global hotkeys for the application
    /// </summary>
    private void InitializeHotkeys()
    {
        try
        {
            var hotkeyService = ServiceLocator.HotkeyService;
            
            if (!hotkeyService.IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("Global hotkeys are not supported on this platform");
                return;
            }

            // Subscribe to hotkey events
            hotkeyService.HotkeyPressed += OnHotkeyPressed;

            // Register useful hotkeys based on the application's functionality
            // These match the keyboard shortcuts mentioned in the README
            
            // Ctrl+Alt+M: Hide/Show main window (minimize to tray)
            var hideShowHotkey = new TodoTxt.Platform.Hotkey(true, true, false, false, 77); // M key
            hotkeyService.RegisterHotkey(hideShowHotkey, 1);
            
            // Ctrl+O: Open file
            var openFileHotkey = new TodoTxt.Platform.Hotkey(true, false, false, false, 79); // O key
            hotkeyService.RegisterHotkey(openFileHotkey, 2);
            
            // Ctrl+N: New file
            var newFileHotkey = new TodoTxt.Platform.Hotkey(true, false, false, false, 78); // N key
            hotkeyService.RegisterHotkey(newFileHotkey, 3);
            
            // Ctrl+S: Save file
            var saveFileHotkey = new TodoTxt.Platform.Hotkey(true, false, false, false, 83); // S key
            hotkeyService.RegisterHotkey(saveFileHotkey, 4);
            
            // F5: Reload file
            var reloadFileHotkey = new TodoTxt.Platform.Hotkey(false, false, false, false, 116); // F5 key
            hotkeyService.RegisterHotkey(reloadFileHotkey, 5);
            
            // Ctrl+A: Archive completed tasks
            var archiveHotkey = new TodoTxt.Platform.Hotkey(true, false, false, false, 65); // A key
            hotkeyService.RegisterHotkey(archiveHotkey, 6);
            
            System.Diagnostics.Debug.WriteLine("Hotkey system initialized with 6 global hotkeys");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to initialize hotkeys: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles global hotkey press events
    /// </summary>
    private void OnHotkeyPressed(object? sender, TodoTxt.Platform.HotkeyPressedEventArgs e)
    {
        try
        {
            // Handle different hotkey IDs
            switch (e.HotkeyId)
            {
                case 1: // Ctrl+Alt+M: Hide/Show main window
                    System.Diagnostics.Debug.WriteLine("Hotkey 1 pressed: Hide/Show main window");
                    ToggleMainWindowVisibility();
                    break;
                case 2: // Ctrl+O: Open file
                    System.Diagnostics.Debug.WriteLine("Hotkey 2 pressed: Open file");
                    OpenFileCommand.Execute(null);
                    break;
                case 3: // Ctrl+N: New file
                    System.Diagnostics.Debug.WriteLine("Hotkey 3 pressed: New file");
                    NewFileCommand.Execute(null);
                    break;
                case 4: // Ctrl+S: Save file
                    System.Diagnostics.Debug.WriteLine("Hotkey 4 pressed: Save file");
                    SaveFile();
                    break;
                case 5: // F5: Reload file
                    System.Diagnostics.Debug.WriteLine("Hotkey 5 pressed: Reload file");
                    ReloadFileCommand.Execute(null);
                    break;
                case 6: // Ctrl+A: Archive completed tasks
                    System.Diagnostics.Debug.WriteLine("Hotkey 6 pressed: Archive completed tasks");
                    ArchiveCompletedTasksCommand.Execute(null);
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"Unknown hotkey {e.HotkeyId} pressed");
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error handling hotkey press: {ex.Message}");
        }
    }

    /// <summary>
    /// Toggles the main window visibility (hide/show)
    /// </summary>
    private void ToggleMainWindowVisibility()
    {
        try
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                {
                    if (desktop.MainWindow.IsVisible)
                    {
                        desktop.MainWindow.Hide();
                        System.Diagnostics.Debug.WriteLine("Main window hidden via hotkey");
                    }
                    else
                    {
                        desktop.MainWindow.Show();
                        desktop.MainWindow.WindowState = WindowState.Normal;
                        desktop.MainWindow.Activate();
                        System.Diagnostics.Debug.WriteLine("Main window shown via hotkey");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error toggling main window visibility: {ex.Message}");
        }
    }

    /// <summary>
    /// Disposes of hotkey resources
    /// </summary>
    private void DisposeHotkeys()
    {
        try
        {
            var hotkeyService = ServiceLocator.HotkeyService;
            hotkeyService.HotkeyPressed -= OnHotkeyPressed;
            hotkeyService.UnregisterAllHotkeys();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error disposing hotkeys: {ex.Message}");
        }
    }

    #endregion

    #region Application Lifecycle

    /// <summary>
    /// Exits the application
    /// </summary>
    [RelayCommand]
    public void Exit()
    {
        // Get the application lifetime and request shutdown
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }


    /// <summary>
    /// Toggles the main window visibility (hide/show)
    /// </summary>
    [RelayCommand]
    public void ToggleWindowVisibility()
    {
        try
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                {
                    if (desktop.MainWindow.IsVisible)
                    {
                        desktop.MainWindow.Hide();
                        System.Diagnostics.Debug.WriteLine("Main window hidden via command");
                    }
                    else
                    {
                        desktop.MainWindow.Show();
                        desktop.MainWindow.WindowState = WindowState.Normal;
                        desktop.MainWindow.Activate();
                        System.Diagnostics.Debug.WriteLine("Main window shown via command");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error toggling main window visibility: {ex.Message}");
        }
    }

    /// <summary>
    /// Focuses the search text box
    /// </summary>
    [RelayCommand]
    public void FocusSearch()
    {
        try
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                {
                    // Find the search text box and focus it
                    var searchTextBox = desktop.MainWindow.FindControl<TextBox>("SearchTextBox");
                    if (searchTextBox != null)
                    {
                        searchTextBox.Focus();
                        System.Diagnostics.Debug.WriteLine("Search text box focused");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error focusing search: {ex.Message}");
        }
    }

    #endregion
}

