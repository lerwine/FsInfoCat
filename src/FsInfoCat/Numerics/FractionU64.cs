using System;
using System.Runtime.InteropServices;

namespace FsInfoCat.Numerics
{
    // TODO: Document FractionU64 type
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [StructLayout(LayoutKind.Sequential)]
    public struct FractionU64 : IEquatable<FractionU64>, IComparable<FractionU64>, IFraction<ulong>
    {
        #region Fields

        public static readonly FractionU64 Zero = new(0UL, 0UL, 1UL);
        private static readonly Coersion<ulong> _coersion = Coersion<ulong>.Default;

        private readonly ulong _wholeNumber;
        private readonly ulong _numerator;
        private readonly ulong _denominator;

        #endregion

        #region Properties

        public ulong WholeNumber { get { return _wholeNumber; } }

        IConvertible IFraction.WholeNumber { get { return _wholeNumber; } }

        public ulong Numerator { get { return _numerator; } }

        IConvertible IFraction.Numerator { get { return _numerator; } }

        public ulong Denominator { get { return _denominator; } }

        IConvertible IFraction.Denominator { get { return _denominator; } }

        #endregion

        #region Constructors

        public FractionU64(ulong wholeNumber, ulong numerator, ulong denominator)
        {
            _wholeNumber = NumericsExtensions.GetNormalizedRationalU64(wholeNumber, numerator, denominator, out numerator, out denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionU64(ulong numerator, ulong denominator)
        {
            _wholeNumber = NumericsExtensions.GetNormalizedRationalU64(0UL, numerator, denominator, out numerator, out denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionU64(IFraction fraction)
        {
            _wholeNumber = NumericsExtensions.GetNormalizedRationalU64(_coersion.Coerce(fraction.WholeNumber), _coersion.Coerce(fraction.Numerator),
                _coersion.Coerce(fraction.Denominator).OneIfZero(), out ulong numerator, out ulong denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionU64(ulong wholeNumber)
        {
            _wholeNumber = wholeNumber;
            _numerator = 0UL;
            _denominator = 1UL;
        }

        #endregion

        #region *Parse

        public static FractionU64 Parse(string s)
        {
            ulong wholeNumber = NumericsExtensions.ParseU64(s, out ulong numerator, out ulong denominator);
            return new FractionU64(wholeNumber, numerator, denominator);
        }

        public static bool TryParse(string s, out FractionU64 value)
        {
            if (NumericsExtensions.TryParseU64(s, out ulong wholeNumber, out ulong numerator, out ulong denominator))
            {
                value = new FractionU64(wholeNumber, numerator, denominator);
                return true;
            }

            value = new FractionU64();
            return false;
        }

        #endregion

        #region Operators

        public static FractionU64 operator +(FractionU64 x, FractionU64 y) { return x.Add(y); }

        public static FractionU64 operator -(FractionU64 x, FractionU64 y) { return x.Subtract(y); }

        public static FractionU64 operator *(FractionU64 x, FractionU64 y) { return x.Multiply(y); }

        public static FractionU64 operator /(FractionU64 x, FractionU64 y) { return x.Divide(y); }

        public static bool operator ==(FractionU64 x, FractionU64 y) { return x.Equals(y); }

        public static bool operator !=(FractionU64 x, FractionU64 y) { return !x.Equals(y); }

        public static bool operator <(FractionU64 x, FractionU64 y) { return x.CompareTo(y) < 0; }

        public static bool operator <=(FractionU64 x, FractionU64 y) { return x.CompareTo(y) <= 0; }

        public static bool operator >(FractionU64 x, FractionU64 y) { return x.CompareTo(y) > 0; }

        public static bool operator >=(FractionU64 x, FractionU64 y) { return x.CompareTo(y) >= 0; }

        #endregion

        #region Add

        public FractionU64 Add(ulong wholeNumber, ulong numerator, ulong denominator)
        {
            if (_numerator == 0UL && _wholeNumber == 0UL)
                return new FractionU64(wholeNumber, numerator, denominator);

            if (numerator == 0UL && wholeNumber == 0UL)
                return (_denominator == 0UL) ? Zero : this;

            ulong w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetNormalizedRationalU64(wholeNumber, numerator, denominator, out ulong n2, out ulong d2);
            NumericsExtensions.ToCommonDenominatorU64(ref n1, ref d1, ref n2, ref d2);
            w1 = NumericsExtensions.GetNormalizedRationalU64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new FractionU64(w1, n1, d1);
        }

        IFraction<ulong> IFraction<ulong>.Add(ulong wholeNumber, ulong numerator, ulong denominator) { return Add(wholeNumber, numerator, denominator); }

        public FractionU64 Add(ulong numerator, ulong denominator) { return Add(0UL, numerator, denominator); }

        IFraction<ulong> IFraction<ulong>.Add(ulong numerator, ulong denominator) { return Add(0UL, numerator, denominator); }

        public FractionU64 Add(FractionU64 other)
        {
            if (_numerator == 0UL && _wholeNumber == 0UL)
                return (other._denominator == 0UL) ? Zero : other;

            if (other._numerator == 0UL && other._wholeNumber == 0UL)
                return (_denominator == 0UL) ? Zero : this;

            ulong w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2 = other._wholeNumber, n2 = other._numerator, d2 = other._denominator;
            NumericsExtensions.ToCommonDenominatorU64(ref n1, ref d1, ref n2, ref d2);
            w1 = NumericsExtensions.GetNormalizedRationalU64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new FractionU64(w1, n1, d1);
        }

        IFraction<ulong> IFraction<ulong>.Add(IFraction<ulong> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is FractionU64 f)
                return Add(f);

            return Add(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public FractionU64 Add(ulong wholeNumber) { return Add(wholeNumber, 0UL, 1UL); }

        IFraction<ulong> IFraction<ulong>.Add(ulong wholeNumber) { return Add(wholeNumber, 0UL, 1UL); }

        public IFraction Add(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<ulong> f)
                return Add(f);

            if (other.Equals(0UL))
                return this;

            if (_numerator == 0UL && _wholeNumber == 0UL)
                return other;

            if (other.GetMaxUnderlyingValue().CompareTo(ulong.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(ulong.MinValue) >= 0)
                return Add(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Add(other);
        }

        #endregion

        #region AsInverted

        public FractionU64 AsInverted()
        {
            if (_numerator == 0UL)
            {
                if (_wholeNumber == 0UL)
                    return Zero;
                return new FractionU64(0UL, 1UL, _wholeNumber);
            }

            if (_wholeNumber == 0UL)
                return new FractionU64(0UL, _denominator, _numerator);

            return new FractionU64(0UL, _denominator, _numerator + (_wholeNumber * _denominator));
        }

        IFraction<ulong> IFraction<ulong>.AsInverted() { return AsInverted(); }

        IFraction IFraction.AsInverted() { return AsInverted(); }

        #endregion

        public ulong AsRoundedValue()
        {
            if (_numerator == 0UL || _numerator < (_denominator >> 1))
                return _wholeNumber;

            return _wholeNumber + 1UL;
        }

        #region CompareTo

        public int CompareTo(FractionU64 other)
        {
            int i = _wholeNumber.CompareTo(other._wholeNumber);
            if (i != 0)
                return i;
            if (_wholeNumber == 0UL)
            {
                if (_numerator == 0UL || other._numerator == 0UL || _denominator == other._denominator)
                    return _numerator.CompareTo(other._numerator);
            }
            else
            {
                if (_numerator == 0UL)
                    return (other._numerator == 0UL) ? 0 : -1;
                if (other._numerator == 0UL)
                    return 1;
            }

            ulong n1 = _numerator, d1 = _denominator, n2 = other._numerator, d2 = other._denominator;
            NumericsExtensions.ToCommonDenominatorU64(ref n1, ref d1, ref n2, ref d2);
            return n1.CompareTo(n2);
        }

        private int CompareTo(IFraction<ulong> other)
        {
            if (other == null)
                return 1;

            if (other is FractionU64 f)
                return CompareTo(f);

            ulong w = NumericsExtensions.GetNormalizedRationalU64(other.WholeNumber, other.Numerator, other.Denominator, out ulong n, out ulong d);

            int i = _wholeNumber.CompareTo(w);
            if (i != 0)
                return i;
            if (_wholeNumber == 0UL)
            {
                if (_numerator == 0UL || n == 0UL || _denominator == d)
                    return _numerator.CompareTo(n);
            }
            else
            {
                if (_numerator == 0UL)
                    return (n == 0UL) ? 0 : -1;
                if (n == 0UL)
                    return 1;
                if (n == 0UL)
                    return 1;
            }

            ulong n1 = _numerator, d1 = _denominator, n2 = n, d2 = d;
            NumericsExtensions.ToCommonDenominatorU64(ref n1, ref d1, ref n2, ref d2);
            return n1.CompareTo(n2);
        }

        int IComparable<IFraction<ulong>>.CompareTo(IFraction<ulong> other) { return CompareTo(other); }

        public int CompareTo(IFraction other)
        {
            if (other == null)
                return 1;

            if (other is IFraction<ulong> f)
                return CompareTo(f);

            if (other.GetMaxUnderlyingValue().CompareTo(ulong.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(0UL) >= 0)
                return CompareTo(new FractionU64(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator)));

            return (new Fraction64(this)).CompareTo(other);
        }

        public int CompareTo(object obj) { return NumericsExtensions.Compare(this, obj); }

        #endregion

        #region Divide

        public FractionU64 Divide(ulong wholeNumber, ulong numerator, ulong denominator)
        {
            if (_numerator == 0UL && _wholeNumber == 0UL)
                return Zero;

            if (numerator == 0UL && wholeNumber == 0UL)
                throw new DivideByZeroException();

            ulong w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetInvertedRationalU64(wholeNumber, numerator, denominator, out ulong n2, out ulong d2);

            if (n2 == 0UL && w2 == 0UL)
                throw new DivideByZeroException();

            w1 = NumericsExtensions.GetNormalizedRationalU64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new FractionU64(w1, n1, d1);
        }

        IFraction<ulong> IFraction<ulong>.Divide(ulong wholeNumber, ulong numerator, ulong denominator) { return Divide(wholeNumber, numerator, denominator); }

        public FractionU64 Divide(ulong numerator, ulong denominator) { return Divide(0UL, numerator, denominator); }

        IFraction<ulong> IFraction<ulong>.Divide(ulong numerator, ulong denominator) { return Divide(0UL, numerator, denominator); }

        public FractionU64 Divide(FractionU64 other)
        {
            if (other._numerator == 0UL && other._wholeNumber == 0UL)
                throw new DivideByZeroException();

            return Multiply(other.AsInverted());
        }

        IFraction<ulong> IFraction<ulong>.Divide(IFraction<ulong> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.Numerator == 0UL && other.WholeNumber == 0UL)
                throw new DivideByZeroException();

            if ((other = other.AsInverted()) is FractionU64)
                return Multiply((FractionU64)other);

            return Multiply(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public IFraction Divide(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<ulong> f)
                return Add(f);

            if (other.Equals(0UL))
                throw new DivideByZeroException();

            if (_numerator == 0UL && _wholeNumber == 0UL)
                return other;

            if (other.GetMaxUnderlyingValue().CompareTo(ulong.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(ulong.MinValue) >= 0)
                return Divide(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Divide(other);
        }

        public FractionU64 Divide(ulong wholeNumber) { return Divide(wholeNumber, 0UL, 1UL); }

        IFraction<ulong> IFraction<ulong>.Divide(ulong wholeNumber) { return Divide(wholeNumber, 0UL, 1UL); }

        #endregion

        #region Equals

        public bool Equals(FractionU64 other)
        {
            if (_numerator == 0UL)
                return other._numerator == 0UL && _wholeNumber == other._wholeNumber;

            return _numerator == other._numerator && _denominator == other._denominator && _wholeNumber == other._wholeNumber;
        }

        private bool Equals(IFraction<ulong> other)
        {
            if (other == null)
                return false;

            if (other is FractionU64 f)
                return Equals(f);

            ulong w = NumericsExtensions.GetNormalizedRationalU64(other.WholeNumber, other.Numerator, other.Denominator, out ulong n, out ulong d);

            if (_numerator == 0UL)
                return n == 0UL && _wholeNumber == w;

            return _numerator == n && _denominator == d && _wholeNumber == w;
        }

        bool IEquatable<IFraction<ulong>>.Equals(IFraction<ulong> other) { return Equals(other); }

        public bool Equals(IFraction other)
        {
            if (other == null)
                return false;

            if (other is IFraction<ulong> f)
                return Equals(f);

            if (other.GetMaxUnderlyingValue().CompareTo(ulong.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(ulong.MinValue) >= 0)
                return Equals(new FractionU64(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator)));

            return (new Fraction64(this)).Equals(other);
        }

        public override bool Equals(object obj) { return NumericsExtensions.EqualTo(this, obj); }

        #endregion

        public override int GetHashCode() { return ToSingle().GetHashCode(); }

        IComparable IFraction.GetMinUnderlyingValue() { return ulong.MinValue; }

        IComparable IFraction.GetMaxUnderlyingValue() { return ulong.MaxValue; }

        TypeCode IConvertible.GetTypeCode() { return TypeCode.Double; }

        #region Multiply

        public FractionU64 Multiply(ulong wholeNumber, ulong numerator, ulong denominator)
        {
            if ((_numerator == 0UL && _wholeNumber == 0UL) || (numerator == 0UL && wholeNumber == 0UL))
                return Zero;

            ulong w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetNormalizedRationalU64(wholeNumber, numerator, denominator, out ulong n2, out ulong d2);

            if (numerator == 0UL && wholeNumber == 0UL)
                return Zero;

            w1 = NumericsExtensions.GetNormalizedRationalU64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new FractionU64(w1, n1, d1);
        }

        IFraction<ulong> IFraction<ulong>.Multiply(ulong wholeNumber, ulong numerator, ulong denominator) { return Multiply(wholeNumber, numerator, denominator); }

        public FractionU64 Multiply(ulong numerator, ulong denominator) { return Multiply(0UL, numerator, denominator); }

        IFraction<ulong> IFraction<ulong>.Multiply(ulong numerator, ulong denominator) { return Multiply(0UL, numerator, denominator); }

        public FractionU64 Multiply(FractionU64 other)
        {
            if ((_numerator == 0UL && _wholeNumber == 0UL) || (other._numerator == 0UL && other._wholeNumber == 0UL))
                return Zero;

            ulong w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2 = other._wholeNumber, n2 = other._numerator, d2 = other._denominator;

            w1 = NumericsExtensions.GetNormalizedRationalU64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new FractionU64(w1, n1, d1);
        }

        IFraction<ulong> IFraction<ulong>.Multiply(IFraction<ulong> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is FractionU64 f)
                return Multiply(f);

            return Multiply(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public FractionU64 Multiply(ulong wholeNumber) { return Multiply(wholeNumber, 0UL, 1UL); }

        IFraction<ulong> IFraction<ulong>.Multiply(ulong wholeNumber) { return Multiply(wholeNumber, 0UL, 1UL); }

        public IFraction Multiply(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<ulong> f)
                return Add(f);

            if (_numerator == 0UL && _wholeNumber == 0UL)
                return other;

            if (other.Equals(0UL))
                return Zero;

            if (other.GetMaxUnderlyingValue().CompareTo(ulong.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(ulong.MinValue) >= 0)
                return Multiply(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Multiply(other);
        }

        #endregion

        #region Subtract

        public FractionU64 Subtract(ulong wholeNumber, ulong numerator, ulong denominator)
        {
            if (numerator == 0UL && wholeNumber == 0UL)
                return (_denominator == 0UL) ? Zero : this;

            ulong w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetNormalizedRationalU64(wholeNumber, numerator, denominator, out ulong n2, out ulong d2);
            NumericsExtensions.ToCommonDenominatorU64(ref n1, ref d1, ref n2, ref d2);
            w1 = NumericsExtensions.GetNormalizedRationalU64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new FractionU64(w1, n1, d1);
        }

        IFraction<ulong> IFraction<ulong>.Subtract(ulong wholeNumber, ulong numerator, ulong denominator) { return Subtract(wholeNumber, numerator, denominator); }

        public FractionU64 Subtract(ulong numerator, ulong denominator) { return Subtract(0UL, numerator, denominator); }

        IFraction<ulong> IFraction<ulong>.Subtract(ulong numerator, ulong denominator) { return Subtract(0UL, numerator, denominator); }

        public FractionU64 Subtract(FractionU64 other)
        {
            if (other._denominator == 0UL)
                return Subtract(0UL, 0UL, 1UL);
            return Subtract(other._wholeNumber, other._numerator, other._denominator);
        }

        IFraction<ulong> IFraction<ulong>.Subtract(IFraction<ulong> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Subtract(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public FractionU64 Subtract(ulong wholeNumber) { return Subtract(wholeNumber, 0UL, 1UL); }

        IFraction<ulong> IFraction<ulong>.Subtract(ulong wholeNumber) { return Subtract(wholeNumber, 0UL, 1UL); }

        public IFraction Subtract(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<ulong> f)
                return Subtract(f);

            if (other.GetMaxUnderlyingValue().CompareTo(ulong.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(ulong.MinValue) >= 0)
                return Subtract(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Subtract(other);
        }

        #endregion

        #region To*

        bool IConvertible.ToBoolean(IFormatProvider provider) { return Convert.ToBoolean(ToDouble(), provider); }

        byte IConvertible.ToByte(IFormatProvider provider) { return Convert.ToByte(AsRoundedValue(), provider); }

        char IConvertible.ToChar(IFormatProvider provider) { return Convert.ToChar(AsRoundedValue(), provider); }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { return Convert.ToDateTime(ToDouble(), provider); }

        public decimal ToDecimal()
        {
            if (_numerator == 0UL)
                return Convert.ToDecimal(_wholeNumber);
            return Convert.ToDecimal(_wholeNumber) + (Convert.ToDecimal(_numerator) / Convert.ToDecimal(_denominator));
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) { return ToDecimal(); }

        public double ToDouble()
        {
            if (_numerator == 0UL)
                return Convert.ToDouble(_wholeNumber);
            return Convert.ToDouble(_wholeNumber) + (Convert.ToDouble(_numerator) / Convert.ToDouble(_denominator));
        }

        double IConvertible.ToDouble(IFormatProvider provider) { return ToDouble(); }

        short IConvertible.ToInt16(IFormatProvider provider) { return Convert.ToInt16(AsRoundedValue(), provider); }

        int IConvertible.ToInt32(IFormatProvider provider) { return Convert.ToInt32(AsRoundedValue(), provider); }

        long IConvertible.ToInt64(IFormatProvider provider) { return Convert.ToInt64(AsRoundedValue(), provider); }

        sbyte IConvertible.ToSByte(IFormatProvider provider) { return Convert.ToSByte(AsRoundedValue(), provider); }

        public float ToSingle()
        {
            if (_numerator == 0UL)
                return Convert.ToSingle(_wholeNumber);
            return Convert.ToSingle(_wholeNumber) + (Convert.ToSingle(_numerator) / Convert.ToSingle(_denominator));
        }

        float IConvertible.ToSingle(IFormatProvider provider) { return ToSingle(); }

        public override string ToString() => _numerator == 0UL
                ? _wholeNumber.ToString()
                : _wholeNumber == 0UL ? $"{_numerator}/{_denominator}" : $"{_wholeNumber} {_numerator}/{_denominator}";

        string IConvertible.ToString(IFormatProvider provider) { return ToString(); }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == null || conversionType.AssemblyQualifiedName == (typeof(double)).AssemblyQualifiedName)
                return ToDouble();
            IConvertible c = this;
            if (conversionType.AssemblyQualifiedName == (typeof(float)).AssemblyQualifiedName)
                return c.ToSingle(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(int)).AssemblyQualifiedName)
                return c.ToInt32(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(string)).AssemblyQualifiedName)
                return c.ToString(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(long)).AssemblyQualifiedName)
                return c.ToInt64(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(decimal)).AssemblyQualifiedName)
                return c.ToDecimal(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(uint)).AssemblyQualifiedName)
                return c.ToUInt32(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(ulong)).AssemblyQualifiedName)
                return c.ToUInt64(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(short)).AssemblyQualifiedName)
                return c.ToInt16(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(ushort)).AssemblyQualifiedName)
                return c.ToUInt16(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(sbyte)).AssemblyQualifiedName)
                return c.ToSByte(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(byte)).AssemblyQualifiedName)
                return c.ToByte(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(DateTime)).AssemblyQualifiedName)
                return c.ToDateTime(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(bool)).AssemblyQualifiedName)
                return c.ToBoolean(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(char)).AssemblyQualifiedName)
                return c.ToChar(provider);
            return Convert.ChangeType(ToDouble(), conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) { return Convert.ToUInt16(AsRoundedValue(), provider); }

        uint IConvertible.ToUInt32(IFormatProvider provider) { return Convert.ToUInt32(AsRoundedValue(), provider); }

        ulong IConvertible.ToUInt64(IFormatProvider provider) { return Convert.ToUInt64(AsRoundedValue(), provider); }

        #endregion
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
