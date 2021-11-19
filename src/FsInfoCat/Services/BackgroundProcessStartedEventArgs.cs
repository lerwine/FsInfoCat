using System;

namespace FsInfoCat.Services
{
    public sealed class BackgroundProcessStartedEventArgs : BackgroundProcessStateEventArgs, IObservable<IBackgroundProgressEvent>
    {
        public IDisposable Subscribe(IObserver<IBackgroundProgressEvent> observer)
        {
            // TODO: Implement Subscribe
            throw new NotImplementedException();
        }

    }
}
