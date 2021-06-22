using System;
using System.Runtime.InteropServices;

namespace FsInfoCat.Fractions
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FractionU32 : IEquatable<FractionU32>, IComparable<FractionU32>, IFraction<uint>
    {
        #region Fields

        public static readonly FractionU32 Zero = new(0, 0, 1);
        private static readonly Coersion<uint> _coersion = Coersion<uint>.Default;


        private readonly uint _wholeNumber;
        private readonly uint _numerator;
        private readonly uint _denominator;

        #endregion

        #region Properties

        public uint WholeNumber { get { return _wholeNumber; } }

        IConvertible IFraction.WholeNumber { get { return _wholeNumber; } }

        public uint Numerator { get { return _numerator; } }

        IConvertible IFraction.Numerator { get { return _numerator; } }

        public uint Denominator { get { return _denominator; } }

        IConvertible IFraction.Denominator { get { return _denominator; } }

        #endregion

        #region Constructors

        public FractionU32(uint wholeNumber, uint numerator, uint denominator)
        {
            _wholeNumber = FractionExtensions.GetNormalizedRational(wholeNumber, numerator, denominator, out numerator, out denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionU32(uint numerator, uint denominator)
        {
            _wholeNumber = FractionExtensions.GetNormalizedRational(0, numerator, denominator, out numerator, out denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionU32(IFraction fraction)
        {
            _wholeNumber = FractionExtensions.GetNormalizedRational(_coersion.Coerce(fraction.WholeNumber), _coersion.Coerce(fraction.Numerator),
                _coersion.Coerce(fraction.Denominator).OneIfZero(), out uint numerator, out uint denominator);
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionU32(uint wholeNumber)
        {
            _wholeNumber = wholeNumber;
            _numerator = 0;
            _denominator = 1;
        }

        #endregion

        #region *Parse

        public static FractionU32 Parse(string s)
        {
            uint wholeNumber = FractionExtensions.ParseU32(s, out uint numerator, out uint denominator);
            return new FractionU32(wholeNumber, numerator, denominator);

        }

        public static bool TryParse(string s, out FractionU32 value)
        {
            if (FractionExtensions.TryParseU32(s, out uint wholeNumber, out uint numerator, out uint denominator))
            {
                value = new FractionU32(wholeNumber, numerator, denominator);
                return true;
            }

            value = new FractionU32();
            return false;
        }

        #endregion

        #region Operators

        public static FractionU32 operator +(FractionU32 x, FractionU32 y) { return x.Add(y); }

        public static FractionU32 operator -(FractionU32 x, FractionU32 y) { return x.Subtract(y); }

        public static FractionU32 operator *(FractionU32 x, FractionU32 y) { return x.Multiply(y); }

        public static FractionU32 operator /(FractionU32 x, FractionU32 y) { return x.Divide(y); }

        public static bool operator ==(FractionU32 x, FractionU32 y) { return x.Equals(y); }

        public static bool operator !=(FractionU32 x, FractionU32 y) { return !x.Equals(y); }

        public static bool operator <(FractionU32 x, FractionU32 y) { return x.CompareTo(y) < 0; }

        public static bool operator <=(FractionU32 x, FractionU32 y) { return x.CompareTo(y) <= 0; }

        public static bool operator >(FractionU32 x, FractionU32 y) { return x.CompareTo(y) > 0; }

        public static bool operator >=(FractionU32 x, FractionU32 y) { return x.CompareTo(y) >= 0; }

        #endregion

        #region Add

        public FractionU32 Add(uint wholeNumber, uint numerator, uint denominator)
        {
            if (_numerator == 0 && _wholeNumber == 0)
                return new FractionU32(wholeNumber, numerator, denominator);

            if (numerator == 0 && wholeNumber == 0)
                return (_denominator == 0) ? FractionU32.Zero : this;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = FractionExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);
            FractionExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            w1 = FractionExtensions.GetNormalizedRational64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new FractionU32((uint)w1, (uint)n1, (uint)d1);
        }

        IFraction<uint> IFraction<uint>.Add(uint wholeNumber, uint numerator, uint denominator) { return Add(wholeNumber, numerator, denominator); }

        public FractionU32 Add(uint numerator, uint denominator) { return Add(0, numerator, denominator); }

        IFraction<uint> IFraction<uint>.Add(uint numerator, uint denominator) { return Add(0, numerator, denominator); }

        public FractionU32 Add(FractionU32 other)
        {
            if (_numerator == 0 && _wholeNumber == 0)
                return (other._denominator == 0) ? FractionU32.Zero : other;

            if (other._numerator == 0 && other._wholeNumber == 0)
                return (_denominator == 0) ? FractionU32.Zero : this;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2 = other._wholeNumber, n2 = other._numerator, d2 = other._denominator;
            FractionExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            w1 = FractionExtensions.GetNormalizedRational64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new FractionU32((uint)w1, (uint)n1, (uint)d1);
        }

        IFraction<uint> IFraction<uint>.Add(IFraction<uint> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is FractionU32 f)
                return Add(f);

            return Add(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public FractionU32 Add(uint wholeNumber) { return Add(wholeNumber, 0, 1); }

        IFraction<uint> IFraction<uint>.Add(uint wholeNumber) { return Add(wholeNumber, 0, 1); }

        public IFraction Add(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<uint> f)
                return Add(f);

            if (other.Equals(0))
                return this;

            if (_numerator == 0 && _wholeNumber == 0)
                return other;

            if (other.GetMaxUnderlyingValue().CompareTo(uint.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(uint.MinValue) >= 0)
                return Add(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Add(other);
        }

        #endregion

        #region AsInverted

        public FractionU32 AsInverted()
        {
            if (_numerator == 0)
            {
                if (_wholeNumber == 0)
                    return FractionU32.Zero;
                return new FractionU32(0, 1, _wholeNumber);
            }

            if (_wholeNumber == 0)
                return new FractionU32(0, _denominator, _numerator);

            return new FractionU32(0, _denominator, _numerator + (_wholeNumber * _denominator));
        }

        IFraction<uint> IFraction<uint>.AsInverted() { return AsInverted(); }

        IFraction IFraction.AsInverted() { return AsInverted(); }

        #endregion

        public uint AsRoundedValue()
        {
            if (_numerator == 0 || _numerator < (_denominator >> 1))
                return _wholeNumber;

            return _wholeNumber + 1;
        }

        #region CompareTo

        public int CompareTo(FractionU32 other)
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
                    return (other._numerator == 0) ? 0 : -1;
                if (other._numerator == 0)
                    return 1;
            }

            long n1 = _numerator, d1 = _denominator, n2 = other._numerator, d2 = other._denominator;
            FractionExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            return n1.CompareTo(n2);
        }

        private int CompareTo(IFraction<uint> other)
        {
            if (other == null)
                return 1;

            if (other is FractionU32 f)
                return CompareTo(f);

            uint w = FractionExtensions.GetNormalizedRational(other.WholeNumber, other.Numerator, other.Denominator, out uint n, out uint d);

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
                    return (n == 0) ? 0 : -1;
                if (n == 0)
                    return 1;
                if (n == 0)
                    return 1;
            }

            long n1 = _numerator, d1 = _denominator, n2 = n, d2 = d;
            FractionExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            return n1.CompareTo(n2);
        }

        int IComparable<IFraction<uint>>.CompareTo(IFraction<uint> other) { return CompareTo(other); }

        public int CompareTo(IFraction other)
        {
            if (other == null)
                return 1;

            if (other is IFraction<uint> f)
                return CompareTo(f);

            if (other.GetMaxUnderlyingValue().CompareTo(uint.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(0) >= 0)
                return CompareTo(new FractionU32(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator)));

            return (new Fraction64(this)).CompareTo(other);
        }

        public int CompareTo(object obj) { return FractionExtensions.Compare<uint>(this, obj); }

        #endregion

        #region Divide

        public FractionU32 Divide(uint wholeNumber, uint numerator, uint denominator)
        {
            if (_numerator == 0 && _wholeNumber == 0)
                return FractionU32.Zero;

            if (numerator == 0 && wholeNumber == 0)
                throw new DivideByZeroException();

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = FractionExtensions.GetInvertedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);

            if (n2 == 0 && w2 == 0)
                throw new DivideByZeroException();

            w1 = FractionExtensions.GetNormalizedRational64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new FractionU32((uint)w1, (uint)n1, (uint)d1);
        }

        IFraction<uint> IFraction<uint>.Divide(uint wholeNumber, uint numerator, uint denominator) { return Divide(wholeNumber, numerator, denominator); }

        public FractionU32 Divide(uint numerator, uint denominator) { return Divide(0, numerator, denominator); }

        IFraction<uint> IFraction<uint>.Divide(uint numerator, uint denominator) { return Divide(0, numerator, denominator); }

        public FractionU32 Divide(FractionU32 other)
        {
            if (other._numerator == 0 && other._wholeNumber == 0)
                throw new DivideByZeroException();

            return Multiply(other.AsInverted());
        }

        IFraction<uint> IFraction<uint>.Divide(IFraction<uint> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.Numerator == 0 && other.WholeNumber == 0)
                throw new DivideByZeroException();

            if ((other = other.AsInverted()) is FractionU32)
                return Multiply((FractionU32)other);

            return Multiply(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public IFraction Divide(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<uint> f)
                return Add(f);

            if (other.Equals(0))
                throw new DivideByZeroException();

            if (_numerator == 0 && _wholeNumber == 0)
                return other;

            if (other.GetMaxUnderlyingValue().CompareTo(uint.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(uint.MinValue) >= 0)
                return Divide(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Divide(other);
        }

        public FractionU32 Divide(uint wholeNumber) { return Divide(wholeNumber, 0, 1); }

        IFraction<uint> IFraction<uint>.Divide(uint wholeNumber) { return Divide(wholeNumber, 0, 1); }

        #endregion

        #region Equals

        public bool Equals(FractionU32 other)
        {
            if (_numerator == 0)
                return other._numerator == 0 && _wholeNumber == other._wholeNumber;

            return _numerator == other._numerator && _denominator == other._denominator && _wholeNumber == other._wholeNumber;
        }

        private bool Equals(IFraction<uint> other)
        {
            if (other == null)
                return false;

            if (other is FractionU32 f)
                return Equals(f);

            uint w = FractionExtensions.GetNormalizedRational(other.WholeNumber, other.Numerator, other.Denominator, out uint n, out uint d);

            if (_numerator == 0)
                return n == 0 && _wholeNumber == w;

            return _numerator == n && _denominator == d && _wholeNumber == w;
        }

        bool IEquatable<IFraction<uint>>.Equals(IFraction<uint> other) { return Equals(other); }

        public bool Equals(IFraction other)
        {
            if (other == null)
                return false;

            if (other is IFraction<uint> f)
                return Equals(f);

            if (other.GetMaxUnderlyingValue().CompareTo(uint.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(uint.MinValue) >= 0)
                return Equals(new FractionU32(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator)));

            return (new Fraction64(this)).Equals(other);
        }

        public override bool Equals(object obj) { return FractionExtensions.EqualTo<uint>(this, obj); }

        #endregion

        public override int GetHashCode() { return ToSingle().GetHashCode(); }

        IComparable IFraction.GetMinUnderlyingValue() { return uint.MinValue; }

        IComparable IFraction.GetMaxUnderlyingValue() { return uint.MaxValue; }

        TypeCode IConvertible.GetTypeCode() { return TypeCode.Double; }

        #region Multiply

        public FractionU32 Multiply(uint wholeNumber, uint numerator, uint denominator)
        {
            if ((_numerator == 0 && _wholeNumber == 0) || (numerator == 0 && wholeNumber == 0))
                return FractionU32.Zero;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = FractionExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);

            if (numerator == 0 && wholeNumber == 0)
                return FractionU32.Zero;

            w1 = FractionExtensions.GetNormalizedRational64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new FractionU32((uint)w1, (uint)n1, (uint)d1);
        }

        IFraction<uint> IFraction<uint>.Multiply(uint wholeNumber, uint numerator, uint denominator) { return Multiply(wholeNumber, numerator, denominator); }

        public FractionU32 Multiply(uint numerator, uint denominator) { return Multiply(0, numerator, denominator); }

        IFraction<uint> IFraction<uint>.Multiply(uint numerator, uint denominator) { return Multiply(0, numerator, denominator); }

        public FractionU32 Multiply(FractionU32 other)
        {
            if ((_numerator == 0 && _wholeNumber == 0) || (other._numerator == 0 && other._wholeNumber == 0))
                return FractionU32.Zero;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2 = other._wholeNumber, n2 = other._numerator, d2 = other._denominator;

            w1 = FractionExtensions.GetNormalizedRational64(w1 * w2, n1 * n2, d1 * d2, out n1, out d1);
            return new FractionU32((uint)w1, (uint)n1, (uint)d1);
        }

        IFraction<uint> IFraction<uint>.Multiply(IFraction<uint> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is FractionU32 f)
                return Multiply(f);

            return Multiply(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public FractionU32 Multiply(uint wholeNumber) { return Multiply(wholeNumber, 0, 1); }

        IFraction<uint> IFraction<uint>.Multiply(uint wholeNumber) { return Multiply(wholeNumber, 0, 1); }

        public IFraction Multiply(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<uint> f)
                return Add(f);

            if (_numerator == 0 && _wholeNumber == 0)
                return other;

            if (other.Equals(0))
                return FractionU32.Zero;

            if (other.GetMaxUnderlyingValue().CompareTo(uint.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(uint.MinValue) >= 0)
                return Multiply(Convert.ToUInt32(other.WholeNumber), Convert.ToUInt32(other.Numerator), Convert.ToUInt32(other.Denominator));

            return (new Fraction64(this)).Multiply(other);
        }

        #endregion

        #region Subtract

        public FractionU32 Subtract(uint wholeNumber, uint numerator, uint denominator)
        {
            if (numerator == 0 && wholeNumber == 0)
                return (_denominator == 0) ? FractionU32.Zero : this;

            long w1 = _wholeNumber, n1 = _numerator, d1 = _denominator, w2;
            w2 = FractionExtensions.GetNormalizedRational64(wholeNumber, numerator, denominator, out long n2, out long d2);
            FractionExtensions.ToCommonDenominator64(ref n1, ref d1, ref n2, ref d2);
            w1 = FractionExtensions.GetNormalizedRational64(w1 + w2, n1 + n2, d1, out n1, out d1);
            return new FractionU32((uint)w1, (uint)n1, (uint)d1);
        }

        IFraction<uint> IFraction<uint>.Subtract(uint wholeNumber, uint numerator, uint denominator) { return Subtract(wholeNumber, numerator, denominator); }

        public FractionU32 Subtract(uint numerator, uint denominator) { return Subtract(0, numerator, denominator); }

        IFraction<uint> IFraction<uint>.Subtract(uint numerator, uint denominator) { return Subtract(0, numerator, denominator); }

        public FractionU32 Subtract(FractionU32 other)
        {
            if (other._denominator == 0)
                return Subtract(0, 0, 1);
            return Subtract(other._wholeNumber, other._numerator, other._denominator);
        }

        IFraction<uint> IFraction<uint>.Subtract(IFraction<uint> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Subtract(other.WholeNumber, other.Numerator, other.Denominator);
        }

        public FractionU32 Subtract(uint wholeNumber) { return Subtract(wholeNumber, 0, 1); }

        IFraction<uint> IFraction<uint>.Subtract(uint wholeNumber) { return Subtract(wholeNumber, 0, 1); }

        public IFraction Subtract(IFraction other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFraction<uint> f)
                return Subtract(f);

            if (other.GetMaxUnderlyingValue().CompareTo(uint.MaxValue) <= 0 && other.GetMinUnderlyingValue().CompareTo(uint.MinValue) >= 0)
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

        public override string ToString()
        {
            if (_numerator == 0)
                return _wholeNumber.ToString();

            if (_wholeNumber == 0)
                return _numerator.ToString() + "/" + _denominator.ToString();

            return _wholeNumber.ToString() + " " + _numerator.ToString() + "/" + _denominator.ToString();
        }

        string IConvertible.ToString(IFormatProvider provider) { return ToString(); }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == null || conversionType.AssemblyQualifiedName == (typeof(double)).AssemblyQualifiedName)
                return ToDouble();
            IConvertible c = this;
            if (conversionType.AssemblyQualifiedName == (typeof(float)).AssemblyQualifiedName)
                c.ToSingle(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(int)).AssemblyQualifiedName)
                c.ToInt32(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(string)).AssemblyQualifiedName)
                c.ToString(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(long)).AssemblyQualifiedName)
                c.ToInt64(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(decimal)).AssemblyQualifiedName)
                c.ToDecimal(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(uint)).AssemblyQualifiedName)
                c.ToUInt32(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(ulong)).AssemblyQualifiedName)
                c.ToUInt64(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(short)).AssemblyQualifiedName)
                c.ToInt16(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(ushort)).AssemblyQualifiedName)
                c.ToUInt16(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(sbyte)).AssemblyQualifiedName)
                c.ToSByte(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(byte)).AssemblyQualifiedName)
                c.ToByte(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(DateTime)).AssemblyQualifiedName)
                c.ToDateTime(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(bool)).AssemblyQualifiedName)
                c.ToBoolean(provider);
            if (conversionType.AssemblyQualifiedName == (typeof(char)).AssemblyQualifiedName)
                c.ToChar(provider);
            return Convert.ChangeType(ToDouble(), conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) { return Convert.ToUInt16(AsRoundedValue(), provider); }

        uint IConvertible.ToUInt32(IFormatProvider provider) { return Convert.ToUInt32(AsRoundedValue(), provider); }

        ulong IConvertible.ToUInt64(IFormatProvider provider) { return Convert.ToUInt64(AsRoundedValue(), provider); }

        #endregion
    }
}
