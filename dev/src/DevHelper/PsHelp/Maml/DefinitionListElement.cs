using System.Collections.ObjectModel;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.definitionList)]
    public class DefinitionListElement : ITextBlockElement
    {
        private Collection<DefinitionListItem> _contents;

        [PsHelpXmlElement(ElementName.definitionList)]
        public Collection<DefinitionListItem> Contents
        {
            get
            {
                Collection<DefinitionListItem> contents = _contents;
                if (contents is null)
                    _contents = contents = new Collection<DefinitionListItem>();
                return contents;
            }
            set => _contents = value;
        }

    }
}
