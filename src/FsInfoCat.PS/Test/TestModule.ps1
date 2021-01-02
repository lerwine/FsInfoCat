Import-Module -Name (($PSScriptRoot | Join-Path -ChildPath '..\..\..\Debug\Desktop\FsInfoCat')) -ErrorAction Stop -Verbose -Debug -InformationAction Continue -WarningAction Continue;

$PwHash = [FsInfoCat.PwHash]::Import('XrVxbJmeSwCzNtQp6PpepBE8WUh1nfhPGlQCBoIOaS6ho6+7UH/lGnJgH0w2hF2oFzxDrQ6liVHWDldtFawAetBTWV+jCsFG');
if ($null -eq $PwHash) {
    Write-Error -Message 'Error parsing base-64 hash' -Category InvalidResult -ErrorId 'PwHash' -ErrorAction Stop;
}
