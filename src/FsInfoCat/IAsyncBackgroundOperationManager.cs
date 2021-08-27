using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat
{
    [Obsolete("Use IAsyncJobFactoryService")]
    public interface IAsyncBackgroundOperationManager
    {
        Task<TResult> FromAsync<TState, TResult>(string title, string initialMessage, TState state, [DisallowNull] Func<TState, IStatusListener, Task<TResult>> func);
        Task<TResult> FromAsync<TResult>(string title, string initialMessage, [DisallowNull] Func<IStatusListener, Task<TResult>> func);
        Task FromAsync<TState>(string title, string initialMessage, TState state, [DisallowNull] Func<TState, IStatusListener, Task> func);
        Task FromAsync(string title, string initialMessage, [DisallowNull] Func<IStatusListener, Task> func);
        void CancelAll();
    }
}
