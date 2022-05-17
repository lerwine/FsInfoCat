using System;
using static FsInfoCat.Numerics.BinaryDenominatedInt64;

namespace FsInfoCat.Numerics
{
    // TODO: Document BinaryDenominatedUInt64 class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct BinaryDenominatedUInt64 : IEquatable<BinaryDenominatedUInt64>, IComparable<BinaryDenominatedUInt64>, IConvertible
    {
        public static readonly BinaryDenominatedUInt64 MaxValue = new(ulong.MaxValue);

        public static readonly BinaryDenominatedUInt64 MinValue = new(ulong.MaxValue);

        public const ulong KB_MAX = 0x003f_ffff_ffff_ffffUL;
        public const ulong KB_DENOMINATOR = 0x0000_0000_0400UL;
        public const ulong MB_MAX = 0x0000_0fff_ffff_ffffUL;
        public const ulong MB_DENOMINATOR = 0x0000_0010_0000UL;
        public const ulong GB_MAX = 0x0000_0003_ffff_ffffUL;
        public const ulong GB_DENOMINATOR = 0x0000_4000_0000UL;
        public const ulong TB_MAX = 0x0000_0000_00ff_ffffUL;
        public const ulong TB_DENOMINATOR = 0x0100_0000_0000UL;

        private readonly ulong _numerator;
        private readonly BinaryDenomination _denominator;

        public ulong Numerator => _numerator;

        public BinaryDenomination Denominator => _denominator;

        public BinaryDenominatedUInt64(ulong numerator, BinaryDenomination denominator)
        {
            switch (denominator)
            {
                case BinaryDenomination.Terabytes:
                    if (numerator > TB_MAX)
                        throw new OverflowException($"{nameof(numerator)} was too large for the {nameof(BinaryDenomination.Terabytes)} denominator.");
                    break;
                case BinaryDenomination.Gigabytes:
                    if (numerator > GB_MAX)
                        throw new OverflowException($"{nameof(numerator)} was too large for the {nameof(BinaryDenomination.Gigabytes)} denominator.");
                    break;
                case BinaryDenomination.Megabytes:
                    if (numerator > MB_MAX)
                        throw new OverflowException($"{nameof(numerator)} was too large for the {nameof(BinaryDenomination.Megabytes)} denominator.");
                    break;
                case BinaryDenomination.Kilobytes:
                    if (numerator > KB_MAX)
                        throw new OverflowException($"{nameof(numerator)} was too large for the {nameof(BinaryDenomination.Kilobytes)} denominator.");
                    break;
            }
            _numerator = numerator;
            _denominator = denominator;
        }

        public BinaryDenominatedUInt64(ulong value)
        {
            ulong bits = 0x3ffL;
            if (value == 0L || (value & bits) != 0L)
            {
                _numerator = value;
                _denominator = BinaryDenomination.Bytes;
            }
            else if ((value & (bits <<= 10)) != 0L)
            {
                _numerator = value >> KB_SHIFT;
                _denominator = BinaryDenomination.Kilobytes;
            }
            else if ((value & (bits <<= 10)) != 0L)
            {
                _numerator = value >> MB_SHIFT;
                _denominator = BinaryDenomination.Megabytes;
            }
            else if ((value & (bits << 10)) != 0L)
            {
                _numerator = value >> GB_SHIFT;
                _denominator = BinaryDenomination.Gigabytes;
            }
            else
            {
                _numerator = value >> TB_SHIFT;
                _denominator = BinaryDenomination.Terabytes;
            }
        }

        public int CompareTo(BinaryDenominatedUInt64 other) => (other._denominator == _denominator) ? other._numerator.CompareTo(_numerator) : other.ToPrimitive().CompareTo(ToPrimitive());

        public bool Equals(BinaryDenominatedUInt64 other) => (other._denominator == _denominator) ? other._numerator.Equals(_numerator) : other.ToPrimitive().Equals(ToPrimitive());

        public override bool Equals(object obj) => obj is BinaryDenominatedUInt64 other && Equals(other);

        public override int GetHashCode() => ToPrimitive().GetHashCode();

        public override string ToString()
        {
            return _denominator switch
            {
                BinaryDenomination.Terabytes => $"{_numerator} {TB_SUFFIX}",
                BinaryDenomination.Gigabytes => $"{_numerator} {GB_SUFFIX}",
                BinaryDenomination.Megabytes => $"{_numerator} {MB_SUFFIX}",
                BinaryDenomination.Kilobytes => $"{_numerator} {KB_SUFFIX}",
                _ => $"{_numerator} {B_SUFFIX}",
            };
        }

        private ulong ToPrimitive() => _denominator switch
        {
            BinaryDenomination.Terabytes => _numerator << TB_SHIFT,
            BinaryDenomination.Gigabytes => _numerator << GB_SHIFT,
            BinaryDenomination.Megabytes => _numerator << MB_SHIFT,
            BinaryDenomination.Kilobytes => _numerator << KB_SHIFT,
            _ => _numerator,
        };

        public static BinaryDenominatedUInt64 Parse(string s)
        {
            if (s is null || (s = s.Trim()).Length == 0)
                throw new ArgumentException($"'{nameof(s)}' cannot be null or whitespace.", nameof(s));
            if (s.Length > 3)
            {
                ulong numerator = ulong.Parse(s[0..^2].Trim());
                switch (s[^2..].ToUpper())
                {
                    case TB_SUFFIX:
                        if (numerator > TB_MAX)
                            throw new OverflowException($"{nameof(numerator)} was too large for the \"{TB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Terabytes);
                    case GB_SUFFIX:
                        if (numerator > GB_MAX)
                            throw new OverflowException($"{nameof(numerator)} was too large for the \"{GB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Gigabytes);
                    case MB_SUFFIX:
                        if (numerator > MB_MAX)
                            throw new OverflowException($"{nameof(numerator)} was too large for the \"{MB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Megabytes);
                    case KB_SUFFIX:
                        if (numerator > KB_MAX)
                            throw new OverflowException($"{nameof(numerator)} was too large for the \"{KB_SUFFIX}\" denominator.");
                        return new(numerator, BinaryDenomination.Kilobytes);
                    case "B":
                        return new(numerator, BinaryDenomination.Bytes);
                }
            }
            return new(ulong.Parse(s));
        }

        public static bool TryParse(string s, out BinaryDenominatedUInt64 result)
        {
            if (s is not null && (s = s.Trim()).Length > 0)
            {
                if (s.Length > 3)
                {
                    if (ulong.TryParse(s[0..^2].Trim(), out ulong numerator))
                        switch (s[^2..].ToUpper())
                        {
                            case TB_SUFFIX:
                                if (numerator <= TB_MAX)
                                {
                                    result = new(numerator, BinaryDenomination.Terabytes);
                                    return true;
                                }
                                break;
                            case GB_SUFFIX:
                                if (numerator <= GB_MAX)
                                {
                                    result = new(numerator, BinaryDenomination.Gigabytes);
                                    return true;
                                }
                                break;
                            case MB_SUFFIX:
                                if (numerator <= MB_MAX)
                                {
                                    result = new(numerator, BinaryDenomination.Megabytes);
                                    return true;
                                }
                                break;
                            case KB_SUFFIX:
                                if (numerator <= KB_MAX)
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
                else if (ulong.TryParse(s, out ulong numerator))
                {
                    result = new(numerator);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public FractionU64 ToFraction() => _denominator switch
        {
            BinaryDenomination.Terabytes => new(_numerator, TB_DENOMINATOR),
            BinaryDenomination.Gigabytes => new(_numerator, GB_DENOMINATOR),
            BinaryDenomination.Megabytes => new(_numerator, MB_DENOMINATOR),
            BinaryDenomination.Kilobytes => new(_numerator, KB_DENOMINATOR),
            _ => new(_numerator, 1UL),
        };

        TypeCode IConvertible.GetTypeCode() => TypeCode.UInt64;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(ToPrimitive(), provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(ToPrimitive(), provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToPrimitive(), provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(ToPrimitive(), provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(ToPrimitive(), provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(ToPrimitive(), provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(ToPrimitive(), provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(ToPrimitive(), provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(ToPrimitive(), provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(ToPrimitive(), provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(ToPrimitive(), provider);
        string IConvertible.ToString(IFormatProvider provider) => (provider is null) ? ToString() : Convert.ToString(ToPrimitive(), provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => (conversionType == typeof(FractionU64)) ? ToFraction() : Convert.ChangeType(ToPrimitive(), conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(ToPrimitive(), provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(ToPrimitive(), provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => ToPrimitive();

        #region Operators

        public static bool operator ==(BinaryDenominatedUInt64 x, BinaryDenominatedUInt64 y) { return x.Equals(y); }

        public static bool operator ==(ulong x, BinaryDenominatedUInt64 y) { return x.Equals(y); }

        public static bool operator ==(BinaryDenominatedUInt64 x, ulong y) { return x.Equals(y); }

        public static bool operator !=(BinaryDenominatedUInt64 x, BinaryDenominatedUInt64 y) { return !x.Equals(y); }

        public static bool operator !=(ulong x, BinaryDenominatedUInt64 y) { return !x.Equals(y); }

        public static bool operator !=(BinaryDenominatedUInt64 x, ulong y) { return !x.Equals(y); }

        public static bool operator <(BinaryDenominatedUInt64 x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) < 0; }

        public static bool operator <(ulong x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) < 0; }

        public static bool operator <(BinaryDenominatedUInt64 x, ulong y) { return x.CompareTo(y) < 0; }

        public static bool operator <=(BinaryDenominatedUInt64 x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) <= 0; }

        public static bool operator <=(ulong x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) <= 0; }

        public static bool operator <=(BinaryDenominatedUInt64 x, ulong y) { return x.CompareTo(y) <= 0; }

        public static bool operator >(BinaryDenominatedUInt64 x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) > 0; }

        public static bool operator >(ulong x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) > 0; }

        public static bool operator >(BinaryDenominatedUInt64 x, ulong y) { return x.CompareTo(y) > 0; }

        public static bool operator >=(BinaryDenominatedUInt64 x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) >= 0; }

        public static bool operator >=(ulong x, BinaryDenominatedUInt64 y) { return x.CompareTo(y) >= 0; }

        public static bool operator >=(BinaryDenominatedUInt64 x, ulong y) { return x.CompareTo(y) >= 0; }

        public static implicit operator BinaryDenominatedUInt64(ulong value) => new(value);

        public static implicit operator ulong(BinaryDenominatedUInt64 value) => value.ToPrimitive();

        #endregion
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
