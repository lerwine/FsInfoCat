Function ConvertTo-SimpleTypeName {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [string[]]$UsingNamespace
    )
    Process {
        switch ($Type) {
            { $_.IsArray } {
                $r = $Type.GetArrayRank();
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    if ($r -lt 2) {
                        "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)[]";
                    } else {
                        "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)[$([string]::new(([char]','), $r - 1))]";
                    }
                } else {
                    if ($r -lt 2) {
                        "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType())[]";
                    } else {
                        "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType())[$([string]::new(([char]','), $r - 1))]";
                    }
                }
                break;
            }
            { $_.IsByRef } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)&";
                } else {
                    "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType())&";
                }
                break;
            }
            { $_.IsPointer } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)*";
                } else {
                    "$(ConvertTo-SimpleTypeName -Type $Type.GetElementType())*";
                }
                break;
            }
            { $_ -eq [System.Byte] } { 'byte' | Write-Output; break; }
            { $_ -eq [System.SByte] } { 'sbyte' | Write-Output; break; }
            { $_ -eq [System.Char] } { 'char' | Write-Output; break; }
            { $_ -eq [System.Int16] } { 'short' | Write-Output; break; }
            { $_ -eq [System.Int32] } { 'int' | Write-Output; break; }
            { $_ -eq [System.Int64] } { 'long' | Write-Output; break; }
            { $_ -eq [System.UInt16] } { 'ushort' | Write-Output; break; }
            { $_ -eq [System.UInt32] } { 'uint' | Write-Output; break; }
            { $_ -eq [System.UInt64] } { 'ulong' | Write-Output; break; }
            { $_ -eq [System.Single] } { 'float' | Write-Output; break; }
            { $_ -eq [System.Double] } { 'double' | Write-Output; break; }
            { $_ -eq [System.Decimal] } { 'decimal' | Write-Output; break; }
            { $_ -eq [System.String] } { 'string' | Write-Output; break; }
            { $_.IsValueType -and $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Nullable`1] } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-SimpleTypeName -Type ([System.Nullable]::GetUnderlyingType($Type)) -UsingNamespace $UsingNamespace)?";
                } else {
                    "$(ConvertTo-SimpleTypeName -Type ([System.Nullable]::GetUnderlyingType($Type)))?";
                }
                break;
            }
            default {
                $Namespace = $Type.Namespace;
                if ($Type.IsNested) {
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        $Namespace = ConvertTo-SimpleTypeName $Type.DeclaringType -UsingNamespace $UsingNamespace;
                    } else {
                        $Namespace = ConvertTo-SimpleTypeName $Type.DeclaringType;
                    }
                } else {
                    if ([string]::IsNullOrEmpty($Namespace)) {
                        $Namespace = '';
                    } else {
                        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                            if ($UsingNamespace -ccontains $Namespace) {
                                $Namespace = '';
                            } else {
                                $ns = ($UsingNamespace | Where-Object { $Namespace.StartsWith("$_.") } | Sort-Object -Property @{ Expression = { $_.Length } }) | Select-Object -Last 1;
                                if ($null -ne $ns) { $Namespace = $Namespace.Substring($ns.Length + 1) }
                            }
                        }
                    }
                }
                if ($Namespace.Length -gt 0) { $Namespace = "$Namespace." }
                if ($Type.IsConstructedGenericType) {
                    $n = $Type.Name;
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        "$Namespace$($n.Substring(0, $n.IndexOf('`'))){$(($Type.GetGenericArguments() | ConvertTo-SimpleTypeName -UsingNamespace $UsingNamespace) -join ', ')}";
                    } else {
                        "$Namespace$($n.Substring(0, $n.IndexOf('`'))){$(($Type.GetGenericArguments() | ConvertTo-SimpleTypeName) -join ', ')}";
                    }
                } else {
                    if ($Type.IsGenericTypeDefinition) {
                        $n = $Type.Name;
                        "$Namespace$($n.Substring(0, $n.IndexOf('`'))){$($Type.GetGenericArguments() -join ', ')}";
                    } else {
                        "$Namespace$($Type.Name)" | Write-Output;
                    }
                }
                break;
            }
        }
    }
}

