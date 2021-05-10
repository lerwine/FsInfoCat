using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.Collections
{

    public class GeneralizableReadOnlyList<T> : ReadOnlyCollection<T>, IGeneralizableReadOnlyList<T>
    {
        public GeneralizableReadOnlyList(IList<T> list) : base(list) { }
    }
}
