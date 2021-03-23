namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IOwnable : ISynchronized
    {
        ISynchronized Owner { get; }
    }

    public interface IOwnable<TOwner> : IOwnable
        where TOwner : ISynchronized
    {
        new TOwner Owner { get; }
    }
}
