using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a 128-bit MD5 checksumm value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    [Serializable]
    public struct MD5Hash : IEquatable<MD5Hash>, IConvertible
    {
        #region Fields

        public const int StringLength_BinHex = 32;
        public const int StringLength_Base64 = 24;
        public const int StringLength_Serialized = 22;
        public const int MD5ByteSize = 16;


        public static readonly Regex Base64MD5Regex = new(@"^\s*((?:[a-z\d+/]\s*){4}){4}(?:[a-z\d+/]\s*){6}(=\s*=\s*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //public static readonly Regex Base64SequenceRegex = new(@"^\s*(([a-z\d+/]\s*){4})*((?<c>[a-z\d+/\s]+(==?)?)|(?<e>[^a-z\d+/=]))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //public static readonly Regex BinHexSequenceRegex = new(@"^\s*([a-f\d\s]+$|[a-f\d\s]*(?<e>[^a-f\d\s]))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //public static readonly Regex BraceUuidRegex = new(@"^\s*(\{[a-f\d]{8}(-[a-f\d]{4}){4}[a-f\d]{8}\}\s*$|(?<e>\{[a-f\d]{8}-([a-f\d]{4}-([a-f\d]{4}-([a-f\d]{4}-([a-f\d]{12}\}?|[a-f\d]{0,12})\s*|[a-f\d]{0,3})|[a-f\d]{0,3})|[a-f\d]{0,3})|[a-f\d]{0,7}))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //public static readonly Regex DashUuidRegex = new(@"^\s*([a-f\d]{8}(-[a-f\d]{4}){4}[a-f\d]{8}\s*$|(?<e>[a-f\d]{8}-([a-f\d]{4}-([a-f\d]{4}-([a-f\d]{4}-([a-f\d]{12}|[a-f\d]{0,12})\s*|[a-f\d]{0,3})|[a-f\d]{0,3})|[a-f\d]{0,3})|[a-f\d]{0,7}))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //public static readonly Regex WsRegex = new(@"[\s\r\n]+", RegexOptions.Compiled);

        public static readonly ValueConverter<MD5Hash, byte[]> Converter = new(
            v => v.GetBuffer(),
            b => new MD5Hash(b)
        );

        private static readonly Regex HexPattern = new(@"\s*([^a-fA-F\d\s]+)?([a-fA-F\d][a-fA-F\d])\s*([\-:,;]\s*)?", RegexOptions.Compiled);

        private static readonly Regex HexDigit = new(@"(?:[\s\-:,;]|([a-fA-F\d])|\s*$)", RegexOptions.Compiled);

        [FieldOffset(0)]
        private readonly long _lowBits;

        [FieldOffset(0)]
        private readonly int _key0;

        [FieldOffset(0)]
        private readonly byte _b0;

        [FieldOffset(1)]
        private readonly byte _b1;

        [FieldOffset(2)]
        private readonly byte _b2;

        [FieldOffset(3)]
        private readonly byte _b3;

        [FieldOffset(4)]
        private readonly int _key1;

        [FieldOffset(4)]
        private readonly byte _b4;

        [FieldOffset(5)]
        private readonly byte _b5;

        [FieldOffset(6)]
        private readonly byte _b6;

        [FieldOffset(7)]
        private readonly byte _b7;

        [FieldOffset(8)]
        private readonly long _highBits;

        [FieldOffset(8)]
        private readonly int _key2;

        [FieldOffset(8)]
        private readonly byte _b8;

        [FieldOffset(9)]
        private readonly byte _b9;

        [FieldOffset(10)]
        private readonly byte _b10;

        [FieldOffset(11)]
        private readonly byte _b11;

        [FieldOffset(12)]
        private readonly int _key3;

        [FieldOffset(12)]
        private readonly byte _b12;

        [FieldOffset(13)]
        private readonly byte _b13;

        [FieldOffset(14)]
        private readonly byte _b14;

        [FieldOffset(15)]
        private readonly byte _b15;

        #endregion

        #region Properties

        /// <summary>
        /// Contains the lower 64 bits of the MD5 checksum value.
        /// </summary>
        public long LowBits => _lowBits;

        /// <summary>
        /// Contains the lower 64 bits of the MD5 checksum value.
        /// </summary>
        public int Key0 => _key0;

        /// <summary>
        /// Contains the lower 64 bits of the MD5 checksum value.
        /// </summary>
        public int Key1 => _key1;

        /// <summary>
        /// Contains the upper 64 bits of the MD5 checksum value.
        /// </summary>
        public long HighBits => _highBits;

        /// <summary>
        /// Contains the upper 64 bits of the MD5 checksum value.
        /// </summary>
        public int Key2 => _key2;

        /// <summary>
        /// Contains the upper 64 bits of the MD5 checksum value.
        /// </summary>
        public int Key3 => _key3;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="MD5Hash"/>.
        /// </summary>
        /// <param name="buffer">A <seealso cref="Byte"/> array representing the 128-bit MD5 checksum. A null value or empty array initializes a zero checksum value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="buffer"/> was not null, empty or 16 bytes in length.</exception>
        public MD5Hash(byte[] buffer)
        {
            _highBits = _lowBits = 0L;
            _key0 = _key1 = _key2 = _key3 = 0;
            if (buffer == null || buffer.Length == 0)
                _b0 = _b1 = _b2 = _b3 = _b4 = _b5 = _b6 = _b7 = _b8 = _b9 = _b10 = _b11 = _b12 = _b13 = _b14 = _b15 = (byte)0;
            else
            {
                if (buffer.Length != MD5ByteSize)
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

        /// <summary>
        /// Gets the 16-byte array representing the MD5 checksum.
        /// </summary>
        /// <returns>A 16-byte array representing the MD5 checksum.</returns>
        public byte[] GetBuffer() { return new byte[] { _b0, _b1, _b2, _b3, _b4, _b5, _b6, _b7, _b8, _b9, _b10, _b11, _b12, _b13, _b14, _b15 }; }

        #region Equals

        /// <summary>
        /// Determines whether the current <see cref="MD5Hash"/> is equal to another.
        /// </summary>
        /// <param name="other"><see cref="MD5Hash"/> to compare to.</param>
        /// <returns><c>true</c> if the current <see cref="MD5Hash"/> is equal to <paramref name="other"/>; othwerise, <c>false</c>.</returns>
        public bool Equals(MD5Hash other) => _highBits == other._highBits && _lowBits == other._lowBits;

        /// <summary>
        /// Determines whether the current <see cref="MD5Hash"/> is equal to another object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns><c>true</c> if <paramref name="other"/> is an <see cref="MD5Hash"/> and is equal to the current <see cref="MD5Hash"/>; othwerise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj is MD5Hash hash && Equals(hash);

        #endregion

        /// <summary>
        /// Gets the hashcode for the current <see cref="MD5Hash"/>.
        /// </summary>
        /// <returns>The hashcode for the current <see cref="MD5Hash"/>.</returns>
        public override int GetHashCode() => (int)(_lowBits & 0xFFFF) | ((int)(_highBits & 0xFFFF) << MD5ByteSize);

        /// <summary>
        /// Gets a hexidecimal string representation of the <see cref="MD5Hash"/>.
        /// </summary>
        /// <returns>A hexidecimal string representation of the <see cref="MD5Hash"/>.</returns>
        public override string ToString() => Convert.ToBase64String(GetBuffer()).Substring(0, StringLength_Serialized);

        /// <summary>
        /// Parses a hexidecimal string into a <see cref="MD5Hash"/> object.
        /// </summary>
        /// <param name="s">Hexidecimal string to parse.</param>
        /// <returns>The parsed <see cref="MD5Hash"/>.</returns>
        /// <exception cref="FormatException"><paramref name="s"/> contains an invalid hexidecmal pair or does not represent a 128-bit hexidecimal value.</exception>
        public static MD5Hash Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new MD5Hash();
            Match match = Base64MD5Regex.Match(s);
            if (match.Success)
                return new MD5Hash(Convert.FromBase64String($"{match.Groups[1].Value}=="));
            byte[] buffer = ByteArrayCoersion.Parse(s).ToArray();
            if (buffer.Length < MD5ByteSize)
                throw new ArgumentOutOfRangeException(nameof(s), "Decoded byte length too short.");
            if (buffer.Length > MD5ByteSize)
                throw new ArgumentOutOfRangeException(nameof(s), "Decoded byte length too long.");
            return new MD5Hash(buffer);
        }

        /// <summary>
        /// Attempts to parses a hexidecimal string into a <see cref="MD5Hash"/> object.
        /// </summary>
        /// <param name="s">Hexidecimal string to parse.</param>
        /// <param name="result">The parsed <see cref="MD5Hash"/>.</param>
        /// <returns><c>true</c> if <paramref name="s"/> could be parsed as a <see cref="MD5Hash"/>; otherwise, false.</returns>
        public static bool TryParse(string s, out MD5Hash result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = new MD5Hash();
                return true;
            }

            Match match = Base64MD5Regex.Match(s);
            if (match.Success)
            {
                result = new MD5Hash(Convert.FromBase64String($"{match.Groups[1].Value}=="));
                return true;
            }
            else if (ByteArrayCoersion.TryParse(s, out IEnumerable<byte> bytes))
            {
                byte[] buffer = bytes.ToArray();
                if (buffer.Length == MD5ByteSize)
                {
                    result = new MD5Hash(buffer);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static async Task<MD5Hash> CreateAsync(Stream stream, CancellationToken cancellationToken)
        {
            //System.Security.Cryptography.MD5
            using MD5 md5 = MD5.Create();
            return new MD5Hash(await md5.ComputeHashAsync(stream, cancellationToken));
        }

        #endregion

        TypeCode IConvertible.GetTypeCode() => TypeCode.String;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(ToString(), provider);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(ToString(), provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToString(), provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(ToString(), provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(ToString(), provider);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(ToString(), provider);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(ToString(), provider);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(ToString(), provider);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(ToString(), provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(ToString(), provider);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(ToString(), provider);
        string IConvertible.ToString(IFormatProvider provider) => ToString();
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType is null)
                throw new ArgumentNullException(nameof(conversionType));
            if (conversionType.Equals(typeof(MD5Hash)))
                return this;
            if (conversionType.Equals(typeof(byte[])) || conversionType.Equals(typeof(IList<byte>)) || conversionType.Equals(typeof(ICollection<byte>)) ||
                conversionType.Equals(typeof(IEnumerable<byte>)))
                return GetBuffer();
            return Convert.ChangeType(ToString(), conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(ToString(), provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(ToString(), provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(ToString(), provider);

        public static bool operator ==(MD5Hash left, MD5Hash right) => left.Equals(right);

        public static bool operator !=(MD5Hash left, MD5Hash right) => !(left == right);

        public static implicit operator MD5Hash(string text) => Parse(text);

        public static implicit operator string(MD5Hash hash) => hash.ToString();

        public static implicit operator MD5Hash(byte[] buffer) => new(buffer);

        public static implicit operator byte[](MD5Hash hash) => hash.GetBuffer();
    }
}
