using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ModalOperationStatusViewModel : DependencyObject
    {
        private const string CancelButtonContent_Cancel = "Cancel";
        private const string CancelButtonContent_OK = "OK";
        private static readonly DependencyPropertyKey IsModalPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsModal), typeof(bool), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsModalProperty = IsModalPropertyKey.DependencyProperty;

        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            private set { SetValue(IsModalPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsIndeterminatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsIndeterminate), typeof(bool),
            typeof(ModalOperationStatusViewModel), new PropertyMetadata(true));

        public static readonly DependencyProperty IsIndeterminateProperty = IsIndeterminatePropertyKey.DependencyProperty;

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            private set { SetValue(IsIndeterminatePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ProgressPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Progress), typeof(int), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(0));

        public static readonly DependencyProperty ProgressProperty = ProgressPropertyKey.DependencyProperty;

        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            private set { SetValue(ProgressPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HeadingPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Heading), typeof(string), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty HeadingProperty = HeadingPropertyKey.DependencyProperty;

        public string Heading
        {
            get { return GetValue(HeadingProperty) as string; }
            private set { SetValue(HeadingPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Message), typeof(string), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MessageProperty = MessagePropertyKey.DependencyProperty;

        public string Message
        {
            get { return GetValue(MessageProperty) as string; }
            private set { SetValue(MessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(TaskStatus), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(TaskStatus.Created));

        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        public TaskStatus Status
        {
            get { return (TaskStatus)GetValue(StatusProperty); }
            private set { SetValue(StatusPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey CancelButtonTextPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(CancelButtonText), typeof(string), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(CancelButtonContent_Cancel));

        public static readonly DependencyProperty CancelButtonTextProperty = CancelButtonTextPropertyKey.DependencyProperty;

        public string CancelButtonText
        {
            get { return GetValue(CancelButtonTextProperty) as string; }
            private set { SetValue(CancelButtonTextPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(ModalOperationStatusViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((ModalOperationStatusViewModel)d).CancelCurrentOperation)));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private readonly object _syncRoot = new object();
        private CancellationTokenSource _cancellationTokenSource;
        private Task _currentTask;

        internal void CancelCurrentOperation()
        {
            VerifyAccess();
            CancellationTokenSource cancellationTokenSource;
            lock (_syncRoot)
            {
                cancellationTokenSource = _cancellationTokenSource;
                _cancellationTokenSource = null;
            }
            if (cancellationTokenSource is null)
                IsModal = false;
            else if (!cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel(true);
        }

        private void OnTaskCompleted(Task task, CancellationTokenSource cancellationTokenSource)
        {
            bool isCurrentTask;
            lock (_syncRoot)
            {
                isCurrentTask = ReferenceEquals(cancellationTokenSource, _cancellationTokenSource);
                if (isCurrentTask)
                    _cancellationTokenSource = null;
            }
            cancellationTokenSource.Dispose();
            if (isCurrentTask)
                Dispatcher.Invoke(new Action(() =>
                {
                    CancelButtonText = CancelButtonContent_OK;
                    switch (task.Status)
                    {
                        case TaskStatus.Canceled:
                            IsModal = true;
                            if (Status == TaskStatus.Canceled)
                                return;
                            Status = TaskStatus.Canceled;
                            Message = "Operation canceled";
                            break;
                        case TaskStatus.Faulted:
                            IsModal = true;
                            if (Status == TaskStatus.Faulted)
                                return;
                            Status = TaskStatus.Faulted;
                            Message = string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message;
                            break;
                        case TaskStatus.RanToCompletion:
                            switch (Status)
                            {
                                case TaskStatus.Canceled:
                                case TaskStatus.Faulted:
                                    IsModal = true;
                                    break;
                                default:
                                    Status = TaskStatus.RanToCompletion;
                                    IsModal = !string.IsNullOrWhiteSpace(Message);
                                    break;
                            }
                            break;
                        default:
                            Message = $"Unexpected task status: {task.Status}";
                            Status = TaskStatus.RanToCompletion;
                            IsModal = true;
                            break;
                    }
                }));
        }

        private CancellationTokenSource CreateNewTokenSource(string heading, string initialMessage)
        {
            VerifyAccess();
            CancellationTokenSource newTokenSource = new CancellationTokenSource();
            CancellationTokenSource oldTokenSource;
            lock (_syncRoot)
            {
                oldTokenSource = _cancellationTokenSource;
                _cancellationTokenSource = newTokenSource;
                if (!(_currentTask is null || oldTokenSource is null) && (_currentTask.IsCompleted || oldTokenSource.IsCancellationRequested))
                    oldTokenSource = null;
            }
            if (!(oldTokenSource is null))
                oldTokenSource.Cancel(true);
            Progress = 0;
            IsIndeterminate = true;
            Status = TaskStatus.Created;
            Heading = heading ?? "";
            Message = initialMessage ?? "";
            IsModal = true;
            CancelButtonText = CancelButtonContent_Cancel;
            return newTokenSource;
        }

        internal async Task<TResult> StartNew<T1, T2, TResult>(string heading, string initialMessage, Func<Controller, T1, T2, Task<TResult>> factory, T1 arg1, T2 arg2)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task<TResult> task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    return factory(controller, arg1, arg2).Result;
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            return await task;
        }

        internal async Task<TResult> StartNew<T1, T2, TResult>(string heading, string initialMessage, Func<Controller, T1, T2, TResult> function, T1 arg1, T2 arg2)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task<TResult> task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    return function(controller, arg1, arg2);
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            return await task;
        }

        internal async Task StartNew<T1, T2>(string heading, string initialMessage, Func<Controller, T1, T2, Task> factory, T1 arg1, T2 arg2)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    factory(controller, arg1, arg2).Wait();
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            await task;
        }

        internal async Task StartNew<T1, T2>(string heading, string initialMessage, Action<Controller, T1, T2> action, T1 arg1, T2 arg2)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    action(controller, arg1, arg2);
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            await task;
        }

        internal async Task<TResult> StartNew<T, TResult>(string heading, string initialMessage, Func<Controller, T, Task<TResult>> factory, T arg)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task<TResult> task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    return factory(controller, arg).Result;
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            return await task;
        }

        internal async Task<TResult> StartNew<T, TResult>(string heading, string initialMessage, Func<Controller, T, TResult> function, T arg)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task<TResult> task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    return function(controller, arg);
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            return await task;
        }

        internal async Task StartNew<T>(string heading, string initialMessage, Func<Controller, T, Task> factory, T arg)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    factory(controller, arg).Wait();
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            await task;
        }

        internal async Task StartNew<T>(string heading, string initialMessage, Action<Controller, T> action, T arg)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    action(controller, arg);
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            await task;
        }

        internal async Task<TResult> StartNew<T1, T2, TResult>(string heading, Func<Controller, T1, T2, TResult> function, T1 arg1, T2 arg2)
            => await StartNew(heading, null, function, arg1, arg2);

        internal async Task StartNew<T1, T2>(string heading, Action<Controller, T1, T2> action, T1 arg1, T2 arg2) => await StartNew(heading, null, action, arg1, arg2);

        internal async Task<TResult> StartNew<TResult>(string heading, string initialMessage, Func<Controller, Task<TResult>> factory)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task<TResult> task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    return factory(controller).Result;
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            return await task;
        }

        internal async Task<TResult> StartNew<TResult>(string heading, string initialMessage, Func<Controller, TResult> function)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task<TResult> task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    return function(controller);
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            return await task;
        }

        internal async Task StartNew(string heading, string initialMessage, Func<Controller, Task> factory)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    factory(controller).Wait();
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            await task;
        }

        internal async Task StartNew(string heading, string initialMessage, Action<Controller> action)
        {
            CancellationTokenSource cancellationTokenSource = CreateNewTokenSource(heading, initialMessage);
            Task task;
            Controller controller = new Controller(this, cancellationTokenSource.Token);
            lock (_syncRoot)
                _currentTask = task = Task.Factory.StartNew(() =>
                {
                    controller.SetStatus(TaskStatus.Running);
                    action(controller);
                }, cancellationTokenSource.Token);
            await task.ContinueWith(t => OnTaskCompleted(t, cancellationTokenSource));
            await task;
        }

        internal async Task<TResult> StartNew<T, TResult>(string heading, Func<Controller, T, TResult> function, T arg) => await StartNew(heading, (string)null, function, arg);

        internal async Task StartNew<T>(string heading, Action<Controller, T> action, T arg) => await StartNew(heading, (string)null, action, arg);

        internal async Task<TResult> StartNew<TResult>(string heading, Func<Controller, TResult> function) => await StartNew(heading, (string)null, function);

        internal async Task StartNew(string heading, Action<Controller> action) => await StartNew(heading, (string)null, action);

        public class Controller
        {
            private readonly ModalOperationStatusViewModel _target;

            public CancellationToken CancellationToken { get; }

            public bool IsCancellationRequested => CancellationToken.IsCancellationRequested;

            public void ThrowIfCancellationRequested() => CancellationToken.ThrowIfCancellationRequested();

            public Controller(ModalOperationStatusViewModel target, CancellationToken cancellationToken)
            {
                _target = target ?? throw new ArgumentNullException(nameof(target));
                CancellationToken = cancellationToken;
            }

            public void SetStatus(string heading, string message, int progress, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));
            }

            public void SetStatus(string message, int progress, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));
            }

            public void SetStatus(string heading, string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public void SetStatus(string heading, string message, int progress)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                }));
            }

            public void SetStatus(string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public void SetStatus(string message, int progress)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                }));
            }

            public void SetStatus(int progress, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Progress = progress;
                    _target.Status = status;
                }));
            }

            public void SetStatus(int progress)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Progress = progress;
                }));
            }

            public void SetStatus(TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() => _target.Status = status));
            }

            public void SetStatus(string heading, string message)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                }));
            }

            public void SetStatus(string message)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() => _target.Message = message));
            }

            public void SetIndeterminateStatus(string heading, string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public void SetIndeterminateStatus(string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public void SetIndeterminateStatus(string heading, string message)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Heading = heading;
                    _target.Message = message;
                }));
            }

            public void SetIndeterminateStatus(TaskStatus status)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Status = status;
                }));
            }

            public void SetIndeterminateStatus(string message)
            {
                ThrowIfCancellationRequested();
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                }));
            }

            public DispatcherOperation BeginSetStatus(string heading, string message, int progress, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetStatus(string message, int progress, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetStatus(string heading, string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetStatus(string heading, string message, int progress)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                }));
            }

            public DispatcherOperation BeginSetStatus(string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetStatus(string message, int progress)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                }));
            }

            public DispatcherOperation BeginSetStatus(int progress, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = false;
                    _target.Progress = progress;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetStatus(string heading, string message)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Heading = heading;
                    _target.Message = message;
                }));
            }

            public DispatcherOperation BeginSetStatus(int progress)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = false;
                    _target.Progress = progress;
                }));
            }

            public DispatcherOperation BeginSetStatus(TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetStatus(string message)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Message = message;
                }));
            }

            public DispatcherOperation BeginSetIndeterminateStatus(string heading, string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Heading = heading;
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetIndeterminateStatus(string message, TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetIndeterminateStatus(string heading, string message)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.Heading = heading;
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                }));
            }

            public DispatcherOperation BeginSetIndeterminateStatus(TaskStatus status)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = true;
                    _target.Status = status;
                }));
            }

            public DispatcherOperation BeginSetIndeterminateStatus(string message)
            {
                ThrowIfCancellationRequested();
                return _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ThrowIfCancellationRequested();
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                }));
            }
        }
    }
}
