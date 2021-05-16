using FsInfoCat.Collections;
using System.Collections.Generic;

namespace FsInfoCat
{

    public interface ISuspendableService
    {
        ISuspendableQueue<T> CreateSuspendableQueue<T>();
        ISuspendableQueue<T> CreateSuspendableQueue<T>(IEqualityComparer<T> itemComparer);
        IEventSuspensionManager NewEventSuspensionManager();
    }
}
