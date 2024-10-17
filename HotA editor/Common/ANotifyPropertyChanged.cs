using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotA_editor.Common;

public class ANotifyPropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (propertyName != null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public bool NotifyingSet<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value))
        {
            return false;
        }
        else
        {
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
    }
}
