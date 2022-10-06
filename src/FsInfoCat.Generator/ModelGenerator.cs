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
using System.Runtime.Serialization;
using System.CodeDom.Compiler;

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

        public static readonly DiagnosticFactory<string> DiagnosticFactory_MissingFileError = new(DiagnosticId.MissingEntityTypesFileError, "Missing Entity Types File", "Entity Types file named '{0}' not found");

        public static readonly WrappedMessageDiagnosticFactory DiagnosticFactory_XmlParseError = new(DiagnosticId.EntityTypesXmlParseError, "Entity Types File Parsing Error",
            "Unexpected error parsing Entity Types file: {0}");

        public static readonly WrappedMessageDiagnosticFactory DiagnosticFactory_SchemaValidationError = new(DiagnosticId.EntityTypesSchemaValidationError, "Entity Types File Validation Error",
            "Entity Types file validation failed: {0}");

        public static readonly WrappedMessageDiagnosticFactory DiagnosticFactory_SchemaValidationWarning = new(DiagnosticId.EntityTypesSchemaValidationWarning, "Entity Types File Validation Warning",
            "Entity Types file validation warning: {0}");

        public static readonly DiagnosticFactory<string> DiagnosticFactory_MissingResourceError = new(DiagnosticId.MissingEntityTypesFileError, "Named Resource Not Found",
            "Could not find resource named \"{0}\".");

        public static readonly DiagnosticFactory<string> DiagnosticFactory_DefinitionElementMissingWarning = new(DiagnosticId.DefinitionElementMissingWarning, "Named Resource Not Found",
            "Element named \"{0}\" is missing.");

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

        public static ModelGenerator Create(GeneratorExecutionContext context, XDocument resourcesXml!!)
        {
            AdditionalText additionalFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_EntityTypesXml, StringComparison.InvariantCultureIgnoreCase));
            if (additionalFile is null)
                throw new FatalDiagosticException(DiagnosticFactory_MissingFileError.Create(FileName_EntityTypesXml));
            SourceText sourceText = additionalFile.GetText(context.CancellationToken);
            TextLineCollection lines = sourceText.Lines;
            string path = additionalFile.Path;
            string text = sourceText.ToString();
            int index = text.IndexOf("<EntityTypes");
            XDocument entityTypes;
            using (StringReader stringReader = new((index > 0) ? text.Substring(index) : text))
            {
                XmlReaderSettings settings = new()
                {
                    CheckCharacters = false,
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    Schemas = new XmlSchemaSet(),
                    ValidationType = ValidationType.Schema,
                    ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.ReportValidationWarnings
                };
                ResourceManager rm = new("FsInfoCat.Generator.Properties.Resources", typeof(Resources).Assembly);
                foreach (string n in new[] { "EntityTypesXsd", "ConstructedTypesXsd", "DocumentationXsd", "EnumDefinitionsXsd", "ExplicitNamesXsd", "SqlStatementsXsd", "TypeNamesXsd" })
                {
                    using StringReader sr = new(rm.GetString(n));
                    using XmlReader reader = XmlReader.Create(sr);
                    settings.Schemas.Add("", reader);
                }
                ValidationListener validationListener = new(context, lines, path);
                settings.ValidationEventHandler += validationListener.ValidationEventHandler;
                try
                {
                    using XmlReader reader = XmlReader.Create(stringReader, settings);
                    entityTypes = XDocument.Load(reader, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                }
                catch (XmlSchemaException exception)
                {
                    throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create(path, exception, lines));
                }
                catch (XmlException exception)
                {
                    throw new FatalDiagosticException(DiagnosticFactory_XmlParseError.Create(path, exception, lines));
                }
                if (validationListener.ErrorWasReported) return null;
            }
            if (entityTypes.Root is null)
                throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create("File has no root XML element", Location.Create(path, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1)))));
            if (entityTypes.Element(XmlNames.EntityTypes) is null)
                throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create(path, entityTypes.Root, lines, "Invalid root element"));
            return new ModelGenerator(entityTypes, path, lines, resourcesXml);
        }

        private string GenerateEnum(IndentedTextWriter writer, XElement enumElement, string codeNamespace, Action<string, IXmlLineInfo> assertResourceName)
        {
            string typeName = enumElement.GetAttributeValue(XmlNames.Name);
            if (!enumElement.TryGetAttributeValue(XmlNames.Flags, out bool flags)) flags = false;
            if (flags) writer.WriteLine("using System;");
            writer.WriteLines("using System.ComponentModel.DataAnnotations;", "", $"namespace {codeNamespace}", "{");
            writer.Indent++;
            writer.WriteDocumentationXml(enumElement.Elements(XmlNames.Documentation).Elements());
            if (flags) writer.WriteLine("[Flags]");
            IEnumerable<XElement> fieldElements = enumElement.Element(XmlNames.Fields).Elements();
            XElement firstField = fieldElements.First();
            XmlNames enumType = (XmlNames)Enum.Parse(typeof(XmlNames), firstField.Name.LocalName);
            writer.Write($"public enum {typeName} : ");
            void writeField(XElement fe)
            {
                writer.WriteLine();
                writer.WriteDocumentationXml(fe.Element(XmlNames.Documentation).Elements());
                string identifier = fe.GetAttributeValue(XmlNames.StatusMessageLevel);
                if (!string.IsNullOrWhiteSpace(identifier))
                    writer.WriteLine($"[StatusMessageLevel(StatusMessageLevel.{identifier})]");
                identifier = fe.GetAttributeValue(XmlNames.ErrorCode);
                if (!string.IsNullOrWhiteSpace(identifier))
                    writer.WriteLine($"[ErrorCode(ErrorCode.{identifier})]");
                identifier = fe.GetAttributeValue(XmlNames.MessageCode);
                if (!string.IsNullOrWhiteSpace(identifier))
                    writer.WriteLine($"[MessageCode(MessageCode.{identifier})]");
                writer.WriteDisplayAttribute(fe, assertResourceName);
                writer.Write($"{fe.GetAttributeValue(XmlNames.Name)} = {fe.GetAttributeValue(XmlNames.Value)}");
            }

            switch (enumType)
            {
                case XmlNames.Byte:
                    writer.WriteLine("byte");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        writeField(fe);
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.SByte:
                    writer.WriteLine("sbyte");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        writeField(fe);
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.Short:
                    writer.WriteLine("short");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        writeField(fe);
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.UShort:
                    writer.WriteLine("ushort");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        writeField(fe);
                    }
                    writer.WriteLine();
                    break;
                case XmlNames.UInt:
                    writer.WriteLine("uint");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine("u,");
                        writeField(fe);
                    }
                    writer.WriteLine("u");
                    break;
                case XmlNames.Long:
                    writer.WriteLine("long");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine("L,");
                        writeField(fe);
                    }
                    writer.WriteLine("L");
                    break;
                case XmlNames.ULong:
                    writer.WriteLine("ulong");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine("UL,");
                        writeField(fe);
                    }
                    writer.WriteLine("UL");
                    break;
                default:
                    writer.WriteLine("int");
                    writer.Write("{");
                    writer.Indent++;
                    writeField(firstField);
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        writer.WriteLine(",");
                        writeField(fe);
                    }
                    writer.WriteLine();
                    break;
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            return typeName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entityElement">AbstractEntityDefinition|GenericEntityDefinition1|GenericEntityDefinition2|TableDefinition</param>
        /// <param name="genericArgumentElements">GenericArgument</param>
        /// <param name="codeNamespace"></param>
        /// <remarks>
        /// /EntityTypes/AbstractEntity|/EntityTypes/GenericEntity1|/EntityTypes/GenericEntity2|/EntityTypes/Local/AbstractEntity|/EntityTypes/Local/Table|/EntityTypes/Upstream/AbstractEntity|/EntityTypes/Upstream/Table
        /// </remarks>
        private string GenerateEntity(IndentedTextWriter writer, XElement entityElement, IEnumerable<XElement> genericArgumentElements, string codeNamespace, Action<Diagnostic> reportDiagnostic, Action<string, IXmlLineInfo> assertResourceName)
        {
            string typeName = entityElement.GetAttributeValue(XmlNames.Name);
            // TODO: Need a better way to determine what using statements are needed.
            if (entityElement.Elements(XmlNames.Columns).Elements().Any())
            {
                if (entityElement.Elements(XmlNames.Columns).Elements(XmlNames.DateTime).Any() || entityElement.Elements(XmlNames.Columns).Elements(XmlNames.Guid).Any())
                    writer.WriteLine("using System;");
                writer.WriteLine("using System.ComponentModel.DataAnnotations;");
                if (entityElement.Elements(XmlNames.Columns).Elements(XmlNames.DriveType).Any())
                    writer.WriteLine("using System.IO;");
            }
            writer.WriteLines("", $"namespace {codeNamespace}", "{");
            writer.Indent++;
            writer.WriteDocumentationXml(entityElement.Elements(XmlNames.Documentation).Elements());
            throw new NotImplementedException();
        }

        [Obsolete("Use GenerateEntity(IndentedTextWriter, XElement, IEnumerable<XElement>, string, Action<Diagnostic>), instead")]
        private string GenerateEntity(CodeBuilder codeBuilder, XElement entityElement, IEnumerable<XElement> genericArgumentElements, string codeNamespace, Action<Diagnostic> reportDiagnostic,
            Func<XElement, (string Label, string ShortName, string Description)> getDisplayAttribute)
        {
            CodeBuilder interfaceBuilder = new CodeBuilder(1).Append("    public interface ");
            string interfaceName = entityElement.GetAttributeValue(XmlNames.Name);
            if (genericArgumentElements is not null)
            {
                int index = interfaceName.IndexOf('`');
                string n = interfaceName.Substring(0, index);
                interfaceName = n + interfaceName.Substring(index + 1);
                interfaceBuilder.Append(n).AppendJoined(genericArgumentElements, XmlNames.Name, ",", "<", ">");
            }
            else
                interfaceBuilder.Append(interfaceName);
            using (IEnumerator<XElement> implementsEnumerator = entityElement.Elements(XmlNames.Implements).Elements().GetEnumerator())
            {
                if (implementsEnumerator.MoveNext())
                {
                    XElement firstImplements = implementsEnumerator.Current;
                    if (implementsEnumerator.MoveNext())
                    {
                        CodeBuilder implementsBuilder = new(2);
                        interfaceBuilder.Append(implementsBuilder);
                        implementsBuilder.Append(" : ").AppendTypeString(firstImplements);
                        XElement lastImplements = implementsEnumerator.Current;
                        while (implementsEnumerator.MoveNext())
                        {
                            implementsBuilder.AppendLine(",").AppendTypeString(lastImplements);
                            lastImplements = implementsEnumerator.Current;
                        }
                        implementsBuilder.AppendLine(",").AppendTypeString(lastImplements, true);
                    }
                    else
                        interfaceBuilder.Append(" : ").AppendTypeString(firstImplements, true);
                }
            }
            codeBuilder.AppendLine("using System;")
                .AppendLine()
                .Append("namespace ").AppendLine(codeNamespace).AppendLine("{").Append(interfaceBuilder).AppendLine("}");

            throw new NotImplementedException();
        }

        internal void GenerateCode(GeneratorExecutionContext context)
        {
            void assertResourceName(string name, IXmlLineInfo lineInfo)
            {
                if (!_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == name))
                    throw new FatalDiagosticException(DiagnosticFactory_MissingResourceError.Create(_path, lineInfo, _lines, $"Could not find resource named \"{name}\"."));
            }
            foreach (var (element, codeNamespace) in EntityTypesXml.Root.Elements(XmlNames.Enum).Select(e =>
                (e, CsNamespace_Core)
            ).Concat(EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.Enum).Select(e =>
                (e, CsNamespace_Upstream))
            ))
            {
                using StringWriter underlyingWriter = new();
                using IndentedTextWriter writer = new(underlyingWriter, "    ");
                string name = GenerateEnum(writer, element, codeNamespace, assertResourceName);
                writer.Flush();
                var sourceText = SourceText.From(underlyingWriter.ToString(), Encoding.UTF8);
                context.AddSource($"{name}-generated.cs", sourceText);
            }

            // foreach (var (element, genericArguments, codeNamespace) in EntityTypesXml.Root.Elements(XmlNames.AbstractEntity).Select(e =>
            //         (e, (IEnumerable<XElement>)null, CsNamespace_Core)
            //     ).Concat(EntityTypesXml.Root.Elements(XmlNames.Local).Elements(XmlNames.AbstractEntity).Concat(EntityTypesXml.Root.Elements(XmlNames.Local).Elements(XmlNames.Table)).Select(e =>
            //         (e, (IEnumerable<XElement>)null, CsNamespace_Local)
            //     )).Concat(EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.AbstractEntity).Concat(EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.Table)).Select(e =>
            //         (e, (IEnumerable<XElement>)null, CsNamespace_Upstream)
            //     )).Concat(EntityTypesXml.Root.Elements(XmlNames.GenericEntity1).Concat(EntityTypesXml.Root.Elements(XmlNames.GenericEntity1)).Select(e =>
            //     (e, e.Elements(XmlNames.GenericArguments).Elements(), CsNamespace_Core)
            // )))
            // {
            //     CodeBuilder codeBuilder = new();
            //     string name = GenerateEntity(codeBuilder, element, genericArguments, codeNamespace, context.ReportDiagnostic, getDisplayAttribute);
            //     var sourceText = SourceText.From(codeBuilder.ToString(), Encoding.UTF8);
            //     context.AddSource($"{name}-generated.cs", sourceText);
            // }
        }

        class ValidationListener
        {
            private readonly GeneratorExecutionContext _context;
            private readonly TextLineCollection _lines;
            private readonly string _path;

            public bool ErrorWasReported { get; set; }

            internal ValidationListener(GeneratorExecutionContext context, TextLineCollection lines, string path) => (_context, _lines, _path) = (context, lines, path);

            internal void ValidationEventHandler(object sender, ValidationEventArgs e)
            {
                if (e.Severity == XmlSeverityType.Warning)
                    _context.ReportDiagnostic(DiagnosticFactory_SchemaValidationWarning.Create(_path, e.Exception, _lines));
                else
                {
                    ErrorWasReported = true;
                    _context.ReportDiagnostic(DiagnosticFactory_SchemaValidationError.Create(_path, e.Exception, _lines));
                }
            }
        }
    }
}
