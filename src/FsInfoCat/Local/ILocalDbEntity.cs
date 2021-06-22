using System;

namespace FsInfoCat.Local
{
    public interface ILocalDbEntity : IDbEntity
    {
        Guid? UpstreamId { get; set; }

        DateTime? LastSynchronizedOn { get; set; }
    }
}
