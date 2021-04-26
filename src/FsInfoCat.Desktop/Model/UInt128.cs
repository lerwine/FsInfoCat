using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Desktop.Model
{
    /// <summary>
    /// Represents a unsigned 128-bit numeric value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct UInt128 : IComparable, IEquatable<UInt128>, IComparable<UInt128>, IReadOnlyList<byte>
    {
        private const decimal FIRST_HIGH_BIT_M = 18446744073709551616m;
        private const double FIRST_HIGH_BIT_D = 18446744073709551616.0;
        private const float FIRST_HIGH_BIT_F = 18446744073709551616f;
        private const ulong MAX_DECIMAL_HIGH_BITS = 4294967295UL;
        private const ulong MAX_SINGLE_HIGH_BITS = 18446742974197923840UL;
        public const int ByteSize = 16;

        #region Fields

        private static readonly Regex HexPattern = new Regex(@"\s*([^a-fA-F\d\s]+)?([a-fA-F\d][a-fA-F\d])\s*([\-:,;]\s*)?", RegexOptions.Compiled);

        private static readonly Regex HexDigit = new Regex(@"(?:[\s\-:,;]|([a-fA-F\d])|\s*$)", RegexOptions.Compiled);

        public static readonly UInt128 MaxValue = new UInt128(ulong.MaxValue, ulong.MaxValue);

        public static readonly UInt128 MinValue = new UInt128(0UL, 0UL);

        [FieldOffset(0)]
        private readonly ulong _lowBits;

        [FieldOffset(0)]
        private readonly uint _dword0;

        [FieldOffset(0)]
        private readonly int _sDword0;

        [FieldOffset(0)]
        private readonly ushort _word0;

        [FieldOffset(0)]
        private readonly short _sWord0;

        [FieldOffset(0)]
        private readonly byte _b0;

        [FieldOffset(1)]
        private readonly byte _b1;

        [FieldOffset(2)]
        private readonly ushort _word1;

        [FieldOffset(2)]
        private readonly short _sWord1;

        [FieldOffset(2)]
        private readonly byte _b2;

        [FieldOffset(3)]
        private readonly byte _b3;

        [FieldOffset(4)]
        private readonly ushort _word2;

        [FieldOffset(4)]
        private readonly short _sWord2;

        [FieldOffset(4)]
        private readonly uint _dword1;

        [FieldOffset(4)]
        private readonly int _sDword1;

        [FieldOffset(4)]
        private readonly byte _b4;

        [FieldOffset(5)]
        private readonly byte _b5;

        [FieldOffset(6)]
        private readonly ushort _word3;

        [FieldOffset(6)]
        private readonly short _sWord3;

        [FieldOffset(6)]
        private readonly byte _b6;

        [FieldOffset(7)]
        private readonly byte _b7;

        [FieldOffset(8)]
        private readonly ushort _word4;

        [FieldOffset(8)]
        private readonly short _sWord4;

        [FieldOffset(8)]
        private readonly ulong _highBits;

        [FieldOffset(8)]
        private readonly uint _dword2;

        [FieldOffset(8)]
        private readonly int _sDword2;

        [FieldOffset(8)]
        private readonly byte _b8;

        [FieldOffset(9)]
        private readonly byte _b9;

        [FieldOffset(10)]
        private readonly ushort _word5;

        [FieldOffset(10)]
        private readonly short _sWord5;

        [FieldOffset(10)]
        private readonly byte _b10;

        [FieldOffset(11)]
        private readonly byte _b11;

        [FieldOffset(12)]
        private readonly ushort _word6;

        [FieldOffset(12)]
        private readonly short _sWord6;

        [FieldOffset(12)]
        private readonly uint _dword3;

        [FieldOffset(12)]
        private readonly int _sDword3;

        [FieldOffset(12)]
        private readonly byte _b12;

        [FieldOffset(13)]
        private readonly byte _b13;

        [FieldOffset(14)]
        private readonly ushort _word7;

        [FieldOffset(14)]
        private readonly short _sWord7;

        [FieldOffset(14)]
        private readonly byte _b14;

        [FieldOffset(15)]
        private readonly byte _b15;

        #endregion

        #region Properties

        /// <summary>
        /// Contains the lower 64 bits of the 128-bit value.
        /// </summary>
        public ulong LowBits => _lowBits;

        /// <summary>
        /// Contains the lower 32 bits of the 128-bit value.
        /// </summary>
        public uint DWord0 => _dword0;

        /// <summary>
        /// Contains the lower 16 bits of the 128-bit value.
        /// </summary>
        public ushort Word0 => _word0;

        /// <summary>
        /// Contains the bits 16 through 31 of the 128-bit value.
        /// </summary>
        public ushort Word1 => _word1;

        /// <summary>
        /// Contains bits 32 through 63 of the 128-bit value.
        /// </summary>
        public uint DWord1 => _dword1;

        /// <summary>
        /// Contains the bits 32 through 47 of the 128-bit value.
        /// </summary>
        public ushort Word2 => _word2;

        /// <summary>
        /// Contains the bits 48 through 63 of the 128-bit value.
        /// </summary>
        public ushort Word3 => _word3;

        /// <summary>
        /// Contains the upper 64 bits of the 128-bit value.
        /// </summary>
        public ulong HighBits => _highBits;

        /// <summary>
        /// Contains the bits 64 through 95 of the 128-bit value.
        /// </summary>
        public uint DWord2 => _dword2;

        /// <summary>
        /// Contains the bits 64 through 79 of the 128-bit value.
        /// </summary>
        public ushort Word4 => _word4;

        /// <summary>
        /// Contains the bits 80 through 95 of the 128-bit value.
        /// </summary>
        public ushort Word5 => _word5;

        /// <summary>
        /// Contains the upper 32 bits of the 128-bit value.
        /// </summary>
        public uint DWord3 => _dword3;

        /// <summary>
        /// Contains the bits 96 through 111 of the 128-bit value.
        /// </summary>
        public ushort Word6 => _word6;

        /// <summary>
        /// Contains the upper 16 of the 128-bit value.
        /// </summary>
        public ushort Word7 => _word7;

        public int Count => ByteSize;

        public int GetInt32(int index)
        {
            switch (index)
            {
                case 0:
                    return _sDword0;
                case 1:
                    return _sDword1;
                case 2:
                    return _sDword2;
                case 3:
                    return _sDword3;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public int GetInt16(int index)
        {
            switch (index)
            {
                case 0:
                    return _sWord0;
                case 1:
                    return _sWord1;
                case 2:
                    return _sWord2;
                case 3:
                    return _sWord3;
                case 4:
                    return _sWord4;
                case 5:
                    return _sWord5;
                case 6:
                    return _sWord6;
                case 7:
                    return _sWord7;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public byte this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0:
                        return _b0;
                    case 1:
                        return _b1;
                    case 2:
                        return _b2;
                    case 3:
                        return _b3;
                    case 4:
                        return _b4;
                    case 5:
                        return _b5;
                    case 6:
                        return _b6;
                    case 7:
                        return _b7;
                    case 8:
                        return _b8;
                    case 9:
                        return _b9;
                    case 10:
                        return _b10;
                    case 11:
                        return _b11;
                    case 12:
                        return _b12;
                    case 13:
                        return _b13;
                    case 14:
                        return _b14;
                    case 15:
                        return _b15;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructors

        public UInt128(ulong lowBits, ulong highBits)
        {
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _highBits = highBits;
            _lowBits = lowBits;
        }

        public UInt128(int dword0, int dword1, int dword2, int dword3)
        {
            _highBits = _lowBits = 0L;
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _sDword0 = dword0;
            _sDword1 = dword1;
            _sDword2 = dword2;
            _sDword3 = dword3;
        }

        public UInt128(uint dword0, uint dword1, uint dword2, uint dword3)
        {
            _highBits = _lowBits = 0L;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _dword0 = dword0;
            _dword1 = dword1;
            _dword2 = dword2;
            _dword3 = dword3;
        }

        public UInt128(ulong value)
        {
            _highBits = 0L;
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _lowBits = value;
        }

        public UInt128(long value)
        {
            if (value < 0)
                throw new OverflowException();
            _highBits = 0L;
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _lowBits = (ulong)value;
        }

        public UInt128(uint value)
        {
            _highBits = _lowBits = 0L;
            _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _dword0 = value;
        }

        public UInt128(int value)
        {
            if (value < 0)
                throw new OverflowException();
            _highBits = _lowBits = 0L;
            _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            _dword0 = (uint)value;
        }

        public UInt128(float value)
        {
            if (value < 0f)
                throw new OverflowException();
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            if (value < FIRST_HIGH_BIT_F)
            {
                _highBits = 0L;
                _lowBits = Convert.ToUInt64(value);
            }
            else
            {
                float lowBits = value - FIRST_HIGH_BIT_F;
                _lowBits = Convert.ToUInt64(lowBits);
                _highBits = Convert.ToUInt64((value - lowBits) / FIRST_HIGH_BIT_F);
            }
        }

        public UInt128(double value)
        {
            if (value < 0.0)
                throw new OverflowException();
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            if (value < FIRST_HIGH_BIT_D)
            {
                _highBits = 0L;
                _lowBits = Convert.ToUInt64(value);
            }
            else
            {
                double lowBits = value - FIRST_HIGH_BIT_D;
                _lowBits = Convert.ToUInt64(lowBits);
                _highBits = Convert.ToUInt64((value - lowBits) / FIRST_HIGH_BIT_D);
            }
        }

        public UInt128(decimal value)
        {
            if (value < 0m)
                throw new OverflowException();
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = 0;
            if (value < FIRST_HIGH_BIT_M)
            {
                _highBits = 0L;
                _lowBits = Convert.ToUInt64(value);
            }
            else
            {
                decimal lowBits = value - FIRST_HIGH_BIT_M;
                _lowBits = Convert.ToUInt64(lowBits);
                _highBits = Convert.ToUInt64((value - lowBits) / FIRST_HIGH_BIT_M);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UInt128"/>.
        /// </summary>
        /// <param name="buffer">A <seealso cref="byte"/> array representing the 128-bit value. A null value or empty array initializes a zero checksum value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="buffer"/> was not null, empty or 16 bytes in length.</exception>
        public UInt128(byte[] buffer)
        {
            _highBits = _lowBits = 0L;
            _sDword0 = _sDword1 = _sDword2 = _sDword3 = 0;
            _word0 = _word1 = _word2 = _word3 = _word4 = _word5 = _word6 = _word7 = 0;
            _sWord0 = _sWord1 = _sWord2 = _sWord3 = _sWord4 = _sWord5 = _sWord6 = _sWord7 = 0;
            _dword0 = _dword1 = _dword2 = _dword3 = 0;
            if (buffer == null || buffer.Length == 0)
                _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = (byte)0;
            else
            {
                if (buffer.Length != ByteSize)
                    throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer length must be 16 bytes");
                _b0 = buffer[0];
                _b1 = buffer[1];
                _b2 = buffer[2];
                _b3 = buffer[3];
                _b4 = buffer[4];
                _b5 = buffer[5];
                _b6 = buffer[6];
                _b7 = buffer[7];
                _b8 = buffer[8];
                _b9 = buffer[9];
                _b10 = buffer[10];
                _b11 = buffer[11];
                _b12 = buffer[12];
                _b13 = buffer[13];
                _b14 = buffer[14];
                _b15 = buffer[15];
            }
        }

        #endregion

        #region Methods

        public IEnumerable<byte> GetBytes()
        {
            yield return _b0;
            yield return _b1;
            yield return _b2;
            yield return _b3;
            yield return _b4;
            yield return _b5;
            yield return _b6;
            yield return _b7;
            yield return _b8;
            yield return _b9;
            yield return _b10;
            yield return _b11;
            yield return _b12;
            yield return _b13;
            yield return _b14;
            yield return _b15;
        }

        #region Equals

        /// <summary>
        /// Determines whether the current <see cref="UInt128"/> is equal to another.
        /// </summary>
        /// <param name="other"><see cref="UInt128"/> to compare to.</param>
        /// <returns><c>true</c> if the current <see cref="UInt128"/> is equal to <paramref name="other"/>; othwerise, <c>false</c>.</returns>
        public bool Equals(UInt128 other) => _highBits == other._highBits && _lowBits == other._lowBits;

        /// <summary>
        /// Determines whether the current <see cref="UInt128"/> is equal to another object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns><c>true</c> if <paramref name="other"/> is an <see cref="UInt128"/> and is equal to the current <see cref="UInt128"/>; othwerise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj != null && obj is UInt128 checksum && Equals(checksum);

        #endregion

        /// <summary>
        /// Gets the hashcode for the current <see cref="UInt128"/>.
        /// </summary>
        /// <returns>The hashcode for the current <see cref="UInt128"/>.</returns>
        public override int GetHashCode() => (int)(_lowBits & 0xFFFF) | ((int)(_highBits & 0xFFFF) << ByteSize);

        /// <summary>
        /// Gets a hexidecimal string representation of the <see cref="UInt128"/>.
        /// </summary>
        /// <returns>A hexidecimal string representation of the <see cref="UInt128"/>.</returns>
        public override string ToString() => _highBits.ToString("x16") + _lowBits.ToString("x16");

        /// <summary>
        /// Parses a hexidecimal string into a <see cref="UInt128"/> object.
        /// </summary>
        /// <param name="s">Hexidecimal string to parse.</param>
        /// <returns>The parsed <see cref="UInt128"/>.</returns>
        /// <exception cref="FormatException"><paramref name="s"/> contains an invalid hexidecmal pair or does not represent a 128-bit hexidecimal value.</exception>
        public static UInt128 Parse(string s)
        {
            if (s == null || (s = s.TrimStart()).Length == 0)
                return new UInt128();
            int startAt = 0;

            StringBuilder sb = new StringBuilder();
            while (startAt < s.Length)
            {
                Match m = HexPattern.Match(s, startAt);
                if (!m.Success || m.Groups[1].Success)
                {
                    if (sb.Length == 32)
                    {
                        m = HexDigit.Match(s, startAt);
                        if (m.Success && !m.Groups[1].Success)
                            break;
                    }
                    throw new FormatException("Invalid hexidecimal pair at offset " + startAt.ToString());
                }
                if (sb.Length == 32 && m.Groups[2].Index == startAt)
                    throw new FormatException("Too many hexidecimal characters at offset " + startAt.ToString());
                sb.Append(m.Groups[2].Value);
                startAt = m.Index + m.Length;
            }
            if (sb.Length < 32)
                throw new FormatException("Expected 16 hexidecimal pairs; Actual: " + (sb.Length / 2).ToString());

            return new UInt128(BitConverter.GetBytes(long.Parse(sb.ToString(ByteSize, ByteSize), System.Globalization.NumberStyles.HexNumber)).Concat(BitConverter.GetBytes(long.Parse(sb.ToString(0, ByteSize), System.Globalization.NumberStyles.HexNumber))).ToArray());
        }

        public static bool TryCreate(byte[] buffer, out UInt128 value)
        {
            if (buffer is null || buffer.Length != ByteSize)
            {
                value = default;
                return false;
            }
            value = new UInt128(buffer);
            return true;
        }

        /// <summary>
        /// Attempts to parses a hexidecimal string into a <see cref="UInt128"/> object.
        /// </summary>
        /// <param name="s">Hexidecimal string to parse.</param>
        /// <param name="value">The parsed <see cref="UInt128"/>.</param>
        /// <returns><c>true</c> if <paramref name="s"/> could be parsed as a <see cref="UInt128"/>; otherwise, false.</returns>
        public static bool TryParse(string s, out UInt128 value)
        {
            if (s == null || (s = s.TrimStart()).Length == 0)
            {
                value = new UInt128();
                return false;
            }
            int startAt = 0;
            StringBuilder sb = new StringBuilder();
            while (startAt < s.Length)
            {
                Match m = HexPattern.Match(s, startAt);
                if (!m.Success || m.Groups[1].Success)
                {
                    if (sb.Length == 32)
                    {
                        m = HexDigit.Match(s, startAt);
                        if (m.Success && !m.Groups[1].Success)
                            break;
                    }
                    value = new UInt128();
                    return false;
                }
                if (sb.Length == 32 && m.Groups[2].Index == startAt)
                {
                    value = new UInt128();
                    return false;
                }
                sb.Append(m.Groups[2].Value);
                startAt = m.Index + m.Length;
            }
            if (sb.Length < 32)
            {
                value = new UInt128();
                return false;
            }
            value = new UInt128(BitConverter.GetBytes(long.Parse(sb.ToString(ByteSize, ByteSize), System.Globalization.NumberStyles.HexNumber)).Concat(BitConverter.GetBytes(long.Parse(sb.ToString(0, ByteSize), System.Globalization.NumberStyles.HexNumber))).ToArray());
            return true;
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
                return 1;
            if (obj is UInt128 uint128)
                return CompareTo(uint128);
            if (obj is ulong uint64)
                return (_highBits > 0UL) ? 1 : _highBits.CompareTo(uint64);
            if (obj is long int64)
                return (int64 < 0L || _highBits > 0UL) ? 1 : _highBits.CompareTo(int64);
            if (obj is uint uint32)
                return (_highBits > 0UL || _dword1 > 0) ? 1 : _dword0.CompareTo(uint32);
            if (obj is int int32)
                return (int32 < 0L ||  _highBits > 0UL || _dword1 > 0) ? 1 : _dword0.CompareTo(int32);
            if (obj is ushort uint16)
                return (_highBits > 0UL || _dword1 > 0 || _word1 > 0) ? 1 : _word0.CompareTo(uint16);
            if (obj is short int16)
                return (int16 < 0 || _highBits > 0UL || _dword1 > 0 || _word1 > 0) ? 1 : _word0.CompareTo(int16);
            if (obj is byte b)
                return (_highBits > 0UL || _dword1 > 0 || _word1 > 0 || _b1 > 0) ? 1 : _b0.CompareTo(b);
            if (obj is sbyte s)
                return (s < 0 || _highBits > 0UL || _dword1 > 0 || _word1 > 0 || _b1 > 0) ? 1 : _b0.CompareTo(s);
            if (obj is decimal m)
                return (m < 0m || _highBits > MAX_DECIMAL_HIGH_BITS) ? 1 : ToDecimal().CompareTo(m);
            if (obj is float f)
                return (f < 0f || _highBits > MAX_SINGLE_HIGH_BITS) ? 1 : ToSingle().CompareTo(f);
            if (obj is double d)
                return (d < 0.0) ? 1 : ToDouble().CompareTo(d);
            if (obj is IComparable c)
                return 0 - ((_highBits > 0UL) ? c.CompareTo(ToDouble()) : c.CompareTo(_lowBits));
            return -1;
        }

        public int CompareTo(UInt128 other)
        {
            int result = _highBits.CompareTo(other._highBits);
            return (result == 0) ? _lowBits.CompareTo(other._lowBits) : result;
        }

        private decimal ToDecimal() => Convert.ToDecimal(_highBits * FIRST_HIGH_BIT_M) + Convert.ToDecimal(_lowBits);
        private double ToDouble() => Convert.ToDouble(_highBits * FIRST_HIGH_BIT_M) + Convert.ToDouble(_lowBits);
        private float ToSingle() => Convert.ToSingle(_highBits * FIRST_HIGH_BIT_M) + Convert.ToSingle(_lowBits);

        public IEnumerator<byte> GetEnumerator() => GetBytes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetBytes()).GetEnumerator();

        #endregion

        #region Operators

        public static UInt128 operator +(UInt128 left, UInt128 right)
        {
            if ((left._lowBits >> 1) + (right._lowBits >> 1) < 0x8000000000000000)
                return new UInt128(left._lowBits + right._lowBits, left._highBits + right._highBits);
            if (left._lowBits > right._lowBits)
                return new UInt128(right._lowBits - (ulong.MaxValue - left._lowBits + 1), left._highBits + right._highBits + 1UL);
            return new UInt128(left._lowBits - (ulong.MaxValue - right._lowBits + 1), left._highBits + right._highBits + 1UL);
        }
        public static UInt128 operator -(UInt128 left, UInt128 right)
        {
            if (left._lowBits < right._lowBits)
                return new UInt128(ulong.MaxValue - left._lowBits - right._lowBits + 1UL, left._highBits - right._highBits - 1UL);
            return new UInt128(left._lowBits - right._lowBits, left._highBits - right._highBits);
        }
        public static UInt128 operator ~(UInt128 value) => new UInt128(~value._lowBits, ~value._highBits);
        public static UInt128 operator ++(UInt128 value) => (value._lowBits == ulong.MaxValue) ? new UInt128(0UL, value._highBits + 1) : new UInt128(value._lowBits + 1UL, value._highBits);
        public static UInt128 operator --(UInt128 value) => (value._lowBits == 0UL) ? new UInt128(ulong.MaxValue, value._highBits - 1) : new UInt128(value._lowBits - 1UL, value._highBits);
        //public static UInt128 operator *(UInt128 left, UInt128 right) => throw new NotImplementedException();
        //public static UInt128 operator /(UInt128 dividend, UInt128 divisor) => throw new NotImplementedException();
        //public static UInt128 operator %(UInt128 dividend, UInt128 divisor) => throw new NotImplementedException();
        public static UInt128 operator &(UInt128 left, UInt128 right) => new UInt128(left._lowBits & right._lowBits, left._highBits & right._highBits);
        public static UInt128 operator |(UInt128 left, UInt128 right) => new UInt128(left._lowBits | right._lowBits, left._highBits | right._highBits);
        public static UInt128 operator ^(UInt128 left, UInt128 right) => new UInt128(left._lowBits ^ right._lowBits, left._highBits ^ right._highBits);
        public static UInt128 operator <<(UInt128 value, int shift)
        {
            if (shift < 0 || shift > 127)
                shift &= 127;
            switch (shift)
            {
                case 0:
                    return value;
                case 64:
                    return new UInt128(0UL, value._lowBits);
                default:
                    if (shift > 64)
                        return new UInt128(0UL, value._lowBits << (shift - 64));
                    return new UInt128(value._lowBits << shift, (value._highBits << shift) | (value._lowBits >> (64 - shift)));
            }
        }
        public static UInt128 operator >>(UInt128 value, int shift)
        {
            if (shift< 0 || shift> 127)
                shift &= 127;
            switch (shift)
            {
                case 0:
                    return value;
                case 64:
                    return new UInt128(value._highBits, 0UL);
                default:
                    if (shift > 64)
                        return new UInt128(value._highBits >> (shift - 64), 0UL);
                    return new UInt128((value._highBits << (64 - shift)) | (value._lowBits >> shift), value._highBits >> shift);
            }
        }
        public static bool operator ==(UInt128 left, UInt128 right) => left._highBits == right._highBits && left._lowBits == right._lowBits;
        public static bool operator ==(UInt128 left, long right) => right >= 0 && left._highBits == 0UL && left._lowBits == (ulong)right;
        public static bool operator ==(long left, UInt128 right) => left >= 0 && right._highBits == 0UL && (ulong)left == right._lowBits;
        public static bool operator ==(ulong left, UInt128 right) => right._highBits == 0UL && right._lowBits == left;
        public static bool operator ==(UInt128 left, ulong right) => left._highBits == 0UL && right == left._lowBits;
        public static bool operator !=(UInt128 left, UInt128 right) => left._highBits != right._highBits || left._lowBits != right._lowBits;
        public static bool operator !=(UInt128 left, long right) => right < 0L || left._highBits != 0UL || left._lowBits != (ulong)right;
        public static bool operator !=(UInt128 left, ulong right) => left._highBits != 0UL || right != left._lowBits;
        public static bool operator !=(long left, UInt128 right) => left < 0L || right._highBits != 0UL || (ulong)left < right._lowBits;
        public static bool operator !=(ulong left, UInt128 right) => right._highBits != 0UL || right._lowBits != left;
        public static bool operator <(UInt128 left, ulong right) => left._highBits == 0UL && left._lowBits < right;
        public static bool operator <(ulong left, UInt128 right) => right._highBits != 0UL || left < right._lowBits;
        public static bool operator <(UInt128 left, long right) => right > 0L && left._highBits == 0UL && left._lowBits < (ulong)right;
        public static bool operator <(UInt128 left, UInt128 right) => left._highBits < right._highBits || (left._highBits == right._highBits && left._lowBits < right._lowBits);
        public static bool operator <(long left, UInt128 right) => left < 0 || right._highBits != 0UL || (ulong)left < right._lowBits;
        public static bool operator >(long left, UInt128 right) => left > 0L && right._highBits == 0UL && (ulong)left > right._lowBits;
        public static bool operator >(UInt128 left, long right) => right <= 0L || left._highBits != 0UL || left._lowBits > (ulong)right;
        public static bool operator >(UInt128 left, ulong right) => left._highBits != 0UL || left._lowBits > right;
        public static bool operator >(UInt128 left, UInt128 right) => left._highBits > right._highBits || (left._highBits == right._highBits && left._lowBits > right._lowBits);
        public static bool operator >(ulong left, UInt128 right) => right._highBits == 0UL && left > right._lowBits;
        public static bool operator <=(UInt128 left, UInt128 right) => left._highBits < right._highBits || (left._highBits == right._highBits && left._lowBits <= right._lowBits);
        public static bool operator <=(UInt128 left, long right) => right >= 0 && left._highBits == 0UL && left._lowBits <= (ulong)right;
        public static bool operator <=(UInt128 left, ulong right) => left._highBits == 0UL && left._lowBits <= right;
        public static bool operator <=(ulong left, UInt128 right) => right._highBits == 0UL || left < right._lowBits; 
        public static bool operator <=(long left, UInt128 right) => left < 0 || right._highBits != 0UL || (ulong)left <= right._lowBits;
        public static bool operator >=(UInt128 left, ulong right) => left._highBits != 0UL || left._lowBits >= right;
        public static bool operator >=(long left, UInt128 right) => left >= 0 && right._highBits == 0UL && (ulong)left >= right._lowBits;
        public static bool operator >=(UInt128 left, UInt128 right) => left._highBits > right._highBits || (left._highBits == right._highBits && left._lowBits >= right._lowBits);
        public static bool operator >=(UInt128 left, long right) => right >= 0 && (left._highBits != 0UL || left._lowBits >= (ulong)right);
        public static bool operator >=(ulong left, UInt128 right) => right._highBits == 0UL && left >= right._lowBits;
        public static implicit operator UInt128(ushort value) => new UInt128(value);
        public static implicit operator UInt128(short value) => new UInt128(value);
        public static implicit operator UInt128(sbyte value) => new UInt128(value);
        public static implicit operator UInt128(ulong value) => new UInt128(value);
        public static implicit operator UInt128(byte value) => new UInt128(value);
        public static implicit operator UInt128(long value) => new UInt128(value);
        public static implicit operator UInt128(uint value) => new UInt128(value);
        public static implicit operator UInt128(int value) => new UInt128(value);
        public static explicit operator ulong(UInt128 value) => (value._highBits == 0UL) ? value._lowBits : throw new OverflowException();
        public static explicit operator UInt128(float value) => new UInt128(value);
        public static explicit operator UInt128(double value) => new UInt128(value);
        public static explicit operator UInt128(decimal value) => new UInt128(value);
        public static explicit operator byte(UInt128 value) => (value._highBits == 0UL && value._dword1 == 0 && value._word1 == 0 && value._b1 == 0) ? value._b0 : throw new OverflowException();
        public static explicit operator sbyte(UInt128 value) => (value._highBits == 0UL && value._dword1 == 0 && value._word1 == 0 && value._b1 == 0 && value._b0 <= sbyte.MaxValue) ? (sbyte)value._b0 : throw new OverflowException();
        public static explicit operator short(UInt128 value) => (value._highBits == 0UL && value._dword1 == 0 && value._word1 == 0 && value._word0 <= short.MaxValue) ? (short)value._word0 : throw new OverflowException();
        public static explicit operator int(UInt128 value) => (value._highBits == 0UL && value._dword1 == 0 && value._dword0 <= int.MaxValue) ? (int)value._dword0 : throw new OverflowException();
        public static explicit operator uint(UInt128 value) => (value._highBits == 0UL && value._dword1 == 0) ? value._dword0 : throw new OverflowException();
        public static explicit operator decimal(UInt128 value) => value.ToDecimal();
        public static explicit operator double(UInt128 value) => value.ToDouble();
        public static explicit operator float(UInt128 value) => value.ToSingle();
        public static explicit operator long(UInt128 value) => (value._highBits == 0UL && value._lowBits <= long.MaxValue) ? (long)value._lowBits : throw new OverflowException();
        public static explicit operator ushort(UInt128 value) => (value._highBits == 0UL && value._dword1 == 0 && value._word1 == 0) ? value._word0 : throw new OverflowException();

        #endregion
    }
}
