using System;

namespace FsInfoCat.Collections
{
    public interface IEventSuspensionManager : ISuspensionQueue<IEventItem>
    {
        bool Enqueue<T>(string eventName, T args) where T : EventArgs;
    }
}
