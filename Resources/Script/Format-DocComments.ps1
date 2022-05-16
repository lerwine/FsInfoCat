Param(
    [string[]]$Path = (
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Desktop',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Local',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Upstream'
    ),
    [bool]$Recurse = $true
)

<#
$SourceFile = [System.IO.FileInfo]::new('C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat\Upstream\DataModel\IUpstreamTagDefinitionRow.cs')
$StreamReader = $SourceFile.OpenText();
$SourceCode = $StreamReader.ReadToEnd()
$StreamReader.Close();
$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($SourceCode, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $SourceFile.FullName)
$CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot()
$NamespaceDeclarationSyntax = @($CompilationUnitRoot.Members) | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } | Select-Object -First 1
$Definition = $NamespaceDeclarationSyntax.Members[0]
#>

$DocCommentBlockRegex = [System.Text.RegularExpressions.Regex]::new('([^\S\r\n]*///[^\r\n]*(\r\n?|\n))+', [System.Text.RegularExpressions.RegexOptions]::Compiled);
$DocCommentLineRegex = [System.Text.RegularExpressions.Regex]::new('(\s*///)\s?(.+)?', [System.Text.RegularExpressions.RegexOptions]::Compiled);


Import-Module -Name './CodeAnalysisUtil' -ErrorAction Stop;

[CodeAnalysisUtil.SourceFile[]]$SourceFiles = @();
if ($Recurse) {
    $SourceFiles = @($Path | Get-SourceFile -Recurse);
} else {
    $SourceFiles = @($Path | Get-SourceFile);
}
$SourceFiles | % {
    $SyntaxTree = $_.GetSyntaxTree();
    if ($null -ne $SyntaxTree) {
        foreach ($ns in (Get-NamespaceDeclaration -SourceFile $_)) {
            $ns | Format-MemberDocComments;
            (Get-TypeDeclarations -Namespace $ns -Interface) | Format-InterfaceDocComments;
            (Get-TypeDeclarations -Namespace $ns -Class) | Format-ClassDocComments;
            (Get-TypeDeclarations -Namespace $ns -Struct) | Format-StructDocComments;
            (Get-TypeDeclarations -Namespace $ns -Record) | Format-RecordDocComments;
            (Get-TypeDeclarations -Namespace $ns -Enum) | Format-EnumDocComments;
            (Get-TypeDeclarations -Namespace $ns -Declaration) | Format-DelegateDocComments;
        }
    }
}
Write-Progress -Activity $ProgressActivity -Status 'Finished' -PercentComplete 100 -Completed;
#>
#Get-Variable
