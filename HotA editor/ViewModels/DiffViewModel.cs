using HotA_editor.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HotA_editor.ViewModels;

public class DiffViewModel : TabViewModelBase
{
    #region Properties

    private ObservableCollection<HdatDiffEntry> _diffList = [];
    public ObservableCollection<HdatDiffEntry> DiffList
    {
        get { return _diffList; }
        set { NotifyingSet(ref _diffList, value); }
    }

    private HdatDiffEntry _selectedDiffEntry;
    public HdatDiffEntry SelectedDiffEntry
    {
        get { return _selectedDiffEntry; }
        set
        {
            NotifyingSet(ref _selectedDiffEntry, value);

            Debug.Assert(_selectedDiffEntry == null || (_selectedDiffEntry.Data1.Count == DataHeaders.Count && _selectedDiffEntry.Data2.Count == DataHeaders.Count));

            for (int i = 0; i < _selectedDiffEntry.Data1.Count; i++)
            {
                DataHeaders[i].Enabled = !string.IsNullOrEmpty(_selectedDiffEntry.Data1[i]) || !string.IsNullOrEmpty(_selectedDiffEntry.Data2[i]);
            }

            // if current data header is disabled in the new entry
            if (!DataHeaders[SelectedDataIndex].Enabled)
            {
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

    private string _diff1Data;
    public string Diff1Data
    {
        get { return _diff1Data; }
        set { NotifyingSet(ref _diff1Data, value); }
    }

    private string _diff2Data;
    public string Diff2Data
    {
        get { return _diff2Data; }
        set { NotifyingSet(ref _diff2Data, value); }
    }

    #endregion

    #region Commands



    #endregion

    #region Methods

    protected override void SelectData()
    {
        if (SelectedDiffEntry != null && DataHeaders[SelectedDataIndex].Enabled)
        {
            Diff1Data = SelectedDiffEntry.Data1[SelectedDataIndex];
            Diff2Data = SelectedDiffEntry.Data2[SelectedDataIndex];
        }
    }

    #endregion
}
