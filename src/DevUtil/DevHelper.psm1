Set-Variable -Name 'XamlNamespaces' -Option Constant -Scope 'Script' -Value ([PSCustomObject]@{
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation";
    x="http://schemas.microsoft.com/winfx/2006/xaml";
    mc="http://schemas.openxmlformats.org/markup-compatibility/2006";
    d="http://schemas.microsoft.com/expression/blend/2008";
});

Function ConvertTo-SimpleTypeName {
    [CmdletBinding()]
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

Function Test-XmlName {
    [CmdletBinding(DefaultParameterSetName = 'Name')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Name,

        [Parameter(ParameterSetName = 'Name')]
        [switch]$CName,

        [Parameter(Mandatory = $true, ParameterSetName = 'NCName')]
        [switch]$NCName
    )

    Process {
        if ([string]::IsNullOrEmpty($Name) -or -not [System.Xml.XmlConvert]::IsStartNCNameChar($Name[0])) {
            $false | Write-Output;
        } else {
            $c = $Name.IndexOf(':');
            if ($c -lt 0) {
                $IsValid = $true;
                for ($i = 1; $i -lt $Name.Length; $i++) {
                    if (-not [System.Xml.XmlConvert]::IsNcNameChar($Name[$i])) {
                        $IsValid = $false;
                        break;
                    }
                }
                $IsValid | Write-Output;
            } else {
                if ($NCName.IsPresent -or $c -eq 0 -or $c -eq $Name.Length - 1 -or -not [System.Xml.XmlConvert]::IsStartNCNameChar($Name[$c + 1])) {
                    $false | Write-Output;
                } else {
                    $IsValid = $true;
                    for ($i = 1; $i -lt $c; $i++) {
                        if (-not [System.Xml.XmlConvert]::IsNcNameChar($Name[$i])) {
                            $IsValid = $false;
                            break;
                        }
                    }
                    if ($IsValid) {
                        for ($i = $c + 2; $i -lt $Name.Length; $i++) {
                            if (-not [System.Xml.XmlConvert]::IsNcNameChar($Name[$i])) {
                                $IsValid = $false;
                                break;
                            }
                        }
                    }
                    $IsValid | Write-Output;
                }
            }
        }
    }
}

Function Get-PreviousElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )
    for ($n = $Node.PreviousSibling; $null -ne $n; $n = $n.PreviousSibling) {
        if ($n -is [System.Xml.XmlElement]) {
            Write-Output -InputObject $n -NoEnumerate;
            break;
        }
    }
}

Function Get-NextElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )
    for ($n = $Node.NextSibling; $null -ne $n; $n = $n.NextSibling) {
        if ($n -is [System.Xml.XmlElement]) {
            Write-Output -InputObject $n -NoEnumerate;
            break;
        }
    }
}

Function Get-PrecedingWhiteSpace {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )

    $XmlCharacterData = $null;
    for ($n = $Node.PreviousSibling; $null -ne $n -and $n -isnot [System.Xml.XmlElement]; $n = $n.PreviousSibling) {
        if ($n -is [System.Xml.XmlCharacterData]) {
            $XmlCharacterData = $n;
            break;
        }
    }
    if ($null -ne $XmlCharacterData) {
        $Text = $XmlCharacterData.InnerText;
        if ($Text.Length -eq 0) {
            (Get-PrecedingWhiteSpace -Node $XmlCharacterData) | Write-Output;
        } else {
            if ($XmlCharacterData -is [System.Xml.XmlWhitespace]) {
                $ws = Get-PrecedingWhiteSpace -Node $XmlCharacterData;
                if ($null -eq $ws) {
                    $Text | Write-Output;
                } else {
                    "$ws$Text" | Write-Output;
                }
            } else {
                $StartIndex = 0;
                for ($c = $Text.Length - 1; $c -ge 0; $c--) {
                    if (-not [System.Xml.XmlConvert]::IsWhitespaceChar($Text[$c])) {
                        $StartIndex = $c + 1;
                        break;
                    }
                }
                if ($StartIndex -lt $Text.Length) {
                    if ($StartIndex -gt 0) {
                        $Text.Substring($StartIndex) | Write-Output;
                    } else {
                        $ws = Get-PrecedingWhiteSpace -Node $XmlCharacterData;
                        if ($null -eq $ws) {
                            $Text | Write-Output;
                        } else {
                            "$ws$Text" | Write-Output;
                        }
                    }
                }
            }
        }
    }
}

