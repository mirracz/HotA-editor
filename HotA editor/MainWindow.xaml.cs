using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPFLocalizeExtension.Providers;

namespace HotA_editor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged
{
    private ObservableCollection<HdatEntry> _list = [];
    private HdatEntry _selectedEntry;

    public MainWindow()
    {
        var settingsLanguage = Properties.Settings.Default.Language;
        ResxLocalizationProvider.Instance.UpdateCultureList(GetType().Assembly.FullName, "Resources");
        WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
        Properties.Resources.Culture = new CultureInfo(settingsLanguage);
        WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = new CultureInfo(settingsLanguage);

        DataContext = this;
        InitializeComponent();
        Title = "HotA editor v0.2";
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        foreach (var item in LanguageMenu.Items.OfType<RadioMenuItem>())
        {
            if (item.Tag is string str && str == settingsLanguage)
            {
                item.IsChecked = true;
            }
        }
    }

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

    public ObservableCollection<HdatEntry> List
    {
        get { return _list; }
        set { _list = value; NotifyPropertyChanged(); }
    }

    public HdatEntry SelectedEntry
    {
        get { return _selectedEntry; }
        set 
        { 
            _selectedEntry = value; 
            NotifyPropertyChanged(); 

            Debug.Assert(_selectedEntry == null || _selectedEntry.Data.Count == DataHeaders.Count);

            for (int i = 0; i < _selectedEntry.Data.Count; i++)
            {
                DataHeaders[i].Enabled = !string.IsNullOrEmpty(_selectedEntry.Data[i]);
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

    private void SelectData()
    {
        if (SelectedEntry != null && DataHeaders[SelectedDataIndex].Enabled)
        {
            _editedData = SelectedEntry.Data[SelectedDataIndex];
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

    private string _editedData;
    public string EditedData
    {
        get { return _editedData; }
        set
        {
            if (_editedData != value)
            {
                _editedData = value;
                NotifyPropertyChanged();

                if (SelectedEntry != null && DataHeaders[SelectedDataIndex].Enabled)
                {
                    SelectedEntry.Data[SelectedDataIndex] = _editedData;
                }
            }
        }
    }

    private bool _textBoxEnabled;
    public bool TextBoxEnabled
    {
        get { return _textBoxEnabled; }
        set { _textBoxEnabled = value; NotifyPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string property = null)
    {
        if (property != null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "HotA.dat|HotA.dat|" + Properties.Resources.SaveLoadDialogueAllFiles + " (*.*)|*.*",
                FileName = "HotA.dat"
            };

            if (fileDialog.ShowDialog() != true)
                return;

            Grbox.IsEnabled = true;
            var fileName = fileDialog.FileName;
            Title = "HotA editor v0.2 -- " + fileName;

            if (Open1250.IsChecked == true)
            {
                List = Hdat.ReadFile(fileName, Encoding.GetEncoding("windows-1250"));
            }

            if (Open1251.IsChecked == true)
            {
                List = Hdat.ReadFile(fileName, Encoding.GetEncoding("windows-1251"));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
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

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is ListViewItem item && item.DataContext is DataHeader header && !header.Enabled)
        {
            e.Handled = true;
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
