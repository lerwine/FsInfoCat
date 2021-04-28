using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace FsInfoCat.Desktop.Model.Bits
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Binary64 : IEquatable<Binary64>, IEquatable<ulong>, IEquatable<long>, IComparable<Binary64>, IComparable<ulong>, IComparable<long>, IComparable, IReadOnlyList<byte>, IList<byte>, IList, IConvertible
    {
        public const int BYTE_COUNT = 8;

        [FieldOffset(0)]
        private readonly ulong _unsigned;
        [FieldOffset(0)]
        private readonly long _signed;
        [FieldOffset(0)]
        private readonly Binary32 _low;
        [FieldOffset(4)]
        private readonly Binary32 _high;
        public ulong Unsigned => _unsigned;
        public long Signed => _signed;
        public Binary32 Low => _low;
        public Binary32 High => _high;

        public Binary64(ulong value)
        {
            _signed = 0;
            _low = _high = new Binary32(0);
            _unsigned = value;
        }

        public Binary64(long value)
        {
            _unsigned = 0;
            _low = _high = new Binary32(0);
            _signed = value;
        }

        public Binary64(ushort low, ushort high)
        {
            _unsigned = 0;
            _signed = 0;
            _low = new Binary32(low);
            _high = new Binary32(high);
        }

        public Binary64(Binary32 low, Binary32 high)
        {
            _unsigned = 0;
            _signed = 0;
            _low = low;
            _high = high;
        }

        int IReadOnlyCollection<byte>.Count => BYTE_COUNT;

        int ICollection<byte>.Count => BYTE_COUNT;

        int ICollection.Count => BYTE_COUNT;

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        bool ICollection<byte>.IsReadOnly => true;

        object ICollection.SyncRoot => null;

        bool ICollection.IsSynchronized => false;

        public byte this[int index] => (index < Binary32.BYTE_COUNT) ? _low[index] : _high[index - Binary32.BYTE_COUNT];

        byte IList<byte>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        void ICollection<byte>.Add(byte item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void ICollection<byte>.Clear() => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        public int CompareTo(Binary64 other) => _unsigned.CompareTo(other._unsigned);

        public int CompareTo(ulong other) => _unsigned.CompareTo(other);

        public int CompareTo(long other) => _signed.CompareTo(other);

        int IComparable.CompareTo(object obj)
        {
            if (obj is null)
                return 1;
            if (obj is Binary64 b32)
                return _unsigned.CompareTo(b32._unsigned);
            if (obj is ushort us)
                return _unsigned.CompareTo(us);
            if (obj is short sv)
                return _signed.CompareTo(sv);
            if (obj is byte b)
                return _unsigned.CompareTo(b);
            if (obj is sbyte sb)
                return _signed.CompareTo(sb);
            if (obj is int i)
                return _signed.CompareTo(i);
            if (obj is uint u)
                return _unsigned.CompareTo(u);
            if (obj is long l)
                return _signed.CompareTo(l);
            if (obj is ulong ul)
                return _unsigned.CompareTo(ul);
            if (obj is float f)
                return (f < ulong.MinValue) ? 1 : ((f > long.MaxValue) ? -1 : _signed.CompareTo((long)f));
            if (obj is double d)
                return (d < ulong.MinValue) ? 1 : ((d > long.MaxValue) ? -1 : _signed.CompareTo((long)d));
            if (obj is decimal m)
                return (m < ulong.MinValue) ? 1 : ((m > long.MaxValue) ? -1 : _signed.CompareTo((long)m));
            if (obj is string s)
                return ToString().CompareTo(s);
            if (obj is bool v)
                return (_unsigned != 0UL).CompareTo(v);
            if (obj is char c)
                return ToString().CompareTo(new string(c, 1));
            if (obj is DateTime t)
                return _unsigned.CompareTo(t.Ticks);
            if (obj is IConvertible x)
                try
                {
                    switch (x.GetTypeCode())
                    {
                        case TypeCode.Boolean:
                            return (_unsigned != 0UL).CompareTo(x.ToBoolean(CultureInfo.CurrentUICulture));
                        case TypeCode.Byte:
                            return _unsigned.CompareTo(x.ToByte(CultureInfo.CurrentUICulture));
                        case TypeCode.Char:
                            return ToString().CompareTo(new string(x.ToChar(CultureInfo.CurrentUICulture), 1));
                        case TypeCode.DateTime:
                            return _unsigned.CompareTo(x.ToDateTime(CultureInfo.CurrentUICulture).Ticks);
                        case TypeCode.Empty:
                        case TypeCode.DBNull:
                            return 1;
                        case TypeCode.Decimal:
                            m = x.ToDecimal(CultureInfo.CurrentUICulture);
                            return (m < ulong.MinValue) ? 1 : ((m > long.MaxValue) ? -1 : _signed.CompareTo((long)m));
                        case TypeCode.Double:
                            d = x.ToDouble(CultureInfo.CurrentUICulture);
                            return (d < ulong.MinValue) ? 1 : ((d > long.MaxValue) ? -1 : _signed.CompareTo((long)d));
                        case TypeCode.Int16:
                            return _signed.CompareTo(x.ToInt16(CultureInfo.CurrentUICulture));
                        case TypeCode.Int32:
                            return _signed.CompareTo(x.ToInt32(CultureInfo.CurrentUICulture));
                        case TypeCode.Int64:
                            return _signed.CompareTo(x.ToInt64(CultureInfo.CurrentUICulture));
                        case TypeCode.SByte:
                            return _signed.CompareTo(x.ToSByte(CultureInfo.CurrentUICulture));
                        case TypeCode.Single:
                            f = x.ToSingle(CultureInfo.CurrentUICulture);
                            return (f < ulong.MinValue) ? 1 : ((f > long.MaxValue) ? -1 : _signed.CompareTo((long)f));
                        case TypeCode.String:
                            return ToString().CompareTo(x.ToString(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt16:
                            return _unsigned.CompareTo(x.ToUInt16(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt32:
                            return _unsigned.CompareTo(x.ToUInt32(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt64:
                            return _unsigned.CompareTo(x.ToUInt64(CultureInfo.CurrentUICulture));
                    }
                }
                catch { /* Ignore exception if conversion fails */ }
            return -1;
        }

        public bool Contains(byte item) => _low.Contains(item) || _high.Contains(item);

        bool IList.Contains(object value) => value is byte b && Contains(b);

        public void CopyTo(byte[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || (arrayIndex + 2) > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            _low.CopyTo(array, arrayIndex);
            _high.CopyTo(array, arrayIndex + Binary32.BYTE_COUNT);
        }

        void ICollection.CopyTo(Array array, int index) => ToArray().CopyTo(array, index);

        public static Binary64 Create(byte[] buffer, int index)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || (index + BYTE_COUNT) > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Binary64(Binary32.Create(buffer, index), Binary32.Create(buffer, index + Binary32.BYTE_COUNT));
        }

        public bool Equals(Binary64 other) => _unsigned.Equals(other._unsigned);

        public bool Equals(ulong other) => _unsigned.Equals(other);

        public bool Equals(long other) => _signed.Equals(other);

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Binary64 b)
                return _unsigned.Equals(b._unsigned);
            if (obj is ulong u)
                return _unsigned.Equals(u);
            return obj is long s && _signed.Equals(s);
        }

        public IEnumerable<byte> GetBytes() => _low.GetBytes().Concat(_high.GetBytes());

        public IEnumerator<byte> GetEnumerator() => GetBytes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ToArray().GetEnumerator();

        public override int GetHashCode() => _unsigned.GetHashCode();

        TypeCode IConvertible.GetTypeCode() => TypeCode.UInt64;

        public int IndexOf(byte item)
        {
            int index = _low.IndexOf(item);
            return (index < 0 && (index = _high.IndexOf(item)) > -1) ? index + Binary32.BYTE_COUNT : index;
        }

        int IList.IndexOf(object value) => (value is byte b) ? IndexOf(b) : -1;

        void IList<byte>.Insert(int index, byte item) => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        bool ICollection<byte>.Remove(byte item) => throw new NotSupportedException();

        void IList<byte>.RemoveAt(int index) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public byte[] ToArray() => GetBytes().ToArray();

        bool IConvertible.ToBoolean(IFormatProvider provider) => _unsigned != 0;

        byte IConvertible.ToByte(IFormatProvider provider) => (byte)_unsigned;

        char IConvertible.ToChar(IFormatProvider provider) => (char)_unsigned;

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => new DateTime(_signed);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => _unsigned;

        double IConvertible.ToDouble(IFormatProvider provider) => _unsigned;

        short IConvertible.ToInt16(IFormatProvider provider) => (short)_signed;

        int IConvertible.ToInt32(IFormatProvider provider) => (int)_signed;

        long IConvertible.ToInt64(IFormatProvider provider) => _signed;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)_signed;

        float IConvertible.ToSingle(IFormatProvider provider) => _unsigned;

        string IConvertible.ToString(IFormatProvider provider) => (provider is null) ? ToString() : _unsigned.ToString(provider);

        public override string ToString() => _unsigned.ToString("X8");

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (typeof(Binary64).IsAssignableFrom(conversionType))
                return this;
            if (conversionType.Equals(typeof(byte)))
                return (byte)_unsigned;
            if (conversionType.Equals(typeof(sbyte)))
                return (sbyte)_signed;
            if (conversionType.Equals(typeof(ushort)))
                return (ushort)_unsigned;
            if (conversionType.Equals(typeof(short)))
                return (short)_signed;
            if (conversionType.Equals(typeof(uint)))
                return (uint)_unsigned;
            if (conversionType.Equals(typeof(int)))
                return (int)_signed;
            if (conversionType.Equals(typeof(ulong)))
                return _unsigned;
            if (conversionType.Equals(typeof(long)))
                return _signed;
            if (conversionType.Equals(typeof(float)))
                return (float)_unsigned;
            if (conversionType.Equals(typeof(decimal)))
                return (decimal)_unsigned;
            if (conversionType.Equals(typeof(double)))
                return (double)_unsigned;
            if (conversionType.Equals(typeof(string)))
                return (provider is null) ? ToString() : _unsigned.ToString(provider);
            if (conversionType.Equals(typeof(bool)))
                return _unsigned != 0UL;
            if (conversionType.Equals(typeof(DateTime)))
                return new DateTime(_signed);
            return Convert.ChangeType(_unsigned, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)_unsigned;

        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)_unsigned;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => _unsigned;

        #region Operators

        public static Binary64 operator ~(Binary64 value) => new Binary64(~value._unsigned);

        public static Binary64 operator &(Binary64 left, Binary64 right) => new Binary64(left._unsigned & right._unsigned);

        public static Binary64 operator |(Binary64 left, Binary64 right) => new Binary64(left._unsigned | right._unsigned);

        public static Binary64 operator ^(Binary64 left, Binary64 right) => new Binary64(left._unsigned ^ right._unsigned);

        public static Binary64 operator <<(Binary64 value, int shift) => new Binary64(value._unsigned << shift);

        public static Binary64 operator >>(Binary64 value, int shift) => new Binary64(value._unsigned >> shift);

        public static bool operator ==(Binary64 left, Binary64 right) => left._unsigned == right._unsigned;

        public static bool operator ==(Binary64 left, long right) => left._signed == right;

        public static bool operator ==(long left, Binary64 right) => left == right._signed;

        public static bool operator ==(ulong left, Binary64 right) => left == right._unsigned;

        public static bool operator ==(Binary64 left, ulong right) => left._unsigned == right;

        public static bool operator !=(Binary64 left, Binary64 right) => left._unsigned != right._unsigned;

        public static bool operator !=(Binary64 left, long right) => left._signed != right;

        public static bool operator !=(Binary64 left, ulong right) => left._unsigned != right;

        public static bool operator !=(long left, Binary64 right) => left != right._signed;

        public static bool operator !=(ulong left, Binary64 right) => left != right._unsigned;

        public static bool operator <(Binary64 left, ulong right) => left._unsigned < right;

        public static bool operator <(ulong left, Binary64 right) => left < right._unsigned;

        public static bool operator <(Binary64 left, long right) => left._signed < right;

        public static bool operator <(Binary64 left, Binary64 right) => left._unsigned < right._unsigned;

        public static bool operator <(long left, Binary64 right) => left < right._signed;

        public static bool operator >(long left, Binary64 right) => left > right._signed;

        public static bool operator >(Binary64 left, long right) => left._signed > right;

        public static bool operator >(Binary64 left, ulong right) => left._unsigned > right;

        public static bool operator >(Binary64 left, Binary64 right) => left._unsigned > right._unsigned;

        public static bool operator >(ulong left, Binary64 right) => left > right._unsigned;

        public static bool operator <=(Binary64 left, Binary64 right) => left._unsigned <= right._unsigned;

        public static bool operator <=(Binary64 left, long right) => left._signed <= right;

        public static bool operator <=(Binary64 left, ulong right) => left._unsigned <= right;

        public static bool operator <=(ulong left, Binary64 right) => left <= right._unsigned;

        public static bool operator <=(long left, Binary64 right) => left <= right._signed;

        public static bool operator >=(Binary64 left, ulong right) => left._unsigned >= right;

        public static bool operator >=(long left, Binary64 right) => left >= right._signed;

        public static bool operator >=(Binary64 left, Binary64 right) => left._unsigned >= right._unsigned;

        public static bool operator >=(Binary64 left, long right) => left._signed >= right;

        public static bool operator >=(ulong left, Binary64 right) => left >= right._unsigned;

        public static implicit operator Binary64(byte value) => new Binary64((ulong)value);

        public static implicit operator Binary64(sbyte value) => new Binary64(value);

        public static implicit operator Binary64(ushort value) => new Binary64((ulong)value);

        public static implicit operator Binary64(short value) => new Binary64(value);

        public static implicit operator Binary64(int value) => new Binary64(value);

        public static implicit operator Binary64(uint value) => new Binary64((ulong)value);

        public static implicit operator Binary64(long value) => new Binary64(value);

        public static implicit operator Binary64(ulong value) => new Binary64(value);

        public static explicit operator Binary64(float value) => (value < 0f) ? new Binary64((long)value) : new Binary64((ulong)value);

        public static explicit operator Binary64(double value) => (value < 0.0) ? new Binary64((long)value) : new Binary64((ulong)value);

        public static explicit operator Binary64(decimal value) => (value < 0m) ? new Binary64((long)value) : new Binary64((ulong)value);

        public static explicit operator long(Binary64 value) => value._signed;

        public static explicit operator ulong(Binary64 value) => value._unsigned;

        public static explicit operator float(Binary64 value) => value._signed;

        public static explicit operator double(Binary64 value) => value._signed;

        public static explicit operator decimal(Binary64 value) => value._signed;

        #endregion
    }
}
