using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CodeGeneration
{
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
    }
}
