using DevHelper.PsHelp.Serialization;
using System.Xml.Serialization;

namespace DevHelper.PsHelp.Command
{
    [PsHelpXmlRoot(ElementName.parameter)]
    public class CommandParameter : ParameterElement
    {
        private bool _pipelineInput = false;

        [XmlAttribute(AttributeName_pipelineInput)]
        public bool PipelineInput
        {
            get => _pipelineInput;
            set => CheckPropertyChange(nameof(PipelineInput), _pipelineInput, value, n => _pipelineInput = n);
        }
    }
}
