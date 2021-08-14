using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.BgOps
{
    public interface IJob : IDisposable
    {
        event EventHandler<JobProgressEventArgs> ProgressChanged;
        Task Task { get; }
        void Cancel(bool throwOnFirstException);
        void Cancel();
    }

    public interface IStateJob<TState> : IJob
    {
        new event EventHandler<JobProgressEventArgs<TState>> ProgressChanged;
    }

    public interface IJob<TTask> : IJob
        where TTask : Task
    {
        new TTask Task { get; }
    }

    public interface IStateJob<TState, TTask> : IJob<TTask>, IStateJob<TState>
        where TTask : Task
    {
    }
}
