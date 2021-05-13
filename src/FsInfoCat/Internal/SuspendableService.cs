using FsInfoCat.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Internal
{

    class SuspendableService : ISuspendableService
    {
        public ISuspendableQueue<T> GetSuspendableQueue<T>()
        {
            Type type = typeof(T);
            if (type.IsValueType)
            {
                if (type.IsNullableType())
                    return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(NullableSuspendableQueue<>).MakeGenericType(Nullable.GetUnderlyingType(type)));
                return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableValueQueue<>).MakeGenericType(type));
            }
            if (type.Equals(typeof(string)))
                return (ISuspendableQueue<T>)new SuspendableStringQueue();
            if (!typeof(IEquatable<T>).IsAssignableFrom(type))
            {
                PropertyDescriptor[] properties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(p => !p.DesignTimeOnly).ToArray();
                if (properties.Length > 0)
                    return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableComponentQueue<>).MakeGenericType(type), new object[] { properties });
            }
            return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableReferenceQueue<>).MakeGenericType(type));
        }
    }
}
