using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public sealed class CancellableFunctionTaskList<TResult> : CancellableTaskList<Task<TResult>>
    {
        public Item StartNew<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(arg1, arg2, arg3, token)));

        public Item StartNew<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(arg1, arg2, token)));

        public Item StartNew<TArg>(TArg arg, Func<TArg, CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(arg, token)));

        public Item StartNew(Func<CancellationToken, TResult> func) => new(this, token => Task.Factory.StartNew(() => func(token)));
    }
}
