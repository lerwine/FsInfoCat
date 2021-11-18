using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public interface IAsyncOpStatus : IAsyncOpInfo
    {
        /// <summary>
        /// Gets the lifecycle stage of the asynchronous operation.
        /// </summary>
        TaskStatus Status { get; }

        /// <summary>
        /// Gets the parent asynchronous operation or <see langword="null"/> if there is no parent operation.
        /// </summary>
        /// <remarks>This serves the same conceptual puropose as the
        /// PowerShell <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.progressrecord.activityid#System_Management_Automation_ProgressRecord_ParentActivityId">ProgressRecord.ParentActivityId</see> property.</remarks>
        new IAsyncOpStatus ParentOperation { get; }

    }

    public interface IAsyncOpStatus<T> : IAsyncOpStatus, IAsyncOpInfo<T>
    {
    }
}
