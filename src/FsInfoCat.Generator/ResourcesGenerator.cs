using Microsoft.CodeAnalysis;
using System.Linq;
using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Text;
using System.Xml;
using System.Text;
using System.CodeDom.Compiler;

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
            using StringWriter underlyingWriter = new();
            using IndentedTextWriter writer = new(underlyingWriter, "    ");
            writer.WriteLines("namespace FsInfoCat.Properties", "{");
            writer.Indent++;
            writer.WriteLine(@"
using System.ComponentModel;
using System.Globalization;
using System.Resources;
/// <summary>
/// A strongly-typed resource class for looking up localized strings, etc.
/// </summary>
[System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""16.0.0.0"")]
[System.Diagnostics.DebuggerNonUserCodeAttribute()]
[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
public class Resources
{");
            writer.Indent++;
            writer.WriteLine(@"
private static ResourceManager _resourceManager;
private static CultureInfo _resourceCulture;
[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Performance"", ""CA1811:AvoidUncalledPrivateCode"")]
internal Resources() { }
/// <summary>
///   Returns the cached ResourceManager instance used by this class.
/// </summary>
[EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
public static ResourceManager ResourceManager
{
    get
    {
        if (object.ReferenceEquals(_resourceManager, null))
        {
            ResourceManager temp = new ResourceManager(""FsInfoCat.Properties.Resources"", typeof(Resources).Assembly);
            _resourceManager = temp;
        }
        return _resourceManager;
    }
}
/// <summary>
///   Overrides the current thread's CurrentUICulture property for all
///   resource lookups using this strongly typed resource class.
/// </summary>
[EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
public static CultureInfo Culture
{
    get
    {
        return _resourceCulture;
    }
    set
    {
        _resourceCulture = value;
    }
}");
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
                    {
                        writer.Write("/// ");
                        writer.WriteLine(line);
                    }
                    else
                        writer.WriteLine("///");
                }
                writer.WriteLine($"public static string {name}  => ResourceManager.GetString(\"{name}\", _resourceCulture);");
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Flush();
            var sourceText = SourceText.From(underlyingWriter.ToString(), Encoding.UTF8);
            context.AddSource("Resources-generated.cs", sourceText);
        }
    }
}
