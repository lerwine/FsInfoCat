<#
if ($null -eq $Script:ModelDefinitionNames) {
    Set-Variable -Name 'ModelDefinitionNames' -Option Constant -Scope 'Script' -Value ([System.Management.Automation.PSObject]@{
        _ns = [System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xs')
    });
    ('ModelDefinitions', 'Sources', 'Source', 'LeadingTrivia', 'TrailingTrivia', 'UnknownSyntax', 'UnknownMemberDeclaration', 'Namespace', 'UnknownNamespaceDeclaration', 'Interface', 'Class', 'Struct', 'Enum',
            'Member', 'Property', 'UnknownPropertyDeclaration', 'Field', 'EventField', 'EventProperty', 'UnknownFieldDeclaration', 'UnknownBaseTypeDeclaration', 'UnknownTypeDeclaration', 'Method',
            'UnknownMethodDeclaration', 'Destructor', 'Constructor', 'Operator', 'ConversionOperator', 'Indexer', 'Delegate', 'GlobalStatement', 'IncompleteMember', 'AttributeList', 'Attribute',
            'Target') | ForEach-Object {
        $Script:ModelDefinitionNames | Add-Member -MemberType NoteProperty -Name $_ -Value $Script:ModelDefinitionNames._ns.GetName($_);
    }
} else {
    ('ModelDefinitions', 'Sources', 'Source', 'LeadingTrivia', 'TrailingTrivia', 'UnknownSyntax', 'UnknownMemberDeclaration', 'Namespace', 'UnknownNamespaceDeclaration', 'Interface', 'Class', 'Struct', 'Enum',
            'Member', 'Property', 'UnknownPropertyDeclaration', 'Field', 'EventField', 'EventProperty', 'UnknownFieldDeclaration', 'UnknownBaseTypeDeclaration', 'UnknownTypeDeclaration', 'Method',
            'UnknownMethodDeclaration', 'Destructor', 'Constructor', 'Operator', 'ConversionOperator', 'Indexer', 'Delegate', 'GlobalStatement', 'IncompleteMember', 'AttributeList', 'Attribute',
            'Target') | ForEach-Object {
        if ($null -eq $Script:ModelDefinitionNames.($_)) {
            $Script:ModelDefinitionNames | Add-Member -MemberType NoteProperty -Name $_ -Value $Script:ModelDefinitionNames._ns.GetName($_);
        }
    }
}
#>
Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath 'bin/Debug/net6.0-windows/DevHelper') -ErrorAction Stop;

$Type = [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax];
$et = @(Get-ExtendingTypes -Type $Type -Directly -ErrorAction Ignore);
if ($et.Count -gt 0) {
    "        switch (`$$($Type.Name -replace 'Syntax$', '')) {"
    $et | ForEach-Object {
        "            # { `$_ -is [$($_.FullName)] } { (Import-$($_.Name) -$($_.Name -replace 'Syntax$', '') `$$($Type.Name -replace 'Syntax$', '')) | Write-Output; break; }"
    }
    @"
            default {
                `$XElement = [System.Xml.Linq.XElement]::new(`$Script:ModelDefinitionNames.Unknown$($Type.Name));
                Set-$($Type.Name)Contents -$($Type.Name -replace 'Syntax$', '')` `$$($Type.Name -replace 'Syntax$', '') -Element `$XElement -IsUnknown;
                Write-Output -InputObject `$XElement -NoEnumerate;
                break;
            }
        }
"@
}
'    <#';
(Get-TypeMemberInfo -Type $Type -IgnoreInherited -ErrorAction Stop) | % { "    $_" };
'    #>';

<#
[Xml]$XmlDocument = '<EntityData/>';
$BasePath = [System.IO.Path]::GetDirectoryName($PSScriptRoot);
[System.IO.Directory]::GetFiles([System.IO.Path]::Combine($BasePath, 'FsInfoCat\Model'), '*.cs') | ForEach-Object {
    $FileInfo = [System.IO.FileInfo]::new($_);
    $StreamReader = $FileInfo.OpenText();
    try { $Text = $StreamReader.ReadToEnd(); }
    finally { $StreamReader.Close(); }
    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
    $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
    if ($null -ne $CompilationUnitRoot) {
        Import-CompilationUnit -Syntax $CompilationUnitRoot -Document $XmlDocument -RelativePath ([System.IO.Path]::GetRelativePath($BasePath, $_)) -ErrorAction Stop;
    }
}

[System.IO.Directory]::GetFiles([System.IO.Path]::Combine($BasePath, 'FsInfoCat\Local'), '*.cs') | ForEach-Object {
    $FileInfo = [System.IO.FileInfo]::new($_);
    $StreamReader = $FileInfo.OpenText();
    try { $Text = $StreamReader.ReadToEnd(); }
    finally { $StreamReader.Close(); }
    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
    $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
    if ($null -ne $CompilationUnitRoot) {
        Import-CompilationUnit -Syntax $CompilationUnitRoot -Document $XmlDocument -RelativePath ([System.IO.Path]::GetRelativePath($BasePath, $_)) -ErrorAction Stop;
    }
}

[System.IO.Directory]::GetFiles([System.IO.Path]::Combine($BasePath, 'FsInfoCat\Upstream'), '*.cs') | ForEach-Object {
    $FileInfo = [System.IO.FileInfo]::new($_);
    $StreamReader = $FileInfo.OpenText();
    try { $Text = $StreamReader.ReadToEnd(); }
    finally { $StreamReader.Close(); }
    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
    $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
    if ($null -ne $CompilationUnitRoot) {
        Import-CompilationUnit -Syntax $CompilationUnitRoot -Document $XmlDocument -RelativePath ([System.IO.Path]::GetRelativePath($BasePath, $_)) -ErrorAction Stop;
    }
}

$XmlWriter = [System.Xml.XmlWriter]::Create([System.IO.Path]::Combine($BasePath, 'DevUtil\TypeDefinitions.xml'), [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false)
});
try {
    $XmlDocument.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
#>
