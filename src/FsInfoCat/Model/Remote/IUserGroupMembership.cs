namespace FsInfoCat.Model.Remote
{
    public interface IUserGroupMembership : IRemoteTimeStampedEntity
    {
        IUserProfile User { get; }
        IUserGroup Group { get; }
    }
}
