using System;

namespace FsInfoCat
{
    public interface IAccessError : IDbEntity
    {
        Guid Id { get; set; }

        AccessErrorCode ErrorCode { get; set; }

        string Message { get; set; }

        string Details { get; set; }

        IDbEntity Target { get; set; }
    }


    public interface IAccessError<TTarget> : IAccessError
        where TTarget : IDbEntity
    {
        new TTarget Target { get; set; }
    }
}
