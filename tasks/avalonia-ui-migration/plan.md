# Avalonia UI Migration Plan

## Objective
Migrate the TodoTxt.Desktop Client project from WPF to Avalonia UI to enable cross-platform support for Windows, macOS, and Linux, with initial focus on macOS development and testing capability.

## Scope

### Included
- Complete UI framework migration from WPF to Avalonia UI
- Cross-platform compatibility (Windows, macOS, Linux)
- All existing functionality preservation
- MVVM pattern maintenance
- Settings and configuration management
- File operations and task management
- Dialog systems and custom controls
- Keyboard shortcuts and command system

### Excluded
- Business logic changes (TodoTxt.Lib remains unchanged)
- File format or data structure changes
- Major feature additions during migration
- UI/UX design overhauls (maintaining current design)

## Approach

### Development Strategy
1. **Mac-First Development**: Start with macOS as the primary development platform since it can be built and tested on current machine
2. **Incremental Migration**: Phased approach to minimize risk and enable testing at each stage
3. **Parallel Development**: Keep WPF version functional during migration
4. **Feature Parity**: Ensure all existing features work before adding new ones

### Technical Approach
1. **New Project Structure**: Create separate Avalonia project alongside existing WPF
2. **Shared Business Logic**: Reuse existing TodoTxt.Lib and TodoTxt.Shared projects
3. **XAML Conversion**: Convert WPF XAML to Avalonia XAML with necessary adaptations
4. **Custom Control Recreation**: Rebuild custom controls using Avalonia patterns
5. **Platform Abstraction**: Create interfaces for platform-specific functionality

## Components

### New Projects to Create
1. **TodoTxt.Avalonia** - Main Avalonia application project
2. **TodoTxt.Avalonia.Core** - Shared Avalonia-specific utilities and controls
3. **TodoTxt.Platform** - Platform-specific implementations (tray, file dialogs, etc.)

### Files/Classes to Modify
1. **Solution Structure**: Update ToDo.Net.sln to include new projects
2. **Build Configuration**: Add Avalonia build targets
3. **Settings System**: Create cross-platform settings management
4. **Platform Services**: Abstract platform-specific functionality

### External Dependencies
1. **Avalonia UI Package**: Core Avalonia framework
2. **Avalonia.Desktop**: Desktop-specific features
3. **Avalonia.Diagnostics**: Development debugging tools
4. **Platform-Specific Packages**: For system tray, notifications, etc.

## Risks

### High Risk
1. **Custom Control Complexity**: IntellisenseTextBox requires complete rewrite
2. **Print Functionality**: Complex printing system needs alternative approach
3. **Platform Integration**: System tray and hotkeys require platform-specific code
4. **Performance**: Potential performance differences between WPF and Avalonia
5. **XAML Compatibility**: Not all WPF XAML features directly translate to Avalonia

### Medium Risk
1. **Dialog System**: Different dialog patterns in Avalonia
2. **Font Rendering**: Different font systems between platforms
3. **File System Integration**: Cross-platform file handling differences
4. **Settings Storage**: Platform-specific settings locations
5. **Keyboard Shortcuts**: Different key handling across platforms

### Low Risk
1. **Business Logic**: Already cross-platform compatible
2. **Basic XAML**: Most layout controls have direct Avalonia equivalents
3. **Data Binding**: Similar concepts with minor syntax differences
4. **MVVM Pattern**: Well-supported in Avalonia

### Risk Mitigation Strategies
1. **Prototype Critical Components**: Build IntellisenseTextBox prototype early
2. **Platform Testing**: Set up testing on all target platforms
3. **Fallback Options**: Have alternative approaches for complex features
4. **Incremental Validation**: Test each component as it's migrated
5. **Community Support**: Leverage Avalonia community for guidance

## Success Metrics

