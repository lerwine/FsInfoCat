$Path = $PSScriptRoot | Join-Path -ChildPath 'PropertySystem.xml';
$BaseUrl = 'https://docs.microsoft.com/en-us/windows/win32/properties/props-system';
[Xml]$Xml = '<Properties/>';
$Xml.Load($Path);
$ClipboardText = '';
foreach ($SetElement in @($Xml.DocumentElement.SelectNodes('Set'))) {
    [Xml]$ResultXml = '<CommentDoc/>';
    $ResultXml.DocumentElement.Attributes.Append($ResultXml.CreateAttribute('Name')).Value = $SetElement.Name;
    foreach ($PropertyElement in @($SetElement.SelectNodes('Property[not(count(Column)=0)]'))) {
        $TextElement = $PropertyElement.SelectSingleNode('Text');
        $Win32Element = $PropertyElement.SelectSingleNode('Win32');
        $FormatElement = $Xml.DocumentElement.SelectSingleNode("Format[@ID=`"$($Win32Element.FormatID)`"]");
        
        $Summary = ('' + $TextElement.Summary).Trim();
        if ($Summary.Length -eq 0) { $Summary = '' + $TextElement.DisplayName }
        $ResultXml.PreserveWhitespace = $true;
        $ResultXml.DocumentElement.AppendChild($ResultXml.CreateTextNode("`n    ")) | Out-Null;
        $DocElement = $ResultXml.DocumentElement.AppendChild($ResultXml.CreateElement('property'));
        $DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $XmlElement = $DocElement.AppendChild($ResultXml.CreateElement('summary'));
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// $($Summary)`n        /// ")) | Out-Null;
        $DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        if (-not [string]::IsNullOrWhiteSpace($TextElement.ValueDesc)) {
            $XmlElement = $DocElement.AppendChild($ResultXml.CreateElement('value'));
            $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// $($TextElement.ValueDesc)`n        /// ")) | Out-Null;
            $DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        }
        $XmlElement = $DocElement.AppendChild($ResultXml.CreateElement('remarks'));
        if (-not $TextElement.IsEmpty) {
            $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// $($TextElement.InnerText)`n        /// ")) | Out-Null;
        }
        #$DocElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $XmlElement = $XmlElement.AppendChild($ResultXml.CreateElement('list'));
        $XmlElement.Attributes.Append($ResultXml.CreateAttribute('type')).Value = 'bullet';
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
        $ItemElement.AppendChild($ResultXml.CreateElement('term')).InnerText = 'Name';
        $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = $TextElement.DisplayName;
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
        $ItemElement.AppendChild($ResultXml.CreateElement('term')).InnerText = 'Format ID';
        $n = '';
        if ($null -ne $FormatElement) { $n = '' + $FormatElement.Name }
        if ($n.Length -gt 0) {
            $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = "$($Win32Element.FormatID) ($n)";
        } else {
            $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = $Win32Element.FormatID;
        }
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
        $ItemElement.AppendChild($ResultXml.CreateElement('term')).InnerText = 'Property ID';
        $ItemElement.AppendChild($ResultXml.CreateElement('description')).InnerText = $Win32Element.ID;
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $Url = ('' + $Win32Element.InnerText).Trim();
        if ($Url.Length -gt 0) {
            $ItemElement = $XmlElement.AppendChild($ResultXml.CreateElement('item'));
            $e = $ItemElement.AppendChild($ResultXml.CreateElement('description')).AppendChild($ResultXml.CreateElement('a'));
            $e.Attributes.Append($ResultXml.CreateAttribute('href')).Value = $Win32Element.InnerText;
            $e.InnerText = '[Reference Link]';
        }
        $XmlElement.AppendChild($ResultXml.CreateTextNode("`n        /// ")) | Out-Null;
        $DocElement.AppendChild($ResultXml.CreateTextNode("`n    ")) | Out-Null;
    }
    $ResultXml.DocumentElement.AppendChild($ResultXml.CreateTextNode("`n")) | Out-Null;
    $ClipboardText = "`n$ClipboardText$($ResultXml.OuterXml.Trim())"
}
[System.Windows.Clipboard]::SetText($ClipboardText);

<#
$XmlWriter = [System.Xml.XmlWriter]::Create($Path, [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
});
try {
    $Xml.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
#>
get-verb | Out-GridView