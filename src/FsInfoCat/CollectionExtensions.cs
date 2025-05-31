using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat;

/// <summary>
/// Extension methods for collection objects.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Asynchronously gets a value produced by a delegate with synchronized access to a collection.
    /// </summary>
    /// <typeparam name="TCollection">The type of <see cref="System.Collections.ICollection"/> object that access is being synchronized on.</typeparam>
    /// <typeparam name="TResult">The type of value returned by the delegate.</typeparam>
    /// <param name="collection">The collection to synchronize access to.</param>
    /// <param name="func">The delegate to invoke.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the value produced by the delegate.</returns>
    public static async Task<TResult> SyncDeriveAsync<TCollection, TResult>(this TCollection collection, [DisallowNull] Func<TCollection, Task<TResult>> func)
        where TCollection : System.Collections.ICollection
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(func);
        object syncRoot = collection.IsSynchronized ? (collection.SyncRoot ?? collection) : collection;
        Monitor.Enter(syncRoot);
        try { return await func(collection); }
        finally { Monitor.Exit(syncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to a collection.
    /// </summary>
    /// <typeparam name="TCollection">The type of <see cref="System.Collections.ICollection"/> object that access is being synchronized on.</typeparam>
    /// <param name="collection">The collection to synchronize access to.</param>
    /// <param name="action">The delegate to invoke.</param>
    /// <returns>A <see cref="Task"/> for teh asynchronous invocation of the delegate.</returns>
    public static async Task SyncInvokeAsync<TCollection>(this TCollection collection, [DisallowNull] Func<TCollection, Task> action)
        where TCollection : System.Collections.ICollection
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);
        object syncRoot = collection.IsSynchronized ? (collection.SyncRoot ?? collection) : collection;
        Monitor.Enter(syncRoot);
        try { await action(collection); }
        finally { Monitor.Exit(syncRoot); }
    }

    /// <summary>
    /// Gets a value produced by a delegate with synchronized access to a collection.
    /// </summary>
    /// <typeparam name="TCollection">The type of <see cref="System.Collections.ICollection"/> object that access is being synchronized on.</typeparam>
    /// <typeparam name="TResult">The type of value returned by the delegate.</typeparam>
    /// <param name="collection">The collection to synchronize access to.</param>
    /// <param name="func">The delegate to invoke.</param>
    /// <returns>The value produced by the delegate.</returns>
    public static TResult SyncDerive<TCollection, TResult>(this TCollection collection, [DisallowNull] Func<TCollection, TResult> func)
        where TCollection : System.Collections.ICollection
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(func);
        object syncRoot = collection.IsSynchronized ? (collection.SyncRoot ?? collection) : collection;
        Monitor.Enter(syncRoot);
        try { return func(collection); }
        finally { Monitor.Exit(syncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to a collection.
    /// </summary>
    /// <typeparam name="TCollection">The type of <see cref="System.Collections.ICollection"/> object that access is being synchronized on.</typeparam>
    /// <param name="collection">The collection to synchronize access to.</param>
    /// <param name="action">The delegate to invoke.</param>
    public static void SyncInvoke<TCollection>(this TCollection collection, [DisallowNull] Action<TCollection> action)
        where TCollection : System.Collections.ICollection
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);
        object syncRoot = collection.IsSynchronized ? (collection.SyncRoot ?? collection) : collection;
        Monitor.Enter(syncRoot);
        try { action(collection); }
        finally { Monitor.Exit(syncRoot); }
    }
}
