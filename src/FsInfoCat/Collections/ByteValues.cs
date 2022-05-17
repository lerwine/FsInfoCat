using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Collections
{
    // TODO: Document ByteValues class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public sealed class ByteValues : ReadOnlyCollection<byte>, IEquatable<ByteValues>, IConvertible
    {
        public static readonly ValueConverter<ByteValues, byte[]> Converter = new(
            v => v == null ? null : v.ToArray(),
            s => s == null ? null : new ByteValues(s)
        );

        public ByteValues(IList<byte> list) : base(list) { }

        public ByteValues(string s) : base(string.IsNullOrWhiteSpace(s) ? Array.Empty<byte>() : Convert.FromBase64String(s)) { }

        public ByteValues() : base(Array.Empty<byte>()) { }

        public bool Equals(ByteValues other) => other is not null && (ReferenceEquals(this, other) || other.SequenceEqual(this));

        public override bool Equals(object obj) => obj is ByteValues other && Equals(other);

        public override int GetHashCode() => Items.GetAggregateHashCode();

        public override string ToString() => Count > 0 ? Convert.ToBase64String(Items.ToArray(), Base64FormattingOptions.None) : "";

        TypeCode IConvertible.GetTypeCode() => TypeCode.String;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(ToString(), provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(ToString(), provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToString(), provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(ToString(), provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(ToString(), provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(ToString(), provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(ToString(), provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(ToString(), provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(ToString(), provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(ToString(), provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(ToString(), provider);
        string IConvertible.ToString(IFormatProvider provider) => ToString();
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            (conversionType ?? throw new ArgumentNullException(nameof(conversionType))).IsInstanceOfType(this) ? this :
                Convert.ChangeType(ToString(), conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(ToString(), provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(ToString(), provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(ToString(), provider);

        public static bool operator ==(ByteValues left, ByteValues right) => left.Equals(right);

        public static bool operator !=(ByteValues left, ByteValues right) => !(left == right);

        public static implicit operator ByteValues(byte[] values) => values is null ? null : new(values);

        public static implicit operator byte[](ByteValues values) => values?.ToArray();

        public static implicit operator ByteValues(string text) => text is null ? null : new(text);

        public static implicit operator string(ByteValues values) => values?.ToString();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
