using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (Constants.NewLineRegex.IsMatch(text))
            {
                string[] lines = Constants.NewLineRegex.Split(text);
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
                    XmlReaderSettings readerSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
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

            foreach (XElement enumElement in entityDefinitionsElement.Elements().Elements(Constants.XNAME_EnumTypes).Elements())
            {
                string typeName = enumElement.Attribute(Constants.XNAME_Name)?.Value;
                XName elementName = enumElement.Name;
                if (string.IsNullOrWhiteSpace(typeName))
                    WriteLine($"#warning {Constants.XNAME_Name} attribute missing or empty for /{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{Constants.XNAME_EnumTypes}/{elementName}[{(enumElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)}]");
                else
                {
                    string enumXPath = $"/{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{Constants.XNAME_EnumTypes}/{elementName}[@Name='{typeName}']";
                    foreach (XElement fieldElement in enumElement.Elements(Constants.XNAME_Field))
                    {
                        string fieldName = fieldElement.Attribute(Constants.XNAME_Name)?.Value;
                        if (string.IsNullOrWhiteSpace(fieldName))
                            WriteLine($"#warning {Constants.XNAME_Name} attribute missing or empty for {enumXPath}/{Constants.XNAME_Field}[{(fieldElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == Constants.XNAME_Field) + 1)}]");
                        else
                        {
                            string fieldXPath = $"{enumXPath}/{Constants.XNAME_Field}[@Name='{fieldName}']";
                            XAttribute attribute = fieldElement.Attribute(Constants.XNAME_FullName);
                            string expected = $"{typeName}.{fieldName}";
                            if (attribute is null)
                            {
                                WriteLine($"#warning {Constants.XNAME_FullName} attribute missing for {fieldXPath}");
                                fieldElement.SetAttributeValue(Constants.XNAME_FullName, expected);
                            }
                            else if (attribute.Value != expected)
                            {
                                WriteLine($"#warning {Constants.XNAME_FullName} attribute does not match the value \"{expected}\" for {fieldXPath}");
                                fieldElement.SetAttributeValue(Constants.XNAME_FullName, expected);
                            }
                        }
                    }
                }
            }

            foreach (XElement entityElement in entityDefinitionsElement.Elements().Elements(Constants.XNAME_Entity))
            {
                string typeName = entityElement.Attribute(Constants.XNAME_Name)?.Value;
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    XName elementName = entityElement.Name;
                    WriteLine($"#warning {Constants.XNAME_Name} attribute missing or empty for /{entityElement.Parent.Parent.Name}/{entityElement.Parent.Name}/{Constants.XNAME_Entity}[(entityElement.NodesBeforeSelf().OfType<XElement>().Count()]");
                }
                else
                {
                    string typeXPath = $"/{entityElement.Parent.Parent.Name}/{entityElement.Parent.Name}/{Constants.XNAME_Entity}[@Name='{typeName}']";
                    foreach (XElement propertyElement in entityElement.Elements(Constants.XNAME_Properties).Elements())
                    {
                        XName elementName = propertyElement.Name;
                        string propertyName = propertyElement.Attribute(Constants.XNAME_Name)?.Value;
                        if (string.IsNullOrWhiteSpace(propertyName))
                            WriteLine($"#warning {Constants.XNAME_Name} attribute missing or empty for {typeXPath}/{elementName}[{(propertyElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)}]");
                        else
                        {
                            string propertyXPath = $"{typeXPath}/{elementName}[@Name='{propertyName}']";
                            XAttribute attribute = propertyElement.Attribute(Constants.XNAME_FullName);
                            string expected = $"{typeName}.{propertyName}";
                            if (attribute is null)
                            {
                                WriteLine($"#warning {Constants.XNAME_FullName} attribute missing for {propertyXPath}");
                                propertyElement.SetAttributeValue(Constants.XNAME_FullName, expected);
                            }
                            else if (attribute.Value != expected)
                            {
                                WriteLine($"#warning {Constants.XNAME_FullName} attribute does not match the value \"{expected}\" for {propertyXPath}");
                                propertyElement.SetAttributeValue(Constants.XNAME_FullName, expected);
                            }
                        }
                    }
                }
            }

            PushIndent("    "); 
            bool isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(Constants.XNAME_Root).Elements(Constants.XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(Constants.XNAME_Root).Elements(Constants.XNAME_Entity), isSubsequentMember);
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
            isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(Constants.XNAME_Local).Elements(Constants.XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(Constants.XNAME_Local).Elements(Constants.XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
}}

