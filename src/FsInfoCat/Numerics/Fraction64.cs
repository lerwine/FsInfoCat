using System;
using System.Runtime.InteropServices;

namespace FsInfoCat.Numerics
{
    // TODO: Document Fraction64 class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Fraction64 : IEquatable<Fraction64>, IComparable<Fraction64>, IFraction<long>
    {
        #region Fields

        public static readonly Fraction64 Zero = new(0, 0, 1);
        private static readonly Coersion<long> _coersion = Coersion<long>.Default;

        private readonly long _wholeNumber;
        private readonly long _numerator;
        private readonly long _denominator;

        #endregion

        #region Properties

        public long WholeNumber { get { return _wholeNumber; } }

        IConvertible IFraction.WholeNumber { get { return _wholeNumber; } }

        public long Numerator { get { return _numerator; } }

        IConvertible IFraction.Numerator { get { return _numerator; } }

        public long Denominator { get { return _denominator; } }

        IConvertible IFraction.Denominator { get { return _denominator; } }

        #endregion

        #region Constructors

        public Fraction64(long wholeNumber, long numerator, long denominator)
        {
            _wholeNumber = NumericsExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out numerator, out denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public Fraction64(long numerator, long denominator)
        {
            _wholeNumber = NumericsExtensions.GetNormalizedRational64(0, numerator, denominator, out numerator, out denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public Fraction64(IFraction fraction)
        {
            _wholeNumber = NumericsExtensions.GetNormalizedRational64(_coersion.Coerce(fraction.WholeNumber), _coersion.Coerce(fraction.Numerator),
                _coersion.Coerce(fraction.Denominator).OneIfZero(), out long numerator, out long denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public Fraction64(long wholeNumber)
        {
            _wholeNumber = wholeNumber;
            _numerator = 0;
            _denominator = 1;
        }

        #endregion

        #region *Parse

        public static Fraction64 Parse(string s)
        {
            long wholeNumber = NumericsExtensions.Parse64(s, out long numerator, out long denominator);
            return new Fraction64(wholeNumber, numerator, denominator);
        }

        public static bool TryParse(string s, out Fraction64 value)
        {
            if (NumericsExtensions.TryParse64(s, out long wholeNumber, out long numerator, out long denominator))
            {
                value = new Fraction64(wholeNumber, numerator, denominator);
                return true;
            }

            value = new Fraction64();
            return false;
        }

        #endregion

        #region Operators

        public static Fraction64 operator +(Fraction64 x, Fraction64 y) { return x.Add(y); }

        public static Fraction64 operator -(Fraction64 x, Fraction64 y) { return x.Subtract(y); }

        public static Fraction64 operator *(Fraction64 x, Fraction64 y) { return x.Multiply(y); }

        public static Fraction64 operator /(Fraction64 x, Fraction64 y) { return x.Divide(y); }

        public static bool operator ==(Fraction64 x, Fraction64 y) { return x.Equals(y); }

        public static bool operator !=(Fraction64 x, Fraction64 y) { return !x.Equals(y); }

        public static bool operator <(Fraction64 x, Fraction64 y) { return x.CompareTo(y) < 0; }

        public static bool operator <=(Fraction64 x, Fraction64 y) { return x.CompareTo(y) <= 0; }

        public static bool operator >(Fraction64 x, Fraction64 y) { return x.CompareTo(y) > 0; }

        public static bool operator >=(Fraction64 x, Fraction64 y) { return x.CompareTo(y) >= 0; }

        #endregion

        #region Add

        public Fraction64 Add(long wholeNumber, long numerator, long denominator)
        {
            if (_numerator == 0 && _wholeNumber == 0)
                return new Fraction64(wholeNumber, numerator, denominator);

            if (numerator == 0 && wholeNumber == 0)
                return (_denominator == 0) ? Zero : this;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);
            NumericsExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            w1 = NumericsExtensions.GetNormalizedRational64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new Fraction64(w1, n1, d1);
        }

        IFraction<long> IFraction<long>.Add(long wholeNumber, long numerator, long denominator) { return Add(wholeNumber, numerator, denominator); }

        public Fraction64 Add(long numerator, long denominator) { return Add(0, numerator, denominator); }

        IFraction<long> IFraction<long>.Add(long numerator, long denominator) { return Add(0, numerator, denominator); }

        public Fraction64 Add(Fraction64 other)
        {
            if (_numerator == 0 && _wholeNumber == 0)
                return (other._denominator == 0) ? Zero : other;

            if (other._numerator == 0 && other._wholeNumber == 0)
                return (_denominator == 0) ? Zero : this;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2 = other._wholeNumber, n2 = other._numerator, d2 = other._denominator;
            NumericsExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            w1 = NumericsExtensions.GetNormalizedRational64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new Fraction64(w1, n1, d1);
        }

        IFraction<long> IFraction<long>.Add(IFraction<long> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is Fraction64 f)
                return Add(f);

            return Add(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public Fraction64 Add(long wholeNumber) { return Add(wholeNumber, 0, 1); }

        IFraction<long> IFraction<long>.Add(long wholeNumber) { return Add(wholeNumber, 0, 1); }

        public IFraction Add(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<long> f)
                return Add(f);

            if (other.Equals(0))
                return this;

            if (_numerator == 0 && _wholeNumber == 0)
                return other;

            if (other.GetMaxUnderlyingValue().CompareTo(long.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(long.MinValue) >= 0)
                return Add(Convert.ToInt64(other.WholeNumber), Convert.ToInt64(other.Numerator), Convert.ToInt64(other.Denominator));

            return (new Fraction64(this)).Add(other);
        }

        #endregion

        #region AsInverted

        public Fraction64 AsInverted()
        {
            if (_numerator == 0)
            {
                if (_wholeNumber == 0)
                    return Zero;
                return new Fraction64(0, 1, _wholeNumber);
            }

            if (_wholeNumber == 0)
                return new Fraction64(0, _denominator, _numerator);

            return new Fraction64(0, _denominator, _numerator + (_wholeNumber * _denominator));
        }

        IFraction<long> IFraction<long>.AsInverted() { return AsInverted(); }

        IFraction IFraction.AsInverted() { return AsInverted(); }

        #endregion

        public long AsRoundedValue()
        {
            if (_numerator == 0 || _numerator < (_denominator >> 1))
                return _wholeNumber;

            return _wholeNumber + 1;
        }

        #region CompareTo

        public int CompareTo(Fraction64 other)
        {
            int i = _wholeNumber.CompareTo(other._wholeNumber);
            if (i != 0)
                return i;
            if (_wholeNumber == 0)
            {
                if (_numerator == 0 || other._numerator == 0 || _denominator == other._denominator)
                    return _numerator.CompareTo(other._numerator);
            }
            else
            {
                if (_numerator == 0)
                    return (other._numerator < int.MinValue) ? -1 : ((other._numerator > 0L) ? 1 : 0);
                if (other._numerator == 0)
                    return (_numerator < int.MinValue) ? -1 : ((_numerator > 0L) ? 1 : 0);
            }

            long n1 = _numerator, d1 = _denominator, n2 = other._numerator, d2 = other._denominator;
            NumericsExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            return n1.CompareTo(n2);
        }

        private int CompareTo(IFraction<long> other)
        {
            if (other == null)
                return 1;

            if (other is Fraction64 f)
                return CompareTo(f);

            long w = NumericsExtensions.GetNormalizedRational64(other.WholeNumber, other.Numerator, other.Denominator, out long n, out long d);

            int i = _wholeNumber.CompareTo(w);
            if (i != 0)
                return i;
            if (_wholeNumber == 0)
            {
                if (_numerator == 0 || n == 0 || _denominator == d)
                    return _numerator.CompareTo(n);
            }
            else
            {
                if (_numerator == 0)
                    return (n < int.MinValue) ? -1 : ((n > 0L) ? 1 : 0);
                if (n == 0)
                    return (_numerator < int.MinValue) ? -1 : ((_numerator > 0L) ? 1 : 0);
            }

            long n1 = _numerator, d1 = _denominator, n2 = n, d2 = d;
            NumericsExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            return n1.CompareTo(n2);
        }

        int IComparable<IFraction<long>>.CompareTo(IFraction<long> other) { return CompareTo(other); }

        public int CompareTo(IFraction other)
        {
            if (other == null)
                return 1;

            if (other is IFraction<long> f)
                return CompareTo(f);

            if (other.GetMaxUnderlyingValue().CompareTo(long.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(long.MinValue) >= 0)
                return CompareTo(new Fraction64(Convert.ToInt64(other.WholeNumber), Convert.ToInt64(other.Numerator), Convert.ToInt64(other.Denominator)));

            return (new Fraction64(this)).CompareTo(other);
        }

        public int CompareTo(object obj) { return NumericsExtensions.Compare(this, obj); }

        #endregion

        #region Divide

        public Fraction64 Divide(long wholeNumber, long numerator, long denominator)
        {
            if (_numerator == 0 && _wholeNumber == 0)
                return Zero;

            if (numerator == 0 && wholeNumber == 0)
                throw new DivideByZeroException();

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetInvertedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);

            if (n2 == 0 && w2 == 0)
                throw new DivideByZeroException();

            w1 = NumericsExtensions.GetNormalizedRational64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new Fraction64(w1, n1, d1);
        }

        IFraction<long> IFraction<long>.Divide(long wholeNumber, long numerator, long denominator) { return Divide(wholeNumber, numerator, denominator); }

        public Fraction64 Divide(long numerator, long denominator) { return Divide(0, numerator, denominator); }

        IFraction<long> IFraction<long>.Divide(long numerator, long denominator) { return Divide(0, numerator, denominator); }

        public Fraction64 Divide(Fraction64 other)
        {
            if (other._numerator == 0 && other._wholeNumber == 0)
                throw new DivideByZeroException();

            return Multiply(other.AsInverted());
        }

        IFraction<long> IFraction<long>.Divide(IFraction<long> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.Numerator == 0 && other.WholeNumber == 0)
                throw new DivideByZeroException();

            if ((other = other.AsInverted()) is Fraction64)
                return Multiply((Fraction64)other);

            return Multiply(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public IFraction Divide(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<long> f)
                return Add(f);

            if (other.Equals(0))
                throw new DivideByZeroException();

            if (_numerator == 0 && _wholeNumber == 0)
                return other;

            if (other.GetMaxUnderlyingValue().CompareTo(long.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(long.MinValue) >= 0)
                return Divide(Convert.ToInt64(other.WholeNumber), Convert.ToInt64(other.Numerator), Convert.ToInt64(other.Denominator));

            return (new Fraction64(this)).Divide(other);
        }

        public Fraction64 Divide(long wholeNumber) { return Divide(wholeNumber, 0, 1); }

        IFraction<long> IFraction<long>.Divide(long wholeNumber) { return Divide(wholeNumber, 0, 1); }

        #endregion

        #region Equals

        public bool Equals(Fraction64 other)
        {
            if (_numerator == 0)
                return other._numerator == 0 && _wholeNumber == other._wholeNumber;

            return _numerator == other._numerator && _denominator == other._denominator && _wholeNumber == other._wholeNumber;
        }

        private bool Equals(IFraction<long> other)
        {
            if (other == null)
                return false;

            if (other is Fraction64 f)
                return Equals(f);

            long w = NumericsExtensions.GetNormalizedRational64(other.WholeNumber, other.Numerator, other.Denominator, out long n, out long d);

            if (_numerator == 0)
                return n == 0 && _wholeNumber == w;

            return _numerator == n && _denominator == d && _wholeNumber == w;
        }

        bool IEquatable<IFraction<long>>.Equals(IFraction<long> other) { return Equals(other); }

        public bool Equals(IFraction other)
        {
            if (other == null)
                return false;

            if (other is IFraction<long> f)
                return Equals(f);

            if (other.GetMaxUnderlyingValue().CompareTo(long.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(long.MinValue) >= 0)
                return Equals(new Fraction64(Convert.ToInt64(other.WholeNumber), Convert.ToInt64(other.Numerator), Convert.ToInt64(other.Denominator)));

            return (new Fraction64(this)).Equals(other);
        }

        public override bool Equals(object obj) { return NumericsExtensions.EqualTo(this, obj); }

        #endregion

        public override int GetHashCode() { return ToSingle().GetHashCode(); }

        IComparable IFraction.GetMinUnderlyingValue() { return long.MinValue; }

        IComparable IFraction.GetMaxUnderlyingValue() { return long.MaxValue; }

        TypeCode IConvertible.GetTypeCode() { return TypeCode.Double; }

        #region Multiply

        public Fraction64 Multiply(long wholeNumber, long numerator, long denominator)
        {
            if ((_numerator == 0 && _wholeNumber == 0) || (numerator == 0 && wholeNumber == 0))
                return Zero;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);

            if (numerator == 0 && wholeNumber == 0)
                return Zero;

            w1 = NumericsExtensions.GetNormalizedRational64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new Fraction64(w1, n1, d1);
        }

        IFraction<long> IFraction<long>.Multiply(long wholeNumber, long numerator, long denominator) { return Multiply(wholeNumber, numerator, denominator); }

        public Fraction64 Multiply(long numerator, long denominator) { return Multiply(0, numerator, denominator); }

        IFraction<long> IFraction<long>.Multiply(long numerator, long denominator) { return Multiply(0, numerator, denominator); }

        public Fraction64 Multiply(Fraction64 other)
        {
            if ((_numerator == 0 && _wholeNumber == 0) || (other._numerator == 0 && other._wholeNumber == 0))
                return Zero;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2 = other._wholeNumber, n2 = other._numerator, d2 = other._denominator;

            w1 = NumericsExtensions.GetNormalizedRational64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new Fraction64(w1, n1, d1);
        }

        IFraction<long> IFraction<long>.Multiply(IFraction<long> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is Fraction64 f)
                return Multiply(f);

            return Multiply(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public Fraction64 Multiply(long wholeNumber) { return Multiply(wholeNumber, 0, 1); }

        IFraction<long> IFraction<long>.Multiply(long wholeNumber) { return Multiply(wholeNumber, 0, 1); }

        public IFraction Multiply(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<long> f)
                return Add(f);

            if (_numerator == 0 && _wholeNumber == 0)
                return other;

            if (other.Equals(0))
                return Zero;

            if (other.GetMaxUnderlyingValue().CompareTo(long.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(long.MinValue) >= 0)
                return Multiply(Convert.ToInt64(other.WholeNumber), Convert.ToInt64(other.Numerator), Convert.ToInt64(other.Denominator));

            return (new Fraction64(this)).Multiply(other);
        }

        #endregion

        #region Subtract

        public Fraction64 Subtract(long wholeNumber, long numerator, long denominator)
        {
            if (numerator == 0 && wholeNumber == 0)
                return (_denominator == 0) ? Zero : this;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = NumericsExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);
            NumericsExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            w1 = NumericsExtensions.GetNormalizedRational64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new Fraction64(w1, n1, d1);
        }

        IFraction<long> IFraction<long>.Subtract(long wholeNumber, long numerator, long denominator) { return Subtract(wholeNumber, numerator, denominator); }

        public Fraction64 Subtract(long numerator, long denominator) { return Subtract(0, numerator, denominator); }

        IFraction<long> IFraction<long>.Subtract(long numerator, long denominator) { return Subtract(0, numerator, denominator); }

        public Fraction64 Subtract(Fraction64 other)
        {
            if (other._denominator == 0)
                return Subtract(0, 0, 1);
            return Subtract(other._wholeNumber, other._numerator, other._denominator);
        }

        IFraction<long> IFraction<long>.Subtract(IFraction<long> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Subtract(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public Fraction64 Subtract(long wholeNumber) { return Subtract(wholeNumber, 0, 1); }

        IFraction<long> IFraction<long>.Subtract(long wholeNumber) { return Subtract(wholeNumber, 0, 1); }

        public IFraction Subtract(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<long> f)
                return Subtract(f);

            if (other.GetMaxUnderlyingValue().CompareTo(long.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(long.MinValue) >= 0)
                return Subtract(Convert.ToInt64(other.WholeNumber), Convert.ToInt64(other.Numerator), Convert.ToInt64(other.Denominator));

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
            if (_numerator == 0)
                return Convert.ToDecimal(_wholeNumber);
            return Convert.ToDecimal(_wholeNumber) + (Convert.ToDecimal(_numerator) / Convert.ToDecimal(_denominator));
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) { return ToDecimal(); }

        public double ToDouble()
        {
            if (_numerator == 0)
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
            if (_numerator == 0)
                return Convert.ToSingle(_wholeNumber);
            return Convert.ToSingle(_wholeNumber) + (Convert.ToSingle(_numerator) / Convert.ToSingle(_denominator));
        }

        float IConvertible.ToSingle(IFormatProvider provider) { return ToSingle(); }

        public override string ToString() => _numerator == 0
                ? _wholeNumber.ToString()
                : _wholeNumber == 0 ? $"{_numerator}/{_denominator}" : $"{_wholeNumber} {_numerator}/{_denominator}";

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
