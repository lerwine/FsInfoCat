$Path = $PSScriptRoot | Join-Path -ChildPath 'PropertySystem.xml';
$BaseUrl = 'https://docs.microsoft.com/en-us/windows/win32/properties/props-system';
[Xml]$Xml = '<Properties/>';
$Xml.Load($Path);
foreach ($FormatIDAttribute in @($Xml.DocumentElement.SelectNodes('Set/Property/Win32/@FormatID'))) {
    if ($null -eq $Xml.DocumentElement.SelectSingleNode("Format[@ID='$($FormatIDAttribute.Value)']")) {
        $Xml.DocumentElement.AppendChild($Xml.CreateElement('Format')).Attributes.Append($Xml.CreateAttribute('ID')).Value = $FormatIDAttribute.Value;
    }
}
#<#
$XmlWriter = [System.Xml.XmlWriter]::Create($Path, [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
});
try {
    $Xml.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
#>