using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public abstract partial class InvocationListener<IO> : IInvocationResult
        where IO : IInvocationIO
    {
        private IO _io;

        public bool WasInvoked { get; private set; }

        public IO Results
        {
            get => _io;
            set
            {
                _io = value ?? throw new ArgumentNullException(nameof(value));
                WasInvoked = true;
            }
        }

        ITestInputData IInvocationIO.Inputs => _io.Inputs;

        ITestOutputData IInvocationIO.Outputs => _io.Outputs;

        protected InvocationListener(IO io)
        {
            if (io is null)
                throw new ArgumentNullException(nameof(io));
            _io = io;
        }

        internal static void SetInvoked<T>(T listener)
            where T : InvocationListener<InvocationIO>
        {
            listener.WasInvoked = true;
        }
    }

    public class InvocationListener<I, O, IO> : InvocationListener<IO>, IInvocationResult<I, O>
        where I : ITestInputData
        where O : ITestOutputData
        where IO : IInvocationIO<I, O>
    {
        public I Inputs => Results.Inputs;

        public O Outputs => Results.Outputs;

        public InvocationListener(IO io) : base(io) { }
    }

    public class InvocationListener<I, O, R, IO> : InvocationListener<I, O, IO>, IInvocationResult<I, O, R>
        where I : ITestInputData
        where O : ITestOutputData
        where IO : IInvocationIO<I, O, R>
    {
        public R ReturnValue => Results.ReturnValue;

        public InvocationListener(IO io) : base(io) { }
    }
}
