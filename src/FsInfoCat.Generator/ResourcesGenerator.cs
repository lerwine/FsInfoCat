using Microsoft.CodeAnalysis;
using System.Linq;
using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Text;
using System.Xml;
using System.Text;

namespace FsInfoCat.Generator
{
    public class ResourcesGenerator
    {
        public const string FileName_Resources = "Resources.resx";

        public const string TypeName_ResXFileRef = "System.Resources.ResXFileRef, System.Windows.Forms";

        private static readonly DiagnosticFactory<string> DiagnosticFactory_MissingFileError = new(DiagnosticId.MissingResourcesFileError, "Missing Resources File", "Resources file named '{0}' not found");

        private static readonly WrappedMessageDiagnosticFactory DiagnosticFactory_XmlParseError = new(DiagnosticId.ResxXmlParseError, "Resources File Parsing Error", "Unexpected error parsing Resources file: {0}");

        private static readonly WrappedMessageDiagnosticFactory DiagnosticFactory_SchemaValidationError = new(DiagnosticId.ResxSchemaValidationError, "Resources File Validation Error",
            "Resources file validation failed: {0}");

        private static readonly DiagnosticFactory<string, string> DiagnosticFactory_ResourceTypeNotSupported = new(DiagnosticId.MissingResourcesFileError, "Resource Type Not Supported", "{0} {1} not supported");

        private static readonly DiagnosticFactory<string, string, string> DiagnosticFactory_ReferencingError = new(DiagnosticId.ResxReferencingError, "Resources Value Lookup Error",
            "Error {type} \"{value}\": {exc.Message}");

        public XDocument Xml { get; }
        private readonly TextLineCollection _lines;
        private readonly string _path;

        private ResourcesGenerator(XDocument xml, string path, TextLineCollection lines)
        {
            Xml = xml;
            _path = path;
            _lines = lines;
        }

        public static ResourcesGenerator Create(GeneratorExecutionContext context)
        {
            AdditionalText resourcesFile = context.AdditionalFiles.FirstOrDefault(at => Path.GetFileName(at.Path).Equals(FileName_Resources, StringComparison.InvariantCultureIgnoreCase));
            if (resourcesFile is null)
                throw new FatalDiagosticException(DiagnosticFactory_MissingFileError.Create(FileName_Resources));
            string resourcesPath = resourcesFile.Path;
            SourceText resourcesText = resourcesFile.GetText(context.CancellationToken);
            TextLineCollection resourcesLines = resourcesText.Lines;
            XDocument resourceDefinitions;
            try
            {
                resourceDefinitions = XDocument.Parse(resourcesText.ToString(), LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
            }
            catch (XmlException exception)
            {
                throw new FatalDiagosticException(DiagnosticFactory_XmlParseError.Create(resourcesPath, exception, resourcesLines));
            }
            if (resourceDefinitions.Root is null)
                throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create("File has no root XML element", Location.Create(resourcesPath, TextSpan.FromBounds(0, 1),
                    new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1)))));

            return new ResourcesGenerator(resourceDefinitions, resourcesPath, resourcesLines);
        }

        public void GenerateCode(GeneratorExecutionContext context)
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
            string resourceRoot = Path.GetDirectoryName(_path);
            foreach (XElement resourceElement in Xml.Root.Elements(XNamespace.None.GetName("data")))
            {
                XAttribute attribute = resourceElement.Attribute(mimetypeName);
                if (attribute is not null)
                    throw new FatalDiagosticException(DiagnosticFactory_ResourceTypeNotSupported.Create(_path, resourceElement, _lines, "Attribute", "mimetype"));
                attribute = resourceElement.Attribute(nameName);
                if (attribute is null)
                    throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create(_path, resourceElement, _lines, "Name attribute missing"));
                XElement element = resourceElement.Element(valueName);
                if (element is null)
                    throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create(_path, resourceElement, _lines, "Value element missing"));
                if (element.IsEmpty)
                    throw new FatalDiagosticException(DiagnosticFactory_SchemaValidationError.Create(_path, element, _lines, "Value element is nil"));
                string name = attribute.Value;
                string comment = resourceElement.Element(commentName)?.Value;
                if (string.IsNullOrWhiteSpace(comment))
                {
                    string value = element.Value.Trim();
                    if ((attribute = resourceElement.Attribute(typeName)) is not null)
                    {
                        if (attribute.Value != TypeName_ResXFileRef)
                            throw new FatalDiagosticException(DiagnosticFactory_ResourceTypeNotSupported.Create(_path, attribute, _lines, "Type", attribute.Value));
                        string[] parts = value.Split(';');
                        if (parts.Length != 3)
                            throw new FatalDiagosticException(DiagnosticFactory_ResourceTypeNotSupported.Create(_path, resourceElement, _lines, "ResXFileRef format", value));
                        Type type;
                        try { type = Type.GetType(parts[1]); }
                        catch (Exception exc)
                        {
                            throw new FatalDiagosticException(DiagnosticFactory_ReferencingError.Create(_path, element, _lines, "looking up type", parts[1], exc.Message));
                        }
                        if (type != typeof(string))
                            throw new FatalDiagosticException(DiagnosticFactory_ResourceTypeNotSupported.Create(_path, resourceElement, _lines, "type", type.FullName));
                        Encoding encoding;
                        try { encoding = CodePagesEncodingProvider.Instance.GetEncoding(parts[2]); }
                        catch (Exception exc)
                        {
                            throw new FatalDiagosticException(DiagnosticFactory_ReferencingError.Create(_path, element, _lines, "looking up encoding", parts[2], exc.Message));
                        }

                        string path;
                        try { path = Path.GetFullPath(Path.Combine(resourceRoot, parts[0])); }
                        catch (Exception exc)
                        {
                            throw new FatalDiagosticException(DiagnosticFactory_ReferencingError.Create(_path, resourceElement, _lines, "validating path", parts[0], exc.Message));
                        }

                        try { value = File.ReadAllText(path, encoding).Trim(); }
                        catch (Exception exc)
                        {
                            throw new FatalDiagosticException(DiagnosticFactory_ReferencingError.Create(_path, resourceElement, _lines, "reading from path", path, exc.Message));
                        }
                    }
                    comment = (value.Length == 0) ? "Looks up a localized string similar to an empty string" :
                        (value.Length > 512) ? $"Looks up a localized string similar to {value.Substring(0, 512)} [rest of string was truncated]." : $"Looks up a localized string similar to {value}.";
                }
                foreach (string line in SourceGenerator.NewlineRegex.Split(new XElement(XNamespace.None.GetName("summary"), new XText($"\n{comment}\n")).ToString()).Select(s => s.TrimEnd()))
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
        }
    }
}
