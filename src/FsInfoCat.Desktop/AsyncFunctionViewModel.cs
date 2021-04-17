using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public class AsyncFunctionViewModel<TResult> : AsyncOperationViewModel, IAsyncFunction<TResult>
    {

        public event DependencyPropertyChangedEventHandler ResultPropertyChanged;

        private static readonly DependencyPropertyKey ResultPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Result), typeof(TResult), typeof(AsyncFunctionViewModel<TResult>),
            new PropertyMetadata(default(TResult), (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AsyncFunctionViewModel<TResult>).OnResultPropertyChanged(e)));

        public static readonly DependencyProperty ResultProperty = ResultPropertyKey.DependencyProperty;
        private readonly Func<CancellationToken, TResult> _function;

        public TResult Result
        {
            get { return (TResult)GetValue(ResultProperty); }
            private set { SetValue(ResultPropertyKey, value); }
        }

        protected virtual void OnResultPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnResultPropertyChanged((TResult)args.OldValue, (TResult)args.NewValue); }
            finally { ResultPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnResultPropertyChanged(TResult oldValue, TResult newValue)
        {
            // TODO: Implement OnResultPropertyChanged Logic
        }

        public AsyncFunctionViewModel(Func<CancellationToken, TResult> function)
        {
            _function = function;
        }

        public Task<TResult> StartAsync(TaskCreationOptions creationOptions)
        {
            Task<TResult> result = StartOperationAsync(c => new Task<TResult>(() => _function(c), c, creationOptions), (t, c) => t.ContinueWith(f => _function(c), c, ToContinuationOptions(creationOptions), Task.Factory.Scheduler));
            result.ContinueWith(r =>
            {
                Result = r.Result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            return result;
        }
    }

    public class AsyncFunctionViewModel<TState, TResult> : AsyncFunctionViewModel<TResult>, IAsyncFunction<TState, TResult>
    {
        public event DependencyPropertyChangedEventHandler AsyncStatePropertyChanged;

        private static readonly DependencyPropertyKey AsyncStatePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AsyncState), typeof(TState), typeof(AsyncFunctionViewModel<TState, TResult>),
                new PropertyMetadata(default(TState), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncFunctionViewModel<TState, TResult>).OnAsyncStatePropertyChanged(e)));

        public static readonly DependencyProperty AsyncStateProperty = AsyncStatePropertyKey.DependencyProperty;

        public AsyncFunctionViewModel(Func<CancellationToken, TResult> function) : base(function)
        {
        }

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

        public Task<TResult> StartAsync(TState state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> StartWhenAll<TAntecedentResult>(IAsyncResult<TAntecedentResult>[] inputs, Func<IAsyncResult<TAntecedentResult>[], TState> aggregateFunction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> StartWhenAny(IAsyncResult<TState>[] inputs, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }
    }
}