Function Get-FollowingWhiteSpace {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )

    $XmlCharacterData = $null;
    for ($n = $Node.NextSibling; $null -ne $n -and $n -isnot [System.Xml.XmlElement]; $n = $n.NextSibling) {
        if ($n -is [System.Xml.XmlCharacterData]) {
            $XmlCharacterData = $n;
            break;
        }
    }
    if ($null -ne $XmlCharacterData) {
        $Text = $XmlCharacterData.InnerText;
        if ($Text.Length -eq 0) {
            (Get-FollowingWhiteSpace -Node $XmlCharacterData) | Write-Output;
        } else {
            if ($XmlCharacterData -is [System.Xml.XmlWhitespace]) {
                $ws = Get-FollowingWhiteSpace -Node $XmlCharacterData;
                if ($null -eq $ws) {
                    $Text | Write-Output;
                } else {
                    "$Text$ws" | Write-Output;
                }
            } else {
                $Length = $Text.Length;
                for ($c = 0; $c -lt $Length; $c++) {
                    if (-not [System.Xml.XmlConvert]::IsWhitespaceChar($Text[$c])) {
                        $Length = $c;
                        break;
                    }
                }
                if ($Length -gt 0) {
                    if ($Length -lt $Text.Length) {
                        $Text.Substring(0, $Length) | Write-Output;
                    } else {
                        $ws = Get-FollowingWhiteSpace -Node $XmlCharacterData;
                        if ($null -eq $ws) {
                            $Text | Write-Output;
                        } else {
                            "$Text$ws" | Write-Output;
                        }
                    }
                }
            }
        }
    }
}

