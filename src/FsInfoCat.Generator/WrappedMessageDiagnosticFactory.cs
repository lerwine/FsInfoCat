using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace FsInfoCat.Generator
{
    public sealed class WrappedMessageDiagnosticFactory : AbstractDiagnosticFactory
    {
        public WrappedMessageDiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, ""); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(string message, Location location = null) => Diagnostic.Create(Descriptor, location, message);

        private Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines!!, string message)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(message, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition,
                new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo lineInfo!!, TextLineCollection lines!!, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            return Create(path, Math.Max(1, lineInfo.LineNumber) - 1, Math.Max(1, lineInfo.LinePosition) - 1, lines, message);
        }

        public Diagnostic Create(string path, XmlSchemaException exception!!, TextLineCollection lines!!)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlException)}" : exception.Message);
        }

        public Diagnostic Create(string path, XmlException exception!!, TextLineCollection lines!!)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlException)}" : exception.Message);
        }
    }

    public sealed class WrappedMessageDiagnosticFactory<T> : AbstractDiagnosticFactory
    {
        public WrappedMessageDiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, default(T), ""); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(T arg, string message, Location location = null) => Diagnostic.Create(Descriptor, location, arg, message);

        private Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines!!, T arg, string message)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(arg, message, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition,
                new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo lineInfo!!, TextLineCollection lines!!, T arg, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            return Create(path, Math.Max(1, lineInfo.LineNumber) - 1, Math.Max(1, lineInfo.LinePosition) - 1, lines, arg, message);
        }

        public Diagnostic Create(string path, XmlSchemaException exception!!, TextLineCollection lines!!, T arg)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines, arg,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlSchemaException)}" : exception.Message);
        }

        public Diagnostic Create(string path, XmlException exception!!, TextLineCollection lines!!, T arg)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines, arg,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlException)}" : exception.Message);
        }
    }

    public sealed class WrappedMessageDiagnosticFactory<T0, T1> : AbstractDiagnosticFactory
    {
        public WrappedMessageDiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, default(T0), default(T1), ""); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(T0 arg0, T1 arg1, string message, Location location = null) => Diagnostic.Create(Descriptor, location, arg0, arg1, message);

        private Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines!!, T0 arg0, T1 arg1, string message)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(arg0, arg1, message, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition,
                new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo lineInfo!!, TextLineCollection lines!!, T0 arg0, T1 arg1, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            return Create(path, Math.Max(1, lineInfo.LineNumber) - 1, Math.Max(1, lineInfo.LinePosition) - 1, lines, arg0, arg1, message);
        }

        public Diagnostic Create(string path, XmlSchemaException exception!!, TextLineCollection lines!!, T0 arg0, T1 arg1)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines, arg0, arg1,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlSchemaException)}" : exception.Message);
        }

        public Diagnostic Create(string path, XmlException exception!!, TextLineCollection lines!!, T0 arg0, T1 arg1)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines, arg0, arg1,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlException)}" : exception.Message);
        }
    }

    public sealed class WrappedMessageDiagnosticFactory<T0, T1, T2> : AbstractDiagnosticFactory
    {
        public WrappedMessageDiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault = true, string description = null, string helpLinkUri = null, params string[] customTags)
            : base(id, title, messageFormat, isEnabledByDefault, description, helpLinkUri, customTags)
        {
            try { string.Format(messageFormat, default(T0), default(T1), default(T2), ""); }
            catch (FormatException exc) { throw new ArgumentException(exc.Message, nameof(messageFormat), exc); }
        }

        public Diagnostic Create(T0 arg0, T1 arg1, T2 arg2, string message, Location location = null) => Diagnostic.Create(Descriptor, location, arg0, arg1, arg2, message);

        private Diagnostic Create(string path, int lineIndex, int colIndex, TextLineCollection lines!!, T0 arg0, T1 arg1, T2 arg2, string message)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            TextLine textLine;
            try { textLine = lines.First(l => l.LineNumber == lineIndex); }
            catch { throw new ArgumentOutOfRangeException(nameof(lineIndex)); }
            if (colIndex >= textLine.Span.Length) throw new ArgumentOutOfRangeException(nameof(colIndex));
            LinePosition startPosition = new(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            return Create(arg0, arg1, arg2, message, Location.Create(path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition,
                new LinePosition(startPosition.Line, startPosition.Character + 1))));
        }

        public Diagnostic Create(string path, IXmlLineInfo lineInfo!!, TextLineCollection lines!!, T0 arg0, T1 arg1, T2 arg2, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            return Create(path, Math.Max(1, lineInfo.LineNumber) - 1, Math.Max(1, lineInfo.LinePosition) - 1, lines, arg0, arg1, arg2, message);
        }

        public Diagnostic Create(string path, XmlSchemaException exception!!, TextLineCollection lines!!, T0 arg0, T1 arg1, T2 arg2)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines, arg0, arg1, arg2,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlSchemaException)}" : exception.Message);
        }

        public Diagnostic Create(string path, XmlException exception!!, TextLineCollection lines!!, T0 arg0, T1 arg1, T2 arg2)
        {
            return Create(path, Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1, lines, arg0, arg1, arg2,
                string.IsNullOrWhiteSpace(exception?.Message) ? $"Unexpected {nameof(XmlException)}" : exception.Message);
        }
    }
}
