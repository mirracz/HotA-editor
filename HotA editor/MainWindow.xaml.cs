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
                /*
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                dupa.ReadChars(8);
                TextB.AppendText("tutaj przeskakujemy" + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                */
                int IleBohaterow = 22;

                for (int i = 0; i < IleBohaterow; i++)
                {
                    TextB.AppendText("*** *** *** *** *** *** *** " + "***" + Environment.NewLine);
                    
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    dupa.ReadChars(8);
                    TextB.AppendText("tutaj przeskakujemy" + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadBytes(ref dupa, 137) + "***" + Environment.NewLine);
                }

                int IleKlas = 2;

                for (int i = 0; i < IleKlas; i++)
                {
                    TextB.AppendText(@"*** \/\/\/\/\/\/\/\/\/\/\/\/ " + "***" + Environment.NewLine);

                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    dupa.ReadChars(8);
                    TextB.AppendText("tutaj przeskakujemy" + "***" + Environment.NewLine);
                    TextB.AppendText(ReadString(ref dupa, dupa.ReadInt32()) + "***" + Environment.NewLine);
                    TextB.AppendText(ReadBytes(ref dupa, 117) + "***" + Environment.NewLine);
                }


                //137
                //TextB.AppendText(dupa.BaseStream.Position + "***" + Environment.NewLine);
   
                /*
                TextB.AppendText(dupa.ReadString() + "***" + Environment.NewLine);
                TextB.AppendText(dupa.ReadString() + "***" + Environment.NewLine);
                TextB.AppendText(dupa.ReadString() + "***" + Environment.NewLine);
                TextB.AppendText(dupa.ReadString() + "***" + Environment.NewLine);
                TextB.AppendText(dupa.ReadString() + "***" + Environment.NewLine);
                TextB.AppendText(dupa.ReadString() + "***" + dupa.BaseStream.Position);*/
                
            }
        }

        private string ReadString(ref BinaryReader stream, int length)
        {
            var a = new string(stream.ReadChars(length));
            return a;
        }

        private string ReadBytes(ref BinaryReader stream, int length)
        {
            var dupa = stream.ReadBytes(length);
            string dataString = String.Concat(dupa.Select(b => b.ToString("x2")));
            var a = dataString;
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DisplayValues();
        }
    }
}
