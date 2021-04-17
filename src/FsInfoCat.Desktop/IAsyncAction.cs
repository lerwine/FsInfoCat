using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public interface IAsyncAction : IAsyncOperation
    {
        Task StartAsync(TaskCreationOptions creationOptions);
    }

    public interface IAsyncAction<TState> : IAsyncAction, IAsyncOperation<TState>
    {
        event DependencyPropertyChangedEventHandler AsyncStatePropertyChanged;
        Task StartAsync(TState state, TaskCreationOptions creationOptions);
        Task StartWhenAll<TAntecedentResult>(IAsyncResult<TAntecedentResult>[] inputs, Func<IAsyncResult<TAntecedentResult>[], TState> aggregateFunction,
            TaskContinuationOptions continuationOptions);

        Task StartWhenAny(IAsyncResult<TState>[] inputs, TaskContinuationOptions continuationOptions);
    }
}
