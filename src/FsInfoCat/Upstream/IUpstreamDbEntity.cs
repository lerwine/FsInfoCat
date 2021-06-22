namespace FsInfoCat.Upstream
{
    public interface IUpstreamDbEntity : IDbEntity
    {
        IUserProfile CreatedBy { get; set; }

        IUserProfile ModifiedBy { get; set; }
    }
}
