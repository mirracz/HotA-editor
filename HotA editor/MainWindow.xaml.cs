using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

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
            using (var read = new BinaryReader(File.Open(FileName, FileMode.Open), Encoding.ASCII))
            {
                var reader = read;

                var s = ReadString(ref reader, 4) + reader.ReadInt32();
                if (s != "HDAT2")
                    return;

                var noOfFiles = reader.ReadInt32();

                for (var i = 0; i < noOfFiles; i++)
                {
                    var tmp = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Nazwa pliku:       " + tmp + Environment.NewLine);

                    var tmp1 = ReadString(ref reader, reader.ReadInt32());
                    TextB.AppendText("Katalog:           " + tmp1 + Environment.NewLine);

                    var tmp2 = ReadBytes(ref reader, 8);
                    //TextB.AppendText("***pozostałe opcje***" + Environment.NewLine + tmp2 + Environment.NewLine);

                    var tmp3 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Ikona duża:        " + tmp3 + Environment.NewLine);

                    var tmp4 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Ikona mała:        " + tmp4 + Environment.NewLine);

                    var tmp5 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Spec. krótka:      " + tmp5 + Environment.NewLine);

                    var tmp6 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Spec. długa:       " + tmp6 + Environment.NewLine);

                    var tmp7 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Xxx1               " + tmp7 + Environment.NewLine);

                    var tmp8 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Xxx2               " + tmp8 + Environment.NewLine);

                    var tmp9 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Xxx3               " + tmp9 + Environment.NewLine);

                    var tmp10 = ReadString(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Xxx4               " + tmp10 + Environment.NewLine);

                    var tmp11 = ReadBoolean(ref reader);
                    //TextB.AppendText("Dane Dodatkowe?    " + tmp11 + Environment.NewLine);
                    if (tmp11)
                    {
                        var tmp12 = ReadBytes(ref reader, reader.ReadInt32());
                        //TextB.AppendText("Xxx6               " + tmp12 + Environment.NewLine);
                    }

                    var tmp13 = ReadInts(ref reader, reader.ReadInt32());
                    //TextB.AppendText("Ile licz?          " + tmp13 + Environment.NewLine);
                }
            }
        }

        private static string ReadString(ref BinaryReader stream, int length)
        {
            return new string(stream.ReadChars(length));
        }

        private static Boolean ReadBoolean(ref BinaryReader stream)
        {
            return stream.ReadBoolean();
        }

        private static string ReadBytes(ref BinaryReader stream, int length)
        {
            return String.Concat(stream.ReadBytes(length).Select(b => b.ToString("X2") + " "));
        }

        private static string ReadInts(ref BinaryReader stream, int length)
        {
            var ret = string.Empty;
            for (var i = 0; i < length; i++)
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
