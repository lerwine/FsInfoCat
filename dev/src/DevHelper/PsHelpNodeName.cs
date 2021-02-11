using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace DevHelper
{
    public enum PsHelpNodeName
    {
        [XmlQName(XmlDocumentNamespace.msh)]
        helpItems,

        [XmlQName(XmlDocumentNamespace.command)]
        command,

        [XmlQName(XmlDocumentNamespace.maml)]
        para,

        [XmlQName(XmlDocumentNamespace.maml)]
        description,

        [XmlQName(XmlDocumentNamespace.maml)]
        copyright,

        [XmlQName(XmlDocumentNamespace.command)]
        verb,

        [XmlQName(XmlDocumentNamespace.command)]
        noun,

        [XmlQName(XmlDocumentNamespace.dev)]
        version,

        [XmlQName(XmlDocumentNamespace.command)]
        details,

        [XmlQName(XmlDocumentNamespace.command, LocalName = "command")]
        commandName,

        [XmlQName(XmlDocumentNamespace.maml)]
        name,

        [XmlQName(XmlDocumentNamespace.maml)]
        uri,

        [XmlQName(XmlDocumentNamespace.command)]
        syntax,

        [XmlQName(XmlDocumentNamespace.command)]
        syntaxItem,

        [XmlQName(XmlDocumentNamespace.command)]
        parameter,

        [XmlQName(XmlDocumentNamespace.dev)]
        type,

        [XmlQName(XmlDocumentNamespace.dev)]
        defaultValue,

        [XmlQName(XmlDocumentNamespace.command)]
        parameters,

        [XmlQName(XmlDocumentNamespace.command)]
        inputTypes,

        [XmlQName(XmlDocumentNamespace.command)]
        returnValues,

        [XmlQName(XmlDocumentNamespace.command)]
        terminatingErrors,

        [XmlQName(XmlDocumentNamespace.command)]
        nonTerminatingErrors,

        [XmlQName(XmlDocumentNamespace.maml)]
        alertSet,

        [XmlQName(XmlDocumentNamespace.command)]
        examples,

        [XmlQName(XmlDocumentNamespace.maml)]
        relatedLinks
    }
}