namespace <#=DefaultNamespace#>.Upstream
{{
");
            #endregion

            PushIndent("    ");
            isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(Constants.XNAME_Upstream).Elements(Constants.XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(Constants.XNAME_Upstream).Elements(Constants.XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
}}
");
            #endregion
        }

        void WriteAmbientValueAttribute(XElement memberElement)
        {
            string ambientValue = memberElement.Elements(Constants.XNAME_AmbientString).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (ambientValue is not null)
            {
                Write("[AmbientValue(\"");
                Write(ambientValue.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n"));
                WriteLine("\")]");
                return;
            }
            foreach (XName name in new[] { Constants.XNAME_AmbientEnum, Constants.XNAME_AmbientBoolean, Constants.XNAME_AmbientInt })
            {
                ambientValue = memberElement.Elements(name).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ambientValue))
                {
                    Write("[AmbientValue(");
                    Write(ambientValue);
                    WriteLine(")]");
                    return;
                }
            }
            ambientValue = memberElement.Elements(Constants.XNAME_AmbientUInt).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("u)]");
                return;
            }
            ambientValue = memberElement.Elements(Constants.XNAME_AmbientLong).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("L)]");
                return;
            }
            ambientValue = memberElement.Elements(Constants.XNAME_AmbientULong).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("UL)]");
                return;
            }
            ambientValue = memberElement.Elements(Constants.XNAME_AmbientDouble).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine(ambientValue.Contains(".") ? ")]" : ".0)]");
                return;
            }
            ambientValue = memberElement.Elements(Constants.XNAME_AmbientFloat).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("f)]");
                return;
            }
            foreach (XName name in new[] { Constants.XNAME_AmbientByte, Constants.XNAME_AmbientSByte, Constants.XNAME_AmbientShort, Constants.XNAME_AmbientUShort })
            {
                ambientValue = memberElement.Elements(name).Attributes(Constants.XNAME_Value).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ambientValue))
                {
                    Write("[AmbientValue((");
                    Write(name.LocalName.Substring(7).ToLower());
                    Write(")");
                    Write(ambientValue);
                    WriteLine(")]");
                    return;
                }
            }
        }
        void WriteDisplayAttribute(string memberName, Func<XName, IEnumerable<XAttribute>> getAttributes, string typeName = null)
        {
            string displayNameResource = getAttributes(Constants.XNAME_DisplayNameResource).Select(a => a.Value).DefaultIfEmpty(string.IsNullOrWhiteSpace(typeName) ?
                $"DisplayName_{memberName}" : $"DisplayName_{typeName}_{memberName}").First();
            string descriptionResource = getAttributes(Constants.XNAME_DescriptionResource).Select(a => a.Value).FirstOrDefault();
            string resourceType = getAttributes(Constants.XNAME_ResourceType).Select(a => a.Value).DefaultIfEmpty("Properties.Resources").First();
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
                foreach (string lt in Constants.NewLineRegex.Split(xmlDocElement.ToString(SaveOptions.None)))
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
                if (!Constants.TrailingEmptyLine.IsMatch(lastNode.Value))
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
                    if (Constants.LeadingEmptyLine.Match(firstNode.Value).Length < firstNode.Value.Length)
                        firstNode.Value = $"\n{firstNode.Value}";
                }
                else if (!Constants.LeadingEmptyLine.IsMatch(firstNode.Value))
                    firstNode.Value = $"\n{firstNode.Value}";
            }
            else
                xmlDocElement.FirstNode.AddBeforeSelf(new XText("\n"));
            IEnumerable<string> lines = Constants.NewLineRegex.Split(xmlDocElement.ToString(SaveOptions.None));
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

            if (Constants.LeadingWsRegex.IsMatch(lines.Skip(1).First()))
            {
                int indent = lines.Skip(1).Reverse().Skip(1).Reverse().Select(s => Constants.LeadingWsRegex.Match(s)).Select(m => m.Success ? m.Length : 0).Min();
                if (indent > 0)
                    lines = lines.Take(1).Concat(lines.Skip(1).Reverse().Skip(1).Reverse().Select(s => s.Substring(indent))).Concat(lines.Reverse().Take(1));
            }
            else
            {
                int indent = lines.Skip(2).Reverse().Skip(1).Reverse().Select(s => Constants.LeadingWsRegex.Match(s)).Select(m => m.Success ? m.Length : 0).Min();
                if (indent > 0)
                    lines = lines.Take(2).Concat(lines.Skip(2).Reverse().Skip(1).Reverse().Select(s => s.Substring(indent))).Concat(lines.Reverse().Take(1));
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
            GenerateXmlDoc(enumElement.Element(Constants.XNAME_summary));
            GenerateXmlDoc(enumElement.Element(Constants.XNAME_remarks));
            foreach (XElement e in enumElement.Elements(Constants.XNAME_seealso))
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
                GenerateXmlDoc(field.Source.Element(Constants.XNAME_summary));
                GenerateXmlDoc(field.Source.Element(Constants.XNAME_remarks));
                foreach (XElement e in field.Source.Elements(Constants.XNAME_seealso))
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
            XElement commentDocElement = property.Source.Element(Constants.XNAME_summary) ?? property.Inherited.Select(p => p.Source).Elements(Constants.XNAME_summary).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No summary element found for {typeName}.{property.Name}");
            else
                GenerateXmlDoc(commentDocElement);
            commentDocElement = property.Source.Element(Constants.XNAME_value) ?? property.Inherited.Select(p => p.Source).Elements(Constants.XNAME_value).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No value element found for {typeName}.{property.Name}");
            else
                GenerateXmlDoc(commentDocElement);
            commentDocElement = property.Source.Element(Constants.XNAME_remarks) ?? property.Inherited.Select(p => p.Source).Elements(Constants.XNAME_remarks).FirstOrDefault();
            if (commentDocElement is not null)
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in property.Source.Elements(Constants.XNAME_seealso).Concat(property.Inherited.Select(p => p.Source).Elements(Constants.XNAME_seealso)).Distinct())
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
                case Constants.NAME_Byte:
                case Constants.NAME_SByte:
                case Constants.NAME_Short:
                case Constants.NAME_UShort:
                case Constants.NAME_Int:
                case Constants.NAME_UInt:
                case Constants.NAME_Long:
                case Constants.NAME_ULong:
                case Constants.NAME_Float:
                case Constants.NAME_Double:
                    Write(property.Source.Name.LocalName.ToLower());
                    Write(property.AllowNull ? "? " : " ");
                    break;
                case Constants.NAME_Text:
                case Constants.NAME_NVarChar:
                    Write("string ");
                    break;
                case Constants.NAME_VolumeIdentifier:
                case Constants.NAME_DriveType:
                case Constants.NAME_MD5Hash:
                case Constants.NAME_DateTime:
                    Write(property.Source.Name.LocalName);
                    Write(property.AllowNull ? "? " : " ");
                    break;
                case Constants.NAME_ByteValues:
                case Constants.NAME_MultiStringValue:
                    Write(property.Source.Name.LocalName);
                    Write(" ");
                    break;
                case Constants.NAME_UniqueIdentifier:
                case Constants.NAME_NewIdNavRef:
                    Write(property.AllowNull ? "Guid? " : "Guid ");
                    break;
                case Constants.NAME_Bit:
                    Write(property.AllowNull ? "bool? " : "bool ");
                    break;
                case Constants.NAME_Enum:
                    Write(property.Source.Attribute(Constants.XNAME_Type)?.Value);
                    Write(property.AllowNull ? "? " : " ");
                    break;
                case Constants.NAME_CollectionNavigation:
                case Constants.NAME_NewCollectionNavigation:
                    Write("IEnumerable<");
                    Write(property.ReferencedType.CsName);
                    Write("> ");
                    break;
                case Constants.NAME_RelatedEntity:
                case Constants.NAME_NewRelatedEntity:
                case Constants.NAME_NewRelatedEntityKey:
                    Write(property.ReferencedType.CsName);
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
            XElement commentDocElement = entityElement.Element(Constants.XNAME_summary) ?? generationInfo.BaseTypes.Select(b => b.Entity.Source).Elements(Constants.XNAME_summary).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No summary element found for {generationInfo.CsName}");
            else
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in entityElement.Elements(Constants.XNAME_typeparam).Concat(generationInfo.BaseTypes.Select(b => b.Entity.Source).Elements(Constants.XNAME_typeparam)).Distinct())
                GenerateXmlDoc(e);
            commentDocElement = entityElement.Element(Constants.XNAME_remarks) ?? generationInfo.BaseTypes.Select(b => b.Entity.Source).Elements(Constants.XNAME_remarks).FirstOrDefault();
            if (commentDocElement is not null)
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in entityElement.Elements(Constants.XNAME_seealso).Concat(generationInfo.BaseTypes.Select(b => b.Entity.Source).Select(s => new XElement(Constants.XNAME_seealso, new XAttribute(Constants.XNAME_cref, s)))))
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            IEnumerable<PropertyGenerationInfo> properties = generationInfo.Properties.Where(p => ReferenceEquals(p.ReferencedType, generationInfo));
            Write("public interface ");
            if (generationInfo.BaseTypes.Count > 0)
            {
                Write(generationInfo.CsName);
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
                    Write(generationInfo.CsName);
                    WriteLine("{ }");
                    return;
                }
                WriteLine(generationInfo.CsName);
            }

            WriteLine("{");
            PushIndent("    ");

            GenerateProperty(generationInfo.CsName, properties.First());
            foreach (PropertyGenerationInfo p in properties.Skip(1))
            {
                WriteLine("");
                GenerateProperty(generationInfo.CsName, p);
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
                typeName = Constants.SQL_TYPENAME_BLOB;
                return typeName;
            }

            switch (propertyElement.Name.LocalName)
            {
                case Constants.NAME_Enum:
                    string sqlType = PropertyElementToSqlType(EntityDefinitionsDocument.FindLocalEnumTypeByName(propertyElement.Attribute(Constants.XNAME_Type)?.Value), out typeName, out _);
                    isNumeric = false;
                    return sqlType;
                case Constants.NAME_UniqueIdentifier:
                case Constants.NAME_RelatedEntity:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_UNIQUEIDENTIFIER;
                    return typeName;
                case Constants.NAME_NVarChar:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_NVARCHAR;
                    return $"{typeName}({propertyElement.Attribute(Constants.XNAME_MaxLength)?.Value})";
                case Constants.NAME_VolumeIdentifier:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_NVARCHAR;
                    return $"{typeName}(1024)";
                case Constants.NAME_MultiStringValue:
                case Constants.NAME_Text:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_TEXT;
                    return typeName;
                case Constants.NAME_DateTime:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_DATETIME;
                    return typeName;
                case Constants.NAME_TimeSpan:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_TIME;
                    return typeName;
                case Constants.NAME_Bit:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_BIT;
                    return typeName;
                case Constants.NAME_ByteValues:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_VARBINARY;
                    return $"{typeName}({propertyElement.Attribute(Constants.XNAME_MaxLength)?.Value})";
                case Constants.NAME_MD5Hash:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_BINARY;
                    return $"{typeName}(16)";
                case Constants.NAME_DriveType:
                    isNumeric = false;
                    typeName = Constants.SQL_TYPENAME_UNSIGNED_TINYINT;
                    return typeName;
                case Constants.NAME_Byte:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_UNSIGNED_TINYINT;
                    return typeName;
                case Constants.NAME_SByte:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_TINYINT;
                    return typeName;
                case Constants.NAME_Short:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_SMALLINT;
                    return typeName;
                case Constants.NAME_UShort:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_UNSIGNED_SMALLINT;
                    return typeName;
                case Constants.NAME_Int:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_INT;
                    return typeName;
                case Constants.NAME_UInt:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_UNSIGNED_INT;
                    return typeName;
                case Constants.NAME_Long:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_BIGINT;
                    return typeName;
                case Constants.NAME_ULong:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_UNSIGNED_BIGINT;
                    return typeName;
                case Constants.NAME_Float:
                case Constants.NAME_Double:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_REAL;
                    return typeName;
                case Constants.NAME_Decimal:
                    isNumeric = true;
                    typeName = Constants.SQL_TYPENAME_NUMERIC;
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
            if (isNumeric)
            {
                string minValue = sources.Attributes(Constants.XNAME_MinValue).Select(a => a.Value.Trim()).DefaultIfEmpty("").First();
                string maxValue = sources.Attributes(Constants.XNAME_MaxValue).Select(a => a.Value.Trim()).DefaultIfEmpty("").First();
                if (minValue.Length > 0)
                {
                    check = propertyTypeName switch
                    {
                        Constants.NAME_Byte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Byte(minValue)),
                        Constants.NAME_SByte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.SByte(minValue)),
                        Constants.NAME_Short => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Short(minValue)),
                        Constants.NAME_UShort => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UShort(minValue)),
                        Constants.NAME_UInt => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UInt(minValue)),
                        Constants.NAME_Long => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Long(minValue)),
                        Constants.NAME_ULong => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.ULong(minValue)),
                        Constants.NAME_Decimal => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Decimal(minValue)),
                        Constants.NAME_Double => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Double(minValue)),
                        Constants.NAME_Float => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Float(minValue)),
                        _ => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Int(minValue)),
                    };
                }
                if (maxValue.Length > 0)
                {
                    ComparisonConstraint cc = propertyTypeName switch
                    {
                        Constants.NAME_Byte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Byte(maxValue)),
                        Constants.NAME_SByte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.SByte(maxValue)),
                        Constants.NAME_Short => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Short(maxValue)),
                        Constants.NAME_UShort => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UShort(maxValue)),
                        Constants.NAME_UInt => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UInt(maxValue)),
                        Constants.NAME_Long => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Long(maxValue)),
                        Constants.NAME_ULong => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.ULong(maxValue)),
                        Constants.NAME_Decimal => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Decimal(maxValue)),
                        Constants.NAME_Double => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Double(maxValue)),
                        Constants.NAME_Float => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Float(maxValue)),
                        _ => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Int(maxValue)),
                    };
                    check = (check is null) ? cc : check.And(cc);
                }
            }
            else
            {
                int minLength;
                switch (propertyTypeName)
                {
                    case Constants.NAME_Enum:
                        XElement enumType = EntityDefinitionsDocument.FindLocalEnumTypeByName(baseProperty.Attribute(Constants.XNAME_Type)?.Value);
                        IEnumerable<string> enumValueStrings = enumType.Elements(Constants.XNAME_Field).Attributes(Constants.XNAME_Value).Select(a => a.Value);
                        bool isFlags = baseProperty.AttributeToBoolean(Constants.XNAME_IsFlags) ?? false;
                        check = typeName switch
                        {
                            Constants.SQL_TYPENAME_TINYINT => GetEnumCheckConstraintMinMax(colName, sbyte.MinValue, sbyte.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToSByte(v)), isFlags ? (x, y) => (sbyte)(x | y) : null),
                            Constants.SQL_TYPENAME_UNSIGNED_TINYINT => GetEnumCheckConstraintMinMax(colName, byte.MinValue, byte.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToByte(v)), isFlags ? (x, y) => (byte)(x | y) : null),
                            Constants.SQL_TYPENAME_SMALLINT => GetEnumCheckConstraintMinMax(colName, short.MinValue, short.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToInt16(v)), isFlags ? (x, y) => (short)(x | y) : null),
                            Constants.SQL_TYPENAME_UNSIGNED_SMALLINT => GetEnumCheckConstraintMinMax(colName, ushort.MinValue, ushort.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToUInt16(v)), isFlags ? (x, y) => (ushort)(x | y) : null),
                            Constants.SQL_TYPENAME_UNSIGNED_INT => GetEnumCheckConstraintMinMax(colName, uint.MinValue, uint.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToUInt32(v)), isFlags ? (x, y) => x | y : null),
                            Constants.SQL_TYPENAME_BIGINT => GetEnumCheckConstraintMinMax(colName, long.MinValue, long.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToInt64(v)), isFlags ? (x, y) => x | y : null),
                            Constants.SQL_TYPENAME_UNSIGNED_BIGINT => GetEnumCheckConstraintMinMax(colName, ulong.MinValue, ulong.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToUInt64(v)), isFlags ? (x, y) => x | y : null),
                            _ => GetEnumCheckConstraintMinMax(colName, int.MinValue, int.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToInt32(v)), isFlags ? (x, y) => x | y : null),
                        };
                        break;
                    case Constants.NAME_NVarChar:
                        if (sources.AttributeToBoolean(Constants.XNAME_IsNormalized) ?? false)
                            check = new SimpleColumnValueReference(colName).Trimmed().Length().IsEqualTo(new SimpleColumnValueReference(colName).Length());
                        minLength = sources.AttributeToInt32(Constants.XNAME_MinLength) ?? 0;
                        if (minLength > 0)
                            check = (check is null) ? new SimpleColumnValueReference(colName).GreaterThanLiteral(minLength - 1) : check.And(new SimpleColumnValueReference(colName).GreaterThanLiteral(minLength - 1));
                        break;
                    case Constants.NAME_ByteValues:
                        minLength = sources.AttributeToInt32(Constants.XNAME_MinLength) ?? 0;
                        if (minLength > 0)
                            check = new SimpleColumnValueReference(colName).Length().GreaterThanLiteral(minLength - 1);
                        int maxLength = sources.AttributeToInt32(Constants.XNAME_MaxLength) ?? 0;
                        if (maxLength > 0)
                            check = new SimpleColumnValueReference(colName).Length().LessThanLiteral(maxLength + 1);
                        break;
                    case Constants.NAME_UniqueIdentifier:
                        IEnumerable<(string ConstraintName, string TableName)> relatedEntities = sources.Attributes(Constants.XNAME_Navigation).Select(a =>
                        {
                            string n = a.Value;
                            XElement[] refersTo = sources.Select(p => p.FindCurrentEntityPropertyByName(n)).Where(e => e is not null).ToArray();
                            return (
                                ConstraintName: refersTo.Attributes(Constants.XNAME_ConstraintName).Select(e => e.Value).FirstOrDefault(),
                                TableName: refersTo.Attributes(Constants.XNAME_Reference).Select(e => EntityDefinitionsDocument.FindLocalEntityByName(e.Value)?.Attribute(Constants.XNAME_TableName)?.Value).Where(n => n is not null).FirstOrDefault()
                            );
                        }).Where(t => !(string.IsNullOrEmpty(t.ConstraintName) || string.IsNullOrEmpty(t.TableName)));
                        if (relatedEntities.Any())
                        {
                            (string ConstraintName, string TableName) = relatedEntities.First();
                            Write(" CONSTRAINT \"");
                            Write(ConstraintName);
                            Write("\" REFERENCES \"");
                            Write(TableName);
                            Write("\"(\"Id\") ON DELETE RESTRICT");
                        }
                        break;
                    case Constants.NAME_RelatedEntity:
                        string constraintName = sources.Elements(Constants.XNAME_DbRelationship).Attributes(Constants.XNAME_Name).Select(e => e.Value).FirstOrDefault();
                        if (constraintName is not null)
                        {
                            Write(" CONSTRAINT \"");
                            Write(constraintName);
                            Write("\" REFERENCES \"");
                            Write(sources.Attributes(Constants.XNAME_Reference).Select(e => EntityDefinitionsDocument.FindLocalEntityByName(e.Value)?.Attribute(Constants.XNAME_TableName)?.Value)
                                .Where(n => n is not null).First());
                            Write("\"(\"Id\") ON DELETE RESTRICT");
                        }
                        break;
                    case Constants.NAME_DriveType:
                        check = new SimpleColumnValueReference(colName).NotLessThanLiteral(0).And(new SimpleColumnValueReference(colName).LessThanLiteral(7));
                        break;
                    default:
                        check = null;
                        break;
                }
            }
            if (generationInfo.DbRelationship.HasValue)
            {
                Write(" CONSTRAINT \"");
                Write(generationInfo.DbRelationship.Value.Name);
                Write("\" REFERENCES \"");
                Write(TableName);
                Write("\"(\"Id\") ON DELETE RESTRICT");
            }
            if (check is not null)
            {
                Write(" CHECK(");
                Write((allowNull ? new NullCheckConstraint(new SimpleColumnValueReference(colName), true).Or(check) : check).ToSqlString());
                Write(")");
            }

            if (defaultNull)
            {
                comment = null;
                Write(" DEFAULT NULL");
            }
            else
            {
                //string defaultValue = sources.Elements(Constants.XNAME_Default).Select(e => e.Value).FirstOrDefault();
                switch (propertyTypeName)
                {
                    case Constants.NAME_Enum:
                        string defaultEnumField = sources.ElementToString(Constants.XNAME_Default);
                        if (defaultEnumField is not null)
                        {
                            Write(" DEFAULT ");
                            Write(EntityDefinitionsDocument.FindLocalFieldByFullName(defaultEnumField)?.Attribute(Constants.XNAME_Value)?.Value);
                            comment = defaultEnumField.Trim();
                        }
                        else
                            comment = null;
                        break;
                    case Constants.NAME_Char:
                    case Constants.NAME_NVarChar:
                    case Constants.NAME_VolumeIdentifier:
                    case Constants.NAME_MultiStringValue:
                    case Constants.NAME_Text:
                        comment = null;
                        string defaultText = sources.ElementToString(Constants.XNAME_Default);
                        if (defaultText is not null)
                        {
                            Write(" DEFAULT '");
                            Write(defaultText.Replace("'", "''"));
                            Write("'");
                        }
                        break;
                    case Constants.NAME_TimeSpan:
                        comment = null;
                        if (sources.Elements(Constants.XNAME_DefaultZero).Any())
                            Write(" DEFAULT (time('00:00:00.000'))");
                        else
                        {
                            TimeSpan? defaultTimeSpan = sources.ElementToTimeSpan(Constants.XNAME_Default);
                            if (defaultTimeSpan.HasValue)
                            {
                                Write(" DEFAULT ");
                                Write(defaultTimeSpan.Value.ToString(@"\'hh\:mm\:ss\.fff\'"));
                            }
                        }
                        break;
                    case Constants.NAME_DateTime:
                        if (sources.Elements(Constants.XNAME_DefaultNow).Any())
                            Write(" DEFAULT (datetime('now','localtime'))");
                        else
                        {
                            DateTime? defaultDateTime = sources.ElementToDateTime(Constants.XNAME_Default);
                            if (defaultDateTime.HasValue)
                                Write(defaultDateTime.Value.ToLocalTime().ToString(@"'yyyy-MM-dd HH:mm:ss"));
                        }
                        comment = null;
                        break;
                    case Constants.NAME_Bit:
                        comment = null;
                        bool? isDefaultTrue = sources.ElementToBoolean(Constants.XNAME_Default);
                        if (isDefaultTrue.HasValue)
                            Write(isDefaultTrue.Value ? " DEFAULT 1," : " DEFAULT 0");
                        break;
                    case Constants.NAME_ByteValues:
                        comment = null;
                        if (sources.Elements(Constants.XNAME_DefaultEmpty).Any())
                            Write(" DEFAULT X''");
                        else
                        {
                            byte[] data = sources.ElementToBinary(Constants.XNAME_Default);
                            if (data is not null)
                            {
                                Write(" DEFAULT X'");
                                Write(BitConverter.ToString(data));
                                Write("'");
                            }
                        }
                        break;
                    case Constants.NAME_DriveType:
                        string defaultDriveType = sources.ElementToString(Constants.XNAME_Default);
                        if (defaultDriveType is not null)
                            switch (defaultDriveType.Trim())
                            {
                                case "NoRootDirectory":
                                    Write(" DEFAULT 1");
                                    comment = "DriveType.NoRootDirectory";
                                    break;
                                case "Removable":
                                    Write(" DEFAULT 2");
                                    comment = "DriveType.Removable";
                                    break;
                                case "Fixed":
                                    Write(" DEFAULT 3");
                                    comment = "DriveType.Fixed";
                                    break;
                                case "Network":
                                    Write(" DEFAULT 4");
                                    comment = "DriveType.Network";
                                    break;
                                case "CDRom":
                                    Write(" DEFAULT 5");
                                    comment = "DriveType.CDRom";
                                    break;
                                case "Ram":
                                    Write(" DEFAULT 6");
                                    comment = "DriveType.Ram";
                                    break;
                                default:
                                    Write(" DEFAULT 0");
                                    comment = defaultDriveType.Trim();
                                    break;
                            }
                        else
                            comment = "";
                        break;
                    default:
                        comment = null;
                        string defaultValue = sources.ElementToString(Constants.XNAME_Default);
                        if (defaultValue is not null)
                        {
                            Write(" DEFAULT ");
                            Write(defaultValue);
                        }
                        break;
                }
            }

            switch (propertyTypeName)
            {
                case Constants.NAME_RelatedEntity:
                case Constants.NAME_UniqueIdentifier:
                case Constants.NAME_VolumeIdentifier:
                    Write(" COLLATE NOCASE");
                    break;
                case Constants.NAME_NVarChar:
                    bool? isCaseSensitive = sources.AttributeToBoolean(Constants.XNAME_IsCaseSensitive);
                    if (isCaseSensitive.HasValue)
                    {
                        Write(" COLLATE ");
                        Write(isCaseSensitive.Value ? Constants.SQL_TYPENAME_BINARY : "NOCASE");
                    }
                    break;
            }
            return colName;
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
