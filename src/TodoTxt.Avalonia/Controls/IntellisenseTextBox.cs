using System.Net.Http.Headers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using ToDoLib;

namespace TodoTxt.Avalonia.Controls
{
    /// <summary>
    /// A TextBox control that provides autocompletion functionality for todo.txt projects, contexts, and priorities.
    /// </summary>
    public class IntellisenseTextBox : TextBox
    {
        private Popup? _intellisensePopup;
        private ListBox? _intellisenseList;
        private int _triggerPosition = -1;

        /// <summary>
        /// Defines the TaskListProperty dependency property.
        /// </summary>
        public static readonly StyledProperty<TaskList?> TaskListProperty =
            AvaloniaProperty.Register<IntellisenseTextBox, TaskList?>(nameof(TaskList));

        /// <summary>
        /// Gets or sets the TaskList that provides data for autocompletion.
        /// </summary>
        public TaskList? TaskList
        {
            get => GetValue(TaskListProperty);
            set => SetValue(TaskListProperty, value);
        }

        /// <summary>
        /// Defines the CaseSensitiveProperty dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> CaseSensitiveProperty =
            AvaloniaProperty.Register<IntellisenseTextBox, bool>(nameof(CaseSensitive), false);

        /// <summary>
        /// Gets or sets whether filtering should be case-sensitive.
        /// </summary>
        public bool CaseSensitive
        {
            get => GetValue(CaseSensitiveProperty);
            set => SetValue(CaseSensitiveProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the IntellisenseTextBox class.
        /// </summary>
        public IntellisenseTextBox()
        {
            this.TextChanged += IntellisenseTextBox_TextChanged;
            this.KeyDown += IntellisenseTextBox_KeyDown;
            this.KeyUp += IntellisenseTextBox_KeyUp;
            this.LostFocus += IntellisenseTextBox_LostFocus;

            _intellisenseList = new ListBox
            {
                MinHeight = 200,
                MinWidth = 200,
                MaxHeight = 400,
                MaxWidth = 200,
                Focusable = false,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2)
            };

            _intellisenseList.SelectionChanged += IntellisenseList_SelectionChanged;
            _intellisenseList.KeyDown += IntellisenseList_KeyDown;


            _intellisensePopup = new Popup
            {
                PlacementTarget = this,
                Placement = PlacementMode.BottomEdgeAlignedLeft,
                IsLightDismissEnabled = true,
                Child = _intellisenseList,
                Focusable = false,
            };
            this.LogicalChildren.Add(_intellisensePopup);
        }

        /// <summary>
        /// Shows the autocompletion popup.
        /// </summary>
        public void ShowPopup()
        {
            _intellisensePopup!.IsOpen = true;
            // Always select the first item when popup opens
            if (_intellisenseList!.Items.Count > 0)
            {
                _intellisenseList.SelectedIndex = 0;
            }
            // Don't change focus - keep the TextBox focused so user can continue typing
        }

        /// <summary>
        /// Hides the autocompletion popup.
        /// </summary>
        public void HidePopup()
        {
            _intellisensePopup!.IsOpen = false;
        }

        private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Text) || this.CaretIndex < 1)
                {
                    HidePopup();
                    _triggerPosition = -1;
                    return;
                }

                // Validate cursor position
                if (this.CaretIndex > this.Text.Length)
                {
                    HidePopup();
                    _triggerPosition = -1;
                    return;
                }

