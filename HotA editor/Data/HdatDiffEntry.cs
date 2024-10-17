using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotA_editor;

public class HdatDiffEntry : INotifyPropertyChanged, IHdatEntry
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; NotifyPropertyChanged(); }
    }

    public List<string> Data1 { get; } = [null, null, null, null, null, null, null, null, null];
    public List<string> Data2 { get; } = [null, null, null, null, null, null, null, null, null];

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
