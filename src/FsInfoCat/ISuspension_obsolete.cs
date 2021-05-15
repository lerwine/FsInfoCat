using System;

namespace FsInfoCat
{
    /// <summary>
    /// Manages the suspended state of a <see cref="ISuspensionProvider"/> object, causing the <see cref="ISuspensionProvider"/> to enter the suspended
    /// as this type is instantiated through the <see cref="Suspend"/> method.
    /// </summary>
    /// <remarks>The <see cref="ISuspensionProvider"/> remains in the suspended state until the last object of this type is disposed.</remarks>
    /// <seealso cref="IDisposable" />
    public interface ISuspension_obsolete : IDisposable
    {
        /// <summary>
        /// Gets the concurrency token that is used to track suspension states.
        /// </summary>
        /// <value>
        /// The concurrency token that is used to track suspension states. A new token is created each time the <see cref="IsSuspended"/>
        /// property changes to <see langword="true"/>.
        /// </value>
        object Token { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        bool IsDisposed { get; }
    }

}
