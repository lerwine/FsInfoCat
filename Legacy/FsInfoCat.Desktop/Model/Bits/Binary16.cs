using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace FsInfoCat.Desktop.Model.Bits
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Binary16 : IEquatable<Binary16>, IEquatable<ushort>, IEquatable<short>, IComparable<Binary16>, IComparable<ushort>, IComparable<short>, IComparable, IReadOnlyList<byte>, IList<byte>, IList, IConvertible
    {
        public const int BYTE_COUNT = 2;

        [FieldOffset(0)]
        private readonly ushort _unsigned;
        [FieldOffset(0)]
        private readonly short _signed;
        [FieldOffset(0)]
        private readonly byte _low;
        [FieldOffset(1)]
        private readonly byte _high;
        public ushort Unsigned => _unsigned;
        public short Signed => _signed;
        public byte Low => _low;
        public byte High => _high;

        public Binary16(ushort value)
        {
            _signed = 0;
            _low = _high = 0;
            _unsigned = value;
        }

        public Binary16(short value)
        {
            _unsigned = 0;
            _low = _high = 0;
            _signed = value;
        }

        public Binary16(byte low, byte high)
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

        byte IReadOnlyList<byte>.this[int index] => Get(index);

        byte IList<byte>.this[int index] { get => Get(index); set => throw new NotSupportedException(); }

        object IList.this[int index] { get => Get(index); set => throw new NotSupportedException(); }

        void ICollection<byte>.Add(byte item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void ICollection<byte>.Clear() => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        public int CompareTo(Binary16 other) => _unsigned.CompareTo(other._unsigned);

        public int CompareTo(ushort other) => _unsigned.CompareTo(other);

        public int CompareTo(short other) => _signed.CompareTo(other);

        int IComparable.CompareTo(object obj)
        {
            if (obj is null)
                return 1;
            if (obj is Binary16 b16)
                return _unsigned.CompareTo(b16._unsigned);
            if (obj is ushort us)
                return _unsigned.CompareTo(us);
            if (obj is short sv)
                return _signed.CompareTo(sv);
            if (obj is byte b)
                return _unsigned.CompareTo(b);
            if (obj is sbyte sb)
                return _signed.CompareTo(sb);
            if (obj is int i)
                return (i < short.MinValue) ? 1 : ((i > short.MaxValue) ? -1 : _signed.CompareTo((short)i));
            if (obj is uint u)
                return (u > ushort.MaxValue) ? -1 : _unsigned.CompareTo((ushort)u);
            if (obj is long l)
                return (l < short.MinValue) ? 1 : ((l > short.MaxValue) ? -1 : _signed.CompareTo((short)l));
            if (obj is ulong ul)
                return (ul > ushort.MaxValue) ? -1 : _unsigned.CompareTo((ushort)ul);
            if (obj is float f)
                return (f < ushort.MinValue) ? 1 : ((f > short.MaxValue) ? -1 : _signed.CompareTo((short)f));
            if (obj is double d)
                return (d < ushort.MinValue) ? 1 : ((d > short.MaxValue) ? -1 : _signed.CompareTo((short)d));
            if (obj is decimal m)
                return (m < ushort.MinValue) ? 1 : ((m > short.MaxValue) ? -1 : _signed.CompareTo((short)m));
            if (obj is string s)
                return ToString().CompareTo(s);
            if (obj is bool v)
                return (_unsigned != 0).CompareTo(v);
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
                            return (_unsigned != 0).CompareTo(x.ToBoolean(CultureInfo.CurrentUICulture));
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
                            return (m < ushort.MinValue) ? 1 : ((m > short.MaxValue) ? -1 : _signed.CompareTo((short)m));
                        case TypeCode.Double:
                            d = x.ToDouble(CultureInfo.CurrentUICulture);
                            return (d < ushort.MinValue) ? 1 : ((d > short.MaxValue) ? -1 : _signed.CompareTo((short)d));
                        case TypeCode.Int16:
                            return _signed.CompareTo(x.ToInt16(CultureInfo.CurrentUICulture));
                        case TypeCode.Int32:
                            i = x.ToInt32(CultureInfo.CurrentUICulture);
                            return (i < short.MinValue) ? 1 : ((i > short.MaxValue) ? -1 : _signed.CompareTo((short)i));
                        case TypeCode.Int64:
                            l = x.ToInt64(CultureInfo.CurrentUICulture);
                            return (l < short.MinValue) ? 1 : ((l > short.MaxValue) ? -1 : _signed.CompareTo((short)l));
                        case TypeCode.SByte:
                            return _signed.CompareTo(x.ToSByte(CultureInfo.CurrentUICulture));
                        case TypeCode.Single:
                            f = x.ToSingle(CultureInfo.CurrentUICulture);
                            return (f < ushort.MinValue) ? 1 : ((f > short.MaxValue) ? -1 : _signed.CompareTo((short)f));
                        case TypeCode.String:
                            return ToString().CompareTo(x.ToString(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt16:
                            return _unsigned.CompareTo(x.ToUInt16(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt32:
                            u = x.ToUInt32(CultureInfo.CurrentUICulture);
                            return (u > ushort.MaxValue) ? -1 : _unsigned.CompareTo((ushort)u);
                        case TypeCode.UInt64:
                            ul = x.ToUInt64(CultureInfo.CurrentUICulture);
                            return (ul > ushort.MaxValue) ? -1 : _unsigned.CompareTo((ushort)ul);
                    }
                }
                catch { /* Ignore exception if conversion fails */ }
            return -1;
        }

        public bool Contains(byte item) => item == _low || item == _high;

        bool IList.Contains(object value) => value is byte b && Contains(b);

        public void CopyTo(byte[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || (arrayIndex + BYTE_COUNT) > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            array[arrayIndex] = _low;
            array[arrayIndex + 1] = _high;
        }

        void ICollection.CopyTo(Array array, int index) => ToArray().CopyTo(array, index);

        public static Binary16 Create(byte[] buffer, int index)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || (index + 2) > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Binary16(buffer[index], buffer[index + 1]);
        }

        public bool Equals(Binary16 other) => _unsigned.Equals(other._unsigned);

        public bool Equals(ushort other) => _unsigned.Equals(other);

        public bool Equals(short other) => _signed.Equals(other);

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Binary16 b)
                return _unsigned.Equals(b._unsigned);
            if (obj is ushort u)
                return _unsigned.Equals(u);
            return obj is short s && _signed.Equals(s);
        }

        private byte Get(int index)
        {
            switch (index)
            {
                case 0:
                    return _low;
                case 1:
                    return _high;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public IEnumerable<byte> GetBytes()
        {
            yield return _low;
            yield return _high;
        }

        public IEnumerator<byte> GetEnumerator() => GetBytes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ToArray().GetEnumerator();

        public override int GetHashCode() => _signed;

        TypeCode IConvertible.GetTypeCode() => TypeCode.UInt16;

        public int IndexOf(byte item) => item.Equals(_low) ? 0 : (item.Equals(_high) ? 1 : -1);

        int IList.IndexOf(object value) => (value is byte b) ? IndexOf(b) : -1;

        void IList<byte>.Insert(int index, byte item) => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        bool ICollection<byte>.Remove(byte item) => throw new NotSupportedException();

        void IList<byte>.RemoveAt(int index) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public byte[] ToArray() => new byte[] { _low, _high };

        bool IConvertible.ToBoolean(IFormatProvider provider) => _unsigned != 0;

        byte IConvertible.ToByte(IFormatProvider provider) => (byte)_unsigned;

        char IConvertible.ToChar(IFormatProvider provider) => (char)_unsigned;

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => new DateTime(_unsigned);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => _unsigned;

        double IConvertible.ToDouble(IFormatProvider provider) => _unsigned;

        short IConvertible.ToInt16(IFormatProvider provider) => _signed;

        int IConvertible.ToInt32(IFormatProvider provider) => _signed;

        long IConvertible.ToInt64(IFormatProvider provider) => _signed;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)_signed;

        float IConvertible.ToSingle(IFormatProvider provider) => _unsigned;

        string IConvertible.ToString(IFormatProvider provider) => (provider is null) ? ToString() : _unsigned.ToString(provider);

        public override string ToString() => _unsigned.ToString("X4");

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (typeof(Binary16).IsAssignableFrom(conversionType))
                return this;
            if (conversionType.Equals(typeof(byte)))
                return (byte)_unsigned;
            if (conversionType.Equals(typeof(sbyte)))
                return (sbyte)_signed;
            if (conversionType.Equals(typeof(ushort)))
                return _unsigned;
            if (conversionType.Equals(typeof(short)))
                return _signed;
            if (conversionType.Equals(typeof(uint)))
                return (uint)_unsigned;
            if (conversionType.Equals(typeof(int)))
                return (int)_signed;
            if (conversionType.Equals(typeof(ulong)))
                return (ulong)_unsigned;
            if (conversionType.Equals(typeof(long)))
                return (long)_signed;
            if (conversionType.Equals(typeof(float)))
                return (float)_unsigned;
            if (conversionType.Equals(typeof(decimal)))
                return (decimal)_unsigned;
            if (conversionType.Equals(typeof(double)))
                return (double)_unsigned;
            if (conversionType.Equals(typeof(string)))
                return (provider is null) ? ToString() : _unsigned.ToString(provider);
            if (conversionType.Equals(typeof(bool)))
                return _unsigned != 0;
            if (conversionType.Equals(typeof(DateTime)))
                return new DateTime(_unsigned);
            return Convert.ChangeType(_unsigned, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) => _unsigned;

        uint IConvertible.ToUInt32(IFormatProvider provider) => _unsigned;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => _unsigned;

        #region Operators

        public static Binary16 operator ~(Binary16 value) => new Binary16((ushort)~value._unsigned);

        public static Binary16 operator &(Binary16 left, Binary16 right) => new Binary16((ushort)(left._unsigned & right._unsigned));

        public static Binary16 operator |(Binary16 left, Binary16 right) => new Binary16((ushort)(left._unsigned | right._unsigned));

        public static Binary16 operator ^(Binary16 left, Binary16 right) => new Binary16((ushort)(left._unsigned ^ right._unsigned));

        public static Binary16 operator <<(Binary16 value, int shift) => new Binary16((ushort)(value._unsigned << shift));

        public static Binary16 operator >>(Binary16 value, int shift) => new Binary16((ushort)(value._unsigned >> shift));

        public static bool operator ==(Binary16 left, Binary16 right) => left._unsigned == right._unsigned;

        public static bool operator ==(Binary16 left, short right) => left._signed == right;

        public static bool operator ==(short left, Binary16 right) => left == right._signed;

        public static bool operator ==(ushort left, Binary16 right) => left == right._unsigned;

        public static bool operator ==(Binary16 left, ushort right) => left._unsigned == right;

        public static bool operator !=(Binary16 left, Binary16 right) => left._unsigned != right._unsigned;

        public static bool operator !=(Binary16 left, short right) => left._signed != right;

        public static bool operator !=(Binary16 left, ushort right) => left._unsigned != right;

        public static bool operator !=(short left, Binary16 right) => left != right._signed;

        public static bool operator !=(ushort left, Binary16 right) => left != right._unsigned;

        public static bool operator <(Binary16 left, ushort right) => left._unsigned < right;

        public static bool operator <(ushort left, Binary16 right) => left < right._unsigned;

        public static bool operator <(Binary16 left, short right) => left._signed < right;

        public static bool operator <(Binary16 left, Binary16 right) => left._unsigned < right._unsigned;

        public static bool operator <(short left, Binary16 right) => left < right._signed;

        public static bool operator >(short left, Binary16 right) => left > right._signed;

        public static bool operator >(Binary16 left, short right) => left._signed > right;

        public static bool operator >(Binary16 left, ushort right) => left._unsigned > right;

        public static bool operator >(Binary16 left, Binary16 right) => left._unsigned > right._unsigned;

        public static bool operator >(ushort left, Binary16 right) => left > right._unsigned;

        public static bool operator <=(Binary16 left, Binary16 right) => left._unsigned <= right._unsigned;

        public static bool operator <=(Binary16 left, short right) => left._signed <= right;

        public static bool operator <=(Binary16 left, ushort right) => left._unsigned <= right;

        public static bool operator <=(ushort left, Binary16 right) => left <= right._unsigned;

        public static bool operator <=(short left, Binary16 right) => left <= right._signed;

        public static bool operator >=(Binary16 left, ushort right) => left._unsigned >= right;

        public static bool operator >=(short left, Binary16 right) => left >= right._signed;

        public static bool operator >=(Binary16 left, Binary16 right) => left._unsigned >= right._unsigned;

        public static bool operator >=(Binary16 left, short right) => left._signed >= right;

        public static bool operator >=(ushort left, Binary16 right) => left >= right._unsigned;

        public static implicit operator Binary16(byte value) => new Binary16((ushort)value);

        public static implicit operator Binary16(sbyte value) => new Binary16(value);

        public static implicit operator Binary16(ushort value) => new Binary16(value);

        public static implicit operator Binary16(short value) => new Binary16(value);

        public static implicit operator Binary16(int value) => new Binary16((short)value);

        public static implicit operator Binary16(uint value) => new Binary16((ushort)value);

        public static implicit operator Binary16(long value) => new Binary16((short)value);

        public static implicit operator Binary16(ulong value) => new Binary16((ushort)value);

        public static explicit operator Binary16(float value) => (value < 0f) ? new Binary16((short)value) : new Binary16((ushort)value);

        public static explicit operator Binary16(double value) => (value < 0.0) ? new Binary16((short)value) : new Binary16((ushort)value);

        public static explicit operator Binary16(decimal value) => (value < 0m) ? new Binary16((short)value) : new Binary16((ushort)value);

        public static explicit operator short(Binary16 value) => value._signed;

        public static explicit operator ushort(Binary16 value) => value._unsigned;

        public static explicit operator int(Binary16 value) => value._signed;

        public static explicit operator uint(Binary16 value) => value._unsigned;

        public static explicit operator long(Binary16 value) => value._signed;

        public static explicit operator ulong(Binary16 value) => value._unsigned;

        public static explicit operator float(Binary16 value) => value._signed;

        public static explicit operator double(Binary16 value) => value._signed;

        public static explicit operator decimal(Binary16 value) => value._signed;

        #endregion
    }
}
