using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM
    {
        internal class Job : IBackgroundJob
        {
            private readonly object _syncRoot = new();
            private readonly CancellationTokenSource _tokenSource = new();
            private readonly BackgroundJobVM _viewModel;
            private readonly Stopwatch _stopwatch;

            public Guid ConcurrencyId { get; } = Guid.NewGuid();

            public AsyncJobStatus JobStatus { get; private set; } = AsyncJobStatus.WaitingToRun;

            public Task Task { get; }

            public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

            public string Title { get; private set; }

            public string Message { get; private set; }

            public StatusMessageLevel MessageLevel { get; private set; }

            public TimeSpan Duration => _stopwatch.Elapsed;

            protected internal Job([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] BackgroundJobVM viewModel,
                [DisallowNull] Func<StatusListener, IBackgroundJob, Task> createTask)
            {
                Title = title;
                Message = initialMessage;
                _viewModel = viewModel;

                viewModel.TitlePropertyChanged += ViewModel_TitlePropertyChanged;
                viewModel.MessagePropertyChanged += ViewModel_MessagePropertyChanged;
                viewModel.MessageLevelPropertyChanged += ViewModel_MessageLevelPropertyChanged;
                viewModel.CancelInvoked += ViewModel_CancelInvoked;
                StatusListener statusListener = new StatusListener(viewModel, ConcurrencyId, _tokenSource.Token);
                Task = createTask(statusListener, this);
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
                Timer timer = new Timer(TimerTick, null, 1000, 1000);
                Task.ContinueWith(async t =>
                {
                    try { await timer.DisposeAsync(); }
                    finally
                    {
                        try { _stopwatch.Stop(); }
                        finally
                        {
                            if (SetJobStatus(out AsyncJobStatus newValue))
                                await _viewModel.Dispatcher.InvokeAsync(() =>
                                {
                                    try { _viewModel.Duration = _stopwatch.Elapsed; }
                                    finally { _viewModel.JobStatus = JobStatus; }
                                }, DispatcherPriority.Background);
                            else
                                await _viewModel.Dispatcher.InvokeAsync(() => _viewModel.Duration = _stopwatch.Elapsed, DispatcherPriority.Background);
                        }
                    }
                });
            }

            private void TimerTick(object state)
            {
                _viewModel.Dispatcher.Invoke(() => _viewModel.Duration = _stopwatch.Elapsed);
            }

            private void ViewModel_TitlePropertyChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e) => Title = e.NewValue as string ?? "";

            private void ViewModel_MessagePropertyChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e) => Message = e.NewValue as string ?? "";

            private void ViewModel_MessageLevelPropertyChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e) =>
                MessageLevel = e.NewValue as StatusMessageLevel? ?? StatusMessageLevel.Information;

            private bool SetJobStatus(out AsyncJobStatus newValue)
            {
                lock (_syncRoot)
                {
                    switch (Task.Status)
                    {
                        case TaskStatus.Running:
                        case TaskStatus.WaitingForChildrenToComplete:
                            newValue = _tokenSource.IsCancellationRequested ? AsyncJobStatus.Cancelling : AsyncJobStatus.Running;
                            break;
                        case TaskStatus.RanToCompletion:
                            newValue = AsyncJobStatus.Succeeded;
                            break;
                        case TaskStatus.Canceled:
                            newValue = AsyncJobStatus.Canceled;
                            break;
                        case TaskStatus.Faulted:
                            newValue = AsyncJobStatus.Faulted;
                            break;
                        default:
                            newValue = _tokenSource.IsCancellationRequested ? AsyncJobStatus.Cancelling : AsyncJobStatus.WaitingToRun;
                            break;
                    }
                    if (newValue == JobStatus)
                        return false;
                    JobStatus = newValue;
                }
                return true;
            }

            public async Task RaiseStatusChangedAsync()
            {
                if (SetJobStatus(out AsyncJobStatus newValue))
                    await _viewModel.Dispatcher.InvokeAsync(() => _viewModel.JobStatus = JobStatus, DispatcherPriority.Background);
            }

            private void ViewModel_CancelInvoked(object sender, Commands.CommandEventArgs e) => Cancel(true);

            public void Cancel(bool throwOnFirstException)
            {
                if (!(Task.IsCompleted || _tokenSource.IsCancellationRequested))
                    try { _viewModel.Dispatcher.CheckInvoke(() => _viewModel.JobStatus = AsyncJobStatus.Cancelling, DispatcherPriority.Background); }
                    finally { _tokenSource.Cancel(throwOnFirstException); }
            }

            public void Cancel()
            {
                if (!(Task.IsCompleted || _tokenSource.IsCancellationRequested))
                    try { _viewModel.Dispatcher.CheckInvoke(() => _viewModel.JobStatus = AsyncJobStatus.Cancelling, DispatcherPriority.Background); }
                    finally { _tokenSource.Cancel(); }
            }
        }

        class Job<TResult> : Job, IAsyncJob<TResult>
        {
            public new Task<TResult> Task => (Task<TResult>)base.Task;

            internal Job([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] BackgroundJobVM viewModel,
                [DisallowNull] Func<StatusListener, IBackgroundJob, Task<TResult>> createTask)
                : base(title, initialMessage, viewModel, (listener, b) => createTask(listener, b)) { }
        }
    }
}
