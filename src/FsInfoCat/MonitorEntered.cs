using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Helper class to acquire monitor locks through <see cref="Monitor"/>.
    /// <para>Implements the <see cref="IDisposable" /> interface.</para>
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <seealso cref="Monitor"/>
    [Obsolete("Use IDeferredDelegationService, instead")]
    public abstract class MonitorEntered : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Gets the object upon which the monitor lock was obtained.
        /// </summary>
        /// <value>The object upon which the exclusive monitor lock was obtained.</value>
        public object SyncRoot { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorEntered"/> class.
        /// </summary>
        /// <param name="syncRoot">The object upon which a monitor lock was already taken.</param>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> was <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">No monitor lock was obtained on <paramref name="syncRoot"/>.</exception>
        protected MonitorEntered([DisallowNull] object syncRoot)
        {
            if (!Monitor.IsEntered(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot))))
                throw new InvalidOperationException();
            SyncRoot = syncRoot;
        }

        private readonly Queue<(Delegate Action, object[] Args)> _postDisposalActions = new();

        public void AddPostDisposalDelegate(Delegate d, params object[] args)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(MonitorEntered));
            _postDisposalActions.Enqueue((d, args));
        }

        public void AddPostDisposalAction(Action action) => AddPostDisposalDelegate(action);

        public void AddPostDisposalAction<TArg>(TArg arg, Action<TArg> action) => AddPostDisposalDelegate(action, arg);

        public void AddPostDisposalAction<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Action<TArg1, TArg2> action) => AddPostDisposalDelegate(action, arg1, arg2);

        public void AddPostDisposalAction<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Action<TArg1, TArg2, TArg3> action) =>
            AddPostDisposalDelegate(action, arg1, arg2, arg3);

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

        /// <summary>
        /// Obtains an exclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <returns>A <see cref="MonitorEntered{T}"/> object that will <see cref="Monitor.Exit(object)">relase the monitor lock</see> when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> TakeLock<T>([DisallowNull] T syncRoot) where T : class =>
            MonitorEntered<T>.Create(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot);

        /// <summary>
        /// Obtains an exclusive monitor lock on the specified object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference. This value must be <see langword="false"/> when this method is invoked.</param>
        /// <returns>A <see cref="MonitorEntered{T}"/> object that will <see cref="Monitor.Exit(object)">relase the monitor lock</see> when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> TakeLock<T>([DisallowNull] T syncRoot, ref bool lockTaken) where T : class =>
            MonitorEntered<T>.Create(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, ref lockTaken);

        /// <summary>
        /// Tries to obtain an xclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLock<T>([DisallowNull] T syncRoot, out MonitorEntered<T> monitorEntered) where T : class =>
            MonitorEntered<T>.TryCreate(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, out monitorEntered);

        /// <summary>
        /// Tries to obtain an xclusive monitor lock on the specified object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLock<T>([DisallowNull] T syncRoot, ref bool lockTaken, out MonitorEntered<T> monitorEntered) where T : class =>
            MonitorEntered<T>.TryCreate(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, ref lockTaken, out monitorEntered);

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLock<T>([DisallowNull] T syncRoot, TimeSpan timeout, out MonitorEntered<T> monitorEntered) where T : class =>
            MonitorEntered<T>.TryCreate(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, timeout, out monitorEntered);

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLock<T>([DisallowNull] T syncRoot, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered) where T : class =>
            MonitorEntered<T>.TryCreate(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, timeout, ref lockTaken, out monitorEntered);

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLock<T>([DisallowNull] T syncRoot, int millisecondsTimeout, out MonitorEntered<T> monitorEntered) where T : class =>
            MonitorEntered<T>.TryCreate(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, millisecondsTimeout, out monitorEntered);

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLock<T>([DisallowNull] T syncRoot, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered) where T : class =>
            MonitorEntered<T>.TryCreate(syncRoot ?? throw new ArgumentNullException(nameof(syncRoot)), syncRoot, millisecondsTimeout, ref lockTaken, out monitorEntered);

        /// <summary>
        /// Obtains an exclusive monitor lock on the specified object if it is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TakeLockIfNotNull<T>([AllowNull] T syncRoot, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            monitorEntered = MonitorEntered<T>.Create(syncRoot, syncRoot);
            return true;
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the specified object if it is not <see langword="null"/>, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference. This value must be <see langword="false"/>.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TakeLockIfNotNull<T>([AllowNull] T syncRoot, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            monitorEntered = MonitorEntered<T>.Create(syncRoot, syncRoot, ref lockTaken);
            return true;
        }

        /// <summary>
        /// Tries to obtain an xclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfNotNull<T>([AllowNull] T syncRoot, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            return MonitorEntered<T>.TryCreate(syncRoot, syncRoot, out monitorEntered);
        }

        /// <summary>
        /// Tries to obtain an xclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfNotNull<T>([AllowNull] T syncRoot, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            return MonitorEntered<T>.TryCreate(syncRoot, syncRoot, ref lockTaken, out monitorEntered);
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfNotNull<T>([AllowNull] T syncRoot, TimeSpan timeout, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            return MonitorEntered<T>.TryCreate(syncRoot, syncRoot, timeout, out monitorEntered);
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfNotNull<T>([AllowNull] T syncRoot, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            return MonitorEntered<T>.TryCreate(syncRoot, syncRoot, timeout, ref lockTaken, out monitorEntered);
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfNotNull<T>([AllowNull] T syncRoot, int millisecondsTimeout, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            return MonitorEntered<T>.TryCreate(syncRoot, syncRoot, millisecondsTimeout, out monitorEntered);
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified object, atomically setting a value by reference to indicate whether
        /// the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of object to obtain a monitor lock on.</typeparam>
        /// <param name="syncRoot">The object to obtain a monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if an exclusive monitor lock was obtained on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfNotNull<T>([AllowNull] T syncRoot, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class
        {
            if (syncRoot is null)
            {
                monitorEntered = null;
                return false;
            }
            return MonitorEntered<T>.TryCreate(syncRoot, syncRoot, millisecondsTimeout, ref lockTaken, out monitorEntered);
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <returns>The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/>
        /// is <see langword="false"/>) or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> TakeSynchronizedLock<T>([DisallowNull] T collection)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.Create(collection, syncRoot);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified collection,
        /// atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <returns>The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/>
        /// is <see langword="false"/>)
        /// or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</exception>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> TakeSynchronizedLock<T>([DisallowNull] T collection, ref bool lockTaken)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.Create(collection, syncRoot, ref lockTaken);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/>
        /// is <see langword="false"/>)
        /// or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="Monitor.TryEnter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizedLock<T>([DisallowNull] T collection, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/>
        /// is <see langword="false"/>)
        /// or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizedLock<T>([DisallowNull] T collection, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, ref lockTaken, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="null"/>)
        ///         or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</item>
        ///     <item><paramref name="timeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizedLock<T>([DisallowNull] T collection, TimeSpan timeout, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, timeout, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="null"/>)
        ///         or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</item>
        ///     <item><paramref name="timeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizedLock<T>([DisallowNull] T collection, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, timeout, ref lockTaken, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="null"/>)
        ///         or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</item>
        ///     <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizedLock<T>([DisallowNull] T collection, int millisecondsTimeout, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, millisecondsTimeout, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries the take synchronized
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item><paramref name="collection"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="null"/>)
        ///         or the <see cref="ICollection.SyncRoot"/> property was <see langword="null"/> or could not be accessed.</item>
        ///     <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizedLock<T>([DisallowNull] T collection, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, millisecondsTimeout, ref lockTaken, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(collection), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="monitorEntered">The monitor entered.</param>
        /// <returns><see langword="true"/> if the current the <paramref name="collection"/> is synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="true"/>);
        /// otherwise, <see langword="false"/> if <paramref name="collection"/> is <see langword="null"/>, <see cref="ICollection.IsSynchronized"/> is <see langword="null"/>,
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</returns>
        /// <seealso cref="Monitor.Enter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TakeLockIfSynchronized<T>([AllowNull] T collection, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
            {
                monitorEntered = MonitorEntered<T>.Create(collection, syncRoot);
                return true;
            }
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The monitor entered.</param>
        /// <returns><see langword="true"/> if the current the <paramref name="collection"/> is synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="true"/>);
        /// otherwise, <see langword="false"/> if <paramref name="collection"/> is <see langword="null"/>, <see cref="ICollection.IsSynchronized"/> is <see langword="null"/>,
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</returns>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TakeLockIfSynchronized<T>([AllowNull] T collection, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection ?? throw new ArgumentNullException(nameof(collection)), out object syncRoot))
            {
                monitorEntered = MonitorEntered<T>.Create(collection, syncRoot, ref lockTaken);
                return true;
            }
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfSynchronized<T>([AllowNull] T collection, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfSynchronized<T>([AllowNull] T collection, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, ref lockTaken, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfSynchronized<T>([AllowNull] T collection, TimeSpan timeout, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, timeout, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfSynchronized<T>([AllowNull] T collection, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, timeout, ref lockTaken, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfSynchronized<T>([AllowNull] T collection, int millisecondsTimeout, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, millisecondsTimeout, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified collection if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection.</typeparam>
        /// <param name="collection">The collection to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ICollection.SyncRoot"/> property of the
        /// target <paramref name="collection"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeLockIfSynchronized<T>([AllowNull] T collection, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ICollection
        {
            if (collection is not null && TryGetSyncRoot(collection, out object syncRoot))
                return MonitorEntered<T>.TryCreate(collection, syncRoot, millisecondsTimeout, ref lockTaken, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <returns>The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        /// or could not be accessed.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> TakeSynchronizableLock<T>([DisallowNull] T target)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.Create(target, syncRoot);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified object,
        /// atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <returns>The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        /// or could not be accessed.</exception>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> TakeSynchronizableLock<T>([DisallowNull] T target, ref bool lockTaken)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.Create(target, syncRoot, ref lockTaken);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        /// or could not be accessed.</exception>
        /// <seealso cref="Monitor.TryEnter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLock<T>([DisallowNull] T target, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        /// or could not be accessed.</exception>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLock<T>([DisallowNull] T target, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, ref lockTaken, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item>The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        ///             or could not be accessed.</item>
        ///     <item><paramref name="timeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLock<T>([DisallowNull] T target, TimeSpan timeout, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, timeout, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item>The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        ///             or could not be accessed.</item>
        ///     <item><paramref name="timeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLock<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, timeout, ref lockTaken, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item>The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        ///             or could not be accessed.</item>
        ///     <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLock<T>([DisallowNull] T target, int millisecondsTimeout, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, millisecondsTimeout, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of the following:
        /// <list type="bullet">
        ///     <item>The <see cref="ISynchronizable.SyncRoot"/> property on <paramref name="target"/> was <see langword="null"/>
        ///             or could not be accessed.</item>
        ///     <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLock<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, millisecondsTimeout, ref lockTaken, out monitorEntered);
            throw new ArgumentOutOfRangeException(nameof(target), "Collection is not synchronized or the SyncRoot property was null or could not be accessed.");
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="monitorEntered">The monitor entered.</param>
        /// <returns><see langword="true"/> if the current the <paramref name="target"/> is not null and <see cref="ISynchronizable.IsSynchronized"/>
        /// is not null and accssible; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Monitor.Enter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TakeSynchronizableLockIfNotNull<T>([AllowNull] T target, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
            {
                monitorEntered = MonitorEntered<T>.Create(target, syncRoot);
                return true;
            }
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The monitor entered.</param>
        /// <returns><see langword="true"/> if the current the <paramref name="target"/> is not null and <see cref="ISynchronizable.IsSynchronized"/>
        /// is not null and accssible; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TakeSynchronizableLockIfNotNull<T>([AllowNull] T target, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target ?? throw new ArgumentNullException(nameof(target)), out object syncRoot))
            {
                monitorEntered = MonitorEntered<T>.Create(target, syncRoot, ref lockTaken);
                return true;
            }
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLockIfNotNull<T>([AllowNull] T target, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will relase the monitor lock when disposed
        /// or <see langword="null"/> if no lock was obtained.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLockIfNotNull<T>([AllowNull] T target, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, ref lockTaken, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLockIfNotNull<T>([AllowNull] T target, TimeSpan timeout, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, timeout, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLockIfNotNull<T>([AllowNull] T target, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, timeout, ref lockTaken, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLockIfNotNull<T>([AllowNull] T target, int millisecondsTimeout, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, millisecondsTimeout, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the synchronization object of the specified object if it is synchronized.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object.</typeparam>
        /// <param name="target">The object to obtain an exclusive monitor lock on.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on the <see cref="ISynchronizable.SyncRoot"/> property of the
        /// target <paramref name="target"/>; otherwise, <see langword="false"/> if no lock was obtained.</returns>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryTakeSynchronizableLockIfNotNull<T>([AllowNull] T target, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
            where T : class, ISynchronizable
        {
            if (target is not null && TryGetSyncRoot(target, out object syncRoot))
                return MonitorEntered<T>.TryCreate(target, syncRoot, millisecondsTimeout, ref lockTaken, out monitorEntered);
            monitorEntered = null;
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    if (Monitor.IsEntered(SyncRoot))
                        Monitor.Exit(SyncRoot);
                    while (_postDisposalActions.TryDequeue(out (Delegate Action, object[] Args) action))
                        action.Action.DynamicInvoke(action.Args);
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// CLass which is used to maintain an exclusive monitor lock until disposed..
    /// <para>Extends <see cref="MonitorEntered" />.</para>
    /// </summary>
    /// <typeparam name="T">The type of object that is presumed to be the target of the synchronization.</typeparam>
    /// <seealso cref="MonitorEntered" />
    /// <seealso cref="Monitor"/>
    public sealed class MonitorEntered<T> : MonitorEntered
        where T : class
    {
        public T Target { get; }

        /// <summary>
        /// Obtains an exclusive monitor lock on the specified synchronization object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="syncRoot">The synchronize root.</param>
        /// <returns>A <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <seealso cref="Monitor.Enter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> Create([DisallowNull] T target, [DisallowNull] object syncRoot)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            Monitor.Enter(syncRoot);
            return new(target, syncRoot);
        }

        /// <summary>
        /// Obtains an exclusive monitor lock on the specified synchronization object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="syncRoot">The synchronize root.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference. This value must be <see langword="false"/> when this method is invoked.</param>
        /// <returns>A <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="lockTaken"/> was <see langword="true"/>.</exception>
        /// <seealso cref="Monitor.Enter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static MonitorEntered<T> Create([DisallowNull] T target, [DisallowNull] object syncRoot, ref bool lockTaken)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            Monitor.Enter(syncRoot, ref lockTaken);
            return new(target, syncRoot);
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the specified synchronization object.
        /// </summary>
        /// <param name="target">The presumed object being synchronized.</param>
        /// <param name="syncRoot">The object upon which the exclusive monitor lock is obtained for synchronized access.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <seealso cref="Monitor.TryEnter(object)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryCreate([DisallowNull] T target, [DisallowNull] object syncRoot, out MonitorEntered<T> monitorEntered)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            if (Monitor.TryEnter(syncRoot))
            {
                monitorEntered = new(target, syncRoot);
                return true;
            }
            monitorEntered = null;
            return false;
        }

        /// <summary>
        /// Tries to obtain an exclusive monitor lock on the specified synchronization object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <param name="target">The presumed object being synchronized.</param>
        /// <param name="syncRoot">The object upon which the exclusive monitor lock is obtained for synchronized access.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <seealso cref="Monitor.TryEnter(object, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryCreate([DisallowNull] T target, [DisallowNull] object syncRoot, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            bool success;
            Thread.BeginCriticalRegion();
            try
            {
                success = !lockTaken;
                if (success)
                {
                    try { Monitor.TryEnter(syncRoot, ref lockTaken); }
                    catch { /* Exceptions are of no significance in this context */ }
                    success = lockTaken;
                }
                monitorEntered = success ? new(target, syncRoot) : null;
            }
            finally { Thread.EndCriticalRegion(); }
            return success;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified synchronization object.
        /// </summary>
        /// <param name="target">The presumed object being synchronized.</param>
        /// <param name="syncRoot">The object upon which the exclusive monitor lock is obtained for synchronized access.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryCreate([DisallowNull] T target, [DisallowNull] object syncRoot, TimeSpan timeout, out MonitorEntered<T> monitorEntered)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            bool success;
            try { success = Monitor.TryEnter(syncRoot, timeout); }
            catch (ArgumentOutOfRangeException exception)
            {
                switch (exception.ParamName ?? "")
                {
                    case nameof(syncRoot):
                    case nameof(timeout):
                        throw;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(timeout), exception.Message);
                }
            }
            monitorEntered = success ? new(target, syncRoot) : null;
            return success;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified synchronization object, atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <param name="target">The presumed object being synchronized.</param>
        /// <param name="syncRoot">The object upon which the exclusive monitor lock is obtained for synchronized access.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> can be used for an infinite wait.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryCreate([DisallowNull] T target, [DisallowNull] object syncRoot, TimeSpan timeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            bool success;
            Thread.BeginCriticalRegion();
            try
            {
                success = !lockTaken;
                if (success)
                {
                    try { Monitor.TryEnter(syncRoot, timeout, ref lockTaken); }
                    catch (ArgumentOutOfRangeException exception)
                    {
                        if (!lockTaken)
                            switch (exception.ParamName ?? "")
                            {
                                case nameof(syncRoot):
                                case nameof(timeout):
                                    throw;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(timeout), exception.Message);
                            }
                    }
                    success = lockTaken;
                }
                monitorEntered = success ? new(target, syncRoot) : null;
            }
            finally { Thread.EndCriticalRegion(); }
            return success;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified synchronization object.
        /// </summary>
        /// <param name="target">The presumed object being synchronized.</param>
        /// <param name="syncRoot">The object upon which the exclusive monitor lock is obtained for synchronized access.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, int)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryCreate([DisallowNull] T target, [DisallowNull] object syncRoot, int millisecondsTimeout, out MonitorEntered<T> monitorEntered)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            bool success;
            try { success = Monitor.TryEnter(syncRoot, millisecondsTimeout); }
            catch (ArgumentOutOfRangeException exception)
            {
                switch (exception.ParamName ?? "")
                {
                    case nameof(syncRoot):
                    case nameof(millisecondsTimeout):
                        throw;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout), exception.Message);
                }
            }
            monitorEntered = success ? new(target, syncRoot) : null;
            return success;
        }

        /// <summary>
        /// Tries for a specific amount of time to obtain an exclusive monitor lock on the specified synchronization object,
        /// atomically setting a value by reference to indicate whether the lock is taken.
        /// </summary>
        /// <param name="target">The presumed object being synchronized.</param>
        /// <param name="syncRoot">The object upon which the exclusive monitor lock is obtained for synchronized access.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">A value coordinating whether acquire the lock was obtained, passed by reference.</param>
        /// <param name="monitorEntered">The <see cref="MonitorEntered{T}"/> object that will release the exclusive lock when disposed.</param>
        /// <returns><see langword="true"/> if the current thread has acquired an exclusive lock on <paramref name="syncRoot"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="syncRoot"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
        /// <seealso cref="Monitor.TryEnter(object, int, ref bool)"/>
        /// <seealso cref="Monitor.Exit(object)"/>
        public static bool TryCreate([DisallowNull] T target, [DisallowNull] object syncRoot, int millisecondsTimeout, ref bool lockTaken, out MonitorEntered<T> monitorEntered)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (syncRoot is null)
                throw new ArgumentNullException(nameof(syncRoot));
            bool success;
            Thread.BeginCriticalRegion();
            try
            {
                success = !lockTaken;
                if (success)
                {
                    try { Monitor.TryEnter(syncRoot, millisecondsTimeout, ref lockTaken); }
                    catch (ArgumentOutOfRangeException exception)
                    {
                        if (!lockTaken)
                            switch (exception.ParamName ?? "")
                            {
                                case nameof(syncRoot):
                                case nameof(millisecondsTimeout):
                                    throw;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout), exception.Message);
                            }
                    }
                    success = lockTaken;
                }
                monitorEntered = success ? new(target, syncRoot) : null;
            }
            finally { Thread.EndCriticalRegion(); }
            return success;
        }

        private MonitorEntered([DisallowNull] T target, [DisallowNull] object syncRoot) : base(syncRoot) { Target = target; }
    }
}
