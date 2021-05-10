using System.ComponentModel;

namespace FsInfoCat.Models
{
    public interface INotifyPropertyValueChanged : INotifyPropertyChanged
    {
        event PropertyValueChangeEventHandler PropertyValueChanged;
    }

    public interface INotifyPropertyValueChanged<T> : INotifyPropertyValueChanged
    {
        new event PropertyValueChangeEventHandler<T> PropertyValueChanged;
    }
}
