using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace TodoTxt.Avalonia.Core.Controls
{
    public partial class FilterDialog : BaseDialog
    {
        public string ActiveFilter
        {
            get { return ActiveFilterTextBox.Text ?? ""; }
            set { ActiveFilterTextBox.Text = value ?? ""; }
        }

        public List<string> PresetFilters { get; private set; }

        public FilterDialog()
        {
            InitializeComponent();
            PresetFilters = new List<string>();
        }

        public void SetPresetFilters(List<string> filters)
        {
            PresetFilters = filters ?? new List<string>();
            
            // Update the text boxes
            var textBoxes = new[] 
            {
                PresetFilter1TextBox, PresetFilter2TextBox, PresetFilter3TextBox,
                PresetFilter4TextBox, PresetFilter5TextBox, PresetFilter6TextBox,
                PresetFilter7TextBox, PresetFilter8TextBox, PresetFilter9TextBox
            };

            for (int i = 0; i < textBoxes.Length && i < PresetFilters.Count; i++)
            {
                textBoxes[i].Text = PresetFilters[i];
            }
        }

        public List<string> GetPresetFilters()
        {
            var textBoxes = new[] 
            {
                PresetFilter1TextBox, PresetFilter2TextBox, PresetFilter3TextBox,
                PresetFilter4TextBox, PresetFilter5TextBox, PresetFilter6TextBox,
                PresetFilter7TextBox, PresetFilter8TextBox, PresetFilter9TextBox
            };

            var filters = new List<string>();
            foreach (var textBox in textBoxes)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    filters.Add(textBox.Text.Trim());
                }
            }
            return filters;
        }

        private void FilterTextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnOkClick(this, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                OnCancelClick(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void ClearActive_Click(object? sender, RoutedEventArgs e)
        {
            ActiveFilterTextBox.Text = "";
        }

        private void ClearAll_Click(object? sender, RoutedEventArgs e)
        {
            ActiveFilterTextBox.Text = "";
            var textBoxes = new[] 
            {
                PresetFilter1TextBox, PresetFilter2TextBox, PresetFilter3TextBox,
                PresetFilter4TextBox, PresetFilter5TextBox, PresetFilter6TextBox,
                PresetFilter7TextBox, PresetFilter8TextBox, PresetFilter9TextBox
            };

            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
            }
        }
    }
}
