using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.AsyncOps
{
    /// <summary>
    /// Describes a type that generates progress event objects.
    /// </summary>
    /// <typeparam name="TEvent">The type of the progress event.</typeparam>
    /// <typeparam name="TProgress">The type of the progress context object.</typeparam>
    public interface IBackgroundProgressEventFactory<TEvent, TProgress>
            where TEvent : IBackgroundProgressEvent
            where TProgress : IBackgroundProgress<TEvent>
    {
        /// <summary>
        /// Creates a progress event object.
        /// </summary>
        /// <param name="backgroundProgress">The background progress context object.</param>
        /// <param name="statusDescription">The background progress status description.</param>
        /// <param name="currentOperation">The current operation.</param>
        /// <param name="code">The message code or <see langword="null"/> for no message code.</param>
        /// <param name="percentComplete">The percent complete.</param>
        /// <param name="error">The error associated with the progress event or <see langword="null"/> if there is no error.</param>
        /// <returns>A progress event object of type <typeparamref name="TEvent"/> to represent a progress event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundProgress"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="statusDescription"/> is <see langword="null"/>, empty or whitespace-only
        /// <para>- or -</para>
        /// <para><paramref name="percentComplete"/> is greater than <c>100</c>.</para></exception>
        TEvent CreateProgressEvent([DisallowNull] IBackgroundProgress<TEvent> backgroundProgress, [DisallowNull] string statusDescription, string currentOperation, MessageCode? code,
            byte? percentComplete, Exception error);
    }
}
