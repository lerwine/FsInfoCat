using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace FsInfoCat.ComponentSupport
{
    public sealed class ValidationResultCollection : ObservableCollection<ValidationResultCollection.Item>, IList<ValidationResult>
    {
        public void Add(ValidationResult item) => base.Add(Item.AsComparableValidationResult(item));

        public void Insert(int index, ValidationResult item)
            => base.Insert(index, Item.AsComparableValidationResult(item));

        public bool Remove(ValidationResult item) => base.Remove(Item.AsComparableValidationResult(item));

        public bool Contains(ValidationResult item) => base.Contains(Item.AsComparableValidationResult(item));

        public int IndexOf(ValidationResult item) => base.IndexOf(Item.AsComparableValidationResult(item));

        public void CopyTo(ValidationResult[] array, int arrayIndex) => this.Cast<ValidationResult>().ToList().CopyTo(array, arrayIndex);

        protected override void InsertItem(int index, Item item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.ErrorMessage))
                throw new ArgumentOutOfRangeException(nameof(item), $"{nameof(Item.ErrorMessage)} is empty.");
            using (IDisposable reentrancyBlock = BlockReentrancy())
            {
                int oldIndex = base.IndexOf(item);
                base.InsertItem(index, item);
                if (oldIndex >= 0)
                    RemoveItem((oldIndex < index) ? oldIndex : oldIndex + 1);
             }
        }

        protected override void SetItem(int index, Item item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(item.ErrorMessage))
                throw new ArgumentOutOfRangeException(nameof(item), $"{nameof(Item.ErrorMessage)} is empty.");
            using (IDisposable reentrancyBlock = BlockReentrancy())
            {
                int oldIndex = base.IndexOf(item);
                base.SetItem(index, item);
                if (oldIndex >= 0 && oldIndex != index)
                    RemoveItem(oldIndex);
            }
        }

        bool ICollection<ValidationResult>.IsReadOnly => false;

        ValidationResult IList<ValidationResult>.this[int index] { get => this[index]; set => this[index] = Item.AsComparableValidationResult(value); }

        IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator() => this.Cast<ValidationResult>().GetEnumerator();

        public class Item : ValidationResult, IEquatable<Item>
        {
            private static readonly StringComparer _backingComparer = StringComparer.InvariantCulture;

            internal Item(ValidationResult source) : base(source.ErrorMessage,
                new ReadOnlyCollection<string>(source.MemberNames.NotNullOrWhiteSpace().ToArray()))
            {
            }

            internal static Item AsComparableValidationResult(ValidationResult source)
            {
                if (source is null)
                    return null;
                return (source is Item cr) ? cr : new Item(source);
            }
            public bool Equals(Item other)
            {
                return !(other is null) && ReferenceEquals(this, other) || (_backingComparer.Equals(ErrorMessage, other.ErrorMessage) &&
                    MemberNames.SequenceEqual(other.MemberNames, _backingComparer));
            }
        }
    }
}
