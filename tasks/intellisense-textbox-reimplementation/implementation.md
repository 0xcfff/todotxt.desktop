# IntellisenseTextBox Implementation Steps

## Overview
This document outlines the detailed step-by-step implementation of the IntellisenseTextBox component. Each step builds upon the previous one, allowing for incremental testing and issue identification.

## Implementation Steps

### Step 1: Basic TextBox Inheritance ✅
**Objective**: Create a minimal IntellisenseTextBox that inherits from TextBox with no extra functionality.

**Implementation**:
```csharp
public class IntellisenseTextBox : TextBox
{
    public IntellisenseTextBox()
    {
        // Empty constructor - just inherit TextBox behavior
    }
}
```

**Testing**:
- [ ] Control appears in MainWindow
- [ ] Text input works normally
- [ ] No visual differences from regular TextBox
- [ ] No exceptions or errors

**Success Criteria**: Control functions identically to regular TextBox

---

### Step 2: Add Dependency Properties ✅
**Objective**: Add TaskList and CaseSensitive properties using Avalonia StyledProperty system.

**Implementation**:
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
- [ ] Properties can be set in XAML
- [ ] Data binding works correctly
- [ ] Properties maintain their values
- [ ] No impact on basic TextBox functionality

**Success Criteria**: Properties work correctly with data binding

---

### Step 3: Add Basic Popup Display ✅
**Objective**: Add popup and listbox components without any functionality.

**Implementation**:
```csharp
private Popup? _intellisensePopup;
private ListBox? _intellisenseList;

public IntellisenseTextBox()
{
    // Initialize popup
    _intellisensePopup = new Popup
    {
        PlacementTarget = this,
        Placement = PlacementMode.Bottom,
        IsLightDismissEnabled = true,
        IsOpen = false
    };

    // Initialize listbox
    _intellisenseList = new ListBox
    {
        MaxHeight = 200,
        MinWidth = 200
    };

    _intellisensePopup.Child = _intellisenseList;
}
```

**Testing**:
- [ ] Popup and ListBox are created without errors
- [ ] No visual impact on TextBox
- [ ] Popup is initially closed
- [ ] No memory leaks or exceptions

**Success Criteria**: Popup components exist but don't interfere with TextBox

---

### Step 4: Implement Text Change Detection ✅
**Objective**: Detect when specific trigger characters (+ @ () are typed.

**Implementation**:
```csharp
private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    // Basic text change detection
    if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
        return;

    var lastChar = this.Text[this.CaretIndex - 1];
    if (lastChar == '+' || lastChar == '@' || lastChar == '(')
    {
        // Log for debugging
        System.Diagnostics.Debug.WriteLine($"Trigger character detected: {lastChar}");
    }
}

public IntellisenseTextBox()
{
    // ... existing popup code ...
    
    this.TextChanged += IntellisenseTextBox_TextChanged;
}
```

**Testing**:
- [ ] Text change events are detected
- [ ] Trigger characters are identified correctly
- [ ] No performance impact on typing
- [ ] Debug output shows trigger detection

**Success Criteria**: Trigger characters are reliably detected

---

### Step 5: Add Suggestion Filtering and Display ✅
**Objective**: Show popup with filtered suggestions based on trigger character.

**Implementation**:
```csharp
private int _intelliPos;

private void ShowIntellisensePopup(IEnumerable<string> suggestions)
{
    if (suggestions == null || !suggestions.Any())
        return;

    _intellisenseList!.ItemsSource = suggestions;
    _intellisenseList.SelectedItem = null;
    _intellisensePopup!.IsOpen = true;
}

private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    // ... existing detection code ...
    
    if (lastChar == '+')
    {
        _intelliPos = this.CaretIndex - 1;
        ShowIntellisensePopup(TaskList?.Projects ?? new List<string>());
    }
    else if (lastChar == '@')
    {
        _intelliPos = this.CaretIndex - 1;
        ShowIntellisensePopup(TaskList?.Contexts ?? new List<string>());
    }
    else if (lastChar == '(')
    {
        _intelliPos = this.CaretIndex - 1;
        var priorities = Enumerable.Range('A', 26).Select(i => $"({Convert.ToChar(i)})").ToList();
        ShowIntellisensePopup(priorities);
    }
}
```

**Testing**:
- [ ] Popup appears when trigger characters are typed
- [ ] Correct suggestions are shown for each trigger
- [ ] Popup closes when clicking outside
- [ ] No exceptions with null TaskList

**Success Criteria**: Popup shows appropriate suggestions for each trigger

---

### Step 6: Implement Keyboard Navigation ✅
**Objective**: Add arrow key navigation and basic keyboard interaction.

**Implementation**:
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
                HideIntellisensePopup();
                e.Handled = true;
                break;
        }
    }
}

private void HideIntellisensePopup()
{
    _intellisensePopup!.IsOpen = false;
}
```

**Testing**:
- [ ] Down arrow selects first item in popup
- [ ] Escape key closes popup
- [ ] Focus moves to popup when navigating
- [ ] Other keys still work normally in TextBox

**Success Criteria**: Basic keyboard navigation works

---

### Step 7: Add Mouse Interaction ✅
**Objective**: Allow mouse selection of popup items.

**Implementation**:
```csharp
private void IntellisenseList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
{
    // Handle selection changes
}

private void IntellisenseList_PointerReleased(object? sender, PointerReleasedEventArgs e)
{
    if (_intellisenseList!.SelectedItem != null)
    {
        InsertIntellisenseText();
    }
}

