$Path = $PSScriptRoot | Join-Path -ChildPath 'PropertySystem.xml';
$BaseUrl = 'https://docs.microsoft.com/en-us/windows/win32/properties/props-system';
[Xml]$Xml = '<Properties/>';
$Xml.Load($Path);
foreach ($SetElement in @($Xml.DocumentElement.SelectNodes('Set'))) {
    $PropertyElementList = @($SetElement.PSBase.SelectNodes('Property'));
    foreach ($PropertyElement in $PropertyElementList) {
        $XmlAttribute = $PropertyElement.Attributes.Remove($PropertyElement.SelectSingleNode('@PropertyName'));
        $PropertyElement.Attributes.Append($Xml.CreateAttribute('Name')).Value = $XmlAttribute.Value;
        $ClrType = $PropertyElement.Attributes.Remove($PropertyElement.SelectSingleNode('@ClrType')).Value;
        $IsArray = $ClrType.EndsWith('[]');
        if ($IsArray) { $ClrType = $ClrType.Substring(0, $ClrType.Length - 2); }
        $IsNullable = $ClrType.EndsWith('?');
        if ($IsNullable) { $ClrType = $ClrType.Substring(0, $ClrType.Length - 1); }
        $PropertyElement.Attributes.Append($Xml.CreateAttribute('Type')).value = $ClrType;
        if ($IsNullable) { $PropertyElement.Attributes.Append($Xml.CreateAttribute('Nullable')).value = 'true' }
        if ($IsArray) { $PropertyElement.Attributes.Append($Xml.CreateAttribute('Multiple')).value = 'true' }
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