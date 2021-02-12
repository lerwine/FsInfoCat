using DevHelper.PsHelp.Dev;
using System.Xml.Serialization;

namespace DevHelper.PsHelp.Command
{
    public class CommandReturnTypeElement : ReturnTypeElement
    {
        private const string AttributeName_isTrustedData = "isTrustedData";

        private TrustedDataType? _isTrustedData;

        [XmlAttribute(AttributeName_isTrustedData, Namespace = PsHelpUtil.Namespace_URI_command)]
        public TrustedDataType? IsTrustedData
        {
            get => _isTrustedData;
            set => CheckPropertyChange(nameof(IsTrustedData), _isTrustedData, value, n => _isTrustedData = n);
        }
    }
}
