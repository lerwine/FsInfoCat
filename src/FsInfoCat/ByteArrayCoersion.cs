using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    public class ByteArrayCoersion : EnumerableCoersion<byte, byte[]>
    {
        public static readonly ByteArrayCoersion Default = new();

        //public static readonly Regex BinHexSequenceRegex = new(@"^\s*([a-f\d]\s*[a-f\d]\s*)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //public static readonly Regex WsRegex = new(@"[\s\r\n]+", RegexOptions.Compiled);

        public ByteArrayCoersion() : base() { }

        public static IEnumerable<byte> Parse(IEnumerable<char> b64BinHexOrUuid)
        {
            if (b64BinHexOrUuid is null)
                return null;

            using var enumerator = b64BinHexOrUuid.GetEnumerator();
            bool hv = true;
            int offset = -1;
            LinkedEnumerableBuilder<byte> enumerable = new();
            byte value = 0;
            int wsIndex = -1;
            while (enumerator.MoveNext())
            {
                offset++;
                char c = enumerator.Current;
                if (hv)
                    switch (c)
                    {
                        case '0':
                            value = 0x00;
                            break;
                        case '1':
                            value = 0x10;
                            break;
                        case '2':
                            value = 0x20;
                            break;
                        case '3':
                            value = 0x30;
                            break;
                        case '4':
                            value = 0x40;
                            break;
                        case '5':
                            value = 0x50;
                            break;
                        case '6':
                            value = 0x60;
                            break;
                        case '7':
                            value = 0x70;
                            break;
                        case '8':
                            value = 0x80;
                            break;
                        case '9':
                            value = 0x90;
                            break;
                        case 'A':
                        case 'a':
                            value = 0xa0;
                            break;
                        case 'B':
                        case 'b':
                            value = 0xb0;
                            break;
                        case 'C':
                        case 'c':
                            value = 0xc0;
                            break;
                        case 'D':
                        case 'd':
                            value = 0xd0;
                            break;
                        case 'E':
                        case 'e':
                            value = 0xe0;
                            break;
                        case 'F':
                        case 'f':
                            value = 0xf0;
                            break;
                        case '+':
                        case '/':
                        case '=':
                            if (b64BinHexOrUuid is string s)
                                return Convert.FromBase64String((offset > 0) ? s[offset..] : s);
                            if (b64BinHexOrUuid is char[] arr)
                            {
                                if (offset > 0)
                                    arr = arr[offset..];
                            }
                            else
                                arr = ((offset > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray();
                            return Convert.FromBase64CharArray(arr, 0, arr.Length);
                        case '{':
                            if (enumerable.Count == 0)
                            {
                                if (b64BinHexOrUuid is string uuid)
                                    return Guid.Parse((offset > 0) ? uuid[offset..] : uuid).ToByteArray();
                                return Guid.Parse(((offset > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray()).ToByteArray();
                            }
                            throw new FormatException($"Invalid base64/BinHex character at offset {offset}.");
                        case '-':
                            if (enumerable.Count == 4)
                            {
                                int index = offset - 8;
                                if (index > wsIndex)
                                {
                                    if (b64BinHexOrUuid is string uuid)
                                        return Guid.Parse((index > 0) ? uuid[offset..] : uuid).ToByteArray();
                                    return Guid.Parse(((index > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray()).ToByteArray();
                                }
                            }
                            throw new FormatException($"Invalid position for UUID character ({offset}).");
                        default:
                            if (!(char.IsWhiteSpace(enumerator.Current) || char.IsControl(enumerator.Current)))
                                throw new FormatException($"Invalid base64/BinHex/UUID character at offset {offset}.");
                            wsIndex = offset;
                            break;
                    }
                else
                    switch (c)
                    {
                        case '0':
                            enumerable.Add(value);
                            break;
                        case '1':
                            enumerable.Add((byte)(value | 0x01));
                            break;
                        case '2':
                            enumerable.Add((byte)(value | 0x02));
                            break;
                        case '3':
                            enumerable.Add((byte)(value | 0x03));
                            break;
                        case '4':
                            enumerable.Add((byte)(value | 0x04));
                            break;
                        case '5':
                            enumerable.Add((byte)(value | 0x05));
                            break;
                        case '6':
                            enumerable.Add((byte)(value | 0x06));
                            break;
                        case '7':
                            enumerable.Add((byte)(value | 0x07));
                            break;
                        case '8':
                            enumerable.Add((byte)(value | 0x08));
                            break;
                        case '9':
                            enumerable.Add((byte)(value | 0x09));
                            break;
                        case 'A':
                        case 'a':
                            enumerable.Add((byte)(value | 0x0a));
                            break;
                        case 'B':
                        case 'b':
                            enumerable.Add((byte)(value | 0x0b));
                            break;
                        case 'C':
                        case 'c':
                            enumerable.Add((byte)(value | 0x0c));
                            break;
                        case 'D':
                        case 'd':
                            enumerable.Add((byte)(value | 0x0d));
                            break;
                        case 'E':
                        case 'e':
                            enumerable.Add((byte)(value | 0x0e));
                            break;
                        case 'F':
                        case 'f':
                            enumerable.Add((byte)(value | 0x0f));
                            break;
                        case '+':
                        case '/':
                        case '=':
                            if (b64BinHexOrUuid is string s)
                                return Convert.FromBase64String(s);
                            if (b64BinHexOrUuid is not char[] arr)
                                arr = b64BinHexOrUuid.ToArray();
                            return Convert.FromBase64CharArray(arr, 0, arr.Length);
                        default:
                            if (!(char.IsWhiteSpace(enumerator.Current) || char.IsControl(enumerator.Current)))
                                throw new FormatException($"Invalid base64/BinHex character at position {offset}.");
                            break;
                    }
                hv = !hv;
            }
            if (!hv)
                throw new FormatException("Uneven number of hexidecimal characters.");
            return enumerable.Build();
        }

        public static bool TryParse(IEnumerable<char> b64BinHexOrUuid, out IEnumerable<byte> result)
        {
            if (b64BinHexOrUuid is null)
            {
                result = null;
                return true;
            }

            using var enumerator = b64BinHexOrUuid.GetEnumerator();
            bool hv = true;
            int offset = -1;
            LinkedEnumerableBuilder<byte> enumerable = new();
            byte value = 0;
            int wsIndex = -1;
            while (enumerator.MoveNext())
            {
                offset++;
                char c = enumerator.Current;
                if (hv)
                    switch (c)
                    {
                        case '0':
                            value = 0x00;
                            break;
                        case '1':
                            value = 0x10;
                            break;
                        case '2':
                            value = 0x20;
                            break;
                        case '3':
                            value = 0x30;
                            break;
                        case '4':
                            value = 0x40;
                            break;
                        case '5':
                            value = 0x50;
                            break;
                        case '6':
                            value = 0x60;
                            break;
                        case '7':
                            value = 0x70;
                            break;
                        case '8':
                            value = 0x80;
                            break;
                        case '9':
                            value = 0x90;
                            break;
                        case 'A':
                        case 'a':
                            value = 0xa0;
                            break;
                        case 'B':
                        case 'b':
                            value = 0xb0;
                            break;
                        case 'C':
                        case 'c':
                            value = 0xc0;
                            break;
                        case 'D':
                        case 'd':
                            value = 0xd0;
                            break;
                        case 'E':
                        case 'e':
                            value = 0xe0;
                            break;
                        case 'F':
                        case 'f':
                            value = 0xf0;
                            break;
                        case '+':
                        case '/':
                            try
                            {
                                if (b64BinHexOrUuid is string s)
                                {
                                    result = Convert.FromBase64String((offset > 0) ? s[offset..] : s);
                                    return true;
                                }
                                if (b64BinHexOrUuid is char[] arr)
                                {
                                    if (offset > 0)
                                        arr = arr[offset..];
                                }
                                else
                                    arr = ((offset > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray();
                                result = Convert.FromBase64CharArray(arr, 0, arr.Length);
                                return true;
                            }
                            catch
                            {
                                result = null;
                                return false;
                            }
                        case '{':
                            if (enumerable.Count == 0 && ((b64BinHexOrUuid is string uuid) ? Guid.TryParse((offset > 0) ? uuid[offset..] : uuid, out Guid guid) :
                                Guid.TryParse(((offset > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray(), out guid)))
                            {
                                result = guid.ToByteArray();
                                return true;
                            }
                            result = null;
                            return false;
                        case '-':
                            if (enumerable.Count == 4)
                            {
                                int index = offset - 8;
                                if (index > wsIndex && ((b64BinHexOrUuid is string u) ? Guid.TryParse((index > 0) ? u[offset..] : u, out Guid g) :
                                    Guid.TryParse(((index > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray(), out g)))
                                {
                                    result = g.ToByteArray();
                                    return true;
                                }
                            }
                            result = null;
                            return false;
                        default:
                            if (!(char.IsWhiteSpace(enumerator.Current) || char.IsControl(enumerator.Current)))
                            {
                                result = null;
                                return false;
                            }
                            wsIndex = offset;
                            break;
                    }
                else
                    switch (c)
                    {
                        case '0':
                            enumerable.Add(value);
                            break;
                        case '1':
                            enumerable.Add((byte)(value | 0x01));
                            break;
                        case '2':
                            enumerable.Add((byte)(value | 0x02));
                            break;
                        case '3':
                            enumerable.Add((byte)(value | 0x03));
                            break;
                        case '4':
                            enumerable.Add((byte)(value | 0x04));
                            break;
                        case '5':
                            enumerable.Add((byte)(value | 0x05));
                            break;
                        case '6':
                            enumerable.Add((byte)(value | 0x06));
                            break;
                        case '7':
                            enumerable.Add((byte)(value | 0x07));
                            break;
                        case '8':
                            enumerable.Add((byte)(value | 0x08));
                            break;
                        case '9':
                            enumerable.Add((byte)(value | 0x09));
                            break;
                        case 'A':
                        case 'a':
                            enumerable.Add((byte)(value | 0x0a));
                            break;
                        case 'B':
                        case 'b':
                            enumerable.Add((byte)(value | 0x0b));
                            break;
                        case 'C':
                        case 'c':
                            enumerable.Add((byte)(value | 0x0c));
                            break;
                        case 'D':
                        case 'd':
                            enumerable.Add((byte)(value | 0x0d));
                            break;
                        case 'E':
                        case 'e':
                            enumerable.Add((byte)(value | 0x0e));
                            break;
                        case 'F':
                        case 'f':
                            enumerable.Add((byte)(value | 0x0f));
                            break;
                        case '+':
                        case '/':
                        case '=':
                            try
                            {
                                if (b64BinHexOrUuid is string s)
                                {
                                    result = Convert.FromBase64String((offset > 0) ? s[offset..] : s);
                                    return true;
                                }
                                if (b64BinHexOrUuid is char[] arr)
                                {
                                    if (offset > 0)
                                        arr = arr[offset..];
                                }
                                else
                                    arr = ((offset > 0) ? b64BinHexOrUuid.Skip(offset) : b64BinHexOrUuid).ToArray();
                                result = Convert.FromBase64CharArray(arr, 0, arr.Length);
                                return true;
                            }
                            catch
                            {
                                result = null;
                                return false;
                            }
                        default:
                            if (!(char.IsWhiteSpace(enumerator.Current) || char.IsControl(enumerator.Current)))
                            {
                                result = null;
                                return false;
                            }
                            break;
                    }
                hv = !hv;
            }
            if (hv)
            {
                result = enumerable.Build();
                return true;
            }
            result = null;
            return false;
        }

        public static IEnumerable<byte> ParseBinHex(IEnumerable<char> binHex) => (binHex is null) ? null : ParseBinHexPrivate(binHex);

        public static bool TryParseBinHex(IEnumerable<char> binHex, out IEnumerable<byte> result)
        {
            if (binHex is null)
            {
                result = null;
                return true;
            }
            return TryParseBinHexPrivate(binHex, out result);
        }

        private static IEnumerable<byte> ParseBinHexPrivate([DisallowNull] IEnumerable<char> binHex)
        {
            bool hv = true;
            using (var enumerator = binHex.Select((Value, Index) => (Value, Index)).GetEnumerator())
            {
                byte value = 0;
                while (enumerator.MoveNext())
                {
                    char c = enumerator.Current.Value;
                    if (hv)
                        switch (c)
                        {
                            case '0':
                                value = 0x00;
                                break;
                            case '1':
                                value = 0x10;
                                break;
                            case '2':
                                value = 0x20;
                                break;
                            case '3':
                                value = 0x30;
                                break;
                            case '4':
                                value = 0x40;
                                break;
                            case '5':
                                value = 0x50;
                                break;
                            case '6':
                                value = 0x60;
                                break;
                            case '7':
                                value = 0x70;
                                break;
                            case '8':
                                value = 0x80;
                                break;
                            case '9':
                                value = 0x90;
                                break;
                            case 'a':
                            case 'A':
                                value = 0xa0;
                                break;
                            case 'b':
                            case 'B':
                                value = 0xb0;
                                break;
                            case 'c':
                            case 'C':
                                value = 0xc0;
                                break;
                            case 'd':
                            case 'D':
                                value = 0xd0;
                                break;
                            case 'e':
                            case 'E':
                                value = 0xe0;
                                break;
                            case 'f':
                            case 'F':
                                value = 0xf0;
                                break;
                            default:
                                if (!(char.IsWhiteSpace(c) || char.IsControl(c)))
                                    throw new FormatException($"Invalid BinHex sequence at index {enumerator.Current.Index}.");
                                break;
                        }
                    else
                        switch (c)
                        {
                            case '0':
                                yield return value;
                                break;
                            case '1':
                                yield return (byte)(value | 0x01);
                                break;
                            case '2':
                                yield return (byte)(value | 0x02);
                                break;
                            case '3':
                                yield return (byte)(value | 0x03);
                                break;
                            case '4':
                                yield return (byte)(value | 0x04);
                                break;
                            case '5':
                                yield return (byte)(value | 0x05);
                                break;
                            case '6':
                                yield return (byte)(value | 0x06);
                                break;
                            case '7':
                                yield return (byte)(value | 0x07);
                                break;
                            case '8':
                                yield return (byte)(value | 0x08);
                                break;
                            case '9':
                                yield return (byte)(value | 0x09);
                                break;
                            case 'a':
                            case 'A':
                                yield return (byte)(value | 0x0a);
                                break;
                            case 'b':
                            case 'B':
                                yield return (byte)(value | 0x0b);
                                break;
                            case 'c':
                            case 'C':
                                yield return (byte)(value | 0x0c);
                                break;
                            case 'd':
                            case 'D':
                                yield return (byte)(value | 0x0d);
                                break;
                            case 'e':
                            case 'E':
                                yield return (byte)(value | 0x0e);
                                break;
                            case 'f':
                            case 'F':
                                yield return (byte)(value | 0x0f);
                                break;
                            default:
                                if (!(char.IsWhiteSpace(c) || char.IsControl(c)))
                                    throw new FormatException($"Invalid BinHex sequence at index {enumerator.Current.Index}.");
                                break;
                        }
                }
            }
            if (!hv)
                throw new FormatException("Uneven number of hexidecimal characters.");
        }

        private static bool TryParseBinHexPrivate([DisallowNull] IEnumerable<char> binHex, out IEnumerable<byte> result)
        {
            bool hv = true;
            LinkedEnumerableBuilder<byte> enumerable = new();
            using (var enumerator = binHex.Select((Value, Index) => (Value, Index)).GetEnumerator())
            {
                byte value = 0;
                while (enumerator.MoveNext())
                {
                    char c = enumerator.Current.Value;
                    if (hv)
                        switch (c)
                        {
                            case '0':
                                value = 0x00;
                                break;
                            case '1':
                                value = 0x10;
                                break;
                            case '2':
                                value = 0x20;
                                break;
                            case '3':
                                value = 0x30;
                                break;
                            case '4':
                                value = 0x40;
                                break;
                            case '5':
                                value = 0x50;
                                break;
                            case '6':
                                value = 0x60;
                                break;
                            case '7':
                                value = 0x70;
                                break;
                            case '8':
                                value = 0x80;
                                break;
                            case '9':
                                value = 0x90;
                                break;
                            case 'a':
                            case 'A':
                                value = 0xa0;
                                break;
                            case 'b':
                            case 'B':
                                value = 0xb0;
                                break;
                            case 'c':
                            case 'C':
                                value = 0xc0;
                                break;
                            case 'd':
                            case 'D':
                                value = 0xd0;
                                break;
                            case 'e':
                            case 'E':
                                value = 0xe0;
                                break;
                            case 'f':
                            case 'F':
                                value = 0xf0;
                                break;
                            default:
                                if (!(char.IsWhiteSpace(c) || char.IsControl(c)))
                                {
                                    result = null;
                                    return false;
                                }
                                break;
                        }
                    else
                        switch (c)
                        {
                            case '0':
                                enumerable.Add(value);
                                break;
                            case '1':
                                enumerable.Add((byte)(value | 0x01));
                                break;
                            case '2':
                                enumerable.Add((byte)(value | 0x02));
                                break;
                            case '3':
                                enumerable.Add((byte)(value | 0x03));
                                break;
                            case '4':
                                enumerable.Add((byte)(value | 0x04));
                                break;
                            case '5':
                                enumerable.Add((byte)(value | 0x05));
                                break;
                            case '6':
                                enumerable.Add((byte)(value | 0x06));
                                break;
                            case '7':
                                enumerable.Add((byte)(value | 0x07));
                                break;
                            case '8':
                                enumerable.Add((byte)(value | 0x08));
                                break;
                            case '9':
                                enumerable.Add((byte)(value | 0x09));
                                break;
                            case 'a':
                            case 'A':
                                enumerable.Add((byte)(value | 0x0a));
                                break;
                            case 'b':
                            case 'B':
                                enumerable.Add((byte)(value | 0x0b));
                                break;
                            case 'c':
                            case 'C':
                                enumerable.Add((byte)(value | 0x0c));
                                break;
                            case 'd':
                            case 'D':
                                enumerable.Add((byte)(value | 0x0d));
                                break;
                            case 'e':
                            case 'E':
                                enumerable.Add((byte)(value | 0x0e));
                                break;
                            case 'f':
                            case 'F':
                                enumerable.Add((byte)(value | 0x0f));
                                break;
                            default:
                                if (!(char.IsWhiteSpace(c) || char.IsControl(c)))
                                {
                                    result = null;
                                    return false;
                                }
                                break;
                        }
                }
            }
            if (hv)
            {
                result = enumerable.Build();
                return true;
            }
            result = null;
            return false;
        }

        protected override byte[] CreateFromEnumerable([AllowNull] IEnumerable<byte> elements) => elements?.ToArray();

        protected override bool TryCreateFromEnumerable([AllowNull] IEnumerable<byte> elements, out byte[] result)
        {
            result = elements?.ToArray();
            return true;
        }

        public override byte[] Coerce(object obj)
        {
            if (obj is IEnumerable<char> ec)
                return Parse(ec).ToArray();
            return base.Coerce(obj);
        }

        public override bool TryCoerce(object obj, out byte[] result)
        {
            if (obj is IEnumerable<char> ec)
            {
                result = Parse(ec).ToArray();
                return true;
            }
            return base.TryCoerce(obj, out result);
        }
    }
}
