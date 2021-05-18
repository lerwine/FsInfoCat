using System;

namespace FsInfoCat.Model.Local
{
    public interface ILocalModel
    {
        Guid? UpstreamId { get; }
        DateTime? LastSynchronized { get; }
    }
}
