using System;

namespace DevHelperGUI
{
    public class EvaluationInputsModeEventArgs : EventArgs
    {
        public EvaluationInputsMode Mode { get; }

        public EvaluationInputsModeEventArgs(EvaluationInputsMode mode)
        {
            Mode = mode;
        }
    }
}
