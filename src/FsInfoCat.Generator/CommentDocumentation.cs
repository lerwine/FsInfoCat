using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public class CommentDocumentation
    {
        public static CommentDocumentation Create(SyntaxNode memberSyntax)
        {
            if (!memberSyntax.HasLeadingTrivia) return null;
            XmlDocument ownerDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };
            StringBuilder stringBuilder = new StringBuilder();
            XmlDocumentFragment fragment = ownerDocument.CreateDocumentFragment();
            foreach (XmlElementSyntax elementSyntax in memberSyntax.GetLeadingTrivia().OfType<DocumentationCommentTriviaSyntax>().SelectMany(t => t.Content.OfType<XmlElementSyntax>()))
                stringBuilder.AppendLine(elementSyntax.GetText().ToString());
            string text;
            if (stringBuilder.Length == 0 || (text = stringBuilder.ToString().Trim()).Length == 0) return null;

            try { fragment.InnerXml = text; }
            catch
            {
                XmlElement element = ownerDocument.CreateElement("NotParsable");
                element.AppendChild(ownerDocument.CreateCDataSection(text));
                fragment.AppendChild(element);
            }
            XmlElement[] elements = fragment.OfType<XmlElement>().ToArray();
            return (elements.Length > 0) ? new CommentDocumentation() { Elements = elements } : null;
        }

        [XmlAnyElement()]
        public XmlElement[] Elements { get; set; }
    }
}
