using System;
using System.ComponentModel;

namespace FsInfoCat
{

    public abstract partial class RevertibleChangeTracking
    {
        public interface IPropertyChangeTracker : IRevertibleChangeTracking
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
}
