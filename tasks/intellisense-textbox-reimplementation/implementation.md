# IntellisenseTextBox Implementation Steps

## ⚠️ CRITICAL IMPLEMENTATION RULE ⚠️
**DO NOT PERFORM MORE THAN ONE STEP AT A TIME! STRICTLY!!! NEVER!!!**

**MANDATORY PROCESS:**
1. Complete ONE step completely
2. Test that step thoroughly 
3. Get user confirmation that the step works
4. ONLY THEN proceed to the next step
5. If any step fails, STOP and get user input before continuing

**VIOLATION OF THIS RULE WILL RESULT IN COMPLETE FAILURE**

## Overview
This document provides a detailed, step-by-step implementation plan for reimplementing the IntellisenseTextBox component. Each step is designed to be small, testable, and build upon the previous step.

## Phase 1: Basic Functionality

### Step 1.1: Create Minimal TextBox
**Objective**: Create a basic TextBox that inherits from Avalonia TextBox and is visible

**Tasks**:
- [ ] Create new IntellisenseTextBox class inheriting from TextBox
- [ ] Add basic constructor with no extra functionality
- [ ] Add minimal using statements
- [ ] Test that component renders and is visible
- [ ] Verify basic text input works

**Code Structure**:
```csharp
using Avalonia.Controls;
using ToDoLib;

namespace TodoTxt.Avalonia.Core.Controls
{
    public class IntellisenseTextBox : TextBox
    {
        public IntellisenseTextBox()
        {
            // Empty constructor for now
        }
    }
}
```

**Testing**:
- [ ] Replace TextBox in MainWindow.axaml with IntellisenseTextBox
- [ ] Verify component is visible
- [ ] Verify text input works
- [ ] Verify focus works

### Step 1.2: Add Basic Properties
**Objective**: Add the required properties for data binding

**Tasks**:
- [ ] Add TaskList property using StyledProperty
- [ ] Add CaseSensitive property using StyledProperty
- [ ] Test property binding in MainWindow
- [ ] Verify properties can be set and retrieved

**Code Addition**:
```csharp
public static readonly StyledProperty<TaskList?> TaskListProperty =
    AvaloniaProperty.Register<IntellisenseTextBox, TaskList?>(nameof(TaskList));

public TaskList? TaskList
{
    get => GetValue(TaskListProperty);
    set => SetValue(TaskListProperty, value);
}

public static readonly StyledProperty<bool> CaseSensitiveProperty =
    AvaloniaProperty.Register<IntellisenseTextBox, bool>(nameof(CaseSensitive), false);

public bool CaseSensitive
{
    get => GetValue(CaseSensitiveProperty);
    set => SetValue(CaseSensitiveProperty, value);
}
```

**Testing**:
- [ ] Set TaskList property in MainWindow.axaml
- [ ] Set CaseSensitive property in MainWindow.axaml
- [ ] Verify properties are bound correctly
- [ ] Test with sample TaskList data

### Step 1.3: Add Text Change Detection
**Objective**: Detect when text changes and log trigger characters

