using System;
using System.Runtime.Serialization;

namespace FsInfoCat.Local
{
    internal class JobExtentException(bool isTimeout) : Exception(isTimeout ? "Maximum duration limit reached" : "Maximum item limit exceeded")
    {
        public bool IsTimeout { get; } = isTimeout;
    }
}
