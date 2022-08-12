using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class EnumElement : TypeDefElement
    {
        public const string ElementName_Enum = "Enum";

        public const string ElementName_Fields = "Fields";

        public ReadOnlyCollection<EnumFieldElement> Fields { get; }

        public override EntityScope Scope { get; }

        private EnumElement(XElement element) : base(element)
        {
            Scope = element.Parent.Name.LocalName switch
            {
                ElementName_Local => EntityScope.Local,
                ElementName_Upstream => EntityScope.Upstream,
                _ => EntityScope.Core,
            };
            Fields = new ReadOnlyCollection<EnumFieldElement>(element.Elements(ElementName_Fields).Elements().Select(e =>
            {
                EnumFieldElement f = new(e, this);
                e.AddAnnotation(f);
                return f;
            }).ToArray());
        }

        public static bool Test(XElement element)
        {
            if (element is null || element.Name.NamespaceName.Length > 0 || element.Name.LocalName != ElementName_Enum || element.Parent is null || element.Parent.Name.NamespaceName.Length > 0) return false;
            return element.Parent.Name.LocalName switch
            {
                ElementName_EntityTypes => ReferenceEquals(element.Parent.Document?.Root, element.Parent),
                ElementName_Local or ElementName_Upstream => ReferenceEquals(element.Parent.Parent.Document?.Root, element.Parent.Parent),
                _ => false,
            };
        }

        public static EnumElement Get(XElement element)
        {
            if (Test(element))
            {
                EnumElement result = element.Annotation<EnumElement>();
                if (result is null)
                {
                    result = new EnumElement(element);
                    element.AddAnnotation(result);
                }
                return result;
            }
            return null;
        }
    }
}
