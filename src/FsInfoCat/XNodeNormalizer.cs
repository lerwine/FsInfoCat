using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FsInfoCat
{
    public class XNodeNormalizer
    {
        public const string DefaultNewLine = @"
";
        public const string DefaultIndent = "    ";
        public static readonly Regex NonNormalizedLineSeparatorRegex = new(@"\r(?!\n)|[\x00-\b\v\f\x0e-\x1f\p{Zl}\p{Zp}]", RegexOptions.Compiled);
        public static readonly Regex NonNormalizedWhiteSpaceRegex = new(@" [^\r\n\S]+|[^\r\n\S ][^\r\n\S]*", RegexOptions.Compiled);
        public static readonly Regex ExtraneousLineWhitespaceRegex = new(@"(?<=\r\n?|\n|^)[^\r\n\S]+|[^\r\n\S]+(?=\r\n?|\n|$)", RegexOptions.Compiled);
        public static readonly Regex ExtraneousInnerLineWSRegex = new(@"(?<=\r\n?|\n|^)[^\r\n\S]+|[^\r\n\S]+(?=\r\n?|\n|$)", RegexOptions.Compiled);
        public static readonly Regex TrailingInnerLineWsRegex = new(@"(?<=[\S\r\n])[^\r\n\S]+(?=\r\n?|\n)", RegexOptions.Compiled);
        public static readonly Regex LineStartWsRegex = new(@"(\r\n?|\n|^)[^\r\n\S]*", RegexOptions.Compiled);
        public static readonly Regex TrailingNewLineWithOptWs = new(@"(\r\n?|\n)([^\r\n\S]+)?$", RegexOptions.Compiled);
        public static readonly Regex LineSeparatorCharRegex = new(@"[\x00-\b\n-\x1f\p{Zl}\p{Zp}]", RegexOptions.Compiled);

        //static readonly Regex BlankInnerLineRegex = new(@"(\r\n?|\n)((?![\r\n])\s)*(?=\r\n?|\n)", RegexOptions.Compiled);
        //static readonly Regex BlankLinesOnlyRegex = new(@"^((?![\r\n])\s)*((\r\n?|\n)((?![\r\n])\s)*)+$", RegexOptions.Compiled);
        //static readonly Regex BlankLeadingLinesRegex = new(@"^(((?![\r\n])\s)*(\r\n?|\n))+", RegexOptions.Compiled);
        //static readonly Regex BlankTrailingLinesRegex = new(@"((\r\n?|\n)((?![\r\n])\s)*)+$", RegexOptions.Compiled);
        // \r\nabc123
        public static readonly Regex EmptyOrWhiteSpaceLineRegex = new(@"^(([^\r\n\S]*(\r\n?|\n))+([^\r\n\S]+$)?|[^\r\n\S]+$)|(\r\n?|\n)[^\r\n\S]*((?=[\r\n])|$)", RegexOptions.Compiled);

        //public static readonly Regex LineSeparatorOrNonWsRegex = new(@"\r\n?|[\x00-\b\n-\x1f\p{Zl}\p{Zp}\S]", RegexOptions.Compiled);
        //public static readonly Regex LineSeparatorRegex = new(@"\r\n?|[\x00-\b\n-\x1f\p{Zl}\p{Zp}]", RegexOptions.Compiled);
        public static readonly Regex NewLineRegex = new(@"\r\n?|\n", RegexOptions.Compiled);
        //public static readonly Regex IndentWsRegex = new(@"(?:^|(\r\n?|[\x00-\b\n-\x1f\p{Zl}\p{Zp}]))[\t\p{Zs}]*(?=\S)", RegexOptions.Compiled);
        //public static readonly Regex InnerNewLineIndentRegex = new(@"(\r\n?|[\x00-\b\n-\x1f\p{Zl}\p{Zp}])[\t\p{Zs}]*(?=\S)", RegexOptions.Compiled);
        protected virtual string IndentString => DefaultIndent;
        protected virtual string NewLine => DefaultNewLine;
        public static void Normalize(XComment comment, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(comment ?? throw new ArgumentNullException(nameof(comment)), indentLevel);
        public static void Normalize(XContainer container, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(container ?? throw new ArgumentNullException(nameof(container)), indentLevel);
        public static void Normalize(XText text, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).PrivateNormalize(text ?? throw new ArgumentNullException(nameof(text)), indentLevel);
        public static void Normalize(XComment comment, int indentLevel) => Normalize(comment, null, indentLevel);
        public static void Normalize(XContainer container, int indentLevel) => Normalize(container, null, indentLevel);
        public static void Normalize(XText text, int indentLevel) => Normalize(text, null, indentLevel);
        private void PrivateNormalize(XComment comment, int indentLevel) => Normalize(comment, new Context(indentLevel, comment, this));
        private void PrivateNormalize(XContainer container, int indentLevel) => Normalize(container, new Context(indentLevel, container, this));
        private void PrivateNormalize(XText text, int indentLevel) => Normalize(text, new Context(indentLevel, text, this));
        protected virtual void Normalize(XComment comment, Context context)
        {
            if (comment.Value.Length == 0)
                return;
            string text = NonNormalizedLineSeparatorRegex.IsMatch(comment.Value) ? NonNormalizedLineSeparatorRegex.Replace(comment.Value, NewLine) : comment.Value;
            bool hasNewLine = NewLineRegex.IsMatch(text);
            if (EmptyOrWhiteSpaceLineRegex.IsMatch(text) && (text = EmptyOrWhiteSpaceLineRegex.Replace(text, "")).Length == 0)
                text = " ";
            else
            {
                if (NonNormalizedWhiteSpaceRegex.IsMatch(text))
                    text = NonNormalizedWhiteSpaceRegex.Replace(text, " ");
                if (ExtraneousLineWhitespaceRegex.IsMatch(text))
                    text = ExtraneousLineWhitespaceRegex.Replace(text, "");
                if (hasNewLine)
                {
                    if (IndentString.Length < 1 || context.IndentLevel < 0)
                        text = $"{NewLine}{text}{NewLine}";
                    else if (context.CurrentIndent.Length > 0)
                    {
                        string indent = $"{context.CurrentIndent}{IndentString}";
                        text = $"{NewLine}{indent}{NewLineRegex.Replace(text, m => $"{m.Value}{indent}")}{NewLine}{context.CurrentIndent}";
                    }
                    else
                        text = $"{NewLine}{IndentString}{NewLineRegex.Replace(text, m => $"{m.Value}{IndentString}")}{NewLine}";
                }
            }
            if (comment.Value != text)
                comment.Value = text;
        }
        protected virtual void Normalize(XContainer container, Context context)
        {
            if (container is XElement el && el.IsEmpty)
                return;
            bool hasNewLine = container.DescendantNodes().OfType<XText>().Any(t => t.Value.Length > 0);
            if (hasNewLine)
            {
                hasNewLine = container.DescendantNodes().OfType<XText>().Any(t => LineSeparatorCharRegex.IsMatch(t.Value));
                while (container.FirstNode is XText leadingNode)
                {
                    string t = leadingNode.Value.TrimStart();
                    if (t.Length > 0)
                    {
                        if (leadingNode.Value != t)
                            leadingNode.Value = t;
                        break;
                    }
                    leadingNode.Remove();
                }
                while (container.LastNode is XText trailingNode)
                {
                    string t = trailingNode.Value.TrimEnd();
                    if (t.Length > 0)
                    {
                        if (trailingNode.Value != t)
                            trailingNode.Value = t;
                        break;
                    }
                    trailingNode.Remove();
                }
                if (container is XElement element && element.IsEmpty)
                {
                    element.Value = " ";
                    return;
                }
                XText textNode;
                if (container.FirstNode is not XText && container.FirstNode is not XContainer)
                {
                    IEnumerable<XText> leadingTextNodes = container.Nodes().TakeWhile(n => n is not XContainer).OfType<XText>().Where(n => n.Value.Length > 0);
                    while ((textNode = leadingTextNodes.FirstOrDefault()) is not null)
                    {
                        string t = textNode.Value.TrimEnd();
                        if (t.Length > 0)
                        {
                            textNode.Value = t;
                            break;
                        }
                    }
                }
                if (container.LastNode is not XText && container.LastNode is not XContainer)
                {
                    IEnumerable<XText> trailingTextNodes = container.Nodes().Reverse().TakeWhile(n => n is not XContainer).OfType<XText>().Where(n => n.Value.Length > 0);
                    while ((textNode = trailingTextNodes.FirstOrDefault()) is not null)
                    {
                        string t = textNode.Value.TrimEnd();
                        if (t.Length > 0)
                        {
                            textNode.Value = t;
                            break;
                        }
                    }
                }
            }
            else
            {
                while (container.FirstNode is XText leadingNode)
                    leadingNode.Remove();
                while (container.LastNode is XText trailingNode)
                    trailingNode.Remove();
                if (container is XElement element && element.IsEmpty)
                {
                    element.Value = "";
                    return;
                }
            }

            Context childContext = context.FirstNode();
            while (childContext is not null)
            {
                if (childContext.Node is XText text)
                {
                    Context nextContext = childContext.NextNode();
                    while (nextContext is not null && nextContext.Node is XText toMerge)
                    {
                        if (toMerge is XCData && text is not XCData)
                        {
                            if (text.Value.Length == 0)
                                nextContext = (childContext = childContext.Remove()).NextNode();
                            else
                            {
                                text = new XCData($"{text.Value}{toMerge.Value}");
                                childContext = childContext.ReplaceWith(text);
                                nextContext = nextContext.Remove();
                            }
                        }
                        else
                        {
                            if (toMerge.Value.Length > 0)
                                text.Value = $"{text.Value}{toMerge.Value}";
                            nextContext = nextContext.Remove();
                        }
                    }
                    Normalize(text, hasNewLine ? childContext.Indented() : childContext);
                    childContext = nextContext;
                }
                else
                {
                    if (childContext.Node is XElement childElement)
                    {
                        Normalize(childElement, childContext);
                        if (ShouldForceLineBreak(childElement, context))
                        {
                            if (childElement.PreviousNode is XText prededingText)
                            {
                                Match match = TrailingNewLineWithOptWs.Match(prededingText.Value);
                                if (match.Success)
                                {
                                    if (!match.Groups[2].Success || match.Groups[2].Value != context.CurrentIndent)
                                    {
                                        if (context.CurrentIndent.Length > 0)
                                            prededingText.Value = TrailingNewLineWithOptWs.Replace(prededingText.Value, m => $"{m.Groups[1].Value}{context.CurrentIndent}");
                                        else
                                            prededingText.Value = TrailingNewLineWithOptWs.Replace(prededingText.Value, m => m.Groups[1].Value);
                                    }
                                }
                                else
                                    prededingText.Value = (context.CurrentIndent.Length > 0) ? $"{prededingText.Value}{NewLine}{context.CurrentIndent}" : $"{prededingText.Value}{NewLine}";
                            }
                            else
                                childElement.AddBeforeSelf(new XText((context.CurrentIndent.Length > 0) ? $"{NewLine}{context.CurrentIndent}" : NewLine));
                        }
                    }
                    else if (childContext.Node is XComment comment)
                        Normalize(comment, childContext);
                    childContext = childContext.NextNode();
                }
            }
        }

        protected virtual bool ShouldForceLineBreak(XElement element, Context parentContext) => element.DescendantNodes().OfType<XText>().Any(t => NewLineRegex.IsMatch(t.Value));

        protected virtual void Normalize(XText textNode, Context context)
        {
            bool hasPrecedingContent = textNode.NodesBeforeSelf().OfType<XContainer>().Any() || textNode.NodesBeforeSelf().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));
            bool hasFollowingContent = textNode.NodesAfterSelf().OfType<XContainer>().Any() || textNode.NodesAfterSelf().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));
            string text = hasPrecedingContent ? (hasFollowingContent ? textNode.Value : textNode.Value.TrimEnd()) : hasFollowingContent ? textNode.Value.TrimStart() : textNode.Value.Trim();
            if (text.Length > 0)
            {
                if (NonNormalizedLineSeparatorRegex.IsMatch(textNode.Value))
                    text = NonNormalizedLineSeparatorRegex.Replace(text, NewLine);
                if (EmptyOrWhiteSpaceLineRegex.IsMatch(text) && (text = EmptyOrWhiteSpaceLineRegex.Replace(text, "")).Length == 0)
                {
                    if (hasPrecedingContent && hasFollowingContent)
                        text = " ";
                }
                else
                {
                    if (NonNormalizedWhiteSpaceRegex.IsMatch(text))
                        text = NonNormalizedWhiteSpaceRegex.Replace(text, " ");
                    if (ExtraneousLineWhitespaceRegex.IsMatch(text))
                        text = ExtraneousLineWhitespaceRegex.Replace(text, " ");
                    if (NewLineRegex.IsMatch(text) && context.CurrentIndent.Length > 0)
                        text = NewLineRegex.Replace(text, m => $"{m.Value}{context.CurrentIndent}");
                }
            }
            if (textNode.Value != text)
                textNode.Value = text;
        }
        protected class Context
        {
            private readonly XNodeNormalizer _normalizer;
            internal Context Parent { get; }
            public int IndentLevel { get; }
            public int Depth { get; }
            public string CurrentIndent { get; }
            public XNode Node { get; }
            public XNode ParentNode { get; }
            internal Context(int indentLevel, XNode node, XNodeNormalizer normalizer) : this(indentLevel, 0, node ?? throw new ArgumentNullException(nameof(node)),
                normalizer ?? throw new ArgumentNullException(nameof(normalizer))) { }
            private Context(Context context, [DisallowNull] XNode node)
            {
                _normalizer = context._normalizer;
                IndentLevel = context.IndentLevel;
                Depth = context.Depth;
                CurrentIndent = context.CurrentIndent;
                ParentNode = (Node = node)?.Parent;
            }
            private Context(int indentLevel, int depth, [DisallowNull] XNode node, [DisallowNull] XNodeNormalizer normalizer)
            {
                _normalizer = normalizer;
                IndentLevel = indentLevel;
                Depth = depth;
                string indent = normalizer.IndentString;
                CurrentIndent = (string.IsNullOrEmpty(indent) || indentLevel < 1) ? "" : (indentLevel == 1) ? indent : string.Join("", Enumerable.Repeat(indent, indentLevel));
                ParentNode = (Node = node)?.Parent;
            }
            internal Context FirstNode()
            {
                if (Node is XContainer container)
                {
                    XNode node = container.FirstNode;
                    if (node is not null)
                        return (container is XDocument) ? new Context(IndentLevel, Depth + 1, node, _normalizer) : new Context(IndentLevel + 1, Depth + 1, node, _normalizer);
                }
                return null;
            }
            internal Context NextNode()
            {
                XNode node = Node.NextNode;
                return (node is null) ? null : new Context(this, node);
            }
            internal Context Remove()
            {
                XNode node = Node.NextNode;
                Node.Remove();
                return (node is null) ? null : new Context(this, node);
            }
            internal Context ReplaceWith(XNode node)
            {
                Node.ReplaceWith(node);
                return new Context(this, node);
            }

            internal Context Indented() => new Context(IndentLevel + 1, Depth, Node, _normalizer);
        }
    }

}
