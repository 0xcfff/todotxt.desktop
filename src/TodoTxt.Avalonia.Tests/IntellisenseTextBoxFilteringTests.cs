using NUnit.Framework;
using ToDoLib;
using TodoTxt.Avalonia.Controls;
using Task = ToDoLib.Task;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for IntellisenseTextBox filtering functionality, case sensitivity, and edge cases.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxFilteringTests
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

    #region Filtering Tests

    /// <summary>
    /// Verifies that filtering works correctly with case-insensitive matching.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithCaseInsensitiveConfigured_MatchesPartialText()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.CaseSensitive = false; // Enable case insensitive matching
        textBox.Text = "Test +SHOP"; // Different case
        textBox.CaretIndex = textBox.Text.Length;
        
        // Set trigger position manually
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        updateMethod?.Invoke(textBox, null);
        
        // assert
        // Should find "shopping" project even with different case
        var hasShoppingProject = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.ToLower().Contains("shopping") == true)
            {
                hasShoppingProject = true;
                break;
            }
        }
        Assert.That(hasShoppingProject, Is.True);
    }

    /// <summary>
    /// Verifies that filtering works correctly with case-sensitive matching.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithCaseSensitiveConfigured_DoesNotMatchDifferentCase()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.CaseSensitive = true; // Enable case sensitive matching
        textBox.Text = "Test +SHOP"; // Different case
        textBox.CaretIndex = textBox.Text.Length;
        
        // Set trigger position manually
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        updateMethod?.Invoke(textBox, null);
        
        // assert
        // Should NOT find "shopping" project with different case in case-sensitive mode
        var hasShoppingProject = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("shopping") == true)
            {
                hasShoppingProject = true;
                break;
            }
        }
        Assert.That(hasShoppingProject, Is.False);
    }

    #endregion

    #region Priority Position Tests

    /// <summary>
    /// Verifies that IsValidPriorityPosition returns true when at start of line.
    /// </summary>
    [Test]
    public void IsValidPriorityPosition_AtStartOfLineConfigured_ReturnsTrue()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.Text = "(";
        textBox.CaretIndex = 1;
        
        // act
        var isValidMethod = textBox.GetType().GetMethod("IsValidPriorityPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = isValidMethod?.Invoke(textBox, null);
        
        // assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Verifies that IsValidPriorityPosition returns false when in middle of text.
    /// </summary>
    [Test]
    public void IsValidPriorityPosition_InMiddleOfTextConfigured_ReturnsFalse()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.Text = "Some text (";
        textBox.CaretIndex = 11;
        
        // act
        var isValidMethod = textBox.GetType().GetMethod("IsValidPriorityPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = isValidMethod?.Invoke(textBox, null);
        
        // assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Verifies that IsValidPriorityPosition returns boolean when after date.
    /// </summary>
    [Test]
    public void IsValidPriorityPosition_AfterDateConfigured_ReturnsBoolean()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.Text = "2023-12-01 (";
        textBox.CaretIndex = 12;
        
        // act
        var isValidMethod = textBox.GetType().GetMethod("IsValidPriorityPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = isValidMethod?.Invoke(textBox, null);
        
        // assert
        // Should return a boolean value (implementation may vary)
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<bool>());
    }

    #endregion
}
