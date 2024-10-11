using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HotA_editor;

public class RadioMenuItem : MenuItem
{
    public RadioMenuItem() 
    { 
        IsCheckable = true;
        PreviewMouseLeftButtonUp += RadioMenuItem_PreviewMouseLeftButtonUp;
    }

    private void RadioMenuItem_PreviewMouseLeftButtonUp(object sender, RoutedEventArgs e)
    {
        if (IsChecked)
        {
            // prevents unchecking
            e.Handled = true;
        }
    }

    protected override void OnChecked(RoutedEventArgs e)
    {
        base.OnChecked(e);

        if (Parent is MenuItem parent)
        {
            foreach (var child in parent.Items.OfType<RadioMenuItem>())
            {
                if (child != this)
                {
                    child.IsChecked = false;
                }
            }
        }
    }
}
