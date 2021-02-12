using System.Collections.ObjectModel;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Maml
{
    [PsHelpXmlRoot(ElementName.definitionList)]
    public class DefinitionListElement : PropertyChangeSupport, ITextBlockElement
    {
        private ObservableCollection<DefinitionListItem> _contents;

        [PsHelpXmlElement(ElementName.definitionList)]
        public ObservableCollection<DefinitionListItem> Contents
        {
            get => _contents;
            set => CheckPropertyChange(nameof(Contents), _contents, value ?? new ObservableCollection<DefinitionListItem>(), n =>
            {
#warning Need to attach change handlers
                _contents = n;
            });
        }
    }
}
