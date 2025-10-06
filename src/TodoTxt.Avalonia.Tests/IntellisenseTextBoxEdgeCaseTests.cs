using NUnit.Framework;
using TodoTxt.Core.Tasks;
using TodoTxt.Avalonia.Controls;
using Task = TodoTxt.Core.Tasks.Task;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for IntellisenseTextBox edge cases including empty TaskList scenarios, null TaskList scenarios,
/// empty suggestions, invalid trigger positions, and text replacement edge cases.
/// </summary>
[TestFixture]
public class IntellisenseTextBoxEdgeCaseTests
{
    #region Empty TaskList Scenarios

    /// <summary>
    /// Verifies that ShowSuggestions with empty TaskList handles gracefully and shows no suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithEmptyTaskList_ShowsNoSuggestions()
    {
        // arrange
        var emptyTaskList = new TaskList();
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = emptyTaskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        textBox.ShowSuggestions('+');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Should handle gracefully - dropdown should be closed or empty
        Assert.That(popup?.IsOpen, Is.False);
        Assert.That(list?.Items.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that UpdateFiltering with empty TaskList handles gracefully.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithEmptyTaskList_HandlesGracefully()
    {
        // arrange
        var emptyTaskList = new TaskList();
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = emptyTaskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with empty suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            updateMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - no items to filter
        Assert.That(list?.Items.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that keyboard navigation with empty TaskList handles gracefully.
    /// </summary>
    [Test]
    public void KeyboardNavigation_WithEmptyTaskList_HandlesGracefully()
    {
        // arrange
        var emptyTaskList = new TaskList();
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = emptyTaskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with empty suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        
        // act
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs { Key = Key.Down, Handled = false };
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        });
        
        // assert
        // Should handle gracefully - no items to navigate
        Assert.That(list?.Items.Count, Is.EqualTo(0));
        Assert.That(keyEventArgs.Handled, Is.False); // Should not handle key when no items
    }

    #endregion

    #region Null TaskList Scenarios

    /// <summary>
    /// Verifies that ShowSuggestions with null TaskList handles gracefully.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithNullTaskList_HandlesGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = null;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        // Should not throw an exception
        Assert.DoesNotThrow(() => textBox.ShowSuggestions('+'));
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Should handle gracefully - dropdown should be closed
        Assert.That(popup?.IsOpen, Is.False);
        Assert.That(list?.Items.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that UpdateFiltering with null TaskList handles gracefully.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithNullTaskList_HandlesGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = null;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            updateMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - no TaskList to filter from
    }

    /// <summary>
    /// Verifies that keyboard navigation with null TaskList handles gracefully.
    /// </summary>
    [Test]
    public void KeyboardNavigation_WithNullTaskList_HandlesGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = null;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs { Key = Key.Down, Handled = false };
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        });
        
