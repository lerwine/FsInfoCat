using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    public sealed class PSDocumentationComments
    {
        private XmlDocumentFragment _xml;

        public string RawText { get; }

        public PSSyntaxBase Parent { get; }

        internal PSDocumentationComments([DisallowNull] IEnumerable<DocumentationCommentTriviaSyntax> trivia, [DisallowNull] PSSyntaxBase parent)
        {
            RawText = string.Join("\n", trivia.Select(s => s.ToString()));
            Parent = parent;
        }

        public XmlDocumentFragment GetXml()
        {
            Monitor.Enter(Parent.BaseSyntax);
            try
            {
                if (_xml is null)
                {
                    XmlDocument xmlDocument = new();
                    XmlDocumentFragment xml = xmlDocument.CreateDocumentFragment();
                    xml.InnerXml = RawText;
                    _xml = xml;
                }
            }
            finally { Monitor.Exit(Parent.BaseSyntax); }
            return _xml;
        }
    }
}
