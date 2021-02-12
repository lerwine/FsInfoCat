using System.Xml.Serialization;

namespace DevHelper.PsHelp.Command
{
    public class ParameterValueElement : PropertyChangeSupport
    {
        private const string AttributeName_required = "required";
        private const string AttributeName_variableLength = "variableLength";

        private bool _required = false;
        private bool _variableLength = false;
        private string _typeName = "";

        [XmlAttribute(AttributeName_required)]
        public bool Required
        {
            get => _required;
            set => CheckPropertyChange(nameof(Required), _required, value, n => _required = n);
        }

        [XmlAttribute(AttributeName_variableLength)]
        public bool VariableLength
        {
            get => _variableLength;
            set => CheckPropertyChange(nameof(VariableLength), _variableLength, value, n => _variableLength = n);
        }

        [XmlText]
        public string TypeName
        {
            get => _typeName;
            set => CheckPropertyChange(nameof(TypeName), _typeName, value ?? "", n => _typeName = n);
        }
    }
}
