using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoTxt.Avalonia.Models
{
    /// <summary>
    /// Cross-platform application settings model
    /// </summary>
    public class ApplicationSettings : INotifyPropertyChanged
    {
        #region File and Archive Settings
        
        private string _filePath = string.Empty;
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        private string _archiveFilePath = string.Empty;
        public string ArchiveFilePath
        {
            get => _archiveFilePath;
            set => SetProperty(ref _archiveFilePath, value);
        }

        private bool _autoArchive = false;
        public bool AutoArchive
        {
            get => _autoArchive;
            set => SetProperty(ref _autoArchive, value);
        }

        private bool _autoSelectArchivePath = false;
        public bool AutoSelectArchivePath
        {
            get => _autoSelectArchivePath;
            set => SetProperty(ref _autoSelectArchivePath, value);
        }

        #endregion

        #region Window Settings
        
        private double _windowHeight = 400;
        public double WindowHeight
        {
            get => _windowHeight;
            set => SetProperty(ref _windowHeight, value);
        }

        private double _windowWidth = 400;
        public double WindowWidth
        {
            get => _windowWidth;
            set => SetProperty(ref _windowWidth, value);
        }

        private double _windowLeft = 0;
        public double WindowLeft
        {
            get => _windowLeft;
            set => SetProperty(ref _windowLeft, value);
        }

        private double _windowTop = 0;
        public double WindowTop
        {
            get => _windowTop;
            set => SetProperty(ref _windowTop, value);
        }

        #endregion

        #region Filter and Display Settings
        
        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value);
        }

        private bool _filterCaseSensitive = false;
        public bool FilterCaseSensitive
        {
            get => _filterCaseSensitive;
            set => SetProperty(ref _filterCaseSensitive, value);
        }

        private bool _filterFutureTasks = true;
        public bool FilterFutureTasks
        {
            get => _filterFutureTasks;
            set => SetProperty(ref _filterFutureTasks, value);
        }

        private bool _showHiddenTasks = false;
        public bool ShowHiddenTasks
        {
            get => _showHiddenTasks;
            set => SetProperty(ref _showHiddenTasks, value);
        }

        private int _currentSort = 0;
        public int CurrentSort
        {
            get => _currentSort;
            set => SetProperty(ref _currentSort, value);
        }

        private bool _allowGrouping = true;
        public bool AllowGrouping
        {
            get => _allowGrouping;
            set => SetProperty(ref _allowGrouping, value);
        }

        #endregion

        #region Font Settings
        
        private string _taskListFontFamily = "Segoe UI";
        public string TaskListFontFamily
        {
            get => _taskListFontFamily;
            set => SetProperty(ref _taskListFontFamily, value);
        }

        private double _taskListFontSize = 12;
        public double TaskListFontSize
        {
            get => _taskListFontSize;
            set => SetProperty(ref _taskListFontSize, value);
        }

        private string _taskListFontStyle = "Normal";
        public string TaskListFontStyle
        {
            get => _taskListFontStyle;
            set => SetProperty(ref _taskListFontStyle, value);
        }

        private string _taskListFontStretch = "Normal";
        public string TaskListFontStretch
        {
            get => _taskListFontStretch;
            set => SetProperty(ref _taskListFontStretch, value);
        }

        private string _taskListFontWeight = "Normal";
        public string TaskListFontWeight
        {
            get => _taskListFontWeight;
            set => SetProperty(ref _taskListFontWeight, value);
        }

        private string _taskListFontBrushColor = "Black";
        public string TaskListFontBrushColor
        {
            get => _taskListFontBrushColor;
            set => SetProperty(ref _taskListFontBrushColor, value);
        }

        #endregion

        #region Behavior Settings
        
        private bool _autoRefresh = false;
        public bool AutoRefresh
        {
            get => _autoRefresh;
            set => SetProperty(ref _autoRefresh, value);
        }

        private bool _addCreationDate = false;
        public bool AddCreationDate
        {
            get => _addCreationDate;
            set => SetProperty(ref _addCreationDate, value);
        }

        private bool _moveFocusToTaskListAfterAddingNewTask = true;
        public bool MoveFocusToTaskListAfterAddingNewTask
        {
            get => _moveFocusToTaskListAfterAddingNewTask;
            set => SetProperty(ref _moveFocusToTaskListAfterAddingNewTask, value);
        }

        private bool _requireCtrlEnter = false;
        public bool RequireCtrlEnter
        {
            get => _requireCtrlEnter;
            set => SetProperty(ref _requireCtrlEnter, value);
        }

        private bool _preserveWhiteSpace = false;
        public bool PreserveWhiteSpace
        {
            get => _preserveWhiteSpace;
            set => SetProperty(ref _preserveWhiteSpace, value);
        }

        private bool _wordWrap = false;
        public bool WordWrap
        {
            get => _wordWrap;
            set => SetProperty(ref _wordWrap, value);
        }

        private bool _intellisenseCaseSensitive = false;
        public bool IntellisenseCaseSensitive
        {
            get => _intellisenseCaseSensitive;
            set => SetProperty(ref _intellisenseCaseSensitive, value);
        }

        #endregion

        #region System Tray Settings
        
        private bool _minimiseToSystemTray = false;
        public bool MinimiseToSystemTray
        {
            get => _minimiseToSystemTray;
            set => SetProperty(ref _minimiseToSystemTray, value);
        }

        private bool _minimiseOnClose = false;
        public bool MinimiseOnClose
        {
            get => _minimiseOnClose;
            set => SetProperty(ref _minimiseOnClose, value);
        }

        #endregion

        #region UI Settings
        
        private bool _displayStatusBar = true;
        public bool DisplayStatusBar
        {
            get => _displayStatusBar;
            set => SetProperty(ref _displayStatusBar, value);
        }

        private bool _checkForUpdates = true;
        public bool CheckForUpdates
        {
            get => _checkForUpdates;
            set => SetProperty(ref _checkForUpdates, value);
        }

        #endregion

        #region Debug Settings
        
        private bool _debugLoggingOn = false;
        public bool DebugLoggingOn
        {
            get => _debugLoggingOn;
            set => SetProperty(ref _debugLoggingOn, value);
        }

        private bool _firstRun = true;
        public bool FirstRun
        {
            get => _firstRun;
            set => SetProperty(ref _firstRun, value);
        }

        #endregion

        #region Filter Presets
        
        private string _filterTextPreset1 = string.Empty;
        public string FilterTextPreset1
        {
            get => _filterTextPreset1;
            set => SetProperty(ref _filterTextPreset1, value);
        }

        private string _filterTextPreset2 = string.Empty;
        public string FilterTextPreset2
        {
            get => _filterTextPreset2;
            set => SetProperty(ref _filterTextPreset2, value);
        }

        private string _filterTextPreset3 = string.Empty;
        public string FilterTextPreset3
        {
            get => _filterTextPreset3;
            set => SetProperty(ref _filterTextPreset3, value);
        }

        private string _filterTextPreset4 = string.Empty;
        public string FilterTextPreset4
        {
            get => _filterTextPreset4;
            set => SetProperty(ref _filterTextPreset4, value);
        }

        private string _filterTextPreset5 = string.Empty;
        public string FilterTextPreset5
        {
            get => _filterTextPreset5;
            set => SetProperty(ref _filterTextPreset5, value);
        }

        private string _filterTextPreset6 = string.Empty;
        public string FilterTextPreset6
        {
            get => _filterTextPreset6;
            set => SetProperty(ref _filterTextPreset6, value);
        }

        private string _filterTextPreset7 = string.Empty;
        public string FilterTextPreset7
        {
            get => _filterTextPreset7;
            set => SetProperty(ref _filterTextPreset7, value);
        }

        private string _filterTextPreset8 = string.Empty;
        public string FilterTextPreset8
        {
            get => _filterTextPreset8;
            set => SetProperty(ref _filterTextPreset8, value);
        }

        private string _filterTextPreset9 = string.Empty;
        public string FilterTextPreset9
        {
            get => _filterTextPreset9;
            set => SetProperty(ref _filterTextPreset9, value);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}

