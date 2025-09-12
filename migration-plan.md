# TodoTxt.Net Migration Plan: Modernizing to Cross-Platform with Avalonia UI

## Project Overview

This document outlines the comprehensive migration plan for modernizing the TodoTxt.Net project from a Windows-only WPF application to a cross-platform solution using Avalonia UI. The migration will maintain full compatibility with the todo.txt specification while providing a modern, maintainable codebase.

## Current Project Analysis

### Current Structure
- **Framework**: .NET Framework 4.0 Client Profile
- **UI Technology**: WPF (Windows Presentation Foundation)
- **Architecture**: Multi-project solution with clear separation of concerns
- **Projects**:
  - `ToDoLib`: Core business logic library
  - `Client`: WPF UI application
  - `ToDoTests`: Unit tests (NUnit 2.6.4)
  - `ColorFont`: UI controls for color/font selection
  - `CommonExtensions`: Extension methods
  - `Installer`: Windows installer setup

### Key Features to Preserve
- Full todo.txt specification compliance
- Keyboard-driven interface
- Task management (CRUD operations)
- File watching and auto-reload
- Sorting and filtering capabilities
- Project and context management
- Due date and priority handling
- Archive functionality

## Migration Strategy

### Phase 1: Repository Restructuring (Priority: High)

#### 1.1 Modern .NET Solution Structure
```
todotxt.net/
├── src/
│   ├── TodoTxt.Lib/                 # Core business logic (.NET 8)
│   ├── TodoTxt.Lib.Tests/           # Unit tests (.NET 8)
│   ├── TodoTxt.UI/                  # Avalonia UI application (.NET 8)
│   └── TodoTxt.Shared/              # Shared utilities and models
├── docs/                            # Documentation
├── scripts/                         # Build and deployment scripts
├── .github/
│   └── workflows/                   # CI/CD pipelines
├── global.json                      # .NET SDK version pinning
├── Directory.Build.props            # Common MSBuild properties
└── README.md
```

#### 1.2 Project File Modernization
- Convert from old-style `.csproj` to SDK-style projects
- Target .NET 8 for maximum cross-platform compatibility
- Implement proper dependency management with PackageReference
- Add nullable reference types support
- Configure code analysis and style rules

#### 1.3 Directory Structure Changes
- Move `ToDoLib` → `src/TodoTxt.Lib`
- Move `ToDoTests` → `src/TodoTxt.Lib.Tests`
- Create new `src/TodoTxt.UI` for Avalonia application
- Consolidate `CommonExtensions` into `TodoTxt.Shared`
- Archive `ColorFont` functionality (to be reimplemented in Avalonia)

### Phase 2: Library Modernization (Priority: High)

#### 2.1 ToDoLib Migration
- **Target Framework**: .NET 8
- **Dependencies**: Remove .NET Framework dependencies
- **Key Changes**:
  - Update `Task.cs` to use modern C# features
  - Implement `IEquatable<Task>` and proper `GetHashCode()`
  - Add async/await support for file operations
  - Implement `IDisposable` pattern for file handling
  - Add comprehensive XML documentation
  - Implement proper exception handling with custom exception types

#### 2.2 Cross-Platform Considerations
- Replace Windows-specific file operations with cross-platform alternatives
- Implement proper path handling using `Path` class
- Add support for different line ending formats (CRLF, LF)
- Ensure case-sensitive file system compatibility
- Add proper file locking mechanisms

#### 2.3 API Improvements
- Add fluent API for task creation
- Implement builder pattern for complex task operations
- Add validation attributes and methods
- Implement proper logging with Microsoft.Extensions.Logging
- Add configuration management support

### Phase 3: Test Modernization (Priority: High)

#### 3.1 Test Framework Migration
- **From**: NUnit 2.6.4
- **To**: NUnit latest
- **Additional Tools**:
  - FluentAssertions for readable assertions
  - Moq for mocking
  - Testcontainers for integration testing
  - Coverlet for code coverage

#### 3.2 Test Structure Improvements
- Implement proper test data builders
- Add integration tests for file operations
- Implement cross-platform test scenarios
- Add performance benchmarks
- Implement property-based testing for task parsing

#### 3.3 Test Coverage Goals
- Achieve 90%+ code coverage for core library
- Add comprehensive edge case testing
- Implement stress testing for large files
- Add concurrency testing for file operations

### Phase 4: Avalonia UI Implementation (Priority: Medium)

#### 4.1 UI Architecture
- **Pattern**: MVVM with ReactiveUI
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **State Management**: ReactiveUI observables
- **Navigation**: Avalonia NavigationView or custom solution

