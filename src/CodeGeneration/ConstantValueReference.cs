using System;
using System.Xml;

namespace CodeGeneration
{
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
}
