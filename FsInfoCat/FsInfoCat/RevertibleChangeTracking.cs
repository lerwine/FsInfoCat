using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    public abstract partial class RevertibleChangeTracking : NotifyPropertyValueChanging, IRevertibleChangeTracking
    {
        private readonly Dictionary<string, IPropertyChangeTracker> _changeTrackers = new();

        bool IChangeTracking.IsChanged => IsChanged();

        public virtual void AcceptChanges()
        {
            IPropertyChangeTracker[] changeTrackers = _changeTrackers.Values.ToArray();
            foreach (IPropertyChangeTracker tracker in changeTrackers)
                tracker.AcceptChanges();
        }

        protected IPropertyChangeTracker<T> AddChangeTracker<T>([DisallowNull] string propertyName, [AllowNull] T initialValue, ICoersion<T> coersion = null) =>
            new PropertyChangeTracker<T>(this, propertyName, initialValue, coersion);

        public bool IsChanged() => _changeTrackers.Values.Any(t => t.IsChanged);

        public virtual void RejectChanges()
        {
            IPropertyChangeTracker[] changeTrackers = _changeTrackers.Values.ToArray();
            foreach (IPropertyChangeTracker tracker in changeTrackers)
                tracker.RejectChanges();
        }

        protected bool RemoveChangeTracker<T>(IPropertyChangeTracker<T> changeTracker)
        {
            lock (SyncRoot)
            {
                if (_changeTrackers.TryGetValue(changeTracker.PropertyName, out IPropertyChangeTracker ct) && ReferenceEquals(ct, changeTracker))
                {
                    _changeTrackers.Remove(changeTracker.PropertyName);
                    return true;
                }
            }
            return false;
        }
    }
}
