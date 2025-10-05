# Proposed Avalonia Project Structure

## Solution Structure
```
ToDo.Net.sln
├── src/
│   ├── TodoTxt.Lib/                     # Existing - Business Logic
│   │   ├── Task.cs
│   │   ├── TaskList.cs
│   │   ├── TaskException.cs
│   │   └── ToDoLib.csproj
│   ├── TodoTxt.Shared/                  # Existing - Utilities
│   │   ├── StringExtensions.cs
│   │   ├── IEnumerableExtensions.cs
│   │   └── CommonExtensions.csproj
│   ├── TodoTxt.UI/                      # Existing WPF - Keep for reference
│   │   └── [existing WPF files]
│   ├── TodoTxt.Avalonia/                # New - Main Avalonia App
│   │   ├── Views/
│   │   │   ├── MainWindow.axaml
│   │   │   ├── MainWindow.axaml.cs
│   │   │   └── Dialogs/
│   │   │       ├── OptionsDialog.axaml
│   │   │       ├── FilterDialog.axaml
│   │   │       ├── SetDueDateDialog.axaml
│   │   │       └── ...
│   │   ├── ViewModels/
│   │   │   ├── MainWindowViewModel.cs
│   │   │   ├── ViewModelBase.cs
│   │   │   └── Dialogs/
│   │   │       ├── OptionsDialogViewModel.cs
│   │   │       └── ...
│   │   ├── Models/
│   │   │   └── AppSettings.cs
│   │   ├── Services/
│   │   │   ├── ISettingsService.cs
│   │   │   ├── SettingsService.cs
│   │   │   ├── IDialogService.cs
│   │   │   └── DialogService.cs
│   │   ├── Converters/
│   │   │   ├── TaskToClassConverter.cs
│   │   │   └── BooleanToVisibilityConverter.cs
│   │   ├── Assets/
│   │   │   ├── icon.ico
│   │   │   └── avalonia-logo.ico
│   │   ├── Styles/
│   │   │   ├── App.axaml
│   │   │   └── TaskStyles.axaml
│   │   ├── App.axaml
│   │   ├── App.axaml.cs
│   │   ├── Program.cs
│   │   └── TodoTxt.Avalonia.csproj
│   ├── TodoTxt.Avalonia.Core/           # New - Shared Avalonia Components
│   │   ├── Controls/
│   │   │   ├── IntellisenseTextBox.axaml
│   │   │   ├── IntellisenseTextBox.axaml.cs
│   │   │   └── TaskListBox.axaml
│   │   ├── Behaviors/
│   │   │   ├── TextBoxEnterKeyBehavior.cs
│   │   │   └── ListBoxKeyNavigationBehavior.cs
│   │   ├── Extensions/
│   │   │   ├── WindowExtensions.cs
│   │   │   └── ControlExtensions.cs
│   │   └── TodoTxt.Avalonia.Core.csproj
│   └── TodoTxt.Platform/                # New - Platform Services
│       ├── Interfaces/
│       │   ├── ITrayService.cs
│       │   ├── IFileDialogService.cs
│       │   ├── IHotkeyService.cs
│       │   ├── IPrintService.cs
│       │   └── ISettingsStorageService.cs
│       ├── Windows/
│       │   ├── WindowsTrayService.cs
│       │   ├── WindowsHotkeyService.cs
│       │   └── WindowsSettingsStorageService.cs
│       ├── MacOS/
│       │   ├── MacOSTrayService.cs
│       │   └── MacOSSettingsStorageService.cs
│       ├── Linux/
│       │   ├── LinuxTrayService.cs
│       │   └── LinuxSettingsStorageService.cs
│       ├── Common/
│       │   ├── FileDialogService.cs
│       │   └── PrintService.cs
│       └── TodoTxt.Platform.csproj
└── tests/
    ├── TodoTxt.Lib.Tests/               # Existing
    ├── TodoTxt.Avalonia.Tests/          # New - UI Tests
    └── TodoTxt.Platform.Tests/          # New - Platform Tests
```

## Project File Examples

