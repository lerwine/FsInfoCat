using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public interface IGeneralizableEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
    }
}
