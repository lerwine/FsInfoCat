using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IItemPropertyDefinition : IItemPropertyInfo
    {
        bool SupportsChangeEvents { get; }
        bool IsConvertibleFrom(ITypeDescriptorContext context, object value);
        bool IsConvertibleFrom(object value);
        string ConvertToString(ITypeDescriptorContext context, object value);
        string ConvertToString(object value);
        object ConvertFromString(ITypeDescriptorContext context, string text);
        object ConvertFromString(string text);
        object ConvertFrom(object value);
    }

    public interface IItemPropertyDefinition<TValue> : IItemPropertyDefinition
    {
        string ConvertToString(ITypeDescriptorContext context, TValue value);
        string ConvertToString(TValue value);
        new TValue ConvertFromString(ITypeDescriptorContext context, string text);
        new TValue ConvertFromString(string text);
        new TValue ConvertFrom(object value);
    }
}
