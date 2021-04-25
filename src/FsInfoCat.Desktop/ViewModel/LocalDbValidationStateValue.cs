using System.ComponentModel;

namespace FsInfoCat.Desktop
{
    public enum LocalDbValidationStateValue : byte
    {
        [Description("Validating...")]
        Validating = 0,

        [Description("Database file name not specified.")]
        NotSpecified,

        [Description("Database file name does not refer to a file.")]
        NotAFile,

        [Description("Database file access error.")]
        AccessError,

        [Description("Database file not found.")]
        NotFound,

        [Description("Database file not initialized.")]
        NotInitialized,

        [Description("Database file closed.")]
        Closed,

        [Description("Opening database...")]
        Opening,

        [Description("Database file open error.")]
        OpenFailed,

        [Description("Database file opened.")]
        Opened
    }
}
