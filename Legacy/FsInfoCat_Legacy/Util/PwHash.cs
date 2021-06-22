using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PwHash : IEquatable<PwHash>, IEquatable<string>
    {
        private const int FIELD_OFFSET_1 = 8;
        private const int FIELD_OFFSET_2 = 16;
        private const int FIELD_OFFSET_3 = 24;
        private const int FIELD_OFFSET_4 = 32;
        private const int FIELD_OFFSET_5 = 40;
        private const int FIELD_OFFSET_6 = 48;
        private const int FIELD_OFFSET_7 = 56;
        public const int HASH_BYTES_LENGTH = 64;
        public const int SALT_BYTES_LENGTH = 8;
        public const int TOTAL_BYTES_LENGTH = HASH_BYTES_LENGTH + SALT_BYTES_LENGTH;
        public const string PASSWORD_VALIDATION_PATTERN = @"^(?=(.*[!@#$%^&*()_+-=\[\]\\;',./_+{}|:""<>?]){2})(?=(.*[a-z]){2})(?=(.*[A-Z]){2})(?=(.*\d){2}).{10}";
        public const string PASSWORD_VALIDATION_MESSAGE = "Password must be at least 10 characters long, contain at least 2 symbols, 2 upper case letters, 2 lower case letters and 2 numbers!";
        public static readonly Regex PasswordValidationRegex = new Regex(PASSWORD_VALIDATION_PATTERN, RegexOptions.Compiled);

        [FieldOffset(0)]
        private readonly ulong _hashBits000_03f;

        [FieldOffset(FIELD_OFFSET_1)]
        private readonly ulong _hashBits040_07f;

        [FieldOffset(FIELD_OFFSET_2)]
        private readonly ulong _hashBits080_0bf;

        [FieldOffset(FIELD_OFFSET_3)]
        private readonly ulong _hashBits0c0_0ff;

        [FieldOffset(FIELD_OFFSET_4)]
        private readonly ulong _hashBits100_13f;

        [FieldOffset(FIELD_OFFSET_5)]
        private readonly ulong _hashBits140_17f;

        [FieldOffset(FIELD_OFFSET_6)]
        private readonly ulong _hashBits180_1bf;

        [FieldOffset(FIELD_OFFSET_7)]
        private readonly ulong _hashBits1c0_1ff;

        [FieldOffset(HASH_BYTES_LENGTH)]
        private readonly ulong _saltBits;

        public ulong HashBits000_03f => _hashBits000_03f;
        public ulong HashBits040_07f => _hashBits040_07f;
        public ulong HashBits080_0bf => _hashBits080_0bf;
        public ulong HashBits0c0_0ff => _hashBits0c0_0ff;
        public ulong HashBits100_13f => _hashBits100_13f;
        public ulong HashBits140_17f => _hashBits140_17f;
        public ulong HashBits180_1bf => _hashBits180_1bf;
        public ulong HashBits1c0_1ff => _hashBits1c0_1ff;
        public ulong SaltBits => _saltBits;

        public PwHash(ulong hashBits000_03f, ulong hashBits040_07f, ulong hashBits080_0bf, ulong hashBits0c0_0ff, ulong hashBits100_13f, ulong hashBits140_17f, ulong hashBits180_1bf,
            ulong hashBits1c0_1ff, ulong saltBits)
        {
            _hashBits000_03f = hashBits000_03f;
            _hashBits040_07f = hashBits040_07f;
            _hashBits080_0bf = hashBits080_0bf;
            _hashBits0c0_0ff = hashBits0c0_0ff;
            _hashBits100_13f = hashBits100_13f;
            _hashBits140_17f = hashBits140_17f;
            _hashBits180_1bf = hashBits180_1bf;
            _hashBits1c0_1ff = hashBits1c0_1ff;
            _saltBits = saltBits;
        }

        public PwHash(IEnumerable<byte> sha512HashBytes, int hashStartIndex, IEnumerable<byte> saltBytes, int saltStartIndex)
        {
            if (sha512HashBytes is null)
                throw new ArgumentNullException(nameof(sha512HashBytes));
            if (saltBytes is null)
                throw new ArgumentNullException(nameof(saltBytes));
            if (hashStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(hashStartIndex));
            if (saltStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(saltStartIndex));
            if (!(sha512HashBytes is byte[] hash))
            {
                hash = sha512HashBytes.Skip(hashStartIndex).Take(HASH_BYTES_LENGTH).ToArray();
                hashStartIndex = 0;
            }
            if (hash.Length < (HASH_BYTES_LENGTH + hashStartIndex))
                throw new ArgumentException("SHA512 hash array must contain at least" + HASH_BYTES_LENGTH.ToString() + " bytes after the start index.", nameof(sha512HashBytes));
            if (!(saltBytes is byte[] salt))
            {
                salt = saltBytes.Skip(saltStartIndex).Take(SALT_BYTES_LENGTH).ToArray();
                saltStartIndex = 0;
            }
            if (salt.Length < (SALT_BYTES_LENGTH + saltStartIndex))
                throw new ArgumentException("Salt array must contain at least" + SALT_BYTES_LENGTH.ToString() + " bytes after the start index.", nameof(saltBytes));
            _hashBits000_03f = BitConverter.ToUInt64(hash, hashStartIndex);
            _hashBits040_07f = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_1);
            _hashBits080_0bf = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_2);
            _hashBits0c0_0ff = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_3);
            _hashBits100_13f = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_4);
            _hashBits140_17f = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_5);
            _hashBits180_1bf = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_6);
            _hashBits1c0_1ff = BitConverter.ToUInt64(hash, hashStartIndex + FIELD_OFFSET_7);
            _saltBits = BitConverter.ToUInt64(salt, saltStartIndex);
        }

        public PwHash(IEnumerable<byte> hashAndSaltBytes, int startIndex)
        {
            if (hashAndSaltBytes is null)
                throw new ArgumentNullException(nameof(hashAndSaltBytes));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (!(hashAndSaltBytes is byte[] data))
            {
                data = hashAndSaltBytes.Skip(startIndex).Take(TOTAL_BYTES_LENGTH).ToArray();
                startIndex = 0;
            }
            if (data.Length < (TOTAL_BYTES_LENGTH + startIndex))
                throw new ArgumentException("Array must contain at least" + TOTAL_BYTES_LENGTH.ToString() + " bytes after the start index.", nameof(hashAndSaltBytes));
            _hashBits000_03f = BitConverter.ToUInt64(data, startIndex);
            _hashBits040_07f = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_1);
            _hashBits080_0bf = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_2);
            _hashBits0c0_0ff = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_3);
            _hashBits100_13f = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_4);
            _hashBits140_17f = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_5);
            _hashBits180_1bf = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_6);
            _hashBits1c0_1ff = BitConverter.ToUInt64(data, startIndex + FIELD_OFFSET_7);
            _saltBits = BitConverter.ToUInt64(data, startIndex + HASH_BYTES_LENGTH);
        }

        private PwHash(byte[] sha512Hash, byte[] saltBytes)
        {
            _hashBits000_03f = BitConverter.ToUInt64(sha512Hash, 0);
            _hashBits040_07f = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_1);
            _hashBits080_0bf = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_2);
            _hashBits0c0_0ff = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_3);
            _hashBits100_13f = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_4);
            _hashBits140_17f = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_5);
            _hashBits180_1bf = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_6);
            _hashBits1c0_1ff = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_7);
            _saltBits = BitConverter.ToUInt64(saltBytes, 0);
        }

        private PwHash(byte[] hashAndSaltBytes)
        {
            _hashBits000_03f = BitConverter.ToUInt64(hashAndSaltBytes, 0);
            _hashBits040_07f = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_1);
            _hashBits080_0bf = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_2);
            _hashBits0c0_0ff = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_3);
            _hashBits100_13f = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_4);
            _hashBits140_17f = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_5);
            _hashBits180_1bf = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_6);
            _hashBits1c0_1ff = BitConverter.ToUInt64(hashAndSaltBytes, FIELD_OFFSET_7);
            _saltBits = BitConverter.ToUInt64(hashAndSaltBytes, HASH_BYTES_LENGTH);
        }

        public IEnumerable<byte> GetHashAndSaltBytes() => (new ulong[]
        {
            _hashBits000_03f, _hashBits040_07f, _hashBits080_0bf, _hashBits0c0_0ff,
            _hashBits100_13f, _hashBits140_17f, _hashBits180_1bf, _hashBits1c0_1ff,
            _saltBits
        }).SelectMany(v => BitConverter.GetBytes(v));

        public IEnumerable<byte> GetSha512HashBytes() => (new ulong[]
        {
            _hashBits000_03f, _hashBits040_07f, _hashBits080_0bf, _hashBits0c0_0ff,
            _hashBits100_13f, _hashBits140_17f, _hashBits180_1bf, _hashBits1c0_1ff
        }).SelectMany(v => BitConverter.GetBytes(v));

        public byte[] GetSaltBytes() => BitConverter.GetBytes(_saltBits);

        private static byte[] ComputeHash(string rawPw, byte[] salt)
        {
            using (SHA512 sha = SHA512.Create())
            {
                sha.ComputeHash(Encoding.ASCII.GetBytes(rawPw).Concat(salt).ToArray());
                return sha.Hash;
            }
        }

        public static PwHash? Import(string base64EncodedHash)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedHash))
                return null;
            byte[] data;
            try { data = Convert.FromBase64String(base64EncodedHash); }
            catch (Exception exc) { throw new ArgumentException("Invalid base-64 string", nameof(base64EncodedHash), exc); }
            if (data.Length != TOTAL_BYTES_LENGTH)
                throw new ArgumentException("Invalid data length", nameof(base64EncodedHash));
            return new PwHash(data);
        }

        public static PwHash? Create(string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return null;
            byte[] salt = new byte[SALT_BYTES_LENGTH];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(salt);
            return new PwHash(ComputeHash(rawPw, salt), salt);
        }

        public static PwHash? Create(string rawPw, ulong saltBits)
        {
            if (string.IsNullOrEmpty(rawPw))
                return null;
            byte[] saltBytes = BitConverter.GetBytes(saltBits);
            return new PwHash(ComputeHash(rawPw, saltBytes), saltBytes);
        }

        public static bool Test(PwHash? hash, string rawPw)
        {
            return (hash.HasValue) ? hash.Value.Test(rawPw) : string.IsNullOrEmpty(rawPw);
        }

        public static bool Test(string base64EncodedHash, string rawPw)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedHash))
                return string.IsNullOrEmpty(rawPw);
            byte[] data;
            try { data = Convert.FromBase64String(base64EncodedHash); }
            catch (Exception exc) { throw new ArgumentException("Invalid base-64 string", nameof(base64EncodedHash), exc); }
            if (data.Length != TOTAL_BYTES_LENGTH)
                throw new ArgumentException("Invalid data length", nameof(base64EncodedHash));
            if (string.IsNullOrEmpty(rawPw))
                return false;
            byte[] salt = new byte[SALT_BYTES_LENGTH];
            Array.Copy(data, salt, SALT_BYTES_LENGTH);
            byte[] hash = ComputeHash(rawPw, salt);
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != data[i])
                    return false;
            }
            return true;
        }

        public bool Test(string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return false;
            byte[] hash = ComputeHash(rawPw, GetSaltBytes());
            return BitConverter.ToUInt64(hash, 0) == _hashBits000_03f &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_1) == _hashBits040_07f && BitConverter.ToUInt64(hash, FIELD_OFFSET_2) == _hashBits080_0bf &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_3) == _hashBits0c0_0ff && BitConverter.ToUInt64(hash, FIELD_OFFSET_4) == _hashBits100_13f &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_5) == _hashBits140_17f && BitConverter.ToUInt64(hash, FIELD_OFFSET_6) == _hashBits180_1bf &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_7) == _hashBits1c0_1ff;
        }

        public override string ToString()
        {
            return Convert.ToBase64String(GetHashAndSaltBytes().ToArray());
        }

        public bool Equals(PwHash other)
        {
            return _hashBits000_03f == other._hashBits000_03f && _hashBits040_07f == other._hashBits040_07f && _hashBits080_0bf == other._hashBits080_0bf &&
                _hashBits0c0_0ff == other._hashBits0c0_0ff && _hashBits100_13f == other._hashBits100_13f && _hashBits140_17f == other._hashBits140_17f &&
                _hashBits180_1bf == other._hashBits180_1bf && _hashBits1c0_1ff == other._hashBits1c0_1ff && _saltBits == other._saltBits;
        }

        public bool Equals(string other)
        {
            if (string.IsNullOrWhiteSpace(other))
                return false;
            byte[] data;
            try
            {
                if ((data = Convert.FromBase64String(other)) is null || data.Length != TOTAL_BYTES_LENGTH)
                    return false;
            }
            catch { return false; }
            return !(data is null) && data.Length == TOTAL_BYTES_LENGTH && BitConverter.ToUInt64(data, 0) == _hashBits000_03f &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_1) == _hashBits040_07f && BitConverter.ToUInt64(data, FIELD_OFFSET_2) == _hashBits080_0bf &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_3) == _hashBits0c0_0ff && BitConverter.ToUInt64(data, FIELD_OFFSET_4) == _hashBits100_13f &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_5) == _hashBits140_17f && BitConverter.ToUInt64(data, FIELD_OFFSET_6) == _hashBits180_1bf &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_7) == _hashBits1c0_1ff && BitConverter.ToUInt64(data, HASH_BYTES_LENGTH) == _saltBits;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is PwHash hash)
                return Equals(hash);
            return obj is string s && Equals(s);
        }

        public override int GetHashCode()
        {
            if ((_saltBits & 1UL) == 1UL)
                return (_hashBits000_03f.GetHashCode() & 0x0000000f) |
                    (_hashBits040_07f.GetHashCode() & 0x000000f0) |
                    (_hashBits080_0bf.GetHashCode() & 0x00000700) |
                    (_hashBits0c0_0ff.GetHashCode() & 0x00007800) |
                    (_hashBits100_13f.GetHashCode() & 0x00038000) |
                    (_hashBits140_17f.GetHashCode() & 0x003c0000) |
                    (_hashBits180_1bf.GetHashCode() & 0x01c00000) |
                    (_hashBits1c0_1ff.GetHashCode() & 0x1e000000) |
                    (int)(_saltBits.GetHashCode() & 0xe0000000);
            return (_hashBits000_03f.GetHashCode() & 0x0000000f) |
                (_hashBits040_07f.GetHashCode() & 0x00000070) |
                (_hashBits080_0bf.GetHashCode() & 0x00000780) |
                (_hashBits0c0_0ff.GetHashCode() & 0x00003800) |
                (_hashBits100_13f.GetHashCode() & 0x0003c000) |
                (_hashBits140_17f.GetHashCode() & 0x001c0000) |
                (_hashBits180_1bf.GetHashCode() & 0x01e00000) |
                (_hashBits1c0_1ff.GetHashCode() & 0x1e000000) |
                (int)(_saltBits.GetHashCode() & 0xe0000000);
        }

        public static bool operator ==(PwHash left, PwHash right) => left.Equals(right);

        public static bool operator !=(PwHash left, PwHash right) => !left.Equals(right);
    }
}
