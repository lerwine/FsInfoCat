using System;
using Microsoft.CodeAnalysis;

namespace FsInfoCat.Generator
{
    public class FatalDiagosticException : Exception
    {
        public Diagnostic Diagnostic { get; }
        public FatalDiagosticException() { }
        public FatalDiagosticException(Diagnostic diagnostic) : base((diagnostic ?? throw new ArgumentNullException(nameof(diagnostic))).ToString()) { Diagnostic = diagnostic; }
        public FatalDiagosticException(Diagnostic diagnostic, Exception inner) : base((diagnostic ?? throw new ArgumentNullException(nameof(diagnostic))).ToString(), inner) { Diagnostic = diagnostic; }
    }
}
