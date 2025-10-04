using NUnit.Framework;
using TodoTxt.Lib;
using TodoTxt.Avalonia.Controls;
using Task = TodoTxt.Lib.Task;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for IntellisenseTextBox ShowSuggestions functionality and dropdown behavior.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxSuggestionsTests
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

    #region ShowSuggestions Tests

    /// <summary>
    /// Verifies that ShowSuggestions with '+' trigger shows project suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithPlusTriggerProvided_ShowsProjectSuggestions()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        textBox.ShowSuggestions('+');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Should contain project suggestions
        var hasProjectSuggestion = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("shopping") == true ||
                list.Items[i]?.ToString()?.Contains("work") == true ||
                list.Items[i]?.ToString()?.Contains("important") == true)
            {
                hasProjectSuggestion = true;
                break;
            }
        }
        Assert.That(hasProjectSuggestion, Is.True);
    }

    /// <summary>
    /// Verifies that ShowSuggestions with '@' trigger shows context suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithAtTriggerProvided_ShowsContextSuggestions()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test @";
        textBox.CaretIndex = 6;
        
        // act
        textBox.ShowSuggestions('@');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Should contain context suggestions
        var hasContextSuggestion = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("home") == true ||
                list.Items[i]?.ToString()?.Contains("phone") == true ||
                list.Items[i]?.ToString()?.Contains("office") == true)
            {
                hasContextSuggestion = true;
                break;
            }
        }
        Assert.That(hasContextSuggestion, Is.True);
    }

    /// <summary>
    /// Verifies that ShowSuggestions with '(' trigger shows priority suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithParenthesisTriggerProvided_ShowsPrioritySuggestions()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up text and cursor position for valid priority position
        textBox.Text = "(";
        textBox.CaretIndex = 1;
        
        // act
        // Call ShowSuggestions with '(' trigger
        textBox.ShowSuggestions('(');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Should contain priority suggestions
        var hasPrioritySuggestion = false;
        for (int i = 0; i < list!.Items.Count; i++)
        {
            if (list.Items[i]?.ToString()?.Contains("A)") == true ||
                list.Items[i]?.ToString()?.Contains("B)") == true)
            {
                hasPrioritySuggestion = true;
                break;
            }
        }
        Assert.That(hasPrioritySuggestion, Is.True);
    }

    /// <summary>
    /// Verifies that ShowSuggestions handles null TaskList gracefully.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithNullTaskList_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = null;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        // This should not throw an exception
        Assert.DoesNotThrow(() => textBox.ShowSuggestions('+'));
        
        // assert
        // Should handle gracefully - dropdown should be closed
        var popup = textBox.DropDownPopup;
        Assert.That(popup?.IsOpen, Is.False);
    }

    /// <summary>
    /// Verifies that ShowSuggestions handles empty TaskList gracefully.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithEmptyTaskList_HandlesExceptionGracefully()
    {
        // arrange
        var emptyTaskList = new TaskList();
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = emptyTaskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        // This should not throw an exception
        Assert.DoesNotThrow(() => textBox.ShowSuggestions('+'));
        
        // assert
        // Should handle gracefully - dropdown should be closed
        var popup = textBox.DropDownPopup;
        Assert.That(popup?.IsOpen, Is.False);
    }

    #endregion

    #region Dropdown Control Tests

    /// <summary>
    /// Verifies that ShowDropDown opens the dropdown and selects the first item.
    /// </summary>
    [Test]
    public void ShowDropDown_WithPopulatedListConfigured_SelectsFirstItem()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // First show suggestions to populate the list
        textBox.ShowSuggestions('+');
        
        // act
        textBox.ShowDropDown();
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        Assert.That(list?.SelectedIndex, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that ShowDropDown makes the dropdown visible regardless of previous state.
    /// </summary>
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void ShowDropDown_WithInitialVisibilityStateProvided_MakesDropdownVisibleIrrespectiveOfPreviousState(bool initialVisibility)
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set initial visibility state
        var popup = textBox.DropDownPopup;
        if (popup != null)
        {
            popup.IsOpen = initialVisibility;
        }
        
        // act
        textBox.ShowDropDown();
        
        // assert
        Assert.That(popup?.IsOpen, Is.True);
    }

    /// <summary>
    /// Verifies that HideDropDown closes the dropdown.
    /// </summary>
    [Test]
    public void HideDropDown_WhenCalled_ClosesDropdown()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.ShowDropDown(); // Open dropdown first
        
        // act
        textBox.HideDropDown();
        
        // assert
        var popup = textBox.DropDownPopup;
        Assert.That(popup?.IsOpen, Is.False);
    }

    #endregion

    #region Text Content Tests

    /// <summary>
    /// Verifies that text content is properly set when project trigger is used.
    /// </summary>
    [Test]
    public void Text_WithProjectTriggerProvided_ContainsPlusCharacter()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // act
        textBox.Text = "Test +";
        
        // assert
        Assert.That(textBox.Text, Does.Contain("+"));
    }

    /// <summary>
    /// Verifies that text content is properly set when context trigger is used.
    /// </summary>
    [Test]
    public void Text_WithContextTriggerProvided_ContainsAtCharacter()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // act
        textBox.Text = "Test @";
        
        // assert
        Assert.That(textBox.Text, Does.Contain("@"));
    }

    /// <summary>
    /// Verifies that text content is properly set when priority trigger is used.
    /// </summary>
    [Test]
    public void Text_WithPriorityTriggerProvided_ContainsParenthesisCharacter()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // act
        textBox.Text = "Test (";
        
        // assert
        Assert.That(textBox.Text, Does.Contain("("));
    }

    #endregion
}
