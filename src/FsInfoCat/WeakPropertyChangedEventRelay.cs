using System;
using System.ComponentModel;

namespace FsInfoCat
{
    public sealed class WeakPropertyChangedEventRelay : WeakEventRelay<INotifyPropertyChanged, PropertyChangedEventArgs, PropertyChangedEventHandler>
    {
        public static WeakPropertyChangedEventRelay Attach(INotifyPropertyChanged sender, PropertyChangedEventHandler eventHandler)
        {
            WeakPropertyChangedEventRelay eventRelay = new(eventHandler ?? throw new ArgumentNullException(nameof(sender)));
            eventRelay.Attach(sender ?? throw new ArgumentNullException(nameof(sender)));
            return eventRelay;
        }

        protected override void AttachSource(INotifyPropertyChanged source) => source.PropertyChanged += RaiseEvent;

        protected override void DetachSource(INotifyPropertyChanged source) => source.PropertyChanged -= RaiseEvent;

        public WeakPropertyChangedEventRelay(PropertyChangedEventHandler eventHandler) : base(eventHandler) { }
    }
}
