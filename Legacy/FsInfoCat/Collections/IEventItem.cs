using System;

namespace FsInfoCat.Collections
{
    public interface IEventItem : IEquatable<IEventItem>
    {
        string EventName { get; }
        EventArgs Args { get; }
    }

    public interface IEventItem<T> : IEventItem
        where T : EventArgs
    {
        new T Args { get; }
    }
}
