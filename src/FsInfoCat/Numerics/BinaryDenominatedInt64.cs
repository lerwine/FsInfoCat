using System;
using System.Globalization;

namespace FsInfoCat.Numerics
{
    public struct BinaryDenominatedInt64 : IEquatable<BinaryDenominatedInt64>, IComparable<BinaryDenominatedInt64>, IConvertible
    {
        public static readonly BinaryDenominatedInt64 MaxValue = new(long.MaxValue);

        public static readonly BinaryDenominatedInt64 MinValue = new(long.MaxValue);

        public static readonly BinaryDenominatedInt64 Zero = new(0L);

        public const long KB_MAX = 0x001f_ffff_ffff_ffffL;
        public const long KB_MIN = -9007199254740992L;
        public const long KB_DENOMINATOR = 0x0000_0000_0400L;
        public const int KB_SHIFT = 10;
        public const long MB_MAX = 0x0000_07ff_ffff_ffffL;
        public const long MB_MIN = -8796093022208L;
        public const long MB_DENOMINATOR = 0x0000_0010_0000L;
        public const int MB_SHIFT = 20;
        public const long GB_MAX = 0x0000_0001_ffff_ffffL;
        public const long GB_MIN = -8589934592L;
        public const long GB_DENOMINATOR = 0x0000_4000_0000L;
        public const int GB_SHIFT = 30;
        public const long TB_MAX = 0x0000_0000_007f_ffffL;
        public const long TB_MIN = -8388608L;
        public const long TB_DENOMINATOR = 0x0100_0000_0000L;
        public const int TB_SHIFT = 40;
        public const string TB_SUFFIX = "TB";
        public const string GB_SUFFIX = "GB";
        public const string MB_SUFFIX = "MB";
        public const string KB_SUFFIX = "KB";
        public const string B_SUFFIX = "b";

        private readonly long _numerator;
        private readonly BinaryDenomination _denominator;

        public long Numerator => _numerator;

        public BinaryDenomination Denominator => _denominator;

