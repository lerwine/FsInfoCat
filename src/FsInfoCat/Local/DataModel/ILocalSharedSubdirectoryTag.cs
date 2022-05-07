namespace FsInfoCat.Local
{
    public interface ILocalSharedSubdirectoryTag : ILocalSharedTag, ISharedSubdirectoryTag, ILocalSubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalSharedTagDefinition> { }
}
