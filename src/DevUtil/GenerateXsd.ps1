Function Get-XName {
    [CmdletBinding(DefaultParameterSetName = 'none')]
    [OutputType([System.Xml.Linq.XName])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$LocalName,

        [Parameter(Mandatory = $true, ParameterSetName = 'xs')]
        [switch]$xs,

        [Parameter(Mandatory = $true, ParameterSetName = 'md')]
        [switch]$md,

        [Parameter(Mandatory = $true, ParameterSetName = 'xml')]
        [switch]$xml,

        [Parameter(Mandatory = $true, ParameterSetName = 'xmlns')]
        [switch]$xmlns,

        [Parameter(ParameterSetName = 'none')]
        [switch]$None
    )

    Begin {
        if ($null -eq $Script:__XName_NS) {
            Set-Variable -Name '__GetXName_NS' -Option Constant -Scope 'Script' -Value ([System.Management.Automation.PSObject]@{
                md = [System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xs');
                xs = [System.Xml.Linq.XNamespace]::Get('http://www.w3.org/2001/XMLSchema');
            });
        }
    }

    Process {
        switch ($PSCmdlet.ParameterSetName) {
            'none' {
                Write-Output -InputObject ([System.Xml.Linq.XNamespace]::None.GetName($LocalName)) -NoEnumerate;
                break;
            }
            'xml' {
                Write-Output -InputObject ([System.Xml.Linq.XNamespace]::Xml.GetName($LocalName)) -NoEnumerate;
                break;
            }
            'xmlns' {
                Write-Output -InputObject ([System.Xml.Linq.XNamespace]::Xmlns.GetName($LocalName)) -NoEnumerate;
                break;
            }
            default {
                Write-Output -InputObject $Script:__GetXName_NS.($PSCmdlet.ParameterSetName).GetName($LocalName) -NoEnumerate;
                break;
            }
        }
    }
}

Function New-XAttribute {
    [CmdletBinding(DefaultParameterSetName = 'none')]
    [OutputType([System.Xml.Linq.XAttribute])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$LocalName,

        [Parameter(Mandatory = $true)]
        [string]$Value,

        [Parameter(Mandatory = $true, ParameterSetName = 'xs')]
        [switch]$xs,

        [Parameter(Mandatory = $true, ParameterSetName = 'md')]
        [switch]$md,

        [Parameter(Mandatory = $true, ParameterSetName = 'xml')]
        [switch]$xml,

        [Parameter(Mandatory = $true, ParameterSetName = 'xmlns')]
        [switch]$xmlns,

        [Parameter(ParameterSetName = 'none')]
        [switch]$None
    )

    switch ($PSCmdlet.ParameterSetName) {
        'xs' {
            Write-Output -InputObject ([System.Xml.Linq.XAttribute]::new((Get-XName -LocalName $LocalName -xs), $Value)) -NoEnumerate;
            break;
        }
        'md' {
            Write-Output -InputObject ([System.Xml.Linq.XAttribute]::new((Get-XName -LocalName $LocalName -md), $Value)) -NoEnumerate;
            break;
        }
        'xmlns' {
            Write-Output -InputObject ([System.Xml.Linq.XAttribute]::new((Get-XName -LocalName $LocalName -xmlns), $Value)) -NoEnumerate;
            break;
        }
        'xml' {
            Write-Output -InputObject ([System.Xml.Linq.XAttribute]::new((Get-XName -LocalName $LocalName -xml), $Value)) -NoEnumerate;
            break;
        }
        default {
            Write-Output -InputObject ([System.Xml.Linq.XAttribute]::new((Get-XName -LocalName $LocalName -none), $Value)) -NoEnumerate;
            break;
        }
    }
}

Function New-XElement {
    [CmdletBinding(DefaultParameterSetName = 'none')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [object[]]$Content,

        [Parameter(Mandatory = $true)]
        [string]$LocalName,

        [Parameter(Mandatory = $true, ParameterSetName = 'xs')]
        [switch]$xs,

        [Parameter(Mandatory = $true, ParameterSetName = 'md')]
        [switch]$md,

        [Parameter(Mandatory = $true, ParameterSetName = 'xml')]
        [switch]$xml,

        [Parameter(Mandatory = $true, ParameterSetName = 'xmlns')]
        [switch]$xmlns,

        [Parameter(ParameterSetName = 'none')]
        [switch]$None
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        switch ($PSCmdlet.ParameterSetName) {
            'xs' {
                $XElement = [System.Xml.Linq.XElement]::new((Get-XName -LocalName $LocalName -xs));
                break;
            }
            'md' {
                $XElement = [System.Xml.Linq.XElement]::new((Get-XName -LocalName $LocalName -md));
                break;
            }
            'xmlns' {
                $XElement = [System.Xml.Linq.XElement]::new((Get-XName -LocalName $LocalName -xmlns));
                break;
            }
            'xml' {
                $XElement = [System.Xml.Linq.XElement]::new((Get-XName -LocalName $LocalName -xml));
                break;
            }
            default {
                $XElement = [System.Xml.Linq.XElement]::new((Get-XName -LocalName $LocalName -none));
                break;
            }
        }
    }

    Process {
        if ($PSBoundParameters.ContainsKey('Content')) {
            foreach ($obj in $Content) { $XElement.Add($obj) }
        }
    }

    End { Write-Output -InputObject $XElement -NoEnumerate }
}

Function Get-XsdDefinition {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Parent,

        [Parameter(Mandatory = $true, ParameterSetName = 'simpleType')]
        [switch]$simpleType,

        [Parameter(Mandatory = $true, ParameterSetName = 'complexType')]
        [switch]$complexType,

        [Parameter(Mandatory = $true, ParameterSetName = 'group')]
        [switch]$group,

        [Parameter(Mandatory = $true, ParameterSetName = 'attributeGroup')]
        [switch]$attributeGroup,

        [Parameter(Mandatory = $true, ParameterSetName = 'attribute')]
        [switch]$attribute,

        [Parameter(Mandatory = $true, ParameterSetName = 'element')]
        [switch]$element,

        [Parameter(ParameterSetName = 'element')]
        [Parameter(ParameterSetName = 'group')]
        [Parameter(ParameterSetName = 'attributeGroup')]
        [switch]$ref
    )

    Begin {
        $AttributeName = $null;
        if ($ref.IsPresent) {
            $AttributeName = Get-XName -LocalName 'ref';
        } else {
            $AttributeName = Get-XName -LocalName 'name';
        }
        $ElementName = Get-XName -LocalName $PSCmdlet.ParameterSetName -xs;
    }

    Process {
        foreach ($XElement in $SchemaDocument.Root.Elements($ElementName)) {
            $XAttribute = $XElement.Attribute($AttributeName);
            if ($null -ne $XAttribute -and $XAttribute.Value -eq $Name) {
                Write-Output -InputObject $XElement -NoEnumerate;
                break;
            }
        }
    }
}

