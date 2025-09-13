# IntellisenseTextBox Reimplementation Task

## Overview
This task involves reimplementing the IntellisenseTextBox component for Avalonia UI using a step-by-step, incremental approach. The current Avalonia implementation is not working (not showing up in the UI), so we need to rebuild it from scratch using the original WPF implementation as a reference.

## Problem Statement
- Current Avalonia IntellisenseTextBox is not visible/functional
- Regular TextBox works correctly in the same location
- Need to reimplement with full feature parity to WPF version
- Must use incremental approach to identify and resolve issues step-by-step

## Task Structure

### Documentation Files:
- **`research.md`** - Analysis of current issues and WPF vs Avalonia differences
- **`plan.md`** - High-level strategy, scope, risks, and success criteria
- **`implementation.md`** - Detailed step-by-step implementation plan
- **`references.md`** - Complete WPF implementation for reference
- **`README.md`** - This overview document

### Implementation Steps:
1. **Step 1**: Basic TextBox inheritance (no extra functionality)
2. **Step 2**: Add dependency properties (TaskList, CaseSensitive)
3. **Step 3**: Add basic popup display capability
4. **Step 4**: Implement text change detection and trigger recognition
5. **Step 5**: Add suggestion filtering and display
6. **Step 6**: Implement keyboard navigation
7. **Step 7**: Add mouse interaction
8. **Step 8**: Implement text insertion and cursor management
9. **Step 9**: Add proper styling and theming
10. **Step 10**: Add error handling and edge cases

## Key Features to Implement

### Autocompletion Triggers:
- `+` for projects (from TaskList.Projects)
- `@` for contexts (from TaskList.Contexts)
- `(` for priorities (A-Z, with date validation)

### User Interaction:
- Keyboard navigation (arrow keys, enter, escape, tab, space)
- Mouse selection
- Real-time filtering as user types
- Case-sensitive/insensitive filtering options

### Technical Requirements:
- Proper Avalonia StyledProperty usage
- Cross-platform compatibility
- Integration with existing MainWindow and ViewModel
- Error handling and graceful degradation

## Success Criteria

### Functional:
- [ ] Control appears and functions as TextBox
- [ ] Shows suggestions for all trigger characters
- [ ] Filters suggestions based on typed characters
- [ ] Keyboard and mouse navigation work
- [ ] Selected suggestions are properly inserted
- [ ] Integrates seamlessly with MainWindow

### Quality:
- [ ] No crashes or exceptions
- [ ] No performance issues
- [ ] Follows Avalonia best practices
- [ ] Maintains basic TextBox functionality
- [ ] Handles edge cases gracefully

## Timeline
- **Estimated Time**: 15-25 hours
- **Approach**: Incremental development with testing at each step
- **Risk Mitigation**: Keep working versions in version control

## Getting Started

1. **Review Documentation**: Read through all documentation files
2. **Understand Current State**: Analyze the current broken implementation
3. **Start with Step 1**: Begin with basic TextBox inheritance
4. **Test Each Step**: Verify functionality before proceeding
5. **Reference WPF Implementation**: Use original implementation as specification

## Files to Modify

### Primary:
- `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs` - Main implementation
- `src/TodoTxt.Avalonia/Views/MainWindow.axaml` - Integration point

### Reference:
- `src/TodoTxt.UI/Controls/IntellisenseTextBox.cs` - Original WPF implementation
- `src/TodoTxt.Avalonia/ViewModels/MainWindowViewModel.cs` - Data binding context

## Notes

- This is a **planning phase only** - no implementation should be done yet
- The incremental approach allows for early issue identification
- Each step should be testable in the actual application
- Keep the original WPF implementation as the functional specification
- Focus on getting basic functionality working before adding advanced features

## Next Steps

1. Get user approval for this plan
2. Begin implementation with Step 1
3. Test each step thoroughly
4. Document any issues or deviations from plan
5. Integrate working implementation into MainWindow
