using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    public class ArrayCoersion<T> : EnumerableCoersion<T, T[]>
    {
        public static readonly ArrayCoersion<T> Default = new();

        public ArrayCoersion(ICoersion<T> elementCoersion) : base(elementCoersion) { }

        public ArrayCoersion() : base() { }

        protected override T[] CreateFromEnumerable([MaybeNull] IEnumerable<T> elements) => elements?.ToArray();

        protected override bool TryCreateFromEnumerable([MaybeNull] IEnumerable<T> elements, out T[] result)
        {
            result = elements?.ToArray();
            return true;
        }
    }

    public class ByteArrayCoersion : EnumerableCoersion<byte, byte[]>
    {
        public static readonly ByteArrayCoersion Default = new();

        // TODO: Create better BinHex parsing
        public static readonly Regex Base64SequenceRegex = new(@"^\s*(([a-z\d+/]\s*){4}(?=[a-z\d+/]\s*[a-z\d+/]))*((?<t>[a-z\d+/]([a-z\d+/]([a-z\d+/][a-z\d+/=]|==)|===)\s*$)|([a-z\d+/]([a-z\d+/]([a-z\d+/][a-z\d+/=]?|==?)?|=(==?)?)?)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex BinHexSequenceRegex = new(@"^\s*([a-f\d]\s*[a-f\d]\s*)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex WsRegex = new(@"[\s\r\n]+", RegexOptions.Compiled);


        public ByteArrayCoersion() : base() { }

        protected override byte[] CreateFromEnumerable([MaybeNull] IEnumerable<byte> elements) => elements?.ToArray();

        protected override bool TryCreateFromEnumerable([MaybeNull] IEnumerable<byte> elements, out byte[] result)
        {
            result = elements?.ToArray();
            return true;
        }

        public override byte[] Coerce(object obj)
        {
            return base.Coerce(obj);
        }

        public override bool TryCoerce(object obj, out byte[] result)
        {
            return base.TryCoerce(obj, out result);
        }
    }
}
