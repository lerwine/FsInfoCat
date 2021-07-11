using System;
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
        public static readonly Regex ExtraneousLineWhitespaceRegex = new(@"(?<=[\r\n]|^)[^\r\n\S]+|[^\r\n\S]+(?=[\r\n]|$)", RegexOptions.Compiled);
        public static readonly Regex ExtraneousInnerLineWSRegex = new(@"(?<=[\r\n\S])[^\r\n\S]+(?=[\r\n])|(?<=[\r\n])[^\r\n\S]+(?=\S)", RegexOptions.Compiled);
        //public static readonly Regex TrailingInnerLineWsRegex = new(@"(?<=[\S\r\n])[^\r\n\S]+(?=\r\n?|\n)", RegexOptions.Compiled);
        //public static readonly Regex LineStartWsRegex = new(@"(\r\n?|\n|^)[^\r\n\S]*", RegexOptions.Compiled);
        public static readonly Regex LeadingNewLineWithOptWs = new(@"^(\r\n?|\n)([^\r\n\S]+)?", RegexOptions.Compiled);
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
        public static void Normalize(XComment comment, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).BaseNormalize(comment ?? throw new ArgumentNullException(nameof(comment)), indentLevel);
        public static void Normalize(XContainer container, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).BaseNormalize(container ?? throw new ArgumentNullException(nameof(container)), indentLevel);
        public static void Normalize(XText text, XNodeNormalizer normalizer = null, int indentLevel = 0) => (normalizer ?? new()).BaseNormalize(text ?? throw new ArgumentNullException(nameof(text)), indentLevel);
        public static void Normalize(XComment comment, int indentLevel) => Normalize(comment, null, indentLevel);
        public static void Normalize(XContainer container, int indentLevel) => Normalize(container, null, indentLevel);
        public static void Normalize(XText text, int indentLevel) => Normalize(text, null, indentLevel);
        protected virtual void BaseNormalize(XComment comment, int indentLevel) => Normalize(comment, new Context(indentLevel, comment, this));
        protected virtual void BaseNormalize(XContainer container, int indentLevel)
        {
            if (container is XDocument document)
                Normalize(document.Root, new Context(indentLevel, document.Root, this));
            else
                Normalize((XElement)container, new Context(indentLevel, container, this));
        }
        protected virtual void BaseNormalize(XText text, int indentLevel) => Normalize(text, new Context(indentLevel, text, this));

        record NonWhiteSpaceCharacteristics(bool ContainsNewLine);

        record OuterLineBreakCharacteristics(bool SurroundWithLineBreaks);

        record LeadingNewLinex(string Value, bool? IncludesIndent);

        record TrailingNewLinex(string Value, bool? IncludesIndent);

        protected virtual bool ShouldForceLineBreak(XElement element, Context context) => element.DescendantNodes().OfType<XText>().Any(t => NewLineRegex.IsMatch(t.Value)) ||
            element.DescendantNodes().OfType<XComment>().Any(t => NewLineRegex.IsMatch(t.Value));

        protected virtual void Normalize(XComment comment, Context context)
        {
            comment.RemoveAnnotations<OuterLineBreakCharacteristics>();
            if (comment.Value.Length == 0)
            {
                comment.AddAnnotation(new OuterLineBreakCharacteristics(false));
                return;
            }
            string text = NonNormalizedLineSeparatorRegex.IsMatch(comment.Value) ? NonNormalizedLineSeparatorRegex.Replace(comment.Value, NewLine) : comment.Value;
            if ((text = text.Trim()).Length == 0)
            {
                comment.AddAnnotation(new OuterLineBreakCharacteristics(false));
                text = " ";
            }
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
                    string leadingNewLine = (leadingWsMatch.Success) ? leadingWsMatch.Groups[0].Value : NewLine;
                    string trailingNewLIne = (trailingWsMatch.Success) ? leadingWsMatch.Groups[0].Value : NewLine;
                    comment.AddAnnotation(new OuterLineBreakCharacteristics(true));
                    if (context.CurrentIndent.Length > 0)
                    {
                        text = NewLineRegex.Replace(text, m => $"{m.Value}{context.CurrentIndent}");
                        string indent = context.GetParentIndent();
                        text = (indent.Length > 0) ? $"{leadingNewLine}{context.CurrentIndent}{text}{trailingNewLIne}{indent}" : $"{leadingNewLine}{context.CurrentIndent}{text}{trailingNewLIne}";
                    }
                    else
                        text = $"{leadingNewLine}{text}{trailingNewLIne}";
                }
                else
                {
                    comment.AddAnnotation(new OuterLineBreakCharacteristics(false));
                    text = $" {text} ";
                }
            }
            if (comment.Value != text)
                comment.Value = text;
        }

        protected virtual void Normalize(XElement container, Context context)
        {
            if (container.IsEmpty)
                return;

            bool foundNewLine = false;

            for (Context childContext = context.FirstNode(); childContext is not null; childContext = childContext.NextNode())
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
                        foundNewLine = textNode.Annotations<NonWhiteSpaceCharacteristics>().Select(c => c.ContainsNewLine).DefaultIfEmpty(false).First();
                }
                else if (childContext.Node is XElement elementNode)
                {
                    Normalize(elementNode, childContext);
                    if (!foundNewLine)
                        foundNewLine = elementNode.Annotations<OuterLineBreakCharacteristics>().Select(c => c.SurroundWithLineBreaks).DefaultIfEmpty(false).First();
                }
                else if (childContext.Node is XComment commentNode)
                {
                    Normalize(commentNode, childContext);
                    if (!foundNewLine)
                        foundNewLine = commentNode.Annotations<OuterLineBreakCharacteristics>().Select(c => c.SurroundWithLineBreaks).DefaultIfEmpty(false).First();
                }
            }

            if (foundNewLine || ShouldForceLineBreak(container, context))
            {
                container.RemoveAnnotations<OuterLineBreakCharacteristics>();
                container.AddAnnotation(new OuterLineBreakCharacteristics(true));
                for (XNode node = container.FirstNode; node is not null; node = node.NextNode)
                {
                    if (node is not XText && node.Annotations<OuterLineBreakCharacteristics>().Select(c => c.SurroundWithLineBreaks).DefaultIfEmpty(false).First())
                    {
                        if (node.PreviousNode is XText previousTextNode)
                        {
                            TrailingNewLinex trailingNewLine = previousTextNode.Annotations<TrailingNewLinex>().FirstOrDefault();
                            if (trailingNewLine is null)
                            {
                                if (string.IsNullOrWhiteSpace(previousTextNode.Value))
                                    previousTextNode.Value = (context.CurrentIndent.Length > 0) ? $"{NewLine}{context.CurrentIndent}" : NewLine;
                                else
                                    previousTextNode.Value = (context.CurrentIndent.Length > 0) ? $"{previousTextNode.Value}{NewLine}{context.CurrentIndent}" : $"{previousTextNode.Value}{NewLine}";
                            }
                            else if (trailingNewLine.IncludesIndent.HasValue)
                            {
                                if (!trailingNewLine.IncludesIndent.Value && context.CurrentIndent.Length > 0)
                                    previousTextNode.Value = $"{previousTextNode.Value}{context.CurrentIndent}";
                            }
                            else if (string.IsNullOrWhiteSpace(previousTextNode.Value))
                                previousTextNode.Value = (context.CurrentIndent.Length > 0) ? $"{trailingNewLine.Value}{context.CurrentIndent}" : trailingNewLine.Value;
                            else
                                previousTextNode.Value = (context.CurrentIndent.Length > 0) ? $"{previousTextNode.Value}{trailingNewLine.Value}{context.CurrentIndent}" :
                                    $"{previousTextNode.Value}{trailingNewLine.Value}";
                        }
                        else
                            node.AddBeforeSelf(new XText((context.CurrentIndent.Length > 0) ? $"{NewLine}{context.CurrentIndent}" : NewLine));
                        if (node.NextNode is XText nextTextNode)
                        {
                            LeadingNewLinex leadingNewLine = nextTextNode.Annotations<LeadingNewLinex>().FirstOrDefault();
                            if (leadingNewLine is null)
                            {
                                if (string.IsNullOrWhiteSpace(nextTextNode.Value))
                                {
                                    nextTextNode.RemoveAnnotations<TrailingNewLinex>();
                                    nextTextNode.AddAnnotation(new TrailingNewLinex(NewLine, context.CurrentIndent.Length == 0));
                                    nextTextNode.Value = NewLine;
                                }
                                else
                                    nextTextNode.Value = $"{nextTextNode.Value}{NewLine}";
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(nextTextNode.Value))
                                {
                                    nextTextNode.RemoveAnnotations<TrailingNewLinex>();
                                    nextTextNode.AddAnnotation(new TrailingNewLinex(leadingNewLine.Value, context.CurrentIndent.Length == 0));
                                    nextTextNode.Value = leadingNewLine.Value;
                                }
                                else
                                    nextTextNode.Value = $"{nextTextNode.Value}{leadingNewLine.Value}";
                            }
                        }
                        else
                        {
                            nextTextNode = new XText(NewLine);
                            nextTextNode.AddAnnotation(new TrailingNewLinex(NewLine, context.CurrentIndent.Length == 0));
                            node.AddAfterSelf(nextTextNode);
                        }
                    }
                }
                //string indent = context.GetParentIndent();
                if (container.LastNode is XText lastTextNode)
                {
                    TrailingNewLinex trailingNewLine = lastTextNode.Annotations<TrailingNewLinex>().FirstOrDefault();
                    if (trailingNewLine is null)
                    {
                        if (context.CurrentIndent.Length > 0)
                            lastTextNode.Value = $"{lastTextNode.Value}{NewLine}{context.CurrentIndent}";
                        else
                            lastTextNode.Value = $"{lastTextNode.Value}{NewLine}";
                    }
                    else if (trailingNewLine.IncludesIndent.HasValue)
                    {
                        if (trailingNewLine.IncludesIndent.Value || context.CurrentIndent.Length == 0)
                            lastTextNode.Value = $"{lastTextNode.Value}";
                        else
                            lastTextNode.Value = $"{lastTextNode.Value}{context.CurrentIndent}";
                    }
                    else if (context.CurrentIndent.Length > 0)
                        lastTextNode.Value = $"{lastTextNode.Value}{trailingNewLine.Value}{context.CurrentIndent}";
                    else
                        lastTextNode.Value = $"{lastTextNode.Value}{trailingNewLine.Value}";
                }
                else
                {
                    lastTextNode = new XText((context.CurrentIndent.Length > 0) ? $"{NewLine}{context.CurrentIndent}" : NewLine);
                    container.Add(lastTextNode);
                }
                string indent = (context.CurrentIndent.Length > 0) ? $"{context.CurrentIndent}{IndentString}" : (context.IndentLevel < 0) ? "" : IndentString;
                if (container.FirstNode is XText firstTextNode)
                {
                    if (!(ReferenceEquals(lastTextNode, firstTextNode) && string.IsNullOrWhiteSpace(firstTextNode.Value)))
                    {
                        LeadingNewLinex leadingNewLine = firstTextNode.Annotations<LeadingNewLinex>().FirstOrDefault();
                        if (leadingNewLine is null)
                        {
                            if (indent.Length > 0)
                                lastTextNode.Value = $"{NewLine}{indent}{firstTextNode.Value}";
                            else
                                lastTextNode.Value = $"{NewLine}{firstTextNode.Value}";
                        }
                        else if (leadingNewLine.IncludesIndent.HasValue)
                        {
                            if (!leadingNewLine.IncludesIndent.Value && indent.Length > 0)
                                lastTextNode.Value = $"{indent}{firstTextNode.Value}";
                        }
                        else if (indent.Length > 0)
                            lastTextNode.Value = $"{leadingNewLine.Value}{indent}{firstTextNode.Value}";
                        else
                            lastTextNode.Value = $"{leadingNewLine.Value}{firstTextNode.Value}";
                    }
                }
                else if (container.FirstNode is not null)
                    container.FirstNode.AddBeforeSelf(new XText((indent.Length > 0) ? $"{NewLine}{indent}" : NewLine));
            }
            else
            {
                container.RemoveAnnotations<OuterLineBreakCharacteristics>();
                container.AddAnnotation(new OuterLineBreakCharacteristics(false));
            }

            foreach (XNode node in container.Nodes())
            {
                if (node is XText)
                {
                    node.RemoveAnnotations<NonWhiteSpaceCharacteristics>();
                    node.RemoveAnnotations<LeadingNewLinex>();
                    node.RemoveAnnotations<TrailingNewLinex>();
                }
                else if (node is XElement || node is XComment)
                    node.RemoveAnnotations<OuterLineBreakCharacteristics>();
            }
        }

        protected virtual void Normalize(XText textNode, Context context)
        {
            if (textNode.Value.Length == 0)
                return;
            textNode.RemoveAnnotations<NonWhiteSpaceCharacteristics>();
            textNode.RemoveAnnotations<LeadingNewLinex>();
            textNode.RemoveAnnotations<TrailingNewLinex>();
            bool hasPrecedingContent = textNode.NodesBeforeSelf().OfType<XContainer>().Any() || textNode.NodesBeforeSelf().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));
            bool hasFollowingContent = textNode.NodesAfterSelf().OfType<XContainer>().Any() || textNode.NodesAfterSelf().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value));
            string text = hasPrecedingContent ? (hasFollowingContent ? textNode.Value : textNode.Value.TrimEnd()) : hasFollowingContent ? textNode.Value.TrimStart() : textNode.Value.Trim();
            if (text.Length > 0)
            {
                if (NonNormalizedLineSeparatorRegex.IsMatch(text))
                    text = NonNormalizedLineSeparatorRegex.Replace(text, NewLine);
                Match leadingWsMatch = LeadingNewLineWithOptWs.Match(text);
                if (leadingWsMatch.Success)
                    textNode.AddAnnotation(new LeadingNewLinex(leadingWsMatch.Groups[1].Value, null));
                Match trailingWsMatch = TrailingNewLineWithOptWs.Match(text);
                if (trailingWsMatch.Success)
                    textNode.AddAnnotation(new TrailingNewLinex(trailingWsMatch.Groups[1].Value, null));
                if (EmptyOrWhiteSpaceLineRegex.IsMatch(text) && (text = EmptyOrWhiteSpaceLineRegex.Replace(text, "")).Length == 0)
                    text = " ";
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
                        textNode.AddAnnotation(new NonWhiteSpaceCharacteristics(true));
                    }
                    else
                        textNode.AddAnnotation(new NonWhiteSpaceCharacteristics(false));
                }
            }
            else if (!(hasPrecedingContent || hasFollowingContent))
                text = " ";
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
            public string GetParentIndent()
            {
                if (Parent is null)
                    return (string.IsNullOrEmpty(_normalizer.IndentString) || IndentLevel < 2) ? "" : (IndentLevel == 2) ? _normalizer.IndentString : string.Join("", Enumerable.Repeat(_normalizer.IndentString, IndentLevel - 1));
                return Parent.CurrentIndent;
            }
            internal Context(int indentLevel, XNode node, XNodeNormalizer normalizer) : this(indentLevel, 0, node ?? throw new ArgumentNullException(nameof(node)),
                normalizer ?? throw new ArgumentNullException(nameof(normalizer)))
            { }
            private Context([DisallowNull] Context context, [DisallowNull] XNode node)
            {
                _normalizer = (Parent = context)._normalizer;
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

            internal Context Indented() => new(IndentLevel + 1, Depth, Node, _normalizer);
        }
    }

}
