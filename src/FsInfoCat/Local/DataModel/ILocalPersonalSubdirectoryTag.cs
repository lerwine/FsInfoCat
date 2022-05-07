namespace FsInfoCat.Local
{
    public interface ILocalPersonalSubdirectoryTag : ILocalPersonalTag, IPersonalSubdirectoryTag, ILocalSubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalPersonalTagDefinition> { }
}
