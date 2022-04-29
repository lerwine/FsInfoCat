namespace FsInfoCat.Local
{
    public interface ILocalSubdirectoryTag : ILocalItemTag, ISubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ITagDefinition>
    {
        new ILocalSubdirectory Tagged { get; }
    }
}
