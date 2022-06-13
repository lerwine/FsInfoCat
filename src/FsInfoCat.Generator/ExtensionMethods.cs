using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FsInfoCat.Generator
{
    public enum ElementNames
    {
        Models,
        Namespace,
        Class,
        Record,
        Struct,
        Interface,
        Enum,
        Field,
        Property,
        EventProperty,
        EventField,
        Documentation,
        BaseTypes,
        Delegate,
        Method,
        Constructor,
        Destructor,
        Operator,
        ConversionOperator,
        Unsupported,
        Type,
        DeclaringType,
        BaseType,
        Implements,
        Types
    }
    public static class ExtensionMethods
    {
        public static readonly XNamespace ModelNamespace = XNamespace.Get("http://git.erwinefamily.net/FsInfoCat/V1/FsInfoCat.Generator/ModelDefinitions.xsd");

        public static XName GetXName(this ElementNames name) => ModelNamespace.GetName(name.ToString("F"));

        public static XElement AddXElement(this XContainer container, ElementNames name)
        {
            XElement result = new XElement(GetXName(name));
            container.Add(result);
            return result;
        }

        public static XElement AddXElement(this XContainer container, ElementNames name, object content)
        {
            XElement result = new XElement(GetXName(name), content);
            container.Add(result);
            return result;
        }

        public static XElement AddXElement(this XContainer container, ElementNames name, params object[] content)
        {
            XElement result = new XElement(GetXName(name), content);
            container.Add(result);
            return result;
        }

        public static XElement Element(this XContainer container, ElementNames name) => container?.Element(GetXName(name));

        public static IEnumerable<XElement> Elements(this XContainer container, ElementNames name) => container?.Elements(GetXName(name)) ?? Enumerable.Empty<XElement>();

        public static bool IsModelElement(this XElement element, ElementNames name) => GetXName(name).Equals(element?.Name);

        public static void ReportDiagnostic(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, Location location, DiagnosticSeverity severity, params object[] messageArgs) => context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(id: ((int)id).ToString("d3"),
            title: title,
            messageFormat: messageFormat,
            category: "FsInfoCat.Generator",
            severity,
            isEnabledByDefault: true
        ), location, messageArgs));

        public static void ReportDiagnosticError(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, Location location, params object[] messageArgs)
            => ReportDiagnostic(context, id, title, messageFormat, location, DiagnosticSeverity.Error, messageArgs);

        public static void ReportDiagnosticWarning(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, Location location, params object[] messageArgs)
            => ReportDiagnostic(context, id, title, messageFormat, location, DiagnosticSeverity.Warning, messageArgs);

        public static void ReportDiagnosticInfo(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, Location location, params object[] messageArgs)
            => ReportDiagnostic(context, id, title, messageFormat, location, DiagnosticSeverity.Info, messageArgs);

        public static void ReportDiagnosticError(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, params object[] messageArgs)
            => ReportDiagnosticError(context, id, title, messageFormat, Location.None, messageArgs);

        public static void ReportDiagnosticWarning(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, params object[] messageArgs)
            => ReportDiagnosticWarning(context, id, title, messageFormat, Location.None, messageArgs);

        public static void ReportDiagnosticInfo(this GeneratorExecutionContext context, DiagnosticId id, string title, string messageFormat, params object[] messageArgs)
            => ReportDiagnosticInfo(context, id, title, messageFormat, Location.None, messageArgs);

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
            LinePosition startPosition = new LinePosition(exception.LineNumber - 1, exception.LinePosition - 1);
            LinePositionSpan positionSpan = new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
            result = Location.Create(filePath, ToTextSpan(text, positionSpan.Start, positionSpan.End), positionSpan);
            return true;
        }

        public static Location GetLocation(this XmlException exception, string text, string filePath, out LinePosition startPosition)
        {
            if (text is null || exception is null)
                startPosition = new LinePosition(0, 0);
            else
                startPosition = new LinePosition(Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1);
            LinePositionSpan positionSpan = new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
            return Location.Create(filePath, ToTextSpan(text, positionSpan.Start, positionSpan.End), positionSpan);
        }

        public static void AddDocumentationElement(this XElement parentElement, SyntaxNode memberSyntax, GeneratorExecutionContext context)
        {
            if (!memberSyntax.HasLeadingTrivia) return;
            XElement documentationElement = new XElement(GetXName(ElementNames.Documentation));
            foreach (XmlElementSyntax elementSyntax in memberSyntax.GetLeadingTrivia().OfType<DocumentationCommentTriviaSyntax>().SelectMany(t => t.Content.OfType<XmlElementSyntax>()))
            {
                XElement element;
                string text = elementSyntax.GetText().ToString();
                try { element = XElement.Parse(text, LoadOptions.SetLineInfo | LoadOptions.PreserveWhitespace); }
                catch (XmlException xmlException)
                {
                    Location elementLocation = elementSyntax.SyntaxTree.GetLocation(elementSyntax.Span);
                    FileLinePositionSpan elementPosition = elementLocation.GetLineSpan();
                    Location relativeLocation = GetLocation(xmlException, text, elementPosition.Path, out LinePosition relativePosition);
                    LinePosition startPosition;
                    if (relativePosition.Line == 0)
                        startPosition = new LinePosition(elementPosition.StartLinePosition.Line, relativePosition.Character + elementPosition.StartLinePosition.Character);
                    else
                        startPosition = new LinePosition(elementPosition.StartLinePosition.Line + relativePosition.Line, relativePosition.Character);
                    int start = elementLocation.SourceSpan.Start + relativeLocation.SourceSpan.Start;
                    if (string.IsNullOrWhiteSpace(xmlException.Message))
                        context.ReportDiagnosticWarning(DiagnosticId.DocCommentParseError, "Documentation comment parse error", "Failed to parse documentation comment in {0}, line {1}, column {2}",
                            Location.Create(elementPosition.Path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1))),
                            elementPosition.Path, startPosition.Line + 1, startPosition.Character + 1);
                    else
                        context.ReportDiagnosticWarning(DiagnosticId.DocCommentParseError, "Documentation comment parse error", "Failed to parse documentation comment in {0}, line {1}, column {2} ({3})",
                            Location.Create(elementPosition.Path, TextSpan.FromBounds(start, start + 1), new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1))),
                            elementPosition.Path, startPosition.Line + 1, startPosition.Character + 1, xmlException.Message);
                    continue;
                }
                catch (Exception exception)
                {
                    Location elementLocation = elementSyntax.SyntaxTree.GetLocation(elementSyntax.Span);
                    FileLinePositionSpan elementPosition = elementLocation.GetLineSpan();
                    if (string.IsNullOrWhiteSpace(exception.Message))
                        context.ReportDiagnosticWarning(DiagnosticId.DocCommentParseError, "Unexpected documentation comment parse error",
                            "Unexpected {0} while parsing documentation comment in {1}, line {2}, column {3}", elementLocation, exception.GetType().Name, elementPosition.Path,
                            elementPosition.StartLinePosition.Line + 1, elementPosition.StartLinePosition.Character + 1);
                    else
                        context.ReportDiagnosticWarning(DiagnosticId.DocCommentParseError, "Unexpected documentation comment parse error",
                            "Unexpected {0} while parsing documentation comment in {1}, line {2}, column {3} ({4})", elementLocation, exception.GetType().Name, elementPosition.Path,
                            elementPosition.StartLinePosition.Line + 1, elementPosition.StartLinePosition.Character + 1, exception.Message);
                    continue;
                }
                documentationElement.Add(element);
            }
            if (!documentationElement.IsEmpty)
                parentElement.Add(documentationElement);
        }

        public static void AddBaseTypeElements(this XElement typeElement, BaseListSyntax baseTypes)
        {
            if (baseTypes is null || baseTypes.Types.Count == 0) return;
            XElement parentElement = typeElement.AddXElement(ElementNames.BaseTypes);
            foreach (TypeSyntax type in baseTypes.Types.Select(t => t.Type))
                AddTypeElement(parentElement, type);
        }

        public static void AddTypeElement(this XElement parentElement, TypeSyntax type)
        {

        }
    }
}
