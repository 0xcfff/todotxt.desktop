# IntellisenseTextBox Reimplementation Changes

## Files to be Modified

### 1. `src/TodoTxt.Avalonia.Core/Controls/IntellisenseTextBox.cs`
**Action**: Complete replacement
**Reason**: Current implementation is completely non-functional
**Changes**:
- Replace entire file content with new implementation
- Follow Avalonia patterns instead of WPF patterns
- Implement step-by-step as outlined in implementation.md

### 2. `src/TodoTxt.Avalonia/Views/MainWindow.axaml`
**Action**: Uncomment IntellisenseTextBox and comment out regular TextBox
**Reason**: Enable the new IntellisenseTextBox functionality
**Changes**:
- Uncomment lines 77-87 (IntellisenseTextBox)
- Comment out lines 89-97 (regular TextBox)
- Ensure proper data binding

### 3. `src/TodoTxt.Avalonia/Views/MainWindow.axaml.cs`
**Action**: Verify event handlers work with new implementation
**Reason**: Ensure integration works correctly
**Changes**:
- Test `TaskTextBox_KeyUp` event handler
- Verify no conflicts with new implementation
- Update if necessary

## Implementation Steps

### Phase 1: Basic Functionality
- [ ] **Step 1.1**: Create minimal TextBox inheritance
  - Replace entire IntellisenseTextBox.cs with basic TextBox
  - Test visibility and basic functionality
  
- [ ] **Step 1.2**: Add basic properties
  - Add TaskList and CaseSensitive properties using StyledProperty
  - Test property binding
  
- [ ] **Step 1.3**: Add text change detection
  - Add TextChanged event handler
  - Add basic trigger character detection
  - Test trigger detection

### Phase 2: Popup System
- [ ] **Step 2.1**: Create basic popup
  - Add Popup field and initialization
  - Add ShowPopup/HidePopup methods
  - Test popup visibility
  
- [ ] **Step 2.2**: Add ListBox to popup
  - Add ListBox field and initialization
  - Set ListBox as Popup child
  - Test ListBox in popup
  
- [ ] **Step 2.3**: Connect trigger detection to popup
  - Modify TextChanged handler to show popup
  - Add test data for popup
  - Test popup appears on trigger characters

### Phase 3: Suggestion Display
- [ ] **Step 3.1**: Add real data sources
  - Use TaskList data instead of test data
  - Handle different trigger types
  - Test with real data
  
- [ ] **Step 3.2**: Add basic selection
  - Add SelectionChanged event handler
  - Add text insertion logic
  - Test selection and insertion
  
- [ ] **Step 3.3**: Add keyboard selection
  - Add KeyDown event handler to ListBox
  - Handle Enter and Escape keys
  - Test keyboard navigation

### Phase 4: Full Functionality
- [ ] **Step 4.1**: Add TextBox keyboard handling
  - Override OnKeyUp method
  - Handle Down arrow, Escape, Space, Enter
  - Test keyboard control
  
- [ ] **Step 4.2**: Add text filtering
  - Track trigger position
  - Filter suggestions based on typed text
  - Test filtering functionality
  
- [ ] **Step 4.3**: Improve text insertion
  - Replace only typed portion
  - Handle cursor positioning
  - Test insertion accuracy
  
- [ ] **Step 4.4**: Add priority-specific logic
  - Handle priority positioning rules
  - Test priority autocompletion
  - Test date detection

### Phase 5: Integration and Polish
- [ ] **Step 5.1**: Full integration
  - Uncomment IntellisenseTextBox in MainWindow.axaml
  - Comment out regular TextBox
  - Test full integration
  
- [ ] **Step 5.2**: Error handling and edge cases
  - Add null checks and error handling
  - Handle edge cases
  - Test error conditions
  
- [ ] **Step 5.3**: Code cleanup and documentation
  - Remove debug statements
  - Add XML documentation
  - Clean up code formatting

## Testing Strategy

### After Each Step
- [ ] Test the specific functionality added in that step
- [ ] Verify no regression in existing functionality
- [ ] Test integration with MainWindow
- [ ] Document any issues encountered

### Integration Testing
- [ ] Test with real TaskList data
- [ ] Test all trigger types (+/@/()
- [ ] Test keyboard navigation
- [ ] Test mouse selection
- [ ] Test filtering functionality
- [ ] Test edge cases and error conditions

### User Testing
- [ ] Test in MainWindow context
- [ ] Test with various TaskList configurations
- [ ] Test performance and responsiveness
- [ ] Test accessibility features

## Rollback Plan

### If Any Step Fails
1. **Document the Issue**: Record what didn't work and why
2. **Revert Changes**: Go back to previous working step
3. **Analyze Problem**: Understand the root cause
4. **Adjust Approach**: Modify the step or break it down further
5. **Retry**: Attempt the step again with modifications

### Emergency Rollback
- Keep the current working TextBox as fallback
- Comment out IntellisenseTextBox in MainWindow.axaml
- Restore previous working state
- Document the failure for future reference

## Success Criteria

### Functional Requirements
- [ ] Component is visible and renders correctly
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

## Risk Mitigation

### High Risk Items
- **Popup Positioning**: Start with simple positioning, improve later
- **Text Cursor Position**: Implement basic positioning, enhance incrementally
- **Focus Management**: Start with simple focus handling, add complexity gradually
- **Event Handling**: Test each event type individually

### Medium Risk Items
- **Property Binding**: Follow Avalonia patterns exactly
- **ListBox Filtering**: Use LINQ-based filtering approach
- **Integration**: Test integration at each step

### Low Risk Items
- **Basic Inheritance**: TextBox inheritance should be straightforward
- **Trigger Detection**: Character detection logic is simple
- **Data Access**: TaskList integration is well-defined

## Documentation Updates

### Code Documentation
- [ ] Add XML documentation to all public methods
- [ ] Add inline comments for complex logic
- [ ] Document any Avalonia-specific patterns used

### User Documentation
- [ ] Update any user-facing documentation
- [ ] Document any new features or changes
- [ ] Update help text if needed

### Developer Documentation
- [ ] Document the implementation approach
- [ ] Document any lessons learned
- [ ] Document any Avalonia-specific considerations
- [ ] Update this changes document with actual changes made
