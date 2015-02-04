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

        const string FileName = "HotA.dat";

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

                const int ileBohaterow = 22;

                for (var i = 0; i < ileBohaterow; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Ikona mała:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. krótka:      " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. długa:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Opis specjalności: " + Environment.NewLine + "***" + Environment.NewLine + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine + "***" + Environment.NewLine);
                    TextB.AppendText("Imię:              " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Historia bohatera: " + Environment.NewLine + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 137) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                const int ileKlas = 2;

                for (var i = 0; i < ileKlas; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + Environment.NewLine);
                    TextB.AppendText("Nazwa klasy:       " + ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 117) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                const int ilePotworow = 18;

                for (var i = 0; i < ilePotworow; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Ikona mała:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. krótka:      " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. długa:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 157) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                const int ileZamkow = 1;

                for (var i = 0; i < ileZamkow; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Ikona mała:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. krótka:      " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. długa:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx1               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx2               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx3               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx4               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 377) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                const int CenyWZamku = 1;

                for (var i = 0; i < CenyWZamku; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + "***pozostałe opcje***" + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 20) + "***pozostałe opcje***" + Environment.NewLine);
                    TextB.AppendText("Nazwa:" + Environment.NewLine + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Opis dłuższy:" + Environment.NewLine + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + ReadBytes(ref dupa, 417) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                const int CenyBudynkow = 1;

                for (var i = 0; i < CenyBudynkow; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + "***pozostałe opcje***" + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 24) + "***pozostałe opcje***" + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Nazwa:" + Environment.NewLine + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + ReadBytes(ref dupa, 277) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                const int BitwaWZamku = 1;

                for (var i = 0; i < BitwaWZamku; i++)
                {
                    TextB.AppendText("Nazwa pliku:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Katalog:           " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 8) + Environment.NewLine);
                    TextB.AppendText("Ikona duża:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Ikona mała:        " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. krótka:      " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Spec. długa:       " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx1               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx2               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx3               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("Xxx4               " + ReadString(ref dupa, dupa.ReadInt32()) + Environment.NewLine);
                    TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + ReadBytes(ref dupa, 377) + Environment.NewLine);

                    TextB.AppendText(Environment.NewLine + Environment.NewLine);
                }

                TextB.AppendText(dupa.BaseStream.Position.ToString());
            }
        }

        private string ReadString(ref BinaryReader stream, int length)
        {
            return new string(stream.ReadChars(length));
        }

        private string ReadBytes(ref BinaryReader stream, int length)
        {
            var dupa = stream.ReadBytes(length);
            var dataString = String.Concat(dupa.Select(b => b.ToString("X2") + " "));
            return dataString;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DisplayValues();
        }
    }
}