### TodoTxt.Avalonia.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <UseAvalonia>true</UseAvalonia>
    <ApplicationIcon>Assets/icon.ico</ApplicationIcon>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.0.0" />
    <PackageReference Include="Avalonia.Desktop" Version="11.0.0" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.0.0" />
    <PackageReference Include="Avalonia.Fonts.Inter" Version="11.0.0" />
    <PackageReference Include="Avalonia.ReactiveUI" Version="11.0.0" />
    <!--Condition below is needed to remove Avalonia.Diagnostics package from build output in Release configuration.-->
    <PackageReference Condition="'$(Configuration)' == 'Debug'" Include="Avalonia.Diagnostics" Version="11.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\TodoTxt.Lib\ToDoLib.csproj" />
    <ProjectReference Include="..\TodoTxt.Shared\CommonExtensions.csproj" />
    <ProjectReference Include="..\TodoTxt.Avalonia.Core\TodoTxt.Avalonia.Core.csproj" />
    <ProjectReference Include="..\TodoTxt.Platform\TodoTxt.Platform.csproj" />
  </ItemGroup>

</Project>
```

### TodoTxt.Avalonia.Core.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <UseAvalonia>true</UseAvalonia>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.0.0" />
    <PackageReference Include="Avalonia.ReactiveUI" Version="11.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\TodoTxt.Lib\ToDoLib.csproj" />
    <ProjectReference Include="..\TodoTxt.Shared\CommonExtensions.csproj" />
  </ItemGroup>

</Project>
```

### TodoTxt.Platform.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.0.0" />
  </ItemGroup>

  <!-- Windows-specific packages -->
  <ItemGroup Condition="$([MSBuild]::IsOSPlatform('Windows'))">
    <PackageReference Include="System.Windows.Forms" Version="9.0.0" />
  </ItemGroup>

  <!-- macOS-specific packages -->
  <ItemGroup Condition="$([MSBuild]::IsOSPlatform('OSX'))">
    <PackageReference Include="MonoMac.NetStandard" Version="0.7.1" />
  </ItemGroup>

  <!-- Linux-specific packages -->
  <ItemGroup Condition="$([MSBuild]::IsOSPlatform('Linux'))">
    <PackageReference Include="Tmds.DBus" Version="0.9.1" />
  </ItemGroup>

</Project>
```

## Key Architecture Decisions

### 1. Separation of Concerns
- **TodoTxt.Avalonia**: Main UI application
- **TodoTxt.Avalonia.Core**: Reusable Avalonia-specific components
- **TodoTxt.Platform**: Platform-specific implementations

### 2. MVVM Pattern
- Views in `Views/` folder with `.axaml` files
- ViewModels in `ViewModels/` folder
- Models represent application state and settings

### 3. Dependency Injection
```csharp
// In App.axaml.cs or Program.cs
services.AddSingleton<ISettingsService, SettingsService>();
services.AddSingleton<IDialogService, DialogService>();
services.AddSingleton<ITrayService, GetTrayService>(); // Platform-specific
```

### 4. Service Abstraction
All platform-specific functionality behind interfaces:
- File operations
- System tray
- Settings storage
- Print services
- Hotkey registration

### 5. Theme and Styling
- Use Avalonia's Fluent theme as base
- Custom styles in separate `.axaml` files
- Theme-aware color scheme

## Migration Sequence

1. **Create TodoTxt.Avalonia project** with basic window
2. **Create TodoTxt.Platform project** with interface definitions
3. **Create TodoTxt.Avalonia.Core project** for shared components
4. **Implement basic MainWindow** with task list
5. **Port ViewModels** from WPF version
6. **Implement custom controls** (IntellisenseTextBox)
7. **Create dialog system** and port all dialogs
8. **Implement platform services** for each target platform
9. **Add styling and theming**
10. **Testing and polish**

## Build Configuration

### Desktop Targets
```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

### Platform-Specific Builds
- Windows: `dotnet publish -c Release -r win-x64`
- macOS: `dotnet publish -c Release -r osx-x64`
- Linux: `dotnet publish -c Release -r linux-x64`