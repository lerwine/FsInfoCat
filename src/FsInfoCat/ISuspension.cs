using System;

namespace FsInfoCat
{
    /// <summary>
    /// An object that is used to maintain a suspended state on a <see cref="ISuspendable"/> opbject.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface ISuspension : IDisposable
    {
        /// <summary>
        /// Gets an object that can be used to determine whether this represents the same suspension as another <see cref="ISuspendable"/>.
        /// </summary>
        /// <value>
        /// Ann object that can be used to determine whether this represents the same suspension as another <see cref="ISuspendable"/>.
        /// </value>
        object ConcurrencyToken { get; }
    }
}
