using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public interface IAsyncFunction<TResult> : IAsyncResult<TResult>
    {
        event DependencyPropertyChangedEventHandler ResultPropertyChanged;
        Task<TResult> StartAsync(TaskCreationOptions creationOptions);
    }

    public interface IAsyncFunction<TState, TResult> : IAsyncFunction<TResult>, IAsyncResult<TState, TResult>
    {
        Task<TResult> StartAsync(TState state, TaskCreationOptions creationOptions);
        Task<TResult> StartWhenAll<TAntecedentResult>(IAsyncResult<TAntecedentResult>[] inputs, Func<IAsyncResult<TAntecedentResult>[], TState> aggregateFunction,
            TaskContinuationOptions continuationOptions);

        Task<TResult> StartWhenAny(IAsyncResult<TState>[] inputs, TaskContinuationOptions continuationOptions);
    }
}
