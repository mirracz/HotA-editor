using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFLocalizeExtension.Providers;

namespace HotA_editor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged
{
    public const string TITLE_STRING = "HotA editor v0.2";

    private string _editFileName;
    private HDatList _diff1List;
    private HDatList _diff2List;

    public MainWindow()
    {
        var settingsLanguage = Properties.Settings.Default.Language;
        ResxLocalizationProvider.Instance.UpdateCultureList(GetType().Assembly.FullName, "Resources");
        WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
        Properties.Resources.Culture = new CultureInfo(settingsLanguage);
        WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = new CultureInfo(settingsLanguage);

        DataContext = this;
        InitializeComponent();
        Title = TITLE_STRING;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        foreach (var item in LanguageMenu.Items.OfType<RadioMenuItem>())
        {
            if (item.Tag is string str && str == settingsLanguage)
            {
                item.IsChecked = true;
            }
        }
    }

    #region Properties

    public List<DataHeader> DataHeaders { get; set; } =
    [
        new DataHeader("Data1"),
        new DataHeader("Data2"),
        new DataHeader("Data3"),
        new DataHeader("Data4"),
        new DataHeader("Data5"),
        new DataHeader("Data6"),
        new DataHeader("Data7"),
        new DataHeader("Data8"),
        new DataHeader("NewData")
    ];

    private int _selectedDataIndex;
    public int SelectedDataIndex
    {
        get { return _selectedDataIndex; }
        set
        {
            _selectedDataIndex = value;
            NotifyPropertyChanged();
            SelectData();
        }
    }

    private ObservableCollection<HdatEntry> _list = [];
    public ObservableCollection<HdatEntry> List
    {
        get { return _list; }
        set { _list = value; NotifyPropertyChanged(); }
    }

    private ObservableCollection<HdatDiffEntry> _diffList = [];
    public ObservableCollection<HdatDiffEntry> DiffList
    {
        get { return _diffList; }
        set { _diffList = value; NotifyPropertyChanged(); }
    }

