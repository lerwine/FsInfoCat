Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath 'bin/Debug/net6.0-windows/DevHelper') -ErrorAction Stop;

Function Test-ModelDefinitionsXml {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$XDocument
    )
    $Namespace = $XDocument.Root.Name.Namespace;
    $MemberReferenceDictionary = Get-MemberReferenceDictionary -XDocument $XDocument;
    $RootSelector = [DevUtil.XElementSelector]::new($XDocument.Root);
    $Attributes = $RootSelector.Descendants($Namespace.GetName('seealso')).Attributes('cref');
    $Attributes = $Attributes.Concat($RootSelector.Descendants($Namespace.GetName('see')).Attributes('cref').GetItems());
    foreach ($XAttribute in $Attributes.GetItems()) {
        if (-not $MemberReferenceDictionary.ContainsKey($XAttribute.Value)) {
            Write-Warning -Message "$($XAttribute.Name) attribute references an unknown type `"$($XAttribute.Value)`": Line $($XAttribute.LineNumber), Column $($XAttribute.LinePosition)";
        }
    }
    foreach ($XAttribute in $RootSelector.Descendants($Namespace.GetName('Type')).Attributes('Ref').GetItems()) {
        if (-not $MemberReferenceDictionary.ContainsKey($XAttribute.Value)) {
            Write-Warning -Message "$($XAttribute.Name) attribute references an unknown type `"$($XAttribute.Value)`": Line $($XAttribute.LineNumber), Column $($XAttribute.LinePosition)";
        }
    }
    foreach ($XAttribute in $RootSelector.Descendants($Namespace.GetName('Argument')).Attributes('Ref').GetItems()) {
        $ModelElement = [DevUtil.XElementSelector]::new($XAttribute.Parent).Ancestors($Namespace.GetName('Model')).FirstOrDefault();
        if ($null -eq $ModelElement-or -not [DevUtil.XElementSelector]::new($ModelElement).ElementsWithAttribute($Namespace.GetName('TypeParameter'), 'Name', $XAttribute.Value).Any()) {
            Write-Warning -Message "$($XAttribute.Name) attribute references an generic argument `"$($XAttribute.Value)`": Line $($XAttribute.LineNumber), Column $($XAttribute.LinePos0ition)";
        }
    }
}
$ModelDefinitionsPath = $PSScriptRoot | Join-Path -ChildPath 'ModelDefinitions.xml';
$ResourcesPath = $PSScriptRoot | Join-Path -ChildPath '../FsInfoCat/Properties/Resources.resx';

$ModelDefinitionsDocument = [System.Xml.Linq.XDocument]::Load($ModelDefinitionsPath, #[System.Xml.Linq.LoadOptions]::SetLineInfo);
    ([System.Xml.Linq.LoadOptions]([System.Xml.Linq.LoadOptions]::SetLineInfo -bor [System.Xml.Linq.LoadOptions]::PreserveWhitespace)));
$Namespace = $ModelDefinitionsDocument.Root.Name.Namespace;
$SourceSelector = [DevUtil.XElementSelector]::new($ModelDefinitionsDocument.Root).Elements($Namespace.GetName('Resources')).Elements($Namespace.GetName('data'));
$ValueXName = $Namespace.GetName('value');
$SourceDictionary = [System.Collections.Generic.Dictionary[string, [System.Xml.Linq.XElement]]]::new([System.StringComparer]::OrdinalIgnoreCase);
foreach ($SourceElement in $SourceSelector.GetItems()) {
    $Name = $SourceElement.Attribute('name').Value;
    $ValueElement = $SourceElement.Element($ValueXName);
    if ($null -ne $ValueElement) {
        $Key = $ValueElement.Value -replace '\W+', '';
        if ($SourceDictionary.ContainsKey($Key)) {
            Write-Warning -Message "$Name is a possible duplicate of $($SourceDictionary[$Key].Attribute('name').Value)";
        } else {
            $SourceDictionary.Add($Key, $SourceElement);
        }
    } else {
        Write-Warning -Message "$Name has no value in source";
    }
}
$ResourcesDocument = [System.Xml.Linq.XDocument]::Load($ResourcesPath);
$NameCollection = @($ResourcesDocument.Root.Elements('data') | ForEach-Object {
    $TargetName = $_.Attribute('name').Value
    $ValueElement = $_.Element('value');
    if ($null -ne $ValueElement) {
        $Key = $ValueElement.Value -replace '\W+', '';
        if ($SourceDictionary.ContainsKey($Key)) {
            $SourceElement = $SourceDictionary[$Key];
            $SourceName = $SourceElement.Attribute('name').Value;
            $SourceName | Write-Output;
            if ($SourceName -cne $TargetName) {
                $_.Add(([System.Xml.Linq.XElement]::new([System.Xml.Linq.XName]::Get('comment'),
                    "TODO: Rename to $SourceName"
                )));
            }
        } else {
            $_.Add(([System.Xml.Linq.XElement]::new([System.Xml.Linq.XName]::Get('comment'),
                "TODO: Ensure resource is still needed"
            )));
        }
    }
});
foreach ($SourceElement in $SourceSelector.GetItems()) {
    $Name = $SourceElement.Attribute('name').Value;
    if ($NameCollection -cnotcontains $Name) {
        $ValueElement = $SourceElement.Element($ValueXName);
        if ($null -ne $ValueElement) {
            $ResourcesDocument.Root.Add(([System.Xml.Linq.XElement]::new([System.Xml.Linq.XName]::Get('data'),
                [System.Xml.Linq.XAttribute]::new('name', $Name),
                [System.Xml.Linq.XElement]::new([System.Xml.Linq.XName]::Get('value'),
                    $ValueElement.Value
                ),
                [System.Xml.Linq.XElement]::new([System.Xml.Linq.XName]::Get('comment'),
                    "TODO: Ensure this not equivalent to another"
                )
            )))
        }
    }
}
#<#
[System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($ResourcesPath, [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
    IndentChars = '    ';
});
try {
    $ResourcesDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}
#>
<#
[System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($ModelDefinitionsPath, [System.Xml.XmlWriterSettings]@{
    #Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
    #IndentChars = '    ';
});
try {
    $ModelDefinitionsDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}
#>
