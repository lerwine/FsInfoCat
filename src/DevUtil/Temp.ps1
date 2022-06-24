Param(
    [Type[]]$Type = @([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax])
)

$StringBuilder = [System.Text.StringBuilder]::new();

foreach ($CurrentType in $Type) {
    $Inherited = @();
    if (-not $CurrentType.IsSealed) { $Inherited = @($CurrentType.Assembly.GetTypes() | Where-Object { $_.BaseType -eq $CurrentType }) }
    $OmitNames = @($CurrentType.BaseType.GetProperties() | Where-Object {
        -not $_.GetGetMethod().IsStatic
    } | ForEach-Object { $_.Name });
    $StringBuilder.Append('Function Import-').Append(($CurrentType.Name -replace 'Syntax$', '')).AppendLine(' {') | Out-Null;
    if ($Inherited.Count -eq 0) {
        $StringBuilder.AppendLine('    [CmdletBinding()]').AppendLine('    Param(').AppendLine('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]') | Out-Null;
        $StringBuilder.Append('        [').Append($CurrentType.FullName).AppendLine(']$Syntax,').AppendLine('') | Out-Null;
        $StringBuilder.AppendLine('        [Parameter(Mandatory = $true)]').AppendLine('        [System.Xml.XmlElement]$ParentElement').AppendLine('    )').AppendLine('') | Out-Null;
        $StringBuilder.AppendLine('    Begin { $OwnerDocument = $ParentElement.OwnerDocument }').AppendLine('').AppendLine('    Process {') | Out-Null;
        $StringBuilder.Append('        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement(''').Append(($CurrentType.Name -replace '(Declaration)?(Syntax)?$', '')) | Out-Null;
        $StringBuilder.AppendLine('''));').AppendLine('').Append('        Import-').Append(($CurrentType.BaseType.Name -replace 'Syntax$', '')).AppendLine(' -Syntax $Syntax -MemberElement $MemberElement;') | Out-Null;
        $CurrentType.GetProperties() | Where-Object { $OmitNames -cnotcontains $_.Name -and -not $_.GetGetMethod().IsStatic } | ForEach-Object {
            $StringBuilder.AppendLine('').Append('        # [').Append($_.PropertyType.FullName).AppendLine(']').Append('        # Import-Type -Syntax $Syntax.').Append($_.Name) | Out-Null;
            $StringBuilder.AppendLine('PropertyName -ParentElement $MemberElement;') | Out-Null;
        }
    } else {
        $StringBuilder.Append("    # [") | Out-Null;
        $Inherited | ForEach-Object { $StringBuilder.Append($_.FullName).Append('], [') } | Out-Null;
        $StringBuilder.AppendLine(']').AppendLine('    [CmdletBinding(DefaultParameterSetName = ''ToParent'')]').AppendLine('    Param(') | Out-Null;
        $StringBuilder.AppendLine('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]').Append('        [').Append($CurrentType.FullName).AppendLine(']$Syntax,').AppendLine('') | Out-Null;
        $StringBuilder.AppendLine('        [Parameter(Mandatory = $true, ParameterSetName = ''ToParent'')]').AppendLine('        [System.Xml.XmlElement]$ParentElement,').AppendLine('') | Out-Null;
        $StringBuilder.AppendLine('        [Parameter(Mandatory = $true, ParameterSetName = ''ToMember'')]').AppendLine('        [System.Xml.XmlElement]$MemberElement,').AppendLine('') | Out-Null;
        $StringBuilder.AppendLine('        [Parameter(ParameterSetName = ''ToMember'')]').AppendLine('        [switch]$IsUnknown').AppendLine('    )').AppendLine('').AppendLine('    Process {') | Out-Null;
        $StringBuilder.AppendLine('        if ($PSCmdlet.ParameterSetName -eq ''ToParent'') {').AppendLine('            switch ($Syntax) {') | Out-Null;
        $Inherited | ForEach-Object {
            $StringBuilder.Append('                # { $_ -is [').Append($_.FullName).Append('] } { Import-').Append(($_.Name -replace 'Syntax$', '')) | Out-Null;
            $StringBuilder.AppendLine(' -Syntax $Syntax -ParentElement $ParentElement; break; }') | Out-Null;
        }
        $StringBuilder.AppendLine('                default {').Append('                    Import-').Append(($CurrentType.Name -replace 'Syntax$', '')) | Out-Null;
        $StringBuilder.Append(' -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement(''Unknown') | Out-Null;
        $StringBuilder.Append(($CurrentType.Name -replace '(Declaration)?(Syntax)?$', '')).AppendLine('''))) -IsUnknown;').AppendLine('                    break;').AppendLine('                }') | Out-Null;
        $StringBuilder.AppendLine('            }').AppendLine('        } else {').AppendLine('            if ($IsUnknown.IsPresent) {').Append('                Import-') | Out-Null;
        $StringBuilder.Append(($CurrentType.BaseType.Name -replace 'Syntax$', '')).AppendLine(' -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;').AppendLine('            } else {') | Out-Null;
        $StringBuilder.Append('                Import-').Append(($CurrentType.BaseType.Name -replace 'Syntax$', '')).AppendLine(' -Syntax $Syntax -MemberElement $MemberElement;').AppendLine('            }') | Out-Null;
        $CurrentType.GetProperties() | Where-Object { $OmitNames -cnotcontains $_.Name -and -not $_.GetGetMethod().IsStatic } | ForEach-Object {
            $StringBuilder.AppendLine('').Append('            # [').Append($_.PropertyType.FullName).Append(']$').Append($_.Name).AppendLine(';').Append('            # Import-Type -Syntax $Syntax.') | Out-Null;
            $StringBuilder.Append($_.Name).AppendLine(' -ParentElement $MemberElement;') | Out-Null;
        }
        $StringBuilder.AppendLine('        }') | Out-Null;
    }
    $StringBuilder.AppendLine('    }').AppendLine('}').AppendLine('') | Out-Null;
}
[System.IO.File]::WriteAllText(($PSScriptRoot | Join-Path -ChildPath 'Temp.txt'), $StringBuilder.ToString().TrimEnd());
