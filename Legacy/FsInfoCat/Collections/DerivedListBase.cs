using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Collections
{
    /// <summary>
    /// Base class for a list whose elements are derived from a source list.
    /// </summary>
    /// <typeparam name="TSource">The type of the element in the source list.</typeparam>
    /// <typeparam name="TDerived">The type of the derived element.</typeparam>
    /// <seealso cref="GeneralizableReadOnlyListBase{TDerived}" />
    public abstract class DerivedListBase<TSource, TDerived> : GeneralizableReadOnlyListBase<TDerived>
    {
        /// <summary>
        /// Gets the source list.
        /// </summary>
        /// <value>
        /// The source list.
        /// </value>
        protected IList<TSource> Source { get; }

        private readonly IEqualityComparer<TDerived> _derivedValueComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedListBase{TSource, TDerived}"/> class with a new, empty <see cref="Source"/> list.
        /// </summary>
        /// <param name="derivedValueComparer">The <see cref="IEqualityComparer{T}">IEqualityComparer</see><c>&lt<typeparamref name="TDerived"/>;&gt;</c> that
        /// is used to test whether two <typeparamref name="TDerived"/> values are equal.</param>
        /// <remarks>With this constructor, the default <see cref="IEqualityComparer{T}">IEqualityComparer</see><c>&lt<typeparamref name="TDerived"/>;&gt;</c>
        /// will ber used to test whether two <typeparamref name="TDerived"/> values are equal.</remarks>
        protected DerivedListBase() : this(null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedListBase{TSource, TDerived}"/> class.
        /// </summary>
        /// <param name="source">The source <seealso cref="IList{T}"/>.</param>
        /// <remarks>With this constructor, the default <see cref="IEqualityComparer{T}">IEqualityComparer</see><c>&lt<typeparamref name="TDerived"/>;&gt;</c>
        /// will ber used to test whether two <typeparamref name="TDerived"/> values are equal.</remarks>
        protected DerivedListBase(IList<TSource> source) : this(source, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedListBase{TSource, TDerived}"/> class with a new, empty <see cref="Source"/> list.
        /// </summary>
        /// <param name="derivedValueComparer">The <see cref="IEqualityComparer{T}">IEqualityComparer</see><c>&lt<typeparamref name="TDerived"/>;&gt;</c> that
        /// is used to test whether two <typeparamref name="TDerived"/> values are equal.</param>
        protected DerivedListBase(IEqualityComparer<TDerived> derivedValueComparer) : this(null, derivedValueComparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedListBase{TSource, TDerived}"/> class.
        /// </summary>
        /// <param name="source">The source <seealso cref="IList{T}"/>.</param>
        /// <param name="derivedValueComparer">The <see cref="IEqualityComparer{T}">IEqualityComparer</see><c>&lt<typeparamref name="TDerived"/>;&gt;</c> that
        /// is used to test whether two <typeparamref name="TDerived"/> values are equal.</param>
        protected DerivedListBase(IList<TSource> source, IEqualityComparer<TDerived> derivedValueComparer)
        {
            Source = source ?? new Collection<TSource>();
            _derivedValueComparer = derivedValueComparer ?? EqualityComparer<TDerived>.Default;
        }

        /// <summary>
        /// Gets the value derived from the <typeparamref name="TSource"/> element.
        /// </summary>
        /// <param name="item"><typeparamref name="TSource"/> list item.</param>
        /// <returns>The <typeparamref name="TDerived"/> value.</returns>
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
