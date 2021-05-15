using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Internal
{
    class PropertyComparer<TOwner, TProperty> : IGeneralizableOrderAndEqualityComparer<TOwner>, IComparable<PropertyComparer<TOwner, TProperty>>
    {
        private static readonly Coersion<TOwner> _coersion = Coersion<TOwner>.Default;
        private readonly PropertyDescriptor _descriptor;
        private readonly IEqualityComparer<TProperty> _equalityComparer;
        private readonly IComparer<TProperty> _sortComparer;

        public PropertyComparer(PropertyDescriptor descriptor)
        {
            _descriptor = descriptor;
            Type t = typeof(TProperty);
            if (t.IsPrimitive || t.Equals(typeof(string)))
            {
                _equalityComparer = EqualityComparer<TProperty>.Default;
                _sortComparer = Comparer<TProperty>.Default;
            }
            else
            {
                if (t.IsSelfComparable(true) && !t.IsValueType)
                {
                    IGeneralizableOrderAndEqualityComparer<TProperty> comparer = (IGeneralizableOrderAndEqualityComparer<TProperty>)typeof(ComparableEqualityComparer<>).MakeGenericType(t).GetField(nameof(ComparableEqualityComparer<string>.Default)).GetValue(null);
                    _sortComparer = comparer;
                    _equalityComparer = comparer;
                }
                else if (t.IsSelfComparable())
                {
                    _sortComparer = Comparer<TProperty>.Default;
                    if (t.IsSelfEquatable() || !t.HasTypeConverter())
                        _equalityComparer = EqualityComparer<TProperty>.Default;
                    else
                        _equalityComparer = new TypeConvertingEqualityComparer<TProperty>(descriptor.Converter);
                }
                else if (t.IsSelfEquatable())
                {
                    _equalityComparer = EqualityComparer<TProperty>.Default;
                    if (t.HasTypeConverter())
                        _sortComparer = new TypeConvertingEqualityComparer<TProperty>(descriptor.Converter);
                    else
                        _sortComparer = Comparer<TProperty>.Default;
                }
                else if (t.HasTypeConverter())
                {
                    TypeConvertingEqualityComparer<TProperty> comparer = new TypeConvertingEqualityComparer<TProperty>(descriptor.Converter);
                    _equalityComparer = comparer;
                    _sortComparer = comparer;
                }
                else
                {
                    _equalityComparer = EqualityComparer<TProperty>.Default;
                    _sortComparer = Comparer<TProperty>.Default;
                }
            }
        }

        public int Compare(TOwner x, TOwner y) => _sortComparer.Compare((TProperty)_descriptor.GetValue(x), (TProperty)_descriptor.GetValue(y));

        int IComparer.Compare(object x, object y) => _coersion.TryCoerce(x, out TOwner a) ?
            (_coersion.TryCoerce(y, out TOwner b) ? Compare(a, b) : -1) : (_coersion.TryCoerce(y, out _) ? 1 :
            ((x is null) ? ((y is null) ? 0 : -1) : (ReferenceEquals(x, y) ? 0 : Comparer.Default.Compare(x, y))));

        public int CompareTo(PropertyComparer<TOwner, TProperty> other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            int result = _descriptor.Name.CompareTo(other._descriptor.Name);
            return (result == 0 && (result = _descriptor.IsReadOnly.CompareTo(other._descriptor.IsReadOnly)) == 0) ?
                _descriptor.PropertyType.AssemblyQualifiedName.CompareTo(other._descriptor.PropertyType.AssemblyQualifiedName) : result;
        }

        public bool Equals(TOwner x, TOwner y) => _equalityComparer.Equals((TProperty)_descriptor.GetValue(x), (TProperty)_descriptor.GetValue(y));

        bool IEqualityComparer.Equals(object x, object y) => _coersion.TryCoerce(x, out TOwner a) ?
            (_coersion.TryCoerce(y, out TOwner b) && Equals(a, b)) : (!_coersion.TryCoerce(y, out _) &&
            ((x is null) ? y is null : (ReferenceEquals(x, y) || x.Equals(y))));

        public int GetHashCode(TOwner obj) => _equalityComparer.GetHashCode((TProperty)_descriptor.GetValue(obj));

        int IEqualityComparer.GetHashCode(object obj) => _coersion.TryCoerce(obj, out TOwner owner) ?
            _equalityComparer.GetHashCode((TProperty)_descriptor.GetValue(owner)) :
            ((obj is null) ? 0 : obj.GetHashCode());
    }
}
