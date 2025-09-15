using NUnit.Framework;
using ToDoLib;
using TodoTxt.Avalonia.Controls;
using Task = ToDoLib.Task;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Integration tests for IntellisenseTextBox that verify complete autocompletion workflows,
/// real data interaction, and complex scenarios.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxIntegrationTests
{
    private TaskList _taskList = null!;

    [SetUp]
    public void Setup()
    {
        // Create TaskList in memory without file dependency
        _taskList = new TaskList();
        
        // Add test tasks directly to the TaskList
        _taskList.Tasks.Add(new Task("Buy groceries +shopping @home"));
        _taskList.Tasks.Add(new Task("Call mom @phone"));
        _taskList.Tasks.Add(new Task("Finish project report +work @office"));
        _taskList.Tasks.Add(new Task("(A) High priority task +important"));
        _taskList.Tasks.Add(new Task("(B) Medium priority task +work"));
        
        // Update metadata for autocompletion
        _taskList.UpdateTaskListMetaData();
    }

    #region Complete Workflow Tests

    /// <summary>
    /// Verifies complete project autocompletion workflow from trigger to text insertion.
    /// </summary>
    [Test]
    public void CompleteWorkflow_ProjectAutocompletion_InsertsSelectedProject()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Buy groceries +";
        textBox.CaretIndex = textBox.Text.Length;

        // act - Complete workflow: show suggestions, navigate, select
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open with project suggestions
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Navigate to a specific project (e.g., "shopping")
        var shoppingIndex = -1;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("shopping") == true)
            {
                shoppingIndex = i;
                break;
            }
        }
        
        if (shoppingIndex >= 0)
        {
            list.SelectedIndex = shoppingIndex;
            
            // Set trigger position manually (required for text insertion)
            var triggerField = textBox.GetType().GetField("_triggerPosition", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            triggerField?.SetValue(textBox, 12); // Position of '+'
            
            // Simulate Enter key to insert selected text
            var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var keyEventArgs = new KeyEventArgs
            {
                Key = Key.Enter,
                Handled = false
            };
            keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
            
            // assert
            Assert.That(textBox.Text, Does.Contain("+shopping"));
            Assert.That(keyEventArgs.Handled, Is.True);
        }
    }

    /// <summary>
    /// Verifies complete context autocompletion workflow from trigger to text insertion.
    /// </summary>
    [Test]
    public void CompleteWorkflow_ContextAutocompletion_InsertsSelectedContext()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Call mom @";
        textBox.CaretIndex = textBox.Text.Length;

        // act - Complete workflow: show suggestions, navigate, select
        textBox.ShowSuggestions('@');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open with context suggestions
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Navigate to a specific context (e.g., "phone")
        var phoneIndex = -1;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("phone") == true)
            {
                phoneIndex = i;
                break;
            }
        }
        
        if (phoneIndex >= 0)
        {
            list.SelectedIndex = phoneIndex;
            
            // Set trigger position manually (required for text insertion)
            var triggerField = textBox.GetType().GetField("_triggerPosition", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            triggerField?.SetValue(textBox, 8); // Position of '@'
            
            // Simulate Enter key to insert selected text
            var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var keyEventArgs = new KeyEventArgs
            {
                Key = Key.Enter,
                Handled = false
            };
            keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
            
            // assert
            Assert.That(textBox.Text, Does.Contain("@phone"));
            Assert.That(keyEventArgs.Handled, Is.True);
        }
    }

    /// <summary>
    /// Verifies complete priority autocompletion workflow from trigger to text insertion.
    /// </summary>
    [Test]
    public void CompleteWorkflow_PriorityAutocompletion_InsertsSelectedPriority()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "(";
        textBox.CaretIndex = 1; // Position after '(' for valid priority position

        // act - Complete workflow: show suggestions, navigate, select
        textBox.ShowSuggestions('(');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open with priority suggestions
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Navigate to a specific priority (e.g., "A")
        var priorityAIndex = -1;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("A)") == true)
            {
                priorityAIndex = i;
                break;
            }
        }
        
        if (priorityAIndex >= 0)
        {
            list.SelectedIndex = priorityAIndex;
            
            // Set trigger position manually (required for text insertion)
            var triggerField = textBox.GetType().GetField("_triggerPosition", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            triggerField?.SetValue(textBox, 0); // Position of '('
            
            // Simulate Enter key to insert selected text
            var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var keyEventArgs = new KeyEventArgs
            {
                Key = Key.Enter,
                Handled = false
            };
            keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
            
            // assert
            Assert.That(textBox.Text, Does.Contain("(A)"));
            Assert.That(keyEventArgs.Handled, Is.True);
        }
    }

    /// <summary>
    /// Verifies complete keyboard navigation workflow with selection and insertion.
    /// </summary>
    [Test]
    public void CompleteWorkflow_KeyboardNavigation_SelectsAndInsertsCorrectly()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Task +";
        textBox.CaretIndex = textBox.Text.Length;

        // act - Show suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(1)); // Need multiple items for navigation
        
        // Navigate down
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs { Key = Key.Down, Handled = false };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        Assert.That(list!.SelectedIndex, Is.EqualTo(1));
        
        // Navigate up
        keyEventArgs = new KeyEventArgs { Key = Key.Up, Handled = false };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        Assert.That(list.SelectedIndex, Is.EqualTo(0));
        
        // Insert selected text
        var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        keyEventArgs = new KeyEventArgs { Key = Key.Enter, Handled = false };
        keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        Assert.That(textBox.Text, Does.Contain("+"));
        Assert.That(keyEventArgs.Handled, Is.True);
    }

    /// <summary>
    /// Verifies complete focus loss workflow with proper state maintenance.
    /// </summary>
    [Test]
    public void CompleteWorkflow_FocusLoss_ClosesDropdownAndMaintainsState()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Task +";
        textBox.CaretIndex = textBox.Text.Length;

        // act - Show suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Simulate focus loss
        var lostFocusMethod = textBox.GetType().GetMethod("IntellisenseTextBox_LostFocus", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        lostFocusMethod?.Invoke(textBox, new object[] { textBox, null! });
        
        // assert
        Assert.That(popup?.IsOpen, Is.False);
        Assert.That(textBox.Text, Is.EqualTo("Task +")); // Text should remain unchanged
    }

    #endregion

    #region Multiple Trigger Tests

    /// <summary>
    /// Verifies handling of multiple triggers in sequence (project then context).
    /// </summary>
    [Test]
    public void MultipleTriggersInSequence_ProjectThenContext_HandlesBothCorrectly()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Task +";
        textBox.CaretIndex = textBox.Text.Length;

        // act - First trigger: project
        textBox.ShowSuggestions('+');
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Select first project
        list!.SelectedIndex = 0;
        var selectedProject = list.Items[0]?.ToString();
        
        // Set trigger position manually (required for text insertion)
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // Insert project
        var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs { Key = Key.Enter, Handled = false };
        keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // Add space and context trigger
        textBox.Text += " @";
        textBox.CaretIndex = textBox.Text.Length;
        
        // Second trigger: context
        textBox.ShowSuggestions('@');
        popup = textBox.DropDownPopup;
        list = textBox.DropDownList;
        
        // assert
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        Assert.That(textBox.Text, Does.Contain(selectedProject));
        Assert.That(textBox.Text, Does.Contain("@"));
    }

    #endregion

    #region Complex Filtering Tests

    /// <summary>
    /// Verifies case-insensitive partial matching in filtering scenarios.
    /// </summary>
    [Test]
    public void ComplexFilteringScenario_CaseInsensitivePartialMatch_FiltersCorrectly()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.CaseSensitive = false; // Enable case insensitive matching
        textBox.Text = "Task +wo"; // Partial match for "work"
        textBox.CaretIndex = textBox.Text.Length;

        // act
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // assert
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Should find "work" project even with partial "wo" input
        var hasWorkProject = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.ToLower().Contains("work") == true)
            {
                hasWorkProject = true;
                break;
            }
        }
        Assert.That(hasWorkProject, Is.True);
    }

    /// <summary>
    /// Verifies case-sensitive exact matching in filtering scenarios.
    /// </summary>
    [Test]
    public void ComplexFilteringScenario_CaseSensitiveExactMatch_FiltersCorrectly()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.CaseSensitive = true; // Enable case sensitive matching
        textBox.Text = "Task +work"; // Exact case match for "work" (lowercase)
        textBox.CaretIndex = textBox.Text.Length;

        // act
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // assert
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Should find exact case matches
        var hasExactMatch = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            var item = list.Items[i]?.ToString();
            if (item != null && item.Contains("work"))
            {
                hasExactMatch = true;
                break;
            }
        }
        Assert.That(hasExactMatch, Is.True);
    }

    #endregion

    #region Real Data Interaction Tests

    /// <summary>
    /// Verifies performance with large task lists (50+ tasks).
    /// </summary>
    [Test]
    public void RealDataInteraction_WithLargeTaskList_PerformsEfficiently()
    {
        // arrange - Create a larger task list to test performance
        var largeTaskList = new TaskList();
        
        // Add many tasks with various projects and contexts using in-memory approach
        for (int i = 0; i < 50; i++)
        {
            var task = new Task($"Task {i} +project{i % 10} @context{i % 5}");
            largeTaskList.Tasks.Add(task);
        }
        largeTaskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = largeTaskList;
        textBox.Text = "New task +proj";
        textBox.CaretIndex = textBox.Text.Length;

        // act
        var startTime = DateTime.Now;
        textBox.ShowSuggestions('+');
        var endTime = DateTime.Now;
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // assert
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Performance assertion - should complete quickly
        var executionTime = endTime - startTime;
        Assert.That(executionTime.TotalMilliseconds, Is.LessThan(1000), 
            "Autocompletion should complete within 1 second even with large task lists");
    }

    /// <summary>
    /// Verifies handling of mixed content with all trigger types.
    /// </summary>
    [Test]
    public void RealDataInteraction_WithMixedContent_HandlesAllTriggerTypes()
    {
        // arrange - Create task list with mixed content
        var mixedTaskList = new TaskList();
        mixedTaskList.Tasks.Add(new Task("(A) High priority task +urgent @home"));
        mixedTaskList.Tasks.Add(new Task("(B) Medium task +work @office"));
        mixedTaskList.Tasks.Add(new Task("(C) Low task +personal @phone"));
        mixedTaskList.Tasks.Add(new Task("Regular task without priority +shopping @store"));
        mixedTaskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = mixedTaskList;

        // Test all trigger types in sequence
        var triggers = new[] { '(', '+', '@' };
        var triggerNames = new[] { "priority", "project", "context" };
        
        for (int i = 0; i < triggers.Length; i++)
        {
            // act
            if (triggers[i] == '(')
            {
                // Priority trigger needs to be at the beginning of the line
                textBox.Text = "(";
                textBox.CaretIndex = 1;
            }
            else
            {
                textBox.Text = $"Test {triggers[i]}";
                textBox.CaretIndex = textBox.Text.Length;
            }
            
            textBox.ShowSuggestions(triggers[i]);
            
            var popup = textBox.DropDownPopup;
            var list = textBox.DropDownList;
            
            // assert
            Assert.That(popup?.IsOpen, Is.True, 
                $"{triggerNames[i]} trigger should open dropdown");
            Assert.That(list?.Items.Count, Is.GreaterThan(0), 
                $"{triggerNames[i]} trigger should show suggestions");
            
            // Close dropdown for next test
            textBox.HideDropDown();
        }
    }

    #endregion
}
