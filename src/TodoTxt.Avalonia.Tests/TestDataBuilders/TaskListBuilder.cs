using TodoTxt.Core;
using Task = TodoTxt.Core.Task;

namespace TodoTxt.Avalonia.Tests.TestDataBuilders;

/// <summary>
/// Builder class for creating test TaskList instances with consistent setup and validation.
/// </summary>
public class TaskListBuilder
{
    private readonly TaskList _taskList;
    private bool _metadataUpdated;

    /// <summary>
    /// Initializes a new instance of the TaskListBuilder.
    /// </summary>
    public TaskListBuilder()
    {
        _taskList = new TaskList();
        _metadataUpdated = false;
    }

    /// <summary>
    /// Adds a task to the TaskList.
    /// </summary>
    /// <param name="taskText">The task text to add.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithTask(string taskText)
    {
        _taskList.Tasks.Add(new Task(taskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds multiple tasks to the TaskList.
    /// </summary>
    /// <param name="taskTexts">The task texts to add.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithTasks(params string[] taskTexts)
    {
        foreach (var taskText in taskTexts)
        {
            _taskList.Tasks.Add(new Task(taskText));
        }
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds a task with specific projects.
    /// </summary>
    /// <param name="taskText">The base task text.</param>
    /// <param name="projects">The projects to add to the task.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithTaskWithProjects(string taskText, params string[] projects)
    {
        var projectString = string.Join(" ", projects.Select(p => p.StartsWith("+") ? p : "+" + p));
        var fullTaskText = $"{taskText} {projectString}";
        _taskList.Tasks.Add(new Task(fullTaskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds a task with specific contexts.
    /// </summary>
    /// <param name="taskText">The base task text.</param>
    /// <param name="contexts">The contexts to add to the task.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithTaskWithContexts(string taskText, params string[] contexts)
    {
        var contextString = string.Join(" ", contexts.Select(c => c.StartsWith("@") ? c : "@" + c));
        var fullTaskText = $"{taskText} {contextString}";
        _taskList.Tasks.Add(new Task(fullTaskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds a task with specific priority.
    /// </summary>
    /// <param name="taskText">The base task text.</param>
    /// <param name="priority">The priority to add to the task (e.g., "A", "B", "C").</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithTaskWithPriority(string taskText, string priority)
    {
        var priorityString = priority.StartsWith("(") ? priority : $"({priority})";
        var fullTaskText = $"{priorityString} {taskText}";
        _taskList.Tasks.Add(new Task(fullTaskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds a task with priority, projects, and contexts.
    /// </summary>
    /// <param name="taskText">The base task text.</param>
    /// <param name="priority">The priority to add to the task.</param>
    /// <param name="projects">The projects to add to the task.</param>
    /// <param name="contexts">The contexts to add to the task.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithCompleteTask(string taskText, string priority, string[] projects, string[] contexts)
    {
        var priorityString = priority.StartsWith("(") ? priority : $"({priority})";
        var projectString = string.Join(" ", projects.Select(p => p.StartsWith("+") ? p : "+" + p));
        var contextString = string.Join(" ", contexts.Select(c => c.StartsWith("@") ? c : "@" + c));
        var fullTaskText = $"{priorityString} {taskText} {projectString} {contextString}";
        _taskList.Tasks.Add(new Task(fullTaskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds a task with a due date.
    /// </summary>
    /// <param name="taskText">The base task text.</param>
    /// <param name="dueDate">The due date in YYYY-MM-DD format.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithTaskWithDueDate(string taskText, string dueDate)
    {
        var fullTaskText = $"{dueDate} {taskText}";
        _taskList.Tasks.Add(new Task(fullTaskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Adds a task with completion date.
    /// </summary>
    /// <param name="taskText">The base task text.</param>
    /// <param name="completionDate">The completion date in YYYY-MM-DD format.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithCompletedTask(string taskText, string completionDate)
    {
        var fullTaskText = $"x {completionDate} {taskText}";
        _taskList.Tasks.Add(new Task(fullTaskText));
        _metadataUpdated = false;
        return this;
    }

    /// <summary>
    /// Sets the preserve whitespace property.
    /// </summary>
    /// <param name="preserveWhitespace">Whether to preserve whitespace.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    public TaskListBuilder WithPreserveWhitespace(bool preserveWhitespace)
    {
        _taskList.PreserveWhiteSpace = preserveWhitespace;
        return this;
    }

    /// <summary>
    /// Builds the TaskList and updates metadata if needed.
    /// </summary>
    /// <returns>The configured TaskList instance.</returns>
    public TaskList Build()
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        return _taskList;
    }

    /// <summary>
    /// Builds the TaskList without updating metadata (for testing metadata update scenarios).
    /// </summary>
    /// <returns>The configured TaskList instance without updated metadata.</returns>
    public TaskList BuildWithoutMetadataUpdate()
    {
        return _taskList;
    }

    /// <summary>
    /// Validates that the TaskList has the expected number of tasks.
    /// </summary>
    /// <param name="expectedCount">The expected number of tasks.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the task count doesn't match.</exception>
    public TaskListBuilder ValidateTaskCount(int expectedCount)
    {
        if (_taskList.Tasks.Count != expectedCount)
        {
            throw new InvalidOperationException($"Expected {expectedCount} tasks, but found {_taskList.Tasks.Count}");
        }
        return this;
    }

    /// <summary>
    /// Validates that the TaskList has the expected number of projects.
    /// </summary>
    /// <param name="expectedCount">The expected number of projects.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the project count doesn't match.</exception>
    public TaskListBuilder ValidateProjectCount(int expectedCount)
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        
        if (_taskList.Projects.Count != expectedCount)
        {
            throw new InvalidOperationException($"Expected {expectedCount} projects, but found {_taskList.Projects.Count}");
        }
        return this;
    }

    /// <summary>
    /// Validates that the TaskList has the expected number of contexts.
    /// </summary>
    /// <param name="expectedCount">The expected number of contexts.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the context count doesn't match.</exception>
    public TaskListBuilder ValidateContextCount(int expectedCount)
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        
        if (_taskList.Contexts.Count != expectedCount)
        {
            throw new InvalidOperationException($"Expected {expectedCount} contexts, but found {_taskList.Contexts.Count}");
        }
        return this;
    }

    /// <summary>
    /// Validates that the TaskList has the expected number of priorities.
    /// </summary>
    /// <param name="expectedCount">The expected number of priorities.</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the priority count doesn't match.</exception>
    public TaskListBuilder ValidatePriorityCount(int expectedCount)
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        
        if (_taskList.Priorities.Count != expectedCount)
        {
            throw new InvalidOperationException($"Expected {expectedCount} priorities, but found {_taskList.Priorities.Count}");
        }
        return this;
    }

    /// <summary>
    /// Validates that the TaskList contains a specific project.
    /// </summary>
    /// <param name="project">The project to validate (with or without + prefix).</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the project is not found.</exception>
    public TaskListBuilder ValidateContainsProject(string project)
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        
        var normalizedProject = project.StartsWith("+") ? project : "+" + project;
        if (!_taskList.Projects.Contains(normalizedProject))
        {
            throw new InvalidOperationException($"Expected project '{normalizedProject}' not found in TaskList");
        }
        return this;
    }

    /// <summary>
    /// Validates that the TaskList contains a specific context.
    /// </summary>
    /// <param name="context">The context to validate (with or without @ prefix).</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the context is not found.</exception>
    public TaskListBuilder ValidateContainsContext(string context)
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        
        var normalizedContext = context.StartsWith("@") ? context : "@" + context;
        if (!_taskList.Contexts.Contains(normalizedContext))
        {
            throw new InvalidOperationException($"Expected context '{normalizedContext}' not found in TaskList");
        }
        return this;
    }

    /// <summary>
    /// Validates that the TaskList contains a specific priority.
    /// </summary>
    /// <param name="priority">The priority to validate (with or without parentheses).</param>
    /// <returns>The TaskListBuilder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the priority is not found.</exception>
    public TaskListBuilder ValidateContainsPriority(string priority)
    {
        if (!_metadataUpdated)
        {
            _taskList.UpdateTaskListMetaData();
            _metadataUpdated = true;
        }
        
        var normalizedPriority = priority.StartsWith("(") ? priority : $"({priority})";
        if (!_taskList.Priorities.Contains(normalizedPriority))
        {
            throw new InvalidOperationException($"Expected priority '{normalizedPriority}' not found in TaskList");
        }
        return this;
    }
}
