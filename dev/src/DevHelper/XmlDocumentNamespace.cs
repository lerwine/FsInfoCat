namespace DevHelper
{
    public enum XmlDocumentNamespace
    {
        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MSH, Prefix = "")]
        None,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MSH, SchemaLocation = "Msh.xsd")]
        msh,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MAML, IsCommandNS = true, SchemaLocation = "PSMaml/Maml.xsd", ElementFormQualified = true)]
        maml,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_COMMAND, IsCommandNS = true, SchemaLocation = "PSMaml/developerCommand.xsd", ElementFormQualified = true)]
        command,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_DEV, IsCommandNS = true, SchemaLocation = "PSMaml/developer.xsd", ElementFormQualified = true)]
        dev,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MSHELP, IsCommandNS = true, ElementFormQualified = true)]
        MSHelp,
        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_XSI, ElementFormQualified = true)]
        xsi,
        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_XMLNS, ElementFormQualified = true)]
        xmlns
    }
}
