using System;
using System.ComponentModel;

namespace FsInfoCat
{
    public abstract partial class RevertibleChangeTracking
    {
        // TODO: Document RevertibleChangeTracking.IPropertyChangeTracker class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public interface IPropertyChangeTracker : IRevertibleChangeTracking, IEquatable<IPropertyChangeTracker>
        {
            event EventHandler ValueChanged;

            string PropertyName { get; }

            bool IsSet { get; }

            object GetValue();

            bool SetValue(object newValue);

            bool IsEqualTo(object obj);
        }

        public interface IPropertyChangeTracker<T> : IPropertyChangeTracker
        {
            new T GetValue();

            bool SetValue(T newValue);

            bool IsEqualTo(T other);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
