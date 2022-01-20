using System;

namespace FsInfoCat.BgOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpInfo : IAsyncOpInfo
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedAsyncOpInfo<T> : ITimedAsyncOpInfo, IAsyncOpInfo<T>
    {
    }
}
