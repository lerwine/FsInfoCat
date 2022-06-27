Param(
    [Type[]]$Type = @([Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax])
)

Function Get-PropertyInfo {
    Param([Type]$Type)
    $Names = @($Type.BaseType.GetProperties() | ? { -not $_.GetGetMethod().IsStatic } | % { $_.Name });
    $Enumerable = [System.Collections.Generic.IEnumerable`1];
    $Type.GetProperties() | ? { $Names -cnotcontains $_.Name -and -not $_.GetGetMethod().IsStatic } | % {
        "        # [$($_.PropertyType.FullName)]`$$($_.Name) # IsValueType = $($_.PropertyType.IsValueType)";
        if ($_.PropertyType.IsConstructedGenericType) {
            $_.PropertyType.GetGenericArguments() | % {
                "        #     [$($_.FullName)] # IsValueType = $($_.IsValueType)";
            }
        }
        $_.PropertyType.GetInterfaces() | ? { $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq $Enumerable } | % {
            $_.GetGenericArguments() | % { "        #     Enumerable: $($_.FullName)" }
        }
    }
}

$CSharpAssembly = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].Assembly;

$Writer = [System.IO.StringWriter]::new();
foreach ($CurrentType in $Type) {
    $InheritingTypes = @();
    if (-not $CurrentType.IsSealed) {
        $InheritingTypes = @($CurrentType.Assembly.GetTypes() | Where-Object { $_.BaseType -eq $CurrentType });
        if ($CurrentType.Assembly.FullName -cne $CSharpAssembly.FullName) {
            $InheritingTypes = @($InheritingTypes + @($CSharpAssembly.GetTypes() | Where-Object { $_.BaseType -eq $CurrentType }));
        }
    }
    $BasePropertyNames = @($CurrentType.BaseType.GetProperties() | Where-Object {
        -not $_.GetGetMethod().IsStatic
    } | ForEach-Object { $_.Name });
    $CurrentProperties = @($CurrentType.GetProperties() | Where-Object { $BasePropertyNames -cnotcontains $_.Name -and -not $_.GetGetMethod().IsStatic });
    $NounName = $CurrentType.Name -Replace 'Syntax$', '';
    $ElementName = $NounName -replace 'Declaration$', '';
    $ParentNounName = $CurrentType.BaseType.Name -Replace 'Syntax$', '';
    $Writer.WriteLine('');
    $Writer.Write('Function Import-');
    $Writer.Write($NounName);
    $Writer.WriteLine(' {');
    if ($InheritingTypes.Count -gt 0) {
        $Writer.Write('    # [');
        $Writer.Write($InheritingTypes[0].FullName);
        ($InheritingTypes | Select-Object -Skip 1) | ForEach-Object {
            $Writer.Write('], [');
            $Writer.Write($_.FullName);
        }
        $Writer.WriteLine(']');
        $Writer.WriteLine('    [CmdletBinding(DefaultParameterSetName = ''ToParent'')]');
        $Writer.WriteLine('    Param(');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
        $Writer.Write('        [');
        $Writer.Write($CurrentType.FullName);
        $Writer.WriteLine(']$Syntax,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ParameterSetName = ''ToParent'')]');
        $Writer.WriteLine('        [System.Xml.XmlElement]$ParentElement,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ParameterSetName = ''ToMember'')]');
        $Writer.WriteLine('        [System.Xml.XmlElement]$MemberElement,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(ParameterSetName = ''ToParent'')]');
        $Writer.WriteLine('        [string]$ElementName,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(ParameterSetName = ''ToMember'')]');
        $Writer.WriteLine('        [switch]$IsUnknown');
        $Writer.WriteLine('    )');
        $Writer.WriteLine('');
        $Writer.WriteLine('    Process {');
        $Writer.WriteLine('        if ($PSCmdlet.ParameterSetName -eq ''ToParent'') {');
        $Writer.WriteLine('            switch ($Syntax) {');
        $InheritingTypes | ForEach-Object {
                $Writer.Write('                # { $_ -is [');
            $Writer.Write($_.FullName);
            $Writer.WriteLine('] } {');
            $Writer.WriteLine('                #     if ($PSBoundParameters.ContainsKey(''ElementName'')) {');
                $Writer.Write('                #         Import-');
            $Writer.Write(($_.Name -Replace 'Syntax$', ''));
            $Writer.WriteLine(' -Syntax $Syntax -ParentElement $ParentElement -ElementName $ElementName;');
            $Writer.WriteLine('                #     } else {');
                $Writer.Write('                #         Import-');
            $Writer.Write(($_.Name -Replace 'Syntax$', ''));
            $Writer.WriteLine(' -Syntax $Syntax -ParentElement $ParentElement;');
            $Writer.WriteLine('                #     }');
            $Writer.WriteLine('                #     break;');
            $Writer.WriteLine('                # }');
        }
        $Writer.WriteLine('                default {');
        $Writer.WriteLine('                    if ($PSBoundParameters.ContainsKey(''ElementName'')) {');
        $Writer.Write('                        Import-');
        $Writer.Write($NounName);
        $Writer.Write(' -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement($ElementName)).AppendChild($ParentElement.OwnerDocument.CreateElement(''Unknown');
        $Writer.Write($ElementName);
        $Writer.WriteLine('''))) -IsUnknown;');
        $Writer.WriteLine('                    } else {');
        $Writer.Write('                        Import-');
        $Writer.Write($NounName);
        $Writer.Write(' -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement(''Unknown');
        $Writer.Write($ElementName);
        $Writer.WriteLine('''))) -IsUnknown;');
        $Writer.WriteLine('                    }');
        $Writer.WriteLine('                    break;');
        $Writer.WriteLine('                }');
        $Writer.WriteLine('            }');
        $Writer.WriteLine('        } else {');
        $Writer.WriteLine('            if ($IsUnknown.IsPresent) {');
        $Writer.Write('                Import-');
        $Writer.Write($ParentNounName);
        $Writer.WriteLine(' -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;');
        $Writer.WriteLine('            } else {');
        $Writer.Write('                Import-');
        $Writer.Write($ParentNounName);
        $Writer.WriteLine(' -Syntax $Syntax -MemberElement $MemberElement;');
        $Writer.WriteLine('            }');
        $CurrentProperties | ForEach-Object {
            $Writer.WriteLine('');
            $Writer.Write('            # [');
            $Writer.Write($_.PropertyType.FullName);
            $Writer.Write(']$');
            $Writer.Write($_.Name);
            $Writer.Write('; # IsValueType = ');
            $Writer.WriteLine($_.PropertyType.IsValueType.ToString());
            $Writer.Write('            # Import-');
            $Writer.Write(($_.PropertyType.Name -replace 'Syntax$', ''));
            $Writer.Write(' -Syntax $Syntax.');
            $Writer.Write($_.Name);
            $Writer.WriteLine(' -ParentElement $MemberElement;');
        }
        $Writer.WriteLine('        }');
    } else {
        $Writer.WriteLine('    [CmdletBinding()]');
        $Writer.WriteLine('    Param(');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
        $Writer.Write('        [');
        $Writer.Write($CurrentType.FullName);
        $Writer.WriteLine(']$Syntax,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(Mandatory = $true)]');
        $Writer.WriteLine('        [System.Xml.XmlElement]$ParentElement,');
        $Writer.WriteLine('');
        $Writer.Write('        [string]$ElementName = ''');
        $Writer.Write($ElementName);
        $Writer.WriteLine('''');
        $Writer.WriteLine('    )');
        $Writer.WriteLine('');
        $Writer.WriteLine('    Begin { $OwnerDocument = $ParentElement.OwnerDocument }');
        $Writer.WriteLine('');
        $Writer.WriteLine('    Process {');
        $Writer.WriteLine('        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement($ElementName));');
        $Writer.WriteLine('');
        $Writer.Write('        Import-');
        $Writer.Write($ParentNounName);
        $Writer.WriteLine(' -Syntax $Syntax -MemberElement $MemberElement;');
        $CurrentProperties | ForEach-Object {
            $Writer.WriteLine('');
            $Writer.Write('        # [');
            $Writer.Write($_.PropertyType.FullName);
            $Writer.Write(']$');
            $Writer.Write($_.Name);
            $Writer.Write('; # IsValueType = ');
            $Writer.WriteLine($_.PropertyType.IsValueType.ToString());
            $Writer.Write('        # Import-');
            $Writer.Write(($_.PropertyType.Name -replace 'Syntax$', ''));
            $Writer.Write(' -Syntax $Syntax.');
            $Writer.Write($_.Name);
            $Writer.WriteLine(' -ParentElement $MemberElement;');
        }
    }
    $Writer.WriteLine('    }');
    $Writer.WriteLine('}');
}
[System.IO.File]::WriteAllText(($PSScriptRoot | Join-Path -ChildPath 'Temp.txt'), $Writer.ToString().TrimEnd());
