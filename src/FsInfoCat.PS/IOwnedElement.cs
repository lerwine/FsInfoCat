namespace FsInfoCat.PS
{
    public interface IOwnedElement<TOwner> : ISynchonizedElement
        where TOwner : ISynchonizedElement
    {
        TOwner Owner { get; }
    }
}
