using System.ComponentModel;

namespace FsInfoCat
{
    public interface INotifyPropertyValueChanged : INotifyPropertyChanged
    {
        event PropertyValueChangedEventHandler PropertyValueChanged;
    }

    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);
}
