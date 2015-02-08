using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace HotA_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        string _fileName;
        readonly Hdat _hdatfile = new Hdat();
        private List<HdatEntry> _entries; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog {Filter = "HotA.dat|HotA.dat|Wszystkie pliki (*.*)|*.*", FileName = "HotA.dat" };
                if (fileDialog.ShowDialog() != true) return;

                Grbox.IsEnabled = true;
                _fileName = fileDialog.FileName;
                TxtBoxFileName.Text = _fileName;
                _entries = _hdatfile.ReadFile(_fileName);

                TextB.Text = _entries[1].Name;
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

                _hdatfile.WriteFile(fileDialog.FileName, _entries);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
