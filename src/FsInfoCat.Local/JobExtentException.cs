using System;
using System.Runtime.Serialization;

namespace FsInfoCat.Local
{
    internal class JobExtentException : Exception
    {
        public JobExtentException(bool isTimeout) : base(isTimeout ? "Maximum duration limit reached" : "Maximum item limit exceeded")
        {
            IsTimeout = isTimeout;
        }

        public bool IsTimeout { get; }
    }
}
