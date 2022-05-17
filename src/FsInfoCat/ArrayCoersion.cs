using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    // TODO: Document ArrayCoersion class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ArrayCoersion<T> : EnumerableCoersion<T, T[]>
    {
        public static readonly ArrayCoersion<T> Default = new();

        public ArrayCoersion(ICoersion<T> elementCoersion) : base(elementCoersion) { }

        public ArrayCoersion() : base() { }

        protected override T[] CreateFromEnumerable([AllowNull] IEnumerable<T> elements) => elements?.ToArray();

        protected override bool TryCreateFromEnumerable([AllowNull] IEnumerable<T> elements, out T[] result)
        {
            result = elements?.ToArray();
            return true;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