### Phase Completion Criteria
1. **Phase 1**: Basic app launches and displays tasks on macOS
2. **Phase 2**: Core task operations work (CRUD operations)
3. **Phase 3**: All dialogs and advanced features functional
4. **Phase 4**: Platform-specific features working on all targets
5. **Phase 5**: Performance and polish meet quality standards

### Quality Gates
1. **Functionality**: All existing features work equivalently
2. **Performance**: Response times within 10% of WPF version
3. **Memory Usage**: No significant memory leaks or excessive usage
4. **Cross-Platform**: Identical behavior across Windows, macOS, Linux
5. **User Experience**: Consistent UI/UX across platforms

## Timeline Estimate

### Phase 1: Foundation (2-3 weeks)
- Project setup and basic window
- Task display functionality
- File loading and basic operations

### Phase 2: Core Features (3-4 weeks)
- Task CRUD operations
- Basic filtering and sorting
- Settings management
- File operations

### Phase 3: Advanced UI (4-5 weeks)
- All dialog windows
- Custom controls (IntellisenseTextBox)
- Advanced filtering and grouping
- Keyboard shortcuts

### Phase 4: Platform Features (2-3 weeks)
- System tray integration
- Platform-specific optimizations
- Printing functionality
- Hot keys

### Phase 5: Polish & Testing (2-3 weeks)
- Cross-platform testing
- Performance optimization
- Bug fixes and refinement
- Documentation updates

**Total Estimated Duration: 13-18 weeks**

## Resource Requirements

### Development Environment
- macOS development machine (primary)
- Windows VM or machine for testing
- Linux VM or machine for testing
- Visual Studio or JetBrains Rider IDE

### Technical Resources
- Avalonia UI documentation and samples
- Cross-platform deployment knowledge
- XAML and C# development expertise
- Platform-specific API knowledge (macOS, Windows, Linux)

### External Dependencies
- Avalonia UI framework (stable release)
- Platform-specific libraries for system integration
- Build and deployment tools for multiple platforms

## Constraints

### Technical Constraints
1. Must maintain backward compatibility with existing todo.txt files
2. Cannot break existing business logic or data handling
3. Must support .NET 9.0 target framework
4. Performance must be acceptable on older hardware

### Project Constraints
1. Current machine cannot build or test WPF version
2. Must be able to validate progress on macOS during development
3. Limited access to Windows/Linux testing environments initially
4. Migration must not impact existing WPF codebase until replacement is ready

### Resource Constraints
1. Single developer working on migration
2. Limited time for extensive cross-platform testing
3. Must leverage existing code as much as possible
4. Cannot afford extensive UI/UX redesign efforts

## Dependencies

### Internal Dependencies
1. TodoTxt.Lib project (no changes needed)
2. TodoTxt.Shared project (minor updates may be needed)
3. Test suite (will need updates for Avalonia testing)

### External Dependencies
1. Avalonia UI framework stability and compatibility
2. Platform-specific libraries for system integration
3. Cross-platform deployment tooling
4. Community support and documentation

### Cross-Project Dependencies
1. Solution file updates for new projects
2. Build system updates for multi-platform builds
3. Documentation updates for new deployment process
4. User migration guides for platform differences

## Approval Criteria

Before proceeding with implementation, this plan should be validated for:

1. **Technical Feasibility**: All identified risks have mitigation strategies
2. **Resource Availability**: Necessary tools and knowledge are accessible
3. **Timeline Acceptability**: Estimated duration aligns with project needs
4. **Quality Standards**: Success metrics are measurable and achievable
5. **User Impact**: Migration benefits justify the effort and complexity

## Next Steps

1. **Plan Review**: Validate this plan with stakeholders
2. **Environment Setup**: Prepare development environment for Avalonia
3. **Prototype Creation**: Build minimal Avalonia prototype to validate approach
4. **Implementation Planning**: Create detailed implementation guide
5. **Risk Assessment**: Validate risk mitigation strategies
6. **Go/No-Go Decision**: Final approval to proceed with migration