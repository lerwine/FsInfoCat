namespace FsInfoCat.Local
{
    public interface ILocalFileTag : ILocalItemTag, IFileTag, IHasMembershipKeyReference<ILocalFile, ITagDefinition>
    {
        new ILocalFile Tagged { get; }
    }
}
