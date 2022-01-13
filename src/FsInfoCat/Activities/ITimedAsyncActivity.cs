namespace FsInfoCat.Activities
{
    /// <summary>
    /// Represents a timed asynchronous activity.
    /// </summary>
    /// <seealso cref="ITimedOperationInfo" />
    /// <seealso cref="IAsyncActivity" />
    public interface ITimedAsyncActivity : ITimedOperationInfo, IAsyncActivity { }
}
