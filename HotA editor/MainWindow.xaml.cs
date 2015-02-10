using System.Text;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace HotA_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        readonly Hdat _hdatfile = new Hdat();
        private ObservableCollection<HdatEntry> _list = new ObservableCollection<HdatEntry>();
        private HdatEntry _selectedEntry;

        public MainWindow()
        {
            InitializeComponent();
            Title = "HotA editor v0.1";
        }

        public ObservableCollection<HdatEntry> List
        {
            get { return _list; }
            set { _list = value; NotifyPropertyChanged("List"); }
        }

        public HdatEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set { _selectedEntry = value; NotifyPropertyChanged("SelectedEntry"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog {Filter = "HotA.dat|HotA.dat|Wszystkie pliki (*.*)|*.*", FileName = "HotA.dat" };
                if (fileDialog.ShowDialog() != true) return;

                Grbox.IsEnabled = true;
                var fileName = fileDialog.FileName;
                Title = "HotA editor v0.1 -- " + fileName;

                if (Open1250.IsChecked == true)
                {
                    List = _hdatfile.ReadFile(fileName, Encoding.GetEncoding("windows-1250"));
                }

                if (Open1251.IsChecked == true)
                {
                    List = _hdatfile.ReadFile(fileName, Encoding.GetEncoding("windows-1251"));
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
                var fileDialog = new SaveFileDialog { Filter = "HotA.dat|HotA.dat|Wszystkie pliki (*.*)|*.*", FileName = "HotA.dat" };
                if (fileDialog.ShowDialog() != true) return;

                if (Save1251.IsChecked == true)
                {
                    _hdatfile.WriteFile(fileDialog.FileName, List, Encoding.GetEncoding("windows-1251"));
                }

                if (Save1250.IsChecked == true)
                {
                    _hdatfile.WriteFile(fileDialog.FileName, List, Encoding.GetEncoding("windows-1250"));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
