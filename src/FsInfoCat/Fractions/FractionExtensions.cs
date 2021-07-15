using System;
using System.Text.RegularExpressions;

namespace FsInfoCat.Fractions
{
    public static class FractionExtensions
    {
        #region Generic Methods

        private static T GetGCD<T>(IValueHelper<T> valueHelper, T d1, params T[] denominators)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (denominators == null || denominators.Length == 0)
                return d1;
            T gcd = valueHelper.Abs(d1);
            foreach (T d in denominators)
            {
                T b;
                if (d.CompareTo(gcd) > 0)
                {
                    b = gcd;
                    gcd = valueHelper.Abs(d);
                }
                else
                    b = valueHelper.Abs(d);
                while (valueHelper.IsPositiveNonZero(b))
                {
                    T rem = valueHelper.Modulus(gcd, b);
                    gcd = b;
                    b = rem;
                }
            }

            return gcd;
        }

        private static T GetLCM<T>(IValueHelper<T> valueHelper, T d1, T d2, out T secondMultiplier)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            T zero = default;
            if (d1.CompareTo(zero) < 0)
                return GetLCM(valueHelper, valueHelper.Abs(d1), d2, out secondMultiplier);

            if (d2.CompareTo(zero) < 0)
                return GetLCM(valueHelper, d1, valueHelper.Abs(d2), out secondMultiplier);

            if (d1.Equals(d2))
            {
                secondMultiplier = valueHelper.PositiveOne;
                return secondMultiplier;
            }

            if (d1.CompareTo(d2) < 0)
            {
                secondMultiplier = GetLCM(valueHelper, d2, d1, out d1);
                return d1;
            }

            secondMultiplier = d1;

            while (!valueHelper.Modulus(secondMultiplier, d2).Equals(zero))
                secondMultiplier = valueHelper.Add(secondMultiplier, d1);

