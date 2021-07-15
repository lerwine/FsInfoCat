using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace FsInfoCat.Collections
{
    public partial class MultiStringValue : ReadOnlyCollection<string>, IEquatable<MultiStringValue>, IConvertible
    {
        public static bool NullOrNotAny(MultiStringValue source) => source is null || source.Count == 0;

        public static MultiStringValue NullIfNotAny(MultiStringValue source) => (source is null || source.Count == 0) ? null : source;

        public static readonly NullIfWhiteSpaceOrNormalizedStringCoersion Coersion = new(StringComparer.InvariantCultureIgnoreCase);

        public static bool AreEqual(MultiStringValue x, MultiStringValue y)
        {
            if (NullOrNotAny(x))
                return NullOrNotAny(y);
            return x.Equals(y);
        }

        public static readonly ValueConverter<MultiStringValue, string> Converter = new(
            v => (v == null) ? null : v.ToString(),
            s => (s == null) ? null : new MultiStringValue(s)
        );

        private const char ESCAPE = '`';
        private const char EMPTY = '!';
        private const char NULL = '?';
        private const char RECORD_SEPARATOR = 'z';
        private static readonly string ESCAPED_EMPTY = $"{ESCAPE}{EMPTY}";
        private static readonly string ESCAPED_NULL = $"{ESCAPE}{NULL}";
        private static readonly string ESCAPED_LITERAL = $"{ESCAPE}{ESCAPE}";
        private static readonly string UNESCAPED_LITERAL = $"{ESCAPE}";

        public static string Encode([DisallowNull] IEnumerable<string> rawValues)
        {
            if (rawValues is null)
                throw new ArgumentNullException(nameof(rawValues));
            using IEnumerator<string> enumerator = rawValues.GetEnumerator();
            if (!enumerator.MoveNext())
                return ESCAPED_EMPTY;
            string text = Coersion.Normalize(enumerator.Current);
            if (!enumerator.MoveNext())
                return (text is null) ? ESCAPED_NULL : text.Replace(UNESCAPED_LITERAL, ESCAPED_LITERAL);
            StringBuilder stringBuilder = new((text is null) ? ESCAPED_NULL : text.Replace(UNESCAPED_LITERAL, ESCAPED_LITERAL));
            do
            {
                text = Coersion.Normalize(enumerator.Current);
                stringBuilder.Append(ESCAPE).Append(RECORD_SEPARATOR).Append((text is null) ? ESCAPED_NULL : text.Replace(UNESCAPED_LITERAL, ESCAPED_LITERAL));
            } while (enumerator.MoveNext());
            return stringBuilder.ToString();
        }

        public static IEnumerable<string> Decode([DisallowNull] string encodedText)
        {
            if (encodedText is null)
                throw new ArgumentNullException(nameof(encodedText));
            int index = encodedText.IndexOf(ESCAPE);
            if (index < 0)
                yield return encodedText;
            else
            {
                int e = encodedText.Length;
                IValueBuilder valueBuilder = new NullValueBuilder();
                if (index == 0)
                {
                    if (e == 1)
                        throw new ArgumentOutOfRangeException(nameof(encodedText), "Invalid escape sequence at index 0");
                    if (encodedText[1] == EMPTY)
                    {
                        if (e > 2)
                            throw new ArgumentOutOfRangeException(nameof(encodedText), "Invalid escape sequence at index 2");
                        yield break;
                    }
                }
                int startIndex = 0;
                do
                {
                    int len = index - startIndex;
                    if (len == 1)
                        valueBuilder = valueBuilder.Append(encodedText[startIndex]);
                    else if (len > 0)
                        valueBuilder = valueBuilder.Append(encodedText.Substring(startIndex, len));
                    switch (encodedText[++index])
                    {
                        case ESCAPE:
                            valueBuilder = valueBuilder.Append(ESCAPE);
                            continue;
                        case NULL:
                            if (valueBuilder.TryGetValue(out string n))
                                yield return n;
                            yield return null;
                            break;
                        case RECORD_SEPARATOR:
                            yield return valueBuilder.GetValue();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(encodedText), $"Invalid escape sequence at index {index}");
                    }
                    valueBuilder = new NullValueBuilder();
                }
                while ((index = encodedText.IndexOf(ESCAPE, startIndex)) > 0);
                if (valueBuilder.TryGetValue(out string t))
                    yield return t;
            }
        }

        public MultiStringValue(string encodedText) : base(string.IsNullOrEmpty(encodedText) ? new List<string>() : Decode(encodedText).ToList()) { }

        public MultiStringValue() : base(Array.Empty<string>()) { }

        public bool Equals(MultiStringValue other) => (Count == 0) ? other is null || other.Count == 0 : ReferenceEquals(this, other) ||
            (other is not null && other.Count > 0 && ToString().Equals(other.ToString().EmptyIfNullOrWhiteSpace(), StringComparison.InvariantCultureIgnoreCase));

        public override bool Equals(object obj) => obj is MultiStringValue other && Equals(other);

        public override int GetHashCode() => Coersion.GetHashCode(ToString());

        public override string ToString() => (Count == 0) ? "" : Encode(this);

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
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) =>
            (conversionType ?? throw new ArgumentNullException(nameof(conversionType))).IsInstanceOfType(this) ? this :
                Convert.ChangeType(ToString(), conversionType, provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(ToString(), provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(ToString(), provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(ToString(), provider);

        public static bool operator ==(MultiStringValue left, MultiStringValue right) => left.Equals(right);

        public static bool operator !=(MultiStringValue left, MultiStringValue right) => !(left == right);

        public static implicit operator MultiStringValue(string[] values) => (values is null || values.Length == 0 || (values = values.Select(StringExtensionMethods.NullIfWhiteSpace).Where(t => t is not null).ToArray()).Length == 0) ? null : new(values);

        public static implicit operator string[](MultiStringValue values) => values?.ToArray();

        public static implicit operator MultiStringValue(string text) => string.IsNullOrWhiteSpace(text) ? null : new(text);

        public static implicit operator string(MultiStringValue values) => values?.ToString();
    }
}
