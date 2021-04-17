using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class AsyncOperationViewModel : DependencyObject, IAsyncResult
    {
        private const string StatusMessage_Running = "Running";
        private const string StatusMessage_Canceled = "Canceled";
        private const string StatusMessage_Finished = "Finished";
        private const string StatusMessage_Starting = "Starting";
        private static readonly ILogger<AsyncOperationViewModel> _logger = App.LoggerFactory.CreateLogger<AsyncOperationViewModel>();
        private readonly object _syncRoot = new object();
        private CancellationTokenSource _cancellationTokenSource;
        private Task _asyncOperationTask;

        protected Task AsyncOperationTask
        {
            get
            {
                lock (_syncRoot)
                    return _asyncOperationTask;
            }
        }

        private static readonly DependencyPropertyKey IsCompletedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsCompleted), typeof(bool), typeof(AsyncOperationViewModel),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsCompletedProperty = IsCompletedPropertyKey.DependencyProperty;

        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            private set { SetValue(IsCompletedPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey CompletedSynchronouslyPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(CompletedSynchronously), typeof(bool), typeof(AsyncOperationViewModel),
                new PropertyMetadata(false));

        public static readonly DependencyProperty CompletedSynchronouslyProperty = CompletedSynchronouslyPropertyKey.DependencyProperty;

        public bool CompletedSynchronously
        {
            get { return (bool)GetValue(CompletedSynchronouslyProperty); }
            private set { SetValue(CompletedSynchronouslyPropertyKey, value); }
        }

        public event DependencyPropertyChangedEventHandler StatusValuePropertyChanged;

        private static readonly DependencyPropertyKey StatusValuePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(TaskStatus), typeof(AsyncOperationViewModel),
                new PropertyMetadata(TaskStatus.Created, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncOperationViewModel).OnStatusValuePropertyChanged(e)));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public TaskStatus StatusValue
        {
            get { return (TaskStatus)GetValue(StatusValueProperty); }
            private set { SetValue(StatusValuePropertyKey, value); }
        }

        protected virtual void OnStatusValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnStatusPropertyChanged((TaskStatus)args.OldValue, (TaskStatus)args.NewValue); }
            finally { StatusValuePropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnStatusPropertyChanged(TaskStatus oldValue, TaskStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        private static readonly DependencyPropertyKey StatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusMessage), typeof(string),
            typeof(AsyncOperationViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty StatusMessageProperty = StatusMessagePropertyKey.DependencyProperty;

        public string StatusMessage
        {
            get { return GetValue(StatusMessageProperty) as string; }
            private set { SetValue(StatusMessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Error), typeof(Exception), typeof(AsyncOperationViewModel),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorProperty = ErrorPropertyKey.DependencyProperty;

        public Exception Error
        {
            get { return (Exception)GetValue(ErrorProperty); }
            private set { SetValue(ErrorPropertyKey, value); }
        }

        protected WaitHandle AsyncWaitHandle => throw new NotImplementedException();

        WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();

        object IAsyncResult.AsyncState => GetAsyncState();

        protected TaskScheduler Scheduler { get; }

        protected AsyncOperationViewModel(TaskScheduler scheduler)
        {
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        protected abstract object GetAsyncState();

        private static TaskContinuationOptions ToContinuationOptions(TaskCreationOptions options)
        {
            TaskContinuationOptions result = TaskContinuationOptions.None;
            if (options.HasFlag(TaskCreationOptions.AttachedToParent))
                result = TaskContinuationOptions.AttachedToParent;
            if (options.HasFlag(TaskCreationOptions.DenyChildAttach))
                result |= TaskContinuationOptions.DenyChildAttach;
            if (options.HasFlag(TaskCreationOptions.HideScheduler))
                result |= TaskContinuationOptions.HideScheduler;
            if (options.HasFlag(TaskCreationOptions.LongRunning))
                result |= TaskContinuationOptions.LongRunning;
            if (options.HasFlag(TaskCreationOptions.PreferFairness))
                result |= TaskContinuationOptions.PreferFairness;
            if (options.HasFlag(TaskCreationOptions.RunContinuationsAsynchronously))
                result |= TaskContinuationOptions.RunContinuationsAsynchronously;
            return result;
        }

        protected virtual void OnTaskStarted(CancellationToken token)
        {
            StatusMessage = StatusMessage_Running;
            StatusValue = TaskStatus.Running;
        }

        protected virtual void OnTaskStarted(object state, CancellationToken token) => OnTaskStarted(token);

        protected virtual void OnTaskCanceled()
        {
            StatusMessage = StatusMessage_Canceled;
            StatusValue = TaskStatus.Canceled;
        }

        protected virtual void OnTaskCanceled(object state) => OnTaskCanceled(state);

        protected virtual void OnTaskFaulted(AggregateException exception)
        {
            StatusMessage = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            Error = exception;
            StatusValue = TaskStatus.Faulted;
        }

        protected virtual void OnTaskFaulted(AggregateException exception, object state) => OnTaskFaulted(exception);

        protected virtual void OnTaskRanToCompletion()
        {
            StatusMessage = StatusMessage_Finished;
            StatusValue = TaskStatus.RanToCompletion;
        }

        protected virtual void OnTaskRanToCompletion(object state) => OnTaskRanToCompletion();

        protected virtual void OnTaskRanToCompletion<TResult>(TResult result) => OnTaskRanToCompletion();

        protected virtual void OnTaskRanToCompletion<TResult>(object state, TResult result) => OnTaskRanToCompletion(result);

        protected virtual void OnAsyncOperationScheduled<TResult>(Task<TResult> task, object state)
        {
            OnAsyncOperationScheduled((Task)task, state);
        }

        protected virtual void OnAsyncOperationScheduled(Task task, object state)
        {
            OnAsyncOperationScheduled(task);
        }

        protected virtual void OnAsyncOperationScheduled<TResult>(Task<TResult> task)
        {
            OnAsyncOperationScheduled((Task)task);
        }

        protected virtual void OnAsyncOperationScheduled(Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.Canceled:
                    StatusValue = TaskStatus.Canceled;
                    StatusMessage = StatusMessage_Canceled;
                    Error = null;
                    break;
                case TaskStatus.Faulted:
                    StatusValue = TaskStatus.Faulted;
                    StatusMessage = string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message;
                    Error = task.Exception;
                    break;
                case TaskStatus.RanToCompletion:
                    StatusValue = TaskStatus.RanToCompletion;
                    StatusMessage = StatusMessage_Finished;
                    Error = null;
                    break;
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    StatusValue = TaskStatus.Running;
                    StatusMessage = StatusMessage_Running;
                    Error = null;
                    break;
                default:
                    StatusValue = TaskStatus.WaitingToRun;
                    StatusMessage = StatusMessage_Starting;
                    Error = null;
                    break;
            }
        }

        private void RaiseAsyncOperationScheduled<TResult>(Task<TResult> task, object state)
        {
            _logger.LogDebug("Task {Id} scheduled", (_asyncOperationTask = task).Id);
            Dispatcher.BeginInvoke(new Action<Task<TResult>, object>(OnAsyncOperationScheduled), task, state);
        }

        private void RaiseAsyncOperationScheduled(Task task, object state)
        {
            _logger.LogDebug("Task {Id} scheduled", (_asyncOperationTask = task).Id);
            Dispatcher.BeginInvoke(new Action<Task, object>(OnAsyncOperationScheduled), task, state);
        }

        private void RaiseAsyncOperationScheduled<TResult>(Task<TResult> task)
        {
            _logger.LogDebug("Task {Id} scheduled", (_asyncOperationTask = task).Id);
            Dispatcher.BeginInvoke(new Action<Task<TResult>>(OnAsyncOperationScheduled), task);
        }

        private void RaiseAsyncOperationScheduled(Task task)
        {
            _logger.LogDebug("Task {Id} scheduled", (_asyncOperationTask = task).Id);
            Dispatcher.BeginInvoke(new Action<Task>(OnAsyncOperationScheduled), task);
        }

        private void RaiseTaskStarted(Action<CancellationToken> action, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                _logger.LogDebug("Task {Id} cancelling", Task.CurrentId);
            else
            {
                _logger.LogDebug("Task {Id} running", Task.CurrentId);
                Dispatcher.BeginInvoke(new Action<CancellationToken>(OnTaskStarted), token);
                action(token);
            }
        }

        private void RaiseTaskStarted(Action<object, CancellationToken> action, object state, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                _logger.LogDebug("Task {Id} cancelling", Task.CurrentId);
            else
            {
                _logger.LogDebug("Task {Id} running", Task.CurrentId);
                Dispatcher.BeginInvoke(new Action<object, CancellationToken>(OnTaskStarted), state, token);
                action(state, token);
            }
        }

        private TResult RaiseTaskStarted<TResult>(Func<CancellationToken, TResult> function, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                _logger.LogDebug("Task {Id} cancelling", Task.CurrentId);
                return default;
            }
            _logger.LogDebug("Task {Id} running", Task.CurrentId);
            Dispatcher.BeginInvoke(new Action<CancellationToken>(OnTaskStarted), token);
            return function(token);
        }

        private TResult RaiseTaskStarted<TResult>(Func<object, CancellationToken, TResult> function, object state, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                _logger.LogDebug("Task {Id} cancelling", Task.CurrentId);
                return default;
            }
            _logger.LogDebug("Task {Id} running", Task.CurrentId);
            Dispatcher.BeginInvoke(new Action<object, CancellationToken>(OnTaskStarted), state, token);
            return function(state, token);
        }

        private void RaiseTaskCompleted<TResult>(Task<TResult> task)
        {
            if (!ReferenceEquals(task, _asyncOperationTask))
                return;
            if (task.IsCanceled)
            {
                _logger.LogDebug("Task {Id} canceled.", task.Id);
                Dispatcher.BeginInvoke(new Action(OnTaskCanceled));
            }
            else if (task.IsFaulted)
            {
                _logger.LogError(task.Exception, "Task {Id} faulted.", task.Id);
                Dispatcher.BeginInvoke(new Action<AggregateException>(OnTaskFaulted), task.Exception);
            }
            else
            {
                _logger.LogDebug("Task {Id} returned {Result}", task.Id, task.Result);
                Dispatcher.BeginInvoke(new Action<TResult>(OnTaskRanToCompletion), task.Result);
            }
        }

        private void RaiseTaskCompleted(Task task)
        {
            if (!ReferenceEquals(task, _asyncOperationTask))
                return;
            if (task.IsCanceled)
            {
                _logger.LogDebug("Task {Id} canceled.", task.Id);
                Dispatcher.BeginInvoke(new Action(OnTaskCanceled));
            }
            else if (task.IsFaulted)
            {
                _logger.LogError(task.Exception, "Task {Id} faulted.", task.Id);
                Dispatcher.BeginInvoke(new Action<AggregateException>(OnTaskFaulted), task.Exception);
            }
            else
            {
                _logger.LogDebug("Task {Id} ran to completion", task.Id);
                Dispatcher.BeginInvoke(new Action(OnTaskRanToCompletion));
            }
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T, TResult}"><c>&lt;<seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected Task<TResult> StartNew<TResult>(Func<CancellationToken, TResult> function) => StartNew(function, TaskCreationOptions.None);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T}"><c>&lt;<seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts the
        /// current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected Task StartNew(Action<CancellationToken> action) => StartNew(action, TaskCreationOptions.None);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T1, T2}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts
        /// the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="action"/> delegate.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task"/>.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual Task StartNew(Action<object, CancellationToken> action, object state, TaskCreationOptions creationOptions)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCompleted)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    RaiseAsyncOperationScheduled(Task.Factory.StartNew(obj => RaiseTaskStarted(action, obj, token), state, token, creationOptions, Scheduler), state);
                }
                else
                {
                    CancellationTokenSource tokenSource = _cancellationTokenSource;
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    if (!tokenSource.IsCancellationRequested)
                    {
                        _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                        tokenSource.Cancel(true);
                    }
                    _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                    RaiseAsyncOperationScheduled(_asyncOperationTask.ContinueWith((task, obj) => RaiseTaskStarted(action, obj, token), state, token,
                        ToContinuationOptions(creationOptions), Scheduler), state);
                }
                _asyncOperationTask.ContinueWith(RaiseTaskCompleted);
            }
            return _asyncOperationTask;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T1, T2, TResult}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="function"/> delegate.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual Task<TResult> StartNew<TResult>(Func<object, CancellationToken, TResult> function, object state) => StartNew(function, state, TaskCreationOptions.None);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T1, T2, TResult}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="function"/> delegate.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task{TResult}"/>.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual Task<TResult> StartNew<TResult>(Func<object, CancellationToken, TResult> function, object state, TaskCreationOptions creationOptions)
        {
            VerifyAccess();
            Task<TResult> asyncOperationTask;
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCompleted)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOperationTask = Task.Factory.StartNew(obj => RaiseTaskStarted(function, obj, token), state, token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOperationTask, state);
                }
                else
                {
                    CancellationTokenSource tokenSource = _cancellationTokenSource;
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    if (!tokenSource.IsCancellationRequested)
                    {
                        _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                        tokenSource.Cancel(true);
                    }
                    _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                    asyncOperationTask = _asyncOperationTask.ContinueWith((task, obj) => RaiseTaskStarted(function, obj, token), state, token,
                        ToContinuationOptions(creationOptions), Scheduler);
                    RaiseAsyncOperationScheduled(asyncOperationTask, state);
                }
                _logger.LogDebug("Task {Id} scheduled", asyncOperationTask.Id);
                asyncOperationTask.ContinueWith(RaiseTaskCompleted);
            }
            return asyncOperationTask;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T}"><c>&lt;<seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts the
        /// current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task"/>.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected Task StartNew(Action<CancellationToken> action, TaskCreationOptions creationOptions)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCompleted)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    RaiseAsyncOperationScheduled(Task.Factory.StartNew(() => RaiseTaskStarted(action, token), token, creationOptions, Scheduler));
                }
                else
                {
                    CancellationTokenSource tokenSource = _cancellationTokenSource;
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    if (!tokenSource.IsCancellationRequested)
                    {
                        _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                        tokenSource.Cancel(true);
                    }
                    _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                    RaiseAsyncOperationScheduled(_asyncOperationTask.ContinueWith(t => RaiseTaskStarted(action, token), token, ToContinuationOptions(creationOptions), Scheduler));
                }
                _logger.LogDebug("Task {Id} scheduled", _asyncOperationTask.Id);
                _asyncOperationTask.ContinueWith(RaiseTaskCompleted);
            }
            return _asyncOperationTask;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T, TResult}"><c>&lt;<seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task{TResult}"/>.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected Task<TResult> StartNew<TResult>(Func<CancellationToken, TResult> function, TaskCreationOptions creationOptions)
        {
            VerifyAccess();
            Task<TResult> asyncOperationTask;
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCompleted)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOperationTask = Task.Factory.StartNew(() => RaiseTaskStarted(function, token), token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOperationTask);
                }
                else
                {
                    CancellationTokenSource tokenSource = _cancellationTokenSource;
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    if (!tokenSource.IsCancellationRequested)
                    {
                        _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                        tokenSource.Cancel(true);
                    }
                    _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                    asyncOperationTask = _asyncOperationTask.ContinueWith(t => RaiseTaskStarted(function, token), token,
                        ToContinuationOptions(creationOptions), Scheduler);
                    RaiseAsyncOperationScheduled(asyncOperationTask);
                }
                _logger.LogDebug("Task {Id} scheduled", asyncOperationTask.Id);
                asyncOperationTask.ContinueWith(RaiseTaskCompleted);
            }
            return asyncOperationTask;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation, cancelling the previous <seealso cref="Task"/> if one is currently running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T1, T2}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts
        /// the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="action"/> delegate.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual Task StartNew(Action<object, CancellationToken> action, object state) => StartNew(action, state, TaskCreationOptions.None);

        /// <summary>
        /// Ensures that a <seealso cref="Task{TResult}"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the
        /// current asynchronous operation if necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T, TResult}"><c>&lt;<seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the first asynchronous operation;
        /// otherwise, <see langword="false"/> to indicate that a previous <seealso cref="Task{TResult}"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> is null.</exception>
        /// <exception cref="InvalidOperationException">A previous <seealso cref="Task"/> is still running as the asynchronous operation and its type does not match the
        /// result <seealso cref="Task{TResult}"/> type for this function
        /// <para>-or-</para>
        /// <para>The calling thread does not have access to this <see cref="AsyncOperationViewModel"/></para>.</exception>
        protected Task<TResult> EnsureOperation<TResult>(Func<CancellationToken, TResult> function, out bool newOpstarted) =>
            EnsureOperation(function, TaskCreationOptions.None, out newOpstarted);

        /// <summary>
        /// Ensures that a <seealso cref="Task"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the current asynchronous
        /// operation if necessary.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T}"><c>&lt;<seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts the
        /// current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task"/> was started as the first asynchronous operation; otherwise, <see langword="false"/>
        /// to indicate that a previous <seealso cref="Task"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected Task EnsureOperation(Action<CancellationToken> action, out bool newOpstarted) =>
            EnsureOperation(action, TaskCreationOptions.None, out newOpstarted);

        /// <summary>
        /// Ensures that a <seealso cref="Task"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the current asynchronous
        /// operation if necessary.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T1, T2}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts
        /// the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="action"/> delegate.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task"/>.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task"/> was started as the first asynchronous operation; otherwise, <see langword="false"/>
        /// to indicate that a previous <seealso cref="Task"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual Task EnsureOperation(Action<object, CancellationToken> action, object state, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    RaiseAsyncOperationScheduled(Task.Factory.StartNew(obj => RaiseTaskStarted(action, obj, token), state, token, creationOptions, Scheduler), state);
                    _logger.LogDebug("Task {Id} scheduled", _asyncOperationTask.Id);
                    newOpstarted = true;
                    _asyncOperationTask.ContinueWith(RaiseTaskCompleted);
                }
                else
                {
                    _logger.LogDebug("Returning existing task {Id}", _asyncOperationTask.Id);
                    newOpstarted = false;
                }
            }
            return _asyncOperationTask;
        }

        /// <summary>
        /// Ensures that a <seealso cref="Task{TResult}"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the
        /// current asynchronous operation if necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T1, T2, TResult}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="function"/> delegate.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the first asynchronous operation;
        /// otherwise, <see langword="false"/> to indicate that a previous <seealso cref="Task{TResult}"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">A previous <seealso cref="Task"/> is still running as the asynchronous operation and its type does not match the
        /// result <seealso cref="Task{TResult}"/> type for this function
        /// <para>-or-</para>
        /// <para>The calling thread does not have access to this <see cref="AsyncOperationViewModel"/></para>.</exception>
        protected virtual Task<TResult> EnsureOperation<TResult>(Func<object, CancellationToken, TResult> function, object state, out bool newOpstarted) =>
            EnsureOperation(function, state, TaskCreationOptions.None, out newOpstarted);

        /// <summary>
        /// Ensures that a <seealso cref="Task{TResult}"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the
        /// current asynchronous operation if necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T1, T2, TResult}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="function"/> delegate.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task{TResult}"/>.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the first asynchronous operation;
        /// otherwise, <see langword="false"/> to indicate that a previous <seealso cref="Task{TResult}"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">A previous <seealso cref="Task"/> is still running as the asynchronous operation and its type does not match the
        /// result <seealso cref="Task{TResult}"/> type for this function
        /// <para>-or-</para>
        /// <para>The calling thread does not have access to this <see cref="AsyncOperationViewModel"/></para>.</exception>
        protected virtual Task<TResult> EnsureOperation<TResult>(Func<object, CancellationToken, TResult> function, object state, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            VerifyAccess();
            Task<TResult> asyncOperationTask;
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    _asyncOperationTask = asyncOperationTask = Task.Factory.StartNew(obj => RaiseTaskStarted(function, obj, token), state, token, creationOptions, Scheduler);
                }
                else
                {
                    if (_asyncOperationTask is Task<TResult> task)
                    {
                        newOpstarted = false;
                        _logger.LogDebug("Returning existing task {Id}", _asyncOperationTask.Id);
                        return task;
                    }
                    if (_asyncOperationTask.IsCompleted)
                    {
                        CancellationTokenSource tokenSource = _cancellationTokenSource;
                        CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                        if (!tokenSource.IsCancellationRequested)
                        {
                            _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                            tokenSource.Cancel(true);
                        }
                        _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                        _asyncOperationTask = asyncOperationTask = _asyncOperationTask.ContinueWith((t, obj) => RaiseTaskStarted(function, obj, token), state, token,
                            ToContinuationOptions(creationOptions), Scheduler);
                    }
                    else
                        throw new InvalidOperationException("Running async operation type mismatch");
                }
                _logger.LogDebug("Task {Id} scheduled", asyncOperationTask.Id);
                asyncOperationTask.ContinueWith(RaiseTaskCompleted);
            }
            newOpstarted = true;
            return asyncOperationTask;
        }

        /// <summary>
        /// Ensures that a <seealso cref="Task"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the current asynchronous
        /// operation if necessary.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T}"><c>&lt;<seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts the
        /// current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task"/>.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task"/> was started as the first asynchronous operation; otherwise, <see langword="false"/>
        /// to indicate that a previous <seealso cref="Task"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected Task EnsureOperation(Action<CancellationToken> action, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    RaiseAsyncOperationScheduled(Task.Factory.StartNew(() => RaiseTaskStarted(action, token), token, creationOptions, Scheduler));
                    _logger.LogDebug("Task {Id} scheduled", _asyncOperationTask.Id);
                    newOpstarted = true;
                    _asyncOperationTask.ContinueWith(RaiseTaskCompleted);
                }
                else
                {
                    _logger.LogDebug("Returning existing task {Id}", _asyncOperationTask.Id);
                    newOpstarted = false;
                }
            }
            return _asyncOperationTask;
        }

        /// <summary>
        /// Ensures that a <seealso cref="Task{TResult}"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the
        /// current asynchronous operation if necessary.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T, TResult}"><c>&lt;<seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task{TResult}"/>.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the first asynchronous operation;
        /// otherwise, <see langword="false"/> to indicate that a previous <seealso cref="Task{TResult}"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">A previous <seealso cref="Task"/> is still running as the asynchronous operation and its type does not match the
        /// result <seealso cref="Task{TResult}"/> type for this function
        /// <para>-or-</para>
        /// <para>The calling thread does not have access to this <see cref="AsyncOperationViewModel"/></para>.</exception>
        protected Task<TResult> EnsureOperation<TResult>(Func<CancellationToken, TResult> function, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            VerifyAccess();
            Task<TResult> asyncOperationTask;
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOperationTask = Task.Factory.StartNew(() => RaiseTaskStarted(function, token), token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOperationTask);
                }
                else
                {
                    if (_asyncOperationTask is Task<TResult> task)
                    {
                        _logger.LogDebug("Returning existing task {Id}", _asyncOperationTask.Id);
                        newOpstarted = false;
                        return task;
                    }
                    if (_asyncOperationTask.IsCompleted)
                    {
                        CancellationTokenSource tokenSource = _cancellationTokenSource;
                        CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                        if (!tokenSource.IsCancellationRequested)
                        {
                            _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                            tokenSource.Cancel(true);
                        }
                        _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                        asyncOperationTask = _asyncOperationTask.ContinueWith(t => RaiseTaskStarted(function, token), token,
                            ToContinuationOptions(creationOptions), Scheduler);
                        RaiseAsyncOperationScheduled(asyncOperationTask);
                    }
                    else
                        throw new InvalidOperationException("Running async operation type mismatch");
                }
                _logger.LogDebug("Task {Id} scheduled", asyncOperationTask.Id);
                asyncOperationTask.ContinueWith(RaiseTaskCompleted);
            }
            newOpstarted = true;
            return asyncOperationTask;
        }

        /// <summary>
        /// Ensures that a <seealso cref="Task"/> is running or has run to completion, creating and starting a new <seealso cref="Task{TResult}"/> as the current asynchronous
        /// operation if necessary.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T1, T2}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts
        /// the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="action"/> delegate.</param>
        /// <param name="newOpstarted"><see langword="true"/> if a new <seealso cref="Task"/> was started as the first asynchronous operation; otherwise, <see langword="false"/>
        /// to indicate that a previous <seealso cref="Task"/> is still running or has run to completion.</param>
        /// <returns>The new <seealso cref="Task"/> that is running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual Task EnsureOperation(Action<object, CancellationToken> action, object state, out bool newOpstarted) =>
            EnsureOperation(action, state, TaskCreationOptions.None, out newOpstarted);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T, TResult}"><c>&lt;<seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation or <see langword="null"/> if there was
        /// already another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected bool TryStartNew<TResult>(Func<CancellationToken, TResult> function, out Task<TResult> asyncOpTask) =>
            TryStartNew(function, TaskCreationOptions.None, out asyncOpTask);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T}"><c>&lt;<seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts the
        /// current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task"/> that is running as the current asynchronous operation or <see langword="null"/> if there was already
        /// another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected bool TryStartNew(Action<CancellationToken> action, out Task asyncOpTask) => TryStartNew(action, TaskCreationOptions.None, out asyncOpTask);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T1, T2}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts
        /// the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="action"/> delegate.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task"/>.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task"/> that is running as the current asynchronous operation or <see langword="null"/> if there was already
        /// another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual bool TryStartNew(Action<object, CancellationToken> action, object state, TaskCreationOptions creationOptions, out Task asyncOpTask)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOpTask = Task.Factory.StartNew(obj => RaiseTaskStarted(action, obj, token), state, token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOpTask, state);
                    _logger.LogDebug("Task {Id} scheduled", asyncOpTask.Id);
                    asyncOpTask.ContinueWith(RaiseTaskCompleted);
                    return true;
                }
                asyncOpTask = null;
                _logger.LogDebug("Returning false due to existing task {Id}", _asyncOperationTask.Id);
            }
            return false;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T1, T2, TResult}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="function"/> delegate.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation or <see langword="null"/> if there was
        /// already another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual bool TryStartNew<TResult>(Func<object, CancellationToken, TResult> function, object state, out Task<TResult> asyncOpTask) =>
            TryStartNew(function, state, TaskCreationOptions.None, out asyncOpTask);

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T1, T2, TResult}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="function"/> delegate.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task{TResult}"/>.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation or <see langword="null"/> if there was
        /// already another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual bool TryStartNew<TResult>(Func<object, CancellationToken, TResult> function, object state, TaskCreationOptions creationOptions, out Task<TResult> asyncOpTask)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOpTask = Task.Factory.StartNew(obj => RaiseTaskStarted(function, obj, token), state, token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOpTask, state);
                }
                else
                {
                    if (_asyncOperationTask is Task<TResult> task)
                    {
                        asyncOpTask = null;
                        _logger.LogDebug("Returning false due to existing task {Id}", _asyncOperationTask.Id);
                        return false;
                    }
                    if (_asyncOperationTask.IsCompleted)
                    {
                        CancellationTokenSource tokenSource = _cancellationTokenSource;
                        CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                        if (!tokenSource.IsCancellationRequested)
                        {
                            _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                            tokenSource.Cancel(true);
                        }
                        _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                        asyncOpTask = _asyncOperationTask.ContinueWith((t, obj) => RaiseTaskStarted(function, obj, token), state, token,
                            ToContinuationOptions(creationOptions), Scheduler);
                        RaiseAsyncOperationScheduled(asyncOpTask, state);
                    }
                    else
                        throw new InvalidOperationException("Running async operation type mismatch");
                }
                _logger.LogDebug("Task {Id} scheduled", asyncOpTask.Id);
                asyncOpTask.ContinueWith(RaiseTaskCompleted);
            }
            return true;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T}"><c>&lt;<seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts the
        /// current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task"/>.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task"/> that is running as the current asynchronous operation or <see langword="null"/> if there was already
        /// another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected bool TryStartNew(Action<CancellationToken> action, TaskCreationOptions creationOptions, out Task asyncOpTask)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOpTask = Task.Factory.StartNew(() => RaiseTaskStarted(action, token), token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOpTask);
                    _logger.LogDebug("Task {Id} scheduled", asyncOpTask.Id);
                    asyncOpTask.ContinueWith(RaiseTaskCompleted);
                    return true;
                }
                asyncOpTask = null;
                _logger.LogDebug("Returning false due to existing task {Id}", _asyncOperationTask.Id);
            }
            return false;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task{TResult}"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <typeparam name="TResult">The type of the result available through the <seealso cref="Task{TResult}"/>.</typeparam>
        /// <param name="function">A <seealso cref="Func{T, TResult}"><c>&lt;<seealso cref="CancellationToken"/>, <typeparamref name="TResult"/>&gt;</c></seealso>
        /// delegate that accepts the current <seealso cref="CancellationToken"/> and returns the <typeparamref name="TResult"/> value.</param>
        /// <param name="creationOptions">A <seealso cref="TaskCreationOptions"/> value that controls the behavior of the created <seealso cref="Task{TResult}"/>.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task{TResult}"/> that is running as the current asynchronous operation or <see langword="null"/> if there was
        /// already another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task{TResult}"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="function"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> argument contains in invalid <seealso cref="TaskCreationOptions"/> value.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected bool TryStartNew<TResult>(Func<CancellationToken, TResult> function, TaskCreationOptions creationOptions, out Task<TResult> asyncOpTask)
        {
            VerifyAccess();
            lock (_syncRoot)
            {
                if (_asyncOperationTask is null || _asyncOperationTask.IsCanceled)
                {
                    if (!(_cancellationTokenSource is null))
                        _cancellationTokenSource.Dispose();
                    CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                    asyncOpTask = Task.Factory.StartNew(() => RaiseTaskStarted(function, token), token, creationOptions, Scheduler);
                    RaiseAsyncOperationScheduled(asyncOpTask);
                }
                else
                {
                    if (_asyncOperationTask is Task<TResult> task)
                    {
                        asyncOpTask = null;
                        _logger.LogDebug("Returning false due to existing task {Id}", _asyncOperationTask.Id);
                        return false;
                    }
                    if (_asyncOperationTask.IsCompleted)
                    {
                        CancellationTokenSource tokenSource = _cancellationTokenSource;
                        CancellationToken token = (_cancellationTokenSource = new CancellationTokenSource()).Token;
                        if (!tokenSource.IsCancellationRequested)
                        {
                            _logger.LogDebug("Canceling task {Id}", _asyncOperationTask.Id);
                            tokenSource.Cancel(true);
                        }
                        _asyncOperationTask.ContinueWith(t => tokenSource.Dispose());
                        asyncOpTask = _asyncOperationTask.ContinueWith(t=> RaiseTaskStarted(function, token), token,
                            ToContinuationOptions(creationOptions), Scheduler);
                        RaiseAsyncOperationScheduled(asyncOpTask);
                    }
                    else
                        throw new InvalidOperationException("Running async operation type mismatch");
                }
                _logger.LogDebug("Task {Id} scheduled", asyncOpTask.Id);
                asyncOpTask.ContinueWith(RaiseTaskCompleted);
            }
            return true;
        }

        /// <summary>
        /// Creates and starts a new <seealso cref="Task"/> as the current asynchronous operation unless a previous <seealso cref="Task"/> is still running.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{T1, T2}"><c>&lt;<seealso cref="object"/>, <seealso cref="CancellationToken"/>&gt;</c></seealso> delegate that accepts
        /// the <paramref name="state"/> value and the current <seealso cref="CancellationToken"/> and runs asynchronously.</param>
        /// <param name="state">An object containing data to be used by the <paramref name="action"/> delegate.</param>
        /// <param name="asyncOpTask">Returns the new <seealso cref="Task"/> that is running as the current asynchronous operation or <see langword="null"/> if there was already
        /// another <seealso cref="Task"/> running as the current asynchronous operation.</param>
        /// <returns><see langword="true"/> if a new <seealso cref="Task"/> was started as the current asynchronous operation; otherwise, <see langword="false"/>
        /// if there was already another <seealso cref="Task"/> running as the current asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this <see cref="AsyncOperationViewModel"/>.</exception>
        protected virtual bool TryStartNew(Action<object, CancellationToken> action, object state, out Task asyncOpTask) =>
            TryStartNew(action, state, TaskCreationOptions.None, out asyncOpTask);
    }

    public abstract class AsyncOperationViewModel<TState> : AsyncOperationViewModel
    {
        private static readonly DependencyPropertyKey AsyncStatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(AsyncState), typeof(TState), typeof(AsyncOperationViewModel),
            new PropertyMetadata(default(TState)));

        public static readonly DependencyProperty AsyncStateProperty = AsyncStatePropertyKey.DependencyProperty;

        protected AsyncOperationViewModel(TaskScheduler scheduler) : base(scheduler) { }

        public TState AsyncState
        {
            get { return (TState)GetValue(AsyncStateProperty); }
            private set { SetValue(AsyncStatePropertyKey, value); }
        }

        protected virtual void OnTaskCanceled(TState state)
        {
            base.OnTaskCanceled(state);
        }

        protected override void OnTaskCanceled(object state) => OnTaskCanceled((TState)state);

        protected virtual void OnTaskFaulted(AggregateException exception, TState state)
        {
            base.OnTaskFaulted(exception, state);
        }

        protected override void OnTaskFaulted(AggregateException exception, object state) => OnTaskFaulted(exception, (TState)state);

        protected virtual void OnTaskRanToCompletion(TState state)
        {
            base.OnTaskRanToCompletion(state);
        }

        protected override void OnTaskRanToCompletion(object state) => OnTaskRanToCompletion((TState)state);

        protected virtual void OnTaskRanToCompletion<TResult>(TState state, TResult result)
        {
            base.OnTaskRanToCompletion(state, result);
        }

        protected override void OnTaskRanToCompletion<TResult>(object state, TResult result) => OnTaskRanToCompletion((TState)state, result);

        protected virtual void OnTaskStarted(TState state, CancellationToken token)
        {
            base.OnTaskStarted(state, token);
        }

        protected override void OnTaskStarted(object state, CancellationToken token) => OnTaskStarted((TState)state, token);

        protected virtual void OnAsyncOperationScheduled(Task task, TState state)
        {
            AsyncState = state;
            base.OnAsyncOperationScheduled(task, state);
        }

        protected override void OnAsyncOperationScheduled(Task task, object state) => OnAsyncOperationScheduled(task, (TState)state);

        protected virtual void OnAsyncOperationScheduled<TResult>(Task<TResult> task, TState state)
        {
            AsyncState = state;
            base.OnAsyncOperationScheduled(task, state);
        }

        protected override void OnAsyncOperationScheduled<TResult>(Task<TResult> task, object state) => OnAsyncOperationScheduled(task, (TState)state);

        protected virtual Task EnsureOperation(Action<TState, CancellationToken> action, TState state, out bool newOpstarted)
        {
            return base.EnsureOperation((obj, c) => action(state, c), state, out newOpstarted);
        }

        protected override Task EnsureOperation(Action<object, CancellationToken> action, object state, out bool newOpstarted) =>
            EnsureOperation(action, (TState)state, out newOpstarted);

        protected virtual Task EnsureOperation(Action<TState, CancellationToken> action, TState state, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            return base.EnsureOperation((obj, c) => action(state, c), state, creationOptions, out newOpstarted);
        }

        protected override Task EnsureOperation(Action<object, CancellationToken> action, object state, TaskCreationOptions creationOptions, out bool newOpstarted) =>
            EnsureOperation(action, (TState)state, creationOptions, out newOpstarted);

        protected virtual Task<TResult> EnsureOperation<TResult>(Func<TState, CancellationToken, TResult> function, TState state, out bool newOpstarted)
        {
            return base.EnsureOperation((obj, c) => function(state, c), state, out newOpstarted);
        }

        protected override Task<TResult> EnsureOperation<TResult>(Func<object, CancellationToken, TResult> function, object state, out bool newOpstarted) =>
            EnsureOperation(function, (TState)state, out newOpstarted);

        protected virtual Task<TResult> EnsureOperation<TResult>(Func<TState, CancellationToken, TResult> function, TState state, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            return base.EnsureOperation((obj, c) => function(state, c), state, creationOptions, out newOpstarted);
        }

        protected override Task<TResult> EnsureOperation<TResult>(Func<object, CancellationToken, TResult> function, object state, TaskCreationOptions creationOptions, out bool newOpstarted) =>
            EnsureOperation(function, (TState)state, creationOptions, out newOpstarted);

        protected virtual Task StartNew(Action<TState, CancellationToken> action, TState state)
        {
            return base.StartNew((obj, c) => action(state, c), state);
        }

        protected override Task StartNew(Action<object, CancellationToken> action, object state) =>
            StartNew(action, (TState)state);

        protected virtual Task StartNew(Action<TState, CancellationToken> action, TState state, TaskCreationOptions creationOptions)
        {
            return base.StartNew((obj, c) => action(state, c), state, creationOptions);
        }

        protected override Task StartNew(Action<object, CancellationToken> action, object state, TaskCreationOptions creationOptions) =>
            StartNew(action, (TState)state, creationOptions);

        protected virtual Task<TResult> StartNew<TResult>(Func<TState, CancellationToken, TResult> function, TState state)
        {
            return base.StartNew((obj, c) => function(state, c), state);
        }

        protected override Task<TResult> StartNew<TResult>(Func<object, CancellationToken, TResult> function, object state) =>
            StartNew(function, (TState)state);

        protected virtual Task<TResult> StartNew<TResult>(Func<TState, CancellationToken, TResult> function, TState state, TaskCreationOptions creationOptions)
        {
            return base.StartNew((obj, c) => function(state, c), state, creationOptions);
        }

        protected override Task<TResult> StartNew<TResult>(Func<object, CancellationToken, TResult> function, object state, TaskCreationOptions creationOptions) =>
            StartNew(function, (TState)state, creationOptions);

        protected virtual bool TryStartNew(Action<TState, CancellationToken> action, TState state, out Task asyncOpTask)
        {
            return base.TryStartNew((obj, c) => action(state, c), state, out asyncOpTask);
        }

        protected override bool TryStartNew(Action<object, CancellationToken> action, object state, out Task asyncOpTask) =>
            TryStartNew(action, (TState)state, out asyncOpTask);

        protected virtual bool TryStartNew(Action<TState, CancellationToken> action, TState state, TaskCreationOptions creationOptions, out Task asyncOpTask)
        {
            return base.TryStartNew((obj, c) => action(state, c), state, creationOptions, out asyncOpTask);
        }

        protected override bool TryStartNew(Action<object, CancellationToken> action, object state, TaskCreationOptions creationOptions, out Task asyncOpTask) =>
            TryStartNew(action, (TState)state, creationOptions, out asyncOpTask);

        protected virtual bool TryStartNew<TResult>(Func<TState, CancellationToken, TResult> function, TState state, out Task<TResult> asyncOpTask)
        {
            return base.TryStartNew((obj, c) => function(state, c), state, out asyncOpTask);
        }

        protected override bool TryStartNew<TResult>(Func<object, CancellationToken, TResult> function, object state, out Task<TResult> asyncOpTask) =>
            TryStartNew(function, (TState)state, out asyncOpTask);

        protected virtual bool TryStartNew<TResult>(Func<TState, CancellationToken, TResult> function, TState state, TaskCreationOptions creationOptions, out Task<TResult> asyncOpTask)
        {
            return base.TryStartNew((obj, c) => function(state, c), state, creationOptions, out asyncOpTask);
        }

        protected override bool TryStartNew<TResult>(Func<object, CancellationToken, TResult> function, object state, TaskCreationOptions creationOptions, out Task<TResult> asyncOpTask) =>
            TryStartNew(function, (TState)state, creationOptions, out asyncOpTask);

        protected override object GetAsyncState() => AsyncState;
    }

}
