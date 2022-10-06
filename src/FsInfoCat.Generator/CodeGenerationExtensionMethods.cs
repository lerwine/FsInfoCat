using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FsInfoCat.Generator
{
    public static class CodeGenerationExtensionMethods
    {
        public static string GetIndentString(int level) => (level > 0) ? new string(' ', level * 4) : "";

        public static CodeBuilder AppendTypeString(this CodeBuilder stringBuilder!!, XElement element, bool appendNewLine = false)
        {
            // TODO: Implement CodeGenerationExtensionMethods.AppendTypeString(StringBuilder, XElement)
            throw new NotImplementedException();
        }

        public static CodeBuilder AppendJoined(this CodeBuilder codeBuilder!!, IEnumerable<XElement> elements, XName attributeName!!, string separator, string leading = null, string trailing = null)
        {
            if (string.IsNullOrEmpty(separator)) throw new ArgumentException($"'{nameof(separator)}' cannot be null or empty.", nameof(separator));
            if (elements is null) return codeBuilder;
            using IEnumerator<XAttribute> enumerator = elements.Where(e => e is not null).Attributes(attributeName).GetEnumerator();
            if (enumerator.MoveNext())
            {
                if (!string.IsNullOrEmpty(leading)) codeBuilder.Append(leading);
                codeBuilder.Append(enumerator.Current.Value);
                while (enumerator.MoveNext())
                    codeBuilder.Append(separator).Append(enumerator.Current.Value);
                if (!string.IsNullOrEmpty(trailing)) codeBuilder.Append(trailing);
            }
            return codeBuilder;
        }

        public static CodeBuilder AppendJoined(this CodeBuilder codeBuilder!!, IEnumerable<XElement> elements, XmlNames attributeName, string separator, string leading = null, string trailing = null) =>
            AppendJoined(codeBuilder, elements, attributeName.GetLocalXName(), separator, leading, trailing);

        public static TextSpan ToTextSpan(this string text, LinePosition start, LinePosition end)
        {
            if (start < end) throw new ArgumentException($"{nameof(start)} must be less than {nameof(end)} {nameof(LinePosition)}", nameof(start));
            char[] lineBreakChars = new[]{ '\r', '\n' };
            int startIndex = 0, lineIndex = 0;
            do
            {
                int nextIndex = text.IndexOfAny(lineBreakChars, startIndex);
                if (nextIndex < 0 || (startIndex = nextIndex + 1) == text.Length || (text[nextIndex] == '\r' && text[startIndex] == '\n' && ++startIndex == text.Length))
                {
                    if (++lineIndex < start.Line) throw new ArgumentOutOfRangeException(nameof(start));
                    break;
                }
            } while (++lineIndex < start.Line);
            int endIndex = startIndex;
            if ((startIndex += start.Character) >= text.Length) throw new ArgumentOutOfRangeException(nameof(start));
            while (lineIndex < end.Line)
            {
                lineIndex++;
                int nextIndex = text.IndexOfAny(lineBreakChars, endIndex);
                if (nextIndex < 0 || (endIndex = nextIndex + 1) == text.Length || (text[nextIndex] == '\r' && text[endIndex] == '\n' && ++endIndex == text.Length))
                {
                    if (lineIndex < end.Line) throw new ArgumentOutOfRangeException(nameof(end));
                    break;
                }
            }
            if ((endIndex += start.Character) > text.Length) throw new ArgumentOutOfRangeException(nameof(end));
            return TextSpan.FromBounds(startIndex, endIndex);
        }

        public static bool TryGetLocation(this XmlException exception, string text, string filePath, out Location result)
        {
            if(text is null || exception is null || exception.LineNumber < 1 || exception.LinePosition < 1)
            {
                result = default;
                return false;
            }
            LinePosition startPosition = new(exception.LineNumber - 1, exception.LinePosition - 1);
            LinePositionSpan positionSpan = new(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
            result = Location.Create(filePath, ToTextSpan(text, positionSpan.Start, positionSpan.End), positionSpan);
            return true;
        }

        public static Location GetLocation(this XmlException exception, string text, string filePath, out LinePosition startPosition)
        {
            if (text is null || exception is null)
                startPosition = new LinePosition(0, 0);
            else
                startPosition = new LinePosition(Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1);
            LinePositionSpan positionSpan = new(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
            return Location.Create(filePath, ToTextSpan(text, positionSpan.Start, positionSpan.End), positionSpan);
        }
    }
}
