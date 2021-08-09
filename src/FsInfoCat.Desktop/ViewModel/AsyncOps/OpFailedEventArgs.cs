using System;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class OpFailedEventArgs : EventArgs
    {
        public OpFailedEventArgs(AggregateException exception)
        {
            Exception e = exception;
            while (exception.InnerExceptions.Count == 1)
            {
                if ((e = exception.InnerException) is not AggregateException a)
                    break;
                exception = a;
            }
            Exception = e;
        }

        /// <summary>
        /// Gets the exception that was thrown during execution of the background operation.
        /// </summary>
        /// <value>The exception that was thrown during execution of the background operation.</value>
        public Exception Exception { get; }
    }
}
