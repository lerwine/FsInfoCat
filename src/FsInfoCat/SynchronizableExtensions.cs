using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat;

/// <summary>
/// Extension methods for types that are <see cref="ISynchronizable"/>.
/// </summary>
public static class SynchronizableExtensions
{
    /// <summary>
    /// Asynchronously gets a value produced by a delegate with synchronized access to an object.
    /// </summary>
    /// <typeparam name="TSynchronizable">The type of <see cref="ISynchronizable"/> object that access is being synchronized on.</typeparam>
    /// <typeparam name="TResult">The type of value returned by the delegate.</typeparam>
    /// <param name="synchronizable">The object to synchronize access to.</param>
    /// <param name="func">The delegate to invoke.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the value produced by the delegate.</returns>
    public static async Task<TResult> SyncDeriveAsync<TSynchronizable, TResult>(this TSynchronizable synchronizable, [DisallowNull] Func<TSynchronizable, Task<TResult>> func)
        where TSynchronizable : ISynchronizable
    {
        ArgumentNullException.ThrowIfNull(synchronizable);
        ArgumentNullException.ThrowIfNull(func);
        Monitor.Enter(synchronizable.SyncRoot);
        try { return await func(synchronizable); }
        finally { Monitor.Exit(synchronizable.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to an object.
    /// </summary>
    /// <typeparam name="TSynchronizable">The type of <see cref="ISynchronizable"/> object that access is being synchronized on.</typeparam>
    /// <param name="synchronizable">The object to synchronize access to.</param>
    /// <param name="action">The delegate to invoke.</param>
    /// <returns>A <see cref="Task"/> for the asynchronous invocation of the delegate.</returns>
    public static async Task SyncInvokeAsync<TSynchronizable>(this TSynchronizable synchronizable, [DisallowNull] Func<TSynchronizable, Task> action)
        where TSynchronizable : ISynchronizable
    {
        ArgumentNullException.ThrowIfNull(synchronizable);
        ArgumentNullException.ThrowIfNull(action);
        Monitor.Enter(synchronizable.SyncRoot);
        try { await action(synchronizable); }
        finally { Monitor.Exit(synchronizable.SyncRoot); }
    }

    /// <summary>
    /// Gets a value produced by a delegate with synchronized access to an object.
    /// </summary>
    /// <typeparam name="TSynchronizable">The type of <see cref="ISynchronizable"/> object that access is being synchronized on.</typeparam>
    /// <typeparam name="TResult">The type of value returned by the delegate.</typeparam>
    /// <param name="synchronizable">The object to synchronize access to.</param>
    /// <param name="func">The delegate to invoke.</param>
    /// <returns>The value that was produced by the delegate.</returns>
    public static TResult SyncDerive<TSynchronizable, TResult>(this TSynchronizable synchronizable, [DisallowNull] Func<TSynchronizable, TResult> func)
        where TSynchronizable : ISynchronizable
    {
        ArgumentNullException.ThrowIfNull(synchronizable);
        ArgumentNullException.ThrowIfNull(func);
        Monitor.Enter(synchronizable.SyncRoot);
        try { return func(synchronizable); }
        finally { Monitor.Exit(synchronizable.SyncRoot); }
    }

    /// <summary>
    /// Gets a value produced by a delegate with synchronized access to an object.
    /// </summary>
    /// <typeparam name="TResult">The type of value returned by the delegate.</typeparam>
    /// <param name="synchronizable">The object to synchronize access to.</param>
    /// <param name="func">The delegate to invoke.</param>
    /// <returns>The value that was produced by the delegate.</returns>
    public static TResult SyncDerive<TResult>(this ISynchronizable synchronizable, [DisallowNull] Func<TResult> func)
    {
        ArgumentNullException.ThrowIfNull(synchronizable);
        ArgumentNullException.ThrowIfNull(func);
        Monitor.Enter(synchronizable.SyncRoot);
        try { return func(); }
        finally { Monitor.Exit(synchronizable.SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to an object.
    /// </summary>
    /// <typeparam name="TSynchronizable">The type of <see cref="ISynchronizable"/> object that access is being synchronized on.</typeparam>
    /// <param name="synchronizable">The object to synchronize access to.</param>
    /// <param name="action">The delegate to invoke.</param>
    public static void SyncInvoke<TSynchronizable>(this TSynchronizable synchronizable, [DisallowNull] Action<TSynchronizable> action)
        where TSynchronizable : ISynchronizable
    {
        ArgumentNullException.ThrowIfNull(synchronizable);
        ArgumentNullException.ThrowIfNull(action);
        Monitor.Enter(synchronizable.SyncRoot);
        try { action(synchronizable); }
        finally { Monitor.Exit(synchronizable.SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to an object.
    /// </summary>
    /// <param name="synchronizable">The object to synchronize access to.</param>
    /// <param name="action">The delegate to invoke.</param>
    public static void SyncInvoke(this ISynchronizable synchronizable, [DisallowNull] Action action)
    {
        ArgumentNullException.ThrowIfNull(synchronizable);
        ArgumentNullException.ThrowIfNull(action);
        Monitor.Enter(synchronizable.SyncRoot);
        try { action(); }
        finally { Monitor.Exit(synchronizable.SyncRoot); }
    }
}
