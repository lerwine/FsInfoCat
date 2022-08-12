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
using System.Text.RegularExpressions;
using System.Text;
using System.Resources;

namespace FsInfoCat.Generator
{
    [Generator]
    public class ModelGenerator : ISourceGenerator
    {
        // public const string Schema_RelativePath = "XmlSchemas\\Model\\EntityTypes.xsd";

        public const string FileName_EntityTypesXml = "EntityTypes.xml";

        public const string FileName_Resources = "Resources.resx";

        public const string DiagnosticID_MissingResourcesFileError = "MD0001";

        public const string DiagnosticID_XmlParseError = "MD0002";

        public const string DiagnosticID_SchemaValidationError = "MD0003";

        public const string DiagnosticID_ResourceTypeNotSupported = "MD0004";

        public const string DiagnosticID_SchemaValidationWarning = "MD0005";

        public const string ScopeName_All = "All";

        public const string CsNamespace_Core = "FsInfoCat.Model";

        public const string CsNamespace_Local = "FsInfoCat.Local.Model";

        public const string CsNamespace_Upstream = "FsInfoCat.Upstream.Model";

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

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_ResourceTypeNotSupported =
            new DiagnosticDescriptor(
                DiagnosticID_ResourceTypeNotSupported,
                "Resource Type Not Supported",
                "Resource Validation Error: {0}",
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

        private static readonly Regex NewlineRegex = new Regex(@"\r\n|\n", RegexOptions.Compiled);

        private XDocument _entityTypes;
        private XDocument _resourceDefinitions;
        private TextLineCollection _resourcesLines;
        private TextLineCollection _entityTypesLines;
        private string _resourcesPath;
        private string _entityTypesPath;

        public void Execute(GeneratorExecutionContext context)
        {
            // LoadModelDefinitions(context);
            if (LoadModelDefinitions(context))
            {
                if (!GenerateResourcesClass(context)) return;
                foreach (XElement element in _entityTypes.Root.Elements(XmlNames.Enum))
                    if (!GenerateEnum(context, element, CsNamespace_Core)) return;
                foreach (XElement element in _entityTypes.Root.Elements(XmlNames.Local).Elements(XmlNames.Enum))
                    if (!GenerateEnum(context, element, CsNamespace_Local)) return;
                foreach (XElement element in _entityTypes.Root.Elements(XmlNames.Upstream).Elements(XmlNames.Enum))
                    if (!GenerateEnum(context, element, CsNamespace_Upstream)) return;
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Initialization not required.
        }

        private bool LoadModelDefinitions(GeneratorExecutionContext context)
        {
            AdditionalText entityTypesFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_EntityTypesXml, StringComparison.InvariantCultureIgnoreCase));
            AdditionalText resourcesFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_Resources, StringComparison.InvariantCultureIgnoreCase));
            if (entityTypesFile is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_MissingResourcesFileError, null, "Entity types file not found."));
                return false;
            }
            if (resourcesFile is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_MissingResourcesFileError, null, "Resources file not found."));
                return false;
            }
            _resourcesPath = resourcesFile.Path;
            SourceText resourcesText = resourcesFile.GetText(context.CancellationToken);
            _resourcesLines = resourcesText.Lines;
            try
            {
                _resourceDefinitions = XDocument.Parse(resourcesText.ToString(), LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
            }
            catch (XmlSchemaException exception)
            {
                LinePositionSpan positionSpan = exception.GetPositionSpan(_resourcesLines, out TextSpan textSpan, out string message);
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                    Location.Create(_resourcesPath, textSpan, positionSpan), message ?? "(unexpected validation error)"));
                return false;
            }
            catch (XmlException exception)
            {
                LinePositionSpan positionSpan = exception.GetPositionSpan(_resourcesLines, out TextSpan textSpan, out string message);
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_XmlParseError,
                    Location.Create(_resourcesPath, textSpan, positionSpan), message ?? "(unexpected parse error)"));
                return false;
            }
            if (_resourceDefinitions.Root is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError, Location.Create(_resourcesPath, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1))), "File has no root XML element"));
                return false;
            }

            SourceText entityDefinitionsText = entityTypesFile.GetText(context.CancellationToken);
            _entityTypesLines = entityDefinitionsText.Lines;
            _entityTypesPath = entityTypesFile.Path;
            string text = entityDefinitionsText.ToString();
            int index = text.IndexOf("<EntityTypes");
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
                ValidationListener validationListener = new ValidationListener(context, entityDefinitionsText.Lines, entityTypesFile.Path);
                settings.ValidationEventHandler += validationListener.ValidationEventHandler;
                try
                {
                    using (XmlReader reader = XmlReader.Create(stringReader, settings))
                        _entityTypes = XDocument.Load(reader, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                }
                catch (XmlSchemaException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(entityDefinitionsText.Lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(entityTypesFile.Path, textSpan, positionSpan), message ?? "(unexpected validation error)"));
                    return false;
                }
                catch (XmlException exception)
                {
                    LinePositionSpan positionSpan = exception.GetPositionSpan(entityDefinitionsText.Lines, out TextSpan textSpan, out string message);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_XmlParseError,
                        Location.Create(entityTypesFile.Path, textSpan, positionSpan), message ?? "(unexpected parse error)"));
                    return false;
                }
            }
            if (_entityTypes.Root is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError, Location.Create(entityTypesFile.Path, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1))), "File has no root XML element"));
                return false;
            }
            if (_entityTypes.Element(XmlNames.EntityTypes) is null)
            {
                LinePositionSpan positionSpan = _entityTypes.Root.GetPositionSpan(entityDefinitionsText.Lines, out TextSpan textSpan);
                context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                    Location.Create(entityTypesFile.Path, textSpan, positionSpan), "Invalid root element"));
                return false;
            }
            return true;
        }

        public const string TypeName_ResXFileRef = "System.Resources.ResXFileRef, System.Windows.Forms";

        private bool GenerateResourcesClass(GeneratorExecutionContext context)
        {
            StringBuilder sourceCode = new StringBuilder("namespace FsInfoCat.Properties")
                .AppendLine("{")
                .AppendLine("    using System.ComponentModel;")
                .AppendLine("    using System.Globalization;")
                .AppendLine("    using System.Resources;")
                .AppendLine("    /// <summary>")
                .AppendLine("    /// A strongly-typed resource class for looking up localized strings, etc.")
                .AppendLine("    /// </summary>")
                .AppendLine("    [System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.Resources.Tools.StronglyTypedResourceBuilder\", \"16.0.0.0\")]")
                .AppendLine("    [System.Diagnostics.DebuggerNonUserCodeAttribute()]")
                .AppendLine("    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]")
                .AppendLine("    public class Resources")
                .AppendLine("    {")
                .AppendLine("        private static ResourceManager _resourceManager;")
                .AppendLine("        private static CultureInfo _resourceCulture;")
                .AppendLine("        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(\"Microsoft.Performance\", \"CA1811:AvoidUncalledPrivateCode\")]")
                .AppendLine("        internal Resources() { }")
                .AppendLine("        /// <summary>")
                .AppendLine("        ///   Returns the cached ResourceManager instance used by this class.")
                .AppendLine("        /// </summary>")
                .AppendLine("        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]")
                .AppendLine("        public static ResourceManager ResourceManager")
                .AppendLine("        {")
                .AppendLine("            get")
                .AppendLine("            {")
                .AppendLine("                if (object.ReferenceEquals(_resourceManager, null))")
                .AppendLine("                {")
                .AppendLine("                    ResourceManager temp = new ResourceManager(\"FsInfoCat.Properties.Resources\", typeof(Resources).Assembly);")
                .AppendLine("                    _resourceManager = temp;")
                .AppendLine("                }")
                .AppendLine("                return _resourceManager;")
                .AppendLine("            }")
                .AppendLine("        }")
                .AppendLine("        /// <summary>")
                .AppendLine("        ///   Overrides the current thread's CurrentUICulture property for all")
                .AppendLine("        ///   resource lookups using this strongly typed resource class.")
                .AppendLine("        /// </summary>")
                .AppendLine("        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]")
                .AppendLine("        public static CultureInfo Culture")
                .AppendLine("        {")
                .AppendLine("            get")
                .AppendLine("            {")
                .AppendLine("                return _resourceCulture;")
                .AppendLine("            }")
                .AppendLine("            set")
                .AppendLine("            {")
                .AppendLine("                _resourceCulture = value;")
                .AppendLine("            }")
                .AppendLine("        }");
            XName valueName = XNamespace.None.GetName("value");
            XName typeName = XNamespace.None.GetName("type");
            XName nameName = XNamespace.None.GetName("name");
            XName mimetypeName = XNamespace.None.GetName("mimetype");
            XName commentName = XNamespace.None.GetName("comment");
            string resourceRoot = Path.GetDirectoryName(_resourcesPath);
            foreach (XElement resourceElement in _resourceDefinitions.Root.Elements(XNamespace.None.GetName("data")))
            {
                XAttribute attribute = resourceElement.Attribute(mimetypeName);
                if (attribute is not null)
                {
                    LinePositionSpan positionSpan = attribute.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_ResourceTypeNotSupported,
                        Location.Create(_resourcesPath, textSpan, positionSpan), "Attribute 'mimetype' is not supported"));
                    return false;
                }
                attribute = resourceElement.Attribute(nameName);
                if (attribute is null)
                {
                    LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(_resourcesPath, textSpan, positionSpan), "Name attribute missing"));
                    return false;
                }
                XElement element = resourceElement.Element(valueName);
                if (element is null)
                {
                    LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(_resourcesPath, textSpan, positionSpan), "Value element missing"));
                    return false;
                }
                if (element.IsEmpty)
                {
                    LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                        Location.Create(_resourcesPath, textSpan, positionSpan), "Value element is nil"));
                    return false;
                }
                string name = attribute.Value;
                string comment = resourceElement.Element(commentName)?.Value;
                if (string.IsNullOrWhiteSpace(comment))
                {
                    string value = element.Value.Trim();
                    if ((attribute = resourceElement.Attribute(typeName)) is not null)
                    {
                        if (attribute.Value != TypeName_ResXFileRef)
                        {
                            LinePositionSpan positionSpan = attribute.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_ResourceTypeNotSupported,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"Type \"{attribute.Value}\" is not supported."));
                            return false;
                        }
                        string[] parts = value.Split(';');
                        if (parts.Length != 3)
                        {
                            LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_ResourceTypeNotSupported,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"ResXFileRef format \"{value}\" not supported."));
                            return false;
                        }
                        Type type;
                        try { type = Type.GetType(parts[1]); }
                        catch (Exception exc)
                        {
                            LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_ResourceTypeNotSupported,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"Error looking up type \"{parts[1]}\": {exc.Message}."));
                            return false;
                        }
                        if (type != typeof(string))
                        {
                            LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_ResourceTypeNotSupported,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"Resource type \"{type.FullName}\" not supported."));
                            return false;
                        }
                        Encoding encoding;
                        try { encoding = CodePagesEncodingProvider.Instance.GetEncoding(parts[2]); }
                        catch (Exception exc)
                        {
                            LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_ResourceTypeNotSupported,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"Error looking up encoding \"{parts[2]}\": {exc.Message}."));
                            return false;
                        }

                        string path;
                        try { path = Path.GetFullPath(Path.Combine(resourceRoot, parts[0])); }
                        catch (Exception exc)
                        {
                            LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"Error validating path \"{parts[0]}\": {exc.Message}."));
                            return false;
                        }

                        try { value = File.ReadAllText(path, encoding).Trim(); }
                        catch (Exception exc)
                        {
                            LinePositionSpan positionSpan = resourceElement.GetPositionSpan(_resourcesLines, out TextSpan textSpan);
                            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_SchemaValidationError,
                                Location.Create(_resourcesPath, textSpan, positionSpan), $"Error reading from path \"{path}\": {exc.Message}."));
                            return false;
                        }
                    }
                    comment = (value.Length == 0) ? "Looks up a localized string similar to an empty string" :
                        (value.Length > 512) ? $"Looks up a localized string similar to {value.Substring(0, 512)} [rest of string was truncated]." : $"Looks up a localized string similar to {value}.";
                }
                foreach (string line in NewlineRegex.Split(new XElement(XNamespace.None.GetName("summary"), new XText($"\n{comment}\n")).ToString()).Select(s => s.TrimEnd()))
                {
                    if (line.Length > 0)
                        sourceCode.Append("        /// ").AppendLine(line);
                    else
                        sourceCode.AppendLine("        ///");
                }
                sourceCode.Append("        public static string ").Append(name).Append(" => ResourceManager.GetString(\"").Append(name).AppendLine("\", _resourceCulture);");
            }
            var sourceText = SourceText.From(sourceCode.AppendLine("    }").AppendLine("}").ToString(), Encoding.UTF8);
            context.AddSource("Resources-generated.cs", sourceText);
            return true;
        }

        private void WriteCommentDocumentation(XElement documentElement, int indentLevel, StringWriter writer)
        {
            string indent = (indentLevel > 0) ? new string(' ', indentLevel * 4) + "/// " : "/// ";
            foreach (string line in documentElement.Elements().SelectMany(e => NewlineRegex.Split(e.ToString())))
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
            if (_resourceDefinitions.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == label))
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
                    if (_resourceDefinitions.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == description))
                    {
                        writer.Write("), Description = nameof(Properties.Resources.");
                        writer.Write(description);
                        writer.WriteLine("),");
                        writer.WriteLine("            ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    label = description;
                }
                else if (_resourceDefinitions.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == shortName))
                {
                    writer.Write("), ShortName = nameof(Properties.Resources.");
                    writer.Write(shortName);
                    writer.WriteLine("),");
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        writer.WriteLine("            ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    if (_resourceDefinitions.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == description))
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
            LinePositionSpan positionSpan = displayElement.GetPositionSpan(_entityTypesLines, out TextSpan textSpan);
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptor_MissingResourcesFileError, Location.Create(_entityTypesPath, textSpan, positionSpan), $"Could not find resource named \"{label}\"."));
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