        // assert
        // Should handle gracefully - no TaskList to navigate
        Assert.That(keyEventArgs.Handled, Is.False); // Should not handle key when no TaskList
    }

    /// <summary>
    /// Verifies that text insertion with null TaskList handles gracefully.
    /// </summary>
    [Test]
    public void TextInsertion_WithNullTaskList_HandlesGracefully()
    {
        // arrange
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = null;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - text should remain unchanged
        Assert.That(textBox.Text, Is.EqualTo("Test +"));
    }

    #endregion

    #region Empty Suggestions Scenarios

    /// <summary>
    /// Verifies that ShowSuggestions with TaskList containing no projects shows no suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithTaskListContainingNoProjects_ShowsNoProjectSuggestions()
    {
        // arrange
        var taskListWithoutProjects = new TaskList();
        taskListWithoutProjects.Tasks.Add(new Task("Task without projects @context"));
        taskListWithoutProjects.Tasks.Add(new Task("Another task @another"));
        taskListWithoutProjects.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskListWithoutProjects;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // act
        textBox.ShowSuggestions('+');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Should handle gracefully - no project suggestions available
        Assert.That(popup?.IsOpen, Is.False);
        Assert.That(list?.Items.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that ShowSuggestions with TaskList containing no contexts shows no suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithTaskListContainingNoContexts_ShowsNoContextSuggestions()
    {
        // arrange
        var taskListWithoutContexts = new TaskList();
        taskListWithoutContexts.Tasks.Add(new Task("Task without contexts +project"));
        taskListWithoutContexts.Tasks.Add(new Task("Another task +another"));
        taskListWithoutContexts.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskListWithoutContexts;
        textBox.Text = "Test @";
        textBox.CaretIndex = 6;
        
        // act
        textBox.ShowSuggestions('@');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Should handle gracefully - no context suggestions available
        Assert.That(popup?.IsOpen, Is.False);
        Assert.That(list?.Items.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Verifies that ShowSuggestions with TaskList containing no priorities shows no suggestions.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithTaskListContainingNoPriorities_ShowsNoPrioritySuggestions()
    {
        // arrange
        var taskListWithoutPriorities = new TaskList();
        taskListWithoutPriorities.Tasks.Add(new Task("Task without priority +project @context"));
        taskListWithoutPriorities.Tasks.Add(new Task("Another task +another @another"));
        taskListWithoutPriorities.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskListWithoutPriorities;
        textBox.Text = "(";
        textBox.CaretIndex = 1;
        
        // act
        textBox.ShowSuggestions('(');
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Should handle gracefully - may show default priority suggestions or be closed
        // The exact behavior depends on implementation - check if it's either closed or has default suggestions
        if (popup?.IsOpen == true)
        {
            // If open, it should show default priority suggestions (A, B, C, etc.)
            Assert.That(list?.Items.Count, Is.GreaterThan(0));
        }
        else
        {
            // If closed, that's also acceptable behavior
            Assert.That(list?.Items.Count, Is.EqualTo(0));
        }
    }

    #endregion

    #region Invalid Trigger Positions

    /// <summary>
    /// Verifies that ShowSuggestions with invalid trigger position handles gracefully.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithInvalidTriggerPosition_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 3; // Invalid position - not at the end where trigger should be
        
        // act
        // Should not throw an exception
        Assert.DoesNotThrow(() => textBox.ShowSuggestions('+'));
        
        // assert
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // Should handle gracefully - may not show suggestions due to invalid position
        // The exact behavior depends on implementation, but should not crash
    }

    /// <summary>
    /// Verifies that UpdateFiltering with invalid trigger position handles gracefully.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithInvalidTriggerPosition_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
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
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            updateMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - invalid trigger position should not crash
    }

    /// <summary>
    /// Verifies that InsertSelectedText with invalid trigger position handles gracefully.
    /// </summary>
    [Test]
    public void InsertSelectedText_WithInvalidTriggerPosition_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
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
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - invalid trigger position should not crash
        // Text may remain unchanged or be handled gracefully
    }

    #endregion

    #region Text Replacement Edge Cases

    /// <summary>
    /// Verifies that text replacement with empty selected item handles gracefully.
    /// </summary>
    [Test]
    public void TextReplacement_WithEmptySelectedItem_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = 0;
        
        // Set selected item to empty string (simulate edge case)
        var itemsField = list.GetType().GetField("_items", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (itemsField != null)
        {
            var items = itemsField.GetValue(list) as System.Collections.IList;
            if (items != null && items.Count > 0)
            {
                items[0] = ""; // Empty string
            }
        }
        
        // Set trigger position manually
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - empty selected item should not crash
        // Text may remain unchanged or be handled gracefully
    }

    /// <summary>
    /// Verifies that text replacement with trigger position beyond text length handles gracefully.
    /// </summary>
    [Test]
    public void TextReplacement_WithTriggerPositionBeyondTextLength_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        list!.SelectedIndex = 0;
        
        // Set trigger position beyond text length
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 100); // Beyond text length
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - trigger position beyond text length should not crash
        // Text may remain unchanged or be handled gracefully
    }

    /// <summary>
    /// Verifies that text replacement with negative trigger position handles gracefully.
    /// </summary>
    [Test]
    public void TextReplacement_WithNegativeTriggerPosition_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
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
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - negative trigger position should not crash
        // Text may remain unchanged or be handled gracefully
    }

    /// <summary>
    /// Verifies that text replacement with null selected item handles gracefully.
    /// </summary>
    [Test]
    public void TextReplacement_WithNullSelectedItem_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
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
                items[0] = null; // Null item
            }
        }
        
        // Set trigger position manually
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        triggerField?.SetValue(textBox, 5); // Position of '+'
        
        // act
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            insertMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - null selected item should not crash
        // Text may remain unchanged or be handled gracefully
    }

    #endregion

    #region Boundary Conditions

    /// <summary>
    /// Verifies that ShowSuggestions with very long text handles gracefully.
    /// </summary>
    [Test]
    public void ShowSuggestions_WithVeryLongText_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        
        // Create very long text
        var longText = new string('A', 10000) + " +";
        textBox.Text = longText;
        textBox.CaretIndex = longText.Length;
        
        // act
        // Should not throw an exception
        Assert.DoesNotThrow(() => textBox.ShowSuggestions('+'));
        
        // assert
        // Should handle gracefully - very long text should not crash
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        // The exact behavior depends on implementation, but should not crash
    }

    /// <summary>
    /// Verifies that UpdateFiltering with very long filter text handles gracefully.
    /// </summary>
    [Test]
    public void UpdateFiltering_WithVeryLongFilterText_HandlesGracefully()
    {
        // arrange
        var taskList = new TaskList();
        taskList.Tasks.Add(new Task("Task +project @context"));
        taskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = taskList;
        
        // Create very long filter text
        var longFilterText = new string('A', 10000) + " +";
        textBox.Text = longFilterText;
        textBox.CaretIndex = longFilterText.Length;
        
        // Set up dropdown with suggestions
        textBox.ShowSuggestions('+');
        
        // act
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            updateMethod?.Invoke(textBox, null);
        });
        
        // assert
        // Should handle gracefully - very long filter text should not crash
    }

    /// <summary>
    /// Verifies that keyboard navigation with single item list handles gracefully.
    /// </summary>
    [Test]
    public void KeyboardNavigation_WithSingleItemList_HandlesGracefully()
    {
        // arrange
        var singleItemTaskList = new TaskList();
        singleItemTaskList.Tasks.Add(new Task("Single task +project"));
        singleItemTaskList.UpdateTaskListMetaData();
        
        var textBox = new IntellisenseTextBox();
        textBox.TaskList = singleItemTaskList;
        textBox.Text = "Test +";
        textBox.CaretIndex = 6;
        
        // Set up dropdown with single suggestion
        textBox.ShowSuggestions('+');
        
        var list = textBox.DropDownList;
        Assert.That(list?.Items.Count, Is.EqualTo(1));
        
        // act
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var keyEventArgs = new KeyEventArgs { Key = Key.Down, Handled = false };
        
        // Should not throw an exception
        Assert.DoesNotThrow(() => 
        {
            keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        });
        
        // assert
        // Should handle gracefully - single item navigation should not crash
        // Selection should remain at 0 or wrap appropriately
        Assert.That(list?.SelectedIndex, Is.GreaterThanOrEqualTo(0));
        Assert.That(list?.SelectedIndex, Is.LessThan(list?.Items.Count ?? 0));
    }

    #endregion
}
