namespace FsInfoCat.Upstream
{
    public interface IGroupMembership : IUpstreamDbEntity
    {
        bool IsGroupAdmin { get; set; }

        IUserGroup Group { get; set; }

        IUserProfile User { get; set; }
    }
}
