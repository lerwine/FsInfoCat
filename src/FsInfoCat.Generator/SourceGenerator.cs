using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FsInfoCat.Generator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        #region Static members

        public const string DiagnosticID_MissingResourcesFileError = "MD0001";

        public const string DiagnosticID_MissingEntityTypesFileError = "MD0002";

        public const string DiagnosticID_XmlParseError = "MD0003";

        public const string DiagnosticID_SchemaValidationError = "MD0004";

        public const string DiagnosticID_ResourceTypeNotSupported = "MD0005";

        public const string DiagnosticID_SchemaValidationWarning = "MD0006";

        public const string DiagnosticID_MissingResourceError = "MD0007";

        internal static readonly DiagnosticDescriptor DiagnosticDescriptor_XmlParseError =
            new(
                DiagnosticID_XmlParseError,
                "XML Parsing Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor DiagnosticDescriptor_SchemaValidationError =
            new(
                DiagnosticID_SchemaValidationError,
                "Schema Validation Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public static readonly Regex NewlineRegex = new(@"\r\n|\n", RegexOptions.Compiled);

        #endregion

        public void Execute(GeneratorExecutionContext context)
        {
            ResourcesGenerator resourcesGenerator = ResourcesGenerator.Create(context);
            if (resourcesGenerator is null) return;
            ModelGenerator modelGenerator = ModelGenerator.Create(context, resourcesGenerator.Xml);
            if (modelGenerator is null) return;
            if (resourcesGenerator.GenerateCode(context))
                modelGenerator.GenerateCode(context);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Initialization not required.
        }
    }
}
