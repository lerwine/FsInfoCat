using System;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Contains information about the state of an activity.
    /// </summary>
    public interface IActivityInfo
    {
        /// <summary>
        /// Gets the unique identifier of the described activity.
        /// </summary>
        /// <value>The <see cref="Guid"/> value that is unique to the described activity.</value>
        Guid ActivityId { get; }

        /// <summary>
        /// Gets the unique identifier of the parent activity.
        /// </summary>
        /// <value>The <see cref="Guid"/> value that is unique to the parent activity or <see langword="null"/> if there is no parent activity.</value>
        Guid? ParentActivityId { get; }

        /// <summary>
        /// Gets the short description of the activity.
        /// </summary>
        /// <value>A <see cref="string"/> that describes the activity.</value>
        string ShortDescription { get; }

        /// <summary>
        /// Gets the description of the activity's status.
        /// </summary>
        /// <value>A <see cref="string"/> that gives a verbose description the status for the activity.</value>
        string StatusDescription { get; }
    }

    /// <summary>
    /// Contains information about the state of an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user specified value associated with the described activity.</typeparam>
    /// <seealso cref="IActivityInfo" />
    public interface IActivityInfo<TState> : IActivityInfo
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        TState AsyncState { get; }
    }
}
