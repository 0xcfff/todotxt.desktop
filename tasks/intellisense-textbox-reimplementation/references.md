# IntellisenseTextBox Reference Implementation

## Original WPF Implementation Key Code Snippets

### Property Definitions
```csharp
public TaskList TaskList
{
    get { return (TaskList)GetValue(TaskListProperty); }
    set { SetValue(TaskListProperty, value); }
}
public static readonly DependencyProperty TaskListProperty =
    DependencyProperty.Register("TaskList", typeof(TaskList), typeof(IntellisenseTextBox), new UIPropertyMetadata(null));

public bool CaseSensitive
{
    get { return (bool)GetValue(CaseSensitiveProperty); }
    set { SetValue(CaseSensitiveProperty, value); }
}
public static readonly DependencyProperty CaseSensitiveProperty =
    DependencyProperty.Register("CaseSensitive", typeof(bool), typeof(IntellisenseTextBox), new UIPropertyMetadata(false));
```

### Constructor Setup
```csharp
public IntellisenseTextBox()
{
    // Set up the Intellisense list
    this.IntellisenseList = new ListBox();
    this.IntellisenseList.IsTextSearchEnabled = true;
    this.IntellisenseList.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
    this.IntellisenseList.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
    this.IntellisenseList.PreviewKeyUp += new KeyEventHandler(Intellisense_PreviewKeyUp);
    this.IntellisenseList.MouseUp += new MouseButtonEventHandler(Intellisense_MouseUp);

    // Set up the Intellisense popup
    this.IntellisensePopup = new Popup();
    this.IntellisensePopup.IsOpen = false;
    this.IntellisensePopup.Height = Double.NaN; // auto
    this.IntellisensePopup.MinWidth = 150;
    this.IntellisensePopup.MaxWidth = 500;
    this.IntellisensePopup.StaysOpen = false;
    this.IntellisensePopup.Placement = PlacementMode.Bottom;
    this.IntellisensePopup.PlacementTarget = this;
    this.IntellisensePopup.Child = IntellisenseList;

    // Set up an event handler on the text box to trigger Intellisense
    this.TextChanged += new TextChangedEventHandler(IntellisenseTextBox_TextChanged);
}
```

### Popup Management
```csharp
public void ShowIntellisensePopup(IEnumerable<string> s, Rect placement)
{
    if (s == null || s.Count() == 0)
    {
        return;
    }

    this.IntellisensePopup.PlacementRectangle = placement;
    this.IntellisenseList.ItemsSource = s;
    this.IntellisenseList.SelectedItem = null;
    this.IntellisensePopup.IsOpen = true;

    this.Focus();
}

public void HideIntellisensePopup()
{
    this.IntellisensePopup.IsOpen = false;
}
```

### Text Insertion Logic
```csharp
private void InsertIntellisenseText()
{
    HideIntellisensePopup();

    if (this.IntellisenseList.SelectedItem == null)
    {
        this.Focus();
        return;
    }

    this.Text = this.Text.Remove(this.IntelliPos, this.CaretIndex - this.IntelliPos);

    var newText = this.IntellisenseList.SelectedItem.ToString();
    this.Text = this.Text.Insert(this.IntelliPos, newText);
    this.CaretIndex = this.IntelliPos + newText.Length;

    this.Focus();
}
```

### Keyboard Event Handling
```csharp
public virtual void Intellisense_PreviewKeyUp(object sender, KeyEventArgs e)
{
    switch (e.Key)
    {
        case Key.Enter:
        case Key.Tab:
        case Key.Space:
            InsertIntellisenseText();
            e.Handled = true;
            break;
        case Key.Escape:
            HideIntellisensePopup();
            this.CaretIndex = this.Text.Length;
            this.Focus();
            e.Handled = true;
            break;
    }
}
```

### TextBox Key Handling
```csharp
protected override void OnPreviewKeyUp(KeyEventArgs e)
{
    base.OnPreviewKeyUp(e);

    if (this.IntellisensePopup.IsOpen && !this.IntellisenseList.IsFocused)
    {
        if (this.CaretIndex <= this.IntelliPos) // we've moved behind the symbol, drop out of intellisense
        {
            HideIntellisensePopup();
            e.Handled = false; // allow key event to be passed to TextBox
            return;
        }

        switch (e.Key)
        {
            case Key.Down:
                if (this.IntellisenseList.Items.Count != 0)
                {
                    this.IntellisenseList.SelectedIndex = 0; 
                    var listBoxItem = (ListBoxItem)this.IntellisenseList.ItemContainerGenerator.ContainerFromItem(
                        this.IntellisenseList.SelectedItem);
                    listBoxItem.Focus();
                }
                e.Handled = true;
                break;
            case Key.Escape:
                HideIntellisensePopup();
                e.Handled = true;
                break;
            case Key.Space:
            case Key.Enter:
                HideIntellisensePopup();
                e.Handled = false; // allow key event to be passed to TextBox
                break;
            default:
                var word = FindIntelliWord();
                if (this.CaseSensitive)
                {
                    this.IntellisenseList.Items.Filter = (o) => o.ToString().Contains(word);
                }
                else
                {
                    this.IntellisenseList.Items.Filter = (o) => (o.ToString().IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0);
                }
                e.Handled = true;
                break;
        }
    }
}
```

