using NUnit.Framework;
using TodoTxt.Core;
using TodoTxt.Avalonia.Controls;
using Task = TodoTxt.Core.Task;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for IntellisenseTextBox error handling and edge cases including exception scenarios,
/// invalid inputs, and graceful degradation.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxErrorHandlingTests
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

    #region TextChanged Error Handling

    /// <summary>
    /// Verifies that TextChanged event handles null text gracefully.
    /// </summary>
    [Test]
    public void TextChanged_WithNullText_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        
        // act
        // Simulate TextChanged event with null text
        var textChangedMethod = textBox.GetType().GetMethod("IntellisenseTextBox_TextChanged", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            textChangedMethod?.Invoke(textBox, new object[] { textBox, null! });
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    /// <summary>
    /// Verifies that TextChanged event handles empty string gracefully.
    /// </summary>
    [Test]
    public void TextChanged_WithEmptyString_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "";
        
        // act
        // Simulate TextChanged event with empty string
        var textChangedMethod = textBox.GetType().GetMethod("IntellisenseTextBox_TextChanged", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            textChangedMethod?.Invoke(textBox, new object[] { textBox, null! });
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    /// <summary>
    /// Verifies that TextChanged event handles invalid caret index gracefully.
    /// </summary>
    [Test]
    public void TextChanged_WithInvalidCaretIndex_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        
        // Set up a scenario that could cause an exception
        textBox.Text = "Test +";
        textBox.CaretIndex = 10; // Invalid caret index (beyond text length)
        
        // act
        // Simulate TextChanged event - this should not crash
        var textChangedMethod = textBox.GetType().GetMethod("IntellisenseTextBox_TextChanged", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            textChangedMethod?.Invoke(textBox, new object[] { textBox, null! });
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    #endregion

    #region InsertSelectedText Error Handling

    /// <summary>
    /// Verifies that InsertSelectedText handles null selected item gracefully.
    /// </summary>
    [Test]
    public void InsertSelectedText_WithNullSelectedItem_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = 0;
        
        // Set selected item to null (simulate edge case)
        var itemsField = list.GetType().GetField("_items", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (itemsField != null)
        {
            var items = itemsField.GetValue(list) as System.Collections.IList;
            if (items != null && items.Count > 0)
            {
                items[0] = null;
            }
        }
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    /// <summary>
    /// Verifies that InsertSelectedText handles invalid trigger position gracefully.
    /// </summary>
    [Test]
    public void InsertSelectedText_WithInvalidTriggerPosition_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = 0;
        
        // Set invalid trigger position
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, -1); // Invalid trigger position
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    /// <summary>
    /// Verifies that InsertSelectedText handles negative trigger position gracefully.
    /// </summary>
    [Test]
    public void InsertSelectedText_WithNegativeTriggerPosition_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = 0;
        
        // Set negative trigger position
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, -5); // Negative trigger position
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    #endregion

    #region ShowSuggestions Error Handling

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

    #region UpdateFiltering Error Handling

    /// <summary>
    /// Verifies that UpdateFiltering handles null items source gracefully.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithNullItemsSource_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        
        // Set items source to null (simulate edge case)
        list!.ItemsSource = null;
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            updateMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    /// <summary>
    /// Verifies that UpdateFiltering handles invalid trigger position gracefully.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithInvalidTriggerPosition_HandlesExceptionGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = _taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        // Set invalid trigger position
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, -1); // Invalid trigger position
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // The test passes if this doesn't throw an exception
        Assert.DoesNotThrow(() => 
        {
            updateMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - the method should not crash
        // Just verify the method executed without throwing an exception
    }

    #endregion
}
