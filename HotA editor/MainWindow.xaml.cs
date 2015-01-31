using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotA_editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        const string fileName = "HotA.dat";

        void WriteDefaultValues()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(714);
                writer.Write(@"c:\Temp" + Environment.NewLine + "dupa");
                writer.Write(10);
                writer.Write(true);
            }
        }

        void DisplayValues()
        {
            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    var s = new string(reader.ReadChars(4));
                    TextB.AppendText(s + "<---- START ----" + Environment.NewLine);
                    TextB.AppendText(reader.BaseStream.Position + Environment.NewLine);
                    TextB.AppendText(reader.ReadString() + "***" + reader.BaseStream.Position + Environment.NewLine);
                    TextB.AppendText(reader.ReadString() + "***" + Environment.NewLine);
                    TextB.AppendText(reader.ReadString() + "***" + Environment.NewLine);
                    TextB.AppendText(reader.ReadString() + "***" + Environment.NewLine);
                    TextB.AppendText(reader.ReadString() + "***" + Environment.NewLine);
                    TextB.AppendText(reader.ReadString() + "***" + reader.BaseStream.Position);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //WriteDefaultValues();
            DisplayValues();
        }
    }
}
