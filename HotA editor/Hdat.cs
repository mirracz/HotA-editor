using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotA_editor
{
    class Hdat
    {
        internal string ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (var read = new BinaryReader(File.Open(path, FileMode.Open), Encoding.GetEncoding("windows-1251")))
            {
                var reader = read;

                var s = ReadString(ref reader, 4) + reader.ReadInt32();
                if (s != "HDAT2")
                    return string.Empty;

                var noOfFiles = reader.ReadInt32();

                for (var i = 0; i < noOfFiles; i++)
                {
                    var name = ReadString(ref reader, reader.ReadInt32());

                    var folder = ReadString(ref reader, reader.ReadInt32());

                    ReadInts(ref reader, 2);

                    var tmp3 = ReadString(ref reader, reader.ReadInt32());

                    var tmp4 = ReadString(ref reader, reader.ReadInt32());

                    var tmp5 = ReadString(ref reader, reader.ReadInt32());

                    var tmp6 = ReadString(ref reader, reader.ReadInt32());

                    var tmp7 = ReadString(ref reader, reader.ReadInt32());

                    var tmp8 = ReadString(ref reader, reader.ReadInt32());

                    var tmp9 = ReadString(ref reader, reader.ReadInt32());

                    var tmp10 = ReadString(ref reader, reader.ReadInt32());

                    // chceck if extra data exist
                    if (ExtraDataExist(ref reader))
                    {
                        var tmp12 = ReadExtraData(ref reader, reader.ReadInt32());
                    }

                    var tmp13 = ReadInts(ref reader, reader.ReadInt32());
                }
            }

            return string.Empty;
        }

        private static string ReadString(ref BinaryReader stream, int length)
        {
            return new string(stream.ReadChars(length));
        }

        private static Boolean ExtraDataExist(ref BinaryReader stream)
        {
            return stream.ReadBoolean();
        }

        private static string ReadExtraData(ref BinaryReader stream, int length)
        {
            return String.Concat(stream.ReadBytes(length).Select(b => b.ToString("X2")));
        }

        private static int[] ReadInts(ref BinaryReader stream, int length)
        {
            var ret = new int[length];
            for (var i = 0; i < length; i++)
            {
                ret[i] += stream.ReadInt32();
            }
            return ret;
        }

        internal void WriteFile(string path)
        {
            using (var write = new BinaryWriter(File.Open(path, FileMode.Create), Encoding.GetEncoding("windows-1251")))
            {
                var writer = write;

                writer.Write(new []{'H', 'D', 'A', 'T'});
                writer.Write(2);

                //tmp
                var files = 3;

                writer.Write(files);

                for (var i = 0; i < files; i++)
                {
                    //var name = 
                    WriteString(ref writer, "test0");

                    //var folder = 
                    WriteString(ref writer, "0test");

                    WriteInts(ref writer, new []{9,0}, false);

                    //var tmp3 = 
                    WriteString(ref writer, "test1");

                    //var tmp4 = 
                    WriteString(ref writer, "test2");

                    //var tmp5 = 
                    WriteString(ref writer, "test4");

                    //var tmp6 = 
                    WriteString(ref writer, "test5");

                    //var tmp7 = 
                    WriteString(ref writer, "test6");

                    //var tmp8 = 
                    WriteString(ref writer, "test7" + Environment.NewLine + "test8");

                    //var tmp9 = 
                    WriteString(ref writer, "zażółć gęślą jaźń");

                    //var tmp10 = 
                    WriteString(ref writer, "test60");

                    var tmp12 = new[] {'a', 'e', 'i', 'o'};
                    // chceck if extra data exist
                    if (tmp12.Length > 0)
                    {
                        writer.Write(true);
                        WriteExtraData(ref writer, tmp12);
                    }
                    else
                    {
                        writer.Write(false);
                    }

                    int[] tmp13 = {28, 2, 3, 6, 189}; 
                    WriteInts(ref writer, tmp13, true);
                }
            }
        }

        private static void WriteString(ref BinaryWriter stream, string text)
        {
            stream.Write(text.Length);

            foreach (var t in text)
            {
                stream.Write(t);
            }
        }

        private static void WriteExtraData(ref BinaryWriter stream, char[] bytes)
        {
            stream.Write(bytes.Length);
            stream.Write(bytes);
        }

        private static void WriteInts(ref BinaryWriter stream, int[] bytes, bool writeLength)
        {
            if (writeLength)
                stream.Write(bytes.Length);

            foreach (var t in bytes)
            {
                stream.Write(t);
            }
        }
    }
}
