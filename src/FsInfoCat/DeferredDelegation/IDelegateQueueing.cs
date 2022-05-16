using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.DeferredDelegation
{
    public interface IDelegateQueueing
    {
        /// <summary>
        /// Gets synchronization object.
        /// </summary>
        /// <value>The object that is locked for exclusive access by a specific thread.</value>
        object SyncRoot { get; }

        /// <summary>
        /// Gets the object that is the target of the synchronized access.
        /// </summary>
        /// <value>The object that is the target of the synchronized access.</value>
        object Target { get; }

        /// <summary>
        /// Gets the number of <see cref="Delegate">delegates</see> that are enqueued.
        /// </summary>
        /// <value>The the number of <see cref="Delegate">delegates</see> that are enqueued to be invoked, in the sequence in which they were added,
        /// after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity dequeued/invoked
        /// using <see cref="DequeueDelegates"/>.</value>
        int DelegateQueueCount { get; }

        /// <summary>
        /// Dequeues and invokes the enqueued delegates in the order inwhich they were added.
        /// </summary>
        void DequeueDelegates();

        /// <summary>
        /// Defers a delegated invocation.
        /// </summary>
        /// <param name="delegate">The <see cref="Delegate">delegate</see> to be enqueued for deferred invocation.</param>
        /// <param name="args">The arguments to pass to the delegate upon invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferDelegate([DisallowNull] Delegate @delegate, params object[] args);

        /// <summary>
        /// Defers a delegated invocation with an uncaught exception handler.
        /// </summary>
        /// <param name="delegate">The <see cref="Delegate">delegate</see> to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <param name="args">The arguments to pass to the delegate upon invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferDelegateWithErrorHandler([DisallowNull] Delegate @delegate, [DisallowNull] DeferredDelegateErrorHandler onError, params object[] args);

        /// <summary>
        /// Defers an action invocation.
        /// </summary>
        /// <param name="action">The delegate <see cref="<see cref="Action"/>"/> to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferAction([DisallowNull] Action action, DeferredActionErrorHandler onError = null);

        /// <summary>
        /// Defers an action invocation.
        /// </summary>
        /// <typeparam name="TArg">The type of the argument that is passed to the <see cref="Action{TArg}"/> upon invocation.</typeparam>
        /// <param name="arg">The argument to be passed to the <see cref="Action{TArg}"/> upon invocation.</param>
        /// <param name="action">The delegate <see cref="<see cref="Action{TArg}"/>"/> to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferAction<TArg>(TArg arg, [DisallowNull] Action<TArg> action, DeferredActionErrorHandler<TArg> onError = null);

        /// <summary>
        /// Defers an action invocation.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument that is passed to the <see cref="Action{TArg1, TArg2}"/> upon invocation.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the <see cref="Action{TArg1, TArg2}"/> upon invocation.</typeparam>
        /// <param name="arg1">The first argument to be passed to the <see cref="Action{TArg1, TArg2}"/> upon invocation.</param>
        /// <param name="arg2">The second argument to be passed to the <see cref="Action{TArg1, TArg2}"/> upon invocation.</param>
        /// <param name="action">The delegate <see cref="<see cref="Action{TArg1, TArg2}"/>"/> to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, [DisallowNull] Action<TArg1, TArg2> action, DeferredActionErrorHandler<TArg1, TArg2> onError = null);

        /// <summary>
        /// Defers an action invocation.
        /// </summary>
        /// <typeparam name="TArg1">The type of the first argument that is passed to the <see cref="Action{TArg1, TArg2, TArg3}"/> upon invocation.</typeparam>
        /// <typeparam name="TArg2">The type of the second argument that is passed to the <see cref="Action{TArg1, TArg2, TArg3}"/> upon invocation.</typeparam>
        /// <typeparam name="TArg3">The type of the third argument that is passed to the <see cref="Action{TArg1, TArg2, TArg3}"/> upon invocation.</typeparam>
        /// <param name="arg1">The first argument to be passed to the <see cref="Action{TArg1, TArg2, TArg3}"/> upon invocation.</param>
        /// <param name="arg2">The second argument to be passed to the <see cref="Action{TArg1, TArg2, TArg3}"/> upon invocation.</param>
        /// <param name="arg3">The third argument to be passed to the <see cref="Action{TArg1, TArg2, TArg3}"/> upon invocation.</param>
        /// <param name="action">The delegate <see cref="<see cref="Action{TArg1, TArg2, TArg3}"/>"/> to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] Action<TArg1, TArg2, TArg3> action, DeferredActionErrorHandler<TArg1, TArg2, TArg3> onError = null);

        void DeferPropertyChangedEvent([DisallowNull] INotifyPropertyChanged sender, [DisallowNull] PropertyChangedEventArgs eventArgs, [DisallowNull] PropertyChangedEventHandler eventHandler,
            DeferredEventErrorHandler<PropertyChangedEventArgs> onError = null);

        void DeferCollectionChangedEvent([DisallowNull] INotifyCollectionChanged sender, [DisallowNull] NotifyCollectionChangedEventArgs eventArgs,
            [DisallowNull] NotifyCollectionChangedEventHandler eventHandler, DeferredEventErrorHandler<NotifyCollectionChangedEventArgs> onError = null);

        void DeferUnhandledExceptionEvent([DisallowNull] object sender, [DisallowNull] UnhandledExceptionEventArgs eventArgs, [DisallowNull] UnhandledExceptionEventHandler eventHandler);

        /// <summary>
        /// Defers an event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data argument to be passed to the event handler.</typeparam>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="TEventArgs"/> instance containing the event data.</param>
        /// <param name="eventHandler">The event handler to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferEvent<TEventArgs>([DisallowNull] object sender, [DisallowNull] TEventArgs eventArgs, [DisallowNull] EventHandler<TEventArgs> eventHandler,
            DeferredEventErrorHandler<TEventArgs> onError = null) where TEventArgs : EventArgs;

        /// <summary>
        /// Defers an event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <param name="eventHandler">The event handler to be enqueued for deferred invocation.</param>
        /// <param name="onError">The optional callback to invoke if there is an unhandled exeption during delegate invocation.</param>
        /// <remarks>This will be invoked after the last <see cref="IDelegateDeference"/> sharing the same <see cref="Target"/> object is disposed or until explicity
        /// dequeued/invoked using <see cref="DequeueDelegates"/>.</remarks>
        void DeferEvent([DisallowNull] object sender, [DisallowNull] EventArgs eventArgs, [DisallowNull] EventHandler eventHandler, DeferredEventErrorHandler<EventArgs> onError = null);

        void DeferEvent([DisallowNull] object sender, [DisallowNull] EventHandler eventHandler, DeferredEventErrorHandler<EventArgs> onError = null);
    }
    public interface IDelegateQueueing<TTarget> : IDelegateQueueing
        where TTarget : class
    {
        /// <summary>
        /// Gets the object that is the target of the synchronized access.
        /// </summary>
        /// <value>The object that is the target of the synchronized access.</value>
        new TTarget Target { get; }
    }
}
