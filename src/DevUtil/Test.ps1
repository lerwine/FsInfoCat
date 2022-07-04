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

Function Export-PsCode {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$CurrentType,

        [Parameter(Mandatory = $true)]
        [Type[]]$AllTypes,

        [Parameter(Mandatory = $true)]
        [AllowEmptyCollection()]
        [AllowEmptyString()]
        [System.Collections.ObjectModel.Collection[string]]$CodeLines,

        [Parameter(Mandatory = $true)]
        [string]$SetCommand
    )

    Process {
        Write-Host -Object "Importing $($CurrentType.FullName)" -ForegroundColor Cyan;
        $FirstLine = "Function Import-$($CurrentType.Name) {";
        if (-not $CodeLines.Contains($FirstLine)) {
            $BaseTypes = [System.Collections.ObjectModel.Collection[Type]]::new();
            if ($null -ne $CurrentType.BaseType) {
                for ($Type = $CurrentType.BaseType; $null -ne $Type.BaseType; $Type = $Type.BaseType) { $BaseTypes.Add($Type) }
            }
            [Type[]]$ExtendingTypes = @();
            if (-not $CurrentType.IsSealed) { [Type[]]$ExtendingTypes = @($AllTypes | Where-Object { $_.BaseType -eq $CurrentType }) }
            [System.Reflection.PropertyInfo[]]$Properties = @($CurrentType.GetProperties() | Where-Object {
                $m = $_.GetGetMethod();
                if ($null -eq $m -or $m.IsStatic) { return $false }
                if ($_.PropertyType.IsConstructedGenericType) {
                    $a = $_.PropertyType.GetGenericArguments();
                    return $a.Length -eq 1 -and -not $a[0].IsGenericType;
                }
                return -not $_.PropertyType.IsGenericType;
            } | Where-Object { $_.DeclaringType -eq $CurrentType });
            if ($ExtendingTypes.Length -gt 0) {
                $Name = ($CurrentType.Name -replace 'Syntax$', '') -replace 'Declaration$', '';
                $Name | Write-Output;
                "Unknown$($CurrentType.Name)" | Write-Output;
                if ($Properties.Length -gt 0) {
                    $ExtendingTypes | Export-PsCode -AllTypes $AllTypes -CodeLines $CodeLines -SetCommand "Set-$($CurrentType.Name)Contents -$Name";
                } else {
                    $ExtendingTypes | Export-PsCode -AllTypes $AllTypes -CodeLines $CodeLines -SetCommand $SetCommand;
                }
                $CodeLines.Add('');
                $CodeLines.Add($FirstLine);
                $CodeLines.Add('    [CmdletBinding()]');
                $CodeLines.Add('    [OutputType([System.Xml.Linq.XElement])]');
                $CodeLines.Add('    Param(');
                $CodeLines.Add('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
                $CodeLines.Add("        [$($CurrentType.FullName)]`$$Name");
                $CodeLines.Add('    )');
                $CodeLines.Add('');
                $CodeLines.Add('    Process {');
                $CodeLines.Add("        switch (`$$Name) {");
                $ExtendingTypes | ForEach-Object {
                    $CodeLines.Add("            { `$_ -is [$($_.FullName)] } { (Import-$($_.Name) -$(($_.Name -replace 'Syntax$', '') -replace 'Declaration$', '') `$$Name) | Write-Output; break; }");
                }
                [System.Reflection.MethodInfo[]]$Methods = @($CurrentType.GetMethods() | Where-Object { $_.DeclaringType -eq $CurrentType -and -not ($_.IsSpecialName -or $_.IsStatic -or $_.ReturnType -eq [Void]) });
                $CodeLines.Add('            default {');
                $CodeLines.Add("                `$Element = [System.Xml.Linq.XElement]::new(`$Script:ModelDefinitionsNS.GetName('$Name'));");
                $CodeLines.Add("                Set-$($CurrentType.Name)Contents -$Name `$$Name -Element `$Element -IsUnknown;");
                $CodeLines.Add('                Write-Output -InputObject $Element -NoEnumerate;');
                $CodeLines.Add('                break;');
                $CodeLines.Add('            }');
                $CodeLines.Add('        }');
                if ($Properties.Length -gt 0) {
                    $CodeLines.Add('    }');
                    $CodeLines.Add('}');
                    $CodeLines.Add('');
                    $CodeLines.Add("Function Set-$($CurrentType.Name)Contents {");
                    $CodeLines.Add('    [CmdletBinding()]');
                    $CodeLines.Add('    [OutputType([System.Xml.Linq.XElement])]');
                    $CodeLines.Add('    Param(');
                    $CodeLines.Add('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
                    $CodeLines.Add("        [$($CurrentType.FullName)]`$$Name,");
                    $CodeLines.Add('');
                    $CodeLines.Add('        [Parameter(Mandatory = $true)]');
                    $CodeLines.Add('        [System.Xml.Linq.XElement]$Element,');
                    $CodeLines.Add('');
                    $CodeLines.Add('        [switch]$IsUnknown');
                    $CodeLines.Add('    )');
                    $CodeLines.Add('');
                    $CodeLines.Add('    if ($IsUnknown.IsPresent) {');
                    $CodeLines.Add("        $SetCommand `$$Name -Element `$Element -IsUnknown;");
                    $CodeLines.Add('    } else {');
                    $CodeLines.Add("        $SetCommand `$$Name -Element `$Element;");
                    $CodeLines.Add('    }');
                    $CodeLines.Add('');
                    $CodeLines.Add('    <#');
                    $e = $BaseTypes.Count - 1;
                    if ($e -eq 0) {
                        $CodeLines.Add("        BaseType: $($BaseTypes[0].FullName)");
                        $CodeLines.Add('');
                    } else {
                        if ($e -gt 0) {
                            $CodeLines.Add("        BaseType: $($BaseTypes[0].FullName) :");
                            for ($i = 0; $i -lt $e; $i++) {
                                $CodeLines.Add("            $($BaseTypes[$i].FullName),");
                            }
                            $CodeLines.Add("            $($BaseTypes[$e].FullName)");
                            $CodeLines.Add('');
                        }
                    }
                    $Properties | ForEach-Object { $CodeLines.Add("        $($_.ToString())") }
                    if ($Methods.Length -gt 0) {
                        $CodeLines.Add('');
                        $Methods | ForEach-Object { $CodeLines.Add("        $($_.ToString())") }
                    }
                    $CodeLines.Add('    #>');
                    $CodeLines.Add('}');
                } else {
                    if ($Methods.Length -gt 0) {
                        $CodeLines.Add('');
                        $CodeLines.Add('        <#');
                        $e = $BaseTypes.Count - 1;
                        if ($e -eq 0) {
                            $CodeLines.Add("            BaseType: $($BaseTypes[0].FullName)");
                        } else {
                            if ($e -gt 0) {
                                $CodeLines.Add("            BaseType: $($BaseTypes[0].FullName) :");
                                for ($i = 0; $i -lt $e; $i++) {
                                    $CodeLines.Add("                $($BaseTypes[$i].FullName),");
                                }
                                $CodeLines.Add("            $($BaseTypes[$e].FullName)");
                            }
                        }
                        if ($Methods.Length -gt 0) {
                            if ($e -ge 0) { $CodeLines.Add('') }
                            $Methods | ForEach-Object { $CodeLines.Add("            $($_.ToString())") };
                        }
                        $CodeLines.Add('        #>');
                    }
                    $CodeLines.Add('    }');
                    $CodeLines.Add('}');
                }
            } else {
                if ($Properties.Length -gt 0) {
                    $CodeLines.Add('');
                    $CodeLines.Add($FirstLine);
                    $Name = ($CurrentType.Name -replace 'Syntax$', '') -replace 'Declaration$', '';
                    $Name | Write-Output;
                    $CodeLines.Add('    [CmdletBinding()]');
                    $CodeLines.Add('    [OutputType([System.Xml.Linq.XElement])]');
                    $CodeLines.Add('    Param(');
                    $CodeLines.Add('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
                    $CodeLines.Add("        [$($CurrentType.FullName)]`$$Name");
                    $CodeLines.Add('    )');
                    $CodeLines.Add('');
                    $CodeLines.Add('    Process {');
                    $CodeLines.Add("        `$Element = [System.Xml.Linq.XElement]::new(`$Script:ModelDefinitionsNS.GetName('$Name'));");
                    $CodeLines.Add("        $SetCommand `$$Name -Element `$Element;");
                    $CodeLines.Add('        Write-Output -InputObject $Element -NoEnumerate;');
                    $CodeLines.Add('        <#');
                    $e = $BaseTypes.Count - 1;
                    if ($e -eq 0) {
                        $CodeLines.Add("        BaseType: $($BaseTypes[0].FullName)");
                    } else {
                        if ($e -gt 0) {
                            $CodeLines.Add("        BaseType: $($BaseTypes[0].FullName) :");
                            for ($i = 0; $i -lt $e; $i++) {
                                $CodeLines.Add("            $($BaseTypes[$i].FullName),");
                            }
                            $CodeLines.Add("            $($BaseTypes[$e].FullName)");
                        }
                    }
                    if ($Properties.Length -gt 0) {
                        if ($e -ge 0) { $CodeLines.Add('') }
                        $Properties | ForEach-Object { $CodeLines.Add("            $($_.ToString())") };
                    }
                    [System.Reflection.MethodInfo[]]$Methods = @($CurrentType.GetMethods() | Where-Object { $_.DeclaringType -eq $CurrentType -and -not ($_.IsSpecialName -or $_.IsStatic -or $_.ReturnType -eq [Void]) });
                    if ($Methods.Length -gt 0) {
                        if ($Properties.Length -gt 0) { $CodeLines.Add('') }
                        $Methods | ForEach-Object { $CodeLines.Add("            $($_.ToString())") };
                    }
                    $CodeLines.Add('        #' + '>');
                    $CodeLines.Add('    }');
                    $CodeLines.Add('}');
                }
            }
        }
    }
}

;
$XmlPath = $PSScriptRoot | Join-Path -ChildPath 'ModelDefinitions.xml';
<#
$ModelDefinitionsDocument = New-ModelDefinitionDocument;
('Model\*.cs', 'Local\Model\*.cs', 'Upstream\Model\*.cs', 'DbConstants.cs') | Import-SourceFile -BasePath ($PSScriptRoot | Join-Path -ChildPath '../FsInfoCat') -ModelDefinitions $ModelDefinitionsDocument;
[System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($XmlPath, [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
});
try {
    $ModelDefinitionsDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}
#>
$ModelDefinitionsDocument = [System.Xml.Linq.XDocument]::Load($XmlPath);
@($ModelDefinitionsDocument.Root.Elements([DevUtil.XLinqHelper]::GetMdName('Source')) | ForEach-Object {
    $_.Elements([DevUtil.XLinqHelper]::GetMdName('Namespace')) | ForEach-Object {
        $_.Elements([DevUtil.XLinqHelper]::GetMdName('Members')) | ForEach-Object {
            $_.Elements([DevUtil.XLinqHelper]::GetMdName('Interface')) | ForEach-Object {
                $_.Elements([DevUtil.XLinqHelper]::GetMdName('Members')) | ForEach-Object {
                    $_.Elements([DevUtil.XLinqHelper]::GetMdName('Property')) | ForEach-Object {
                        $_.Elements([DevUtil.XLinqHelper]::GetMdName('AttributeLists')) | ForEach-Object {
                            $_.Elements([DevUtil.XLinqHelper]::GetMdName('AttributeList')) | ForEach-Object {
                                $_.Elements([DevUtil.XLinqHelper]::GetMdName('LeadingTrivia')) | Write-Output;
                            }
                        }
                    }
                }
            }
        }
    }
}) | ForEach-Object { $_.Remove() }

try {
    $ModelDefinitionsDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}


#[DevUtil.XLinqHelper]::GetMdName('LeadingTrivia')

