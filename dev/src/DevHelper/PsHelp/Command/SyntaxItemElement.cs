using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DevHelper.PsHelp.Serialization;
using FsInfoCat.Util;

namespace DevHelper.PsHelp.Command
{
    [PsHelpXmlRoot(ElementName.syntaxItem)]
    public class SyntaxItemElement : PropertyChangeSupport
    {
        public const string AtributeName_address = "address";
        public const string ADDRESS_PARAMETER_SET_NAME = "parameterSetName";
        private string _parameterSetName = "";
        private string _name = "";
        private ObservableCollection<SyntaxParameter> _parameters;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute()]
        public string __Address
        {
            get => (_parameterSetName.Length > 0) ? XmlConvert.EncodeLocalName($"{ADDRESS_PARAMETER_SET_NAME}={_parameterSetName}") : null;
            set
            {
                if (!(string.IsNullOrWhiteSpace(value)))
                {
                    string decoded = XmlConvert.DecodeName(value);
                    int index = decoded.IndexOf("=");
                    if (index > 0 && decoded.Substring(0, index).Equals(ADDRESS_PARAMETER_SET_NAME))
                    {
                        CheckPropertyChange(nameof(ParameterSetName), _parameterSetName, decoded.Substring(index + 1), n => _parameterSetName = n);
                        return;
                    }
                }
                CheckPropertyChange(nameof(ParameterSetName), _parameterSetName, "", n => _parameterSetName = n);
            }
        }

        [XmlIgnore]
        public string ParameterSetName
        {
            get => _parameterSetName;
            set => CheckPropertyChange(nameof(ParameterSetName), _parameterSetName, value ?? "", n => _parameterSetName = n);
        }

        [PsHelpXmlElement(ElementName.name, Order = 0, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public string Name
        {
            get => _name;
            set => CheckPropertyChange(nameof(Name), _name, value ?? "", n => _name = n);
        }

        [PsHelpXmlElement(ElementName.parameter, Order = 1, Form = XmlSchemaForm.Qualified, IsNullable = false)]
        public ObservableCollection<SyntaxParameter> Parameters
        {
            get
            {
                ObservableCollection<SyntaxParameter> parameters = _parameters;
                if (parameters is null)
                    _parameters = parameters = new ObservableCollection<SyntaxParameter>();
                return parameters;
            }
            set => _parameters = value;
        }
    }
}
