﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace HotA_editor.Data;

class Hdat
{
    internal static HDatList ReadFile(string path, Encoding enc)
    {
        var entries = new HDatList() { FileName = path };

        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        using (var read = new BinaryReader(File.Open(path, FileMode.Open), enc))
        {
            var reader = read;

            var s = ReadString(ref reader, 4) + reader.ReadInt32();
            if (s != "HDAT2")
                return null;

            var noOfFiles = reader.ReadInt32();

            for (var i = 0; i < noOfFiles; i++)
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var tmp = new HdatEntry
                {
                    Name = ReadString(ref reader, reader.ReadInt32()),
                    FolderName = ReadString(ref reader, reader.ReadInt32()),

                    Int1 = reader.ReadInt32(),
                    Int2 = reader.ReadInt32()
                };

                if (tmp.Int2 > 0)
                {
                    tmp.Data[0] = ReadString(ref reader, tmp.Int2);
                }
                else
                {
                    tmp.Data[0] = ReadString(ref reader, reader.ReadInt32());
                }

                tmp.Data[1] = ReadString(ref reader, reader.ReadInt32());
                tmp.Data[2] = ReadString(ref reader, reader.ReadInt32());
                tmp.Data[3] = ReadString(ref reader, reader.ReadInt32());
                tmp.Data[4] = ReadString(ref reader, reader.ReadInt32());
                tmp.Data[5] = ReadString(ref reader, reader.ReadInt32());
                tmp.Data[6] = ReadString(ref reader, reader.ReadInt32());
                tmp.Data[7] = ReadString(ref reader, reader.ReadInt32());

                if (tmp.Int2 > 0)
                {
                    tmp.Data[8] = ReadString(ref reader, reader.ReadInt32());
                }

                var exists = ExtraDataExist(ref reader);

                // chceck if extra data exist
                if (exists)
                {
                    tmp.Data9 = ReadExtraData(ref reader, reader.ReadInt32());
                }

                tmp.Data10 = ReadInts(ref reader, reader.ReadInt32());

                entries.Add(tmp);
            }
        }
        return entries;
    }

    private static string ReadString(ref BinaryReader stream, int length)
    {
        return new string(stream.ReadChars(length));
    }

    private static bool ExtraDataExist(ref BinaryReader stream)
    {
        return stream.ReadBoolean();
    }

    private static byte[] ReadExtraData(ref BinaryReader stream, int length)
    {
        return stream.ReadBytes(length);
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

    internal static readonly char[] hdat = ['H', 'D', 'A', 'T'];

    internal static void WriteFile(string path, ObservableCollection<HdatEntry> entries, Encoding enc)
    {
        using var write = new BinaryWriter(File.Open(path, FileMode.Create), enc);
        var writer = write;

        writer.Write(hdat);
        writer.Write(2);

        writer.Write(entries.Count);

        foreach (var t in entries)
        {
            WriteString(ref writer, t.Name);
            WriteString(ref writer, t.FolderName);

            writer.Write(t.Int1);

            if (t.Int2 == 0)
            {
                writer.Write(t.Int2);
            }

            WriteString(ref writer, t.Data[0]);
            WriteString(ref writer, t.Data[1]);
            WriteString(ref writer, t.Data[2]);
            WriteString(ref writer, t.Data[3]);
            WriteString(ref writer, t.Data[4]);
            WriteString(ref writer, t.Data[5]);
            WriteString(ref writer, t.Data[6]);
            WriteString(ref writer, t.Data[7]);

            if (t.Int2 > 0)
            {
                WriteString(ref writer, t.Data[8]);
            }

            // chceck if extra data exist
            if (t.Data9 != null)
            {
                writer.Write(true);
                WriteExtraData(ref writer, t.Data9);
            }
            else
            {
                writer.Write(false);
            }

            WriteInts(ref writer, t.Data10, true);
        }
    }

    private static void WriteString(ref BinaryWriter stream, string text)
    {
        if (text == null)
        {
            stream.Write(0);
            return;
        }

        stream.Write(text.Length);

        foreach (var t in text)
        {
            stream.Write(t);
        }
    }

    private static void WriteExtraData(ref BinaryWriter stream, byte[] bytes)
    {
        if (bytes == null)
        {
            stream.Write(0);
            return;
        }

        stream.Write(bytes.Length);
        stream.Write(bytes);
    }

    private static void WriteInts(ref BinaryWriter stream, int[] bytes, bool writeLength)
    {
        if (bytes == null)
        {
            stream.Write(0);
            return;
        }

        if (writeLength)
        {
            stream.Write(bytes.Length);
        }

        foreach (var t in bytes)
        {
            stream.Write(t);
        }
    }
}

class HDatList : List<HdatEntry>
{
    public string FileName { get; set; }
}
