namespace FsInfoCat.Model.Upstream
{
    public interface IUserGroupMembership : IUpstreamTimeStampedEntity
    {
        IUserProfile User { get; }
        IUserGroup Group { get; }
    }
}
