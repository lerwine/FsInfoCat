using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.DeferredDelegation
{
    partial class DeferredDelegationService : IDeferredDelegationService
    {
        private readonly LinkedList<IDeferenceCollection> _deferenceCollections = new();
        private readonly ILogger<DeferredDelegationService> _logger;

        public DeferredDelegationService(ILogger<DeferredDelegationService> logger)
        {
            _logger = logger;
            _logger.LogDebug($"{nameof(IDeferredDelegationService)} Service instantiated");
        }

        [ServiceBuilderHandler]
        private static void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(DeferredDelegationService).FullName}.{nameof(ConfigureServices)}");
            _ = services.AddSingleton<IDeferredDelegationService, DeferredDelegationService>();
        }

        /// <summary>
        /// Tries the get value <see cref="ICollection.SyncRoot"/> property.
        /// </summary>
        /// <param name="collection">The target collection.</param>
        /// <param name="syncRoot">The value if the value <see cref="ICollection.SyncRoot"/> property or <see langword="null"/> if the collection is not synchronzied.</param>
        /// <returns><see langword="true"/> if <see cref="ICollection.IsSynchronized"/> is <see langword="true"/>;
        /// otherwise, <see langword="false"/> if <paramref name="collection"/> is null, <see cref="ICollection.IsSynchronized"/> is <see langword="null"/>,
        /// or <see cref="ICollection.SyncRoot"/> is null or could not be accessed.</returns>
        public static bool TryGetSyncRoot([AllowNull] ICollection collection, out object syncRoot)
        {
            if (collection is not null && collection.IsSynchronized)
            {
                try
                {
                    if ((syncRoot = collection.SyncRoot) is not null)
                        return true;
                }
                catch { /* Makes no difference in this context if exception was thrown */ }
            }
            syncRoot = null;
            return false;
        }

        /// <summary>
        /// Tries the get value <see cref="ICollection.SyncRoot"/> property.
        /// </summary>
        /// <param name="target">The target collection.</param>
        /// <param name="syncRoot">The value if the value <see cref="ICollection.SyncRoot"/> property or <see langword="null"/> if the collection is not synchronzied.</param>
        /// <returns><see langword="true"/> if <see cref="ICollection.IsSynchronized"/> is <see langword="true"/>;
        /// otherwise, <see langword="false"/> if <paramref name="target"/> is null, <see cref="ICollection.IsSynchronized"/> is <see langword="null"/>,
        /// or <see cref="ICollection.SyncRoot"/> is null or could not be accessed.</returns>
        public static bool TryGetSyncRoot([AllowNull] ISynchronizable target, out object syncRoot)
        {
            if (target is not null)
            {
                try
                {
                    if ((syncRoot = target.SyncRoot) is not null)
                        return true;
                }
                catch { /* Makes no difference in this context if exception was thrown */ }
            }
            syncRoot = null;
            return false;
        }

        bool TryFind<T>(T instance, out DelegateDeference<T>.DeferenceCollection collection)
            where T : class
        {
            Monitor.Enter(_deferenceCollections);
            try
            {
                LinkedListNode<IDeferenceCollection> node = _deferenceCollections.First;
                while (node is not null)
                {
                    if (node.Value is DelegateDeference<T>.DeferenceCollection deferenceCollection && ReferenceEquals(deferenceCollection.Target, instance))
                    {
                        if (deferenceCollection.Verify())
                        {
                            collection = deferenceCollection;
                            return true;
                        }
                        break;
                    }
                    node = node.Next;
                }
            }
            finally { Monitor.Exit(_deferenceCollections); }
            collection = null;
            return false;
        }

        /// <summary>
        /// Determines whether delegate deference is active for the specified target.
        /// </summary>
        /// <typeparam name="T">The type of target object for which events may be deferred.</typeparam>
        /// <param name="target">The object for which events may be deferred.</param>
        /// <returns><see langword="true" /> if event deference is active for the <paramref name="target" /> object; otherwise, <see langword="false" />.</returns>
        public bool IsDeferred<T>(T target) where T : class => TryFind(target, out _);

        /// <summary>
        /// Creates a delegate deference object for specified target object.
        /// </summary>
        /// <typeparam name="T">The type of object on which events can be deferred.</typeparam>
        /// <param name="target">The object on which events can be deferred.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>
        public IDelegateDeference<T> Enter<T>([DisallowNull] T target) where T : class
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(Enter), target))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.Enter(collection);
                    return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, new());
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified target object.
        /// </summary>
        /// <typeparam name="T">The type of object on which events can be deferred.</typeparam>
        /// <param name="target">The object on which events can be deferred.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> is <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public IDelegateDeference<T> Enter<T>([DisallowNull] T target, ref bool lockTaken) where T : class
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(Enter), target, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.Enter(collection, ref lockTaken);
                    return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, new(), ref lockTaken);
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified synchronized collection, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>
        public IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target})", nameof(EnterSynchronized), target))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.Enter(collection);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified collection if it is synchronized, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object)"/>
        public bool EnterIfSynchronized<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target})", nameof(EnterIfSynchronized), target))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.Enter(collection);
                            return true;
                        }
                        if (TryGetSyncRoot(target, out object syncRoot))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot);
                            return true;
                        }
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified synchronized collection, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(EnterSynchronized), target, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.Enter(collection, ref lockTaken);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot, ref lockTaken);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified collection if it is synchronized, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public bool EnterIfSynchronized<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(EnterIfSynchronized), target, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.Enter(collection, ref lockTaken);
                            return true;
                        }
                        if (TryGetSyncRoot(target, out object syncRoot))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot, ref lockTaken);
                            return true;
                        }
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <see cref="Monitor.TryEnter(object)"/>
        public bool TryEnterSynchronized<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target})", nameof(TryEnterSynchronized), target))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <see cref="Monitor.TryEnter(object)"/>
        public bool TryEnterIfSynchronized<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target})", nameof(TryEnterIfSynchronized), target))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public bool TryEnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(TryEnterSynchronized), target, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, ref lockTaken, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, ref lockTaken, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public bool TryEnterIfSynchronized<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(TryEnterIfSynchronized), target, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, ref lockTaken, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, ref lockTaken, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, int)"/>
        public bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout})", nameof(TryEnterSynchronized), target, millisecondsTimeout))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, int)"/>
        public bool TryEnterIfSynchronized<T>([AllowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout})", nameof(TryEnterIfSynchronized), target, millisecondsTimeout))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, int, ref bool)"/>
        public bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout}, {lockTaken})", nameof(TryEnterSynchronized), target, millisecondsTimeout, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, ref lockTaken, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, ref lockTaken, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, int, ref bool)"/>
        public bool TryEnterIfSynchronized<T>([AllowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout}, {lockTaken})", nameof(TryEnterIfSynchronized), target, millisecondsTimeout, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, ref lockTaken, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, ref lockTaken, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan)"/>
        public bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {timeout})", nameof(TryEnterSynchronized), target, timeout))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.  This will be <see langword="null"/> if the taret collection
        /// is <see langword="null"/> or <see cref="ICollection.SyncRoot"/> is null or could not be accessed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan)"/>
        public bool TryEnterIfSynchronized<T>([AllowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target}, {timeout})", nameof(TryEnterIfSynchronized), target, timeout))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan, ref bool))"/>
        public bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {timeout}, {lockTaken})", nameof(TryEnterSynchronized), target, timeout, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, ref lockTaken, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, ref lockTaken, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronized collection.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.  This will be <see langword="null"/> if the taret collection is <see langword="null"/>
        /// or <see cref="ICollection.SyncRoot"/> is null or could not be accessed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan, ref bool))"/>
        public bool TryEnterIfSynchronized<T>([AllowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            using (_logger.BeginScope("{MethodName}({target}, {timeout}, {lockTaken})", nameof(TryEnterIfSynchronized), target, timeout, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, ref lockTaken, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, ref lockTaken, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object, creating a thread-exclusive lock on
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property of <paramref name="target"/> is <see langword="null"/> or could not
        /// be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>
        public IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target})", nameof(EnterSynchronizable), target))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.Enter(collection);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object if it is not <see langword="null"/> and
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see> is not <see langword="null"/>,
        /// creating a thread-exclusive lock on its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the taret object
        /// is <see langword="null"/> or <see cref="ISynchronizable.SyncRoot"/> is null or could not be accessed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object)"/>
        public bool EnterSynchronizableIfNotNull<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target})", nameof(EnterSynchronizableIfNotNull), target))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.Enter(collection);
                            return true;
                        }
                        if (TryGetSyncRoot(target, out object syncRoot))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot);
                            return true;
                        }
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object, creating a thread-exclusive lock on
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property of <paramref name="target"/> is <see langword="null"/> or could not
        /// be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(EnterSynchronizable), target, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.Enter(collection, ref lockTaken);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot, ref lockTaken);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object if it is not <see langword="null"/> and
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see> is not <see langword="null"/>,
        /// creating a thread-exclusive lock on its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the taret object is <see langword="null"/>
        /// or <see cref="ISynchronizable.SyncRoot"/> is null or could not be accessed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        public bool EnterSynchronizableIfNotNull<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(EnterSynchronizableIfNotNull), target, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.Enter(collection, ref lockTaken);
                            return true;
                        }
                        if (TryGetSyncRoot(target, out object syncRoot))
                        {
                            deference = DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot, ref lockTaken);
                            return true;
                        }
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property on the <paramref name="target"/> object is <see langword="null"/>
        /// or could not be accessed.</exception>
        /// <see cref="Monitor.TryEnter(object)"/>
        public bool TryEnterSynchronizable<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target})", nameof(TryEnterSynchronizable), target))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <see cref="Monitor.TryEnter(object)"/>
        public bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target})", nameof(TryEnterSynchronizableIfNotNull), target))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property on the <paramref name="target"/> object is <see langword="null"/>
        /// or could not be accessed.</exception>
        /// <see cref="Monitor.TryEnter(object, ref bool)"/>
        public bool TryEnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(TryEnterSynchronizable), target, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, ref lockTaken, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, ref lockTaken, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <see cref="Monitor.TryEnter(object, ref bool)"/>
        public bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target}, {lockTaken})", nameof(TryEnterSynchronizableIfNotNull), target, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, ref lockTaken, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, ref lockTaken, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, int)"/>
        public bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout})", nameof(TryEnterSynchronizable), target, millisecondsTimeout))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, int)"/>
        public bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout})", nameof(TryEnterSynchronizableIfNotNull), target, millisecondsTimeout))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, int, ref bool)"/>
        public bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout}, {lockTaken})", nameof(TryEnterSynchronizable), target, millisecondsTimeout, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, ref lockTaken, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, ref lockTaken, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, int, ref bool)"/>
        public bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target}, {millisecondsTimeout}, {lockTaken})", nameof(TryEnterSynchronizableIfNotNull), target, millisecondsTimeout, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, millisecondsTimeout, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/> or is greater
        /// than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan)"/>
        public bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {timeout})", nameof(TryEnterSynchronizable), target, timeout))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan)"/>
        public bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target}, {timeout})", nameof(TryEnterSynchronizableIfNotNull), target, timeout))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by any of the following reasons:
        /// <list type="bullet">
        /// <item><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</item>
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/> or is greater
        /// than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        public bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            using (_logger.BeginScope("{MethodName}({target}, {timeout}, {lockTaken})", nameof(TryEnterSynchronizable), target, timeout, lockTaken))
            {
                Monitor.Enter(_deferenceCollections);
                try
                {
                    if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                        return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, ref lockTaken, out deference);
                    if (TryGetSyncRoot(target, out object syncRoot))
                        return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, ref lockTaken, out deference);
                    throw new ArgumentOutOfRangeException(nameof(target));
                }
                finally { Monitor.Exit(_deferenceCollections); }
            }
        }

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        public bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            using (_logger.BeginScope("{MethodName}({target}, {timeout}, {lockTaken})", nameof(TryEnterSynchronizableIfNotNull), target, timeout, lockTaken))
            {
                if (target is not null)
                {
                    Monitor.Enter(_deferenceCollections);
                    try
                    {
                        if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                            return DelegateDeference<T>.DeferenceCollection.TryEnter(collection, timeout, ref lockTaken, out deference);
                        if (TryGetSyncRoot(target, out object syncRoot))
                            return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, ref lockTaken, out deference);
                    }
                    finally { Monitor.Exit(_deferenceCollections); }
                }
                deference = null;
                return false;
            }
        }
    }
}
