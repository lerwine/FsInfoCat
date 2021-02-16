$XslCompiledTransform = [System.Xml.Xsl.XslCompiledTransform]::new();
$XmlReader = [System.Xml.XmlReader]::Create(($PSScriptRoot | Join-Path -ChildPath 'IntermediateToPsHelp.xslt'));
try {
    $XslCompiledTransform.Load($XmlReader);
    $XslCompiledTransform.Transform(($PSScriptRoot | Join-Path -ChildPath 'PsHelpIntermediate.xml'), ($PSScriptRoot | Join-Path -ChildPath 'IntermediateToPsHelp.xml'));
} finally { $XmlReader.Close() }
