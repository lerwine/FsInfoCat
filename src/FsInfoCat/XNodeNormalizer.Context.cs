using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FsInfoCat
{
    public partial class XNodeNormalizer
    {
        /// <summary>
        /// Defines the context of <see cref="XNode"/> normalization. This class cannot be inherited.
        /// </summary>
        protected sealed class Context
        {
            private readonly XNodeNormalizer _normalizer;

            /// <summary>
            /// Gets the normalization context of the parent <see cref="XNode"/>.
            /// </summary>
            /// <value>The <see cref="Context"/> object for normalization of the parent <see cref="XNode"/>.</value>
            public Context Parent { get; }

            /// <summary>
            /// Gets the indent level.
            /// </summary>
            /// <value>The number of times to indent each line. A value less than <c>1</c> indicates that lines are not indented/</value>
            public int IndentLevel { get; }

            /// <summary>
            /// Gets the recursion depth for normalization of the current <see cref="Node"/>.
            /// </summary>
            /// <value>The recursion depth of the current <see cref="Node"/>. A value of <c>0</c> indicates that this is the top-most node being normalized.</value>
            /// <remarks>This value does not necessarily indicate the node depth as it relates to its owning <see cref="XDocument"/>.</remarks>
            public int Depth { get; }

            /// <summary>
            /// Gets the current indent string.
            /// </summary>
            /// <value>The current indent string.</value>
            [NotNull]
            public string CurrentIndent { get; }

            /// <summary>
            /// Gets the node being normalized.
            /// </summary>
            /// <value>The <see cref="XNode"/> that is being normalized.</value>
            [NotNull]
            public XNode Node { get; }

            /// <summary>
            /// Gets the parent node.
            /// </summary>
            /// <value>The parent <see cref="XNode"/> of the current <see cref="Node"/>.</value>
            public XNode ParentNode { get; }

            /// <summary>
            /// Gets the indent string for the parent context.
            /// </summary>
            /// <returns>The indent string of the <see cref="Parent"/> <see cref="Context"/>. If <see cref="Parent"/> is <see langword="null"/>, this
            /// will calculate the indent string for <code><see cref="IndentLevel"/> - 1</code></returns>
            public string GetParentIndent()
            {
                if (Parent is null)
                    return (string.IsNullOrEmpty(_normalizer.IndentString) || IndentLevel < 2) ? "" : (IndentLevel == 2) ? _normalizer.IndentString : string.Join("", Enumerable.Repeat(_normalizer.IndentString, IndentLevel - 1));
                return Parent.CurrentIndent;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Context"/> class.
            /// </summary>
            /// <param name="indentLevel">The indent level.</param>
            /// <param name="node">The <see cref="XNode"/> to be normalized.</param>
            /// <param name="normalizer">The <see cref="XNodeNormalizer"/> instance that will perform the normalization.</param>
            internal Context(int indentLevel, [DisallowNull] XNode node, XNodeNormalizer normalizer)
                : this(indentLevel, 0, node ?? throw new ArgumentNullException(nameof(node)), normalizer ?? throw new ArgumentNullException(nameof(normalizer)), null,
                      (string.IsNullOrEmpty(normalizer.IndentString) || indentLevel < 1) ? "" : (indentLevel == 1) ? normalizer.IndentString :
                      string.Join("", Enumerable.Repeat(normalizer.IndentString, indentLevel))) { }

            private Context([DisallowNull] Context context, [DisallowNull] XNode node) : this(context.IndentLevel, context.Depth, node, context._normalizer, context, context.CurrentIndent) { }

            private Context(int indentLevel, int depth, [DisallowNull] XNode node, [DisallowNull] XNodeNormalizer normalizer, Context parent, string currentIndent)
            {
                _normalizer = normalizer;
                IndentLevel = indentLevel;
                Depth = depth;
                CurrentIndent = currentIndent;
                Parent = parent;
                ParentNode = (Node = node).Parent;
            }

            /// <summary>
            /// Creates a normalization context for the first child node.
            /// </summary>
            /// <returns>A new normalization <see cref="Context"/> for the <see cref="XContainer.FirstNode">first node</see>.
            /// <para>This will return <see langword="null"/> if the current <see cref="Node"/> meets any of the following conditions:
            /// <list type="bullet">
            /// <item>is not an <see cref="XContainer"/>.</item>
            /// <item>is an <see cref="XElement.IsEmpty">empty element</see>.</item>
            /// <item>is a <see cref="XDocument"/> without a <see cref="XDocument.Root">root element</see></item>
            /// </list></para></returns>
            internal Context GetFirstNodeContext()
            {
                if (Node is XContainer container)
                {
                    XNode node = container.FirstNode;
                    if (node is not null)
                    {
                        string indent = (string.IsNullOrEmpty(_normalizer.IndentString) || IndentLevel < 1) ? "" : (IndentLevel == 1) ? _normalizer.IndentString : string.Join("", Enumerable.Repeat(_normalizer.IndentString, IndentLevel));
                        return (container is XDocument) ? new Context(IndentLevel, Depth + 1, node, _normalizer, this, indent) : new Context(IndentLevel + 1, Depth + 1, node, _normalizer, this, indent);
                    }
                }
                return null;
            }

            /// <summary>
            /// Creates a normalization context for the next sibling node.
            /// </summary>
            /// <returns>A new normalization <see cref="Context"/> for the <see cref="XNode.NextNode">next node</see> or <see langword="null"/> if <code><see cref="Node"/>.<see cref="XNode.NextNode"/></code> returns <see langword="null"/>.</returns>
            internal Context NextNode()
            {
                XNode node = Node.NextNode;
                return (node is null) ? null : new Context(this, node);
            }
        }
    }
}
