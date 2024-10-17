using HotA_editor.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HotA_editor.ViewModels;

public class EditViewModel : TabViewModelBase
{
    #region Properties

    private ObservableCollection<HdatEntry> _list = [];
    public ObservableCollection<HdatEntry> List
    {
        get { return _list; }
        set { NotifyingSet(ref _list, value); }
    }

    private HdatEntry _selectedEntry;
    public HdatEntry SelectedEntry
    {
        get { return _selectedEntry; }
        set
        {
            NotifyingSet(ref _selectedEntry, value);

            Debug.Assert(_selectedEntry == null || _selectedEntry.Data.Count == DataHeaders.Count);

            if (_selectedEntry == null)
            {
                for (int i = 0; i < DataHeaders.Count; i++)
                {
                    DataHeaders[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < _selectedEntry.Data.Count; i++)
                {
                    DataHeaders[i].Enabled = !string.IsNullOrEmpty(_selectedEntry.Data[i]);
                }
            }

            // if current data header is disabled in the new entry
            if (!DataHeaders[SelectedDataIndex].Enabled)
            {
                TextBoxEnabled = false;
                // Select the first non-disabled data header
                for (int i = 0; i < DataHeaders.Count; i++)
                {
                    if (DataHeaders[i].Enabled)
                    {
                        SelectedDataIndex = i;
                        break;
                    }
                }
            }
            else
            {
                SelectData();
            }
        }
    }

    private string _editedData;
    public string EditedData
    {
        get { return _editedData; }
        set
        {
            if (NotifyingSet(ref _editedData, value))
            {
                if (SelectedEntry != null && DataHeaders[SelectedDataIndex].Enabled)
                {
                    SelectedEntry.Data[SelectedDataIndex] = _editedData;
                }
            }
        }
    }

    private bool _textBoxEnabled;
    public bool TextBoxEnabled
    {
        get { return _textBoxEnabled; }
        set { NotifyingSet(ref _textBoxEnabled, value); }
    }

    #endregion

    #region Commands



    #endregion

    #region Methods

    protected override void SelectData()
    {
        if (SelectedEntry is HdatEntry entry && DataHeaders[SelectedDataIndex].Enabled)
        {
            _editedData = entry.Data[SelectedDataIndex];
            NotifyPropertyChanged(nameof(EditedData));
            TextBoxEnabled = true;
        }
        else
        {
            _editedData = null;
            NotifyPropertyChanged(nameof(EditedData));
            TextBoxEnabled = false;
        }
    }

    #endregion
}
