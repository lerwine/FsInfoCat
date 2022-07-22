using System.Collections.Generic;
using System.Xml.Linq;

namespace DevUtil
{
    public interface IXObjectSelector<T> where T : XObject
    {
        IEnumerable<T> GetItems();

        bool Any();

        T FirstOrDefault();

        T LastOrDefault();

        T ItemAtOrDefault(int index);

        IXObjectSelector<T> Concat(params T[] other);

        IXObjectSelector<T> Concat(IEnumerable<T> other);
    }
}
