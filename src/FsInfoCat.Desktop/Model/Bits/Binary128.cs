using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace FsInfoCat.Desktop.Model.Bits
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Binary128 : IEquatable<Binary128>, IEquatable<Guid>, IComparable<Binary128>, IComparable<Guid>, IComparable, IReadOnlyList<byte>, IList<byte>, IList, IConvertible
    {
        public const int BYTE_COUNT = 16;
        private const ulong SIGNED_MASK = 0x7fffffffffffffff;
        private const ulong NEGATIVE_BIT = 0x8000000000000000;
        private const decimal FIRST_HIGH_BIT_M = 18446744073709551616m;
        private const double FIRST_HIGH_BIT_D = 18446744073709551616.0;
        private const float FIRST_HIGH_BIT_F = 18446744073709551616f;
        private const ulong MAX_DECIMAL_HIGH_BITS = 4294967295UL;
        private const ulong MAX_SINGLE_HIGH_BITS = 18446742974197923840UL;

        [FieldOffset(0)]
        private readonly Guid _guid;
        [FieldOffset(0)]
        private readonly Binary64 _low;
        [FieldOffset(8)]
        private readonly Binary64 _high;
        public Binary64 Low => _low;
        public Binary64 High => _high;
        public Guid UUID => _guid;

        public Binary128(Guid value)
        {
            _low = _high = new Binary64(0);
            _guid = value;
        }

        public Binary128(ulong low, ulong high)
        {
            _guid = Guid.Empty;
            _low = new Binary64(low);
            _high = new Binary64(high);
        }

        public Binary128(Binary64 low, Binary64 high)
        {
            _guid = Guid.Empty;
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

        public byte this[int index] => (index < Binary64.BYTE_COUNT) ? _low[index] : _high[index - Binary64.BYTE_COUNT];

        byte IList<byte>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        void ICollection<byte>.Add(byte item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void ICollection<byte>.Clear() => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        public int CompareTo(Binary128 other) => _guid.CompareTo(other._guid);

        public int CompareTo(Guid other) => _guid.CompareTo(other);

        int IComparable.CompareTo(object obj)
        {
            if (obj is null)
                return 1;
            if (obj is Binary128 b32)
                return _guid.CompareTo(b32._guid);
            if (obj is ushort us)
                return (_high.Signed == 0) ? _low.Unsigned.CompareTo(us) : 1;
            if (obj is short sv)
                return TryConvertToInt16(out short cv) ? cv.CompareTo(sv) : (IsNegative() ? -1 : 1);
            if (obj is byte b)
                return TryConvertToByte(out byte cv) ? cv.CompareTo(b) : 1;
            if (obj is sbyte sb)
                return TryConvertToSByte(out sbyte cv) ? cv.CompareTo(sb) : (IsNegative() ? -1 : 1);
            if (obj is int i)
                return TryConvertToInt32(out int cv) ? cv.CompareTo(i) : (IsNegative() ? -1 : 1);
            if (obj is uint u)
                return TryConvertToUInt32(out uint cv) ? cv.CompareTo(u) : 1;
            if (obj is long l)
                return TryConvertToInt64(out long cv) ? cv.CompareTo(l) : (IsNegative() ? -1 : 1);
            if (obj is ulong ul)
                return TryConvertToUInt64(out ulong cv) ? cv.CompareTo(ul) : 1;
            if (obj is float f)
                return TryConvertToSingle(out float cv) ? cv.CompareTo(f) : (IsNegative() ? -1 : 1);
            if (obj is double d)
                return ToDouble().CompareTo(d);
            if (obj is decimal m)
                return TryConvertToDecimal(out decimal cv) ? cv.CompareTo(m) : (IsNegative() ? -1 : 1);
            if (obj is string s)
                return ToString().CompareTo(s);
            if (obj is bool v)
                return (_high.Unsigned != 0UL || _low.Unsigned != 0UL).CompareTo(v);
            if (obj is char c)
                return ToString().CompareTo(new string(c, 1));
            if (obj is DateTime t)
                return TryConvertToDateTime(out DateTime cv) ? cv.CompareTo(t) : (IsNegative() ? -1 : 1);
            if (obj is IConvertible x)
                try
                {
                    switch (x.GetTypeCode())
                    {
                        case TypeCode.Boolean:
                            return (_high.Unsigned != 0UL || _low.Unsigned != 0UL).CompareTo(x.ToBoolean(CultureInfo.CurrentUICulture));
                        case TypeCode.Byte:
                            return CompareTo(x.ToByte(CultureInfo.CurrentUICulture));
                        case TypeCode.Char:
                            return ToString().CompareTo(new string(x.ToChar(CultureInfo.CurrentUICulture), 1));
                        case TypeCode.DateTime:
                            return TryConvertToByte(out byte dt) ? dt.CompareTo(x.ToDateTime(CultureInfo.CurrentUICulture)) : 1;
                        case TypeCode.Empty:
                        case TypeCode.DBNull:
                            return 1;
                        case TypeCode.Decimal:
                            return TryConvertToDecimal(out m) ? m.CompareTo(x.ToDecimal(CultureInfo.CurrentUICulture)) : (IsNegative() ? -1 : 1);
                        case TypeCode.Double:
                            return ToDouble().CompareTo(x.ToDouble(CultureInfo.CurrentUICulture));
                        case TypeCode.Int16:
                            return TryConvertToInt16(out sv) ? sv.CompareTo(x.ToInt16(CultureInfo.CurrentUICulture)) : (IsNegative() ? -1 : 1);
                        case TypeCode.Int32:
                            return TryConvertToInt32(out i) ? i.CompareTo(x.ToInt32(CultureInfo.CurrentUICulture)) : (IsNegative() ? -1 : 1);
                        case TypeCode.Int64:
                            return TryConvertToInt64(out l) ? l.CompareTo(x.ToInt64(CultureInfo.CurrentUICulture)) : (IsNegative() ? -1 : 1);
                        case TypeCode.SByte:
                            return TryConvertToSByte(out sb) ? sb.CompareTo(x.ToSByte(CultureInfo.CurrentUICulture)) : (IsNegative() ? -1 : 1);
                        case TypeCode.Single:
                            return TryConvertToSingle(out f) ? f.CompareTo(x.ToSingle(CultureInfo.CurrentUICulture)) : (IsNegative() ? -1 : 1);
                        case TypeCode.String:
                            return ToString().CompareTo(x.ToString(CultureInfo.CurrentUICulture));
                        case TypeCode.UInt16:
                            return TryConvertToUInt16(out us) ? us.CompareTo(x.ToUInt16(CultureInfo.CurrentUICulture)) : 1;
                        case TypeCode.UInt32:
                            return TryConvertToUInt32(out u) ? u.CompareTo(x.ToUInt32(CultureInfo.CurrentUICulture)) : 1;
                        case TypeCode.UInt64:
                            return TryConvertToUInt64(out ul) ? ul.CompareTo(x.ToUInt64(CultureInfo.CurrentUICulture)) : 1;
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
            _high.CopyTo(array, arrayIndex + Binary64.BYTE_COUNT);
        }

        void ICollection.CopyTo(Array array, int index) => ToArray().CopyTo(array, index);

        public static Binary128 Create(byte[] buffer, int index)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || (index + BYTE_COUNT) > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new Binary128(Binary64.Create(buffer, index), Binary64.Create(buffer, index + Binary64.BYTE_COUNT));
        }

        public bool Equals(Binary128 other) => _low.Equals(other._high) && _low.Equals(other._high);

        public bool Equals(Guid other) => _guid.Equals(other);

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Binary128 b)
                return _low.Equals(b._high) && _low.Equals(b._high);
            return obj is Guid g && _guid.Equals(g);
        }

        public IEnumerable<byte> GetBytes() => _low.GetBytes().Concat(_high.GetBytes());

        public IEnumerator<byte> GetEnumerator() => GetBytes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ToArray().GetEnumerator();

        public override int GetHashCode() => _guid.GetHashCode();

        TypeCode IConvertible.GetTypeCode() => TypeCode.UInt64;

        public int IndexOf(byte item)
        {
            int index = _low.IndexOf(item);
            return (index < 0 && (index = _high.IndexOf(item)) > -1) ? index + Binary64.BYTE_COUNT : index;
        }

        int IList.IndexOf(object value) => (value is byte b) ? IndexOf(b) : -1;

        void IList<byte>.Insert(int index, byte item) => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        bool ICollection<byte>.Remove(byte item) => throw new NotSupportedException();

        void IList<byte>.RemoveAt(int index) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        public byte[] ToArray() => GetBytes().ToArray();

        private bool IsNegative() => (_high.Unsigned & NEGATIVE_BIT) == 0UL;

        private bool TryConvertToByte(out byte result)
        {
            if (_high.Unsigned == 0UL && _low.Unsigned <= byte.MaxValue)
            {
                result = (byte)_low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToSByte(out sbyte result)
        {
            if (_high.Unsigned == NEGATIVE_BIT)
            {
                if (_low.Signed >= 0L)
                {
                    long v = _low.Signed * -1;
                    if (v >= sbyte.MinValue)
                    {
                        result = (sbyte)v;
                        return true;
                    }
                }
            }
            else if (_high.Unsigned == 0UL && _low.Unsigned <= (ulong)sbyte.MaxValue)
            {
                result = (sbyte)_low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToInt16(out short result)
        {
            if (_high.Unsigned == NEGATIVE_BIT)
            {
                if (_low.Signed >= 0L)
                {
                    long v = _low.Signed * -1;
                    if (v >= short.MinValue)
                    {
                        result = (short)v;
                        return true;
                    }
                }
            }
            else if (_high.Unsigned == 0UL && _low.Unsigned <= (ulong)short.MaxValue)
            {
                result = (short)_low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToUInt16(out ushort result)
        {
            if (_high.Unsigned == 0UL && _low.Unsigned <= ushort.MaxValue)
            {
                result = (ushort)_low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToInt32(out int result)
        {
            if (_high.Unsigned == NEGATIVE_BIT)
            {
                if (_low.Signed >= 0L)
                {
                    long v = _low.Signed * -1;
                    if (v >= int.MinValue)
                    {
                        result = (int)v;
                        return true;
                    }
                }
            }
            else if (_high.Unsigned == 0UL && _low.Unsigned <= (ulong)int.MaxValue)
            {
                result = (int)_low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToUInt32(out uint result)
        {
            if (_high.Unsigned == 0UL && _low.Unsigned <= uint.MaxValue)
            {
                result = (uint)_low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToInt64(out long result)
        {
            if (_low.Signed >= 0L)
            {
                if (_high.Unsigned == NEGATIVE_BIT)
                {
                    result = _low.Signed * -1;
                    return true;
                }
                else if (_high.Unsigned == 0UL)
                {
                    result = _low.Signed;
                    return true;
                }
            }
            result = default;
            return false;
        }

        private bool TryConvertToUInt64(out ulong result)
        {
            if (_high.Unsigned == 0UL)
            {
                result = _low.Unsigned;
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToSingle(out float result)
        {
            if (_high.Unsigned <= MAX_SINGLE_HIGH_BITS)
            {
                result = (FIRST_HIGH_BIT_F * Convert.ToSingle(_high.Unsigned)) + Convert.ToSingle(_low.Unsigned);
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToDecimal(out decimal result)
        {
            if (_high.Unsigned <= MAX_DECIMAL_HIGH_BITS)
            {
                result = (FIRST_HIGH_BIT_M * Convert.ToDecimal(_high.Unsigned)) + Convert.ToDecimal(_low.Unsigned);
                return true;
            }
            result = default;
            return false;
        }

        private bool TryConvertToDateTime(out DateTime result)
        {
            if (TryConvertToInt64(out long ticks) && ticks >= DateTime.MinValue.Ticks && ticks <= DateTime.MaxValue.Ticks)
            {
                result = new DateTime(ticks);
                return true;
            }
            result = default;
            return false;
        }

        private double ToDouble() => (FIRST_HIGH_BIT_D * Convert.ToDouble(_high.Unsigned)) + Convert.ToDouble(_low.Unsigned);

        bool IConvertible.ToBoolean(IFormatProvider provider) => _low.Unsigned != 0 || _high.Unsigned != 0;

        byte IConvertible.ToByte(IFormatProvider provider) => TryConvertToByte(out byte result) ? result : throw new OverflowException();

        char IConvertible.ToChar(IFormatProvider provider) => TryConvertToUInt32(out uint result) ? (char)result : throw new OverflowException();

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => TryConvertToDateTime(out DateTime result) ? result : throw new OverflowException();

        decimal IConvertible.ToDecimal(IFormatProvider provider) => TryConvertToDecimal(out decimal result) ? result : throw new OverflowException();

        double IConvertible.ToDouble(IFormatProvider provider) => ToDouble();

        short IConvertible.ToInt16(IFormatProvider provider) => TryConvertToInt16(out short result) ? result : throw new OverflowException();

        int IConvertible.ToInt32(IFormatProvider provider) => TryConvertToInt32(out int result) ? result : throw new OverflowException();

        long IConvertible.ToInt64(IFormatProvider provider) => TryConvertToInt64(out long result) ? result : throw new OverflowException();

        sbyte IConvertible.ToSByte(IFormatProvider provider) => TryConvertToSByte(out sbyte result) ? result : throw new OverflowException();

        float IConvertible.ToSingle(IFormatProvider provider) => TryConvertToSingle(out float result) ? result : throw new OverflowException();

        string IConvertible.ToString(IFormatProvider provider) => _guid.ToString();

        public override string ToString() => _guid.ToString();

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (typeof(Binary128).IsAssignableFrom(conversionType))
                return this;
            if (typeof(Guid).IsAssignableFrom(conversionType))
                return _guid;
            if (conversionType.Equals(typeof(byte)))
                return TryConvertToByte(out byte result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(sbyte)))
                return TryConvertToSByte(out sbyte result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(ushort)))
                return TryConvertToUInt16(out ushort result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(short)))
                return TryConvertToInt16(out short result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(uint)))
                return TryConvertToUInt32(out uint result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(int)))
                return TryConvertToInt32(out int result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(ulong)))
                return TryConvertToUInt64(out ulong result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(long)))
                return TryConvertToInt64(out long result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(float)))
                return TryConvertToSingle(out float result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(decimal)))
                return TryConvertToDecimal(out decimal result) ? result : throw new OverflowException();
            if (conversionType.Equals(typeof(double)))
                return ToDouble();
            if (conversionType.Equals(typeof(string)))
                return _guid.ToString();
            if (conversionType.Equals(typeof(bool)))
                return _low.Unsigned != 0 || _high.Unsigned != 0;
            if (conversionType.Equals(typeof(DateTime)))
                return TryConvertToDateTime(out DateTime result) ? result : throw new OverflowException();
            return Convert.ChangeType(_guid, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) => TryConvertToUInt16(out ushort result) ? result : throw new OverflowException();

        uint IConvertible.ToUInt32(IFormatProvider provider) => TryConvertToUInt32(out uint result) ? result : throw new OverflowException();

        ulong IConvertible.ToUInt64(IFormatProvider provider) => TryConvertToUInt64(out ulong result) ? result : throw new OverflowException();

        #region Operators

        public static Binary128 operator ~(Binary128 value) => new Binary128(~value._low, ~value._high);

        public static Binary128 operator &(Binary128 left, Binary128 right) => new Binary128(left._low & right._low, left._high & right._high);

        public static Binary128 operator |(Binary128 left, Binary128 right) => new Binary128(left._low | right._low, left._high | right._high);

        public static Binary128 operator ^(Binary128 left, Binary128 right) => new Binary128(left._low ^ right._low, left._high ^ right._high);

        public static Binary128 operator <<(Binary128 value, int shift)
        {
            switch (shift &= 127)
            {
                case 0:
                    return value;
                case 64:
                    return new Binary128(0UL, value._low.Unsigned);
                default:
                    if (shift > 64)
                        return new Binary128(0UL, value._low >> (shift - 64));
                    return new Binary128((value._high.Unsigned << shift) | (value._low >> (128 - shift)), value._low << shift);
            }
        }

        public static Binary128 operator >>(Binary128 value, int shift)
        {
            switch (shift &= 127)
            {
                case 0:
                    return value;
                case 64:
                    return new Binary128(value._high.Unsigned, 0UL);
                default:
                    if (shift > 64)
                        return new Binary128(value._high >> (shift - 64), 0UL);
                    return new Binary128((value._high << (128 - shift)) | (value._low >> shift), value._high.Unsigned >> shift);
            }
        }

        public static bool operator ==(Binary128 left, Binary128 right) => left._low.Equals(right._low) && left._high.Equals(right._high);

        public static bool operator ==(Guid left, Binary128 right) => left.Equals(right._guid);

        public static bool operator ==(Binary128 left, Guid right) => left._guid.Equals(right);

        public static bool operator !=(Binary128 left, Binary128 right) => !(left._low.Equals(right._low) && left._high.Equals(right._high));

        public static bool operator !=(Binary128 left, Guid right) => !left._guid.Equals(right);

        public static bool operator !=(Guid left, Binary128 right) => !left.Equals(right._guid);

        public static bool operator <(Binary128 left, Binary128 right) => left._high < right._high || (left._high == right._high && left._low < right._low);

        public static bool operator >(Binary128 left, Binary128 right) => left._high > right._high || (left._high == right._high && left._low > right._low);

        public static bool operator <=(Binary128 left, Binary128 right) => left._high < right._high || (left._high == right._high && left._low >= right._low);

        public static bool operator >=(Binary128 left, Binary128 right) => left._high > right._high || (left._high == right._high && left._low >= right._low);

        public static implicit operator Binary128(byte value) => new Binary128(new Binary64((ulong)value), new Binary64(0UL));

        public static implicit operator Binary128(sbyte value) => new Binary128(new Binary64(value), new Binary64((value < 0) ? -1L : 0L));

        public static implicit operator Binary128(ushort value) => new Binary128(new Binary64((ulong)value), new Binary64(0UL));

        public static implicit operator Binary128(short value) => new Binary128(new Binary64(value), new Binary64((value < 0) ? -1L : 0L));

        public static implicit operator Binary128(int value) => new Binary128(new Binary64(value), new Binary64((value < 0) ? -1L : 0L));

        public static implicit operator Binary128(uint value) => new Binary128(new Binary64((ulong)value), new Binary64(0UL));

        public static implicit operator Binary128(long value) => new Binary128(new Binary64(value), new Binary64((value < 0) ? -1L : 0L));

        public static implicit operator Binary128(ulong value) => new Binary128(new Binary64(value), new Binary64(0UL));

        public static explicit operator Binary128(float value)
        {
            if (value < 0f)
            {
                if (value < long.MinValue)
                {
                    double r = Math.Round(value / Convert.ToSingle(long.MaxValue));
                    return new Binary128(new Binary64((long)(value - r)), new Binary64((long)r));
                }
                return new Binary128(new Binary64((long)(value * -1f)), new Binary64(-1L));
            }
            if (value > ulong.MaxValue)
            {
                double r = Math.Round(value / Convert.ToSingle(ulong.MaxValue));
                return new Binary128(new Binary64((ulong)(value - r)), new Binary64((ulong)r));
            }
            return new Binary128(new Binary64((ulong)value), new Binary64(0UL));
        }

        public static explicit operator Binary128(double value)
        {
            if (value < 0f)
            {
                if (value < long.MinValue)
                {
                    double r = Math.Round(value / Convert.ToDouble(long.MaxValue));
                    return new Binary128(new Binary64((long)(value - r)), new Binary64((long)r));
                }
                return new Binary128(new Binary64((long)(value * -1.0)), new Binary64(-1L));
            }
            if (value > ulong.MaxValue)
            {
                double r = Math.Round(value / Convert.ToDouble(ulong.MaxValue));
                return new Binary128(new Binary64((ulong)(value - r)), new Binary64((ulong)r));
            }
            return new Binary128(new Binary64((ulong)value), new Binary64(0UL));
        }

        public static explicit operator Binary128(decimal value)
        {
            if (value < 0m)
            {
                if (value < long.MinValue)
                {
                    decimal r = Math.Round(value / Convert.ToDecimal(long.MaxValue));
                    return new Binary128(new Binary64((long)(value - r)), new Binary64((long)r));
                }
                return new Binary128(new Binary64((long)(value * -1m)), new Binary64(-1L));
            }
            if (value > ulong.MaxValue)
            {
                decimal r = Math.Round(value / Convert.ToDecimal(ulong.MaxValue));
                return new Binary128(new Binary64((ulong)(value - r)), new Binary64((ulong)r));
            }
            return new Binary128(new Binary64((ulong)value), new Binary64(0UL));
        }

        public static implicit operator Binary128(Guid value) => new Binary128(value);

        public static explicit operator float(Binary128 value) => value.TryConvertToSingle(out float result) ? result : throw new OverflowException();

        public static explicit operator double(Binary128 value) => value.ToDouble();

        public static explicit operator decimal(Binary128 value) => value.TryConvertToDecimal(out decimal result) ? result : throw new OverflowException();

        public static explicit operator Guid(Binary128 value) => value._guid;

        #endregion
    }
}
