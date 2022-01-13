using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents information about the state of an activity.
    /// </summary>
    public interface IActivityInfo
    {
        /// <summary>
        /// Gets the unique identifier of the described activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the described activity.</value>
        Guid ActivityId { get; }

        /// <summary>
        /// Gets the unique identifier of the parent activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the parent activity or <see langword="null" /> if there is no parent activity.</value>
        Guid? ParentActivityId { get; }

        /// <summary>
        /// Gets the short description of the activity.
        /// </summary>
        /// <remarks>This should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
        /// <value>A <see cref="string" /> that describes the activity.</value>
        string ShortDescription { get; }

        /// <summary>
        /// Gets the activity's status message.
        /// </summary>
        /// <remarks>This should never be <see langword="null" /> or <see cref="string.Empty" />.</remarks>
        /// <value>A <see cref="string" /> that contains a short message describing status for the activity.</value>
        string StatusMessage { get; }
    }

    /// <summary>
    /// Represents information about the state of an activity that is associated with a user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityInfo<TState> : IActivityInfo
    {
        /// <summary>
        /// Gets the user-defined value.
        /// </summary>
        /// <value>The user-defined vaue that is associated with the activity.</value>
        TState AsyncState { get; }
    }
}
