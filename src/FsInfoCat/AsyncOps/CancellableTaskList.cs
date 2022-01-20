using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public sealed class CancellableTaskList
    {
        private readonly LinkedList<IItem> _items = new();

        public Item<TTask> FromAsync<TTask>(Func<CancellationToken, TTask> func) where TTask : Task => new(this, func);

        public Item<TTask> FromAsync<TArg, TTask>(TArg arg, Func<TArg, CancellationToken, TTask> func) where TTask : Task => Item<TTask>.FromAsync(this, arg, func);

        public Item<TTask> FromAsync<TArg1, TArg2, TTask>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, CancellationToken, TTask> func) where TTask : Task => Item<TTask>.FromAsync(this, arg1, arg2, func);

        public Item<TTask> FromAsync<TArg1, TArg2, TArg3, TTask>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, CancellationToken, TTask> func) where TTask : Task => Item<TTask>.FromAsync(this, arg1, arg2, arg3, func);

        public Item<Task<TResult>> StartNew<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(arg1, arg2, arg3, token)));

        public Item<Task> StartNew<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Action<TArg1, TArg2, TArg3, CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(arg1, arg2, arg3, token)));

        public Item<Task<TResult>> StartNew<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(arg1, arg2, token)));

        public Item<Task> StartNew<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Action<TArg1, TArg2, CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(arg1, arg2, token)));

        public Item<Task<TResult>> StartNew<TArg, TResult>(TArg arg, Func<TArg, CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(arg, token)));

        public Item<Task> StartNew<T>(T arg, Action<T, CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(arg, token)));

        public Item<Task<TResult>> StartNew<TResult>(Func<CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(token)));

        public Item<Task> StartNew(Action<CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(token)));

        public void CancelAll(bool throwOnFirstException)
        {
            for (LinkedListNode<IItem> item = _items.Last; item is not null; item = _items.Last)
                item.Value.Cancel(throwOnFirstException);
        }

        public interface IItem : IDisposable
        {
            Task Task { get; }
            void Cancel(bool throwOnFirstException);
        }

        public sealed class Item<TTask> : IItem
            where TTask : Task
        {
            private bool _disposed;
            private readonly CancellationTokenSource _tokenSource;
            private readonly CancellableTaskList _list;
            private readonly TTask _task;

            public TTask Task => _task;

            Task IItem.Task => _task;

            public void Cancel(bool throwOnFirstException)
            {
                lock (_tokenSource)
                    if (!(_disposed || _task.IsCompleted || !_tokenSource.IsCancellationRequested))
                        _tokenSource.Cancel(throwOnFirstException);
            }

            public void Dispose()
            {
                lock (_tokenSource)
                {
                    if (_disposed)
                        return;
                    _disposed = true;
                    if (_list._items.Contains(this))
                        _ = _list._items.Remove(this);
                }
                if (!_task.IsCompleted)
                {
                    if (!_tokenSource.IsCancellationRequested)
                        _tokenSource.Cancel(true);
                    Thread.Sleep(100);
                }
                _tokenSource.Dispose();
            }

            internal Item(CancellableTaskList list, Func<CancellationToken, TTask> func)
            {
                _tokenSource = new CancellationTokenSource();
                _list = list;
                _task = func(_tokenSource.Token);
                _ = _task.ContinueWith(t => Dispose());
            }

            internal static Item<TTask> FromAsync<TArg>(CancellableTaskList list, TArg arg, Func<TArg, CancellationToken, TTask> func) => new(list, t => func(arg, t));

            internal static Item<TTask> FromAsync<TArg1, TArg2>(CancellableTaskList list, TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, CancellationToken, TTask> func) => new(list, t => func(arg1, arg2, t));

            internal static Item<TTask> FromAsync<TArg1, TArg2, TArg3>(CancellableTaskList list, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, CancellationToken, TTask> func) => new(list, t => func(arg1, arg2, arg3, t));
        }
    }
}

namespace FsInfoCat.AsyncOps
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public abstract class CancellableTaskList<TTask>
        where TTask : Task
    {
        private readonly LinkedList<Item> _items = new();

        public Item FromAsync(Func<CancellationToken, TTask> func) => new(this, func);

        public Item FromAsync<TArg>(TArg arg, Func<TArg, CancellationToken, TTask> func) => Item.FromAsync(this, arg, func);

        public Item FromAsync<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, CancellationToken, TTask> func) => Item.FromAsync(this, arg1, arg2, func);

        public Item FromAsync<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, CancellationToken, TTask> func) => Item.FromAsync(this, arg1, arg2, arg3, func);

        public void CancelAll(bool throwOnFirstException)
        {
            for (LinkedListNode<Item> item = _items.Last; item is not null; item = _items.Last)
                item.Value.Cancel(throwOnFirstException);
        }

        public sealed class Item
        {
            private bool _disposed;
            private readonly CancellationTokenSource _tokenSource;
            private readonly CancellableTaskList<TTask> _list;
            private readonly TTask _task;

            public TTask Task { get; }

            public void Cancel(bool throwOnFirstException)
            {
                lock (_tokenSource)
                    if (!(_disposed || _task.IsCompleted || !_tokenSource.IsCancellationRequested))
                        _tokenSource.Cancel(throwOnFirstException);
            }

            public void Dispose()
            {
                lock (_tokenSource)
                {
                    if (_disposed)
                        return;
                    _disposed = true;
                    if (_list._items.Contains(this))
                        _ = _list._items.Remove(this);
                }
                if (!_task.IsCompleted)
                {
                    if (!_tokenSource.IsCancellationRequested)
                        _tokenSource.Cancel(true);
                    Thread.Sleep(100);
                }
                _tokenSource.Dispose();
            }

            internal Item(CancellableTaskList<TTask> list, Func<CancellationToken, TTask> func)
            {
                _tokenSource = new CancellationTokenSource();
                _list = list;
                _task = func(_tokenSource.Token);
                _ = _task.ContinueWith(t => Dispose());
            }

            internal static Item FromAsync<TArg>(CancellableTaskList<TTask> list, TArg arg, Func<TArg, CancellationToken, TTask> func) => new(list, t => func(arg, t));

            internal static Item FromAsync<TArg1, TArg2>(CancellableTaskList<TTask> list, TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, CancellationToken, TTask> func) => new(list, t => func(arg1, arg2, t));

            internal static Item FromAsync<TArg1, TArg2, TArg3>(CancellableTaskList<TTask> list, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, CancellationToken, TTask> func) => new(list, t => func(arg1, arg2, arg3, t));
        }
    }
}
