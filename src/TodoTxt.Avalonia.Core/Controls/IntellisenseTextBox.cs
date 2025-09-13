using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using ToDoLib;

namespace TodoTxt.Avalonia.Core.Controls
{
    /// <summary>
    /// This class represents a text box with Intellisense popup behavior built in, for autocompletion 
    /// of projects and contexts from the MainWindowViewModel's task list.
    /// </summary>
    public class IntellisenseTextBox : TextBox
    {
        #region Properties

        private Popup? _intellisensePopup = null;
        private ListBox? _intellisenseList = null;
        private int _intelliPos; // used to position the Intellisense popup

        private readonly Regex _startDateWithPriorityRegex = new Regex(@"[0-9]{4}\-[0-9]{2}\-[0-9]{2}\s\(");
        private readonly List<string> _priorities = Enumerable.Range('A', 26).Select(i => $"({Convert.ToChar(i)})").ToList();

        public static readonly StyledProperty<TaskList?> TaskListProperty =
            AvaloniaProperty.Register<IntellisenseTextBox, TaskList?>(nameof(TaskList));

        public TaskList? TaskList
        {
            get => GetValue(TaskListProperty);
            set => SetValue(TaskListProperty, value);
        }

        public static readonly StyledProperty<bool> CaseSensitiveProperty =
            AvaloniaProperty.Register<IntellisenseTextBox, bool>(nameof(CaseSensitive), false);

        public bool CaseSensitive
        {
            get => GetValue(CaseSensitiveProperty);
            set => SetValue(CaseSensitiveProperty, value);
        }

        #endregion

        #region Constructor

        public IntellisenseTextBox()
        {
            try
            {
                // Initialize the intellisense popup
                _intellisensePopup = new Popup
                {
                    PlacementTarget = this,
                    Placement = PlacementMode.Bottom,
                    IsLightDismissEnabled = true
                };

                // Initialize the intellisense list
                _intellisenseList = new ListBox
                {
                    MaxHeight = 200,
                    MinWidth = 200
                };

                // Add the list to the popup
                _intellisensePopup.Child = _intellisenseList;

                // Set up event handlers
                _intellisenseList.SelectionChanged += IntellisenseList_SelectionChanged;
                _intellisenseList.KeyDown += IntellisenseList_KeyDown;
                
                // Set up an event handler on the text box to trigger Intellisense.
                this.TextChanged += IntellisenseTextBox_TextChanged;
                
                // Ensure the control is visible
                this.IsVisible = true;
                this.Opacity = 1.0;
                
                System.Diagnostics.Debug.WriteLine("IntellisenseTextBox constructor completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing IntellisenseTextBox: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        #endregion

        #region Intellisense Popup-related Methods

        /// <summary>
        /// Show the Intellisense popup.
        /// </summary>
        /// <param name="s">Source for the Intellisense list.</param>
        /// <param name="placement">This value should be set to the cursor position in the text box.</param>
        public void ShowIntellisensePopup(IEnumerable<string> s, Rect placement)
        {
            if (s == null || !s.Any())
            {
                return;
            }

            // Note: Avalonia Popup doesn't have PlacementRectangle, using PlacementTarget instead
            _intellisenseList!.ItemsSource = s;
            _intellisenseList.SelectedItem = null;
            _intellisensePopup!.IsOpen = true;

            this.Focus();
        }

        /// <summary>
        /// Hide the Intellisense popup.
        /// </summary>
        public void HideIntellisensePopup()
        {
            _intellisensePopup!.IsOpen = false;
        }

        /// <summary>
        /// Insert the selected Intellisense text into the textbox.
        /// Called in Intellisense_PreviewKeyUp and Intellisense_PointerReleased methods.
        /// </summary>
        private void InsertIntellisenseText()
        {
            HideIntellisensePopup();

            if (_intellisenseList!.SelectedItem == null)
            {
                this.Focus();
                return;
            }

            var currentText = this.Text ?? string.Empty;
            var newText = _intellisenseList.SelectedItem.ToString() ?? string.Empty;
            
            // Remove the text from IntelliPos to current caret position
            var textToRemove = currentText.Substring(_intelliPos, this.CaretIndex - _intelliPos);
            var updatedText = currentText.Remove(_intelliPos, this.CaretIndex - _intelliPos);
            
            // Insert the new text
            updatedText = updatedText.Insert(_intelliPos, newText);
            
            this.Text = updatedText;
            this.CaretIndex = _intelliPos + newText.Length;

            this.Focus();
        }

        /// <summary>
        /// Tab, Enter and Space keys will all add the selected text into the task string.
        /// Escape key cancels out.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">The key to trigger on.</param>
        public virtual void Intellisense_PreviewKeyUp(object? sender, KeyEventArgs e)
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
                    this.CaretIndex = (this.Text ?? string.Empty).Length;
                    this.Focus();
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Allow the user to click on an entry in the Intellisense list and insert that entry into the text box.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        public virtual void Intellisense_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            InsertIntellisenseText();
        }

        #endregion

        #region TextBox Event Handler Overrides

        /// <summary>
        /// Ensure that tabbing away from the text box hides the Intellisense popup before focus is lost.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Tab)
            {
                HideIntellisensePopup();
                e.Handled = false;
            }
        }

        /// <summary>
        /// Handle key events in the textbox that impact the Intellisense list and popup.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (_intellisensePopup!.IsOpen && !_intellisenseList!.IsFocused)
            {
                if (this.CaretIndex <= _intelliPos) // we've moved behind the symbol, drop out of intellisense
                {
                    HideIntellisensePopup();
                    e.Handled = false; // allow key event to be passed to TextBox
                    return;
                }

                switch (e.Key)
                {
                    case Key.Down:
                        if (_intellisenseList.Items.Count != 0)
                        {
                            _intellisenseList.SelectedIndex = 0;
                            _intellisenseList.Focus();
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
                            // Filter items that contain the word
                            var filteredItems = _intellisenseList.ItemsSource?.Cast<string>()
                                .Where(item => item.Contains(word))
                                .ToList();
                            _intellisenseList.ItemsSource = filteredItems;
                        }
                        else
                        {
                            // Filter items that contain the word (case insensitive)
                            var filteredItems = _intellisenseList.ItemsSource?.Cast<string>()
                                .Where(item => item.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
                                .ToList();
                            _intellisenseList.ItemsSource = filteredItems;
                        }
                        e.Handled = true;
                        break;
                }
            }
        }

        private string FindIntelliWord()
        {
            var currentText = this.Text ?? string.Empty;
            if (_intelliPos + 1 >= this.CaretIndex)
                return string.Empty;
                
            return currentText.Substring(_intelliPos + 1, this.CaretIndex - _intelliPos - 1);
        }

        /// <summary>
        /// Triggers the Intellisense popup to appear when "+", "@" or "(" is pressed in the text box.
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Event arguments</param>
        private void IntellisenseTextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            // For now, just log that the event is working
            System.Diagnostics.Debug.WriteLine("IntellisenseTextBox_TextChanged called");
            
            // We'll add the intellisense functionality back later
            // For now, just ensure the basic TextBox functionality works
        }

