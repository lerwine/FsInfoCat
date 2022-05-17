using System.ComponentModel;

namespace FsInfoCat
{
    // TODO: Document INotifyPropertyValueChanged interface
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface INotifyPropertyValueChanged : INotifyPropertyChanged
    {
        event PropertyValueChangedEventHandler PropertyValueChanged;
    }

    public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
