using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Core.Controls
{
    public partial class OptionsDialog : BaseDialog
    {
        public string ArchiveFile
        {
            get { return ArchiveFileTextBox.Text ?? ""; }
            set { ArchiveFileTextBox.Text = value ?? ""; }
        }

        public bool AutoArchive
        {
            get { return AutoArchiveCheckBox.IsChecked ?? false; }
            set { AutoArchiveCheckBox.IsChecked = value; }
        }

        public bool AutoSelectArchivePath
        {
            get { return AutoSelectArchivePathCheckBox.IsChecked ?? false; }
            set { AutoSelectArchivePathCheckBox.IsChecked = value; }
        }

        public string CurrentFont
        {
            get { return CurrentFontDisplayTextBox.Text ?? ""; }
            set { CurrentFontDisplayTextBox.Text = value ?? ""; }
        }

        public bool AddCreationDate
        {
            get { return AddCreationDateCheckBox.IsChecked ?? false; }
            set { AddCreationDateCheckBox.IsChecked = value; }
        }

        public bool MoveFocusToTaskListAfterAddingNewTask
        {
            get { return MoveFocusToTaskListAfterAddingNewTaskCheckBox.IsChecked ?? false; }
            set { MoveFocusToTaskListAfterAddingNewTaskCheckBox.IsChecked = value; }
        }

        public bool AutoRefresh
        {
            get { return AutoRefreshCheckBox.IsChecked ?? false; }
            set { AutoRefreshCheckBox.IsChecked = value; }
        }

        public bool CaseSensitiveFilter
        {
            get { return CaseSensitiveFilterCheckBox.IsChecked ?? false; }
            set { CaseSensitiveFilterCheckBox.IsChecked = value; }
        }

        public bool IntellisenseCaseSensitive
        {
            get { return IntellisenseCaseSensitiveCheckBox.IsChecked ?? false; }
            set { IntellisenseCaseSensitiveCheckBox.IsChecked = value; }
        }

        public bool MinToSysTray
        {
            get { return MinToSysTrayCheckBox.IsChecked ?? false; }
            set { MinToSysTrayCheckBox.IsChecked = value; }
        }

        public bool MinOnClose
        {
            get { return MinOnCloseCheckBox.IsChecked ?? false; }
            set { MinOnCloseCheckBox.IsChecked = value; }
        }

        public bool DebugOn
        {
            get { return DebugOnCheckBox.IsChecked ?? false; }
            set { DebugOnCheckBox.IsChecked = value; }
        }

        public bool RequireCtrlEnter
        {
            get { return RequireCtrlEnterCheckBox.IsChecked ?? false; }
            set { RequireCtrlEnterCheckBox.IsChecked = value; }
        }

        public bool AllowGrouping
        {
            get { return AllowGroupingCheckBox.IsChecked ?? false; }
            set { AllowGroupingCheckBox.IsChecked = value; }
        }

        public bool PreserveWhiteSpace
        {
            get { return PreserveWhiteSpaceCheckBox.IsChecked ?? false; }
            set { PreserveWhiteSpaceCheckBox.IsChecked = value; }
        }

        public bool WordWrap
        {
            get { return WordWrapCheckBox.IsChecked ?? false; }
            set { WordWrapCheckBox.IsChecked = value; }
        }

        public bool DisplayStatusBar
        {
            get { return DisplayStatusBarCheckBox.IsChecked ?? false; }
            set { DisplayStatusBarCheckBox.IsChecked = value; }
        }

        public bool CheckForUpdates
        {
            get { return CheckForUpdatesCheckBox.IsChecked ?? false; }
            set { CheckForUpdatesCheckBox.IsChecked = value; }
        }

        public OptionsDialog()
        {
            InitializeComponent();
        }

        private void SelectArchive_Click(object? sender, RoutedEventArgs e)
        {
            // TODO: Implement file dialog for archive file selection
            // For now, just log the action
            System.Diagnostics.Debug.WriteLine("Select archive file clicked - to be implemented");
        }

        private void SelectFont_Click(object? sender, RoutedEventArgs e)
        {
            // TODO: Implement font selection dialog
            // For now, just log the action
            System.Diagnostics.Debug.WriteLine("Select font clicked - to be implemented");
        }
    }
}
