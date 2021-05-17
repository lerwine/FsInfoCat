using System;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteTimeStampedEntity : ITimeStampedEntity
    {
        Guid CreatedById { get; }

        Guid ModifiedById { get; }
        IUserProfile CreatedBy { get; }
        IUserProfile ModifiedBy { get; }
    }
}
