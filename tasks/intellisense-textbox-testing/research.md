# IntellisenseTextBox Testing - Research

## Component Analysis

### IntellisenseTextBox Overview
The `IntellisenseTextBox` is a custom Avalonia UI control that extends the standard `TextBox` to provide autocompletion functionality for todo.txt projects, contexts, and priorities.

**Location**: `src/TodoTxt.Avalonia/Controls/IntellisenseTextBox.cs`

### Key Features
1. **Autocompletion Triggers**: 
   - `+` for projects
   - `@` for contexts  
   - `(` for priorities

2. **Popup Management**:
   - Shows/hides autocompletion popup
   - Keyboard navigation (Up/Down arrows)
   - Enter key selection

3. **Filtering**:
   - Case-sensitive/insensitive filtering
   - Real-time filtering as user types

4. **Priority Validation**:
   - Only allows priorities at start of line or after date (YYYY-MM-DD )

### Dependencies
- **Avalonia UI Framework**: Uses `TextBox`, `Popup`, `ListBox` controls
- **ToDoLib**: Depends on `TaskList` for autocompletion data
- **System.Text.RegularExpressions**: For date validation in priority positioning

## Current Test Analysis

### Existing Test File
**Location**: `src/TodoTxt.Avalonia.Tests/IntellisenseTextBoxUnitTests.cs`

### Test Coverage Status
**Total Tests**: 14 tests
**Status**: ❌ All tests failing due to TaskList file dependency issue

### Current Test Categories
1. **Property Tests** (4 tests):
   - Default initialization
   - TaskList property setting
   - CaseSensitive property setting
   - Null/empty TaskList handling

2. **Data Extraction Tests** (4 tests):
   - Project extraction from TaskList
   - Context extraction from TaskList
   - Priority extraction from TaskList
   - Multiple projects/contexts handling

3. **Basic UI Tests** (6 tests):
   - Text input handling
   - Caret position handling
   - Trigger character detection

### Root Cause of Test Failures
The `TaskList` constructor requires a real file path and attempts to read from it during construction:
```csharp
var _taskList = new TaskList("test.txt", false); // File doesn't exist
```

This causes `FileNotFoundException` in the `TaskList.ReloadTasks()` method.

**Architectural Issue**: The TaskList constructor calls `ReloadTasks()` immediately, which violates the principle of separation of concerns. The constructor should not perform I/O operations, making the class difficult to test and use in scenarios where file I/O is not desired.

## Architecture Patterns

### Test Infrastructure Patterns
From `src/TodoTxt.Lib.Tests/TaskListTests.cs`:
- Uses `Data.TestDataPath` resource for test file path
- Creates temporary test files in `[OneTimeSetUp]`
- Cleans up files in `[OneTimeTearDown]`
- Uses `File.WriteAllText()` and `File.Delete()` for file management

### Dependency Management
The IntellisenseTextBox has a strong dependency on `TaskList` which requires file I/O. This creates challenges for unit testing.

## Missing Test Coverage Analysis

### Critical Missing Areas
1. **Popup Functionality** (0% coverage):
   - `ShowPopup()` method
   - `HidePopup()` method
   - Popup visibility states
   - Popup positioning

2. **Autocompletion Logic** (0% coverage):
   - `ShowSuggestions()` method
   - `UpdateFiltering()` method
   - Case-sensitive filtering
   - Priority position validation

3. **User Interaction** (0% coverage):
   - Keyboard navigation
   - Text insertion
   - Focus handling
   - Event handling

4. **Error Handling** (0% coverage):
   - Exception handling in event handlers
   - Invalid input scenarios
   - Edge cases

### Test Complexity Levels
1. **Simple Unit Tests**: Property getters/setters, basic initialization
2. **Integration Tests**: TaskList interaction, file I/O
3. **UI Tests**: User interaction, keyboard events, focus management
4. **End-to-End Tests**: Complete autocompletion workflows

## Technical Constraints

### Avalonia UI Testing
- Avalonia controls require proper initialization
- UI tests may need test application context
- Some UI interactions are difficult to test without actual rendering

### File System Dependencies
- TaskList requires real file paths
- Test files need proper cleanup
- Cross-platform file path considerations

### Event Handling
- TextChanged events are complex to test
- Keyboard events require proper event simulation
- Focus events need UI context

## Recommendations

### Immediate Actions
1. **Fix TaskList Architecture**: 
   - Add parameterless constructor for in-memory creation
   - Modify existing constructor to not automatically load files
   - Make ReloadTasks() public and handle empty _filePath gracefully
   - Add LoadFromFile(string filePath) method for explicit file loading
2. Get existing 14 tests passing with improved TaskList design
3. Add proper test cleanup

### Testing Strategy
1. **Unit Tests**: Test individual methods in isolation
2. **Integration Tests**: Test TaskList interaction with proper file management
3. **UI Tests**: Test user interactions and events
4. **Mocking**: Consider mocking TaskList for pure unit tests

### Test Data Management
- Use temporary files for TaskList tests when file I/O is needed
- Create test data builders for consistent setup
- Implement proper cleanup in teardown methods
- **Prefer in-memory TaskList creation** for most unit tests
