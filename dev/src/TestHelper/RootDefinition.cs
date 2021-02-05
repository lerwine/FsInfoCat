using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class RootDefinition
    {
        public const string ROOT_ELEMENT_NAME = "Root";
        private int _id = -1;
        private string _description = "";
        private Collection<int> _templateRefs = null;

        [XmlAttribute()]
        public int ID
        {
            get => _id;
            set => _id = (value  < -1) ? -1 : value;
        }

        [XmlAttribute()]
        public string Description
        {
            get => _description;
            set => _description = (value is null) ? "" : value;
        }

        [XmlElement("TemplateRef", typeof(int))]
        public Collection<int> TemplateRefs
        {
            get
            {
                Collection<int> items = _templateRefs;
                if (items is null)
                    _templateRefs = items = new Collection<int>();
                return items;
            }
            set => _templateRefs = value;
        }
    }
}
