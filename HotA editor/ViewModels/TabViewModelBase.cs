using HotA_editor.Common;
using System.Collections.Generic;

namespace HotA_editor.ViewModels;

public abstract class TabViewModelBase : ANotifyPropertyChanged
{
    public List<DataHeader> DataHeaders { get; set; } =
    [
        new DataHeader("Data1"),
        new DataHeader("Data2"),
        new DataHeader("Data3"),
        new DataHeader("Data4"),
        new DataHeader("Data5"),
        new DataHeader("Data6"),
        new DataHeader("Data7"),
        new DataHeader("Data8"),
        new DataHeader("NewData")
    ];

    private int _selectedDataIndex;
    public int SelectedDataIndex
    {
        get { return _selectedDataIndex; }
        set
        {
            NotifyingSet(ref _selectedDataIndex, value);
            SelectData();
        }
    }

    protected abstract void SelectData();
}
