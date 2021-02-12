using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using DevHelper.PsHelp.Maml;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Command
{
    [PsHelpXmlRoot(ElementName.command)]
    public class CommandHelp
    {
        private DetailsElement _details;
        private Collection<ITextBlockElement> _description;
        private Collection<SyntaxItemElement> _syntax;
        private Collection<CommandParameter> _parameters;
        private Collection<TypeElement> _inputTypes;
        private Collection<TypeElement> _returnValues;
        private Collection<ErrorElement> _terminatingErrors;
        private Collection<ErrorElement> _nonTerminatingErrors;
        private AlertSetElement _alertSet;
        private Collection<ExampleElement> _examples;
        private RelatedLinksContainer _relatedLinks;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get; set; }

        [PsHelpXmlElement(ElementName.details, Order = 0, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public DetailsElement Details
        {
            get
            {
                DetailsElement details = _details;
                if (details is null)
                    _details = details = new DetailsElement();
                return details;
            }
            set => _details = value;
        }

        [PsHelpXmlArray(ElementName.description, Order = 1, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.para, typeof(ParagraphElement))]
        [PsHelpXmlArrayItemAttribute(ElementName.list, typeof(ListElement))]
        [PsHelpXmlArrayItemAttribute(ElementName.quote, typeof(QuoteElement))]
        [PsHelpXmlArrayItemAttribute(ElementName.definitionList, typeof(DefinitionListElement))]
        [PsHelpXmlArrayItemAttribute(ElementName.table, typeof(TableElement))]
        [PsHelpXmlArrayItemAttribute(ElementName.alertSet, typeof(AlertSetElement))]
    //   <element ref="maml:para" minOccurs="0" maxOccurs="unbounded"/>
    //   <element ref="maml:list" minOccurs="0" maxOccurs="unbounded"/>
    //   <element ref="maml:table" minOccurs="0" maxOccurs="unbounded"/>
    //   <element ref="maml:example" minOccurs="0" maxOccurs="unbounded"/>
    //   <element ref="maml:alertSet" minOccurs="0" maxOccurs="unbounded"/>
    //   <element ref="maml:quote" minOccurs="0" maxOccurs="unbounded"/>
    //   <element ref="maml:definitionList" minOccurs="0" maxOccurs="unbounded"/>
        public Collection<ITextBlockElement> Description
        {
            get
            {
                Collection<ITextBlockElement> description = _description;
                if (description is null)
                    _description = description = new Collection<ITextBlockElement>();
                return description;
            }
            set => _description = value;
        }

        [PsHelpXmlArray(ElementName.syntax, Order = 2, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.syntaxItem, typeof(SyntaxItemElement))]
        public Collection<SyntaxItemElement> Syntax
        {
            get
            {
                Collection<SyntaxItemElement> syntax = _syntax;
                if (syntax is null)
                    _syntax = syntax = new Collection<SyntaxItemElement>();
                return syntax;
            }
            set => _syntax = value;
        }

        [PsHelpXmlArray(ElementName.parameters, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.parameter, typeof(CommandParameter))]
        public Collection<CommandParameter> Parameters
        {
            get
            {
                Collection<CommandParameter> parameters = _parameters;
                if (parameters is null)
                    _parameters = parameters = new Collection<CommandParameter>();
                return parameters;
            }
            set => _parameters = value;
        }

        [PsHelpXmlArray(ElementName.inputTypes, Order = 4, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.inputType, typeof(TypeElement))]
        public Collection<TypeElement> InputTypes { get; set; }

        [PsHelpXmlArray(ElementName.returnValues, Order = 5, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.returnValue, typeof(TypeElement))]
        public Collection<TypeElement> ReturnValues { get; set; }

        [PsHelpXmlArray(ElementName.terminatingErrors, Order = 6, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.terminatingError, typeof(ErrorElement))]
        public Collection<ErrorElement> TerminatingErrors { get; set; }

        [PsHelpXmlArray(ElementName.nonTerminatingErrors, Order = 7, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.nonTerminatingError, typeof(ErrorElement))]
        public Collection<ErrorElement> NonTerminatingErrors { get; set; }

        [PsHelpXmlElement(ElementName.alertSet, Order = 8, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public AlertSetElement AlertSet { get; set; }

        [PsHelpXmlArray(ElementName.examples, Order = 9, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItemAttribute(ElementName.example, typeof(ExampleElement))]
        public Collection<ExampleElement> Examples { get; set; }

        [PsHelpXmlElement(ElementName.relatedLinks, Order = 10, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public RelatedLinksContainer RelatedLinks { get; set; }

        public CommandHelp()
        {
            Xmlns = new XmlSerializerNamespaces();
            foreach (NamespaceURI ns in Enum.GetValues(typeof(NamespaceURI)))
            {
                if (ns.TryGetCommandHelpNamespace(out string commandNs))
                    Xmlns.Add(ns.GetDefaultPrefix(), commandNs);
            }
        }

        [PsHelpXmlRoot(ElementName.details)]
        public class DetailsElement
        {
            private object _syncRoot = new object();
            private string _name = "";
            private Collection<ITextBlockElement> _synopsis;
            private Collection<string> _synonyms;
            private Collection<ParagraphElement> _copyright;
            private string _verb = "";
            private string _noun = "";
            private string _version = "";

            [PsHelpXmlElement(ElementName.commandName, Order = 0, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            public string Name
            {
                get => _name;
                set
                {
                    lock (_syncRoot)
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            if (_name.Length == 0)
                                return;
                            _name = _noun = _verb = "";
                        }
                        else if (value != _name)
                        {
                            _name = value;
                            int index = value.IndexOf("-");
                            if (index < 0)
                            {
                                _verb = "";
                                _noun = value;
                            }
                            else
                            {
                                _verb = value.Substring(0, index).Trim();
                                _noun = value.Substring(index + 1).Trim();
                            }
                        }
                    }
                }
            }

            [PsHelpXmlArray(ElementName.description, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            [PsHelpXmlArrayItemAttribute(ElementName.para, typeof(ParagraphElement))]
            public Collection<ITextBlockElement> Synopsis
            {
                get
                {
                    Collection<ITextBlockElement> description = _synopsis;
                    if (description is null)
                        _synopsis = description = new Collection<ITextBlockElement>();
                    return description;
                }
                set => _synopsis = value;
            }

            [PsHelpXmlArray(ElementName.synonyms, Order = 2, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            [PsHelpXmlArrayItemAttribute(ElementName.synonym)]
            public Collection<string> Synonyms { get; set; }

            [PsHelpXmlArray(ElementName.copyright, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            [PsHelpXmlArrayItemAttribute(ElementName.para, typeof(ParagraphElement))]
            public Collection<ParagraphElement> Copyright
            {
                get
                {
                    Collection<ParagraphElement> copyright = _copyright;
                    if (copyright is null)
                        _copyright = copyright = new Collection<ParagraphElement>();
                    return copyright;
                }
                set => _copyright = value;
            }

            [PsHelpXmlElement(ElementName.verb, Order = 4, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            public string Verb
            {
                get => _verb;
                set
                {
                    lock (_syncRoot)
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            _name = _noun;
                            _verb = "";
                        }
                        else if (value != _verb)
                        {
                            _verb = value;
                            if (_noun.Length > 0)
                                _name = $"{value}-{_noun}";
                            else
                                _name = value;
                        }
                    }
                }
            }

            [PsHelpXmlElement(ElementName.noun, Order = 5, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            public string Noun
            {
                get => _noun;
                set
                {
                    lock (_syncRoot)
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            _name = _verb;
                            _noun = "";
                        }
                        else if (value != _noun)
                        {
                            _noun = value;
                            if (_verb.Length > 0)
                                _name = $"{_verb}-{value}";
                            else
                                _name = value;
                        }
                    }
                }
            }

            [PsHelpXmlElement(ElementName.version, Order = 6, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            public string Version
            {
                get => _version;
                set => _version = value ?? "";
            }
        }
    }
}
