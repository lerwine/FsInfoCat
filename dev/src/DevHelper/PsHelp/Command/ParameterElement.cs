using DevHelper.PsHelp.Dev;
using DevHelper.PsHelp.Maml;
using DevHelper.PsHelp.Serialization;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DevHelper.PsHelp.Command
{
    public abstract class ParameterElement : PropertyChangeSupport
    {
        public const string POSITION_NAMED = "named";
        protected const string AttributeName_pipelineInput = "pipelineInput";
        private const string AttributeName_required = "required";
        private const string AttributeName_position = "position";
        private const string AttributeName_variableLength = "variableLength";
        private const string AttributeName_globbing = "globbing";
        private const string AttributeName_requiresTrustedData = "requiresTrustedData";

        private bool _required = false;
        private int? _position = null;
        private bool _variableLength = false;
        private bool _globbing = false;
        private TrustedDataType? _requiresTrustedData;
        private string _name = "";
        private ObservableCollection<ITextBlockElement> _description;
        private ParameterValueElement _parameterValue = new ParameterValueElement();
        private TypeElement _type = null;
        private string _defaultValue = null;
        private ObservableCollection<PossibleValueElement> _possibleValues = null;
        private ValidationElement _validation = null;

        [XmlAttribute(AttributeName_required)]
        public bool Required
        {
            get => _required;
            set => CheckPropertyChange(nameof(Required), _required, value, n => _required = n);
        }

        [XmlIgnore]
        public int? Position
        {
            get => _position;
            set => CheckPropertyChange(nameof(Position), _position, value, n => _position = n);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute(AttributeName_position)]
        public string PositionText
        {
            get => (_position.HasValue) ? XmlConvert.ToString(_position.Value) : POSITION_NAMED;
            set
            {
                string p;
                if (!(value is null || (p = value.Trim()).Length == 0 || p.Equals(POSITION_NAMED, StringComparison.InvariantCultureIgnoreCase)))
                    try {
                        Position = XmlConvert.ToInt32(p);
                        return;
                    } catch { /* Ignored on purpose */ }
                Position = null;
            }
        }

        [XmlAttribute(AttributeName_variableLength)]
        public bool VariableLength
        {
            get => _variableLength;
            set => CheckPropertyChange(nameof(VariableLength), _variableLength, value, n => _variableLength = n);
        }

        [XmlAttribute(AttributeName_globbing)]
        public bool Globbing
        {
            get => _globbing;
            set => CheckPropertyChange(nameof(Globbing), _globbing, value, n => _globbing = n);
        }

        [XmlAttribute(AttributeName_requiresTrustedData, Namespace = PsHelpUtil.Namespace_URI_command)]
        public TrustedDataType? RequiresTrustedData
        {
            get => _requiresTrustedData;
            set => CheckPropertyChange(nameof(RequiresTrustedData), _requiresTrustedData, value, n => _requiresTrustedData = n);
        }

        [PsHelpXmlElement(ElementName.name, Order = 0, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public string Name
        {
            get => _name;
            set => CheckPropertyChange(nameof(Name), _name, value ?? "", n => _name = n);
        }

        [PsHelpXmlArray(ElementName.description, Order = 1, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.para, typeof(ParagraphElement))]
        [PsHelpXmlArrayItem(ElementName.list, typeof(ListElement))]
        [PsHelpXmlArrayItem(ElementName.quote, typeof(QuoteElement))]
        [PsHelpXmlArrayItem(ElementName.definitionList, typeof(DefinitionListElement))]
        [PsHelpXmlArrayItem(ElementName.table, typeof(TableElement))]
        [PsHelpXmlArrayItem(ElementName.alertSet, typeof(AlertSetElement))]
        public ObservableCollection<ITextBlockElement> Description
        {
            get => _description;
            set => CheckPropertyChange(nameof(Type), _description ?? new ObservableCollection<ITextBlockElement>(), value, n =>
            {
#warning Need to attach/detach event handlers
                _description = n;
            });
        }

        [PsHelpXmlElement(ElementName.parameterValue, Order = 2, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public ParameterValueElement ParameterValue
        {
            get => _parameterValue;
            set => CheckPropertyChange(nameof(Name), _parameterValue, value, n =>
            {
#warning Need to attach/detach event handlers
                _parameterValue = n;
            });
        }

        [PsHelpXmlElement(ElementName.type, Order = 3, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public TypeElement Type
        {
            get => _type;
            set => CheckPropertyChange(nameof(Type), _type, value, n =>
            {
#warning Need to attach/detach event handlers
                _type = n;
            });
        }

        [PsHelpXmlElement(ElementName.defaultValue, Order = 4, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public string DefaultValue
        {
            get => _defaultValue;
            set => CheckPropertyChange(nameof(DefaultValue), _defaultValue, value, n => _defaultValue = n);
        }

        [PsHelpXmlArray(ElementName.possibleValues, Order = 5, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        [PsHelpXmlArrayItem(ElementName.possibleValue, typeof(PossibleValueElement))]
        public ObservableCollection<PossibleValueElement> PossibleValues
        {
            get => _possibleValues;
            set => CheckPropertyChange(nameof(PossibleValues), _possibleValues, value, n =>
            {
#warning Need to attach/detach event handlers
                _possibleValues = n;
            });
        }

        [PsHelpXmlElement(ElementName.validation, Order = 4, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public ValidationElement Validation
        {
            get => _validation;
            set => CheckPropertyChange(nameof(Validation), _validation, value, n =>
            {
#warning Need to attach/detach event handlers
                _validation = n;
            });
        }
    }
}
