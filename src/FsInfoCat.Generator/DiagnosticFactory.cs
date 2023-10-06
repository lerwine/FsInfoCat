using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Xml;

namespace FsInfoCat.Generator
{
    public sealed class DiagnosticFactory : AbstractDiagnosticFactory
    {
        public DiagnosticFactory(DiagnosticId id, string title, string message, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, message, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try
            {
                if (string.Format(message, 0) == message) return;
            }
            catch (FormatException exc) { throw new ArgumentException("Message cannot have any positional placeholders", nameof(message), exc); }
            new ArgumentException("Message cannot have any positional placeholders", nameof(message));
        }

        public Diagnostic Create(Location location = null) => Diagnostic.Create(Descriptor, location);

        public Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines)
        {
            if (lineIndex < 0) throw new ArgumentOutOfRangeException(nameof(lineIndex));
            if (colIndex < 0) throw new ArgumentOutOfRangeException(nameof(colIndex));
            if (lines is null) throw new ArgumentNullException(nameof(lines));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo obj, TextLineCollection lines)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return Create(path, Math.Max(1, obj.LineNumber) - 1, Math.Max(1, obj.LinePosition) - 1, lines);
        }
    }

    public sealed class DiagnosticFactory<T> : AbstractDiagnosticFactory
    {
        public DiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, default(T)); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(T arg, Location location = null) => Diagnostic.Create(Descriptor, location, arg);

        public Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines, T arg)
        {
            if (lineIndex < 0) throw new ArgumentOutOfRangeException(nameof(lineIndex), "Line index cannot be less than zero");
            if (colIndex < 0) throw new ArgumentOutOfRangeException(nameof(colIndex), "Column index cannot be less than zero");
            if (lines is null) throw new ArgumentNullException(nameof(lines));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex), "Line index must be less than the number of lines"); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex), $"Column index {lineIndex} : {arg} must be less than the line length ({textLine.Span.Length}: {textLine})");
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(arg, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo obj, TextLineCollection lines, T arg)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return Create(path, Math.Max(1, obj.LineNumber) - 1, Math.Max(1, obj.LinePosition) - 1, lines, arg);
        }
    }

    public sealed class DiagnosticFactory<T0, T1> : AbstractDiagnosticFactory
    {
        public DiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, default(T0), default(T1)); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(T0 arg0, T1 arg1, Location location = null) => Diagnostic.Create(Descriptor, location, arg0, arg1);

        public Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines, T0 arg0, T1 arg1)
        {
            if (lineIndex < 0) throw new ArgumentOutOfRangeException(nameof(lineIndex));
            if (colIndex < 0) throw new ArgumentOutOfRangeException(nameof(colIndex));
            if (lines is null) throw new ArgumentNullException(nameof(lines));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(arg0, arg1, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo obj, TextLineCollection lines, T0 arg0, T1 arg1)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return Create(path, Math.Max(1, obj.LineNumber) - 1, Math.Max(1, obj.LinePosition) - 1, lines, arg0, arg1);
        }
    }

    public sealed class DiagnosticFactory<T0, T1, T2> : AbstractDiagnosticFactory
    {
        public DiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, default(T0), default(T1), default(T2)); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(T0 arg0, T1 arg1, T2 arg2, Location location = null) => Diagnostic.Create(Descriptor, location, arg0, arg1, arg2);

        public Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines, T0 arg0, T1 arg1, T2 arg2)
        {
            if (lineIndex < 0) throw new ArgumentOutOfRangeException(nameof(lineIndex));
            if (colIndex < 0) throw new ArgumentOutOfRangeException(nameof(colIndex));
            if (lines is null) throw new ArgumentNullException(nameof(lines));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(arg0, arg1, arg2, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo obj, TextLineCollection lines, T0 arg0, T1 arg1, T2 arg2)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            return Create(path, Math.Max(1, obj.LineNumber) - 1, Math.Max(1, obj.LinePosition) - 1, lines, arg0, arg1, arg2);
        }
    }
}