Function Test-TypeAssignableTo {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [Type]$BaseType,

        [switch]$IncludeGenericArguments
    )

    Process {
        if ($BaseType.IsAssignableFrom($Type)) {
            $true | Write-Output;
        } else {
            if ($Type.IsConstructedGenericType -and $IncludeGenericArguments.IsPresent) {
                $Result = $false;
                foreach ($g in $Type.GetGenericArguments()) {
                    if (Test-TypeAssignableTo -Type $g -BaseType $BaseType) {
                        $Result = $true;
                        break;
                    }
                }
                $Result | Write-Output;
            } else {
                $false | Write-Output;
            }
        }
    }
}

Function Test-TypeEquals {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [Type]$BaseType,

        [switch]$IncludeGenericArguments
    )

    Process {
        if ($BaseType -eq $Type) {
            $true | Write-Output;
        } else {
            if ($Type.IsConstructedGenericType -and $IncludeGenericArguments.IsPresent) {
                $Result = $false;
                foreach ($g in $Type.GetGenericArguments()) {
                    if (Test-TypeEquals -Type $g -BaseType $BaseType) {
                        $Result = $true;
                        break;
                    }
                }
                $Result | Write-Output;
            } else {
                $false | Write-Output;
            }
        }
    }
}

Function Test-TypeExtends {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [Type]$BaseType,

        [switch]$Directly,

        [switch]$IncludeGenericArguments
    )

    Process {
        if ($Type -eq $BaseType) {
            $false | Write-Output;
        } else {
            if ($BaseType.IsAssignableFrom($Type)) {
                if ($Directly.IsPresent) {
                    if ($Type.BaseType -ne $BaseType) {
                            if ($BaseType.IsInterface) {
                                $AllInterfaces = $Type.GetInterfaces();
                                $TypeExtends = $false;
                                foreach ($Interface in $AllInterfaces) {
                                    if ($Interface -eq $BaseType) {
                                        $TypeExtends = $true;
                                        foreach ($i in $AllInterfaces) {
                                            if ($i -ne $Interface -and $Interface.IsAssignableFrom($i)) {
                                                $TypeExtends = $false;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                $TypeExtends | Write-Output;
                            } else {
                                $false | Write-Output;
                            }
                    } else {
                        $true | Write-Output;
                    }
                } else {
                    $true | Write-Output;
                }
            } else {
                if ($IncludeGenericArguments.IsPresent -and $Type.IsConstructedGenericType) {
                    $Result = $false;
                    if ($Directly.IsPresent) {
                        foreach ($g in $Type.GetGenericArguments()) {
                            if (Test-TypeExtends -Type $g -BaseType $BaseType -Directly) {
                                $Result = $true;
                                break;
                            }
                        }
                    } else {
                        foreach ($g in $Type.GetGenericArguments()) {
                            if (Test-TypeExtends -Type $g -BaseType $BaseType) {
                                $Result = $true;
                                break;
                            }
                        }
                    }
                    $Result | Write-Output;
                } else {
                    $false | Write-Output;
                }
            }
        }
    }
}

Function Get-ExtendingTypes {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [System.Reflection.Assembly[]]$Assembly,

        [switch]$Directly
    )

    Begin {
        $TypeCollection = @();
        if ($PSBoundParameters.ContainsKey('Assembly')) {
            $TypeCollection = @($Assembly | ForEach-Object { $_.GetTypes() });
        }
    }

    Process {
        $AllTypes = $TypeCollection;
        if ($PSBoundParameters.ContainsKey('Assembly')) {
            if ($Assembly -notcontains $Type.Assembly) { $AllTypes = $Type.Assembly.GetTypes() + $TypeCollection; }
        } else {
            $AllTypes = $Type.Assembly.GetTypes();
        }
        if ($Directly.IsPresent) {
            $AllTypes | Where-Object { $_ | Test-TypeExtends -BaseType $Type -Directly }
        } else {
            $AllTypes | Where-Object { $_ | Test-TypeExtends -BaseType $Type }
        }
    }
}

Function Get-PropertiesReferencingType {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [System.Reflection.Assembly[]]$Assembly,

        [switch]$Directly,

        [switch]$IncludeGenericArguments
    )

    Begin {
        $PropertyCollection = @();
        if ($PSBoundParameters.ContainsKey('Assembly')) {
            $PropertyCollection = @($Assembly | ForEach-Object { $_.GetTypes() | ForEach-Object { $_.GetProperties() | Where-Object { $_.ReflectedType -eq $_.DeclaringType } } });
        }
    }

    Process {
        $AllProperties = $PropertyCollection;
        if ($PSBoundParameters.ContainsKey('Assembly')) {
            if ($Assembly -notcontains $Type.Assembly) {
                $AllProperties = @($Type.Assembly.GetTypes() | ForEach-Object {$_.GetProperties() | Where-Object { $_.ReflectedType -eq $_.DeclaringType } }) + $PropertyCollection;
            }
        } else {
            $AllProperties = @($Type.Assembly.GetTypes() | ForEach-Object { $_.GetProperties() | Where-Object { $_.ReflectedType -eq $_.DeclaringType } });
        }
        if ($Directly.IsPresent) {
            if ($IncludeGenericArguments.IsPresent) {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeEquals -BaseType $Type -IncludeGenericArguments }
            } else {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeEquals -BaseType $Type }
            }
        } else {
            if ($IncludeGenericArguments.IsPresent) {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeAssignableTo -BaseType $Type -IncludeGenericArguments }
            } else {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeAssignableTo -BaseType $Type }
            }
        }
    }
}

