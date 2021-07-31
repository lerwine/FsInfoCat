using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using static CodeGeneration.CgConstants;

namespace CodeGeneration
{
    public class EnumGenerationInfo : ITypeGenerationInfo
    {
        private EnumGenerationInfo(XElement enumElement)
        {
            Func<string, IComparable> toRawValue;
            Func<IEnumerable<IComparable>, (PropertyValueCode Min, PropertyValueCode Max)> getRange;
            IsFlags = enumElement.AttributeToBoolean(XNAME_IsFlags) ?? false;
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

        public static EnumGenerationInfo Get(XElement enumElement)
        {
            if (enumElement is null)
                return null;
            EnumGenerationInfo result = enumElement.Annotation<EnumGenerationInfo>();
            if (result is not null)
                return result;
            if (enumElement.IsEnumTypeElement())
            {
                result = new(enumElement);
                enumElement.AddAnnotation(result);
            }
            return result;
        }

        public IEnumerable<IMemberGenerationInfo> GetMembers() => Fields.Cast<IMemberGenerationInfo>();
    }
}
