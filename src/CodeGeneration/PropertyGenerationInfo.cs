using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static CodeGeneration.CgConstants;

namespace CodeGeneration
{
    public class PropertyGenerationInfo : IMemberGenerationInfo
    {
        private ITypeGenerationInfo _referencedType;

        private PropertyGenerationInfo(XElement propertyElement, EntityGenerationInfo entity)
        {
            Name = (Source = propertyElement).Attribute(NAME_Name)?.Value;
            DeclaringType = entity;
            Inherited = new(entity.BaseTypes.SelectMany(e => e.Entity.Properties.Where(p => p.Name == Name)).Distinct().OrderBy(p => p, new ProxyValueComparer<PropertyGenerationInfo>((x, y) =>
                x.DeclaringType.Extends(y.DeclaringType) ? -1 : y.DeclaringType.Extends(x.DeclaringType) ? 1 : 0)).ToArray());
            bool defaultNull = propertyElement.Elements(XNAME_DefaultNull).Any() || Inherited.Select(i => i.Source).Elements(XNAME_DefaultNull).Any();
            IndexName = propertyElement.Elements(XNAME_Index).AttributeToString(XNAME_Name) ?? Inherited.Select(p => p.Source).Elements(XNAME_Index).AttributeToString(XNAME_Name);
            AllowNull = defaultNull || (propertyElement.AttributeToBoolean(XNAME_AllowNull) ?? Inherited.Select(p => p.Source).AttributeToBoolean(XNAME_AllowNull) ?? false);
            IsGenericWritable = propertyElement.AttributeToBoolean(XNAME_IsGenericWritable) ?? Inherited.Select(p => p.Source).AttributeToBoolean(XNAME_IsGenericWritable) ?? false;
            if (propertyElement.Name.Namespace != XNAME_UniqueIdentifier.Namespace)
            {
                Type = DbType.Object;
                CsTypeName = "object";
                SqlTypeName = SQL_TYPENAME_BLOB;
                return;
            }
            string name, colName;
            XElement element;
            IEnumerable<XElement> currentAndInheritedProperties = Enumerable.Repeat(propertyElement, 1).Concat(Inherited.Select(i => i.Source));
            switch (propertyElement.Name.LocalName)
            {
                case NAME_Bit:
                    Type = DbType.Boolean;
                    CsTypeName = "bool";
                    SqlTypeName = SQL_TYPENAME_BIT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToBoolean(XNAME_Default));
                    break;
                case NAME_Byte:
                    Type = DbType.Byte;
                    CsTypeName = "byte";
                    SqlTypeName = SQL_TYPENAME_UNSIGNED_TINYINT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToByte(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToByte(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToByte(XNAME_MaxValue));
                    break;
                case NAME_SByte:
                    Type = DbType.SByte;
                    CsTypeName = "sbyte";
                    SqlTypeName = SQL_TYPENAME_TINYINT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToSByte(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToSByte(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToSByte(XNAME_MaxValue));
                    break;
                case NAME_Short:
                    Type = DbType.Int16;
                    CsTypeName = "short";
                    SqlTypeName = SQL_TYPENAME_SMALLINT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToInt16(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToInt16(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToInt16(XNAME_MaxValue));
                    break;
                case NAME_UShort:
                    Type = DbType.UInt16;
                    CsTypeName = "ushort";
                    SqlTypeName = SQL_TYPENAME_UNSIGNED_SMALLINT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToUInt16(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToUInt16(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToUInt16(XNAME_MaxValue));
                    break;
                case NAME_Int:
                    Type = DbType.Int32;
                    CsTypeName = "int";
                    SqlTypeName = SQL_TYPENAME_INT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToInt32(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToInt32(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToInt32(XNAME_MaxValue));
                    break;
                case NAME_UInt:
                    Type = DbType.UInt32;
                    CsTypeName = "uint";
                    SqlTypeName = SQL_TYPENAME_UNSIGNED_INT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToUInt32(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToUInt32(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToUInt32(XNAME_MaxValue));
                    break;
                case NAME_Long:
                    Type = DbType.Int64;
                    CsTypeName = "long";
                    SqlTypeName = SQL_TYPENAME_BIGINT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToInt64(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToInt64(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToInt64(XNAME_MaxValue));
                    break;
                case NAME_ULong:
                    Type = DbType.UInt64;
                    CsTypeName = "ulong";
                    SqlTypeName = SQL_TYPENAME_UNSIGNED_BIGINT;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToUInt64(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToUInt64(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToUInt64(XNAME_MaxValue));
                    break;
                case NAME_Float:
                    Type = DbType.Single;
                    CsTypeName = "float";
                    SqlTypeName = SQL_TYPENAME_REAL;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToSingle(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToSingle(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToSingle(XNAME_MaxValue));
                    break;
                case NAME_Double:
                    Type = DbType.Double;
                    CsTypeName = "double";
                    SqlTypeName = SQL_TYPENAME_REAL;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToDouble(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToDouble(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToDouble(XNAME_MaxValue));
                    break;
                case NAME_Decimal:
                    Type = DbType.Decimal;
                    CsTypeName = "decimal";
                    SqlTypeName = SQL_TYPENAME_NUMERIC;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToDecimal(XNAME_Default));
                    MinValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToDecimal(XNAME_MinValue));
                    MaxValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.AttributeToDecimal(XNAME_MaxValue));
                    break;
                case NAME_Char:
                    Type = DbType.StringFixedLength;
                    CsTypeName = "char";
                    SqlTypeName = $"{SQL_TYPENAME_CHARACTER}(1)";
                    MinLength = MaxLength = 1;
                    DefaultValue = (defaultNull) ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToChar(XNAME_Default));
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
                        FieldGenerationInfo defaultField = currentAndInheritedProperties.Elements(XNAME_Default).Where(e => !e.IsEmpty).Select(e => e.Value)
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
                    IsPrimaryKey = currentAndInheritedProperties.AttributeToBoolean(XNAME_IsPrimaryKey) ?? false;
                    break;
                case NAME_NVarChar:
                    Type = DbType.String;
                    CsTypeName = "string";
                    MaxLength = propertyElement.AttributeToInt32(XNAME_MaxLength);
                    MinLength = propertyElement.AttributeToInt32(XNAME_MinLength);
                    SqlTypeName = $"{SQL_TYPENAME_NVARCHAR}({MaxLength})";
                    DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToString(XNAME_Default));
                    IsNormalized = propertyElement.AttributeToBoolean(XNAME_IsNormalized) ?? Inherited.Select(i => i.Source).AttributeToBoolean(XNAME_IsNormalized);
                    IsCaseSensitive = propertyElement.AttributeToBoolean(XNAME_IsCaseSensitive) ?? Inherited.Select(i => i.Source).AttributeToBoolean(XNAME_IsCaseSensitive);
                    break;
                case NAME_VolumeIdentifier:
                    Type = DbType.String;
                    CsTypeName = "VolumeIdentifier";
                    MaxLength = 1024;
                    SqlTypeName = $"{SQL_TYPENAME_NVARCHAR}({MaxLength})";
                    DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToString(XNAME_Default));
                    IsCaseSensitive = false;
                    IsNormalized = true;
                    break;
                case NAME_MultiStringValue:
                    Type = DbType.String;
                    CsTypeName = "MultiStringValue";
                    SqlTypeName = SQL_TYPENAME_TEXT;
                    DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToString(XNAME_Default));
                    break;
                case NAME_Text:
                    Type = DbType.String;
                    CsTypeName = "string";
                    SqlTypeName = SQL_TYPENAME_TEXT;
                    DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToString(XNAME_Default));
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
                        DefaultValue = PropertyValueCode.Of(currentAndInheritedProperties.ElementToDateTime(XNAME_Default));
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
                        DefaultValue = PropertyValueCode.Of(currentAndInheritedProperties.ElementToTimeSpan(XNAME_Default));
                    break;
                case NAME_ByteValues:
                    Type = DbType.Binary;
                    MaxLength = propertyElement.AttributeToInt32(XNAME_MaxLength);
                    MinLength = propertyElement.AttributeToInt32(XNAME_MinLength);
                    CsTypeName = "byte[]";
                    SqlTypeName = $"{SQL_TYPENAME_VARBINARY}({MaxLength})";
                    DefaultValue = defaultNull ? PropertyValueCode.Null : PropertyValueCode.Of(currentAndInheritedProperties.ElementToBinary(XNAME_Default));
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
                        DefaultValue = PropertyValueCode.Of(currentAndInheritedProperties.AttributeToDriveType(XNAME_Default));
                    break;
                case NAME_RelatedEntity:
                    Type = DbType.Guid;
                    CsTypeName = nameof(Guid);
                    SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                    if (defaultNull)
                        DefaultValue = PropertyValueCode.Null;
                    IsCaseSensitive = false;
                    IsNormalized = true;
                    name = currentAndInheritedProperties.ElementToString(XNAME_PrimaryEntity);
                    if ((element = propertyElement.FindEntityByName(name)) is null)
                    {
                        if ((element = propertyElement.FindPropertyByFullName(currentAndInheritedProperties.ElementToString(XNAME_PrimaryProperty))) is not null)
                            PrimaryEntity = (element.Parent?.Parent?.AttributeToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                        IsOneToOne = true;
                    }
                    else
                    {
                        PrimaryEntity = (name, "Id");
                        IsOneToOne = currentAndInheritedProperties.AttributeToBoolean(XNAME_IsOneToOne) ?? false;
                    }
                    if ((name = currentAndInheritedProperties.Elements(XNAME_DbRelationship).ElementToString(XNAME_Name)) is null)
                        return;
                    DbRelationship = (name, currentAndInheritedProperties.Elements(XNAME_DbRelationship).ElementToString(XNAME_FkPropertyName), false);
                    name = DbRelationship?.FkPropertyName;
                    if (!(string.IsNullOrEmpty(name) || entity.Source.Elements(XNAME_Properties).Elements().Any(e => (e.AttributeToString(XNAME_ColName) ?? e.AttributeToString(XNAME_Name)) == name) ||
                        entity.BaseTypes.Any(b => b.Entity.Properties.Any(p => p.ColumnName == name))))
                        ColumnName = name;
                    return;
                case NAME_RelatedEntity_Type:
                    Type = DbType.Guid;
                    CsTypeName = nameof(Guid);
                    SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                    if (defaultNull)
                        DefaultValue = PropertyValueCode.Null;
                    IsCaseSensitive = false;
                    IsNormalized = true;
                    name = propertyElement.AttributeToString(XNAME_Reference);
                    if ((element = propertyElement.FindEntityByName(name)) is null)
                    {
                        name = currentAndInheritedProperties.ElementToString(XNAME_PrimaryEntity);
                        if ((element = propertyElement.FindEntityByName(name)) is null)
                        {
                            if ((element = propertyElement.FindPropertyByFullName(currentAndInheritedProperties.ElementToString(XNAME_PrimaryProperty))) is not null)
                                PrimaryEntity = (element.Parent?.Parent?.AttributeToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                            IsOneToOne = true;
                        }
                        else
                        {
                            IsOneToOne = currentAndInheritedProperties.AttributeToBoolean(XNAME_IsOneToOne) ?? false;
                            PrimaryEntity = (name, "Id");
                        }
                    }
                    else
                    {
                        IsOneToOne = currentAndInheritedProperties.AttributeToBoolean(XNAME_IsOneToOne) ?? false;
                        PrimaryEntity = (name, "Id");
                    }
                    if ((name = currentAndInheritedProperties.Elements(XNAME_DbRelationship).ElementToString(XNAME_Name)) is null)
                        return;
                    DbRelationship = (name, currentAndInheritedProperties.Elements(XNAME_DbRelationship).ElementToString(XNAME_FkPropertyName), false);
                    name = DbRelationship?.FkPropertyName;
                    if (!(string.IsNullOrEmpty(name) || entity.Source.Elements(XNAME_Properties).Elements().Any(e => (e.AttributeToString(XNAME_ColName) ?? e.AttributeToString(XNAME_Name)) == name) ||
                        entity.BaseTypes.Any(b => b.Entity.Properties.Any(p => p.ColumnName == name))))
                        ColumnName = name;
                    return;
                case NAME_RelatedEntity_Key:
                    Type = DbType.Guid;
                    CsTypeName = nameof(Guid);
                    SqlTypeName = SQL_TYPENAME_UNIQUEIDENTIFIER;
                    if (defaultNull)
                        DefaultValue = PropertyValueCode.Null;
                    IsCaseSensitive = false;
                    IsNormalized = true;
                    name = propertyElement.AttributeToString(XNAME_Reference);
                    if ((element = propertyElement.FindPropertyByFullName(name)) is null)
                    {
                        name = currentAndInheritedProperties.ElementToString(XNAME_PrimaryEntity);
                        if ((element = propertyElement.FindEntityByName(name)) is null)
                        {
                            if ((element = propertyElement.FindPropertyByFullName(currentAndInheritedProperties.ElementToString(XNAME_PrimaryProperty))) is not null)
                                PrimaryEntity = (element.Parent?.Parent?.AttributeToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                            IsOneToOne = true;
                        }
                        else
                        {
                            IsOneToOne = currentAndInheritedProperties.AttributeToBoolean(XNAME_IsOneToOne) ?? false;
                            PrimaryEntity = (name, "Id");
                        }
                    }
                    else
                    {
                        IsOneToOne = currentAndInheritedProperties.AttributeToBoolean(XNAME_IsOneToOne) ?? false;
                        PrimaryEntity = (name, "Id");
                    }
                    if ((name = currentAndInheritedProperties.Elements(XNAME_DbRelationship).ElementToString(XNAME_Name)) is null)
                        return;
                    DbRelationship = (name, currentAndInheritedProperties.Elements(XNAME_DbRelationship).ElementToString(XNAME_FkPropertyName), false);
                    name = DbRelationship?.FkPropertyName;
                    if (!(string.IsNullOrEmpty(name) || entity.Source.Elements(XNAME_Properties).Elements().Any(e => (e.AttributeToString(XNAME_ColName) ?? e.AttributeToString(XNAME_Name)) == name) ||
                        entity.BaseTypes.Any(b => b.Entity.Properties.Any(p => p.ColumnName == name))))
                        ColumnName = name;
                    return;
                case NAME_CollectionNavigation:
                    Type = DbType.Object;
                    name = currentAndInheritedProperties.ElementToString(NAME_ItemKey);
                    if ((element = propertyElement.FindPropertyByFullName(name)) is null)
                    {
                        name = currentAndInheritedProperties.ElementToString(NAME_ItemKey);
                        element = propertyElement.FindEntityByName(name);
                        colName = element?.AttributeToString(XNAME_ColName) ?? name;
                        ItemEntity = (name, $"{entity.SingularName}Id");
                    }
                    else // TODO: Would this ever reference RelatedEntity instead of UniqueIdentifier?
                        ItemEntity = (element.Parent?.Parent?.ElementToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                    CsTypeName = $"IEnumerable<{DbRelationship.Value.Name}>";
                    SqlTypeName = SQL_TYPENAME_BLOB;
                    IsOneToOne = false;
                    return;
                case NAME_CollectionNavigation_ItemType:
                    Type = DbType.Object;
                    name = currentAndInheritedProperties.ElementToString(NAME_Reference);
                    if ((element = propertyElement.FindEntityByName(name)) is null)
                    {
                        name = currentAndInheritedProperties.ElementToString(NAME_ItemKey);
                        if ((element = propertyElement.FindPropertyByFullName(name)) is null)
                        {
                            name = currentAndInheritedProperties.ElementToString(NAME_ItemKey);
                            element = propertyElement.FindEntityByName(name);
                            colName = element?.AttributeToString(XNAME_ColName) ?? name;
                            ItemEntity = (name, $"{entity.SingularName}Id");
                        }
                        else // TODO: Would this ever reference RelatedEntity instead of UniqueIdentifier?
                            ItemEntity = (element.Parent?.Parent?.ElementToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                    }
                    else
                        DbRelationship = (name, $"{entity.SingularName}Id", true);
                    CsTypeName = $"IEnumerable<{DbRelationship.Value.Name}>";
                    SqlTypeName = SQL_TYPENAME_BLOB;
                    IsOneToOne = false;
                    return;
                case NAME_CollectionNavigation_ItemKey:
                    Type = DbType.Object;
                    name = currentAndInheritedProperties.ElementToString(NAME_Reference);
                    if ((element = propertyElement.FindPropertyByFullName(name)) is null)
                    {
                        name = currentAndInheritedProperties.ElementToString(NAME_ItemKey);
                        if ((element = propertyElement.FindPropertyByFullName(name)) is null)
                        {
                            name = currentAndInheritedProperties.ElementToString(NAME_ItemKey);
                            element = propertyElement.FindEntityByName(name);
                            colName = element?.AttributeToString(XNAME_ColName) ?? name;
                            ItemEntity = (name, $"{entity.SingularName}Id");
                        }
                        else // TODO: Would this ever reference RelatedEntity instead of UniqueIdentifier?
                            ItemEntity = (element.Parent?.Parent?.ElementToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                    }
                    else // TODO: Would this ever reference RelatedEntity instead of UniqueIdentifier?
                        ItemEntity = (element.Parent?.Parent?.ElementToString(XNAME_Name), element.AttributeToString(XNAME_Name));
                    CsTypeName = $"IEnumerable<{DbRelationship.Value.Name}>";
                    SqlTypeName = SQL_TYPENAME_BLOB;
                    IsOneToOne = false;
                    return;
                default:
                    Type = DbType.Object;
                    CsTypeName = "object";
                    SqlTypeName = SQL_TYPENAME_BLOB;
                    return;
            }
            ColumnName = (propertyElement.AttributeToString(NAME_ColName) ?? Inherited.Select(p => p.Source).AttributeToString(NAME_ColName)) ?? Name;
        }

        /// <summary>
        /// The property name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The column name or <see langword="null"/> if this does not represent a database column.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// The generalized property type.
        /// </summary>
        public DbType Type { get; }

        /// <summary>
        /// The C-Sharp property type name.
        /// </summary>
        public string CsTypeName { get; }

        /// <summary>
        /// The SQLite column type name.
        /// </summary>
        public string SqlTypeName { get; }

        /// <summary>
        /// The optional maximum length.
        /// </summary>
        public int? MaxLength { get; }

        /// <summary>
        /// The optional minimum length CHECK constraint.
        /// </summary>
        public int? MinLength { get; }

        /// <summary>
        /// The optional maximum value CHECK constraint.
        /// </summary>
        public PropertyValueCode? MaxValue { get; }

        /// <summary>
        /// The optional minimum value CHECK constraint.
        /// </summary>
        public PropertyValueCode? MinValue { get; }

        /// <summary>
        /// The optional default column value.
        /// </summary>
        public PropertyValueCode? DefaultValue { get; }

        /// <summary>
        /// Optionally specifies whether the column value should be normalized. This only applies to data stored as string values.
        /// </summary>
        public bool? IsNormalized { get; }

        /// <summary>
        /// Represents a reference to the primary entity in a one-to-one or one-to-many relationship where the current entity is the dependent.
        /// </summary>
        /// <remarks>
        /// If this value is not <see langword="null"/>, then <see cref="ItemEntity"/> will be null, and vice-versa.
        /// <list type="bullet">
        /// <item><term>EntityName</term><description>The name of the related entity.</description></item>
        /// <item><term>FkPropertyName</term><description>The name of the primary key property on the primary entity.</description></item>
        /// </list>
        /// </remarks>
        public (string EntityName, string PrimaryKey)? PrimaryEntity { get; }

        /// <summary>
        /// Represents a reference to the dependent entity in a one-to-many relationship where the current entity is the primary.
        /// </summary>
        /// <remarks>
        /// If this value is not <see langword="null"/>, then <see cref="PrimaryEntity"/> will be null, and vice-versa.
        /// <list type="bullet">
        /// <item><term>EntityName</term><description>The name of the dependent entity.</description></item>
        /// <item><term>FkPropertyName</term><description>The name of the foreign key property on the dependent entity.</description></item>
        /// </list>
        /// </remarks>
        public (string EntityName, string ForeignKey)? ItemEntity { get; }

        /// <summary>
        /// Defines a database relationship.
        /// </summary>
        /// <remarks>
        /// If this value is not <see langword="null"/>, then <see cref="PrimaryEntity"/> will not be null.
        /// <para>If this value is <see langword="null"/> and <see cref="PrimaryEntity"/> is not null, then it is assumed that the current property is the navigation property for the
        /// opposite end of a one-to-one relationship with another entity that is linked by the current entity's primary key.</para>
        /// <list type="bullet">
        /// <item><term>Name</term><description>The name of the foreign key relationship.</description></item>
        /// <item><term>FkPropertyName</term><description>The name of the foreign key property on the current entity.</description></item>
        /// <item><term>IsMany</term><description><see langword="true"/> if this is a one-to-many relationship; otherwise, <see langword="false"/> if it is a
        /// one-to-one relationship.</description></item>
        /// </list>
        /// </remarks>
        public (string Name, string FkPropertyName, bool IsMany)? DbRelationship { get; }

        public bool IsPrimaryKey { get; }

        public bool? IsCaseSensitive { get; }

        /// <summary>
        /// Gets the entity type referenced by <see cref="PrimaryEntity"/> pr <see cref="ItemEntity"/>.
        /// </summary>
        public ITypeGenerationInfo ReferencedType
        {
            get
            {
                if (_referencedType is null)
                {
                    if (PrimaryEntity.HasValue)
                        _referencedType = EntityGenerationInfo.Get(Source.FindEntityByName(PrimaryEntity.Value.EntityName));
                    else if (ItemEntity.HasValue)
                        _referencedType = EntityGenerationInfo.Get(Source.FindEntityByName(ItemEntity.Value.EntityName));
                }
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

        /// <summary>
        /// Gets the property definitions for the same property from inherited entities.
        /// </summary>
        public ReadOnlyCollection<PropertyGenerationInfo> Inherited { get; }

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
