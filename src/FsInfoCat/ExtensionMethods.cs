using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    public static class ExtensionMethods
    {
        public static readonly Regex BackslashEscapablePattern = new(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BackslashEscapableLBPattern = new(@"(?<l>[""\\])|(?<n>\r\n?|\n)|[\0\a\b\f\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static T DefaultIf<T>(T inputValue, Func<T, bool> predicate, T defaultValue) => predicate(inputValue) ? defaultValue : inputValue;

        public static TResult DefaultIf<TInput, TResult>(TInput inputValue, PredicatedProduction<TInput, TResult> producer, TResult defaultValue) =>
            producer(inputValue, out TResult result) ? result : defaultValue;

        public static T GetDefaultIf<T>(T inputValue, Func<T, bool> predicate, Func<T> defaultValueFunc) => predicate(inputValue) ? defaultValueFunc() : inputValue;

        public static TResult GetDefaultIf<TInput, TResult>(TInput inputValue, PredicatedProduction<TInput, TResult> producer, Func<TInput, TResult> defaultValueFunc) =>
            producer(inputValue, out TResult result) ? result : defaultValueFunc(inputValue);

        public static bool TryGetDescription(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is null)
            {
                result = null;
                return false;
            }
            DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
            if (attribute is not null && (result = attribute.GetDescription().NullIfWhiteSpace()) is not null)
                return true;
            return (result = memberInfo.GetCustomAttributes<DescriptionAttribute>(true).Select(a => a.Description.NullIfWhiteSpace()).FirstOrDefault(d => d is not null)) is not null;
        }

        public static bool TryGetDisplayName(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is null)
            {
                result = null;
                return false;
            }
            DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
            if (attribute is not null && (result = attribute.GetName().NullIfWhiteSpace()) is not null)
                return true;
            return (result = memberInfo.GetCustomAttributes<DisplayNameAttribute>(true).Select(a => a.DisplayName.NullIfWhiteSpace()).FirstOrDefault(d => d is not null)) is not null;
        }

        public static string GetDisplayName<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                FieldInfo fieldInfo = typeof(TEnum).GetField(name);
                if (fieldInfo.TryGetDisplayName(out string result))
                    return result;
            }
            return  Enum.GetName(typeof(TEnum), value);
        }

        public static bool TryGetDisplayName<TEnum>(this TEnum value, out string result) where TEnum : struct, Enum
        {
            result = Enum.GetName(value);
            return result is not null && typeof(TEnum).GetField(result).TryGetDisplayName(out result);
        }

        public static bool TryGetDescription<TEnum>(this TEnum value, out string result) where TEnum : struct, Enum
        {
            result = Enum.GetName(value);
            return result is not null && typeof(TEnum).GetField(result).TryGetDescription(out result);
        }

        public static bool TryGetAmbientValue<TEnum, TResult>(this TEnum value, out TResult result)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                AmbientValueAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<AmbientValueAttribute>();
                if (attribute is not null && attribute.Value is TResult r)
                {
                    result = r;
                    return false;
                }
            }
            result = default;
            return false;
        }

        public static TResult GetAmbientValue<TEnum, TResult>(this TEnum value, TResult defaultValue = default)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                AmbientValueAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<AmbientValueAttribute>();
                if (attribute is not null && attribute.Value is TResult r)
                    return r;
            }
            return defaultValue;
        }

        public static IEnumerable<TEnum> GetFlagValues<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            Type type = typeof(TEnum);
            if (type.GetCustomAttribute<FlagsAttribute>() is null)
                return new[] { value };
#pragma warning disable CA2248 // Provide correct 'enum' argument to 'Enum.HasFlag'
            return Enum.GetValues<TEnum>().Where(v => value.HasFlag(v));
