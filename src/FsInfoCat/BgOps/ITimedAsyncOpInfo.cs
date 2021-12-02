using System;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpInfo : IAsyncOpInfo
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedAsyncOpInfo<T> : ITimedAsyncOpInfo, IAsyncOpInfo<T>
    {
    }
}