            return GetSimplifiedRational(valueHelper, valueHelper.Divide(secondMultiplier, d1), secondMultiplier, out secondMultiplier);
        }

        private static T GetSimplifiedRational<T>(IValueHelper<T> valueHelper, T n, T d, out T denominator)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (valueHelper.IsZero(d))
                throw new DivideByZeroException();

            if (valueHelper.IsZero(n))
            {
                denominator = valueHelper.PositiveOne;
                return valueHelper.Zero;
            }

            if (n.Equals(d))
            {
                denominator = valueHelper.PositiveOne;
                return valueHelper.PositiveOne;
            }

            if (valueHelper.IsNegative(d))
            {
                d = valueHelper.InvertSigned(d);
                n = valueHelper.InvertSigned(n);
            }
            T gcd = GetGCD(valueHelper, n, d);
            denominator = valueHelper.Divide(d, gcd);
            return valueHelper.Divide(n, gcd);
        }

        private static T GetNormalizedRational<T>(IValueHelper<T> valueHelper, T w, T n, T d, out T numerator, out T denominator)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            n = GetSimplifiedRational(valueHelper, n, d, out denominator);

            if (valueHelper.IsZero(n))
            {
                numerator = n;
                return w;
            }

            if (denominator.Equals(valueHelper.PositiveOne))
            {
                numerator = valueHelper.Zero;
                return valueHelper.Add(w, n);
            }

            if (n.CompareTo(denominator) > 0)
            {
                numerator = valueHelper.Modulus(n, denominator);
                if (valueHelper.IsNegative(w))
                    w = valueHelper.Subtract(w, valueHelper.Divide(valueHelper.Subtract(n, numerator), denominator));
                else
                    w = valueHelper.Add(w, valueHelper.Divide(valueHelper.Subtract(n, numerator), denominator));
                numerator = GetSimplifiedRational(valueHelper, numerator, denominator, out denominator);
            }
            else
                numerator = n;

            if (valueHelper.IsZero(w))
                return w;

            if (valueHelper.IsNegative(numerator))
            {
                if (valueHelper.IsNegative(w))
                    w = valueHelper.Add(w, valueHelper.PositiveOne);
                else
                    w = valueHelper.Subtract(w, valueHelper.PositiveOne);
                numerator = valueHelper.Add(numerator, denominator);
            }

            return w;
        }

        private static T GetInvertedRational<T>(IValueHelper<T> valueHelper, T w, T n, T d, out T numerator, out T denominator)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            w = GetNormalizedRational(valueHelper, w, n, d, out numerator, out denominator);

            if (valueHelper.IsZero(numerator))
            {
                if (valueHelper.IsZero(w))
                    return w;
                numerator = valueHelper.PositiveOne;
                denominator = w;
                return valueHelper.Zero;
            }

            if (valueHelper.IsZero(w))
                return GetNormalizedRational(valueHelper, valueHelper.Zero, d, n, out numerator, out denominator);

            return GetNormalizedRational(valueHelper, valueHelper.Zero, d, valueHelper.Add(n, valueHelper.Multiply(d, w)), out numerator, out denominator);
        }

        private static void ToCommonDenominator<T>(IValueHelper<T> valueHelper, ref T n1, ref T d1, ref T n2, ref T d2)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (valueHelper.IsZero(d1) || valueHelper.IsZero(d2))
                throw new DivideByZeroException();

            if (valueHelper.IsZero(n1))
                d1 = d2;
            else if (valueHelper.IsZero(n2))
                d2 = d1;
            else if (!d1.Equals(d2))
            {
                n1 = GetSimplifiedRational(valueHelper, n1, d1, out d1);
                n2 = GetSimplifiedRational(valueHelper, n2, d2, out d2);

                if (d1.Equals(valueHelper.PositiveOne))
                    n1 = valueHelper.Multiply(n1, d2);
                else if (d2.Equals(valueHelper.PositiveOne))
                    n2 = valueHelper.Multiply(n2, d1);
                else if (!d1.Equals(d2))
                {
                    T m1 = GetLCM(valueHelper, d1, d2, out _);
                    n1 = valueHelper.Multiply(n1, m1);
                    d1 = valueHelper.Multiply(d1, m1);
                    n2 = valueHelper.Multiply(n2, m1);
                    d2 = valueHelper.Multiply(d2, m1);
                }
            }
        }

        private static readonly Regex FractionParseRegex = new(@"^(?(?=-?\d+(\s|$))(?<w>-?\d+)(\s+(?<n>-?\d+)/(?<d>-?\d+))?|(?<n>-?\d+)/(?<d>-?\d+))$", RegexOptions.Compiled);

        private static T Parse<T>(IValueHelper<T> valueHelper, string s, out T n, out T d)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (s.Length == 0)
                throw new FormatException("Input string was empty.");

            Match m = FractionParseRegex.Match(s);
            if (!m.Success)
                throw new FormatException("Input string was not in a correct format.");

            T w;
            if (m.Groups["w"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["w"].Value, out w))
                    throw new FormatException("Whole number in input string was not in a correct format.");
            }
            else
                w = valueHelper.Zero;
            if (m.Groups["n"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["n"].Value, out n))
                    throw new FormatException("Numerator in input string was not in a correct format.");
                if (!valueHelper.TryParse(m.Groups["d"].Value, out d))
                    throw new FormatException("Denominator in input string was not in a correct format.");
            }
            else
            {
                n = valueHelper.Zero;
                d = valueHelper.PositiveOne;
            }
            return w;
        }

        private static bool TryParse<T>(IValueHelper<T> valueHelper, string s, out T w, out T n, out T d)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            Match m;
            if (string.IsNullOrEmpty(s) || !(m = FractionParseRegex.Match(s)).Success)
            {
                w = valueHelper.Zero;
                n = valueHelper.Zero;
                d = valueHelper.PositiveOne;
                return false;
            }
            if (m.Groups["w"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["w"].Value, out w))
                {
                    n = valueHelper.Zero;
                    d = valueHelper.PositiveOne;
                    return false;
                }
            }
            else
                w = valueHelper.Zero;
            if (m.Groups["n"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["n"].Value, out n))
                {
                    d = valueHelper.PositiveOne;
                    return false;
                }
                if (!valueHelper.TryParse(m.Groups["d"].Value, out d))
                    return false;
            }
            else
            {
                n = valueHelper.Zero;
                d = valueHelper.PositiveOne;
            }

            return true;
        }

        public static bool EqualTo<T>(IFraction<T> fraction, object obj)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (obj == null)
                return fraction is null;

            if (fraction is null)
                return false;

            if (obj is IFraction n)
                return fraction.Equals(n);

            if (obj is double d)
                return fraction.ToDouble().Equals(d);

            if (obj is float f)
                return fraction.ToSingle().Equals(f);

            if (obj is decimal m)
                return fraction.ToDecimal().Equals(m);

            if (obj is string s)
                return s.Equals(fraction.ToString());

            if (obj is IComparable c)
                return c.CompareTo(fraction.ToDecimal()) == 0;

            if (obj is IConvertible v)
            {
                TypeCode typeCode = v.GetTypeCode();
                return typeCode switch
                {
                    TypeCode.Decimal => EqualTo(fraction, Convert.ToDecimal(obj)),
                    TypeCode.Double => EqualTo(fraction, Convert.ToDouble(obj)),
                    TypeCode.Single => EqualTo(fraction, Convert.ToSingle(obj)),
                    TypeCode.Int16 => EqualTo(fraction, Convert.ToInt16(obj)),
                    TypeCode.Int32 => EqualTo(fraction, Convert.ToInt32(obj)),
                    TypeCode.Int64 => EqualTo(fraction, Convert.ToInt64(obj)),
                    TypeCode.Byte => EqualTo(fraction, Convert.ToByte(obj)),
                    TypeCode.SByte => EqualTo(fraction, Convert.ToSByte(obj)),
                    TypeCode.UInt32 => EqualTo(fraction, Convert.ToUInt32(obj)),
                    TypeCode.UInt64 => EqualTo(fraction, Convert.ToUInt64(obj)),
                    TypeCode.UInt16 => EqualTo(fraction, Convert.ToUInt16(obj)),
                    _ => fraction.ToString() == obj.ToString(),
                };
            }

            return false;
        }

        public static int Compare<T>(IFraction<T> fraction, object obj)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (obj == null)
                return 1;

            if (obj is IFraction n)
                return fraction.CompareTo(n);

            if (obj is double d)
                return fraction.ToDouble().CompareTo(d);

            if (obj is float f)
                return fraction.ToSingle().CompareTo(f);

            if (obj is decimal m)
                return fraction.ToDecimal().CompareTo(m);

            if (obj is string s)
                return s.CompareTo(fraction.ToString());

            if (obj is IComparable c)
                return 0 - c.CompareTo(fraction.ToDecimal());

            if (obj is IConvertible v)
            {
                TypeCode typeCode = v.GetTypeCode();
                return typeCode switch
                {
                    TypeCode.Decimal => Compare(fraction, Convert.ToDecimal(obj)),
                    TypeCode.Double => Compare(fraction, Convert.ToDouble(obj)),
                    TypeCode.Single => Compare(fraction, Convert.ToSingle(obj)),
                    TypeCode.Int16 => Compare(fraction, Convert.ToInt16(obj)),
                    TypeCode.Int32 => Compare(fraction, Convert.ToInt32(obj)),
                    TypeCode.Int64 => Compare(fraction, Convert.ToInt64(obj)),
                    TypeCode.Byte => Compare(fraction, Convert.ToByte(obj)),
                    TypeCode.SByte => Compare(fraction, Convert.ToSByte(obj)),
                    TypeCode.UInt32 => Compare(fraction, Convert.ToUInt32(obj)),
                    TypeCode.UInt64 => Compare(fraction, Convert.ToUInt64(obj)),
                    TypeCode.UInt16 => Compare(fraction, Convert.ToUInt16(obj)),
                    _ => (fraction.ToString() ?? "").CompareTo(obj.ToString() ?? ""),
                };
            }

            return -1;
        }

        #endregion

        #region 64-bit Methods

        public static long OneIfZero(this long value) => (value == 0L) ? 1L : value;

        public static long Parse64(string s, out long n, out long d) { return Parse(ValueHelper64.Instance, s, out n, out d); }

        public static bool TryParse64(string s, out long w, out long n, out long d) { return TryParse(ValueHelper64.Instance, s, out w, out n, out d); }

        public static long GetGCD64(long d1, params long[] denominators) { return GetGCD(ValueHelper64.Instance, d1, denominators); }

        public static long GetLCM64(long d1, long d2, out long secondMultiplier) { return GetLCM(ValueHelper64.Instance, d1, d2, out secondMultiplier); }

        public static long GetSimplifiedRational64(long n, long d, out long denominator) { return GetSimplifiedRational(ValueHelper64.Instance, n, d, out denominator); }

        public static long GetInvertedRational64(long w, long n, long d, out long numerator, out long denominator) { return GetInvertedRational(ValueHelper64.Instance, w, n, d, out numerator, out denominator); }

        public static void ToCommonDenominator64(ref long n1, ref long d1, ref long n2, ref long d2) { ToCommonDenominator(ValueHelper64.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static long GetNormalizedRational64(long w, long n, long d, out long numerator, out long denominator) { return GetNormalizedRational(ValueHelper64.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region 32-bit Methods

        public static int OneIfZero(this int value) => (value == 0) ? 1 : value;

        public static uint OneIfZero(this uint value) => (value == 0) ? 1 : value;

        public static int Parse32(string s, out int n, out int d) { return Parse(ValueHelper32.Instance, s, out n, out d); }

        public static uint ParseU32(string s, out uint n, out uint d) { return Parse(ValueHelperU32.Instance, s, out n, out d); }

        public static bool TryParse32(string s, out int w, out int n, out int d) { return TryParse(ValueHelper32.Instance, s, out w, out n, out d); }

        public static bool TryParseU32(string s, out uint w, out uint n, out uint d) { return TryParse(ValueHelperU32.Instance, s, out w, out n, out d); }

        public static int GetGCD(int d1, params int[] denominators) { return GetGCD(ValueHelper32.Instance, d1, denominators); }

        public static uint GetGCD(uint d1, params uint[] denominators) { return GetGCD(ValueHelperU32.Instance, d1, denominators); }

        public static int GetLCM(int d1, int d2, out int secondMultiplier) { return GetLCM(ValueHelper32.Instance, d1, d2, out secondMultiplier); }

        public static uint GetLCM(uint d1, uint d2, out uint secondMultiplier) { return GetLCM(ValueHelperU32.Instance, d1, d2, out secondMultiplier); }

        public static int GetSimplifiedRational(int n, int d, out int denominator) { return GetSimplifiedRational(ValueHelper32.Instance, n, d, out denominator); }

        public static uint GetSimplifiedRational(uint n, uint d, out uint denominator) { return GetSimplifiedRational(ValueHelperU32.Instance, n, d, out denominator); }

        public static int GetInvertedRational(int w, int n, int d, out int numerator, out int denominator) { return GetInvertedRational(ValueHelper32.Instance, w, n, d, out numerator, out denominator); }

        public static uint GetInvertedRational(uint w, uint n, uint d, out uint numerator, out uint denominator) { return GetInvertedRational(ValueHelperU32.Instance, w, n, d, out numerator, out denominator); }

        public static void ToCommonDenominator(ref uint n1, ref uint d1, ref uint n2, ref uint d2) { ToCommonDenominator(ValueHelperU32.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static void ToCommonDenominator(ref int n1, ref int d1, ref int n2, ref int d2) { ToCommonDenominator(ValueHelper32.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static uint GetNormalizedRational(uint w, uint n, uint d, out uint numerator, out uint denominator) { return GetNormalizedRational(ValueHelperU32.Instance, w, n, d, out numerator, out denominator); }

        public static int GetNormalizedRational(int w, int n, int d, out int numerator, out int denominator) { return GetNormalizedRational(ValueHelper32.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region 16-bit Methods

        public static short OneIfZero(this short value) => (value == 0) ? (short)1 : value;

        public static short Parse16(string s, out short n, out short d) { return Parse(ValueHelper16.Instance, s, out n, out d); }

        public static bool TryParse16(string s, out short w, out short n, out short d) { return TryParse(ValueHelper16.Instance, s, out w, out n, out d); }

        public static short GetGCD16(short d1, params short[] denominators) { return GetGCD(ValueHelper16.Instance, d1, denominators); }

        public static short GetLCM16(short d1, short d2, out short secondMultiplier) { return GetLCM(ValueHelper16.Instance, d1, d2, out secondMultiplier); }

        public static short GetSimplifiedRational16(short n, short d, out short denominator) { return GetSimplifiedRational(ValueHelper16.Instance, n, d, out denominator); }

        public static void ToCommonDenominator16(ref short n1, ref short d1, ref short n2, ref short d2) { ToCommonDenominator(ValueHelper16.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static short GetInvertedRational16(short w, short n, short d, out short numerator, out short denominator) { return GetInvertedRational(ValueHelper16.Instance, w, n, d, out numerator, out denominator); }

        public static short GetNormalizedRational16(short w, short n, short d, out short numerator, out short denominator) { return GetNormalizedRational(ValueHelper16.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region 8-bit Methods

        public static sbyte OneIfZero(this sbyte value) => (value == 0) ? (sbyte)1 : value;

        public static sbyte Parse8(string s, out sbyte n, out sbyte d) { return Parse(ValueHelper8.Instance, s, out n, out d); }

        public static bool TryParse8(string s, out sbyte w, out sbyte n, out sbyte d) { return TryParse(ValueHelper8.Instance, s, out w, out n, out d); }

        public static sbyte GetGCD8(sbyte d1, params sbyte[] denominators) { return GetGCD(ValueHelper8.Instance, d1, denominators); }

        public static sbyte GetLCM8(sbyte d1, sbyte d2, out sbyte secondMultiplier) { return GetLCM(ValueHelper8.Instance, d1, d2, out secondMultiplier); }

        public static sbyte GetSimplifiedRational8(sbyte n, sbyte d, out sbyte denominator) { return GetSimplifiedRational(ValueHelper8.Instance, n, d, out denominator); }

        public static sbyte GetInvertedRational8(sbyte w, sbyte n, sbyte d, out sbyte numerator, out sbyte denominator) { return GetInvertedRational(ValueHelper8.Instance, w, n, d, out numerator, out denominator); }

        public static void ToCommonDenominator8(ref sbyte n1, ref sbyte d1, ref sbyte n2, ref sbyte d2) { ToCommonDenominator(ValueHelper8.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static sbyte GetNormalizedRational8(sbyte w, sbyte n, sbyte d, out sbyte numerator, out sbyte denominator) { return GetNormalizedRational(ValueHelper8.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region IValueHelper<T> Implementations

        interface IValueHelper<T>
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            T Zero { get; }

            T PositiveOne { get; }

            bool IsZero(T value);

            bool IsNegative(T value);

            bool IsPositiveNonZero(T value);

            T Parse(string s);

            bool TryParse(string s, out T value);

            T InvertSigned(T value);

            T Abs(T value);

            T Add(T x, T y);

            T Subtract(T x, T y);

            T Divide(T x, T y);

            T Modulus(T x, T y);

            T Multiply(T x, T y);
        }

        class ValueHelper8 : IValueHelper<sbyte>
        {
            public static readonly ValueHelper8 Instance = new();

            private ValueHelper8() { }

            public sbyte Zero => 0;

            public sbyte PositiveOne => 1;

            public bool IsZero(sbyte value) => value == 0;

            public bool IsNegative(sbyte value) => value < 0;

            public bool IsPositiveNonZero(sbyte value) => value > 0;

            public sbyte InvertSigned(sbyte value) => (sbyte)(value * -1);

            public sbyte Parse(string s) => sbyte.Parse(s);

            public bool TryParse(string s, out sbyte value) => sbyte.TryParse(s, out value);

            public sbyte Abs(sbyte value) => Math.Abs(value);

            public sbyte Add(sbyte x, sbyte y) => (sbyte)(x + y);

            public sbyte Subtract(sbyte x, sbyte y) => (sbyte)(x - y);

            public sbyte Divide(sbyte x, sbyte y) => (sbyte)(x / y);

            public sbyte Modulus(sbyte x, sbyte y) => (sbyte)(x % y);

            public sbyte Multiply(sbyte x, sbyte y) => (sbyte)(x * y);
        }

        class ValueHelper16 : IValueHelper<short>
        {
            public static readonly ValueHelper16 Instance = new();

            private ValueHelper16() { }

            public short Zero => 0;

            public short PositiveOne => 1;

            public bool IsZero(short value) => value == 0;

            public bool IsNegative(short value) => value < 0;

            public bool IsPositiveNonZero(short value) => value > 0;

            public short InvertSigned(short value) => (short)(value * -1);

            public short Parse(string s) => short.Parse(s);

            public bool TryParse(string s, out short value) => short.TryParse(s, out value);

            public short Abs(short value) => Math.Abs(value);

            public short Add(short x, short y) => (short)(x + y);

            public short Subtract(short x, short y) => (short)(x - y);

            public short Divide(short x, short y) => (short)(x / y);

            public short Modulus(short x, short y) => (short)(x % y);

            public short Multiply(short x, short y) => (short)(x * y);
        }

        class ValueHelper32 : IValueHelper<int>
        {
            public static readonly ValueHelper32 Instance = new();

            private ValueHelper32() { }

            public int Zero => 0;

            public int PositiveOne => 1;

            public bool IsZero(int value) => value == 0;

            public bool IsNegative(int value) => value < 0;

            public bool IsPositiveNonZero(int value) => value > 0;

            public int InvertSigned(int value) => value * -1;

            public int Parse(string s) => int.Parse(s);

            public bool TryParse(string s, out int value) => int.TryParse(s, out value);

            public int Abs(int value) => Math.Abs(value);

            public int Add(int x, int y) => x + y;

            public int Subtract(int x, int y) => x - y;

            public int Divide(int x, int y) => x / y;

            public int Modulus(int x, int y) => x % y;

            public int Multiply(int x, int y) => x * y;
        }

        class ValueHelperU32 : IValueHelper<uint>
        {
            public static readonly ValueHelperU32 Instance = new();

            private ValueHelperU32() { }

            public uint Zero => 0;

            public uint PositiveOne => 1;

            public bool IsZero(uint value) => value == 0;

            public bool IsNegative(uint value) => false;

            public bool IsPositiveNonZero(uint value) => value > 0;

            public uint InvertSigned(uint value) => value;

            public uint Parse(string s) => uint.Parse(s);

            public bool TryParse(string s, out uint value) => uint.TryParse(s, out value);

            public uint Abs(uint value) => value;

            public uint Add(uint x, uint y) => x + y;

            public uint Subtract(uint x, uint y) => x - y;

            public uint Divide(uint x, uint y) => x / y;

            public uint Modulus(uint x, uint y) => x % y;

            public uint Multiply(uint x, uint y) => x * y;
        }

        class ValueHelper64 : IValueHelper<long>
        {
            public static readonly ValueHelper64 Instance = new();

            private ValueHelper64() { }

            public long Zero => 0L;

            public long PositiveOne => 1L;

            public bool IsZero(long value) => value == 0L;

            public bool IsNegative(long value) => value < 0L;

            public bool IsPositiveNonZero(long value) => value > 0L;

            public long InvertSigned(long value) => value * -1L;

            public long Parse(string s) => long.Parse(s);

            public bool TryParse(string s, out long value) => long.TryParse(s, out value);

            public long Abs(long value) => Math.Abs(value);

            public long Add(long x, long y) => x + y;

            public long Subtract(long x, long y) => x - y;

            public long Divide(long x, long y) => x / y;

            public long Modulus(long x, long y) => x % y;

            public long Multiply(long x, long y) => x * y;
        }

        #endregion
    }
}
