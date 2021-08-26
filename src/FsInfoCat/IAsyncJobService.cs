using System;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IAsyncJobService
    {
        IAsyncJobModel<Task<TResult>> Create<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3,
            Func<TArg1, TArg2, TArg3, IStatusListener, Task<TResult>> method);

        IAsyncJobModel<Task<TResult>> Create<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, IStatusListener, Task<TResult>> method);

        IAsyncJobModel<Task<TResult>> Create<TArg, TResult>(TArg arg, Func<TArg, IStatusListener, Task<TResult>> method);

        IAsyncJobModel<Task<TResult>> Create<TResult>(Func<IStatusListener, Task<TResult>> method);

        IAsyncJobModel<Task> Create<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<TArg1, TArg2, TArg3, IStatusListener, Task> method);

        IAsyncJobModel<Task> Create<TArg1, TArg2>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, IStatusListener, Task> method);

        IAsyncJobModel<Task> Create<TArg>(TArg arg, Func<TArg, IStatusListener, Task> method);

        IAsyncJobModel<Task> Create(Func<IStatusListener, Task> method);
    }
}
