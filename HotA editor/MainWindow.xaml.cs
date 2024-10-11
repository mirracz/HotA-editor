using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
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

    public ObservableCollection<HdatEntry> List
    {
        get { return _list; }
        set { _list = value; NotifyPropertyChanged(); }
    }

    public HdatEntry SelectedEntry
    {
        get { return _selectedEntry; }
        set { _selectedEntry = value; NotifyPropertyChanged(); }
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
}

public class TextToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return !string.IsNullOrEmpty((string)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return string.Empty;
    }
}
