using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.DeferredDelegation
{
    /// <summary>
    /// Provides functionality for synchronization and deferred delegation.
    /// </summary>
    public interface IDeferredDelegationService
    {
        /// <summary>
        /// Determines whether delegate deference is active for the specified target.
        /// </summary>
        /// <typeparam name="T">The type of target object for which events may be deferred.</typeparam>
        /// <param name="target">The object for which events may be deferred.</param>
        /// <returns><see langword="true"/> if event deference is active for the <paramref name="target"/> object; otherwise, <see langword="false"/>.</returns>
        bool IsDeferred<T>(T target) where T : class;

        /// <summary>
        /// Creates a delegate deference object for specified target object.
        /// </summary>
        /// <typeparam name="T">The type of object on which events can be deferred.</typeparam>
        /// <param name="target">The object on which events can be deferred.</param>
        /// <returns>An <see cref="IDelegateDeference{T}"/> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target"/>
        /// object are disposed or until explicitly executed.</returns>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        IDelegateDeference<T> Enter<T>(T target) where T : class;

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
        /// <seealso cref="System.Threading.Monitor.Enter(object, ref bool)"/>
        IDelegateDeference<T> Enter<T>(T target, ref bool lockTaken) where T : class;

        /// <summary>
        /// Creates a delegate deference object for specified synchronized collection, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target) where T : class, ICollection;

        /// <summary>
        /// Creates a delegate deference object for specified collection if it is synchronized, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed or <see langword="null"/> if its <see langword="null"/>, not synchronized
        /// (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        bool EnterIfSynchronized<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Creates a delegate deference object for specified synchronized collection, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ICollection;

        /// <summary>
        /// Creates a delegate deference object for specified collection if it is synchronized, creating a thread-exclusive lock on the collection's synchronization object.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed or <see langword="null"/> if its <see langword="null"/>, not synchronized
        /// (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        bool EnterIfSynchronized<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object)"/>
        bool TryEnterSynchronized<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection;


        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <see cref="System.Threading.Monitor.TryEnter(object)"/>
        bool TryEnterIfSynchronized<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="target"/> is not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>),
        /// or <see cref="ICollection.SyncRoot"/> is <see langword="null"/> or could not be accessed.</exception>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        bool TryEnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        bool TryEnterIfSynchronized<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
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
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int)"/>
        bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int)"/>
        bool TryEnterIfSynchronized<T>([AllowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
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
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int, ref bool)"/>
        bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified collection's synchronization object if it is synchronized, creating a delegate deference object,
        /// if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the <paramref name="target"/> collection
        /// is <see langword="null"/>, not synchronized (<see cref="ICollection.IsSynchronized"/> is <see langword="false"/>), or <see cref="ICollection.SyncRoot"/>
        /// is <see langword="null"/> or could not be accessed.</param>
        /// <returns><see langword="true"/> if the <paramref name="target"/> is synchronized and a lock was obtained on the <see cref="ICollection.SyncRoot"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int, ref bool)"/>
        bool TryEnterIfSynchronized<T>([AllowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
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
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan)"/>
        bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchronized collection's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The  synchronized collection.</param>
        /// <param name="timeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the
        /// <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ICollection.SyncRoot"/> of the <paramref name="target"/> collection;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan)"/>
        bool TryEnterIfSynchronized<T>([AllowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ICollection;

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
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

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
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        bool TryEnterIfSynchronized<T>([AllowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object, creating a thread-exclusive lock on
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the
        /// <paramref name="target" /> object are disposed or until explicitly executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property of <paramref name="target"/> is <see langword="null"/> or could not
        /// be accessed.</exception>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target) where T : class, ISynchronizable;

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object if it is not <see langword="null"/> and
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see> is not <see langword="null"/>, creating a thread-exclusive lock on
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see>. </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing 
        ///the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the taret object is <see langword="null"/>
        /// or <see cref="ISynchronizable.SyncRoot"/> is null or could not be accessed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        bool EnterSynchronizableIfNotNull<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object, creating a thread-exclusive lock on
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <returns>An <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing the <paramref name="target" />
        /// object are disposed or until explicitly
        /// executed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <see cref="ISynchronizable.SyncRoot"/> property of <paramref name="target"/> is <see langword="null"/> or could not
        /// be accessed.</exception>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ISynchronizable;

        /// <summary>
        /// Creates a delegate deference object for specified synchronizable object if it is not <see langword="null"/> and
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see> is not <see langword="null"/>, creating a thread-exclusive lock on
        /// its <see cref="ISynchronizable.SyncRoot">synchronization object</see>.
        /// </summary>
        /// <typeparam name="T">The type of synchronized collection on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed. This will be <see langword="null"/> if the taret object is <see langword="null"/>
        /// or <see cref="ISynchronizable.SyncRoot"/> is null or could not be accessed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="System.Threading.Monitor.Enter(object)"/>
        bool EnterSynchronizableIfNotNull<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <see cref="System.Threading.Monitor.TryEnter(object)"/>
        bool TryEnterSynchronizable<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <see cref="System.Threading.Monitor.TryEnter(object)"/>
        bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <see cref="System.Threading.Monitor.TryEnter(object, ref bool)"/>
        bool TryEnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <see cref="System.Threading.Monitor.TryEnter(object, ref bool)"/>
        bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <item><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int)"/>
        bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int)"/>
        bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="millisecondsTimeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
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
        /// <item>The value of <paramref name="millisecondsTimeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int, ref bool)"/>
        bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

        /// <summary>
        /// Attempts to create a thread-exclusive lock on the specified synchrnizable object's synchronization object, creating a delegate deference object, if successful.
        /// </summary>
        /// <typeparam name="T">The type of synchronizable object on which events can be deferred.</typeparam>
        /// <param name="target">The synchronizable object.</param>
        /// <param name="millisecondsTimeout">The amount of time to wait for the lock. A value of <c>-1</c> millisecond specifies an infinite wait.</param>
        /// <param name="lockTaken">The boolean value, passed by reference, to track whether the lock was acquired. This must be <see langword="false"/> upon invocation.</param>
        /// <param name="deference">The <see cref="IDelegateDeference{T}" /> that can enqueue (defer) delegates, and executes them after all instances referencing
        /// the <paramref name="target" /> object are disposed or until explicitly executed.</param>
        /// <returns><see langword="true"/> if a thread-exclusive lock could be obtained on the <see cref="ISynchronizable.SyncRoot"/> of the <paramref name="target"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, int, ref bool)"/>
        bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan)"/>
        bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan)"/>
        bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <item>The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/> or is greater
        ///     than <see cref="int.MaxValue"/>.</item>
        /// </list></exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;

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
        /// <exception cref="ArgumentOutOfRangeException">The value of <paramref name="timeout"/> in milliseconds is negative, and not equal to <see cref="System.Threading.Timeout.Infinite"/>
        /// or is greater than <see cref="int.MaxValue"/>.</exception>
        /// <see cref="System.Threading.Monitor.TryEnter(object, TimeSpan, ref bool)"/>
        bool TryEnterSynchronizableIfNotNull<T>([AllowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
    }
}
