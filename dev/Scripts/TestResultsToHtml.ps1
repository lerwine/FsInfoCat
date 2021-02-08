$XslCompiledTransform = [System.Xml.Xsl.XslCompiledTransform]::new();
$XmlReader = [System.Xml.XmlReader]::Create('C:\Users\lerwi\Git\FsInfoCat\dev\Scripts\TestResultsToHtml.xslt');
try {
    $XslCompiledTransform.Load($XmlReader);
    $XslCompiledTransform.Transform('C:\Users\lerwi\Git\FsInfoCat\dev\log\AllTestResults.xml', 'C:\Users\lerwi\Git\FsInfoCat\dev\log\AllTestResults.html');
} finally { $XmlReader.Close() }
# $XslCompiledTransform.OutputSettings = [System.Xml.XmlWriterSettings] @{ Indent = $true; Encoding = [System.Text.UTF8Encoding]::new($false, $true); OmitXmlDeclaration = $true; }
# $XmlDocument = [System.Xml.XmlDocument]::new();
# $XmlDocument.Load('C:\Users\lerwi\Git\FsInfoCat\dev\log\P2TestResults.xml');
