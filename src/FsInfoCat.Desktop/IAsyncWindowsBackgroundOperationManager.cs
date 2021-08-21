using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public interface IAsyncWindowsBackgroundOperationManager : IAsyncBackgroundOperationManager
    {
        Task<TResult> FromAsync<TState, TResult>(string title, string initialMessage, TState state, [DisallowNull] Func<TState, IWindowsStatusListener, Task<TResult>> func);
        Task<TResult> FromAsync<TResult>(string title, string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task<TResult>> func);
        Task FromAsync<TState>(string title, string initialMessage, TState state, [DisallowNull] Func<TState, IWindowsStatusListener, Task> func);
        Task FromAsync(string title, string initialMessage, [DisallowNull] Func<IWindowsStatusListener, Task> func);
    }
}
