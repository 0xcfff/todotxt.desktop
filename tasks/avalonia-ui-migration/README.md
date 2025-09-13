# TodoTxt Desktop Avalonia UI Migration

## Overview

This task documents the comprehensive plan to migrate the TodoTxt Desktop Client project from WPF to Avalonia UI, enabling cross-platform support for Windows, macOS, and Linux. The migration is designed with macOS as the primary development platform since the current WPF version cannot be built or tested on the current machine.

## Documentation Structure

### Core Planning Documents
- **[research.md](./research.md)** - Comprehensive analysis of current WPF architecture and Avalonia migration requirements
- **[plan.md](./plan.md)** - High-level migration strategy with scope, approach, risks, and timeline
- **[implementation.md](./implementation.md)** - Detailed step-by-step implementation guide with testing strategy

### Supporting Documentation
- **[api-docs/](./api-docs/)** - Technical reference materials
  - `avalonia-key-differences.md` - Key differences between WPF and Avalonia
- **[samples/](./samples/)** - Example code and project structure
  - `basic-mainwindow-example.axaml` - Sample Avalonia main window
  - `project-structure-example.md` - Proposed project organization

## Migration Summary

### Scope
- **Full UI Framework Migration**: Complete transition from WPF to Avalonia UI
- **Cross-Platform Support**: Windows, macOS, and Linux compatibility
- **Feature Parity**: All existing functionality preserved
- **Business Logic Preservation**: TodoTxt.Lib and TodoTxt.Shared projects remain unchanged

### Approach
- **Mac-First Development**: Primary development on macOS for immediate testing capability
- **Phased Implementation**: 5-phase approach minimizing risk and enabling incremental validation
- **Parallel Development**: Maintain WPF version functionality during migration
- **Service Abstraction**: Platform-specific features behind clean interfaces

### Timeline Estimate
**13-18 weeks** total, broken down as:
- Phase 1: Foundation Setup (2-3 weeks)
- Phase 2: Core Task Operations (3-4 weeks)  
- Phase 3: Advanced UI Features (4-5 weeks)
- Phase 4: Platform-Specific Features (2-3 weeks)
- Phase 5: Polish and Testing (2-3 weeks)

## Key Technical Challenges

### High Complexity
1. **Custom IntellisenseTextBox** - Complete rewrite required for Avalonia popup system
2. **Print Functionality** - Alternative to WPF's WebBrowser-based printing needed
3. **Cross-Platform System Integration** - Tray, hotkeys, file dialogs per platform
4. **Complex XAML Migration** - Extensive menu systems and styling conversions

### Medium Complexity  
1. **Dialog System** - Different patterns in Avalonia vs WPF dialogs
2. **Command System** - Migration from RoutedUICommand to ReactiveCommand
3. **Settings Management** - Cross-platform settings storage
4. **Font and Styling Systems** - Different approaches between frameworks

## Project Structure

### New Projects
- **TodoTxt.Avalonia** - Main cross-platform application
- **TodoTxt.Avalonia.Core** - Shared Avalonia-specific components and controls
- **TodoTxt.Platform** - Platform-specific service implementations

### Preserved Projects
- **TodoTxt.Lib** - Business logic (no changes needed)
- **TodoTxt.Shared** - Utility extensions (minimal changes)
- **TodoTxt.UI** - Original WPF version (kept for reference)

## Success Criteria

### Technical Validation
- All automated tests pass across platforms
- Performance meets or exceeds WPF version
- Memory usage and responsiveness acceptable
- Cross-platform behavior consistency

### Functional Validation
- 100% feature parity with WPF version
- All user workflows function identically
- Data integrity and file compatibility maintained
- Settings migration from WPF version works

### Quality Validation
- Professional appearance across all platforms
- Accessibility requirements met
- Error handling and robustness
- Documentation and deployment readiness

## Risk Mitigation

### Development Risks
- **Early Prototyping**: Build critical components first to validate feasibility
- **Incremental Testing**: Validate each phase before proceeding
- **Platform Testing**: Set up testing environments for all target platforms
- **Community Support**: Leverage Avalonia community for guidance

### Project Risks
- **Rollback Plan**: Keep WPF version functional as fallback
- **Partial Success Options**: Ship single-platform or reduced-feature versions if needed
- **Regular Checkpoints**: Assess progress and viability at each phase boundary

## Getting Started

### Prerequisites
1. .NET 9.0 SDK installed
2. Avalonia UI project templates: `dotnet new install Avalonia.ProjectTemplates`
3. IDE with Avalonia support (VS Code, Rider, or Visual Studio)
4. Access to Windows/Linux environments for testing (can be VMs)

### Next Steps
1. **Environment Setup**: Install Avalonia development tools
2. **Prototype Creation**: Build minimal working Avalonia app to validate approach
3. **Phase 1 Execution**: Begin with foundation setup following implementation guide
4. **Regular Review**: Assess progress and adjust plan as needed

## Decision Points

This migration should proceed only if:
- ✅ **Technical Feasibility Confirmed**: Avalonia can support all required features
- ✅ **Resource Availability**: Sufficient time and expertise available  
- ✅ **Quality Standards**: Migration can meet current application quality
- ✅ **User Value**: Cross-platform benefits justify the development effort

## References

- [Avalonia UI Documentation](https://docs.avaloniaui.net/)
- [WPF to Avalonia Migration Guide](https://docs.avaloniaui.net/docs/guides/platforms/migration-from-wpf)
- [Avalonia Community Samples](https://github.com/AvaloniaUI/Avalonia.Samples)
- [Cross-Platform .NET Development Best Practices](https://docs.microsoft.com/en-us/dotnet/standard/cross-platform/)

---

*This migration plan represents a comprehensive strategy for moving TodoTxt Desktop to a modern, cross-platform UI framework while maintaining all existing functionality and ensuring a smooth transition for users.*