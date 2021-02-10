using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace DevHelper
{
    public class CommandElement : BaseElement
    {
        private XmlElement EnsureDetailsElement(PsHelpNodeName name)
        {
            Monitor.Enter(Xml);
            try
            {
                XmlElement element = Xml.SelectSingleNode($"{OwnerDocument.ToXPathName(PsHelpNodeName.details)}/{OwnerDocument.ToXPathName(name)}") as XmlElement;
                if (element is null)
                    element = DetailsElement.AppendChild(OwnerDocument.CreateElement(name)) as XmlElement;
                return element;
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

        public XmlElement DetailsElement => EnsureElement(PsHelpNodeName.details);

        public XmlElement DescriptionElement => EnsureElement(PsHelpNodeName.description);

        public XmlElement SyntaxElement => EnsureElement(PsHelpNodeName.syntax);

        public XmlElement ParametersElement => EnsureElement(PsHelpNodeName.parameters);

        public XmlElement InputTypesElement => EnsureElement(PsHelpNodeName.inputTypes);

        public XmlElement ReturnValuesElement => EnsureElement(PsHelpNodeName.returnValues);

        public XmlElement TerminatingErrorsElement => EnsureElement(PsHelpNodeName.terminatingErrors);

        public XmlElement NonTerminatingErrorsElement => EnsureElement(PsHelpNodeName.nonTerminatingErrors);

        public XmlElement AlertSetElement => EnsureElement(PsHelpNodeName.alertSet);

        public XmlElement ExamplesElement => EnsureElement(PsHelpNodeName.examples);

        public XmlElement RelatedLinksElement => EnsureElement(PsHelpNodeName.relatedLinks);

        public CommandElement(XmlElement commandElement) : base(commandElement)
        {
            foreach (PsHelpNodeName name in new PsHelpNodeName[] { PsHelpNodeName.details, PsHelpNodeName.description, PsHelpNodeName.syntax, PsHelpNodeName.parameters,
                    PsHelpNodeName.inputTypes, PsHelpNodeName.returnValues, PsHelpNodeName.terminatingErrors, PsHelpNodeName.nonTerminatingErrors,
                    PsHelpNodeName.alertSet, PsHelpNodeName.examples, PsHelpNodeName.relatedLinks })
                EnsureElement(name);
            foreach (PsHelpNodeName name in new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.description, PsHelpNodeName.copyright, PsHelpNodeName.verb,
                    PsHelpNodeName.noun, PsHelpNodeName.version })
                EnsureDetailsElement(name);
        }
    }
}
