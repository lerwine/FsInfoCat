using Microsoft.CodeAnalysis;

namespace FsInfoCat.Generator
{
    public enum DiagnosticId : ushort
    {
        [DiagnostiCategory(nameof(ResourcesGenerator))]
        MissingResourcesFileError = 0x0001,

        [DiagnostiCategory(nameof(ModelGenerator))]
        MissingEntityTypesFileError,

        [DiagnostiCategory(nameof(ResourcesGenerator))]
        ResxXmlParseError,

        [DiagnostiCategory(nameof(ModelGenerator))]
        EntityTypesXmlParseError,

        [DiagnostiCategory(nameof(ResourcesGenerator))]
        ResxSchemaValidationError,

        [DiagnostiCategory(nameof(ModelGenerator))]
        EntityTypesSchemaValidationError,

        [DiagnostiCategory(nameof(ResourcesGenerator))]
        ResourceTypeNotSupported,

        [DiagnostiCategory(nameof(ModelGenerator), DefaultSeverity = DiagnosticSeverity.Warning)]
        EntityTypesSchemaValidationWarning,

        [DiagnostiCategory(nameof(ResourcesGenerator))]
        MissingResourceError,

        [DiagnostiCategory(nameof(ResourcesGenerator))]
        ResxReferencingError,

        [DiagnostiCategory(nameof(ResourcesGenerator), DefaultSeverity = DiagnosticSeverity.Warning)]
        DefinitionElementMissingWarning,

        UnhandledException
    }
}
