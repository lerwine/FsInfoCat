using System;
using System.ComponentModel;

namespace FsInfoCat
{
    /// <summary>
    /// Thread-safe suspension state provider.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public interface ISuspensionProvider : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether this instance is suspended.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is suspended; otherwise, <c>false</c>.
        /// </value>
        bool IsSuspended { get; }

        /// <summary>
        /// Occurs when <see cref="IsSuspended"/> changes to <see langword="true"/> as the first <see cref="ISuspension_obsolete"/> is instantiated.
        /// </summary>
        event EventHandler BeginSuspension;

        /// <summary>
        /// Occurs when <see cref="IsSuspended"/> changes to <see langword="false"/> as the last <see cref="ISuspension_obsolete"/> is disposed.
        /// </summary>
        event EventHandler EndSuspension;

        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Synchronously invokes an <see cref="Action"/> delegate if this <see cref="SuspensionProvider"/> is not suspended.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> delegate to invoke if this <see cref="SuspensionProvider"/> is not suspended.</param>
        /// <returns><see langword="true"/> if the <paramref name="action"/> was invoked; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null</exception>
        bool IfNotSuspended(Action action);

        TResult NotSuspendedElse<TResult>(Func<TResult> ifSuspendedFalse, Func<TResult> ifSuspendedTrue);

        TResult NotSuspendedElse<TResult>(Func<TResult> ifSuspendedFalse, TResult ifSuspendedTrue);

        TResult NotSuspendedElse<TResult>(TResult ifSuspendedFalse, Func<TResult> ifSuspendedTrue);

        /// <summary>
        /// Enters the suspended state.
        /// </summary>
        /// <returns>The <see cref="ISuspension_obsolete"/> which is used to maintain the suspended state.</returns>
        ISuspension_obsolete Suspend();

        /// <summary>
        /// Synchronously calls a <see cref="Func{TResult}"/> delegate if this <see cref="SuspensionProvider"/> is not suspended.
        /// </summary>
        /// <typeparam name="TResult">The type of value returned by the <see cref="Func{TResult}"/> delegate.</typeparam>
        /// <param name="function">The <see cref="Func{TResult}"/> delegate to call if this <see cref="SuspensionProvider"/> is not suspended.</param>
        /// <param name="result">The result value from the <see cref="Func{TResult}"/> delegate or the default value if this <see cref="SuspensionProvider"/>
        /// is suspended.</param>
        /// <returns><see langword="true"/> if the <paramref name="function"/> was called; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null</exception>
        bool TryIfNotSuspended<TResult>(Func<TResult> function, out TResult result);

        /// <summary>
        /// Verifies that this suspension provider is not in the suspended state.
        /// </summary>
        /// <exception cref="InvalidOperationException">This suspension provider is in a suspended state.</exception>
        void VerifyNotSuspended();
    }
}