        public BinaryDenominatedInt64(long numerator, BinaryDenomination denominator)
        {
            switch (denominator)
            {
                case BinaryDenomination.Terabytes:
                    if (numerator > TB_MAX || numerator < TB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Terabytes)} denominator.");
                    break;
                case BinaryDenomination.Gigabytes:
                    if (numerator > GB_MAX || numerator < GB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Gigabytes)} denominator.");
                    break;
                case BinaryDenomination.Megabytes:
                    if (numerator > MB_MAX || numerator < MB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Megabytes)} denominator.");
                    break;
                case BinaryDenomination.Kilobytes:
                    if (numerator > KB_MAX || numerator < KB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Kilobytes)} denominator.");
                    break;
            }
            _numerator = numerator;
            _denominator = denominator;
        }

        public BinaryDenominatedInt64(long value)
        {
            long bits = 0x3ffL;
            long a;
            if (value == 0L || ((a = Math.Abs(value)) & bits) != 0L)
            {
                _numerator = value;
                _denominator = BinaryDenomination.Bytes;
            }
            else if ((a & (bits <<= 10)) != 0L)
            {
                _numerator = (value < 0L) ? (a >> KB_SHIFT) * -1L : value >> KB_SHIFT;
                _denominator = BinaryDenomination.Kilobytes;
            }
            else if ((a & (bits <<= 10)) != 0L)
            {
                _numerator = (value < 0L) ? (a >> MB_SHIFT) * -1L : value >> MB_SHIFT;
                _denominator = BinaryDenomination.Megabytes;
            }
            else if ((a & (bits << 10)) != 0L)
            {
                _numerator = (value < 0L) ? (a >> GB_SHIFT) * -1L : value >> GB_SHIFT;
                _denominator = BinaryDenomination.Gigabytes;
            }
            else
            {
                _numerator = (value < 0L) ? (a >> TB_SHIFT) * -1L : value >> TB_SHIFT;
                _denominator = BinaryDenomination.Terabytes;
            }
        }

        public int CompareTo(BinaryDenominatedInt64 other) => (other._denominator == _denominator) ? other._numerator.CompareTo(_numerator) : other.ToPrimitive().CompareTo(ToPrimitive());

        public bool Equals(BinaryDenominatedInt64 other) => (other._denominator == _denominator) ? other._numerator.Equals(_numerator) : other.ToPrimitive().Equals(ToPrimitive());

        public override bool Equals(object obj) => obj is BinaryDenominatedInt64 other && Equals(other);

        public override int GetHashCode() => ToPrimitive().GetHashCode();

        public string ToString(IFormatProvider provider) => _denominator switch
        {
            BinaryDenomination.Terabytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {TB_SUFFIX}",
            BinaryDenomination.Gigabytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {GB_SUFFIX}",
            BinaryDenomination.Megabytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {MB_SUFFIX}",
            BinaryDenomination.Kilobytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {KB_SUFFIX}",
            _ => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {B_SUFFIX}",
        };

        public override string ToString() => ToString(CultureInfo.InvariantCulture);

        public long ToPrimitive()
        {
            if (_numerator < 0L)
                return _denominator switch
                {
                    BinaryDenomination.Terabytes => ((_numerator * -1L) << TB_SHIFT) * -1L,
                    BinaryDenomination.Gigabytes => ((_numerator * -1L) << GB_SHIFT) * -1L,
                    BinaryDenomination.Megabytes => ((_numerator * -1L) << MB_SHIFT) * -1L,
                    BinaryDenomination.Kilobytes => ((_numerator * -1L) << KB_SHIFT) * -1L,
                    _ => _numerator,
                };
            return _denominator switch
            {
                BinaryDenomination.Terabytes => _numerator << TB_SHIFT,
                BinaryDenomination.Gigabytes => _numerator << GB_SHIFT,
                BinaryDenomination.Megabytes => _numerator << MB_SHIFT,
                BinaryDenomination.Kilobytes => _numerator << KB_SHIFT,
                _ => _numerator,
            };
        }

        public static BinaryDenominatedInt64 Parse(string s, IFormatProvider provider)
        {
            if (s is null || (s = s.Trim()).Length == 0)
                throw new ArgumentException($"'{nameof(s)}' cannot be null or whitespace.", nameof(s));
            if (s.Length > 3)
            {
                long numerator = long.Parse(s[0..^2].Trim(), provider ?? CultureInfo.CurrentUICulture);
                switch (s[^2..].ToUpper())
                {
                    case TB_SUFFIX:
                        if (numerator > TB_MAX || numerator < TB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{TB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Terabytes);
                    case GB_SUFFIX:
                        if (numerator > GB_MAX || numerator < GB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{GB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Gigabytes);
                    case MB_SUFFIX:
                        if (numerator > MB_MAX || numerator < MB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{MB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Megabytes);
                    case KB_SUFFIX:
                        if (numerator > KB_MAX || numerator < KB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{KB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Kilobytes);
                    case "B":
                        return new(numerator, BinaryDenomination.Bytes);
                }
            }
            return new(long.Parse(s, provider ?? CultureInfo.CurrentUICulture));
        }

        public static BinaryDenominatedInt64 Parse(string s) => Parse(s, CultureInfo.InvariantCulture);

        public static bool TryParse(string s, IFormatProvider provider, out BinaryDenominatedInt64 result)
        {
            if (s is not null && (s = s.Trim()).Length > 0)
            {
                if (s.Length > 3)
                {
                    if (long.TryParse(s[0..^2].Trim(), NumberStyles.Any, provider ?? CultureInfo.CurrentUICulture, out long numerator))
                        switch (s[^2..].ToUpper())
                        {
                            case TB_SUFFIX:
                                if (numerator <= TB_MAX && numerator >= TB_MIN)
                                {
                                    result = new(numerator, BinaryDenomination.Terabytes);
                                    return true;
                                }
                                break;
                            case GB_SUFFIX:
                                if (numerator <= GB_MAX || numerator >= GB_MIN)
                                {
                                    result = new(numerator, BinaryDenomination.Gigabytes);
                                    return true;
                                }
                                break;
                            case MB_SUFFIX:
                                if (numerator <= MB_MAX || numerator >= MB_MIN)
                                {
                                    result = new(numerator, BinaryDenomination.Megabytes);
                                    return true;
                                }
                                break;
                            case KB_SUFFIX:
                                if (numerator <= KB_MAX || numerator >= KB_MIN)
                                {
                                    result = new(numerator, BinaryDenomination.Kilobytes);
                                    return true;
                                }
                                break;
                            case "B":
                                result = new(numerator, BinaryDenomination.Bytes);
                                return true;
                        }
                }
                else if (long.TryParse(s, NumberStyles.Any, provider ?? CultureInfo.CurrentUICulture, out long numerator))
                {
                    result = new(numerator);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryParse(string s, out BinaryDenominatedInt64 result) => TryParse(s, CultureInfo.InvariantCulture, out result);

        public Fraction64 ToFraction() => _denominator switch
        {
            BinaryDenomination.Terabytes => new(_numerator, TB_DENOMINATOR),
            BinaryDenomination.Gigabytes => new(_numerator, GB_DENOMINATOR),
            BinaryDenomination.Megabytes => new(_numerator, MB_DENOMINATOR),
            BinaryDenomination.Kilobytes => new(_numerator, KB_DENOMINATOR),
            _ => new (_numerator, 1L),
        };

        TypeCode IConvertible.GetTypeCode() => TypeCode.Int64;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(ToPrimitive(), provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(ToPrimitive(), provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToPrimitive(), provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(ToPrimitive(), provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(ToPrimitive(), provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(ToPrimitive(), provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(ToPrimitive(), provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(ToPrimitive(), provider);
        long IConvertible.ToInt64(IFormatProvider provider) => ToPrimitive();
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(ToPrimitive(), provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(ToPrimitive(), provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => (conversionType == typeof(FractionU64)) ? ToFraction() : Convert.ChangeType(ToPrimitive(), conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(ToPrimitive(), provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(ToPrimitive(), provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(ToPrimitive(), provider);

        #region Operators

        public static bool operator ==(BinaryDenominatedInt64 x, BinaryDenominatedInt64 y) { return x.Equals(y); }

        public static bool operator ==(long x, BinaryDenominatedInt64 y) { return x.Equals(y); }

        public static bool operator ==(BinaryDenominatedInt64 x, long y) { return x.Equals(y); }

        public static bool operator !=(BinaryDenominatedInt64 x, BinaryDenominatedInt64 y) { return !x.Equals(y); }

        public static bool operator !=(long x, BinaryDenominatedInt64 y) { return !x.Equals(y); }

        public static bool operator !=(BinaryDenominatedInt64 x, long y) { return !x.Equals(y); }

        public static bool operator <(BinaryDenominatedInt64 x, BinaryDenominatedInt64 y) { return x.CompareTo(y) < 0; }

        public static bool operator <(long x, BinaryDenominatedInt64 y) { return x.CompareTo(y) < 0; }

        public static bool operator <(BinaryDenominatedInt64 x, long y) { return x.CompareTo(y) < 0; }

        public static bool operator <=(BinaryDenominatedInt64 x, BinaryDenominatedInt64 y) { return x.CompareTo(y) <= 0; }

        public static bool operator <=(long x, BinaryDenominatedInt64 y) { return x.CompareTo(y) <= 0; }

        public static bool operator <=(BinaryDenominatedInt64 x, long y) { return x.CompareTo(y) <= 0; }

        public static bool operator >(BinaryDenominatedInt64 x, BinaryDenominatedInt64 y) { return x.CompareTo(y) > 0; }

        public static bool operator >(long x, BinaryDenominatedInt64 y) { return x.CompareTo(y) > 0; }

        public static bool operator >(BinaryDenominatedInt64 x, long y) { return x.CompareTo(y) > 0; }

        public static bool operator >=(BinaryDenominatedInt64 x, BinaryDenominatedInt64 y) { return x.CompareTo(y) >= 0; }

        public static bool operator >=(long x, BinaryDenominatedInt64 y) { return x.CompareTo(y) >= 0; }

        public static bool operator >=(BinaryDenominatedInt64 x, long y) { return x.CompareTo(y) >= 0; }

        public static implicit operator BinaryDenominatedInt64(long value) => new(value);

        public static implicit operator long(BinaryDenominatedInt64 value) => value.ToPrimitive();

        #endregion
    }
}
