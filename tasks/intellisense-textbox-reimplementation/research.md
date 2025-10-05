# IntellisenseTextBox Reimplementation Research

## Current State Analysis

### Original WPF Implementation (Working)
- **Location**: `src/TodoTxt.UI/Controls/IntellisenseTextBox.cs`
- **Namespace**: `Client`
- **Base Class**: `TextBox` (WPF)
- **Key Features**:
  - Autocompletion for projects (prefixed with `+`)
  - Autocompletion for contexts (prefixed with `@`)
  - Autocompletion for priorities (prefixed with `(`)
  - Popup-based suggestion list
  - Keyboard navigation (Tab, Enter, Space, Escape, Down arrow)
  - Mouse click selection
  - Case-sensitive/insensitive filtering
  - Text filtering as user types

### Current Avalonia Implementation (Non-Working)
- **Location**: `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs`
- **Namespace**: `TodoTxt.Avalonia.Core.Controls`
- **Base Class**: `TextBox` (Avalonia)
- **Issues Identified**:
  - Component is not visible/rendering properly
  - Popup positioning issues
  - Event handling problems
  - Property binding issues
  - Constructor has excessive debugging code and styling attempts

### Integration Points
- **MainWindow Usage**: Currently commented out in `MainWindow.axaml` (lines 77-87)
- **Replaced With**: Standard `TextBox` (lines 89-97)
- **Data Binding**: 
  - `TaskList="{Binding TaskList}"`
  - `CaseSensitive="{Binding FilterCaseSensitive}"`
  - `Text="{Binding TaskInputText}"`

## Key Differences Between WPF and Avalonia

### Property System
- **WPF**: Uses `DependencyProperty` with `DependencyProperty.Register`
- **Avalonia**: Uses `StyledProperty` with `AvaloniaProperty.Register`

### Popup System
- **WPF**: `Popup` with `PlacementRectangle` property
- **Avalonia**: `Popup` without `PlacementRectangle`, uses `PlacementTarget` and `Placement`

### Event Handling
- **WPF**: `PreviewKeyUp`, `MouseUp` events
- **Avalonia**: `KeyUp`, `PointerReleased` events

### ListBox Filtering
- **WPF**: Uses `Items.Filter` property with predicate
- **Avalonia**: Uses `ItemsSource` filtering with LINQ

### Text Positioning
- **WPF**: `GetRectFromCharacterIndex()` method available
- **Avalonia**: No direct equivalent, needs custom implementation

## Core Functionality Requirements

### 1. Basic TextBox Functionality
- Inherit from Avalonia `TextBox`
- Support standard text input
- Maintain cursor position tracking
- Handle text changes

### 2. Popup System
- Create and manage `Popup` control
- Position popup relative to text cursor
- Show/hide popup based on trigger characters
- Handle popup focus management

### 3. Suggestion List
- Display filtered suggestions in `ListBox`
- Support keyboard navigation (Up/Down arrows)
- Support mouse selection
- Handle selection events

### 4. Trigger Detection
- Detect `+` for project suggestions
- Detect `@` for context suggestions  
- Detect `(` for priority suggestions
- Track trigger position for text replacement

### 5. Text Insertion
- Replace text from trigger position to cursor
- Insert selected suggestion
- Update cursor position
- Maintain focus

### 6. Filtering
- Filter suggestions based on typed text
- Support case-sensitive/insensitive matching
- Update suggestions in real-time

## Dependencies

### External Libraries
- `ToDoLib` - For `TaskList` and `Task` classes
- `Avalonia.Controls` - For UI controls
- `Avalonia.Input` - For input handling
- `Avalonia.Interactivity` - For events

### Data Sources
- `TaskList.Projects` - List of project names
- `TaskList.Contexts` - List of context names
- Priority list - Hardcoded list of priority levels (A-Z)

## Risk Assessment

### High Risk
- Popup positioning in Avalonia (no `PlacementRectangle`)
- Text cursor position calculation
- Focus management between TextBox and Popup

### Medium Risk
- Event handling differences between WPF and Avalonia
- Property binding compatibility
- ListBox filtering implementation

### Low Risk
- Basic TextBox inheritance
- Data binding setup
- Trigger character detection

## Success Criteria

### Phase 1: Basic Visibility
- Component renders and is visible
- Can receive focus and text input
- Basic TextBox functionality works

### Phase 2: Popup System
- Popup appears when trigger characters are typed
- Popup is positioned correctly
- Popup can be hidden

### Phase 3: Suggestion Display
- Suggestions are displayed in popup
- Basic selection works
- Text insertion works

### Phase 4: Full Functionality
- All trigger types work (+/@/()
- Keyboard navigation works
- Mouse selection works
- Filtering works
- Case sensitivity works

## Implementation Strategy

### Incremental Approach
1. Start with minimal TextBox inheritance
2. Add popup system step by step
3. Add suggestion display
4. Add text insertion
5. Add filtering
6. Add keyboard navigation
7. Add mouse support
8. Add all trigger types
9. Polish and optimize

### Testing Strategy
- Test each phase independently
- Use simple test data initially
- Verify integration with MainWindow
- Test with real TaskList data
- Test edge cases and error conditions
