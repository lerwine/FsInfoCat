using System;
using System.Runtime.Serialization;

namespace FsInfoCat.Local
{
    [Serializable]
    internal class JobExtentException : Exception
    {
        public JobExtentException(bool isTimeout) : base(isTimeout ? "Maximum duration limit reached" : "Maximum item limit exceeded")
        {
            IsTimeout = isTimeout;
        }

        protected JobExtentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool IsTimeout { get; }
    }
}
