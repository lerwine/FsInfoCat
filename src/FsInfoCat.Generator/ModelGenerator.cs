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
            new(
                SourceGenerator.DiagnosticID_MissingEntityTypesFileError,
                "Missing EntityTypes.xml file",
                "Resources file error: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_SchemaValidationWarning =
            new(
                SourceGenerator.DiagnosticID_SchemaValidationWarning,
                "XML Parsing Error",
                "Unexpected error parsing XML file: {0}",
                nameof(ModelGenerator),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DiagnosticDescriptor_MissingResourceError =
            new(
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
                foreach (string n in new[] { "EntityTypesXsd", "ConstructedTypesXsd", "DocumentationXsd", "EnumDefinitionsXsd", "ExplicitNamesXsd", "InterfaceDefinitionsXsd", "SqlStatementsXsd", "TypeNamesXsd" })
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
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.AbstractEntity))
                if (!GenerateAbstractEntity(context, element, CsNamespace_Core)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.GenericEntity1))
                if (!GenerateGenericEntity1(context, element)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.GenericEntity2))
                if (!GenerateGenericEntity2(context, element)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Local).Elements(XmlNames.Enum))
                if (!GenerateEnum(context, element, CsNamespace_Local)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Local).Elements(XmlNames.AbstractEntity))
                if (!GenerateAbstractEntity(context, element, CsNamespace_Local)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Local).Elements(XmlNames.Table))
                if (!GenerateTableInterface(context, element, CsNamespace_Local)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.Enum))
                if (!GenerateEnum(context, element, CsNamespace_Upstream)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.AbstractEntity))
                if (!GenerateAbstractEntity(context, element, CsNamespace_Upstream)) return;
            foreach (XElement element in EntityTypesXml.Root.Elements(XmlNames.Upstream).Elements(XmlNames.Table))
                if (!GenerateTableInterface(context, element, CsNamespace_Upstream)) return;
        }

        private void WriteCommentDocumentation(XElement documentElement, int indentLevel, StringBuilder stringBuilder!!)
        {
            string indent = (indentLevel > 0) ? new string(' ', indentLevel * 4) + "/// " : "/// ";
            foreach (string line in documentElement.Elements().SelectMany(e => SourceGenerator.NewlineRegex.Split(e.ToString())))
                stringBuilder.Append(indent).AppendLine(line);
        }

        private bool TryWriteDisplayAttribute(GeneratorExecutionContext context, XElement displayElement!!, StringBuilder stringBuilder!!)
        {
            string label = displayElement.GetAttributeValue(XmlNames.Label);
            if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == label))
            {
                stringBuilder.Append("        [Display(Name = nameof(Properties.Resources.").Append(label);
                string shortName = displayElement.GetAttributeValue(XmlNames.ShortName);
                string description = displayElement.GetAttributeValue(XmlNames.Description);
                if (string.IsNullOrWhiteSpace(shortName))
                {
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        stringBuilder.AppendLine("), ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == description))
                    {
                        stringBuilder.Append("), Description = nameof(Properties.Resources.").Append(description).AppendLine("),")
                            .AppendLine("            ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    label = description;
                }
                else if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == shortName))
                {
                    stringBuilder.Append("), ShortName = nameof(Properties.Resources.").Append(shortName).AppendLine("),");
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        stringBuilder.AppendLine("            ResourceType = typeof(Properties.Resources))]");
                        return true;
                    }
                    if (_resourcesXml.Root.Elements(XmlNames.data).Any(e => e.GetAttributeValue(XmlNames.name) == description))
                    {
                        stringBuilder.Append("            Description = nameof(Properties.Resources.").Append(description)
                            .AppendLine("), ResourceType = typeof(Properties.Resources))]");
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

        private bool GenerateTypeParameters(GeneratorExecutionContext context, IEnumerable<XElement> typeElements)
        {
            throw new NotImplementedException();
        }

        private bool GenerateAbstractEntity(GeneratorExecutionContext context, XElement entityElement, string codeNamespace)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("using System;")
                .AppendLine("using System.ComponentModel.DataAnnotations;")
                .Append("namespace ").AppendLine(codeNamespace)
                .AppendLine("{");
            WriteCommentDocumentation(entityElement.Element(XmlNames.Documentation), 1, stringBuilder);
            stringBuilder.Append("    public interface ");
            XElement[] implementsElements = entityElement.Element(XmlNames.Implements).Elements().ToArray();
            XmlNames name;
            switch (implementsElements.Length)
            {
                case 0:
                    stringBuilder.AppendLine(entityElement.Attribute(XmlNames.Name).Value);
                    break;
                case 1:
                    stringBuilder.Append(entityElement.Attribute(XmlNames.Name).Value).Append(" : ");
                    name = (XmlNames)Enum.Parse(typeof(XmlNames), implementsElements[0].Name.LocalName);
                    string t;
                    switch (name)
                    {
                        case XmlNames.GenericAbstract1:
                            t = entityElement.Attribute(XmlNames.Ref).Value;
                            stringBuilder.Append(t.Substring(0, t.Length - 2)).Append("<");
                            if (!GenerateTypeParameters(context, entityElement.Element(XmlNames.GenericArguments).Elements().Take(1))) return false;
                            stringBuilder.Append(">");
                            break;
                        case XmlNames.GenericAbstract2:
                            t = entityElement.Attribute(XmlNames.Ref).Value;
                            stringBuilder.Append(t.Substring(0, t.Length - 2)).Append("<");
                            if (!GenerateTypeParameters(context, entityElement.Element(XmlNames.GenericArguments).Elements().Take(2))) return false;
                            stringBuilder.Append(">");
                            break;
                        case XmlNames.Interface:
                            stringBuilder.AppendLine(entityElement.Attribute(XmlNames.Type).Value);
                            break;
                        case XmlNames.GenericInterface1:
                            t = entityElement.Attribute(XmlNames.Type).Value;
                            stringBuilder.Append(t.Substring(0, t.Length - 2)).Append("<");
                            if (!GenerateTypeParameters(context, entityElement.Element(XmlNames.GenericArguments).Elements().Take(1))) return false;
                            stringBuilder.Append(">");
                            break;
                        case XmlNames.GenericInterface2:
                            t = entityElement.Attribute(XmlNames.Type).Value;
                            stringBuilder.Append(t.Substring(0, t.Length - 2)).Append("<");
                            if (!GenerateTypeParameters(context, entityElement.Element(XmlNames.GenericArguments).Elements().Take(2))) return false;
                            stringBuilder.Append(">");
                            break;
                        default: // XmlNames.Abstract
                            stringBuilder.AppendLine(entityElement.Attribute(XmlNames.Ref).Value);
                            break;
                    }
                    break;
                case 2:
                    stringBuilder.Append(entityElement.Attribute(XmlNames.Name).Value).Append(" : ");
                    break;
                default:
                    stringBuilder.Append(entityElement.Attribute(XmlNames.Name).Value).Append(" : ");
                    break;
            }
            throw new NotImplementedException();
            /*
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IEquatable{IComparison}" />
    /// <seealso cref="IHasMembershipKeyReference{IFile, IFile}" />
    /// <seealso cref="Upstream.Model.IUpstreamComparison" />
    /// <seealso cref="Local.Model.ILocalComparison" />
    /// <seealso cref="IDbContext.Comparisons" />
    /// <seealso cref="IFile.BaselineComparisons" />
    /// <seealso cref="IFile.CorrelativeComparisons" />
IComparison : IDbEntity, IEquatable<IComparison>, IHasMembershipKeyReference<IFile, IFile>
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="Baseline" /> and <see cref="Correlative" /> are identical byte-for-byte.
        /// </summary>
        /// <value><see langword="true" /> if <see cref="Baseline" /> and <see cref="Correlative" /> are identical byte-for-byte; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.AreEqual), ResourceType = typeof(Properties.Resources))]
        bool AreEqual { get; }

        /// <summary>
        /// Gets the date and time when the files were compared.
        /// </summary>
        /// <value>The date and time when <see cref="Baseline" /> was compared to <see cref="Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.ComparedOn), ResourceType = typeof(Properties.Resources))]
        DateTime ComparedOn { get; }

        /// <summary>
        /// Gets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="Baseline" /><see cref="IFile">file entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.BaselineFileId), ResourceType = typeof(Properties.Resources))]
        Guid BaselineId { get; }

        /// <summary>
        /// Gets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.BaselineFile), ResourceType = typeof(Properties.Resources))]
        IFile Baseline { get; }

        /// <summary>
        /// Gets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="Correlative" /><see cref="IFile">file entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.CorrelativeFileId), ResourceType = typeof(Properties.Resources))]
        Guid CorrelativeId { get; }

        /// <summary>
        /// Gets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.CorrelativeFile), ResourceType = typeof(Properties.Resources))]
        IFile Correlative { get; }

        /// <summary>
        /// Gets the value of the <see cref="BaselineId" /> property or the unique identifier of the <see cref="Baseline" /> entity if it has been assigned.
        /// </summary>
        /// <param name="baselineId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier for the associated <see cref="IFile" /> baseline entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetBaselineId(out Guid baselineId);

        /// <summary>
        /// Gets value of the <see cref="CorrelativeId" /> property or the unique identifier of the <see cref="Correlative" /> entity if it has been assigned.
        /// </summary>
        /// <param name="correlativeId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier for the associated <see cref="IFile" /> correlative entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetCorrelativeId(out Guid correlativeId);
    }
}
            */
        }

        private bool GenerateTableInterface(GeneratorExecutionContext context, XElement entityElement, string codeNamespace)
        {
            StringBuilder stringBuilder = new();
            throw new NotImplementedException();
        }

        private bool GenerateGenericEntity1(GeneratorExecutionContext context, XElement entityElement)
        {
            StringBuilder stringBuilder = new();
            throw new NotImplementedException();
        }

        private bool GenerateGenericEntity2(GeneratorExecutionContext context, XElement entityElement)
        {
            StringBuilder stringBuilder = new();
            throw new NotImplementedException();
        }

        private bool GenerateEnum(GeneratorExecutionContext context, XElement enumElement, string codeNamespace)
        {
            StringBuilder stringBuilder = new();
            if (!enumElement.TryGetAttributeValue(XmlNames.Flags, out bool flags)) flags = false;
            IEnumerable<XElement> fieldElements = enumElement.Element(XmlNames.Fields).Elements();
            XElement firstField = fieldElements.First();
            XmlNames enumType = (XmlNames)Enum.Parse(typeof(XmlNames), firstField.Name.LocalName);

            fieldElements.Attributes(XmlNames.StatusMessageLevel).Any();
            if (flags) stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.ComponentModel.DataAnnotations;")
                .Append("namespace ").AppendLine(codeNamespace)
                .AppendLine("{");
            WriteCommentDocumentation(enumElement.Element(XmlNames.Documentation), 1, stringBuilder);
            if (flags) stringBuilder.AppendLine("    [Flags]");
            stringBuilder.Append("    public enum ");
            string enumName = enumElement.GetAttributeValue(XmlNames.Name);
            stringBuilder.Append(enumName).Append(" : ");
            bool writeField(XElement fe)
            {
                stringBuilder.AppendLine();
                WriteCommentDocumentation(fe.Element(XmlNames.Documentation), 2, stringBuilder);
                string label = fe.GetAttributeValue(XmlNames.StatusMessageLevel);
                if (!string.IsNullOrWhiteSpace(label))
                    stringBuilder.Append("        [StatusMessageLevel(StatusMessageLevel.").Append(label).AppendLine(")]");
                label = fe.GetAttributeValue(XmlNames.ErrorCode);
                if (!string.IsNullOrWhiteSpace(label))
                    stringBuilder.Append("        [ErrorCode(ErrorCode.").Append(label).AppendLine(")]");
                label = fe.GetAttributeValue(XmlNames.MessageCode);
                if (!string.IsNullOrWhiteSpace(label))
                    stringBuilder.Append("        [MessageCode(MessageCode.").Append(label).AppendLine(")]");
                if (!TryWriteDisplayAttribute(context, fe.Element(XmlNames.Display), stringBuilder)) return false;
                stringBuilder.Append("        ").Append(fe.GetAttributeValue(XmlNames.Name)).Append(" = ").Append(fe.GetAttributeValue(XmlNames.Value));
                return true;
            }
            switch (enumType)
            {
                case XmlNames.Byte:
                    stringBuilder.AppendLine("byte").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine(",");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine();
                    break;
                case XmlNames.SByte:
                    stringBuilder.AppendLine("sbyte").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine(",");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine();
                    break;
                case XmlNames.Short:
                    stringBuilder.AppendLine("short").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine(",");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine();
                    break;
                case XmlNames.UShort:
                    stringBuilder.AppendLine("ushort").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine(",");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine();
                    break;
                case XmlNames.UInt:
                    stringBuilder.AppendLine("uint").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine("u,");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine("u");
                    break;
                case XmlNames.Long:
                    stringBuilder.AppendLine("long").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine("L,");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine("L");
                    break;
                case XmlNames.ULong:
                    stringBuilder.AppendLine("ulong").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine("UL,");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine("UL");
                    break;
                default:
                    stringBuilder.AppendLine("int").Append("    {");
                    if (!writeField(firstField)) return false;
                    foreach (XElement fe in fieldElements.Skip(1))
                    {
                        stringBuilder.AppendLine(",");
                        if (!writeField(fe)) return false;
                    }
                    stringBuilder.AppendLine();
                    break;
            }
            stringBuilder.AppendLine("    }").AppendLine("}");
            var sourceText = SourceText.From(stringBuilder.ToString(), Encoding.UTF8);
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
