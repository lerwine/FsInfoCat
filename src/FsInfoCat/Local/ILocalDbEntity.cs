using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface ILocalDbEntity
    /// Implements the <see cref="IDbEntity" />
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface ILocalDbEntity : IDbEntity
    {
        Guid? UpstreamId { get; set; }

        DateTime? LastSynchronizedOn { get; set; }
    }
}
