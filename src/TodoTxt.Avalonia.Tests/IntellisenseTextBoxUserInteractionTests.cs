using NUnit.Framework;
using TodoTxt.Core;
using TodoTxt.Avalonia.Controls;
using Task = TodoTxt.Core.Task;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for IntellisenseTextBox user interaction functionality including keyboard navigation,
/// text insertion, and focus handling.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxUserInteractionTests
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

    #region Keyboard Navigation Tests

    /// <summary>
    /// Verifies that Down arrow key moves selection down in the dropdown list.
    /// </summary>
    [Test]
    public void KeyDown_WithDownArrowWhenDropdownOpen_MovesSelectionDown()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open and has items
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(1)); // Need at least 2 items to test navigation
        
        // Set initial selection to first item
        list!.SelectedIndex = 0;
        
        // act
        // Simulate Down arrow key press
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs
        {
            Key = Key.Down,
            Handled = false
        };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        // Selection should move down to second item
        Assert.That(list.SelectedIndex, Is.EqualTo(1));
        Assert.That(keyEventArgs.Handled, Is.True);
    }

    /// <summary>
    /// Verifies that Up arrow key moves selection up in the dropdown list.
    /// </summary>
    [Test]
    public void KeyDown_WithUpArrowWhenDropdownOpen_MovesSelectionUp()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open and has items
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(1)); // Need at least 2 items to test navigation
        
        // Set initial selection to second item
        list!.SelectedIndex = 1;
        
        // act
        // Simulate Up arrow key press
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs
        {
            Key = Key.Up,
            Handled = false
        };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        // Selection should move up to first item
        Assert.That(list.SelectedIndex, Is.EqualTo(0));
        Assert.That(keyEventArgs.Handled, Is.True);
    }

    /// <summary>
    /// Verifies that Down arrow key wraps to first item when at last item.
    /// </summary>
    [Test]
    public void KeyDown_WithDownArrowAtLastItemWhenDropdownOpen_WrapsToFirstItem()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open and has items
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(1)); // Need at least 2 items to test wrapping
        
        // Set initial selection to last item
        list!.SelectedIndex = list.Items.Count - 1;
        
        // act
        // Simulate Down arrow key press
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs
        {
            Key = Key.Down,
            Handled = false
        };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        // Selection should wrap to first item
        Assert.That(list.SelectedIndex, Is.EqualTo(0));
        Assert.That(keyEventArgs.Handled, Is.True);
    }

    /// <summary>
    /// Verifies that Up arrow key wraps to last item when at first item.
    /// </summary>
    [Test]
    public void KeyDown_WithUpArrowAtFirstItemWhenDropdownOpen_WrapsToLastItem()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open and has items
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(1)); // Need at least 2 items to test wrapping
        
        // Set initial selection to first item
        list!.SelectedIndex = 0;
        
        // act
        // Simulate Up arrow key press
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs
        {
            Key = Key.Up,
            Handled = false
        };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        // Selection should wrap to last item
        Assert.That(list.SelectedIndex, Is.EqualTo(list.Items.Count - 1));
        Assert.That(keyEventArgs.Handled, Is.True);
    }

    /// <summary>
    /// Verifies that Left and Right arrow keys allow cursor movement when dropdown is open.
    /// </summary>
    [Test]
    public void KeyDown_WithLeftRightArrowsWhenDropdownOpen_AllowsCursorMovement()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open
        Assert.That(popup?.IsOpen, Is.True);
        
        // act
        // Simulate Left arrow key press
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs
        {
            Key = Key.Left,
            Handled = false
        };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        // Left arrow should not be handled (allows cursor movement)
        Assert.That(keyEventArgs.Handled, Is.False);
        
        // Simulate Right arrow key press
        keyEventArgs = new KeyEventArgs
        {
            Key = Key.Right,
            Handled = false
        };
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        // Right arrow should not be handled (allows cursor movement)
        Assert.That(keyEventArgs.Handled, Is.False);
    }

    #endregion

    #region Text Insertion Tests

    /// <summary>
    /// Verifies that Enter key inserts the selected text when dropdown is open.
    /// </summary>
    [Test]
    public void KeyUp_WithEnterKeyWhenDropdownOpen_InsertsSelectedText()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up text and trigger position
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set trigger position manually
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open and has items
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // Select first item
        list!.SelectedIndex = 0;
        var selectedItem = list.Items[0]?.ToString();
        
        // act
        // Simulate Enter key press
        var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs
        {
            Key = Key.Enter,
            Handled = false
        };
        keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        
        // assert
        Assert.That(textBox.Text, Does.Contain(selectedItem));
        Assert.That(keyEventArgs.Handled, Is.True);
    }

    /// <summary>
    /// Verifies that InsertSelectedText method works correctly with valid selection.
    /// </summary>
    [Test]
    public void InsertSelectedText_WithValidSelection_ReplacesTextFromTriggerPosition()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set trigger position manually
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = 0;
        var selectedItem = list.Items[0]?.ToString();
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        insertMethod?.Invoke(textBox, null);
        
        // assert
        Assert.That(textBox.Text, Does.Contain(selectedItem));
    }

    /// <summary>
    /// Verifies that InsertSelectedText handles no selection gracefully.
    /// </summary>
    [Test]
    public void InsertSelectedText_WithNoSelection_DoesNotModifyText()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        var originalText = textBox.Text;
        
        // Set up dropdown with suggestions but no selection
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = -1; // No selection
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        insertMethod?.Invoke(textBox, null);
        
        // assert
        Assert.That(textBox.Text, Is.EqualTo(originalText));
    }

    #endregion

    #region Focus Handling Tests

    /// <summary>
    /// Verifies that LostFocus event closes the dropdown.
    /// </summary>
    [Test]
    public void LostFocus_WhenCalled_HidesDropdown()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Verify dropdown is open
        Assert.That(popup?.IsOpen, Is.True);
        Assert.That(list?.Items.Count, Is.GreaterThan(0));
        
        // act
        // Simulate LostFocus event
        var lostFocusMethod = textBox.GetType().GetMethod("IntellisenseTextBox_LostFocus", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        lostFocusMethod?.Invoke(textBox, new object[] { textBox, null! });
        
        // assert
        Assert.That(popup?.IsOpen, Is.False);
    }

    #endregion
}
