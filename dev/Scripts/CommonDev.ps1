<#
Add-Type -AssemblynName 'PresentationCore'
[System.Windows.Clipboard]::SetText([Guid]::NewGuid().ToString('b'));
[System.Windows.Clipboard]::SetText([Guid]::NewGuid().ToString('d'));
#>