public IntellisenseTextBox()
{
    // ... existing code ...
    
    _intellisenseList.SelectionChanged += IntellisenseList_SelectionChanged;
    _intellisenseList.PointerReleased += IntellisenseList_PointerReleased;
}
```

**Testing**:
- [ ] Mouse clicks select items in popup
- [ ] Selection changes are detected
- [ ] Clicking outside popup closes it
- [ ] No interference with TextBox mouse events

**Success Criteria**: Mouse interaction works correctly

---

### Step 8: Implement Text Insertion and Cursor Management ✅
**Objective**: Insert selected text and manage cursor position.

**Implementation**:
```csharp
private void InsertIntellisenseText()
{
    HideIntellisensePopup();

    if (_intellisenseList!.SelectedItem == null)
    {
        this.Focus();
        return;
    }

    var currentText = this.Text ?? string.Empty;
    var newText = _intellisenseList.SelectedItem.ToString() ?? string.Empty;
    
    // Remove text from trigger position to cursor
    var textToRemove = currentText.Substring(_intelliPos, this.CaretIndex - _intelliPos);
    var updatedText = currentText.Remove(_intelliPos, this.CaretIndex - _intelliPos);
    
    // Insert new text
    updatedText = updatedText.Insert(_intelliPos, newText);
    
    this.Text = updatedText;
    this.CaretIndex = _intelliPos + newText.Length;
    this.Focus();
}

protected override void OnKeyUp(KeyEventArgs e)
{
    // ... existing code ...
    
    if (_intellisensePopup!.IsOpen && _intellisenseList!.IsFocused)
    {
        switch (e.Key)
        {
            case Key.Enter:
            case Key.Tab:
            case Key.Space:
                InsertIntellisenseText();
                e.Handled = true;
                break;
        }
    }
}
```

**Testing**:
- [ ] Selected text is inserted correctly
- [ ] Cursor position is updated properly
- [ ] Text replacement works for partial matches
- [ ] Focus returns to TextBox after insertion

**Success Criteria**: Text insertion works correctly

---

### Step 9: Add Proper Styling and Theming ✅
**Objective**: Remove hardcoded styling and integrate with application theme.

**Implementation**:
```csharp
public IntellisenseTextBox()
{
    // ... existing popup code ...
    
    // Remove hardcoded styling
    // Let the control inherit from application theme
    
    // Set reasonable popup styling
    _intellisenseList.Background = Brushes.White;
    _intellisenseList.Foreground = Brushes.Black;
    _intellisenseList.BorderBrush = Brushes.Gray;
    _intellisenseList.BorderThickness = new Thickness(1);
}
```

**Testing**:
- [ ] Control inherits application theme
- [ ] Popup has appropriate styling
- [ ] No hardcoded colors interfere with theme
- [ ] Control looks consistent with rest of application

**Success Criteria**: Styling integrates properly with application theme

---

### Step 10: Add Error Handling and Edge Cases ✅
**Objective**: Handle edge cases and add robust error handling.

**Implementation**:
```csharp
private void ShowIntellisensePopup(IEnumerable<string> suggestions)
{
    try
    {
        if (suggestions == null || !suggestions.Any())
            return;

        if (_intellisensePopup == null || _intellisenseList == null)
            return;

        _intellisenseList.ItemsSource = suggestions;
        _intellisenseList.SelectedItem = null;
        _intellisensePopup.IsOpen = true;
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error showing intellisense popup: {ex.Message}");
        // Gracefully degrade - just don't show popup
    }
}

private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    try
    {
        // ... existing implementation with try-catch ...
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error in text changed handler: {ex.Message}");
        // Don't let errors break text input
    }
}
```

**Testing**:
- [ ] Handles null TaskList gracefully
- [ ] Handles empty suggestion lists
- [ ] Handles rapid text changes
- [ ] No exceptions break basic TextBox functionality
- [ ] Debug output shows any errors

**Success Criteria**: Robust error handling prevents crashes

---

## Testing Strategy

### Unit Testing
- Test each step in isolation
- Verify no regression in basic TextBox functionality
- Test edge cases and error conditions

### Integration Testing
- Test in actual MainWindow context
- Verify data binding works correctly
- Test with real TaskList data

### User Testing
- Compare behavior with original WPF implementation
- Verify keyboard shortcuts work as expected
- Test on different platforms (if possible)

## Rollback Plan

### For Each Step:
1. Keep previous working version in version control
2. Test rollback procedure before proceeding
3. Document any breaking changes
4. Maintain ability to revert to regular TextBox

### Complete Rollback:
1. Revert IntellisenseTextBox.cs to basic TextBox inheritance
2. Update MainWindow.axaml to use regular TextBox
3. Remove any IntellisenseTextBox-specific code
4. Verify application still functions correctly

## Success Metrics

### Functional Metrics:
- [ ] All trigger characters work (+ @ ()
- [ ] Suggestions are filtered correctly
- [ ] Keyboard navigation works
- [ ] Mouse selection works
- [ ] Text insertion works correctly
- [ ] Cursor positioning is accurate

### Performance Metrics:
- [ ] No noticeable lag in popup display
- [ ] No memory leaks during extended use
- [ ] Smooth typing experience
- [ ] Responsive keyboard navigation

### Quality Metrics:
- [ ] No exceptions during normal use
- [ ] Graceful handling of edge cases
- [ ] Consistent behavior across platforms
- [ ] Code follows Avalonia best practices
