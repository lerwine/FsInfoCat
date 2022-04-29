namespace FsInfoCat.Local
{
    public interface ILocalPersonalFileTag : ILocalPersonalTag, IPersonalFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalPersonalTagDefinition> { }
}
