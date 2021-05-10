namespace FsInfoCat.Models
{
    public interface IPropertyValueChangeEventArgs
    {
        string PropertyName { get; }
        object OldValue { get; }
        object NewValue { get; }
    }

    public interface IPropertyValueChangeEventArgs<T> : IPropertyValueChangeEventArgs
    {
        new T OldValue { get; }
        new T NewValue { get; }
    }
}
