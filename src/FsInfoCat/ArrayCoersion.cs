using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    public class ArrayCoersion<T> : EnumerableCoersion<T, T[]>
    {
        public static readonly ArrayCoersion<T> Default = new();

        public ArrayCoersion(ICoersion<T> elementCoersion) : base(elementCoersion) { }

        public ArrayCoersion() : base() { }

        protected override T[] CreateFromEnumerable([MaybeNull] IEnumerable<T> elements) => elements?.ToArray();

        protected override bool TryCreateFromEnumerable([MaybeNull] IEnumerable<T> elements, out T[] result)
        {
            result = elements?.ToArray();
            return true;
        }
    }
}
