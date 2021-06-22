using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public interface IAsyncOperation : IAsyncResult
    {
        bool IsCanceled { get; }
        TaskStatus Status { get; }
        AggregateException Exception { get; }
        bool IsFaulted { get; }
    }

    public interface IAsyncOperation<TState> : IAsyncOperation
    {
        new TState AsyncState { get; }
    }
}
