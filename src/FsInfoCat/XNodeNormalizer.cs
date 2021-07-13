using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FsInfoCat
{
    /// <summary>
    /// A class which handles normalization of <see cref="XContainer"/>, <see cref="XComment"/> and <see cref="XText"/> nodes.
    /// </summary>
    public partial class XNodeNormalizer
    {

        /// <summary>
        /// The default new line character sequence.
        /// </summary>
        public const string DefaultNewLine = @"
";

        /// <summary>
        /// The default indent string.
        /// </summary>
        public const string DefaultIndent = "    ";

        /// <summary>
        /// Matches non-normalized line separator characters.
        /// </summary>
        public static readonly Regex NonNormalizedLineSeparatorRegex = new(@"\r(?!\n)|[\x00-\b\v\f\x0e-\x1f\p{Zl}\p{Zp}]", RegexOptions.Compiled);

        /// <summary>
        /// Matches non-normalized white-space character sequences.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex NonNormalizedWhiteSpaceRegex = new(@" [^\r\n\S]+|[^\r\n\S ][^\r\n\S]*", RegexOptions.Compiled);

        /// <summary>
        /// Matches white-space characters at the beginning or end of lines.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex ExtraneousLineWhitespaceRegex = new(@"(?<=[\r\n]|^)[^\r\n\S]+|[^\r\n\S]+(?=[\r\n]|$)", RegexOptions.Compiled);

        /// <summary>
        /// Matches white-space characters at the beginning and end if inner lines (lines other than the first and last).
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex ExtraneousInnerLineWSRegex = new(@"(?<=[\r\n\S])[^\r\n\S]+(?=[\r\n])|(?<=[\r\n])[^\r\n\S]+(?=\S)", RegexOptions.Compiled);

        /// <summary>
        /// Matches the leading newline sequence and any following white-space characters.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex LeadingNewLineWithOptWs = new(@"^(\r\n|\n)([^\r\n\S]+)?", RegexOptions.Compiled);

        /// <summary>
        /// Matches the last newline sequence that does not precede any non-white-space charaters, including any following white-space characters.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex TrailingNewLineWithOptWs = new(@"(\r\n|\n)([^\r\n\S]+)?$", RegexOptions.Compiled);

        /// <summary>
        /// Matches the leading newline sequence and any following white-space characters or leading white-space characters with no preceding newline sequence.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex LeadingNewLineAndOrWhiteSpace = new(@"^((?<n>\r\n|\n)(?<w>[^\r\n\S]+)?|(?<w>[^\r\n\S]+))", RegexOptions.Compiled);

        /// <summary>
        /// Matches the trailing newline sequence and any following white-space characters or trailing white-space characters with no preceding newline sequence.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex TrailingNewLineAndOrWhiteSpace = new(@"((?<n>\r\n|\n)(?<w>[^\r\n\S]+)?|(?<w>[^\r\n\S]+))$", RegexOptions.Compiled);

        /// <summary>
        /// Matches any line that is empty or contains only white-space characters.
        /// </summary>
        /// <remarks>This assumes that the input string does not contain any character sequences that would be matched by the <see cref="NonNormalizedLineSeparatorRegex"/> pattern.</remarks>
        public static readonly Regex EmptyOrWhiteSpaceLineRegex = new(@"^(([^\r\n\S]*(\r\n|\n))+([^\r\n\S]+$)?|[^\r\n\S]+$)|(\r\n|\n)[^\r\n\S]*((?=[\r\n])|$)", RegexOptions.Compiled);

        /// <summary>
        /// Matches a normalized new line character sequence.
        /// </summary>
        public static readonly Regex NewLineRegex = new(@"\r\n|\n", RegexOptions.Compiled);

        /// <summary>
        /// Gets the indent string.
        /// </summary>
        /// <value>The string that will be inserted as an indenteation.</value>
        [DisallowNull]
        protected virtual string IndentString => DefaultIndent;

        /// <summary>
        /// Gets the normalized fall-back new line character sequence.
        /// </summary>
        /// <value>The character sequence that will be used when a new line sequence is inserted or a non-normalized line separator is replaced.</value>
        [DisallowNull]
        protected virtual string NewLine => DefaultNewLine;

        /// <summary>
        /// Normalizes the specified text node.
        /// </summary>
        /// <param name="textNode">The <see cref="XText"/> node to normalize.</param>
        /// <param name="normalizer">The <see cref="XNodeNormalizer"/> that ionstance will be used to normalize the target <see cref="XText"/> node or <see langword="null"/> to use a default instance.</param>
        /// <param name="indentLevel">The number of times to indent each subsequent line within the <see cref="XText"/> node. The default value is <c>0</c>.</param>
        /// <seealso cref="Normalize(XText, Context)"/>
        public static void Normalize([DisallowNull] XText textNode, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(textNode ?? throw new ArgumentNullException(nameof(textNode)), indentLevel);

        /// <summary>
        /// Normalizes the specified text node.
        /// </summary>
        /// <param name="textNode">The <see cref="XText"/> node to normalize.</param>
        /// <param name="indentLevel">The number of times to indent each subsequent line within the <see cref="XText"/> node.</param>
        /// <seealso cref="Normalize(XText, Context)"/>
        public static void Normalize([DisallowNull] XText textNode, int indentLevel) => Normalize(textNode, null, indentLevel);

        /// <summary>
        /// Normalizes the specified comment node.
        /// </summary>
        /// <param name="commentNode">The <see cref="XComment"/> node to normalize.</param>
        /// <param name="normalizer">The <see cref="XNodeNormalizer"/> instance that will be used to normalize the target <see cref="XComment"/> node or <see langword="null"/> to use a default instance.</param>
        /// <param name="indentLevel">The number of times to indent the <see cref="XComment"/> node. The default value is <c>0</c>.</param>
        /// <seealso cref="Normalize(XComment, Context)"/>
        public static void Normalize([DisallowNull] XComment commentNode, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(commentNode ?? throw new ArgumentNullException(nameof(commentNode)), indentLevel);

        /// <summary>
        /// Normalizes the specified comment node.
        /// </summary>
        /// <param name="commentNode">The <see cref="XComment"/> node to normalize.</param>
        /// <param name="indentLevel">The number of times to indent the <see cref="XComment"/> node.</param>
        /// <seealso cref="Normalize(XComment, Context)"/>
        public static void Normalize([DisallowNull] XComment commentNode, int indentLevel) => Normalize(commentNode, null, indentLevel);

        /// <summary>
        /// Normalizes the specified container node.
        /// </summary>
        /// <param name="elementNode">The <see cref="XElement"/> node to normalize.</param>
        /// <param name="normalizer">The <see cref="XNodeNormalizer"/> that ionstance will be used to normalize the target <see cref="XElement"/> node or <see langword="null"/> to use a default instance.</param>
        /// <param name="indentLevel">The number of times that the <see cref="XElement"/> node is indented. The default value is <c>0</c>.</param>
        /// <seealso cref="Normalize(XContainer, Context)"/>
        public static void Normalize([DisallowNull] XElement elementNode, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(elementNode ?? throw new ArgumentNullException(nameof(elementNode)), indentLevel);

        /// <summary>
        /// Normalizes the specified container node.
        /// </summary>
        /// <param name="elementNode">The <see cref="XElement"/> node to normalize.</param>
        /// <param name="indentLevel">The number of times that the <see cref="XElement"/> node is indented. The default value is <c>0</c>.</param>
        /// <seealso cref="Normalize(XContainer, Context)"/>
        public static void Normalize([DisallowNull] XElement elementNode, int indentLevel) => Normalize(elementNode, null, indentLevel);

        /// <summary>
        /// Normalizes the specified container node.
        /// </summary>
        /// <param name="document">The <see cref="XDocument"/> node to normalize.</param>
        /// <param name="normalizer">The <see cref="XNodeNormalizer"/> that ionstance will be used to normalize the target <see cref="XDocument"/> node or <see langword="null"/> to use a default instance.</param>
        /// <param name="indentLevel">The number of times that the <see cref="XDocument.Root"/> node is indented. The default value is <c>0</c>.</param>
        /// <seealso cref="Normalize(XContainer, Context)"/>
        public static void Normalize([DisallowNull] XDocument document, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(document ?? throw new ArgumentNullException(nameof(document)), indentLevel);

        /// <summary>
        /// Normalizes the specified container node.
        /// </summary>
        /// <param name="document">The <see cref="XDocument"/> node to normalize.</param>
        /// <param name="indentLevel">The number of times that the <see cref="XDocument.Root"/> node is indented. The default value is <c>0</c>.</param>
        /// <seealso cref="Normalize(XContainer, Context)"/>
        public static void Normalize([DisallowNull] XDocument document, int indentLevel) => Normalize(document, null, indentLevel);

        private void PrivateNormalize([DisallowNull] XText textNode, int indentLevel) => Normalize(textNode, new Context(indentLevel, textNode, this));

        private void PrivateNormalize([DisallowNull] XComment commentNode, int indentLevel) => Normalize(commentNode, new Context(indentLevel, commentNode, this));

        private void PrivateNormalize([DisallowNull] XElement elementNode, int indentLevel) => Normalize(elementNode, new Context(indentLevel, elementNode, this));

        private void PrivateNormalize([DisallowNull] XDocument document, int indentLevel) => Normalize(document, new Context(indentLevel, document, this));

        /// <summary>
        /// Checks whether the target element should always have leading and trailing line break / indent sequences.
        /// </summary>
        /// <param name="targetElement">The target <see cref="XElement" /> being normalized.</param>
        /// <param name="context">The normalization <see cref="Context"/> for the  target <see cref="XElement" />.</param>
        /// <returns><see langword="true"/> if a new line / indent should always be inserted before and after the <paramref name="targetElement"/>; otherwise, <see langword="false"/>.</returns>
        protected virtual bool ForceSurroundingLineBreaks([DisallowNull] XElement targetElement, [DisallowNull] Context context) => targetElement.Nodes().OfType<XText>().Any(t => NewLineRegex.IsMatch(t.Value)) || targetElement.Nodes().OfType<XComment>().Any(t => NewLineRegex.IsMatch(t.Value));

        /// <summary>
        /// Checks whether the target element contents should always be indented
        /// </summary>
        /// <param name="targetElement">The target <see cref="XElement" /> being normalized.</param>
        /// <param name="context">The normalization <see cref="Context"/> for the  target <see cref="XElement" />.</param>
        /// <returns><see langword="true"/> if the content of the <paramref name="targetElement"/> should always be indented; otherwise, <see langword="false"/>.</returns>
        protected virtual bool ForceIndentContent([DisallowNull] XElement targetElement, [DisallowNull] Context context) => false;

        /// <summary>
        /// Normalizes the specified text node.
        /// </summary>
        /// <param name="targetTextNode">The <see cref="XText"/> node to normalize.</param>
        /// <param name="context">The normalization <see cref="Context"/> object that includes the current indent string.</param>
        /// <remarks>All lines that are empty or contain only whitespace characters will be removed. This will not indent the first line, but will indent any lines that follow.</remarks>
        protected virtual void Normalize([DisallowNull] XText targetTextNode, [DisallowNull] Context context)
        {
            if (targetTextNode.Value.Length == 0)
                return;
            targetTextNode.RemoveAnnotations<TextNodeCharacteristics2>();
            bool hasPrecedingContent = targetTextNode.NodesBeforeSelf().OfType<XContainer>().Any() || targetTextNode.NodesBeforeSelf().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));
            bool hasFollowingContent = targetTextNode.NodesAfterSelf().OfType<XContainer>().Any() || targetTextNode.NodesAfterSelf().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));
            string text = hasPrecedingContent ? (hasFollowingContent ? targetTextNode.Value : targetTextNode.Value.TrimEnd()) : hasFollowingContent ? targetTextNode.Value.TrimStart() : targetTextNode.Value.Trim();
            if (text.Length > 0)
            {
                if (NonNormalizedLineSeparatorRegex.IsMatch(text))
                    text = NonNormalizedLineSeparatorRegex.Replace(text, NewLine);
                Match leadingWsMatch = LeadingNewLineWithOptWs.Match(text);
                Match trailingWsMatch = TrailingNewLineWithOptWs.Match(text);
                if (EmptyOrWhiteSpaceLineRegex.IsMatch(text) && (text = EmptyOrWhiteSpaceLineRegex.Replace(text, "")).Length == 0)
                {
                    text = " ";
                    targetTextNode.AddAnnotation(new TextNodeCharacteristics2(false, false, leadingWsMatch.Success ? new StringValueCharacteristics(leadingWsMatch.Groups[1].Value, false) : null,
                        true, trailingWsMatch.Success ? new StringValueCharacteristics(trailingWsMatch.Groups[1].Value, false) : null, true));
                }
                else
                {
                    if (NonNormalizedWhiteSpaceRegex.IsMatch(text))
                        text = NonNormalizedWhiteSpaceRegex.Replace(text, " ");
                    if (ExtraneousInnerLineWSRegex.IsMatch(text))
                        text = ExtraneousInnerLineWSRegex.Replace(text, "");
                    if (NewLineRegex.IsMatch(text))
                    {
                        if (context.CurrentIndent.Length > 0)
                            text = NewLineRegex.Replace(text, m => $"{m.Value}{context.CurrentIndent}");
                        targetTextNode.AddAnnotation(new TextNodeCharacteristics2(true, true, leadingWsMatch.Success ? new StringValueCharacteristics(leadingWsMatch.Groups[1].Value, false) : null,
                            leadingWsMatch.Success ? leadingWsMatch.Groups[2].Success : char.IsWhiteSpace(text[0]),
                            trailingWsMatch.Success ? new StringValueCharacteristics(trailingWsMatch.Groups[1].Value, false) : null,
                            trailingWsMatch.Success ? trailingWsMatch.Groups[2].Success : char.IsWhiteSpace(text.Last())));
                    }
                    else
                        targetTextNode.AddAnnotation(new TextNodeCharacteristics2(false, true, leadingWsMatch.Success ? new StringValueCharacteristics(leadingWsMatch.Groups[1].Value, false) : null,
                            leadingWsMatch.Success ? leadingWsMatch.Groups[2].Success : char.IsWhiteSpace(text[0]),
                            trailingWsMatch.Success ? new StringValueCharacteristics(trailingWsMatch.Groups[1].Value, false) : null,
                            trailingWsMatch.Success ? trailingWsMatch.Groups[2].Success : char.IsWhiteSpace(text.Last())));
                }
            }
            else if (hasPrecedingContent || hasFollowingContent)
                targetTextNode.AddAnnotation(new TextNodeCharacteristics2(false, false, null, false, null, false));
            else
            {
                targetTextNode.AddAnnotation(new TextNodeCharacteristics2(false, false, null, true, null, true));
                text = " ";
            }
            if (targetTextNode.Value != text)
                targetTextNode.Value = text;
        }

        /// <summary>
        /// Normalizes the specified comment.
        /// </summary>
        /// <param name="targetComment">The <see cref="XComment"/> node to normalize.</param>
        /// <param name="context">The normalization <see cref="Context"/> object that includes the current indent string.</param>
        /// <remarks>This does insert any sibling nodes before or after the target <see cref="XComment"/> node (does indent the opening <c>&lt;</c> character or add a new line sequence after the outer closing <c>&gt;</c> character).
        /// Indenting will only occur if the <see cref="XComment"/> node contains 2 or more lines. Empty lines will be stripped.
        /// Single-line content will result in a single-line comment.
        /// <para>Following are the results when the <see cref="XComment"/> node contains 2 or more lines (after blank lines are removed):
        /// <list type="bullet">
        /// <item><term><code><paramref name="indentLevel"/> == 0</code></term>
        ///     <description>The closing character sequence will not be indented; Each line of text will be indented once.</description></item>
        /// <item><term><code><paramref name="indentLevel"/> &lt; 0</code></term>
        ///     <description>Neither the closing character sequence nor any line of text will be indented.</description></item>
        /// <item><term><code><paramref name="indentLevel"/> &gt; 0</code></term>
        ///     <description>The closing character sequence the number of times indicated by the <paramref name="indentLevel"/> value; Each line of text will be indented one level greater.</description></item>
        /// </list></para></remarks>
        protected virtual void Normalize([DisallowNull] XComment targetComment, [DisallowNull] Context context)
        {
            if (targetComment.Value.Length == 0)
            {
                if (IndentContents(targetComment))
                    targetComment.Value = " ";
                return;
            }
            string text = NonNormalizedLineSeparatorRegex.IsMatch(targetComment.Value) ? NonNormalizedLineSeparatorRegex.Replace(targetComment.Value, NewLine) : targetComment.Value;
            if ((text = text.Trim()).Length == 0)
                text = " ";
            else
            {
                Match leadingWsMatch = LeadingNewLineWithOptWs.Match(text);
                Match trailingWsMatch = TrailingNewLineWithOptWs.Match(text);
                if (EmptyOrWhiteSpaceLineRegex.IsMatch(text))
                    text = EmptyOrWhiteSpaceLineRegex.Replace(text, "");
                if (NonNormalizedWhiteSpaceRegex.IsMatch(text))
                    text = NonNormalizedWhiteSpaceRegex.Replace(text, " ");
                if (ExtraneousLineWhitespaceRegex.IsMatch(text))
                    text = ExtraneousLineWhitespaceRegex.Replace(text, "");
                if (NewLineRegex.IsMatch(text))
                {
                    IndentContents(targetComment, true);
                    SurroundWithLineBreaks(targetComment, true);
                }
                if (IndentContents(targetComment))
                {
                    string indent = (context.CurrentIndent.Length > 0) ? $"{context.CurrentIndent}{IndentString}" : (context.IndentLevel < 0) ? "" : IndentString;
                    string leadingNewLine = (leadingWsMatch.Success) ? leadingWsMatch.Groups[0].Value : NewLine;
                    string trailingNewLIne = (trailingWsMatch.Success) ? leadingWsMatch.Groups[0].Value : NewLine;
                    if (indent.Length > 0)
                    {
                        text = NewLineRegex.Replace(text, m => $"{m.Value}{indent}");
                        text = (context.CurrentIndent.Length > 0) ? $"{leadingNewLine}{indent}{text}{trailingNewLIne}{context.CurrentIndent}" : $"{leadingNewLine}{indent}{text}{trailingNewLIne}";
                    }
                    else
                        text = $"{leadingNewLine}{text}{trailingNewLIne}";
                }
                else
                    text = $" {text} ";
            }
            if (targetComment.Value != text)
                targetComment.Value = text;
        }

        protected static TextNodeCharacteristics2 GetTextNodeCharacteristics(XText textNode)
        {
            TextNodeCharacteristics2 textNodeCharacteristics = textNode.Annotation<TextNodeCharacteristics2>();
            if (textNodeCharacteristics is null)
            {
                string text = textNode.Value;
                Match leadingWsMatch = LeadingNewLineWithOptWs.Match(textNode.Value);
                Match trailingWsMatch = TrailingNewLineWithOptWs.Match(textNode.Value);
                if (leadingWsMatch.Success)
                {
                    if (trailingWsMatch.Success)
                    {
                        StringValueCharacteristics trailingNewLine = new StringValueCharacteristics(trailingWsMatch.Groups[1].Value, true);
                        if (leadingWsMatch.Index != trailingWsMatch.Index)
                        {
                            text = text.Substring(leadingWsMatch.Groups[1].Length, trailingWsMatch.Index - leadingWsMatch.Groups[1].Length);
                            textNodeCharacteristics = new TextNodeCharacteristics2(NewLineRegex.IsMatch(text), !string.IsNullOrWhiteSpace(text),
                                new StringValueCharacteristics(leadingWsMatch.Groups[1].Value, true), false, trailingNewLine, false);
                        }
                        else
                        {
                            text = text.Substring(leadingWsMatch.Groups[1].Length);
                            textNodeCharacteristics = new TextNodeCharacteristics2(NewLineRegex.IsMatch(text), !string.IsNullOrWhiteSpace(text), trailingNewLine, false, trailingNewLine, false);
                        }
                    }
                    else
                    {
                        text = text.Substring(leadingWsMatch.Groups[1].Length);
                        textNodeCharacteristics = new TextNodeCharacteristics2(NewLineRegex.IsMatch(text), !string.IsNullOrWhiteSpace(text),
                            new StringValueCharacteristics(leadingWsMatch.Groups[1].Value, true), false, null, false);
                    }
                }
                else if (trailingWsMatch.Success)
                {
                    text = text.Substring(0, trailingWsMatch.Index);
                    textNodeCharacteristics = new TextNodeCharacteristics2(NewLineRegex.IsMatch(text), !string.IsNullOrWhiteSpace(text), null, false, new StringValueCharacteristics(trailingWsMatch.Groups[1].Value, true), false);
                }
                else
                    textNodeCharacteristics = new TextNodeCharacteristics2(NewLineRegex.IsMatch(text), !string.IsNullOrWhiteSpace(text), null, false, null, false);
                textNode.AddAnnotation(textNodeCharacteristics);
            }
            return textNodeCharacteristics;
        }

        /// <summary>
        /// Normalizes the specified element.
        /// </summary>
        /// <param name="targetElement">The <see cref="XElement"/> to normalize.</param>
        /// <param name="context">The normalization <see cref="Context"/> object that includes the current indent string.</param>
        /// <remarks>This does insert any sibling nodes before or after the target <see cref="XComment"/> node (does indent the opening <c>&lt;</c> character or add a new line sequence after the outer closing <c>&gt;</c> character).
        /// <para>Elements which contain only text and do not have multiple non-blank lines are normalized as a single-line element.</para></remarks>
        protected virtual void Normalize([DisallowNull] XElement targetElement, [DisallowNull] Context context)
        {
            if (targetElement.IsEmpty)
                return;

            bool foundNewLine = false;

            for (Context childContext = context.GetFirstNodeContext(); childContext is not null; childContext = childContext.NextNode())
            {
                if (childContext.Node is XText textNode)
                {
                    if (textNode.NextNode is XText consecutiveTextNode)
                    {
                        string text = (consecutiveTextNode.Value.Length > 0) ? ((textNode.Value.Length > 0) ? $"{textNode.Value}{consecutiveTextNode.Value}" : consecutiveTextNode.Value) : textNode.Value;
                        consecutiveTextNode.Remove();
                        while (textNode.NextNode is XText t)
                        {
                            if (t.Value.Length > 0)
                                text = $"{text}{t.Value}";
                            t.Remove();
                        }
                        textNode.Value = text;
                    }
                    Normalize(textNode, childContext);
                    if (!foundNewLine)
                        foundNewLine = textNode.Annotations<TextNodeCharacteristics2>().Select(c => c.HasNewLine).DefaultIfEmpty(false).First();
                }
                else if (childContext.Node is XElement elementNode)
                {
                    Normalize(elementNode, childContext);
                    if (!foundNewLine)
                        foundNewLine = IndentContents(elementNode) || SurroundWithLineBreaks(elementNode);
                }
                else if (childContext.Node is XComment commentNode)
                {
                    Normalize(commentNode, childContext);
                    if (!foundNewLine)
                        foundNewLine = IndentContents(commentNode) || SurroundWithLineBreaks(commentNode);
                }
            }

            if (foundNewLine)
            {
                IndentContents(targetElement, true);
                SurroundWithLineBreaks(targetElement, true);
            }
            if (!IndentContents(targetElement))
                return;

            XNode currentNode = targetElement.FirstNode;
            if (currentNode is null)
            {
                targetElement.Value = " ";
                // TODO: Update TextNodeCharacteristics annotation
                return;
            }
            string indent = (context.CurrentIndent.Length > 0) ? $"{context.CurrentIndent}{IndentString}" : (context.IndentLevel < 0) ? "" : IndentString;

            if (currentNode is XText firstTextNode)
            {
                if (!(currentNode.NextNode is null && string.IsNullOrWhiteSpace(firstTextNode.Value)))
                {
                    TextNodeCharacteristics2 textNodeCharacteristics = GetTextNodeCharacteristics(firstTextNode);
                    StringValueCharacteristics trailingNewLine = textNodeCharacteristics.TrailingNewLine;
                    if (trailingNewLine is null)
                    {
                        if (indent.Length > 0)
                            firstTextNode.Value = $"{NewLine}{indent}{firstTextNode.Value}";
                        else
                            firstTextNode.Value = $"{NewLine}{firstTextNode.Value}";
                        // TODO: Update TextNodeCharacteristics annotation
                    }
                    else if (trailingNewLine.IsPresent)
                    {
                        if (!textNodeCharacteristics.IncludesTrailingIndent)
                        {
                            if (indent.Length > 0)
                                firstTextNode.Value = $"{indent}{firstTextNode.Value}";
                            // TODO: Update TextNodeCharacteristics annotation
                        }
                    }
                    else if (indent.Length > 0)
                        firstTextNode.Value = $"{trailingNewLine.Value}{indent}{firstTextNode.Value}";
                    else
                        firstTextNode.Value = $"{trailingNewLine.Value}{firstTextNode.Value}";
                }
            }
            else if (targetElement.FirstNode is not null)
            {
                firstTextNode = new XText((indent.Length > 0) ? $"{NewLine}{indent}" : NewLine);
                // TODO: Add TextNodeCharacteristics annotation
                targetElement.FirstNode.AddBeforeSelf(firstTextNode);
            }

            for (XNode nextNode = currentNode.NextNode; nextNode is not null; nextNode = nextNode.NextNode)
            {
                // TODO: Prepend newline and indent to nextNode
                currentNode = nextNode;
            }

            if (currentNode is XText lastTextNode)
            {
                // TODO: Append NewLine and CurrentIndent to lastTextNode
            }
            else
            {
                lastTextNode = new XText((context.CurrentIndent.Length > 0) ? $"{NewLine}{context.CurrentIndent}" : NewLine);
                targetElement.Add(lastTextNode);
            }
        }

        /// <summary>
        /// Normalizes the specified document.
        /// </summary>
        /// <param name="document">The <see cref="XDocument"/> to normalize.</param>
        /// <param name="context">The normalization <see cref="Context"/> object that includes the initial indent string.</param>
        protected virtual void Normalize([DisallowNull] XDocument document, [DisallowNull] Context context)
        {
            for (Context childContext = context.GetFirstNodeContext(); childContext is not null; childContext = childContext.NextNode())
            {
                if (childContext.Node is XText textNode)
                {
                    if (textNode.NextNode is XText consecutiveTextNode)
                    {
                        string text = (consecutiveTextNode.Value.Length > 0) ? ((textNode.Value.Length > 0) ? $"{textNode.Value}{consecutiveTextNode.Value}" : consecutiveTextNode.Value) : textNode.Value;
                        consecutiveTextNode.Remove();
                        while (textNode.NextNode is XText t)
                        {
                            if (t.Value.Length > 0)
                                text = $"{text}{t.Value}";
                            t.Remove();
                        }
                        textNode.Value = text;
                    }
                    Normalize(textNode, childContext);
                }
                else if (childContext.Node is XElement elementNode)
                    Normalize(elementNode, childContext);
                else if (childContext.Node is XComment commentNode)
                    Normalize(commentNode, childContext);
            }

            while (document.FirstNode is XText leadingTextNode)
                leadingTextNode.Remove();
            XNode currentNode = document.FirstNode;
            if (currentNode is null)
                return;
            string currentNewLine = NewLine;
            while (currentNode.NextNode is XText initialTextNode)
            {
                TextNodeCharacteristics2 textNodeCharacteristics = GetTextNodeCharacteristics(initialTextNode);
                StringValueCharacteristics trailingNewLine = textNodeCharacteristics.TrailingNewLine;
                if (trailingNewLine is null)
                    currentNewLine = (textNodeCharacteristics.LeadingNewLine is null) ? NewLine : textNodeCharacteristics.LeadingNewLine.Value;
                else
                    currentNewLine = trailingNewLine.Value;
                initialTextNode.Remove();
            }
            if (currentNode.NextNode is null)
                return;
            if (context.CurrentIndent.Length > 0)
            {
                currentNode.AddBeforeSelf(new XText(context.CurrentIndent));
                for (XNode nextNode = currentNode.NextNode; nextNode is not null; nextNode = nextNode.NextNode)
                {
                    string nextNewLine = NewLine;
                    while (nextNode.NextNode is XText nextTextNode)
                    {
                        TextNodeCharacteristics2 textNodeCharacteristics = GetTextNodeCharacteristics(nextTextNode);
                        StringValueCharacteristics trailingNewLine = textNodeCharacteristics.TrailingNewLine;
                        if (trailingNewLine is null)
                            nextNewLine = (textNodeCharacteristics.LeadingNewLine is null) ? NewLine : textNodeCharacteristics.LeadingNewLine.Value;
                        else
                            nextNewLine = trailingNewLine.Value;
                        nextTextNode.Remove();
                    }
                    currentNode.AddAfterSelf(new XText($"{currentNewLine}{context.CurrentIndent}"));
                    currentNode = nextNode;
                    currentNewLine = nextNewLine;
                }
            }
            else for (XNode nextNode = currentNode.NextNode; nextNode is not null; nextNode = nextNode.NextNode)
                {
                    string nextNewLine = NewLine;
                    while (nextNode.NextNode is XText nextTextNode)
                    {
                        TextNodeCharacteristics2 textNodeCharacteristics = GetTextNodeCharacteristics(nextTextNode);
                        StringValueCharacteristics trailingNewLine = textNodeCharacteristics.TrailingNewLine;
                        if (trailingNewLine is null)
                            nextNewLine = (textNodeCharacteristics.LeadingNewLine is null) ? NewLine : textNodeCharacteristics.LeadingNewLine.Value;
                        else
                            nextNewLine = trailingNewLine.Value;
                        nextTextNode.Remove();
                    }
                    currentNode.AddAfterSelf(new XText(currentNewLine));
                    currentNode = nextNode;
                    currentNewLine = nextNewLine;
                }
            while (document.LastNode is XText lastTextNode)
                lastTextNode.Remove();
        }

        protected static void IndentContents(XElement elementNode, bool value)
        {
            LineBreakCharacteristics annotation = elementNode.Annotation<LineBreakCharacteristics>();
            if (annotation is null)
                elementNode.AddAnnotation(new LineBreakCharacteristics(false, value));
            else if (annotation.IndentContents != value)
            {
                elementNode.RemoveAnnotations<LineBreakCharacteristics>();
                elementNode.AddAnnotation(annotation with { IndentContents = value });
            }
        }

        protected static void IndentContents(XComment commentNode, bool value)
        {
            LineBreakCharacteristics annotation = commentNode.Annotation<LineBreakCharacteristics>();
            if (annotation is null)
                commentNode.AddAnnotation(new LineBreakCharacteristics(false, value));
            else if (annotation.IndentContents != value)
            {
                commentNode.RemoveAnnotations<LineBreakCharacteristics>();
                commentNode.AddAnnotation(annotation with { IndentContents = value });
            }
        }

        protected static bool IndentContents(XElement elementNode) => elementNode.Annotations<LineBreakCharacteristics>().Select(a => a.IndentContents).DefaultIfEmpty(false).First();

        protected static bool IndentContents(XComment commentNode) => commentNode.Annotations<LineBreakCharacteristics>().Select(a => a.IndentContents).DefaultIfEmpty(false).First();

        protected static void SurroundWithLineBreaks(XElement elementNode, bool value)
        {
            LineBreakCharacteristics annotation = elementNode.Annotation<LineBreakCharacteristics>();
            if (annotation is null)
                elementNode.AddAnnotation(new LineBreakCharacteristics(value, false));
            else if (annotation.SurroundWithLineBreaks != value)
            {
                elementNode.RemoveAnnotations<LineBreakCharacteristics>();
                elementNode.AddAnnotation(annotation with { SurroundWithLineBreaks = value });
            }
        }

        protected static void SurroundWithLineBreaks(XComment commentNode, bool value)
        {
            throw new NotImplementedException();
        }

        protected static bool SurroundWithLineBreaks(XElement elementNode) => elementNode.Annotations<LineBreakCharacteristics>().Select(a => a.SurroundWithLineBreaks).DefaultIfEmpty(false).First();

        protected static bool SurroundWithLineBreaks(XComment commentNode) => commentNode.Annotations<LineBreakCharacteristics>().Select(a => a.SurroundWithLineBreaks).DefaultIfEmpty(false).First();

        record LineBreakCharacteristics(bool SurroundWithLineBreaks, bool IndentContents);

        protected record StringValueCharacteristics(string Value, bool IsPresent);

        protected record TextNodeCharacteristics2(bool HasNewLine, bool HasNonWhiteSpace, StringValueCharacteristics LeadingNewLine, bool IncludesLeadingIndent, StringValueCharacteristics TrailingNewLine,
            bool IncludesTrailingIndent);

        public TextNodeCharacteristics GetTextNodeCharacteristics([DisallowNull] XText textNode, bool preferNotPresent = false, bool forceRefresh = false)
        {
            if (textNode is null)
                throw new ArgumentNullException(nameof(textNode));
            TextNodeCharacteristics result, current = textNode.Annotation<TextNodeCharacteristics>();
            if (!(current is null || forceRefresh))
                return current;
            LineExtent start, end;
            string text = textNode.Value;
            if (text.Length > 0)
            {
                if (NonNormalizedLineSeparatorRegex.IsMatch(text))
                    text = NonNormalizedLineSeparatorRegex.Replace(text, NewLine);
                Match trailingMatch = TrailingNewLineAndOrWhiteSpace.Match(text);
                if (trailingMatch.Success)
                {
                    end = trailingMatch.Groups["n"].Success ? new LineExtent
                    {
                        IsCrLf = new OptionalValue<bool> { Value = trailingMatch.Groups["n"].Length > 1, IsPresent = !preferNotPresent },
                        WhiteSpace = trailingMatch.Groups["w"].Success ? new OptionalValue<string> { Value = trailingMatch.Groups["w"].Value, IsPresent = !preferNotPresent } : null
                    } : new LineExtent { IsCrLf = null, WhiteSpace = new OptionalValue<string> { Value = trailingMatch.Groups["w"].Value, IsPresent = !preferNotPresent } };
                    if (trailingMatch.Index == 0)
                    {
                        start = end;
                        text = "";
                    }
                    else
                    {
                        text = text.Substring(0, trailingMatch.Index);
                        Match leadingMatch = LeadingNewLineAndOrWhiteSpace.Match(text);
                        if (leadingMatch.Success)
                        {
                            start = leadingMatch.Groups["n"].Success ? new LineExtent
                            {
                                IsCrLf = new OptionalValue<bool> { Value = leadingMatch.Groups["n"].Length > 1, IsPresent = !preferNotPresent },
                                WhiteSpace = leadingMatch.Groups["w"].Success ? new OptionalValue<string> { Value = leadingMatch.Groups["w"].Value, IsPresent = !preferNotPresent } : null
                            } : new LineExtent { IsCrLf = null, WhiteSpace = new OptionalValue<string> { Value = leadingMatch.Groups["w"].Value, IsPresent = !preferNotPresent } };
                            text = (leadingMatch.Length < text.Length) ? text.Substring(leadingMatch.Length) : "";
                        }
                        else
                            start = null;
                    }
                }
                else
                {
                    end = null;
                    Match leadingMatch = LeadingNewLineAndOrWhiteSpace.Match(text);
                    if (leadingMatch.Success)
                    {
                        start = leadingMatch.Groups["n"].Success ? new LineExtent
                        {
                            IsCrLf = new OptionalValue<bool> { Value = leadingMatch.Groups["n"].Length > 1, IsPresent = !preferNotPresent },
                            WhiteSpace = leadingMatch.Groups["w"].Success ? new OptionalValue<string> { Value = leadingMatch.Groups["w"].Value, IsPresent = !preferNotPresent } : null
                        } : new LineExtent { IsCrLf = null, WhiteSpace = new OptionalValue<string> { Value = leadingMatch.Groups["w"].Value, IsPresent = !preferNotPresent } };
                        text = (leadingMatch.Length < text.Length) ? text.Substring(leadingMatch.Length) : "";
                    }
                    else
                        start = null;
                }
            }
            else
                start = end = null;
            if (text.Length > 0)
            {
                if ((text = text.Trim()).Length > 0)
                {
                    if (EmptyOrWhiteSpaceLineRegex.IsMatch(text))
                        text = NonNormalizedLineSeparatorRegex.Replace(text, "");
                    if (NonNormalizedWhiteSpaceRegex.IsMatch(text))
                        text = NonNormalizedWhiteSpaceRegex.Replace(text, " ");
                    result = (current is null) ? new TextNodeCharacteristics
                    {
                        HasNonWhiteSpace = true,
                        IsMultiLine = NewLineRegex.IsMatch(text),
                        Start = start,
                        End = end
                    } : new TextNodeCharacteristics
                    {
                        HasNonWhiteSpace = true,
                        IsMultiLine = NewLineRegex.IsMatch(text),
                        Start = LineExtent.Merge(start, current.Start, preferNotPresent),
                        End = LineExtent.Merge(end, current.End, preferNotPresent)
                    };
                }
                else
                {
                    text = " ";
                    result = (current is null) ? new TextNodeCharacteristics
                    {
                        HasNonWhiteSpace = false,
                        IsMultiLine = true,
                        Start = start,
                        End = end
                    } : new TextNodeCharacteristics
                    {
                        HasNonWhiteSpace = false,
                        IsMultiLine = true,
                        Start = LineExtent.Merge(start, current.Start, preferNotPresent),
                        End = LineExtent.Merge(end, current.End, preferNotPresent)
                    };
                }
            }
            else
                result = (current is null) ? new TextNodeCharacteristics
                {
                    HasNonWhiteSpace = false,
                    IsMultiLine = true,
                    Start = start,
                    End = end
                } : new TextNodeCharacteristics
                {
                    HasNonWhiteSpace = false,
                    IsMultiLine = true,
                    Start = LineExtent.Merge(start, current.Start, preferNotPresent),
                    End = LineExtent.Merge(end, current.End, preferNotPresent)
                };
            if (current is not null)
                textNode.RemoveAnnotations<TextNodeCharacteristics>();
            textNode.AddAnnotation(result);
            if (result.Start?.WhiteSpace?.IsPresent ?? false)
                text = $"{result.Start.WhiteSpace?.Value}{text}";
            if (result.Start?.IsCrLf?.IsPresent ?? false)
                text = $"{((result.Start.IsCrLf?.Value ?? false) ? "\r\n" : "\n")}{text}";
            if (result.End?.WhiteSpace?.IsPresent ?? false)
                text = $"{text}{result.End.WhiteSpace?.Value}";
            if (result.End?.IsCrLf?.IsPresent ?? false)
                text = $"{text}{((result.End.IsCrLf?.Value ?? false) ? "\r\n" : "\n")}";
            if (textNode.Value != text)
                textNode.Value = text;
            return result;
        }
    }

    public record OptionalValue<T>
    {
        public T Value { get; init; }
        public bool IsPresent { get; init; }
        public static OptionalValue<T> Merge(OptionalValue<T> x, OptionalValue<T> y, bool preferNotPresent = false) => (x is null) ? y :
            (y is null || x.Equals(y) || (preferNotPresent ? (!x.IsPresent || y.IsPresent) : (x.IsPresent || !y.IsPresent))) ? x : x with { IsPresent = !preferNotPresent };
    }

    public record LineExtent
    {
        public OptionalValue<bool> IsCrLf { get; init; }
        public OptionalValue<string> WhiteSpace { get; init; }
        public static LineExtent Merge(LineExtent x, LineExtent y, bool preferNotPresent = false) => (x is null) ? y : (y is null || x.Equals(y)) ? x : new LineExtent
        {
            IsCrLf = OptionalValue<bool>.Merge(x.IsCrLf, y.IsCrLf, preferNotPresent),
            WhiteSpace = OptionalValue<string>.Merge(x.WhiteSpace, y.WhiteSpace, preferNotPresent)
        };
    }

    public record TextNodeCharacteristics
    {
        public bool IsMultiLine { get; init; }
        public bool HasNonWhiteSpace { get; init; }
        public LineExtent Start { get; init; }
        public LineExtent End { get; init; }
        public static TextNodeCharacteristics Merge(TextNodeCharacteristics x, TextNodeCharacteristics y, bool preferNotPresent = false) => (x is null) ? y : (y is null || x.Equals(y) ||
            (((x.Start is null) ? y.Start is null : x.Start.Equals(y.Start)) && ((x.End is null) ? y.End is null : x.End.Equals(y.End)))) ? x : x with
            {
                Start = LineExtent.Merge(x.Start, y.Start, preferNotPresent),
                End = LineExtent.Merge(x.End, y.End, preferNotPresent)
            };
    }

    public record TagNodeCharacteristics
    {
        public bool IndentContents { get; init; }
        public LineExtent Before { get; init; }
        public LineExtent After { get; init; }
        public static TagNodeCharacteristics Merge(TagNodeCharacteristics x, TagNodeCharacteristics y, bool preferNotPresent = false) => (x is null) ? y : (y is null || x.Equals(y)) ? x : new TagNodeCharacteristics
        {
            IndentContents = x.IndentContents || y.IndentContents,
            Before = LineExtent.Merge(x.Before, y.Before, preferNotPresent),
            After = LineExtent.Merge(x.After, y.After, preferNotPresent)
        };
    }
}
