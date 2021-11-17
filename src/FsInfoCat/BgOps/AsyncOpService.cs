using FsInfoCat.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    class AyncOperation : AsyncOpService.AyncOperation<IAsyncOpEventArgs>
    {

    }
    class AyncOperation<T> : AsyncOpService.AyncOperation<IAsyncOpEventArgs<T>>
    {

    }
    public partial class AsyncOpService : BackgroundService, IAsyncOpService
    {
        private readonly object _syncRoot = new();
        private readonly ILogger<AsyncOpService> _logger;

        internal class AyncOperation<T> : Observable<T>
            where T : IAsyncOpEventArgs
        {

        }
        public AsyncOpService(ILogger<AsyncOpService> logger)
        {
            _logger = logger;
        }

        int IReadOnlyCollection<IAsyncOperation>.Count => throw new NotImplementedException();

        [ServiceBuilderHandler]
        public static void ConfigureServices([DisallowNull] IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(AsyncOpService).FullName}.{nameof(ConfigureServices)}");
            services.AddHostedService<IAsyncOpService>(serviceProvider => new AsyncOpService(serviceProvider.GetRequiredService<ILogger<AsyncOpService>>()));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TState, TResult> FromAsync<TState, TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task<TResult>> asyncMethodDelegate, TState state)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncProducer<TResult> FromAsync<TResult>(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task<TResult>> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, IObserver<IAsyncOpEventArgs<TState>> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state, Func<IAsyncOpEventArgs<TState>, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<TState> FromAsync<TState>(string activity, string initialStatusMessage, Func<IAsyncOpProgress<TState>, Task> asyncMethodDelegate, TState state)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, IObserver<IAsyncOpEventArgs> observer)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate, Func<IAsyncOpEventArgs, string> getFinalStatusMessage)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation FromAsync(string activity, string initialStatusMessage, Func<IAsyncOpProgress, Task> asyncMethodDelegate)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IAsyncOperation> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
