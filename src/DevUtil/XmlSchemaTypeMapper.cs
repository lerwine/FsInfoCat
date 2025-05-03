using System;
using System.Xml;

namespace DevUtil
{
    public class XmlSchemaTypeMapper(bool byteArrayToBase64) : TypeMapper(PREFIX_xs, System.Xml.Schema.XmlSchema.Namespace)
    {
        #region String constants

        public const string PREFIX_xs = "xs";
        public const string NCNAME_anyURI = "anyURI";
        public const string NCNAME_base64Binary = "base64Binary";
        public const string NCNAME_boolean = "boolean";
        public const string NCNAME_byte = "byte";
        public const string NCNAME_date = "date";
        public const string NCNAME_dateTime = "dateTime";
        public const string NCNAME_decimal = "decimal";
        public const string NCNAME_double = "double";
        public const string NCNAME_duration = "duration";
        public const string NCNAME_ENTITIES = "ENTITIES";
        public const string NCNAME_ENTITY = "ENTITY";
        public const string NCNAME_float = "float";
        public const string NCNAME_gDay = "gDay";
        public const string NCNAME_gMonthDay = "gMonthDay";
        public const string NCNAME_gYear = "gYear";
        public const string NCNAME_gYearMonth = "gYearMonth";
        public const string NCNAME_hexBinary = "hexBinary";
        public const string NCNAME_ID = "ID";
        public const string NCNAME_IDREF = "IDREF";
        public const string NCNAME_IDREFS = "IDREFS";
        public const string NCNAME_int = "int";
        public const string NCNAME_integer = "integer";
        public const string NCNAME_language = "language";
        public const string NCNAME_long = "long";
        public const string NCNAME_gMonth = "gMonth";
        public const string NCNAME_Name = "Name";
        public const string NCNAME_NCName = "NCName";
        public const string NCNAME_negativeInteger = "negativeInteger";
        public const string NCNAME_NMTOKEN = "NMTOKEN";
        public const string NCNAME_NMTOKENS = "NMTOKENS";
        public const string NCNAME_nonNegativeInteger = "nonNegativeInteger";
        public const string NCNAME_nonPositiveInteger = "nonPositiveInteger";
        public const string NCNAME_normalizedString = "normalizedString";
        public const string NCNAME_NOTATION = "NOTATION";
        public const string NCNAME_positiveInteger = "positiveInteger";
        public const string NCNAME_QName = "QName";
        public const string NCNAME_short = "short";
        public const string NCNAME_string = "string";
        public const string NCNAME_time = "time";
        public const string NCNAME_token = "token";
        public const string NCNAME_unsignedByte = "unsignedByte";
        public const string NCNAME_unsignedInt = "unsignedInt";
        public const string NCNAME_unsignedLong = "unsignedLong";
        public const string NCNAME_unsignedShort = "unsignedShort";
        public const string NCNAME_anySimpleType = "anySimpleType";

        #endregion

        #region static read-only XmlQualifiedName fields

