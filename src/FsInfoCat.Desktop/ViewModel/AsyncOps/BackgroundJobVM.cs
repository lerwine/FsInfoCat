using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class BackgroundJobVM : DependencyObject, IAsyncJobModel<Task>
    {
        private readonly CancellationTokenSource _tokenSource = new();

        #region Cancel Prop.erty Members

        /// <summary>
        /// Occurs when the <see cref="Cancel">Cancel Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> CancelInvoked;

        private static readonly DependencyPropertyKey CancelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Cancel), typeof(Commands.RelayCommand),
            typeof(BackgroundJobVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Cancel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelProperty = CancelPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Cancel => (Commands.RelayCommand)GetValue(CancelProperty);

        /// <summary>
        /// Called when the Cancel event is raised by <see cref="Cancel" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/>
        /// method on <see cref="Cancel" />.</param>
        private void RaiseCancelInvoked(object parameter)
        {
            try { OnCancelInvoked(parameter); }
            finally { CancelInvoked?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="Cancel">Cancel Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/>
        /// method on <see cref="Cancel" />.</param>
        protected virtual void OnCancelInvoked(object parameter)
        {
            // TODO: Implement OnCancelInvoked Logic
        }

        #endregion
        #region IsCompleted Property Members

        private static readonly DependencyPropertyKey IsCompletedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsCompleted), typeof(bool),
            typeof(BackgroundJobVM), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsCompleted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCompletedProperty = IsCompletedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsCompleted { get => (bool)GetValue(IsCompletedProperty); private set => SetValue(IsCompletedPropertyKey, value); }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler StatusPropertyChanged;

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(AsyncJobStatus),
            typeof(BackgroundJobVM), new PropertyMetadata(AsyncJobStatus.WaitingToRun, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as BackgroundJobVM)?.StatusPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public AsyncJobStatus Status { get => (AsyncJobStatus)GetValue(StatusProperty); private set => SetValue(StatusPropertyKey, value); }

        #endregion

        protected bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

        bool IAsyncJobModel.IsCancellationRequested => _tokenSource.IsCancellationRequested;

        protected internal Task Task { get; }

        protected Guid ConcurrencyId { get; } = Guid.NewGuid();

        public Guid GetConcurrencyId() => ConcurrencyId;

        void IAsyncJobModel.Cancel(bool throwOnFirstException) => _tokenSource.Cancel(throwOnFirstException);

        void IAsyncJobModel.Cancel() => _tokenSource.Cancel();

        protected internal BackgroundJobVM(Func<StatusListener, BackgroundJobVM, Task> createTask)
        {
            StatusListener statusListener = new StatusListener(this, _tokenSource.Token);
            Task = createTask(statusListener, this);
        }

        protected internal BackgroundJobVM(BackgroundJobServiceVM owner, Func<StatusListener, BackgroundJobVM, Task> createTask)
        {
            Owner = owner;
            StatusListener statusListener = new StatusListener(this, _tokenSource.Token);
            Task = createTask(statusListener, this);
            Task.ContinueWith(t => RaiseStatusChanged());
        }

        Task IAsyncJobModel<Task>.Task => Task;

        Task IAsyncJobModel.Task => Task;

        Guid IAsyncJobModel.ConcurrencyId => ConcurrencyId;

        protected BackgroundJobServiceVM Owner { get; }

        internal void RaiseStatusChanged() => Dispatcher.Invoke(() =>
        {
            switch (Task.Status)
            {
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    Status = _tokenSource.IsCancellationRequested ? AsyncJobStatus.Cancelling : AsyncJobStatus.Running;
                    break;
                case TaskStatus.RanToCompletion:
                    Status = AsyncJobStatus.Succeeded;
                    break;
                case TaskStatus.Canceled:
                    Status = AsyncJobStatus.Canceled;
                    break;
                case TaskStatus.Faulted:
                    Status = AsyncJobStatus.Faulted;
                    break;
                default:
                    Status = _tokenSource.IsCancellationRequested ? AsyncJobStatus.Cancelling : AsyncJobStatus.WaitingToRun;
                    break;
            }
        }, DispatcherPriority.Background);
    }

    public class BackgroundJobVM<TResult> : BackgroundJobVM, IAsyncJobModel<Task<TResult>>
    {
        protected new Task<TResult> Task => (Task<TResult>)base.Task;

        Task<TResult> IAsyncJobModel<Task<TResult>>.Task => Task;

        protected internal BackgroundJobVM(Func<StatusListener, BackgroundJobVM<TResult>, Task<TResult>> createTask)
            : base((listener, b) => createTask(listener, (BackgroundJobVM<TResult>)b)) { }
    }
}
