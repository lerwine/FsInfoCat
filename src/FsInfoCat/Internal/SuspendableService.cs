using FsInfoCat.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Internal
{

    class SuspendableService : ISuspendableService
    {
        public ISuspendableQueue<T> GetSuspendableQueue<T>() => GetSuspendableQueue<T>(null);

        public ISuspendableQueue<T> GetSuspendableQueue<T>(IEqualityComparer<T> itemComparer)
        {
            Type type = typeof(T);
            if (type.IsValueType)
            {
                if (type.IsNullableType())
                    return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(NullableSuspendableQueue<>).MakeGenericType(Nullable.GetUnderlyingType(type)), new object[] { itemComparer });
                return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableValueQueue<>).MakeGenericType(type), new object[] { itemComparer });
            }
            if (type.Equals(typeof(string)))
                return (ISuspendableQueue<T>)new SuspendableStringQueue((IEqualityComparer<string>)itemComparer);
            if (itemComparer is null)
            {
                if (!typeof(IEquatable<T>).IsAssignableFrom(type))
                {
                    PropertyDescriptor[] properties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(p => !p.DesignTimeOnly).ToArray();
                    if (properties.Length > 0)
                        return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableComponentQueue<>).MakeGenericType(type), new object[] { properties });
                }
            }
            return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableReferenceQueue<>).MakeGenericType(type), new object[] { itemComparer });
        }
    }
}
