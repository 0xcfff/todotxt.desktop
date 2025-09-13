# IntellisenseTextBox Reimplementation Research

## Current State Analysis

### Problem Statement
The current Avalonia implementation of `IntellisenseTextBox` is not working - it's not showing up when used in the MainWindow. When replaced with a regular `TextBox`, the application works correctly. This indicates fundamental issues with the current Avalonia implementation.

### Current Avalonia Implementation Issues
Based on analysis of `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs`:

1. **Visibility Issues**: The control is not appearing in the UI
2. **Constructor Problems**: Complex initialization with try-catch blocks suggests instability
3. **Styling Conflicts**: Hardcoded background/foreground colors may conflict with application theme
4. **Event Handling**: Incomplete event handling implementation
5. **Popup Positioning**: Simplified `GetRectFromCharacterIndex` method that doesn't properly calculate character positions

### Original WPF Implementation Analysis
The working WPF implementation in `src/TodoTxt.UI/Controls/IntellisenseTextBox.cs` provides the reference:

#### Key Features:
1. **Autocompletion Triggers**: 
   - `+` for projects
   - `@` for contexts  
   - `(` for priorities (with date validation)

2. **Popup Behavior**:
   - Shows filtered list based on typed characters
   - Keyboard navigation (arrow keys, enter, escape, tab, space)
   - Mouse selection support
   - Case-sensitive/insensitive filtering

3. **Text Insertion**:
   - Replaces text from trigger position to cursor
   - Updates cursor position after insertion
   - Maintains focus on textbox

#### Core Components:
- `Popup` with `ListBox` for suggestions
- `TaskList` dependency property for data source
- `CaseSensitive` dependency property for filtering behavior
- Event handlers for text changes, key events, and mouse events

### Key Differences: WPF vs Avalonia

#### 1. Dependency Properties
**WPF:**
```csharp
public static readonly DependencyProperty TaskListProperty =
    DependencyProperty.Register("TaskList", typeof(TaskList), typeof(IntellisenseTextBox), new UIPropertyMetadata(null));
```

**Avalonia:**
```csharp
public static readonly StyledProperty<TaskList?> TaskListProperty =
    AvaloniaProperty.Register<IntellisenseTextBox, TaskList?>(nameof(TaskList));
```

#### 2. Popup Behavior
**WPF:**
- `PlacementRectangle` for precise positioning
- `StaysOpen = false` for auto-dismiss
- Direct `IsOpen` property control

**Avalonia:**
- `PlacementTarget` and `Placement` mode
- `IsLightDismissEnabled` for auto-dismiss
- Different positioning system

#### 3. Event Handling
**WPF:**
- `PreviewKeyUp` events
- `MouseUp` events
- `TextChanged` with `TextChangedEventArgs.Changes`

**Avalonia:**
- `KeyUp` events
- `PointerReleased` events  
- `TextChanged` with different event args structure

#### 4. ListBox Filtering
**WPF:**
```csharp
this.IntellisenseList.Items.Filter = (o) => o.ToString().Contains(word);
```

**Avalonia:**
```csharp
var filteredItems = _intellisenseList.ItemsSource?.Cast<string>()
    .Where(item => item.Contains(word))
    .ToList();
_intellisenseList.ItemsSource = filteredItems;
```

### Current Usage Context
The IntellisenseTextBox is used in `MainWindow.axaml` for task input:
- Bound to `TaskInputText` property
- Connected to `TaskList` for autocompletion data
- Has `KeyUp` event handler for task creation
- Currently commented out due to non-functionality

### Integration Points
1. **ViewModel**: `MainWindowViewModel` provides `TaskList` property
2. **Data Binding**: Text binding to `TaskInputText`
3. **Event Handling**: `TaskTextBox_KeyUp` method in MainWindow code-behind
4. **Styling**: Should inherit from application theme, not hardcoded colors

## Root Cause Analysis

### Primary Issues:
1. **Complex Initialization**: The current constructor tries to do too much at once
2. **Avalonia API Misunderstanding**: Incorrect usage of Avalonia-specific APIs
3. **Event System Differences**: WPF event patterns don't directly translate
4. **Styling Conflicts**: Hardcoded styling interferes with application appearance
5. **Missing Error Handling**: No graceful degradation when components fail

### Secondary Issues:
1. **Incomplete Feature Set**: Many features from WPF version are missing or broken
2. **Poor Separation of Concerns**: UI logic mixed with control initialization
3. **No Testing Strategy**: No way to verify functionality incrementally

## Recommended Approach

### Incremental Development Strategy
1. **Start Simple**: Basic TextBox inheritance with no extra functionality
2. **Add Properties**: Implement dependency properties one at a time
3. **Add Popup**: Implement basic popup display
4. **Add Event Handling**: Implement text change detection
5. **Add Filtering**: Implement suggestion filtering
6. **Add Navigation**: Implement keyboard and mouse interaction
7. **Add Styling**: Implement proper theming integration
8. **Add Error Handling**: Implement robust error handling

### Testing Strategy
- Each step should be testable in the actual application
- Use debug output to verify functionality
- Compare behavior with regular TextBox at each step
- Validate against original WPF implementation

### Risk Mitigation
- Keep original WPF implementation as reference
- Maintain working TextBox fallback
- Use version control for each incremental step
- Document each step's functionality and issues
