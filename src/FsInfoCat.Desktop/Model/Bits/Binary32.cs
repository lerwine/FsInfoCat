using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace FsInfoCat.Desktop.Model.Bits
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Binary32 : IEquatable<Binary32>, IEquatable<uint>, IEquatable<int>, IComparable<Binary32>, IComparable<uint>, IComparable<int>, IComparable, IReadOnlyList<byte>, IList<byte>, IList, IConvertible
    {
        public const int BYTE_COUNT = 4;

        [FieldOffset(0)]
        private readonly uint _unsigned;
        [FieldOffset(0)]
        private readonly int _signed;
        [FieldOffset(0)]
        private readonly Binary16 _low;
        [FieldOffset(2)]
        private readonly Binary16 _high;
        public uint Unsigned => _unsigned;
        public int Signed => _signed;
        public Binary16 Low => _low;
        public Binary16 High => _high;

        public Binary32(uint value)
        {
            _signed = 0;
            _low = _high = new Binary16(0);
            _unsigned = value;
        }

        public Binary32(int value)
        {
            _unsigned = 0;
            _low = _high = new Binary16(0);
            _signed = value;
        }

        public Binary32(ushort low, ushort high)
        {
            _unsigned = 0;
            _signed = 0;
            _low = new Binary16(low);
            _high = new Binary16(high);
        }

        public Binary32(Binary16 low, Binary16 high)
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

        public byte this[int index]
        {
            get
            {
                switch (index >> 1)
                {
                    case 0:
                        return _low.Low;
                    case 1:
                        return _low.High;
                    case 2:
                        return _high.Low;
                    case 3:
                        return _high.High;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }


        byte IList<byte>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        void ICollection<byte>.Add(byte item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void ICollection<byte>.Clear() => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        public int CompareTo(Binary32 other) => _unsigned.CompareTo(other._unsigned);

        public int CompareTo(uint other) => _unsigned.CompareTo(other);

        public int CompareTo(int other) => _signed.CompareTo(other);

        int IComparable.CompareTo(object obj)
        {
            if (obj is null)
                return 1;
            if (obj is Binary32 b32)
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
                return (l < int.MinValue) ? 1 : ((l > int.MaxValue) ? -1 : _signed.CompareTo((int)l));
            if (obj is ulong ul)
                return (ul > uint.MaxValue) ? -1 : _unsigned.CompareTo((uint)ul);
            if (obj is float f)
                return (f < uint.MinValue) ? 1 : ((f > int.MaxValue) ? -1 : _signed.CompareTo((int)f));
            if (obj is double d)
                return (d < uint.MinValue) ? 1 : ((d > int.MaxValue) ? -1 : _signed.CompareTo((int)d));
            if (obj is decimal m)
                return (m < uint.MinValue) ? 1 : ((m > int.MaxValue) ? -1 : _signed.CompareTo((int)m));
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
                            return (m < uint.MinValue) ? 1 : ((m > int.MaxValue) ? -1 : _signed.CompareTo((int)m));
                        case TypeCode.Double:
                            d = x.ToDouble(CultureInfo.CurrentUICulture);
                            return (d < uint.MinValue) ? 1 : ((d > int.MaxValue) ? -1 : _signed.CompareTo((int)d));
                        case TypeCode.Int16:
                            return _signed.CompareTo(x.ToInt16(CultureInfo.CurrentUICulture));
                        case TypeCode.Int32:
                            return _signed.CompareTo(x.ToInt32(CultureInfo.CurrentUICulture));
                        case TypeCode.Int64:
                            l = x.ToInt64(CultureInfo.CurrentUICulture);
                            return (l < int.MinValue) ? 1 : ((l > int.MaxValue) ? -1 : _signed.CompareTo((int)l));
                        case TypeCode.SByte:
                            return _signed.CompareTo(x.ToSByte(CultureInfo.CurrentUICulture));
                        case TypeCode.Single:
                            f = x.ToSingle(CultureInfo.CurrentUICulture);
                            return (f < uint.MinValue) ? 1 : ((f > int.MaxValue) ? -1 : _signed.CompareTo((int)f));
                        case TypeCode.String:
                            return ToString().CompareTo(x.ToString(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt16:
                            return _unsigned.CompareTo(x.ToUInt16(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt32:
                            return _unsigned.CompareTo(x.ToUInt32(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt64:
                            ul = x.ToUInt64(CultureInfo.CurrentUICulture);
                            return (ul > uint.MaxValue) ? -1 : _unsigned.CompareTo((uint)ul);
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
            _high.CopyTo(array, arrayIndex + Binary16.BYTE_COUNT);
        }

        void ICollection.CopyTo(Array array, int index) => ToArray().CopyTo(array, index);

        public static Binary32 Create(byte[] buffer, int index)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || (index + BYTE_COUNT) > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Binary32(Binary16.Create(buffer, index), Binary16.Create(buffer, index + Binary16.BYTE_COUNT));
        }

        public bool Equals(Binary32 other) => _unsigned.Equals(other._unsigned);

        public bool Equals(uint other) => _unsigned.Equals(other);

        public bool Equals(int other) => _signed.Equals(other);

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Binary32 b)
                return _unsigned.Equals(b._unsigned);
            if (obj is uint u)
                return _unsigned.Equals(u);
            return obj is int s && _signed.Equals(s);
        }

        public IEnumerable<byte> GetBytes()
        {
            yield return _low.Low;
            yield return _low.High;
            yield return _high.Low;
            yield return _high.High;
        }

        public IEnumerator<byte> GetEnumerator() => GetBytes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ToArray().GetEnumerator();

        public override int GetHashCode() => _signed;

        TypeCode IConvertible.GetTypeCode() => TypeCode.UInt32;

        public int IndexOf(byte item)
        {
            int index = _low.IndexOf(item);
            return (index < 0 && (index = _high.IndexOf(item)) > -1) ? index + Binary16.BYTE_COUNT : index;
        }

        int IList.IndexOf(object value) => (value is byte b) ? IndexOf(b) : -1;

        void IList<byte>.Insert(int index, byte item) => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        bool ICollection<byte>.Remove(byte item) => throw new NotSupportedException();

        void IList<byte>.RemoveAt(int index) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public byte[] ToArray() => new byte[] { _low.Low, _low.High, _high.Low, _high.High };

        bool IConvertible.ToBoolean(IFormatProvider provider) => _unsigned != 0;

        byte IConvertible.ToByte(IFormatProvider provider) => (byte)_unsigned;

        char IConvertible.ToChar(IFormatProvider provider) => (char)_unsigned;

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => new DateTime(_unsigned);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => _unsigned;

        double IConvertible.ToDouble(IFormatProvider provider) => _unsigned;

        short IConvertible.ToInt16(IFormatProvider provider) => (short)_signed;

        int IConvertible.ToInt32(IFormatProvider provider) => _signed;

        long IConvertible.ToInt64(IFormatProvider provider) => _signed;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte)_signed;

        float IConvertible.ToSingle(IFormatProvider provider) => _unsigned;

        string IConvertible.ToString(IFormatProvider provider) => (provider is null) ? ToString() : _unsigned.ToString(provider);

        public override string ToString() => _unsigned.ToString("X8");

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (typeof(Binary32).IsAssignableFrom(conversionType))
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
                return _unsigned;
            if (conversionType.Equals(typeof(int)))
                return _signed;
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

        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)_unsigned;

        uint IConvertible.ToUInt32(IFormatProvider provider) => _unsigned;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => _unsigned;

        #region Operators

        public static Binary32 operator ~(Binary32 value) => new Binary32(~value._unsigned);

        public static Binary32 operator &(Binary32 left, Binary32 right) => new Binary32(left._unsigned & right._unsigned);

        public static Binary32 operator |(Binary32 left, Binary32 right) => new Binary32(left._unsigned | right._unsigned);

        public static Binary32 operator ^(Binary32 left, Binary32 right) => new Binary32(left._unsigned ^ right._unsigned);

        public static Binary32 operator <<(Binary32 value, int shift) => new Binary32(value._unsigned << shift);

        public static Binary32 operator >>(Binary32 value, int shift) => new Binary32(value._unsigned >> shift);

        public static bool operator ==(Binary32 left, Binary32 right) => left._unsigned == right._unsigned;

        public static bool operator ==(Binary32 left, int right) => left._signed == right;

        public static bool operator ==(int left, Binary32 right) => left == right._signed;

        public static bool operator ==(uint left, Binary32 right) => left == right._unsigned;

        public static bool operator ==(Binary32 left, uint right) => left._unsigned == right;

        public static bool operator !=(Binary32 left, Binary32 right) => left._unsigned != right._unsigned;

        public static bool operator !=(Binary32 left, int right) => left._signed != right;

        public static bool operator !=(Binary32 left, uint right) => left._unsigned != right;

        public static bool operator !=(int left, Binary32 right) => left != right._signed;

        public static bool operator !=(uint left, Binary32 right) => left != right._unsigned;

        public static bool operator <(Binary32 left, uint right) => left._unsigned < right;

        public static bool operator <(uint left, Binary32 right) => left < right._unsigned;

        public static bool operator <(Binary32 left, int right) => left._signed < right;

        public static bool operator <(Binary32 left, Binary32 right) => left._unsigned < right._unsigned;

        public static bool operator <(int left, Binary32 right) => left < right._signed;

        public static bool operator >(int left, Binary32 right) => left > right._signed;

        public static bool operator >(Binary32 left, int right) => left._signed > right;

        public static bool operator >(Binary32 left, uint right) => left._unsigned > right;

        public static bool operator >(Binary32 left, Binary32 right) => left._unsigned > right._unsigned;

        public static bool operator >(uint left, Binary32 right) => left > right._unsigned;

        public static bool operator <=(Binary32 left, Binary32 right) => left._unsigned <= right._unsigned;

        public static bool operator <=(Binary32 left, int right) => left._signed <= right;

        public static bool operator <=(Binary32 left, uint right) => left._unsigned <= right;

        public static bool operator <=(uint left, Binary32 right) => left <= right._unsigned;

        public static bool operator <=(int left, Binary32 right) => left <= right._signed;

        public static bool operator >=(Binary32 left, uint right) => left._unsigned >= right;

        public static bool operator >=(int left, Binary32 right) => left >= right._signed;

        public static bool operator >=(Binary32 left, Binary32 right) => left._unsigned >= right._unsigned;

        public static bool operator >=(Binary32 left, int right) => left._signed >= right;

        public static bool operator >=(uint left, Binary32 right) => left >= right._unsigned;

        public static implicit operator Binary32(byte value) => new Binary32((uint)value);

        public static implicit operator Binary32(sbyte value) => new Binary32(value);

        public static implicit operator Binary32(ushort value) => new Binary32((uint)value);

        public static implicit operator Binary32(short value) => new Binary32(value);

        public static implicit operator Binary32(int value) => new Binary32(value);

        public static implicit operator Binary32(uint value) => new Binary32(value);

        public static implicit operator Binary32(long value) => new Binary32((int)value);

        public static implicit operator Binary32(ulong value) => new Binary32((uint)value);

        public static explicit operator Binary32(float value) => (value < 0f) ? new Binary32((int)value) : new Binary32((uint)value);

        public static explicit operator Binary32(double value) => (value < 0.0) ? new Binary32((int)value) : new Binary32((uint)value);

        public static explicit operator Binary32(decimal value) => (value < 0m) ? new Binary32((int)value) : new Binary32((uint)value);

        public static explicit operator int(Binary32 value) => value._signed;

        public static explicit operator uint(Binary32 value) => value._unsigned;

        public static explicit operator long(Binary32 value) => value._signed;

        public static explicit operator ulong(Binary32 value) => value._unsigned;

        public static explicit operator float(Binary32 value) => value._unsigned;

        public static explicit operator double(Binary32 value) => value._unsigned;

        public static explicit operator decimal(Binary32 value) => value._unsigned;

        #endregion
    }
}
