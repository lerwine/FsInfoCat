using System.Xml.Linq;

namespace DevUtil
{
    public class PropertyElement : TypeDefElement
    {
        public EntityElement DeclaringType { get; }

        public override EntityScope Scope => DeclaringType.Scope;

        private PropertyElement(XElement propertyElement, EntityElement declaringType) : base(propertyElement)
        {
            DeclaringType = declaringType;
            propertyElement.AddAnnotation(this);
        }

        public static PropertyElement Get(XElement element)
        {
            PropertyElement result = element.Annotation<PropertyElement>();
            if (result is null) {
                EntityElement entity = EntityElement.Get(element.Parent);
                if (entity is not null) return new PropertyElement(element, entity);
            }
            return result;
        }
    }
}
