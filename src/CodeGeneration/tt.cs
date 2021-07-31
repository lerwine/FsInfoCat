using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using CodeGeneration;

namespace TextTemplating
{
    class tt
    {
        #region Fakes

        internal StringBuilder HostEnvironment = new();
        readonly Stack<string> _indentValues = new();
        int _lastLineStart = 0;
        bool _lastLineNotEmpty = false;
        private string _currentIndent = "";
        void Write(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (HostEnvironment.Length < _lastLineStart)
            {
                if (HostEnvironment.Length > 0)
                {
                    text = HostEnvironment.Append(text).ToString();
                    HostEnvironment.Clear();
                }
                _lastLineStart = 0;
                _lastLineNotEmpty = false;
            }
            if (CgConstants.NewLineRegex.IsMatch(text))
            {
                string[] lines = CgConstants.NewLineRegex.Split(text);
                if (_currentIndent.Length > 0)
                {
                    if (_lastLineNotEmpty)
                        HostEnvironment.Insert(_lastLineStart, _currentIndent).AppendLine(lines[0]);
                    else if (!lines[0].All(c => char.IsWhiteSpace(c)))
                    {
                        if (_lastLineStart < HostEnvironment.Length)
                            HostEnvironment.Insert(_lastLineStart, _currentIndent).AppendLine(lines[0]);
                        else
                            HostEnvironment.Append(_currentIndent).AppendLine(lines[0]);
                    }
                    else
                        HostEnvironment.AppendLine(lines[0]);
                    if (lines.Length > 2)
                        foreach (string s in lines.Skip(1).Reverse().Skip(1).Reverse())
                        {
                            if (s.Length > 0 && !lines[0].All(c => char.IsWhiteSpace(c)))
                                HostEnvironment.Append(_currentIndent).AppendLine(s);
                            else
                                HostEnvironment.AppendLine(s);
                        }
                }
                else
                    foreach (string s in lines.Reverse().Skip(1).Reverse())
                        HostEnvironment.AppendLine(s);
                _lastLineStart = HostEnvironment.Length;
                _lastLineNotEmpty = (text = lines.Last()).Length > 0 && !text.All(c => char.IsWhiteSpace(c));
                HostEnvironment.Append(text);
            }
            else
            {
                if (!(_lastLineNotEmpty || text.All(c => !char.IsWhiteSpace(c))))
                    _lastLineNotEmpty = true;
                HostEnvironment.Append(text);
            }
        }

        string DefaultNamespace { get; } = "";

        void WriteLine(string text)
        {
            Write(text);
            if (_lastLineNotEmpty && _currentIndent.Length > 0)
                HostEnvironment.Insert(_lastLineStart, _currentIndent).AppendLine();
            else
                HostEnvironment.AppendLine();
            _lastLineStart = HostEnvironment.Length;
            _lastLineNotEmpty = false;
        }

        void PushIndent(string indent)
        {
            _indentValues.Push(indent ?? "");
            _currentIndent = string.Join("", _indentValues);
        }

        void PopIndent()
        {
            if (_indentValues.Count == 0)
                return;
            _indentValues.Pop();
            _currentIndent = (_indentValues.Count > 0) ? string.Join("", _indentValues) : "";
        }

        (string FullName, string Name) Solution = (Path.GetFullPath(Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, "../../../..")), "FsInfoCat");

        #endregion

        #region core.ttinclude

        private XDocument _entityDefinitionsDocument;

        List<ValidationEventArgs> ValidationErrors { get; } = new();

        XDocument EntityDefinitionsDocument
        {
            get
            {
                if (_entityDefinitionsDocument is null)
                {
                    XmlReaderSettings readerSettings = new() { ValidationType = ValidationType.Schema };
                    readerSettings.Schemas.Add("", Path.Combine(Path.GetDirectoryName(Solution.FullName), "FsInfoCat\\Resources\\EntityDefinitions.xsd"));
                    readerSettings.ValidationEventHandler += XmlValidationEventHandler;
                    using XmlReader reader = XmlReader.Create(Path.Combine(Path.GetDirectoryName(Solution.FullName), "FsInfoCat\\Resources\\EntityDefinitions.xml"), readerSettings);
                    _entityDefinitionsDocument = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
                }
                return _entityDefinitionsDocument;
            }
        }

        void XmlValidationEventHandler(object sender, ValidationEventArgs e) { ValidationErrors.Add(e); }

        #endregion

        #region EntityTypes.tt

