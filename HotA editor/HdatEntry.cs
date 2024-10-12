using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotA_editor;

public class HdatEntry : INotifyPropertyChanged
{
    private string _name;
    private string _folderName;

    public int Int1 { get; set; }
    public int Int2 { get; set; }

    private byte[] _data9;
    private int[] _data10;

    public List<string> Data { get; } = [null, null, null, null, null, null, null, null, null];

    public string Name
    {
        get { return _name; }
        set { _name = value; NotifyPropertyChanged(); }
    }

    public string FolderName
    {
        get { return _folderName; }
        set { _folderName = value; NotifyPropertyChanged(); }
    }

    public byte[] Data9
    {
        get { return _data9; }
        set { _data9 = value; NotifyPropertyChanged(); }
    }

    public int[] Data10
    {
        get { return _data10; }
        set { _data10 = value; NotifyPropertyChanged(); }
    }

    public string DisplayMember
    {
        get { return Name; }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string property = null)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(DisplayMember)));
        }
    }
}
