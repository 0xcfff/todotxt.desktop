# IntellisenseTextBox Reimplementation Task

## Overview
This task involves completely reimplementing the IntellisenseTextBox component for the Avalonia UI framework. The current implementation is non-functional and needs to be replaced with a working version that provides autocompletion functionality for todo.txt projects, contexts, and priorities.

## Problem Statement
- **Current State**: The existing IntellisenseTextBox in `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs` does not work at all
- **Impact**: The component is not visible/rendering, preventing users from using autocompletion features
- **Workaround**: Currently using a standard TextBox instead of the IntellisenseTextBox
- **Goal**: Create a fully functional IntellisenseTextBox that works with Avalonia UI

## Approach
**Incremental Development**: Build the component in very small, testable steps:
1. Start with basic TextBox functionality
2. Add popup system step by step
3. Add suggestion display and selection
4. Add filtering and keyboard navigation
5. Add all trigger types and polish

## Key Features to Implement
- **Project Autocompletion**: Triggered by `+` character
- **Context Autocompletion**: Triggered by `@` character  
- **Priority Autocompletion**: Triggered by `(` character
- **Popup-based Suggestions**: Display suggestions in a popup ListBox
- **Keyboard Navigation**: Arrow keys, Enter, Escape, Tab, Space
- **Mouse Selection**: Click to select suggestions
- **Text Filtering**: Filter suggestions based on typed text
- **Case Sensitivity**: Support both case-sensitive and case-insensitive filtering

## Files Involved

### Primary Files
- `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs` - Complete replacement
- `src/TodoTxt.Avalonia/Views/MainWindow.axaml` - Integration point

### Reference Files
- `src/TodoTxt.UI/Controls/IntellisenseTextBox.cs` - Original WPF implementation
- `src/TodoTxt.Avalonia/ViewModels/MainWindowViewModel.cs` - Data source

## Implementation Phases

### Phase 1: Basic Functionality (Steps 1.1-1.3)
- Create minimal TextBox inheritance
- Add required properties (TaskList, CaseSensitive)
- Add basic text change detection

### Phase 2: Popup System (Steps 2.1-2.3)
- Create and manage Popup control
- Add ListBox for suggestions
- Connect trigger detection to popup display

### Phase 3: Suggestion Display (Steps 3.1-3.3)
- Use real TaskList data
- Add basic selection functionality
- Add keyboard selection support

### Phase 4: Full Functionality (Steps 4.1-4.4)
- Add TextBox keyboard handling
- Implement text filtering
- Improve text insertion logic
- Add priority-specific positioning rules

### Phase 5: Integration and Polish (Steps 5.1-5.3)
- Full MainWindow integration
- Error handling and edge cases
- Code cleanup and documentation

## Key Challenges

### High Risk
- **Popup Positioning**: Avalonia doesn't have WPF's `PlacementRectangle`
- **Text Cursor Position**: No direct equivalent to `GetRectFromCharacterIndex`
- **Focus Management**: Complex focus handling between TextBox and Popup

### Medium Risk
- **Property Binding**: StyledProperty vs DependencyProperty differences
- **ListBox Filtering**: Different filtering mechanisms
- **Event Handling**: Different event models between WPF and Avalonia

## Success Criteria
- [ ] Component is visible and renders correctly
- [ ] All trigger types work (+/@/()
- [ ] Popup appears and disappears correctly
- [ ] Suggestions are displayed and selectable
- [ ] Text insertion works correctly
- [ ] Filtering works (case-sensitive and insensitive)
- [ ] Keyboard navigation works
- [ ] Mouse selection works
- [ ] Full integration with MainWindow
- [ ] Performance is acceptable

## Testing Strategy
- **Unit Testing**: Test each method individually
- **Integration Testing**: Test in MainWindow context
- **User Testing**: Test all interaction scenarios
- **Edge Case Testing**: Test error conditions and edge cases

## Rollback Plan
- Keep current working TextBox as fallback
- Document each step for potential rollback
- Test each step independently before proceeding

## Timeline Estimate
**Total Estimated Time**: 9-14 hours
- Phase 1: 1-2 hours
- Phase 2: 2-3 hours  
- Phase 3: 2-3 hours
- Phase 4: 3-4 hours
- Phase 5: 1-2 hours

## Next Steps
1. Review and approve the implementation plan
2. Begin Phase 1: Create minimal TextBox functionality
3. Test each step thoroughly before proceeding
4. Document progress and any issues encountered
5. Complete all phases incrementally

## Documentation
- `research.md` - Detailed analysis of current state and requirements
- `plan.md` - High-level implementation strategy and approach
- `implementation.md` - Detailed step-by-step implementation guide
- `README.md` - This overview document
