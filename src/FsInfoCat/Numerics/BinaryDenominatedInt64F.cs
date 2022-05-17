using System;
using System.Globalization;

namespace FsInfoCat.Numerics
{
    // TODO: Document BinaryDenominatedInt64F type
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct BinaryDenominatedInt64F : IEquatable<BinaryDenominatedInt64F>, IComparable<BinaryDenominatedInt64F>, IConvertible
    {
        public static readonly BinaryDenominatedInt64F MaxValue = new(long.MaxValue);

        public static readonly BinaryDenominatedInt64F MinValue = new(long.MaxValue);

        public static readonly BinaryDenominatedInt64F Zero = new(0L);

        public const double B_MAX = 9223372036854775807.0;
        public const double B_MIN = -9223372036854775808.0;
        public const double KB_MAX = 9007199254740991.0;
        public const double KB_MIN = -9007199254740992.0;
        public const double KB_DENOMINATOR = 1024.0;
        //public const int KB_SHIFT = 10;
        public const double MB_MAX = 8796093022207.0;
        public const double MB_MIN = -8796093022208.0;
        public const double MB_DENOMINATOR = 1048576.0;
        //public const int MB_SHIFT = 20;
        public const double GB_MAX = 8589934591.0;
        public const double GB_MIN = -8589934592.0;
        public const double GB_DENOMINATOR = 1073741824.0;
        //public const int GB_SHIFT = 30;
        public const double TB_MAX = 8388607.0;
        public const double TB_MIN = -8388608.0;
        public const double TB_DENOMINATOR = 1099511627776.0;
        //public const int TB_SHIFT = 40;
        public const string TB_SUFFIX = "TB";
        public const string GB_SUFFIX = "GB";
        public const string MB_SUFFIX = "MB";
        public const string KB_SUFFIX = "KB";
        public const string B_SUFFIX = "b";

        private readonly long _value;
        private readonly double _numerator;
        private readonly BinaryDenomination _denominator;

        public long BinaryValue => _value;

        public double Numerator => _numerator;

        public BinaryDenomination Denominator => _denominator;

        public BinaryDenominatedInt64F(double numerator, BinaryDenomination denominator)
        {
            switch (denominator)
            {
                case BinaryDenomination.Terabytes:
                    if (numerator > TB_MAX || numerator < TB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Terabytes)} denominator of a 64-bit signed value.");
                    _value = Convert.ToInt64(numerator * TB_DENOMINATOR);
                    _numerator = Convert.ToDouble(_value) / TB_DENOMINATOR;
                    break;
                case BinaryDenomination.Gigabytes:
                    if (numerator > GB_MAX || numerator < GB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Gigabytes)} denominator of a 64-bit signed value.");
                    _value = Convert.ToInt64(numerator * GB_DENOMINATOR);
                    _numerator = Convert.ToDouble(_value) / GB_DENOMINATOR;
                    break;
                case BinaryDenomination.Megabytes:
                    if (numerator > MB_MAX || numerator < MB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Megabytes)} denominator of a 64-bit signed value.");
                    _value = Convert.ToInt64(numerator * MB_DENOMINATOR);
                    _numerator = Convert.ToDouble(_value) / MB_DENOMINATOR;
                    break;
                case BinaryDenomination.Kilobytes:
                    if (numerator > KB_MAX || numerator < KB_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the {nameof(BinaryDenomination.Kilobytes)} denominator of a 64-bit signed value.");
                    _value = Convert.ToInt64(numerator * KB_DENOMINATOR);
                    _numerator = Convert.ToDouble(_value) / KB_DENOMINATOR;
                    break;
                default:
                    if (numerator > B_MAX || numerator < B_MIN)
                        throw new OverflowException($"{nameof(numerator)} was either too large or too small for the denominator of a 64-bit signed value.");
                    _value = Convert.ToInt64(numerator);
                    _numerator = Convert.ToDouble(_value);
                    break;
            }
            _denominator = denominator;
        }

        public BinaryDenominatedInt64F(long value, int decimalPlaces = 2)
        {
            _value = value;
            long bits = 0x3ffL;
            long a;
            int shift = 0;
            double d = (decimalPlaces > 0) ? Convert.ToDouble(value) / (decimalPlaces * 10) : Convert.ToDouble(value);
            value = Convert.ToInt64((decimalPlaces > 0) ? Math.Floor(d) * (decimalPlaces * 10) : Math.Floor(d));
            if (value == 0L || ((a = Math.Abs(value)) & bits) != 0L)
            {
                _numerator = d;
                _denominator = BinaryDenomination.Bytes;
            }
            else if ((a & (bits << (shift += 10))) != 0L)
            {
                _numerator = d / KB_DENOMINATOR;
                _denominator = BinaryDenomination.Kilobytes;
            }
            else if ((a & (bits << (shift += 10))) != 0L)
            {
                _numerator = d / MB_DENOMINATOR;
                _denominator = BinaryDenomination.Megabytes;
            }
            else if ((a & (bits << (shift + 10))) != 0L)
            {
                _numerator = d / GB_DENOMINATOR;
                _denominator = BinaryDenomination.Gigabytes;
            }
            else
            {
                _numerator = d / TB_DENOMINATOR;
                _denominator = BinaryDenomination.Terabytes;
            }
        }

        public BinaryDenominatedInt64F(BinaryDenomination denominator, long value)
        {
            _value = value;
            _denominator = denominator;
            switch (denominator)
            {
                case BinaryDenomination.Terabytes:
                    _numerator = Convert.ToDouble(value) / TB_DENOMINATOR;
                    break;
                case BinaryDenomination.Gigabytes:
                    _numerator = Convert.ToDouble(value) / GB_DENOMINATOR;
                    break;
                case BinaryDenomination.Megabytes:
                    _numerator = Convert.ToDouble(value) / MB_DENOMINATOR;
                    break;
                case BinaryDenomination.Kilobytes:
                    _numerator = Convert.ToDouble(value) / KB_DENOMINATOR;
                    break;
                default:
                    _numerator = Convert.ToDouble(value);
                    break;
            }
        }

        public int CompareTo(BinaryDenominatedInt64F other) => (other._denominator == _denominator) ? other._numerator.CompareTo(_numerator) : other._value.CompareTo(_value);

        public bool Equals(BinaryDenominatedInt64F other) => (other._denominator == _denominator) ? other._numerator.Equals(_numerator) : other._value.Equals(_value);

        public override bool Equals(object obj) => obj is BinaryDenominatedInt64F other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public string ToString(IFormatProvider provider) => _denominator switch
        {
            BinaryDenomination.Terabytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {TB_SUFFIX}",
            BinaryDenomination.Gigabytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {GB_SUFFIX}",
            BinaryDenomination.Megabytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {MB_SUFFIX}",
            BinaryDenomination.Kilobytes => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {KB_SUFFIX}",
            _ => $"{_numerator.ToString(provider ?? CultureInfo.CurrentUICulture)} {B_SUFFIX}",
        };

        public override string ToString() => ToString(CultureInfo.InvariantCulture);

        public static BinaryDenominatedInt64F Parse(string s, IFormatProvider provider)
        {
            if (s is null || (s = s.Trim()).Length == 0)
                throw new ArgumentException($"'{nameof(s)}' cannot be null or whitespace.", nameof(s));
            if (s.Length > 3)
            {
                double numerator = double.Parse(s[0..^2].Trim(), provider ?? CultureInfo.CurrentUICulture);
                switch (s[^2..].ToUpper())
                {
                    case TB_SUFFIX:
                        if (numerator > TB_MAX || numerator < TB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{TB_SUFFIX}\" denominator of a 64-bit signed value.");
                        return new(numerator, BinaryDenomination.Terabytes);
                    case GB_SUFFIX:
                        if (numerator > GB_MAX || numerator < GB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{GB_SUFFIX}\" denominator of a 64-bit signed value.");
                        return new(numerator, BinaryDenomination.Gigabytes);
                    case MB_SUFFIX:
                        if (numerator > MB_MAX || numerator < MB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{MB_SUFFIX}\" denominator of a 64-bit signed value.");
                        return new(numerator, BinaryDenomination.Megabytes);
                    case KB_SUFFIX:
                        if (numerator > KB_MAX || numerator < KB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for the \"{KB_SUFFIX}\" denominator of a 64-bit signed value.");
                        return new(numerator, BinaryDenomination.Kilobytes);
                    case "B":
                        if (numerator > KB_MAX || numerator < KB_MIN)
                            throw new OverflowException($"{nameof(numerator)} was either too large or too small for a 64-bit signed value.");
                        return new(numerator, BinaryDenomination.Bytes);
                }
            }
            return new(Convert.ToInt64(double.Parse(s, provider ?? CultureInfo.CurrentUICulture)));
        }

        public static BinaryDenominatedInt64F Parse(string s) => Parse(s, CultureInfo.InvariantCulture);

        public static bool TryParse(string s, IFormatProvider provider, out BinaryDenominatedInt64F result)
        {
            if (s is not null && (s = s.Trim()).Length > 0)
            {
                if (s.Length > 3)
                {
                    if (double.TryParse(s[0..^2].Trim(), NumberStyles.Any, provider ?? CultureInfo.CurrentUICulture, out double numerator))
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
                else if (double.TryParse(s, NumberStyles.Any, provider ?? CultureInfo.CurrentUICulture, out double numerator))
                {
                    result = new(Convert.ToInt64(Math.Floor(numerator)));
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryParse(string s, out BinaryDenominatedInt64F result) => TryParse(s, CultureInfo.InvariantCulture, out result);

        TypeCode IConvertible.GetTypeCode() => TypeCode.Int64;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(_value, provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(_value, provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(_value, provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(_value, provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(_value, provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(_value, provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(_value, provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(_value, provider);
        long IConvertible.ToInt64(IFormatProvider provider) => _value;
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(_value, provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(_value, provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(_value, conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(_value, provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(_value, provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(_value, provider);

        #region Operators

        public static bool operator ==(BinaryDenominatedInt64F x, BinaryDenominatedInt64F y) { return x.Equals(y); }

        public static bool operator ==(long x, BinaryDenominatedInt64F y) { return x.Equals(y); }

        public static bool operator ==(BinaryDenominatedInt64F x, long y) { return x.Equals(y); }

        public static bool operator !=(BinaryDenominatedInt64F x, BinaryDenominatedInt64F y) { return !x.Equals(y); }

        public static bool operator !=(long x, BinaryDenominatedInt64F y) { return !x.Equals(y); }

        public static bool operator !=(BinaryDenominatedInt64F x, long y) { return !x.Equals(y); }

        public static bool operator <(BinaryDenominatedInt64F x, BinaryDenominatedInt64F y) { return x.CompareTo(y) < 0; }

        public static bool operator <(long x, BinaryDenominatedInt64F y) { return x.CompareTo(y) < 0; }

        public static bool operator <(BinaryDenominatedInt64F x, long y) { return x.CompareTo(y) < 0; }

        public static bool operator <=(BinaryDenominatedInt64F x, BinaryDenominatedInt64F y) { return x.CompareTo(y) <= 0; }

        public static bool operator <=(long x, BinaryDenominatedInt64F y) { return x.CompareTo(y) <= 0; }

        public static bool operator <=(BinaryDenominatedInt64F x, long y) { return x.CompareTo(y) <= 0; }

        public static bool operator >(BinaryDenominatedInt64F x, BinaryDenominatedInt64F y) { return x.CompareTo(y) > 0; }

        public static bool operator >(long x, BinaryDenominatedInt64F y) { return x.CompareTo(y) > 0; }

        public static bool operator >(BinaryDenominatedInt64F x, long y) { return x.CompareTo(y) > 0; }

        public static bool operator >=(BinaryDenominatedInt64F x, BinaryDenominatedInt64F y) { return x.CompareTo(y) >= 0; }

        public static bool operator >=(long x, BinaryDenominatedInt64F y) { return x.CompareTo(y) >= 0; }

        public static bool operator >=(BinaryDenominatedInt64F x, long y) { return x.CompareTo(y) >= 0; }

        public static implicit operator BinaryDenominatedInt64F(long value) => new(value);

        public static implicit operator long(BinaryDenominatedInt64F value) => value._value;

        #endregion
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