#pragma warning restore CA2248 // Provide correct 'enum' argument to 'Enum.HasFlag'
        }

        public static ErrorCode ToErrorCode(this AccessErrorCode errorCode) => errorCode.GetAmbientValue(ErrorCode.Unexpected);

        public static AccessErrorCode ToAccessErrorCode(this ErrorCode errorCode) =>
            Enum.GetValues<AccessErrorCode>().Where(e => e.ToErrorCode() == errorCode).DefaultIfEmpty(AccessErrorCode.Unspecified).First();

        public static EventId ToEventId(this AccessErrorCode errorCode) => errorCode.ToErrorCode().ToEventId();

        public static EventId ToEventId(this ErrorCode errorCode) => new((byte)errorCode, errorCode.TryGetDescription(out string name) ? name : errorCode.GetDisplayName());

        public static bool IsNullableType(this Type type) => (type ?? throw new ArgumentNullException(nameof(type))).IsValueType && type.IsGenericType &&
            typeof(Nullable<>).Equals(type.GetGenericTypeDefinition());

        public static bool IsNullAssignable(this Type type) => !(type ?? throw new ArgumentNullException(nameof(type))).IsValueType || type.IsNullableType();

        public static string ToCsTypeName(this Type type, bool omitNamespaces = false)
        {
            if (type is null)
                return "null";
            if (type.IsGenericParameter)
                return type.Name;
            if (type.IsPointer)
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}*";
            if (type.IsByRef)
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}&";
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                if (rank < 2)
                    return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[]";
                if (rank == 2)
                    return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[,]";
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[{new string(',', rank - 1)}]";
            }
            if (type.IsNullableType())
                return $"{ToCsTypeName(Nullable.GetUnderlyingType(type), omitNamespaces)}?";

            if (type.IsValueType)
            {
                if (type.Equals(typeof(void)))
                    return "void";
                if (type.Equals(typeof(char)))
                    return "char";
                if (type.Equals(typeof(bool)))
                    return "bool";
                if (type.Equals(typeof(byte)))
                    return "byte";
                if (type.Equals(typeof(sbyte)))
                    return "sbyte";
                if (type.Equals(typeof(short)))
                    return "short";
                if (type.Equals(typeof(ushort)))
                    return "ushort";
                if (type.Equals(typeof(int)))
                    return "int";
                if (type.Equals(typeof(uint)))
                    return "uint";
                if (type.Equals(typeof(long)))
                    return "long";
                if (type.Equals(typeof(ulong)))
                    return "ulong";
                if (type.Equals(typeof(float)))
                    return "float";
                if (type.Equals(typeof(double)))
                    return "double";
                if (type.Equals(typeof(decimal)))
                    return "decimal";
            }
            else
            {
                if (type.Equals(typeof(string)))
                    return "string";
                if (type.Equals(typeof(object)))
                    return "object";
            }
            string n = type.Name;
            string ns;
            if (type.IsNested)
                ns = ToCsTypeName(type.DeclaringType, omitNamespaces);
            else if (omitNamespaces || (ns = type.Namespace) is null || ns == "System")
                ns = "";

            if (type.IsGenericType)
            {
                int i = n.IndexOf("`");
                if (i > 0)
                    n = n.Substring(0, i);
                if (ns.Length > 0)
                    return $"{ns}.{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName(omitNamespaces)))}>";
                return $"{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName(omitNamespaces)))}>";
            }
            return (ns.Length > 0) ? $"{ns}.{n}" : n;
        }

        public static string ToPseudoCsText(object obj)
        {
            if (obj is null)
                return "null";
            if (obj is string s)
                return $"\"{EscapeCsString(s)}\"";
            if (obj is char c)
                return c switch
                {
                    '\'' => "'\\''",
                    '"' => "'\"'",
                    _ => $"'{EscapeCsString(new string(new char[] { c }))}'",
                };
            if (obj is bool bv)
                return bv ? "true" : "false";
            if (obj is byte bn)
                return bn.ToString("X2");
            if (obj is sbyte sb)
                return $"(sbyte){sb:X2}";
            if (obj is short sv)
                return sv.ToString("X4");
            if (obj is ushort us)
                return $"(ushort){us:X4}";
            if (obj is int i)
                return i.ToString("X8");
            if (obj is uint ui)
                return $"{ui:X8}U";
            if (obj is long l)
                return l.ToString("X16");
            if (obj is ulong ul)
                return $"{ul:16}UL";
            if (obj is float fv)
                return $"{fv}f";
            if (obj is double d)
                return d.ToString();
            if (obj is decimal m)
                return $"{m}m";
            if (obj is DateTime dt)
                return dt.ToString();
            if (obj is DBNull)
                return "DBNull";
            if (obj is Type t)
                return t.ToCsTypeName();
            if (obj is IFormattable fm)
                fm.ToString();
            if (obj is IConvertible cv)
            {
                switch (cv.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return ToPseudoCsText(cv.ToBoolean(null));
                    case TypeCode.Byte:
                        return ToPseudoCsText(cv.ToByte(null));
                    case TypeCode.Char:
                        return ToPseudoCsText(cv.ToChar(null));
                    case TypeCode.DateTime:
                        return ToPseudoCsText(cv.ToDateTime(null));
                    case TypeCode.DBNull:
                        return "DBNull";
                    case TypeCode.Decimal:
                        return ToPseudoCsText(cv.ToDecimal(null));
                    case TypeCode.Double:
                        return ToPseudoCsText(cv.ToDouble(null));
                    case TypeCode.Int16:
                        return ToPseudoCsText(cv.ToInt16(null));
                    case TypeCode.Int32:
                        return ToPseudoCsText(cv.ToInt32(null));
                    case TypeCode.Int64:
                        return ToPseudoCsText(cv.ToInt64(null));
                    case TypeCode.SByte:
                        return ToPseudoCsText(cv.ToSByte(null));
                    case TypeCode.Single:
                        return ToPseudoCsText(cv.ToSingle(null));
                    case TypeCode.String:
                        return ToPseudoCsText(cv.ToString(null));
                    case TypeCode.UInt16:
                        return ToPseudoCsText(cv.ToUInt16(null));
                    case TypeCode.UInt32:
                        return ToPseudoCsText(cv.ToUInt32(null));
                    case TypeCode.UInt64:
                        return ToPseudoCsText(cv.ToUInt64(null));
                }
            }
            return obj.ToString();
        }

        public static string EscapeCsString(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !BackslashEscapablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return BackslashEscapableLBPattern.Replace(source, m =>
                {
                    if (m.Groups["l"].Success)
                        return $"\\{m.Value}";
                    Group g = m.Groups["n"];
                    if (g.Success)
                        return g.Value switch
                        {
                            "\r" => "\\r\r",
                            "\n" => "\\n\n",
                            _ => "\\r\\n\r\n",
                        };
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '\0':
                            return "\\0";
                        case '\a':
                            return "\\a";
                        case '\b':
                            return "\\b";
                        case '\f':
                            return "\\f";
                        case '\t':
                            return "\\t";
                        case '\v':
                            return "\\v";
                        default:
                            g = m.Groups["x"];
                            uint i = (uint)c;
                            if (g.Success)
                                return $"\\x{i:x4}{g.Value}";
                            return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                    }
                });
            return BackslashEscapablePattern.Replace(source, m =>
            {
                if (m.Groups["l"].Success)
                    return $"\\{m.Value}";
                char c = m.Value[0];
                switch (c)
                {
                    case '\0':
                        return "\\0";
                    case '\a':
                        return "\\a";
                    case '\b':
                        return "\\b";
                    case '\f':
                        return "\\f";
                    case '\n':
                        return "\\n";
                    case '\r':
                        return "\\r";
                    case '\t':
                        return "\\t";
                    case '\v':
                        return "\\v";
                    default:
                        Group g = m.Groups["x"];
                        uint i = (uint)c;
                        if (g.Success)
                            return $"\\x{i:x4}{g.Value}";
                        return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                }
            });
        }
    }

    public record IndexedValue<T>(int Index, T Value);
}
