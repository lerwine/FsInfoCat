namespace FsInfoCat.ComponentSupport
{
    public interface ITypedModelPropertyDescriptor<TValue> : ITypedModelProperty<TValue>, IModelPropertyDescriptor
    {
        new TValue GetValue(object model);
    }
}
