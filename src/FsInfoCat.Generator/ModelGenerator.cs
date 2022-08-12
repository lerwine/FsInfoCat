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
using System.Text;
using System.Resources;

namespace FsInfoCat.Generator
{
    public class ModelGenerator
    {
        public const string FileName_EntityTypesXml = "EntityTypes.xml";

        public const string ScopeName_All = "All";

        public const string CsNamespace_Core = "FsInfoCat.Model";

        public const string CsNamespace_Local = "FsInfoCat.Local.Model";

        public const string CsNamespace_Upstream = "FsInfoCat.Upstream.Model";

        public const string ScopeName_Local = "Local";

        public const string ScopeName_Upstream = "Upstream";

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_MissingEntityTypesFileError =
            new DiagnosticDescriptor(
                SourceGenerator.DiagnosticID_MissingEntityTypesFileError,
                "Missing EntityTypes.xml file",
                "Resources file error: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_SchemaValidationWarning =
            new DiagnosticDescriptor(
                SourceGenerator.DiagnosticID_SchemaValidationWarning,
                "XML Parsing Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_MissingResourceError =
            new DiagnosticDescriptor(
                SourceGenerator.DiagnosticID_MissingResourceError,
                "Missing resources file",
                "Resources file error: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public XDocument EntityTypesXml { get; }

        private readonly TextLineCollection _lines;
        private readonly string _path;
        private readonly XDocument _resourcesXml;

        private ModelGenerator(XDocument entityTypes, string path, TextLineCollection lines, XDocument resourcesXml)
        {
            EntityTypesXml = entityTypes;
            _path = path;
            _lines = lines;
            _resourcesXml = resourcesXml;
        }

        public static ModelGenerator Create(GeneratorExecutionContext context, XDocument resourcesXml)
        {
            AdditionalText additionalFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_EntityTypesXml, StringComparison.InvariantCultureIgnoreCase));
            if (additionalFile is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_MissingEntityTypesFileError, null, "Entity types file not found."));
                return null;
            }
            SourceText sourceText = additionalFile.GetText(context.CancellationToken);
            TextLineCollection lines = sourceText.Lines;
            string path = additionalFile.Path;
            string text = sourceText.ToString();
            int index = text.IndexOf("<EntityTypes");
            XDocument entityTypes;
            using (StringReader stringReader = new StringReader((index > 0) ? text.Substring(index) : text))
            {
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    CheckCharacters = false,
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    Schemas = new XmlSchemaSet(),
                    ValidationType = ValidationType.Schema,
                    ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.ReportValidationWarnings
                };
                ResourceManager rm = new ResourceManager("FsInfoCat.Generator.Properties.Resources", typeof(Resources).Assembly);
                foreach (string n in new[] { "EntityTypesXsd", "ConstructedTypesXsd", "DocumentationXsd", "EnumDefinitionsXsd", "ExplicitNamesXsd", "InterfaceDefinitionsXsd", "SqlStatementsXsd", "TypeNamesXsd" })
                {
                    using (StringReader sr = new StringReader(rm.GetString(n)))
                        using (XmlReader reader = XmlReader.Create(sr))
                            settings.Schemas.Add("", reader);
                }
                ValidationListener validationListener = new ValidationListener(context, lines, path);
                settings.ValidationEventHandler += validationListener.ValidationEventHandler;
                try
                {
                    using (XmlReader reader = XmlReader.Create(stringReader, settings))
                        entityTypes = XDocument.Load(reader, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                }
                catch (XmlSchemaException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(SourceGenerator.DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(path, textSpan, positionSpan), message ?? "(unexpected validation error)"));
                    return null;
                }
                catch (XmlException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(SourceGenerator.DiagnosticDescriptor_XmlParseError,
                        Location.Create(path, textSpan, positionSpan), message ?? "(unexpected parse error)"));
                    return null;
                }
            }
            if (entityTypes.Root is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(SourceGenerator.DiagnosticDescriptor_SchemaValidationError, Location.Create(path, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1))), "File has no root XML element"));
                return null;
            }
            if (entityTypes.Element(XmlNames.EntityTypes) is null)
            {
                LinePositionSpan positionSpan = entityTypes.Root.GetPositionSpan(lines, out TextSpan textSpan);
                context.ReportDiagnostic(Diagnostic.Create(SourceGenerator.DiagnosticDescriptor_SchemaValidationError,
                    Location.Create(path, textSpan, positionSpan), "Invalid root element"));
                return null;
            }
            return new ModelGenerator(entityTypes, path, lines, resourcesXml);
        }

        internal void GenerateCode(GeneratorExecutionContext context)
        {
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Enum))
                if (!GenerateEnum(context, element, CsNamespace_Core)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Local).Elements(XmlNames.Enum))
                if (!GenerateEnum(context, element, CsNamespace_Local)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.Enum))
                if (!GenerateEnum(context, element, CsNamespace_Upstream)) return;
        }

        private void WriteCommentDocumentation(XElement documentElement, int indentLevel, StringWriter writer)
        {
            string indent = (indentLevel > 0) ? new string(' ', indentLevel * 4) + "/// " : "/// ";
            foreach (string line in documentElement.Elements().SelectMany(e => SourceGenerator.NewlineRegex.Split(e.ToString())))
            {
                writer.Write(indent);
                writer.WriteLine(line);
            }
        }

        private bool TryWriteDisplayAttribute(GeneratorExecutionContext context, XElement displayElement, StringWriter writer)
        {
            if (displayElement is null) throw new ArgumentNullException(nameof(displayElement));
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            string label = displayElement.GetAttributeValue(XmlNames.Label);
            if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == label))
            {
                writer.Write("        [Display(Name = nameof(Properties.Resources.");
                writer.Write(label);
                string shortName = displayElement.GetAttributeValue(XmlNames.ShortName);
                string description = displayElement.GetAttributeValue(XmlNames.Description);
                if (string.IsNullOrWhiteSpace(shortName))
                {
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        writer.WriteLine("), ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == description))
                    {
                        writer.Write("), Description = nameof(Properties.Resources.");
                        writer.Write(description);
                        writer.WriteLine("),");
                        writer.WriteLine("            ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    label = description;
                }
                else if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == shortName))
                {
                    writer.Write("), ShortName = nameof(Properties.Resources.");
                    writer.Write(shortName);
                    writer.WriteLine("),");
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        writer.WriteLine("            ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == description))
                    {
                        writer.Write("            Description = nameof(Properties.Resources.");
                        writer.Write(description);
                        writer.WriteLine("), ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    label = description;
                }
                else
                    label = shortName;
            }
            LinePositionSpan positionSpan = displayElement.GetPositionSpan(_lines, out TextSpan textSpan);
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_MissingResourceError, Location.Create(_path, textSpan, positionSpan), $"Could not find resource named \"{label}\"."));
            return false;
        }

        private bool GenerateEnum(GeneratorExecutionContext context, XElement entityDefinitionElement, string @namespace)
        {
            using StringWriter writer = new StringWriter();
            if (!entityDefinitionElement.TryGetAttributeValue(XmlNames.Flags, out bool flags)) flags = false;
            IEnumerable<XElement> fieldElements = entityDefinitionElement.Element(XmlNames.Fields).Elements();
            XElement firstField = fieldElements.First();
            XmlNames enumType = (XmlNames)Enum.Parse(typeof(XmlNames), firstField.Name.LocalName);

            fieldElements.Attributes(XmlNames.StatusMessageLevel).Any();
            if (flags) writer.WriteLine("using System;");
            writer.WriteLine("using System.ComponentModel.DataAnnotations;");
            writer.Write("namespace ");
            writer.WriteLine(@namespace);
            writer.WriteLine("{");
            WriteCommentDocumentation(entityDefinitionElement.Element(XmlNames.Documentation), 1, writer);
            if (flags) writer.WriteLine("    [Flags]");
            writer.Write("    public enum ");
            string enumName = entityDefinitionElement.GetAttributeValue(XmlNames.Name);
            writer.Write(enumName);
            writer.Write(" : ");
            bool writeField(XElement fe)
            {
                writer.WriteLine();
                WriteCommentDocumentation(fe.Element(XmlNames.Documentation), 2, writer);
                string label = fe.GetAttributeValue(XmlNames.StatusMessageLevel);
                if (!string.IsNullOrWhiteSpace(label))
                {
                    writer.Write("        [StatusMessageLevel(StatusMessageLevel.");
                    writer.Write(label);
                    writer.WriteLine(")]");
                }
                label = fe.GetAttributeValue(XmlNames.ErrorCode);
                if (!string.IsNullOrWhiteSpace(label))
                {
                    writer.Write("        [ErrorCode(ErrorCode.");
                    writer.Write(label);
                    writer.WriteLine(")]");
                }
                label = fe.GetAttributeValue(XmlNames.MessageCode);
                if (!string.IsNullOrWhiteSpace(label))
                {
                    writer.Write("        [MessageCode(MessageCode.");
                    writer.Write(label);
                    writer.WriteLine(")]");
                }
                if (!TryWriteDisplayAttribute(context, fe.Element(XmlNames.Display), writer)) return false;
                writer.Write("        ");
                writer.Write(fe.GetAttributeValue(XmlNames.Name));
                writer.Write(" = ");
                writer.Write(fe.GetAttributeValue(XmlNames.Value));
                return true;
            }
            switch (enumType)
            {
                case XmlNames.Byte:
                    writer.WriteLine("byte");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.SByte:
                    writer.WriteLine("sbyte");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.Short:
                    writer.WriteLine("short");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.UShort:
                    writer.WriteLine("ushort");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.UInt:
                    writer.WriteLine("uint");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine("u,");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine("u");
                    break;
                case XmlNames.Long:
                    writer.WriteLine("long");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine("L,");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine("L");
                    break;
                case XmlNames.ULong:
                    writer.WriteLine("ulong");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine("UL,");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine("UL");
                    break;
                default:
                    writer.WriteLine("int");
                    writer.Write("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        if (!writeField(fe)) return false;
                    }
                    writer.WriteLine();
                    break;
            }
            writer.WriteLine("    }");
            writer.WriteLine("}");
            var sourceText = SourceText.From(writer.ToString(), Encoding.UTF8);
            context.AddSource($"{enumName}-generated.cs", sourceText);
            return true;
        }

        class ValidationListener
        {
            private readonly GeneratorExecutionContext _context;
            private readonly TextLineCollection _lines;
            private readonly string _path;

            internal ValidationListener(GeneratorExecutionContext context, TextLineCollection lines, string path) => (_context, _lines, _path) = (context, lines, path);

            internal void ValidationEventHandler(object sender, ValidationEventArgs e)
            {
                LinePositionSpan positionSpan = e.Exception.GetPositionSpan(_lines, out TextSpan textSpan, out string message);
                if (e.Severity == XmlSeverityType.Warning)
                    _context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationWarning, Location.Create(_path, textSpan, positionSpan), message ?? "(unspecified validation warning)"));
                else
                    _context.ReportDiagnostic(Diagnostic.Create(SourceGenerator.DiagnosticDescriptor_SchemaValidationError, Location.Create(_path, textSpan, positionSpan), message ?? "(unexpected validation error)"));
            }
        }
    }
}
