using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace DevHelper
{
    public class SyntaxItemElement : BaseElement
    {
        private static readonly ReadOnlyCollection<PsHelpNodeName> _allElements = new ReadOnlyCollection<PsHelpNodeName>(
            new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.parameter }
        );

        public string Name
        {
            get => (NameElement.IsEmpty) ? "" : NameElement.InnerText;
            set => NameElement.InnerText = value;
        }

        internal XmlElement NameElement => EnsureElement(PsHelpNodeName.name, _allElements);

        public IEnumerable<SyntaxParameterElement> GetParameterElements()
        {
            throw new NotImplementedException();
        }

        public SyntaxItemElement(XmlElement xmlElement) : base(xmlElement)
        {
            if (!PsHelpNodeName.syntaxItem.IsMatch(xmlElement))
                throw new ArgumentOutOfRangeException(nameof(xmlElement));
            foreach (PsHelpNodeName name in _allElements)
                EnsureElement(name, _allElements);
        }
    }
}
