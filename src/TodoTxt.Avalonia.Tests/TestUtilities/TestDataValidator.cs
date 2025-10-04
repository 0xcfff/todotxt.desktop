using TodoTxt.Lib;
using Task = TodoTxt.Lib.Task;

namespace TodoTxt.Avalonia.Tests.TestUtilities;

/// <summary>
/// Utility class for validating test data and ensuring test consistency.
/// </summary>
public static class TestDataValidator
{
    /// <summary>
    /// Validates that a TaskList has the expected structure and content.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <param name="expectedTaskCount">The expected number of tasks.</param>
    /// <param name="expectedProjectCount">The expected number of projects.</param>
    /// <param name="expectedContextCount">The expected number of contexts.</param>
    /// <param name="expectedPriorityCount">The expected number of priorities.</param>
    /// <returns>True if validation passes.</returns>
    public static bool ValidateTaskListStructure(TaskList taskList, int expectedTaskCount, int expectedProjectCount, int expectedContextCount, int expectedPriorityCount)
    {
        if (taskList.Tasks.Count != expectedTaskCount)
            return false;
        
        if (taskList.Projects.Count != expectedProjectCount)
            return false;
        
        if (taskList.Contexts.Count != expectedContextCount)
            return false;
        
        if (taskList.Priorities.Count != expectedPriorityCount)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates that a TaskList contains specific projects.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <param name="expectedProjects">The expected projects.</param>
    /// <returns>True if all expected projects are found.</returns>
    public static bool ValidateContainsProjects(TaskList taskList, params string[] expectedProjects)
    {
        foreach (var project in expectedProjects)
        {
            var normalizedProject = project.StartsWith("+") ? project : "+" + project;
            if (!taskList.Projects.Contains(normalizedProject))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a TaskList contains specific contexts.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <param name="expectedContexts">The expected contexts.</param>
    /// <returns>True if all expected contexts are found.</returns>
    public static bool ValidateContainsContexts(TaskList taskList, params string[] expectedContexts)
    {
        foreach (var context in expectedContexts)
        {
            var normalizedContext = context.StartsWith("@") ? context : "@" + context;
            if (!taskList.Contexts.Contains(normalizedContext))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a TaskList contains specific priorities.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <param name="expectedPriorities">The expected priorities.</param>
    /// <returns>True if all expected priorities are found.</returns>
    public static bool ValidateContainsPriorities(TaskList taskList, params string[] expectedPriorities)
    {
        foreach (var priority in expectedPriorities)
        {
            var normalizedPriority = priority.StartsWith("(") ? priority : $"({priority})";
            if (!taskList.Priorities.Contains(normalizedPriority))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a task text contains expected elements.
    /// </summary>
    /// <param name="taskText">The task text to validate.</param>
    /// <param name="expectedProjects">The expected projects in the task.</param>
    /// <param name="expectedContexts">The expected contexts in the task.</param>
    /// <param name="expectedPriority">The expected priority in the task (optional).</param>
    /// <returns>True if all expected elements are found.</returns>
    public static bool ValidateTaskContent(string taskText, string[] expectedProjects, string[] expectedContexts, string? expectedPriority = null)
    {
        // Validate projects
        foreach (var project in expectedProjects)
        {
            var normalizedProject = project.StartsWith("+") ? project : "+" + project;
            if (!taskText.Contains(normalizedProject))
                return false;
        }
        
        // Validate contexts
        foreach (var context in expectedContexts)
        {
            var normalizedContext = context.StartsWith("@") ? context : "@" + context;
            if (!taskText.Contains(normalizedContext))
                return false;
        }
        
        // Validate priority if specified
        if (!string.IsNullOrEmpty(expectedPriority))
        {
            var normalizedPriority = expectedPriority.StartsWith("(") ? expectedPriority : $"({expectedPriority})";
            if (!taskText.Contains(normalizedPriority))
                return false;
        }
        
        return true;
    }

    /// <summary>
    /// Validates that a task has the correct priority format.
    /// </summary>
    /// <param name="taskText">The task text to validate.</param>
    /// <returns>True if the task has a valid priority format.</returns>
    public static bool ValidatePriorityFormat(string taskText)
    {
        // Priority should be at the beginning of the line or after a date
        var trimmedText = taskText.Trim();
        
        // Check if it starts with priority
        if (trimmedText.StartsWith("(") && trimmedText.Length >= 3 && trimmedText[2] == ')')
        {
            return true;
        }
        
        // Check if it has priority after date (YYYY-MM-DD format)
        if (trimmedText.Length >= 11 && trimmedText[4] == '-' && trimmedText[7] == '-' && trimmedText[10] == ' ')
        {
            var afterDate = trimmedText.Substring(11);
            if (afterDate.StartsWith("(") && afterDate.Length >= 3 && afterDate[2] == ')')
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// Validates that a task has the correct date format.
    /// </summary>
    /// <param name="taskText">The task text to validate.</param>
    /// <returns>True if the task has a valid date format.</returns>
    public static bool ValidateDateFormat(string taskText)
    {
        var trimmedText = taskText.Trim();
        
        // Check for YYYY-MM-DD format at the beginning
        if (trimmedText.Length >= 10)
        {
            var datePart = trimmedText.Substring(0, 10);
            if (datePart.Length == 10 && datePart[4] == '-' && datePart[7] == '-')
            {
                // Try to parse the date
                if (DateTime.TryParseExact(datePart, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    /// <summary>
    /// Validates that a task has the correct completion format.
    /// </summary>
    /// <param name="taskText">The task text to validate.</param>
    /// <returns>True if the task has a valid completion format.</returns>
    public static bool ValidateCompletionFormat(string taskText)
    {
        var trimmedText = taskText.Trim();
        
        // Completed tasks should start with "x YYYY-MM-DD"
        if (trimmedText.StartsWith("x ") && trimmedText.Length >= 12)
        {
            var datePart = trimmedText.Substring(2, 10);
            if (datePart.Length == 10 && datePart[4] == '-' && datePart[7] == '-')
            {
                // Try to parse the date
                if (DateTime.TryParseExact(datePart, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    /// <summary>
    /// Validates that a TaskList has consistent metadata.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if the metadata is consistent.</returns>
    public static bool ValidateMetadataConsistency(TaskList taskList)
    {
        // Check that all projects in metadata exist in tasks
        foreach (var project in taskList.Projects)
        {
            bool foundInTasks = false;
            foreach (var task in taskList.Tasks)
            {
                if (task.Raw.Contains(project))
                {
                    foundInTasks = true;
                    break;
                }
            }
            if (!foundInTasks)
                return false;
        }
        
        // Check that all contexts in metadata exist in tasks
        foreach (var context in taskList.Contexts)
        {
            bool foundInTasks = false;
            foreach (var task in taskList.Tasks)
            {
                if (task.Raw.Contains(context))
                {
                    foundInTasks = true;
                    break;
                }
            }
            if (!foundInTasks)
                return false;
        }
        
        // Check that all priorities in metadata exist in tasks
        foreach (var priority in taskList.Priorities)
        {
            bool foundInTasks = false;
            foreach (var task in taskList.Tasks)
            {
                if (task.Raw.Contains(priority))
                {
                    foundInTasks = true;
                    break;
                }
            }
            if (!foundInTasks)
                return false;
        }
        
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has no duplicate projects.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if there are no duplicate projects.</returns>
    public static bool ValidateNoDuplicateProjects(TaskList taskList)
    {
        var projectSet = new HashSet<string>();
        foreach (var project in taskList.Projects)
        {
            if (!projectSet.Add(project))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has no duplicate contexts.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if there are no duplicate contexts.</returns>
    public static bool ValidateNoDuplicateContexts(TaskList taskList)
    {
        var contextSet = new HashSet<string>();
        foreach (var context in taskList.Contexts)
        {
            if (!contextSet.Add(context))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has no duplicate priorities.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if there are no duplicate priorities.</returns>
    public static bool ValidateNoDuplicatePriorities(TaskList taskList)
    {
        var prioritySet = new HashSet<string>();
        foreach (var priority in taskList.Priorities)
        {
            if (!prioritySet.Add(priority))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has no duplicate tasks.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if there are no duplicate tasks.</returns>
    public static bool ValidateNoDuplicateTasks(TaskList taskList)
    {
        var taskSet = new HashSet<string>();
        foreach (var task in taskList.Tasks)
        {
            if (!taskSet.Add(task.Raw))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has proper task ordering (completed tasks at the end).
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if tasks are properly ordered.</returns>
    public static bool ValidateTaskOrdering(TaskList taskList)
    {
        bool foundCompletedTask = false;
        
        foreach (var task in taskList.Tasks)
        {
            bool isCompleted = task.Raw.Trim().StartsWith("x ");
            
            if (isCompleted)
            {
                foundCompletedTask = true;
            }
            else if (foundCompletedTask)
            {
                // Found an incomplete task after a completed task
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has proper whitespace handling based on PreserveWhiteSpace setting.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if whitespace handling is correct.</returns>
    public static bool ValidateWhitespaceHandling(TaskList taskList)
    {
        foreach (var task in taskList.Tasks)
        {
            var taskText = task.Raw;
            
            if (taskList.PreserveWhiteSpace)
            {
                // With preserve whitespace, we expect the original formatting
                // This is harder to validate without knowing the original input
                // For now, just check that the text is not empty
                if (string.IsNullOrEmpty(taskText))
                    return false;
            }
            else
            {
                // Without preserve whitespace, we expect normalized formatting
                // Check that there are no excessive whitespace characters
                if (taskText.Contains("  ") || taskText.Contains("\t\t"))
                    return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Validates that a TaskList has proper line ending handling.
    /// </summary>
    /// <param name="taskList">The TaskList to validate.</param>
    /// <returns>True if line ending handling is correct.</returns>
    public static bool ValidateLineEndingHandling(TaskList taskList)
    {
        // This is difficult to validate without knowing the original file content
        // For now, just check that tasks don't contain unexpected line endings
        foreach (var task in taskList.Tasks)
        {
            var taskText = task.Raw;
            
            // Check for unexpected line endings within task text
            if (taskText.Contains("\r\n") || taskText.Contains("\n") || taskText.Contains("\r"))
            {
                // This might be expected in some cases, so we'll be lenient
                // Just check that it's not excessive
                var lineEndingCount = taskText.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None).Length - 1;
                if (lineEndingCount > 1)
                    return false;
            }
        }
        
        return true;
    }
}
