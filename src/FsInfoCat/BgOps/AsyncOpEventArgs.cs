using System;
using System.Threading.Tasks;

namespace FsInfoCat.BgOps
{
    public class AsyncOpEventArgs<TState> : EventArgs, IAsyncOpEventArgs<TState>
    {
        public IAsyncOpEventArgs ParentOperation => throw new NotImplementedException();

        public Exception Exception => throw new NotImplementedException();

        public TaskStatus Status => throw new NotImplementedException();

        public TState AsyncState => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public string Activity => throw new NotImplementedException();

        public string StatusDescription => throw new NotImplementedException();

        public string CurrentOperation => throw new NotImplementedException();

        IAsyncOpStatus IAsyncOpStatus.ParentOperation => throw new NotImplementedException();

        IAsyncOpInfo IAsyncOpInfo.ParentOperation => throw new NotImplementedException();
    }
}
