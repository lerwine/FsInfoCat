Param(
    [string[]]$Path = (
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat',
        #'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Desktop',
        #'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Local',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Upstream'
    ),
    [bool]$Recurse = $true
)
<#
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process;
#>
Import-Module -Name './CodeAnalysisUtil' -ErrorAction Stop;

[CodeAnalysisUtil.SourceFile[]]$SourceFiles = @();
if ($Recurse) {
    $SourceFiles = @($Path | Get-SourceFile -Recurse);
} else {
    $SourceFiles = @($Path | Get-SourceFile);
}
[long]$TotalBytes = 0;
$SourceFiles | % { $TotalBytes += $_.File.Length };
[CodeAnalysisUtil.LineComplianceInfo[]]$LineLimitData = $SourceFiles | Get-LineLengthCompliance;
if ($LineLimitData.Length -eq 0) {
    Write-Information -MessageData 'No files exceed line length limits.' -InformationAction Continue;
} else {
    $LineLimitData | % {
        $Uri = [Uri]::new($_.File.FullName, [System.UriKind]::Absolute).AbsoluteUri;
        if ($_.Violations.Count -eq 1) {
            "$Uri;`tLine: $($_.Violations[0].Number)" | Write-Output;
        } else {
            "$Uri;`tLines: $(($_.Violations | % { $_.Number }) -join ', ')" | Write-Output;
        }
    }
}
Write-Progress -Activity $ProgressActivity -Status 'Finished' -PercentComplete 100 -Completed;
#>
#Get-Variable
