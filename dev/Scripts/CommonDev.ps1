<#
Add-Type -AssemblynName 'PresentationCore'
[System.Windows.Clipboard]::SetText([Guid]::NewGuid().ToString('b'));
[System.Windows.Clipboard]::SetText([Guid]::NewGuid().ToString('d'));
#>

Function ConvertTo-Binary {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [byte]$Value
    )

    Process {
        $Binary = '0';
        if (($Value -band 1) -eq 1) { $Binary = '1' }
        for ($i = 0; $i -lt 7; $i++) {
            if ((($Value = $Value -shr 1) -band 1) -eq 1) { $Binary = "1$Binary" } else { $Binary = "0$Binary" }
        }
        $Binary | Write-Output;
    }
}