Function Add-XmlElement {
    [CmdletBinding(DefaultParameterSetName = 'Last')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'ByIndex')]
        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Last')]
        [System.Xml.XmlElement]$Parent,

        [Parameter(Mandatory = $true, ParameterSetName = 'ByIndex')]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$Index,

        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [switch]$First,

        [Parameter(ParameterSetName = 'Last')]
        [switch]$Last,

        [Parameter(Mandatory = $true, ParameterSetName = 'Before')]
        [ValidateScript({ $null -ne $_.ParentNode })]
        [System.Xml.XmlElement]$Before,

        [Parameter(Mandatory = $true, ParameterSetName = 'After')]
        [ValidateScript({ $null -ne $_.ParentNode })]
        [System.Xml.XmlElement]$After,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$LocalName,

        [ValidateScript({ [string]::IsNullOrEmpty($_) -or ($_ | Test-XmlName -NCName) })]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Prefix,

        [ValidateScript({ [string]::IsNullOrEmpty($_) -or [Uri]::IsWellFormedUriString($_, [UriKind]::Absolute)})]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$NamespaceURI
    )

    $ns = $NamespaceURI;
    $p = $Prefix;
    $XmlElement = $null;
    switch ($PSCmdlet.ParameterSetName) {
        'Before' {
            if ($PSBoundParameters.ContainsKey('NamespaceURI')) {
                if (-not $PSBoundParameters.ContainsKey('Prefix')) {
                    if ($ns -ceq $Before.NamespaceURI) {
                        $p = $Before.Prefix;
                    } else {
                        $p = $Before.GetPrefixOfNamespace($ns);
                        if ([string]::IsNullOrEmpty($p) -and $Before.GetNamespaceOfPrefix($p) -cne $ns) {
                            $i = 0;
                            do {
                                $i++;
                                $p = "ns$i";
                            } while (-not [string]::IsNullOrEmpty($Before.GetNamespaceOfPrefix($p)));
                        }
                    }
                }
            } else {
                $ns = $Before.NamespaceURI;
                if (-not $PSBoundParameters.ContainsKey('Prefix')) { $p = $Before.Prefix; }
            }
            if ($null -eq $ns) { $ns = '' }
            if ($null -eq $p) { $p = '' }
            $OwnerDocument = $Before.OwnerDocument;
            $XmlElement = $Before.ParentNode.InsertBefore($OwnerDocument.CreateElement($p, $LocalName, $ns), $Before);
            $Ws = Get-PrecedingWhiteSpace -Node $XmlElement;
            if ($null -eq $ws) {
                $ws = Get-FollowingWhiteSpace -Node $Before;
                if ($null -ne $ws) {
                    if ($null -eq (Get-NextElement -Node $Before)) { $ws = "$ws    " }
                    $Before.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $Before) | Out-Null;
                    $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                }
            } else {
                $Before.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $Before) | Out-Null;
            }
            break;
        }
        'After' {
            $OwnerDocument = $After.OwnerDocument;
            if ($PSBoundParameters.ContainsKey('NamespaceURI')) {
                if (-not $PSBoundParameters.ContainsKey('Prefix')) {
                    if ($ns -ceq $After.NamespaceURI) {
                        $p = $After.Prefix;
                    } else {
                        $p = $After.GetPrefixOfNamespace($ns);
                        if ([string]::IsNullOrEmpty($p) -and $After.GetNamespaceOfPrefix($p) -cne $ns) {
                            $i = 0;
                            do {
                                $i++;
                                $p = "ns$i";
                            } while (-not [string]::IsNullOrEmpty($After.GetNamespaceOfPrefix($p)));
                        }
                    }
                }
            } else {
                $ns = $After.NamespaceURI;
                if (-not $PSBoundParameters.ContainsKey('Prefix')) { $p = $After.Prefix; }
            }
            if ($null -eq $ns) { $ns = '' }
            if ($null -eq $p) { $p = '' }
            $XmlElement = $After.ParentNode.InsertAfter($OwnerDocument.CreateElement($p, $LocalName, $ns), $After);
            $ws = Get-PrecedingWhiteSpace -Node $After;
            if ($null -eq $ws) {
                $ws = Get-FollowingWhiteSpace -Node $XmlElement;
                if ($null -ne $ws) {
                    if ($null -eq (Get-NextElement -Node $XmlElement)) { $ws = "    $ws" }
                    $After.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $After) | Out-Null;
                    $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                }
            } else {
                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
            }
            break;
        }
        default {
            $OwnerDocument = $Parent.OwnerDocument;
            if ($PSBoundParameters.ContainsKey('NamespaceURI')) {
                if (-not $PSBoundParameters.ContainsKey('Prefix')) {
                    if ($ns -ceq $Parent.NamespaceURI) {
                        $p = $Parent.Prefix;
                    } else {
                        $p = $Parent.GetPrefixOfNamespace($ns);
                        if ([string]::IsNullOrEmpty($p) -and $Parent.GetNamespaceOfPrefix($p) -cne $ns) {
                            $i = 0;
                            do {
                                $i++;
                                $p = "ns$i";
                            } while (-not [string]::IsNullOrEmpty($Parent.GetNamespaceOfPrefix($p)));
                        }
                    }
                }
            } else {
                $ns = $Parent.NamespaceURI;
                if (-not $PSBoundParameters.ContainsKey('Prefix')) { $p = $Parent.Prefix; }
            }
            if ($null -eq $ns) { $ns = '' }
            if ($null -eq $p) { $p = '' }
            if ($First.IsPresent) {
                $XmlElement = $Parent.PrependChild($OwnerDocument.CreateElement($p, $LocalName, $ns));
                $NextElement = Get-NextElement -Node $XmlElement;
                if ($null -eq $NextElement) {
                    $ws = Get-PrecedingWhiteSpace -Node $Parent;
                    if ($null -ne $ws) {
                        $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace("$ws    "), $XmlElement) | Out-Null;
                    }
                } else {
                    $Ws = Get-PrecedingWhiteSpace -Node $XmlElement;
                    if ($null -eq $ws) {
                        $ws = Get-FollowingWhiteSpace -Node $NextElement;
                        if ($null -ne $ws) {
                            if ($null -eq (Get-NextElement -Node $NextElement)) { $ws = "$ws    " }
                            $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                            $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                        }
                    } else {
                        $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                    }
                }
            } else {
                if ($PSBoundParameters.ContainsKey('Index')) {
                    $NextElement = $Parent.SelectSingleNode("*[$($Index + 1)]");
                    if ($null -eq $NextElement) {
                        $XmlElement = $Parent.AppendChild($OwnerDocument.CreateElement($p, $LocalName, $ns));
                        $PreviousElement = Get-PreviousElement -Node $XmlElement;
                        if ($null -eq $PreviousElement) {
                            $ws = Get-PrecedingWhiteSpace -Node $Parent;
                            if ($null -ne $ws) {
                                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace("$ws    "), $XmlElement) | Out-Null;
                            }
                        } else {
                            $ws = Get-PrecedingWhiteSpace -Node $PreviousElement;
                            if ($null -ne $ws) {
                                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                            }
                        }
                    } else {
                        $XmlElement = $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateElement($p, $LocalName, $ns), $NextElement);
                        $Ws = Get-PrecedingWhiteSpace -Node $XmlElement;
                        if ($null -eq $ws) {
                            $ws = Get-FollowingWhiteSpace -Node $NextElement;
                            if ($null -ne $ws) {
                                if ($null -eq (Get-NextElement -Node $NextElement)) { $ws = "$ws    " }
                                $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                            }
                        } else {
                            $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                        }
                    }
                } else {
                    $XmlElement = $Parent.AppendChild($OwnerDocument.CreateElement($p, $LocalName, $ns));
                    $PreviousElement = Get-PreviousElement -Node $XmlElement;
                    if ($null -eq $PreviousElement) {
                        $ws = Get-PrecedingWhiteSpace -Node $Parent;
                        if ($null -ne $ws) {
                            $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace("$ws    "), $XmlElement) | Out-Null;
                        }
                    } else {
                        $ws = Get-PrecedingWhiteSpace -Node $PreviousElement;
                        if ($null -ne $ws) {
                            $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                        }
                    }
                }
            }
            break;
        }
    }

    Write-Output -InputObject $XmlElement -NoEnumerate;
}

