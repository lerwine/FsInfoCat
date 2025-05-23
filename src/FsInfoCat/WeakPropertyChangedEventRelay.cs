using System;
using System.ComponentModel;

namespace FsInfoCat
{
    // TODO: Document WeakPropertyChangedEventRelay class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public sealed class WeakPropertyChangedEventRelay(PropertyChangedEventHandler eventHandler) : WeakEventRelay<INotifyPropertyChanged, PropertyChangedEventArgs, PropertyChangedEventHandler>(eventHandler)
    {
        public static WeakPropertyChangedEventRelay Attach(INotifyPropertyChanged sender, PropertyChangedEventHandler eventHandler)
        {
            WeakPropertyChangedEventRelay eventRelay = new(eventHandler ?? throw new ArgumentNullException(nameof(sender)));
            eventRelay.Attach(sender ?? throw new ArgumentNullException(nameof(sender)));
            return eventRelay;
        }

        protected override void AttachSource(INotifyPropertyChanged source) => source.PropertyChanged += RaiseEvent;

        protected override void DetachSource(INotifyPropertyChanged source) => source.PropertyChanged -= RaiseEvent;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
