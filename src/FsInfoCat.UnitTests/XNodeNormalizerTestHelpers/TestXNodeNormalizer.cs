using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests.XNodeNormalizerTestHelpers
{
    public class TestXNodeNormalizer : XNodeNormalizer
    {
        public const string CustomIndent = "  ";
        public const string ElementName_Summary = "summary";
        public const string ElementName_Para = "para";
        public const string ElementName_Param = "param";
        public const string ElementName_Typeparam = "typeparam"; 
        public const string ElementName_Example = "example";
        public const string ElementName_Seealso = "seealso";
        public const string ElementName_Remarks = "remarks";
        public const string ElementName_Value = "value";
        public const string ElementName_Returns = "returns";
        public const string ElementName_Exception = "exception";
        public const string ElementName_List = "list";
        public const string ElementName_Item = "item";
        public const string ElementName_Term = "term";
        public const string ElementName_Description = "description";
        public static readonly XName XName_Summary = XName.Get(ElementName_Summary);
        public static readonly XName XName_LangWord = XName.Get("langword");
        public static readonly XName XName_See = XName.Get("see");
        public static readonly XName XName_Cref = XName.Get("cref");

        protected override string IndentString => CustomIndent;

        protected override void Normalize(XElement targetElement, Context context)
        {
            foreach (XElement el in targetElement.Elements(XName_LangWord).ToArray())
                el.ReplaceWith(new XElement(XName_See, new XAttribute(XName_Cref, el.Value)));
            base.Normalize(targetElement, context);
            if (targetElement.Name == XName_Summary || context.Depth == 0)
            {
                if (targetElement.IsEmpty)
                    targetElement.Add(new XText(context.CurrentIndent.Length > 0 ? $"{NewLine}{context.CurrentIndent}" : NewLine));
                else if (targetElement.FirstNode is XText text && ReferenceEquals(text, targetElement.LastNode))
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

        protected override bool ForceSurroundingLineBreaks([DisallowNull] XElement targetElement, [DisallowNull] Context context)
        {
            if (targetElement.Name.NamespaceName.Length == 0)
                return targetElement.Name.LocalName switch
                {
                    ElementName_List or ElementName_Para or ElementName_Param or ElementName_Typeparam or ElementName_Example or ElementName_Seealso or ElementName_Remarks or ElementName_Value or ElementName_Returns or ElementName_Exception => true,
                    _ => base.ForceSurroundingLineBreaks(targetElement, context)
                };
            return base.ForceSurroundingLineBreaks(targetElement, context);
        }

        protected override bool ForceIndentContent([DisallowNull] XElement targetElement, [DisallowNull] Context context)
        {
            if (targetElement.Name.NamespaceName.Length == 0)
                return targetElement.Name.LocalName switch
                {
                    ElementName_Summary or ElementName_List or ElementName_Item => true,
                    _ => base.ForceIndentContent(targetElement, context)
                };
            return base.ForceIndentContent(targetElement, context);
        }
    }
}
