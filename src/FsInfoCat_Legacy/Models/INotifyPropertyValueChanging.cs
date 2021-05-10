using System.ComponentModel;

namespace FsInfoCat.Models
{
    public interface INotifyPropertyValueChanging : INotifyPropertyChanging
    {
        event PropertyValueChangeEventHandler PropertyValueChanging;
    }

    public interface INotifyPropertyValueChanging<T> : INotifyPropertyValueChanging
    {
        new event PropertyValueChangeEventHandler<T> PropertyValueChanging;
    }
}
