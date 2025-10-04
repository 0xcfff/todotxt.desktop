# Cross-Platform Testing Guide

This document outlines the testing strategy and procedures for validating the TodoTxt.Desktop Avalonia application across Windows, macOS, and Linux platforms.

## Overview

The TodoTxt.Desktop application has been migrated from WPF to Avalonia UI to enable cross-platform support. This testing guide ensures that all functionality works correctly across all target platforms.

## Testing Environments

### Automated Testing (GitHub Actions)

The project uses GitHub Actions for automated cross-platform testing:

- **Windows**: `windows-latest` runner
- **macOS**: `macos-latest` runner  
- **Linux**: `ubuntu-latest` runner

**Configuration Matrix:**
- Debug and Release configurations
- All three platforms tested simultaneously
- Code coverage collection and reporting

### Local Testing

#### macOS Testing
```bash
# Run the cross-platform test script
./scripts/cross-platform-test.sh

# Or run individual test commands
dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj
dotnet test src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj
dotnet build src/TodoTxt.Avalonia/TodoTxt.Avalonia.csproj
```

#### Windows Testing
```cmd
REM Run the cross-platform test script
scripts\cross-platform-test.bat

REM Or run individual test commands
dotnet test src\TodoTxt.Lib.Tests\ToDoTests.csproj
dotnet test src\TodoTxt.Avalonia.Tests\TodoTxt.Avalonia.Tests.csproj
dotnet build src\TodoTxt.Avalonia\TodoTxt.Avalonia.csproj
```

#### Linux Testing
```bash
# Run the cross-platform test script
./scripts/cross-platform-test.sh

# Or run individual test commands
dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj
dotnet test src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj
dotnet build src/TodoTxt.Avalonia/TodoTxt.Avalonia.csproj
```

## Test Categories

### 1. Core Library Tests
- **Project**: `src/TodoTxt.Lib.Tests/ToDoTests.csproj`
- **Purpose**: Test business logic and data handling
- **Expected Result**: All tests should pass
- **Platform Independence**: These tests should behave identically on all platforms

### 2. Avalonia UI Tests
- **Project**: `src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj`
- **Purpose**: Test UI components and Avalonia-specific functionality
- **Expected Result**: Most tests should pass, some may fail during development
- **Platform Considerations**: UI behavior may vary slightly between platforms

### 3. Application Build Tests
- **Project**: `src/TodoTxt.Avalonia/TodoTxt.Avalonia.csproj`
- **Purpose**: Verify the application compiles and builds successfully
- **Expected Result**: Build should succeed on all platforms
- **Platform Considerations**: Platform-specific dependencies must be resolved

### 4. Platform-Specific Feature Tests

#### System Tray Integration
- **Windows**: Test Windows system tray functionality
- **macOS**: Test macOS menu bar integration
- **Linux**: Test Linux system tray functionality

#### File Dialog System
- **All Platforms**: Test native file dialogs
- **Verification**: File selection, filtering, and platform-specific behavior

#### Printing Functionality
- **All Platforms**: Test print preview and printing
- **Verification**: Print dialog appearance and functionality

#### Global Hotkeys
- **Windows**: Test global hotkey registration
- **macOS**: Test global hotkey registration
- **Linux**: Test global hotkey registration (if supported)

## Test Execution

### Prerequisites
- .NET 9.0 SDK installed
- Platform-specific dependencies resolved
- Test data files available

### Running Tests

#### Full Test Suite
```bash
# Run all tests
./test.sh

# Or manually
dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj
dotnet test src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj
```

#### Individual Test Projects
```bash
# Core library tests only
dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj

# Avalonia tests only
dotnet test src/TodoTxt.Avalonia.Tests/TodoTxt.Avalonia.Tests.csproj

# With coverage
dotnet test src/TodoTxt.Lib.Tests/ToDoTests.csproj --collect:"XPlat Code Coverage"
```

#### Build Verification
```bash
# Debug build
dotnet build src/TodoTxt.Avalonia/TodoTxt.Avalonia.csproj --configuration Debug

# Release build
dotnet build src/TodoTxt.Avalonia/TodoTxt.Avalonia.csproj --configuration Release
```

## Expected Results

