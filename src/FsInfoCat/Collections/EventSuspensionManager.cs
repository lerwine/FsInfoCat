using System;

namespace FsInfoCat.Collections
{

    public class EventSuspensionManager : Internal.EventSuspensionManager
    {
        public EventSuspensionManager() : base() { }

        public abstract class Item<T> : EventItem
            where T : EventArgs
        {
            public new T Args => (T)base.Args;
            public Item(string eventName, T args) : base(eventName, args) { }
            public override bool Equals(EventItem other) => !(other is null) && (ReferenceEquals(this, other) || (other.Args is T args && ArgsEqual(args)));
            protected abstract bool ArgsEqual(T args);
        }
        public sealed class Item : Item<EventArgs>
        {
            public Item(string eventName, EventArgs args) : base(eventName, args) { }
            protected override bool ArgsEqual(EventArgs args) => ReferenceEquals(args, Args);
        }
    }
}
