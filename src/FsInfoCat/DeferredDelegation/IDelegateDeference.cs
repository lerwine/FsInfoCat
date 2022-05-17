using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.DeferredDelegation
{
    /// <summary>
    /// Provides methods for deferring delegate methods along with thread-exclusive synchonization locking for a target object.
    /// <para>Implements the <see cref="IDisposable" /> interface.</para>
    /// </summary>
    /// <remarks>Each instance of this interface created using methods from the <see cref="IDeferredDelegationService"/> maintains a thread-exclusive
    /// lock on a <see cref="IDelegateQueueing.SyncRoot">synchronization object</see> /// that is intended to facilitate synchronized access to a <see cref="IDelegateQueueing.Target">target</see> object.
    /// Delegates, such as event invocations, can be deferred using this interface to mitigate the chances of other code being executed which may result in untimely modifications
    /// or deadlocks.
    /// <para>Deferred delegates are invoked when the last <c>IDelegateDeference</c> instance sharing the same <see cref="IDelegateQueueing.Target">target</see> object is disposed or
    /// when <see cref="IDelegateQueueing.DequeueDelegates()"/> is invoked.</para></remarks>
    /// <seealso cref="IDisposable" />
    public interface IDelegateDeference : IDelegateQueueing, IDisposable
    {
        // TODO: Document IDelegateDeference members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void DeferPropertyChangedEvent([DisallowNull] INotifyPropertyChanged sender, [DisallowNull] string propertyName, [DisallowNull] PropertyChangedEventHandler eventHandler,
            DeferredEventErrorHandler<PropertyChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [DisallowNull] NotifyCollectionChangedEventHandler eventHandler,
            DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] IList changedItems,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object changedItem,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [DisallowNull] IList newItems, [DisallowNull] IList oldItems,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] IList changedItems, int startingIndex,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object changedItem, int index,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object newItem, [AllowNull] object oldItem,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [DisallowNull] IList newItems, [DisallowNull] IList oldItems, int startingIndex,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] IList changedItems, int index, int oldIndex,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object changedItem, int index, int oldIndex,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, NotifyCollectionChangedAction action, [AllowNull] object newItem, [AllowNull] object oldItem, int index,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferUnhandledExceptionEvent([DisallowNull] object sender, [DisallowNull] Exception exception, [DisallowNull] UnhandledExceptionEventHandler eventHandler, bool isTerminating = false);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Provides methods for deferring delegate methods along with thread-exclusive synchonization locking for a target object.
    /// <para>Extends the <see cref="IDelegateDeference" /> interface.</para>
    /// </summary>
    /// <remarks>Each instance of this interface created using methods from the <see cref="IDeferredDelegationService"/> maintains a thread-exclusive
    /// lock on a <see cref="IDelegateQueueing.SyncRoot">synchronization object</see> that is intended to facilitate synchronized access to a <see cref="IDelegateQueueing.Target">target</see> object.
    /// Delegates, such as event invocations, can be deferred using this interface to mitigate the chances of other code being executed which may result in untimely modifications
    /// or deadlocks.
    /// <para>Deferred delegates are invoked when the last <c>IDelegateDeference</c> instance sharing the same <see cref="IDelegateQueueing.Target">target</see> object is disposed or
    /// when <see cref="IDelegateQueueing.DequeueDelegates()"/> is invoked.</para></remarks>
    /// <seealso cref="IDelegateDeference" />
    public interface IDelegateDeference<TTarget> : IDelegateQueueing<TTarget>, IDelegateDeference where TTarget : class { }

        // TODO: Document IDelegateDeference members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public delegate void DeferredDelegateErrorHandler(Exception exception, object[] args);

    public delegate void DeferredActionErrorHandler(Exception exception);

    public delegate void DeferredActionErrorHandler<TArg>(Exception exception, TArg arg);

    public delegate void DeferredActionErrorHandler<TArg1, TArg2>(Exception exception, TArg1 arg1, TArg2 arg2);

    public delegate void DeferredActionErrorHandler<TArg1, TArg2, TArg3>(Exception exception, TArg1 arg1, TArg2 arg2, TArg3 arg3);

    public delegate void DeferredEventErrorHandler<TEventArgs>(Exception exception, object sender, TEventArgs args) where TEventArgs : EventArgs;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