        public static readonly XmlQualifiedName QNAME_anyURI = new(NCNAME_anyURI, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_base64Binary = new(NCNAME_base64Binary, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_boolean = new(NCNAME_boolean, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_byte = new(NCNAME_byte, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_date = new(NCNAME_date, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_dateTime = new(NCNAME_dateTime, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_decimal = new(NCNAME_decimal, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_double = new(NCNAME_double, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_duration = new(NCNAME_duration, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_ENTITIES = new(NCNAME_ENTITIES, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_ENTITY = new(NCNAME_ENTITY, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_float = new(NCNAME_float, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_gDay = new(NCNAME_gDay, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_gMonthDay = new(NCNAME_gMonthDay, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_gYear = new(NCNAME_gYear, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_gYearMonth = new(NCNAME_gYearMonth, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_hexBinary = new(NCNAME_hexBinary, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_ID = new(NCNAME_ID, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_IDREF = new(NCNAME_IDREF, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_IDREFS = new(NCNAME_IDREFS, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_int = new(NCNAME_int, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_integer = new(NCNAME_integer, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_language = new(NCNAME_language, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_long = new(NCNAME_long, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_gMonth = new(NCNAME_gMonth, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_Name = new(NCNAME_Name, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_NCName = new(NCNAME_NCName, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_negativeInteger = new(NCNAME_negativeInteger, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_NMTOKEN = new(NCNAME_NMTOKEN, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_NMTOKENS = new(NCNAME_NMTOKENS, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_nonNegativeInteger = new(NCNAME_nonNegativeInteger, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_nonPositiveInteger = new(NCNAME_nonPositiveInteger, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_normalizedString = new(NCNAME_normalizedString, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_NOTATION = new(NCNAME_NOTATION, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_positiveInteger = new(NCNAME_positiveInteger, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_QName = new(NCNAME_QName, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_short = new(NCNAME_short, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_string = new(NCNAME_string, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_time = new(NCNAME_time, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_token = new(NCNAME_token, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_unsignedByte = new(NCNAME_unsignedByte, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_unsignedInt = new(NCNAME_unsignedInt, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_unsignedLong = new(NCNAME_unsignedLong, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_unsignedShort = new(NCNAME_unsignedShort, PREFIX_xs);
        public static readonly XmlQualifiedName QNAME_anySimpleType = new(NCNAME_anySimpleType, PREFIX_xs);

        #endregion

        private readonly bool _byteArrayToBase64 = byteArrayToBase64;

        public override string GetNCNameOrNull(Type type)
        {
            if (type is null) return null;
            if (type.IsPrimitive)
            {
                if (type.Equals(typeof(bool))) return NCNAME_boolean;
                if (type.Equals(typeof(byte))) return NCNAME_unsignedByte;
                if (type.Equals(typeof(char))) return NCNAME_string;
                if (type.Equals(typeof(double))) return NCNAME_double;
                if (type.Equals(typeof(short))) return NCNAME_short;
                if (type.Equals(typeof(int))) return NCNAME_int;
                if (type.Equals(typeof(long))) return NCNAME_long;
                if (type.Equals(typeof(sbyte))) return NCNAME_byte;
                if (type.Equals(typeof(float))) return NCNAME_float;
                if (type.Equals(typeof(ushort))) return NCNAME_unsignedShort;
                if (type.Equals(typeof(uint))) return NCNAME_unsignedInt;
                if (type.Equals(typeof(ulong))) return NCNAME_unsignedLong;
                return null;
            }
            if (type.IsValueType)
            {
                if (type.Equals(typeof(DateTime))) return NCNAME_dateTime;
                if (type.Equals(typeof(TimeSpan))) return NCNAME_duration;
                if (type.Equals(typeof(decimal))) return NCNAME_decimal;
                return null;
            }
            if (!type.IsClass || type.IsGenericType || type.IsPointer || type.IsByRef) return null;
            if (type.Equals(typeof(string))) return NCNAME_string;
            if (type.Equals(typeof(Uri))) return NCNAME_anyURI;
            if (type.Equals(typeof(byte[]))) return _byteArrayToBase64 ? NCNAME_base64Binary : NCNAME_hexBinary;
            if (type.Equals(typeof(XmlQualifiedName))) return NCNAME_QName;
            return null;
        }

        public override bool CanMapToXsdType(Type type, string ncName)
        {
            if (type is null) throw new System.ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(ncName)) return false;
            return ncName switch
            {
                NCNAME_anyURI => type.Equals(typeof(Uri)),
                NCNAME_base64Binary => type.Equals(typeof(byte[])),
                NCNAME_boolean => type.Equals(typeof(bool)),
                NCNAME_byte => type.Equals(typeof(sbyte)),
                NCNAME_date => type.Equals(typeof(DateTime)),
                NCNAME_dateTime => type.Equals(typeof(DateTime)),
                NCNAME_decimal => type.Equals(typeof(decimal)),
                NCNAME_double => type.Equals(typeof(double)),
                NCNAME_duration => type.Equals(typeof(TimeSpan)),
                NCNAME_ENTITIES => type.Equals(typeof(string[])),
                NCNAME_ENTITY => type.Equals(typeof(string)),
                NCNAME_float => type.Equals(typeof(float)),
                NCNAME_gDay => type.Equals(typeof(DateTime)),
                NCNAME_gMonthDay => type.Equals(typeof(DateTime)),
                NCNAME_gYear => type.Equals(typeof(DateTime)),
                NCNAME_gYearMonth => type.Equals(typeof(DateTime)),
                NCNAME_hexBinary => type.Equals(typeof(byte[])),
                NCNAME_ID => type.Equals(typeof(string)),
                NCNAME_IDREF => type.Equals(typeof(string)),
                NCNAME_IDREFS => type.Equals(typeof(string[])),
                NCNAME_int => type.Equals(typeof(int)),
                NCNAME_integer => type.Equals(typeof(decimal)),
                NCNAME_language => type.Equals(typeof(decimal)),
                NCNAME_long => type.Equals(typeof(long)),
                NCNAME_gMonth => type.Equals(typeof(DateTime)),
                NCNAME_Name => type.Equals(typeof(string)),
                NCNAME_NCName => type.Equals(typeof(string)),
                NCNAME_negativeInteger => type.Equals(typeof(decimal)),
                NCNAME_NMTOKEN => type.Equals(typeof(string)),
                NCNAME_NMTOKENS => type.Equals(typeof(string[])),
                NCNAME_nonNegativeInteger => type.Equals(typeof(decimal)),
                NCNAME_nonPositiveInteger => type.Equals(typeof(decimal)),
                NCNAME_normalizedString => type.Equals(typeof(string)),
                NCNAME_NOTATION => type.Equals(typeof(XmlQualifiedName)),
                NCNAME_positiveInteger => type.Equals(typeof(string)),
                NCNAME_QName => type.Equals(typeof(XmlQualifiedName)),
                NCNAME_short => type.Equals(typeof(short)),
                NCNAME_string => type.Equals(typeof(string)) || type.Equals(typeof(char)),
                NCNAME_time => type.Equals(typeof(DateTime)),
                NCNAME_token => type.Equals(typeof(string)),
                NCNAME_unsignedByte => type.Equals(typeof(byte)),
                NCNAME_unsignedInt => type.Equals(typeof(uint)),
                NCNAME_unsignedLong => type.Equals(typeof(ulong)),
                NCNAME_unsignedShort => type.Equals(typeof(ushort)),
                NCNAME_anySimpleType => type.Equals(typeof(string)),
                _ => false,
            };
        }

        public override XmlQualifiedName ToXsdType(Type type)
        {
            if (type is null) return null;
            if (type.IsPrimitive)
            {
                if (type.Equals(typeof(bool))) return QNAME_boolean;
                if (type.Equals(typeof(byte))) return QNAME_unsignedByte;
                if (type.Equals(typeof(char))) return QNAME_string;
                if (type.Equals(typeof(double))) return QNAME_double;
                if (type.Equals(typeof(short))) return QNAME_short;
                if (type.Equals(typeof(int))) return QNAME_int;
                if (type.Equals(typeof(long))) return QNAME_long;
                if (type.Equals(typeof(sbyte))) return QNAME_byte;
                if (type.Equals(typeof(float))) return QNAME_float;
                if (type.Equals(typeof(ushort))) return QNAME_unsignedShort;
                if (type.Equals(typeof(uint))) return QNAME_unsignedInt;
                if (type.Equals(typeof(ulong))) return QNAME_unsignedLong;
                return null;
            }
            if (type.IsValueType)
            {
                if (type.Equals(typeof(DateTime))) return QNAME_dateTime;
                if (type.Equals(typeof(TimeSpan))) return QNAME_duration;
                if (type.Equals(typeof(decimal))) return QNAME_decimal;
                return null;
            }
            if (!type.IsClass || type.IsGenericType || type.IsPointer || type.IsByRef) return null;
            if (type.Equals(typeof(string))) return QNAME_string;
            if (type.Equals(typeof(Uri))) return QNAME_anyURI;
            if (type.Equals(typeof(byte[]))) return _byteArrayToBase64 ? QNAME_base64Binary : QNAME_hexBinary;
            if (type.Equals(typeof(XmlQualifiedName))) return QNAME_QName;
            return null;
        }

        public override bool IsMappedType(Type type)
        {
            if (type is null) return false;
            if (type.IsPrimitive) return !(type.Equals(typeof(IntPtr)) || type.Equals(typeof(UIntPtr)));
            if (type.IsValueType) return type.Equals(typeof(DateTime)) || type.Equals(typeof(TimeSpan)) || type.Equals(typeof(decimal));
            return type.IsClass && !(type.IsGenericType || type.IsPointer || type.IsByRef) &&
                (type.Equals(typeof(string)) || type.Equals(typeof(Uri)) || type.Equals(typeof(byte[])) || type.Equals(typeof(XmlQualifiedName)));
        }
    }
}
