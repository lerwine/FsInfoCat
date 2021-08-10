using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class AsyncOpManagerViewModel<TState, TTask, TItem, TListener>
    {
        /// <summary>
        /// A view model which indicates the status of a background operation.
        /// <para>Extends <see cref="DependencyObject" />.</para>
        /// </summary>
        /// <seealso cref="DependencyObject" />
        public abstract partial class AsyncOpViewModel : DependencyObject
        {
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly Stopwatch _stopWatch = new();
            private Timer _timer;

            /// <summary>
            /// Occurs when the <see cref="State"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler StatePropertyChanged;

            /// <summary>
            /// Occurs when the <see cref="AsyncOpStatus"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler AsyncOpStatusPropertyChanged;

            /// <summary>
            /// Occurs when the <see cref="StatusMessage"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler StatusMessagePropertyChanged;

            /// <summary>
            /// Occurs when the <see cref="MessageLevel"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;

            /// <summary>
            /// Occurs when the <see cref="Started"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler StartedPropertyChanged;

            /// <summary>
            /// Occurs when the <see cref="Stopped"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler StoppedPropertyChanged;

            /// <summary>
            /// Occurs when the <see cref="Duration"/> property has changed.
            /// </summary>
            public event DependencyPropertyChangedEventHandler DurationPropertyChanged;

            public event EventHandler OperationRanToCompletion;

            public event EventHandler OperationCanceled;

            public event EventHandler<OpFailedEventArgs> OperationFailed;

            private static readonly DependencyPropertyKey StatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(State), typeof(TState), typeof(AsyncOpViewModel),
                    new PropertyMetadata(default, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.StatePropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty StateProperty = StatePropertyKey.DependencyProperty;

            /// <summary>
            /// Gets the state value associated with the current background operation.
            /// </summary>
            /// <value>The state value associated with the current background operation.</value>
            public TState State
            {
                get => (TState)GetValue(StateProperty);
                private set => SetValue(StatePropertyKey, value);
            }

            private readonly TListener _listener;
            private readonly TTask _task;
            private static readonly DependencyPropertyKey AsyncOpStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AsyncOpStatus), typeof(AsyncOpStatusCode), typeof(AsyncOpViewModel),
                    new PropertyMetadata(AsyncOpStatusCode.NotStarted, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.AsyncOpStatusPropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty AsyncOpStatusProperty = AsyncOpStatusPropertyKey.DependencyProperty;

            /// <summary>
            /// Gets a value which indicates the status of the current asynchronous operation.
            /// </summary>
            /// <value>A <see cref="AsyncOpStatusCode"/> value that indicates the status of the current asynchronous operation.</value>
            public AsyncOpStatusCode AsyncOpStatus
            {
                get => (AsyncOpStatusCode)GetValue(AsyncOpStatusProperty);
                private set => SetValue(AsyncOpStatusPropertyKey, value);
            }

            private static readonly DependencyPropertyKey MessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MessageLevel), typeof(StatusMessageLevel), typeof(AsyncOpViewModel),
                    new PropertyMetadata(StatusMessageLevel.Information, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.MessageLevelPropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty MessageLevelProperty = MessageLevelPropertyKey.DependencyProperty;

            /// <summary>
            /// Gets the level (severity) of the current status message.
            /// </summary>
            /// <value>A <see cref="StatusMessageLevel"/> that indicates the level or severity of the current <see cref="StatusMessage"/>.</value>
            public StatusMessageLevel MessageLevel
            {
                get => (StatusMessageLevel)GetValue(MessageLevelProperty);
                private set => SetValue(MessageLevelPropertyKey, value);
            }

            private static readonly DependencyPropertyKey StatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusMessage), typeof(string), typeof(AsyncOpViewModel),
                    new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.StatusMessagePropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty StatusMessageProperty = StatusMessagePropertyKey.DependencyProperty;

            /// <summary>
            /// Gets the message which is a textual representation of the status of the current background operation.
            /// </summary>
            /// <value>The status message produced by the delegate method for the current background operation.</value>
            public string StatusMessage
            {
                get => GetValue(StatusMessageProperty) as string;
                private set => SetValue(StatusMessagePropertyKey, value);
            }

            private static readonly DependencyPropertyKey StartedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Started), typeof(DateTime?), typeof(AsyncOpViewModel),
                    new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.StartedPropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty StartedProperty = StartedPropertyKey.DependencyProperty;

            /// <summary>
            /// Gets the date and time when the background operation was started.
            /// </summary>
            /// <value>The <see cref="DateTime"/> value when the background operation was started or <see langword="null"/> if it has not yet been started.</value>
            public DateTime? Started
            {
                get => (DateTime?)GetValue(StartedProperty);
                private set => SetValue(StartedPropertyKey, value);
            }

            private static readonly DependencyPropertyKey StoppedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Stopped), typeof(DateTime?), typeof(AsyncOpViewModel),
                    new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.StoppedPropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty StoppedProperty = StoppedPropertyKey.DependencyProperty;

            /// <summary>
            /// Gets the date and time when the background operation was completed.
            /// </summary>
            /// <value>The <see cref="DateTime"/> value when the background operation was null or <see langword="null"/> if it has not been started or has not yet finished.</value>
            public DateTime? Stopped
            {
                get => (DateTime?)GetValue(StoppedProperty);
                private set => SetValue(StoppedPropertyKey, value);
            }

            private static readonly DependencyPropertyKey DurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Duration), typeof(TimeSpan?), typeof(AsyncOpViewModel),
                    new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncOpViewModel)?.DurationPropertyChanged?.Invoke(d, e)));

            public static readonly DependencyProperty DurationProperty = DurationPropertyKey.DependencyProperty;

            /// <summary>
            /// Gets the duration of the background operation.
            /// </summary>
            /// <value>The duration of the background operation or <see langword="null"/> if it has not yet been started.</value>
            public TimeSpan? Duration
            {
                get => (TimeSpan?)GetValue(DurationProperty);
                private set => SetValue(DurationPropertyKey, value);
            }

            /// <summary>
            /// Updates the value of the <see cref="AsyncOpStatus"/> property according to a <see cref="TaskStatus"/> value.
            /// </summary>
            /// <param name="status">The <see cref="TaskStatus"/> taken from the <see cref="TTask"/> for the background operation.</param>
            /// <remarks>This MUST be invoked on the UI thread.</remarks>
            /// <exception cref="InvalidOperationException">This method was not invoked on the UI thread (or a thread which does not have access to the properties of the
            /// current <see cref="AsyncOpViewModel"/>).</exception>
            internal void UpdateOpStatus(TaskStatus status)
            {
                VerifyAccess();
                switch (status)
                {
                    case TaskStatus.Faulted:
                        AsyncOpStatus = AsyncOpStatusCode.Faulted;
                        break;
                    case TaskStatus.Canceled:
                        AsyncOpStatus = AsyncOpStatusCode.Canceled;
                        break;
                    case TaskStatus.RanToCompletion:
                        AsyncOpStatus = AsyncOpStatusCode.RanToCompletion;
                        break;
                    default:
                        if (AsyncOpStatus != AsyncOpStatusCode.CancellationPending)
                            AsyncOpStatus = AsyncOpStatusCode.Running;
                        break;
                }
            }

            private void RaiseCompleted(TTask task)
            {
                if (task.IsCanceled)
                    OnCanceled();
                else if (task.IsFaulted)
                    OnFaulted(task.Exception);
                else
                    OnRanToCompletion(task);
            }

            /// <summary>
            /// Called when the background operation was canceled.
            /// </summary>
            /// <remarks>This is invoked on the UI thread.</remarks>
            protected virtual void OnCanceled()
            {
                if (MessageLevel == StatusMessageLevel.Information)
                {
                    MessageLevel = StatusMessageLevel.Warning;
                    StatusMessage = StatusMessage = FsInfoCat.Properties.Resources.Description_Bg_Operation_Canceled;
                }
                OperationCanceled?.Invoke(this, EventArgs.Empty);
            }

            protected virtual void OnFaulted(AggregateException exception)
            {
                OpFailedEventArgs args = new(exception);
                MessageLevel = StatusMessageLevel.Error;
                if (args.Exception is AsyncOperationFailureException asyncOperationFailure && !string.IsNullOrWhiteSpace(asyncOperationFailure.UserMessage))
                    StatusMessage = asyncOperationFailure.UserMessage;
                else
                    StatusMessage = FsInfoCat.Properties.Resources.ErrorMessage_UnexpectedError;
                OperationFailed?.Invoke(this, new OpFailedEventArgs(exception));
            }

            protected virtual void OnRanToCompletion(TTask task) => OperationRanToCompletion?.Invoke(this, EventArgs.Empty);

            /// <summary>
            /// Adds the specified specified <typeparamref name="TItem">item</typeparamref> to a <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}"/> for tracking.
            /// </summary>
            /// <param name="owner">The <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}"/> that will track the background operation.</param>
            /// <param name="item">The <typeparamref name="TItem"/> to be tracked.</param>
            /// <exception cref="InvalidOperationException">The current item is not being tracked by the specified <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}"/>.</exception>
            protected static void Add(AsyncOpManagerViewModel<TState, TTask, TItem, TListener> owner, TItem item)
            {
                item.VerifyAccess();
                if (owner._allOperations.Contains(item))
                    throw new InvalidOperationException();
                Thread.BeginCriticalRegion();
                try
                {
                    owner._allOperations.Add(item);
                    if (item.AsyncOpStatus > AsyncOpStatusCode.CancellationPending)
                        owner._completedOperations.Add(item);
                    switch (item.AsyncOpStatus)
                    {
                        case AsyncOpStatusCode.NotStarted:
                            owner._pendingOperations.Add(item);
                            break;
                        case AsyncOpStatusCode.RanToCompletion:
                            owner._successsfulOperations.Add(item);
                            break;
                        case AsyncOpStatusCode.Canceled:
                            owner._canceledOperations.Add(item);
                            break;
                        case AsyncOpStatusCode.Faulted:
                            owner._failedOperations.Add(item);
                            break;
                        default:
                            owner._activeOperations.Add(item);
                            break;
                    }
                    item.AsyncOpStatusPropertyChanged += owner.Item_AsyncOpStatusPropertyChanged;
                }
                finally { Thread.EndCriticalRegion(); }
                item.GetTask().ContinueWith(task =>
                {
                    item.GetStatusListener().RaiseTaskCompleted((TTask)task, owner);
                    item.AsyncOpStatusPropertyChanged -= owner.Item_AsyncOpStatusPropertyChanged;
                });
            }

            /// <summary>
            /// Cancels the current background operation.
            /// </summary>
            /// <param name="throwOnFirstException"><see langword="true"/> if exceptions should immediately propagate; otherwise, <see langword="false"/>.</param>
            /// <remarks>This MUST be invoked on the UI thread.</remarks>
            /// <seealso cref="CancellationTokenSource.Cancel(bool)"/>
            public void Cancel(bool throwOnFirstException)
            {
                VerifyAccess();
                if (GetTask().IsCompleted || _tokenSource.IsCancellationRequested)
                    return;
                _tokenSource.Cancel(throwOnFirstException);
            }

            /// <summary>
            /// Cancels the current background operation.
            /// </summary>
            /// <remarks>This MUST be invoked on the UI thread.</remarks>
            /// <seealso cref="CancellationTokenSource.Cancel()"/>
            public void Cancel()
            {
                VerifyAccess();
                if (GetTask().IsCompleted || _tokenSource.IsCancellationRequested)
                    return;
                _tokenSource.Cancel();
            }

            /// <summary>
            /// Gets the task for the background operation.
            /// </summary>
            /// <returns>The <typeparamref name="TTask"/> for the background operation.</returns>
            internal TTask GetTask() => _task;

            /// <summary>
            /// Gets the listener that can be used to monitor for cancellation and to update the current view model item.
            /// </summary>
            /// <returns>The <typeparamref name="TListener"/> that can be used to monitor for cancellation and to update the current <see cref="AsyncOpViewModel"/>.</returns>
            internal TListener GetStatusListener() => _listener;

            /// <summary>
            /// Gets the cancellation token for the background operation.
            /// </summary>
            /// <returns>The <see cref="CancellationToken"/> for the current background operation.</returns>
            internal CancellationToken GetCancellationToken() => _tokenSource.Token;

            [Obsolete("Use AsyncOpViewModel(ItemBuilder), instead.")]
            protected AsyncOpViewModel(TState initialState)
            {
                State = initialState;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncOpViewModel"/> class.
            /// </summary>
            /// <param name="builder">The <see cref="ItemBuilder"/> that is used to initialize the current object.</param>
            protected AsyncOpViewModel(ItemBuilder builder)
            {
                State = builder.InitialState;
                _listener = builder.GetStatusListener(this);
                _task = builder.GetTask(State, _listener, this);
                UpdateOpStatus(_task.Status);
            }
        }

        public abstract class ItemBuilder
        {
            internal TState InitialState { get; }
            protected ItemBuilder(TState initialState) { InitialState = initialState; }
            protected internal abstract TListener GetStatusListener(AsyncOpViewModel instance);
            protected internal abstract TTask GetTask(TState state, TListener listener, AsyncOpViewModel instance);
        }
    }
}