                var lastChar = this.Text[this.CaretIndex - 1];
                if (lastChar == '+' || lastChar == '@' || lastChar == '(')
                {
                    _triggerPosition = this.CaretIndex - 1;
                    ShowSuggestions(lastChar);
                }
                else if (_triggerPosition >= 0 && this.CaretIndex > _triggerPosition)
                {
                    // Update filtering
                    UpdateFiltering();
                }
                else
                {
                    HidePopup();
                    _triggerPosition = -1;
                }
            }
            catch (Exception ex)
            {
                // Log error and gracefully handle by hiding popup
                System.Diagnostics.Debug.WriteLine($"Error in text changed handler: {ex.Message}");
                HidePopup();
                _triggerPosition = -1;
            }
        }

        private void IntellisenseList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_intellisenseList!.SelectedItem != null)
            {
                // InsertSelectedText();
            }
        }

        private void IntellisenseTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            // Only handle keys when popup is closed
            if (!_intellisensePopup!.IsOpen)
            {
                switch (e.Key)
                {
                    case Key.Up:
                    case Key.Down:
                    case Key.PageUp:
                    case Key.PageDown:
                    case Key.Home:
                    case Key.End:
                        // Let TextBox handle these keys normally when popup is closed
                        e.Handled = false;
                        break;
                    case Key.Left:
                    case Key.Right:
                        // Let TextBox handle cursor movement normally
                        e.Handled = false;
                        break;
                    default:
                        // Let TextBox handle all other keys normally
                        e.Handled = false;
                        break;
                }
            }
            else
            {
                // If popup is open, only handle specific keys that should work in TextBox
                switch (e.Key)
                {
                    case Key.Left:
                    case Key.Right:
                        // Allow cursor movement even when popup is open
                        e.Handled = false;
                        break;
                    case Key.Up:
                        // Move selection up in the list
                        if (_intellisenseList!.Items.Count > 0)
                        {
                            if (_intellisenseList.SelectedIndex > 0)
                            {
                                _intellisenseList.SelectedIndex--;
                            }
                            else
                            {
                                _intellisenseList.SelectedIndex = _intellisenseList.Items.Count - 1; // Wrap to last item
                            }
                        }
                        e.Handled = true;
                        break;
                    case Key.Down:
                        // Move selection down in the list
                        if (_intellisenseList!.Items.Count > 0)
                        {
                            if (_intellisenseList.SelectedIndex < _intellisenseList.Items.Count - 1)
                            {
                                _intellisenseList.SelectedIndex++;
                            }
                            else
                            {
                                _intellisenseList.SelectedIndex = 0; // Wrap to first item
                            }
                        }
                        e.Handled = true;
                        break;
                    case Key.Enter:
                        // Prevent Enter from being processed by TextBox when popup is open
                        // The KeyUp handler will handle the selection
                        e.Handled = true;
                        break;
                    default:
                        // For typing and other keys, let TextBox handle them
                        // This allows filtering of the popup list
                        e.Handled = false;
                        break;
                }
            }
        }

        private void IntellisenseTextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            // Handle Enter in KeyUp to prevent double processing
            if (e.Key == Key.Enter && _intellisensePopup!.IsOpen)
            {
                InsertSelectedText();
                e.Handled = true;
            }
        }

        private void IntellisenseTextBox_LostFocus(object? sender, RoutedEventArgs e)
        {
            // Hide popup when TextBox loses focus (e.g., Tab to next control)
            HidePopup();
        }

        private void IntellisenseList_KeyDown(object? sender, KeyEventArgs e)
        {
            // Since the list is not focusable, this handler should rarely be called
            // Most key events will be handled by the TextBox
            // This is kept as a fallback for any edge cases
            e.Handled = false;
        }

        private void InsertSelectedText()
        {
            try
            {
                if (_intellisenseList!.SelectedItem == null || _triggerPosition < 0)
                    return;

                // Validate positions
                if (_triggerPosition >= this.Text?.Length || this.CaretIndex <= _triggerPosition)
                    return;
                    
                var selectedText = _intellisenseList.SelectedItem.ToString() ?? string.Empty;
                var currentText = this.Text ?? string.Empty;
                
                // Replace from trigger position to current cursor position
                var textToReplace = currentText.Substring(_triggerPosition, this.CaretIndex - _triggerPosition);
                var newText = currentText.Remove(_triggerPosition, this.CaretIndex - _triggerPosition);
                newText = newText.Insert(_triggerPosition, selectedText);
                
                this.Text = newText;
                this.CaretIndex = _triggerPosition + selectedText.Length;
                
                HidePopup();
                _triggerPosition = -1;
                
                // Ensure focus is on the TextBox and prevent further key processing
                this.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InsertSelectedText: {ex.Message}");
                HidePopup();
                _triggerPosition = -1;
            }
        }

        private int FindTriggerPosition()
        {
            var text = this.Text ?? string.Empty;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                if (text[i] == '+' || text[i] == '@' || text[i] == '(')
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Shows suggestions based on the trigger character.
        /// </summary>
        /// <param name="trigger">The trigger character that was typed.</param>
        private void ShowSuggestions(char trigger)
        {
            try
            {
                // Special handling for priority - only at start of line or after date
                if (trigger == '(')
                {
                    if (!IsValidPriorityPosition())
                    {
                        HidePopup();
                        return;
                    }
                }
                
                // Get appropriate data source based on trigger character
                IEnumerable<string> data = trigger switch
                {
                    '+' => TaskList?.Projects ?? new List<string>(),  // Project autocompletion
                    '@' => TaskList?.Contexts ?? new List<string>(),  // Context autocompletion
                    '(' => GetPriorityList(),                         // Priority autocompletion
                    _ => new List<string>()
                };
                
                if (data.Any())
                {
                    _intellisenseList!.ItemsSource = data;
                    ShowPopup();
                }
            }
            catch (Exception ex)
            {
                // Log error and gracefully handle by hiding popup
                System.Diagnostics.Debug.WriteLine($"Error in ShowSuggestions: {ex.Message}");
                HidePopup();
            }
        }

        /// <summary>
        /// Updates the filtering of suggestions based on the currently typed text.
        /// </summary>
        private void UpdateFiltering()
        {
            try
            {
                if (_triggerPosition < 0 || _intellisenseList?.ItemsSource == null)
                    return;

                // Validate positions to prevent index out of bounds
                if (_triggerPosition >= this.Text?.Length || this.CaretIndex <= _triggerPosition)
                    return;
                    
                // Extract the text typed after the trigger character
                var typedText = this.Text!.Substring(_triggerPosition + 1, this.CaretIndex - _triggerPosition - 1);
                var allItems = _intellisenseList.ItemsSource.Cast<string>();
                
                // Filter items based on case sensitivity setting
                var filteredItems = CaseSensitive 
                    ? allItems.Where(item => item.Contains(typedText))
                    : allItems.Where(item => item.IndexOf(typedText, StringComparison.CurrentCultureIgnoreCase) >= 0);
                
                _intellisenseList.ItemsSource = filteredItems.ToList();
            }
            catch (Exception ex)
            {
                // Log error and gracefully handle by hiding popup
                System.Diagnostics.Debug.WriteLine($"Error in UpdateFiltering: {ex.Message}");
                HidePopup();
            }
        }

        /// <summary>
        /// Determines if the current cursor position is valid for priority autocompletion.
        /// Priorities can only be at the start of a line or after a date (YYYY-MM-DD ).
        /// </summary>
        /// <returns>True if the position is valid for priority, false otherwise.</returns>
        private bool IsValidPriorityPosition()
        {
            var text = this.Text ?? string.Empty;
            var caretIndex = this.CaretIndex;
            
            // Priority can be at start of line (position 1)
            if (caretIndex == 1)
                return true;
                
            // Priority can be after date (YYYY-MM-DD ) at position 12
            if (caretIndex == 12)
            {
                var startText = text.Substring(0, 12);
                var dateRegex = new System.Text.RegularExpressions.Regex(@"^[0-9]{4}\-[0-9]{2}\-[0-9]{2}\s$");
                return dateRegex.IsMatch(startText);
            }
            
            return false;
        }

        /// <summary>
        /// Generates a list of priority options from (A) to (Z).
        /// </summary>
        /// <returns>A list of priority strings in the format (A), (B), ..., (Z).</returns>
        private List<string> GetPriorityList()
        {
            return Enumerable.Range('A', 26).Select(i => $"({Convert.ToChar(i)})").ToList();
        }


        protected override Type StyleKeyOverride { get { return typeof(TextBox); } }
    }
}
