using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.para)]
    public class ParagraphElement : ITextBlockElement
    {
        private Collection<XmlNode> _contents;

        [XmlAnyElement]
        public Collection<XmlNode> Contents
        {
            get
            {
                Collection<XmlNode> contents = _contents;
                if (contents is null)
                    _contents = contents = new Collection<XmlNode>();
                return contents;
            }
            set => _contents = value;
        }
    }
}
