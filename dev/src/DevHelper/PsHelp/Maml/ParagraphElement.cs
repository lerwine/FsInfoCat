using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.para)]
    public class ParagraphElement : PropertyChangeSupport, ITextBlockElement
    {
        private ObservableCollection<XmlNode> _contents;

        [XmlAnyElement]
        public ObservableCollection<XmlNode> Contents
        {
            get => _contents;
            set => CheckPropertyChange(nameof(Contents), _contents, value ?? new ObservableCollection<XmlNode>(), n =>
            {
#warning Need to attach change handlers
                _contents = n;
            });
        }
    }
}
