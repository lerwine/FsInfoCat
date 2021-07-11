using System.Linq;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests.XNodeNormalizerTestHelpers
{
    public class TestXNodeNormalizer : XNodeNormalizer
    {
        public const string CustomIndent = "  ";
        public static readonly XName XName_Summary = XName.Get("summary");
        public static readonly XName XName_LangWord = XName.Get("langword");
        public static readonly XName XName_See = XName.Get("see");
        public static readonly XName XName_Cref = XName.Get("cref");
        public const string ElementName_Para = "para";
        public const string ElementName_List = "list";
        public const string ElementName_Item = "item";
        public const string ElementName_Term = "term";
        public const string ElementName_Description = "description";
        protected override string IndentString => CustomIndent;
        protected override void BaseNormalize(XContainer container, int indentLevel)
        {
            if (container is XDocument document)
            {
                if (document.Root is not null && document.Root.Name == XName_LangWord)
                    document.Root.ReplaceWith(new XElement(XName_See, new XAttribute(XName_Cref, document.Root.Value)));
            }
            else if (container is XElement element && element.Name == XName_LangWord)
                container.ReplaceWith(new XElement(XName_See, new XAttribute(XName_Cref, element.Value)));
            base.BaseNormalize(container, indentLevel);
        }
        protected override void Normalize(XElement element, Context context)
        {
            foreach (XElement el in element.Elements(XName_LangWord).ToArray())
                el.ReplaceWith(new XElement(XName_See, new XAttribute(XName_Cref, el.Value)));
            base.Normalize(element, context);
            if (element.Name == XName_Summary)
            {
                if (element.IsEmpty)
                    element.Add(new XText(context.CurrentIndent.Length > 0 ? $"{NewLine}{context.CurrentIndent}" : NewLine));
                else if (element.FirstNode is XText text && ReferenceEquals(text, element.LastNode))
                {
                    if (text.Value.Trim().Length == 0)
                        text.Value = (context.CurrentIndent.Length > 0) ? $"{NewLine}{context.CurrentIndent}" : NewLine;
                    else if (!NewLineRegex.IsMatch(text.Value))
                    {
                        if (context.CurrentIndent.Length > 0)
                            text.Value = $"{NewLine}{context.CurrentIndent}{IndentString}{text.Value}{NewLine}{context.CurrentIndent}";
                        else
                            text.Value = $"{NewLine}{text.Value}{NewLine}";
                    }
                }
            }
        }
        protected override bool ShouldForceLineBreak(XElement element, Context context)
        {
            if (context.Depth == 0 || element.Name.NamespaceName.Length > 0 || element.Document is not null && (ReferenceEquals(element, element.Document.Root) || ReferenceEquals(element.Parent, element.Document.Root)))
                return true;
            return element.Name.LocalName switch
            {
                ElementName_Para or ElementName_List or ElementName_Item or ElementName_Term or ElementName_Description => true,
                _ => element.DescendantNodes().OfType<XText>().Any(t => NewLineRegex.IsMatch(t.Value)),
            };
        }
    }
}
