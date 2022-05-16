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
class LineLimitInfo {
    [string]$FullName;
    [int[]]$LineNumbers;
    LineLimitInfo([string]$FullName) {
        $this.FullName = $FullName;
        $lc = @((Get-Content -LiteralPath $FullName) | % { $_.Length });
        $this.LineNumbers = @(for ($i = 0; $i -lt $lc.Count; $i++) { if ($lc[$i] -gt 180) { $i + 1 } });
    }
}
$ProgressActivity = [System.IO.Path]::GetFileNameWithoutExtension($MyInvocation.MyCommand.Source).Replace('-', ' ');
Write-Progress -Activity $ProgressActivity -Status 'Getting File Paths' -PercentComplete 0;
[System.IO.FileInfo[]]$FileInfos = @();
if ($Recurse) {
    $FileInfos = @(Get-ChildItem -LiteralPath $Path -Filter '*.cs' -Exclude '*.Designer.cs' -File -Recurse);
} else {
    $FileInfos = @(Get-ChildItem -LiteralPath $Path -Filter '*.cs' -Exclude '*.Designer.cs' -File);
}
[long]$TotalBytes = 0;
$FileInfos | % { $TotalBytes += $_.Length };
$TotalCount = $FileInfos.Count;
$PercentComplete = 0;
$DirectoryPath = '';
$ItemIndex = -1;
[long]$BytesCompleted = 0;
[long]$TotalResultCount = 0;
[LineLimitInfo[]]$LineLimitData = $FileInfos | % {
    $ItemIndex++;
    [int]$pc = ($BytesCompleted * 100) / $TotalBytes;
    if ($DirectoryPath -ne $_.FullName -or $PercentComplete -ne $pc) {
        $PercentComplete = $pc;
        $DirectoryPath = $_.FullName;
        Write-Progress -Activity $ProgressActivity -Status "Checking file $($ItemIndex + 1) of $TotalCount" -PercentComplete $pc -CurrentOperation $DirectoryPath;
    }
    $LineLimitInfo = [LineLimitInfo]::new($_.FullName);
    if ($LineLimitInfo.LineNumbers.Length -gt 0) {
        $TotalResultCount += $LineLimitInfo.LineNumbers.Length;
        $LineLimitInfo | Write-Output;
    }
    $BytesCompleted += $_.Length;
}
if ($LineLimitData.Length -eq 0) {
    Write-Information -MessageData 'No files exceed line length limits.' -InformationAction Continue;
} else {
    $LineLimitData | % {
        $Uri = [Uri]::new($_.FullName, [System.UriKind]::Absolute).AbsoluteUri;
        if ($_.LineNumbers.Length -eq 1) {
            "$Uri;`tLine: $($_.LineNumbers[0])" | Write-Output;
        } else {
            "$Uri;`tLines: $($_.LineNumbers -join ', ')" | Write-Output;
        }
    }
}
Write-Progress -Activity $ProgressActivity -Status 'Finished' -PercentComplete 100 -Completed;
#>
#Get-Variable
