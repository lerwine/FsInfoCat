using FsInfoCat.AsyncOps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public partial class BgActivityService : BackgroundService, IBgActivityService
    {
        private readonly object _syncRoot = new();
        private readonly ILogger<BgActivityService> _logger;
        private List<IAsyncAction> _jobs = new();

        public int Count => _jobs.Count;

        public BgActivityService([DisallowNull] ILogger<BgActivityService> logger)
        {
            _logger = logger;
        }

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(BgActivityService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<BgActivityService>(serviceProvider => new BgActivityService(serviceProvider.GetRequiredService<ILogger<BgActivityService>>()));
        }

#pragma warning disable CA2016 // Forward the 'CancellationToken' parameter to methods that take one
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
#pragma warning restore CA2016 // Forward the 'CancellationToken' parameter to methods that take one
        {
            try { stoppingToken.WaitHandle.WaitOne(); }
            finally
            {
                lock (_syncRoot)
                {
                    IAsyncAction[] actions = _jobs.ToArray();
                    _jobs.Clear();
                    foreach (IAsyncAction a in actions)
                        a.Cancel();
                }
            }
        });

        public IEnumerator<IAsyncAction> GetEnumerator() => _jobs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_jobs).GetEnumerator();

        public IAsyncAction<TState> InvokeAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, IObserver<IBgStatusEventArgs<TState>> observer,
            Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            return new AsyncAction<TState>(activityCode, initialStatusMessage, state, observer, asyncMethodDelegate, this);
        }

        public IAsyncAction<TState> InvokeAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, IProgress<IBgStatusEventArgs<TState>> listener,
            Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            return new AsyncAction<TState>(activityCode, initialStatusMessage, state, listener, asyncMethodDelegate, this);
        }

        public IAsyncAction<TState> InvokeAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            return new AsyncAction<TState>(activityCode, initialStatusMessage, state, asyncMethodDelegate, this);
        }

        public ITimedAsyncAction<TState> InvokeTimedAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state,
            IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncAction<TState> InvokeTimedAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state,
            IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncAction<TState> InvokeTimedAsync<TState>(ActivityCode activityCode, string initialStatusMessage, TState state,
            Func<IBgActivityProgress<TState>, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncAction InvokeAsync(ActivityCode activityCode, string initialStatusMessage, IObserver<IBgStatusEventArgs> observer,
            Func<IBgActivityProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncAction InvokeAsync(ActivityCode activityCode, string initialStatusMessage, IProgress<IBgStatusEventArgs> listener,
            Func<IBgActivityProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncAction InvokeAsync(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncAction InvokeTimedAsync(ActivityCode activityCode, string initialStatusMessage, IObserver<ITimedBgStatusEventArgs> observer,
            Func<IBgActivityProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncAction InvokeTimedAsync(ActivityCode activityCode, string initialStatusMessage, IProgress<ITimedBgStatusEventArgs> listener,
            Func<IBgActivityProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncAction InvokeTimedAsync(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state,
            IObserver<IBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state,
            IProgress<IBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncFunc<TState, TResult> InvokeAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state,
            Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TState, TResult> InvokeTimedAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state,
            IObserver<ITimedBgStatusEventArgs<TState>> observer, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TState, TResult> InvokeTimedAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state,
            IProgress<ITimedBgStatusEventArgs<TState>> listener, Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TState, TResult> InvokeTimedAsync<TState, TResult>(ActivityCode activityCode, string initialStatusMessage, TState state,
            Func<IBgActivityProgress<TState>, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncFunc<TResult> InvokeAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IObserver<IBgStatusEventArgs> observer,
            Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncFunc<TResult> InvokeAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IProgress<IBgStatusEventArgs> listener,
            Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncFunc<TResult> InvokeAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TResult> InvokeTimedAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IObserver<ITimedBgStatusEventArgs> observer,
            Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TResult> InvokeTimedAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, IProgress<ITimedBgStatusEventArgs> listener,
            Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public ITimedAsyncFunc<TResult> InvokeTimedAsync<TResult>(ActivityCode activityCode, string initialStatusMessage, Func<IBgActivityProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }
    }
}
