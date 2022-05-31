using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    // TODO: Document CancellableTask records
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
