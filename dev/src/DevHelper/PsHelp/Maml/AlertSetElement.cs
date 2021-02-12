using System.Xml.Schema;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.alertSet)]
    public class AlertSetElement : PropertyChangeSupport, ITextBlockElement
    {
        private string _title = "";

        [PsHelpXmlElement(ElementName.version, Order = 0, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public string Title
        {
            get => _title;
            set => CheckPropertyChange(nameof(Title), _title, value ?? "", n => _title = n);
        }
    }
}
