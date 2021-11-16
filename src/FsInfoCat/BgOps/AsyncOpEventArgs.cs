using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    internal class AsyncOpEventArgs<T> : EventArgs, IAsyncOpEventArgs<T>
    {
        public TaskStatus Status { get; } = TaskStatus.Running;

        public IAsyncOpEventArgs ParentOperation { get; }

        public Exception Exception { get; }

        public T AsyncState { get; }

        public Guid Id { get; }

        public string Activity { get; }

        public string StatusDescription { get; }

        public string CurrentOperation { get; }

        IAsyncOpStatus IAsyncOpStatus.ParentOperation => throw new NotImplementedException();

        IAsyncOpInfo IAsyncOpInfo.ParentOperation => throw new NotImplementedException();

        public AsyncOpEventArgs([DisallowNull] IAsyncOpProgress<T> source, Exception exception)
        {
            AsyncState = (source ?? throw new ArgumentNullException(nameof(source))).AsyncState;
            Id = source.Id;
            Activity = source.Activity;
            StatusDescription = source.StatusDescription;
            CurrentOperation = source.CurrentOperation;
            ParentOperation = source.ParentOperation;
            Exception = exception;
        }
    }
}
