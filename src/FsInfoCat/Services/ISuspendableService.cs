using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Services
{
    public interface IComparisonService
    {
        IEqualityComparer<T> GetEqualityComparer<T>();
        IEqualityComparer GetEqualityComparer(Type type);
        IComparer<T> GetComparer<T>();
        IComparer GetComparer(Type type);
    }

    public interface ISuspendableService
    {
        ISuspendableQueue<T> GetSuspendableQueue<T>();
    }

    public interface ISuspendableQueue<T>
    {

    }
}
