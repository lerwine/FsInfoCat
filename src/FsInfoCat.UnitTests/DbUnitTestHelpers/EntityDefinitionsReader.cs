using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace FsInfoCat.UnitTests.DbUnitTestHelpers
{
    public class EntityDefinitionsReader
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
            if (NewLineRegex.IsMatch(text))
            {
                string[] lines = NewLineRegex.Split(text);
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

        List<ValidationEventArgs> ValidationErrors { get; } = new();

        #endregion

        #region core.ttinclude

        #region Entity Related Fields

        const string SQL_TYPENAME_BLOB = "BLOB";
        const string SQL_TYPENAME_UNIQUEIDENTIFIER = "UNIQUEIDENTIFIER";
        const string SQL_TYPENAME_NVARCHAR = "NVARCHAR";
        const string SQL_TYPENAME_CHARACTER = "CHARACTER";
        const string SQL_TYPENAME_TEXT = "TEXT";
        const string SQL_TYPENAME_DATETIME = "DATETIME";
        const string SQL_TYPENAME_TIME = "TIME";
        const string SQL_TYPENAME_BIT = "BIT";
        const string SQL_TYPENAME_VARBINARY = "VARBINARY";
        const string SQL_TYPENAME_BINARY = "BINARY";
        const string SQL_TYPENAME_TINYINT = "TINYINT";
        const string SQL_TYPENAME_UNSIGNED_TINYINT = "UNSIGNED TINYINT";
        const string SQL_TYPENAME_SMALLINT = "SMALLINT";
        const string SQL_TYPENAME_UNSIGNED_SMALLINT = "UNSIGNED SMALLINT";
        const string SQL_TYPENAME_INT = "INT";
        const string SQL_TYPENAME_UNSIGNED_INT = "UNSIGNED INT";
        const string SQL_TYPENAME_BIGINT = "BIGINT";
        const string SQL_TYPENAME_UNSIGNED_BIGINT = "UNSIGNED BIGINT";
        const string SQL_TYPENAME_REAL = "REAL";
        const string SQL_TYPENAME_NUMERIC = "NUMERIC";
        const string NAME_Root = "Root";
        const string NAME_Upstream = "Upstream";
        const string NAME_Local = "Local";
        const string NAME_Entity = "Entity";
        const string NAME_Enum = "Enum";
        const string NAME_Name = "Name";
        const string NAME_FullName = "FullName";
        const string NAME_Field = "Field";
        const string NAME_CollectionNavigation = "CollectionNavigation";
        const string NAME_NewCollectionNavigation = "NewCollectionNavigation";
        const string NAME_RelatedEntity = "RelatedEntity";
        const string NAME_NewRelatedEntity = "NewRelatedEntity";
        const string NAME_ItemType = "ItemType";
        const string NAME_Reference = "Reference";
        const string NAME_AmbientEnum = "AmbientEnum";
        const string NAME_Default = "Default";
        const string NAME_Value = "Value";
        const string NAME_EnumTypes = "EnumTypes";
        const string NAME_Properties = "Properties"; 
        const string NAME_BaseType = "BaseType";
        const string NAME_ExtendsEntity = "ExtendsEntity";
        const string NAME_ExtendsGenericEntity = "ExtendsGenericEntity";
        const string NAME_Implements = "Implements";
        const string NAME_ImplementsEntity = "ImplementsEntity";
        const string NAME_ImplementsGenericEntity = "ImplementsGenericEntity";
        const string NAME_RootInterface = "RootInterface";
        const string NAME_Type = "Type";
        const string NAME_TypeDef = "TypeDef";
        const string NAME_ItemKey = "ItemKey";
        const string NAME_AmbientBoolean = "AmbientBoolean";
        const string NAME_AmbientInt = "AmbientInt";
        const string NAME_AmbientByte = "AmbientByte";
        const string NAME_AmbientSByte = "AmbientSByte";
        const string NAME_AmbientShort = "AmbientShort";
        const string NAME_AmbientUShort = "AmbientUShort";
        const string NAME_AmbientFloat = "AmbientFloat";
        const string NAME_AmbientDouble = "AmbientDouble";
        const string NAME_summary = "summary";
        const string NAME_remarks = "remarks";
        const string NAME_seealso = "seealso";
        const string NAME_IsFlags = "IsFlags";
        const string NAME_typeparam = "typeparam";
        const string NAME_cref = "cref";
        const string NAME_NewIdNavRef = "NewIdNavRef";
        const string NAME_UniqueIdentifier = "UniqueIdentifier";
        const string NAME_value = "value";
        const string NAME_DefaultNull = "DefaultNull";
        const string NAME_DefaultNow = "DefaultNow";
        const string NAME_DefaultZero = "DefaultZero";
        const string NAME_DefaultEmpty = "DefaultEmpty";
        const string NAME_AllowNull = "AllowNull";
        const string NAME_IsGenericWritable = "IsGenericWritable";
        const string NAME_DisplayNameResource = "DisplayNameResource";
        const string NAME_DescriptionResource = "DescriptionResource";
        const string NAME_ResourceType = "ResourceType";
        const string NAME_TableName = "TableName";
        const string NAME_ColName = "ColName";
        const string NAME_Byte = "Byte";
        const string NAME_SByte = "SByte";
        const string NAME_MultiStringValue = "MultiStringValue";
        const string NAME_MD5Hash = "MD5Hash";
        const string NAME_ByteValues = "ByteValues";
        const string NAME_Short = "Short";
        const string NAME_UShort = "UShort";
        const string NAME_Int = "Int";
        const string NAME_UInt = "UInt";
        const string NAME_Long = "Long";
        const string NAME_ULong = "ULong";
        const string NAME_Double = "Double";
        const string NAME_Float = "Float";
        const string NAME_Decimal = "Decimal";
        const string NAME_NVarChar = "NVarChar";
        const string NAME_Char = "Char";
        const string NAME_DateTime = "DateTime";
        const string NAME_TimeSpan = "TimeSpan";
        const string NAME_Bit = "Bit";
        const string NAME_Text = "Text";
        const string NAME_VolumeIdentifier = "VolumeIdentifier";
        const string NAME_DriveType = "DriveType";
        const string NAME_MaxLength = "MaxLength";
        const string NAME_MinLength = "MinLength";
        const string NAME_MaxValue = "MaxValue";
        const string NAME_MinValue = "MinValue";
        const string NAME_CreatedOn = "CreatedOn";
        const string NAME_ModifiedOn = "ModifiedOn";
        const string NAME_UpstreamId = "UpstreamId";
        const string NAME_LastSynchronizedOn = "LastSynchronizedOn";
        const string NAME_IsNormalized = "IsNormalized";
        const string NAME_Index = "Index";
        const string NAME_Unique = "Unique";
        const string NAME_IsPrimaryKey = "IsPrimaryKey";
        const string NAME_Property = "Property";
        const string NAME_LeftProperty = "LeftProperty";
        const string NAME_RightProperty = "RightProperty";
        const string NAME_Operator = "Operator";
        const string XNAME_langword = "langword";
        const string XNAME_see = "see";
        const string NAME_PrimaryKey = "PrimaryKey";
        const string NAME_ForeignKey = "ForeignKey";
        const string NAME_AmbientString = "AmbientString";
        const string NAME_AmbientUInt = "AmbientUInt";
        const string NAME_AmbientLong = "AmbientLong";
        const string NAME_AmbientULong = "AmbientULong";
        const string NAME_IsCaseSensitive = "IsCaseSensitive";
        [Obsolete("Remove this")]
        const string NAME_ConstraintName = "ConstraintName";
        const string NAME_DbRelationship = "DbRelationship";
        const string NAME_FkPropertyName = "FkPropertyName";
        const string NAME_Check = "Check";
        const string NAME_And = "And";
        const string NAME_Or = "Or";
        const string NAME_IsNull = "IsNull";
        const string NAME_NotNull = "NotNull";
        const string NAME_LessThan = "LessThan";
        const string NAME_NotGreaterThan = "NotGreaterThan";
        const string NAME_Equals = "Equals";
        const string NAME_NotEquals = "NotEquals";
        const string NAME_NotLessThan = "NotLessThan";
        const string NAME_GreaterThan = "GreaterThan";
        const string NAME_OtherProperty = "OtherProperty";
        const string NAME_True = "True";
        const string NAME_False = "False";
        const string NAME_Now = "Now";
        const string NAME_Trimmed = "Trimmed";
        const string NAME_Length = "Length";
        const string NAME_Navigation = "Navigation";
        static readonly XName XNAME_Root = XName.Get(NAME_Root);
        static readonly XName XNAME_Upstream = XName.Get(NAME_Upstream);
        static readonly XName XNAME_Local = XName.Get(NAME_Local);
        static readonly XName XNAME_Entity = XName.Get(NAME_Entity);
        static readonly XName XNAME_Enum = XName.Get(NAME_Enum);
        static readonly XName XNAME_Name = XName.Get(NAME_Name);
        static readonly XName XNAME_FullName = XName.Get(NAME_FullName);
        static readonly XName XNAME_Field = XName.Get(NAME_Field);
        static readonly XName XNAME_CollectionNavigation = XName.Get(NAME_CollectionNavigation);
        static readonly XName XNAME_NewCollectionNavigation = XName.Get(NAME_NewCollectionNavigation);
        static readonly XName XNAME_RelatedEntity = XName.Get(NAME_RelatedEntity);
        static readonly XName XNAME_NewRelatedEntity = XName.Get(NAME_NewRelatedEntity);
        static readonly XName XNAME_ItemType = XName.Get(NAME_ItemType);
        static readonly XName XNAME_Reference = XName.Get(NAME_Reference);
        static readonly XName XNAME_AmbientEnum = XName.Get(NAME_AmbientEnum);
        static readonly XName XNAME_Default = XName.Get(NAME_Default);
        static readonly XName XNAME_Value = XName.Get(NAME_Value);
        static readonly XName XNAME_EnumTypes = XName.Get(NAME_EnumTypes);
        static readonly XName XNAME_Properties = XName.Get(NAME_Properties); 
        static readonly XName XNAME_BaseType = XName.Get(NAME_BaseType);
        static readonly XName XNAME_ExtendsEntity = XName.Get(NAME_ExtendsEntity);
        static readonly XName XNAME_ExtendsGenericEntity = XName.Get(NAME_ExtendsGenericEntity);
        static readonly XName XNAME_Implements = XName.Get(NAME_Implements);
        static readonly XName XNAME_ImplementsEntity = XName.Get(NAME_ImplementsEntity);
        static readonly XName XNAME_ImplementsGenericEntity = XName.Get(NAME_ImplementsGenericEntity);
        static readonly XName XNAME_RootInterface = XName.Get(NAME_RootInterface);
        static readonly XName XNAME_Type = XName.Get(NAME_Type);
        static readonly XName XNAME_TypeDef = XName.Get(NAME_TypeDef);
        static readonly XName XNAME_ItemKey = XName.Get(NAME_ItemKey);
        static readonly XName XNAME_AmbientBoolean = XName.Get(NAME_AmbientBoolean);
        static readonly XName XNAME_AmbientInt = XName.Get(NAME_AmbientInt);
        static readonly XName XNAME_AmbientByte = XName.Get(NAME_AmbientByte);
        static readonly XName XNAME_AmbientSByte = XName.Get(NAME_AmbientSByte);
        static readonly XName XNAME_AmbientShort = XName.Get(NAME_AmbientShort);
        static readonly XName XNAME_AmbientUShort = XName.Get(NAME_AmbientUShort);
        static readonly XName XNAME_AmbientFloat = XName.Get(NAME_AmbientFloat);
        static readonly XName XNAME_AmbientDouble = XName.Get(NAME_AmbientDouble);
        static readonly XName XNAME_summary = XName.Get(NAME_summary);
        static readonly XName XNAME_remarks = XName.Get(NAME_remarks);
        static readonly XName XNAME_seealso = XName.Get(NAME_seealso);
        static readonly XName XNAME_IsFlags = XName.Get(NAME_IsFlags);
        static readonly XName XNAME_typeparam = XName.Get(NAME_typeparam);
        static readonly XName XNAME_cref = XName.Get(NAME_cref);
        static readonly XName XNAME_NewIdNavRef = XName.Get(NAME_NewIdNavRef);
        static readonly XName XNAME_UniqueIdentifier = XName.Get(NAME_UniqueIdentifier);
        static readonly XName XNAME_value = XName.Get(NAME_value);
        static readonly XName XNAME_DefaultNull = XName.Get(NAME_DefaultNull);
        static readonly XName XNAME_DefaultNow = XName.Get(NAME_DefaultNow);
        static readonly XName XNAME_DefaultZero = XName.Get(NAME_DefaultZero);
        static readonly XName XNAME_AllowNull = XName.Get(NAME_AllowNull);
        static readonly XName XNAME_IsGenericWritable = XName.Get(NAME_IsGenericWritable);
        static readonly XName XNAME_DisplayNameResource = XName.Get(NAME_DisplayNameResource);
        static readonly XName XNAME_DescriptionResource = XName.Get(NAME_DescriptionResource);
        static readonly XName XNAME_ResourceType = XName.Get(NAME_ResourceType);
        static readonly XName XNAME_TableName = XName.Get(NAME_TableName);
        static readonly XName XNAME_ColName = XName.Get(NAME_ColName);
        static readonly XName XNAME_Byte = XName.Get(NAME_Byte);
        static readonly XName XNAME_SByte = XName.Get(NAME_SByte);
        static readonly XName XNAME_MultiStringValue = XName.Get(NAME_MultiStringValue);
        static readonly XName XNAME_MD5Hash = XName.Get(NAME_MD5Hash);
        static readonly XName XNAME_ByteValues = XName.Get(NAME_ByteValues);
        static readonly XName XNAME_Short = XName.Get(NAME_Short);
        static readonly XName XNAME_UShort = XName.Get(NAME_UShort);
        static readonly XName XNAME_Int = XName.Get(NAME_Int);
        static readonly XName XNAME_UInt = XName.Get(NAME_UInt);
        static readonly XName XNAME_Long = XName.Get(NAME_Long);
        static readonly XName XNAME_ULong = XName.Get(NAME_ULong);
        static readonly XName XNAME_Double = XName.Get(NAME_Double);
        static readonly XName XNAME_Float = XName.Get(NAME_Float);
        static readonly XName XNAME_Decimal = XName.Get(NAME_Decimal);
        static readonly XName XNAME_NVarChar = XName.Get(NAME_NVarChar);
        static readonly XName XNAME_Char = XName.Get(NAME_Char);
        static readonly XName XNAME_DateTime = XName.Get(NAME_DateTime);
        static readonly XName XNAME_TimeSpan = XName.Get(NAME_TimeSpan);
        static readonly XName XNAME_Bit = XName.Get(NAME_Bit);
        static readonly XName XNAME_Text = XName.Get(NAME_Text);
        static readonly XName XNAME_VolumeIdentifier = XName.Get(NAME_VolumeIdentifier);
        static readonly XName XNAME_DriveType = XName.Get(NAME_DriveType);
        static readonly XName XNAME_MaxLength = XName.Get(NAME_MaxLength);
        static readonly XName XNAME_CreatedOn = XName.Get(NAME_CreatedOn);
        static readonly XName XNAME_ModifiedOn = XName.Get(NAME_ModifiedOn);
        static readonly XName XNAME_UpstreamId = XName.Get(NAME_UpstreamId);
        static readonly XName XNAME_LastSynchronizedOn = XName.Get(NAME_LastSynchronizedOn);
        static readonly XName XNAME_IsNormalized = XName.Get(NAME_IsNormalized);
        static readonly XName XNAME_MinLength = XName.Get(NAME_MinLength);
        static readonly XName XNAME_MaxValue = XName.Get(NAME_MaxValue);
        static readonly XName XNAME_MinValue = XName.Get(NAME_MinValue);
        static readonly XName XNAME_Index = XName.Get(NAME_Index);
        static readonly XName XNAME_DefaultEmpty = XName.Get(NAME_DefaultEmpty);
        static readonly XName XNAME_Unique = XName.Get(NAME_Unique);
        static readonly XName XNAME_IsPrimaryKey = XName.Get(NAME_IsPrimaryKey);
        static readonly XName XNAME_Property = XName.Get(NAME_Property);
        static readonly XName XNAME_LeftProperty = XName.Get(NAME_LeftProperty);
        static readonly XName XNAME_RightProperty = XName.Get(NAME_RightProperty);
        static readonly XName XNAME_Operator = XName.Get(NAME_Operator);
        static readonly XName XNAME_PrimaryKey = XName.Get(NAME_PrimaryKey);
        static readonly XName XNAME_ForeignKey = XName.Get(NAME_ForeignKey);
        static readonly XName XNAME_AmbientString = XName.Get(NAME_AmbientString);
        static readonly XName XNAME_AmbientUInt = XName.Get(NAME_AmbientUInt);
        static readonly XName XNAME_AmbientLong = XName.Get(NAME_AmbientLong);
        static readonly XName XNAME_AmbientULong = XName.Get(NAME_AmbientULong);
        static readonly XName XNAME_IsCaseSensitive = XName.Get(NAME_IsCaseSensitive);
        [Obsolete("Remove this")]
        static readonly XName XNAME_ConstraintName = XName.Get(NAME_ConstraintName);
        static readonly XName XNAME_DbRelationship = XName.Get(NAME_DbRelationship);
        static readonly XName XNAME_FkPropertyName = XName.Get(NAME_FkPropertyName);
        static readonly XName XNAME_Check = XName.Get(NAME_Check);
        static readonly XName XNAME_And = XName.Get(NAME_And);
        static readonly XName XNAME_Or = XName.Get(NAME_Or);
        static readonly XName XNAME_IsNull = XName.Get(NAME_IsNull);
        static readonly XName XNAME_NotNull = XName.Get(NAME_NotNull);
        static readonly XName XNAME_LessThan = XName.Get(NAME_LessThan);
        static readonly XName XNAME_NotGreaterThan = XName.Get(NAME_NotGreaterThan);
        static readonly XName XNAME_Equals = XName.Get(NAME_Equals);
        static readonly XName XNAME_NotEquals = XName.Get(NAME_NotEquals);
        static readonly XName XNAME_NotLessThan = XName.Get(NAME_NotLessThan);
        static readonly XName XNAME_GreaterThan = XName.Get(NAME_GreaterThan);
        static readonly XName XNAME_OtherProperty = XName.Get(NAME_OtherProperty);
        static readonly XName XNAME_True = XName.Get(NAME_True);
        static readonly XName XNAME_False = XName.Get(NAME_False);
        static readonly XName XNAME_Now = XName.Get(NAME_Now);
        static readonly XName XNAME_Trimmed = XName.Get(NAME_Trimmed);
        static readonly XName XNAME_Length = XName.Get(NAME_Length);
        static readonly XName XNAME_Navigation = XName.Get(NAME_Navigation);
        static readonly Regex NewLineRegex = new(@"\r\n?|[\n\p{Zl}\p{Zp}]", RegexOptions.Compiled);
        static readonly Regex NormalizeWsRegex = new(@" ((?![\r\n])\s)*|(?! )((?![\r\n])\s)+", RegexOptions.Compiled);
        static readonly Regex NormalizeNewLineRegex = new(@"[\v\t\p{Zl}\p{Zp}]|\r(?!\n)", RegexOptions.Compiled);
        static readonly Regex TrimOuterBlankLinesRegex = new(@"^(\s*(\r\n?|\n))+|((\r\n?|\n)\s*)+$", RegexOptions.Compiled);
        static readonly Regex StripWsRegex = new(@"^ [ \t\u0085\p{Zs}]+(?=[\r\n\v\t\p{Zl}\p{Zp}])|(?<=[\r\n\v\t\p{Zl}\p{Zp}])[ \t\u0085\p{Zs}]+", RegexOptions.Compiled);
        static readonly Regex LeadingWsRegex = new(@"^\s+", RegexOptions.Compiled);
        static readonly Regex LeadingEmptyLine = new(@"^([^\r\n\S]+)?(\r\n?|\n)", RegexOptions.Compiled);
        static readonly Regex TrailingEmptyLine = new(@"(\r\n?|\n)([^\r\n\S]+)?$", RegexOptions.Compiled);
        static readonly Regex TrailingWsRegex = new(@"\s+$", RegexOptions.Compiled);

        #endregion

        #region Entity-Related Methods

        IEnumerable<XElement> AllRootEntityElements => EntityDefinitionsDocument.Root?.Elements(XNAME_Root).Elements(XNAME_Entity);
        IEnumerable<XElement> AllRootEnumTypeElements => EntityDefinitionsDocument.Root?.Elements(XNAME_Root).Elements(XNAME_EnumTypes).Elements();
        IEnumerable<XElement> AllRootPropertyElements => AllRootEntityElements.Elements(XNAME_Properties).Elements();
        IEnumerable<XElement> AllRootEnumFieldElements => AllRootEnumTypeElements.Elements(XNAME_Field);
        IEnumerable<XElement> AllLocalEntityElements => AllRootEntityElements.Concat(EntityDefinitionsDocument.Root?.Elements(XNAME_Local).Elements(XNAME_Entity));
        IEnumerable<XElement> AllLocalEnumTypeElements => AllRootEnumTypeElements.Concat(EntityDefinitionsDocument.Root?.Elements(XNAME_Local).Elements(XNAME_EnumTypes).Elements());
        IEnumerable<XElement> AllLocalPropertyElements => AllRootPropertyElements.Concat(AllLocalEntityElements.Elements(XNAME_Properties).Elements());
        IEnumerable<XElement> AllLocalEnumFieldElements => AllRootEnumFieldElements.Concat(AllLocalEnumTypeElements.Elements(XNAME_Properties).Elements());
        IEnumerable<XElement> AllUpstreamEntityElements => AllRootEntityElements.Concat(EntityDefinitionsDocument.Root?.Elements(XNAME_Upstream).Elements(XNAME_Entity));
        IEnumerable<XElement> AllUpstreamEnumTypeElements => AllRootEnumTypeElements.Concat(EntityDefinitionsDocument.Root?.Elements(XNAME_Upstream).Elements(XNAME_EnumTypes).Elements());
        IEnumerable<XElement> AllUpstreamPropertyElements => AllRootPropertyElements.Concat(AllUpstreamEntityElements.Elements(XNAME_Properties).Elements());
        IEnumerable<XElement> AllUpstreamEnumFieldElements => AllRootEnumFieldElements.Concat(AllUpstreamEnumTypeElements.Elements(XNAME_Properties).Elements());
        static bool? FromXmlBoolean(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToBoolean(xml); }
            catch { return null; }
        }
        static int? FromXmlInt32(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToInt32(xml); }
            catch { return null; }
        }
        static DateTime? FromXmlDateTime(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToDateTime(xml, XmlDateTimeSerializationMode.RoundtripKind); }
            catch { return null; }
        }
        static TimeSpan? FromXmlTimeSpan(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToTimeSpan(xml); }
            catch { return null; }
        }
        static Guid? FromXmlGuid(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToGuid(xml); }
            catch { return null; }
        }
        static byte[] FromXmlBinary(string xml)
        {
            if (xml is null)
                return null;
            if ((xml = xml.Trim()).Length > 0)
                try { return Convert.FromBase64String(xml); }
                catch { return null; }
            return Array.Empty<byte>();
        }
        static TValue GetAnnotatedCacheValue<TCache, TValue>(XElement target, Func<TCache> ifNotExist)
            where TCache : class, IElementCacheItem<TValue>
        {
            if (target is null)
                return default;
            TCache cacheItem = target.Annotation<TCache>();
            if (cacheItem is null)
            {
                cacheItem = ifNotExist();
                target.AddAnnotation(cacheItem);
            }
            return cacheItem.Value;
        }
        static IEnumerable<XElement> GetElementsByNames(XElement source, params XName[] names)
        {
            if (names is null || names.Length == 0)
                return source.Elements();
            if (names.Length > 1)
                return source.Elements().Where(e => names.Any(n => e.Name == n));
            return source.Elements(names[0]);
        }
        static IEnumerable<XElement> GetElementsByName(IEnumerable<XElement> source, params XName[] names)
        {
            if (names is null || names.Length == 0)
                return source.Elements();
            if (names.Length > 1)
                return source.Elements().Where(e => names.Any(n => e.Name == n));
            return source.Elements(names[0]);
        }
        static IEnumerable<XElement> GetElementsByAttributeValue(XName name, string value, IEnumerable<XElement> source) => (value is null || source is null || !source.Any()) ?
            Enumerable.Empty<XElement>() : source.Attributes(name).Where(a => a.Value == value).Select(a => a.Parent);
        static IEnumerable<XElement> GetElementsByAttributeValue(XName name, IEnumerable<string> values, IEnumerable<XElement> source) =>
            (values is null || source is null || !values.Any() || !source.Any()) ? Enumerable.Empty<XElement>() :
            values.Distinct().SelectMany(v => source.Attributes(name).Where(a => a.Value == v)).Select(a => a.Parent);
        XElement FindRootEntityByName(string name) => GetElementsByAttributeValue(XNAME_Name, name, AllRootEntityElements).FirstOrDefault();
        XElement FindRootEnumTypeByName(string name) => GetElementsByAttributeValue(XNAME_Name, name, AllRootEnumTypeElements).FirstOrDefault();
        XElement FindLocalEntityByName(string name) => GetElementsByAttributeValue(XNAME_Name, name, AllLocalEntityElements).FirstOrDefault();
        XElement FindLocalEnumTypeByName(string name) => GetElementsByAttributeValue(XNAME_Name, name, AllLocalEnumTypeElements).FirstOrDefault();
        XElement FindUpstreamEntityByName(string name) => GetElementsByAttributeValue(XNAME_Name, name, AllUpstreamEntityElements).FirstOrDefault();
        XElement FindUpstreamEnumTypeByName(string name) => GetElementsByAttributeValue(XNAME_Name, name, AllUpstreamEnumTypeElements).FirstOrDefault();
        XElement FindRootPropertyByFullName(string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, AllRootPropertyElements).FirstOrDefault();
        XElement FindRootFieldByFullName(string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, AllRootEnumFieldElements).FirstOrDefault();
        XElement FindLocalPropertyByFullName(string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, AllLocalPropertyElements).FirstOrDefault();
        XElement FindLocalFieldByFullName(string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, AllLocalEnumFieldElements).FirstOrDefault();
        XElement FindUpstreamPropertyByFullName(string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, AllUpstreamPropertyElements).FirstOrDefault();
        XElement FindUpstreamFieldByFullName(string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, AllUpstreamEnumFieldElements).FirstOrDefault();
        static bool IsScopeElement(XElement element)
        {
            if (element is null)
                return false;
            XElement root = element.Document?.Root;
            if (root is null)
                return element.Name == XNAME_Root || element.Name == XNAME_Local || element.Name == XNAME_Upstream;
            return ReferenceEquals(root, element.Parent);
        }
        static bool IsEntityElement(XElement element) => element is not null && element.Name == XNAME_Entity && (element.Parent is null || IsScopeElement(element.Parent));
        static bool IsEnumTypeElement(XElement element) => element is not null && element.Parent?.Name == XNAME_EnumTypes && (element.Parent?.Parent is null || IsScopeElement(element.Parent?.Parent));
        static bool IsPropertyElement(XElement element) => element is not null && element.Parent?.Name == XNAME_Properties && IsEntityElement(element.Parent?.Parent);
        static bool IsEnumFieldElement(XElement element) => element is not null && element.Name == XNAME_Field && IsEnumTypeElement(element.Parent);
        static XElement GetCurrentScopeElement(XElement refElement)
        {
            XElement root = refElement.Document?.Root;
            return (root is null) ? null : refElement.AncestorsAndSelf().FirstOrDefault(e => ReferenceEquals(root, e.Parent));
        }
        static XElement GetCurrentEntityElement(XElement refElement) => refElement?.AncestorsAndSelf().FirstOrDefault(IsEntityElement);
        static XElement GetCurrentPropertyElement(XElement refElement) => refElement?.AncestorsAndSelf().FirstOrDefault(IsPropertyElement);
        static IEnumerable<XElement> GetAllEntityElements(XElement refElement)
        {
            XElement scopeElement = GetCurrentScopeElement(refElement);
            if (scopeElement is null)
                return Enumerable.Empty<XElement>();
            if (scopeElement.Name != XNAME_Root)
                return scopeElement.Elements(XNAME_Entity).Concat(scopeElement.Ancestors().Take(1).Elements(XNAME_Root).Elements(XNAME_Entity));
            return scopeElement.Elements(XNAME_Entity);
        }
        static IEnumerable<XElement> GetAllPropertyElements(XElement refElement) => GetAllEntityElements(refElement).Elements(XNAME_Properties).Elements();
        static IEnumerable<XElement> GetAllEnumTypeElements(XElement refElement)
        {
            XElement scopeElement = GetCurrentScopeElement(refElement);
            if (scopeElement is null)
                return Enumerable.Empty<XElement>();
            if (scopeElement.Name != XNAME_Root)
            {
                XElement rootElement = scopeElement.Parent?.Element(XNAME_Root);
                if (rootElement is not null)
                    return scopeElement.Elements(XNAME_EnumTypes).Concat(scopeElement.Ancestors().Take(1).Elements(XNAME_Root).Elements(XNAME_EnumTypes)).Elements();
            }
            return scopeElement.Elements(XNAME_EnumTypes).Elements();
        }
        static IEnumerable<XElement> GetAllEnumFieldElements(XElement refElement) => GetAllEnumTypeElements(refElement).Elements(XNAME_Field);
        static XElement FindCurrentEntityPropertyByName(XElement refElement, string name) => (name is null) ? null :
            GetCurrentEntityElement(refElement)?.Element(XNAME_Properties).Attributes(XNAME_Name).Where(a => a.Value == name).Select(a => a.Parent).FirstOrDefault();
        static XElement FindEntityByName(XElement refElement, string name) => GetElementsByAttributeValue(XNAME_Name, name, GetAllEntityElements(refElement)).FirstOrDefault();
        static IEnumerable<XElement> FindEntitiesByNames(XElement refElement, IEnumerable<string> names) => GetElementsByAttributeValue(XNAME_Name, names, GetAllEntityElements(refElement));
        static XElement FindEnumTypeByName(XElement refElement, string name) => GetElementsByAttributeValue(XNAME_Name, name, GetAllEnumTypeElements(refElement)).FirstOrDefault();
        static XElement FindPropertyByFullName(XElement refElement, string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, GetAllPropertyElements(refElement)).FirstOrDefault();
        static XElement FindFieldByFullName(XElement refElement, string fullName) => GetElementsByAttributeValue(XNAME_FullName, fullName, GetAllEnumFieldElements(refElement)).FirstOrDefault();
        static XElement GetEnumType(XElement enumPropertyElement) => GetAnnotatedCacheValue<EnumTypeCacheItem, XElement>(enumPropertyElement,
            () => new EnumTypeCacheItem(FindEnumTypeByName(enumPropertyElement, enumPropertyElement?.Attribute(XNAME_Name)?.Value)));
        static XElement GetDefaultValueField(XElement enumPropertyElement) => GetAnnotatedCacheValue<DefaultValueCacheItem<XElement>, XElement>(enumPropertyElement,
            () => new DefaultValueCacheItem<XElement>((enumPropertyElement?.Name == XNAME_Enum) ?
                FindFieldByFullName(enumPropertyElement, enumPropertyElement.Element(XNAME_Default)?.Value) : null));
        static XElement GetAmbientValueField(XElement enumFieldElement) => GetAnnotatedCacheValue<RelatedValueCacheItem<XElement>, XElement>(enumFieldElement,
            () => new RelatedValueCacheItem<XElement>((enumFieldElement?.Name == XNAME_Field) ?
                FindFieldByFullName(enumFieldElement, enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault()) : null));
        static XElement GetRelatedEntity(XElement relatedEntityPropertyElement)
        {
            if (relatedEntityPropertyElement is null || (relatedEntityPropertyElement.Name != XNAME_RelatedEntity && relatedEntityPropertyElement.Name != XNAME_NewRelatedEntity))
                return null;
            return GetAnnotatedCacheValue<RelatedValueCacheItem<XElement>, XElement>(relatedEntityPropertyElement, () =>
                new RelatedValueCacheItem<XElement>(FindEntityByName(relatedEntityPropertyElement, relatedEntityPropertyElement.Attribute(XNAME_Reference)?.Value)));
        }
        static XElement GetItemEntity(XElement collectionNavigationPropertyElement)
        {
            if (collectionNavigationPropertyElement is null || (collectionNavigationPropertyElement.Name != XNAME_CollectionNavigation &&
                    collectionNavigationPropertyElement.Name != XNAME_NewCollectionNavigation))
                return null;
            return GetAnnotatedCacheValue<RelatedValueCacheItem<XElement>, XElement>(collectionNavigationPropertyElement, () =>
            {
                XElement itemTypeElement = collectionNavigationPropertyElement.Element(XNAME_ItemType);
                return new RelatedValueCacheItem<XElement>((itemTypeElement is null) ?
                    FindPropertyByFullName(collectionNavigationPropertyElement, collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value)?.Parent :
                    FindEntityByName(itemTypeElement, itemTypeElement.Value));
            });
        }
        static XElement[] GetBaseEntities(XElement entityElement)
        {
            if (entityElement is null || entityElement.Name != XNAME_Entity)
                return Array.Empty<XElement>();
            return GetAnnotatedCacheValue<ImmediateBaseValuesCacheItem<XElement>, XElement[]>(entityElement, () =>
                new ImmediateBaseValuesCacheItem<XElement>(FindEntitiesByNames(entityElement, entityElement.Elements(XNAME_ExtendsEntity).Attributes(XNAME_Type)
                    .Concat(entityElement.Elements(XNAME_ExtendsGenericEntity).Attributes(XNAME_TypeDef))
                    .Concat(entityElement.Attributes(XNAME_RootInterface))
                    .Concat(entityElement.Elements().Select(e => (e.Name == XNAME_ImplementsEntity) ? e.Attribute(XNAME_Type) :
                        (e.Name == XNAME_ImplementsGenericEntity) ? e.Attribute(XNAME_TypeDef) : null).Where(a => a is not null))
                    .Select(a => a.Value)).ToArray()));
        }
        static AllBaseValuesCacheItem<XElement> GetAllBaseEntitiesCachItem(XElement entityElement)
        {
            List<XElement> result = GetBaseEntities(entityElement).ToList();
            if (result.Count == 0)
                return new AllBaseValuesCacheItem<XElement>(Array.Empty<XElement>());
            foreach (XElement element in result.SelectMany((e, n) => GetAllBaseEntities(e).Select((e, i) => (e, i + result.Count + n))).OrderBy(t => t.Item2).Select(t => t.e).ToArray())
            {
                if (!result.Any(e => ReferenceEquals(e, element)))
                    result.Add(element);
            }
            return new AllBaseValuesCacheItem<XElement>(result.ToArray());
        }
        static XElement[] GetAllBaseEntities(XElement entityElement)
        {
            if (entityElement is null || entityElement.Name != XNAME_Entity)
                return Array.Empty<XElement>();
            return GetAnnotatedCacheValue<AllBaseValuesCacheItem<XElement>, XElement[]>(entityElement, () => GetAllBaseEntitiesCachItem(entityElement));
        }
        static XElement[] GetProperties(XElement entityElement) => (entityElement is null || entityElement.Name != XNAME_Entity) ? Array.Empty<XElement>() :
            GetAnnotatedCacheValue<ImmediateMembersCacheItem<XElement>, XElement[]>(entityElement,
                () => new ImmediateMembersCacheItem<XElement>(entityElement.Elements(XNAME_Properties).Elements().ToArray()));
        static (string Name, List<XElement> Sources)[] GetAllProperties(XElement entityElement)
        {
            if (entityElement is null || entityElement.Name != XNAME_Entity)
                return Array.Empty<(string Name, List<XElement> Sources)>();
            return GetAnnotatedCacheValue<AllMembersCacheItem<(string Name, List<XElement> Sources)>, (string Name, List<XElement> Sources)[]>(entityElement, () =>
            {
                List<(string Name, List<XElement> Sources)> results = new();
                foreach ((string Name, List<XElement> Sources) p in GetAllBaseEntities(entityElement).Reverse().SelectMany(e => GetAllProperties(e)))
                {
                    if (results.Any(t => t.Name == p.Name))
                    {
                        List<XElement> sources = results.First(t => t.Name == p.Name).Sources;
                        foreach (XElement s in p.Sources)
                        {
                            if (!sources.Any(e => ReferenceEquals(s, e)))
                                sources.Add(s);
                        }
                    }
                    else
                        results.Add(p);
                }
                foreach (XElement element in GetProperties(entityElement))
                {
                    string name = element.Attribute(XNAME_Name)?.Value ?? "";
                    if (results.Any(t => t.Name == name))
                    {
                        List<XElement> sources = results.First(t => t.Name == name).Sources;
                        if (!sources.Any(e => ReferenceEquals(element, e)))
                            sources.Add(element);
                    }
                    else
                    {
                        List<XElement> sources = new();
                        sources.Add(element);
                        results.Add((name, sources));
                    }
                }
                return new AllMembersCacheItem<(string Name, List<XElement> Sources)>(results.ToArray());
            });
        }
        [Obsolete()]
        static AllBasePropertiesCacheItem GetAllBasePropertiesCacheItem(XElement propertyElement)
        {
            string propertyName = propertyElement?.Attribute(XNAME_Name)?.Value;
            XElement[] allBaseEntities;
            if ((allBaseEntities = GetAllBaseEntities(propertyElement.Parent?.Parent)).Length == 0)
                return new AllBasePropertiesCacheItem(new PropertyInheritanceInfo(propertyElement, false, false, Array.Empty<XElement>(), propertyElement));
            XName baseName;
            bool n, e;
            switch (propertyElement.Name.LocalName)
            {
                case NAME_NewIdNavRef:
                    n = false;
                    e = true;
                    baseName = XNAME_UniqueIdentifier;
                    break;
                case NAME_NewRelatedEntity:
                    n = true;
                    e = false;
                    baseName = XNAME_RelatedEntity;
                    break;
                case NAME_NewCollectionNavigation:
                    n = true;
                    e = false;
                    baseName = XNAME_CollectionNavigation;
                    break;
                default:
                    return new AllBasePropertiesCacheItem(new PropertyInheritanceInfo(propertyElement, false, false,
                        GetElementsByAttributeValue(XNAME_Name, propertyName, allBaseEntities.Elements(XNAME_Properties).Elements()).ToArray(), propertyElement));
            }
            IEnumerable<XElement> results = GetElementsByAttributeValue(XNAME_Name, propertyName, allBaseEntities.Elements(XNAME_Properties).Elements());
            return new AllBasePropertiesCacheItem(new PropertyInheritanceInfo(results.Where(e => e.Name == baseName).DefaultIfEmpty(propertyElement).First(), n, e,
                results.ToArray(), propertyElement));
        }
        [Obsolete()]
        static PropertyInheritanceInfo GetAllBaseProperties(XElement propertyElement)
        {
            if (propertyElement is null || propertyElement.Parent?.Name != XNAME_Properties)
                return new PropertyInheritanceInfo(propertyElement, false, true, Array.Empty<XElement>(), propertyElement);
            return GetAnnotatedCacheValue<AllBasePropertiesCacheItem, PropertyInheritanceInfo>(propertyElement, () => GetAllBasePropertiesCacheItem(propertyElement));
        }
        static XText ToWhiteSpaceNormalized(XText source)
        {
            string text = source.Value;
            if (NormalizeNewLineRegex.IsMatch(text))
                text = NormalizeNewLineRegex.Replace(text, Environment.NewLine);
            if (TrimOuterBlankLinesRegex.IsMatch(text))
                text = TrimOuterBlankLinesRegex.Replace(text, Environment.NewLine);
            if (NormalizeWsRegex.IsMatch(text))
                text = NormalizeWsRegex.Replace(text, " ");
            return new XText(StripWsRegex.IsMatch(text) ? StripWsRegex.Replace(text, "") : text);
        }
        static XElement WsNormalizedWithoutElementNamespace(XElement sourceParent)
        {
            if (sourceParent.Name.LocalName == XNAME_langword)
                return new XElement(XNamespace.None.GetName(XNAME_see), new XAttribute(XNAME_langword, sourceParent.Value));
            XElement resultElement = new(XNamespace.None.GetName(sourceParent.Name.LocalName));
            foreach (XAttribute attribute in sourceParent.Attributes())
                resultElement.Add(new XAttribute(attribute));
            XNode sourceNode = sourceParent.Nodes().FirstOrDefault(n => n is XElement || n is XText t && !string.IsNullOrWhiteSpace(t.Value));
            if (sourceNode is null)
                return resultElement;
            if (sourceNode is XText text)
                resultElement.Add(ToWhiteSpaceNormalized(text));
            else
                resultElement.Add(WsNormalizedWithoutElementNamespace((XElement)sourceNode));
            while ((sourceNode = sourceNode.NodesAfterSelf().FirstOrDefault(n => n is XElement || n is XText t && (!string.IsNullOrWhiteSpace(t.Value) || (t.NextNode is not null && t.NextNode is not XText)))) is not null)
            {
                if (sourceNode is XText xText)
                    resultElement.Add(ToWhiteSpaceNormalized(xText));
                else
                    resultElement.Add(WsNormalizedWithoutElementNamespace((XElement)sourceNode));
            }
            return resultElement;
        }
        static XElement WithoutElementNamespace(XElement sourceParent)
        {
            if (sourceParent.Name.LocalName == XNAME_langword)
                return new XElement(XNamespace.None.GetName(XNAME_see), new XAttribute(XNAME_langword, sourceParent.Value));
            XElement resultElement = new(XNamespace.None.GetName(sourceParent.Name.LocalName));
            foreach (XAttribute attribute in sourceParent.Attributes())
                resultElement.Add(new XAttribute(attribute));
            XNode sourceNode = sourceParent.Nodes().FirstOrDefault(n => n is XElement || n is XText t && !string.IsNullOrWhiteSpace(t.Value));
            if (sourceNode is null)
                return resultElement;
            if (sourceNode is XText text)
                resultElement.Add(new XText(text));
            else
                resultElement.Add(WithoutElementNamespace((XElement)sourceNode));
            while ((sourceNode = sourceNode.NodesAfterSelf().FirstOrDefault(n => n is XElement || n is XText t && (!string.IsNullOrWhiteSpace(t.Value) || (t.NextNode is not null && t.NextNode is not XText)))) is not null)
            {
                if (sourceNode is XText xText)
                    resultElement.Add(new XText(xText));
                else
                    resultElement.Add(WithoutElementNamespace((XElement)sourceNode));
            }
            return resultElement;
        }
        static Type ToUnderlyingType(Type type)
        {
            if (type is null)
                return null;
            Type e;
            if (type.HasElementType)
            {
                e = type.GetElementType();
                if (e.IsArray || e.IsPointer)
                    return type;
                if (type.IsArray || type.IsPointer)
                    return ToUnderlyingType(e).MakeArrayType();
                return ToUnderlyingType(e);
            }

            if (type.IsValueType)
            {
                if (type.IsGenericType && typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                    return ToUnderlyingType(Nullable.GetUnderlyingType(type));
                if (type.IsEnum)
                    type = Enum.GetUnderlyingType(type);
                if (type.IsPrimitive || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan) || type == typeof(Guid))
                    return type;
            }
            else
            {
                if (type == typeof(string))
                    return type;
                if (type == typeof(Uri))
                    return typeof(string);
            }
            e = type.GetInterfaces().Where(i => i.IsGenericType && typeof(IEnumerable<>) == i.GetGenericTypeDefinition()).Select(i => i.GetGenericArguments()[0]).FirstOrDefault();
            if (e is not null)
            {
                Type u = ToUnderlyingType(e);
                if (u.IsPrimitive || u == typeof(string) || u == typeof(decimal) || u == typeof(DateTime) || u == typeof(DateTimeOffset) || u == typeof(TimeSpan) || u == typeof(Guid))
                    return u.MakeArrayType();
                return e.MakeArrayType();
            }
            return type;
        }
        static int FindPrimeNumber(int startValue)
        {
            try
            {
                if ((Math.Abs(startValue) & 1) == 0)
                    startValue++;
                while (!IsPrimeNumber(startValue))
                    startValue += 2;
            }
            catch (OverflowException) { return 1; }
            return startValue;
        }
        static bool IsPrimeNumber(int value)
        {
            if (((value = Math.Abs(value)) & 1) == 0)
                return false;
            for (int i = value >> 1; i > 1; i--)
            {
                if (value % i == 0)
                    return false;
            }
            return true;
        }
        interface IElementCacheItem<T> { T Value { get; } }
        public class DefaultValueCacheItem<T> : IElementCacheItem<T> { public T Value { get; } public DefaultValueCacheItem(T value) { Value = value; } }
        public class EnumTypeCacheItem : IElementCacheItem<XElement> { public XElement Value { get; } public EnumTypeCacheItem(XElement value) { Value = value; } }
        public class RelatedValueCacheItem<T> : IElementCacheItem<T> { public T Value { get; } public RelatedValueCacheItem(T value) { Value = value; } }
        public class ImmediateBaseValuesCacheItem<T> : IElementCacheItem<T[]> { public T[] Value { get; } public ImmediateBaseValuesCacheItem(T[] value) { Value = value; } }
        public class AllBaseValuesCacheItem<T> : IElementCacheItem<T[]> { public T[] Value { get; } public AllBaseValuesCacheItem(T[] value) { Value = value; } }
        public class ImmediateMembersCacheItem<T> : IElementCacheItem<T[]> { public T[] Value { get; } public ImmediateMembersCacheItem(T[] value) { Value = value; } }
        public class AllMembersCacheItem<T> : IElementCacheItem<T[]> { public T[] Value { get; } public AllMembersCacheItem(T[] value) { Value = value; } }
        public interface IMemberGenerationInfo
        {
            string Name { get; }
            DbType Type { get; }
            string CsTypeName { get; }
            string SqlTypeName { get; }
            XElement Source { get; }
            ITypeGenerationInfo DeclaringType { get; }
        }
        public interface ITypeGenerationInfo
        {
            string Name { get; }
            XElement Source { get; }
            IEnumerable<IMemberGenerationInfo> GetMembers();
        }
        public struct PropertyValueCode
        {
            public string CsCode { get; }
            public string SqlCode { get; }
            public static readonly PropertyValueCode Null = new("null", "NULL");
            public static readonly PropertyValueCode Now = new("DateTime.Now", "datetime('now','localtime')");
            public static readonly PropertyValueCode Zero = new("TimeSpan.Zero", "'0:00:00'");

            private PropertyValueCode(string csCode, string sqlCode)
            {
                CsCode = csCode;
                SqlCode = sqlCode;
            }
            internal static PropertyValueCode? Of(bool? value) => value.HasValue ? (value.Value ? new("true", "1") : new("false", "0")) : null;
            internal static PropertyValueCode? Of(byte? value) => value.HasValue ? new($"(byte){value.Value}", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(sbyte? value) => value.HasValue ? new($"(sbyte){value.Value}", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(short? value) => value.HasValue ? new($"(short){value.Value}", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(ushort? value) => value.HasValue ? new($"(ushort){value.Value}", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(int? value) => value.HasValue ? new(value.Value.ToString(), value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(uint? value) => value.HasValue ? new($"{value.Value}u", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(long? value) => value.HasValue ? new($"{value.Value}L", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(ulong? value) => value.HasValue ? new($"{value.Value}LU", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(decimal? value) => value.HasValue ? new($"{value.Value}m", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(double? value)
            {
                if (value.HasValue)
                {
                    string s = value.Value.ToString();
                    return new(s.Contains('.') ? $"{s}.0" : s, s);
                }
                return null;
            }
            internal static PropertyValueCode? Of(float? value) => value.HasValue ? new($"{value.Value}f", value.Value.ToString()) : null;
            internal static PropertyValueCode? Of(char? value)
            {
                if (value.HasValue)
                    switch (value.Value)
                    {
                        case '\\':
                            return new("'\\\\'", "N'\\'");
                        case '\'':
                            return new("'\\\''", "N''''");
                        case '\a':
                            return new("'\\\a'", "N'\a'");
                        case '\b':
                            return new("'\\\b'", "N'\b'");
                        case '\f':
                            return new("'\\\f'", "N'\f'");
                        case '\n':
                            return new("'\\\n'", "N'\n'");
                        case '\r':
                            return new("'\\\r'", "N'\r'");
                        case '\t':
                            return new("'\\\t'", "N'\t'");
                        case '\v':
                            return new("'\\\v'", "N'\v'");
                        default:
                            if (char.IsControl(value.Value) || char.IsWhiteSpace(value.Value) || char.IsHighSurrogate(value.Value) || char.IsLowSurrogate(value.Value))
                            {
                                int i = (int)value.Value;
                                if (i < 256)
                                    return new($"'\\x{i:X2}'", $"N'{value.Value}'");
                                return new($"'\\u{i:X4}'", $"N'{value.Value}'");
                            }
                            return new($"'{value.Value}'", $"N'{value.Value}'");
                    }
                return null;
            }
            internal static PropertyValueCode? Of(DateTime? value) =>
                value.HasValue ? new($"new DateTime({value.Value.Year}, {value.Value.Month}, {value.Value.Day}, {value.Value.Hour}, {value.Value.Minute}, {value.Value.Second})",
                $"'{value.Value:yyyy-MM-dd HH:mm:ss}'") : null;
            internal static PropertyValueCode? Of(Guid? value) => value.HasValue ? new(value.Value.Equals(Guid.Empty) ? "Guid.Empty" : $"Guid.Parse(\"{value.Value:N}\")",
                $"'{value:N}'") : null;
            internal static PropertyValueCode? Of(TimeSpan? value) => value.HasValue ? new(value.Value.Equals(TimeSpan.Zero) ? "TimeSpan.Zero" :
                $"new TimeSpan({value.Value.Days}, {value.Value.Hours}, {value.Value.Minutes}, {value.Value.Seconds}),",
                (value.Value.Hours == 0) ? $"'{value.Value:h\\:mm\\:ss}'" : $"'{(value.Value.Days * 24) + value.Value.Hours}{value.Value:mm\\:ss}'") : null;
            internal static PropertyValueCode? Of(string value)
            {
                if (value is null)
                    return null;
                if (value.Length == 0)
                    return new("\"\"", "N''");
                return new($"\"{value.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\a", @"\a").Replace("\b", @"\b").Replace("\f", @"\f").Replace("\n", @"\n").Replace("\r", @"\r").Replace("\t", @"\t").Replace("\v", @"\v")}\"",
                    $"N'{value.Replace("'", "''")}'");
            }
            internal static PropertyValueCode? Of(byte[] value)
            {
                if (value is null)
                    return null;
                if (value.Length == 0)
                    return new("Array.Empty<byte>()", "x''");
                return new($"Convert.FromBase64String(\"{Convert.ToBase64String(value)}\")", $"x'{BitConverter.ToString(value)}'");
            }
            internal static PropertyValueCode? Of(DriveType? driveType) => driveType.HasValue ? new($"{nameof(DriveType)}.{driveType:F}", ((int)driveType).ToString()) :
                null;
            internal static PropertyValueCode? OfEnumType(FieldGenerationInfo field)
            {
                if (field is null)
                    return null;
                return new PropertyValueCode(field.Name, field.RawValue.ToString());
            }
            internal static T? ToValue<T>(string text, Func<string, T> converter)
                where T : struct
            {
                if (text is not null && (text = text.Trim()).Length > 0)
                    try { return converter(text); } catch { return null; }
                return null;
            }
            internal static T? ElementToValue<T>(XName name, XElement element, Func<string, T> converter)
                where T : struct => ((element = element?.Element(name)) is null || element.IsEmpty) ? null : ToValue(element.Value, converter);
            internal static T? AttributeToValue<T>(XName name, XElement element, Func<string, T> converter)
                where T : struct => ToValue(element?.Attribute(name)?.Value, converter);
            internal static T? ToFirstValue<T>(IEnumerable<string> values, Func<string, T> converter)
                where T : struct
            {
                foreach (string s in values.Where(s => s is not null).Select(s => s.Trim()))
                {
                    if (s.Length > 0)
                        try { return converter(s); } catch { /* okay to ignore */ }
                }
                return null;
            }
            internal static T? ToFirstElementValue<T>(XName name, IEnumerable<XElement> sourceElements, Func<string, T> converter)
                where T : struct => ToFirstValue(sourceElements.Elements(name).Select(e => (e is null || e.IsEmpty) ? null : e.Value), converter);
            internal static T? ToFirstAttributeValue<T>(XName name, IEnumerable<XElement> sourceElements, Func<string, T> converter)
                where T : struct => ToFirstValue(sourceElements.Attributes(name).Select(a => a?.Value), converter);
            internal static T ToFirstObject<T>(IEnumerable<string> values, Func<string, T> converter)
                where T : class
            {
                foreach (string s in values.Where(s => s is not null).Select(s => s.Trim()))
                {
                    if (s.Length > 0)
                        try
                        {
                            T result = converter(s);
                            if (result is not null)
                                return result;
                        }
                        catch { /* okay to ignore */ }
                }
                return null;
            }
            internal static T ToFirstElementObject<T>(XName name, IEnumerable<XElement> sourceElements, Func<string, T> converter)
                where T : class => ToFirstObject(sourceElements.Elements(name).Select(e => (e is null || e.IsEmpty) ? null : e.Value), converter);
            internal static T ToFirstAttributeObject<T>(XName name, IEnumerable<XElement> sourceElements, Func<string, T> converter)
                where T : class => ToFirstObject(sourceElements.Attributes(name).Select(a => a?.Value), converter);
            internal static T ToObject<T>(string text, Func<string, T> converter)
                where T : class
            {
                if (text is not null && (text = text.Trim()).Length > 0)
                    try { return converter(text); } catch { return null; }
                return null;
            }
            public static string ElementToString(XName name, IEnumerable<XElement> sourceElements) => sourceElements?.Elements(name).Where(e => !e.IsEmpty)
                .Select(e => e.Value).FirstOrDefault();
            public static string ElementToString(XName name, XElement element) => element?.Elements(name).Where(e => !e.IsEmpty)
                .Select(e => e.Value).FirstOrDefault();
            public static string AttributeToString(XName name, IEnumerable<XElement> sourceElements) => sourceElements?.Attributes(name)
                .Select(e => e.Value).FirstOrDefault();
            public static string AttributeToString(XName name, XElement element) => element?.Attributes(name).Select(e => e.Value).FirstOrDefault();
            public static byte[] ElementToBinary(XName name, IEnumerable<XElement> sourceElements) => ToFirstObject(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), Convert.FromBase64String);
            public static byte[] ElementToBinary(XName name, XElement element) => ToObject(element.Element(name)?.Value, Convert.FromBase64String);
            public static byte[] AttributeToBinary(XName name, IEnumerable<XElement> sourceElements) => ToFirstObject(sourceElements.Attributes(name)
                .Select(a => a.Value), Convert.FromBase64String);
            public static byte[] AttributeToBinary(XName name, XElement element) => ToObject(element.Attribute(name)?.Value, Convert.FromBase64String);
            public static bool? ElementToBoolean(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToBoolean);
            public static bool? ElementToBoolean(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToBoolean);
            public static bool? AttributeToBoolean(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToBoolean);
            public static bool? AttributeToBoolean(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToBoolean);
            public static byte? ElementToByte(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToByte);
            public static byte? ElementToByte(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToByte);
            public static byte? AttributeToByte(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToByte);
            public static byte? AttributeToByte(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToByte);
            public static sbyte? ElementToSByte(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToSByte);
            public static sbyte? ElementToSByte(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToSByte);
            public static sbyte? AttributeToSByte(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToSByte);
            public static sbyte? AttributeToSByte(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToSByte);
            public static char? ElementToChar(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToChar);
            public static char? ElementToChar(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToChar);
            public static char? AttributeToChar(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToChar);
            public static char? AttributeToChar(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToChar);
            public static DateTime? ElementToDateTime(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));
            public static DateTime? ElementToDateTime(XName name, XElement element) => ToValue(element.Element(name)?.Value,
                t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));
            public static DateTime? AttributeToDateTime(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));
            public static DateTime? AttributeToDateTime(XName name, XElement element) => ToValue(element.Attribute(name)?.Value,
                t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));
            public static Guid? ElementToGuid(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToGuid);
            public static Guid? ElementToGuid(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToGuid);
            public static Guid? AttributeToGuid(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToGuid);
            public static Guid? AttributeToGuid(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToGuid);
            public static short? ElementToInt16(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToInt16);
            public static short? ElementToInt16(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToInt16);
            public static short? AttributeToInt16(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToInt16);
            public static short? AttributeToInt16(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToInt16);
            public static int? ElementToInt32(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToInt32);
            public static int? ElementToInt32(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToInt32);
            public static int? AttributeToInt32(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToInt32);
            public static int? AttributeToInt32(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToInt32);
            public static long? ElementToInt64(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToInt64);
            public static long? ElementToInt64(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToInt64);
            public static long? AttributeToInt64(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToInt64);
            public static long? AttributeToInt64(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToInt64);
            public static ushort? ElementToUInt16(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToUInt16);
            public static ushort? ElementToUInt16(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToUInt16);
            public static ushort? AttributeToUInt16(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToUInt16);
            public static ushort? AttributeToUInt16(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToUInt16);
            public static uint? ElementToUInt32(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToUInt32);
            public static uint? ElementToUInt32(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToUInt32);
            public static uint? AttributeToUInt32(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToUInt32);
            public static uint? AttributeToUInt32(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToUInt32);
            public static ulong? ElementToUInt64(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToUInt64);
            public static ulong? ElementToUInt64(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToUInt64);
            public static ulong? AttributeToUInt64(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToUInt64);
            public static ulong? AttributeToUInt64(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToUInt64);
            public static decimal? ElementToDecimal(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToDecimal);
            public static decimal? ElementToDecimal(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToDecimal);
            public static decimal? AttributeToDecimal(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToDecimal);
            public static decimal? AttributeToDecimal(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToDecimal);
            public static double? ElementToDouble(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToDouble);
            public static double? ElementToDouble(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToDouble);
            public static double? AttributeToDouble(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToDouble);
            public static double? AttributeToDouble(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToDouble);
            public static float? ElementToSingle(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToSingle);
            public static float? ElementToSingle(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToSingle);
            public static float? AttributeToSingle(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToSingle);
            public static float? AttributeToSingle(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToSingle);
            public static TimeSpan? ElementToTimeSpan(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToTimeSpan);
            public static TimeSpan? ElementToTimeSpan(XName name, XElement element) => ToValue(element.Element(name)?.Value, XmlConvert.ToTimeSpan);
            public static TimeSpan? AttributeToTimeSpan(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), XmlConvert.ToTimeSpan);
            public static TimeSpan? AttributeToTimeSpan(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToTimeSpan);
            public static DriveType? ElementToDriveType(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Elements(name)
                .Select(e => e.IsEmpty ? null : e.Value), s => Enum.Parse<DriveType>(s));
            public static DriveType? ElementToDriveType(XName name, XElement element) => ToValue(element.Element(name)?.Value, s => Enum.Parse<DriveType>(s));
            public static DriveType? AttributeToDriveType(XName name, IEnumerable<XElement> sourceElements) => ToFirstValue(sourceElements.Attributes(name)
                .Select(a => a.Value), s => Enum.Parse<DriveType>(s));
            public static DriveType? AttributeToDriveType(XName name, XElement element) => ToValue(element.Attribute(name)?.Value, s => Enum.Parse<DriveType>(s));
        }
        public class FieldGenerationInfo : IMemberGenerationInfo
        {
            internal FieldGenerationInfo(XElement fieldElement, EnumGenerationInfo declaringType, Func<string, IComparable> toRawValue)
            {
                Name = (Source = fieldElement).Attribute(XNAME_Name)?.Value;
                DeclaringType = declaringType;
                RawValue = toRawValue(fieldElement.Attribute(XNAME_Value)?.Value);
                Value = PropertyValueCode.OfEnumType(this).Value;
            }
            public string Name { get; }
            public PropertyValueCode Value { get; }
            public IComparable RawValue { get; }
            public DbType Type => DeclaringType.Type;
            public string CsTypeName => DeclaringType.CsTypeName;
            public XElement Source { get; }
            public EnumGenerationInfo DeclaringType { get; }
            ITypeGenerationInfo IMemberGenerationInfo.DeclaringType => DeclaringType;
            public string SqlTypeName => DeclaringType.SqlTypeName;
        }
        public class EnumGenerationInfo : ITypeGenerationInfo
        {
            private EnumGenerationInfo(XElement enumElement)
            {
                Func<string, IComparable> toRawValue;
                Func<IEnumerable<IComparable>, (PropertyValueCode Min, PropertyValueCode Max)> getRange;
                IsFlags = PropertyValueCode.AttributeToBoolean(XNAME_IsFlags, enumElement) ?? false;
                Name = (Source = enumElement).Attribute(NAME_Name)?.Value;
                if (enumElement.Name.Namespace != XNAME_EnumTypes.Namespace)
                {
                    Type = DbType.Int32;
                    CsTypeName = "int";
                    SqlTypeName = SQL_TYPENAME_INT;
                    toRawValue = s => XmlConvert.ToInt32(s);
                    if (IsFlags)
                        getRange = en => (PropertyValueCode.Of(en.Cast<int>().Min()).Value, PropertyValueCode.Of(en.Cast<int>().Aggregate((x, y) => x | y)).Value);
                    else
                        getRange = en => (PropertyValueCode.Of(en.Cast<int>().Min()).Value, PropertyValueCode.Of(en.Cast<int>().Max()).Value);
                }
                else
                {
                    switch (enumElement.Name.LocalName)
                    {
                        case NAME_Byte:
                            Type = DbType.Byte;
                            CsTypeName = "byte";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                            toRawValue = s => XmlConvert.ToByte(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<byte>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<byte>().Aggregate((x, y) => (byte)(x | y))).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<byte>().Min()).Value,
                                    PropertyValueCode.Of(en.Cast<byte>().Max()).Value);
                            break;
                        case NAME_SByte:
                            Type = DbType.SByte;
                            CsTypeName = "sbyte";
                            SqlTypeName = SQL_TYPENAME_TINYINT;
                            toRawValue = s => XmlConvert.ToSByte(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<sbyte>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<sbyte>().Aggregate((x, y) => (sbyte)(x | y))).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<sbyte>().Min()).Value, PropertyValueCode.Of(en.Cast<sbyte>().Max()).Value);
                            break;
                        case NAME_Short:
                            Type = DbType.Int16;
                            CsTypeName = "short";
                            SqlTypeName = SQL_TYPENAME_SMALLINT;
                            toRawValue = s => XmlConvert.ToInt16(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<short>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<short>().Aggregate((x, y) => (short)(x | y))).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<short>().Min()).Value, PropertyValueCode.Of(en.Cast<short>().Max()).Value);
                            break;
                        case NAME_UShort:
                            Type = DbType.UInt16;
                            CsTypeName = "ushort";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_SMALLINT;
                            toRawValue = s => XmlConvert.ToUInt16(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<ushort>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<ushort>().Aggregate((x, y) => (ushort)(x | y))).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<ushort>().Min()).Value, PropertyValueCode.Of(en.Cast<ushort>().Max()).Value);
                            break;
                        case NAME_UInt:
                            Type = DbType.UInt32;
                            CsTypeName = "uint";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_INT;
                            toRawValue = s => XmlConvert.ToUInt32(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<uint>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<uint>().Aggregate((x, y) => x | y)).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<uint>().Min()).Value, PropertyValueCode.Of(en.Cast<uint>().Max()).Value);
                            break;
                        case NAME_Long:
                            Type = DbType.Int64;
                            CsTypeName = "long";
                            SqlTypeName = SQL_TYPENAME_BIGINT;
                            toRawValue = s => XmlConvert.ToInt64(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<long>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<long>().Aggregate((x, y) => x | y)).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<long>().Min()).Value, PropertyValueCode.Of(en.Cast<long>().Max()).Value);
                            break;
                        case NAME_ULong:
                            Type = DbType.UInt64;
                            CsTypeName = "ulong";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_BIGINT;
                            toRawValue = s => XmlConvert.ToUInt64(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<ulong>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<ulong>().Aggregate((x, y) => x | y)).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<ulong>().Min()).Value, PropertyValueCode.Of(en.Cast<ulong>().Max()).Value);
                            break;
                        default:
                            Type = DbType.Int32;
                            CsTypeName = "int";
                            SqlTypeName = SQL_TYPENAME_INT;
                            toRawValue = s => XmlConvert.ToInt32(s);
                            if (IsFlags)
                                getRange = en => (PropertyValueCode.Of(en.Cast<int>().Min()).Value,
                                PropertyValueCode.Of(en.Cast<int>().Aggregate((x, y) => x | y)).Value);
                            else
                                getRange = en => (PropertyValueCode.Of(en.Cast<int>().Min()).Value, PropertyValueCode.Of(en.Cast<int>().Max()).Value);
                            break;
                    }
                }
                Fields = new ReadOnlyCollection<FieldGenerationInfo>(enumElement.Elements(XNAME_Field).Select(e => new FieldGenerationInfo(e, this, toRawValue))
                    .ToArray());
                (PropertyValueCode min, PropertyValueCode max) = getRange(Fields.Select(f => f.RawValue));
                MinValue = min;
                MaxValue = max;
            }
            public string Name { get; }
            public DbType Type { get; }
            public string CsTypeName { get; }
            public string SqlTypeName { get; }
            public XElement Source { get; }
            public PropertyValueCode MaxValue { get; }
            public PropertyValueCode MinValue { get; }
            public ReadOnlyCollection<FieldGenerationInfo> Fields { get; }
            public bool IsFlags { get; }

            internal static EnumGenerationInfo Get(XElement xElement)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IMemberGenerationInfo> GetMembers()
            {
                throw new NotImplementedException();
            }
        }
        public class PropertyGenerationInfo : IMemberGenerationInfo
        {
            private PropertyGenerationInfo(XElement propertyElement, EntityGenerationInfo entity)
            {
                Name = (Source = propertyElement).Attribute(NAME_Name)?.Value;
                DeclaringType = entity;
                Inherited = new(entity.BaseTypes.SelectMany(e => e.Entity.Properties.Where(p => p.Name == Name)).Distinct().OrderBy(p => p, new ProxyValueComparer<PropertyGenerationInfo>((x, y) =>
                    x.DeclaringType.Extends(y.DeclaringType) ? -1 : y.DeclaringType.Extends(x.DeclaringType) ? 1 : 0)).ToArray());
                ColumnName = propertyElement.Attribute(NAME_ColName)?.Value ?? Inherited.Select(p => p.ColumnName).FirstOrDefault(n => n is not null);
                bool defaultNull = propertyElement.Elements(XNAME_DefaultNull).Any() || Inherited.Select(i => i.Source).Elements(XNAME_DefaultNull).Any();
                AllowNull = defaultNull || (FromXmlBoolean(propertyElement.Attribute(XNAME_AllowNull)?.Value) ??
                    Inherited.Any(i => i.Source.Attributes(XNAME_AllowNull).Any(a => FromXmlBoolean(a.Value) ?? false)));
                IsGenericWritable = FromXmlBoolean(propertyElement.Attribute(XNAME_IsGenericWritable)?.Value) ??
                    Inherited.Any(i => i.Source.Attributes(XNAME_IsGenericWritable).Any(a => FromXmlBoolean(a.Value) ?? false));
                if (propertyElement.Name.Namespace != XNAME_UniqueIdentifier.Namespace)
                {
                    Type = DbType.Object;
                    CsTypeName = "object";
                    SqlTypeName = SQL_TYPENAME_BLOB;
                }
                else
                {
                    IEnumerable<XElement> allPropertyElements = Enumerable.Repeat(propertyElement, 1).Concat(Inherited.Select(i => i.Source));
                    switch (propertyElement.Name.LocalName)
                    {
                        case NAME_Bit:
                            Type = DbType.Boolean;
                            CsTypeName = "bool";
                            SqlTypeName = SQL_TYPENAME_BIT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToBoolean(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_Byte:
                            Type = DbType.Byte;
                            CsTypeName = "byte";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToByte(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToByte(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToByte(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_SByte:
                            Type = DbType.SByte;
                            CsTypeName = "sbyte";
                            SqlTypeName = SQL_TYPENAME_TINYINT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToSByte(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToSByte(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToSByte(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Short:
                            Type = DbType.Int16;
                            CsTypeName = "short";
                            SqlTypeName = SQL_TYPENAME_SMALLINT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToInt16(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToInt16(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToInt16(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_UShort:
                            Type = DbType.UInt16;
                            CsTypeName = "ushort";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_SMALLINT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToUInt16(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToUInt16(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToUInt16(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Int:
                            Type = DbType.Int32;
                            CsTypeName = "int";
                            SqlTypeName = SQL_TYPENAME_INT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToInt32(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToInt32(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToInt32(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_UInt:
                            Type = DbType.UInt32;
                            CsTypeName = "uint";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_INT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToUInt32(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToUInt32(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToUInt32(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Long:
                            Type = DbType.Int64;
                            CsTypeName = "long";
                            SqlTypeName = SQL_TYPENAME_BIGINT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToInt64(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToInt64(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToInt64(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_ULong:
                            Type = DbType.UInt64;
                            CsTypeName = "ulong";
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_BIGINT;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToUInt64(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToUInt64(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToUInt64(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Float:
                            Type = DbType.Single;
                            CsTypeName = "float";
                            SqlTypeName = SQL_TYPENAME_REAL;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToSingle(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToSingle(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToSingle(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Double:
                            Type = DbType.Double;
                            CsTypeName = "double";
                            SqlTypeName = SQL_TYPENAME_REAL;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToDouble(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToDouble(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToDouble(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Decimal:
                            Type = DbType.Decimal;
                            CsTypeName = "decimal";
                            SqlTypeName = SQL_TYPENAME_NUMERIC;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToDecimal(XNAME_Default, allPropertyElements));
                            MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToDecimal(XNAME_MinValue, allPropertyElements));
                            MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.AttributeToDecimal(XNAME_MaxValue, allPropertyElements));
                            break;
                        case NAME_Char:
                            Type = DbType.StringFixedLength;
                            CsTypeName = "char";
                            SqlTypeName = $"{SQL_TYPENAME_CHARACTER}(1)";
                            MinLength = MaxLength = 1;
                            DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToChar(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_Enum:
                            CsTypeName = propertyElement.Attribute(XNAME_Type)?.Value;
                            EnumGenerationInfo enumGenerationInfo = EnumGenerationInfo.Get(FindEnumTypeByName(propertyElement, CsTypeName));
                            ReferencedType = enumGenerationInfo;
                            Type = enumGenerationInfo.Type;
                            SqlTypeName = enumGenerationInfo.SqlTypeName;
                            MinValue = enumGenerationInfo.MinValue;
                            MaxValue = enumGenerationInfo.MaxValue;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            else
                            {
                                FieldGenerationInfo defaultField = allPropertyElements.Elements(XNAME_Default).Where(e => !e.IsEmpty).Select(e => e.Value)
                                    .Select(s => enumGenerationInfo.Fields.FirstOrDefault(f => f.Name == s)).FirstOrDefault(f => f is not null);
                                if (defaultField is not null)
                                    DefaultValue = defaultField.Value;
                            }
                            break;
                        case NAME_UniqueIdentifier:
                            Type = DbType.Guid;
                            CsTypeName = nameof(Guid);
                            SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            IsCaseSensitive = false;
                            IsNormalized = true;
                            IsPrimaryKey = PropertyValueCode.AttributeToBoolean(XNAME_IsPrimaryKey, allPropertyElements) ?? false;
                            NavigationProperty = allPropertyElements.Attributes(XNAME_Navigation);
                            break;
                        case NAME_NewIdNavRef:
                            Type = DbType.Guid;
                            CsTypeName = nameof(Guid);
                            SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            IsCaseSensitive = false;
                            IsNormalized = true;
                            IsPrimaryKey = PropertyValueCode.AttributeToBoolean(XNAME_IsPrimaryKey, allPropertyElements) ?? false;
                            NavigationProperty = allPropertyElements.Attributes(XNAME_Navigation);
                            break;
                        case NAME_RelatedEntity:
                            Type = DbType.Guid;
                            CsTypeName = nameof(Guid);
                            SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            IsCaseSensitive = false;
                            IsNormalized = true;
                            DbRelationship = allPropertyElements.Elements(XNAME_DbRelationship).Attributes(XNAME_Name).Select(a => (a.Value, a.Parent.Attribute(XNAME_FkPropertyName)?.Value))
                                .Cast<(string Name, string FkPropertyName)?>().FirstOrDefault();
                            break;
                        case NAME_NewRelatedEntity:
                            Type = DbType.Guid;
                            CsTypeName = nameof(Guid);
                            SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            IsCaseSensitive = false;
                            IsNormalized = true;
                            break;
                        case NAME_NVarChar:
                            Type = DbType.String;
                            CsTypeName = "string";
                            MaxLength = FromXmlInt32(propertyElement.Attribute(XNAME_MaxLength)?.Value);
                            MinLength = FromXmlInt32(propertyElement.Attribute(XNAME_MinLength)?.Value);
                            SqlTypeName = $"{SQL_TYPENAME_NVARCHAR}({MaxLength})";
                            DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToString(XNAME_Default, allPropertyElements));
                            IsNormalized = FromXmlBoolean(propertyElement.Attribute(XNAME_IsNormalized)?.Value) ??
                                Inherited.SelectMany(i => i.Source.Attributes(XNAME_IsNormalized).Select(a => FromXmlBoolean(a.Value)))
                                .Where(b => b.HasValue).FirstOrDefault();
                            IsCaseSensitive = FromXmlBoolean(propertyElement.Attribute(XNAME_IsCaseSensitive)?.Value) ??
                                Inherited.Any(i => i.Source.Attributes(XNAME_IsCaseSensitive).Any(a => FromXmlBoolean(a.Value) ?? false));
                            break;
                        case NAME_VolumeIdentifier:
                            Type = DbType.String;
                            CsTypeName = "VolumeIdentifier";
                            MaxLength = 1024;
                            SqlTypeName = $"{SQL_TYPENAME_NVARCHAR}({MaxLength})";
                            DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToString(XNAME_Default, allPropertyElements));
                            IsCaseSensitive = false;
                            IsNormalized = true;
                            break;
                        case NAME_MultiStringValue:
                            Type = DbType.String;
                            CsTypeName = "MultiStringValue";
                            SqlTypeName = SQL_TYPENAME_TEXT;
                            DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToString(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_Text:
                            Type = DbType.String;
                            CsTypeName = "string";
                            SqlTypeName = SQL_TYPENAME_TEXT;
                            DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToString(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_DateTime:
                            Type = DbType.DateTime;
                            CsTypeName = nameof(DateTime);
                            SqlTypeName = SQL_TYPENAME_DATETIME;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            else if (propertyElement.Elements(XNAME_DefaultNull).Any() || Inherited.Select(i => i.Source).Elements(XNAME_DefaultNull).Any())
                                DefaultValue = PropertyValueCode.Now;
                            else
                                DefaultValue = PropertyValueCode.Of(PropertyValueCode.ElementToDateTime(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_TimeSpan:
                            Type = DbType.Time;
                            CsTypeName = nameof(TimeSpan);
                            SqlTypeName = SQL_TYPENAME_TIME;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            else if (propertyElement.Elements(XNAME_DefaultZero).Any() || Inherited.Select(i => i.Source).Elements(XNAME_DefaultZero).Any())
                                DefaultValue = PropertyValueCode.Zero;
                            else
                                DefaultValue = PropertyValueCode.Of(PropertyValueCode.ElementToTimeSpan(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_ByteValues:
                            Type = DbType.Binary;
                            MaxLength = FromXmlInt32(propertyElement.Attribute(XNAME_MaxLength)?.Value);
                            MinLength = FromXmlInt32(propertyElement.Attribute(XNAME_MinLength)?.Value);
                            CsTypeName = "byte[]";
                            SqlTypeName = $"{SQL_TYPENAME_VARBINARY}({MaxLength})";
                            DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(PropertyValueCode.ElementToBinary(XNAME_Default, allPropertyElements));
                            break;
                        case NAME_MD5Hash:
                            Type = DbType.Binary;
                            MinLength = MaxLength = 16;
                            CsTypeName = "MD5Hash";
                            SqlTypeName = $"{SQL_TYPENAME_BINARY}(16)";
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            break;
                        case NAME_DriveType:
                            Type = DbType.Byte;
                            CsTypeName = nameof(DriveType);
                            IEnumerable<DriveType> values = Enum.GetValues<DriveType>().OrderBy(dt => dt);
                            MinValue = PropertyValueCode.Of(values.First());
                            MaxValue = PropertyValueCode.Of(values.Last());
                            SqlTypeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                            if (defaultNull)
                                DefaultValue = PropertyValueCode.Null;
                            else
                            {
                                DefaultValue = PropertyValueCode.Of(PropertyValueCode.AttributeToDriveType(XNAME_Default, allPropertyElements));
                            }
                            break;
                        case NAME_CollectionNavigation:
                            Type = DbType.Object;
                            break;
                        case NAME_NewCollectionNavigation:
                            Type = DbType.Object;
                            break;
                        default:
                            Type = DbType.Object;
                            CsTypeName = "object";
                            SqlTypeName = SQL_TYPENAME_BLOB;
                            break;
                    }
                }
            }
            public string Name { get; }
            public string ColumnName { get; }
            public DbType Type { get; }
            public string CsTypeName { get; }
            public string SqlTypeName { get; }
            public int? MaxLength { get; }
            public int? MinLength { get; }
            public PropertyValueCode? MaxValue { get; }
            public PropertyValueCode? MinValue { get; }
            public PropertyValueCode? DefaultValue { get; }
            public bool? IsNormalized { get; }
            public (string Name, string FkPropertyName)? DbRelationship { get; }
            public bool IsPrimaryKey { get; }
            public IEnumerable<XAttribute> NavigationProperty { get; }
            public bool? IsCaseSensitive { get; }
            public ReadOnlyCollection<PropertyGenerationInfo> Inherited { get; }
            public ITypeGenerationInfo ReferencedType { get; }
            public EntityGenerationInfo DeclaringType { get; }
            ITypeGenerationInfo IMemberGenerationInfo.DeclaringType => DeclaringType;
            public XElement Source { get; }
            public bool AllowNull { get; }
            public bool IsGenericWritable { get; }
            internal static PropertyGenerationInfo Get(XElement propertyElement)
            {
                if (propertyElement is null)
                    return null;
                EntityGenerationInfo entityGenerationInfo = EntityGenerationInfo.Get(propertyElement.Parent);
                PropertyGenerationInfo result = propertyElement.Annotation<PropertyGenerationInfo>();
                if (result is not null)
                    return result;
                if (IsPropertyElement(propertyElement) && entityGenerationInfo is not null)
                    return new PropertyGenerationInfo(propertyElement, entityGenerationInfo);
                return null;
            }
        }
        public class EntityGenerationInfo : ITypeGenerationInfo
        {
            private ReadOnlyCollection<EntityGenerationInfo> _allOrdered = null;
            private EntityGenerationInfo(XElement entityElement, Collection<PropertyGenerationInfo> properties,
                Collection<(string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties)> uniqueConstraints)
            {
                Name = (Source = entityElement).Attribute(NAME_Name)?.Value;
                TableName = entityElement.Attribute(NAME_TableName)?.Value;
                BaseTypes = new(entityElement.Elements(XNAME_ExtendsGenericEntity).Select(e => (false, e.Attribute(XNAME_Type)?.Value, Get(FindEntityByName(entityElement, e.Attribute(XNAME_TypeDef)?.Value))))
                    .Concat(entityElement.Elements(XNAME_ExtendsEntity).Attributes(XNAME_Type).Select(a => (false, a.Value, Get(FindEntityByName(entityElement, a.Value)))))
                    .Concat(entityElement.Elements(XNAME_BaseType).Attributes(XNAME_Type).Select(a => (true, a.Value, (EntityGenerationInfo)null)))
                    .Concat(entityElement.Elements(XNAME_RootInterface).Attributes(XNAME_Type).Select(a => (false, a.Value, Get(FindEntityByName(entityElement, a.Value)))))
                    .Concat(entityElement.Elements().Select(e =>
                    {
                        if (e.Name == XNAME_ImplementsGenericEntity)
                            return (false, e.Attribute(XNAME_Type)?.Value, Get(FindEntityByName(entityElement, e.Attribute(XNAME_TypeDef)?.Value)));
                        if (e.Name == XNAME_ImplementsEntity)
                        {
                            string n = e.Attribute(XNAME_Type)?.Value;
                            return (false, n, Get(FindEntityByName(entityElement, n)));
                        }
                        if (e.Name == XNAME_Implements)
                            return (true, e.Attribute(XNAME_Type)?.Value, (EntityGenerationInfo)null);
                        return (false, null, null);
                    })).Where(t => t.Item1 ? t.Item2 is not null : t.Item3 is not null).Select(t => (t.Item2 ?? t.Item3.Name, t.Item3)).ToArray());
                Properties = new ReadOnlyCollection<PropertyGenerationInfo>(properties);
                UniqueConstraints = new(uniqueConstraints);
                CheckConstraint cc = CheckConstraint.Import(entityElement.Element(XNAME_Check));
                foreach ((string Name, EntityGenerationInfo Entity) in BaseTypes)
                {
                    CheckConstraint bc = Entity?.CheckConstraints;
                    if (bc is not null)
                        cc = (cc is null) ? bc : cc.And(bc);
                }
                CheckConstraints = cc;
            }
            public string Name { get; }
            public XElement Source { get; }
            public string TableName { get; }
            public ReadOnlyCollection<(string Name, EntityGenerationInfo Entity)> BaseTypes { get; }
            public ReadOnlyCollection<PropertyGenerationInfo> Properties { get; }
            public ReadOnlyCollection<(string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties)> UniqueConstraints { get; }
            public CheckConstraint CheckConstraints { get; }
            public bool Extends(EntityGenerationInfo other) => !(other is null || ReferenceEquals(this, other)) &&
                (BaseTypes.Any(e => ReferenceEquals(other, e.Entity)) || BaseTypes.Any(e => e.Entity.Extends(other)));
            IEnumerable<EntityGenerationInfo> GetAllBaseTypes() => (BaseTypes.Count > 0) ?
                BaseTypes.Select(b => b.Entity).Concat(BaseTypes.SelectMany(b => b.Entity.GetAllBaseTypes())).Distinct() : Enumerable.Empty<EntityGenerationInfo>();
            public ReadOnlyCollection<EntityGenerationInfo> GetAllBaseTypesOrdered()
            {
                if (_allOrdered is not null)
                    return _allOrdered;
                if (BaseTypes.Count == 0)
                    _allOrdered = new(Array.Empty<EntityGenerationInfo>());
                else
                {
                    LinkedList<EntityGenerationInfo> result = new();
                    result.AddLast(BaseTypes[0].Entity);
                    foreach (EntityGenerationInfo g in GetAllBaseTypes().Skip(1))
                    {
                        LinkedListNode<EntityGenerationInfo> node = result.Last;
                        while (g.Extends(node.Value))
                        {
                            if ((node = node.Previous) is null)
                                break;
                        }
                        if (node is null)
                            result.AddFirst(g);
                        else if (!ReferenceEquals(node.Value, g))
                            result.AddAfter(node, g);
                    }
                    _allOrdered = new(result.ToArray());
                }
                return _allOrdered;
            }
            public static EntityGenerationInfo Get(XElement entityElement)
            {
                if (entityElement is null)
                    return null;
                EntityGenerationInfo result = entityElement.Annotation<EntityGenerationInfo>();
                if (result is not null)
                    return result;
                if (IsEntityElement(entityElement))
                {
                    Collection<PropertyGenerationInfo> properties = new();
                    Collection<(string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties)> uniqueConstraints = new();
                    result = new EntityGenerationInfo(entityElement, properties, uniqueConstraints);
                    entityElement.AddAnnotation(result);
                    foreach (XElement element in entityElement.Elements(XNAME_Properties).Elements())
                    {
                        PropertyGenerationInfo property = PropertyGenerationInfo.Get(element);
                        if (property is not null)
                            properties.Add(property);
                    }
                    foreach (PropertyGenerationInfo bp in result.GetAllBaseTypes().SelectMany(t => t.Properties).Distinct())
                    {
                        string n = bp.Name;
                        PropertyGenerationInfo ep = properties.FirstOrDefault(p => p.Name == n);
                        if (ep is null)
                            properties.Add(bp);
                        else if (!ReferenceEquals(ep.DeclaringType, result) && bp.DeclaringType.Extends(ep.DeclaringType))
                            properties[properties.IndexOf(ep)] = bp;
                    }
                    foreach ((string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties) uc in entityElement.Elements(XNAME_Unique).Attributes(XNAME_Name).Select<XAttribute, (string, PropertyGenerationInfo[])>(a => (
                        a.Value,
                        a.Parent.Elements(XNAME_Property).Attributes(XNAME_Name).Select(n => n.Value).Select(n => properties.FirstOrDefault(p => p.Name == n))
                        .Where(p => p is not null).ToArray()
                    )).Where(t => t.Item2.Length > 0).Select(t => (t.Item1, new ReadOnlyCollection<PropertyGenerationInfo>(t.Item2))))
                        uniqueConstraints.Add(uc);
                }
                return null;
            }
            IEnumerable<IMemberGenerationInfo> ITypeGenerationInfo.GetMembers() => Properties.Cast<IMemberGenerationInfo>();
        }
        class ProxyValueComparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _compareFunc;
            internal ProxyValueComparer(Func<T, T, int> compareFunc) { _compareFunc = compareFunc; }
            public int Compare(T x, T y) => _compareFunc(x, y);
        }
        class CalculatedValueEqualityComparer<T, V> : IEqualityComparer<T>
            where V : class
        {
            private readonly Func<T, V> _getValue;
            private readonly IEqualityComparer<V> _resultComparer;
            internal CalculatedValueEqualityComparer(Func<T, V> getValue, IEqualityComparer<V> resultComparer)
            {
                _getValue = getValue;
                _resultComparer = resultComparer;
            }

            public bool Equals(T x, T y)
            {
                V a = _getValue(x);
                V b = _getValue(y);
                return (a is null) ? b is null : (b is not null && _resultComparer.Equals(a, b));
            }

            public int GetHashCode(T obj)
            {
                V v = _getValue(obj);
                return (v is null) ? 0 : _resultComparer.GetHashCode(v);
            }
        }
        
        [Obsolete("Use PropertyGenerationInfo, instead")]
        public class PropertyInheritanceInfo
        {
            public XElement BaseDefinitionElement { get; }
            public bool IsNew { get; }
            public bool DoNotEmit { get; }
            public XElement[] InheritedProperties { get; }
            public XElement Source { get; }
            public PropertyInheritanceInfo(XElement baseDefinitionElement, bool isNew, bool doNotEmit, XElement[] inheritedProperties, XElement source)
            {
                BaseDefinitionElement = baseDefinitionElement;
                IsNew = isNew;
                DoNotEmit = doNotEmit;
                InheritedProperties = inheritedProperties;
                Source = source;
            }
        }
        [Obsolete("Use EntityGenerationInfo, instead")]
        public class AllBasePropertiesCacheItem : IElementCacheItem<PropertyInheritanceInfo> { public PropertyInheritanceInfo Value { get; } public AllBasePropertiesCacheItem(PropertyInheritanceInfo value) { Value = value; } }

        #endregion

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

            foreach (XElement enumElement in entityDefinitionsElement.Elements().Elements(XNAME_EnumTypes).Elements())
            {
                string typeName = enumElement.Attribute(XNAME_Name)?.Value;
                XName elementName = enumElement.Name;
                if (string.IsNullOrWhiteSpace(typeName))
                    WriteLine($"#warning {XNAME_Name} attribute missing or empty for /{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{XNAME_EnumTypes}/{elementName}[{(enumElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)}]");
                else
                {
                    string enumXPath = $"/{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{XNAME_EnumTypes}/{elementName}[@Name='{typeName}']";
                    foreach (XElement fieldElement in enumElement.Elements(XNAME_Field))
                    {
                        string fieldName = fieldElement.Attribute(XNAME_Name)?.Value;
                        if (string.IsNullOrWhiteSpace(fieldName))
                            WriteLine($"#warning {XNAME_Name} attribute missing or empty for {enumXPath}/{XNAME_Field}[{(fieldElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == XNAME_Field) + 1)}]");
                        else
                        {
                            string fieldXPath = $"{enumXPath}/{XNAME_Field}[@Name='{fieldName}']";
                            XAttribute attribute = fieldElement.Attribute(XNAME_FullName);
                            string expected = $"{typeName}.{fieldName}";
                            if (attribute is null)
                            {
                                WriteLine($"#warning {XNAME_FullName} attribute missing for {fieldXPath}");
                                fieldElement.SetAttributeValue(XNAME_FullName, expected);
                            }
                            else if (attribute.Value != expected)
                            {
                                WriteLine($"#warning {XNAME_FullName} attribute does not match the value \"{expected}\" for {fieldXPath}");
                                fieldElement.SetAttributeValue(XNAME_FullName, expected);
                            }
                        }
                    }
                }
            }

            foreach (XElement entityElement in entityDefinitionsElement.Elements().Elements(XNAME_Entity))
            {
                string typeName = entityElement.Attribute(XNAME_Name)?.Value;
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    XName elementName = entityElement.Name;
                    WriteLine($"#warning {XNAME_Name} attribute missing or empty for /{entityElement.Parent.Parent.Name}/{entityElement.Parent.Name}/{XNAME_Entity}[(entityElement.NodesBeforeSelf().OfType<XElement>().Count()]");
                }
                else
                {
                    string typeXPath = $"/{entityElement.Parent.Parent.Name}/{entityElement.Parent.Name}/{XNAME_Entity}[@Name='{typeName}']";
                    foreach (XElement propertyElement in entityElement.Elements(XNAME_Properties).Elements())
                    {
                        XName elementName = propertyElement.Name;
                        string propertyName = propertyElement.Attribute(XNAME_Name)?.Value;
                        if (string.IsNullOrWhiteSpace(propertyName))
                            WriteLine($"#warning {XNAME_Name} attribute missing or empty for {typeXPath}/{elementName}[{(propertyElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)}]");
                        else
                        {
                            string propertyXPath = $"{typeXPath}/{elementName}[@Name='{propertyName}']";
                            XAttribute attribute = propertyElement.Attribute(XNAME_FullName);
                            string expected = $"{typeName}.{propertyName}";
                            if (attribute is null)
                            {
                                WriteLine($"#warning {XNAME_FullName} attribute missing for {propertyXPath}");
                                propertyElement.SetAttributeValue(XNAME_FullName, expected);
                            }
                            else if (attribute.Value != expected)
                            {
                                WriteLine($"#warning {XNAME_FullName} attribute does not match the value \"{expected}\" for {propertyXPath}");
                                propertyElement.SetAttributeValue(XNAME_FullName, expected);
                            }
                        }
                    }
                }
            }

            PushIndent("    ");
            bool isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(XNAME_Root).Elements(XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(XNAME_Root).Elements(XNAME_Entity), isSubsequentMember);
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
            isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(XNAME_Local).Elements(XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(XNAME_Local).Elements(XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
}}

namespace <#=DefaultNamespace#>.Upstream
{{
");
            #endregion

            PushIndent("    ");
            isSubsequentMember = GenerateEnumTypes(entityDefinitionsElement.Elements(XNAME_Upstream).Elements(XNAME_EnumTypes).Elements());
            GenerateEntityTypes(entityDefinitionsElement.Elements(XNAME_Upstream).Elements(XNAME_Entity), isSubsequentMember);
            PopIndent();

            #region TemplatedText

            WriteLine(@$"
}}
");
            #endregion
        }

        static IEnumerable<string> GetBaseTypeNames(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Enumerable.Empty<string>();

            IEnumerable<string> extends = GetElementsByNames(entityElement, XNAME_ExtendsEntity, XNAME_ExtendsGenericEntity).Attributes(XNAME_Type).Select(a => a.Value);
            IEnumerable<string> implements = GetElementsByNames(entityElement, XNAME_Implements, XNAME_ImplementsEntity, XNAME_ImplementsGenericEntity).Attributes(XNAME_Type).Select(a => a.Value);
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    extends = extends.Concat(implements).Distinct();
                    break;
                case NAME_Local:
                case NAME_Upstream:
                    extends = extends.Concat(entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value)).Concat(implements).Distinct();
                    break;
                default:
                    return Enumerable.Empty<string>();
            }
            return extends.Select(n => n.Replace("{", "<").Replace("}", ">"));
        }
        void WriteAmbientValueAttribute(XElement memberElement)
        {
            string ambientValue = memberElement.Elements(XNAME_AmbientString).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (ambientValue is not null)
            {
                Write("[AmbientValue(\"");
                Write(ambientValue.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n"));
                WriteLine("\")]");
                return;
            }
            foreach (XName name in new[] { XNAME_AmbientEnum, XNAME_AmbientBoolean, XNAME_AmbientInt })
            {
                ambientValue = memberElement.Elements(name).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(ambientValue))
                {
                    Write("[AmbientValue(");
                    Write(ambientValue);
                    WriteLine(")]");
                    return;
                }
            }
            ambientValue = memberElement.Elements(XNAME_AmbientUInt).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("u)]");
                return;
            }
            ambientValue = memberElement.Elements(XNAME_AmbientLong).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("L)]");
                return;
            }
            ambientValue = memberElement.Elements(XNAME_AmbientULong).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("UL)]");
                return;
            }
            ambientValue = memberElement.Elements(XNAME_AmbientDouble).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine(ambientValue.Contains(".") ? ")]" : ".0)]");
                return;
            }
            ambientValue = memberElement.Elements(XNAME_AmbientFloat).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("f)]");
                return;
            }
            foreach (XName name in new[] { XNAME_AmbientByte, XNAME_AmbientSByte, XNAME_AmbientShort, XNAME_AmbientUShort })
            {
                ambientValue = memberElement.Elements(name).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
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
            string displayNameResource = getAttributes(XNAME_DisplayNameResource).Select(a => a.Value).DefaultIfEmpty(string.IsNullOrWhiteSpace(typeName) ?
                $"DisplayName_{memberName}" : $"DisplayName_{typeName}_{memberName}").First();
            string descriptionResource = getAttributes(XNAME_DescriptionResource).Select(a => a.Value).FirstOrDefault();
            string resourceType = getAttributes(XNAME_ResourceType).Select(a => a.Value).DefaultIfEmpty("Properties.Resources").First();
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
                foreach (string lt in NewLineRegex.Split(xmlDocElement.ToString(SaveOptions.None)))
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
                if (!TrailingEmptyLine.IsMatch(lastNode.Value))
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
                    if (LeadingEmptyLine.Match(firstNode.Value).Length < firstNode.Value.Length)
                        firstNode.Value = $"\n{firstNode.Value}";
                }
                else if (!LeadingEmptyLine.IsMatch(firstNode.Value))
                    firstNode.Value = $"\n{firstNode.Value}";
            }
            else
                xmlDocElement.FirstNode.AddBeforeSelf(new XText("\n"));
            IEnumerable<string> lines = NewLineRegex.Split(xmlDocElement.ToString(SaveOptions.None));
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

            if (LeadingWsRegex.IsMatch(lines.Skip(1).First()))
            {
                int indent = lines.Skip(1).Reverse().Skip(1).Reverse().Select(s => LeadingWsRegex.Match(s)).Select(m => m.Success ? m.Length : 0).Min();
                if (indent > 0)
                    lines = lines.Take(1).Concat(lines.Skip(1).Reverse().Skip(1).Reverse().Select(s => s.Substring(indent))).Concat(lines.Reverse().Take(1));
            }
            else
            {
                int indent = lines.Skip(2).Reverse().Skip(1).Reverse().Select(s => LeadingWsRegex.Match(s)).Select(m => m.Success ? m.Length : 0).Min();
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
            GenerateXmlDoc(enumElement.Element(XNAME_summary));
            GenerateXmlDoc(enumElement.Element(XNAME_remarks));
            foreach (XElement e in enumElement.Elements(XNAME_seealso))
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            if (enumElement.Attributes(XNAME_IsFlags).Any(a => FromXmlBoolean(a.Value) ?? false))
                WriteLine("[Flags]");
            Write("public enum ");
            string typeName = enumElement.Attribute(XNAME_Name)?.Value;
            Write(typeName);
            Write(" : ");
            WriteLine(enumElement.Name.LocalName.ToLower());
            WriteLine("{");
            PushIndent("    ");
            bool isSubsequentMember = false;
            foreach (XElement fieldElement in enumElement.Elements(XNAME_Field))
            {
                if (isSubsequentMember)
                {
                    WriteLine(",");
                    WriteLine("");
                    isSubsequentMember = true;
                }
                else
                    isSubsequentMember = true;
                GenerateXmlDoc(fieldElement.Element(XNAME_summary));
                GenerateXmlDoc(fieldElement.Element(XNAME_remarks));
                foreach (XElement e in fieldElement.Elements(XNAME_seealso))
                {
                    if (!e.IsEmpty && e.Value.Trim().Length == 0)
                        e.RemoveAll();
                    GenerateXmlDoc(e);
                }
                WriteAmbientValueAttribute(fieldElement);
                string fieldName = fieldElement.Attribute(XNAME_Name)?.Value;
                WriteDisplayAttribute(fieldName, n => fieldElement.Attributes(n), typeName);
                Write(fieldName);
                Write(" = ");
                Write(fieldElement.Attribute(XNAME_Value)?.Value);
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
        
        [Obsolete("Need to create a method that uses PropertyGenerationInfo")]
        void GenerateProperty(string typeName, PropertyInheritanceInfo property)
        {
            string propertyName = property.BaseDefinitionElement.Attribute(XNAME_Name)?.Value;
            XElement commentDocElement = property.Source.Element(XNAME_summary) ?? property.InheritedProperties.Elements(XNAME_summary).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No summary element found for {typeName}.{propertyName}");
            else
                GenerateXmlDoc(commentDocElement);
            commentDocElement = property.Source.Element(XNAME_value) ?? property.InheritedProperties.Elements(XNAME_value).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No value element found for {typeName}.{propertyName}");
            else
                GenerateXmlDoc(commentDocElement);
            commentDocElement = property.Source.Element(XNAME_remarks) ?? property.InheritedProperties.Elements(XNAME_remarks).FirstOrDefault();
            if (commentDocElement is not null)
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in property.Source.Elements(XNAME_seealso).Concat(property.InheritedProperties.Elements(XNAME_seealso)).Distinct())
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            WriteDisplayAttribute(propertyName, n => property.Source.Attributes(n).Concat(property.InheritedProperties.Attributes(n)));
            bool allowsNull = property.BaseDefinitionElement.Attributes(XNAME_AllowNull).Any(a => FromXmlBoolean(a.Value) ?? false) || property.BaseDefinitionElement.Elements(XNAME_DefaultNull).Any();
            if (property.IsNew)
                Write("new ");
            switch (property.Source.Name.LocalName)
            {
                case NAME_Byte:
                case NAME_SByte:
                case NAME_Short:
                case NAME_UShort:
                case NAME_Int:
                case NAME_UInt:
                case NAME_Long:
                case NAME_ULong:
                case NAME_Float:
                case NAME_Double:
                    Write(property.Source.Name.LocalName.ToLower());
                    Write(allowsNull ? "? " : " ");
                    break;
                case NAME_Text:
                case NAME_NVarChar:
                    Write("string ");
                    break;
                case NAME_VolumeIdentifier:
                case NAME_DriveType:
                case NAME_MD5Hash:
                case NAME_DateTime:
                    Write(property.Source.Name.LocalName);
                    Write(allowsNull ? "? " : " ");
                    break;
                case NAME_ByteValues:
                case NAME_MultiStringValue:
                    Write(property.Source.Name.LocalName);
                    Write(" ");
                    break;
                case NAME_UniqueIdentifier:
                case NAME_NewIdNavRef:
                    Write(allowsNull ? "Guid? " : "Guid ");
                    break;
                case NAME_Bit:
                    Write(allowsNull ? "bool? " : "bool ");
                    break;
                case NAME_Enum:
                    Write(property.Source.Attribute(XNAME_Type)?.Value);
                    Write(allowsNull ? "? " : " ");
                    break;
                case NAME_CollectionNavigation:
                case NAME_NewCollectionNavigation:
                    Write("IEnumerable<");
                    Write(property.Source.Attribute(XNAME_ItemType)?.Value ?? GetItemEntity(property.Source)?.Attribute(XNAME_Name)?.Value);
                    Write("> ");
                    break;
                case NAME_RelatedEntity:
                case NAME_NewRelatedEntity:
                    Write(property.Source.Attribute(XNAME_TypeDef)?.Value ?? GetRelatedEntity(property.Source)?.Attribute(XNAME_Name)?.Value);
                    Write(" ");
                    break;
                default:
                    WriteLine($"#warning Unknown element: {property.Source.Name.LocalName}");
                    Write("object ");
                    break;
            }

            Write(propertyName);
            WriteLine(property.BaseDefinitionElement.Attributes(XNAME_IsGenericWritable).Any(a => FromXmlBoolean(a.Value) ?? false) ? " { get; set; }" : " { get; }");
        }
        void GenerateEntityInterface(XElement entityElement)
        {
            XElement[] baseEntities = GetAllBaseEntities(entityElement);
            string typeName = entityElement.Attribute(XNAME_Name)?.Value.Replace("{", "<").Replace("}", ">");

            string[] baseTypeNames = GetBaseTypeNames(entityElement).ToArray();
            XElement commentDocElement = entityElement.Element(XNAME_summary) ?? baseEntities.Elements(XNAME_summary).FirstOrDefault();
            if (commentDocElement is null)
                WriteLine($"#warning No summary element found for {typeName}");
            else
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in entityElement.Elements(XNAME_typeparam).Concat(baseEntities.Elements(XNAME_typeparam)).Distinct())
                GenerateXmlDoc(e);
            commentDocElement = entityElement.Element(XNAME_remarks) ?? baseEntities.Elements(XNAME_remarks).FirstOrDefault();
            if (commentDocElement is not null)
                GenerateXmlDoc(commentDocElement);
            foreach (XElement e in entityElement.Elements(XNAME_seealso).Concat(baseTypeNames.Select(s => new XElement(XNAME_seealso, new XAttribute(XNAME_cref, s)))))
            {
                if (!e.IsEmpty && e.Value.Trim().Length == 0)
                    e.RemoveAll();
                GenerateXmlDoc(e);
            }
            PropertyInheritanceInfo[] properties = entityElement.Elements(XNAME_Properties).Elements().Select(e => GetAllBaseProperties(e)).Where(e => !e.DoNotEmit).ToArray();
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
                if (properties.Length == 0)
                {
                    Write(baseTypeNames.Last());
                    WriteLine("{ }");
                    return;
                }
                WriteLine(baseTypeNames.Last());
            }
            else
            {
                if (properties.Length == 0)
                {
                    Write(typeName);
                    WriteLine("{ }");
                    return;
                }
                WriteLine(typeName);
            }

            WriteLine("{");
            PushIndent("    ");

            GenerateProperty(typeName, properties[0]);
            foreach (PropertyInheritanceInfo p in properties.Skip(1))
            {
                WriteLine("");
                GenerateProperty(typeName, p);
            }

            PopIndent();
            WriteLine("}");
        }

        #endregion

        #region CreateLocalDb.tt

        void GenerateCreateTableSql(string tableName, XElement entityElement)
        {
            string entityName = entityElement.Attribute(XNAME_Name)?.Value;
            Write("CREATE TABLE IF NOT EXISTS \"");
            Write(tableName);
            WriteLine("\" (");
            PushIndent("    ");
            (string Name, List<XElement> Sources)[] collection = GetAllProperties(entityElement)
                .Where(t => !t.Sources.Any(e => e.Name == XNAME_CollectionNavigation || (e.Name == XNAME_RelatedEntity && e.Attribute(XNAME_FkPropertyName) is null))).ToArray();
            (string Name, List<XElement> Sources) firstCol = new(GenerateTableColumnSql(tableName, entityName, collection[0].Name, collection[0].Sources, out string comment),
                collection[0].Sources);
            (string Name, List<XElement> Sources)[] columns = new (string Name, List<XElement> Sources)[] { firstCol }
                .Concat(collection.Skip(1).Select(t =>
                {
                    if (string.IsNullOrEmpty(comment))
                        WriteLine(",");
                    else
                    {
                        Write(", -- ");
                        WriteLine(comment);
                    }
                    (string Name, List<XElement> Sources) r = new(GenerateTableColumnSql(tableName, entityName, t.Name, t.Sources, out string comment2), t.Sources);
                    comment = comment2;
                    return r;
                }).Where(t => t.Name is not null)).ToArray();
            string[] keyColumns = columns.Where(c => c.Sources.Any(e => (FromXmlBoolean(e.Attribute(XNAME_IsPrimaryKey)?.Value) ?? false) && e.Name == XNAME_UniqueIdentifier))
                .Select(c => $"\"{c.Name}\"").ToArray();
            CheckConstraint constraints = null;
            IEnumerable<XElement> currentAndBaseEntities = new XElement[] { entityElement }.Concat(GetAllBaseEntities(entityElement));
            foreach (XElement element in currentAndBaseEntities.Elements())
            {
                if (element.Name == XNAME_Check)
                {
                    CheckConstraint cc = CheckConstraint.Import(element);
                    if (cc is not null)
                        constraints = (constraints is null) ? cc : constraints.And(cc);
                }
            }
            (string Name, string[] Properties)[] uniqueConstraints = currentAndBaseEntities.Elements(XNAME_Unique).Attributes(XNAME_Name).Select(a => (Name: a.Value, Properties: a.Parent.Elements(XNAME_Property).Attributes(XNAME_Name).Select(a => a.Value).ToArray()))
                .Where(t => t.Properties.Length > 0).ToArray();
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
                Write(tableName);
                Write("\" PRIMARY KEY(");
                Write(keyColumns[0]);
                if (constraints is null && uniqueConstraints.Length == 0)
                    WriteLine(")");
                else
                    Write(")");
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
                Write(tableName);
                Write("\" PRIMARY KEY(\"");
                Write(keyColumns[0]);
                foreach (string c in keyColumns.Skip(1))
                {
                    Write(", ");
                    Write(c);
                }
                if (constraints is null && uniqueConstraints.Length == 0)
                    WriteLine("\")");
                else
                    Write("\")");
            }
            if (uniqueConstraints.Length > 0)
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
                Write(uniqueConstraints[0].Name);
                Write("\" UNIQUE(\"");
                Write(uniqueConstraints[0].Properties[0]);
                foreach (string n in uniqueConstraints[0].Properties.Skip(1))
                {
                    Write("\", \"");
                    Write(n);
                }
                Write("\")");
                foreach ((string Name, string[] Properties) u in uniqueConstraints.Skip(1))
                {
                    WriteLine(",");
                    Write("CONSTRAINT \"");
                    Write(u.Name);
                    Write("\" UNIQUE(\"");
                    Write(u.Properties[0]);
                    foreach (string n in u.Properties.Skip(1))
                    {
                        Write("\", \"");
                        Write(n);
                    }
                    Write("\")");
                }
            }
            if (constraints is not null)
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
                Write(constraints.ToSqlString());
                WriteLine(")");
            }
            else if (keyColumns.Length == 0 && uniqueConstraints.Length == 0)
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
            foreach ((string Name, List<XElement> Sources) in columns)
            {
                string indexName = Sources.Elements(XNAME_Index).Attributes(XNAME_Name).Select(a => a.Value).LastOrDefault();
                if (!string.IsNullOrEmpty(indexName))
                {
                    WriteLine("");
                    Write($"CREATE INDEX \"{indexName}\" ON \"{tableName}\" (\"{Name}\"");
                    switch (Sources.Last().Name.LocalName)
                    {
                        case NAME_RelatedEntity:
                        case NAME_UniqueIdentifier:
                        case NAME_VolumeIdentifier:
                            Write(" COLLATE NOCASE");
                            break;
                        case NAME_NVarChar:
                            XAttribute attribute = Sources.Attributes(XNAME_IsCaseSensitive).FirstOrDefault();
                            if (attribute is not null)
                            {
                                Write(" COLLATE ");
                                Write((FromXmlBoolean(attribute.Value) ?? false) ? SQL_TYPENAME_BINARY : "NOCASE");
                            }
                            break;
                    }
                    WriteLine(");");
                }
            }
        }
        string PropertyElementToSqlType(XElement propertyElement, out string typeName, out bool isNumeric)
        {
            if (propertyElement is null || propertyElement.Name.NamespaceName.Length > 0)
            {
                isNumeric = false;
                typeName = SQL_TYPENAME_BLOB;
                return typeName;
            }

            switch (propertyElement.Name.LocalName)
            {
                case NAME_Enum:
                    string sqlType = PropertyElementToSqlType(FindLocalEnumTypeByName(propertyElement.Attribute(XNAME_Type)?.Value), out typeName, out _);
                    isNumeric = false;
                    return sqlType;
                case NAME_UniqueIdentifier:
                case NAME_RelatedEntity:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                    return typeName;
                case NAME_NVarChar:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_NVARCHAR;
                    return $"{typeName}({propertyElement.Attribute(XNAME_MaxLength)?.Value})";
                case NAME_VolumeIdentifier:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_NVARCHAR;
                    return $"{typeName}(1024)";
                case NAME_MultiStringValue:
                case NAME_Text:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_TEXT;
                    return typeName;
                case NAME_DateTime:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_DATETIME;
                    return typeName;
                case NAME_TimeSpan:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_TIME;
                    return typeName;
                case NAME_Bit:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_BIT;
                    return typeName;
                case NAME_ByteValues:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_VARBINARY;
                    return $"{typeName}({propertyElement.Attribute(XNAME_MaxLength)?.Value})";
                case NAME_MD5Hash:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_BINARY;
                    return $"{typeName}(16)";
                case NAME_DriveType:
                    isNumeric = false;
                    typeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                    return typeName;
                case NAME_Byte:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                    return typeName;
                case NAME_SByte:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_TINYINT;
                    return typeName;
                case NAME_Short:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_SMALLINT;
                    return typeName;
                case NAME_UShort:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_UNSIGNED_SMALLINT;
                    return typeName;
                case NAME_Int:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_INT;
                    return typeName;
                case NAME_UInt:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_UNSIGNED_INT;
                    return typeName;
                case NAME_Long:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_BIGINT;
                    return typeName;
                case NAME_ULong:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_UNSIGNED_BIGINT;
                    return typeName;
                case NAME_Float:
                case NAME_Double:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_REAL;
                    return typeName;
                case NAME_Decimal:
                    isNumeric = true;
                    typeName = SQL_TYPENAME_NUMERIC;
                    return typeName;
                default:
                    isNumeric = false;
                    typeName = null;
                    return typeName;
            }
        }
        string GenerateTableColumnSql(string tableName, string entityName, string propertyName, IList<XElement> sources, out string comment)
        {
            XElement baseProperty = sources.First();
            if (baseProperty.Name == XNAME_CollectionNavigation)
            {
                comment = null;
                return null;
            }
            XElement implProperty = sources.Last();
            string propertyTypeName = baseProperty.Name.LocalName;
            string colName = (baseProperty.Name == XNAME_RelatedEntity) ? (baseProperty.Attribute(XNAME_FkPropertyName)?.Value ?? $"{propertyName}Id") :
                implProperty.Attribute(XNAME_ColName)?.Value ?? propertyName;
            bool defaultNull = sources.Elements(XNAME_DefaultNull).Any();
            bool allowNull = defaultNull || sources.Attributes(XNAME_AllowNull).Any(a => FromXmlBoolean(a.Value) ?? false);
            Write("\"");
            Write(colName);
            Write("\" ");
            Write(PropertyElementToSqlType(baseProperty, out string typeName, out bool isNumeric));
            if (!allowNull)
                Write(" NOT NULL");

            CheckConstraint check = null;
            if (isNumeric)
            {
                string minValue = sources.Attributes(XNAME_MinValue).Select(a => a.Value.Trim()).DefaultIfEmpty("").First();
                string maxValue = sources.Attributes(XNAME_MaxValue).Select(a => a.Value.Trim()).DefaultIfEmpty("").First();
                if (minValue.Length > 0)
                {
                    check = propertyTypeName switch
                    {
                        NAME_Byte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Byte(minValue)),
                        NAME_SByte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.SByte(minValue)),
                        NAME_Short => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Short(minValue)),
                        NAME_UShort => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UShort(minValue)),
                        NAME_UInt => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UInt(minValue)),
                        NAME_Long => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Long(minValue)),
                        NAME_ULong => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.ULong(minValue)),
                        NAME_Decimal => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Decimal(minValue)),
                        NAME_Double => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Double(minValue)),
                        NAME_Float => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Float(minValue)),
                        _ => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Int(minValue)),
                    };
                }
                if (maxValue.Length > 0)
                {
                    ComparisonConstraint cc = propertyTypeName switch
                    {
                        NAME_Byte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Byte(maxValue)),
                        NAME_SByte => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.SByte(maxValue)),
                        NAME_Short => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Short(maxValue)),
                        NAME_UShort => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UShort(maxValue)),
                        NAME_UInt => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.UInt(maxValue)),
                        NAME_Long => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Long(maxValue)),
                        NAME_ULong => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.ULong(maxValue)),
                        NAME_Decimal => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Decimal(maxValue)),
                        NAME_Double => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Double(maxValue)),
                        NAME_Float => ComparisonConstraint.NotLessThan(new SimpleColumnValueReference(colName), ConstantValueReference.Float(maxValue)),
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
                    case NAME_Enum:
                        XElement enumType = FindLocalEnumTypeByName(baseProperty.Attribute(XNAME_Type)?.Value);
                        IEnumerable<string> enumValueStrings = enumType.Elements(XNAME_Field).Attributes(XNAME_Value).Select(a => a.Value);
                        bool isFlags = FromXmlBoolean(baseProperty.Attribute(XNAME_IsFlags)?.Value) ?? false;
                        check = typeName switch
                        {
                            SQL_TYPENAME_TINYINT => GetEnumCheckConstraintMinMax(colName, sbyte.MinValue, sbyte.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToSByte(v)), isFlags ? (x, y) => (sbyte)(x | y) : null),
                            SQL_TYPENAME_UNSIGNED_TINYINT => GetEnumCheckConstraintMinMax(colName, byte.MinValue, byte.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToByte(v)), isFlags ? (x, y) => (byte)(x | y) : null),
                            SQL_TYPENAME_SMALLINT => GetEnumCheckConstraintMinMax(colName, short.MinValue, short.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToInt16(v)), isFlags ? (x, y) => (short)(x | y) : null),
                            SQL_TYPENAME_UNSIGNED_SMALLINT => GetEnumCheckConstraintMinMax(colName, ushort.MinValue, ushort.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToUInt16(v)), isFlags ? (x, y) => (ushort)(x | y) : null),
                            SQL_TYPENAME_UNSIGNED_INT => GetEnumCheckConstraintMinMax(colName, uint.MinValue, uint.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToUInt32(v)), isFlags ? (x, y) => x | y : null),
                            SQL_TYPENAME_BIGINT => GetEnumCheckConstraintMinMax(colName, long.MinValue, long.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToInt64(v)), isFlags ? (x, y) => x | y : null),
                            SQL_TYPENAME_UNSIGNED_BIGINT => GetEnumCheckConstraintMinMax(colName, ulong.MinValue, ulong.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToUInt64(v)), isFlags ? (x, y) => x | y : null),
                            _ => GetEnumCheckConstraintMinMax(colName, int.MinValue, int.MaxValue, ConstantValueReference.Of, enumValueStrings.Select(v => XmlConvert.ToInt32(v)), isFlags ? (x, y) => x | y : null),
                        };
                        break;
                    case NAME_NVarChar:
                        if (sources.Attributes(XNAME_IsNormalized).Any(a => FromXmlBoolean(a.Value) ?? false))
                            check = new SimpleColumnValueReference(colName).Trimmed().Length().IsEqualTo(new SimpleColumnValueReference(colName).Length());
                        minLength = sources.Attributes(XNAME_MinLength).Select(a => FromXmlInt32(a.Value.Trim()) ?? 0).DefaultIfEmpty(0).First();
                        if (minLength > 0)
                            check = (check is null) ? new SimpleColumnValueReference(colName).GreaterThanLiteral(minLength - 1) : check.And(new SimpleColumnValueReference(colName).GreaterThanLiteral(minLength - 1));
                        break;
                    case NAME_ByteValues:
                        minLength = sources.Attributes(XNAME_MinLength).Select(a => FromXmlInt32(a.Value.Trim()) ?? 0).DefaultIfEmpty(0).First();
                        if (minLength > 0)
                            check = new SimpleColumnValueReference(colName).Length().GreaterThanLiteral(minLength - 1);
                        int maxLength = sources.Attributes(XNAME_MaxLength).Select(a => FromXmlInt32(a.Value.Trim()) ?? 0).DefaultIfEmpty(0).First();
                        if (maxLength > 0)
                            check = new SimpleColumnValueReference(colName).Length().LessThanLiteral(maxLength + 1);
                        break;
                    case NAME_UniqueIdentifier:
                        IEnumerable<(string ConstraintName, string TableName)> relatedEntities = sources.Attributes(XNAME_Navigation).Select(a =>
                        {
                            string n = a.Value;
                            XElement[] refersTo = sources.Select(p => FindCurrentEntityPropertyByName(p, n)).Where(e => e is not null).ToArray();
                            return (
                                ConstraintName: refersTo.Attributes(XNAME_ConstraintName).Select(e => e.Value).FirstOrDefault(),
                                TableName: refersTo.Attributes(XNAME_Reference).Select(e => FindLocalEntityByName(e.Value)?.Attribute(XNAME_TableName)?.Value).Where(n => n is not null).FirstOrDefault()
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
                    case NAME_RelatedEntity:
                        string constraintName = sources.Elements(XNAME_DbRelationship).Attributes(XNAME_Name).Select(e => e.Value).FirstOrDefault();
                        if (constraintName is not null)
                        {
                            Write(" CONSTRAINT \"");
                            Write(constraintName);
                            Write("\" REFERENCES \"");
                            Write(sources.Attributes(XNAME_Reference).Select(e => FindLocalEntityByName(e.Value)?.Attribute(XNAME_TableName)?.Value)
                                .Where(n => n is not null).First());
                            Write("\"(\"Id\") ON DELETE RESTRICT");
                        }
                        break;
                    case NAME_DriveType:
                        check = new SimpleColumnValueReference(colName).NotLessThanLiteral(0).And(new SimpleColumnValueReference(colName).LessThanLiteral(7));
                        break;
                    default:
                        check = null;
                        break;
                }
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
                string defaultValue = sources.Elements(XNAME_Default).Select(e => e.Value).FirstOrDefault();
                if (defaultValue is null)
                {
                    switch (propertyTypeName)
                    {
                        case NAME_DateTime:
                            if (sources.Elements(XNAME_DefaultNow).Any())
                                Write(" DEFAULT (datetime('now','localtime'))");
                            break;
                        case NAME_TimeSpan:
                            if (sources.Elements(XNAME_DefaultZero).Any())
                                Write(" DEFAULT (time('00:00:00.000'))");
                            break;
                        case NAME_ByteValues:
                            if (sources.Elements(XNAME_DefaultEmpty).Any())
                                Write(" DEFAULT X''");
                            break;
                    }
                    comment = null;
                }
                else
                {
                    switch (propertyTypeName)
                    {
                        case NAME_Enum:
                            Write(" DEFAULT ");
                            Write(FindLocalFieldByFullName(defaultValue.Trim())?.Attribute(XNAME_Value)?.Value);
                            comment = defaultValue.Trim();
                            break;
                        case NAME_Char:
                        case NAME_NVarChar:
                        case NAME_VolumeIdentifier:
                        case NAME_MultiStringValue:
                        case NAME_Text:
                            comment = null;
                            Write(" DEFAULT '");
                            Write(defaultValue.Replace("'", "''"));
                            Write("'");
                            break;
                        case NAME_TimeSpan:
                            comment = null;
                            Write(" DEFAULT ");
                            Write(FromXmlTimeSpan(defaultValue.Trim()).Value.ToString(@"\'hh\:mm\:ss\.fff\'"));
                            break;
                        case NAME_DateTime:
                            comment = null;
                            Write(FromXmlDateTime(defaultValue.Trim()).Value.ToLocalTime().ToString(@"'yyyy-MM-dd HH:mm:ss"));
                            break;
                        case NAME_Bit:
                            comment = null;
                            Write((FromXmlBoolean(defaultValue) ?? false) ? " DEFAULT 1," : " DEFAULT 0");
                            break;
                        case NAME_ByteValues:
                            comment = null;
                            Write(" DEFAULT X'");
                            Write(BitConverter.ToString(FromXmlBinary(defaultValue.Trim())));
                            Write("'");
                            break;
                        case NAME_DriveType:
                            switch (defaultValue.Trim())
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
                                    comment = defaultValue.Trim();
                                    break;
                            }
                            break;
                        default:
                            comment = null;
                            Write(" DEFAULT ");
                            Write(defaultValue);
                            break;
                    }
                }
            }

            switch (propertyTypeName)
            {
                case NAME_RelatedEntity:
                case NAME_UniqueIdentifier:
                case NAME_VolumeIdentifier:
                    Write(" COLLATE NOCASE");
                    break;
                case NAME_NVarChar:
                    XAttribute attribute = sources.Attributes(XNAME_IsCaseSensitive).FirstOrDefault();
                    if (attribute is not null)
                    {
                        Write(" COLLATE ");
                        Write((FromXmlBoolean(attribute.Value) ?? false) ? SQL_TYPENAME_BINARY : "NOCASE");
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
        public abstract class CheckConstraint : IEquatable<CheckConstraint>
        {
            public abstract bool IsCompound { get; }
            public static CheckConstraint Import(XElement checkElement)
            {
                if (checkElement is null)
                    return null;
                if (checkElement.Name == XNAME_Check || checkElement.Name == XNAME_And)
                {
                    CheckConstraint[] constraints = checkElement.Elements().Select(e => Import(e)).Where(c => c is not null).ToArray();
                    if (constraints.Length == 1)
                        return constraints[0];
                    if (constraints.Length > 1)
                        return new ComparisonGroup(false, constraints);
                    return null;
                }
                if (checkElement.Name == XNAME_Or)
                {
                    CheckConstraint[] constraints = checkElement.Elements().Select(e => Import(e)).Where(c => c is not null).ToArray();
                    if (constraints.Length == 1)
                        return constraints[0];
                    if (constraints.Length > 1)
                        return new ComparisonGroup(true, constraints);
                    return null;
                }
                SimpleColumnValueReference name = new(checkElement.Attribute(XNAME_Name)?.Value);
                if (checkElement.Name == XNAME_IsNull)
                    return new NullCheckConstraint(name, true);
                if (checkElement.Name == XNAME_NotNull)
                    return new NullCheckConstraint(name, false);

                ColumnValueReference lValue = (FromXmlBoolean(checkElement.Attribute(XNAME_Trimmed)?.Value) ?? false) ? ColumnValueMethodResultReference.Trim(name) : name;
                if (FromXmlBoolean(checkElement.Attribute(XNAME_Length)?.Value) ?? false)
                    lValue = ColumnValueMethodResultReference.Length(lValue);
                XElement other = checkElement.Elements().First();
                ValueReference rValue;
                if (other.Name == XNAME_OtherProperty)
                {
                    name = new SimpleColumnValueReference(other.Attribute(XNAME_Name)?.Value);
                    ColumnValueReference r = (FromXmlBoolean(other.Attribute(XNAME_Trimmed)?.Value) ?? false) ? ColumnValueMethodResultReference.Trim(name) : name;
                    rValue = (FromXmlBoolean(other.Attribute(XNAME_Length)?.Value) ?? false) ? ColumnValueMethodResultReference.Length(r) : r;
                }
                else if (other.Name == XNAME_True)
                    rValue = ConstantValueReference.Of(true);
                else if (other.Name == XNAME_False)
                    rValue = ConstantValueReference.Of(false);
                else if (other.Name == XNAME_Now)
                    rValue = ConstantValueReference.Now();
                else
                {
                    string t = other.Attribute(XNAME_Value)?.Value;
                    if (other.Name == XNAME_Byte)
                        rValue = ConstantValueReference.Byte(t);
                    else if (other.Name == XNAME_SByte)
                        rValue = ConstantValueReference.SByte(t);
                    else if (other.Name == XNAME_Short)
                        rValue = ConstantValueReference.Short(t);
                    else if (other.Name == XNAME_UShort)
                        rValue = ConstantValueReference.UShort(t);
                    else if (other.Name == XNAME_Int)
                        rValue = ConstantValueReference.Int(t);
                    else if (other.Name == XNAME_UInt)
                        rValue = ConstantValueReference.UInt(t);
                    else if (other.Name == XNAME_Long)
                        rValue = ConstantValueReference.Long(t);
                    else if (other.Name == XNAME_ULong)
                        rValue = ConstantValueReference.ULong(t);
                    else if (other.Name == XNAME_Double)
                        rValue = ConstantValueReference.Double(t);
                    else if (other.Name == XNAME_Float)
                        rValue = ConstantValueReference.Float(t);
                    else if (other.Name == XNAME_Decimal)
                        rValue = ConstantValueReference.Decimal(t);
                    else if (other.Name == XNAME_DateTime)
                        rValue = ConstantValueReference.DateTime(t);
                    else
                        rValue = ConstantValueReference.String(t);
                }
                if (checkElement.Name == XNAME_LessThan)
                    return ComparisonConstraint.LessThan(lValue, rValue);
                if (checkElement.Name == XNAME_NotGreaterThan)
                    return ComparisonConstraint.NotGreaterThan(lValue, rValue);
                if (checkElement.Name == XNAME_NotEquals)
                    return ComparisonConstraint.NotEqual(lValue, rValue);
                if (checkElement.Name == XNAME_NotLessThan)
                    return ComparisonConstraint.NotLessThan(lValue, rValue);
                if (checkElement.Name == XNAME_GreaterThan)
                    return ComparisonConstraint.GreaterThan(lValue, rValue);
                return ComparisonConstraint.AreEqual(lValue, rValue);
            }
            public abstract bool Equals(CheckConstraint other);
            public abstract string ToSqlString();
            public abstract string ToCsString();
            public virtual CheckConstraint And(CheckConstraint cc)
            {
                if (cc is null || Equals(cc))
                    return this;
                return new ComparisonGroup(false, this, cc);
            }
            public virtual CheckConstraint Or(CheckConstraint cc)
            {
                if (cc is null || Equals(cc))
                    return this;
                return new ComparisonGroup(true, this, cc);
            }
        }
        public abstract class ValueReference : IEquatable<ValueReference>
        {
            public abstract bool Equals(ValueReference other);
            public abstract string ToSqlString();
            public abstract string ToCsString();
        }
        public abstract class ColumnValueReference : ValueReference, IEquatable<ColumnValueReference>
        {
            public abstract string Name { get; }
            public abstract bool Equals(ColumnValueReference other);
            internal ComparisonConstraint LessThanLiteral(int value) => ComparisonConstraint.LessThan(this, ConstantValueReference.Of(value));
            internal ComparisonConstraint NotLessThanLiteral(int minValue) => ComparisonConstraint.NotLessThan(this, ConstantValueReference.Of(minValue));
            internal ComparisonConstraint GreaterThanLiteral(int value) => ComparisonConstraint.GreaterThan(this, ConstantValueReference.Of(value));
            internal ComparisonConstraint IsEqualTo(ValueReference value) => ComparisonConstraint.AreEqual(this, value);
            internal ColumnValueReference Length() => ColumnValueMethodResultReference.Length(this);
            internal ColumnValueReference Trimmed() => ColumnValueMethodResultReference.Trim(this);
        }
        public sealed class ConstantValueReference : ValueReference, IEquatable<ConstantValueReference>
        {
            private ConstantValueReference(string sqlValue, string csValue)
            {
                SqlValue = sqlValue;
                CsValue = csValue;
            }
            public string SqlValue { get; }
            public string CsValue { get; }
            public bool Equals(ConstantValueReference other) => other is not null && (ReferenceEquals(this, other) || CsValue == other.CsValue);
            public override bool Equals(ValueReference other) => other is ConstantValueReference o && Equals(o);
            public override bool Equals(object obj) => obj is ConstantValueReference other && Equals(other);
            public override int GetHashCode() => CsValue.GetHashCode();
            public override string ToString() => SqlValue;
            public override string ToCsString() => CsValue;
            public override string ToSqlString() => SqlValue;
            public static ConstantValueReference Int(string value) => new(value, value);
            public static ConstantValueReference String(string value) => new($"N'{value}'", $"{value.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\"", "\\\"")}");
            internal static ConstantValueReference DateTime(string value) => Of(XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind));
            internal static ConstantValueReference Decimal(string value) => new(value, $"{value}m");
            internal static ConstantValueReference Float(string value) => new(value, $"{value}f");
            internal static ConstantValueReference Double(string value) => new(value, value.Contains('.') ? value : $"{value}.0");
            internal static ConstantValueReference ULong(string value) => new(value, $"{value}UL");
            internal static ConstantValueReference Long(string value) => new(value, $"{value}L");
            internal static ConstantValueReference UInt(string value) => new(value, $"{value}u");
            internal static ConstantValueReference UShort(string value) => new(value, $"(ushort){value}");
            internal static ConstantValueReference Short(string value) => new(value, $"(short){value}");
            internal static ConstantValueReference SByte(string value) => new(value, $"(sbyte){value}");
            internal static ConstantValueReference Byte(string value) => new(value, $"(byte){value}");
            internal static ConstantValueReference Now() => new("(datetime('now','localtime'))", "DateTime.Now");
            public static ConstantValueReference Of(bool value) => value ? new ConstantValueReference("1", "true") : new ConstantValueReference("0", "false");
            internal static ConstantValueReference Of(sbyte value) => new(value.ToString(), $"(sbyte){value}");
            internal static ConstantValueReference Of(byte value) => new(value.ToString(), $"(byte){value}");
            internal static ConstantValueReference Of(short value) => new(value.ToString(), $"(short){value}");
            internal static ConstantValueReference Of(ushort value) => new(value.ToString(), $"(ushort){value}");
            internal static ConstantValueReference Of(int value) => new(value.ToString(), value.ToString());
            internal static ConstantValueReference Of(uint value) => new(value.ToString(), $"{value}u");
            internal static ConstantValueReference Of(long value) => new(value.ToString(), $"{value}L");
            internal static ConstantValueReference Of(ulong value) => new(value.ToString(), $"{value}UL");
            internal static ConstantValueReference Of(DateTime dateTime) => new(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), $"new DateTime({dateTime.Year}, {dateTime.Month}, {dateTime.Day}, {dateTime.Hour}, {dateTime.Minute}, {dateTime.Second})");
        }
        public sealed class SimpleColumnValueReference : ColumnValueReference, IEquatable<SimpleColumnValueReference>
        {
            private readonly string _name;
            public SimpleColumnValueReference(string name) { _name = name ?? ""; }
            public override string Name => _name;
            public bool Equals(SimpleColumnValueReference other) => other is not null && (ReferenceEquals(this, other) || _name == other._name);
            public override bool Equals(ColumnValueReference other) => other is SimpleColumnValueReference o && Equals(o);
            public override bool Equals(ValueReference other) => other is SimpleColumnValueReference o && Equals(o);
            public override bool Equals(object obj) => obj is SimpleColumnValueReference other && Equals(other);
            public override int GetHashCode() => _name.GetHashCode();
            public override string ToString() => ToSqlString();
            public override string ToCsString() => _name;
            public override string ToSqlString() => $"\"{_name}\"";
        }
        public sealed class ColumnValueMethodResultReference : ColumnValueReference, IEquatable<ColumnValueMethodResultReference>
        {
            public const string SqlMethodName_trim = "trim";
            public const string CsNemberName_trim = "Trim()";
            public const string SqlMethodName_length = "length";
            public const string CsNemberName_length = "Length";
            private ColumnValueMethodResultReference(ColumnValueReference column, string methodName, string csMemberName)
            {
                Column = column;
                SqlMethodName = methodName;
            }
            public ColumnValueReference Column { get; }
            public string SqlMethodName { get; }
            public string CsMemberName { get; }
            public override string Name => Column.Name;
            public static ColumnValueMethodResultReference LengthTrimmed(ColumnValueReference column) => (column is ColumnValueMethodResultReference m) ?
                ((m.SqlMethodName == SqlMethodName_length) ? m : Length((m.SqlMethodName == SqlMethodName_trim) ? m : Trim(column))) : Length(Trim(column));
            public static ColumnValueMethodResultReference Trim(ColumnValueReference column)
            {
                if (column is ColumnValueMethodResultReference m)
                {
                    if (m.SqlMethodName == SqlMethodName_trim)
                        return m;
                    if (m.SqlMethodName == SqlMethodName_length)
                    {
                        if (m.Column is ColumnValueMethodResultReference c && c.SqlMethodName == SqlMethodName_trim)
                            return m;
                        return Length(Trim(m.Column));
                    }
                }
                return new ColumnValueMethodResultReference(column, SqlMethodName_trim, CsNemberName_trim);
            }
            public static ColumnValueMethodResultReference Length(ColumnValueReference column) => (column is ColumnValueMethodResultReference m && m.SqlMethodName == SqlMethodName_length) ? m :
                new ColumnValueMethodResultReference(column, SqlMethodName_length, CsNemberName_length);
            public bool Equals(ColumnValueMethodResultReference other) => other is not null && (ReferenceEquals(this, other) || (SqlMethodName == other.SqlMethodName && Column.Equals(other.Column)));
            public override bool Equals(ColumnValueReference other) => other is ColumnValueMethodResultReference o && Equals(o);
            public override bool Equals(ValueReference other) => other is ColumnValueMethodResultReference o && Equals(o);
            public override bool Equals(object obj) => obj is ColumnValueMethodResultReference other && Equals(other);
            public override int GetHashCode() { unchecked { return (SqlMethodName.GetHashCode() * 3) ^ Column.GetHashCode(); } }
            public override string ToString() => ToSqlString();
            public override string ToSqlString() => $"{SqlMethodName}({Column.ToSqlString()})";
            public override string ToCsString() => $"{Column.ToCsString()}.{CsMemberName}";
        }
        public sealed class NullCheckConstraint : CheckConstraint, IEquatable<NullCheckConstraint>
        {
            public NullCheckConstraint(SimpleColumnValueReference column, bool isNull)
            {
                Column = column;
                IsNull = isNull;
            }
            public SimpleColumnValueReference Column { get; }
            public bool IsNull { get; }
            public override bool IsCompound => false;
            public bool Equals(NullCheckConstraint other) => other is not null && (ReferenceEquals(this, other) || (IsNull == other.IsNull && Column.Equals(other.Column)));
            public override bool Equals(CheckConstraint other) => other is NullCheckConstraint o && Equals(o);
            public override bool Equals(object obj) => obj is NullCheckConstraint other && Equals(other);
            public override int GetHashCode() { unchecked { return (IsNull ? 0 : 3) ^ Column.GetHashCode(); } }
            public override string ToString() => ToSqlString();
            public override string ToSqlString() => IsNull ? $"{Column.ToSqlString()} IS NULL" : $"{Column.ToSqlString()} IS NOT NULL";
            public override string ToCsString() => IsNull ? $"{Column.ToCsString()} is null" : $"{Column.ToCsString()} is not null";
        }
        public sealed class ComparisonConstraint : CheckConstraint, IEquatable<ComparisonConstraint>
        {
            private ComparisonConstraint(ColumnValueReference lValue, string sqlOp, string csOp, ValueReference rValue)
            {
                LValue = lValue;
                SqlOp = sqlOp;
                CsOp = csOp;
                RValue = rValue;
            }
            public ColumnValueReference LValue { get; }
            public string SqlOp { get; }
            public string CsOp { get; }
            public ValueReference RValue { get; }
            public override bool IsCompound => false;
            public static ComparisonConstraint AreEqual(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "=", "==", rValue);
            public static ComparisonConstraint NotEqual(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<>", "!=", rValue);
            public static ComparisonConstraint LessThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<", "<", rValue);
            public static ComparisonConstraint NotGreaterThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<=", "<=", rValue);
            public static ComparisonConstraint GreaterThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, ">", ">", rValue);
            public static ComparisonConstraint NotLessThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, ">=", ">=", rValue);
            public bool Equals(ComparisonConstraint other) => other is not null && (ReferenceEquals(this, other) || (LValue.Equals(other.LValue) && SqlOp == other.SqlOp && RValue.Equals(other.RValue)));
            public override bool Equals(CheckConstraint other) => other is ComparisonConstraint o && Equals(o);
            public override bool Equals(object obj) => obj is ComparisonConstraint other && Equals(other);
            public override int GetHashCode() { unchecked { return (((LValue.GetHashCode() * 3) ^ SqlOp.GetHashCode()) * 5) ^ RValue.GetHashCode(); } }
            public override string ToString() => ToSqlString();
            public override string ToSqlString() => $"{LValue.ToSqlString()}{SqlOp}{RValue.ToSqlString()}";
            public override string ToCsString() => $"{LValue.ToCsString()} {CsOp} {RValue.ToCsString()}";
        }
        public sealed class ComparisonGroup : CheckConstraint, IEquatable<ComparisonGroup>
        {
            public ComparisonGroup(bool isOr, params CheckConstraint[] constraints)
            {
                if (constraints is null || (constraints = constraints.Where(c => c is not null)
                        .SelectMany(c => (c is ComparisonGroup g && g.IsOr == isOr) ? g.Constraints : Enumerable.Repeat(c, 1)).ToArray()).Length == 0)
                    throw new ArgumentOutOfRangeException(nameof(constraints));
                using IEnumerator<CheckConstraint> enumerator = (constraints ?? Array.Empty<CheckConstraint>()).Where(c => c is not null).GetEnumerator();
                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException(nameof(constraints));
                IsOr = isOr;
                List<CheckConstraint> checkConstraints = new();
                do
                {
                    if (!checkConstraints.Contains(enumerator.Current))
                        checkConstraints.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                Constraints = new ReadOnlyCollection<CheckConstraint>(checkConstraints);
            }
            public bool IsOr { get; }
            public ReadOnlyCollection<CheckConstraint> Constraints { get; }
            public override bool IsCompound => Constraints.Count > 1;
            public override CheckConstraint And(CheckConstraint cc)
            {
                if (cc is null || Equals(cc) || Constraints.Contains(cc))
                    return this;
                if (IsOr)
                    return base.And(cc);
                return new ComparisonGroup(false, Constraints.Concat((cc is ComparisonGroup cg && !cg.IsOr) ? cg.Constraints : Enumerable.Repeat(cc, 1)).ToArray());
            }
            public override CheckConstraint Or(CheckConstraint cc)
            {
                if (cc is null || Equals(cc) || Constraints.Contains(cc))
                    return this;
                if (!IsOr)
                    return base.Or(cc);
                return new ComparisonGroup(true, Constraints.Concat((cc is ComparisonGroup cg && cg.IsOr) ? cg.Constraints : Enumerable.Repeat(cc, 1)).ToArray());
            }
            public bool Equals(ComparisonGroup other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                if (IsOr != other.IsOr || Constraints.Count != other.Constraints.Count)
                    return false;
                return Constraints.All(c => other.Constraints.Contains(c));
            }
            public override bool Equals(CheckConstraint other) => other is ComparisonGroup g && Equals(g);
            public override bool Equals(object obj) => obj is ComparisonGroup other && Equals(other);
            public override int GetHashCode()
            {
                if (Constraints.Count == 0)
                    return IsOr ? 1 : 0;
                int seed = Constraints.Count + 1;
                int prime = FindPrimeNumber(seed & 0xffff);
                seed = FindPrimeNumber(prime + 1);
                return new int[] { IsOr ? 1 : 0 }.Concat(Constraints.Select(c => c.GetHashCode())).Aggregate(seed, (a, i) =>
                {
                    unchecked { return (a * prime) ^ i; }
                });
            }
            public override string ToString() => ToSqlString();
            public override string ToSqlString()
            {
                if (Constraints.Count == 0)
                    return "";
                if (Constraints.Count == 1)
                    return Constraints[0].ToSqlString();
                return string.Join(IsOr ? " OR " : " AND ", Constraints.Select(c => c.IsCompound ? $"({c.ToSqlString()})" : c.ToSqlString()));
            }
            public override string ToCsString()
            {
                if (Constraints.Count == 0)
                    return "";
                if (Constraints.Count == 1)
                    return Constraints[0].ToCsString();
                return string.Join(IsOr? " || " : " && ", Constraints.Select(c => c.IsCompound? $"({c.ToCsString()})" : c.ToCsString()));
            }
        }

        #endregion

        XDocument EntityDefinitionsDocument
        {
            get
            {
                if (_document is Exception exception)
                    throw new AssertInconclusiveException(string.IsNullOrWhiteSpace(exception.Message) ? $"An unexpected {exception.GetType().Name} has occurred" : exception.Message, exception);
                return (XDocument)_document;
            }
        }

        static readonly EntityDefinitionsReader Instance = new();
        private readonly object _document;
        //private readonly ReadOnlyCollection<(string Selector, ReadOnlyCollection<string> Fields)> _keys;
        private EntityDefinitionsReader()
        {
            Collection<(string Selector, ReadOnlyCollection<string> Field)> keys = new();
            object document;
            try
            {
                if (TestHelper.ProjectDirectory is null)
                    throw new InvalidOperationException($"Could not find parent directory with the same name as the test assembly ({System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}). Ensure that the subdirectory for the current test project is named accordingly.");
                string resourcesDirectory = Path.Combine(Path.GetDirectoryName(TestHelper.ProjectDirectory), typeof(BaseDbContext).Namespace, "Resources");
                string path = Path.Combine(resourcesDirectory, "EntityDefinitions.xsd");
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Could not find entity configuration schema file {path}", path);
                XmlReaderSettings readerSettings = new() { ValidationType = ValidationType.Schema };
                //XmlSchema schema = readerSettings.Schemas.Add("", path);
                readerSettings.Schemas.Add("", path);
                Collection<ValidationEventArgs> validationArgs = new();
                readerSettings.ValidationEventHandler += new ValidationEventHandler((object sender, ValidationEventArgs e) => validationArgs.Add(e));
                path = Path.Combine(resourcesDirectory, "EntityDefinitions.xml");
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Could not find entity configuration schema file {path}", path);
                using XmlReader reader = XmlReader.Create(path, readerSettings);
                XDocument d = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
                if (d.Root is null)
                    throw new InvalidDataException("Not root element found.");
                document = d;
                //Collection<XmlSchemaKey> keyElements = new();
                //GetAllSchemaKeys(schema, keyElements);
                //foreach (XmlSchemaKey key in keyElements)
                //{
                //    string selector = key.Selector?.XPath;
                //    if (string.IsNullOrEmpty(selector))
                //        continue;
                //    string[] fields = key.Fields.OfType<XmlSchemaXPath>().Select(f => f.XPath).Where(f => !(string.IsNullOrEmpty(f) || f.Contains('/'))).ToArray();
                //    if (fields.Length > 0)
                //        keys.Add(new(selector, new ReadOnlyCollection<string>(fields)));
                //}
            }
            catch (Exception exception)
            {
                document = exception;
            }
            //finally
            //{
            //    _keys = new(keys);
            //}
            _document = document;
        }

        record CodeValues(string SQL, string CLR);
        interface IColumnConstraint { }
        record ColumnDefinitionOld(string Name, bool IsPrimaryKey, CodeValues DefaultValue, ReadOnlyCollection<IColumnConstraint> Constraints, PropertyDefinitionOld Property, XElement Source);
        record TableDefinitionOld(string Name, ReadOnlyCollection<ColumnDefinitionOld> Columns, XElement Source);

        record PropertyDefinitionOld(string Name, string ClrType, bool AllowsNull, bool IsGenericWritable, ReadOnlyCollection<PropertyDefinitionOld> Base, XElement Source);

        record EntityDefinitionOld(string Name, TableDefinitionOld Table, ReadOnlyCollection<PropertyDefinitionOld> Properties, ReadOnlyCollection<EntityDefinitionOld> BaseDefinitions, ReadOnlyCollection<string> BaseTypeNames, XElement Source);

        private static readonly ReadOnlyCollection<XName> NameKeyed = new(new[] { XNAME_Entity, XNAME_Field, XNAME_Byte, XNAME_SByte, XNAME_MultiStringValue, XNAME_MD5Hash, XNAME_ByteValues, XNAME_Short, XNAME_UShort, XNAME_Int,
            XNAME_UInt, XNAME_Long, XNAME_ULong, XNAME_Double, XNAME_Float, XNAME_Decimal, XNAME_NVarChar, XNAME_Char, XNAME_DateTime, XNAME_TimeSpan, XNAME_UniqueIdentifier, XNAME_NewIdNavRef, XNAME_Bit, XNAME_Text, XNAME_VolumeIdentifier,
            XNAME_DriveType, XNAME_Enum, XNAME_RelatedEntity, XNAME_NewRelatedEntity, XNAME_CollectionNavigation, XNAME_NewCollectionNavigation, XNAME_Byte, XNAME_SByte, XNAME_Short, XNAME_UShort, XNAME_Int, XNAME_UInt, XNAME_Long,
            XNAME_ULong });

        private static readonly ReadOnlyCollection<XName> TypeKeyed = new(new[] { XNAME_ImplementsGenericEntity, XNAME_ImplementsEntity, XNAME_Implements });

        string ToXPath(XNode node)
        {
            if (node is XDocument)
                return "/";
            if (node is XElement element)
            {
                string leaf;
                if (NameKeyed.Contains(element.Name))
                {
                    string name = element.Attribute(XNAME_Name)?.Value;
                    leaf = string.IsNullOrWhiteSpace(name) ? null : $"{element.Name}[@{NAME_Name}=\"{name.Replace("\"", "&quot;")}\"]";
                }
                else if (TypeKeyed.Contains(element.Name))
                {
                    string type = element.Attribute(XNAME_Name)?.Value;
                    leaf = string.IsNullOrWhiteSpace(type) ? null : $"{element.Name}[@{NAME_Name}=\"{type.Replace("\"", "&quot;")}\"]";
                }
                else
                    leaf = null;
                if (leaf is null)
                {
                    int count = element.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == element.Name);
                    if (count > 0 || element.NodesAfterSelf().OfType<XElement>().Any(e => e.Name == element.Name))
                        leaf = $"{element.Name}[@{count + 1}]";
                    else
                        leaf = element.Name.ToString();
                }
                return (node.Parent is null) ? ((node.Document is null) ? leaf : $"/{leaf}") : $"{ToXPath(node.Parent)}/{leaf}";
            }
            return (node.Parent is null) ? ((node.Document is null) ? "." : "/") : ToXPath(node.Parent);
        }

        XElement GetRelatedEntityPrimaryKeyProperty(XElement relatedEntityPropertyElement)
        {
            XElement parent = relatedEntityPropertyElement?.Parent;
            if (parent is null || (relatedEntityPropertyElement.Name != XNAME_RelatedEntity && relatedEntityPropertyElement.Name != XNAME_NewRelatedEntity))
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootPropertyByFullName(relatedEntityPropertyElement.Attribute(XNAME_PrimaryKey)?.Value),
                NAME_Local => FindLocalPropertyByFullName(relatedEntityPropertyElement.Attribute(XNAME_PrimaryKey)?.Value),
                NAME_Upstream => FindUpstreamPropertyByFullName(relatedEntityPropertyElement.Attribute(XNAME_PrimaryKey)?.Value),
                _ => null,
            };
        }

        XElement GetCollectionNavigationForeignKeyProperty(XElement collectionNavigationPropertyElement)
        {
            XElement parent = collectionNavigationPropertyElement?.Parent;
            if (parent is null || (collectionNavigationPropertyElement.Name != XNAME_CollectionNavigation && collectionNavigationPropertyElement.Name != XNAME_NewCollectionNavigation))
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootPropertyByFullName(collectionNavigationPropertyElement.Attribute(XNAME_ForeignKey)?.Value),
                NAME_Local => FindLocalPropertyByFullName(collectionNavigationPropertyElement.Attribute(XNAME_ForeignKey)?.Value),
                NAME_Upstream => FindUpstreamPropertyByFullName(collectionNavigationPropertyElement.Attribute(XNAME_ForeignKey)?.Value),
                _ => null,
            };
        }

        SqlColType? ToSqlSqlColType(XElement memberElement, out string sqlExpr)
        {
            if (memberElement is null || memberElement.Name.NamespaceName.Length > 0)
            {
                sqlExpr = SqlColType.NULL.ToString("F");
                return SqlColType.NULL;
            }

            SqlColType? result;
            switch (memberElement.Name.LocalName)
            {
                case NAME_NVarChar:
                    sqlExpr = $"{SqlColType.NVARCHAR}({memberElement.Attribute(XNAME_MaxLength)?.Value})";
                    return SqlColType.NVARCHAR;
                case NAME_VolumeIdentifier:
                    sqlExpr = $"{SqlColType.NVARCHAR}(1024)";
                    return SqlColType.NVARCHAR;
                case NAME_Char:
                    sqlExpr = $"{SqlColType.CHARACTER}(1)";
                    return SqlColType.CHARACTER;
                case NAME_ByteValues:
                    sqlExpr = $"{SqlColType.BINARY}({memberElement.Attribute(XNAME_MaxLength)?.Value})";
                    return SqlColType.BINARY;
                case NAME_MD5Hash:
                    sqlExpr = $"{SqlColType.BINARY}(16)";
                    return SqlColType.BINARY;
                case NAME_Enum:
                    result = (memberElement.Attribute(XNAME_Type)?.Value ?? "") switch
                    {
                        NAME_Byte => SqlColType.UNSIGNED_TINYINT,
                        NAME_SByte => SqlColType.TINYINT,
                        NAME_Short => SqlColType.SMALLINT,
                        NAME_UShort => SqlColType.UNSIGNED_SMALLINT,
                        NAME_UInt => SqlColType.UNSIGNED_INT,
                        NAME_Long => SqlColType.BIGINT,
                        NAME_ULong => SqlColType.UNSIGNED_BIGINT,
                        _ => SqlColType.INT
                    };
                    break;
                case NAME_RelatedEntity:
                case NAME_NewRelatedEntity:
                case NAME_CollectionNavigation:
                case NAME_NewCollectionNavigation:
                    result = null;
                    break;
                default:
                    result = memberElement.Name.LocalName switch
                    {
                        NAME_Byte => SqlColType.UNSIGNED_TINYINT,
                        NAME_DriveType => SqlColType.UNSIGNED_TINYINT,
                        NAME_SByte => SqlColType.TINYINT,
                        NAME_Short => SqlColType.SMALLINT,
                        NAME_UShort => SqlColType.UNSIGNED_SMALLINT,
                        NAME_Int => SqlColType.INT,
                        NAME_UInt => SqlColType.UNSIGNED_INT,
                        NAME_Long => SqlColType.BIGINT,
                        NAME_ULong => SqlColType.UNSIGNED_BIGINT,
                        NAME_Float => SqlColType.REAL,
                        NAME_Double => SqlColType.REAL,
                        NAME_Decimal => SqlColType.NUMERIC,
                        NAME_DateTime => SqlColType.DATETIME,
                        NAME_TimeSpan => SqlColType.TIME,
                        NAME_NewIdNavRef => SqlColType.UNIQUEIDENTIFIER,
                        NAME_UniqueIdentifier => SqlColType.UNIQUEIDENTIFIER,
                        NAME_Bit => SqlColType.BIT,
                        NAME_Text => SqlColType.TEXT,
                        NAME_MultiStringValue => SqlColType.TEXT,
                        _ => SqlColType.BLOB
                    };
                    break;
            }
            sqlExpr = (result.HasValue) ? result.Value.ToString("F") : null;
            return result;
        }
    }
    enum SqlColType
    {
        NVARCHAR,
        CHARACTER,
        BINARY,
        TINYINT,
        UNSIGNED_TINYINT,
        SMALLINT,
        UNSIGNED_SMALLINT,
        INT,
        UNSIGNED_INT,
        BIGINT,
        UNSIGNED_BIGINT,
        REAL,
        NUMERIC,
        DATETIME,
        TIME,
        UNIQUEIDENTIFIER,
        BIT,
        TEXT,
        BLOB,
        NULL
    }
}