#### 4.2 Key UI Components
- **MainWindow**: Task list with filtering and sorting
- **TaskEditor**: Create/edit task dialog
- **SettingsWindow**: Application configuration
- **AboutDialog**: Application information
- **Custom Controls**:
  - TaskListView with virtualization
  - FilterTextBox with autocomplete
  - PrioritySelector
  - DatePicker for due dates

#### 4.3 Cross-Platform UI Considerations
- Implement responsive design for different screen sizes
- Ensure proper theming (light/dark mode support)
- Handle platform-specific UI patterns
- Implement proper accessibility support
- Add keyboard navigation support

#### 4.4 Feature Parity
- Maintain all existing keyboard shortcuts
- Implement tray icon functionality (platform-specific)
- Add file watching and auto-reload
- Implement drag-and-drop support
- Add context menus and toolbars

### Phase 5: CI/CD Pipeline Setup (Priority: Medium)

#### 5.1 GitHub Actions Workflow
```yaml
name: CI/CD Pipeline
on: [push, pull_request]
jobs:
  test:
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        dotnet-version: ['8.0.x']
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: ${{ matrix.dotnet-version }}
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
      - name: Code Coverage
        run: dotnet test --collect:"XPlat Code Coverage"
```

#### 5.2 Build and Release Pipeline
- **Build Artifacts**: Cross-platform executables
- **Packaging**: 
  - Windows: MSI installer
  - macOS: DMG package
  - Linux: AppImage and DEB/RPM packages
- **Distribution**: GitHub Releases with automatic updates
- **Quality Gates**: Code coverage, security scanning, dependency checking

#### 5.3 Additional CI Features
- Automated dependency updates (Dependabot)
- Code quality analysis (SonarCloud)
- Security vulnerability scanning
- Performance regression testing
- Cross-platform UI testing

### Phase 6: Documentation and Deployment (Priority: Low)

#### 6.1 Documentation Updates
- Update README with modern installation instructions
- Create user manual with screenshots
- Add developer documentation
- Create migration guide for existing users
- Document API for library consumers

#### 6.2 Distribution Strategy
- **Package Managers**: 
  - Windows: Chocolatey, Winget
  - macOS: Homebrew
  - Linux: Snap, Flatpak
- **Direct Downloads**: GitHub Releases
- **Auto-updates**: Implement update checking mechanism

## Implementation Timeline

### Week 1-2: Repository Restructuring
- [ ] Create new solution structure
- [ ] Migrate ToDoLib to .NET 8
- [ ] Update project files and dependencies
- [ ] Set up basic CI pipeline

### Week 3-4: Library and Tests
- [ ] Modernize ToDoLib with new C# features
- [ ] Migrate tests to xUnit
- [ ] Implement comprehensive test coverage
- [ ] Add cross-platform file handling

### Week 5-8: UI Development
- [ ] Set up Avalonia project structure
- [ ] Implement core UI components
- [ ] Add MVVM architecture with ReactiveUI
- [ ] Implement all existing features

### Week 9-10: CI/CD and Packaging
- [ ] Complete CI pipeline setup
- [ ] Implement cross-platform packaging
- [ ] Set up automated releases
- [ ] Add update mechanism

### Week 11-12: Testing and Polish
- [ ] Cross-platform testing
- [ ] Performance optimization
- [ ] Documentation updates
- [ ] User acceptance testing

## Risk Mitigation

### Technical Risks
- **Avalonia Learning Curve**: Allocate extra time for UI framework learning
- **Cross-Platform Compatibility**: Implement comprehensive testing on all platforms
- **Performance**: Use profiling tools to ensure acceptable performance
- **File System Differences**: Implement robust path handling and testing

### Project Risks
- **Scope Creep**: Maintain focus on core functionality first
- **Timeline Delays**: Build in buffer time for unexpected issues
- **User Adoption**: Provide clear migration path and documentation

## Success Criteria

### Technical Goals
- [ ] 100% feature parity with existing application
- [ ] Cross-platform compatibility (Windows, macOS, Linux)
- [ ] 90%+ test coverage
- [ ] Modern, maintainable codebase
- [ ] Automated CI/CD pipeline

### User Experience Goals
- [ ] Improved performance and responsiveness
- [ ] Modern, intuitive user interface
- [ ] Maintained keyboard-driven workflow
- [ ] Seamless migration from existing version

## Conclusion

This migration plan provides a comprehensive roadmap for modernizing TodoTxt.Net while maintaining its core functionality and user experience. The phased approach ensures that each component is properly modernized before moving to the next, reducing risk and maintaining project momentum.

The focus on cross-platform compatibility, modern development practices, and comprehensive testing will result in a robust, maintainable application that serves users across all major operating systems.
