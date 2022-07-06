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

$XmlPath = $PSScriptRoot | Join-Path -ChildPath 'TypeDefinitions.xml';
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
$XslPath = $PSScriptRoot | Join-Path -ChildPath 'ModelDefinitions.xslt';
$OutputPath = $PSScriptRoot | Join-Path -ChildPath 'ModelDefinitions.xml';

$XslCompiledTransform = [System.Xml.Xsl.XslCompiledTransform]::new(); $XslCompiledTransform.Load($XslPath); $XslCompiledTransform.Transform($XmlPath, $OutputPath);
<#
$ModelDefinitionsDocument = $null;
[Xml]$XmlDocument = '<ModelDefinitions xmlns="http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd"/>';
$XmlDocument.Load($XmlPath);
$nsmgr = [System.Xml.XmlNamespaceManager]::new($XmlDocument.NameTable);
$nsmgr.AddNamespace('md', 'http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd');
foreach ($MembersElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Members', $nsmgr))) {
    foreach ($XmlElement in @($MembersElement.SelectNodes('md:*', $nsmgr))) {
        [System.Xml.XmlElement]$NamespaceElement = $MembersElement.ParentNode;
        $MembersElement.RemoveChild($XmlElement) | Out-Null;
        $NamespaceElement.InsertBefore($XmlElement, $MembersElement) | Out-Null;
    }
    $NamespaceElement.RemoveChild($MembersElement) | Out-Null;
}
@('Model\BaseDbContext.cs', 'Model\DbEntity.cs', 'Model\DbEntity.DbValidationContext.cs') | ForEach-Object {
    $SourceElement = $XmlDocument.DocumentElement.SelectSingleNode("md:Sources/md:Source[@Path=`"$_`"]", $nsmgr)
    $SourceElement.ParentNode.RemoveChild($SourceElement) | Out-Null;
}
foreach ($ClassElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Class', $nsmgr))) {
    $ModifiersElement = $ClassElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'public';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="partial"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('IsPartial')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="abstract"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('IsAbstract')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="sealed"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('IsSealed')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="static"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $ClassElement.Attributes.Append($XmlDocument.CreateAttribute('IsStatic')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $ClassElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
foreach ($InterfaceElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Interface', $nsmgr))) {
    $ModifiersElement = $InterfaceElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $InterfaceElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'public';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $InterfaceElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $InterfaceElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $InterfaceElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $InterfaceElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
foreach ($EnumElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Enum', $nsmgr))) {
    $ModifiersElement = $EnumElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $EnumElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'public'
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $EnumElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $EnumElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $EnumElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $EnumElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
foreach ($FieldElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Class/md:Members/md:Field', $nsmgr))) {
    $ModifiersElement = $FieldElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public" or .="private"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $FieldElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = $AccessElement.InnerText;
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $FieldElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $FieldElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $FieldElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="readonly"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $FieldElement.Attributes.Append($XmlDocument.CreateAttribute('IsReadOnly')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="static"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $FieldElement.Attributes.Append($XmlDocument.CreateAttribute('IsStatic')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $FieldElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
foreach ($ConstructorElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Class/md:Members/md:Constructor', $nsmgr))) {
    $ModifiersElement = $ConstructorElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public" or .="private"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $ConstructorElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = $AccessElement.InnerText;
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $ConstructorElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $ConstructorElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $ConstructorElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $ConstructorElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
foreach ($MethodElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Class/md:Members/md:Method', $nsmgr))) {
    $ModifiersElement = $MethodElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public" or .="private"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = $AccessElement.InnerText;
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="abstract"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('IsAbstract')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="virtual"]', $nsmgr);
            if ($null -ne $AccessElement) {
                $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('IsVirtual')).Value = 'true';
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            }
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="async"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('IsAsync')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="override"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('IsOverride')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="static"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $MethodElement.Attributes.Append($XmlDocument.CreateAttribute('IsStatic')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $MethodElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
foreach ($PropertyElement in @($XmlDocument.DocumentElement.SelectNodes('md:Sources/md:Source/md:Namespace/md:Class/md:Members/md:Property', $nsmgr))) {
    $ModifiersElement = $PropertyElement.SelectSingleNode('md:Modifiers', $nsmgr);
    if ($null -ne $ModifiersElement) {
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="public" or .="private"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = $AccessElement.InnerText;
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="protected"]', $nsmgr);
            $AccessElement2 = $ModifiersElement.SelectSingleNode('md:Modifier[.="internal"]', $nsmgr);
            if ($null -ne $AccessElement) {
                if ($null -ne $AccessElement2) {
                    $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'protected internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                } else {
                    $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                }
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            } else {
                if ($null -ne $AccessElement) {
                    $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('Access')).Value = 'internal';
                    $AccessElement2.ParentNode.RemoveChild($AccessElement) | Out-Null;
                }
            }
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="abstract"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('IsAbstract')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        } else {
            $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="virtual"]', $nsmgr);
            if ($null -ne $AccessElement) {
                $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('IsVirtual')).Value = 'true';
                $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
            }
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="override"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('IsOverride')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        $AccessElement = $ModifiersElement.SelectSingleNode('md:Modifier[.="static"]', $nsmgr);
        if ($null -ne $AccessElement) {
            $PropertyElement.Attributes.Append($XmlDocument.CreateAttribute('IsStatic')).Value = 'true';
            $AccessElement.ParentNode.RemoveChild($AccessElement) | Out-Null;
        }
        if ($null -eq $ModifiersElement.SelectSingleNode('md:Modifier', $nsmgr)) {
            $PropertyElement.RemoveChild($ModifiersElement) | Out-Null;
        }
    }
}
$Regex = [System.Text.RegularExpressions.Regex]::new('nameof\(Properties.Resources.(\w+)\)');
foreach ($AttributeElement in @($XmlDocument.SelectNodes('//md:Attribute[@Name="Display"]', $nsmgr))) {
    $DislayElement = $AttributeElement.ParentNode.InsertBefore($XmlDocument.CreateElement('Display', 'http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd'), $AttributeElement);
    $a = $AttributeElement.SelectSingleNode('md:Arguments', $nsmgr);
    if ($null -ne $a) {
        foreach ($ArgumentElement in @($a.SelectNodes('md:Argument', $nsmgr))) {
            $m = $Regex.Match($ArgumentElement.InnerText);
            if ($m.Success) {
                switch ($ArgumentElement.SelectSingleNode('@Name').value) {
                    'Name' {
                        $DislayElement.Attributes.Append($XmlDocument.CreateAttribute('Name')).Value = $m.Groups[1].Value;
                        $a.RemoveChild($ArgumentElement) | Out-Null;
                        break;
                    }
                    'ShortName' {
                        $DislayElement.Attributes.Append($XmlDocument.CreateAttribute('ShortName')).Value = $m.Groups[1].Value;
                        $a.RemoveChild($ArgumentElement) | Out-Null;
                        break;
                    }
                    'Description' {
                        $DislayElement.Attributes.Append($XmlDocument.CreateAttribute('Description')).Value = $m.Groups[1].Value;
                        $a.RemoveChild($ArgumentElement) | Out-Null;
                        break;
                    }
                    'ResourceType' {
                        $a.RemoveChild($ArgumentElement) | Out-Null;
                        break;
                    }
                }
            }
        }
        if ($null -eq $a.SelectSingleNode('md:Argument', $nsmgr)) {
            $AttributeElement.ParentNode.RemoveChild($ArgumentElement) | Out-Null;
        }
    }
}
foreach ($ArgumentElement in @($XmlDocument.SelectNodes('//md:Display/md:Arguments/md:Argument[not(count(@Name)=0)]', $nsmgr))) {
    $m = $Regex.Match($ExpressionElement.InnerText);
    if ($m.Success) {
        switch ($ArgumentElement.SelectSingleNode('@Name').Value) {
            'Name' {
                $ArgumentElement.ParentNode.ParentNode.Attribute.Append($XmlDocument.CreateAttribute('Name')).Value = $m.Groups[1].Value;
                $ArgumentElement.ParentNode.RemoveChild($ArgumentElement) | Out-Null;
                break;
            }
            'ShortName' {
                $ArgumentElement.ParentNode.ParentNode.Attribute.Append($XmlDocument.CreateAttribute('ShortName')).Value = $m.Groups[1].Value;
                $ArgumentElement.ParentNode.RemoveChild($ArgumentElement) | Out-Null;
                break;
            }
        }
    }
}
[System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($XmlPath, [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
});
try {
    $XmlDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}
#>
