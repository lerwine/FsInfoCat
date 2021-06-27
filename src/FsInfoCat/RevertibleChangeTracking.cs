using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    public abstract partial class RevertibleChangeTracking : NotifyPropertyValueChanging, IRevertibleChangeTracking
    {
        private readonly LinkedList<IPropertyChangeTracker> _changeTrackers = new();

        bool IChangeTracking.IsChanged => IsChanged();

        public virtual void AcceptChanges()
        {
            IPropertyChangeTracker[] changeTrackers = _changeTrackers.ToArray();
            foreach (IPropertyChangeTracker tracker in changeTrackers)
                tracker.AcceptChanges();
        }

        protected IPropertyChangeTracker<T> AddChangeTracker<T>([DisallowNull] string propertyName, [AllowNull] T initialValue, ICoersion<T> coersion = null) =>
            new PropertyChangeTracker<T>(this, propertyName, initialValue, coersion);

        public bool IsChanged() => _changeTrackers.Any(t => t.IsChanged);

        public virtual void RejectChanges()
        {
            IPropertyChangeTracker[] changeTrackers = _changeTrackers.ToArray();
            foreach (IPropertyChangeTracker tracker in changeTrackers)
                tracker.RejectChanges();
        }

        protected bool RemoveChangeTracker<T>(IPropertyChangeTracker<T> changeTracker)
        {
            lock (SyncRoot)
                return _changeTrackers.Remove(changeTracker);
        }

        protected bool ArePropertyChangeTrackersEqual(RevertibleChangeTracking other, Func<string, bool> filter = null) => other is not null && (ReferenceEquals(this, other) ||
            ((filter is null) ? _changeTrackers.SequenceEqual(other._changeTrackers) : _changeTrackers.Where(t => filter(t.PropertyName)).SequenceEqual(other._changeTrackers.Where(t => filter(t.PropertyName)))));
    }
}
