using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Test.Helpers
{
    public class ArrayEqualityHelper<T> : EnumerableEqualityHelper<T, IEnumerator>
        where T : class, IList
    {
        public static ArrayEqualityHelper<T> Default { get; }
        public IEqualityComparer ItemComparer { get; }

        static ArrayEqualityHelper()
        {
            Type t = typeof(T);
            Default = (t.IsArray) ? new ArrayEqualityHelper<T>() : null;
        }

        private ArrayEqualityHelper()
        {
            ItemComparer = (IEqualityComparer)typeof(EqualityHelper<>).MakeGenericType(typeof(T).GetElementType())
                .GetMethod("GetComparer").Invoke(null, new object[] { true });
        }

        public override int GetCount(T collection) => collection.Count;

        public override bool WithEnumerator(T collection, Func<IEnumerator, bool> getResultFunc) => getResultFunc(collection.GetEnumerator());

        protected override bool AreCurrentValuesEqual(IEnumerator x, IEnumerator y) => ItemComparer.Equals(x.Current, y.Current);
    }
}
