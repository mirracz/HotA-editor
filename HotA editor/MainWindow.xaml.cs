using HotA_editor.Common;
using HotA_editor.Controls;
using HotA_editor.Data;
using HotA_editor.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
public partial class MainWindow
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

        EditVM = new EditViewModel();
        DiffVM = new DiffViewModel();

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

    #region Dependency Properties

    public EditViewModel EditVM
    {
        get { return (EditViewModel)GetValue(EditVMProperty); }
        set { SetValue(EditVMProperty, value); }
    }

    public static readonly DependencyProperty EditVMProperty = DependencyProperty.Register(nameof(EditVM), typeof(EditViewModel), typeof(MainWindow));

    public DiffViewModel DiffVM
    {
        get { return (DiffViewModel)GetValue(DiffVMProperty); }
        set { SetValue(DiffVMProperty, value); }
    }

    public static readonly DependencyProperty DiffVMProperty = DependencyProperty.Register(nameof(DiffVM), typeof(DiffViewModel), typeof(MainWindow));

    #endregion

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
            _editFileName = list.FileName;
            SetEditTitle();
            EditVM.List = new ObservableCollection<HdatEntry>(list);
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
                Hdat.WriteFile(fileDialog.FileName, EditVM.List, Encoding.GetEncoding("windows-1251"));
            }

            if (Save1250.IsChecked == true)
            {
                Hdat.WriteFile(fileDialog.FileName, EditVM.List, Encoding.GetEncoding("windows-1250"));
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

                    for (int j = 0; j < DiffVM.DataHeaders.Count; j++)
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
                    DiffVM.DiffList = new ObservableCollection<HdatDiffEntry>(diffList);
                }
            }
        }
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            _editFileName = _diff1List.FileName;
            SetEditTitle();
            EditVM.List = new ObservableCollection<HdatEntry>(_diff1List);

            if (DiffVM.SelectedDiffEntry != null)
            {
                EditVM.SelectedEntry = _diff1List.FirstOrDefault((e) => e.Name == DiffVM.SelectedDiffEntry.Name);
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
            _editFileName = _diff2List.FileName;
            SetEditTitle();
            EditVM.List = new ObservableCollection<HdatEntry>(_diff2List);
            if (DiffVM.SelectedDiffEntry != null)
            {
                EditVM.SelectedEntry = _diff2List.FirstOrDefault((e) => e.Name == DiffVM.SelectedDiffEntry.Name);
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
