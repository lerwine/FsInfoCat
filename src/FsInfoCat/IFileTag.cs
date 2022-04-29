namespace FsInfoCat
{
    public interface IFileTag : IItemTag, IHasMembershipKeyReference<IFile, ITagDefinition>
    {
        new IFile Tagged { get; }
    }
}
