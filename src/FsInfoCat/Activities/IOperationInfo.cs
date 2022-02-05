namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents operational information about the state of an activity.
    /// </summary>
    /// <remarks>This is roughly based off of the same concept as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord" target="_blank">ProgressRecord</a> class.</remarks>
    public interface IOperationInfo : IActivityStatusInfo
    {

        /// <summary>
        /// Gets the description of the current operation of the many required to accomplish the activity.
        /// </summary>
        /// <value>The description of the current operation being performed or <see cref="string.Empty" /> if no operation has been started or no operation description has been provided.</value>
        /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.currentoperation" target="_blank">ProgressRecord.CurrentOperation</a> property
        /// and should never be <see langword="null" />.</remarks>
        string CurrentOperation { get; }

        /// <summary>
        /// Gets and sets the estimate of the percentage of total work for the activity that is completed.
        /// </summary>
        /// <value>The estimated percentage completion value from <c>0</c> to <c>100</c> or <c>-1</c> to indicate that the percentage completed should not be displayed.</value>
        /// <remarks>This serves the same conceptual purpose as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.percentcomplete" target="_blank">ProgressRecord.ParentActivityId</a> property.</remarks>
        int PercentComplete { get; }
    }

    /// <summary>
    /// Represents operational information about the state of an activity that is associated with a user-specified value.
    /// </summary>
    /// <typeparam name="TState">The type of the user-defined value that is associated with this activity.</typeparam>
    /// <remarks>This is roughly based off of the same concept as the PowerShell <a href="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord" target="_blank">ProgressRecord</a> class.</remarks>
    /// <seealso cref="IActivityInfo{TState}" />
    /// <seealso cref="IOperationInfo" />
    public interface IOperationInfo<TState> : IActivityStatusInfo<TState>, IOperationInfo { }
}
