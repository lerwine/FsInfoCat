using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml;

namespace DevHelper
{
    public class CommandElement : BaseElement
    {
        private static readonly ReadOnlyCollection<PsHelpNodeName> _allCommandElements = new ReadOnlyCollection<PsHelpNodeName>(
            new PsHelpNodeName[] { PsHelpNodeName.details, PsHelpNodeName.description, PsHelpNodeName.syntax, PsHelpNodeName.parameters,
                PsHelpNodeName.inputTypes, PsHelpNodeName.returnValues, PsHelpNodeName.terminatingErrors, PsHelpNodeName.nonTerminatingErrors,
                PsHelpNodeName.alertSet, PsHelpNodeName.examples, PsHelpNodeName.relatedLinks }
            );

        private static readonly ReadOnlyCollection<PsHelpNodeName> _allDetailsElements = new ReadOnlyCollection<PsHelpNodeName>(
            new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.description, PsHelpNodeName.copyright, PsHelpNodeName.verb,
                PsHelpNodeName.noun, PsHelpNodeName.version }
            );

        private XmlElement EnsureDetailsElement(PsHelpNodeName name)
        {
            Monitor.Enter(Xml);
            try
            {
                return Xml.EnsureChildElementInSequence(name.LocalName(out string ns), ns,
                    _allDetailsElements.Select(s => s.QualifiedName()));
            }
            finally { Monitor.Exit(Xml); }
        }

        private string GetDetailsString(PsHelpNodeName name)
        {
            XmlElement element = EnsureDetailsElement(name);
            return (element.IsEmpty) ? "" : element.InnerText;
        }

        public string Name => GetDetailsString(PsHelpNodeName.commandName);

        public IEnumerable<string> Synopsis
        {
            get => GetParaStrings(EnsureDetailsElement(PsHelpNodeName.description));
            set => SetParaStrings(EnsureDetailsElement(PsHelpNodeName.description), value);
        }

        public string Verb => GetDetailsString(PsHelpNodeName.verb);

        public string Noun => GetDetailsString(PsHelpNodeName.noun);

        public string Copyright
        {
            get => GetDetailsString(PsHelpNodeName.copyright);
            set => EnsureDetailsElement(PsHelpNodeName.copyright).InnerText = value;
        }

        public Version Version
        {
            get
            {
                string s = GetDetailsString(PsHelpNodeName.version);
                if (s.Length > 0 && Version.TryParse(s, out Version result))
                    return result;
                return null;
            }
            set => EnsureDetailsElement(PsHelpNodeName.copyright).InnerText = (value is null) ? "" : value.ToString();
        }

        public IEnumerable<string> Description
        {
            get => GetParaStrings(DescriptionElement);
            set => SetParaStrings(DescriptionElement, value);
        }

        public XmlElement DetailsElement => EnsureElement(PsHelpNodeName.details, _allCommandElements);

        public XmlElement DescriptionElement => EnsureElement(PsHelpNodeName.description, _allCommandElements);

        public XmlElement SyntaxElement => EnsureElement(PsHelpNodeName.syntax, _allCommandElements);

        public XmlElement ParametersElement => EnsureElement(PsHelpNodeName.parameters, _allCommandElements);

        public XmlElement InputTypesElement => EnsureElement(PsHelpNodeName.inputTypes, _allCommandElements);

        public XmlElement ReturnValuesElement => EnsureElement(PsHelpNodeName.returnValues, _allCommandElements);

        public XmlElement TerminatingErrorsElement => EnsureElement(PsHelpNodeName.terminatingErrors, _allCommandElements);

        public XmlElement NonTerminatingErrorsElement => EnsureElement(PsHelpNodeName.nonTerminatingErrors, _allCommandElements);

        public XmlElement AlertSetElement => EnsureElement(PsHelpNodeName.alertSet, _allCommandElements);

        public XmlElement ExamplesElement => EnsureElement(PsHelpNodeName.examples, _allCommandElements);

        public XmlElement RelatedLinksElement => EnsureElement(PsHelpNodeName.relatedLinks, _allCommandElements);

        public CommandElement(XmlElement commandElement) : base(commandElement)
        {
            foreach (PsHelpNodeName name in _allCommandElements)
                EnsureElement(name, _allCommandElements);
            foreach (PsHelpNodeName name in _allDetailsElements)
                EnsureDetailsElement(name);
        }
    }
}
