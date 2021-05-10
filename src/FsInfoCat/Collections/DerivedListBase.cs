using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Collections
{
    public abstract class DerivedListBase<TSource, TDerived> : GeneralizableReadOnlyListBase<TDerived>
    {
        protected IList<TSource> Source { get; }

        private readonly IEqualityComparer<TDerived> _derivedValueComparer;

        protected DerivedListBase() : this(null, null) { }

        protected DerivedListBase(IList<TSource> source) : this(source, null) { }

        protected DerivedListBase(IEqualityComparer<TDerived> derivedValueComparer) : this(null, derivedValueComparer) { }

        protected DerivedListBase(IList<TSource> source, IEqualityComparer<TDerived> derivedValueComparer)
        {
            Source = source ?? new Collection<TSource>();
            _derivedValueComparer = derivedValueComparer ?? EqualityComparer<TDerived>.Default;
        }

        protected abstract TDerived GetDerivedValue(TSource item);

        public override TDerived this[int index] => GetDerivedValue(Source[index]);

        public override int Count => Source.Count;

        public override bool Contains(TDerived item) => Source.Any(i => _derivedValueComparer.Equals(GetDerivedValue(i), item));

        public override IEnumerator<TDerived> GetEnumerator() => Source.Select(i => GetDerivedValue(i)).GetEnumerator();

        public override int IndexOf(TDerived item) => Source.Select((e, i) => new { E = e, I = i })
            .Where(a => _derivedValueComparer.Equals(GetDerivedValue(a.E), item)).Select(a => a.I).DefaultIfEmpty(-1).First();

        protected override IEnumerator GetGenericEnumerator() => ((IEnumerable)Source.Select(i => GetDerivedValue(i))).GetEnumerator();
    }
}
