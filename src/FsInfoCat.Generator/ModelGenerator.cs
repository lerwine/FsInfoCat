using Microsoft.CodeAnalysis;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Text;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;

namespace FsInfoCat.Generator
{
    [Generator]
    public class ModelGenerator : ISourceGenerator
    {
        public const string Schema_RelativePath = "XmlSchemas\\ModelDefinitions.xsd";

        public const string FileName_ModelDefinitions = "ModelDefinitions.xml";

        public const string FileName_Resources = "Resources.resx";

        public const string DiagnosticID_MissingResourcesFileError = "MD0001";

        public const string DiagnosticID_XmlParseError = "MD0002";

        public const string DiagnosticID_SchemaValidationError = "MD0003";

        public const string DiagnosticID_SchemaValidationWarning = "MD0004";

        public const string ScopeName_All = "All";

        public const string ScopeName_Local = "Local";

        public const string ScopeName_Upstream = "Upstream";

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_MissingResourcesFileError =
            new DiagnosticDescriptor(
                DiagnosticID_MissingResourcesFileError,
                "Missing resources file",
                "Resources file error: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_XmlParseError =
            new DiagnosticDescriptor(
                DiagnosticID_XmlParseError,
                "XML Parsing Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_SchemaValidationError =
            new DiagnosticDescriptor(
                DiagnosticID_SchemaValidationError,
                "Schema Validation Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_SchemaValidationWarning =
            new DiagnosticDescriptor(
                DiagnosticID_SchemaValidationWarning,
                "XML Parsing Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private XDocument _modelDefinitions;
        private XDocument _resourceDefinitions;
        private TextLineCollection _modelLines;
        private string _modelDefinitionsPath;

        public void Execute(GeneratorExecutionContext context)
        {
            if (LoadModelDefinitions(context))
            {
                GenerateModels(context);
            }
        }

        private void GenerateModels(GeneratorExecutionContext context)
        {
            foreach (XElement element in _modelDefinitions.Root.Elements(XmlNames.ByteEnum))
                GenerateEnum(context, element, "byte");
            throw new NotImplementedException();
        }

        private void GenerateEnum(GeneratorExecutionContext context, XElement element, string baseTypeName)
        {
            string name = element.GetAttributeValue(XmlNames.Name);
            StringWriter writer = new StringWriter();
            if (!element.TryGetAttributeValue(XmlNames.Flags, out bool flags)) flags = false;
            if (element.Elements(XmlNames.Field).Any(e => !(e.Element(XmlNames.Display) is null)))
            {
                if (flags) writer.WriteLine("Use System;");
                writer.WriteLine("using System.ComponentModel.DataAnnotations;");
                writer.WriteLine();
            }
            else
            {
                if (flags) writer.WriteLine("Use System;");
                writer.WriteLine();
            }

            switch (element.GetAttributeValue(XmlNames.Scope) ?? ScopeName_All)
            {
                case ScopeName_Local:
                    writer.WriteLine("namespace FsInfoCat.Local.Model");
                    break;
                case ScopeName_Upstream:
                    writer.WriteLine("namespace FsInfoCat.Upstream.Model");
                    break;
                default:
                    writer.WriteLine("namespace FsInfoCat.Model");
                    break;
            }
            writer.WriteLine("{");
            if (!WriteCommentDocumentation(context, element, 1, writer)) return;
            if (flags) writer.WriteLine("    [Flags]");
            writer.Write("    public enum ");
            writer.Write(name);
            writer.Write(" : ");
            writer.WriteLine(baseTypeName);
            writer.WriteLine("    {");
            foreach (XElement fieldElement in element.Elements(XmlNames.Field))
            {
                if (!WriteCommentDocumentation(context, fieldElement, 2, writer)) return;
                name = fieldElement.GetAttributeValue(XmlNames.Name);
                XElement displayElement = fieldElement.Element(XmlNames.Display);
                string label = displayElement.GetAttributeValue(XmlNames.Name);
                string shortName = displayElement.GetAttributeValue(XmlNames.ShortName);
                if (displayElement is null)
                {
                }
                else
                {

                }
            }
            throw new NotImplementedException();
        }

        private bool WriteCommentDocumentation(GeneratorExecutionContext context, XElement element, int indentLevel, StringWriter writer)
        {
            throw new NotImplementedException();
        }

        private bool LoadModelDefinitions(GeneratorExecutionContext context)
        {
            // _resourceDefinitions
            AdditionalText modelDefinitionsFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_ModelDefinitions, StringComparison.InvariantCultureIgnoreCase));
            AdditionalText resourcesFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_Resources, StringComparison.InvariantCultureIgnoreCase));
            if (modelDefinitionsFile is null) return false;
            if (resourcesFile is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_MissingResourcesFileError, null, "Resources file not found."));
                return false;
            }
            SourceText resourcesText = modelDefinitionsFile.GetText(context.CancellationToken);
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                    resourcesText.Write(writer);
                stream.Seek(0L, SeekOrigin.Begin);
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    CheckCharacters = false,
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    Schemas = new XmlSchemaSet(),
                    ValidationType = ValidationType.Schema,
                    ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessInlineSchema
                };

                settings.Schemas.Add(ExtensionMethods.ModelNamespaceURI, Path.Combine(Path.GetDirectoryName(typeof(ModelGenerator).Assembly.Location), Schema_RelativePath));
                ValidationListener validationListener = new ValidationListener(context, resourcesText.Lines, resourcesFile.Path);
                settings.ValidationEventHandler += validationListener.ValidationEventHandler;
                try
                {
                    using (XmlReader reader = XmlReader.Create(stream, settings))
                        _resourceDefinitions = XDocument.Load(stream, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                }
                catch (XmlSchemaException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(resourcesText.Lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(resourcesFile.Path, textSpan, positionSpan), message ?? "(unexpected validation error)"));
                    return false;
                }
                catch (XmlException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(resourcesText.Lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_XmlParseError,
                        Location.Create(resourcesFile.Path, textSpan, positionSpan), message ?? "(unexpected parse error)"));
                    return false;
                }
            }
            if (_resourceDefinitions.Root is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError, Location.Create(resourcesFile.Path, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1))), "File has no root XML element"));
                return false;
            }

            SourceText modelDefinitionsText = modelDefinitionsFile.GetText(context.CancellationToken);
            _modelLines = modelDefinitionsText.Lines;
            _modelDefinitionsPath = modelDefinitionsFile.Path;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                    modelDefinitionsText.Write(writer);
                stream.Seek(0L, SeekOrigin.Begin);
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    CheckCharacters = false,
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    Schemas = new XmlSchemaSet(),
                    ValidationType = ValidationType.Schema,
                    ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.ReportValidationWarnings
                };

                settings.Schemas.Add(ExtensionMethods.ModelNamespaceURI, Path.Combine(Path.GetDirectoryName(typeof(ModelGenerator).Assembly.Location), Schema_RelativePath));
                ValidationListener validationListener = new ValidationListener(context, modelDefinitionsText.Lines, modelDefinitionsFile.Path);
                settings.ValidationEventHandler += validationListener.ValidationEventHandler;
                try
                {
                    using (XmlReader reader = XmlReader.Create(stream, settings))
                        _modelDefinitions = XDocument.Load(stream, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                }
                catch (XmlSchemaException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(modelDefinitionsText.Lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(modelDefinitionsFile.Path, textSpan, positionSpan), message ?? "(unexpected validation error)"));
                    return false;
                }
                catch (XmlException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(modelDefinitionsText.Lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_XmlParseError,
                        Location.Create(modelDefinitionsFile.Path, textSpan, positionSpan), message ?? "(unexpected parse error)"));
                    return false;
                }
            }
            if (_modelDefinitions.Root is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError, Location.Create(modelDefinitionsFile.Path, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1))), "File has no root XML element"));
                return false;
            }
            if (_modelDefinitions.Element(XmlNames.ModelDefinitions) is null)
            {
                LinePositionSpan positionSpan = _modelDefinitions.Root.GetPositionSpan(modelDefinitionsText.Lines, out TextSpan textSpan);
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                    Location.Create(modelDefinitionsFile.Path, textSpan, positionSpan), "Invalid root element"));
                return false;
            }
            return true;
        }

        // private void ProcessModelXml(string path, XElement rootElement, Dictionary<string, XElement> allModels)
        // {
        //     throw new NotImplementedException();
        // }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Initialization not required.
        }

        class ValidationListener
        {
            private readonly GeneratorExecutionContext _context;
            private readonly TextLineCollection _lines;
            private readonly string _filePath;

            internal ValidationListener(GeneratorExecutionContext context, TextLineCollection lines, string filePath) => (_context, _lines, _filePath) = (context, lines, filePath);

            internal void ValidationEventHandler(object sender, ValidationEventArgs e)
            {
                LinePositionSpan positionSpan = e.Exception.GetPositionSpan(_lines, out TextSpan textSpan, out string message);
                if (e.Severity == XmlSeverityType.Warning)
                    _context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationWarning, Location.Create(_filePath, textSpan, positionSpan), message ?? "(unspecified validation warning)"));
                else
                    _context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError, Location.Create(_filePath, textSpan, positionSpan), message ?? "(unexpected validation error)"));
            }
        }
    }
}