Function Test-IsTypedEnumerable {
    [CmdletBinding(DefaultParameterSetName = 'Constructed')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true, ParameterSetName = 'Constructed')]
        [switch]$Constructed,

        [Parameter(ParameterSetName = 'NotConstructed')]
        [switch]$NotConstructed,

        [Parameter(Mandatory = $true, ParameterSetName = 'Definition')]
        [switch]$Definition,

        [Parameter(Mandatory = $true, ParameterSetName = 'Parameter')]
        [switch]$Parameter,

        [Parameter(ParameterSetName = 'Constructed')]
        [switch]$ElementNotGeneric,

        [Parameter(Mandatory = $true, ParameterSetName = 'AssignableTo')]
        [Type]$ElementAssignableTo
    )

    Process {
        if (-not $Type.IsGenericType) {
            $false | Write-Output;
            continue;
        }
        $GenericArgs = $Type.GetGenericArguments();
        if ($GenericArgs.Length -ne 1 -or ($ElementNotGeneric.IsPresent -and $GenericArgs[0].IsGenericType)) {
            $false | Write-Output;
            continue;
        }
        $InterfaceType = [System.Collections.Generic.IEnumerable`1].MakeGenericType($GenericArgs[0]);
        if (-not ($Type.Equals($InterfaceType) -or $Type.GetInterfaces() -contains $InterfaceType)) {
            $false | Write-Output;
            continue;
        }
        if ($NotConstructed.IsPresent) {
            if ($Type.IsConstructedGenericType) {
                $false | Write-Output;
                continue;
            }
        } else {
            if ($Definition.IsPresent) {
                if (-not $Type.IsGenericTypeDefinition) {
                    $false | Write-Output;
                    continue;
                }
            } else {
                if ($Parameter.IsPresent) {
                    if (-not $Type.IsGenericParameter) {
                        $false | Write-Output;
                        continue;
                    }
                } else {
                    if ($Constructed.IsPresent) {
                        if (-not $Type.IsConstructedGenericType) {
                            $false | Write-Output;
                            continue;
                        }
                    } else {
                        if ($PSBoundParameters.ContainsKey('ElementAssignableTo')) {
                            return $Type.IsConstructedGenericType -and $ElementAssignableTo.IsAssignableFrom($GenericArgs[0]);
                        }
                    }
                }
            }
        }
    }

    End { $true | Write-Output }
}

Function New-XsdDefinition {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement[]]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateSet('complexType', 'simpleType', 'group', 'attributeGroup')]
        [string]$Kind,

        [Parameter(Mandatory = $true)]
        [string]$Name
    )

    Begin {
        $XElement = (New-XAttribute -LocalName 'name' -Value $Name) | New-XElement -LocalName $Kind -xs;
    }

    Process {
        if ($PSBoundParameters.ContainsKey($Content)) {
            foreach ($e in $Content) {
                $XElement.Add($e);
            }
        }
    }

    End { Write-Output -InputObject $XElement -NoEnumerate }
}

Function New-XsdElement {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [string]$Type,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    if ($PSBoundParameters.ContainsKey('MinOccurs')) {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'name' -Value $Name),
                (New-XAttribute -LocalName 'type' -Value $Type),
                (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName 'element' -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName 'element' -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs)))
                ) | New-XElement -LocalName 'element' -xs) -NoEnumerate;
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'name' -Value $Name),
                (New-XAttribute -LocalName 'type' -Value $Type),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName 'element' -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName 'element' -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type)
                ) | New-XElement -LocalName 'element' -xs) -NoEnumerate;
            }
        }
    }
}

Function New-XsdGroupRef {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    if ($PSBoundParameters.ContainsKey('MinOccurs')) {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'ref' -Value $Name),
                (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName 'group' -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'ref' -Value $Name),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName 'group' -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'ref' -Value $Name),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs)))
                ) | New-XElement -LocalName 'group' -xs) -NoEnumerate;
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'ref' -Value $Name),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName 'group' -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'ref' -Value $Name),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName 'group' -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((New-XAttribute -LocalName 'ref' -Value $Name) | New-XElement -LocalName 'group' -xs) -NoEnumerate;
            }
        }
    }
}

Function New-XsdAttribute {
    [CmdletBinding(DefaultParameterSetName = 'NotRequired')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [string]$Type,

        [ValidateRange(ParameterSetName = 'NotRequired')]
        [string]$DefaultValue,

        [ValidateRange(ParameterSetName = 'NotRequired')]
        [switch]$Optional,

        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [switch]$Required
    )

    if ($Required.IsPresent) {
        Write-Output -InputObject ((
            (New-XAttribute -LocalName 'name' -Value $Name),
            (New-XAttribute -LocalName 'type' -Value $Type),
            (New-XAttribute -LocalName 'use' -Value 'required')
        ) | New-XElement -LocalName 'attribute' -xs) -NoEnumerate;
    } else {
        if ($PSBoundParameters.ContainsKey('DefaultValue')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'name' -Value $Name),
                (New-XAttribute -LocalName 'type' -Value $Type),
                (New-XAttribute -LocalName 'use' -Value 'optional'),
                (New-XAttribute -LocalName 'default' -Value $DefaultValue)
            ) | New-XElement -LocalName 'attribute' -xs) -NoEnumerate;
        } else {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'name' -Value $Name),
                (New-XAttribute -LocalName 'type' -Value $Type),
                (New-XAttribute -LocalName 'use' -Value 'optional')
            ) | New-XElement -LocalName 'attribute' -xs) -NoEnumerate;
        }
    }
    if ($PSBoundParameters.ContainsKey('MinOccurs')) {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'name' -Value $Name),
                (New-XAttribute -LocalName 'type' -Value $Type),
                (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs)))
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'name' -Value $Name),
                (New-XAttribute -LocalName 'type' -Value $Type),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'name' -Value $Name),
                    (New-XAttribute -LocalName 'type' -Value $Type),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            }
        }
    }
}

Function New-XsdAttributeGroupRef {
    [CmdletBinding(DefaultParameterSetName = 'NotRequired')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateSet('attribute', 'attributeGroup')]
        [string]$Type = 'attribute',

        [ValidateRange(ParameterSetName = 'NotRequired')]
        [string]$DefaultValue,

        [ValidateRange(ParameterSetName = 'NotRequired')]
        [switch]$Optional,

        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [switch]$Required
    )


    if ($PSBoundParameters.ContainsKey('MinOccurs')) {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'ref' -Value $XsdName),
                (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'ref' -Value $XsdName),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'ref' -Value $XsdName),
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs)))
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'ref' -Value $XsdName),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'ref' -Value $XsdName),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((New-XAttribute -LocalName 'ref' -Value $XsdName) | New-XElement -LocalName $ElementName -xs) -NoEnumerate;
            }
        }
    }
}

Function New-XsdExplicitGroup {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [ValidateSet('sequence', 'choice', 'all', 'any')]
        [string]$Kind,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    if ($PSBoundParameters.ContainsKey('MinOccurs')) {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((
                (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                (New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs)))
            ) | New-XElement -LocalName $Kind -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((
                    (New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))),
                    (New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded')
                ) | New-XElement -LocalName $Kind -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject ((New-XAttribute -LocalName 'minOccurs' -Value ([System.Xml.XmlConvert]::ToString($MinOccurs))) | New-XElement -LocalName $Kind -xs) -NoEnumerate;
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
            Write-Output -InputObject ((New-XAttribute -LocalName 'maxOccurs' -Value ([System.Xml.XmlConvert]::ToString($MaxOccurs))) | New-XElement -LocalName $Kind -xs) -NoEnumerate;
        } else {
            if ($MaxUnbounded.IsPresent) {
                Write-Output -InputObject ((New-XAttribute -LocalName 'maxOccurs' -Value 'unbounded') | New-XElement -LocalName $Kind -xs) -NoEnumerate;
            } else {
                Write-Output -InputObject (New-XElement -LocalName $Kind -xs) -NoEnumerate;
            }
        }
    }
}

Function New-XsdComplexTypeExtension {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement[]]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Base
    )

    Begin {
        $XElement = ((New-XAttribute -LocalName 'base' -Value $Base) | New-XElement -LocalName 'extension' -xs) | New-XElement -LocalName 'complexContent' -xs;
    }

    Process {
        if ($PSBoundParameters.ContainsKey($Content)) {
            foreach ($e in $Content) {
                $XElement.Add($e);
            }
        }
    }

    End { Write-Output -InputObject $XElement -NoEnumerate }
}

Function Get-XsdName {
    [CmdletBinding(DefaultParameterSetName = 'Explicit')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(ParameterSetName = 'Explicit')]
        [switch]$Ref,

        [Parameter(Mandatory = $true, ParameterSetName = 'Element')]
        [switch]$Element
    )

    Process {
        if ($Type.IsGenericType) {
            if ($Type | Test-IsTypedEnumerable -Constructed -ElementNotGeneric) {
                $ComplexTypeName = $Type.Name -replace '`\d+$', '';
                $GenericName = $Type.GetGenericArguments()[0] | Get-XsdName -Element;
                if ($null -ne $GenericName) {
                    if ($ComplexTypeName.EndsWith('List')) {
                        if ($Ref.IsPresent) {
                            "md:$($GenericName)ListType" | Write-Output;
                        } else {
                            if ($Element.IsPresent) {
                                "$($GenericName)List" | Write-Output;
                            } else {
                                "$($GenericName)ListType" | Write-Output;
                            }
                        }
                    } else {
                        if ($ComplexTypeName.EndsWith('Collection')) {
                            if ($Element.IsPresent) {
                                $ComplexTypeName = "$($GenericName)Collection";
                            } else {
                                if ($Ref.IsPresent) {
                                    $ComplexTypeName = "md:$($GenericName)CollectionType";
                                } else {
                                    $ComplexTypeName = "$($GenericName)CollectionType";
                                }
                            }
                        } else {
                            if ($ComplexTypeName.EndsWith('Enumerable')) {
                                if ($Element.IsPresent) {
                                    $ComplexTypeName = "$($GenericName)Enumerable";
                                } else {
                                    if ($Ref.IsPresent) {
                                        $ComplexTypeName = "md:$($GenericName)EnumerableType";
                                    } else {
                                        $ComplexTypeName = "$($GenericName)EnumerableType";
                                    }
                                }
                            } else {
                                if ($Element.IsPresent) {
                                    $ComplexTypeName = "$GenericName$(($ComplexTypeName -replace 'Syntax$', '') -replace 'Definition$', '')";
                                } else {
                                    if ($Ref.IsPresent) {
                                        $ComplexTypeName = "md:$GenericName$(($ComplexTypeName -replace 'Syntax$', '') -replace 'Definition$', '')Type";
                                    } else {
                                        $ComplexTypeName = "$GenericName$(($ComplexTypeName -replace 'Syntax$', '') -replace 'Definition$', '')Type";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } else {
            if ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($Type)) {
                if ($Type -eq [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                    if ($Element.IsPresent) {
                        'SyntaxNode' | Write-Output;
                    } else {
                        if ($Ref.IsPresent) {
                            'md:SyntaxNodeType' | Write-Output;
                        } else {
                            'SyntaxNodeType' | Write-Output;
                        }
                    }
                } else {
                    if ($Element.IsPresent) {
                        (($Type.Name -replace 'Syntax$', '') -replace 'Definition$', '') | Write-Output;
                    } else {
                        if ($Ref.IsPresent) {
                            "md:$($Type.Name -replace 'Syntax$', '')Type" | Write-Output;
                        } else {
                            "$($Type.Name -replace 'Syntax$', '')Type" | Write-Output;
                        }
                    }
                }
            } else {
                if ($Ref.IsPresent) {
                    switch ($Type) {
                        { $_ -eq [string] } { 'xs:string' | Write-Output; break; }
                        { $_ -eq [bool] } { 'xs:boolean' | Write-Output; break; }
                        { $_ -eq [byte] } { 'xs:unsignedByte' | Write-Output; break; }
                        { $_ -eq [SByte] } { 'xs:byte' | Write-Output; break; }
                        { $_ -eq [short] } { 'xs:short' | Write-Output; break; }
                        { $_ -eq [ushort] } { 'xs:unsignedShort' | Write-Output; break; }
                        { $_ -eq [int] } { 'xs:int' | Write-Output; break; }
                        { $_ -eq [uint] } { 'xs:unsignedInt' | Write-Output; break; }
                        { $_ -eq [long] } { 'xs:long' | Write-Output; break; }
                        { $_ -eq [ulong] } { 'xs:unsignedLong' | Write-Output; break; }
                        { $_ -eq [double] } { 'xs:double' | Write-Output; break; }
                        { $_ -eq [float] } { 'xs:float' | Write-Output; break; }
                        { $_ -eq [decimal] } { 'xs:decimal' | Write-Output; break; }
                        { $_ -eq [DateTime] } { 'xs:dateTime' | Write-Output; break; }
                        { $_ -eq [Uri] } { 'xs:anyURI' | Write-Output; break; }
                        { $_ -eq [Guid] } { 'md:GuidType' | Write-Output; break; }
                        { $_ -eq [TimeSpan] } { 'md:duration' | Write-Output; break; }
                        { $_ -eq [byte[]] } { 'xs:base64Binary' | Write-Output; break; }
                        { $_ -eq [Microsoft.CodeAnalysis.CSharp.SyntaxKind] } { 'md:SyntaxKindType' | Write-Output; break; }
                    }
                }
            }
        }
    }
}

Function Import-SyntaxSchema {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValueScript({
            if ($_.IsGenericType) { return ($_ | Test-IsTypedEnumerable -ElementAssignableTo ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode])) -and -not $_.GetGenericArguments()[0].IsGenericType; }
            return [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($_);
        })]
        [Type]$Type,

        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XDocument]$SchemaDocument,

        [int]$MaxDepth,

        [switch]$PassThru
    )

    Process {
        $ComplexTypeName = $Type | Get-XsdName;
        $XElement = Get-XsdDefinition -Parent $SchemaDocument.Root -Name $ComplexTypeName -complexType
        if ($null -eq $XElement) {
            if ($Type -eq [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                $XElement = (
                    (New-XsdExplicitGroup -Kind 'sequence'),
                    (New-XsdAttribute -Name 'IsMissing' -Type 'xs:boolean' -DefaultValue 'false')
                ) | New-XsdDefinition -Name $ComplexTypeName -Kind 'complexType';
                $SchemaDocument.Root.Add($XElement);
                if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                    if ($MaxDepth -gt 0) {
                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] | Import-SyntaxSchema -SchemaDocument $SchemaDocument -MaxDepth ($MaxDepth - 1);
                    }
                } else {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] | Import-SyntaxSchema -SchemaDocument $SchemaDocument;
                }
            } else {
                $BaseName = Get-XsdName -Type $Type.BaseType -Ref;
                if ($Type.IsConstructedGenericType) {
                    $GenericArg = $Type.GetGenericArguments()[0];
                    if ($null -ne $BaseName) {
                        $XElement = ((New-XsdExplicitGroup -Kind 'sequence') | New-XsdComplexTypeExtension -Base $BaseName) | New-XsdDefinition -Name $ComplexTypeName -Kind 'complexType';
                        $XElement.AddAnnotation($GenericArg);
                        $SchemaDocument.Root.Add($XElement);
                        if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                            Import-SyntaxSchema -Type $Type.BaseType -SchemaDocument $SchemaDocument -MaxDepth $MaxDepth;
                        } else {
                            Import-SyntaxSchema -Type $Type.BaseType -SchemaDocument $SchemaDocument;
                        }
                    } else {
                        $XElement = (New-XsdExplicitGroup -Kind 'sequence') | New-XsdDefinition -Name $ComplexTypeName -Kind 'complexType';
                        $XElement.AddAnnotation($GenericArg);
                        $SchemaDocument.Root.Add($XElement);
                    }
                    if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                        Import-SyntaxSchema -Type $GenericArg -SchemaDocument $SchemaDocument -MaxDepth $MaxDepth;
                    } else {
                        Import-SyntaxSchema -Type $GenericArg -SchemaDocument $SchemaDocument;
                    }
                } else {
                    [System.Xml.Linq.XElement]$SequenceElement = New-XsdExplicitGroup -Kind 'sequence';
                    if ($null -ne $BaseName) {
                        $XElement = $SequenceElement | New-XsdComplexTypeExtension -Base $BaseName;
                        $SchemaDocument.Root.Add(($XElement | New-XsdDefinition -Name $ComplexTypeName -Kind 'complexType'));
                        if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                            Import-SyntaxSchema -Type $Type.BaseType -SchemaDocument $SchemaDocument -MaxDepth $MaxDepth;
                        } else {
                            Import-SyntaxSchema -Type $Type.BaseType -SchemaDocument $SchemaDocument;
                        }
                    } else {
                        $XElement = $SequenceElement | New-XsdDefinition -Name $ComplexTypeName -Kind 'complexType';
                        $SchemaDocument.Root.Add($XElement);
                    }
                    if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                        if ($MaxDepth -gt 0) {
                            <#if ($_.IsGenericType) { return ($_ | Test-IsTypedEnumerable -ElementAssignableTo ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode])) -and -not $_.GetGenericArguments()[0].IsGenericType; }
            return [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($_)#>
                            foreach ($PropertyInfo in $Type.GetProperties()) {
                                $PropertyTypeName = Get-XsdName -Type $PropertyInfo.PropertyType -Ref;
                                if ($PropertyInfo.PropertyType.IsGenericType) {
                                    if ($PropertyInfo.PropertyType | Test-IsTypedEnumerable -Constructed) {
                                        if ($null -ne $PropertyTypeName) {

                                        } else {
                                            # TODO: Get type name of element and add as list
                                        }
                                    }
                                } else {
                                    if ($null -ne $PropertyTypeName) {
                                        if ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($PropertyInfo.PropertyType)) {
                                            # TODO: Add as element
                                        } else {
                                            # TODO: Add as attribute
                                        }
                                    }
                                }
                            }
                            $Type.GetProperties() | Where-Object {
                                if ($_.PropertyType.IsGenericType) {
                                    return ;
                                }
                            }
                        }
                    } else {

                    }
                }
            }
        }
        if ($PassThru.IsPresent) { $XElement | Write-Output }
        if ($Type -eq [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
            if ($null -eq (Get-XsdDefinition -Parent $SchemaDocument.Root -Name 'SyntaxType' -complexType)) {
            }
        } else {
            if ($Type.IsConstructedGenericType) {
                $ComplexTypeName = $Type.Name -replace '`\d+$', '';
                $GenericArg = $Type.GetGenericArguments()[0];
                $GenericName = $GenericArg.Name -replace 'Syntax$', '';
                if ($null -eq (Get-XsdDefinition -Parent $SchemaDocument.Root -Name 'SyntaxType' -complexType)) {
                    $SchemaDocument.Root.Add((New-XsdDefinition -Name $GroupName -Kind 'group'));
                    $SchemaDocument.Root.Add((((New-XsdGroupRef -Name "md:$GroupName" -MinOccurs 0 -MaxUnbounded) | New-XsdExplicitGroup -Kind 'sequence') | New-XsdDefinition -Name 'SyntaxType' -Kind 'complexType'));
                    if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                        if ($NoParent.IsPresent) {
                            $GenericArg | Import-SyntaxSchema -SchemaDocument $SchemaDocument -MaxDepth $MaxDepth -NoParent;
                        } else {
                            $GenericArg | Import-SyntaxSchema -SchemaDocument $SchemaDocument -MaxDepth $MaxDepth;
                        }
                    } else {
                        if ($NoParent.IsPresent) {
                            $GenericArg | Import-SyntaxSchema -SchemaDocument $SchemaDocument -NoParent;
                        } else {
                            $GenericArg | Import-SyntaxSchema -SchemaDocument $SchemaDocument;
                        }
                    }
                }
            } else {
                $ComplexTypeName = $Type.Name -replace 'Syntax$', '';
                $ElementName = $ComplexTypeName -replace 'Definition$', '';
                $ComplexTypeName += 'Type';
                if ($null -eq (Get-XsdDefinition -Parent $SchemaDocument.Root -Name $ComplexTypeName -complexType)) {
                    if ($Type.BaseType -eq [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                        New-XsdComplexTypeExtension -Base 'SyntaxType';
                    } else {

                    }
                    $SchemaDocument.Root.Add((New-XsdDefinition -Name 'SyntaxGroup' -Kind 'group'));
                    $SchemaDocument.Root.Add(((
                        ((New-XsdGroupRef -Name 'SyntaxGroup' -MinOccurs 0 -MaxUnbounded) | New-XsdExplicitGroup -Kind 'sequence'),
                        (New-XsdAttribute -Name 'IsMissing' -Type 'xs:boolean' -DefaultValue 'false')
                    ) | New-XsdDefinition -Name ComplexTypeName -Kind 'complexType'));
                    [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] | Import-SyntaxSchema -SchemaDocument $SchemaDocument -NoParent;
                }
                if ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($Type.BaseType) -and -not $NoParent.IsPresent) {
                    $Type.BaseType | Import-SyntaxSchema -SchemaDocument $SchemaDocument -MaxDepth 0;
                }
                if ($PSBoundParameters.ContainsKey('MaxDepth')) {
                    if ($MaxDepth -gt 0) {

                    }
                } else {

                }
            }
        }
    }
}
