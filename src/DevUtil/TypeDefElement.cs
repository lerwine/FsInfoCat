using System;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public abstract class TypeDefElement
    {
        public const string ElementName_EntityTypes = "EntityTypes";

        public const string ElementName_Local = "Local";

        public const string ElementName_Upstream = "Upstream";

        public const string AttributeName_Name = "Name";

        public const string ElementName_Display = "Display";

        public const string AttributeName_Label = "Label";

        public const string AttributeName_ShortName = "ShortName";

        public const string AttributeName_Description = "Description";

        public XElement Element { get; }

        public string Name { get; }

        public abstract EntityScope Scope { get; }

        protected TypeDefElement(XElement element)
        {
            Element = element;
            Name = element.Attribute(AttributeName_Name)?.Value ?? throw new ArgumentException((element is IXmlLineInfo lineInfo) ?
                $"Name attribute not found in element {element.Name.LocalName}, Line: {lineInfo.LineNumber}, Position: {lineInfo.LinePosition}" :
                $"Name attribute not found in element {element.Name.LocalName}", nameof(element));
        }
    }
}
