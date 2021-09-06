using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobFactoryServiceViewModel
    {
        class AsyncJobService : IWindowsAsyncJobFactoryService
        {
            private readonly object _syncRoot = new();
            private readonly ConcurrentStack<JobFactoryServiceViewModel> _viewModels = new();
            private JobFactoryServiceViewModel _viewModel;

            public AsyncJobService()
            {
                _viewModel = (JobFactoryServiceViewModel)Application.Current.FindResource("AsyncJobFactoryService");
            }

            public IAsyncJob<TResult> StartNew<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                TArg3 arg3, [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(arg1, arg2, arg3, listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            public IAsyncJob<TResult> StartNew<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(arg1, arg2, listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            public IAsyncJob<TResult> StartNew<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
                [DisallowNull] Func<TArg, IWindowsStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(arg, listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            public IAsyncJob<TResult> StartNew<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
                [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            public IAsyncJob StartNew<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(arg1, arg2, arg3, listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            public IAsyncJob StartNew<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(arg1, arg2, listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            public IAsyncJob StartNew<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
                [DisallowNull] Func<TArg, IWindowsStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(arg, listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            public IAsyncJob StartNew([DisallowNull] string title, [DisallowNull] string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            public void SetWindowViewModel([DisallowNull] Window window, [DisallowNull] JobFactoryServiceViewModel viewModel)
            {
                if (window is null)
                    throw new ArgumentNullException(nameof(window));
                if (viewModel is null)
                    throw new ArgumentNullException(nameof(viewModel));
                window.Initialized += (object sender, EventArgs e) =>
                {
                    lock (_syncRoot)
                    {
                        _viewModels.Push(_viewModel);
                        _viewModel = viewModel;
                    }
                };
                window.Closed += (object sender, EventArgs e) =>
                {
                    lock (_syncRoot)
                    {
                        if (_viewModels.TryPop(out JobFactoryServiceViewModel vm))
                            _viewModel = vm;
                    }
                };
            }

            IAsyncJob<TResult> IAsyncJobFactoryService.StartNew<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
                TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Func<TArg1, TArg2, TArg3, IStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(arg1, arg2, arg3, listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            IAsyncJob<TResult> IAsyncJobFactoryService.StartNew<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1,
                TArg2 arg2, [DisallowNull] Func<TArg1, TArg2, IStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(arg1, arg2, listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            IAsyncJob<TResult> IAsyncJobFactoryService.StartNew<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
                [DisallowNull] Func<TArg, IStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(arg, listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            IAsyncJob<TResult> IAsyncJobFactoryService.StartNew<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
                [DisallowNull] Func<IStatusListener, Task<TResult>> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    return await method(listener);
                }, listener.CancellationToken), out IAsyncJob<TResult> job), job);
                return job;
            }

            IAsyncJob IAsyncJobFactoryService.StartNew<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                TArg3 arg3, [DisallowNull] Func<TArg1, TArg2, TArg3, IStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(arg1, arg2, arg3, listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            IAsyncJob IAsyncJobFactoryService.StartNew<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(arg1, arg2, listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            IAsyncJob IAsyncJobFactoryService.StartNew<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
                [DisallowNull] Func<TArg, IStatusListener, Task> method)
            {
                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(arg, listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            IAsyncJob IAsyncJobFactoryService.StartNew([DisallowNull] string title, [DisallowNull] string initialMessage,
                [DisallowNull] Func<IStatusListener, Task> method)
            {
                if (method is null)
                    throw new ArgumentNullException(nameof(method));

                _viewModel.AddJob(BackgroundJobVM.Create(title, initialMessage, (listener, instance) => Task.Run(async () =>
                {
                    await instance.RaiseStatusChangedAsync();
                    await method(listener);
                }, listener.CancellationToken), out IAsyncJob job), job);
                return job;
            }

            public Task<TResult> RunAsync<TArg1, TArg2, TArg3, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                TArg3 arg3, [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, arg1, arg2, arg3, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task<TResult> RunAsync<TArg1, TArg2, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, arg1, arg2, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task<TResult> RunAsync<TArg, TResult>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
                [DisallowNull] Func<TArg, IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, arg, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task<TResult> RunAsync<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
                [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete = null)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task RunAsync<TArg1, TArg2, TArg3>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                [DisallowNull] Func<TArg1, TArg2, TArg3, IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null)
            {
                IAsyncJob job = StartNew(title, initialMessage, arg1, arg2, arg3, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task RunAsync<TArg1, TArg2>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg1 arg1, TArg2 arg2,
                [DisallowNull] Func<TArg1, TArg2, IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null)
            {
                IAsyncJob job = StartNew(title, initialMessage, arg1, arg2, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task RunAsync<TArg>([DisallowNull] string title, [DisallowNull] string initialMessage, TArg arg,
                [DisallowNull] Func<TArg, IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null)
            {
                IAsyncJob job = StartNew(title, initialMessage, arg, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            public Task RunAsync([DisallowNull] string title, [DisallowNull] string initialMessage,
                [DisallowNull] Func<IWindowsStatusListener, Task> method, Action<IAsyncJob> onComplete = null)
            {
                IAsyncJob job = StartNew(title, initialMessage, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task<TResult> IAsyncJobFactoryService.RunAsync<TArg1, TArg2, TArg3, TResult>(string title, string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                Func<TArg1, TArg2, TArg3, IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, arg1, arg2, arg3, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task<TResult> IAsyncJobFactoryService.RunAsync<TArg1, TArg2, TResult>(string title, string initialMessage, TArg1 arg1, TArg2 arg2,
                Func<TArg1, TArg2, IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, arg1, arg2, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task<TResult> IAsyncJobFactoryService.RunAsync<TArg, TResult>(string title, string initialMessage, TArg arg,
                Func<TArg, IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, arg, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task<TResult> IAsyncJobFactoryService.RunAsync<TResult>(string title, string initialMessage,
                Func<IStatusListener, Task<TResult>> method, Action<IAsyncJob<TResult>> onComplete)
            {
                IAsyncJob<TResult> job = StartNew(title, initialMessage, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task IAsyncJobFactoryService.RunAsync<TArg1, TArg2, TArg3>(string title, string initialMessage, TArg1 arg1, TArg2 arg2, TArg3 arg3,
                Func<TArg1, TArg2, TArg3, IStatusListener, Task> method, Action<IAsyncJob> onComplete)
            {
                IAsyncJob job = StartNew(title, initialMessage, arg1, arg2, arg3, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task IAsyncJobFactoryService.RunAsync<TArg1, TArg2>(string title, string initialMessage, TArg1 arg1, TArg2 arg2,
                Func<TArg1, TArg2, IStatusListener, Task> method, Action<IAsyncJob> onComplete)
            {
                IAsyncJob job = StartNew(title, initialMessage, arg1, arg2, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task IAsyncJobFactoryService.RunAsync<TArg>(string title, string initialMessage, TArg arg, Func<TArg, IStatusListener, Task> method,
                Action<IAsyncJob> onComplete)
            {
                IAsyncJob job = StartNew(title, initialMessage, arg, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }

            Task IAsyncJobFactoryService.RunAsync(string title, string initialMessage, Func<IStatusListener, Task> method, Action<IAsyncJob> onComplete)
            {
                IAsyncJob job = StartNew(title, initialMessage, method);
                if (onComplete is not null)
                    _ = job.Task.ContinueWith(t => onComplete(job));
                return job.Task;
            }
        }
    }
}
