using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.ComponentSupport
{
    public class ValidationAttributeCollection : GeneralizableListBase<ValidationAttribute>
    {
        private LinkedList<ValidationAttribute> _backingList = new LinkedList<ValidationAttribute>();
        public override ValidationAttribute this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int Count => throw new NotImplementedException();

        public override void Add(ValidationAttribute item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            lock (_backingList)
            {
                ValidationAttributeComparer comparer = ValidationAttributeComparer.Default;
                for (LinkedListNode<ValidationAttribute> node = _backingList.First; !(node is null); node = node.Next)
                {
                    if (comparer.Equals(node.Value, item))
                    {
                        _backingList.AddBefore(node, item);
                        _backingList.Remove(node);
                        return;
                    }
                }
                _backingList.AddLast(item);
            }
        }

        public override void Clear()
        {
            lock (_backingList)
            {
                _backingList.Clear();
            }
        }

        public override bool Contains(ValidationAttribute item) => !(item is null) && _backingList.Contains(item, ValidationAttributeComparer.Default);

        public override void CopyTo(ValidationAttribute[] array, int arrayIndex) => _backingList.CopyTo(array, arrayIndex);

        public override IEnumerator<ValidationAttribute> GetEnumerator() => _backingList.GetEnumerator();

        public override int IndexOf(ValidationAttribute item)
        {
            if (item is null)
                return -1;
            ValidationAttributeComparer comparer = ValidationAttributeComparer.Default;
            return _backingList.Select((a, i) => new { A = a, I = i }).Where(a => comparer.Equals(a.A, item)).Select(a => a.I).DefaultIfEmpty(-1).First();
        }

        public override void Insert(int index, ValidationAttribute item)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            lock (_backingList)
            {
                if (index >= _backingList.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                LinkedListNode<ValidationAttribute> node = _backingList.First;
                for (int i = 1; i < index; i++)
                    node = node.Next;
                _backingList.AddBefore(node, item);
            }
        }

        public override bool Remove(ValidationAttribute item)
        {
            if (item is null)
                return false;
            lock (_backingList)
            {
                ValidationAttributeComparer comparer = ValidationAttributeComparer.Default;
                for (LinkedListNode<ValidationAttribute> node = _backingList.First; !(node is null); node = node.Next)
                {
                    if (comparer.Equals(node.Value, item))
                    {
                        _backingList.Remove(node);
                        return true;
                    }
                }
                _backingList.AddLast(item);
            }
            return false;
        }

        public override void RemoveAt(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            lock (_backingList)
            {
                if (index >= _backingList.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                LinkedListNode<ValidationAttribute> node = _backingList.First;
                for (int i = 1; i < index; i++)
                    node = node.Next;
                _backingList.Remove(node);
            }
        }

        protected override void CopyTo(Array array, int index) => _backingList.ToArray().CopyTo(array, index);

        protected override IEnumerator GetGenericEnumerator() => ((IEnumerable)_backingList).GetEnumerator();

        public class ValidationAttributeComparer : IEqualityComparer<ValidationAttribute>
        {
            public static readonly ValidationAttributeComparer Default = new ValidationAttributeComparer();

            private static readonly StringComparer _comparer = StringComparer.InvariantCulture;

            public bool Equals(ValidationAttribute x, ValidationAttribute y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                if (ReferenceEquals(x, y))
                    return true;
                if (x is CustomValidationAttribute customValidationA)
                    return y is CustomValidationAttribute customValidationB &&
                        _comparer.Equals(customValidationA.ErrorMessage ?? "", customValidationB.ErrorMessage ?? "") &&
                            _comparer.Equals(customValidationA.ErrorMessageResourceName ?? "", customValidationB.ErrorMessageResourceName ?? "") &&
                            _comparer.Equals(customValidationA.Method ?? "", customValidationB.Method ?? "") &&
                            ((customValidationA.ValidatorType is null) ? customValidationB.ValidatorType is null :
                            customValidationA.ValidatorType.Equals(customValidationB.ValidatorType)) &&
                            ((customValidationA.ErrorMessageResourceType is null) ? customValidationB.ErrorMessageResourceType is null :
                            customValidationA.ErrorMessageResourceType.Equals(customValidationB.ErrorMessageResourceType));
                if (x is CompareAttribute)
                    return y is CompareAttribute;
                if (x is CreditCardAttribute)
                    return y is CreditCardAttribute;
                if (x is EmailAddressAttribute)
                    return y is EmailAddressAttribute;
                if (x is FileExtensionsAttribute)
                    return y is FileExtensionsAttribute;
                if (x is MaxLengthAttribute)
                    return y is MaxLengthAttribute;
                if (x is MinLengthAttribute)
                    return y is MinLengthAttribute;
                if (x is PhoneAttribute)
                    return y is PhoneAttribute;
                if (x is RangeAttribute)
                    return y is RangeAttribute;
                if (x is RegularExpressionAttribute)
                    return y is RegularExpressionAttribute;
                if (x is RequiredAttribute)
                    return y is RequiredAttribute;
                if (x is StringLengthAttribute)
                    return y is StringLengthAttribute;
                if (x is UrlAttribute)
                    return y is UrlAttribute;
                Type a = x.GetType();
                Type b = y.GetType();
                if (!(a.IsAssignableFrom(b) || b.IsAssignableFrom(a)))
                    return false;
                if (x is DataTypeAttribute dataTypeAttribute)
                    return y is DataTypeAttribute; // TODO: Need to check other derrived custom types.
                // TODO: Need to check if attribute allows multiple.
                return true;
            }

            public int GetHashCode(ValidationAttribute obj)
            {
                if (obj is CompareAttribute)
                    return 1;
                if (obj is CreditCardAttribute)
                    return 2;
                if (obj is EmailAddressAttribute)
                    return 3;
                if (obj is FileExtensionsAttribute)
                    return 4;
                if (obj is MaxLengthAttribute)
                    return 5;
                if (obj is MinLengthAttribute)
                    return 6;
                if (obj is PhoneAttribute)
                    return 7;
                if (obj is RangeAttribute)
                    return 8;
                if (obj is RegularExpressionAttribute)
                    return 9;
                if (obj is RequiredAttribute)
                    return 10;
                if (obj is StringLengthAttribute)
                    return 11;
                if (obj is UrlAttribute)
                    return 12;
                // TODO: Need to create hash code from property names and values
                return obj.GetHashCode();
            }
        }
    }
}
