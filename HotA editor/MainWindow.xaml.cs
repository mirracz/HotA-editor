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

        const string FileName = @"E:\Gry\DUPA!!!!\HotA.dat";

        void DisplayValues()
        {
            if (!File.Exists(FileName)) return;
            using (var reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                var dupa = reader;

                var s = new string(dupa.ReadChars(4));
                if (s != "HDAT")
                    return;

                dupa.ReadChars(8);

                const int ileBohaterow = 62;

                for (var i = 0; i < ileBohaterow; i++)
                {
                    Console.WriteLine(i);
                    var tmp = ReadString(ref dupa, dupa.ReadInt32());
                    TextB.AppendText("Nazwa pliku:       " + tmp + Environment.NewLine);
                    Console.WriteLine(tmp);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Ikona mała:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. krótka:      " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. długa:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx1               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx2               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    Console.WriteLine("xx3 = " + dupa.BaseStream.Position);
                    TextB.AppendText("Xxx3               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    Console.WriteLine("xx4 = " + dupa.BaseStream.Position);
                    TextB.AppendText("Xxx4               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    Console.WriteLine(dupa.BaseStream.Position.ToString());
                    var tmp2 = ReadBoolean(ref dupa);
                    TextB.AppendText("Dane Dodatkowe?    " + tmp2 + Environment.NewLine);
                    if (tmp2)
                    {
                        TextB.AppendText("Xxx6               " + ReadBytes(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    }
                    TextB.AppendText("Ile licz?          " + ReadInts(ref dupa, dupa.ReadInt32()) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                TextB.AppendText(dupa.BaseStream.Position.ToString());
            }
        }

        private string ReadString(ref BinaryReader stream, int length)
        {
            return new string(stream.ReadChars(length));
        }

        private Boolean ReadBoolean(ref BinaryReader stream)
        {
            return stream.ReadBoolean();
        }

        private string ReadBytes(ref BinaryReader stream, int length)
        {
            return String.Concat(stream.ReadBytes(length).Select(b => b.ToString("X2") + " "));
        }

        private string ReadInts(ref BinaryReader stream, int length)
        {
            var ret = string.Empty;
            for (int i = 0; i < length; i++)
            {
                ret += stream.ReadInt32() + " ";
            }
            return ret;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DisplayValues();
        }
    }
}
