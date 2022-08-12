using System;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public class ImplementsElement
    {
        public const string ElementName_Ref = "Ref";

        public const string ElementName_Abstract = "Abstract";

        public const string ElementName_GenericAbstract1 = "GenericAbstract1";

        public const string ElementName_GenericAbstract2 = "GenericAbstract2";

        public XElement Element { get; }

        public string Ref { get; }

        public EntityElement Target { get; }

        private ImplementsElement(XElement element, EntityScope scope)
        {
            Element = element;
            Ref = element.Attribute(ElementName_Ref)?.Value ?? throw new ArgumentException((element is IXmlLineInfo lineInfo) ?
                $"Ref attribute not found in element {element.Name.LocalName}, Line: {lineInfo.LineNumber}, Position: {lineInfo.LinePosition}" :
                $"Ref attribute not found in element {element.Name.LocalName}", nameof(element));
            switch (element.Name.LocalName)
            {
                case ElementName_Abstract:
                    Target = EntityElement.FindEntity(element.Document, EntityType.AbstractEntity, scope, Ref);
                    break;
                case ElementName_GenericAbstract1:
                    Target = EntityElement.FindEntity(element.Document, EntityType.GenericEntity1, scope, Ref);
                    break;
                case ElementName_GenericAbstract2:
                    Target = EntityElement.FindEntity(element.Document, EntityType.GenericEntity2, scope, Ref);
                    break;
            }
        }

        public static ImplementsElement Get(XElement element)
        {
            ImplementsElement result = element.Annotation<ImplementsElement>();
            if (result is null) {
                EntityElement entity = EntityElement.Get(element.Parent);
                if (entity is null) return null;
                result = new ImplementsElement(element, entity.Scope);
                element.AddAnnotation(result);
            }
            return result;
        }
    }
}
