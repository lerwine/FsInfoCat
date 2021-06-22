using System;

namespace FsInfoCat.Collections
{
    public interface IEventSuspensionManager : ISuspendableQueue<IEventItem>
    {
        bool Enqueue<T>(string eventName, T args) where T : EventArgs;
    }
}
