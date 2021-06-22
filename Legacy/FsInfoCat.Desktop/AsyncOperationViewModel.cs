using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public class AsyncOperationViewModel : DependencyObject, IAsyncOperation
    {
        private readonly object _syncRoot = new object();
        private Task _currentTask;
        private CancellationTokenSource _tokenSource;

        private static readonly DependencyPropertyKey IsCanceledPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsCanceled), typeof(bool), typeof(AsyncActionViewModel),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsCanceledProperty = IsCanceledPropertyKey.DependencyProperty;

        public bool IsCanceled
        {
            get { return (bool)GetValue(IsCanceledProperty); }
            private set { SetValue(IsCanceledPropertyKey, value); }
        }


        public event DependencyPropertyChangedEventHandler StatusPropertyChanged;

        private static readonly DependencyPropertyKey StatusPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Status), typeof(TaskStatus), typeof(AsyncActionViewModel),
                new PropertyMetadata(TaskStatus.Created, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncActionViewModel).OnStatusPropertyChanged(e)));

        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        public TaskStatus Status
        {
            get { return (TaskStatus)GetValue(StatusProperty); }
            private set { SetValue(StatusPropertyKey, value); }
        }

        protected virtual void OnStatusPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnStatusPropertyChanged((TaskStatus)args.OldValue, (TaskStatus)args.NewValue); }
            finally { StatusPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnStatusPropertyChanged(TaskStatus oldValue, TaskStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        private static readonly DependencyPropertyKey ExceptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Exception), typeof(AggregateException), typeof(AsyncActionViewModel),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ExceptionProperty = ExceptionPropertyKey.DependencyProperty;

        public AggregateException Exception
        {
            get { return (AggregateException)GetValue(ExceptionProperty); }
            private set { SetValue(ExceptionPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsFaultedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsFaulted), typeof(bool), typeof(AsyncActionViewModel),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsFaultedProperty = IsFaultedPropertyKey.DependencyProperty;

        public bool IsFaulted
        {
            get { return (bool)GetValue(IsFaultedProperty); }
            private set { SetValue(IsFaultedPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsCompletedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsCompleted), typeof(bool), typeof(AsyncActionViewModel),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsCompletedProperty = IsCompletedPropertyKey.DependencyProperty;

        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            private set { SetValue(IsCompletedPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey CompletedSynchronouslyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CompletedSynchronously), typeof(bool),
            typeof(AsyncActionViewModel), new PropertyMetadata(false));

        public static readonly DependencyProperty CompletedSynchronouslyProperty = CompletedSynchronouslyPropertyKey.DependencyProperty;

        public bool CompletedSynchronously
        {
            get { return (bool)GetValue(CompletedSynchronouslyProperty); }
            private set { SetValue(CompletedSynchronouslyPropertyKey, value); }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();

        object IAsyncResult.AsyncState => throw new NotImplementedException();

        protected static TaskContinuationOptions ToContinuationOptions(TaskCreationOptions options)
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

        protected TTask StartOperationAsync<TTask>(Func<CancellationToken, TTask> createTask, Func<Task, CancellationToken, TTask> continueTask)
            where TTask : Task
        {
            CancellationTokenSource tokenSource = _tokenSource;
            TTask result;
            if (tokenSource is null)
            {
                CancellationToken token = (_tokenSource = new CancellationTokenSource()).Token;
                _currentTask = result = createTask(token);
            }
            else if (_currentTask.IsCompleted)
            {
                tokenSource.Dispose();
                CancellationToken token = (_tokenSource = new CancellationTokenSource()).Token;
                _currentTask = result = createTask(token);
            }
            else
            {
                if (!tokenSource.IsCancellationRequested)
                    tokenSource.Cancel();
                _currentTask.ContinueWith(t => tokenSource.Dispose());
                CancellationToken token = (_tokenSource = new CancellationTokenSource()).Token;
                _currentTask = result = continueTask(_currentTask, token);
            }
            _currentTask.Start();
            return result;
        }

        protected TTask StartOperationAsync<TState, TTask>(TState state, Func<TState, CancellationToken, TTask> createTask, Func<TState, Task, CancellationToken, TTask> continueTask)
            where TTask : Task
        {
            CancellationTokenSource tokenSource = _tokenSource;
            TTask result;
            if (tokenSource is null)
            {
                CancellationToken token = (_tokenSource = new CancellationTokenSource()).Token;
                _currentTask = result = createTask(state, token);
            }
            else if (_currentTask.IsCompleted)
            {
                tokenSource.Dispose();
                CancellationToken token = (_tokenSource = new CancellationTokenSource()).Token;
                _currentTask = result = createTask(state, token);
            }
            else
            {
                if (!tokenSource.IsCancellationRequested)
                    tokenSource.Cancel();
                _currentTask.ContinueWith(t => tokenSource.Dispose());
                CancellationToken token = (_tokenSource = new CancellationTokenSource()).Token;
                _currentTask = result = continueTask(state, _currentTask, token);
            }
            _currentTask.Start();
            return result;
        }
    }
}
