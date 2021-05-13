using FsInfoCat.ComponentSupport.Internal;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Collections.Internal
{
    public class EventSuspensionManager : SuspensionQueue<IEventItem>, IEventSuspensionManager
    {
        protected internal EventSuspensionManager() : base() { }

        public bool Enqueue<T>(string eventName, T args) where T : EventArgs => Enqueue(new DefaultItem<T>(eventName, args));

        public override bool Enqueue(IEventItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            return base.Enqueue(item);
        }

        public abstract class EventItem : IEquatable<EventItem>, IEventItem
        {
            public string EventName { get; }
            public EventArgs Args { get; }
            internal EventItem(string eventName, EventArgs args)
            {
                if (string.IsNullOrWhiteSpace(eventName))
                    throw new ArgumentException($"'{nameof(eventName)}' cannot be null or whitespace.", nameof(eventName));
                EventName = eventName;
                Args = args ?? throw new ArgumentNullException(nameof(args));
            }
            public abstract bool Equals(EventItem other);
            bool IEquatable<IEventItem>.Equals(IEventItem other) => other is EventItem item && Equals(item);
        }

        private class DefaultItem<T> : EventItem, IEventItem<T>
            where T : EventArgs
        {
            private static readonly IEqualityComparer<T> _comparer = ComponentPropertyEqualityComparer<T>.Default;

            internal DefaultItem(string eventName, T args) : base(eventName, args) { }

            public new T Args => (T)base.Args;

            public override bool Equals(EventItem other) => !(other is null) && (ReferenceEquals(this, other) || (EventName.Equals(other.EventName) &&
                other is T args && _comparer.Equals(args, Args)));
        }
    }
}