### Core Library Tests
- **Status**: All tests must pass
- **Coverage**: 100% of business logic should be tested
- **Performance**: Tests should complete within reasonable time

### Avalonia UI Tests
- **Status**: Most tests should pass
- **Known Issues**: Some tests may fail during development phase
- **Coverage**: UI components and interactions should be tested

### Application Build
- **Status**: Must succeed on all platforms
- **Output**: Executable should be generated
- **Dependencies**: All platform-specific dependencies resolved

## Troubleshooting

### Common Issues

#### Build Failures
- **Cause**: Missing platform-specific dependencies
- **Solution**: Install required packages for target platform
- **Verification**: Check project references and package dependencies

#### Test Failures
- **Core Library**: Investigate business logic issues
- **Avalonia Tests**: Check UI component implementation
- **Platform-Specific**: Verify platform service implementations

#### Runtime Issues
- **File Access**: Check file permissions and paths
- **Platform Services**: Verify platform-specific service implementations
- **Dependencies**: Ensure all required libraries are available

### Platform-Specific Considerations

#### Windows
- System tray integration requires Windows-specific APIs
- File dialogs use Windows native dialogs
- Global hotkeys require Windows message handling

#### macOS
- Menu bar integration uses macOS-specific APIs
- File dialogs use macOS native dialogs
- Global hotkeys require macOS event handling

#### Linux
- System tray integration varies by desktop environment
- File dialogs use GTK or Qt dialogs
- Global hotkeys may have limited support

## Reporting

### Test Reports
- **Location**: `reports/` directory
- **Format**: Text files with test results
- **Content**: Platform, configuration, timestamp, and results

### Coverage Reports
- **Location**: `coverage/` directory
- **Format**: Cobertura XML format
- **Usage**: Upload to code coverage services

### GitHub Actions
- **Status**: Available in Actions tab
- **Artifacts**: Build outputs and test results
- **Notifications**: Email notifications for failures

## Continuous Integration

### GitHub Actions Workflow
- **File**: `.github/workflows/dotnet-desktop.yml`
- **Triggers**: Push to main/feature branches, pull requests
- **Matrix**: Windows, macOS, Linux × Debug, Release
- **Artifacts**: Test results, coverage reports, build outputs

### Quality Gates
- All core library tests must pass
- Application must build on all platforms
- Code coverage must meet minimum thresholds
- No critical security vulnerabilities

## Manual Testing

### User Interface Testing
1. **Window Management**: Minimize, maximize, close
2. **Menu System**: All menu items functional
3. **Task Operations**: Add, edit, delete, complete tasks
4. **File Operations**: Open, save, reload files
5. **Dialog System**: All dialogs open and function correctly

### Platform Integration Testing
1. **System Tray**: Minimize to tray, restore from tray
2. **File Dialogs**: Native file selection
3. **Printing**: Print preview and actual printing
4. **Hotkeys**: Global hotkey functionality
5. **Settings**: Cross-platform settings persistence

### Performance Testing
1. **Startup Time**: Application launch performance
2. **File Loading**: Large file handling
3. **UI Responsiveness**: Task list scrolling and filtering
4. **Memory Usage**: Memory consumption monitoring

## Success Criteria

### Phase 5.1: Cross-Platform Testing
- [x] Set up Windows testing environment (GitHub Actions)
- [x] Set up Linux testing environment (GitHub Actions)
- [x] Set up macOS testing environment (GitHub Actions)
- [ ] Test all functionality on Windows (automated)
- [ ] Test all functionality on Linux (automated)
- [ ] Test all functionality on macOS (automated)
- [ ] Document platform-specific differences
- [ ] Fix platform-specific bugs

### Quality Metrics
- **Test Coverage**: >80% for core library, >60% for UI components
- **Build Success**: 100% success rate across all platforms
- **Test Pass Rate**: >95% for core library, >80% for UI tests
- **Performance**: Startup time <3 seconds, file loading <1 second for typical files

## Next Steps

1. **Automated Testing**: Ensure GitHub Actions workflow is working
2. **Manual Testing**: Perform comprehensive manual testing on each platform
3. **Bug Fixes**: Address any platform-specific issues found
4. **Documentation**: Update user documentation with platform-specific notes
5. **Deployment**: Prepare cross-platform deployment packages
