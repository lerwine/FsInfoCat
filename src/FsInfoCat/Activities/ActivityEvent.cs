using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents an activity event.
    /// </summary>
    /// <seealso cref="EventArgs" />
    /// <seealso cref="IActivityEvent" />
    public class ActivityEvent : EventArgs, IActivityEvent
    {
        /// <summary>
        /// Gets the unique identifier of the described activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the described activity.</value>
        public Guid ActivityId { get; }

        /// <summary>
        /// Gets the unique identifier of the parent activity.
        /// </summary>
        /// <value>The <see cref="Guid" /> value that is unique to the parent activity or <see langword="null" /> if there is no parent activity.</value>
        public Guid? ParentActivityId { get; }

        /// <summary>
        /// Gets the short description of the activity.
        /// </summary>
        /// <value>A <see cref="string" /> that describes the activity.</value>
        public string ShortDescription { get; }

        /// <summary>
        /// Gets the lifecycle status value.
        /// </summary>
        /// <value>An <see cref="ActivityState" /> value that indicates the lifecycle status of the activity.</value>
        public ActivityState StatusValue { get; }

        /// <summary>
        /// Gets the description of the activity's status.
        /// </summary>
        /// <value>A <see cref="string" /> that gives a verbose description the status for the activity.</value>
        public string StatusDescription { get; }

        /// <summary>
        /// Gets the exception (if any) associated with the event.
        /// </summary>
        /// <value>The <see cref="Exception" /> associated with the event or <see langword="null" /> if there is none.</value>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue)
        {
            if (statusValue == ActivityState.Faulted)
                throw new ArgumentOutOfRangeException(nameof(statusValue));
            ActivityId = (activityInfo ?? throw new ArgumentNullException(nameof(activityInfo))).ActivityId;
            ParentActivityId = activityInfo.ParentActivityId;
            ShortDescription = activityInfo.ShortDescription;
            StatusValue = statusValue;
            StatusDescription = activityInfo.StatusDescription;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEvent"/> class.
        /// </summary>
        /// <param name="activityInfo">The source activity information.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> or <paramref name="error"/> is <see langword="null"/>.</exception>
        public ActivityEvent([DisallowNull] IActivityInfo activityInfo, ActivityState statusValue, [DisallowNull] Exception error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error));
            ActivityId = (activityInfo ?? throw new ArgumentNullException(nameof(activityInfo))).ActivityId;
            ParentActivityId = activityInfo.ParentActivityId;
            ShortDescription = activityInfo.ShortDescription;
            StatusValue = statusValue;
            StatusDescription = activityInfo.StatusDescription;
        }
    }

    /// <summary>
    /// Represents an event for an activity that has an associated user-defined value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value associated with the activity.</typeparam>
    /// <seealso cref="ActivityEvent" />
    /// <seealso cref="IActivityEvent{TState}" />
    public class ActivityEvent<TState> : ActivityEvent, IActivityEvent<TState>
    {
        /// <summary>
        /// Gets the user-specified value that is associated with the activity.
        /// </summary>
        /// <value>The <typeparamref name="TState"/> value that is associated with the described activity.</value>
        public TState AsyncState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">Describes the source activity.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="statusValue"/> is <see cref="ActivityState.Faulted"/>.</exception>
        public ActivityEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue) : base(activityInfo, statusValue) => AsyncState = activityInfo.AsyncState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEvent{TState}"/> class.
        /// </summary>
        /// <param name="activityInfo">Describes the source activity.</param>
        /// <param name="statusValue">The activity lifecycle status value.</param>
        /// <param name="error">The exception that is to be associated with the event.</param>
        /// <exception cref="ArgumentNullException"><paramref name="activityInfo"/> or <paramref name="error"/> is <see langword="null"/>.</exception>
        public ActivityEvent([DisallowNull] IActivityInfo<TState> activityInfo, ActivityState statusValue, [DisallowNull] Exception error) : base(activityInfo, statusValue, error) => AsyncState = activityInfo.AsyncState;
    }
}
