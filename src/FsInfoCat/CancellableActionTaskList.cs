using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public sealed class CancellableActionTaskList : CancellableTaskList<Task>
    {
        public Item StartNew<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Action<TArg1, TArg2, TArg3, CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(arg1, arg2, arg3, token)));

        public Item StartNew<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Action<TArg1, TArg2, CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(arg1, arg2, token)));

        public Item StartNew<T>(T arg, Action<T, CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(arg, token)));

        public Item StartNew(Action<CancellationToken> action) => new(this, token => Task.Factory.StartNew(() => action(token)));
    }
}
