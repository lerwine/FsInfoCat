using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AsyncFunctionViewModel<TResult> : AsyncOperationViewModel
    {
        private readonly Func<CancellationToken, TResult> _function;

        public event DependencyPropertyChangedEventHandler ResultPropertyChanged;

        private static readonly DependencyPropertyKey ResultPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Result), typeof(TResult), typeof(AsyncFunctionViewModel<TResult>),
                new PropertyMetadata(default(TResult), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncFunctionViewModel<TResult>).OnResultPropertyChanged(e)));

        public static readonly DependencyProperty ResultProperty = ResultPropertyKey.DependencyProperty;

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

        public AsyncFunctionViewModel(Func<CancellationToken, TResult> function, TaskScheduler scheduler) : base(scheduler)
        {
            _function = function ?? throw new ArgumentNullException(nameof(function));
        }

        protected override void OnTaskRanToCompletion<T>(T result)
        {
            Result = (TResult)(object)result;
            base.OnTaskRanToCompletion(result);
        }

        protected override object GetAsyncState() => null;

        public Task<TResult> EnsureOperation(out bool newOpstarted)
        {
            return EnsureOperation(_function, out newOpstarted);
        }

        public Task<TResult> EnsureOperation(TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            return EnsureOperation(_function, creationOptions, out newOpstarted);
        }

        public Task<TResult> StartNew()
        {
            return StartNew(_function);
        }

        public Task<TResult> StartNew(TaskCreationOptions creationOptions)
        {
            return StartNew(_function, creationOptions);
        }

        public bool TryStartNew(out Task<TResult> asyncOpTask)
        {
            return TryStartNew(_function, out asyncOpTask);
        }

        public bool TryStartNew(TaskCreationOptions creationOptions, out Task<TResult> asyncOpTask)
        {
            return TryStartNew(_function, creationOptions, out asyncOpTask);
        }
    }

    public class AsyncFunctionViewModel<TState, TResult> : AsyncOperationViewModel<TState>
    {
        private readonly Func<TState, CancellationToken, TResult> _function;

        public event DependencyPropertyChangedEventHandler ResultPropertyChanged;

        private static readonly DependencyPropertyKey ResultPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Result), typeof(TResult),
            typeof(AsyncFunctionViewModel<TState, TResult>), new PropertyMetadata(default(TResult), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AsyncFunctionViewModel<TState, TResult>).OnResultPropertyChanged(e)));

        public static readonly DependencyProperty ResultProperty = ResultPropertyKey.DependencyProperty;

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

        public AsyncFunctionViewModel(Func<TState, CancellationToken, TResult> function, TaskScheduler scheduler) : base(scheduler)
        {
            _function = function ?? throw new ArgumentNullException(nameof(function));
        }

        protected override void OnTaskRanToCompletion<T>(T result)
        {
            Result = (TResult)(object)result;
            base.OnTaskRanToCompletion(result);
        }

        public Task<TResult> EnsureOperation(TState state, out bool newOpstarted)
        {
            return EnsureOperation(_function, state, out newOpstarted);
        }

        public Task<TResult> EnsureOperation(TState state, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            return EnsureOperation(_function, state, creationOptions, out newOpstarted);
        }

        public Task<TResult> StartNew(TState state)
        {
            return StartNew(_function, state);
        }

        public Task<TResult> StartNew(TState state, TaskCreationOptions creationOptions)
        {
            return StartNew(_function, state, creationOptions);
        }

        public bool TryStartNew(TState state, out Task<TResult> asyncOpTask)
        {
            return TryStartNew(_function, state, out asyncOpTask);
        }

        public bool TryStartNew(TState state, TaskCreationOptions creationOptions, out Task<TResult> asyncOpTask)
        {
            return TryStartNew(_function, state, creationOptions, out asyncOpTask);
        }
    }
}