**Tasks**:
- [ ] Add TextChanged event handler
- [ ] Add basic trigger character detection (+/@/()
- [ ] Add debug logging for trigger detection
- [ ] Test trigger detection works

**Code Addition**:
```csharp
public IntellisenseTextBox()
{
    this.TextChanged += IntellisenseTextBox_TextChanged;
}

private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
        return;

    var lastChar = this.Text[this.CaretIndex - 1];
    if (lastChar == '+' || lastChar == '@' || lastChar == '(')
    {
        System.Diagnostics.Debug.WriteLine($"Trigger character detected: {lastChar}");
    }
}
```

**Testing**:
- [ ] Type trigger characters and verify debug output
- [ ] Test with different cursor positions
- [ ] Verify no false triggers

## Phase 2: Popup System

### Step 2.1: Create Basic Popup
**Objective**: Create and manage a basic popup that can be shown/hidden

**Tasks**:
- [ ] Add Popup field to class
- [ ] Initialize Popup in constructor
- [ ] Add ShowPopup() method
- [ ] Add HidePopup() method
- [ ] Test popup can be shown and hidden

**Code Addition**:
```csharp
private Popup? _intellisensePopup;

public IntellisenseTextBox()
{
    this.TextChanged += IntellisenseTextBox_TextChanged;
    
    _intellisensePopup = new Popup
    {
        PlacementTarget = this,
        Placement = PlacementMode.Bottom,
        IsLightDismissEnabled = true
    };
}

public void ShowPopup()
{
    _intellisensePopup!.IsOpen = true;
}

public void HidePopup()
{
    _intellisensePopup!.IsOpen = false;
}
```

**Testing**:
- [ ] Call ShowPopup() and verify popup appears
- [ ] Call HidePopup() and verify popup disappears
- [ ] Test popup positioning (even if not perfect)

### Step 2.2: Add ListBox to Popup
**Objective**: Add a ListBox inside the popup for displaying suggestions

**Tasks**:
- [ ] Add ListBox field to class
- [ ] Initialize ListBox in constructor
- [ ] Set ListBox as Popup child
- [ ] Add basic ListBox styling
- [ ] Test ListBox appears in popup

**Code Addition**:
```csharp
private Popup? _intellisensePopup;
private ListBox? _intellisenseList;

public IntellisenseTextBox()
{
    this.TextChanged += IntellisenseTextBox_TextChanged;
    
    _intellisenseList = new ListBox
    {
        MaxHeight = 200,
        MinWidth = 200
    };
    
    _intellisensePopup = new Popup
    {
        PlacementTarget = this,
        Placement = PlacementMode.Bottom,
        IsLightDismissEnabled = true,
        Child = _intellisenseList
    };
}
```

**Testing**:
- [ ] Show popup and verify ListBox is visible
- [ ] Test ListBox sizing
- [ ] Verify ListBox is properly contained in popup

### Step 2.3: Connect Trigger Detection to Popup
**Objective**: Show popup when trigger characters are detected

**Tasks**:
- [ ] Modify TextChanged handler to show popup
- [ ] Add basic test data for popup
- [ ] Test popup appears on trigger characters
- [ ] Test popup hides when not needed

**Code Addition**:
```csharp
private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
    {
        HidePopup();
        return;
    }

    var lastChar = this.Text[this.CaretIndex - 1];
    if (lastChar == '+' || lastChar == '@' || lastChar == '(')
    {
        System.Diagnostics.Debug.WriteLine($"Trigger character detected: {lastChar}");
        
        // Add test data
        var testData = new List<string> { "Test1", "Test2", "Test3" };
        _intellisenseList!.ItemsSource = testData;
        ShowPopup();
    }
    else
    {
        HidePopup();
    }
}
```

**Testing**:
- [ ] Type '+' and verify popup appears with test data
- [ ] Type '@' and verify popup appears
- [ ] Type '(' and verify popup appears
- [ ] Type other characters and verify popup hides
- [ ] Test cursor movement and popup behavior

## Phase 3: Suggestion Display

### Step 3.1: Add Real Data Sources
**Objective**: Use actual TaskList data instead of test data

**Tasks**:
- [ ] Modify TextChanged handler to use TaskList data
- [ ] Add different data sources for different triggers
- [ ] Test with real TaskList data
- [ ] Handle null TaskList gracefully

**Code Addition**:
```csharp
private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
    {
        HidePopup();
        return;
    }

    var lastChar = this.Text[this.CaretIndex - 1];
    if (lastChar == '+' || lastChar == '@' || lastChar == '(')
    {
        System.Diagnostics.Debug.WriteLine($"Trigger character detected: {lastChar}");
        
        IEnumerable<string> data = lastChar switch
        {
            '+' => TaskList?.Projects ?? new List<string>(),
            '@' => TaskList?.Contexts ?? new List<string>(),
            '(' => GetPriorityList(),
            _ => new List<string>()
        };
        
        if (data.Any())
        {
            _intellisenseList!.ItemsSource = data;
            ShowPopup();
        }
    }
    else
    {
        HidePopup();
    }
}

private List<string> GetPriorityList()
{
    return Enumerable.Range('A', 26).Select(i => $"({Convert.ToChar(i)})").ToList();
}
```

**Testing**:
- [ ] Test with real TaskList data
- [ ] Verify different triggers show different data
- [ ] Test with empty TaskList
- [ ] Test with null TaskList

### Step 3.2: Add Basic Selection
**Objective**: Allow selection of items from the ListBox

**Tasks**:
- [ ] Add SelectionChanged event handler
- [ ] Add basic text insertion logic
- [ ] Test selection and text insertion
- [ ] Hide popup after selection

**Code Addition**:
```csharp
public IntellisenseTextBox()
{
    this.TextChanged += IntellisenseTextBox_TextChanged;
    
    _intellisenseList = new ListBox
    {
        MaxHeight = 200,
        MinWidth = 200
    };
    
    _intellisenseList.SelectionChanged += IntellisenseList_SelectionChanged;
    
    _intellisensePopup = new Popup
    {
        PlacementTarget = this,
        Placement = PlacementMode.Bottom,
        IsLightDismissEnabled = true,
        Child = _intellisenseList
    };
}

private void IntellisenseList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
{
    if (_intellisenseList!.SelectedItem != null)
    {
        InsertSelectedText();
    }
}

private void InsertSelectedText()
{
    if (_intellisenseList!.SelectedItem == null)
        return;
        
    var selectedText = _intellisenseList.SelectedItem.ToString() ?? string.Empty;
    var currentText = this.Text ?? string.Empty;
    
    // Simple replacement for now - replace from trigger position to end
    var triggerPos = FindTriggerPosition();
    if (triggerPos >= 0)
    {
        var newText = currentText.Substring(0, triggerPos) + selectedText;
        this.Text = newText;
        this.CaretIndex = newText.Length;
    }
    
    HidePopup();
}

private int FindTriggerPosition()
{
    var text = this.Text ?? string.Empty;
    for (int i = text.Length - 1; i >= 0; i--)
    {
        if (text[i] == '+' || text[i] == '@' || text[i] == '(')
            return i;
    }
    return -1;
}
```

**Testing**:
- [ ] Click on ListBox items and verify selection
- [ ] Verify text is inserted correctly
- [ ] Verify popup hides after selection
- [ ] Test with different trigger types

### Step 3.3: Add Keyboard Selection
**Objective**: Allow keyboard navigation and selection in the ListBox

**Tasks**:
- [ ] Add KeyDown event handler to ListBox
- [ ] Handle Enter key for selection
- [ ] Handle Escape key to cancel
- [ ] Test keyboard navigation

**Code Addition**:
```csharp
public IntellisenseTextBox()
{
    this.TextChanged += IntellisenseTextBox_TextChanged;
    
    _intellisenseList = new ListBox
    {
        MaxHeight = 200,
        MinWidth = 200
    };
    
    _intellisenseList.SelectionChanged += IntellisenseList_SelectionChanged;
    _intellisenseList.KeyDown += IntellisenseList_KeyDown;
    
    _intellisensePopup = new Popup
    {
        PlacementTarget = this,
        Placement = PlacementMode.Bottom,
        IsLightDismissEnabled = true,
        Child = _intellisenseList
    };
}

private void IntellisenseList_KeyDown(object? sender, KeyEventArgs e)
{
    switch (e.Key)
    {
        case Key.Enter:
            InsertSelectedText();
            e.Handled = true;
            break;
        case Key.Escape:
            HidePopup();
            this.Focus();
            e.Handled = true;
            break;
    }
}
```

**Testing**:
- [ ] Use arrow keys to navigate ListBox
- [ ] Press Enter to select item
- [ ] Press Escape to cancel
- [ ] Verify focus returns to TextBox

## Phase 4: Full Functionality

### Step 4.1: Add TextBox Keyboard Handling
**Objective**: Handle keyboard events in the TextBox to control the popup

**Tasks**:
- [ ] Override OnKeyUp method
- [ ] Handle Down arrow to focus ListBox
- [ ] Handle Escape to hide popup
- [ ] Handle other keys appropriately

**Code Addition**:
```csharp
protected override void OnKeyUp(KeyEventArgs e)
{
    base.OnKeyUp(e);
    
    if (_intellisensePopup!.IsOpen)
    {
        switch (e.Key)
        {
            case Key.Down:
                if (_intellisenseList!.Items.Count > 0)
                {
                    _intellisenseList.SelectedIndex = 0;
                    _intellisenseList.Focus();
                }
                e.Handled = true;
                break;
            case Key.Escape:
                HidePopup();
                e.Handled = true;
                break;
            case Key.Space:
            case Key.Enter:
                HidePopup();
                e.Handled = false; // Allow normal text input
                break;
        }
    }
}
```

**Testing**:
- [ ] Press Down arrow when popup is open
- [ ] Press Escape to hide popup
- [ ] Press Space/Enter to hide popup
- [ ] Verify normal text input still works

### Step 4.2: Add Text Filtering
**Objective**: Filter suggestions based on typed text

**Tasks**:
- [ ] Track trigger position
- [ ] Extract typed text after trigger
- [ ] Filter suggestions based on typed text
- [ ] Update ListBox with filtered results

**Code Addition**:
```csharp
private int _triggerPosition = -1;

private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
    {
        HidePopup();
        return;
    }

    var lastChar = this.Text[this.CaretIndex - 1];
    if (lastChar == '+' || lastChar == '@' || lastChar == '(')
    {
        _triggerPosition = this.CaretIndex - 1;
        ShowSuggestions(lastChar);
    }
    else if (_triggerPosition >= 0 && this.CaretIndex > _triggerPosition)
    {
        // Update filtering
        UpdateFiltering();
    }
    else
    {
        HidePopup();
    }
}

private void ShowSuggestions(char trigger)
{
    IEnumerable<string> data = trigger switch
    {
        '+' => TaskList?.Projects ?? new List<string>(),
        '@' => TaskList?.Contexts ?? new List<string>(),
        '(' => GetPriorityList(),
        _ => new List<string>()
    };
    
    if (data.Any())
    {
        _intellisenseList!.ItemsSource = data;
        ShowPopup();
    }
}

private void UpdateFiltering()
{
    if (_triggerPosition < 0 || _intellisenseList?.ItemsSource == null)
        return;
        
    var typedText = this.Text!.Substring(_triggerPosition + 1, this.CaretIndex - _triggerPosition - 1);
    var allItems = _intellisenseList.ItemsSource.Cast<string>();
    
    var filteredItems = CaseSensitive 
        ? allItems.Where(item => item.Contains(typedText))
        : allItems.Where(item => item.IndexOf(typedText, StringComparison.CurrentCultureIgnoreCase) >= 0);
    
    _intellisenseList.ItemsSource = filteredItems.ToList();
}
```

**Testing**:
- [ ] Type trigger character and verify all suggestions show
- [ ] Type additional characters and verify filtering
- [ ] Test case-sensitive and case-insensitive filtering
- [ ] Test with no matches

### Step 4.3: Improve Text Insertion
**Objective**: Improve text insertion to replace only the typed portion

**Tasks**:
- [ ] Modify InsertSelectedText to replace only typed portion
- [ ] Handle cursor positioning correctly
- [ ] Test with different trigger types
- [ ] Handle edge cases

**Code Addition**:
```csharp
private void InsertSelectedText()
{
    if (_intellisenseList!.SelectedItem == null || _triggerPosition < 0)
        return;
        
    var selectedText = _intellisenseList.SelectedItem.ToString() ?? string.Empty;
    var currentText = this.Text ?? string.Empty;
    
    // Replace from trigger position to current cursor position
    var textToReplace = currentText.Substring(_triggerPosition, this.CaretIndex - _triggerPosition);
    var newText = currentText.Remove(_triggerPosition, this.CaretIndex - _triggerPosition);
    newText = newText.Insert(_triggerPosition, selectedText);
    
    this.Text = newText;
    this.CaretIndex = _triggerPosition + selectedText.Length;
    
    HidePopup();
    _triggerPosition = -1;
}
```

**Testing**:
- [ ] Test text insertion with different trigger types
- [ ] Verify cursor positioning is correct
- [ ] Test with partial typing
- [ ] Test edge cases

### Step 4.4: Add Priority-Specific Logic
**Objective**: Handle priority autocompletion with specific positioning rules

**Tasks**:
- [ ] Add logic for priority positioning (start of line or after date)
- [ ] Test priority autocompletion
- [ ] Handle date detection for priority
- [ ] Test edge cases

**Code Addition**:
```csharp
private void ShowSuggestions(char trigger)
{
    if (trigger == '(')
    {
        // Special handling for priority - only at start of line or after date
        if (!IsValidPriorityPosition())
        {
            HidePopup();
            return;
        }
    }
    
    IEnumerable<string> data = trigger switch
    {
        '+' => TaskList?.Projects ?? new List<string>(),
        '@' => TaskList?.Contexts ?? new List<string>(),
        '(' => GetPriorityList(),
        _ => new List<string>()
    };
    
    if (data.Any())
    {
        _intellisenseList!.ItemsSource = data;
        ShowPopup();
    }
}

private bool IsValidPriorityPosition()
{
    var text = this.Text ?? string.Empty;
    var caretIndex = this.CaretIndex;
    
    // Priority can be at start of line
    if (caretIndex == 1)
        return true;
        
    // Priority can be after date (YYYY-MM-DD )
    if (caretIndex == 12)
    {
        var startText = text.Substring(0, 12);
        var dateRegex = new Regex(@"^[0-9]{4}\-[0-9]{2}\-[0-9]{2}\s$");
        return dateRegex.IsMatch(startText);
    }
    
    return false;
}
```

**Testing**:
- [ ] Test priority at start of line
- [ ] Test priority after date
- [ ] Test priority in invalid positions
- [ ] Test date detection

## Phase 5: Integration and Polish

### Step 5.1: Full Integration
**Objective**: Integrate with MainWindow and test all functionality

**Tasks**:
- [ ] Uncomment IntellisenseTextBox in MainWindow.axaml
- [ ] Comment out regular TextBox
- [ ] Test all functionality in MainWindow
- [ ] Fix any integration issues

**Testing**:
- [ ] Test in MainWindow context
- [ ] Test with real TaskList data
- [ ] Test all trigger types
- [ ] Test keyboard and mouse interaction

### Step 5.2: Error Handling and Edge Cases
**Objective**: Add proper error handling and handle edge cases

**Tasks**:
- [ ] Add null checks and error handling
- [ ] Handle empty TaskList
- [ ] Handle invalid cursor positions
- [ ] Add defensive programming

**Code Addition**:
```csharp
private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    try
    {
        if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
        {
            HidePopup();
            return;
        }

        // Rest of the logic with proper error handling
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error in text changed handler: {ex.Message}");
        HidePopup();
    }
}
```

**Testing**:
- [ ] Test with null TaskList
- [ ] Test with empty TaskList
- [ ] Test with invalid cursor positions
- [ ] Test error conditions

### Step 5.3: Code Cleanup and Documentation
**Objective**: Clean up code and add proper documentation

**Tasks**:
- [ ] Remove debug statements
- [ ] Add XML documentation
- [ ] Clean up code formatting
- [ ] Add comments for complex logic

**Testing**:
- [ ] Verify all functionality still works
- [ ] Test performance
- [ ] Review code quality

## Testing Strategy

### Unit Testing
- Test each method individually
- Test with various input conditions
- Test edge cases and error conditions

### Integration Testing
- Test in MainWindow context
- Test with real TaskList data
- Test all user interaction scenarios

### User Testing
- Test all trigger types
- Test keyboard navigation
- Test mouse interaction
- Test filtering functionality

## Rollback Plan

### If Any Step Fails
1. **Document the Issue**: Record what didn't work
2. **Revert Changes**: Go back to previous working step
3. **Analyze Problem**: Understand why the step failed
4. **Adjust Approach**: Modify the step or break it down further
5. **Retry**: Attempt the step again with modifications

### Emergency Rollback
- Keep the current working TextBox as fallback
- Comment out IntellisenseTextBox in MainWindow
- Restore previous working state

## Success Metrics

### Functional Requirements
- [ ] Component is visible and functional
- [ ] All trigger types work (+/@/()
- [ ] Popup appears and disappears correctly
- [ ] Suggestions are displayed and selectable
- [ ] Text insertion works correctly
- [ ] Filtering works (case-sensitive and insensitive)
- [ ] Keyboard navigation works
- [ ] Mouse selection works

### Performance Requirements
- [ ] Popup appears quickly (< 100ms)
- [ ] Filtering is responsive
- [ ] No memory leaks
- [ ] Smooth user interaction

### Integration Requirements
- [ ] Works with MainWindow
- [ ] Data binding works correctly
- [ ] Event handling works correctly
- [ ] No conflicts with other components