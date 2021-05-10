using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public interface IGeneralizableReadOnlyList<T> : IReadOnlyList<T>, IList
    {
    }
}
