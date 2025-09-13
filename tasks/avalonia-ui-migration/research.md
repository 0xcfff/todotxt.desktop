# WPF to Avalonia UI Migration Research

## Current WPF Architecture Analysis

### Project Structure
The current TodoTxt.UI project is a WPF application with the following key components:

#### Core Projects
- **TodoTxt.UI** (`src/TodoTxt.UI/Client.csproj`): Main WPF application
  - Target Framework: `net9.0-windows`
  - Uses WPF: `true`
  - Output Type: `WinExe`
  - Platform: `x86` (limited to Windows)

- **TodoTxt.Lib** (`src/TodoTxt.Lib/ToDoLib.csproj`): Business logic
  - Target Framework: `net9.0` (cross-platform compatible)
  - Contains `Task`, `TaskList`, `TaskException`, `Log` classes
  - No platform-specific dependencies

- **TodoTxt.Shared** (`src/TodoTxt.Shared/CommonExtensions.csproj`): Utility code
  - Target Framework: `net9.0` (cross-platform compatible)
  - Extension methods for common operations

- **ColorFont** (`src/TodoTxt.UI/ColorFont/ColorFont.csproj`): Font/Color selection
  - Target Framework: `net9.0-windows`
  - Uses WPF: `true`
  - Custom font and color picker components

### Key WPF Components Requiring Migration

#### 1. Main Window (`MainWindow.xaml` + `MainWindow.xaml.cs`)
- Complex XAML layout with Grid, Menu, ListBox, StatusBar
- Extensive command bindings (40+ commands)
- Key bindings for all major operations
- Custom styles and triggers
- Features:
  - Menu system (File, Edit, Task, Sort, Filter, Help)
  - Task input textbox with intellisense
  - Task list with grouping and filtering
  - Status bar with statistics
  - Print preview functionality

#### 2. Custom Controls
- **IntellisenseTextBox**: Custom TextBox with popup-based autocompletion
  - Dependency properties for TaskList and CaseSensitive
  - Popup with ListBox for suggestions
  - Regex-based priority detection
  - Project (@) and context (+) completion

#### 3. Dialog Windows (13 XAML files)
- Options dialog with extensive settings
- Filter dialog for task filtering
- Date picker dialogs (Due Date, Threshold Date)
- Priority selection dialog
- Text append dialog
- Delete confirmation dialog
- Help dialog
- Postpone dialog

#### 4. ColorFont Component
- Font selection with preview
- Color picker functionality
- Custom XAML templates
- Font weight, style, size selection

#### 5. MVVM Architecture
- **MainWindowViewModel**: Primary view model
  - Implements `INotifyPropertyChanged`
  - Complex sorting and filtering logic
  - File change observation
  - Task manipulation methods
  - Print functionality

#### 6. WPF-Specific Features
- **Printing**: WebBrowser control for print preview
- **System Tray**: Windows-specific tray integration
- **File System Watching**: File change notifications
- **Settings**: Windows-specific settings storage
- **Hot Keys**: Global hotkey registration (Windows-specific)

### Dependencies
- **Microsoft.mshtml** (7.0.3300): For print functionality
- **System.Windows**: Core WPF framework
- **System.Windows.Forms**: For some dialogs
- File system APIs
- Registry access (Windows-specific settings)

### Current Challenges for Cross-Platform
1. **WPF Dependency**: Entire UI is WPF-based (Windows only)
2. **Windows-Specific APIs**: Tray, hotkeys, registry
3. **Print System**: Uses WebBrowser control
4. **File Dialogs**: WPF-specific dialogs
5. **Font System**: WPF font rendering and selection
6. **Platform Target**: Currently x86 Windows only

## Avalonia UI Research

### Avalonia Capabilities
- **Cross-platform**: Windows, macOS, Linux support
- **XAML-based**: Similar to WPF XAML syntax
- **MVVM Support**: Full data binding and commanding
- **Styling**: CSS-like styling system
- **Custom Controls**: Full support for custom controls
- **Dialogs**: Cross-platform dialog system

### Migration Considerations
- **XAML Differences**: Some WPF XAML features not directly supported
- **Control Library**: Different set of built-in controls
- **Styling System**: Different approach than WPF styles
- **Platform APIs**: Different APIs for platform-specific features
- **Dependencies**: Need Avalonia-specific packages

### Avalonia Equivalent Components
- **Window**: `Avalonia.Controls.Window`
- **Menu**: `Avalonia.Controls.Menu`
- **ListBox**: `Avalonia.Controls.ListBox`
- **TextBox**: `Avalonia.Controls.TextBox`
- **Popup**: `Avalonia.Controls.Primitives.Popup`
- **Grid/StackPanel**: Same concepts, different namespace
- **Data Binding**: Similar syntax with some differences

## Key Migration Challenges

### High Complexity
1. **Custom IntellisenseTextBox**: Requires complete rewrite for Avalonia
2. **Printing System**: Need alternative to WebBrowser control
3. **System Tray**: Platform-specific implementations needed
4. **Complex XAML**: Extensive menu system and styling
5. **File Dialogs**: Different dialog system in Avalonia

### Medium Complexity
1. **MVVM Patterns**: Largely transferable with minor adjustments
2. **Data Binding**: Similar concepts, some syntax differences
3. **Command Bindings**: Need to adapt to Avalonia command system
4. **Settings Storage**: Cross-platform settings management
5. **Font System**: Different font handling in Avalonia

### Low Complexity
1. **Business Logic**: TodoTxt.Lib is already cross-platform
2. **Utility Code**: TodoTxt.Shared is cross-platform
3. **Basic Controls**: Most standard controls have Avalonia equivalents
4. **Layout System**: Grid/StackPanel concepts are similar

## Recommended Migration Strategy

### Phase 1: Foundation (Mac-focused)
1. Create new Avalonia project structure
2. Migrate core business logic integration
3. Implement basic main window layout
4. Basic task display and input

### Phase 2: Core Functionality
1. Implement task manipulation (add, edit, delete, toggle)
2. Basic filtering and sorting
3. File operations (open, save, reload)
4. Settings system (cross-platform)

### Phase 3: Advanced Features
1. Custom IntellisenseTextBox equivalent
2. All dialog windows
3. Advanced filtering and grouping
4. Keyboard shortcuts

### Phase 4: Platform-Specific Features
1. System tray integration (per platform)
2. Printing functionality
3. Hot keys (platform-specific)
4. File associations

### Phase 5: Polish and Testing
1. Styling and theming
2. Performance optimization
3. Cross-platform testing
4. Documentation updates

## File Change Impact Assessment

### Files to Migrate (High Impact)
- All XAML files (17 files) - Complete rewrite required
- All .xaml.cs code-behind files - Significant changes needed
- MainWindowViewModel.cs - Some adaptations needed
- IntellisenseTextBox.cs - Complete rewrite required

### Files to Adapt (Medium Impact)
- Settings and configuration classes
- File system integration code
- Print-related functionality

### Files Unchanged (Low Impact)
- TodoTxt.Lib project (business logic)
- TodoTxt.Shared project (utilities)
- Test files (may need minor updates)

## Success Criteria
1. **Feature Parity**: All current features working on Mac
2. **Cross-Platform**: Working on Windows, Mac, Linux
3. **Performance**: Similar or better performance than WPF version
4. **Maintainability**: Clean, maintainable codebase
5. **Testing**: Comprehensive test coverage
6. **Documentation**: Updated for cross-platform deployment