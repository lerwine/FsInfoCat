using System.Xml.Linq;

namespace DevUtil
{
    public class EnumFieldElement : TypeDefElement
    {
        public const string ElementName_Value = "Value";

        public EnumElement DeclaringType { get; }

        public string Value { get; }

        public string Label { get; }

        public string ShortName { get; }

        public string Description { get; }

        public override EntityScope Scope => DeclaringType.Scope;

        internal EnumFieldElement(XElement element, EnumElement declaringType) : base(element)
        {
            DeclaringType = declaringType;
            Value = element.Attribute(ElementName_Value)?.Value;
            XElement e = element.Element(ElementName_Display);
            if (e is not null) {
                Label = e.Attribute(AttributeName_Label)?.Value;
                ShortName = e.Attribute(AttributeName_ShortName)?.Value;
                Description = e.Attribute(AttributeName_Description)?.Value;
            }
        }
    }
}
