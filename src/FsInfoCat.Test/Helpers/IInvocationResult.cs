using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public interface IInvocationResult : IInvocationIO
    {
        bool WasInvoked { get; }
    }

    public interface IInvocationResult<R> : IInvocationResult, IInvocationIO<R> { }

    public interface IInvocationResult<I, O> : IInvocationResult, IInvocationIO<I, O>
        where I : IInvocationInput
        where O : IInvocationOutput
    { }

    public interface IInvocationResult<I, O, R> : IInvocationResult<I, O>, IInvocationResult<R>, IInvocationIO<I, O, R>
        where I : IInvocationInput
        where O : IInvocationOutput
    { }

}
