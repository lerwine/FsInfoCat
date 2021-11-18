using System;

namespace FsInfoCat.BgOps
{
    public interface ITimedAsyncOpInfo : IAsyncOpInfo
    {
        public DateTime Started { get; }

        public TimeSpan Duration { get; }
    }

    public interface ITimedAsyncOpInfo<T> : ITimedAsyncOpInfo, IAsyncOpInfo<T>
    {
    }
}
//198.51.241.159
// 198.51.10.223

