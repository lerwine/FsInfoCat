using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public interface IDelegateDeference : IDisposable
    {
        object SyncRoot { get; }

        object Target { get; }

        int EventQueueCount { get; }

        void ReleaseEvents();

        void AddDelegate([DisallowNull] Delegate @delegate, params object[] args);

        void AddAction([DisallowNull] Action action);

        void AddAction<TArg>(TArg arg, [DisallowNull] Action<TArg> action);

        void AddAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, [DisallowNull] Action<TArg1, TArg2> action);

        void AddAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Action<TArg1, TArg2, TArg3> action);
    }
    public interface IDelegateDeference<TTarget> : IDelegateDeference
        where TTarget : class
    {
        new TTarget Target { get; }
    }
}
