using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Stack<string> _indentValues = new();
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

        string TemplateFolder { get; } = "";

        string DefaultNamespace { get; } = "";

        string CustomToolOutput { get; } = "";

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
        const string NAME_ExtendsEntity = "ExtendsEntity";
        const string NAME_ExtendsGenericEntity = "ExtendsGenericEntity";
        const string NAME_Implements = "Implements";
        const string NAME_ImplementsEntity = "ImplementsEntity";
        const string NAME_ImplementsGenericEntity = "ImplementsGenericEntity";
        const string NAME_RootInterface = "RootInterface";
        const string NAME_Type = "Type";
        const string NAME_TypeDef = "TypeDef";
        const string NAME_ReferenceKey = "ReferenceKey";
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
        const string NAME_IsIndexed = "IsIndexed";
        const string NAME_IsUnique = "IsIndexed";
        const string NAME_IsPrimaryKey = "IsPrimaryKey";
        const string NAME_EitherOrConstraint = "EitherOrConstraint";
        const string NAME_Property = "Property";
        const string NAME_DisallowEmptyIfNotNull = "DisallowEmptyIfNotNull";
        const string NAME_IsNullSameConstraint = "IsNullSameConstraint";
        const string NAME_FieldComparisonConstraint = "IsNullSameConstraint";
        const string NAME_LeftProperty = "LeftProperty";
        const string NAME_RightProperty = "RightProperty";
        const string NAME_Operator = "Operator";

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
        static readonly XName XNAME_ExtendsEntity = XName.Get(NAME_ExtendsEntity);
        static readonly XName XNAME_ExtendsGenericEntity = XName.Get(NAME_ExtendsGenericEntity);
        static readonly XName XNAME_Implements = XName.Get(NAME_Implements);
        static readonly XName XNAME_ImplementsEntity = XName.Get(NAME_ImplementsEntity);
        static readonly XName XNAME_ImplementsGenericEntity = XName.Get(NAME_ImplementsGenericEntity);
        static readonly XName XNAME_RootInterface = XName.Get(NAME_RootInterface);
        static readonly XName XNAME_Type = XName.Get(NAME_Type);
        static readonly XName XNAME_TypeDef = XName.Get(NAME_TypeDef);
        static readonly XName XNAME_ReferenceKey = XName.Get(NAME_ReferenceKey);
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
        static readonly XName XNAME_IsIndexed = XName.Get(NAME_IsIndexed);
        static readonly XName XNAME_DefaultEmpty = XName.Get(NAME_DefaultEmpty);
        static readonly XName XNAME_IsUnique = XName.Get(NAME_IsUnique);
        static readonly XName XNAME_IsPrimaryKey = XName.Get(NAME_IsPrimaryKey);
        static readonly XName XNAME_EitherOrConstraint = XName.Get(NAME_EitherOrConstraint);
        static readonly XName XNAME_Property = XName.Get(NAME_Property);
        static readonly XName XNAME_DisallowEmptyIfNotNull = XName.Get(NAME_DisallowEmptyIfNotNull);
        static readonly XName XNAME_IsNullSameConstraint = XName.Get(NAME_IsNullSameConstraint);
        static readonly XName XNAME_FieldComparisonConstraint = XName.Get(NAME_FieldComparisonConstraint);
        static readonly XName XNAME_LeftProperty = XName.Get(NAME_LeftProperty);
        static readonly XName XNAME_RightProperty = XName.Get(NAME_RightProperty);
        static readonly XName XNAME_Operator = XName.Get(NAME_Operator);

        static readonly Regex NewLineRegex = new(@"\r\n?|[\n\p{Zl}\p{Zp}]", RegexOptions.Compiled);
        static readonly Regex NormalizeWsRegex = new(@" ((?![\r\n])\s)*|(?! )((?![\r\n])\s)+", RegexOptions.Compiled);
        static readonly Regex NormalizeNewLineRegex = new(@"[\v\t\p{Zl}\p{Zp}]|\r(?!\n)", RegexOptions.Compiled);
        static readonly Regex TrimOuterBlankLinesRegex = new(@"^(\s*(\r\n?|\n))+|((\r\n?|\n)\s*)+$", RegexOptions.Compiled);
        static readonly Regex StripWsRegex = new(@"^ [ \t\u0085\p{Zs}]+(?=[\r\n\v\t\p{Zl}\p{Zp}])|(?<=[\r\n\v\t\p{Zl}\p{Zp}])[ \t\u0085\p{Zs}]+", RegexOptions.Compiled);
        static readonly Regex LeadingWsRegex = new(@"^\s+", RegexOptions.Compiled);
        static readonly Regex LeadingEmptyLine = new(@"^([^\r\n\S]+)?(\r\n?|\n)", RegexOptions.Compiled);
        static readonly Regex TrailingEmptyLine = new(@"(\r\n?|\n)([^\r\n\S]+)?$", RegexOptions.Compiled);
        static readonly Regex TrailingWsRegex = new(@"\s+$", RegexOptions.Compiled);

        public record ValueRecord
        {
            public string SqlCode { get; init; }
            public string CsCode { get; init; }
        }
        public interface IProperty
        {
            public string Name { get; }
            public string FullName { get; }
            public string ColName { get; }
            public string SqlType { get; }
            public string CsType { get; }
            public bool AllowNull { get; }
            public bool IsGenericWritable { get; }
            public bool IsUnique { get; }
            public bool IsIndexed { get; }
            public Entity Entity { get; }
            public XElement Source { get; }
        }
        public record Property : IProperty
        {
            public string Name { get; init; }
            public string FullName { get; init; }
            public string ColName { get; init; }
            public string SqlType { get; init; }
            public string CsType { get; init; }
            public bool AllowNull { get; init; }
            public bool IsGenericWritable { get; init; }
            public bool IsUnique { get; init; }
            public bool IsIndexed { get; init; }
            public ValueRecord DefaultValue { get; init; }
            public Entity Entity { get; init; }
            public XElement Source { get; init; }
        }
        public record NumericProperty : IProperty
        {
            public string Name { get; init; }
            public string FullName { get; init; }
            public string ColName { get; init; }
            public string SqlType { get; init; }
            public string CsType { get; init; }
            public bool AllowNull { get; init; }
            public bool IsGenericWritable { get; init; }
            public bool IsUnique { get; init; }
            public bool IsIndexed { get; init; }
            public ValueRecord MinValue { get; init; }
            public ValueRecord MaxValue { get; init; }
            public ValueRecord DefaultValue { get; init; }
            public Entity Entity { get; init; }
            public XElement Source { get; init; }
        }
        public record VarCharProperty : IProperty
        {
            public string Name { get; init; }
            public string FullName { get; init; }
            public string ColName { get; init; }
            public string SqlType { get; init; }
            public string CsType { get; init; }
            public bool AllowNull { get; init; }
            public bool IsGenericWritable { get; init; }
            public bool IsUnique { get; init; }
            public bool IsIndexed { get; init; }
            public int? MinLength { get; init; }
            public int MaxLength { get; init; }
            public ValueRecord DefaultValue { get; init; }
            public Entity Entity { get; init; }
            public XElement Source { get; init; }
        }
        public record Entity
        {
            public string Name { get; init; }
            public string Namespace { get; init; }
            public string TableName { get; init; }
            public ReadOnlyCollection<Entity> BaseTypes { get; init; }
            public ReadOnlyCollection<Property> Properties { get; init; }
            public XElement Source { get; init; }
        }
        Entity GetEntity(XElement source)
        {
            if (source is null || source.Name != XNAME_Entity)
                return null;
            Entity entity = source.Annotation<Entity>();
            if (entity is not null)
                return entity;
            XName parentName = source.Parent?.Name;
            string ns;
            Func<string, XElement> findEntityByName;
            Func<string, XElement> findEnumByName;
            Func<string, XElement> findPropertyByFullName;
            Func<string, XElement> findFieldByFullName;
            if (parentName == XNAME_Root)
            {
                findEntityByName = FindRootEntityByName;
                findEnumByName = FindRootEnumByName;
                findPropertyByFullName = FindRootPropertyByFullName;
                findFieldByFullName = FindRootFieldByFullName;
                ns = "";
            }
            else if (parentName == XNAME_Local)
            {
                findEntityByName = FindLocalEntityByName;
                findEnumByName = FindLocalEnumByName;
                findPropertyByFullName = FindLocalPropertyByFullName;
                findFieldByFullName = FindLocalFieldByFullName;
                ns = NAME_Local;
            }
            else if (parentName == XNAME_Local || parentName == XNAME_Upstream)
            {
                findEntityByName = FindUpstreamEntityByName;
                findEnumByName = FindUpstreamEnumByName;
                findPropertyByFullName = FindUpstreamPropertyByFullName;
                findFieldByFullName = FindUpstreamFieldByFullName;
                ns = NAME_Upstream;
            }
            else
                return null;
            Collection<Property> properties = new();
            entity = new()
            {
                Name = source.Attribute(XNAME_Name)?.Value,
                Namespace = ns,
                TableName = source.Attribute(XNAME_TableName)?.Value,
                BaseTypes = new ReadOnlyCollection<Entity>(source.Elements(XNAME_ExtendsGenericEntity).Attributes(XNAME_TypeDef).Concat(source.Elements(XNAME_ExtendsEntity)
                    .Attributes(XNAME_Type)).Take(1).Concat(source.Attributes(XNAME_RootInterface))
                    .Concat(source.Elements().SelectMany(e => (e.Name == XNAME_ImplementsGenericEntity) ? e.Attributes(XNAME_TypeDef) :
                    (e.Name == XNAME_ImplementsEntity) ? e.Attributes("Type") : Enumerable.Empty<XAttribute>())).Select(a => GetEntity(findEntityByName(a.Value)))
                    .Where(e => e is not null).ToArray()),
                Properties = new ReadOnlyCollection<Property>(properties),
                Source = source
            };
            source.AddAnnotation(entity);
            foreach (XElement element in source.Elements(XNAME_Properties).Elements())
            {
                string name = element.Attribute(XNAME_Name)?.Value;
                string fullName = element.Attribute(XNAME_FullName)?.Value;
                string colName = element.Attribute(XNAME_ColName)?.Value;
                bool defaultNull = element.Element(XNAME_DefaultNull) is not null;
                bool allowNull = defaultNull || (FromXmlBoolean(element.Attribute(XNAME_AllowNull)?.Value) ?? false);
                bool isGenericWritable = FromXmlBoolean(element.Attribute(XNAME_IsGenericWritable)?.Value) ?? false;
                bool isUnique = FromXmlBoolean(element.Attribute(XNAME_IsUnique)?.Value) ?? false;
                bool isIndexed = FromXmlBoolean(element.Attribute(XNAME_IsIndexed)?.Value) ?? false;
                IProperty property;
                ValueRecord defaultValue;
                string text;
                if (element.Name == XNAME_MD5Hash)
                {

                }
                else if (element.Name == XNAME_DateTime)
                {
                    DateTime? defaultDateTime;
                    if (defaultNull)
                        defaultValue = new ValueRecord { CsCode = "null", SqlCode = "NULL" };
                    else if ((text = element.Attribute(XNAME_Default)?.Value) is null || !(defaultDateTime = FromXmlDateTime(text)).HasValue)
                    {
                        if (element.Element(XNAME_DefaultNow) is null)
                            defaultValue = null;
                        else
                            defaultValue = new ValueRecord
                            {
                                CsCode = "DateTime.Now",
                                SqlCode = "(datetime('now','localtime'))"
                            };
                    }
                    else
                        defaultValue = new ValueRecord
                        {
                            CsCode = $"new DateTime({defaultDateTime.Value.Year}, {defaultDateTime.Value.Month}, {defaultDateTime.Value.Day}, {defaultDateTime.Value.Hour}, {defaultDateTime.Value.Minute}, {defaultDateTime.Value.Second}, DateTimeKind.Local)",
                            SqlCode = defaultDateTime.Value.ToString("'yyyy-MM-dd HH:mm:ss")
                        };
                    property = new Property
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = "TimeSpan",
                        SqlType = "TIME",
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        DefaultValue = defaultValue,
                        Entity = entity,
                        Source = element
                    };
                }
                else if (element.Name == XNAME_TimeSpan)
                {
                    TimeSpan? defaultTimeSpan;
                    if (defaultNull)
                        defaultValue = new ValueRecord { CsCode = "null", SqlCode = "NULL" };
                    else if ((text = element.Attribute(XNAME_Default)?.Value) is null || !(defaultTimeSpan = FromXmlTimeSpan(text)).HasValue)
                    {
                        if (element.Element(XNAME_DefaultZero) is null)
                            defaultValue = null;
                        else
                            defaultValue = new ValueRecord
                            {
                                CsCode = "TimeSpan.Zero",
                                SqlCode = "'00:00:00.000'"
                            };
                    }
                    else
                        defaultValue = new ValueRecord
                        {
                            CsCode = $"new TimeSpan({defaultTimeSpan.Value.Hours}, {defaultTimeSpan.Value.Minutes}, {defaultTimeSpan.Value.Seconds})",
                            SqlCode = defaultTimeSpan.Value.ToString("\\'hh\\:mm\\:ss\\.fff\\'")
                        };
                    property = new Property
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = "TimeSpan",
                        SqlType = "TIME",
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        DefaultValue = defaultValue,
                        Entity = entity,
                        Source = element
                    };
                }
                else if (element.Name == XNAME_UniqueIdentifier)
                    property = new Property
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = "char",
                        SqlType = "CHAR(1)",
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        DefaultValue = defaultNull ? new ValueRecord { CsCode = "null", SqlCode = "NULL" } : null,
                        Entity = entity,
                        Source = element
                    };
                else if (element.Name == XNAME_Char)
                {
                    if (defaultNull)
                        defaultValue = new ValueRecord { CsCode = "null", SqlCode = "NULL" };
                    else if ((text = element.Attribute(XNAME_Default)?.Value) is null)
                        defaultValue = null;
                    else
                        defaultValue = new ValueRecord
                        {
                            CsCode = $"'{text.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\"", "\\\"")}'",
                            SqlCode = $"'{text.Replace("'", "''")}'"
                        };
                    property = new Property
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = "char",
                        SqlType = "CHAR(1)",
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        DefaultValue = defaultValue,
                        Entity = entity,
                        Source = element
                    };
                }
                else if (element.Name == XNAME_NVarChar)
                {
                    if (defaultNull)
                        defaultValue = new ValueRecord { CsCode = "null", SqlCode = "NULL" };
                    else if ((text = element.Attribute(XNAME_Default)?.Value) is null)
                        defaultValue = null;
                    else
                        defaultValue = new ValueRecord
                        {
                            CsCode = $"\"{text.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\"", "\\\"")}\"",
                            SqlCode = text
                        };
                    int? minLength = FromXmlInt32(element.Attribute(XNAME_MinLength)?.Value);
                    int? maxLength = FromXmlInt32(element.Attribute(XNAME_MaxLength)?.Value);
                    property = new VarCharProperty
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = element.Name.LocalName.ToLower(),
                        SqlType = maxLength.HasValue ? $"NVARCHAR({maxLength.Value})" : "NVARCHAR",
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        MinLength = minLength,
                        MaxLength = maxLength.HasValue ? maxLength.Value : 0,
                        DefaultValue = defaultValue,
                        Entity = entity,
                        Source = element
                    };
                }
                else if (element.Name == XNAME_Text || element.Name == XNAME_MultiStringValue)
                {
                    if (defaultNull)
                        defaultValue = new ValueRecord { CsCode = "null", SqlCode = "NULL" };
                    else if ((text = element.Attribute(XNAME_Default)?.Value) is null)
                        defaultValue = null;
                    else
                        defaultValue = new ValueRecord
                        {
                            CsCode = $"\"{text.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\"", "\\\"")}\"",
                            SqlCode = text
                        };
                    property = new Property
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = "string",
                        SqlType = "TEXT",
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        DefaultValue = defaultValue,
                        Entity = entity,
                        Source = element
                    };
                }
                else if (element.Name == XNAME_NewIdNavRef)
                {

                }
                else if (element.Name == XNAME_Bit)
                {

                }
                else if (element.Name == XNAME_VolumeIdentifier)
                {

                }
                else if (element.Name == XNAME_DriveType)
                {

                }
                else if (element.Name == XNAME_Enum)
                {

                }
                else if (element.Name == XNAME_RelatedEntity)
                {

                }
                else if (element.Name == XNAME_NewRelatedEntity)
                {

                }
                else if (element.Name == XNAME_CollectionNavigation)
                {

                }
                else if (element.Name == XNAME_NewCollectionNavigation)
                {

                }
                else
                {
                    if (defaultNull)
                        defaultValue = new ValueRecord { CsCode = "null", SqlCode = "NULL" };
                    else if ((text = element.Attribute(XNAME_Default)?.Value) is null || (text = text.Trim()).Length == 0)
                        defaultValue = null;
                    else
                        defaultValue = new ValueRecord { CsCode = text, SqlCode = text };
                    string sqlType;
                    if (element.Name == XNAME_Byte)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"(byte){defaultValue.CsCode}" };
                        sqlType = "UNSIGNED TINYINT";
                    }
                    else if (element.Name == XNAME_SByte)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"(sbyte){defaultValue.CsCode}" };
                        sqlType = "TINYINT";
                    }
                    else if (element.Name == XNAME_Short)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"(short){defaultValue.CsCode}" };
                        sqlType = "SMALLINT";
                    }
                    else if (element.Name == XNAME_UShort)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"(ushort){defaultValue.CsCode}" };
                        sqlType = "UNSIGNED SMALLINT";
                    }
                    else if (element.Name == XNAME_Int)
                        sqlType = "INT";
                    else if (element.Name == XNAME_UInt)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"(uint){defaultValue.CsCode}" };
                        sqlType = "UNSIGNED INT";
                    }
                    else if (element.Name == XNAME_Long)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"{defaultValue.CsCode}L" };
                        sqlType = "BIGINT";
                    }
                    else if (element.Name == XNAME_ULong)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"{defaultValue.CsCode}UL" };
                        sqlType = "UNSIGNED BIGINT";
                    }
                    else if (element.Name == XNAME_Decimal)
                    {
                        if (!(defaultNull || defaultValue is null))
                            defaultValue = defaultValue with { CsCode = $"{defaultValue.CsCode}m" };
                        sqlType = "NUMERIC";
                    }
                    else
                    {
                        if (element.Name == XNAME_Double)
                        {
                            if (!(defaultNull || defaultValue is null || defaultValue.CsCode.Contains("")))
                                defaultValue = defaultValue with { CsCode = $"{defaultValue.CsCode}.0" };
                        }
                        else if (element.Name == XNAME_Float)
                        {
                            if (!(defaultNull || defaultValue is null))
                                defaultValue = defaultValue with { CsCode = $"{defaultValue.CsCode}f" };
                        }
                        else
                            continue;
                        sqlType = "REAL";
                    }
                    text = element.Attribute(XNAME_MinValue)?.Value;
                    string maxValue = element.Attribute(XNAME_MaxValue)?.Value;
                    property = new NumericProperty
                    {
                        Name = name,
                        FullName = fullName,
                        ColName = colName,
                        CsType = element.Name.LocalName.ToLower(),
                        SqlType = sqlType,
                        AllowNull = allowNull,
                        IsGenericWritable = isGenericWritable,
                        IsUnique = isUnique,
                        IsIndexed = isIndexed,
                        MinValue = (text is null || (text = text.Trim()).Length == 0) ? null : new ValueRecord { CsCode = text, SqlCode = text },
                        MaxValue = (maxValue is null || (maxValue = maxValue.Trim()).Length == 0) ? null : new ValueRecord { CsCode = maxValue, SqlCode = maxValue },
                        DefaultValue = defaultValue, 
                        Entity = entity,
                        Source = element
                    };
                }
            }
            return entity;
        }

        bool? FromXmlBoolean(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToBoolean(xml); }
            catch { return null; }
        }

        int? FromXmlInt32(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToInt32(xml); }
            catch { return null; }
        }

        DateTime? FromXmlDateTime(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToDateTime(xml, XmlDateTimeSerializationMode.RoundtripKind); }
            catch { return null; }
        }

        TimeSpan? FromXmlTimeSpan(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToTimeSpan(xml); }
            catch { return null; }
        }

        Guid? FromXmlGuid(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;
            try { return XmlConvert.ToGuid(xml); }
            catch { return null; }
        }

        byte[] FromXmlBinary(string xml)
        {
            if (xml is null)
                return null;
            if ((xml = xml.Trim()).Length > 0)
                try { return Convert.FromBase64String(xml); }
                catch { return null; }
            return Array.Empty<byte>();
        }

        XElement FindRootEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Root)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Root)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootByName(string name) { return (name is null) ? null : (FindRootEntityByName(name) ?? FindRootEnumByName(name)); }

        XElement FindLocalEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Local)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEntityByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindLocalEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Local)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEnumByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindLocalByName(string name) { return (name is null) ? null : (FindLocalEntityByName(name) ?? FindLocalEnumByName(name)); }

        XElement FindUpstreamEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Upstream)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEntityByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Upstream)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEnumByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamByName(string name) { return (name is null) ? null : (FindUpstreamEntityByName(name) ?? FindUpstreamEnumByName(name)); }

        XElement FindRootPropertyByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Root).Elements(XNAME_Entity).Elements(XNAME_Properties).Elements().Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootFieldByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Root).Elements(XNAME_EnumTypes).Elements().Elements(XNAME_Field).Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootMemberByFullName(string fullName) { return (fullName is null) ? null : (FindRootPropertyByFullName(fullName) ?? FindRootFieldByFullName(fullName)); }

        XElement FindLocalPropertyByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Local).Elements(XNAME_Entity).Elements(XNAME_Properties).Elements().Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootPropertyByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindLocalFieldByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Local).Elements(XNAME_EnumTypes).Elements().Elements(XNAME_Field).Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootFieldByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindLocalMemberByFullName(string fullName) { return (fullName is null) ? null : (FindLocalPropertyByFullName(fullName) ?? FindLocalFieldByFullName(fullName)); }

        XElement FindUpstreamPropertyByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Upstream).Elements(XNAME_Entity).Elements(XNAME_Properties).Elements().Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootPropertyByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamFieldByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Upstream).Elements(XNAME_EnumTypes).Elements().Elements(XNAME_Field).Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootFieldByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamMemberByFullName(string fullName) { return (fullName is null) ? null : (FindUpstreamPropertyByFullName(fullName) ?? FindUpstreamFieldByFullName(fullName)); }

        XElement GetEnumPropertyEnumType(XElement enumPropertyElement)
        {
            XElement parent = enumPropertyElement?.Parent;
            if (parent is null || enumPropertyElement.Name != XNAME_Enum)
                return null;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    return FindRootEnumByName(enumPropertyElement.Attribute(XNAME_Name)?.Value);
                case NAME_Local:
                    return FindLocalEnumByName(enumPropertyElement.Attribute(XNAME_Name)?.Value);
                case NAME_Upstream:
                    return FindUpstreamEnumByName(enumPropertyElement.Attribute(XNAME_Name)?.Value);
                default:
                    return null;
            }
        }

        XElement GetEnumPropertyDefaultField(XElement enumPropertyElement)
        {
            XElement parent = enumPropertyElement?.Parent;
            if (parent is null || enumPropertyElement.Name != XNAME_Enum)
                return null;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    return FindRootFieldByFullName(enumPropertyElement.Element(XNAME_Default)?.Value);
                case NAME_Local:
                    return FindLocalFieldByFullName(enumPropertyElement.Element(XNAME_Default)?.Value);
                case NAME_Upstream:
                    return FindUpstreamFieldByFullName(enumPropertyElement.Element(XNAME_Default)?.Value);
                default:
                    return null;
            }
        }

        XElement GetEnumFieldAmbientEnumField(XElement enumFieldElement)
        {
            XElement parent = enumFieldElement?.Parent;
            if (parent is null || enumFieldElement.Name != XNAME_Enum)
                return null;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    return FindRootFieldByFullName(enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault());
                case NAME_Local:
                    return FindLocalFieldByFullName(enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault());
                case NAME_Upstream:
                    return FindUpstreamFieldByFullName(enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault());
                default:
                    return null;
            }
        }

        XElement GetRelatedEntity(XElement relatedEntityPropertyElement)
        {
            XElement scopeElement, parentEntityElement = relatedEntityPropertyElement?.Parent?.Parent;
            if (parentEntityElement is null || (scopeElement = parentEntityElement.Parent) is null ||
                    (relatedEntityPropertyElement.Name != XNAME_RelatedEntity && relatedEntityPropertyElement.Name != XNAME_NewRelatedEntity))
                return null;
            XElement referenceElement;
            switch (scopeElement.Name.LocalName)
            {
                case NAME_Root:
                    referenceElement = relatedEntityPropertyElement.Element(XNAME_Reference);
                    return (referenceElement is null) ? FindRootPropertyByFullName(relatedEntityPropertyElement.Element(XNAME_ReferenceKey)?.Value)?.Parent : FindRootEntityByName(referenceElement.Value);
                case NAME_Local:
                    referenceElement = relatedEntityPropertyElement.Element(XNAME_Reference);
                    return (referenceElement is null) ? FindLocalPropertyByFullName(relatedEntityPropertyElement.Element(XNAME_ReferenceKey)?.Value)?.Parent : FindLocalEntityByName(referenceElement.Value);
                case NAME_Upstream:
                    referenceElement = relatedEntityPropertyElement.Element(XNAME_Reference);
                    return (referenceElement is null) ? FindUpstreamPropertyByFullName(relatedEntityPropertyElement.Element(XNAME_ReferenceKey)?.Value)?.Parent :
                        FindUpstreamEntityByName(referenceElement.Value);
                default:
                    return null;
            }
        }

        XElement GetCollectionNavigationItemEntity(XElement collectionNavigationPropertyElement)
        {
            XElement scopeElement, parentEntityElement = collectionNavigationPropertyElement?.Parent?.Parent;
            if (parentEntityElement is null || (scopeElement = parentEntityElement.Parent) is null ||
                    (collectionNavigationPropertyElement.Name != XNAME_CollectionNavigation && collectionNavigationPropertyElement.Name != XNAME_NewCollectionNavigation))
                return null;
            XElement itemTypeElement;
            switch (scopeElement.Name.LocalName)
            {
                case NAME_Root:
                    itemTypeElement = collectionNavigationPropertyElement.Element(XNAME_ItemType);
                    return (itemTypeElement is null) ? FindRootPropertyByFullName(collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value)?.Parent :
                        FindRootEntityByName(itemTypeElement.Value);
                case NAME_Local:
                    itemTypeElement = collectionNavigationPropertyElement.Element(XNAME_ItemType);
                    return (itemTypeElement is null) ? FindLocalPropertyByFullName(collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value)?.Parent :
                        FindLocalEntityByName(itemTypeElement.Value);
                case NAME_Upstream:
                    itemTypeElement = collectionNavigationPropertyElement.Element(XNAME_ItemType);
                    return (itemTypeElement is null) ? FindUpstreamPropertyByFullName(collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value)?.Parent :
                        FindUpstreamEntityByName(itemTypeElement.Value);
                default:
                    return null;
            }
        }

        void GetAllProperties(XElement entityElement, Collection<(string Name, LinkedList<XElement> Sources)> collection)
        {
            foreach (XElement baseEntity in GetBaseEntities(entityElement).Reverse())
                GetAllProperties(baseEntity, collection);
            foreach (XAttribute attribute in entityElement.Elements(XNAME_Properties).Elements().Attributes(XNAME_Name))
            {
                XElement propertyElement = attribute.Parent;
                string propertyName = attribute.Value;
                if (collection.Any(t => t.Name == propertyName))
                {
                    (string Name, LinkedList<XElement> Sources) property = collection.First(t => t.Name == propertyName);
                    if (property.Sources.Any(e => ReferenceEquals(e, propertyElement)))
                        break;
                    property.Sources.AddFirst(propertyElement);
                }
                else
                {
                    (string Name, LinkedList<XElement> Sources) property = new(propertyName, new LinkedList<XElement>());
                    property.Sources.AddLast(propertyElement);
                    collection.Add(property);
                }
            }
        }

        IEnumerable<XElement> GetBaseEntities(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Array.Empty<XElement>();

            IEnumerable<string> names = entityElement.Elements().Select(e => (
                IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
                IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
                Element: e
            )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    return names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null);
                case NAME_Local:
                    return entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindLocalEntityByName(n)).Where(e => e is not null);
                case NAME_Upstream:
                    return entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null);
                default:
                    return Array.Empty<XElement>();
            }
        }

        void GetAllBaseEntities(XElement entityElement, int level, Collection<(XElement Element, int Level)> collection, Func<IEnumerable<string>, IEnumerable<XElement>> getEntities)
        {
            IEnumerable<string> names = entityElement.Elements().Select(e => (
                IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
                IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
                Element: e
            )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
            int nextLevel = level + 1;
            foreach (XElement baseEntity in getEntities(names))
            {
                IEnumerable<(XElement Element, int Level)> items = collection.Where(e => ReferenceEquals(e.Element, baseEntity));
                if (items.Any())
                {
                    (XElement Element, int Level) t = items.First();
                    if (level < t.Level)
                    {
                        collection.Remove(t);
                        collection.Add(new(baseEntity, level));
                    }
                }
                else
                {
                    collection.Add(new(baseEntity, level));
                    GetAllBaseEntities(baseEntity, nextLevel, collection, getEntities);
                }
            }
        }

        XElement[] GetAllBaseEntities(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Array.Empty<XElement>();
            Collection<(XElement Element, int Level)> result = new();
            Func<IEnumerable<string>, IEnumerable<XElement>> getEntities;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    getEntities = names => names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null);
                    break;
                case NAME_Local:
                    getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindLocalEntityByName(n)).Where(e => e is not null);
                    break;
                case NAME_Upstream:
                    getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null);
                    break;
                default:
                    return Array.Empty<XElement>();
            }
            GetAllBaseEntities(entityElement, 0, result, getEntities);
            return result.OrderBy(t => t.Level).Select(t => t.Element).ToArray();
        }

        XElement[] GetAllBaseProperties(XElement propertyElement, XElement[] orderedBaseEntities, out XElement baseProperty, out bool isNew, out bool doNotEmit)
        {
            string propertyName = propertyElement?.Attribute(XNAME_Name)?.Value;
            if (propertyName is null || orderedBaseEntities is null || orderedBaseEntities.Length == 0)
            {
                baseProperty = propertyElement;
                isNew = doNotEmit = false;
                return Array.Empty<XElement>();
            }
            XName baseName;
            XName inheritedName;
            switch (propertyElement.Name.LocalName)
            {
                case "NewIdNavRef":
                    isNew = false;
                    doNotEmit = true;
                    baseName = XNAME_UniqueIdentifier;
                    inheritedName = XNAME_NewIdNavRef;
                    break;
                case "NewRelatedEntity":
                    isNew = true;
                    doNotEmit = false;
                    baseName = XNAME_RelatedEntity;
                    inheritedName = XNAME_NewRelatedEntity;
                    break;
                case "NewCollectionNavigation":
                    isNew = true;
                    doNotEmit = false;
                    baseName = XNAME_CollectionNavigation;
                    inheritedName = XNAME_NewCollectionNavigation;
                    break;
                default:
                    isNew = doNotEmit = false;
                    baseProperty = propertyElement;
                    return orderedBaseEntities.Elements(XNAME_Properties).Elements(propertyElement.Name).Attributes(XNAME_Name).Where(a => a.Value == propertyName).Select(a => a.Parent).ToArray();
            }
            IEnumerable<XElement> results = orderedBaseEntities.Elements(XNAME_Properties).Elements().Where(e => e.Name == baseName || e.Name == inheritedName)
                .Attributes(XNAME_Name).Where(a => a.Value == propertyName).Select(a => a.Parent);
            baseProperty = results.Where(e => e.Name == baseName).DefaultIfEmpty(propertyElement).First();
            return results.ToArray();
        }

        XElement GetRelatedEntityKeyProperty(XElement relatedEntityPropertyElement)
        {
            XElement parent = relatedEntityPropertyElement?.Parent;
            if (parent is null || (relatedEntityPropertyElement.Name != XNAME_RelatedEntity && relatedEntityPropertyElement.Name != XNAME_NewRelatedEntity))
                return null;
            XElement referenceElement;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    if ((referenceElement = relatedEntityPropertyElement.Element(XNAME_ReferenceKey)) is null)
                    {
                        if ((referenceElement = relatedEntityPropertyElement.Element(XNAME_Reference)) is null || (referenceElement = FindRootEntityByName(referenceElement.Value)) is null)
                            return null;
                        string name = relatedEntityPropertyElement.Attribute(XNAME_Name)?.Value;
                        return referenceElement.Elements(XNAME_Properties).Elements().FirstOrDefault(e => e.Attribute(XNAME_Name)?.Value == name);
                    }
                    return FindRootPropertyByFullName(referenceElement.Value);
                case NAME_Local:
                    if ((referenceElement = relatedEntityPropertyElement.Element(XNAME_ReferenceKey)) is null)
                    {
                        if ((referenceElement = relatedEntityPropertyElement.Element(XNAME_Reference)) is null || (referenceElement = FindLocalEntityByName(referenceElement.Value)) is null)
                            return null;
                        string name = relatedEntityPropertyElement.Attribute(XNAME_Name)?.Value;
                        return referenceElement.Elements(XNAME_Properties).Elements().FirstOrDefault(e => e.Attribute(XNAME_Name)?.Value == name);
                    }
                    return FindLocalPropertyByFullName(referenceElement.Value);
                case NAME_Upstream:
                    if ((referenceElement = relatedEntityPropertyElement.Element(XNAME_ReferenceKey)) is null)
                    {
                        if ((referenceElement = relatedEntityPropertyElement.Element(XNAME_Reference)) is null || (referenceElement = FindUpstreamEntityByName(referenceElement.Value)) is null)
                            return null;
                        string name = relatedEntityPropertyElement.Attribute(XNAME_Name)?.Value;
                        return referenceElement.Elements(XNAME_Properties).Elements().FirstOrDefault(e => e.Attribute(XNAME_Name)?.Value == name);
                    }
                    return FindUpstreamPropertyByFullName(referenceElement.Value);
                default:
                    return null;
            }
        }

        XElement GetCollectionNavigationItemKeyProperty(XElement collectionNavigationPropertyElement)
        {
            XElement parent = collectionNavigationPropertyElement?.Parent;
            if (parent is null || (collectionNavigationPropertyElement.Name != XNAME_CollectionNavigation && collectionNavigationPropertyElement.Name != XNAME_NewCollectionNavigation))
                return null;
            XElement itemKeyElement;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    if ((itemKeyElement = collectionNavigationPropertyElement.Element(XNAME_ItemKey)) is null)
                    {
                        if ((itemKeyElement = collectionNavigationPropertyElement.Element(XNAME_ItemType)) is null || (itemKeyElement = FindRootEntityByName(itemKeyElement.Value)) is null)
                            return null;
                        string name = collectionNavigationPropertyElement.Attribute(XNAME_Name)?.Value;
                        return itemKeyElement.Elements(XNAME_Properties).Elements().FirstOrDefault(e => e.Attribute(XNAME_Name)?.Value == name);
                    }
                    return FindRootPropertyByFullName(collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value);
                case NAME_Local:
                    if ((itemKeyElement = collectionNavigationPropertyElement.Element(XNAME_ItemKey)) is null)
                    {
                        if ((itemKeyElement = collectionNavigationPropertyElement.Element(XNAME_ItemType)) is null || (itemKeyElement = FindLocalEntityByName(itemKeyElement.Value)) is null)
                            return null;
                        string name = collectionNavigationPropertyElement.Attribute(XNAME_Name)?.Value;
                        return itemKeyElement.Elements(XNAME_Properties).Elements().FirstOrDefault(e => e.Attribute(XNAME_Name)?.Value == name);
                    }
                    return FindLocalPropertyByFullName(collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value);
                case NAME_Upstream:
                    if ((itemKeyElement = collectionNavigationPropertyElement.Element(XNAME_ItemKey)) is null)
                    {
                        if ((itemKeyElement = collectionNavigationPropertyElement.Element(XNAME_ItemType)) is null || (itemKeyElement = FindUpstreamEntityByName(itemKeyElement.Value)) is null)
                            return null;
                        string name = collectionNavigationPropertyElement.Attribute(XNAME_Name)?.Value;
                        return itemKeyElement.Elements(XNAME_Properties).Elements().FirstOrDefault(e => e.Attribute(XNAME_Name)?.Value == name);
                    }
                    return FindUpstreamPropertyByFullName(collectionNavigationPropertyElement.Element(XNAME_ItemKey)?.Value);
                default:
                    return null;
            }
        }

        XText ToWhiteSpaceNormalized(XText source)
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

        XElement WsNormalizedWithoutElementNamespace(XElement sourceParent)
        {
            if (sourceParent.Name.LocalName == "langword")
                return new XElement(XNamespace.None.GetName("see"), new XAttribute("langword", sourceParent.Value));
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

        XElement WithoutElementNamespace(XElement sourceParent)
        {
            if (sourceParent.Name.LocalName == "langword")
                return new XElement(XNamespace.None.GetName("see"), new XAttribute("langword", sourceParent.Value));
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

        IEnumerable<XElement> GetByNames(XElement source, params XName[] names) => names.SelectMany(n => source.Elements(n));

        string[] ToXmlLines(IEnumerable<XElement> elements)
        {
            if (elements is null || !elements.Any())
                return Array.Empty<string>();
            using StringWriter stringWriter = new();
            XDocument doc = new(new XElement(WsNormalizedWithoutElementNamespace(elements.First())));
            using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true }))
            {
                doc.WriteTo(writer);
                writer.Flush();
            }
            foreach (XElement e in elements.Skip(1))
            {
                stringWriter.WriteLine();
                using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true });
                doc = new XDocument(new XElement(WsNormalizedWithoutElementNamespace(e)));
                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
            }
            string result = stringWriter.ToString();
            string[] lines = NewLineRegex.IsMatch(result) ? NewLineRegex.Split(result) : new string[] { result };
            int trimLength = lines.Select(t => LeadingWsRegex.Match(t)).Select(m => m.Success ? m.Length : 0).Min();
            return (trimLength > 0) ? lines.Select(t => t.Substring(trimLength)).ToArray() : lines;
        }

        Type ToUnderlyingType(Type type)
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

        string ToSqlTypeName(Type type, out string fullName, out Type dbCompatible)
        {
            if (type is null)
            {
                fullName = "NULL";
                dbCompatible = null;
                return fullName;
            }
            dbCompatible = ToUnderlyingType(type);
            if (dbCompatible.IsPrimitive)
            {
                if (dbCompatible == typeof(char))
                {
                    fullName = "CHARACTER(1)";
                    return "CHARACTER";
                }
                if (dbCompatible == typeof(bool))
                    fullName = "BIT";
                else if (dbCompatible == typeof(byte))
                    fullName = "UNSIGNED TINYINT";
                else if (dbCompatible == typeof(sbyte))
                    fullName = "TINYINT";
                else if (dbCompatible == typeof(short))
                    fullName = "SMALLINT";
                else if (dbCompatible == typeof(ushort))
                    fullName = "UNSIGNED SMALLINT";
                else if (dbCompatible == typeof(int))
                    fullName = "INT";
                else if (dbCompatible == typeof(uint))
                    fullName = "UNSIGNED INT";
                else if (dbCompatible == typeof(long))
                    fullName = "BIGINT";
                else if (dbCompatible == typeof(ulong))
                    fullName = "UNSIGNED BIGINT";
                else
                    fullName = "REAL";
            }
            else if (dbCompatible == typeof(decimal))
                fullName = "NUMERIC";
            else if (dbCompatible == typeof(DateTime))
                fullName = "DATETIME";
            else if (dbCompatible == typeof(DateTimeOffset))
                fullName = "DATETIMEOFFSET";
            else if (dbCompatible == typeof(TimeSpan))
                fullName = "TIME";
            else if (dbCompatible == typeof(Guid))
                fullName = "UNIQUEIDENTIFIER";
            else if (dbCompatible == typeof(string))
                fullName = "TEXT";
            else
                fullName = "BLOB";
            return fullName;
        }

        #endregion

        #region EntityTypes.tt

        public void GenerateEntityTypes()
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
                    WriteLine($"#warning {XNAME_Name} attribute missing or empty for /{enumElement.Parent.Parent.Parent.Name}/{enumElement.Parent.Parent.Name}/{XNAME_EnumTypes}/{elementName}[(enumElement.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == elementName) + 1)]");
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

        IEnumerable<string> GetBaseTypeNames(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Enumerable.Empty<string>();

            IEnumerable<string> extends = entityElement.Elements().Where(e => e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ExtendsGenericEntity).Attributes(XNAME_Type).Select(a => a.Value);
            IEnumerable<string> implements = entityElement.Elements().Where(e => e.Name == XNAME_Implements || e.Name == XNAME_ImplementsEntity || e.Name == XNAME_ImplementsGenericEntity).Attributes(XNAME_Type).Select(a => a.Value);
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
            string ambientValue = memberElement.Elements("AmbientString").Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
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
            ambientValue = memberElement.Elements("AmbientUInt").Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("u)]");
                return;
            }
            ambientValue = memberElement.Elements("AmbientLong").Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ambientValue))
            {
                Write("[AmbientValue(");
                Write(ambientValue);
                WriteLine("L)]");
                return;
            }
            ambientValue = memberElement.Elements("AmbientULong").Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault();
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
            if (enumElement.Attributes(XNAME_IsFlags).Any(a => a.Value == "true"))
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
            foreach (XElement propertyElement in entityElement.Elements(XNAME_Properties).Elements())
            {
                if (propertyElement.Name.NamespaceName.Length > 0)
                    continue;
                string propertyName = propertyElement.Attribute(XNAME_Name)?.Value;
                if (string.IsNullOrEmpty(propertyName))
                    continue;
                XElement[] inheritedProperties = GetAllBaseProperties(propertyElement, baseEntities, out XElement baseProperty, out bool isNew, out bool doNotEmit);
                if (doNotEmit)
                    continue;
                if (isSubsequentMember)
                    WriteLine("");
                else
                    isSubsequentMember = true;
                commentDocElement = propertyElement.Element(XNAME_summary) ?? inheritedProperties.Elements(XNAME_summary).FirstOrDefault();
                if (commentDocElement is null)
                    WriteLine($"#warning No summary element found for {typeName}.{propertyName}");
                else
                    GenerateXmlDoc(commentDocElement);
                commentDocElement = propertyElement.Element(XNAME_value) ?? inheritedProperties.Elements(XNAME_value).FirstOrDefault();
                if (commentDocElement is null)
                    WriteLine($"#warning No value element found for {typeName}.{propertyName}");
                else
                    GenerateXmlDoc(commentDocElement);
                commentDocElement = propertyElement.Element(XNAME_remarks) ?? inheritedProperties.Elements(XNAME_remarks).FirstOrDefault();
                if (commentDocElement is not null)
                    GenerateXmlDoc(commentDocElement);
                foreach (XElement e in propertyElement.Elements(XNAME_seealso).Concat(inheritedProperties.Elements(XNAME_seealso)).Distinct())
                {
                    if (!e.IsEmpty && e.Value.Trim().Length == 0)
                        e.RemoveAll();
                    GenerateXmlDoc(e);
                }
                WriteDisplayAttribute(propertyName, n => propertyElement.Attributes(n).Concat(inheritedProperties.Attributes(n)));
                bool allowsNull = baseProperty.Attributes(XNAME_AllowNull).Any(a => a.Value == "true") || baseProperty.Elements(XNAME_DefaultNull).Any();
                if (isNew)
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
                    case "NewIdNavRef":
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
                    case "NewCollectionNavigation":
                        Write("IEnumerable<");
                        Write(propertyElement.Attribute(XNAME_ItemType)?.Value ?? GetCollectionNavigationItemEntity(propertyElement)?.Attribute(XNAME_Name)?.Value);
                        Write("> ");
                        break;
                    case "RelatedEntity":
                    case "NewRelatedEntity":
                        Write(propertyElement.Attribute(XNAME_TypeDef)?.Value ?? GetRelatedEntity(propertyElement)?.Attribute(XNAME_Name)?.Value);
                        Write(" ");
                        break;
                    default:
                        WriteLine($"#warning Unknown element: {propertyElement.Name.LocalName}");
                        Write("object ");
                        break;
                }

                Write(propertyName);
                WriteLine(baseProperty.Attributes(XNAME_IsGenericWritable).Any(a => a.Value == "true") ? " { get; set; }" : " { get; }");
            }

            PopIndent();
            WriteLine("}");
        }

        #endregion

        #region CreateLocalDb.tt

        void GenerateCreateTableSql(string tableName, XElement entityElement)
        {
            string entityName = entityElement.Attribute(XNAME_Name)?.Value;
            Write("CREATE TABLE IF NOT EXISTS ");
            Write(tableName);
            WriteLine(" (");
            PushIndent("    ");
            Collection<(string Name, LinkedList<XElement> Sources)> collection = new();
            GetAllProperties(entityElement, collection);
            (string Name, LinkedList<XElement> Sources) firstCol = new(GenerateTableColumnSql(tableName, entityName, collection[0].Name, collection[0].Sources, out string comment), collection[0].Sources);
            (string Name, LinkedList<XElement> Sources)[] columns = new (string Name, LinkedList<XElement> Sources)[] { firstCol }
                .Concat(collection.Skip(1).Select<(string Name, LinkedList<XElement> Sources), (string Name, LinkedList<XElement> Sources)>(t =>
                {
                    if (string.IsNullOrEmpty(comment))
                        WriteLine(",");
                    else
                    {
                        Write(", -- ");
                        WriteLine(comment);
                    }
                    (string Name, LinkedList<XElement> Sources) r = new(GenerateTableColumnSql(tableName, entityName, t.Name, t.Sources, out string comment2), t.Sources);
                    comment = comment2;
                    return r;
                }).Where(t => t.Name is not null)).ToArray();
            string[] keyColumns = columns.Where(c => c.Sources.Any(e => e.Attribute(XNAME_IsPrimaryKey)?.Value == "true" && e.Name == XNAME_UniqueIdentifier))
                .Select(c => $"\"{c.Name}\"").ToArray();
            Collection<string> constraints = new();
            foreach (XElement element in new XElement[] { entityElement }.Concat(GetAllBaseEntities(entityElement)))
            {
                if (element.Name == XNAME_EitherOrConstraint)
                {
                    string p1 = element.Elements(XNAME_Property).Take(1).Select(p =>
                    {
                        string disallowEmptyIfNotNull = p.Attribute(XNAME_DisallowEmptyIfNotNull)?.Value;
                        if (string.IsNullOrWhiteSpace(disallowEmptyIfNotNull))
                            return $"\"{p.Attribute(XNAME_Name)?.Value}\" IS NULL";
                        return $"(\"{p.Attribute(XNAME_Name)?.Value}\" IS NULL OR length(trim(\"{disallowEmptyIfNotNull}\")>0)";
                    }).First();
                    string p2 = element.Elements(XNAME_Property).Skip(1).Take(1).Select(p =>
                    {
                        string disallowEmptyIfNotNull = p.Attribute(XNAME_DisallowEmptyIfNotNull)?.Value;
                        if (string.IsNullOrWhiteSpace(disallowEmptyIfNotNull))
                            return $"\"{p.Attribute(XNAME_Name)?.Value}\" IS NULL";
                        return $"(\"{p.Attribute(XNAME_Name)?.Value}\" IS NOT NULL AND length(trim(\"{disallowEmptyIfNotNull}\")>0)";
                    }).First();
                    string value = $"{p2} OR {p1}";
                    if (constraints.Contains(value))
                        continue;
                    value = $"{p1} OR {p2}";
                    if (!constraints.Contains(value))
                        constraints.Add(value);
                }
                else if (element.Name == XNAME_IsNullSameConstraint)
                {
                    string[] values = element.Elements(XNAME_Property).Attributes(XNAME_Name).Select(a => a.Value).ToArray();
                    string value = $"\"{values[1]}\" IS NULL OR \"{values[0]}\" IS NOT NULL";
                    if (constraints.Contains(value))
                        continue;
                    value = $"\"{values[0]}\" IS NULL OR \"{values[1]}\" IS NOT NULL";
                    if (!constraints.Contains(value))
                        constraints.Add(value);
                }
                else if (element.Name == XNAME_FieldComparisonConstraint)
                {
                    string leftProperty = element.Attribute(XNAME_LeftProperty)?.Value;
                    string rightProperty = element.Attribute(XNAME_RightProperty)?.Value;
                    string value;
                    switch (element.Attribute(XNAME_Operator)?.Value ?? "")
                    {
                        case "LessThan":
                            value = $"\"{rightProperty}\">=\"{leftProperty}\"";
                            if (constraints.Contains(value))
                                continue;
                            value = $"\"{leftProperty}\"<\"{rightProperty}\"";
                            break;
                        case "NotGreaterThan":
                            value = $"\"{rightProperty}\">\"{leftProperty}\"";
                            if (constraints.Contains(value))
                                continue;
                            value = $"\"{leftProperty}\"<=\"{rightProperty}\"";
                            break;
                        case "NotEqualTo":
                            value = $"\"{rightProperty}\"<>\"{leftProperty}\"";
                            if (constraints.Contains(value))
                                continue;
                            value = $"\"{leftProperty}\"<>\"{rightProperty}\"";
                            break;
                        case "NotLessThan":
                            value = $"\"{rightProperty}\"<\"{leftProperty}\"";
                            if (constraints.Contains(value))
                                continue;
                            value = $"\"{leftProperty}\">=\"{rightProperty}\"";
                            break;
                        case "GreaterThan":
                            value = $"\"{rightProperty}\"<=\"{leftProperty}\"";
                            if (constraints.Contains(value))
                                continue;
                            value = $"\"{leftProperty}\">\"{rightProperty}\"";
                            break;
                        default:
                            value = $"\"{rightProperty}\"=\"{leftProperty}\"";
                            if (constraints.Contains(value))
                                continue;
                            value = $"\"{leftProperty}\"=\"{rightProperty}\"";
                            break;
                    }
                    if (!constraints.Contains(value))
                        constraints.Add(value);
                }
            }
            // TODO: Need to change from appending comma/newline in case there are no constraints or key columns
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
                Write("\" PRIMARY KEY(\"");
                Write(keyColumns[0]);
                if (constraints.Count > 0)
                    Write("\")");
                else
                    WriteLine("\")");
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
                if (constraints.Count > 0)
                    Write("\")");
                else
                    WriteLine("\")");
            }
            if (constraints.Count == 1)
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
                Write(constraints[0]);
                WriteLine(")");
            }
            else if (constraints.Count > 1)
            {
                if (string.IsNullOrEmpty(comment))
                    WriteLine(",");
                else
                {
                    Write(", -- ");
                    WriteLine(comment);
                    comment = null;
                }
                Write("CHECK((");
                Write(constraints[0]);
                foreach (string c in constraints.Skip(1))
                {
                    Write(") AND (");
                    Write(c);
                }
                WriteLine("))");
            }
            else if (keyColumns.Length == 0)
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
            foreach ((string Name, LinkedList<XElement> Sources) tuple in columns)
                GenerateTableIndexSql(tableName, entityName, tuple.Name, tuple.Sources);
        }

        string PropertyElementToSqlType(XElement propertyElement, out string typeName, out bool isNumeric)
        {
            if (propertyElement is null || propertyElement.Name.NamespaceName.Length > 0)
            {
                isNumeric = false;
                typeName = "BLOB";
                return typeName;
            }
            switch (propertyElement.Name.LocalName)
            {
                case NAME_Enum:
                    return PropertyElementToSqlType(FindLocalEnumByName(propertyElement.Attribute(XNAME_Type)?.Value), out typeName, out isNumeric);
                case NAME_UniqueIdentifier:
                case NAME_RelatedEntity:
                    isNumeric = false;
                    typeName = "UNIQUEIDENTIFIER";
                    return typeName;
                case NAME_NVarChar:
                    isNumeric = false;
                    typeName = "NVARCHAR";
                    return $"{typeName}({propertyElement.Attribute(XNAME_MaxLength)?.Value})";
                case NAME_VolumeIdentifier:
                    isNumeric = false;
                    typeName = "NVARCHAR";
                    return "NVARCHAR(1024)";
                case NAME_MultiStringValue:
                case NAME_Text:
                    isNumeric = false;
                    typeName = "TEXT";
                    return typeName;
                case NAME_DateTime:
                    isNumeric = false;
                    typeName = "DATETIME";
                    return typeName;
                case NAME_TimeSpan:
                    isNumeric = false;
                    typeName = "TIME";
                    return typeName;
                case NAME_Bit:
                    isNumeric = false;
                    typeName = "BIT";
                    return typeName;
                case NAME_ByteValues:
                    isNumeric = false;
                    typeName = "VARBINARY";
                    return $"{typeName}({propertyElement.Attribute(XNAME_MaxLength)?.Value})";
                case NAME_MD5Hash:
                    isNumeric = false;
                    typeName = "BINARY";
                    return "BINARY(16)";
                case NAME_DriveType:
                    isNumeric = false;
                    typeName = "UNSIGNED TINYINT";
                    return typeName;
                case NAME_Byte:
                    isNumeric = true;
                    typeName = "UNSIGNED TINYINT";
                    return typeName;
                case NAME_SByte:
                    isNumeric = true;
                    typeName = "TINYINT";
                    return typeName;
                case NAME_Short:
                    isNumeric = true;
                    typeName = "SMALLINT";
                    return typeName;
                case NAME_UShort:
                    isNumeric = true;
                    typeName = "UNSIGNED SMALLINT";
                    return typeName;
                case NAME_Int:
                    isNumeric = true;
                    typeName = "INT";
                    return typeName;
                case NAME_UInt:
                    isNumeric = true;
                    typeName = "UNSIGNED INT";
                    return typeName;
                case NAME_Long:
                    isNumeric = true;
                    typeName = "BIGINT";
                    return typeName;
                case NAME_ULong:
                    isNumeric = true;
                    typeName = "UNSIGNED BIGINT";
                    return typeName;
                case NAME_Float:
                case NAME_Double:
                    isNumeric = true;
                    typeName = "REAL";
                    return typeName;
                case NAME_Decimal:
                    isNumeric = true;
                    typeName = "NUMERIC";
                    return typeName;
                default:
                    isNumeric = false;
                    typeName = null;
                    return typeName;
            }
        }

        string GenerateTableColumnSql(string tableName, string entityName, string propertyName, LinkedList<XElement> sources, out string comment)
        {
            XElement baseProperty = sources.Last.Value;
            if (baseProperty.Name == XNAME_CollectionNavigation)
            {
                comment = null;
                return null;
            }
            XElement implProperty = sources.First.Value;
            string propertyTypeName = baseProperty.Name.LocalName;
            string colName = implProperty.Attribute(XNAME_ColName)?.Value ?? propertyName;
            bool isIndexed = sources.Attributes(XNAME_IsIndexed).Any(a => a.Value == "true");
            bool defaultNull = sources.Elements(XNAME_DefaultNull).Any();
            bool allowNull = defaultNull || sources.Attributes(XNAME_AllowNull).Any(a => a.Value == "true");
            Write("\"");
            Write(colName);
            Write("\" ");
            Write(PropertyElementToSqlType(baseProperty, out string typeName, out bool isNumeric));
            if (!allowNull)
                Write(" NOT NULL");

            if (isNumeric)
            {
                string minValue = sources.Attributes(XNAME_MinValue).Select(a => a.Value.Trim()).DefaultIfEmpty("").First();
                string maxValue = sources.Attributes(XNAME_MaxValue).Select(a => a.Value.Trim()).DefaultIfEmpty("").First();
                if (minValue.Length > 0)
                {
                    if (maxValue.Length > 0)
                    {
                        if (allowNull)
                            Write($" CHECK(\"{colName}\" IS NULL OR (\"{colName}\">={minValue} AND \"{colName}\"<={maxValue}))");
                        else
                            Write($" CHECK(\"{colName}\">={minValue} AND \"{colName}\"<={maxValue})");
                    }
                    else if (allowNull)
                        Write($" CHECK(\"{colName}\" IS NULL OR \"{colName}\">={minValue})");
                    else
                        Write($" CHECK(\"{colName}\">={minValue})");
                }
                else if (maxValue.Length > 0)
                {
                    if (allowNull)
                        Write($" CHECK(\"{colName}\" IS NULL OR \"{colName}\"<={maxValue})");
                    else
                        Write($" CHECK(\"{colName}\"<={maxValue})");
                }
            }
            else
            {
                int minLength;
                switch (propertyTypeName)
                {
                    case NAME_NVarChar:
                        minLength = sources.Attributes(XNAME_MinLength).Select(a => XmlConvert.ToInt32(a.Value.Trim())).DefaultIfEmpty(0).First();
                        if (minLength > 0)
                        {
                            if (sources.Attributes(XNAME_IsNormalized).Any(a => a.Value == "true"))
                            {
                                if (allowNull)
                                    Write($" CHECK(\"{colName}\" IS NULL OR (length(trim(\"{colName}\") = length(\"{colName}\") AND length(\"{colName}\")>{minLength - 1}))");
                                else
                                    Write($" CHECK(length(trim(\"{colName}\") = length(\"{colName}\") AND length(\"{colName}\")>{minLength - 1})");
                            }
                            else if (allowNull)
                                Write($" CHECK(\"{colName}\" IS NULL OR length(\"{colName}\")>{minLength - 1})");
                            else
                                Write($" CHECK(length(\"{colName}\")>{minLength - 1})");
                        }
                        else if (sources.Attributes(XNAME_IsNormalized).Any(a => a.Value == "true"))
                        {
                            if (allowNull)
                                Write($" CHECK(\"{colName}\" IS NULL OR length(trim(\"{colName}\") = length(\"{colName}\"))");
                            else
                                Write($" CHECK(length(trim(\"{colName}\") = length(\"{colName}\"))");
                        }
                        break;
                    case NAME_ByteValues:
                        minLength = sources.Attributes(XNAME_MinLength).Select(a => XmlConvert.ToInt32(a.Value.Trim())).DefaultIfEmpty(0).First();
                        int maxLength = sources.Attributes(XNAME_MaxLength).Select(a => XmlConvert.ToInt32(a.Value.Trim())).DefaultIfEmpty(0).First();
                        if (minLength > 0)
                        {
                            if (maxLength > 0)
                            {
                                if (allowNull)
                                    Write($" CHECK(\"{colName}\" IS NULL OR (length(\"{colName}\")<{maxLength + 1} AND length(\"{colName}\")>{minLength - 1}))");
                                else
                                    Write($" CHECK(length(\"{colName}\")<{maxLength + 1} AND length(\"{colName}\")>{minLength - 1})");
                            }
                            else if (allowNull)
                                Write($" CHECK(\"{colName}\" IS NULL OR length(\"{colName}\")>{minLength - 1})");
                            else
                                Write($" CHECK(length(\"{colName}\")>{minLength - 1})");
                        }
                        else if (maxLength > 0)
                        {
                            if (allowNull)
                                Write($" CHECK(\"{colName}\" IS NULL OR length(\"{colName}\")<{maxLength + 1})");
                            else
                                Write($" CHECK(length(\"{colName}\")<{maxLength + 1})");
                        }
                        break;
                    case NAME_RelatedEntity:
                        XElement parentEntityElement = FindLocalEntityByName(sources.Elements(XNAME_Reference).Select(e => e.Value).First());
                        Write("CONSTRAINT \"FK_");
                        Write(typeName);
                        Write(parentEntityElement.Attribute(XNAME_Name)?.Value);
                        Write("\" REFERENCES \"");
                        Write(parentEntityElement.Attribute(XNAME_TableName)?.Value);
                        Write("\"(\"Id\") ON DELETE RESTRICT");
                        break;
                }
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
                            Write(FindLocalFieldByFullName(defaultValue.Trim())?.Value);
                            comment = defaultValue.Trim();
                            break;
                        case NAME_Char:
                        case NAME_NVarChar:
                        case NAME_VolumeIdentifier:
                        case NAME_MultiStringValue:
                        case NAME_Text:
                            comment = null;
                            Write(" DEFAULT '");
                            Write($"'{defaultValue.Replace("'", "''")}'");
                            Write("'");
                            break;
                        case NAME_TimeSpan:
                            comment = null;
                            Write(" DEFAULT ");
                            Write(XmlConvert.ToTimeSpan(defaultValue.Trim()).ToString(@"\'hh\:mm\:ss\.fff\'"));
                            break;
                        case NAME_DateTime:
                            comment = null;
                            Write(XmlConvert.ToDateTime(defaultValue.Trim(), XmlDateTimeSerializationMode.RoundtripKind).ToLocalTime().ToString(@"'yyyy-MM-dd HH:mm:ss"));
                            break;
                        case NAME_Bit:
                            comment = null;
                            Write((defaultValue == "true") ? " DEFAULT 1," : " DEFAULT 0");
                            break;
                        case NAME_ByteValues:
                            comment = null;
                            Write(" DEFAULT X'");
                            Write(BitConverter.ToString(Convert.FromBase64String(defaultValue.Trim())));
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
            if (sources.Attributes(XNAME_IsUnique).Any(a => a.Value == "true"))
                Write(" UNIQUE");
            switch (propertyTypeName)
            {
                case NAME_RelatedEntity:
                case NAME_UniqueIdentifier:
                case NAME_VolumeIdentifier:
                    Write(" COLLATE NOCASE");
                    break;
                case NAME_NVarChar:
                    XAttribute attribute = sources.Attributes(XNAME_IsUnique).FirstOrDefault();
                    if (attribute is not null)
                    {
                        Write(" COLLATE ");
                        Write(attribute.Value);
                    }
                    break;
            }
            return colName;
        }

        void GenerateTableIndexSql(string tableName, string entityName, string colName, LinkedList<XElement> sources)
        {
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
        record ForeignKeyConstraintOld(string Name, string TableName, string KeyName) : IColumnConstraint;
        record RangeColumnConstraintOld(CodeValues MaxValue, CodeValues MinValue) : IColumnConstraint;
        record LengthColumnConstraintOld(CodeValues MaxLength, CodeValues MinLength) : IColumnConstraint;
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

        //EntityDefinition GetEntityDefinition(XElement entityElement)
        //{
        //    EntityDefinition entityDefinition = entityElement.Annotation<EntityDefinition>();
        //    if (entityDefinition is not null)
        //        return entityDefinition;
        //    XElement parentElement;
        //    if (entityElement is null || entityElement?.Name != XNAME_Entity || (parentElement = entityElement?.Parent) is null)
        //        return null;
        //    Func<string, XElement> findEntityByName;
        //    if (parentElement.Name == XNAME_Root)
        //        findEntityByName = FindRootEntityByName;
        //    else if (parentElement.Name == XNAME_Local)
        //        findEntityByName = FindLocalEntityByName;
        //    else if (parentElement.Name == XNAME_Upstream)
        //        findEntityByName = FindUpstreamByName;
        //    else
        //        return null;
        //    IEnumerable<(string Name, EntityDefinition Definition)> baseTypes = entityElement.Elements(XNAME_ExtendsEntity)
        //        .Select<XElement, (string Name, EntityDefinition Definition)>(e =>
        //        {
        //            XElement element = findEntityByName(e.Attribute(XNAME_Type)?.Value);
        //            if (element is null)
        //                throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_Type)?.Value}\" at {ToXPath(e)}");
        //            return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //        }).Concat(entityElement.Elements(XNAME_ExtendsGenericEntity)
        //        .Select<XElement, (string Name, EntityDefinition Definition)>(e =>
        //    {
        //        XElement element = findEntityByName(e.Attribute(XNAME_TypeDef)?.Value);
        //        if (element is null)
        //            throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_TypeDef)?.Value}\" at {ToXPath(e)}");
        //        return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //    })).Concat(entityElement.Elements().Select<XElement, (string Name, EntityDefinition Definition)>(e =>
        //    {
        //        XElement element;
        //        if (e.Name == XNAME_Implements)
        //            return new(e.Attribute(XNAME_Type)?.Value, null);
        //        if (e.Name == XNAME_ImplementsEntity)
        //        {
        //            element = findEntityByName(e.Attribute(XNAME_Type)?.Value);
        //            if (element is null)
        //                throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_Type)?.Value}\" at {ToXPath(e)}");
        //            return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //        }
        //        if (e.Name != XNAME_ImplementsGenericEntity)
        //            return new (null, null);
        //        element = findEntityByName(e.Attribute(XNAME_TypeDef)?.Value);
        //        if (element is null)
        //            throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_TypeDef)?.Value}\" at {ToXPath(e)}");
        //        return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //    }).Where(t => t.Name is not null));
        //    string rootInterface = entityElement.Attribute(XNAME_RootInterface)?.Value;
        //    if (!string.IsNullOrWhiteSpace(rootInterface))
        //    {
        //        XElement element = findEntityByName(rootInterface);
        //        if (element is null)
        //            throw new InvalidDataException($"Could not find element \"{rootInterface}\" at {ToXPath(entityElement)}");
        //        baseTypes = new (string Name, EntityDefinition Definition)[] { new(rootInterface, GetEntityDefinition(element)) }.Concat(baseTypes);
        //    }
        //    ReadOnlyCollection<EntityDefinition> baseDefinitions = new(baseTypes.Select(b => b.Definition).Where(d => d is not null).ToArray());
        //    ReadOnlyCollection<PropertyDefinition> properties = new(entityElement.Elements(XNAME_Properties).Elements()
        //        .Select(p => GetPropertyDefinition(p, baseDefinitions)).ToArray());
        //    TableDefinition tableDefinition = GetTableDefinition(entityElement, baseDefinitions, properties);
        //    entityDefinition = new(entityElement.Attribute(XNAME_Name)?.Value.Replace("{", "<").Replace("}", ">"), tableDefinition,
        //        properties, baseDefinitions, new ReadOnlyCollection<string>(baseTypes.Select(b => b.Name).ToArray()), entityElement);
        //    entityElement.AddAnnotation(entityDefinition);
        //    return entityDefinition;
        //}

        //private TableDefinition GetTableDefinition(XElement entityElement, ReadOnlyCollection<EntityDefinition> baseDefinitions, ReadOnlyCollection<PropertyDefinition> properties)
        //{
        //    TableDefinition tableDefinition = entityElement.Annotation<TableDefinition>();
        //    if (tableDefinition is not null)
        //        return tableDefinition;
        //    string tableName = entityElement.Attribute(XNAME_TableName)?.Value;
        //    if (tableName is null)
        //        return null;
        //    Collection<IColumnDefinition> columns = new();
        //    foreach (PropertyDefinition pd in properties)
        //    {
        //        IColumnDefinition columnDefinition = GetColumnDefinition(pd, baseDefinitions);
        //        if (columnDefinition is not null)
        //            columns.Add(columnDefinition);
        //    }
        //    tableDefinition = new TableDefinition(tableName, new ReadOnlyCollection<IColumnDefinition>(columns), entityElement);
        //    entityElement.AddAnnotation(tableDefinition);
        //    return tableDefinition;
        //}

        //private IColumnDefinition GetColumnDefinition(PropertyDefinition pd, ReadOnlyCollection<EntityDefinition> baseDefinitions)
        //{
        //    IColumnDefinition columnDefinition = pd.Source.Annotation<IColumnDefinition>();
        //    if (columnDefinition is not null)
        //        return columnDefinition;
        //    string propertyName = pd.Source.Attribute(XNAME_ColName)?.Value ?? pd.Source.Attribute(XNAME_Name)?.Value;
        //    SqlColType? sqlType = ToSqlSqlColType(pd.Source, out string sqlExpr);
        //    if (!sqlType.HasValue || string.IsNullOrWhiteSpace(propertyName))
        //        return null;
        //    switch (sqlType.Value)
        //    {
        //        case SqlColType.UNSIGNED_TINYINT:
        //        case SqlColType.TINYINT:
        //        case SqlColType.UNSIGNED_SMALLINT:
        //        case SqlColType.SMALLINT:
        //        case SqlColType.UNSIGNED_INT:
        //        case SqlColType.INT:
        //        case SqlColType.BIGINT:
        //        case SqlColType.UNSIGNED_BIGINT:
        //        case SqlColType.BINARY:
        //        case SqlColType.NVARCHAR:
        //        case SqlColType.CHARACTER:
        //        case SqlColType.REAL:
        //        case SqlColType.NUMERIC:
        //            break;
        //        case SqlColType.DATETIME:
        //        case SqlColType.TIME:
        //        case SqlColType.UNIQUEIDENTIFIER:
        //        case SqlColType.BIT:
        //        case SqlColType.TEXT:
        //        case SqlColType.BLOB:
        //        case SqlColType.NULL:
        //            break;
        //        default:
        //            break;
        //    }
        //    throw new NotImplementedException();
        //}

        //PropertyDefinition GetPropertyDefinition(XElement propertyElement, IReadOnlyCollection<EntityDefinition> baseDefinitions)
        //{
        //    string propertyName = propertyElement.Attribute(XNAME_Name)?.Value;
        //    ReadOnlyCollection<PropertyDefinition> baseProperties = new(baseDefinitions.SelectMany(d => d.Properties.Where(p => p.Name == propertyName)).ToArray());

        //    PropertyDefinition propertyDefinition = propertyElement.Annotation<PropertyDefinition>();
        //    if (propertyDefinition is not null)
        //        return propertyDefinition;
        //    bool allowsNull = propertyElement.Attributes(XNAME_AllowNull).Any(a => a.Value == "true") || propertyElement.Elements(XNAME_DefaultNull).Any() ||
        //            baseProperties.Any(p => p.AllowsNull);
        //    bool isGenericWritable = propertyElement.Attributes(XNAME_IsGenericWritable).Any(a => a.Value == "true") || baseProperties.Any(p => p.IsGenericWritable);
        //    switch (propertyElement.Name.LocalName)
        //    {
        //        case NAME_Byte:
        //        case NAME_SByte:
        //        case NAME_Short:
        //        case NAME_UShort:
        //        case NAME_Int:
        //        case NAME_UInt:
        //        case NAME_Long:
        //        case NAME_ULong:
        //        case NAME_Float:
        //        case NAME_Double:
        //        case NAME_Decimal:
        //        case NAME_Char:
        //            propertyDefinition = new PropertyDefinition(propertyName,
        //                (allowsNull ? $"{propertyElement.Name.LocalName}?" : propertyElement.Name.LocalName).ToLower(),
        //                allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_ByteArray:
        //            propertyDefinition = new PropertyDefinition(propertyName, "byte[]", allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_Text:
        //        case NAME_NVarChar:
        //            propertyDefinition = new PropertyDefinition(propertyName, "string", allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_VolumeIdentifier:
        //        case NAME_DriveType:
        //        case NAME_MD5Hash:
        //        case NAME_DateTime:
        //        case NAME_TimeSpan:
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? $"{propertyElement.Name.LocalName}?" : propertyElement.Name.LocalName,
        //                allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_ByteValues:
        //        case NAME_MultiStringValue:
        //            propertyDefinition = new PropertyDefinition(propertyName, propertyElement.Name.LocalName, allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_UniqueIdentifier:
        //        case NAME_NewIdNavRef:
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? "Guid?" : "Guid", allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_Bit:
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? "bool?" : "bool", allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_Enum:
        //            string tn = propertyElement.Attribute(XNAME_Type)?.Value;
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? $"{tn}?" : tn, allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_CollectionNavigation:
        //        case NAME_NewCollectionNavigation:
        //            propertyDefinition = new PropertyDefinition(propertyName, $"IEnumerable<{propertyElement.Attribute(XNAME_ItemType)?.Value}>", allowsNull,
        //                isGenericWritable,
        //                baseProperties, propertyElement);
        //            break;
        //        case NAME_RelatedEntity:
        //        case NAME_NewRelatedEntity:
        //            propertyDefinition = new PropertyDefinition(propertyName,
        //                (propertyElement.Attribute(XNAME_TypeDef)?.Value ?? propertyElement.Attribute(XNAME_Reference)?.Value), allowsNull, isGenericWritable,
        //                baseProperties, propertyElement);
        //            break;
        //        default:
        //            propertyDefinition = new PropertyDefinition(propertyName, "object", allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //    }
        //    propertyElement.AddAnnotation(propertyDefinition);
        //    return propertyDefinition;
        //}
        /*

foreach (XElement entityElement in entityDefinitionsElement.Elements().Elements(XNAME_Entity))
{
string typeName = entityElement.Attribute(XNAME_Name)?.Value;
if (string.IsNullOrWhiteSpace(typeName))
continue;

foreach (XElement propertyElement in entityElement.Elements(XNAME_Field))
{
XName elementName = propertyElement.Name;
string propertyName = propertyElement.Attribute(XNAME_Name)?.Value;
if (string.IsNullOrWhiteSpace(propertyName))
   continue;
string expected = $"{typeName}.{propertyName}";
XAttribute attribute;
switch (propertyElement.Name)
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
   case NAME_Decimal:
       if ((attribute = propertyElement.Attribute(XNAME_MaxValue)) is not null)
           WriteLine("")
       //(@MaxValue|@MinValue|Default)
       break;
   case NAME_NVarChar:
   case NAME_ByteValues:
   case NAME_ByteArray:
       //(@MaxLength|@MinLength|Default)
       break;
   case NAME_Char:
   case NAME_Text:
   case NAME_Bit:
   case NAME_VolumeIdentifier:
   case NAME_DateTime:
   case NAME_DriveType:
   case NAME_Enum:
       //Default
       break;
   case NAME_TimeSpan:
       //(Default|DefaultZero)
       break;
}
}
}
*/
        //IEnumerable<XElement> GetBaseEntities(XElement entityElement)
        //{
        //    XElement parent = entityElement?.Parent;
        //    if (parent is null || entityElement?.Name != XNAME_Entity)
        //        return Array.Empty<XElement>();

        //    IEnumerable<string> names = entityElement.Elements().Select(e => (
        //        IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
        //        IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
        //        Element: e
        //    )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
        //    return parent.Name.LocalName switch
        //    {
        //        NAME_Root => names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null),
        //        NAME_Local => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
        //            .Select(n => FindLocalEntityByName(n)).Where(e => e is not null),
        //        NAME_Upstream => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
        //            .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null),
        //        _ => Array.Empty<XElement>(),
        //    };
        //}

        //void GetAllProperties(XElement entityElement, Collection<(string Name, LinkedList<XElement> Sources)> collection)
        //{
        //    foreach (XElement baseEntity in GetBaseEntities(entityElement).Reverse())
        //        GetAllProperties(baseEntity, collection);
        //    foreach (XAttribute attribute in entityElement.Elements(XNAME_Properties).Elements().Attributes(XNAME_Name))
        //    {
        //        XElement propertyElement = attribute.Parent;
        //        string propertyName = attribute.Value;
        //        if (collection.Any(t => t.Name == propertyName))
        //        {
        //            (string Name, LinkedList<XElement> Sources) property = collection.First(t => t.Name == propertyName);
        //            if (!property.Sources.Any(e => ReferenceEquals(e, propertyElement)))
        //                property.Sources.AddFirst(attribute.Parent);
        //        }
        //        else
        //        {
        //            (string Name, LinkedList<XElement> Sources) property = new(propertyName, new LinkedList<XElement>());
        //            property.Sources.AddLast(attribute.Parent);
        //            collection.Add(property);
        //        }
        //    }
        //}

        //void GetAllBaseEntities(XElement entityElement, int level, Collection<(XElement Element, int Level)> collection, Func<IEnumerable<string>, IEnumerable<XElement>> getEntities)
        //{
        //    IEnumerable<string> names = entityElement.Elements().Select(e => (
        //        IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
        //        IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
        //        Element: e
        //    )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
        //    int nextLevel = level + 1;
        //    foreach (XElement baseEntity in getEntities(names))
        //    {
        //        IEnumerable<(XElement Element, int Level)> items = collection.Where(e => ReferenceEquals(e.Element, baseEntity));
        //        if (items.Any())
        //        {
        //            (XElement Element, int Level) t = items.First();
        //            if (level < t.Level)
        //            {
        //                collection.Remove(t);
        //                collection.Add(new(baseEntity, level));
        //            }
        //        }
        //        else
        //        {
        //            collection.Add(new(baseEntity, level));
        //            GetAllBaseEntities(baseEntity, nextLevel, collection, getEntities);
        //        }
        //    }
        //}

        //XElement[] GetAllBaseEntities(XElement entityElement)
        //{
        //    XElement parent = entityElement?.Parent;
        //    if (parent is null || entityElement?.Name != XNAME_Entity)
        //        return Array.Empty<XElement>();
        //    Collection<(XElement Element, int Level)> result = new();
        //    Func<IEnumerable<string>, IEnumerable<XElement>> getEntities;
        //    switch (parent.Name.LocalName)
        //    {
        //        case NAME_Root:
        //            getEntities = names => names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null);
        //            break;
        //        case NAME_Local:
        //            getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
        //                .Select(n => FindLocalEntityByName(n)).Where(e => e is not null);
        //            break;
        //        case NAME_Upstream:
        //            getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
        //                .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null);
        //            break;
        //        default:
        //            return Array.Empty<XElement>();
        //    }
        //    GetAllBaseEntities(entityElement, 0, result, getEntities);
        //    return result.OrderBy(t => t.Level).Select(t => t.Element).ToArray();
        //}

        //static XElement[] GetAllBaseProperties(XElement propertyElement, XElement[] orderedBaseEntities, out XElement baseProperty, out bool isNew, out bool doNotEmit)
        //{
        //    string propertyName = propertyElement?.Attribute(XNAME_Name)?.Value;
        //    if (propertyName is null || orderedBaseEntities is null || orderedBaseEntities.Length == 0)
        //    {
        //        baseProperty = propertyElement;
        //        isNew = doNotEmit = false;
        //        return Array.Empty<XElement>();
        //    }
        //    XName baseName;
        //    XName inheritedName;
        //    switch (propertyElement.Name.LocalName)
        //    {
        //        case NAME_NewIdNavRef:
        //            isNew = false;
        //            doNotEmit = true;
        //            baseName = XNAME_UniqueIdentifier;
        //            inheritedName = XNAME_NewIdNavRef;
        //            break;
        //        case NAME_NewRelatedEntity:
        //            isNew = true;
        //            doNotEmit = false;
        //            baseName = XNAME_RelatedEntity;
        //            inheritedName = XNAME_NewRelatedEntity;
        //            break;
        //        case NAME_NewCollectionNavigation:
        //            isNew = true;
        //            doNotEmit = false;
        //            baseName = XNAME_CollectionNavigation;
        //            inheritedName = XNAME_NewCollectionNavigation;
        //            break;
        //        default:
        //            isNew = doNotEmit = false;
        //            baseProperty = propertyElement;
        //            return orderedBaseEntities.Elements(XNAME_Properties).Elements(propertyElement.Name).Attributes(XNAME_Name).Where(a => a.Value == propertyName).Select(a => a.Parent).ToArray();
        //    }
        //    IEnumerable<XElement> results = orderedBaseEntities.Elements(XNAME_Properties).Elements().Where(e => e.Name == baseName || e.Name == inheritedName)
        //        .Attributes(XNAME_Name).Where(a => a.Value == propertyName).Select(a => a.Parent);
        //    baseProperty = results.Where(e => e.Name == baseName).DefaultIfEmpty(propertyElement).First();
        //    return results.ToArray();
        //}

        const string NAME_PrimaryKey = "PrimaryKey";
        const string NAME_ForeignKey = "ForeignKey";
        public static readonly XName XNAME_PrimaryKey = XName.Get(NAME_PrimaryKey);
        public static readonly XName XNAME_ForeignKey = XName.Get(NAME_ForeignKey);

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
