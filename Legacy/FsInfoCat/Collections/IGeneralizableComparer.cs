using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public interface IGeneralizableComparer<T> : IComparer<T>, IComparer
    {
    }
}