### Text Change Detection
```csharp
private void IntellisenseTextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    if (e.Changes.Count != 1 || e.Changes.First().AddedLength < 1 || this.CaretIndex < 1)
    {
        return;
    }

    if (this.TaskList == null)
    {
        return;
    }

    CheckKeyAndShowPopup();
}

public void CheckKeyAndShowPopup()
{
    var lastAddedCharacter = this.Text.Substring(this.CaretIndex - 1, 1);
    switch (lastAddedCharacter)
    {
        case "+":
            this.IntelliPos = this.CaretIndex - 1;
            ShowIntellisensePopup(this.TaskList.Projects, this.GetRectFromCharacterIndex(this.IntelliPos));
            break;

        case "@":
            this.IntelliPos = this.CaretIndex - 1;
            ShowIntellisensePopup(this.TaskList.Contexts, this.GetRectFromCharacterIndex(this.IntelliPos));
            break;
        case "(":
            if (this.CaretIndex == 1 ||
                (this.CaretIndex == 12 && StartDateWithPriorityRegex.IsMatch(this.Text.Substring(0, 12))))
            {
                this.IntelliPos = this.CaretIndex - 1;
                ShowIntellisensePopup(Priorities, this.GetRectFromCharacterIndex(this.IntelliPos));
            }
            break;
    }
}
```

### Helper Methods
```csharp
private string FindIntelliWord()
{
    return this.Text.Substring(this.IntelliPos + 1, this.CaretIndex - this.IntelliPos - 1);
}

private readonly Regex StartDateWithPriorityRegex = new Regex(@"[0-9]{4}\-[0-9]{2}\-[0-9]{2}\s\(");
private readonly List<string> Priorities = Enumerable.Range('A', 26).Select(i => $"({Convert.ToChar(i)})").ToList();
```

## Key Differences for Avalonia Implementation

### Property System
- **WPF**: `DependencyProperty.Register`
- **Avalonia**: `AvaloniaProperty.Register`

### Popup Positioning
- **WPF**: `PlacementRectangle` property available
- **Avalonia**: No `PlacementRectangle`, use `PlacementTarget` and `Placement`

### Event Handling
- **WPF**: `PreviewKeyUp`, `MouseUp`
- **Avalonia**: `KeyUp`, `PointerReleased`

### ListBox Filtering
- **WPF**: `Items.Filter` property with predicate
- **Avalonia**: Use `ItemsSource` filtering with LINQ

### Text Positioning
- **WPF**: `GetRectFromCharacterIndex()` method available
- **Avalonia**: No direct equivalent, needs custom implementation

## Current Avalonia Implementation Issues

### Constructor Problems
```csharp
// Current problematic code
this.Background = Brushes.Black;
if (this.Background == null)
{
    this.Background = Brushes.White;
}
this.Foreground = Brushes.White;
if (this.Foreground == null)
{
    this.Foreground = Brushes.Black;
}
```

### Popup Positioning Issues
```csharp
// Current problematic code
private Rect GetRectFromCharacterIndex(int index)
{
    // For now, return a simple rectangle at the bottom of the text box
    // In a more sophisticated implementation, we would calculate the actual character position
    return new Rect(0, this.Bounds.Height, 0, 0);
}
```

### Event Handling Problems
```csharp
// Current problematic code
private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
{
    try
    {
        // Basic text change detection
        if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
            return;

        var lastChar = this.Text[this.CaretIndex - 1];
        if (lastChar == '+' || lastChar == '@' || lastChar == '(')
        {
            // Log for debugging
            System.Diagnostics.Debug.WriteLine($"Trigger character detected: {lastChar}");
            
            // Show appropriate popup based on trigger character
            if (lastChar == '+')
            {
                _intelliPos = this.CaretIndex - 1;
                ShowIntellisensePopup(TaskList?.Projects ?? new List<string>(), GetRectFromCharacterIndex(_intelliPos));
            }
            // ... rest of the logic
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error in text changed handler: {ex.Message}");
        // Don't let errors break text input
    }
}
```

## Lessons Learned

### What Works in WPF
- `PlacementRectangle` for precise popup positioning
- `GetRectFromCharacterIndex()` for text positioning
- `Items.Filter` for efficient ListBox filtering
- `PreviewKeyUp` for early key event handling

### What Needs Adaptation for Avalonia
- Use `PlacementTarget` and `Placement` instead of `PlacementRectangle`
- Implement custom text positioning logic
- Use LINQ filtering instead of `Items.Filter`
- Use `KeyUp` instead of `PreviewKeyUp`
- Handle focus management differently

### Key Success Factors
- Start simple and build incrementally
- Test each step thoroughly
- Use Avalonia patterns, not WPF patterns
- Handle errors gracefully
- Focus on functionality first, positioning second
