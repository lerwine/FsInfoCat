namespace FsInfoCat.Models
{
    public delegate void PropertyValueChangeEventHandler(object sender, IPropertyValueChangeEventArgs e);

    public delegate void PropertyValueChangeEventHandler<T>(object sender, IPropertyValueChangeEventArgs<T> e);
}
