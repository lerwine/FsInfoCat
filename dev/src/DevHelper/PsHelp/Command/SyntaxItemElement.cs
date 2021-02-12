using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using DevHelper.PsHelp.Serialization;
using FsInfoCat.Util;

namespace DevHelper.PsHelp.Command
{
    [PsHelpXmlRoot(ElementName.syntaxItem)]
    public class SyntaxItemElement
    {
        public const string AtributeName_address = "address";
        public const string ADDRESS_PARAMETER_SET_NAME = "parameterSetName";
        private string _parameterSetName = "";

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
                        _parameterSetName = decoded.Substring(index + 1);
                        return;
                    }
                }
                _parameterSetName = "";
            }
        }

        [XmlIgnore]
        public string ParameterSetName
        {
            get => _parameterSetName;
            set => _parameterSetName = value ?? "";
        }
    }
}
