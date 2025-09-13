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
- [x] Create new IntellisenseTextBox class inheriting from TextBox
- [x] Add basic constructor with no extra functionality
- [x] Add minimal using statements
- [x] Test that component renders and is visible
- [x] Verify basic text input works

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
- [x] Replace TextBox in MainWindow.axaml with IntellisenseTextBox
- [x] Verify component is visible
- [x] Verify text input works
- [x] Verify focus works

### Step 1.2: Add Basic Properties
**Objective**: Add the required properties for data binding

**Tasks**:
- [x] Add TaskList property using StyledProperty
- [x] Add CaseSensitive property using StyledProperty
- [x] Test property binding in MainWindow
- [x] Verify properties can be set and retrieved

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
- [x] Set TaskList property in MainWindow.axaml
- [x] Set CaseSensitive property in MainWindow.axaml
- [x] Verify properties are bound correctly
- [x] Test with sample TaskList data

### Step 1.3: Add Text Change Detection
**Objective**: Detect when text changes and log trigger characters

**Tasks**:
- [x] Add TextChanged event handler
- [x] Add basic trigger character detection (+/@/()
- [x] Add debug logging for trigger detection
- [x] Test trigger detection works

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
- [x] Type trigger characters and verify debug output
- [x] Test with different cursor positions
- [x] Verify no false triggers

## Phase 2: Popup System

### Step 2.1: Create Basic Popup
**Objective**: Create and manage a basic popup that can be shown/hidden

**Tasks**:
- [x] Add Popup field to class
- [x] Initialize Popup in constructor
- [x] Add ShowPopup() method
- [x] Add HidePopup() method
- [x] Test popup can be shown and hidden

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
- [x] Call ShowPopup() and verify popup appears
- [x] Call HidePopup() and verify popup disappears
- [x] Test popup positioning (even if not perfect)

### Step 2.2: Add ListBox to Popup
**Objective**: Add a ListBox inside the popup for displaying suggestions

**Tasks**:
- [x] Add ListBox field to class
- [x] Initialize ListBox in constructor
- [x] Set ListBox as Popup child
- [x] Add basic ListBox styling
- [x] Test ListBox appears in popup

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
- [x] Show popup and verify ListBox is visible
- [x] Test ListBox sizing
- [x] Verify ListBox is properly contained in popup

### Step 2.3: Connect Trigger Detection to Popup
**Objective**: Show popup when trigger characters are detected

**Tasks**:
- [x] Modify TextChanged handler to show popup
- [x] Add basic test data for popup
- [x] Test popup appears on trigger characters
- [x] Test popup hides when not needed

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
- [x] Type '+' and verify popup appears with test data
- [x] Type '@' and verify popup appears
- [x] Type '(' and verify popup appears
- [x] Type other characters and verify popup hides
- [x] Test cursor movement and popup behavior

## Phase 3: Suggestion Display

### Step 3.1: Add Real Data Sources
**Objective**: Use actual TaskList data instead of test data

**Tasks**:
- [x] Modify TextChanged handler to use TaskList data
- [x] Add different data sources for different triggers
- [x] Test with real TaskList data
- [x] Handle null TaskList gracefully

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
- [x] Test with real TaskList data
- [x] Verify different triggers show different data
- [x] Test with empty TaskList
- [x] Test with null TaskList

### Step 3.2: Add Basic Selection
**Objective**: Allow selection of items from the ListBox

**Tasks**:
- [x] Add SelectionChanged event handler
- [x] Add basic text insertion logic
- [x] Test selection and text insertion
- [x] Hide popup after selection

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
- [x] Click on ListBox items and verify selection
- [x] Verify text is inserted correctly
- [x] Verify popup hides after selection
- [x] Test with different trigger types

### Step 3.3: Add Keyboard Selection
**Objective**: Allow keyboard navigation and selection in the ListBox

**Tasks**:
- [x] Add KeyDown event handler to ListBox
- [x] Handle Enter key for selection
- [x] Handle Escape key to cancel
- [x] Test keyboard navigation

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
- [x] Use arrow keys to navigate ListBox
- [x] Press Enter to select item
- [x] Press Escape to cancel
- [x] Verify focus returns to TextBox

## Phase 4: Full Functionality

### Step 4.1: Add TextBox Keyboard Handling
**Objective**: Handle keyboard events in the TextBox to control the popup

**Tasks**:
- [x] Override OnKeyUp method
- [x] Handle Down arrow to focus ListBox
- [x] Handle Escape to hide popup
- [x] Handle other keys appropriately

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
- [x] Press Down arrow when popup is open
- [x] Press Escape to hide popup
- [x] Press Space/Enter to hide popup
- [x] Verify normal text input still works

### Step 4.2: Add Text Filtering
**Objective**: Filter suggestions based on typed text

**Tasks**:
- [x] Track trigger position
- [x] Extract typed text after trigger
- [x] Filter suggestions based on typed text
- [x] Update ListBox with filtered results

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
- [x] Type trigger character and verify all suggestions show
- [x] Type additional characters and verify filtering
- [x] Test case-sensitive and case-insensitive filtering
- [x] Test with no matches

### Step 4.3: Improve Text Insertion
**Objective**: Improve text insertion to replace only the typed portion

**Tasks**:
- [x] Modify InsertSelectedText to replace only typed portion
- [x] Handle cursor positioning correctly
- [x] Test with different trigger types
- [x] Handle edge cases

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
- [x] Test text insertion with different trigger types
- [x] Verify cursor positioning is correct
- [x] Test with partial typing
- [x] Test edge cases

### Step 4.4: Add Priority-Specific Logic
**Objective**: Handle priority autocompletion with specific positioning rules

**Tasks**:
- [x] Add logic for priority positioning (start of line or after date)
- [x] Test priority autocompletion
- [x] Handle date detection for priority
- [x] Test edge cases

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
- [x] Test priority at start of line
- [x] Test priority after date
- [x] Test priority in invalid positions
- [x] Test date detection

## Phase 5: Integration and Polish

### Step 5.1: Full Integration
**Objective**: Integrate with MainWindow and test all functionality

**Tasks**:
- [x] Uncomment IntellisenseTextBox in MainWindow.axaml
- [x] Comment out regular TextBox
- [x] Test all functionality in MainWindow
- [x] Fix any integration issues

**Testing**:
- [x] Test in MainWindow context
- [x] Test with real TaskList data
- [x] Test all trigger types
- [x] Test keyboard and mouse interaction

### Step 5.2: Error Handling and Edge Cases
**Objective**: Add proper error handling and handle edge cases

**Tasks**:
- [x] Add null checks and error handling
- [x] Handle empty TaskList
- [x] Handle invalid cursor positions
- [x] Add defensive programming

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
- [x] Test with null TaskList
- [x] Test with empty TaskList
- [x] Test with invalid cursor positions
- [x] Test error conditions

### Step 5.3: Code Cleanup and Documentation
**Objective**: Clean up code and add proper documentation

**Tasks**:
- [x] Remove debug statements
- [x] Add XML documentation
- [x] Clean up code formatting
- [x] Add comments for complex logic

**Testing**:
- [x] Verify all functionality still works
- [x] Test performance
- [x] Review code quality

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