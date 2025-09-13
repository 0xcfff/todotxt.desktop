# IntellisenseTextBox Reimplementation Plan

## Objective
Reimplement the IntellisenseTextBox component for Avalonia UI by creating a step-by-step, incremental implementation that starts from a basic TextBox and gradually adds functionality. This approach will help identify and resolve issues at each step rather than attempting a complete rewrite.

## Scope

### Included:
- Complete reimplementation of IntellisenseTextBox for Avalonia
- Step-by-step incremental development
- Full feature parity with original WPF implementation
- Proper integration with existing MainWindow and ViewModel
- Comprehensive testing at each step

### Excluded:
- Changes to business logic (TaskList, Task classes)
- Changes to ViewModel (MainWindowViewModel)
- Changes to other UI components
- Performance optimizations (initial implementation focus)

## Approach

### High-Level Strategy
1. **Incremental Development**: Build functionality step-by-step, testing at each stage
2. **Reference Implementation**: Use original WPF implementation as functional specification
3. **Avalonia Best Practices**: Follow Avalonia-specific patterns and APIs
4. **Fail-Safe Design**: Each step should maintain basic TextBox functionality
5. **Comprehensive Testing**: Verify functionality in actual application context

### Development Methodology
- **Step 1**: Basic TextBox inheritance with no extra functionality
- **Step 2**: Add dependency properties (TaskList, CaseSensitive)
- **Step 3**: Add basic popup display capability
- **Step 4**: Implement text change detection and trigger recognition
- **Step 5**: Add suggestion filtering and display
- **Step 6**: Implement keyboard navigation
- **Step 7**: Add mouse interaction
- **Step 8**: Implement text insertion and cursor management
- **Step 9**: Add proper styling and theming
- **Step 10**: Add error handling and edge cases

## Components

### Files to Modify:
1. `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs` - Main implementation
2. `src/TodoTxt.Avalonia/Views/MainWindow.axaml` - Integration point
3. `src/TodoTxt.Avalonia/Views/MainWindow.axaml.cs` - Event handling (if needed)

### Files to Reference:
1. `src/TodoTxt.UI/Controls/IntellisenseTextBox.cs` - Original WPF implementation
2. `src/TodoTxt.Avalonia/ViewModels/MainWindowViewModel.cs` - Data binding context
3. `src/TodoTxt.Lib/TaskList.cs` - Data source for autocompletion

### New Files (if needed):
- Test files for each implementation step
- Documentation for each step's functionality

## Dependencies

### External Libraries:
- Avalonia UI framework (already included)
- ToDoLib (business logic, already included)
- System.Text.RegularExpressions (for priority detection)

### Internal Dependencies:
- MainWindowViewModel.TaskList property
- MainWindow event handling
- Application styling/theme system

### Platform Considerations:
- Cross-platform compatibility (Avalonia advantage over WPF)
- Consistent behavior across Windows, macOS, Linux
- Proper popup positioning on different platforms

## Risks

### High Risk:
1. **Avalonia API Differences**: Popup and ListBox behavior may differ significantly from WPF
2. **Event System Complexity**: Avalonia event handling may require different patterns
3. **Styling Integration**: Hard to integrate with existing application theme
4. **Performance Issues**: Popup positioning and filtering may be slower than expected

### Medium Risk:
1. **Data Binding Issues**: StyledProperty implementation may have subtle differences
2. **Focus Management**: TextBox focus behavior may differ from WPF
3. **Keyboard Navigation**: Arrow key handling may need platform-specific adjustments

### Low Risk:
1. **Business Logic Integration**: TaskList and filtering logic should work unchanged
2. **Basic TextBox Functionality**: Core text editing should work identically
3. **ViewModel Integration**: Data binding should be straightforward

## Success Criteria

### Functional Requirements:
1. **Basic Functionality**: Control appears and functions as a TextBox
2. **Autocompletion**: Shows suggestions for + (projects), @ (contexts), ( (priorities)
3. **Filtering**: Filters suggestions based on typed characters
4. **Navigation**: Keyboard and mouse navigation work correctly
5. **Text Insertion**: Selected suggestions are properly inserted
6. **Integration**: Works seamlessly with MainWindow and ViewModel

### Non-Functional Requirements:
1. **Performance**: No noticeable lag in popup display or filtering
2. **Usability**: Behavior matches user expectations from WPF version
3. **Reliability**: No crashes or exceptions during normal use
4. **Maintainability**: Code is clear and follows Avalonia best practices

## Rollback Strategy

### At Each Step:
- Keep working version in version control
- Maintain ability to revert to previous step
- Document any breaking changes
- Test rollback procedure

### Complete Rollback:
- Revert to current regular TextBox implementation
- Remove IntellisenseTextBox references
- Restore original MainWindow.axaml
- Ensure application still functions correctly

## Timeline Estimate

### Step-by-Step Breakdown:
- **Step 1-2**: 1-2 hours (basic inheritance and properties)
- **Step 3-4**: 2-3 hours (popup and text detection)
- **Step 5-6**: 3-4 hours (filtering and keyboard navigation)
- **Step 7-8**: 2-3 hours (mouse interaction and text insertion)
- **Step 9-10**: 2-3 hours (styling and error handling)

### Total Estimated Time: 10-15 hours
### Buffer for Issues: 5-10 hours
### **Total Project Time: 15-25 hours**

## Next Steps

1. **User Approval**: Get approval for this plan and approach
2. **Environment Setup**: Ensure development environment is ready
3. **Step 1 Implementation**: Begin with basic TextBox inheritance
4. **Continuous Testing**: Test each step in actual application
5. **Documentation**: Document each step's implementation and issues
6. **Integration**: Integrate working implementation into MainWindow
