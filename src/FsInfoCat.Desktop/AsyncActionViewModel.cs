using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public class AsyncActionViewModel : AsyncOperationViewModel, IAsyncAction
    {
        public Task StartAsync(TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }
    }

    public class AsyncActionViewModel<TState> : AsyncActionViewModel, IAsyncAction<TState>
    {
        public event DependencyPropertyChangedEventHandler AsyncStatePropertyChanged;

        private static readonly DependencyPropertyKey AsyncStatePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AsyncState), typeof(TState), typeof(AsyncActionViewModel<TState>),
                new PropertyMetadata(default(TState), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncActionViewModel<TState>).OnAsyncStatePropertyChanged(e)));

        public static readonly DependencyProperty AsyncStateProperty = AsyncStatePropertyKey.DependencyProperty;

        public TState AsyncState
        {
            get { return (TState)GetValue(AsyncStateProperty); }
            private set { SetValue(AsyncStatePropertyKey, value); }
        }

        protected virtual void OnAsyncStatePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnAsyncStatePropertyChanged((TState)args.OldValue, (TState)args.NewValue); }
            finally { AsyncStatePropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnAsyncStatePropertyChanged(TState oldValue, TState newValue)
        {
            // TODO: Implement OnAsyncStatePropertyChanged Logic
        }

        public Task StartAsync(TState state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public Task StartWhenAll<TAntecedentResult>(IAsyncResult<TAntecedentResult>[] inputs, Func<IAsyncResult<TAntecedentResult>[], TState> aggregateFunction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public Task StartWhenAny(IAsyncResult<TState>[] inputs, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }
    }
}
