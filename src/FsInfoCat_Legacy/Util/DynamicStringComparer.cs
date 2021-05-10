using FsInfoCat.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Util
{
    /// <summary>
    /// A culture-invariant <seealso cref="StringComparer"/> whereby the case-sensitivity may be changed dynamically.
    /// </summary>
    /// <remarks>Empty and null strings are considered to be equal as are empty and null enumerable string values.</remarks>
    public class DynamicStringComparer : StringComparer, IEqualityComparer<IEnumerable<string>>, INotifyPropertyValueChanging<bool>, INotifyPropertyValueChanged<bool>
    {
        public static readonly StringComparer CASE_SENSITIVE = InvariantCulture;
        public static readonly StringComparer IGNORE_CASE = InvariantCultureIgnoreCase;

        private StringComparer _current;

        public event PropertyValueChangeEventHandler<bool> PropertyValueChanging;
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyValueChangeEventHandler<bool> PropertyValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning disable IDE1006 // Naming Styles
        private event PropertyValueChangeEventHandler _propertyValueChanging;
        private event PropertyValueChangeEventHandler _propertyValueChanged;
#pragma warning restore IDE1006 // Naming Styles
        event PropertyValueChangeEventHandler INotifyPropertyValueChanging.PropertyValueChanging
        {
            add => _propertyValueChanging += value;
            remove => _propertyValueChanging -= value;
        }

        event PropertyValueChangeEventHandler INotifyPropertyValueChanged.PropertyValueChanged
        {
            add => _propertyValueChanged += value;
            remove => _propertyValueChanged -= value;
        }

        /// <summary>
        /// A value that changes the case-sensitivity of the comparer.
        /// </summary>
        public bool CaseSensitive
        {
            get => ReferenceEquals(_current, CASE_SENSITIVE);
            set
            {
                bool oldValue = ReferenceEquals(_current, CASE_SENSITIVE);
                if (oldValue == value)
                    return;
                PropertyValueChangingEventArgs<bool> changingEventArgs = new PropertyValueChangingEventArgs<bool>(nameof(CaseSensitive), oldValue, value);
                PropertyValueChanging?.Invoke(this, changingEventArgs);
                _propertyValueChanging?.Invoke(this, changingEventArgs);
                PropertyChanging?.Invoke(this, changingEventArgs);
                _current = value ? CASE_SENSITIVE : IGNORE_CASE;
                PropertyValueChangedEventArgs<bool> changedEventArgs = new PropertyValueChangedEventArgs<bool>(nameof(CaseSensitive), oldValue, value);
                try { PropertyValueChanged?.Invoke(this, changedEventArgs); }
                finally
                {
                    try { _propertyValueChanged?.Invoke(this, changedEventArgs); }
                    finally { PropertyChanged?.Invoke(this, changedEventArgs); }
                }
            }
        }

        public DynamicStringComparer(bool caseSensitive)
        {
            _current = caseSensitive ? CASE_SENSITIVE : IGNORE_CASE;
        }

        /// <summary>
        /// Compares two <seealso cref="string"/> values for sorting.
        /// </summary>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><code>&lt; 0</code> if <paramref name="x"/> is alphabetically less than <paramref name="y"/>; <code>&gt; 0</code> if <paramref name="x"/>
        /// is greater than <paramref name="y"/>; otherwise <code>0</code> if they are equal.</returns>
        /// <remarks>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</remarks>
        public override int Compare(string x, string y) => string.IsNullOrEmpty(x) ? (string.IsNullOrEmpty(y) ? 0 : -1) :
            (string.IsNullOrEmpty(y) ? 1 : _current.Compare(x, y));

        /// <summary>
        /// Indicates whether two <seealso cref="string">strings</seealso> are equal.
        /// </summary>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <see langword="false"/>.</returns>
        /// <remarks>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</remarks>
        public override bool Equals(string x, string y) => string.IsNullOrEmpty(x) ? string.IsNullOrEmpty(y) : (!string.IsNullOrEmpty(y) && _current.Equals(x, y));

        /// <summary>
        /// Gets the hash code for a <seealso cref="string"/> value.
        /// </summary>
        /// <param name="obj">The <seealso cref="string"/> value from which to calculate a hashcode.</param>
        /// <returns>The calculated hashcode value.</returns>
        /// <remarks>A <see langword="null"/> value will return the same hashcode as an empty string.</remarks>
        public override int GetHashCode(string obj) => _current.GetHashCode(obj ?? "");

        /// <summary>
        /// Indicates whether two <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> values are equal.
        /// </summary>
        /// <param name="x">The <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> to compare to <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> are sequentially equal and have the same number of elements;
        /// otherwise, <see langword="false"/>.</returns>
        /// <remarks>A <see langword="null"/> value is considered to be equal to one that has no elements. Likewise, a <see langword="null"/> element
        /// is considered to be equal to an empty one.</remarks>
        public bool Equals(IEnumerable<string> x, IEnumerable<string> y)
        {
            if (x is null)
            {
                if (y is null)
                    return true;
                using (IEnumerator<string> e = y.GetEnumerator())
                    return !e.MoveNext();
            }
            using (IEnumerator<string> a = x.GetEnumerator())
            {
                if (y is null)
                    return !a.MoveNext();
                using (IEnumerator<string> b = y.GetEnumerator())
                {
                    while (a.MoveNext())
                    {
                        if (!(b.MoveNext() && Equals(a.Current, b.Current)))
                            return false;
                    }
                    return !b.MoveNext();
                }
            }
        }

        /// <summary>
        /// Gets the aggregated hashcode for an <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso>.
        /// </summary>
        /// <param name="obj">The <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> from which to create an aggregated hashcode.</param>
        /// <returns>The aggregated hashcode of all the elements in the <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso>.</returns>
        /// <remarks>A <see langword="null"/> value will return the same hashcode as one that has no elements.</remarks>
        public int GetHashCode(IEnumerable<string> obj) => (obj is null) ? 0 : obj.Aggregate(0, (x, y) => x ^ _current.GetHashCode(y ?? ""));

        /// <summary>
        /// Compare string values with case-insensitive preference.
        /// </summary>
        /// <param name="other">An alternate comparer which will be used if the current is case sensitive and the other is not.</param>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <see langword="false"/>.</returns>
        /// <remarks>If the current comparer is <see cref="CaseSensitive"/> and <paramref name="other"/> is not null or <see cref="CaseSensitive"/>,
        /// then the <paramref name="other"/> comparer will make the comparison; otherwise, the current comparer will be used.
        /// <para>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</para></remarks>
        internal bool Equals(DynamicStringComparer other, string x, string y) => (other is null || other.CaseSensitive) ? Equals(x, y) : other.Equals(x, y);

        /// <summary>
        /// Compare <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> values with case-insensitive preference.
        /// </summary>
        /// <param name="other">An alternate comparer which will be used if the current is case sensitive and the other is not.</param>
        /// <param name="x">The <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="IEnumerable{T}"><c>&lt;<seealso cref="string"/>&gt;</c></seealso> to compare to <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> are sequentially equal and have the same number of elements;
        /// otherwise, <see langword="false"/>.</returns>
        /// <remarks>If the current comparer is <see cref="CaseSensitive"/> and <paramref name="other"/> is not null or <see cref="CaseSensitive"/>,
        /// then the <paramref name="other"/> comparer will make the comparison; otherwise, the current comparer will be used.
        /// <para>A <see langword="null"/> value is considered to be equal to one that has no elements.</para></remarks>
        internal bool Equals(DynamicStringComparer other, IEnumerable<string> x, IEnumerable<string> y) =>
            (other is null || other.CaseSensitive) ? Equals(x, y) : other.Equals(x, y);

        /// <summary>
        /// Indicates whether two <seealso cref="string">strings</seealso> are equal, with case indifference.
        /// </summary>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <see langword="false"/>.</returns>
        /// <remarks>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</remarks>
        public static bool IgnoreCaseEquals(string x, string y) => string.IsNullOrEmpty(x) ? string.IsNullOrEmpty(y) : (!string.IsNullOrEmpty(y) && IGNORE_CASE.Equals(x, y));

        /// <summary>
        /// Indicates whether two <seealso cref="string">strings</seealso> are equal, using case-sensitive comparison.
        /// </summary>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, <see langword="false"/>.</returns>
        /// <remarks>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</remarks>
        public static bool CaseSensitiveEquals(string x, string y) => string.IsNullOrEmpty(x) ? string.IsNullOrEmpty(y) : (!string.IsNullOrEmpty(y) && CASE_SENSITIVE.Equals(x, y));

        /// <summary>
        /// Compares two <seealso cref="string"/> values for sorting, with case indifference.
        /// </summary>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><code>&lt; 0</code> if <paramref name="x"/> is alphabetically less than <paramref name="y"/>; <code>&gt; 0</code> if <paramref name="x"/>
        /// is greater than <paramref name="y"/>; otherwise <code>0</code> if they are equal.</returns>
        /// <remarks>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</remarks>
        public static int IgnoreCaseCompare(string x, string y) => string.IsNullOrEmpty(x) ? (string.IsNullOrEmpty(y) ? 0 : -1) :
            (string.IsNullOrEmpty(y) ? 1 : IGNORE_CASE.Compare(x, y));

        /// <summary>
        /// Compares two <seealso cref="string"/> values for sorting, using case-sensitive comparison.
        /// </summary>
        /// <param name="x">The <seealso cref="string"/> to compare to <paramref name="y"/>.</param>
        /// <param name="y">The <seealso cref="string"/> to compare to <paramref name="x"/>.</param>
        /// <returns><code>&lt; 0</code> if <paramref name="x"/> is alphabetically less than <paramref name="y"/>; <code>&gt; 0</code> if <paramref name="x"/>
        /// is greater than <paramref name="y"/>; otherwise <code>0</code> if they are equal.</returns>
        /// <remarks>A <see langword="null"/> value is considered to equal to an empty <seealso cref="string"/>.</remarks>
        public static int CaseSensitiveCompare(string x, string y) => string.IsNullOrEmpty(x) ? (string.IsNullOrEmpty(y) ? 0 : -1) :
            (string.IsNullOrEmpty(y) ? 1 : CASE_SENSITIVE.Compare(x, y));
    }
}