Function Test-XamlNode {
    [CmdletBinding(DefaultParameterSetName = 'Presentation')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [System.Xml.XmlNode]$Node,

        [Parameter(ParameterSetName = 'Presentation')]
        [switch]$Presentation,

        [Parameter(Mandatory = $true, ParameterSetName = 'Xaml')]
        [switch]$Xaml,

        [string]$LocalName
    )

    Process {
        if ($null -eq $Node -or -not ($Node -is [System.Xml.XmlElement] -or $Node -is [System.Xml.XmlAttribute])) {
            $false | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('LocalName') -and $Node.LocalName -cne $LocalName) {
                $false | Write-Output;
            } else {
                if ($Xaml.IsPresent) {
                    ($Node.NamespaceURI -ceq $Script:XamlNamespaces.x) | Write-Output;
                } else {
                    if ($Presentation.IsPresent) {
                        ($Node.NamespaceURI -ceq $Script:XamlNamespaces.xmlns) | Write-Output;
                    } else {
                        ($Node.NamespaceURI -ceq $Script:XamlNamespaces.xmlns -or $Node.NamespaceURI -ceq $Script:XamlNamespaces.x) | Write-Output;
                    }
                }
            }
        }
    }
}

Function Add-XamlGridRow {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Index')]
        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Last')]
        [ValidateScript({ ($_ | Test-XamlNode -LocalName 'Grid' -Presentation) -or ($_ | Test-XamlNode -LocalName 'Grid.RowDefinitions' -Presentation) })]
        [System.Xml.XmlElement]$GridElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'Before')]
        [ValidateScript({ ($_ | Test-XamlNode -LocalName 'RowDefinition' -Presentation) -and ($_.ParentNode | Test-XamlNode -LocalName 'Grid.RowDefinitions' -Presentation) })]
        [System.Xml.XmlElement]$Before,

        [Parameter(Mandatory = $true, ParameterSetName = 'After')]
        [ValidateScript({ ($_ | Test-XamlNode -LocalName 'RowDefinition' -Presentation) -and ($_.ParentNode | Test-XamlNode -LocalName 'Grid.RowDefinitions' -Presentation) })]
        [System.Xml.XmlElement]$After,

        [Parameter(Mandatory = $true, ParameterSetName = 'Index')]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$RowIndex,

        [ValidateRange(1, [int]::MaxValue)]
        [int]$RowCount = 1,

        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [switch]$First,

        [Parameter(ParameterSetName = 'Last')]
        [switch]$Last,

        [ValidatePattern('^(Auto|(\d+(\.\d+)?)\*?|\*)$')]
        [string]$Height,

        [switch]$PassThru
    )
    $RowDefinitionElements = [System.Collections.ObjectModel.Collection[System.Xml.XmlElement]]::new();
    $InsertedIndex = $RowIndex;
    $ParentElement = $GridElement;
    $nsmgr = $null;
    switch ($PSBoundParameters.ParameterSetName) {
        'Before' {
            $ParentElement = $Before.ParentNode;
            $RowDefinition = Add-XmlElement -Before $Before -LocalName 'RowDefinition';
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($Before.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $Before.NamespaceURI);
            $InsertedIndex = $RowDefinition.SelectNodes("preceding-sibling::p:RowDefinition", $nsmgr).Count;
            break;
        }
        'After' {
            $ParentElement = $After.ParentNode;
            $RowDefinition = Add-XmlElement After $After -LocalName 'RowDefinition';
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($After.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $After.NamespaceURI);
            $InsertedIndex = $RowDefinition.SelectNodes("preceding-sibling::p:RowDefinition", $nsmgr).Count;
            break;
        }
        'First' {
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($GridElement.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $GridElement.NamespaceURI);
            if ($GridElement.LocalName -ne 'Grid.RowDefinitions') {
                $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.RowDefinitions', $nsmgr);
                if ($null -eq $DefinitionsElement) {
                    $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.ColumnDefinitions', $nsmgr);
                    if ($null -eq $DefinitionsElement) {
                        $DefinitionsElement = Add-XmlElement -Parent $GridElement -First -LocalName 'Grid.RowDefinitions';
                    } else {
                        $DefinitionsElement = Add-XmlElement Before $DefinitionsElement -First -LocalName 'Grid.RowDefinitions';
                    }
                    $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition';
                } else {
                    $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition' -First;
                }
            } else {
                $ParentElement = $GridElement.ParentNode;
                $RowDefinition = Add-XmlElement -Parent $GridElement -LocalName 'RowDefinition' -First;
            }
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            $InsertedIndex = 0;
            break;
        }
        'Last' {
            $DefinitionsElement = $GridElement;
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($GridElement.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $GridElement.NamespaceURI);
            if ($DefinitionsElement.LocalName -ne 'Grid.RowDefinitions') {
                $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.RowDefinitions', $nsmgr);
                if ($null -eq $DefinitionsElement) {
                    $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.ColumnDefinitions', $nsmgr);
                    if ($null -eq $DefinitionsElement) {
                        $DefinitionsElement = Add-XmlElement -Parent $GridElement -First -LocalName 'Grid.RowDefinitions';
                    } else {
                        $DefinitionsElement = Add-XmlElement Before $DefinitionsElement -First -LocalName 'Grid.RowDefinitions';
                    }
                    $InsertedIndex = 0;
                } else {
                    $InsertedIndex = $DefinitionsElement.SelectNodes('p:RowDefinition', $nsmgr).Count;
                }
            } else {
                $ParentElement = $GridElement.ParentNode;
                $InsertedIndex = $GridElement.SelectNodes('p:RowDefinition', $nsmgr).Count;
            }
            $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition';
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            break;
        }
        default {
            $DefinitionsElement = $GridElement;
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($GridElement.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $GridElement.NamespaceURI);
            $XmlNodeList = $null;
            if ($DefinitionsElement.LocalName -ne 'Grid.RowDefinitions') {
                $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.RowDefinitions', $nsmgr);
                if ($null -eq $DefinitionsElement) {
                    $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.ColumnDefinitions', $nsmgr);
                    if ($null -eq $DefinitionsElement) {
                        $DefinitionsElement = Add-XmlElement -Parent $GridElement -First -LocalName 'Grid.RowDefinitions';
                    } else {
                        $DefinitionsElement = Add-XmlElement Before $DefinitionsElement -First -LocalName 'Grid.RowDefinitions';
                    }
                }
                $XmlNodeList = $DefinitionsElement.SelectNodes('p:RowDefinition', $nsmgr);
            } else {
                $ParentElement = $GridElement.ParentNode;
                $XmlNodeList = $GridElement.SelectNodes('p:RowDefinition', $nsmgr);
            }
            if ($XmlNodeList.Count -gt $InsertedIndex) {
                $RowDefinition = Add-XmlElement -Before $XmlNodeList[$InsertedIndex] -LocalName 'RowDefinition';
            } else {
                for ($i = $XmlNodeList.Count; $i -lt $InsertedIndex; $i++) {
                    (Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition') | Out-Null;
                }
                $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition';
            }
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            break;
        }
    }
    $OwnerDocument = $ParentElement.OwnerDocument;
    if ($PSBoundParameters.ContainsKey('Height')) {
        foreach ($Rd in $RowDefinitionElements) {
            $rd.Attributes.Append($OwnerDocument.CreateAttribute('Height')).Value = $Height;
        }
    }
    if ($InsertedIndex -gt 0) {
        foreach ($XmlAttribute in @($ParentElement.SelectNodes('p:*/@Grid.Row', $nsmgr))) {
            $i = 0;
            try { $i = [System.Xml.XmlConvert]::ToInt32($XmlAttribute.Value) } catch { $i = 0 }
            if ($i -ge $InsertedIndex) {
                $XmlAttribute.Value = [System.Xml.XmlConvert]::ToString($i + $RowCount);
            }
        }
    } else {
        foreach ($Element in @($ParentElement.SelectNodes('p:*', $nsmgr))) {
            $XmlAttribute = $Element.SelectSingleNode('@Grid.Row');
            if ($null -eq $XmlAttribute) {
                if (-not $Element.LocalName.Contains('.')) {
                    $Element.Attributes.Append($OwnerDocument.CreateAttribute('Grid.Row')).Value = "$RowCount";
                }
            } else {
                try {
                    $XmlAttribute.Value = [System.Xml.XmlConvert]::ToString([System.Xml.XmlConvert]::ToInt32($XmlAttribute.Value) + $RowCount);
                } catch {
                    $XmlAttribute.Value = "$RowCount";
                }
            }
        }
    }
    if ($PassThru.IsPresent) { RowDefinitionElements | Write-Output }
}

