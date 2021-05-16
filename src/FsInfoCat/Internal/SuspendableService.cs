using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Internal
{

    class SuspendableService : ISuspendableService
    {
        public ISuspendableQueue<T> CreateSuspendableQueue<T>() => CreateSuspendableQueue<T>(null);

        public ISuspendableQueue<T> CreateSuspendableQueue<T>(IEqualityComparer<T> itemComparer)
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
                itemComparer = Services.GetComparisonService().GetEqualityComparer<T>();
            return (ISuspendableQueue<T>)Activator.CreateInstance(typeof(SuspendableReferenceQueue<>).MakeGenericType(type), new object[] { itemComparer });
        }

        public IEventSuspensionManager NewEventSuspensionManager()
        {
            throw new NotImplementedException();
        }
    }
}