        public void EntityTypes()
        {
            #region TemplatedText

            WriteLine(@$"using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace <#={DefaultNamespace}#>
{{");
            #endregion

            XElement entityDefinitionsElement = EntityDefinitionsDocument.Root;

            foreach (ValidationEventArgs e in ValidationErrors)
            {
                if (e.Severity == XmlSeverityType.Warning)
                    Write($"#warning XML Validation message: {e.Message}");
                else
                {
                    XmlSchemaException error = e.Exception;
                    if (error is null)
                        WriteLine($"#error Validation error: {e.Message}");
                    else
                        WriteLine($"#error LineNumber {error.LineNumber}, LinePosition {error.LinePosition}: {e.Message}");
                }
            }

            foreach (XElement enumElement in entityDefinitionsElement.Elements().Elements(CgConstants.XNAME_EnumTypes).Elements())
            {
                string typeName = enumElement.Attribute(CgConstants.XNAME_Name)?.Value;
                XName elementName = enumElement.Name;
                if (string.IsNullOrWhiteSpace(typeName))
                    WriteLine($"#warning {CgConstants.XNAME_Name} attribute missing or empty for /{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{CgConstants.XNAME_EnumTypes}/{elementName}[{(enumElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)}]");
                else
                {
                    string enumXPath = $"/{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{CgConstants.XNAME_EnumTypes}/{elementName}[@Name='{typeName}']";
                    foreach (XElement fieldElement in enumElement.Elements(CgConstants.XNAME_Field))
                    {
                        string fieldName = fieldElement.Attribute(CgConstants.XNAME_Name)?.Value;
                        if (string.IsNullOrWhiteSpace(fieldName))
                            WriteLine($"#warning {CgConstants.XNAME_Name} attribute missing or empty for {enumXPath}/{CgConstants.XNAME_Field}[{(fieldElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == CgConstants.XNAME_Field) + 1)}]");
                        else
                        {
                            string fieldXPath = $"{enumXPath}/{CgConstants.XNAME_Field}[@Name='{fieldName}']";
                            XAttribute attribute = fieldElement.Attribute(CgConstants.XNAME_FullName);
                            string expected = $"{typeName}.{fieldName}";
                            if (attribute is null)
                            {
                                WriteLine($"#warning {CgConstants.XNAME_FullName} attribute missing for {fieldXPath}");
                                fieldElement.SetAttributeValue(CgConstants.XNAME_FullName, expected);
                            }
                            else if (attribute.Value != expected)
                            {
                                WriteLine($"#warning {CgConstants.XNAME_FullName} attribute does not match the value \"{expected}\" for {fieldXPath}");
                                fieldElement.SetAttributeValue(CgConstants.XNAME_FullName, expected);
                            }
                        }
                    }
                }
            }

            foreach (XElement entityElement in entityDefinitionsElement.Elements().Elements(CgConstants.XNAME_Entity))
            {
                string typeName = entityElement.Attribute(CgConstants.XNAME_Name)?.Value;
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    XName elementName = entityElement.Name;
                    WriteLine($"#warning {CgConstants.XNAME_Name} attribute missing or empty for /{entityElement.Parent.Parent.Name}/{entityElement.Parent.Name}/{CgConstants.XNAME_Entity}[(entityElement.NodesBeforeSelf().OfType<XElement>().Count()]");
                }
                else
                {
                    string typeXPath = $"/{entityElement.Parent.Parent.Name}/{entityElement.Parent.Name}/{CgConstants.XNAME_Entity}[@Name='{typeName}']";
                    foreach (XElement propertyElement in entityElement.Elements(CgConstants.XNAME_Properties).Elements())
                    {
                        XName elementName = propertyElement.Name;
                        string propertyName = propertyElement.Attribute(CgConstants.XNAME_Name)?.Value;
                        if (string.IsNullOrWhiteSpace(propertyName))
                            WriteLine($"#warning {CgConstants.XNAME_Name} attribute missing or empty for {typeXPath}/{elementName}[{(propertyElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)}]");
                        else
                        {
                            string propertyXPath = $"{typeXPath}/{elementName}[@Name='{propertyName}']";
                            XAttribute attribute = propertyElement.Attribute(CgConstants.XNAME_FullName);
                            string expected = $"{typeName}.{propertyName}";
                            if (attribute is null)
                            {
                                WriteLine($"#warning {CgConstants.XNAME_FullName} attribute missing for {propertyXPath}");
                                propertyElement.SetAttributeValue(CgConstants.XNAME_FullName, expected);
                            }
                            else if (attribute.Value != expected)
                            {
                                WriteLine($"#warning {CgConstants.XNAME_FullName} attribute does not match the value \"{expected}\" for {propertyXPath}");
                                propertyElement.SetAttributeValue(CgConstants.XNAME_FullName, expected);
                            }
                        }
                    }
                }
            }

            PushIndent("    "); 
            bool isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(CgConstants.XNAME_Root).Elements(CgConstants.XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(CgConstants.XNAME_Root).Elements(CgConstants.XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
    public static class DbConstants
    {{
        public const int DbColMaxLen_SimpleName = 256;
        public const int DbColMaxLen_LongName = 1024;
        public const int DbColMaxLen_ShortName = 128;
        public const int DbColMaxLen_Identifier = 1024;
        public const int DbColMaxLen_FileName = 1024;
        public const uint DbColDefaultValue_MaxNameLength = 255;
        public const ushort DbColDefaultValue_MaxRecursionDepth = 256;
        public const ulong DbColDefaultValue_MaxTotalItems = ulong.MaxValue;
    }}

}}

namespace <#=DefaultNamespace#>.Local
{{
");
            #endregion

            PushIndent("    ");
            isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(CgConstants.XNAME_Local).Elements(CgConstants.XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(CgConstants.XNAME_Local).Elements(CgConstants.XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
}}

namespace <#=DefaultNamespace#>.Upstream
{{
");
            #endregion

            PushIndent("    ");
            isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(CgConstants.XNAME_Upstream).Elements(CgConstants.XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(CgConstants.XNAME_Upstream).Elements(CgConstants.XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
}}
");
            #endregion
        }

        void WriteAmbientValueAttribute(XElement memberElement)
        {
            string ambientValue = memberElement.Elements(CgConstants.XNAME_AmbientString).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (ambientValue is not null)
            {
                Write("[AmbientValue(\"");
                Write(ambientValue.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n"));
                WriteLine("\")]");
                return;
            }
            foreach (XName name in new[] { CgConstants.XNAME_AmbientEnum, CgConstants.XNAME_AmbientBoolean, CgConstants.XNAME_AmbientInt })
            {
                ambientValue = memberElement.Elements(name).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ambientValue))
                {
                    Write("[AmbientValue(");
                    Write(ambientValue);
                    WriteLine(")]");
                    return;
                }
            }
            ambientValue = memberElement.Elements(CgConstants.XNAME_AmbientUInt).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("u)]");
                return;
            }
            ambientValue = memberElement.Elements(CgConstants.XNAME_AmbientLong).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("L)]");
                return;
            }
            ambientValue = memberElement.Elements(CgConstants.XNAME_AmbientULong).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("UL)]");
                return;
            }
            ambientValue = memberElement.Elements(CgConstants.XNAME_AmbientDouble).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine(ambientValue.Contains(".") ? ")]" : ".0)]");
                return;
            }
            ambientValue = memberElement.Elements(CgConstants.XNAME_AmbientFloat).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("f)]");
                return;
            }
            foreach (XName name in new[] { CgConstants.XNAME_AmbientByte, CgConstants.XNAME_AmbientSByte, CgConstants.XNAME_AmbientShort, CgConstants.XNAME_AmbientUShort })
            {
                ambientValue = memberElement.Elements(name).Attributes(CgConstants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ambientValue))
                {
                    Write("[AmbientValue((");
#pragma warning disable IDE0057 // Use range operator
                    Write(name.LocalName.Substring(7).ToLower());
#pragma warning restore IDE0057 // Use range operator
                    Write(")");
                    Write(ambientValue);
                    WriteLine(")]");
                    return;
                }
            }
        }
        void WriteDisplayAttribute(string memberName, Func<XName, IEnumerable<XAttribute>> getAttributes, string typeName = null)
        {
            string displayNameResource = getAttributes(CgConstants.XNAME_DisplayNameResource).Select(a => a.Value).DefaultIfEmpty(string.IsNullOrWhiteSpace(typeName) ?
                $"DisplayName_{memberName}" : $"DisplayName_{typeName}_{memberName}").First();
            string descriptionResource = getAttributes(CgConstants.XNAME_DescriptionResource).Select(a => a.Value).FirstOrDefault();
            string resourceType = getAttributes(CgConstants.XNAME_ResourceType).Select(a => a.Value).DefaultIfEmpty("Properties.Resources").First();
            if (string.IsNullOrWhiteSpace(displayNameResource))
            {
                if (!string.IsNullOrWhiteSpace(descriptionResource))
                {
                    Write("[Display(Description = nameof(");
                    Write(resourceType);
                    Write(".");
                    Write(descriptionResource);
                    WriteLine("), ResourceType = typeof(Properties.Resources))]");
                }
            }
            else
            {
                Write("[Display(Name = nameof(");
                Write(resourceType);
                Write(".");
                Write(displayNameResource);
                if (!string.IsNullOrWhiteSpace(descriptionResource))
                {
                    Write("), Description = nameof(");
                    Write(resourceType);
                    Write(".");
                    Write(descriptionResource);
                }
                Write("), ResourceType = typeof(");
                Write(resourceType);
                WriteLine("))]");
            }
        }
        void GenerateXmlDoc(XElement xmlDocElement)
        {
            if (xmlDocElement is null)
                return;
            if (xmlDocElement.IsEmpty)
            {
                foreach (string lt in CgConstants.NewLineRegex.Split(xmlDocElement.ToString(SaveOptions.None)))
                {
                    Write("/// ");
                    WriteLine(lt.Trim());
                }
                return;
            }
            if (xmlDocElement.LastNode is XText lastNode)
            {
                while (lastNode.PreviousNode is XText previousTextNode)
                {
                    lastNode.Value = $"{previousTextNode.Value}{lastNode.Value}";
                    previousTextNode.Remove();
                }
                if (!CgConstants.TrailingEmptyLine.IsMatch(lastNode.Value))
                    lastNode.Value = $"{lastNode.Value}\n";
            }
            else
            {
                lastNode = new XText("\n");
                xmlDocElement.Add(lastNode);
            }
            if (xmlDocElement.FirstNode is XText firstNode)
            {
                while (firstNode.NextNode is XText followingTextNode)
                {
                    firstNode.Value = $"{firstNode.Value}{followingTextNode.Value}";
                    followingTextNode.Remove();
                }
                if (ReferenceEquals(firstNode, lastNode))
                {
                    if (CgConstants.LeadingEmptyLine.Match(firstNode.Value).Length < firstNode.Value.Length)
                        firstNode.Value = $"\n{firstNode.Value}";
                }
                else if (!CgConstants.LeadingEmptyLine.IsMatch(firstNode.Value))
                    firstNode.Value = $"\n{firstNode.Value}";
            }
            else
                xmlDocElement.FirstNode.AddBeforeSelf(new XText("\n"));
            IEnumerable<string> lines = CgConstants.NewLineRegex.Split(xmlDocElement.ToString(SaveOptions.None));
            if (!(lines.Skip(2).Any() && (lines = lines.Take(1).Concat(lines.Skip(1).SkipWhile(s => string.IsNullOrWhiteSpace(s))).ToArray()).Skip(3).Any() &&
                (lines = lines.Reverse().Take(1).Concat(lines.Reverse().Skip(1).SkipWhile(s => string.IsNullOrWhiteSpace(s))).Reverse().ToArray()).Skip(3).Any()))
            {
                Write("/// ");
                Write(lines.First());
                if (lines.Skip(2).Any())
                    Write(lines.Skip(1).First().Trim());
                WriteLine(lines.Last().Trim());
                return;
            }

            if (CgConstants.LeadingWsRegex.IsMatch(lines.Skip(1).First()))
            {
                int indent = lines.Skip(1).Reverse().Skip(1).Reverse().Select(s => CgConstants.LeadingWsRegex.Match(s)).Select(m => m.Success ? m.Length : 0).Min();
                if (indent > 0)
#pragma warning disable IDE0057 // Use range operator
                    lines = lines.Take(1).Concat(lines.Skip(1).Reverse().Skip(1).Reverse().Select(s => s.Substring(indent))).Concat(lines.Reverse().Take(1));
#pragma warning restore IDE0057 // Use range operator
            }
            else
            {
                int indent = lines.Skip(2).Reverse().Skip(1).Reverse().Select(s => CgConstants.LeadingWsRegex.Match(s)).Select(m => m.Success ? m.Length : 0).Min();
                if (indent > 0)
#pragma warning disable IDE0057 // Use range operator
                    lines = lines.Take(2).Concat(lines.Skip(2).Reverse().Skip(1).Reverse().Select(s => s.Substring(indent))).Concat(lines.Reverse().Take(1));
#pragma warning restore IDE0057 // Use range operator
            }

            Write("/// ");
            WriteLine(lines.First());
            foreach (string s in lines.Skip(1).Reverse().Skip(1).Reverse())
            {
                Write("/// ");
                WriteLine(s.TrimEnd());
            }
            Write("/// ");
            WriteLine(lines.Last().Trim());
        }
        void GenerateEnumType(XElement enumElement)
        {
            GenerateXmlDoc(enumElement.Element(CgConstants.XNAME_summary));
            GenerateXmlDoc(enumElement.Element(CgConstants.XNAME_remarks));
            foreach (XElement e in enumElement.Elements(CgConstants.XNAME_seealso))
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            EnumGenerationInfo generationInfo = EnumGenerationInfo.Get(enumElement);
            if (generationInfo.IsFlags)
                WriteLine("[Flags]");
            Write("public enum ");
            Write(generationInfo.Name);
            Write(" : ");
            WriteLine(generationInfo.CsTypeName);
            WriteLine("{");
            PushIndent("    ");
            bool isSubsequentMember = false;
            foreach (FieldGenerationInfo field in generationInfo.Fields)
            {
                if (isSubsequentMember)
                {
                    WriteLine(",");
                    WriteLine("");
                    isSubsequentMember = true;
                }
                else
                    isSubsequentMember = true;
                GenerateXmlDoc(field.Source.Element(CgConstants.XNAME_summary));
                GenerateXmlDoc(field.Source.Element(CgConstants.XNAME_remarks));
                foreach (XElement e in field.Source.Elements(CgConstants.XNAME_seealso))
                {
                    if (!e.IsEmpty && e.Value.Trim().Length == 0)
                        e.RemoveAll();
                    GenerateXmlDoc(e);
                }
                WriteAmbientValueAttribute(field.Source);
                WriteDisplayAttribute(field.Name, n => field.Source.Attributes(n), generationInfo.Name);
                Write(field.Name);
                Write(" = ");
                Write(field.Value.CsCode);
            }

            PopIndent();
            WriteLine("");
            WriteLine("}");
        }
        bool GenerateEnumTypes(IEnumerable<XElement> enumElements)
        {
            if (!enumElements.Any())
                return false;
            GenerateEnumType(enumElements.First());
            foreach (XElement typeElement in enumElements.Skip(1))
            {
                WriteLine("");
                GenerateEnumType(typeElement);
            }
            return true;
        }
        void GenerateEntityTypes(IEnumerable<XElement> entityElements, bool isSubsequentMember)
        {
            if (isSubsequentMember)
                WriteLine("");
            GenerateEntityInterface(entityElements.First());
            foreach (XElement typeElement in entityElements.Skip(1))
            {
                WriteLine("");
                GenerateEntityInterface(typeElement);
            }
        }
        void GenerateProperty(string typeName, PropertyGenerationInfo property)
        {
            XElement commentDocElement = property.Source.Element(CgConstants.XNAME_summary) ?? property.Inherited.Select(p => p.Source).Elements(CgConstants.XNAME_summary).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No summary element found for {typeName}.{property.Name}");
            else
                GenerateXmlDoc(commentDocElement);
            commentDocElement = property.Source.Element(CgConstants.XNAME_value) ?? property.Inherited.Select(p => p.Source).Elements(CgConstants.XNAME_value).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No value element found for {typeName}.{property.Name}");
            else
                GenerateXmlDoc(commentDocElement);
            commentDocElement = property.Source.Element(CgConstants.XNAME_remarks) ?? property.Inherited.Select(p => p.Source).Elements(CgConstants.XNAME_remarks).FirstOrDefault();
            if (commentDocElement is not null)
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in property.Source.Elements(CgConstants.XNAME_seealso).Concat(property.Inherited.Select(p => p.Source).Elements(CgConstants.XNAME_seealso)).Distinct())
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            WriteDisplayAttribute(property.Name, n => property.Source.Attributes(n).Concat(property.Inherited.Select(p => p.Source).Attributes(n)));
            if (property.Inherited.Count > 0)
                Write("new ");
            switch (property.Source.Name.LocalName)
            {
                case CgConstants.NAME_Byte:
                case CgConstants.NAME_SByte:
                case CgConstants.NAME_Short:
                case CgConstants.NAME_UShort:
                case CgConstants.NAME_Int:
                case CgConstants.NAME_UInt:
                case CgConstants.NAME_Long:
                case CgConstants.NAME_ULong:
                case CgConstants.NAME_Float:
                case CgConstants.NAME_Double:
                    Write(property.Source.Name.LocalName.ToLower());
                    Write(property.AllowNull ? "? " : " ");
                    break;
                case CgConstants.NAME_Text:
                case CgConstants.NAME_NVarChar:
                    Write("string ");
                    break;
                case CgConstants.NAME_VolumeIdentifier:
                case CgConstants.NAME_DriveType:
                case CgConstants.NAME_MD5Hash:
                case CgConstants.NAME_DateTime:
                    Write(property.Source.Name.LocalName);
                    Write(property.AllowNull ? "? " : " ");
                    break;
                case CgConstants.NAME_ByteValues:
                case CgConstants.NAME_MultiStringValue:
                    Write(property.Source.Name.LocalName);
                    Write(" ");
                    break;
                case CgConstants.NAME_UniqueIdentifier:
                    Write(property.AllowNull ? "Guid? " : "Guid ");
                    break;
                case CgConstants.NAME_Bit:
                    Write(property.AllowNull ? "bool? " : "bool ");
                    break;
                case CgConstants.NAME_Enum:
                    Write(property.Source.Attribute(CgConstants.XNAME_Type)?.Value);
                    Write(property.AllowNull ? "? " : " ");
                    break;
                case CgConstants.NAME_CollectionNavigation:
                case CgConstants.NAME_CollectionNavigation_ItemKey:
                case CgConstants.NAME_CollectionNavigation_ItemType:
                    Write("IEnumerable<");
                    Write(property.ReferencedType.Name);
                    Write("> ");
                    break;
                case CgConstants.NAME_RelatedEntity:
                case CgConstants.NAME_RelatedEntity_Key:
                case CgConstants.NAME_RelatedEntity_Type:
                    Write(property.ReferencedType.Name);
                    Write(" ");
                    break;
                default:
                    WriteLine($"#warning Unknown element: {property.Source.Name.LocalName}");
                    Write("object ");
                    break;
            }

            Write(property.Name);
            WriteLine(property.IsGenericWritable ? " { get; set; }" : " { get; }");
        }
        void GenerateEntityInterface(XElement entityElement)
        {
            EntityGenerationInfo generationInfo = EntityGenerationInfo.Get(entityElement);
            XElement commentDocElement = entityElement.Element(CgConstants.XNAME_summary) ?? generationInfo.BaseTypes.Select(b => b.Entity.Source).Elements(CgConstants.XNAME_summary).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No summary element found for {generationInfo.Name}");
            else
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in entityElement.Elements(CgConstants.XNAME_typeparam).Concat(generationInfo.BaseTypes.Select(b => b.Entity.Source).Elements(CgConstants.XNAME_typeparam)).Distinct())
                GenerateXmlDoc(e);
            commentDocElement = entityElement.Element(CgConstants.XNAME_remarks) ?? generationInfo.BaseTypes.Select(b => b.Entity.Source).Elements(CgConstants.XNAME_remarks).FirstOrDefault();
            if (commentDocElement is not null)
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in entityElement.Elements(CgConstants.XNAME_seealso).Concat(generationInfo.BaseTypes.Select(b => b.Entity.Source).Select(s => new XElement(CgConstants.XNAME_seealso, new XAttribute(CgConstants.XNAME_cref, s)))))
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            IEnumerable<PropertyGenerationInfo> properties = generationInfo.Properties.Where(p => ReferenceEquals(p.ReferencedType, generationInfo));
            Write("public interface ");
            if (generationInfo.BaseTypes.Count > 0)
            {
                Write(generationInfo.Name);
                Write(" : ");

                if (generationInfo.BaseTypes.Count > 1)
                {
                    foreach ((string Name, EntityGenerationInfo Entity) t in generationInfo.BaseTypes.Reverse().Skip(1).Reverse())
                    {
                        Write(t.Name);
                        Write(", ");
                    }
                }
                if (!properties.Any())
                {
                    Write(generationInfo.BaseTypes.Last().Name);
                    WriteLine("{ }");
                    return;
                }
                Write(generationInfo.BaseTypes.Last().Name);
            }
            else
            {
                if (!properties.Any())
                {
                    Write(generationInfo.Name);
                    WriteLine("{ }");
                    return;
                }
                WriteLine(generationInfo.Name);
            }

            WriteLine("{");
            PushIndent("    ");

            GenerateProperty(generationInfo.Name, properties.First());
            foreach (PropertyGenerationInfo p in properties.Skip(1))
            {
                WriteLine("");
                GenerateProperty(generationInfo.Name, p);
            }

            PopIndent();
            WriteLine("}");
        }

        #endregion

        #region CreateLocalDb.tt

        void GenerateCreateTableSql(EntityGenerationInfo generationInfo)
        {
            Write("CREATE TABLE IF NOT EXISTS \"");
            Write(generationInfo.TableName);
            WriteLine("\" (");
            PushIndent("    ");
            IEnumerable<PropertyGenerationInfo> collection = generationInfo.Properties.Where(p => !string.IsNullOrEmpty(p.ColumnName));
            GenerateTableColumnSql(collection.First(), out string comment);
            foreach (PropertyGenerationInfo property in collection.Skip(1))
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine(",");
                else
                {
                    Write(", -- ");
                    WriteLine(comment);
                }
                GenerateTableColumnSql(property, out string c);
                comment = c;
            }
            PropertyGenerationInfo[] keyColumns = collection.Where(c => c.IsPrimaryKey).ToArray();
            if (keyColumns.Length == 1)
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine(",");
                else
                {
                    Write(", -- ");
                    WriteLine(comment);
                    comment = null;
                }
                Write("CONSTRAINT \"PK_");
                Write(generationInfo.TableName);
                Write("\" PRIMARY KEY(\"");
                Write(keyColumns[0].ColumnName);
                if (generationInfo.CheckConstraints is null && generationInfo.UniqueConstraints.Count == 0)
                    WriteLine("\")");
                else
                    Write("\")");
            }
            else if (keyColumns.Length > 1)
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine(",");
                else
                {
                    Write(", -- ");
                    WriteLine(comment);
                    comment = null;
                }
                Write("CONSTRAINT \"PK_");
                Write(generationInfo.TableName);
                Write("\" PRIMARY KEY(\"");
                Write(keyColumns[0].ColumnName);
                foreach (PropertyGenerationInfo c in keyColumns.Skip(1))
                {
                    Write("\", \"");
                    Write(c.ColumnName);
                }
                if (generationInfo.CheckConstraints is null && generationInfo.UniqueConstraints.Count == 0)
                    WriteLine("\")");
                else
                    Write("\")");
            }
            if (generationInfo.UniqueConstraints.Count > 0)
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine(",");
                else
                {
                    Write(", -- ");
                    WriteLine(comment);
                    comment = null;
                }
                Write("CONSTRAINT \"");
                Write(generationInfo.UniqueConstraints[0].Name);
                Write("\" UNIQUE(\"");
                Write(generationInfo.UniqueConstraints[0].Properties[0].ColumnName);
                foreach (PropertyGenerationInfo n in generationInfo.UniqueConstraints[0].Properties.Skip(1))
                {
                    Write("\", \"");
                    Write(n.ColumnName);
                }
                Write("\")");
                foreach ((string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties) u in generationInfo.UniqueConstraints.Skip(1))
                {
                    WriteLine(",");
                    Write("CONSTRAINT \"");
                    Write(u.Name);
                    Write("\" UNIQUE(\"");
                    Write(u.Properties[0].ColumnName);
                    foreach (PropertyGenerationInfo n in u.Properties.Skip(1))
                    {
                        Write("\", \"");
                        Write(n.ColumnName);
                    }
                    Write("\")");
                }
            }
            if (generationInfo.CheckConstraints is not null)
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine(",");
                else
                {
                    Write(", -- ");
                    WriteLine(comment);
                    comment = null;
                }
                Write("CHECK(");
                Write(generationInfo.CheckConstraints.ToSqlString());
                WriteLine(")");
            }
            else if (keyColumns.Length == 0 && generationInfo.UniqueConstraints.Count == 0)
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine("");
                else
                {
                    Write(" -- ");
                    WriteLine(comment);
                }
            }
            PopIndent();
            WriteLine(");");
            foreach (PropertyGenerationInfo property in collection)
            {
                if (!string.IsNullOrEmpty(property.IndexName))
                {
                    WriteLine("");
                    Write($"CREATE INDEX \"{property.IndexName}\" ON \"{generationInfo.TableName}\" (\"{property.Name}\"");
                    if (property.IsCaseSensitive.HasValue)
                        WriteLine(property.IsCaseSensitive.Value ? " COLLATE BINARY);" : " COLLATE NOCASE);");
                    else
                        WriteLine(");");
                }
            }
        }
        string PropertyElementToSqlType(XElement propertyElement, out string typeName, out bool isNumeric)
        {
            if (propertyElement is null || propertyElement.Name.NamespaceName.Length > 0)
            {
                isNumeric = false;
                typeName = CgConstants.SQL_TYPENAME_BLOB;
                return typeName;
            }

            switch (propertyElement.Name.LocalName)
            {
                case CgConstants.NAME_Enum:
                    string sqlType = PropertyElementToSqlType(EntityDefinitionsDocument.FindLocalEnumTypeByName(propertyElement.Attribute(CgConstants.XNAME_Type)?.Value), out typeName, out _);
                    isNumeric = false;
                    return sqlType;
                case CgConstants.NAME_UniqueIdentifier:
                case CgConstants.NAME_RelatedEntity:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_UNIQUEIDENTIFIER;
                    return typeName;
                case CgConstants.NAME_NVarChar:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_NVARCHAR;
                    return $"{typeName}({propertyElement.Attribute(CgConstants.XNAME_MaxLength)?.Value})";
                case CgConstants.NAME_VolumeIdentifier:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_NVARCHAR;
                    return $"{typeName}(1024)";
                case CgConstants.NAME_MultiStringValue:
                case CgConstants.NAME_Text:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_TEXT;
                    return typeName;
                case CgConstants.NAME_DateTime:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_DATETIME;
                    return typeName;
                case CgConstants.NAME_TimeSpan:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_TIME;
                    return typeName;
                case CgConstants.NAME_Bit:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_BIT;
                    return typeName;
                case CgConstants.NAME_ByteValues:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_VARBINARY;
                    return $"{typeName}({propertyElement.Attribute(CgConstants.XNAME_MaxLength)?.Value})";
                case CgConstants.NAME_MD5Hash:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_BINARY;
                    return $"{typeName}(16)";
                case CgConstants.NAME_DriveType:
                    isNumeric = false;
                    typeName = CgConstants.SQL_TYPENAME_UNSIGNED_TINYINT;
                    return typeName;
                case CgConstants.NAME_Byte:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_UNSIGNED_TINYINT;
                    return typeName;
                case CgConstants.NAME_SByte:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_TINYINT;
                    return typeName;
                case CgConstants.NAME_Short:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_SMALLINT;
                    return typeName;
                case CgConstants.NAME_UShort:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_UNSIGNED_SMALLINT;
                    return typeName;
                case CgConstants.NAME_Int:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_INT;
                    return typeName;
                case CgConstants.NAME_UInt:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_UNSIGNED_INT;
                    return typeName;
                case CgConstants.NAME_Long:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_BIGINT;
                    return typeName;
                case CgConstants.NAME_ULong:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_UNSIGNED_BIGINT;
                    return typeName;
                case CgConstants.NAME_Float:
                case CgConstants.NAME_Double:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_REAL;
                    return typeName;
                case CgConstants.NAME_Decimal:
                    isNumeric = true;
                    typeName = CgConstants.SQL_TYPENAME_NUMERIC;
                    return typeName;
                default:
                    isNumeric = false;
                    typeName = null;
                    return typeName;
            }
        }
        void GenerateTableColumnSql(PropertyGenerationInfo generationInfo, out string comment)
        {
            Write("\"");
            Write(generationInfo.ColumnName);
            Write("\" ");
            Write(generationInfo.SqlTypeName);
            if (!generationInfo.AllowNull)
                Write(" NOT NULL");
            CheckConstraint check = null;
            switch (generationInfo.Type)
            {
                case DbType.Byte:
                case DbType.SByte:
                case DbType.Int16:
                case DbType.UInt16:
                case DbType.Int32:
                case DbType.UInt32:
                case DbType.Int64:
                case DbType.UInt64:
                case DbType.Single:
                case DbType.Double:
                case DbType.Decimal:
                    if (generationInfo.MinValue.HasValue)
                    {
                        if (generationInfo.MaxValue.HasValue)
                            check = generationInfo.Type switch
                            {
                                DbType.Byte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Byte(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Byte(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.SByte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.SByte(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.SByte(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.Int16 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Short(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Short(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.UInt16 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UShort(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UShort(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.UInt32 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UInt(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UInt(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.Int64 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Long(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Long(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.UInt64 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.ULong(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.ULong(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.Decimal => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Decimal(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Decimal(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.Double => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Double(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Double(generationInfo.MaxValue.Value.SqlCode))),
                                DbType.Single => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Float(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Float(generationInfo.MaxValue.Value.SqlCode))),
                                _ => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Int(generationInfo.MinValue.Value.SqlCode))
                                    .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Int(generationInfo.MaxValue.Value.SqlCode)))
                            };
                        else
                            check = generationInfo.Type switch
                            {
                                DbType.Byte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Byte(generationInfo.MinValue.Value.SqlCode)),
                                DbType.SByte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.SByte(generationInfo.MinValue.Value.SqlCode)),
                                DbType.Int16 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Short(generationInfo.MinValue.Value.SqlCode)),
                                DbType.UInt16 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UShort(generationInfo.MinValue.Value.SqlCode)),
                                DbType.UInt32 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UInt(generationInfo.MinValue.Value.SqlCode)),
                                DbType.Int64 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Long(generationInfo.MinValue.Value.SqlCode)),
                                DbType.UInt64 => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.ULong(generationInfo.MinValue.Value.SqlCode)),
                                DbType.Decimal => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Decimal(generationInfo.MinValue.Value.SqlCode)),
                                DbType.Double => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Double(generationInfo.MinValue.Value.SqlCode)),
                                DbType.Single => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Float(generationInfo.MinValue.Value.SqlCode)),
                                _ => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Int(generationInfo.MinValue.Value.SqlCode))
                            };
                    }
                    else if (generationInfo.MaxValue.HasValue)
                        check = generationInfo.Type switch
                        {
                            DbType.Byte => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Byte(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.SByte => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.SByte(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.Int16 => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Short(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.UInt16 => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UShort(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.UInt32 => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.UInt(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.Int64 => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Long(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.UInt64 => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.ULong(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.Decimal => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Decimal(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.Double => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Double(generationInfo.MaxValue.Value.SqlCode)),
                            DbType.Single => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Float(generationInfo.MaxValue.Value.SqlCode)),
                            _ => ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(generationInfo.ColumnName), ConstantValueReference.Int(generationInfo.MaxValue.Value.SqlCode))
                        };
                    break;
                default:
                    switch (generationInfo.Source.Name.LocalName)
                    {
                        case CgConstants.NAME_ByteValues:
                        case CgConstants.NAME_NVarChar:
                            if (generationInfo.IsNormalized ?? false)
                                check = new SimpleColumnValueReference(generationInfo.ColumnName).Trimmed().Length().IsEqualTo(new SimpleColumnValueReference(generationInfo.ColumnName).Length());
                            if (generationInfo.MinLength.HasValue)
                                check = (check is null) ? new SimpleColumnValueReference(generationInfo.ColumnName).GreaterThanLiteral(generationInfo.MinLength.Value - 1) :
                                    check.And(new SimpleColumnValueReference(generationInfo.ColumnName).GreaterThanLiteral(generationInfo.MinLength.Value - 1));
                            break;
                        case CgConstants.NAME_UniqueIdentifier:
                            PropertyGenerationInfo nav = generationInfo.DeclaringType.Properties.FirstOrDefault(p => p.DbRelationship?.FkPropertyName == generationInfo.ColumnName);
                            if (nav is not null)
                            {
                                Write(" CONSTRAINT \"");
                                Write(nav.DbRelationship.Value.Name);
                                Write("\" REFERENCES \"");
                                Write(((EntityGenerationInfo)nav.ReferencedType).TableName);
                                Write("\"(\"Id\") ON DELETE RESTRICT");
                            }
                            break;
                        case CgConstants.NAME_RelatedEntity:
                        case CgConstants.NAME_RelatedEntity_Key:
                        case CgConstants.NAME_RelatedEntity_Type:
                            string name = generationInfo.DbRelationship?.FkPropertyName;
                            if (!generationInfo.DeclaringType.Properties.Any(p => p.Name == name))
                            {
                                Write(" CONSTRAINT \"");
                                Write(generationInfo.DbRelationship?.Name);
                                Write("\" REFERENCES \"");
                                Write(((EntityGenerationInfo)generationInfo.ReferencedType).TableName);
                                Write("\"(\"Id\") ON DELETE RESTRICT");
                            }
                            break;
                        case CgConstants.NAME_DriveType:
                            check = new SimpleColumnValueReference(generationInfo.ColumnName).NotLessThanLiteral(0).And(new SimpleColumnValueReference(generationInfo.ColumnName).LessThanLiteral(7));
                            break;
                        default:
                            check = null;
                            break;
                    }
                    break;
            }
            if (check is not null)
            {
                Write(" CHECK(");
                Write((generationInfo.AllowNull ? new NullCheckConstraint(new SimpleColumnValueReference(generationInfo.ColumnName), true).Or(check) : check).ToSqlString());
                Write(")");
            }

            if (generationInfo.DefaultValue.HasValue)
            {
                Write(" DEFAULT ");
                Write(generationInfo.DefaultValue.Value.SqlCode);
                comment = generationInfo.Source.Name.LocalName switch
                {
                    CgConstants.NAME_Enum or CgConstants.NAME_DriveType => generationInfo.DefaultValue.Value.CsCode,
                    _ => null,
                };
            }
            else
                comment = null;

            switch (generationInfo.Source.Name.LocalName)
            {
                case CgConstants.NAME_RelatedEntity:
                case CgConstants.NAME_RelatedEntity_Key:
                case CgConstants.NAME_RelatedEntity_Type:
                case CgConstants.NAME_UniqueIdentifier:
                case CgConstants.NAME_VolumeIdentifier:
                    Write(" COLLATE NOCASE");
                    break;
                case CgConstants.NAME_NVarChar:
                    if (generationInfo.IsCaseSensitive.HasValue)
                    {
                        Write(" COLLATE ");
                        Write(generationInfo.IsCaseSensitive.Value ? CgConstants.SQL_TYPENAME_BINARY : "NOCASE");
                    }
                    break;
            }
        }
        static CheckConstraint GetEnumCheckConstraintMinMax<T>(string colName, T minValue, T maxValue, Func<T, ConstantValueReference> toConstantValueReference, IEnumerable<T> values, Func<T, T, T> bitwiseOrFunc = null)
            where T : struct, IComparable<T>
        {
            T min = values.Min();
            T max = values.Max();
            if (bitwiseOrFunc is not null)
            {
                T av = values.Aggregate(bitwiseOrFunc);
                if (av.CompareTo(min) < 0)
                    min = av;
                else if (av.CompareTo(max) > 0)
                    max = av;
            }
            if (min.CompareTo(minValue) > 0)
            {
                if (max.CompareTo(maxValue) < 0)
                    return ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), toConstantValueReference(min))
                        .And(ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(colName), toConstantValueReference(max)));
                return ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), toConstantValueReference(min));
            }
            else if (max.CompareTo(maxValue) < 0)
                return ComparisonConstraint.NotGreaterThan(new SimpleColumnValueReference(colName), toConstantValueReference(max));
            return null;
        }
        //public abstract class CheckConstraint : IEquatable<CheckConstraint>
        //{
        //    public abstract bool IsCompound { get; }
        //    public static CheckConstraint Import(XElement checkElement)
        //    {
        //        if (checkElement is null)
        //            return null;
        //        if (checkElement.Name == Constants.XNAME_Check || checkElement.Name == Constants.XNAME_And)
        //        {
        //            CheckConstraint[] constraints = checkElement.Elements().Select(e => Import(e)).Where(c => c is not null).ToArray();
        //            if (constraints.Length == 1)
        //                return constraints[0];
        //            if (constraints.Length > 1)
        //                return new ComparisonGroup(false, constraints);
        //            return null;
        //        }
        //        if (checkElement.Name == Constants.XNAME_Or)
        //        {
        //            CheckConstraint[] constraints = checkElement.Elements().Select(e => Import(e)).Where(c => c is not null).ToArray();
        //            if (constraints.Length == 1)
        //                return constraints[0];
        //            if (constraints.Length > 1)
        //                return new ComparisonGroup(true, constraints);
        //            return null;
        //        }
        //        SimpleColumnValueReference name = new(checkElement.Attribute(Constants.XNAME_Name)?.Value);
        //        if (checkElement.Name == Constants.XNAME_IsNull)
        //            return new NullCheckConstraint(name, true);
        //        if (checkElement.Name == Constants.XNAME_NotNull)
        //            return new NullCheckConstraint(name, false);

        //        ColumnValueReference lValue = (checkElement.AttributeToBoolean(Constants.XNAME_Trimmed) ?? false) ? ColumnValueMethodResultReference.Trim(name) : name;
        //        if (checkElement.AttributeToBoolean(Constants.XNAME_Length) ?? false)
        //            lValue = ColumnValueMethodResultReference.Length(lValue);
        //        XElement other = checkElement.Elements().First();
        //        ValueReference rValue;
        //        if (other.Name == Constants.XNAME_OtherProperty)
        //        {
        //            name = new SimpleColumnValueReference(other.Attribute(Constants.XNAME_Name)?.Value);
        //            ColumnValueReference r = (other.AttributeToBoolean(Constants.XNAME_Trimmed) ?? false) ? ColumnValueMethodResultReference.Trim(name) : name;
        //            rValue = (other.AttributeToBoolean(Constants.XNAME_Length) ?? false) ? ColumnValueMethodResultReference.Length(r) : r;
        //        }
        //        else if (other.Name == Constants.XNAME_True)
        //            rValue = ConstantValueReference.Of(true);
        //        else if (other.Name == Constants.XNAME_False)
        //            rValue = ConstantValueReference.Of(false);
        //        else if (other.Name == Constants.XNAME_Now)
        //            rValue = ConstantValueReference.Now();
        //        else
        //        {
        //            string t = other.Attribute(Constants.XNAME_Value)?.Value;
        //            if (other.Name == Constants.XNAME_Byte)
        //                rValue = ConstantValueReference.Byte(t);
        //            else if (other.Name == Constants.XNAME_SByte)
        //                rValue = ConstantValueReference.SByte(t);
        //            else if (other.Name == Constants.XNAME_Short)
        //                rValue = ConstantValueReference.Short(t);
        //            else if (other.Name == Constants.XNAME_UShort)
        //                rValue = ConstantValueReference.UShort(t);
        //            else if (other.Name == Constants.XNAME_Int)
        //                rValue = ConstantValueReference.Int(t);
        //            else if (other.Name == Constants.XNAME_UInt)
        //                rValue = ConstantValueReference.UInt(t);
        //            else if (other.Name == Constants.XNAME_Long)
        //                rValue = ConstantValueReference.Long(t);
        //            else if (other.Name == Constants.XNAME_ULong)
        //                rValue = ConstantValueReference.ULong(t);
        //            else if (other.Name == Constants.XNAME_Double)
        //                rValue = ConstantValueReference.Double(t);
        //            else if (other.Name == Constants.XNAME_Float)
        //                rValue = ConstantValueReference.Float(t);
        //            else if (other.Name == Constants.XNAME_Decimal)
        //                rValue = ConstantValueReference.Decimal(t);
        //            else if (other.Name == Constants.XNAME_DateTime)
        //                rValue = ConstantValueReference.DateTime(t);
        //            else
        //                rValue = ConstantValueReference.String(t);
        //        }
        //        if (checkElement.Name == Constants.XNAME_LessThan)
        //            return ComparisonConstraint.LessThan(lValue, rValue);
        //        if (checkElement.Name == Constants.XNAME_NotGreaterThan)
        //            return ComparisonConstraint.NotGreaterThan(lValue, rValue);
        //        if (checkElement.Name == Constants.XNAME_NotEquals)
        //            return ComparisonConstraint.NotEqual(lValue, rValue);
        //        if (checkElement.Name == Constants.XNAME_NotLessThan)
        //            return ComparisonConstraint.NotLessThan(lValue, rValue);
        //        if (checkElement.Name == Constants.XNAME_GreaterThan)
        //            return ComparisonConstraint.GreaterThan(lValue, rValue);
        //        return ComparisonConstraint.AreEqual(lValue, rValue);
        //    }
        //    public abstract bool Equals(CheckConstraint other);
        //    public abstract string ToSqlString();
        //    public abstract string ToCsString();
        //    public virtual CheckConstraint And(CheckConstraint cc)
        //    {
        //        if (cc is null || Equals(cc))
        //            return this;
        //        return new ComparisonGroup(false, this, cc);
        //    }
        //    public virtual CheckConstraint Or(CheckConstraint cc)
        //    {
        //        if (cc is null || Equals(cc))
        //            return this;
        //        return new ComparisonGroup(true, this, cc);
        //    }
        //}
        //public abstract class ValueReference : IEquatable<ValueReference>
        //{
        //    public abstract bool Equals(ValueReference other);
        //    public abstract string ToSqlString();
        //    public abstract string ToCsString();
        //}
        //public abstract class ColumnValueReference : ValueReference, IEquatable<ColumnValueReference>
        //{
        //    public abstract string Name { get; }
        //    public abstract bool Equals(ColumnValueReference other);
        //    internal ComparisonConstraint LessThanLiteral(int value) => ComparisonConstraint.LessThan(this, ConstantValueReference.Of(value));
        //    internal ComparisonConstraint NotLessThanLiteral(int minValue) => ComparisonConstraint.NotLessThan(this, ConstantValueReference.Of(minValue));
        //    internal ComparisonConstraint GreaterThanLiteral(int value) => ComparisonConstraint.GreaterThan(this, ConstantValueReference.Of(value));
        //    internal ComparisonConstraint IsEqualTo(ValueReference value) => ComparisonConstraint.AreEqual(this, value);
        //    internal ColumnValueReference Length() => ColumnValueMethodResultReference.Length(this);
        //    internal ColumnValueReference Trimmed() => ColumnValueMethodResultReference.Trim(this);
        //}
        //public sealed class ConstantValueReference : ValueReference, IEquatable<ConstantValueReference>
        //{
        //    private ConstantValueReference(string sqlValue, string csValue)
        //    {
        //        SqlValue = sqlValue;
        //        CsValue = csValue;
        //    }
        //    public string SqlValue { get; }
        //    public string CsValue { get; }
        //    public bool Equals(ConstantValueReference other) => other is not null && (ReferenceEquals(this, other) || CsValue == other.CsValue);
        //    public override bool Equals(ValueReference other) => other is ConstantValueReference o && Equals(o);
        //    public override bool Equals(object obj) => obj is ConstantValueReference other && Equals(other);
        //    public override int GetHashCode() => CsValue.GetHashCode();
        //    public override string ToString() => SqlValue;
        //    public override string ToCsString() => CsValue;
        //    public override string ToSqlString() => SqlValue;
        //    public static ConstantValueReference Int(string value) => new(value, value);
        //    public static ConstantValueReference String(string value) => new($"N'{value}'", $"{value.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\"", "\\\"")}");
        //    internal static ConstantValueReference DateTime(string value) => Of(XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind));
        //    internal static ConstantValueReference Decimal(string value) => new(value, $"{value}m");
        //    internal static ConstantValueReference Float(string value) => new(value, $"{value}f");
        //    internal static ConstantValueReference Double(string value) => new(value, value.Contains('.') ? value : $"{value}.0");
        //    internal static ConstantValueReference ULong(string value) => new(value, $"{value}UL");
        //    internal static ConstantValueReference Long(string value) => new(value, $"{value}L");
        //    internal static ConstantValueReference UInt(string value) => new(value, $"{value}u");
        //    internal static ConstantValueReference UShort(string value) => new(value, $"(ushort){value}");
        //    internal static ConstantValueReference Short(string value) => new(value, $"(short){value}");
        //    internal static ConstantValueReference SByte(string value) => new(value, $"(sbyte){value}");
        //    internal static ConstantValueReference Byte(string value) => new(value, $"(byte){value}");
        //    internal static ConstantValueReference Now() => new("(datetime('now','localtime'))", "DateTime.Now");
        //    public static ConstantValueReference Of(bool value) => value ? new ConstantValueReference("1", "true") : new ConstantValueReference("0", "false");
        //    internal static ConstantValueReference Of(sbyte value) => new(value.ToString(), $"(sbyte){value}");
        //    internal static ConstantValueReference Of(byte value) => new(value.ToString(), $"(byte){value}");
        //    internal static ConstantValueReference Of(short value) => new(value.ToString(), $"(short){value}");
        //    internal static ConstantValueReference Of(ushort value) => new(value.ToString(), $"(ushort){value}");
        //    internal static ConstantValueReference Of(int value) => new(value.ToString(), value.ToString());
        //    internal static ConstantValueReference Of(uint value) => new(value.ToString(), $"{value}u");
        //    internal static ConstantValueReference Of(long value) => new(value.ToString(), $"{value}L");
        //    internal static ConstantValueReference Of(ulong value) => new(value.ToString(), $"{value}UL");
        //    internal static ConstantValueReference Of(DateTime dateTime) => new(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), $"new DateTime({dateTime.Year}, {dateTime.Month}, {dateTime.Day}, {dateTime.Hour}, {dateTime.Minute}, {dateTime.Second})");
        //}
        //public sealed class SimpleColumnValueReference : ColumnValueReference, IEquatable<SimpleColumnValueReference>
        //{
        //    private readonly string _name;
        //    public SimpleColumnValueReference(string name) { _name = name ?? ""; }
        //    public override string Name => _name;
        //    public bool Equals(SimpleColumnValueReference other) => other is not null && (ReferenceEquals(this, other) || _name == other._name);
        //    public override bool Equals(ColumnValueReference other) => other is SimpleColumnValueReference o && Equals(o);
        //    public override bool Equals(ValueReference other) => other is SimpleColumnValueReference o && Equals(o);
        //    public override bool Equals(object obj) => obj is SimpleColumnValueReference other && Equals(other);
        //    public override int GetHashCode() => _name.GetHashCode();
        //    public override string ToString() => ToSqlString();
        //    public override string ToCsString() => _name;
        //    public override string ToSqlString() => $"\"{_name}\"";
        //}
        //public sealed class ColumnValueMethodResultReference : ColumnValueReference, IEquatable<ColumnValueMethodResultReference>
        //{
        //    public const string SqlMethodName_trim = "trim";
        //    public const string CsNemberName_trim = "Trim()";
        //    public const string SqlMethodName_length = "length";
        //    public const string CsNemberName_length = "Length";
        //    private ColumnValueMethodResultReference(ColumnValueReference column, string methodName, string csMemberName)
        //    {
        //        Column = column;
        //        SqlMethodName = methodName;
        //    }
        //    public ColumnValueReference Column { get; }
        //    public string SqlMethodName { get; }
        //    public string CsMemberName { get; }
        //    public override string Name => Column.Name;
        //    public static ColumnValueMethodResultReference LengthTrimmed(ColumnValueReference column) => (column is ColumnValueMethodResultReference m) ?
        //        ((m.SqlMethodName == SqlMethodName_length) ? m : Length((m.SqlMethodName == SqlMethodName_trim) ? m : Trim(column))) : Length(Trim(column));
        //    public static ColumnValueMethodResultReference Trim(ColumnValueReference column)
        //    {
        //        if (column is ColumnValueMethodResultReference m)
        //        {
        //            if (m.SqlMethodName == SqlMethodName_trim)
        //                return m;
        //            if (m.SqlMethodName == SqlMethodName_length)
        //            {
        //                if (m.Column is ColumnValueMethodResultReference c && c.SqlMethodName == SqlMethodName_trim)
        //                    return m;
        //                return Length(Trim(m.Column));
        //            }
        //        }
        //        return new ColumnValueMethodResultReference(column, SqlMethodName_trim, CsNemberName_trim);
        //    }
        //    public static ColumnValueMethodResultReference Length(ColumnValueReference column) => (column is ColumnValueMethodResultReference m && m.SqlMethodName == SqlMethodName_length) ? m :
        //        new ColumnValueMethodResultReference(column, SqlMethodName_length, CsNemberName_length);
        //    public bool Equals(ColumnValueMethodResultReference other) => other is not null && (ReferenceEquals(this, other) || (SqlMethodName == other.SqlMethodName && Column.Equals(other.Column)));
        //    public override bool Equals(ColumnValueReference other) => other is ColumnValueMethodResultReference o && Equals(o);
        //    public override bool Equals(ValueReference other) => other is ColumnValueMethodResultReference o && Equals(o);
        //    public override bool Equals(object obj) => obj is ColumnValueMethodResultReference other && Equals(other);
        //    public override int GetHashCode() { unchecked { return (SqlMethodName.GetHashCode() * 3) ^ Column.GetHashCode(); } }
        //    public override string ToString() => ToSqlString();
        //    public override string ToSqlString() => $"{SqlMethodName}({Column.ToSqlString()})";
        //    public override string ToCsString() => $"{Column.ToCsString()}.{CsMemberName}";
        //}
        //public sealed class NullCheckConstraint : CheckConstraint, IEquatable<NullCheckConstraint>
        //{
        //    public NullCheckConstraint(SimpleColumnValueReference column, bool isNull)
        //    {
        //        Column = column;
        //        IsNull = isNull;
        //    }
        //    public SimpleColumnValueReference Column { get; }
        //    public bool IsNull { get; }
        //    public override bool IsCompound => false;
        //    public bool Equals(NullCheckConstraint other) => other is not null && (ReferenceEquals(this, other) || (IsNull == other.IsNull && Column.Equals(other.Column)));
        //    public override bool Equals(CheckConstraint other) => other is NullCheckConstraint o && Equals(o);
        //    public override bool Equals(object obj) => obj is NullCheckConstraint other && Equals(other);
        //    public override int GetHashCode() { unchecked { return (IsNull ? 0 : 3) ^ Column.GetHashCode(); } }
        //    public override string ToString() => ToSqlString();
        //    public override string ToSqlString() => IsNull ? $"{Column.ToSqlString()} IS NULL" : $"{Column.ToSqlString()} IS NOT NULL";
        //    public override string ToCsString() => IsNull ? $"{Column.ToCsString()} is null" : $"{Column.ToCsString()} is not null";
        //}
        //public sealed class ComparisonConstraint : CheckConstraint, IEquatable<ComparisonConstraint>
        //{
        //    private ComparisonConstraint(ColumnValueReference lValue, string sqlOp, string csOp, ValueReference rValue)
        //    {
        //        LValue = lValue;
        //        SqlOp = sqlOp;
        //        CsOp = csOp;
        //        RValue = rValue;
        //    }
        //    public ColumnValueReference LValue { get; }
        //    public string SqlOp { get; }
        //    public string CsOp { get; }
        //    public ValueReference RValue { get; }
        //    public override bool IsCompound => false;
        //    public static ComparisonConstraint AreEqual(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "=", "==", rValue);
        //    public static ComparisonConstraint NotEqual(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<>", "!=", rValue);
        //    public static ComparisonConstraint LessThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<", "<", rValue);
        //    public static ComparisonConstraint NotGreaterThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<=", "<=", rValue);
        //    public static ComparisonConstraint GreaterThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, ">", ">", rValue);
        //    public static ComparisonConstraint NotLessThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, ">=", ">=", rValue);
        //    public bool Equals(ComparisonConstraint other) => other is not null && (ReferenceEquals(this, other) || (LValue.Equals(other.LValue) && SqlOp == other.SqlOp && RValue.Equals(other.RValue)));
        //    public override bool Equals(CheckConstraint other) => other is ComparisonConstraint o && Equals(o);
        //    public override bool Equals(object obj) => obj is ComparisonConstraint other && Equals(other);
        //    public override int GetHashCode() { unchecked { return (((LValue.GetHashCode() * 3) ^ SqlOp.GetHashCode()) * 5) ^ RValue.GetHashCode(); } }
        //    public override string ToString() => ToSqlString();
        //    public override string ToSqlString() => $"{LValue.ToSqlString()}{SqlOp}{RValue.ToSqlString()}";
        //    public override string ToCsString() => $"{LValue.ToCsString()} {CsOp} {RValue.ToCsString()}";
        //}
        //public sealed class ComparisonGroup : CheckConstraint, IEquatable<ComparisonGroup>
        //{
        //    public ComparisonGroup(bool isOr, params CheckConstraint[] constraints)
        //    {
        //        if (constraints is null || (constraints = constraints.Where(c => c is not null)
        //                .SelectMany(c => (c is ComparisonGroup g && g.IsOr == isOr) ? g.Constraints : Enumerable.Repeat(c, 1)).ToArray()).Length == 0)
        //            throw new ArgumentOutOfRangeException(nameof(constraints));
        //        using IEnumerator<CheckConstraint> enumerator = (constraints ?? Array.Empty<CheckConstraint>()).Where(c => c is not null).GetEnumerator();
        //        if (!enumerator.MoveNext())
        //            throw new ArgumentOutOfRangeException(nameof(constraints));
        //        IsOr = isOr;
        //        List<CheckConstraint> checkConstraints = new();
        //        do
        //        {
        //            if (!checkConstraints.Contains(enumerator.Current))
        //                checkConstraints.Add(enumerator.Current);
        //        }
        //        while (enumerator.MoveNext());
        //        Constraints = new ReadOnlyCollection<CheckConstraint>(checkConstraints);
        //    }
        //    public bool IsOr { get; }
        //    public ReadOnlyCollection<CheckConstraint> Constraints { get; }
        //    public override bool IsCompound => Constraints.Count > 1;
        //    public override CheckConstraint And(CheckConstraint cc)
        //    {
        //        if (cc is null || Equals(cc) || Constraints.Contains(cc))
        //            return this;
        //        if (IsOr)
        //            return base.And(cc);
        //        return new ComparisonGroup(false, Constraints.Concat((cc is ComparisonGroup cg && !cg.IsOr) ? cg.Constraints : Enumerable.Repeat(cc, 1)).ToArray());
        //    }
        //    public override CheckConstraint Or(CheckConstraint cc)
        //    {
        //        if (cc is null || Equals(cc) || Constraints.Contains(cc))
        //            return this;
        //        if (!IsOr)
        //            return base.Or(cc);
        //        return new ComparisonGroup(true, Constraints.Concat((cc is ComparisonGroup cg && cg.IsOr) ? cg.Constraints : Enumerable.Repeat(cc, 1)).ToArray());
        //    }
        //    public bool Equals(ComparisonGroup other)
        //    {
        //        if (other is null)
        //            return false;
        //        if (ReferenceEquals(this, other))
        //            return true;
        //        if (IsOr != other.IsOr || Constraints.Count != other.Constraints.Count)
        //            return false;
        //        return Constraints.All(c => other.Constraints.Contains(c));
        //    }
        //    public override bool Equals(CheckConstraint other) => other is ComparisonGroup g && Equals(g);
        //    public override bool Equals(object obj) => obj is ComparisonGroup other && Equals(other);
        //    public override int GetHashCode()
        //    {
        //        if (Constraints.Count == 0)
        //            return IsOr ? 1 : 0;
        //        int seed = Constraints.Count + 1;
        //        int prime = (seed & 0xffff).FindPrimeNumber();
        //        seed = (prime + 1).FindPrimeNumber();
        //        return new int[] { IsOr ? 1 : 0 }.Concat(Constraints.Select(c => c.GetHashCode())).Aggregate(seed, (a, i) =>
        //        {
        //            unchecked { return (a * prime) ^ i; }
        //        });
        //    }
        //    public override string ToString() => ToSqlString();
        //    public override string ToSqlString()
        //    {
        //        if (Constraints.Count == 0)
        //            return "";
        //        if (Constraints.Count == 1)
        //            return Constraints[0].ToSqlString();
        //        return string.Join(IsOr ? " OR " : " AND ", Constraints.Select(c => c.IsCompound ? $"({c.ToSqlString()})" : c.ToSqlString()));
        //    }
        //    public override string ToCsString()
        //    {
        //        if (Constraints.Count == 0)
        //            return "";
        //        if (Constraints.Count == 1)
        //            return Constraints[0].ToCsString();
        //        return string.Join(IsOr ? " || " : " && ", Constraints.Select(c => c.IsCompound ? $"({c.ToCsString()})" : c.ToCsString()));
        //    }
        //}

        #endregion

    }
}
