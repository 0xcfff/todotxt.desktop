# IntellisenseTextBox Reimplementation Plan

## Objective
Reimplement the IntellisenseTextBox component for Avalonia UI by creating a working version that provides autocompletion functionality for todo.txt projects, contexts, and priorities. The current implementation is completely non-functional and needs to be replaced with a step-by-step approach.

## Scope

### Included
- Complete reimplementation of IntellisenseTextBox for Avalonia
- Support for project autocompletion (triggered by `+`)
- Support for context autocompletion (triggered by `@`)
- Support for priority autocompletion (triggered by `(`)
- Popup-based suggestion display
- Keyboard navigation and selection
- Mouse selection support
- Case-sensitive/insensitive filtering
- Integration with existing MainWindow and ViewModel

### Excluded
- Changes to the original WPF implementation
- Changes to the TaskList or Task classes
- Changes to the MainWindowViewModel (except integration)
- UI styling or theming (focus on functionality)

## Approach

### Strategy
**Incremental Development**: Build the component in very small, testable steps, starting from the most basic functionality and gradually adding features. Each step should be independently testable and functional.

### Key Principles
1. **Start Simple**: Begin with a basic TextBox that works
2. **Test Each Step**: Verify functionality at each increment
3. **Fail Fast**: Identify issues early in each step
4. **Reference Original**: Use WPF implementation as reference, not copy
5. **Avalonia-First**: Adapt to Avalonia patterns and APIs

## Components to Modify

### Primary Files
- `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs` - Complete replacement
- `src/TodoTxt.Avalonia/Views/MainWindow.axaml` - Uncomment and integrate
- `src/TodoTxt.Avalonia/Views/MainWindow.axaml.cs` - Update event handlers if needed

### Reference Files
- `src/TodoTxt.UI/Controls/IntellisenseTextBox.cs` - WPF implementation reference
- `src/TodoTxt.Avalonia/ViewModels/MainWindowViewModel.cs` - Integration point

## Dependencies

### External Libraries
- `Avalonia.Controls` - UI controls (TextBox, Popup, ListBox)
- `Avalonia.Input` - Input handling (KeyEventArgs, PointerEventArgs)
- `Avalonia.Interactivity` - Event handling
- `ToDoLib` - TaskList and Task classes

### Internal Dependencies
- `MainWindowViewModel.TaskList` - Source of autocompletion data
- `MainWindowViewModel.FilterCaseSensitive` - Case sensitivity setting
- `MainWindowViewModel.TaskInputText` - Text binding

## Risks

### High Risk
- **Popup Positioning**: Avalonia doesn't have `PlacementRectangle` like WPF
- **Text Cursor Position**: No direct equivalent to `GetRectFromCharacterIndex`
- **Focus Management**: Complex focus handling between TextBox and Popup
- **Event Handling**: Different event models between WPF and Avalonia

### Medium Risk
- **Property Binding**: StyledProperty vs DependencyProperty differences
- **ListBox Filtering**: Different filtering mechanisms
- **Integration Issues**: Binding and event handler compatibility

### Low Risk
- **Basic Inheritance**: TextBox inheritance should be straightforward
- **Trigger Detection**: Character detection logic is simple
- **Data Access**: TaskList integration is well-defined

## Mitigation Strategies

### For High Risk Items
- **Popup Positioning**: Use simplified positioning initially, improve later
- **Text Cursor Position**: Implement basic positioning, enhance incrementally
- **Focus Management**: Start with simple focus handling, add complexity gradually
- **Event Handling**: Test each event type individually

### For Medium Risk Items
- **Property Binding**: Follow Avalonia patterns exactly
- **ListBox Filtering**: Use LINQ-based filtering approach
- **Integration**: Test integration at each step

## Success Criteria

### Phase 1: Basic Functionality
- [ ] Component is visible and renders correctly
- [ ] Can receive focus and accept text input
- [ ] Basic TextBox functionality works
- [ ] Integration with MainWindow works

### Phase 2: Popup System
- [ ] Popup appears when trigger characters are typed
- [ ] Popup is positioned correctly (even if simplified)
- [ ] Popup can be hidden
- [ ] Focus management works

### Phase 3: Suggestion Display
- [ ] Suggestions are displayed in popup ListBox
- [ ] Basic selection works (keyboard or mouse)
- [ ] Text insertion works
- [ ] Popup hides after selection

### Phase 4: Full Functionality
- [ ] All trigger types work (+/@/()
- [ ] Keyboard navigation works (Up/Down arrows)
- [ ] Mouse selection works
- [ ] Filtering works (case-sensitive and insensitive)
- [ ] All edge cases handled

### Phase 5: Integration and Polish
- [ ] Full integration with MainWindow
- [ ] All data binding works
- [ ] Performance is acceptable
- [ ] Code is clean and maintainable

## Rollback Plan

### If Implementation Fails
1. **Keep Current State**: The current IntellisenseTextBox is already commented out
2. **Revert Changes**: Remove any new files or revert modified files
3. **Use Standard TextBox**: Continue using the working TextBox as fallback
4. **Document Issues**: Record what didn't work for future reference

### Backup Strategy
- Keep original WPF implementation as reference
- Keep current (broken) Avalonia implementation as reference
- Document each step for potential rollback

## Timeline Estimate

### Phase 1: Basic Functionality (1-2 hours)
- Simple TextBox inheritance
- Basic visibility and focus
- Integration testing

### Phase 2: Popup System (2-3 hours)
- Popup creation and management
- Basic positioning
- Show/hide functionality

### Phase 3: Suggestion Display (2-3 hours)
- ListBox integration
- Basic selection
- Text insertion

### Phase 4: Full Functionality (3-4 hours)
- All trigger types
- Keyboard navigation
- Mouse support
- Filtering

### Phase 5: Integration and Polish (1-2 hours)
- Full integration
- Testing and bug fixes
- Code cleanup

**Total Estimated Time**: 9-14 hours

## Next Steps

1. **Create Implementation Plan**: Detailed step-by-step implementation guide
2. **Set Up Testing Environment**: Ensure we can test each step
3. **Begin Phase 1**: Start with basic TextBox functionality
4. **Iterate and Test**: Complete each phase before moving to next
5. **Document Progress**: Record what works and what doesn't at each step
