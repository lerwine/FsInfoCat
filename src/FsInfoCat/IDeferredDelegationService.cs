using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public interface IDeferredDelegationService
    {
        /// <summary>
        /// Determines whether event deference is active for the specified target.
        /// </summary>
        /// <typeparam name="T">The type of object on which events may be deferred.</typeparam>
        /// <param name="target">The object on which events may be deferred.</param>
        /// <returns><see langword="true"/> if event deference is active for the <paramref name="target"/> object; otherwise, <see langword="false"/>.</returns>
        bool IsDeferred<T>(T target) where T : class;

        /// <summary>
        /// Creates an event deference object for specified target.
        /// </summary>
        /// <typeparam name="T">The type of object on which events can be deferred.</typeparam>
        /// <param name="target">The object on which events can be deferred.</param>
        /// <returns>A <see cref="IDelegateDeference{T}"/> that can enqueue event delegates, and executes them after all instances referencing the <paramref name="target"/> object are disposed.</returns>
        IDelegateDeference<T> Enter<T>(T target) where T : class;

        //
        // Summary:
        //     Acquires an exclusive lock on the specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the monitor lock.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null. 
        IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target) where T : class, ICollection;
        //
        // Summary:
        //     Acquires an exclusive lock on the specified object, and atomically sets a value
        //     that indicates whether the lock was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to wait.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock. Note If no exception occurs, the output of this method is always
        //     true.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ICollection;
        //
        // Summary:
        //     Attempts to acquire an exclusive lock on the specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        // Returns:
        //     true if the current thread acquires the lock; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        bool TryEnterSynchronized<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection;
        //
        // Summary:
        //     Attempts to acquire an exclusive lock on the specified object, and atomically
        //     sets a value that indicates whether the lock was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        bool TryEnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;
        //
        // Summary:
        //     Attempts, for the specified number of milliseconds, to acquire an exclusive lock
        //     on the specified object, and atomically sets a value that indicates whether the
        //     lock was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait for the lock.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is negative, and not equal to System.Threading.Timeout.Infinite.
        bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;
        //
        // Summary:
        //     Attempts, for the specified amount of time, to acquire an exclusive lock on the
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   timeout:
        //     A System.TimeSpan representing the amount of time to wait for the lock. A value
        //     of -1 millisecond specifies an infinite wait.
        //
        // Returns:
        //     true if the current thread acquires the lock; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value of timeout in milliseconds is negative and is not equal to System.Threading.Timeout.Infinite
        //     (-1 millisecond), or is greater than System.Int32.MaxValue.
        bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ICollection;
        //
        // Summary:
        //     Attempts, for the specified amount of time, to acquire an exclusive lock on the
        //     specified object, and atomically sets a value that indicates whether the lock
        //     was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   timeout:
        //     The amount of time to wait for the lock. A value of -1 millisecond specifies
        //     an infinite wait.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value of timeout in milliseconds is negative and is not equal to System.Threading.Timeout.Infinite
        //     (-1 millisecond), or is greater than System.Int32.MaxValue.
        bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection;
        //
        // Summary:
        //     Attempts, for the specified number of milliseconds, to acquire an exclusive lock
        //     on the specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait for the lock.
        //
        // Returns:
        //     true if the current thread acquires the lock; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is negative, and not equal to System.Threading.Timeout.Infinite.
        bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ICollection;


        //
        // Summary:
        //     Acquires an exclusive lock on the specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the monitor lock.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null. 
        IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target) where T : class, ISynchronizable;
        //
        // Summary:
        //     Acquires an exclusive lock on the specified object, and atomically sets a value
        //     that indicates whether the lock was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to wait.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock. Note If no exception occurs, the output of this method is always
        //     true.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ISynchronizable;
        //
        // Summary:
        //     Attempts to acquire an exclusive lock on the specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        // Returns:
        //     true if the current thread acquires the lock; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        bool TryEnterSynchronizable<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
        //
        // Summary:
        //     Attempts to acquire an exclusive lock on the specified object, and atomically
        //     sets a value that indicates whether the lock was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        bool TryEnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
        //
        // Summary:
        //     Attempts, for the specified number of milliseconds, to acquire an exclusive lock
        //     on the specified object, and atomically sets a value that indicates whether the
        //     lock was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait for the lock.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is negative, and not equal to System.Threading.Timeout.Infinite.
        bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
        //
        // Summary:
        //     Attempts, for the specified amount of time, to acquire an exclusive lock on the
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   timeout:
        //     A System.TimeSpan representing the amount of time to wait for the lock. A value
        //     of -1 millisecond specifies an infinite wait.
        //
        // Returns:
        //     true if the current thread acquires the lock; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value of timeout in milliseconds is negative and is not equal to System.Threading.Timeout.Infinite
        //     (-1 millisecond), or is greater than System.Int32.MaxValue.
        bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
        //
        // Summary:
        //     Attempts, for the specified amount of time, to acquire an exclusive lock on the
        //     specified object, and atomically sets a value that indicates whether the lock
        //     was taken.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   timeout:
        //     The amount of time to wait for the lock. A value of -1 millisecond specifies
        //     an infinite wait.
        //
        //   lockTaken:
        //     The result of the attempt to acquire the lock, passed by reference. The input
        //     must be false. The output is true if the lock is acquired; otherwise, the output
        //     is false. The output is set even if an exception occurs during the attempt to
        //     acquire the lock.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The input to lockTaken is true.
        //
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The value of timeout in milliseconds is negative and is not equal to System.Threading.Timeout.Infinite
        //     (-1 millisecond), or is greater than System.Int32.MaxValue.
        bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
        //
        // Summary:
        //     Attempts, for the specified number of milliseconds, to acquire an exclusive lock
        //     on the specified object.
        //
        // Parameters:
        //   obj:
        //     The object on which to acquire the lock.
        //
        //   millisecondsTimeout:
        //     The number of milliseconds to wait for the lock.
        //
        // Returns:
        //     true if the current thread acquires the lock; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The obj parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     millisecondsTimeout is negative, and not equal to System.Threading.Timeout.Infinite.
        bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable;
    }
}
