using System;

namespace DevHelper.PsHelp.Command
{
    [Flags]
    public enum PiplineInput
    {
        False = 0,
        ByValue = 1,
        ByPropertyName = 2
    }
}
