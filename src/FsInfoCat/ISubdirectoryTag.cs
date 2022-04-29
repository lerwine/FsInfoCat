namespace FsInfoCat
{
    public interface ISubdirectoryTag : IItemTag, IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
    {
        new ISubdirectory Tagged { get; }
    }
}
