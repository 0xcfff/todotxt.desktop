using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using TodoTxt.Avalonia.Controls;

namespace TodoTxt.Avalonia.Tests.TestUtilities;

/// <summary>
/// Utility class providing common test operations for IntellisenseTextBox testing.
/// </summary>
public static class IntellisenseTextBoxTestUtilities
{
    /// <summary>
    /// Simulates a key down event on the IntellisenseTextBox.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="key">The key to simulate.</param>
    /// <returns>The KeyEventArgs with the Handled property set.</returns>
    public static KeyEventArgs SimulateKeyDown(IntellisenseTextBox textBox, Key key)
    {
        var keyDownMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyDown", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        var keyEventArgs = new KeyEventArgs
        {
            Key = key,
            Handled = false
        };
        
        keyDownMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        return keyEventArgs;
    }

    /// <summary>
    /// Simulates a key up event on the IntellisenseTextBox.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="key">The key to simulate.</param>
    /// <returns>The KeyEventArgs with the Handled property set.</returns>
    public static KeyEventArgs SimulateKeyUp(IntellisenseTextBox textBox, Key key)
    {
        var keyUpMethod = textBox.GetType().GetMethod("IntellisenseTextBox_KeyUp", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        var keyEventArgs = new KeyEventArgs
        {
            Key = key,
            Handled = false
        };
        
        keyUpMethod?.Invoke(textBox, new object[] { textBox, keyEventArgs });
        return keyEventArgs;
    }

    /// <summary>
    /// Simulates a lost focus event on the IntellisenseTextBox.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    public static void SimulateLostFocus(IntellisenseTextBox textBox)
    {
        var lostFocusMethod = textBox.GetType().GetMethod("IntellisenseTextBox_LostFocus", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        lostFocusMethod?.Invoke(textBox, new object[] { textBox, null! });
    }

    /// <summary>
    /// Simulates a text changed event on the IntellisenseTextBox.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    public static void SimulateTextChanged(IntellisenseTextBox textBox)
    {
        var textChangedMethod = textBox.GetType().GetMethod("IntellisenseTextBox_TextChanged", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        textChangedMethod?.Invoke(textBox, new object[] { textBox, null! });
    }

    /// <summary>
    /// Sets the trigger position using reflection.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="position">The trigger position to set.</param>
    public static void SetTriggerPosition(IntellisenseTextBox textBox, int position)
    {
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        triggerField?.SetValue(textBox, position);
    }

    /// <summary>
    /// Gets the trigger position using reflection.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <returns>The current trigger position.</returns>
    public static int GetTriggerPosition(IntellisenseTextBox textBox)
    {
        var triggerField = textBox.GetType().GetField("_triggerPosition", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        return (int)(triggerField?.GetValue(textBox) ?? -1);
    }

    /// <summary>
    /// Invokes the UpdateFiltering method using reflection.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    public static void InvokeUpdateFiltering(IntellisenseTextBox textBox)
    {
        var updateMethod = textBox.GetType().GetMethod("UpdateFiltering", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        updateMethod?.Invoke(textBox, null);
    }

    /// <summary>
    /// Invokes the InsertSelectedText method using reflection.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    public static void InvokeInsertSelectedText(IntellisenseTextBox textBox)
    {
        var insertMethod = textBox.GetType().GetMethod("InsertSelectedText", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        insertMethod?.Invoke(textBox, null);
    }

    /// <summary>
    /// Invokes the IsValidPriorityPosition method using reflection.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <returns>The result of the IsValidPriorityPosition method.</returns>
    public static bool InvokeIsValidPriorityPosition(IntellisenseTextBox textBox)
    {
        var isValidMethod = textBox.GetType().GetMethod("IsValidPriorityPosition", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        var result = isValidMethod?.Invoke(textBox, null);
        return (bool)(result ?? false);
    }

    /// <summary>
    /// Sets up a text box with text and cursor position for testing.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="text">The text to set.</param>
    /// <param name="cursorPosition">The cursor position (defaults to end of text).</param>
    public static void SetupTextBox(IntellisenseTextBox textBox, string text, int? cursorPosition = null)
    {
        textBox.Text = text;
        textBox.CaretIndex = cursorPosition ?? text.Length;
    }

    /// <summary>
    /// Sets up a text box for project autocompletion testing.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="baseText">The base text before the trigger.</param>
    /// <param name="triggerPosition">The position of the trigger character.</param>
    /// <returns>The full text with trigger character.</returns>
    public static string SetupForProjectAutocompletion(IntellisenseTextBox textBox, string baseText, int triggerPosition)
    {
        var fullText = baseText + "+";
        SetupTextBox(textBox, fullText, triggerPosition + 1);
        SetTriggerPosition(textBox, triggerPosition);
        return fullText;
    }

    /// <summary>
    /// Sets up a text box for context autocompletion testing.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="baseText">The base text before the trigger.</param>
    /// <param name="triggerPosition">The position of the trigger character.</param>
    /// <returns>The full text with trigger character.</returns>
    public static string SetupForContextAutocompletion(IntellisenseTextBox textBox, string baseText, int triggerPosition)
    {
        var fullText = baseText + "@";
        SetupTextBox(textBox, fullText, triggerPosition + 1);
        SetTriggerPosition(textBox, triggerPosition);
        return fullText;
    }

    /// <summary>
    /// Sets up a text box for priority autocompletion testing.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="baseText">The base text before the trigger.</param>
    /// <param name="triggerPosition">The position of the trigger character.</param>
    /// <returns>The full text with trigger character.</returns>
    public static string SetupForPriorityAutocompletion(IntellisenseTextBox textBox, string baseText, int triggerPosition)
    {
        var fullText = baseText + "(";
        SetupTextBox(textBox, fullText, triggerPosition + 1);
        SetTriggerPosition(textBox, triggerPosition);
        return fullText;
    }

    /// <summary>
    /// Verifies that the dropdown is open and has items.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="expectedMinItems">The minimum number of items expected (default: 1).</param>
    /// <returns>True if the dropdown is open and has the expected number of items.</returns>
    public static bool VerifyDropdownIsOpenWithItems(IntellisenseTextBox textBox, int expectedMinItems = 1)
    {
        var popup = textBox.DropDownPopup;
        var list = textBox.DropDownList;
        
        return popup?.IsOpen == true && 
               list?.Items.Count >= expectedMinItems;
    }

    /// <summary>
    /// Verifies that the dropdown is closed.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <returns>True if the dropdown is closed.</returns>
    public static bool VerifyDropdownIsClosed(IntellisenseTextBox textBox)
    {
        var popup = textBox.DropDownPopup;
        return popup?.IsOpen == false;
    }

    /// <summary>
    /// Gets the selected item from the dropdown list.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <returns>The selected item, or null if no selection.</returns>
    public static object? GetSelectedItem(IntellisenseTextBox textBox)
    {
        var list = textBox.DropDownList;
        if (list?.SelectedIndex >= 0 && list.SelectedIndex < list.Items.Count)
        {
            return list.Items[list.SelectedIndex];
        }
        return null;
    }

    /// <summary>
    /// Sets the selected item in the dropdown list.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="index">The index of the item to select.</param>
    public static void SetSelectedItem(IntellisenseTextBox textBox, int index)
    {
        var list = textBox.DropDownList;
        if (list != null && index >= 0 && index < list.Items.Count)
        {
            list.SelectedIndex = index;
        }
    }

    /// <summary>
    /// Finds the index of an item containing the specified text.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="searchText">The text to search for.</param>
    /// <param name="caseSensitive">Whether the search should be case sensitive.</param>
    /// <returns>The index of the item, or -1 if not found.</returns>
    public static int FindItemIndex(IntellisenseTextBox textBox, string searchText, bool caseSensitive = false)
    {
        var list = textBox.DropDownList;
        if (list == null) return -1;

        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        
        for (int i = 0; i < list.Items.Count; i++)
        {
            var itemText = list.Items[i]?.ToString();
            if (itemText != null && itemText.Contains(searchText, comparison))
            {
                return i;
            }
        }
        
        return -1;
    }

    /// <summary>
    /// Verifies that the dropdown contains an item with the specified text.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="searchText">The text to search for.</param>
    /// <param name="caseSensitive">Whether the search should be case sensitive.</param>
    /// <returns>True if the item is found.</returns>
    public static bool VerifyDropdownContainsItem(IntellisenseTextBox textBox, string searchText, bool caseSensitive = false)
    {
        return FindItemIndex(textBox, searchText, caseSensitive) >= 0;
    }

    /// <summary>
    /// Performs a complete autocompletion workflow: show suggestions, select item, and insert.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="trigger">The trigger character.</param>
    /// <param name="itemIndex">The index of the item to select and insert.</param>
    /// <returns>True if the workflow completed successfully.</returns>
    public static bool PerformCompleteAutocompletionWorkflow(IntellisenseTextBox textBox, char trigger, int itemIndex)
    {
        try
        {
            // Show suggestions
            textBox.ShowSuggestions(trigger);
            
            // Verify dropdown is open
            if (!VerifyDropdownIsOpenWithItems(textBox))
                return false;
            
            // Select the specified item
            SetSelectedItem(textBox, itemIndex);
            
            // Insert the selected text
            InvokeInsertSelectedText(textBox);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Performs keyboard navigation: show suggestions and navigate with arrow keys.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance.</param>
    /// <param name="trigger">The trigger character.</param>
    /// <param name="navigationKeys">The sequence of navigation keys to press.</param>
    /// <returns>True if navigation completed successfully.</returns>
    public static bool PerformKeyboardNavigation(IntellisenseTextBox textBox, char trigger, params Key[] navigationKeys)
    {
        try
        {
            // Show suggestions
            textBox.ShowSuggestions(trigger);
            
            // Verify dropdown is open
            if (!VerifyDropdownIsOpenWithItems(textBox))
                return false;
            
            // Perform navigation
            foreach (var key in navigationKeys)
            {
                SimulateKeyDown(textBox, key);
            }
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Creates a standard test setup for IntellisenseTextBox with common test data.
    /// </summary>
    /// <param name="textBox">The IntellisenseTextBox instance to set up.</param>
    /// <param name="taskList">The TaskList to use (optional, will create default if null).</param>
    /// <returns>The configured TaskList.</returns>
    public static TodoTxt.Core.TaskList CreateStandardTestSetup(IntellisenseTextBox textBox, TodoTxt.Core.TaskList? taskList = null)
    {
        if (taskList == null)
        {
            taskList = new TodoTxt.Core.TaskList();
            taskList.Tasks.Add(new TodoTxt.Core.Task("Buy groceries +shopping @home"));
            taskList.Tasks.Add(new TodoTxt.Core.Task("Call mom @phone"));
            taskList.Tasks.Add(new TodoTxt.Core.Task("Finish project report +work @office"));
            taskList.Tasks.Add(new TodoTxt.Core.Task("(A) High priority task +important"));
            taskList.Tasks.Add(new TodoTxt.Core.Task("(B) Medium priority task +work"));
            taskList.UpdateTaskListMetaData();
        }
        
        textBox.TaskList = taskList;
        return taskList;
    }
}
