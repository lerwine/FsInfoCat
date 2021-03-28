using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DevHelperGUI
{
    public class EvaluationStateEventArgs
    {
        public bool AnySucceeded => IsAnySucceeded(State);
        public bool AnyFailed => IsAnyFailed(State);
        public EvaluationState State { get; }
        public ReadOnlyCollection<string> Messages { get; }
        public EvaluationStateEventArgs(EvaluationState state, IEnumerable<string> messages)
        {
            State = state;
            Messages = (messages is ReadOnlyCollection<string> ro) ? ro :
                new ReadOnlyCollection<string>((messages is null) ? Array.Empty<string>() : messages.Select(s => s ?? "").ToArray());
        }
        public static bool IsAnySucceeded(EvaluationState state) => state == EvaluationState.PartialSuccess || state == EvaluationState.Succeeded;
        public static bool IsAnyFailed(EvaluationState state) => state == EvaluationState.PartialSuccess || state == EvaluationState.Succeeded;
    }
}
