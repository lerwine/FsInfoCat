using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AsyncActionViewModel : AsyncOperationViewModel
    {
        private readonly Action<CancellationToken> _action;

        public AsyncActionViewModel(Action<CancellationToken> action, TaskScheduler scheduler) : base(scheduler)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override object GetAsyncState() => null;

        public Task EnsureOperation(out bool newOpstarted)
        {
            return EnsureOperation(_action, out newOpstarted);
        }

        public Task EnsureOperation(TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            return EnsureOperation(_action, creationOptions, out newOpstarted);
        }

        public Task StartNew()
        {
            return StartNew(_action);
        }

        public Task StartNew(TaskCreationOptions creationOptions)
        {
            return StartNew(_action, creationOptions);
        }

        public bool TryStartNew(out Task asyncOpTask)
        {
            return TryStartNew(_action, out asyncOpTask);
        }

        public bool TryStartNew(TaskCreationOptions creationOptions, out Task asyncOpTask)
        {
            return TryStartNew(_action, creationOptions, out asyncOpTask);
        }
    }

    public class AsyncActionViewModel<TState> : AsyncOperationViewModel<TState>
    {
        private readonly Action<TState, CancellationToken> _action;

        public AsyncActionViewModel(Action<TState, CancellationToken> action, TaskScheduler scheduler) : base(scheduler)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public Task EnsureOperation(TState state, out bool newOpstarted)
        {
            return EnsureOperation(_action, state, out newOpstarted);
        }

        public Task EnsureOperation(TState state, TaskCreationOptions creationOptions, out bool newOpstarted)
        {
            return EnsureOperation(_action, state, creationOptions, out newOpstarted);
        }

        public Task StartNew(TState state)
        {
            return StartNew(_action, state);
        }

        public Task StartNew(TState state, TaskCreationOptions creationOptions)
        {
            return StartNew(_action, state, creationOptions);
        }

        public bool TryStartNew(TState state, out Task asyncOpTask)
        {
            return TryStartNew(_action, state, out asyncOpTask);
        }

        public bool TryStartNew(TState state, TaskCreationOptions creationOptions, out Task asyncOpTask)
        {
            return TryStartNew(_action, state, creationOptions, out asyncOpTask);
        }
    }
}