    private HdatEntry _selectedEntry;
    public HdatEntry SelectedEntry
    {
        get { return _selectedEntry; }
        set
        {
            _selectedEntry = value;
            NotifyPropertyChanged();

            Debug.Assert(_selectedEntry == null || _selectedEntry.Data.Count == DataHeaders.Count);

            if (_selectedEntry == null)
            {
                for (int i = 0; i < DataHeaders.Count; i++)
                {
                    DataHeaders[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < _selectedEntry.Data.Count; i++)
                {
                    DataHeaders[i].Enabled = !string.IsNullOrEmpty(_selectedEntry.Data[i]);
                }
            }

            // if current data header is disabled in the new entry
            if (!DataHeaders[SelectedDataIndex].Enabled)
            {
                TextBoxEnabled = false;
                // Select the first non-disabled data header
                for (int i = 0; i < DataHeaders.Count; i++)
                {
                    if (DataHeaders[i].Enabled)
                    {
                        SelectedDataIndex = i;
                        break;
                    }
                }
            }
            else
            {
                SelectData();
            }
        }
    }

    private HdatDiffEntry _selectedDiffEntry;
    public HdatDiffEntry SelectedDiffEntry
    {
        get { return _selectedDiffEntry; }
        set
        {
            _selectedDiffEntry = value;
            NotifyPropertyChanged();

            Debug.Assert(_selectedDiffEntry == null || (_selectedDiffEntry.Data1.Count == DataHeaders.Count && _selectedDiffEntry.Data2.Count == DataHeaders.Count));

            for (int i = 0; i < _selectedDiffEntry.Data1.Count; i++)
            {
                DataHeaders[i].Enabled = !string.IsNullOrEmpty(_selectedDiffEntry.Data1[i]) || !string.IsNullOrEmpty(_selectedDiffEntry.Data2[i]);
            }

            // if current data header is disabled in the new entry
            if (!DataHeaders[SelectedDataIndex].Enabled)
            {
                // Select the first non-disabled data header
                for (int i = 0; i < DataHeaders.Count; i++)
                {
                    if (DataHeaders[i].Enabled)
                    {
                        SelectedDataIndex = i;
                        break;
                    }
                }
            }
            else
            {
                SelectData();
            }
        }
    }

    private void SelectData()
    {
        if (TabControl.SelectedIndex == 0)
        {
            if (SelectedEntry is HdatEntry entry && DataHeaders[SelectedDataIndex].Enabled)
            {
                _editedData = entry.Data[SelectedDataIndex];
                NotifyPropertyChanged(nameof(EditedData));
                TextBoxEnabled = true;
            }
            else
            {
                _editedData = null;
                NotifyPropertyChanged(nameof(EditedData));
                TextBoxEnabled = false;
            }
        }
        else if (TabControl.SelectedIndex == 1)
        {
            if (SelectedDiffEntry is HdatDiffEntry entry && DataHeaders[SelectedDataIndex].Enabled)
            {
                Diff1Data = entry.Data1[SelectedDataIndex];
                Diff2Data = entry.Data2[SelectedDataIndex];
            }
        }
    }

    private string _editedData;
    public string EditedData
    {
        get { return _editedData; }
        set
        {
            Debug.Assert(TabControl.SelectedIndex == 0);
            if (_editedData != value)
            {
                _editedData = value;
                NotifyPropertyChanged();

                if (SelectedEntry is HdatEntry entry && DataHeaders[SelectedDataIndex].Enabled)
                {
                    entry.Data[SelectedDataIndex] = _editedData;
                }
            }
        }
    }

    private string _diff1Data;
    public string Diff1Data
    {
        get { return _diff1Data; }
        set { _diff1Data = value; NotifyPropertyChanged(); }
    }

    private string _diff2Data;
    public string Diff2Data
    {
        get { return _diff2Data; }
        set { _diff2Data = value; NotifyPropertyChanged(); }
    }

    private bool _textBoxEnabled;
    public bool TextBoxEnabled
    {
        get { return _textBoxEnabled; }
        set { _textBoxEnabled = value; NotifyPropertyChanged(); }
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string property = null)
    {
        if (property != null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    #region Files

    private static HDatList OpenFile(bool use1250encoding)
    {
        try
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "HotA.dat|HotA.dat|" + Properties.Resources.SaveLoadDialogueAllFiles + " (*.*)|*.*",
                FileName = "HotA.dat"
            };

            if (fileDialog.ShowDialog() != true)
                return null;

            var fileName = fileDialog.FileName;

            if (use1250encoding)
            {
                return Hdat.ReadFile(fileName, Encoding.GetEncoding("windows-1250"));
            }
            else
            {
                return Hdat.ReadFile(fileName, Encoding.GetEncoding("windows-1251"));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        var list = OpenFile(Open1250.IsChecked == true);
        if (list != null)
        {
            Grbox.IsEnabled = true;
            _editFileName = list.FileName;
            SetEditTitle();
            List = new ObservableCollection<HdatEntry>(list);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = "HotA.dat|HotA.dat|" + Properties.Resources.SaveLoadDialogueAllFiles + " (*.*)|*.*",
                FileName = "HotA.dat"
            };

            if (fileDialog.ShowDialog() != true)
                return;

            if (Save1251.IsChecked == true)
            {
                Hdat.WriteFile(fileDialog.FileName, List, Encoding.GetEncoding("windows-1251"));
            }

            if (Save1250.IsChecked == true)
            {
                Hdat.WriteFile(fileDialog.FileName, List, Encoding.GetEncoding("windows-1250"));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void OpenDiff1_Click(object sender, RoutedEventArgs e)
    {
        var list = OpenFile(OpenDiff1_1250.IsChecked == true);
        if (list != null)
        {
            _diff1List = list;
            Diff1Confirm.Visibility = Visibility.Visible;
            SetDiffTitle();
            TryCompareDiff();
        }
    }

    private void OpenDiff2_Click(object sender, RoutedEventArgs e)
    {
        var list = OpenFile(OpenDiff2_1250.IsChecked == true);
        if (list != null)
        {
            _diff2List = list;
            Diff2Confirm.Visibility = Visibility.Visible;
            SetDiffTitle();
            TryCompareDiff();
        }
    }

    #endregion

    #region Languages

    private void MenuItemLanguageEn_Click(object sender, RoutedEventArgs e)
    {
        SetLanguage("en");
    }

    private void MenuItemLanguageCz_Click(object sender, RoutedEventArgs e)
    {
        SetLanguage("cs");
    }

    private void MenuItemLanguagePl_Click(object sender, RoutedEventArgs e)
    {
        SetLanguage("pl");
    }

    private static void SetLanguage(string languageCode)
    {
        Properties.Resources.Culture = new CultureInfo(languageCode);
        WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = new CultureInfo(languageCode);

        Properties.Settings.Default.Language = languageCode;
        Properties.Settings.Default.Save();
    }

    #endregion

    private void TryCompareDiff()
    {
        if (_diff1List != null && _diff2List != null) 
        {
            // Currently both files need to have same entries
            var intersection = _diff1List.Intersect(_diff2List, new HdatEntryNameEqualityComparer());
            if (_diff1List.Count == _diff2List.Count && intersection.Count() == _diff1List.Count) 
            {
                var diffList = new List<HdatDiffEntry>();
                for (int i = 0; i < _diff1List.Count; i++)
                {
                    var entry1 = _diff1List[i];
                    var entry2 = _diff2List[i];
                    var diffEntry = new HdatDiffEntry() { Name = entry1.Name };
                    var set = false;

                    for (int j = 0; j < DataHeaders.Count; j++)
                    {
                        if (entry1.Data[j] != entry2.Data[j])
                        {
                            diffEntry.Data1[j] = entry1.Data[j];
                            diffEntry.Data2[j] = entry2.Data[j];
                            set = true;
                        }
                    }

                    if (set)
                    {
                        diffList.Add(diffEntry);
                    }
                }

                if (diffList.Count > 0)
                {
                    DiffList = new ObservableCollection<HdatDiffEntry>(diffList);
                }
            }
        }
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is ListViewItem item && item.DataContext is DataHeader header && !header.Enabled)
        {
            e.Handled = true;
        }
    }

    #region Tabs

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is TabControl tabControl)
        {
            if (tabControl.SelectedIndex == 0)
            {
                SetEditTitle();
            }
            else if (tabControl.SelectedIndex == 1)
            {
                SetDiffTitle();
            }
        }
    }

    private void SetEditTitle()
    {
        if (_editFileName == null)
        {
            Title = TITLE_STRING;
        }
        else
        {
            Title = TITLE_STRING + " -- " + Properties.Resources.TabEdit + " -- " + _editFileName;
        }
    }

    private void SetDiffTitle()
    {
        if (_diff1List == null && _diff2List == null)
        {
            Title = TITLE_STRING;
        }
        else
        {
            Title = string.Format("{0} -- {1} -- {2} || {3}", TITLE_STRING, Properties.Resources.TabDiff, _diff1List?.FileName ?? "???", _diff2List?.FileName ?? "???");
        }
    }

    #endregion

    private ICommand _goToEditFromDiff1Command;
    public ICommand GoToEditFromDiff1Command
    {
        get
        {
            return _goToEditFromDiff1Command ??= new SimpleCommand(GoToEditFromDiff1);
        }
    }

    private void GoToEditFromDiff1(object parameter)
    {
        if (_diff1List != null)
        {
            TabControl.SelectedIndex = 0;
            Grbox.IsEnabled = true;
            _editFileName = _diff1List.FileName;
            SetEditTitle();
            List = new ObservableCollection<HdatEntry>(_diff1List);

            if (SelectedDiffEntry != null)
            {
                SelectedEntry = _diff1List.FirstOrDefault((e) => e.Name == SelectedDiffEntry.Name);
            }
        }
    }

    private ICommand _goToEditFromDiff2Command;
    public ICommand GoToEditFromDiff2Command
    {
        get
        {
            return _goToEditFromDiff2Command ??= new SimpleCommand(GoToEditFromDiff2);
        }
    }

    private void GoToEditFromDiff2(object parameter)
    {
        if (_diff2List != null)
        {
            TabControl.SelectedIndex = 0;
            Grbox.IsEnabled = true;
            _editFileName = _diff2List.FileName;
            SetEditTitle();
            List = new ObservableCollection<HdatEntry>(_diff2List);
            if (SelectedDiffEntry != null)
            {
                SelectedEntry = _diff2List.FirstOrDefault((e) => e.Name == SelectedDiffEntry.Name);
            }
        }
    }
}

public class DataHeader(string name) : INotifyPropertyChanged
{
    public string Name { get; set; } = name;

    private bool _enabled;
    public bool Enabled 
    {
        get { return _enabled; } 
        set { _enabled = value; NotifyPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string property = null)
    {
        if (property != null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

public class HdatEntryNameEqualityComparer : IEqualityComparer<HdatEntry>
{
    public bool Equals(HdatEntry x, HdatEntry y)
    {
        return x.Name == y.Name;
    }

    public int GetHashCode([DisallowNull] HdatEntry obj)
    {
        return obj.Name?.GetHashCode() ?? 0;
    }
}

public class SimpleCommand(Func<object, bool> canExecute, Action<object> execute) : ICommand
{
    public SimpleCommand(Action<object> execute) : this(null, execute) { }

    private readonly Func<object, bool> _canExecute = canExecute ?? new Func<object, bool>(param => true);
    private readonly Action<object> _execute = execute;

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return _canExecute.Invoke(parameter);
    }

    public void Execute(object parameter)
    {
        _execute.Invoke(parameter);
    }
}
