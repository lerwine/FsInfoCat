using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml;

namespace DevHelper
{
    public class CommandParameterElement : BaseElement
    {
        private static readonly ReadOnlyCollection<PsHelpNodeName> _allElements = new ReadOnlyCollection<PsHelpNodeName>(
            new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.description, PsHelpNodeName.parameterValue,
                PsHelpNodeName.type }
        );

        private static readonly ReadOnlyCollection<PsHelpNodeName> _allTypeElements = new ReadOnlyCollection<PsHelpNodeName>(
            new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.uri }
        );

        public string Name
        {
            get => (NameElement.IsEmpty) ? "" : NameElement.InnerText;
            set => NameElement.InnerText = value;
        }

        public IEnumerable<string> Description
        {
            get => GetParaStrings(DescriptionElement);
            set => SetParaStrings(DescriptionElement, value);
        }

        public string ParameterValue
        {
            get => (ParameterValueElement.IsEmpty) ? "" : ParameterValueElement.InnerText;
            set => ParameterValueElement.InnerText = value;
        }

        public string TypeName
        {
            get => GetTypeString(PsHelpNodeName.name);
            set => EnsureTypeElement(PsHelpNodeName.name).InnerText = value;
        }

        private string GetTypeString(PsHelpNodeName name)
        {
            XmlElement element = EnsureTypeElement(name);
            return (element.IsEmpty) ? "" : element.InnerText;
        }

        internal XmlElement NameElement => EnsureElement(PsHelpNodeName.name, _allElements);

        internal XmlElement DescriptionElement => EnsureElement(PsHelpNodeName.description, _allElements);

        internal XmlElement ParameterValueElement => EnsureElement(PsHelpNodeName.parameterValue, _allElements);

        internal XmlElement TypeElement => EnsureElement(PsHelpNodeName.type, _allElements);

        private XmlElement EnsureTypeElement(PsHelpNodeName name)
        {
            Monitor.Enter(Xml);
            try
            {
                return TypeElement.EnsureChildElementInSequence(name.LocalName(out string ns), ns,
                    _allTypeElements.Select(s => s.QualifiedName()));
            }
            finally { Monitor.Exit(Xml); }
        }

        public CommandParameterElement(XmlElement xmlElement) : base(xmlElement)
        {
            if (!PsHelpNodeName.parameter.IsMatch(xmlElement))
                throw new ArgumentOutOfRangeException(nameof(xmlElement));
            foreach (PsHelpNodeName name in _allElements)
                EnsureElement(name, _allElements);
            foreach (PsHelpNodeName name in _allTypeElements)
                EnsureTypeElement(name);
        }
    }
}
