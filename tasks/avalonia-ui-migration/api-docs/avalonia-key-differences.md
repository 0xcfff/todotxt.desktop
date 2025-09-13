# Key Avalonia vs WPF Differences

## XAML Namespace Differences

### WPF
```xml
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
```

### Avalonia
```xml
xmlns="https://github.com/avaloniaui"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
```

## Control Equivalents

| WPF Control | Avalonia Control | Notes |
|-------------|------------------|-------|
| `Window` | `Window` | Same concept, different properties |
| `Grid` | `Grid` | Identical |
| `StackPanel` | `StackPanel` | Identical |
| `ListBox` | `ListBox` | Similar, some property differences |
| `TextBox` | `TextBox` | Similar functionality |
| `Menu` | `Menu` | Similar structure |
| `MenuItem` | `MenuItem` | Same |
| `Button` | `Button` | Identical |
| `CheckBox` | `CheckBox` | Identical |
| `StatusBar` | `DockPanel` | Use DockPanel with Border |
| `Popup` | `Popup` | Available but different behavior |

## Command System

### WPF RoutedUICommand
```xml
<RoutedUICommand x:Key="OpenFile" Text="Open File" />
<CommandBinding Command="{StaticResource OpenFile}" Executed="OpenFileExecuted"/>
```

### Avalonia ReactiveCommand
```csharp
public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

// In constructor
OpenFileCommand = ReactiveCommand.Create(OpenFile);
```

## Data Binding

### WPF
```xml
<ListBox ItemsSource="{Binding Tasks}" SelectedItem="{Binding SelectedItem}" />
```

### Avalonia
```xml
<ListBox ItemsSource="{Binding Tasks}" SelectedItem="{Binding SelectedItem}" />
```
*Similar syntax, but some binding features differ*

## Styling

### WPF Styles
```xml
<Style TargetType="ListBoxItem">
    <Setter Property="Background" Value="White"/>
    <Style.Triggers>
        <DataTrigger Binding="{Binding Completed}" Value="True">
            <Setter Property="Opacity" Value="0.5"/>
        </DataTrigger>
    </Style.Triggers>
</Style>
```

### Avalonia Styles
```xml
<Style Selector="ListBoxItem">
    <Setter Property="Background" Value="White"/>
</Style>
<Style Selector="ListBoxItem[IsCompleted=true]">
    <Setter Property="Opacity" Value="0.5"/>
</Style>
```

## Key Differences to Address

### 1. No RoutedUICommand
- Use ReactiveCommand or ICommand implementations
- Different command binding syntax

### 2. Different Styling System
- CSS-like selectors instead of triggers
- No DataTriggers, use pseudo-classes

### 3. Different Dialog System
- No built-in ShowDialog() equivalent
- Use custom dialog implementation or third-party libraries

### 4. No DependencyObject/DependencyProperty
- Use ReactiveUI or implement INotifyPropertyChanged
- Different attached property system

### 5. Different Resource System
- Resources work differently
- StaticResource vs DynamicResource behavior differs

## Migration Strategies

### Commands
Replace WPF commands with ReactiveCommand:
```csharp
// WPF
private void OpenFileExecuted(object sender, ExecutedRoutedEventArgs e)
{
    // Implementation
}

// Avalonia
public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

private void OpenFile()
{
    // Implementation
}
```

### Styles
Convert WPF styles to Avalonia selectors:
```xml
<!-- WPF -->
<DataTrigger Binding="{Binding IsCompleted}" Value="True">
    <Setter Property="TextDecorations" Value="Strikethrough"/>
</DataTrigger>

<!-- Avalonia -->
<Style Selector="TextBlock.completed">
    <Setter Property="TextDecorations" Value="Strikethrough"/>
</Style>
```

### Dialogs
Create custom dialog base classes:
```csharp
public class DialogService
{
    public async Task<bool> ShowConfirmationDialog(string message)
    {
        var dialog = new ConfirmationDialog { Message = message };
        return await dialog.ShowDialog<bool>(GetMainWindow());
    }
}
```

## Platform Specific Considerations

### File Dialogs
```csharp
public async Task<string[]> ShowOpenFileDialog()
{
    var dialog = new OpenFileDialog
    {
        Title = "Open Todo File",
        Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } }
        }
    };
    
    return await dialog.ShowAsync(GetMainWindow());
}
```

### System Tray (Platform Specific)
```csharp
// Windows
using System.Drawing;
using System.Windows.Forms;

// macOS  
using AppKit;

// Linux
// Depends on desktop environment
```