Function Get-FsInfoCatProjectPath {
    [CmdletBinding(DefaultParameterSetName = 'Base')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Base')]
        [switch]$Base,

        [Parameter(Mandatory = $true, ParameterSetName = 'Local')]
        [switch]$Local,

        [Parameter(Mandatory = $true, ParameterSetName = 'Upstream')]
        [switch]$Upstream
    )

    switch ($PSCmdlet.ParameterSetName) {
        'Local' {
            $PSScriptRoot | Join-Path -ChildPath ($MyInvocation.MyCommand.Module.PrivateData['FsInfoCatLocalProjectPath']);
            break;
        }
        'Upstream' {
            $PSScriptRoot | Join-Path -ChildPath ($MyInvocation.MyCommand.Module.PrivateData['FsInfoCatUpstreamProjectPath']);
            break;
        }
        default {
            $PSScriptRoot | Join-Path -ChildPath ($MyInvocation.MyCommand.Module.PrivateData['FsInfoCatProjectPath']);
            break;
        }
    }
}

Function Get-ExceptionObject {
    [CmdletBinding(DefaultParameterSetName = 'WcPath')]
    [OutputType([System.Xml.Linq.XDocument])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyCollection()]
        [object[]]$InputObject
    )

    Process {
        if ($null -ne $InputObject -and $InputObject.Length -gt 0) {
            foreach ($obj in $InputObject) {
                if ($null -ne $obj) {
                    if ($obj -is [System.Management.Automation.ErrorRecord]) {
                        Get-ExceptionObject -InputObject $obj.Exception;
                    } else {
                        if ($obj -is [System.Exception]) {
                            $e = $Obj;
                            if (($obj -is [System.Management.Automation.MethodInvocationException] -or $obj -is [System.Management.Automation.GetValueInvocationException] `
                                    -or $obj -is [System.Management.Automation.SetValueInvocationException]) -and $null -ne $obj.InnerException) {
                                $e = $obj.InnerException;
                            }
                            if ($e -is [System.AggregateException] -and $e.InnerExceptions.Count -eq 1) {
                                Write-Output -InputObject $e.InnerException -NoEnumerate;
                            } else {
                                Write-Output -InputObject $e -NoEnumerate;
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Read-ProjectDir {
    [CmdletBinding(DefaultParameterSetName = 'WcPath')]
    [OutputType([DevUtil.ProjectDir])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'WcPath')]
        [SupportsWildcards()]
        [string[]]$Path,

        [Parameter(Mandatory = $true, ParameterSetName = 'LiteralPath')]
        [string[]]$LiteralPath
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'LiteralPath') {
            foreach ($P in $LiteralPath) {
                $DirectoryInfo = $null;
                try { $DirectoryInfo = [System.IO.DirectoryInfo]::new($P) }
                catch {
                    Write-Error -ErrorRecord $_ -CategoryActivity "[System.IO.DirectoryInfo]::new('$P')" -CategoryReason 'System.IO.DirectoryInfo constructor threw an exception' -CategoryTargetName 'LiteralPath';
                }
                if ($null -ne $DirectoryInfo) {
                    if ($DirectoryInfo.Exists) {
                        [DevUtil.ProjectDir]::new($DirectoryInfo) | Write-Output;
                    } else {
                        Write-Error -Message 'Project subdirectory not found' -Category ObjectNotFound -ErrorId 'Read-ProjectDir:DirectoryNotFound' -CategoryTargetName 'LiteralPath' -TargetObject $DirectoryInfo;
                    }
                }
            }
        } else {
            foreach ($P in ($Path | Resolve-Path)) {
                $DirectoryInfo = $null;
                try { $DirectoryInfo = [System.IO.DirectoryInfo]::new($P.Path) }
                catch {
                    Write-Error -ErrorRecord $_ -CategoryActivity "[System.IO.DirectoryInfo]::new('$($P.Path)')" -CategoryReason 'System.IO.DirectoryInfo constructor threw an exception' -CategoryTargetName 'LiteralPath';
                }
                if ($null -ne $DirectoryInfo) {
                    if ($DirectoryInfo.Exists) {
                        [DevUtil.ProjectDir]::new($DirectoryInfo) | Write-Output;
                    } else {
                        Write-Error -Message 'Project subdirectory not found' -Category ObjectNotFound -ErrorId 'Read-ProjectDir:DirectoryNotFound' -CategoryTargetName 'LiteralPath' -TargetObject $DirectoryInfo;
                    }
                }
            }
        }
    }
}

Function Start-GetSyntaxTree {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [DevUtil.CsSourceFile[]]$SourceFile
    )

    Begin {
        $AllFiles = [System.Collections.ObjectModel.Collection[DevUtil.CsSourceFile[]]]::new();
    }

    Process {
        $SourceFile | ForEach-Object { $AllFiles.Add($_) }
    }

    End {
        if ($AllFiles.Count -eq 1) {
            Write-Output -InputObject $AllFiles[0].StartGetSyntaxTree() -NoEnumerate;
        } else {
            Write-Output -InputObject ([DevUtil.CsSourceFile].StartGetSyntaxTree($AllFiles)) -NoEnumerate;
        }
    }
}

Function Start-GetCompilationUnitRoot {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [DevUtil.CsSourceFile[]]$SourceFile
    )

    Begin {
        $AllFiles = [System.Collections.ObjectModel.Collection[DevUtil.CsSourceFile[]]]::new();
    }

    Process {
        $SourceFile | ForEach-Object { $AllFiles.Add($_) }
    }

    End {
        if ($AllFiles.Count -eq 1) {
            Write-Output -InputObject $AllFiles[0].StartGetCompilationUnitRoot() -NoEnumerate;
        } else {
            Write-Output -InputObject ([DevUtil.CsSourceFile].StartGetCompilationUnitRoot($AllFiles)) -NoEnumerate;
        }
    }
}