        private void IntellisenseList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            // Handle selection changes in the intellisense list
            System.Diagnostics.Debug.WriteLine("IntellisenseList_SelectionChanged called");
        }

        private void IntellisenseList_KeyDown(object? sender, KeyEventArgs e)
        {
            // Handle key events in the intellisense list
            System.Diagnostics.Debug.WriteLine("IntellisenseList_KeyDown called");
            
            if (e.Key == Key.Enter)
            {
                // Insert the selected item
                InsertIntellisenseText();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                // Hide the popup
                HideIntellisensePopup();
                this.Focus();
                e.Handled = true;
            }
        }

        public void CheckKeyAndShowPopup()
        {
            var currentText = this.Text ?? string.Empty;
            if (this.CaretIndex < 1 || this.CaretIndex > currentText.Length)
                return;

            var lastAddedCharacter = currentText.Substring(this.CaretIndex - 1, 1);
            switch (lastAddedCharacter)
            {
                case "+":
                    _intelliPos = this.CaretIndex - 1;
                    ShowIntellisensePopup(this.TaskList?.Projects ?? new List<string>(), GetRectFromCharacterIndex(_intelliPos));
                    break;

                case "@":
                    _intelliPos = this.CaretIndex - 1;
                    ShowIntellisensePopup(this.TaskList?.Contexts ?? new List<string>(), GetRectFromCharacterIndex(_intelliPos));
                    break;
                case "(":
                    if (this.CaretIndex == 1 ||
                        (this.CaretIndex == 12 && _startDateWithPriorityRegex.IsMatch(currentText.Substring(0, 12))))
                    {
                        _intelliPos = this.CaretIndex - 1;
                        ShowIntellisensePopup(_priorities, GetRectFromCharacterIndex(_intelliPos));
                    }
                    break;
            }
        }

        /// <summary>
        /// Get a rectangle representing the character at the specified index.
        /// This is a simplified implementation for Avalonia.
        /// </summary>
        private Rect GetRectFromCharacterIndex(int index)
        {
            // For now, return a simple rectangle at the bottom of the text box
            // In a more sophisticated implementation, we would calculate the actual character position
            return new Rect(0, this.Bounds.Height, 0, 0);
        }

        #endregion
    }
}
