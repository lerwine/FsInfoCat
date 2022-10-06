using Microsoft.CodeAnalysis;
using System;
using System.Text.RegularExpressions;

namespace FsInfoCat.Generator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        #region Static members

        public static readonly Regex NewlineRegex = new(@"\r\n|\n", RegexOptions.Compiled);

        private static readonly WrappedMessageDiagnosticFactory<string> DiagnosticFactory_UnhandledException = new(DiagnosticId.UnhandledException, "Unhandled Exception",
            "An unhandled exception has occurred: {0} => {1}");

        #endregion

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                ResourcesGenerator resourcesGenerator = ResourcesGenerator.Create(context);
                ModelGenerator modelGenerator = ModelGenerator.Create(context, resourcesGenerator.Xml);
                resourcesGenerator.GenerateCode(context);
                modelGenerator?.GenerateCode(context);
            }
            catch (FatalDiagosticException exc)
            {
                context.ReportDiagnostic(exc.Diagnostic);
            }
            catch (Exception exc)
            {
                string stackTrace;
                try { stackTrace = exc.StackTrace; }
                catch { stackTrace = null; }
                string message = exc.Message;
                context.ReportDiagnostic(DiagnosticFactory_UnhandledException.Create(string.IsNullOrWhiteSpace(message) ? exc.GetType().Name : message, string.IsNullOrWhiteSpace(stackTrace) ? "[Stack trace unavailable]" : NewlineRegex.Replace(stackTrace, "=>")));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Initialization not required.
        }
    }
}
