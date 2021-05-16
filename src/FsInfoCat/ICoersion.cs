using System;

namespace FsInfoCat
{
    public interface ICoersion
    {
        object Cast(object obj);
        bool TryCoerce(object obj, out object result);
        Type ValueType { get; }
    }

    public interface ICoersion<T> : ICoersion
    {
        new T Cast(object obj);
        bool TryCoerce(object obj, out T result);
    }
}
