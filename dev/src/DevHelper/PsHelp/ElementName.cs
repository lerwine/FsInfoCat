using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp
{
    public enum ElementName
    {
        [XmlQualifiedName(NamespaceURI.msh)]
        helpItems,

        [XmlQualifiedName(NamespaceURI.command)]
        command,

        [XmlQualifiedName(NamespaceURI.command)]
        details,

        [XmlQualifiedName(NamespaceURI.maml)]
        description,

        [XmlQualifiedName(NamespaceURI.maml)]
        para,

        [XmlQualifiedName(NamespaceURI.command, LocalName = "name")]
        commandName,

        [XmlQualifiedName(NamespaceURI.maml)]
        copyright,

        [XmlQualifiedName(NamespaceURI.command)]
        verb,

        [XmlQualifiedName(NamespaceURI.command)]
        noun,

        [XmlQualifiedName(NamespaceURI.dev)]
        version,

        [XmlQualifiedName(NamespaceURI.command)]
        synonyms,

        [XmlQualifiedName(NamespaceURI.command)]
        synonym,

        [XmlQualifiedName(NamespaceURI.command)]
        syntax,

        [XmlQualifiedName(NamespaceURI.command)]
        syntaxItem,

        [XmlQualifiedName(NamespaceURI.command)]
        parameters,

        [XmlQualifiedName(NamespaceURI.command)]
        parameter,

        [XmlQualifiedName(NamespaceURI.command)]
        inputTypes,

        [XmlQualifiedName(NamespaceURI.command)]
        inputType,

        [XmlQualifiedName(NamespaceURI.command)]
        returnValues,

        [XmlQualifiedName(NamespaceURI.command)]
        returnValue,

        [XmlQualifiedName(NamespaceURI.command)]
        terminatingErrors,

        [XmlQualifiedName(NamespaceURI.command)]
        terminatingError,

        [XmlQualifiedName(NamespaceURI.command)]
        nonTerminatingErrors,

        [XmlQualifiedName(NamespaceURI.command)]
        nonTerminatingError,

        [XmlQualifiedName(NamespaceURI.maml)]
        alertSet,

        [XmlQualifiedName(NamespaceURI.command)]
        examples,

        [XmlQualifiedName(NamespaceURI.command)]
        example,

        [XmlQualifiedName(NamespaceURI.maml)]
        relatedLinks,

        [XmlQualifiedName(NamespaceURI.maml)]
        navigationLink,

        [XmlQualifiedName(NamespaceURI.maml)]
        list,

        [XmlQualifiedName(NamespaceURI.maml)]
        quote,

        [XmlQualifiedName(NamespaceURI.maml)]
        definitionList,

        [XmlQualifiedName(NamespaceURI.maml)]
        table,

        [XmlQualifiedName(NamespaceURI.maml)]
        definitionListItem,

        [XmlQualifiedName(NamespaceURI.maml)]
        name,

        [XmlQualifiedName(NamespaceURI.dev)]
        possibleValues,

        [XmlQualifiedName(NamespaceURI.dev)]
        possibleValue,

        [XmlQualifiedName(NamespaceURI.command)]
        parameterValue,

        [XmlQualifiedName(NamespaceURI.dev)]
        type,

        [XmlQualifiedName(NamespaceURI.dev)]
        defaultValue,

        [XmlQualifiedName(NamespaceURI.command)]
        validation
    }
}