Function Get-BaseTypes {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [switch]$AllAncestors
    )

    Process {
        if ($AllAncestors.IsPresent) {
            for ($BaseType = $Type.BaseType; $null -ne $Type.BaseType; $BaseType = $BaseType.BaseType) {
                $BaseType | Write-Output;
            }
            $Type.GetInterfaces() | Write-Output;
        } else {
            if ($null -ne $Type.BaseType) { $Type.BaseType | Write-Output }
            $AllInterfaces = $Type.GetInterfaces();
            foreach ($Interface in $AllInterfaces) {
                $IsDirect = $true;
                foreach ($i in $AllInterfaces) {
                    if ($i -ne $Interface -and $Interface.IsAssignableFrom($i)) {
                        $IsDirect = $false;
                        break;
                    }
                }
                if ($IsDirect) { $Interface | Write-Output }
            }
        }
    }
}

Function Add-SeeAlsoElements {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlDocumentFragment]$Xml,

        [Parameter(Mandatory = $true)]
        [string[]]$UsingNamespace
    )

    Begin {
        $OwnerDocument = $Xml.OwnerDocument;
    }
    Process {
        foreach ($ExtendingType in (@($Type | Get-BaseTypes) + @($Type | Get-ExtendingTypes -Directly))) {
            $Xml.AppendChild($OwnerDocument.CreateElement('seealso')).Attributes.Append($OwnerDocument.CreateAttribute('cref')).Value = `
                $ExtendingType | ConvertTo-SimpleTypeName -UsingNamespace $UsingNamespace;
        }
        foreach ($PropertyInfo in ($Type | Get-PropertiesReferencingType -Directly -IncludeGenericArguments)) {
            $Xml.AppendChild($OwnerDocument.CreateElement('seealso')).Attributes.Append($OwnerDocument.CreateAttribute('cref')).Value = `
                "$($PropertyInfo.DeclaringType | ConvertTo-SimpleTypeName -UsingNamespace $UsingNamespace).$($PropertyInfo.Name)";
        }
    }
}
