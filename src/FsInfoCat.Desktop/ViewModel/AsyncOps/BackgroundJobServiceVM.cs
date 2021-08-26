using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class BackgroundJobServiceVM : DependencyObject, IAsyncJobService
    {
        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<BackgroundJobVM>), typeof(BackgroundJobServiceVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion
        #region IsBusy Property Members

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBusy), typeof(bool),
            typeof(BackgroundJobServiceVM), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsBusy { get => (bool)GetValue(IsBusyProperty); private set => SetValue(IsBusyPropertyKey, value); }

        #endregion
        public BackgroundJobServiceVM()
        {
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
        }

        private T AddJob<T>(T item) where T : BackgroundJobVM => Dispatcher.Invoke(() =>
        {
            lock (_backingItems)
            {
                _backingItems.Add(item);
                item.Task.ContinueWith(task => RemoveJob(item));
            }
            IsBusy = _backingItems.Count > 0;
            return item;
        });

        private void RemoveJob(BackgroundJobVM item) => Dispatcher.Invoke(() =>
        {
            lock (_backingItems)
            {
                _backingItems.Remove(item);
            }
            IsBusy = _backingItems.Count > 0;
        });

        public BackgroundJobVM<TResult> Create<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, StatusListener, Task<TResult>> method) => AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(arg1, arg2, arg3, listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM<TResult> Create<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, StatusListener, Task<TResult>> method) =>
            AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(arg1, arg2, listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM<TResult> Create<TArg, TResult>(TArg arg, Func<TArg, StatusListener, Task<TResult>> method) =>
            AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(arg, listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM<TResult> Create<TResult>(Func<StatusListener, Task<TResult>> method) =>
            AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM Create<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, StatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(arg1, arg2, arg3, listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM Create<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, StatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(arg1, arg2, listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM Create<TArg>(TArg arg, Func<TArg, StatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(arg, listener);
            }, listener.CancellationToken)));

        public BackgroundJobVM Create(Func<StatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task<TResult>> IAsyncJobService.Create<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, IStatusListener, Task<TResult>> method) => AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(arg1, arg2, arg3, listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task<TResult>> IAsyncJobService.Create<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2,
            Func<TArg1, TArg2, IStatusListener, Task<TResult>> method) => AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(arg1, arg2, listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task<TResult>> IAsyncJobService.Create<TArg, TResult>(TArg arg, Func<TArg, IStatusListener, Task<TResult>> method) =>
            AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(arg, listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task<TResult>> IAsyncJobService.Create<TResult>(Func<IStatusListener, Task<TResult>> method) =>
            AddJob(new BackgroundJobVM<TResult>((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                return await method(listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task> IAsyncJobService.Create<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, IStatusListener, Task> method) => AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(arg1, arg2, arg3, listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task> IAsyncJobService.Create<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, IStatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(arg1, arg2, listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task> IAsyncJobService.Create<TArg>(TArg arg, Func<TArg, IStatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(arg, listener);
            }, listener.CancellationToken)));

        IAsyncJobModel<Task> IAsyncJobService.Create(Func<IStatusListener, Task> method) =>
            AddJob(new BackgroundJobVM((listener, instance) => Task.Run(async () =>
            {
                await instance.Dispatcher.InvokeAsync(() => instance.RaiseStatusChanged(), DispatcherPriority.Normal, listener.CancellationToken);
                await method(listener);
            }, listener.CancellationToken)));
    }
}
