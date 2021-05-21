using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ICoersion : IEqualityComparer
    {
        object Cast(object obj);
        object Convert(object obj);
        bool TryCast(object obj, out object result);
        bool TryCoerce(object obj, out object result);
        bool TryConvert(object obj, out object result);
        Type ValueType { get; }
    }

    public interface ICoersion<T> : ICoersion, IEqualityComparer<T>
    {
        new T Cast(object obj);
        new T Convert(object obj);
        bool TryCast(object obj, out T result);
        bool TryCoerce(object obj, out T result);
        bool TryConvert(object obj, out T result);
    }
}
