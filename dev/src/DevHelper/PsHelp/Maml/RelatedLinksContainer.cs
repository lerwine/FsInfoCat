using System.Collections.ObjectModel;
using System.Xml.Schema;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.relatedLinks)]
    public class RelatedLinksContainer
    {
        private string _title = "";
        private Collection<NavigationLink> _items;

        [PsHelpXmlElement(ElementName.version, Order = 0, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public string Title
        {
            get => _title;
            set => _title = value ?? "";
        }

        [PsHelpXmlArrayItemAttribute(ElementName.navigationLink, typeof(NavigationLink))]
        public Collection<NavigationLink> Items
        {
            get
            {
                Collection<NavigationLink> items = _items;
                if (items is null)
                    _items = items = new Collection<NavigationLink>();
                return items;
            }
            set => _items = value;
        }

    }
}
