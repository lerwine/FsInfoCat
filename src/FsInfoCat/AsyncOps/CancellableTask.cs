using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public record CancellableTask<T>
        where T : Task
    {
        public CancellationTokenSource TokenSource { get; init; }
        public T Task { get; init; }
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public record CancellableTask<TTask, TState>
    {
        public CancellationTokenSource TokenSource { get; init; }
        public TTask Task { get; init; }
        public TState State { get; init; }
    }
}
