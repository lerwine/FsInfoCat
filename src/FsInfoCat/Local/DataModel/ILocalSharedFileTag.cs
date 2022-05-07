namespace FsInfoCat.Local
{
    public interface ILocalSharedFileTag : ILocalSharedTag, ISharedFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalSharedTagDefinition> { }
}
