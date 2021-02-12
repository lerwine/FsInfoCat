using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Xml.Schema;
using System.Xml.Serialization;
using DevHelper.PsHelp.Dev;
using DevHelper.PsHelp.Maml;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp.Command
{
    [PsHelpXmlRoot(ElementName.command)]
    public class CommandHelp
    {
        private DetailsElement _details;
        private Collection<ITextBlockElement> _description;
        private ObservableCollection<SyntaxItemElement> _syntax;
        private ObservableCollection<CommandParameter> _parameters;
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
            get => _details;
            set
            {
                string oldName, newName;
                if (value is null)
                {
                    if (_details is null)
                        return;
                    oldName = _details.Name;
                    _details.PropertyChanged -= Details_PropertyChanged;
                    _details = null;
                    newName = "";
                }
                else
                {
                    if (_details is null)
                        oldName = "";
                    else
                    {
                        if (ReferenceEquals(_details, value))
                            return;
                        oldName = _details.Name;
                        _details.PropertyChanged -= Details_PropertyChanged;
                    }
                    (_details = value).PropertyChanged += Details_PropertyChanged;
                    newName = _details.Name;
                }
                if (!oldName.Equals(newName))
                    OnCommandNameChanged(newName);
            }
        }

        [PsHelpXmlArray(ElementName.description, Order = 1, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.para, typeof(ParagraphElement))]
        [PsHelpXmlArrayItem(ElementName.list, typeof(ListElement))]
        [PsHelpXmlArrayItem(ElementName.quote, typeof(QuoteElement))]
        [PsHelpXmlArrayItem(ElementName.definitionList, typeof(DefinitionListElement))]
        [PsHelpXmlArrayItem(ElementName.table, typeof(TableElement))]
        [PsHelpXmlArrayItem(ElementName.alertSet, typeof(AlertSetElement))]
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
        [PsHelpXmlArrayItem(ElementName.syntaxItem, typeof(SyntaxItemElement))]
        public ObservableCollection<SyntaxItemElement> Syntax
        {
            get => _syntax;
            set => _syntax = value;
        }

        [PsHelpXmlArray(ElementName.parameters, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.parameter, typeof(CommandParameter))]
        public ObservableCollection<CommandParameter> Parameters
        {
            get
            {
                ObservableCollection<CommandParameter> parameters = _parameters;
                if (parameters is null)
                    _parameters = parameters = new ObservableCollection<CommandParameter>();
                return parameters;
            }
            set => _parameters = value;
        }

        [PsHelpXmlArray(ElementName.inputTypes, Order = 4, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.inputType, typeof(TypeElement))]
        public Collection<InputTypeElement> InputTypes { get; set; }

        [PsHelpXmlArray(ElementName.returnValues, Order = 5, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.returnValue, typeof(TypeElement))]
        public Collection<CommandReturnTypeElement> ReturnValues { get; set; }

        [PsHelpXmlArray(ElementName.terminatingErrors, Order = 6, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.terminatingError, typeof(ErrorElement))]
        public Collection<ErrorElement> TerminatingErrors { get; set; }

        [PsHelpXmlArray(ElementName.nonTerminatingErrors, Order = 7, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.nonTerminatingError, typeof(ErrorElement))]
        public Collection<ErrorElement> NonTerminatingErrors { get; set; }

        [PsHelpXmlElement(ElementName.alertSet, Order = 8, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public AlertSetElement AlertSet { get; set; }

        [PsHelpXmlArray(ElementName.examples, Order = 9, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.example, typeof(ExampleElement))]
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
            _details = new DetailsElement();
            _details.PropertyChanged += Details_PropertyChanged;
            _syntax = new ObservableCollection<SyntaxItemElement>();
            _syntax.CollectionChanged += Syntax_CollectionChanged;
            _parameters = new ObservableCollection<CommandParameter>();
            _parameters.CollectionChanged += Parameters_CollectionChanged;
        }

        private void OnCommandNameChanged(string newName)
        {
            foreach (SyntaxItemElement syntaxItem in _syntax)
            {
                if (!(syntaxItem is null))
                    syntaxItem.Name = newName;
            }
        }

        private void Details_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DetailsElement.Name))
                OnCommandNameChanged(_details.Name);
        }

        private void Syntax_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => OnCommandNameChanged(_details.Name);

        private void Parameters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [PsHelpXmlRoot(ElementName.details)]
        public class DetailsElement : PropertyChangeSupport
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
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            if (_name.Length == 0)
                                return;
                            CheckPropertyChange(
                                new PropertyChanger<string>(nameof(Name), _name, "", n => _name = n),
                                new PropertyChanger<string>(nameof(Noun), _noun, "", n => _noun = n),
                                new PropertyChanger<string>(nameof(Verb), _verb, "", n => _verb = n)
                            );
                        }
                        else if (value != _name)
                        {
                            int index = value.IndexOf("-");
                            if (index < 0)
                                CheckPropertyChange(
                                    new PropertyChanger<string>(nameof(Name), _name, value, n => _name = n),
                                    new PropertyChanger<string>(nameof(Noun), _noun, value, n => _noun = n),
                                    new PropertyChanger<string>(nameof(Verb), _verb, "", n => _verb = n)
                                );
                            else
                                CheckPropertyChange(
                                    new PropertyChanger<string>(nameof(Name), _name, value, n => _name = n),
                                    new PropertyChanger<string>(nameof(Noun), _noun, value.Substring(index + 1).Trim(), n => _noun = n),
                                    new PropertyChanger<string>(nameof(Verb), _verb, value.Substring(0, index).Trim(), n => _verb = n)
                                );
                        }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            [PsHelpXmlArray(ElementName.description, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            [PsHelpXmlArrayItem(ElementName.para, typeof(ParagraphElement))]
            public Collection<ITextBlockElement> Synopsis
            {
                get => _synopsis;
                set => CheckPropertyChange(nameof(Synopsis), _synopsis, value ?? new Collection<ITextBlockElement>(), v => _synopsis = v);
            }

            [PsHelpXmlArray(ElementName.synonyms, Order = 2, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            [PsHelpXmlArrayItem(ElementName.synonym)]
            public Collection<string> Synonyms
            {
                get => _synonyms;
                set => CheckPropertyChange(nameof(Synonyms), _synonyms, value, v => _synonyms = v);
            }

            [PsHelpXmlArray(ElementName.copyright, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            [PsHelpXmlArrayItem(ElementName.para, typeof(ParagraphElement))]
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
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (string.IsNullOrEmpty(value))
                            CheckPropertyChange(
                                new PropertyChanger<string>(nameof(Name), _name, _noun, n => _name = n),
                                new PropertyChanger<string>(nameof(Verb), _verb, "", n => _verb = n)
                            );
                        else if (value != _verb)
                            CheckPropertyChange(
                                new PropertyChanger<string>(nameof(Name), _name, (_noun.Length > 0) ? $"{value}-{_noun}" : value, n => _name = n),
                                new PropertyChanger<string>(nameof(Verb), _verb, value, n => _verb = n)
                            );
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            [PsHelpXmlElement(ElementName.noun, Order = 5, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            public string Noun
            {
                get => _noun;
                set
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (string.IsNullOrEmpty(value))
                            CheckPropertyChange(
                                new PropertyChanger<string>(nameof(Name), _name, _verb, n => _name = n),
                                new PropertyChanger<string>(nameof(Noun), _noun, "", n => _noun = n)
                            );
                        else if (value != _verb)
                            CheckPropertyChange(
                                new PropertyChanger<string>(nameof(Name), _name, (_verb.Length > 0) ? $"{_verb}-{value}" : value, n => _name = n),
                                new PropertyChanger<string>(nameof(Noun), _noun, value, n => _noun = n)
                            );
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            [PsHelpXmlElement(ElementName.version, Order = 6, Form = XmlSchemaForm.Qualified, IsNullable = false)]
            public string Version
            {
                get => _version;
                set => CheckPropertyChange(nameof(Version), _version, value ?? "", v => _version = v);
            }
        }
    }
}
