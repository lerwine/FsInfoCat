using System;
using System.ComponentModel;

namespace DevHelperGUI
{
    public class EvaluationFinishedEventArgs : AsyncCompletedEventArgs
    {
        public object Result { get; }

        public bool IsSingleInput { get; }

        public EvaluationState State { get; }

        public string ErrorMessage { get; }

        public EvaluationFinishedEventArgs(RegexEvaluatableEventArgs args, object userState)
            : base(args.Exception, args.Cancel, userState)
        {
            IsSingleInput = args.IsSingleInput;
            Result = args.Result;
            State = args.State;
            ErrorMessage = args.ErrorMessage;
        }

        public EvaluationFinishedEventArgs(bool isSingleInput, Exception exception, object userState)
            : base(exception, exception is null || exception is OperationCanceledException, userState)
        {
            IsSingleInput = isSingleInput;
            Result = null;
            State = Cancelled ? EvaluationState.NotEvaluated : EvaluationState.EvaluationError;
            ErrorMessage = (exception is null) ? "Operation canceled" : (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);
        }
    }
}
