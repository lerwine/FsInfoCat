using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat
{
    public static class XLinqExtensions
    {
        public const string XmlNamespace_FsInfoCatExport = "http://git.erwinefamily.net/FsInfoCat/V1/FsInfoCatExport.xsd";

        public static readonly XNamespace XNamespace_FsInfoCatExport = XNamespace.Get(XmlNamespace_FsInfoCatExport);

        public static XName ToFsInfoCatExportXmlns(this string name) => XNamespace_FsInfoCatExport.GetName(name);

        public static void Write(string s) { }

        public static void WriteLine(string s) { }

        public static void PushIndent(string s) { }

        public static void PopIndent() { }
        
        public static void GenerateXmlDoc(XElement e) { }

        public static string WriteDisplayAttribute(XElement e) { }

        private static readonly XName XNAME_Root = XName.Get("Root");
        private static readonly XName XNAME_Upstream = XName.Get("Upstream");
        private static readonly XName XNAME_Local = XName.Get("Local");
        private static readonly XName XNAME_Entity = XName.Get("Entity");
        private static readonly XName XNAME_Enum = XName.Get("Enum");
        private static readonly XName XNAME_Name = XName.Get("Name");
        private static readonly XName XNAME_FullName = XName.Get("FullName");
        private static readonly XName XNAME_Field = XName.Get("Field");
        private static readonly XName XNAME_CollectionNavigation = XName.Get("CollectionNavigation");
        private static readonly XName XNAME_NewCollectionNavigation = XName.Get("NewCollectionNavigation");
        private static readonly XName XNAME_RelatedEntity = XName.Get("RelatedEntity");
        private static readonly XName XNAME_NewRelatedEntity = XName.Get("NewRelatedEntity");
        private static readonly XName XNAME_ItemType = XName.Get("ItemType");
        private static readonly XName XNAME_Reference = XName.Get("Reference");
        private static readonly XName XNAME_AmbientEnum = XName.Get("AmbientEnum");
        private static readonly XName XNAME_Default = XName.Get("Default");
        private static readonly XName XNAME_Value = XName.Get("Value");
        private static readonly XName XNAME_EnumTypes = XName.Get("EnumTypes");
        private static readonly XName XNAME_Properties = XName.Get("Properties");
        private static readonly XName XNAME_ExtendsEntity = XName.Get("ExtendsEntity");
        private static readonly XName XNAME_ExtendsGenericEntity = XName.Get("ExtendsGenericEntity");
        private static readonly XName XNAME_Implements = XName.Get("Implements");
        private static readonly XName XNAME_ImplementsEntity = XName.Get("ImplementsEntity");
        private static readonly XName XNAME_ImplementsGenericEntity = XName.Get("ImplementsGenericEntity");
        private static readonly XName XNAME_RootInterface = XName.Get("RootInterface");
        private static readonly XName XNAME_Type = XName.Get("Type");
        private static readonly XName XNAME_TypeDef = XName.Get("TypeDef");
        private static readonly XName XNAME_PrimaryKey = XName.Get("PrimaryKey");
        private static readonly XName XNAME_ForeignKey = XName.Get("ForeignKey");

        private static readonly XName XNAME_AmbientBoolean = XName.Get("AmbientBoolean");
        private static readonly XName XNAME_AmbientInt = XName.Get("AmbientInt");
        private static readonly XName XNAME_AmbientByte = XName.Get("AmbientByte");
        private static readonly XName XNAME_AmbientSByte = XName.Get("AmbientSByte");
        private static readonly XName XNAME_AmbientShort = XName.Get("AmbientShort");
        private static readonly XName XNAME_AmbientUShort = XName.Get("AmbientUShort");
        private static readonly XName XNAME_AmbientFloat = XName.Get("AmbientFloat");
        private static readonly XName XNAME_AmbientDouble = XName.Get("AmbientDouble");
        private static readonly XName XNAME_summary = XName.Get("summary");
        private static readonly XName XNAME_remarks = XName.Get("remarks");
        private static readonly XName XNAME_seealso = XName.Get("seealso");
        private static readonly XName XNAME_IsFlags = XName.Get("IsFlags");
        private static readonly XName XNAME_typeparam = XName.Get("typeparam");
        private static readonly XName XNAME_cref = XName.Get("cref");
        private static readonly XDocument EntityDefinitionsDocument = new XDocument();
        public static XElement FindRootEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Root)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        public static XElement FindRootEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Root)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        public static XElement FindRootByName(string name) { return (name is null) ? null : (FindRootEntityByName(name) ?? FindRootEnumByName(name)); }

        public static XElement FindLocalEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Local)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEntityByName(name);
            return matchingAttribute.Parent;
        }

        public static XElement FindLocalEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Local)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEnumByName(name);
            return matchingAttribute.Parent;
        }

        public static XElement FindLocalByName(string name) { return (name is null) ? null : (FindLocalEntityByName(name) ?? FindLocalEnumByName(name)); }

        public static XElement FindUpstreamEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Upstream)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEntityByName(name);
            return matchingAttribute.Parent;
        }

        public static XElement FindUpstreamEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Upstream)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEnumByName(name);
            return matchingAttribute.Parent;
        }

        public static XElement FindUpstreamByName(string name) { return (name is null) ? null : (FindUpstreamEntityByName(name) ?? FindUpstreamEnumByName(name)); }

        public static IEnumerable<XElement> GetBaseEntities(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Enumerable.Empty<XElement>();

            IEnumerable<string> names = entityElement.Elements().Select(e => (
                IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
                IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
                Element: e
            )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
            switch (parent.Name.LocalName)
            {
                case "Root":
                    return names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null);
                case "Local":
                    return entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindLocalEntityByName(n)).Where(e => e is not null);
                case "Upstream":
                    return entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null);
                default:
                    return Enumerable.Empty<XElement>();
            }
        }

        private static void GetAllBaseEntities(XElement entityElement, int level, Collection<(XElement Element, int Level)> collection, Func<IEnumerable<string>, IEnumerable<XElement>> getEntities)
        {
            IEnumerable<string> names = entityElement.Elements().Select(e => (
                IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
                IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
                Element: e
            )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
            int nextLevel = level + 1;
            foreach (XElement baseEntity in getEntities(names))
            {
                if (!collection.Any(e => e.Level == level && ReferenceEquals(e.Element, baseEntity)))
                {
                    collection.Add(new(baseEntity, level));
                    GetAllBaseEntities(baseEntity, nextLevel, collection, getEntities);
                }
            }
        }

        public static XElement[] GetAllBaseEntities(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Array.Empty<XElement>();
            Collection<(XElement Element, int Level)> result = new();
            Func<IEnumerable<string>, IEnumerable<XElement>> getEntities;
            switch (parent.Name.LocalName)
            {
                case "Root":
                    getEntities = names => names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null);
                    break;
                case "Local":
                    getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindLocalEntityByName(n)).Where(e => e is not null);
                    break;
                case "Upstream":
                    getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null);
                    break;
                default:
                    return Array.Empty<XElement>();
            }
            GetAllBaseEntities(entityElement, 0, result, getEntities);
            return result.OrderBy(t => t.Level).Select(t => t.Element).ToArray();
        }

        public static IEnumerable<string> GetBaseTypeNames(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Enumerable.Empty<string>();

            IEnumerable<string> names = entityElement.Elements().Where(e => e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity ||
                e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity).Attributes(XNAME_Type).Select(a => a.Value);
            switch (parent.Name.LocalName)
            {
                case "Root":
                    names = names.Distinct();
                    break;
                case "Local":
                    names = entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct();
                    break;
                case "Upstream":
                    names = entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct();
                    break;
                default:
                    return Enumerable.Empty<string>();
            }
            return names.Select(n => n.Replace("{", "<").Replace("}", ">"));
        }

        public static void GenerateEntityInterface(XElement entityElement)
        {
            GenerateXmlDoc(entityElement.Element(XNAME_summary));
            foreach (XElement e in entityElement.Elements(XNAME_typeparam))
                GenerateXmlDoc(e);
            GenerateXmlDoc(entityElement.Element(XNAME_remarks));

            string[] baseTypeNames = GetBaseTypeNames(entityElement).ToArray();
            foreach (XElement e in entityElement.Elements(XNAME_seealso).Concat(baseTypeNames.Select(s => new XElement(XNAME_seealso, new XAttribute(XNAME_cref, s)))))
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            string typeName = entityElement.Attribute(XNAME_Name)?.Value.Replace("{", "<").Replace("}", ">");
            Write("public interface ");
            if (baseTypeNames.Length > 0)
            {
                Write(typeName);
                Write(" : ");

                if (baseTypeNames.Skip(1).Any())
                {
                    foreach (string t in baseTypeNames.Reverse().Skip(1).Reverse())
                    {
                        Write(t);
                        Write(", ");
                    }
                }
                WriteLine(baseTypeNames.Last());
            }
            else
                WriteLine(typeName);
            WriteLine("{");
            PushIndent("    ");
            bool isSubsequentMember = false;
            XElement[] baseEntities = GetAllBaseEntities(entityElement);
            foreach (XElement propertyElement in entityElement.Elements(XNAME_Properties).Elements())
            {
                if (propertyElement.Name.NamespaceName.Length > 0)
                    continue;
                if (isSubsequentMember)
                    WriteLine("");
                else
                    isSubsequentMember = true;
                XElement inheritedProperty = GetInheritedProperty(propertyElement, baseEntities, out XElement commentDocElement, out bool isNew);
                XElement definitionPropertyElement = propertyElement;

                GenerateXmlDoc(commentDocElement.Element(XNAME_summary));
                GenerateXmlDoc(commentDocElement.Element(XNAME_value));
                GenerateXmlDoc(commentDocElement.Element(XNAME_remarks));
                foreach (XElement e in commentDocElement.Elements(XNAME_seealso))
                {
                    if (!e.IsEmpty && e.Value.Trim().Length == 0)
                        e.RemoveAll();
                    GenerateXmlDoc(e);
                }
                string propertyName = WriteDisplayAttribute(definitionPropertyElement);
                bool allowsNull = definitionPropertyElement.Elements(XNAME_AllowNull).Any() || propertyElement.Elements(XNAME_DefaultNull).Any();
                if (baseProperties.Contains(propertyName))
                    Write("new ");
                switch (propertyElement.Name.LocalName)
                {
                    case "Byte":
                    case "SByte":
                    case "Short":
                    case "UShort":
                    case "Int":
                    case "UInt":
                    case "Long":
                    case "ULong":
                    case "Float":
                    case "Double":
                        Write(propertyElement.Name.LocalName.ToLower());
                        Write(allowsNull ? "? " : " ");
                        break;
                    case "ByteArray":
                        Write("byte[] ");
                        break;
                    case "Text":
                    case "NVarChar":
                        Write("string ");
                        break;
                    case "VolumeIdentifier":
                    case "DriveType":
                    case "MD5Hash":
                    case "DateTime":
                        Write(propertyElement.Name.LocalName);
                        Write(allowsNull ? "? " : " ");
                        break;
                    case "ByteValues":
                    case "MultiStringValue":
                        Write(propertyElement.Name.LocalName);
                        Write(" ");
                        break;
                    case "UniqueIdentifier":
                        Write(allowsNull ? "Guid? " : "Guid ");
                        break;
                    case "Bit":
                        Write(allowsNull ? "bool? " : "bool ");
                        break;
                    case "Enum":
                        Write(propertyElement.Attribute(XNAME_Type)?.Value);
                        Write(allowsNull ? "? " : " ");
                        break;
                    case "CollectionNavigation":
                        Write("IEnumerable<");
                        Write(propertyElement.Attribute(XNAME_ItemType)?.Value);
                        Write("> ");
                        break;
                    case "RelatedEntity":
                        Write(propertyElement.Attribute(XNAME_TypeDef)?.Value ?? propertyElement.Attribute(XNAME_Reference)?.Value);
                        Write(" ");
                        break;
                    default:
                        Write("#warning Unknown element: ");
                        WriteLine(propertyElement.Name.LocalName);
                        Write("object ");
                        break;
                }

                Write(propertyName);
                WriteLine(propertyElement.Attributes(XNAME_IsGenericWritable).Any(a => a.Value == "true") ? " { get; set; }" : " { get; }");
            }

            PopIndent();
            WriteLine("}");
        }
        public static readonly XName XNAME_NewIdNavRef = XName.Get("NewIdNavRef");
        public static readonly XName XNAME_UniqueIdentifier = XName.Get("UniqueIdentifier");
        public static readonly XName XNAME_value = XName.Get("value");
        public static readonly XName XNAME_AllowNull = XName.Get("value");
        public static readonly XName XNAME_AllowNull = XName.Get("value");
        public static readonly XName XNAME_IsGenericWritable = XName.Get("IsGenericWritable");
        private static XElement GetInheritedProperty(XElement propertyElement, XElement[] baseEntities, out XElement commentDocElement, out bool isNew)
        {
            IEnumerable<XElement> baseProperties;
            XName inheritedName;
            switch (propertyElement.Name.LocalName)
            {
                case "NewIdNavRef":
                    isNew = false;
                    inheritedName = XNAME_UniqueIdentifier;
                    baseProperties = baseEntities.Elements(XNAME_Properties).Elements().Where(e => e.Name == XNAME_NewIdNavRef || e.Name == inheritedName);
                    break;
                case "NewRelatedEntity":
                    isNew = true;
                    inheritedName = XNAME_UniqueIdentifier;
                    baseProperties = baseEntities.Elements(XNAME_Properties).Elements().Where(e => e.Name == XNAME_NewRelatedEntity || e.Name == inheritedName);
                    break;
                case "NewCollectionNavigation":
                    isNew = true;
                    inheritedName = XNAME_CollectionNavigation;
                    baseProperties = baseEntities.Elements(XNAME_Properties).Elements().Where(e => e.Name == XNAME_NewCollectionNavigation || e.Name == inheritedName);
                    break;
                default:
                    isNew = false;
                    commentDocElement = propertyElement;
                    return propertyElement;
            }
            string name = propertyElement.Attribute(XNAME_Name)?.Value;
            commentDocElement = propertyElement.Elements(XNAME_summary).Any() ? propertyElement :
                baseProperties.Attributes(XNAME_Name).Where(a => a.Value == name).Select(a => a.Parent).Where(e => e.Elements(XNAME_summary).Any())
                    .DefaultIfEmpty(propertyElement).First();
            return baseProperties.Where(e => e.Name == inheritedName).DefaultIfEmpty(propertyElement).First();
        }

        public enum NormalizeXTextOption
        {
            /// <summary>
            /// Merge adjacent text nodes as a single <see cref="XCData"/> node when any of the original text nodes are a <see cref="XCData"/> node.
            /// </summary>
            PreferCData,

            /// <summary>
            /// Merge adjacent text nodes as a single <see cref="XText"/> node when any of the original text nodes are not a <see cref="XCData"/> node.
            /// </summary>
            PreferXText,

            /// <summary>
            /// Replace/merge all <see cref="XCData"/> nodes with simple <see cref="XText"/> nodes.
            /// </summary>
            NoCData,

            /// <summary>
            /// Replace/merge all <see cref="XText"/> nodes with <see cref="XCData"/> nodes.
            /// </summary>
            AllCData,

            /// <summary>
            /// Replace all <see cref="XText"/> nodes containing at least one non-whitespace character into <see cref="XCData"/> nodes.
            /// </summary>
            /// <remarks>Merged adjacent <see cref="XText"/> nodes without non-whitespace characters follow the same behavior as <see cref="PreferCData"/>.</remarks>
            NonWhiteSpaceToCData,

            /// <summary>
            /// Convert all <see cref="XText"/> nodes containing at least one line separator character into a <see cref="XCData"/> node.
            /// </summary>
            /// <remarks>Merged adjacent <see cref="XText"/> nodes without line separator characters follow the same behavior as <see cref="PreferCData"/>.</remarks>
            MultilineToCData,

            /// <summary>
            /// Convert all <see cref="XText"/> nodes containing at least one non-whitespace or line separator character into a <see cref="XCData"/> node.
            /// </summary>
            /// <remarks>Merged adjacent <see cref="XText"/> nodes without line separator or non-whitespace characters follow the same behavior as <see cref="PreferCData"/>.</remarks>
            MultilineOrNonWhiteSpaceToCData
        }

        /// <summary>
        /// Gets the adjacent nodes that are of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">node</exception>
        public static IEnumerable<T> GetAdjacentNodes<T>([DisallowNull] this T node) where T : XNode
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            while (node.PreviousNode is T p)
                node = p;
            yield return node;
            while (node.NextNode is T t)
            {
                yield return t;
                node = t;
            }
        }

        public static string AttributeValueOrDefault([AllowNull] this XElement element, [DisallowNull] XName attributeName, string ifNotPresent = null)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return attribute.Value;
            }
            return ifNotPresent;
        }

        public static string GetAttributeValue([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string> ifNotPresent)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (ifNotPresent is null)
                throw new ArgumentNullException(nameof(ifNotPresent));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return attribute.Value;
            }
            return ifNotPresent();
        }

        public static bool TryGetAttributeValue([AllowNull] this XElement element, [DisallowNull] XName attributeName, out string result)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                {
                    result = attribute.Value;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static T AttributeValueOrDefault<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> ifPresent, T ifNotPresent = default)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (ifPresent is null)
                throw new ArgumentNullException(nameof(ifPresent));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return ifPresent(attribute.Value);
            }
            return ifNotPresent;
        }

        public static T GetAttributeValue<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> ifPresent, [DisallowNull] Func<T> ifNotPresent)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (ifPresent is null)
                throw new ArgumentNullException(nameof(ifPresent));
            if (ifNotPresent is null)
                throw new ArgumentNullException(nameof(ifNotPresent));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return ifPresent(attribute.Value);
            }
            return ifNotPresent();
        }

        public static bool TryGetAttributeValue<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> converter, out T result)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (converter is null)
                throw new ArgumentNullException(nameof(converter));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                {
                    result = converter(attribute.Value);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryConvertToBoolean(string value, out bool result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToBoolean(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool TryConvertToDateTime(string value, XmlDateTimeSerializationMode dateTimeOption, out DateTime result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToDateTime(value, dateTimeOption);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool TryConvertToTimeSpan(string value, out TimeSpan result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToTimeSpan(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool TryConvertToInt16(string value, out short result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToInt16(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool TryConvertToInt32(string value, out int result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToInt32(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool TryConvertToInt64(string value, out long result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToInt64(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool TryConvertToEnumValue<TEnum>(string value, out TEnum result)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse(value.Trim(), out result))
                return true;
            result = default;
            return false;
        }

        private static readonly Regex WsRegex = new(@"[\s\r\n]+", RegexOptions.Compiled);

        public static IEnumerable<TEnum> GetEnumList<TEnum>(string value)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                foreach (string n in WsRegex.Split(value))
                {
                    if (!Enum.TryParse(n, out TEnum result))
                        throw new ArgumentOutOfRangeException(nameof(value));
                    yield return result;
                }
        }

        public static bool TryConvertToGuid(string value, out Guid result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToGuid(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool? GetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, bool? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToBoolean(value, out bool result))
                    return result;
            }
            return ifNotPresent;
        }

        public static bool GetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, bool ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToBoolean(value, out bool result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, out bool result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToBoolean(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, out bool? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToBoolean(value, out bool r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static DateTime? GetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, DateTime? ifNotPresent = null, XmlDateTimeSerializationMode dateTimeOption = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToDateTime(value, dateTimeOption, out DateTime result))
                    return result;
            }
            return ifNotPresent;
        }

        public static DateTime GetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, DateTime ifNotPresent, XmlDateTimeSerializationMode dateTimeOption = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToDateTime(value, dateTimeOption, out DateTime result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, XmlDateTimeSerializationMode dateTimeOption, out DateTime result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToDateTime(value, dateTimeOption, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, out DateTime result) => TryGetAttributeDateTime(element, attributeName, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, XmlDateTimeSerializationMode dateTimeOption, out DateTime? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToDateTime(value, dateTimeOption, out DateTime r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, out DateTime? result) => TryGetAttributeDateTime(element, attributeName, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static TimeSpan? GetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, TimeSpan? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToTimeSpan(value, out TimeSpan result))
                    return result;
            }
            return ifNotPresent;
        }

        public static TimeSpan GetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, TimeSpan ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToTimeSpan(value, out TimeSpan result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TimeSpan result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToTimeSpan(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TimeSpan? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToTimeSpan(value, out TimeSpan r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static short? GetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, short? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToInt16(value, out short result))
                    return result;
            }
            return ifNotPresent;
        }

        public static short GetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, short ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt16(value, out short result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, out short result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt16(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, out short? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToInt16(value, out short r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static int? GetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, int? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToInt32(value, out int result))
                    return result;
            }
            return ifNotPresent;
        }

        public static int GetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, int ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt32(value, out int result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, out int result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt32(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, out int? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToInt32(value, out int r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static long? GetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, long? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToInt64(value, out long result))
                    return result;
            }
            return ifNotPresent;
        }

        public static long GetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, long ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt64(value, out long result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, out long result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt64(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, out long? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToInt64(value, out long r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static TEnum? GetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, TEnum? ifNotPresent = null)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToEnumValue(value, out TEnum result))
                    return result;
            }
            return ifNotPresent;
        }

        public static TEnum GetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, TEnum ifNotPresent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToEnumValue(value, out TEnum result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TEnum result)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToEnumValue(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TEnum? result)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToEnumValue(value, out TEnum r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static IEnumerable<TEnum> GetAttributeEnumFlags<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
                return GetEnumList<TEnum>(value);
            return null;
        }

        public static Guid? GetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, Guid? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToGuid(value, out Guid result))
                    return result;
            }
            return ifNotPresent;
        }

        public static Guid GetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, Guid ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToGuid(value, out Guid result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, out Guid result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToGuid(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, out Guid? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToGuid(value, out Guid r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static byte[] GetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, byte[] ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                return ByteArrayCoersion.Parse(value).ToArray();
            }
            return ifNotPresent;
        }

        public static bool TryGetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, out byte[] result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && ByteArrayCoersion.TryParse(value, out IEnumerable<byte> en))
            {
                result = en.ToArray();
                return true;
            }
            result = default;
            return false;
        }
    }

}
