using System;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamTimeStampedEntity : ITimeStampedEntity
    {
        Guid CreatedById { get; }

        Guid ModifiedById { get; }
        IUserProfile CreatedBy { get; }
        IUserProfile ModifiedBy { get; }
    }
}
