using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.AsyncOps
{
    public record CancellableTask<T>
        where T : Task
    {
        public CancellationTokenSource TokenSource { get; init; }
        public T Task { get; init; }
    }

    public record CancellableTask<TTask, TState>
    {
        public CancellationTokenSource TokenSource { get; init; }
        public TTask Task { get; init; }
        public TState State { get; init; }
    }
}
