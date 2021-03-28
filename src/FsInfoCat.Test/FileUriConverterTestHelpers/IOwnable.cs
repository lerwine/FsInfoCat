namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IOwnable : ISynchronized
    {
        ISynchronized Owner { get; }
        string GetXPath();
    }

    public interface IOwnable<TOwner> : IOwnable
        where TOwner : ISynchronized
    {
        new TOwner Owner { get; }
    }
}
