Set-Variable -Name 'TypeDefinitions' -Option Constant -Scope 'Script' -Value ([Xml]::new());
$Script:TypeDefinitions.Load(($PSScriptRoot | Join-Path -ChildPath 'TypeDefinitions.xml'));

Function Get-TypeNames {
    [CmdletBinding(DefaultParameterSetName = 'AllTypes')]
    param (
        [ValidatePattern('^[a-z_A-Z][\da-z_A-Z]*(\.[a-z_A-Z][\da-z_A-Z]*)*$')]
        [string[]]$Namespace,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Entity,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Interface,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Struct,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Class,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Enum,

        [Parameter(ParameterSetName = 'AllTypes')]
        [switch]$AllTypes
    )

    Process {
        $XPath = $null;
        if ($PSBoundParameters.ContainsKey('Namespace')) {
            $XPath = @("Namespace[@Name=`"$($Namespace[0])`"");
            ($Namespace | Select-Object -Skip 1) | ForEach-Object { $XPath = @("$($XPath[0])[or @Name=`"$_`"") }
            $XPath = @("$($XPath[0])]");
        } else {
            $XPath = @('Namespace');
        }
        if ($PSCmdlet.ParameterSetName -eq 'SpecificTypes') {
            $ElementNames = @();
            if ($Entity.IsPresent) { $ElementNames = @('Entity') }
            if ($Interface.IsPresent) { $ElementNames += @('Interface') }
            if ($Struct.IsPresent) { $ElementNames += @('Struct') }
            if ($Class.IsPresent) { $ElementNames += @('Class') }
            if ($Enum.IsPresent) { $ElementNames += @('Enum') }
            if ($ElementNames.Count -eq 5) {
                $XPath = @("$($XPath[0])/*");
            } else {
                $XPath = @($ElementNames | ForEach-Object { "$XPath/$_" });
            }
        } else {
            $XPath = @("$($XPath[0])/*");
        }
        @($Script:TypeDefinitions.DocumentElement.SelectNodes((($XPath | ForEach-Object { "$_/@Name" }) -join '|'))) | ForEach-Object { $_.Value | Write-Output }
    }
}
