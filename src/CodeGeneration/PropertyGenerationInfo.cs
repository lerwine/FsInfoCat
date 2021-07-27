using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static CodeGeneration.Constants;

namespace CodeGeneration
{
    public class PropertyGenerationInfo : IMemberGenerationInfo
    {
        private PropertyGenerationInfo(XElement propertyElement, EntityGenerationInfo entity)
        {
            Name = (Source = propertyElement).Attribute(NAME_Name)?.Value;
            DeclaringType = entity;
            Inherited = new(entity.BaseTypes.SelectMany(e => e.Entity.Properties.Where(p => p.Name == Name)).Distinct().OrderBy(p => p, new ProxyValueComparer<PropertyGenerationInfo>((x, y) =>
                x.DeclaringType.Extends(y.DeclaringType) ? -1 : y.DeclaringType.Extends(x.DeclaringType) ? 1 : 0)).ToArray());
            ColumnName = propertyElement.AttributeToString(NAME_ColName) ?? Inherited.Select(p => p.Source).AttributeToString(NAME_ColName);
            bool defaultNull = propertyElement.Elements(XNAME_DefaultNull).Any() || Inherited.Select(i => i.Source).Elements(XNAME_DefaultNull).Any();
            IndexName = propertyElement.Elements(XNAME_Index).AttributeToString(XNAME_Name) ?? Inherited.Select(p => p.Source).Elements(XNAME_Index).AttributeToString(XNAME_Name);
            AllowNull = defaultNull || (propertyElement.AttributeToBoolean(XNAME_AllowNull) ?? Inherited.Select(p => p.Source).AttributeToBoolean(XNAME_AllowNull) ?? false);
            IsGenericWritable = propertyElement.AttributeToBoolean(XNAME_IsGenericWritable) ?? Inherited.Select(p => p.Source).AttributeToBoolean(XNAME_IsGenericWritable) ?? false;
            if (propertyElement.Name.Namespace != XNAME_UniqueIdentifier.Namespace)
            {
                Type = DbType.Object;
                CsTypeName = "object";
                SqlTypeName = SQL_TYPENAME_BLOB;
                ColumnName = null;
            }
            else
            {
                string name, fullName;
                XElement element;
                IEnumerable<XElement> allPropertyElements = Enumerable.Repeat(propertyElement, 1).Concat(Inherited.Select(i => i.Source));
                switch (propertyElement.Name.LocalName)
                {
                    case NAME_Bit:
                        Type = DbType.Boolean;
                        CsTypeName = "bool";
                        SqlTypeName = SQL_TYPENAME_BIT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToBoolean(XNAME_Default));
                        break;
                    case NAME_Byte:
                        Type = DbType.Byte;
                        CsTypeName = "byte";
                        SqlTypeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToByte(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToByte(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToByte(XNAME_MaxValue));
                        break;
                    case NAME_SByte:
                        Type = DbType.SByte;
                        CsTypeName = "sbyte";
                        SqlTypeName = SQL_TYPENAME_TINYINT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToSByte(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToSByte(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToSByte(XNAME_MaxValue));
                        break;
                    case NAME_Short:
                        Type = DbType.Int16;
                        CsTypeName = "short";
                        SqlTypeName = SQL_TYPENAME_SMALLINT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToInt16(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToInt16(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToInt16(XNAME_MaxValue));
                        break;
                    case NAME_UShort:
                        Type = DbType.UInt16;
                        CsTypeName = "ushort";
                        SqlTypeName = SQL_TYPENAME_UNSIGNED_SMALLINT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToUInt16(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToUInt16(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToUInt16(XNAME_MaxValue));
                        break;
                    case NAME_Int:
                        Type = DbType.Int32;
                        CsTypeName = "int";
                        SqlTypeName = SQL_TYPENAME_INT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToInt32(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToInt32(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToInt32(XNAME_MaxValue));
                        break;
                    case NAME_UInt:
                        Type = DbType.UInt32;
                        CsTypeName = "uint";
                        SqlTypeName = SQL_TYPENAME_UNSIGNED_INT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToUInt32(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToUInt32(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToUInt32(XNAME_MaxValue));
                        break;
                    case NAME_Long:
                        Type = DbType.Int64;
                        CsTypeName = "long";
                        SqlTypeName = SQL_TYPENAME_BIGINT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToInt64(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToInt64(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToInt64(XNAME_MaxValue));
                        break;
                    case NAME_ULong:
                        Type = DbType.UInt64;
                        CsTypeName = "ulong";
                        SqlTypeName = SQL_TYPENAME_UNSIGNED_BIGINT;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToUInt64(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToUInt64(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToUInt64(XNAME_MaxValue));
                        break;
                    case NAME_Float:
                        Type = DbType.Single;
                        CsTypeName = "float";
                        SqlTypeName = SQL_TYPENAME_REAL;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToSingle(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToSingle(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToSingle(XNAME_MaxValue));
                        break;
                    case NAME_Double:
                        Type = DbType.Double;
                        CsTypeName = "double";
                        SqlTypeName = SQL_TYPENAME_REAL;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToDouble(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToDouble(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToDouble(XNAME_MaxValue));
                        break;
                    case NAME_Decimal:
                        Type = DbType.Decimal;
                        CsTypeName = "decimal";
                        SqlTypeName = SQL_TYPENAME_NUMERIC;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToDecimal(XNAME_Default));
                        MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToDecimal(XNAME_MinValue));
                        MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.AttributeToDecimal(XNAME_MaxValue));
                        break;
                    case NAME_Char:
                        Type = DbType.StringFixedLength;
                        CsTypeName = "char";
                        SqlTypeName = $"{SQL_TYPENAME_CHARACTER}(1)";
                        MinLength = MaxLength = 1;
                        DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToChar(XNAME_Default));
                        break;
                    case NAME_Enum:
                        CsTypeName = propertyElement.Attribute(XNAME_Type)?.Value;
                        EnumGenerationInfo enumGenerationInfo = EnumGenerationInfo.Get(propertyElement.FindEnumTypeByName(CsTypeName));
                        _referencedType = enumGenerationInfo;
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
                    case NAME_NewIdNavRef:
                        Type = DbType.Guid;
                        CsTypeName = nameof(Guid);
                        SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                        if (defaultNull)
                            DefaultValue = PropertyValueCode.Null;
                        IsCaseSensitive = false;
                        IsNormalized = true;
                        IsPrimaryKey = allPropertyElements.AttributeToBoolean(XNAME_IsPrimaryKey) ?? false;
                        NavigationProperty = allPropertyElements.Attributes(XNAME_Navigation).Select(a => a.Value).FirstOrDefault();
                        PropertyGenerationInfo re = entity.Properties.FirstOrDefault(p => p.DbRelationship?.FkPropertyName == Name);
                        if (re is not null)
                            re.ColumnName = null;
                        break;
                    case NAME_RelatedEntity:
                    case NAME_NewRelatedEntity:
                    case NAME_NewRelatedEntityKey:
                        Type = DbType.Guid;
                        CsTypeName = nameof(Guid);
                        SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                        if (defaultNull)
                            DefaultValue = PropertyValueCode.Null;
                        IsCaseSensitive = false;
                        IsNormalized = true;
                        DbRelationship = (allPropertyElements.Elements(XNAME_DbRelationship).ElementToString(XNAME_Name), allPropertyElements.Elements(XNAME_DbRelationship).ElementToString(XNAME_FkPropertyName), false);
                        name = DbRelationship?.FkPropertyName;
                        if (string.IsNullOrEmpty(name))
                            ColumnName = null;
                        else if (!entity.Properties.Any(p => p.Name == name))
                            ColumnName = name;
                        IsOneToOne = allPropertyElements.AttributeToBoolean(XNAME_IsOneToOne) ?? false;
                        break;
                    case NAME_NVarChar:
                        Type = DbType.String;
                        CsTypeName = "string";
                        MaxLength = propertyElement.AttributeToInt32(XNAME_MaxLength);
                        MinLength = propertyElement.AttributeToInt32(XNAME_MinLength);
                        SqlTypeName = $"{SQL_TYPENAME_NVARCHAR}({MaxLength})";
                        DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToString(XNAME_Default));
                        IsNormalized = propertyElement.AttributeToBoolean(XNAME_IsNormalized) ?? Inherited.Select(i => i.Source).AttributeToBoolean(XNAME_IsNormalized);
                        IsCaseSensitive = propertyElement.AttributeToBoolean(XNAME_IsCaseSensitive) ?? Inherited.Select(i => i.Source).AttributeToBoolean(XNAME_IsCaseSensitive);
                        break;
                    case NAME_VolumeIdentifier:
                        Type = DbType.String;
                        CsTypeName = "VolumeIdentifier";
                        MaxLength = 1024;
                        SqlTypeName = $"{SQL_TYPENAME_NVARCHAR}({MaxLength})";
                        DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToString(XNAME_Default));
                        IsCaseSensitive = false;
                        IsNormalized = true;
                        break;
                    case NAME_MultiStringValue:
                        Type = DbType.String;
                        CsTypeName = "MultiStringValue";
                        SqlTypeName = SQL_TYPENAME_TEXT;
                        DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToString(XNAME_Default));
                        break;
                    case NAME_Text:
                        Type = DbType.String;
                        CsTypeName = "string";
                        SqlTypeName = SQL_TYPENAME_TEXT;
                        DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToString(XNAME_Default));
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
                            DefaultValue = PropertyValueCode.Of(allPropertyElements.ElementToDateTime(XNAME_Default));
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
                            DefaultValue = PropertyValueCode.Of(allPropertyElements.ElementToTimeSpan(XNAME_Default));
                        break;
                    case NAME_ByteValues:
                        Type = DbType.Binary;
                        MaxLength = propertyElement.AttributeToInt32(XNAME_MaxLength);
                        MinLength = propertyElement.AttributeToInt32(XNAME_MinLength);
                        CsTypeName = "byte[]";
                        SqlTypeName = $"{SQL_TYPENAME_VARBINARY}({MaxLength})";
                        DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(allPropertyElements.ElementToBinary(XNAME_Default));
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
                            DefaultValue = PropertyValueCode.Of(allPropertyElements.AttributeToDriveType(XNAME_Default));
                        break;
                    case NAME_CollectionNavigation:
                        Type = DbType.Object;
                        name = allPropertyElements.ElementToString(NAME_ItemType);
                        element = propertyElement.FindEntityByName(name);
                        if (element is null)
                        {
                            fullName = allPropertyElements.ElementToString(NAME_ItemKey);
                            DbRelationship = (propertyElement.FindPropertyByFullName(fullName)?.Parent?.Parent?.ElementToString(XNAME_Name), fullName, true);
                        }
                        else
                            DbRelationship = (name, $"{name}.Id", true);
                        CsTypeName = $"IEnumerable<{DbRelationship.Value.Name}>";
                        SqlTypeName = SQL_TYPENAME_BLOB;
                        ColumnName = null;
                        break;
                    case NAME_NewCollectionNavigation:
                        Type = DbType.Object;
                        name = allPropertyElements.ElementToString(NAME_ItemType);
                        element = propertyElement.FindEntityByName(name);
                        if (element is null)
                        {
                            fullName = allPropertyElements.ElementToString(NAME_ItemKey);
                            DbRelationship = (propertyElement.FindPropertyByFullName(fullName)?.Parent?.Parent?.ElementToString(XNAME_Name), fullName, true);
                        }
                        else
                            DbRelationship = (name, $"{name}.Id", true);
                        CsTypeName = $"IEnumerable<{DbRelationship.Value.Name}>";
                        SqlTypeName = SQL_TYPENAME_BLOB;
                        ColumnName = null;
                        break;
                    default:
                        Type = DbType.Object;
                        CsTypeName = "object";
                        SqlTypeName = SQL_TYPENAME_BLOB;
                        break;
                }
            }
        }
        private ITypeGenerationInfo _referencedType;
        public string Name { get; }
        public string ColumnName { get; private set; }
        public DbType Type { get; }
        public string CsTypeName { get; }
        public string SqlTypeName { get; }
        public int? MaxLength { get; }
        public int? MinLength { get; }
        public PropertyValueCode? MaxValue { get; }
        public PropertyValueCode? MinValue { get; }
        public PropertyValueCode? DefaultValue { get; }
        public bool? IsNormalized { get; }
        public (string Name, string FkPropertyName, bool IsMany)? DbRelationship { get; }
        public bool IsPrimaryKey { get; }
        public string NavigationProperty { get; }
        public bool? IsCaseSensitive { get; }
        public ReadOnlyCollection<PropertyGenerationInfo> Inherited { get; }
        public ITypeGenerationInfo ReferencedType
        {
            get
            {
                // TODO: Look up referenced type if it is null and DbRelationship.Name is not null or NavigationProperty is not null
                return _referencedType;
            }
        }
        public EntityGenerationInfo DeclaringType { get; }
        ITypeGenerationInfo IMemberGenerationInfo.DeclaringType => DeclaringType;
        public XElement Source { get; }
        public string IndexName { get; }
        public bool AllowNull { get; }
        public bool IsGenericWritable { get; }
        public bool? IsOneToOne { get; }

        public static PropertyGenerationInfo Get(XElement propertyElement)
        {
            if (propertyElement is null)
                return null;
            EntityGenerationInfo entityGenerationInfo = EntityGenerationInfo.Get(propertyElement.Parent);
            PropertyGenerationInfo result = propertyElement.Annotation<PropertyGenerationInfo>();
            if (result is not null)
                return result;
            if (propertyElement.IsPropertyElement() && entityGenerationInfo is not null)
                return new PropertyGenerationInfo(propertyElement, entityGenerationInfo);
            return null;
        }
    }
}
