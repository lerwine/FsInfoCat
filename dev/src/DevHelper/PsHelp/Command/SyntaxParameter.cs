using DevHelper.PsHelp.Serialization;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DevHelper.PsHelp.Command
{
    [PsHelpXmlRoot(ElementName.parameter)]
    public class SyntaxParameter : ParameterElement
    {
        private PiplineInput _pipelineInput;

        [XmlIgnore]
        public PiplineInput PiplineInput
        {
            get => _pipelineInput;
            set => CheckPropertyChange(nameof(Position), _pipelineInput, value, n => _pipelineInput = n);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute(AttributeName_pipelineInput)]
        public string __PiplineInputText
        {
            get => _pipelineInput.ToXmlText();
            set => PiplineInput = value.ToPiplineInput();
        }

    }
}
