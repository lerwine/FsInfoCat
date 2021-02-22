using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public class InvocationResult : IInvocationResult
    {
        public InvocationResult(bool wasInvoked) { WasInvoked = wasInvoked; }
        public bool WasInvoked { get; }
    }

    public class InvocationResult<T> : InvocationResult, IInvocationResult<T>
    {
        public InvocationResult() : base(false) { }
        public InvocationResult(T output0) : base(true) { Output0 = output0; }
        public T Output0 { get; }
    }

    public class InvocationResult<T0, T1> : InvocationResult<T0>, IInvocationResult<T0, T1>
    {
        public InvocationResult() : base() { }
        public InvocationResult(T0 output0, T1 output1)
            : base(output0)
        {
            Output1 = output1;
        }
        public T1 Output1 { get; }
    }

    public class InvocationResult<T0, T1, T2> : InvocationResult<T0, T1>, IInvocationResult<T0, T1, T2>
    {
        public InvocationResult() : base() { }
        public InvocationResult(T0 output0, T1 output1, T2 output2)
            : base(output0, output1)
        {
            Output2 = output2;
        }
        public T2 Output2 { get; }
    }

    public class InvocationResult<T0, T1, T2, T3> : InvocationResult<T0, T1, T2>, IInvocationResult<T0, T1, T2, T3>
    {
        public InvocationResult() : base() { }
        public InvocationResult(T0 output0, T1 output1, T2 output2, T3 output3)
            : base(output0, output1, output2)
        {
            Output3 = output3;
        }
        public T3 Output3 { get; }
    }

    public class InvocationResult<T0, T1, T2, T3, T4> : InvocationResult<T0, T1, T2, T3>, IInvocationResult<T0, T1, T2, T3, T4>
    {
        public InvocationResult() : base() { }
        public InvocationResult(T0 output0, T1 output1, T2 output2, T3 output3, T4 output4)
            : base(output0, output1, output2, output3)
        {
            Output4 = output4;
        }
        public T4 Output4 { get; }
    }

    public class InvocationResult<T0, T1, T2, T3, T4, T5> : InvocationResult<T0, T1, T2, T3, T4>, IInvocationResult<T0, T1, T2, T3, T4, T5>
    {
        public InvocationResult() : base() { }
        public InvocationResult(T0 output0, T1 output1, T2 output2, T3 output3, T4 output4, T5 output5)
            : base(output0, output1, output2, output3, output4)
        {
            Output5 = output5;
        }
        public T5 Output5 { get; }
    }

    public class InvocationResult<T0, T1, T2, T3, T4, T5, T6> : InvocationResult<T0, T1, T2, T3, T4, T5>, IInvocationResult<T0, T1, T2, T3, T4, T5, T6>
    {
        public InvocationResult() : base() { }
        public InvocationResult(T0 output0, T1 output1, T2 output2, T3 output3, T4 output4, T5 output5, T6 output6)
            : base(output0, output1, output2, output3, output4, output5)
        {
            Output6 = output6;
        }
        public T6 Output6 { get; }
    }
}
