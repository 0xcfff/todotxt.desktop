# IntellisenseTextBox Testing - Plan

## Objective
Create comprehensive test coverage for the IntellisenseTextBox component to ensure reliability, maintainability, and proper functionality of the autocompletion feature. Additionally, fix the TaskList architecture to support proper testing by removing automatic file I/O from the constructor.

## Scope

### Included
- **Fix TaskList Architecture**: Add parameterless constructor or modify existing constructor to not automatically load files
- Fix existing failing tests (14 tests currently failing)
- Add unit tests for all public methods and properties
- Add integration tests for TaskList interaction
- Add UI interaction tests for keyboard navigation and text insertion
- Add error handling and edge case tests
- Implement proper test infrastructure with in-memory TaskList creation

### Excluded
- Performance testing (can be added later)
- UI automation testing (beyond basic interaction)
- Cross-platform specific testing
- Accessibility testing

## Approach

### High-Level Strategy
1. **Fix Architecture First**: Address the root cause by fixing TaskList constructor to not automatically load files
2. **Build Foundation**: Establish proper test infrastructure with in-memory TaskList creation
3. **Comprehensive Coverage**: Add tests for all major functionality
4. **Quality Assurance**: Ensure tests are maintainable and reliable

### Testing Philosophy
- **Test Behavior, Not Implementation**: Focus on what the component does, not how it does it
- **Isolation**: Each test should be independent and not rely on other tests
- **Clarity**: Tests should be readable and serve as documentation
- **Reliability**: Tests should be stable and not flaky

## Components

### Files to Modify
1. **`src/TodoTxt.Lib/TaskList.cs`**
   - Add parameterless constructor or modify existing constructor
   - Remove automatic file loading from constructor
   - Make ReloadTasks() public and handle empty _filePath gracefully
   - Add LoadFromFile(string filePath) method for explicit file loading

2. **`src/TodoTxt.Avalonia.Tests/IntellisenseTextBoxUnitTests.cs`**
   - Fix existing test setup to use improved TaskList
   - Add new test methods
   - Improve test organization

3. **`src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj`**
   - Add any required test dependencies
   - Update test configuration if needed

### Files to Create
1. **Test Data Builders** (if needed)
   - Helper classes for creating test TaskList instances
   - Test data factories for consistent setup

2. **Test Utilities** (if needed)
   - Common test setup/teardown utilities
   - Mock objects for testing

## Dependencies

### External Libraries
- **NUnit**: Already included (v4.0.1)
- **Avalonia**: For UI control testing
- **System.IO**: For file management (when needed)

### Internal Dependencies
- **ToDoLib**: TaskList class for autocompletion data (needs architectural fix)
- **TodoTxt.Avalonia**: The component under test
- **TodoTxt.Shared**: Common extensions

### Test Infrastructure
- In-memory TaskList creation for most tests
- Temporary file creation and cleanup (for integration tests only)
- Test data setup and teardown
- Event simulation for UI testing

## Risks

### Technical Risks
1. **TaskList Architecture Changes**: Modifying TaskList constructor may break existing code
2. **UI Testing Complexity**: Avalonia controls may require proper initialization context
3. **Event Handling**: TextChanged and keyboard events are complex to test reliably
4. **Cross-Platform Issues**: File paths and line endings may vary between platforms

### Mitigation Strategies
1. **Backward Compatibility**: Ensure TaskList changes don't break existing functionality
2. **Test Isolation**: Ensure each test is independent and doesn't affect others
3. **In-Memory Testing**: Use in-memory TaskList creation for most unit tests
4. **Error Handling**: Test error scenarios to ensure graceful degradation

### Project Risks
1. **Time Investment**: Comprehensive testing requires significant time
2. **Maintenance Overhead**: More tests mean more maintenance
3. **Test Complexity**: UI tests can be brittle and hard to maintain

### Mitigation Strategies
1. **Phased Approach**: Implement tests in phases, starting with most critical
2. **Quality Focus**: Write maintainable tests that serve as documentation
3. **Selective Testing**: Focus on critical paths and user-facing functionality

## Success Criteria

### Phase 1: Foundation
- [ ] TaskList architecture fixed (parameterless constructor or modified constructor)
- [ ] All 14 existing tests pass
- [ ] Proper test infrastructure established with in-memory TaskList creation

### Phase 2: Core Functionality
- [ ] Popup management tests (ShowPopup, HidePopup)
- [ ] Autocompletion logic tests (ShowSuggestions, UpdateFiltering)
- [ ] Basic user interaction tests (keyboard navigation, text insertion)

### Phase 3: Comprehensive Coverage
- [ ] Error handling tests
- [ ] Edge case tests
- [ ] Integration tests with TaskList
- [ ] Case-sensitive filtering tests

### Phase 4: Quality Assurance
- [ ] Test documentation
- [ ] Test maintainability review
- [ ] Performance considerations
- [ ] Test coverage analysis

## Timeline Estimate
- **Phase 1**: 2-3 days (fix TaskList architecture and existing tests)
- **Phase 2**: 2-3 days (core functionality tests)
- **Phase 3**: 2-3 days (comprehensive coverage)
- **Phase 4**: 1 day (quality assurance)

**Total Estimated Time**: 7-10 days

## Assumptions
1. The IntellisenseTextBox component is stable and won't change significantly during testing
2. The TaskList class behavior is well-defined and consistent
3. Avalonia UI testing patterns are established and reliable
4. The existing test framework (NUnit) is sufficient for all testing needs
5. **TaskList architecture changes can be made without breaking existing functionality**
