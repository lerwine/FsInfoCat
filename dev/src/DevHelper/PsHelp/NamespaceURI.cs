using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp
{
    public enum NamespaceURI
    {
        [PsHelpNamespace("")]
        [DefaultPrefix("")]
        Empty,

        [PsHelpNamespace(PsHelpUtil.Namespace_URI_msh)]
        [DefaultPrefix("")]
        msh,

        [PsHelpNamespace(PsHelpUtil.Namespace_URI_xsi)]
        xsi,

        [PsHelpNamespace(PsHelpUtil.Namespace_URI_maml, IsCommandHelpNamespace = true)]
        maml,

        [PsHelpNamespace(PsHelpUtil.Namespace_URI_command, IsCommandHelpNamespace = true)]
        command,

        [PsHelpNamespace(PsHelpUtil.Namespace_URI_dev, IsCommandHelpNamespace = true)]
        dev
    }
}
