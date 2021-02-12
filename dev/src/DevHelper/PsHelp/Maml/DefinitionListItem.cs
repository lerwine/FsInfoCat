using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.definitionListItem)]
    public class DefinitionListItem
    {
        private string _term;
        private string _definition;
    }
}
