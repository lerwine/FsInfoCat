using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.Collections
{

    public class GeneralizableReadOnlyList<T> : ReadOnlyCollection<T>, IReadOnlyList<T>, IList
    {
        public GeneralizableReadOnlyList(IList<T> list) : base(list) { }
    }
}
