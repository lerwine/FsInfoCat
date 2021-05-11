namespace FsInfoCat.ComponentSupport
{
    public interface IDisplayValue
    {
        object SourceValue { get; }

        string StringValue { get; }
    }

    public interface IDisplayValue<TValue> : IDisplayValue
    {
        new TValue SourceValue { get; }
    }
}
