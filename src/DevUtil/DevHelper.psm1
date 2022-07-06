#region Xml Functions

Set-Variable -Name 'XamlNamespaces' -Option Constant -Scope 'Script' -Value ([PSCustomObject]@{
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation";
    x="http://schemas.microsoft.com/winfx/2006/xaml";
    mc="http://schemas.openxmlformats.org/markup-compatibility/2006";
    d="http://schemas.microsoft.com/expression/blend/2008";
});

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

#endregion

#region XLinq Functions

Function Select-XElement {
    [CmdletBinding(DefaultParameterSetName = 'Elements')]
    [OutputType([System.Xml.Linq.XAttribute])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XContainer]$Parent,

        [Parameter(ParameterSetName = 'Elements')]
        [Parameter(ParameterSetName = 'AttributeEqualsString')]
        [Parameter(ParameterSetName = 'ChildElementEqualsString')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTime')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'AttributeEqualsGuid')]
        [Parameter(ParameterSetName = 'ChildElementEqualsGuid')]
        [Parameter(ParameterSetName = 'AttributeEqualsBoolean')]
        [Parameter(ParameterSetName = 'ChildElementEqualsBoolean')]
        [Parameter(ParameterSetName = 'AttributeEqualsChar')]
        [Parameter(ParameterSetName = 'ChildElementEqualsChar')]
        [Parameter(ParameterSetName = 'AttributeEqualsDecimal')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDecimal')]
        [Parameter(ParameterSetName = 'AttributeEqualsSByte')]
        [Parameter(ParameterSetName = 'ChildElementEqualsSByte')]
        [Parameter(ParameterSetName = 'AttributeEqualsInt16')]
        [Parameter(ParameterSetName = 'ChildElementEqualsInt16')]
        [Parameter(ParameterSetName = 'AttributeEqualsInt32')]
        [Parameter(ParameterSetName = 'ChildElementEqualsInt32')]
        [Parameter(ParameterSetName = 'AttributeEqualsInt64')]
        [Parameter(ParameterSetName = 'ChildElementEqualsInt64')]
        [Parameter(ParameterSetName = 'AttributeEqualsByte')]
        [Parameter(ParameterSetName = 'ChildElementEqualsByte')]
        [Parameter(ParameterSetName = 'AttributeEqualsUInt16')]
        [Parameter(ParameterSetName = 'ChildElementEqualsUInt16')]
        [Parameter(ParameterSetName = 'AttributeEqualsUInt32')]
        [Parameter(ParameterSetName = 'ChildElementEqualsUInt32')]
        [Parameter(ParameterSetName = 'AttributeEqualsUInt64')]
        [Parameter(ParameterSetName = 'ChildElementEqualsUInt64')]
        [Parameter(ParameterSetName = 'AttributeEqualsSingle')]
        [Parameter(ParameterSetName = 'ChildElementEqualsSingle')]
        [Parameter(ParameterSetName = 'AttributeEqualsDouble')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDouble')]
        [Parameter(ParameterSetName = 'AttributeEqualsTimeSpan')]
        [Parameter(ParameterSetName = 'ChildElementEqualsTimeSpan')]
        [System.Xml.Linq.XName]$ElementName,

        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.Xml.Linq.XNamespace]$Namespace,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEquals')]
        [System.Xml.Linq.XName]$AttributeName,

        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEquals')]
        [System.Xml.Linq.XName]$ChildElementName,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsString')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsString')]
        [string]$Equals,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDateTime')]
        [string]$EqualsDateTime,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsBoolean')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsBoolean')]
        [string]$EqualsBoolean,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDecimal')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDecimal')]
        [string]$EqualsDecimal,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsSByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsSByte')]
        [string]$EqualsSByte,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt16')]
        [string]$EqualsInt16,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt32')]
        [string]$EqualsInt32,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt64')]
        [string]$EqualsInt64,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsByte')]
        [string]$EqualsByte,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt16')]
        [string]$EqualsUInt16,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt32')]
        [string]$EqualsUInt32,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt64')]
        [string]$EqualsUInt64,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsSingle')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsSingle')]
        [string]$EqualsSingle,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDouble')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDouble')]
        [string]$EqualsDouble,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsTimeSpan')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsTimeSpan')]
        [string]$EqualsTimeSpan,

        [Parameter(ParameterSetName = 'AttributeEqualsString')]
        [Parameter(ParameterSetName = 'ChildElementEqualsString')]
        [System.StringComparison]$Comparison = [System.StringComparison]::CurrentCulture,

        [Parameter(ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTime')]
        [System.Xml.XmlDateTimeSerializationMode]$DateTimeSerializationMode = [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind,

        [Parameter(ParameterSetName = 'ChildElementEqualsString')]
        [switch]$Trim
    )

    Begin {
        [System.StringComparer]$Comparer = $null;
        if (('AttributeEqualsString', 'ChildElementEqualsString') -contains $PSCmdlet.ParameterSetName) {
            switch ($Comparison) {
                CurrentCultureIgnoreCase {
                    $Comparer = [System.StringComparer]::CurrentCultureIgnoreCase;
                    break;
                }
                InvariantCulture {
                    $Comparer = [System.StringComparer]::InvariantCulture;
                    break;
                }
                InvariantCultureIgnoreCase {
                    $Comparer = [System.StringComparer]::InvariantCultureIgnoreCase;
                    break;
                }
                Ordinal {
                    $Comparer = [System.StringComparer]::Ordinal;
                    break;
                }
                OrdinalIgnoreCase {
                    $Comparer = [System.StringComparer]::OrdinalIgnoreCase;
                    break;
                }
                default {
                    $Comparer = [System.StringComparer]::CurrentCulture;
                    break;
                }
            }
        }
    }

    Process {
        switch ($PSCmdlet.ParameterSetName) {
            'Namespace' {
                $Parent.Elements() | Where-Object { $_.Name.Namespace -eq $Namespace } | Write-Output;
                break;
            }
            'AttributeEqualsString' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        return $null -ne $a -and $Comparer.Equals($a.Value, $Equals);
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        return $null -ne $a -and $Comparer.Equals($a.Value, $Equals);
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsString' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    if ($Trim.IsPresent) {
                        $Parent.Elements($ElementName) | Where-Object {
                            $e = $_.Element($ChildElementName);
                            return $null -ne $e -and (-not $e.IsEmpty) -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    } else {
                        $Parent.Elements($ElementName) | Where-Object {
                            $e = $_.Element($ChildElementName);
                            if ($null -eq $e -or $e.IsEmpty) { return $false }
                            $t = $e.Value.Trim();
                            return $t.Length -gt 0 -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    }
                } else {
                    if ($Trim.IsPresent) {
                        $Parent.Elements() | Where-Object {
                            $e = $_.Element($ChildElementName);
                            if ($null -eq $e -or $e.IsEmpty) { return $false }
                            $t = $e.Value.Trim();
                            return $t.Length -gt 0 -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    } else {
                        $Parent.Elements() | Where-Object {
                            $e = $_.Element($ChildElementName);
                            return $null -ne $e -and (-not $e.IsEmpty) -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    }
                }
                break;
            }
            'AttributeEqualsDateTime' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDateTime($a.Value, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDateTime($a.Value, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsDateTime' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDateTime($t, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDateTime($t, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsGuid' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToGuid($a.Value);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToGuid($a.Value);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsGuid' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToGuid($t);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToGuid($t);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsBoolean' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToBoolean($a.Value);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToBoolean($a.Value);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsBoolean' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToBoolean($t);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToBoolean($t);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsDecimal' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDecimal($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDecimal($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsDecimal' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDecimal($t);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDecimal($t);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsSByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsSByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSByte($t);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSByte($t);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt16($t);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt16($t);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt32($t);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt32($t);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt64($t);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt64($t);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToByte($t);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToByte($t);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsUInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsUInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt16($t);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt16($t);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsUInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsUInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt32($t);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt32($t);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsUInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsUInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt64($t);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt64($t);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsSingle' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSingle($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSingle($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsSingle' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSingle($t);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSingle($t);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsDouble' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDouble($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDouble($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsDouble' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDouble($t);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDouble($t);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsTimeSpan' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($a.Value);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($a.Value);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsTimeSpan' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($t);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($t);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                }
                break;
            }
            default {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Write-Output;
                } else {
                    $Parent.Elements() | Write-Output;
                }
                break;
            }
        }
    }
}

Function Select-XAttribute {
    [CmdletBinding(DefaultParameterSetName = 'Name')]
    [OutputType([System.Xml.Linq.XAttribute])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Element,

        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.Xml.Linq.XNamespace]$Namespace,

        [Parameter(ParameterSetName = 'Name')]
        [System.Xml.Linq.XName]$Name
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'Namespace') {
            $Element.Attributes() | Where-Object { $_.Name.Namespace -eq $Namespace } | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('Name')) {
                $a = $Element.Attribute($Name);
                if ($null -ne $a) { $a | Write-Output }
            } else {
                $Element.Attributes() | Write-Output;
            }
        }
    }
}


#endregion

#region XSD Functions

Function New-XsdRestrictedSimpleType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Base
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('restriction'), [DevUtil.XLinqHelper]::NewAttribute('base', $Base)) }

    Process { $XElement.Add($Content) }

    End { [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('restriction'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name), $XElement) | Write-Output }
}

Function New-XsdAttributeGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attributeGroup'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name)) }

    Process { $XElement.Add($Content) }

    End { $XElement | Write-Output }
}

Function New-XsdGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name)) }

    Process { $XElement.Add($Content) }

    End { $XElement | Write-Output }
}

Function New-XsdAttribute {
    [CmdletBinding(DefaultParameterSetName = 'Optional')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Ref')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Ref,

        [Parameter(Mandatory = $true, ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Type,

        [Parameter(ParameterSetName = 'Optional')]
        [string]$DefaultValue,

        [Parameter(ParameterSetName = 'Optional')]
        [switch]$Optional,

        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [switch]$Required
    )

    if ($Required.IsPresent) {
        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attribute'),
            [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
            [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
            [DevUtil.XLinqHelper]::NewAttribute('use', 'required')
        ) | Write-Output;
    } else {
        if ($PSCmdlet.ParameterSetName -eq 'Ref') {
            [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attributeGroup'), [DevUtil.XLinqHelper]::NewAttribute('ref', $Ref)) | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('DefaultValue')) {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attribute'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('use', 'optional'),
                    [DevUtil.XLinqHelper]::NewAttribute('default', $DefaultValue)
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attribute'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('use', 'optional')
                ) | Write-Output;
            }
        }
    }
}

Function New-XsdComplexType {
    [CmdletBinding()]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Extends,

        [switch]$Abstract,

        [switch]$Mixed
    )

    Begin {
        [System.Xml.Linq.XElement]$ParentElement = $null;
        [System.Xml.Linq.XElement]$ComplexTypeElement = $null;
        if ($PSBoundParameters.ContainsKey('Extends')) {
            $ParentElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('extension'), [DevUtil.XLinqHelper]::NewAttribute('base', $Extends));
            if ($Abstract.IsPresent) {
                if ($Mixed.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true'),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'),
                            [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true'),
                            $ParentElement
                        )
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true'),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'), $ParentElement)
                    );
                }
            } else {
                if ($Mixed.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'),
                            [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true'),
                            $ParentElement
                        )
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'), $ParentElement)
                    );
                }
            }
        } else {
            if ($Mixed.IsPresent) {
                if ($Abstract.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true'),
                        [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true')
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true')
                    );
                }
            } else {
                if ($Abstract.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true')
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name));
                }
            }
            $ParentElement = $ComplexTypeElement;
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $ParentElement.Add($_) } }

    End { $ComplexTypeElement | Write-Output }
}

Function New-XsdSequence {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'), [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'));
                }
            }
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $XElement.Add($_) } }

    End { $XElement | Write-Output }
}

Function New-XsdChoice {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'), [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'));
                }
            }
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $XElement.Add($_) } }

    End { $XElement | Write-Output }
}

Function New-XsdAll {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'), [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'));
                }
            }
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $XElement.Add($_) } }

    End { $XElement | Write-Output }
}

Function New-XsdElement {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxExplicit')]
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Group')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Group,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxExplicit')]
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Type,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    if ($PSCmdlet.ParameterSetName -eq 'Group') {
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                    [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                    [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                        [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                        [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                        [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'), [DevUtil.XLinqHelper]::NewAttribute('ref', $Group)) | Write-Output;
                }
            }
        }
    } else {
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type)
                    ) | Write-Output;
                }
            }
        }
    }
}

#endregion

#region Type Functions

Function ConvertTo-CsTypeName {
    [CmdletBinding()]
    [OutputType([string])]
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
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)[]";
                    } else {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)[$([string]::new(([char]','), $r - 1))]";
                    }
                } else {
                    if ($r -lt 2) {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType())[]";
                    } else {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType())[$([string]::new(([char]','), $r - 1))]";
                    }
                }
                break;
            }
            { $_.IsByRef } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)&";
                } else {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType())&";
                }
                break;
            }
            { $_.IsPointer } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)*";
                } else {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType())*";
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
            { $_ -eq [System.Void] } { 'void' | Write-Output; break; }
            { $_.IsValueType -and $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Nullable`1] } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-CsTypeName -Type ([System.Nullable]::GetUnderlyingType($Type)) -UsingNamespace $UsingNamespace)?";
                } else {
                    "$(ConvertTo-CsTypeName -Type ([System.Nullable]::GetUnderlyingType($Type)))?";
                }
                break;
            }
            default {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    (ConvertTo-NameSyntax $Type.DeclaringType -UsingNamespace $UsingNamespace).ToString() | Write-Output;
                } else {
                    (ConvertTo-NameSyntax $Type.DeclaringType).ToString() | Write-Output;
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
        [System.Reflection.Assembly[]]$AllAssemblies = $Assembly;
        if (-not $PSBoundParameters.ContainsKey('Assembly')) {
            [System.Reflection.Assembly[]]$AllAssemblies = [System.AppDomain]::CurrentDomain.GetAssemblies();
        }
    }

    Process {
        if (-not $Type.IsSealed) {
            if ($Directly.IsPresent) {
                $AllAssemblies | ForEach-Object { $_.GetTypes() } | Where-Object { $_ | Test-TypeExtends -BaseType $Type -Directly }
            } else {
                $AllAssemblies | ForEach-Object { $_.GetTypes() } | Where-Object { $_ | Test-TypeExtends -BaseType $Type }
            }
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

Function Get-UnderlyingType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type
    )

    Process {
        if ($Type.IsArray -or $Type.IsPointer -or $Type.IsByRef) {
            (Get-UnderlyingType -Type $Type.GetElementType()) | Write-Output;
        } else {
            if ($Type.IsEnum) {
                $Type.GetEnumUnderlyingType() | Write-Output;
            } else {
                if ($Type.IsConstructedGenericType) {
                    if ($Type.GetGenericTypeDefinition() -eq [System.Nullable`1]) {
                        [System.Nullable]::GetUnderlyingType($Type) | Write-Output;
                    } else {
                        if ($Type.GetGenericTypeDefinition() -eq [System.Collections.Generic.IEnumerable`1]) {
                            $Type.GetGenericArguments() | Write-Output;
                        } else {
                            @($Type.GetInterfaces() | Where-Object {
                                $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Collections.Generic.IEnumerable`1] -and -not $_.GetGenericArguments()[0].IsGenericMethodParameter;
                            }) | Write-Output;
                        }
                    }
                }
            }
        }
    }
}

Function Get-TypeMemberInfo {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [string[]]$UsingNamespace,

        [switch]$IgnoreInherited
    )

    Process {
        $UnderlyingType = @(Get-UnderlyingType -Type $Type);
        $Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = (ConvertTo-TypeSyntax -Type $Type -UsingNamespace $UsingNamespace).ToString();
        } else {
            $Code = (ConvertTo-TypeSyntax -Type $Type).ToString();
        }
        if ($Type.IsPointer -or $Type.IsByRef -or $Type.IsArray) {
            $ElementType = $UnderlyingType[0];
            if ($ElementType.IsPublic) {
                "public $Code" | Write-Output;
            } else {
                $Code | Write-Output;
            }
            if ($ElementType -ne [string] -and $ElementType -ne [object] -and $ElementType -ne [decimal] -and -not $ElementType.IsPrimitive) {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    if ($IgnoreInherited.IsPresent) {
                        (Get-TypeMemberInfo -Type $ElementType -UsingNamespace $UsingNamespace -IgnoreInherited) | ForEach-Object { "  $_" | Write-Output }
                    } else {
                        (Get-TypeMemberInfo -Type $ElementType -UsingNamespace $UsingNamespace) | ForEach-Object { "  $_" | Write-Output }
                    }
                } else {
                    if ($IgnoreInherited.IsPresent) {
                        (Get-TypeMemberInfo -Type $ElementType -IgnoreInherited) | ForEach-Object { "  $_" | Write-Output }
                    } else {
                        (Get-TypeMemberInfo -Type $ElementType) | ForEach-Object { "  $_" | Write-Output }
                    }
                    (Get-TypeMemberInfo -Type $ElementType) | ForEach-Object { "  $_" | Write-Output }
                }
            }
        } else {
            if ($Type.IsEnum) {
                [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax[]]$Fields = @();
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax -UsingNamespace $UsingNamespace);
                    if ($Type.IsPublic) {
                        "public enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0] -UsingNamespace $UsingNamespace)" | Write-Output;
                    } else {
                        "enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0] -UsingNamespace $UsingNamespace)" | Write-Output;
                    }
                } else {
                    $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                    if ($Type.IsPublic) {
                        "public enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0])" | Write-Output;
                    } else {
                        "enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0])" | Write-Output;
                    }
                }
                $Fields | ForEach-Object { "  $_" | Write-Output }
            } else {
                if ($UnderlyingType.Count -gt 0) {
                    $UnderlyingType = @($UnderlyingType | Where-Object {
                        $ElementType -ne [string] -and $ElementType -ne [object] -and $ElementType -ne [decimal] -and -not $ElementType.IsPrimitive
                    });
                }
                if ($UnderlyingType.Count -eq 0) {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax[]]$Interfaces = @();
                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax[]]$Methods = @();
                    [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax[]]$Properties = @();
                    [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax[]]$Events = @();
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        if ($IgnoreInherited.IsPresent) {
                            $BaseNames = @($Type.BaseType.GetInterfaces() | ForEach-Object { $_.FullName });
                            $Interfaces = @($Type.GetInterfaces() | Where-Object { $BaseNames -cnotcontains $_.FullName } | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace);
                            $BaseNames = @($Type.BaseType.GetMethods() | ForEach-Object { $_.ToString() });
                            $Methods = @($Type.GetMethods() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-MethodDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $BaseNames = @($Type.BaseType.GetProperties() | ForEach-Object { $_.ToString() });
                            $Properties = @($Type.GetProperties() | Where-Object {
                                if ($BaseNames -ccontains $_.ToString()) { return $false }
                                $M = $_.GetGetMethod();
                                if ($null -eq $M) {
                                    $M = $_.GetSetMethod();
                                    return $null -eq $M -or $M.DeclaringType -eq $Type;
                                }
                                return $M.DeclaringType -eq $Type
                            } | ConvertTo-PropertyDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $BaseNames = @($Type.BaseType.GetEvents() | ForEach-Object { $_.ToString() });
                            $Events = @($Type.GetEvents() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-EventDeclarationSyntax -UsingNamespace $UsingNamespace);
                        } else {
                            $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $Interfaces = @($Type.GetInterfaces() | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace);
                            $Methods = @($Type.GetMethods() | ConvertTo-MethodDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $Properties = @($Type.GetProperties() | ConvertTo-PropertyDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $Events = @($Type.GetEvents() | ConvertTo-EventDeclarationSyntax -UsingNamespace $UsingNamespace);
                        }
                    } else {
                        if ($IgnoreInherited.IsPresent) {
                            $BaseNames = @($Type.BaseType.GetInterfaces() | ForEach-Object { $_.FullName });
                            $Interfaces = @($Type.GetInterfaces() | Where-Object { $BaseNames -cnotcontains $_.FullName } | ConvertTo-TypeSyntax);
                            $BaseNames = @($Type.BaseType.GetMethods() | ForEach-Object { $_.ToString() });
                            $Methods = @($Type.GetMethods() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-MethodDeclarationSyntax);
                            $BaseNames = @($Type.BaseType.GetProperties() | ForEach-Object { $_.ToString() });
                            $Properties = @($Type.GetProperties() | Where-Object {
                                if ($BaseNames -ccontains $_.ToString()) { return $false }
                                $M = $_.GetGetMethod();
                                if ($null -eq $M) {
                                    $M = $_.GetSetMethod();
                                    return $null -eq $M -or $M.DeclaringType -eq $Type;
                                }
                                return $M.DeclaringType -eq $Type
                            } | ConvertTo-PropertyDeclarationSyntax);
                            $BaseNames = @($Type.BaseType.GetEvents() | ForEach-Object { $_.ToString() });
                            $Events = @($Type.GetEvents() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-EventDeclarationSyntax);
                        } else {
                            $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                            $Interfaces = @($Type.GetInterfaces() | ConvertTo-TypeSyntax);
                            $Methods = @($Type.GetMethods() | ConvertTo-MethodDeclarationSyntax);
                            $Properties = @($Type.GetProperties() | ConvertTo-PropertyDeclarationSyntax);
                            $Events = @($Type.GetEvents() | ConvertTo-EventDeclarationSyntax);
                        }
                    }
                    if ($Type.IsInterface) {
                        if ($Interfaces.Count -gt 0) {
                            if ($Type.IsPublic) {
                                "public interface $Code :" | Write-Output;
                            } else {
                                "interface $Code :" | Write-Output;
                            }
                            ($Interfaces | Select-Object -SkipLast 1) | ForEach-Object { "    $_," | Write-Output }
                            "    $($Interfaces[$Interfaces.Count - 1])" | Write-Output;
                        } else {
                            if ($Type.IsPublic) {
                                "public interface $Code" | Write-Output;
                            } else {
                                "interface $Code" | Write-Output;
                            }
                        }
                        $Events | ForEach-Object { "  $_" | Write-Output }
                        $Properties | ForEach-Object { "  $_" | Write-Output }
                        $Methods | ForEach-Object { "  $_" | Write-Output }
                    } else {
                        [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax[]]$Fields = @();
                        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax[]]$Constructors = @();
                        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                            if ($IgnoreInherited.IsPresent) {
                                $BaseNames = @($Type.BaseType.GetFields() | ForEach-Object { $_.Name });
                                $Fields = @($Type.GetFields() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.Name } | ConvertTo-FieldDeclarationSyntax -UsingNamespace $UsingNamespace);
                                $Constructors = @($Type.GetConstructors() | Where-Object { $_.DeclaringType -eq $Type } | ConvertTo-ConstructorDeclarationSyntax -UsingNamespace $UsingNamespace);
                            } else {
                                $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                                $Constructors = @($Type.GetConstructors() | ConvertTo-ConstructorDeclarationSyntax);
                            }
                        } else {
                            if ($IgnoreInherited.IsPresent) {
                                $BaseNames = @($Type.BaseType.GetFields() | ForEach-Object { $_.Name });
                                $Fields = @($Type.GetFields() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.Name } | ConvertTo-FieldDeclarationSyntax);
                                $Constructors = @($Type.GetConstructors() | Where-Object { $_.DeclaringType -eq $Type } | ConvertTo-ConstructorDeclarationSyntax);
                            } else {
                                $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                                $Constructors = @($Type.GetConstructors() | ConvertTo-ConstructorDeclarationSyntax);
                            }
                        }
                        if ($Type.IsValueType) {
                            if ($Interfaces.Count -gt 0) {
                                if ($Type.IsPublic) {
                                    "public struct $Code :" | Write-Output;
                                } else {
                                    "struct $Code :" | Write-Output;
                                }
                                ($Interfaces | Select-Object -SkipLast 1) | ForEach-Object { "    $_," | Write-Output }
                                "    $($Interfaces[$Interfaces.Count - 1])" | Write-Output;
                            } else {
                                if ($Type.IsPublic) {
                                    "public struct $Code" | Write-Output;
                                } else {
                                    "struct $Code" | Write-Output;
                                }
                            }
                        } else {
                            if ($Type.IsClass) {
                                if ($Type.IsSealed) {
                                    if ($Type.IsPublic) {
                                        $Code = "public sealed class $Code";
                                    } else {
                                        $Code = "sealed class $Code" | Write-Output;
                                    }
                                } else {
                                    if ($Type.IsPublic) {
                                        $Code = "public class $Code";
                                    } else {
                                        $Code = "class $Code";
                                    }
                                }
                            }
                            $BaseType = $Type.BaseType;
                            if ($Interfaces.Count -gt 0) {
                                "$Code :" | Write-Output;
                                if ($null -ne $BaseType) {
                                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType -UsingNamespace $UsingNamespace)," | Write-Output;
                                    } else {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType)," | Write-Output;
                                    }
                                }
                                ($Interfaces | Select-Object -SkipLast 1) | ForEach-Object { "    $_," | Write-Output }
                                "    $($Interfaces[$Interfaces.Count - 1])" | Write-Output;
                            } else {
                                if ($null -ne $BaseType) {
                                    "$Code :" | Write-Output;
                                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType -UsingNamespace $UsingNamespace)" | Write-Output;
                                    } else {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType)" | Write-Output;
                                    }
                                } else {
                                    $Code | Write-Output;
                                }
                            }
                        }
                    }
                    $Fields | ForEach-Object { "  $_" | Write-Output }
                    $Events | ForEach-Object { "  $_" | Write-Output }
                    $Properties | ForEach-Object { "  $_" | Write-Output }
                    $Constructors | ForEach-Object { "  $_" | Write-Output }
                    $Methods | ForEach-Object { "  $_" | Write-Output }
                } else {
                    if ($Type.IsInterface) {
                        if ($Type.IsPublic) {
                            "public interface $Code" | Write-Output;
                        } else {
                            "interface $Code" | Write-Output;
                        }
                    } else {
                        if ($Type.IsValueType) {
                            if ($Type.IsPublic) {
                                "public struct $Code" | Write-Output;
                            } else {
                                "struct $Code" | Write-Output;
                            }
                        } else {
                            if ($Type.IsClass) {
                                if ($Type.IsSealed) {
                                    if ($Type.IsPublic) {
                                        "public sealed class $Code" | Write-Output;
                                    } else {
                                        "sealed class $Code" | Write-Output;
                                    }
                                } else {
                                    if ($Type.IsPublic) {
                                        "public class $Code" | Write-Output;
                                    } else {
                                        "class $Code" | Write-Output;
                                    }
                                }
                            } else {
                                if ($Type.IsPublic) {
                                    "public $Code" | Write-Output;
                                } else {
                                    $Code | Write-Output;
                                }
                            }
                        }
                    }
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        if ($IgnoreInherited.IsPresent) {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ -UsingNamespace $UsingNamespace -IgnoreInherited } | ForEach-Object { "  $_" | Write-Output }
                        } else {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ -UsingNamespace $UsingNamespace } | ForEach-Object { "  $_" | Write-Output }
                        }
                    } else {
                        if ($IgnoreInherited.IsPresent) {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ -IgnoreInherited } | ForEach-Object { "  $_" | Write-Output }
                        } else {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ } | ForEach-Object { "  $_" | Write-Output }
                        }
                    }
                }
            }
        }
    }
}

#endregion

#region TypeSyntax Conversion Functions

Function ConvertTo-TypeSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [string[]]$UsingNamespace
    )

    Process {
        switch ($Type) {
            { $_.IsArray } {
                $NameSyntax = $null;
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    $NameSyntax = ConvertTo-TypeSyntax -Type $Type.GetElementType() -UsingNamespace $UsingNamespace;
                } else {
                    $NameSyntax = ConvertTo-TypeSyntax -Type $Type.GetElementType();
                }
                $r = $Type.GetArrayRank();
                if ($r -lt 2) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$NameSyntax[]")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$$NameSyntax[$([string]::new(([char]','), $r - 1))]")) -NoEnumerate;
                }
                break;
            }
            { $_.IsByRef } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)&")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType())&")) -NoEnumerate;
                }
                break;
            }
            { $_.IsPointer } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)*")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType())*")) -NoEnumerate;
                }
                break;
            }
            { $_ -eq [System.Byte] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('byte') | Write-Output; break; }
            { $_ -eq [System.SByte] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('sbyte') | Write-Output; break; }
            { $_ -eq [System.Char] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('char') | Write-Output; break; }
            { $_ -eq [System.Int16] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('short') | Write-Output; break; }
            { $_ -eq [System.Int32] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('int') | Write-Output; break; }
            { $_ -eq [System.Int64] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('long') | Write-Output; break; }
            { $_ -eq [System.UInt16] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('ushort') | Write-Output; break; }
            { $_ -eq [System.UInt32] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('uint') | Write-Output; break; }
            { $_ -eq [System.UInt64] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('ulong') | Write-Output; break; }
            { $_ -eq [System.Single] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('float') | Write-Output; break; }
            { $_ -eq [System.Double] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('double') | Write-Output; break; }
            { $_ -eq [System.Decimal] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('decimal') | Write-Output; break; }
            { $_ -eq [System.String] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('string') | Write-Output; break; }
            { $_ -eq [System.Void] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('void') | Write-Output; break; }
            { $_.IsGenericParameter } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName($Type.Name) | Write-Output; break; }
            { $_.IsValueType -and $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Nullable`1] } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type ([System.Nullable]::GetUnderlyingType($Type)) -UsingNamespace $UsingNamespace)?")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type ([System.Nullable]::GetUnderlyingType($Type)))?")) -NoEnumerate;
                }
                break;
            }
            default {
                $LeftSyntax = $null;
                if ($Type.IsNested) {
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        $LeftSyntax = ConvertTo-TypeSyntax -Type $Type.DeclaringType -UsingNamespace $UsingNamespace;
                    } else {
                        $LeftSyntax = ConvertTo-TypeSyntax -Type $Type.DeclaringType;
                    }
                } else {
                    if (-not ([string]::IsNullOrEmpty($Type.Namespace) -or ($PSBoundParameters.ContainsKey('UsingNamespace') -and $UsingNamespace -ccontains $Type.Namespace))) {
                        $LeftSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseName($Type.Namespace);
                    }
                }
                $RightSyntax = $null;
                $n = $Type.Name;
                if ($Type.IsConstructedGenericType) {
                    $n = $n.Substring(0, $n.IndexOf('`'));
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$n<$((($Type.GetGenericArguments() | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace) | ForEach-Object { $_.ToString() }) -join ',')>");
                    } else {
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$n<$((($Type.GetGenericArguments() | ConvertTo-TypeSyntax) | ForEach-Object { $_.ToString() }) -join ',')>");
                    }
                } else {
                    if ($Type.IsGenericTypeDefinition) {
                        $n = $n.Substring(0, $n.IndexOf('`'));
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$n$($n.Substring(0, $n.IndexOf('`'))){$($Type.GetGenericArguments() -join ', ')}");
                    } else {
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName($n) | Write-Output;
                    }
                }
                if ($null -eq $LeftSyntax) {
                    Write-Output -InputObject $RightSyntax -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::QualifiedName($LeftSyntax, $RightSyntax)) -NoEnumerate;
                }
                break;
            }
        }
    }
}

Function ConvertTo-PropertyDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.PropertyInfo]$Property,

        [string[]]$UsingNamespace
    )

    Process {
        [System.Reflection.ParameterInfo[]]$IndexParameters = $Property.GetIndexParameters();
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$PropertyTypeSyntax = $null;
        $Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $PropertyTypeSyntax = ConvertTo-TypeSyntax -Type $Property.PropertyType -UsingNamespace $UsingNamespace;
            if ($IndexParameters.Length -gt 0) {
                $Code = "$PropertyTypeSyntax this[$(($IndexParameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace ) $($_.Name)" }) -join ', ')]"
            } else {
                $Code = "$PropertyTypeSyntax $($Property.Name)";
            }
        } else {
            $PropertyTypeSyntax = ConvertTo-TypeSyntax -Type $Property.PropertyType;
            if ($IndexParameters.Length -gt 0) {
                $Code = "$PropertyTypeSyntax this[$(($IndexParameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType ) $($_.Name)" }) -join ', ')]"
            } else {
                $Code = "$PropertyTypeSyntax $($Property.Name)";
            }
        }
        if ($Property.DeclaringType.IsInterface) {
            if ($Property.CanWrite) {
                if ($Property.CanRead) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code get; set; }")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { set; }")) -NoEnumerate;
                }
            } else {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { get; }")) -NoEnumerate;
            }
        } else {
            [System.Reflection.MethodInfo]$GetAccessor = $Property.GetGetMethod();
            [System.Reflection.MethodInfo]$SetAccessor = $Property.GetSetMethod();
            if ($null -ne $GetAccessor) {
                if ($GetAccessor.IsStatic) { $Code = "static $Code" }
                if ($GetAccessor.IsVirtual) {
                    $Code = "virtual $Code";
                } else {
                    if ($GetAccessor.IsAbstract) { $code = "abstract $Code" }
                }
                if ($null -eq $SetAccessor) {
                    switch ($GetAccessor) {
                        { $_.IsPublic } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsPrivate } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsAssembly } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsFamily } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        default {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; }")) -NoEnumerate;
                            break;
                        }
                    }
                } else {
                    switch ($GetAccessor) {
                        { $_.IsPublic } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; internal set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; protected set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; protected internal set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        { $_.IsPrivate } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        { $_.IsAssembly } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        { $_.IsFamily } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { protected get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; internal set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { protected get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        default {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { protected internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; internal set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; protected set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            } else {
                if ($null -ne $SetAccessor) {
                    if ($SetAccessor.IsStatic) { $Code = "static $Code" }
                    if ($SetAccessor.IsVirtual) {
                        $Code = "virtual $Code";
                    } else {
                        if ($SetAccessor.IsAbstract) { $code = "abstract $Code" }
                    }
                    switch ($SetAccessor) {
                        { $_.IsPublic } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsPrivate } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsAssembly } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsFamily } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        default {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { set; }")) -NoEnumerate;
                            break;
                        }
                    }
                } else {
                    if ($Property.CanWrite) {
                        if ($Property.CanRead) {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { get; set; }")) -NoEnumerate;
                        } else {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { set; }")) -NoEnumerate;
                        }
                    } else {
                        Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { get; }")) -NoEnumerate;
                    }
                }
            }
        }
    }
}

Function ConvertTo-EventDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.EventInfo]$Event,

        [string[]]$UsingNamespace
    )

    Process {
        $Event.EventHandlerType;
        [string]$Code = $null;
        $Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = "event $(ConvertTo-TypeSyntax -Type $Event.EventHandlerType -UsingNamespace $UsingNamespace) $($Event.Name)";
        } else {
            $Code = "event $(ConvertTo-TypeSyntax -Type $Event.EventHandlerType) $($Event.Name)";
        }
        if ($Property.DeclaringType.IsInterface) {
            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code;")) -NoEnumerate;
        } else {
            if ($Event.RaiseMethod.IsStatic) { $Code = "static $Code" }
            if ($Event.RaiseMethod.IsVirtual) {
                $Code = "virtual $Code";
            } else {
                if ($GetAccessor.IsAbstract) { $code = "abstract $Code" }
            }
            switch ($Event.RaiseMethod) {
                { $_.IsPublic } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code")) -NoEnumerate;
                    break;
                }
                { $_.IsPrivate } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code")) -NoEnumerate;
                    break;
                }
                { $_.IsAssembly } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code")) -NoEnumerate;
                    break;
                }
                { $_.IsFamily } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code")) -NoEnumerate;
                    break;
                }
                default {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code")) -NoEnumerate;
                    break;
                }
            }
        }
    }
}

Function ConvertTo-FieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.FieldInfo]$Field,

        [string[]]$UsingNamespace
    )

    Process {
        [string]$Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = "$(ConvertTo-TypeSyntax -Type $Property.FieldType -UsingNamespace $UsingNamespace)) $($Field.Name)";
        } else {
            $Code = "$(ConvertTo-TypeSyntax -Type $Property.FieldType)) $($Field.Name)";
        }
        if ($Field.IsLiteral) {
            if ($Field.IsStatic) { $Code = "const $Code" }
        } else {
            if ($Field.IsInitOnly) { $Code = "readonly $Code" }
            if ($Field.IsStatic) { $Code = "static $Code" }
        }
        switch ($Field) {
            { $_.IsPublic } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code;")) -NoEnumerate;
                break;
            }
            { $_.IsPrivate } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code;")) -NoEnumerate;
                break;
            }
            { $_.IsAssembly } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code;")) -NoEnumerate;
                break;
            }
            { $_.IsFamily } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code;")) -NoEnumerate;
                break;
            }
            default {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code;")) -NoEnumerate;
                break;
            }
        }
    }
}

Function ConvertTo-MethodDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.MethodInfo]$Method,

        [string[]]$UsingNamespace
    )

    Process {
        $Code = $Method.Name;
        [System.Reflection.ParameterInfo[]]$Parameters = $Method.GetParameters();
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = "$(ConvertTo-TypeSyntax -Type $Method.ReturnType -UsingNamespace $UsingNamespace) $Code";
            if ($Method.IsConstructedGenericMethod) {
                $Code = "$($Code.Substring(0, $Code.LastIndexOf('`')))<$((($Method.GetGenericArguments() | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace) | ForEach-Object { $_.ToString() }) -join ',')>";
            } else {
                if ($Method.IsGenericMethodDefinition) {
                    $Code = "$($Code.Substring(0, $Code.LastIndexOf('`')))<$(($Method.GetGenericArguments() | ForEach-Object { $_.Name }) -join ',')>";
                }
            }
            if ($Parameters.Length -eq 0) {
                $Code += "()";
            } else {
                $Code = "$Code($(($Parameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace ) $($_.Name)" }) -join ', '))";
            }
        } else {
            $Code = "$(ConvertTo-TypeSyntax -Type $Method.ReturnType) $Code";
            if ($Method.IsConstructedGenericMethod) {
                $i = $Code.LastIndexOf('`');
                if ($i -gt 0) { $Code = $Code.Substring(0, $i) }
                $Code = "$Code<$((($Method.GetGenericArguments() | ConvertTo-TypeSyntax) | ForEach-Object { $_.ToString() }) -join ',')>";
            } else {
                if ($Method.IsGenericMethodDefinition) {
                    $i = $Code.LastIndexOf('`');
                    if ($i -gt 0) { $Code = $Code.Substring(0, $i) }
                    $Code = "$Code<$(($Method.GetGenericArguments() | ForEach-Object { $_.Name }) -join ',')>";
                }
            }
            if ($Parameters.Length -eq 0) {
                $Code += "()";
            } else {
                $Code = "$Code($(($Parameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType ) $($_.Name)" }) -join ', '))";
            }
        }
        if ($Method.DeclaringType.IsInterface) {
            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code;")) -NoEnumerate;
        } else {
            if ($SetAccessor.IsStatic) { $Code = "static $Code" }
            switch ($Method) {
                { $_.IsPublic } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code;")) -NoEnumerate;
                    break;
                }
                { $_.IsPrivate } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code;")) -NoEnumerate;
                    break;
                }
                { $_.IsAssembly } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code;")) -NoEnumerate;
                    break;
                }
                { $_.IsFamily } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code;")) -NoEnumerate;
                    break;
                }
                default {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code;")) -NoEnumerate;
                    break;
                }
            }
        }
    }
}

Function ConvertTo-ConstructorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.ConstructorInfo]$Constructor,

        [string[]]$UsingNamespace
    )

    Process {
        $Name = $Constructor.DeclaringType.Name;
        if ($Constructor.DeclaringType.IsGenericType) { $Name = $Name.Substring(0, $Name.LastIndexOf('`')) }
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] $Result = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ConstructorDeclaration($Name);
        switch ($Constructor) {
            { $_.IsPublic } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('public ')));
                break;
            }
            { $_.IsPrivate } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('private ')));
                break;
            }
            { $_.IsAssembly } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('internal ')));
                break;
            }
            { $_.IsFamily } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('protected ')));
                break;
            }
            default {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('protected '), [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('internal ')));
                break;
            }
        }

        [System.Reflection.ParameterInfo[]]$Parameters = $Constructor.GetParameters();
        if ($Parameters.Length -gt 0) {
            if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                Write-Output -InputObject $Result.WithParameterList([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseParameterList("($(($Parameters | ForEach-Object {
                    if ($_.IsOut) {
                        "out $(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name)" | Write-Output;
                    } else {
                        if ($_.IsRetval) {
                            "ref $(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name)" | Write-Output;
                        } else {
                            if ($_.IsOptional) {
                                "$c$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name) = " | Write-Output;
                                if ($null -eq $_.DefaultValue) {
                                    "($c)default" | Write-Output;
                                } else {
                                    if ($_.DefaultValue -is [string] -or $_.DefaultValue -is [char] -or $_.DefaultValue -is [double] -or $_.DefaultValue -is [float] -or $_.DefaultValue -is [decimal]) {
                                        "$c$([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::Literal($_.DefaultValue))" | Write-Output;
                                    } else {
                                        if ($_.DefaultValue -is [type]) {
                                            "$($c)typeof($(ConvertTo-TypeSyntax -Type $_.DefaultValue -UsingNamespace $UsingNamespace))" | Write-Output;
                                        } else {
                                            "$c$($_.DefaultValue)" | Write-Output;
                                        }
                                    }
                                }
                            } else {
                                "$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name)" | Write-Output;
                            }
                        }
                    }
                }) -join ', '))")) -NoEnumerate;
            } else {
                Write-Output -InputObject $Result.WithParameterList([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseParameterList("($(($Parameters | ForEach-Object {
                    if ($_.IsOut) {
                        "out $(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name)" | Write-Output;
                    } else {
                        if ($_.IsRetval) {
                            "ref $(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name)" | Write-Output;
                        } else {
                            if ($_.IsOptional) {
                                "$c$(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name) = " | Write-Output;
                                if ($null -eq $_.DefaultValue) {
                                    "($c)default" | Write-Output;
                                } else {
                                    if ($_.DefaultValue -is [string] -or $_.DefaultValue -is [char] -or $_.DefaultValue -is [double] -or $_.DefaultValue -is [float] -or $_.DefaultValue -is [decimal]) {
                                        "$c$([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::Literal($_.DefaultValue))" | Write-Output;
                                    } else {
                                        if ($_.DefaultValue -is [type]) {
                                            "$($c)typeof($(ConvertTo-TypeSyntax -Type $_.DefaultValue))" | Write-Output;
                                        } else {
                                            "$c$($_.DefaultValue)" | Write-Output;
                                        }
                                    }
                                }
                            } else {
                                "$(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name)" | Write-Output;
                            }
                        }
                    }
                }) -join ', '))")) -NoEnumerate;
            }
        } else {
            Write-Output -InputObject $Result -NoEnumerate;
        }
    }
}

#endregion

#region Miscellaneous Functions

Function Get-ErrorDetails {
    [CmdletBinding(DefaultParameterSetName = 'Any')]
    [OutputType([System.Xml.Linq.XDocument])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Any')]
        [object]$ErrorObject,

        [Parameter(Mandatory = $true, ParameterSetName = 'ErrorRecord')]
        [System.Management.Automation.ErrorRecord]$ErrorRecord,

        [Parameter(Mandatory = $true, ParameterSetName = 'Exception')]
        [System.Exception]$Exception,

        [Parameter(ParameterSetName = 'Exception')]
        [switch]$NoRecurse
    )

    switch ($PSCmdlet.ParameterSetName) {
        'ErrorRecord' {
            $ErrorRecord.InvocationInfo.PositionMessage;
            if ($null -ne $ErrorRecord.ErrorDetails) {
                if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.ErrorDetails.Message)) {
                    "  Message: $($ErrorRecord.ErrorDetails.Message)" | Write-Output;
                }
                if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.ErrorDetails.RecommendedAction)) {
                    "  RecommendedAction: $($ErrorRecord.ErrorDetails.RecommendedAction)" | Write-Output;
                }
            }
            "  Category: $($ErrorRecord.CategoryInfo.Category)" | Write-Output;
            "    [Message]: $($ErrorRecord.CategoryInfo.GetMessage())" | Write-Output;
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.Activity)) {
                "    Activity: $($ErrorRecord.CategoryInfo.Activity)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.Reason)) {
                "    Reason: $($ErrorRecord.CategoryInfo.Reason)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.TargetName)) {
                "    TargetName: $($ErrorRecord.CategoryInfo.TargetName)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.TargetType)) {
                "    TargetType: $($ErrorRecord.CategoryInfo.TargetType)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.FullyQualifiedErrorId)) {
                "    ErrorId: $($ErrorRecord.FullyQualifiedErrorId)" | Write-Output;
            }

            $InnerErrorRecord = $ErrorRecord.Exception.ErrorRecord;
            if ($null -eq $InnerErrorRecord) {
                (Get-ErrorDetails -Exception $ErrorRecord.Exception) | ForEach-Object { "  $_" }
            } else {
                if ($null -eq $ErrorRecord.Exception.InnerException -or -not [object]::ReferenceEquals($InnerErrorRecord.Exception, $ErrorRecord.Exception.InnerException)) {
                    (Get-ErrorDetails -Exception $ErrorRecord.Exception) | ForEach-Object { "  $_" }
                } else {
                    (Get-ErrorDetails -Exception $ErrorRecord.Exception -NoRecurse) | ForEach-Object { "  $_" }
                }
                (Get-ErrorDetails -ErrorRecord $InnerErrorRecord) | ForEach-Object { "  $_" }
            }
            break;
        }
        'Exception' {
            $Exception.GetType().FullName | Write-Output;
            if (-not [string]::IsNullOrWhiteSpace($Exception.Message)) {
                "  Message: $($Exception.Message)" | Write-Output;
            }
            if (-not $NoRecurse.IsPresent) {
                if ($Exception -is [System.AggregateException]) {
                    $Exception.InnerExceptions | ForEach-Object {
                        "  InnerException:" | Write-Output;
                        (Get-ErrorDetails -Exception $_) | ForEach-Object { "    $_" }
                    }
                } else {
                    if ($null -ne $Exception.InnerException) {
                        "  InnerException:" | Write-Output;
                        (Get-ErrorDetails -Exception $Exception.InnerException) | ForEach-Object { "    $_" }
                    }
                }
            }
            foreach ($P in @($Exception | Get-Member -MemberType Properties)) {
                switch ($P.Name) {
                    'Message' { break; }
                    'InnerExceptions' { break; }
                    'InnerException' { break; }
                    'StackTrace' { break; }
                    'Data' { break; }
                    default {
                        $Text = ('' + $Exception.$($P.Name)).Trim();
                        if ($Text.Length -gt 0) {
                            $Lines = @(('' + $Exception.$($P.Name)) -split '\r\n?|\n');
                            "  $($P.Name): $($Lines[0])" | Write-Output;
                            ($Lines | Select-Object -Skip 1) | ForEach-Object { "  $_" } | Write-Output;
                        }
                        break;
                    }
                }
            }
            break;
        }
        default {
            if ($ErrorObject -is [System.Management.Automation.ErrorRecord]) {
                Get-ErrorDetails -ErrorRecord $ErrorObject;
            } else {
                if ($ErrorObject -is [System.Exception]) {
                    Get-ErrorDetails -Exception $Exception;
                } else {
                    $ErrorObject.GetType().FullName | Write-Output;
                    "  $ErrorObject" | Write-Output;
                }
            }
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

Function Optimize-PathString {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string[]]$Path
    )

    Process {
        $Path | ForEach-Object {
            $Parent = $_ | Split-Path -Parent;
            if ([string]::IsNullOrEmpty($Parent)) {
                $_ | Write-Output;
            } else {
                $Leaf = $_ | Split-Path -Leaf;
                if ([string]::IsNullOrEmpty($Leaf)) {
                    ($Parent | Optimize-PathInfo) | Write-Output;
                } else {
                    ($Parent | Join-Path -ChildPath $Leaf) | Write-Output;
                }
            }
        }
    }
}

Function Test-IsNotEmptyString {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Value
    )

    Begin { $IsNotEmptyString = $false }
    Process {
        if (-not ($IsNotEmptyString -or [string]::IsNullOrWhiteSpace($Value))) {
            $IsNotEmptyString = $true;
        }
    }

    End { $IsNotEmptyString | Write-Output }
}

#endregion

class ModelDefinitionsTypeMapper : DevUtil.TypeMapper {
    hidden static [string]$PREFIX = 'md';
    hidden static [string]$XMLNS = 'http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd';
    hidden static [System.Xml.Linq.XNamespace]$NAMESPACE = [System.Xml.Linq.XNamespace]::Get([ModelDefinitionsTypeMapper]::XMLNS);
    ModelDefinitionsTypeMapper() : base([ModelDefinitionsTypeMapper]::PREFIX, [ModelDefinitionsTypeMapper]::XMLNS) { }
    static [System.Xml.Linq.XName] GetXName([string]$NCName) {
        if ([string]::IsNullOrEmpty($NCName)) { throw [System.ArgumentException]::new("Name cannot be empty", "NCName") }
        return [ModelDefinitionsTypeMapper]::NAMESPACE.GetName([System.Xml.XmlConvert]::VerifyNCName($NCName));
    }
    hidden static [Type] GetElementType([Type]$ConstructedGenericType)
    {
        $a = $ConstructedGenericType.GetGenericArguments();
        if ($a.Length -eq 1) {
            $i = [System.Collections.Generic.IEnumerable`1].MakeGenericType($a[0]);
            if ($i -ceq $ConstructedGenericType -or $ConstructedGenericType.GetInterfaces() -ccontains $i) { return $a[0] }
        }
        return $null;
    }
    hidden static [bool] HasElementType([Type]$ConstructedGenericType)
    {
        $a = $ConstructedGenericType.GetGenericArguments();
        $i = [System.Collections.Generic.IEnumerable`1].MakeGenericType($a[0]);
        return $a.Length -eq 1 -and ($i -ceq $ConstructedGenericType -or $ConstructedGenericType.GetInterfaces() -ccontains $i);
    }
    hidden static [string] GetNCName([Type]$Type) {
        if ($Type -eq [Guid]) { return "GuidType" }
        if ($Type.IsEnum) { return "$($Type.Name)Type" }
        if ($Type -eq [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) { return 'SyntaxNodeType' }
        if ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($Type)) { return "$($Type.Name -replace 'Syntax$', '')Type" }
        return $null;
    }
    [string] GetNCNameOrNull([Type]$Type) {
        if ($null -eq $Type -or $Type.IsPointer -or $Type.IsByRef) { return $null }
        if ($Type.IsConstructedGenericType) {
            $ElementType = [ModelDefinitionsTypeMapper]::GetElementType($Type);
            if ($null -ne $ElementType -and -not $ElementType.IsGenericType) {
                $ElementName = [ModelDefinitionsTypeMapper]::GetNCName($ElementType);
                if ($null -ne $ElementName) { return "$($ElementName)List" }
            }
        } else {
            if (-not $Type.IsGenericType) { return [ModelDefinitionsTypeMapper]::GetNCName($Type) }
        }
        return $null;
    }
    [bool] CanMapToXsdType([Type]$Type, [string]$NCName) {
        if ($null -eq $Type) { throw [System.ArgumentNullException]::new('Type') }
        if ([string]::IsNullOrEmpty($NCName)) { return $false }
        return $this.GetNCNameOrNull($Type) -ceq $NCName;
    }
    [bool] IsMappedType([Type]$Type) {
        if ($null -eq $Type -or $Type.IsPointer -or $Type.IsByRef) { return $false }
        if ($Type.IsConstructedGenericType) {
            if ($null -eq ($Type = [ModelDefinitionsTypeMapper]::GetElementType($Type))) { return $false }
        }
        return (-not $Type.IsGenericType) -and ($Type -eq [Guid] -or $Type.IsEnum -or [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($Type));
    }
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

Set-Variable -Name 'ModelDefinitionsNS' -Option Constant -Scope 'Script' -Value ([System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xs'));

Function New-ModelDefinitionDocument {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XDocument])]
    Param()
    Write-Output -InputObject ([System.Xml.Linq.XDocument]::new([DevUtil.XLinqHelper]::NewMdElement('ModelDefinitions'))) -NoEnumerate;
}

Function Get-ModelDefinitionSourceElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string[]]$Path,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$Document
    )

    Begin {
        $PathXName = [DevUtil.XLinqHelper]::GetName('Path');
        $SourceElements = @(
            $Document.Root.Elements([System.Xml.Linq.XName]::Get([DevUtil.XLinqHelper]::GetMdName('Sources'))) | ForEach-Object {
                $_.Elements([DevUtil.XLinqHelper]::GetMdName('Source')) | Write-Output;
            }
        ) | Group-Object -Property @{ Expression = { $_.Attribute($PathXName).Value } } -AsHashTable;
    }
    Process {
        if ($SourceElements.Count -gt 0) {
            $Path | ForEach-Object {
                $e = $SourceElements[$_];
                if ($null -ne $e) { Write-Output -InputObject $e -NoEnumerate }
            }
        }
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Import Functions

#region Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Import Functions

#region Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Import Functions

Function Import-AliasQualifiedNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax]$AliasQualifiedName
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('AliasQualifiedName', [DevUtil.XLinqHelper]::NewAttribute('Name', $AliasQualifiedName.Name.ToString()),
            [DevUtil.XLinqHelper]::NewAttribute('Alias', $AliasQualifiedName.Alias.ToString()));
        Set-NameSyntaxContents -Name $AliasQualifiedName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-QualifiedNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax]$QualifiedName
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('QualifiedName', [DevUtil.XLinqHelper]::NewAttribute('Name', $QualifiedName.Right.ToString()));
        (Import-NameSyntax -Name $QualifiedName.Left) | ForEach-Object { $Element.Add($_) }
        Set-NameSyntaxContents -Name $QualifiedName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax Import Functions

Function Import-GenericNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax]$GenericName
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('GenericName');
        if ($GenericName.IsUnboundGenericName) { $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('Unbound', $true))) }
        Set-SimpleNameSyntaxContents -SimpleName $GenericName -Element $Element;
        if ($null -ne $GenericName.TypeArgumentList -and $GenericName.TypeArgumentList.Arguments.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Arguments');
            Set-SyntaxNodeContents -SyntaxNode $GenericName.TypeArgumentList -Element $e;
            ($GenericName.TypeArgumentList.Arguments | Import-TypeSyntax) | ForEach-Object { $Element.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-IdentifierNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax]$IdentifierName
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('IdentifierName');
        Set-SimpleNameSyntaxContents -SimpleName $IdentifierName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-SimpleNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$SimpleName
    )

    Process {
        switch ($SimpleName) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax] } { (Import-GenericNameSyntax -GenericName $SimpleName) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax] } { (Import-IdentifierNameSyntax -IdentifierName $SimpleName) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownSimpleNameSyntax');
                Set-SimpleNameSyntaxContents -SimpleName $SimpleName -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-SimpleNameSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$SimpleName,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('Name', $SimpleName.Identifier.ValueText)));
    if ($IsUnknown.IsPresent) {
        Set-NameSyntaxContents -Name $SimpleName -Element $Element -IsUnknown;
    } else {
        Set-NameSyntaxContents -Name $SimpleName -Element $Element;
    }
}

#endregion

Function Import-NameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$Name
    )

    Process {
        switch ($Name) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax] } { (Import-AliasQualifiedNameSyntax -AliasQualifiedName $Name) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax] } { (Import-QualifiedNameSyntax -QualifiedName $Name) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax] } { (Import-SimpleNameSyntax -SimpleName $Name) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownNameSyntax');
                Set-NameSyntaxContents -Name $Name -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-NameSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$Name,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('Arity', $Name.Arity)));
    if ($IsUnknown.IsPresent) {
        Set-TypeSyntaxContents -Type $Name -Element $Element -IsUnknown;
    } else {
        Set-TypeSyntaxContents -Type $Name -Element $Element;
    }
}

#endregion

Function Import-RefTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax]$RefType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('RefType');
        if (-not $RefType.ReadOnlyKeyword.Span.IsEmpty) { $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('ReadOnly', $true))) }
        Set-TypeSyntaxContents -Type $RefType -Element $Element;
        $e = [DevUtil.XLinqHelper]::NewMdElement('Type');
        (Import-TypeSyntax -Type $RefType.Type) | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-PredefinedTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax]$PredefinedType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('PredefinedType',
            [DevUtil.XLinqHelper]::NewAttribute('Keyword', $PredefinedType.ToString()));
        Set-TypeSyntaxContents -Type $PredefinedType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ArrayTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax]$ArrayType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('ArrayType');
        Set-TypeSyntaxContents -Type $ArrayType -Element $Element;
        if ($null -ne $ArrayType.ElementType) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('ElementType');
            (Import-TypeSyntax -Type $ArrayType.ElementType) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($ArrayType.RankSpecifiers.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('RankSpecifiers');
            $ArrayType.RankSpecifiers | Import-ArrayRankSpecifierSyntax | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-PointerTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax]$PointerType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('PointerType');
        Set-TypeSyntaxContents -Type $PointerType -Element $Element;
        if ($null -ne $PointerType.ElementType) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('ElementType');
            (Import-TypeSyntax -Type $PointerType.ElementType) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-FunctionPointerTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax]$FunctionPointerType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('FunctionPointerType');
        Set-TypeSyntaxContents -Type $FunctionPointerType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken DelegateKeyword
            Microsoft.CodeAnalysis.SyntaxToken AsteriskToken
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax CallingConvention
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax ParameterList
        #>
    }
}

Function Import-NullableTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax]$NullableType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('NullableType');
        Set-TypeSyntaxContents -Type $NullableType -Element $Element;
        $e = [DevUtil.XLinqHelper]::NewMdElement('ElementType');
        (Import-TypeSyntax -Type $NullableType.ElementType) | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-TupleTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax]$TupleType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TupleType');
        Set-TypeSyntaxContents -Type $TupleType -Element $Element;
        $TupleType.Elements | Import-TupleElementSyntax | ForEach-Object { $Element.Add($_) }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-OmittedTypeArgumentSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax]$OmittedTypeArgument
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('OmittedTypeArgument');
        Set-TypeSyntaxContents -Type $OmittedTypeArgument -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OmittedTypeArgumentToken
        #>
    }
}

Function Import-TypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type
    )

    Process {
        switch ($Type) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax] } { (Import-NameSyntax -Name $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax] } { (Import-RefTypeSyntax -RefType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax] } { (Import-PredefinedTypeSyntax -PredefinedType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax] } { (Import-ArrayTypeSyntax -ArrayType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax] } { (Import-PointerTypeSyntax -PointerType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax] } { (Import-FunctionPointerTypeSyntax -FunctionPointerType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax] } { (Import-NullableTypeSyntax -NullableType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax] } { (Import-TupleTypeSyntax -TupleType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax] } { (Import-OmittedTypeArgumentSyntax -OmittedTypeArgument $Type) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownTypeSyntax');
                Set-TypeSyntaxContents -Type $Type -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-TypeSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($Type.IsVar) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'IsVar', $true) }
    if ($Type.IsUnmanaged) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'IsUnmanaged', $true) }
    if ($Type.IsNotNull) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'IsNotNull', $true) }
    if ($Type.IsNint) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'IsNint', $true) }
    if ($Type.IsNuint) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'IsNuint', $true) }
    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $Type -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $Type -Element $Element;
    }
}

#endregion

Function Import-LiteralExpressionSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax]$LiteralExpression
    )

    Process {
        Write-Output -InputObject ([DevUtil.XLinqHelper]::NewMdElement('LiteralExpression', $LiteralExpression.Token.ValueText)) -NoEnumerate;
    }
}

Function Import-DefaultExpressionSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax]$DefaultExpression
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('DefaultExpression');
        Set-SyntaxNodeContents -SyntaxNode $DefaultExpression -Element $Element;
        if ($null -ne $DefaultExpression.Type) {
            $Element.Add((Import-TypeSyntax -Type $DefaultExpression.Type));
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-TypeOfExpressionSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax]$TypeOfExpression
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TypeOfExpression');
        (Import-TypeSyntax -Type $TypeOfExpression.Type) | ForEach-Object { $Element.Add($_) }
        Set-SyntaxNodeContents -SyntaxNode $TypeOfExpression -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-CastExpressionSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax]$CastExpression
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('CastExpression');
        (Import-TypeSyntax -Type $TypeOfExpression.Type) | ForEach-Object { $Element.Add($_) }
        (Import-ExpressionSyntax -Expression $CastExpression.Expression) | ForEach-Object { $Element.Add($_) }
        Set-SyntaxNodeContents -SyntaxNode $CastExpression -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ExpressionSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax]$Expression
    )

    Process {
        switch ($Expression) {
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax] } { (Import-AnonymousFunctionExpressionSyntax -AnonymousFunctionExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StackAllocArrayCreationExpressionSyntax] } { (Import-StackAllocArrayCreationExpressionSyntax -StackAllocArrayCreationExpression $Expression) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax] } { (Import-TypeSyntax -Type $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax] } { (Import-ParenthesizedExpressionSyntax -ParenthesizedExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleExpressionSyntax] } { (Import-TupleExpressionSyntax -TupleExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PrefixUnaryExpressionSyntax] } { (Import-PrefixUnaryExpressionSyntax -PrefixUnaryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax] } { (Import-AwaitExpressionSyntax -AwaitExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PostfixUnaryExpressionSyntax] } { (Import-PostfixUnaryExpressionSyntax -PostfixUnaryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax] } { (Import-MemberAccessExpressionSyntax -MemberAccessExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax] } { (Import-ConditionalAccessExpressionSyntax -ConditionalAccessExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberBindingExpressionSyntax] } { (Import-MemberBindingExpressionSyntax -MemberBindingExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElementBindingExpressionSyntax] } { (Import-ElementBindingExpressionSyntax -ElementBindingExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RangeExpressionSyntax] } { (Import-RangeExpressionSyntax -RangeExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitElementAccessSyntax] } { (Import-ImplicitElementAccessSyntax -ImplicitElementAccess $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax] } { (Import-BinaryExpressionSyntax -BinaryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax] } { (Import-AssignmentExpressionSyntax -AssignmentExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalExpressionSyntax] } { (Import-ConditionalExpressionSyntax -ConditionalExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InstanceExpressionSyntax] } { (Import-InstanceExpressionSyntax -InstanceExpression $Expression) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax] } { (Import-LiteralExpressionSyntax -LiteralExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MakeRefExpressionSyntax] } { (Import-MakeRefExpressionSyntax -MakeRefExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeExpressionSyntax] } { (Import-RefTypeExpressionSyntax -RefTypeExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefValueExpressionSyntax] } { (Import-RefValueExpressionSyntax -RefValueExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CheckedExpressionSyntax] } { (Import-CheckedExpressionSyntax -CheckedExpression $Expression) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax] } { (Import-DefaultExpressionSyntax -DefaultExpression $Expression) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax] } { (Import-TypeOfExpressionSyntax -TypeOfExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SizeOfExpressionSyntax] } { (Import-SizeOfExpressionSyntax -SizeOfExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax] } { (Import-InvocationExpressionSyntax -InvocationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax] } { (Import-ElementAccessExpressionSyntax -ElementAccessExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationExpressionSyntax] } { (Import-DeclarationExpressionSyntax -DeclarationExpression $Expression) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax] } { (Import-CastExpressionSyntax -CastExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefExpressionSyntax] } { (Import-RefExpressionSyntax -RefExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InitializerExpressionSyntax] } { (Import-InitializerExpressionSyntax -InitializerExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseObjectCreationExpressionSyntax] } { (Import-BaseObjectCreationExpressionSyntax -BaseObjectCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WithExpressionSyntax] } { (Import-WithExpressionSyntax -WithExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax] } { (Import-AnonymousObjectCreationExpressionSyntax -AnonymousObjectCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax] } { (Import-ArrayCreationExpressionSyntax -ArrayCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitArrayCreationExpressionSyntax] } { (Import-ImplicitArrayCreationExpressionSyntax -ImplicitArrayCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitStackAllocArrayCreationExpressionSyntax] } { (Import-ImplicitStackAllocArrayCreationExpressionSyntax -ImplicitStackAllocArrayCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax] } { (Import-QueryExpressionSyntax -QueryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedArraySizeExpressionSyntax] } { (Import-OmittedArraySizeExpressionSyntax -OmittedArraySizeExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax] } { (Import-InterpolatedStringExpressionSyntax -InterpolatedStringExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IsPatternExpressionSyntax] } { (Import-IsPatternExpressionSyntax -IsPatternExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ThrowExpressionSyntax] } { (Import-ThrowExpressionSyntax -ThrowExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionSyntax] } { (Import-SwitchExpressionSyntax -SwitchExpression $Expression) | Write-Output; break; }
            default {
                $Type = (($Expression.GetType().Name -replace '`\d+$', '') -replace 'Syntax$', '') -replace 'Expression$', '';
                Write-Output -InputObject ([DevUtil.XLinqHelper]::NewMdElement('Expression', [DevUtil.XLinqHelper]::NewAttribute('Type', $Type),
                    [System.Xml.Linq.XCData]::new($Expression.ToString()))) -NoEnumerate;
                break;
            }
        }
    }
}

#endregion

#region Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode Import Functions

#region Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax Import Functions

Function Import-XmlElementSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementSyntax]$XmlElement,

        [Parameter(Mandatory = $true)]
        [System.Text.StringBuilder]$StringBuilder
    )

    Process {
        if ($null -eq $XmlElement.StartTag.Name.Prefix -or $XmlElement.StartTag.Name.Prefix.Span.IsEmpty) {
            $StringBuilder.Append('<').Append($XmlElement.StartTag.Name.LocalName) | Out-Null;
            if ($XmlElement.Attributes.Count -gt 0) {
                $XmlElement.Attributes | ForEach-Object {
                    $StringBuilder.Append(' ').Append($_.ToString()) | Out-Null;
                }
            }
            $StringBuilder.Append('>') | Out-Null;
            if ($XmlElement.Content.Count -gt 0) {
                $XmlElement.Content | Import-XmlNodeSyntax -StringBuilder $StringBuilder;
            }
            $StringBuilder.Append('</').Append($XmlElement.StartTag.Name.LocalName).Append('>') | Out-Null;
        }
    }
}

Function Import-XmlEmptyElementSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.XmlEmptyElementSyntax]$XmlEmptyElement,

        [Parameter(Mandatory = $true)]
        [System.Text.StringBuilder]$StringBuilder
    )

    Process {
        if ($null -eq $XmlElement.Name.Prefix -or $XmlElement.Name.Prefix.Span.IsEmpty) {
            $StringBuilder.Append('<').Append($XmlElement.Name.LocalName) | Out-Null;
            if ($XmlElement.Attributes.Count -gt 0) {
                $XmlElement.Attributes | ForEach-Object {
                    $StringBuilder.Append(' ').Append($_.ToString()) | Out-Null;
                }
            }
            $StringBuilder.Append($XmlElement.Name.LocalName).Append(' />') | Out-Null;
        }
    }
}

Function Import-XmlProcessingInstructionSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.XmlProcessingInstructionSyntax]$XmlProcessingInstruction,

        [Parameter(Mandatory = $true)]
        [System.Text.StringBuilder]$StringBuilder
    )

    Process {
        if ($null -eq $XmlProcessingInstruction.Name.Prefix -or $XmlProcessingInstruction.Name.Prefix.Span.IsEmpty) {
            $StringBuilder.Append($XmlProcessingInstruction.ToString()) | Out-Null;
        }
    }
}

Function Import-XmlNodeSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax]$XmlNode,

        [Parameter(Mandatory = $true)]
        [System.Text.StringBuilder]$StringBuilder
    )

    Process {
        switch ($StructuredTrivia) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementSyntax] } { (Import-XmlElementSyntax -XmlElement $XmlNode -StringBuilder $StringBuilder) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlEmptyElementSyntax] } { (Import-XmlEmptyElementSyntax -XmlEmptyElement $XmlNode -StringBuilder $StringBuilder) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlProcessingInstructionSyntax] } { (Import-XmlProcessingInstructionSyntax -XmlProcessingInstruction $XmlNode -StringBuilder $StringBuilder) | Write-Output; break; }
            default { $StringBuilder.Append($XmlNode.ToString()) | Out-Null; break; }
        }
    }
}

#endregion

#region Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax Import Functions

Function Import-DocumentationCommentTriviaSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax]$DocumentationCommentTrivia
    )

    Process {
        if ($DocumentationCommentTrivia.Content.Count -gt 0) {
            $Element = [DevUtil.XLinqHelper]::NewMdElement('DocumentationComment');
            $StringBuilder = [System.Text.StringBuilder]::new();
            $DocumentationCommentTrivia.Content | ForEach-Object { $StringBuilder.Append($_.ToString()) } | Out-Null;
            $Text = $StringBuilder.ToString().Trim() -replace '(\r\n?|\n)[ \t]*///[ \t]?', '$1';
            [System.Xml.Linq.XNode[]]$Nodes = @();
            try {
                $Nodes = @([System.Xml.Linq.XElement]::Parse("<doc>$Text</doc>").Nodes());
            } catch {
                $Nodes = @([System.Xml.Linq.XCData]::new($Text));
            }
            $Nodes | ForEach-Object { $Element.Add($_) }
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-StructuredTriviaSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax]$StructuredTrivia
    )

    Process {
        switch ($StructuredTrivia) {
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DirectiveTriviaSyntax] } { (Import-DirectiveTriviaSyntax -Argument $DirectiveTrivia) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SkippedTokensTriviaSyntax] } { (Import-SkippedTokensTriviaSyntax -Argument $SkippedTokensTrivia) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax] } { (Import-DocumentationCommentTriviaSyntax -DocumentationCommentTrivia $StructuredTrivia) | Write-Output; break; }
            default {
                $XElement = [DevUtil.XLinqHelper]::NewMdElement('UnknownStructuredTriviaSyntax');
                Set-SyntaxNodeContents -SyntaxNode $StructuredTrivia -Element $XElement -IsUnknown;
                Write-Output -InputObject $XElement -NoEnumerate;
                break;
            }
        }
    }
}

#endregion

Function Import-AttributeListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax]$AttributeList
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('AttributeList');
        Set-SyntaxNodeContents -SyntaxNode $AttributeList -Element $Element;
        if ($null -ne $AttributeList.Target) {
            (Import-AttributeTargetSpecifierSyntax -AttributeTargetSpecifier $AttributeList.Target) | ForEach-Object { $Element.Add($_) }
        }
        if ($AttributeList.Attributes.Count -gt 0) {
            $AttributeList.Attributes | Import-AttributeSyntax | ForEach-Object { $Element.Add($_) }
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Import Functions

#region Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax

Function Import-FieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]$Field
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Field');
        Set-BaseFieldDeclarationSyntaxContents -BaseField $Field -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-EventFieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]$EventField
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('EventField');
        Set-BaseFieldDeclarationSyntaxContents -BaseField $EventField -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseFieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$BaseField
    )

    Process {
        switch ($BaseField) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax] } { (Import-FieldDeclarationSyntax -Field $BaseField) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax] } { (Import-EventFieldDeclarationSyntax -EventField $BaseField) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseFieldDeclarationSyntax');
                Set-BaseFieldDeclarationSyntaxContents -BaseField $BaseField -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseFieldDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$BaseField,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    $Element.Add([DevUtil.XLinqHelper]::NewAttribute('Name', $BaseField.Declaration.Variables[0].Identifier.ToString()));
    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseField -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseField -Element $Element;
    }
    ($BaseField.Declaration.Type | Import-TypeSyntax) | ForEach-Object { $Element.Add($_) }
}

#endregion

#region Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax

Function Import-ConstructorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]$Constructor
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Constructor',
            ([DevUtil.XLinqHelper]::NewAttribute('Name', $Constructor.Identifier.ValueText)));
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Constructor -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ConversionOperatorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]$ConversionOperator
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('ConversionOperator', [DevUtil.XLinqHelper]::NewAttribute('ImplicitOrExplicit', $ConversionOperator.ImplicitOrExplicitKeyword.ToString()));
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $ConversionOperator -Element $Element;
        if ($null -ne $ConversionOperator.Type) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Type');
            (Import-TypeSyntax -Type $ConversionOperator.Type) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($null -ne $Operator.ExplicitInterfaceSpecifier) {
            $Element.Add((Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $Operator.ExplicitInterfaceSpecifier));
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-DestructorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]$Destructor
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Destructor',
            ([DevUtil.XLinqHelper]::NewAttribute('Name', $Destructor.Identifier.ValueText)));
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Destructor -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-MethodDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]$Method
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Method', [DevUtil.XLinqHelper]::NewAttribute('Name', $Method.Identifier.ValueText),
            [DevUtil.XLinqHelper]::NewAttribute('Arity', $Method.Arity));
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Method -Element $Element;
        if ($null -ne $Method.ReturnType) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('ReturnType');
            (Import-TypeSyntax -Type $Method.ReturnType) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($null -ne $Method.TypeParameterList) {
            (Import-TypeParameterListSyntax -TypeParameterList $Method.TypeParameterList) | ForEach-Object { $Element.Add($_) }
        }
        if ($Method.ConstraintClauses.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Constraints');
            ($Method.ConstraintClauses | Import-TypeParameterConstraintClauseSyntax) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($null -ne $Operator.ExplicitInterfaceSpecifier) {
            $Element.Add((Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $Operator.ExplicitInterfaceSpecifier));
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-OperatorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]$Operator
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Operator');
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Operator -Element $Element;
        if ($null -ne $Operator.ReturnType) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('ReturnType');
            (Import-TypeSyntax -Type $Operator.ReturnType) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($null -ne $Operator.ExplicitInterfaceSpecifier) {
            (Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $Operator.ExplicitInterfaceSpecifier) | ForEach-Object { $Element.Add($_) }
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseMethodDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$BaseMethod
    )

    Process {
        switch ($BaseMethod) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] } { (Import-ConstructorDeclarationSyntax -Constructor $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } { (Import-ConversionOperatorDeclarationSyntax -ConversionOperator $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax] } { (Import-DestructorDeclarationSyntax -Destructor $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } { (Import-MethodDeclarationSyntax -Method $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } { (Import-OperatorDeclarationSyntax -Operator $BaseMethod) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseMethodDeclarationSyntax');
                Set-BaseMethodDeclarationSyntaxContents -BaseMethod $BaseMethod -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseMethodDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$BaseMethod,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseMethod -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseMethod -Element $Element;
    }
    if ($null -ne $BaseMethod.ParameterList -and $BaseMethod.ParameterList.Parameters.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Parameters');
        ($BaseMethod.ParameterList.Parameters | Import-ParameterSyntax) | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax Body
        Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody
        Microsoft.CodeAnalysis.SyntaxToken SemicolonToken
    #>
}

#endregion

#region Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax Import Methods

Function Import-EventDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]$Event
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('EventProperty',
            ([DevUtil.XLinqHelper]::NewAttribute('Name', $Event.Identifier.ValueText)));
        Set-BasePropertyDeclarationSyntaxContents -BaseProperty $Event -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-IndexerDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]$Indexer
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Indexer');
        Set-BasePropertyDeclarationSyntaxContents -BaseProperty $Indexer -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Semicolon
            Microsoft.CodeAnalysis.SyntaxToken ThisKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax ParameterList
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenBracketToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax] Parameters
            Microsoft.CodeAnalysis.SyntaxToken CloseBracketToken
        #>
    }
}

Function Import-PropertyDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]$Property
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Property',
            ([DevUtil.XLinqHelper]::NewAttribute('Name', $Property.Identifier.ValueText)));
        Set-BasePropertyDeclarationSyntaxContents -BaseProperty $Property -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Semicolon
            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody
            Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax Initializer
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken
        #>
    }
}

Function Import-BasePropertyDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$BaseProperty
    )

    Process {
        switch ($BaseProperty) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] } { (Import-EventDeclarationSyntax -Event $BaseProperty) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } { (Import-IndexerDeclarationSyntax -Indexer $BaseProperty) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } { (Import-PropertyDeclarationSyntax -Property $BaseProperty) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBasePropertyDeclarationSyntax');
                Set-BasePropertyDeclarationSyntaxContents -BaseProperty $BaseProperty -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BasePropertyDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$BaseProperty,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseProperty -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseProperty -Element $Element;
    }
    if ($null -ne $BaseProperty.Type) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Type');
        (Import-TypeSyntax -Type $BaseProperty.Type) | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }
    if ($BaseProperty.AccessorList.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Accessors');
        $BaseProperty.AccessorList | ForEach-Object { $e.Add([DevUtil.XLinqHelper]::NewMdElement('Accessor', $_.Value)) }
        $Element.Add($e);
    }
    if ($null -ne $BaseProperty.ExplicitInterfaceSpecifier) {
        $Element.Add((Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $BaseProperty.ExplicitInterfaceSpecifier));
    }
}

#endregion

Function Import-DelegateDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Delegate
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Delegate', [DevUtil.XLinqHelper]::NewAttribute('Name', $Delegate.Identifier.ValueText),
            [DevUtil.XLinqHelper]::NewAttribute('Arity', $Delegate.Arity));
        Set-MemberDeclarationSyntaxContents -Member $Delegate -Element $Element;
        if ($null -ne $Delegate.ReturnType) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('ReturnType');
            (Import-TypeSyntax -Type $Delegate.ReturnType) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($null -ne $Delegate.TypeParameterList) {
            (Import-TypeParameterListSyntax -TypeParameterList $Delegate.TypeParameterList) | ForEach-Object { $Element.Add($_) }
        }
        if ($Delegate.ConstraintClauses.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Constraints');
            ($Delegate.ConstraintClauses | Import-TypeParameterConstraintClauseSyntax) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        if ($null -ne $Delegate.ParameterList -and $Delegate.ParameterList.Parameters.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Parameters');
            ($Delegate.ParameterList.Parameters | Import-ParameterSyntax) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-EnumMemberDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax]$EnumMember
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Member', [DevUtil.XLinqHelper]::NewAttribute('Name', $EnumMember.Identifier.ValueText));
        Set-MemberDeclarationSyntaxContents -Member $EnumMember -Element $Element;
        if ($null -ne $EnumMember.EqualsValue) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Value');
            ($EnumMember.EqualsValue.Value | Import-ExpressionSyntax) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-GlobalStatementSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax]$GlobalStatement
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('GlobalStatement', [DevUtil.XLinqHelper]::NewMdElement('Statement'), $GlobalStatement.Statement.ToString());
        Set-MemberDeclarationSyntaxContents -Member $GlobalStatement -Element $Element;

        Write-Output -InputObject $Element -NoEnumerate;
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax Import Functions

Function Import-NamespaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$Namespace
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Namespace', [DevUtil.XLinqHelper]::NewAttribute('Name', $Namespace.Name.ToString()));
        Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespace $Namespace -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-FileScopedNamespaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax]$FileScopedNamespace
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('FileScopedNamespace', [DevUtil.XLinqHelper]::NewAttribute('Name', $FileScopedNamespace.Name.ToString()));
        Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespace $FileScopedNamespace -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseNamespaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$BaseNamespace
    )

    Process {
        switch ($BaseNamespace) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } { (Import-NamespaceDeclarationSyntax -Namespace $BaseNamespace) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax] } { (Import-FileScopedNamespaceDeclarationSyntax -FileScopedNamespace $BaseNamespace) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseNamespaceDeclarationSyntax');
                Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespace $BaseNamespace -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseNamespaceDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$BaseNamespace,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseNamespace -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseNamespace -Element $Element;
    }
    $BaseNamespace.Externs | Import-ExternAliasDirectiveSyntax | ForEach-Object { $Element.Add($_) }
    $BaseNamespace.Usings | Import-UsingDirectiveSyntax | ForEach-Object { $Element.Add($_) }
    if ($BaseNamespace.Members.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Members');
        $BaseNamespace.Members | Import-MemberDeclarationSyntax | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }
}

#endregion

#region Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax Import Functions

#region Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax Import Functions

Function Import-RecordDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]$Record
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Record');
        if (-not $Record.ClassOrStructKeyword.Span.IsEmpty) {
            $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('ClassOrStruct'), $Record.ClassOrStructKeyword.ToString()));
        }
        Set-TypeDeclarationSyntaxContents -Type $Record -Element $Element;
        if ($null -ne $Record.ParameterList -and $Record.ParameterList.Parameters.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Parameters');
            ($Record.ParameterList.Parameters | Import-ParameterSyntax) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ClassDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$Class
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Class');
        Set-TypeDeclarationSyntaxContents -Type $Class -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-StructDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]$Struct
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Struct');
        Set-TypeDeclarationSyntaxContents -Type $Struct -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-InterfaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$Interface
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Interface');
        Set-TypeDeclarationSyntaxContents -Type $Interface -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-TypeDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]$Type
    )

    Process {
        switch ($Type) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] } { (Import-RecordDeclarationSyntax -Record $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] } { (Import-ClassDeclarationSyntax -Class $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] } { (Import-StructDeclarationSyntax -Struct $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] } { (Import-InterfaceDeclarationSyntax -Interface $Type) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownTypeDeclarationSyntax');
                Set-TypeDeclarationSyntaxContents -Type $Type -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-TypeDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('Arity', $Type.Arity)));
    if ($IsUnknown.IsPresent) {
        Set-BaseTypeDeclarationSyntaxContents -BaseType $Type -Element $Element -IsUnknown;
    } else {
        Set-BaseTypeDeclarationSyntaxContents -BaseType $Type -Element $Element;
    }
    if ($null -ne $Type.TypeParameterList) {
        (Import-TypeParameterListSyntax -TypeParameterList $Type.TypeParameterList) | ForEach-Object { $Element.Add($_) }
    }
    if ($Type.ConstraintClauses.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Constraints');
        ($Type.ConstraintClauses | Import-TypeParameterConstraintClauseSyntax) | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }
    if ($Type.Members.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Members');
        $Type.Members | Import-MemberDeclarationSyntax | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }
    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SyntaxToken Keyword
    #>
}

#endregion

Function Import-EnumDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Enum
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Enum');
        Set-BaseTypeDeclarationSyntaxContents -BaseType $Enum -Element $Element;
        $Enum.Members | Import-EnumMemberDeclarationSyntax | ForEach-Object { $Element.Add($_) }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseTypeDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]$BaseType
    )

    Process {
        switch ($BaseType) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax] } { (Import-TypeDeclarationSyntax -Type $BaseType) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] } { (Import-EnumDeclarationSyntax -Enum $BaseType) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseTypeDeclarationSyntax');
                Set-BaseTypeDeclarationSyntaxContents -BaseType $BaseType -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseTypeDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]$BaseType,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    $Element.Add(([DevUtil.XLinqHelper]::NewAttribute('Name', $BaseType.Identifier.ValueText)));
    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseType -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseType -Element $Element;
    }
    if ($null -ne $BaseType.BaseList) {
        (Import-BaseListSyntax -BaseList $BaseType.BaseList) | ForEach-Object { $Element.Add($_) }
    }
}

#endregion

Function Import-IncompleteMemberSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax]$IncompleteMember
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('IncompleteMember');
        Set-MemberDeclarationSyntaxContents -Member $IncompleteMember -Element $Element;
        if ($null -ne $IncompleteMember.Type) {
            $Element.Add((Import-TypeSyntax -Type $IncompleteMember.Type));
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-MemberDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Member
    )

    Process {
        switch ($Member) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax] } { (Import-BaseFieldDeclarationSyntax -BaseField $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } { (Import-BaseMethodDeclarationSyntax -BaseMethod $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } { (Import-BasePropertyDeclarationSyntax -BaseProperty $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax] } { (Import-DelegateDeclarationSyntax -Delegate $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax] } { (Import-EnumMemberDeclarationSyntax -EnumMember $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax] } { (Import-GlobalStatementSyntax -GlobalStatement $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } { (Import-BaseNamespaceDeclarationSyntax -BaseNamespace $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] } { (Import-BaseTypeDeclarationSyntax -BaseType $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax] } { (Import-IncompleteMemberSyntax -IncompleteMember $Member) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownMemberDeclarationSyntax');
                Set-MemberDeclarationSyntaxContents -Member $Member -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-MemberDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Member,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $Member -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $Member -Element $Element;
    }

    if ($Member.Modifiers.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Modifiers');
        $Member.Modifiers | ForEach-Object { $e.Add([DevUtil.XLinqHelper]::NewMdElement('Modifier', $_.Value)) }
        $Element.Add($e);
    }

    if ($Member.AttributeLists.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('AttributeLists');
        $Member.AttributeLists | Import-AttributeListSyntax | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }
}

#endregion

Function Import-ExplicitInterfaceSpecifierSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]$ExplicitInterfaceSpecifier
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Explicit');
        Set-SyntaxNodeContents -SyntaxNode $ExplicitInterfaceSpecifier -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-UsingDirectiveSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax]$UsingDirective
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Using', [DevUtil.XLinqHelper]::NewAttribute('Name', $UsingDirective.Name.ToString()));
        if ($null -ne $UsingDirective.Alias) {
            [DevUtil.XLinqHelper]::SetAttribute($Element, 'Alias', $UsingDirective.Alias.Name.ToString());
        }
        if (-not $UsingDirective.StaticKeyword.Span.IsEmpty) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'Static', $true) }
        if (-not $UsingDirective.GlobalKeyword.Span.IsEmpty) { [DevUtil.XLinqHelper]::SetAttribute($Element, 'Global', $true) }
        Set-SyntaxNodeContents -SyntaxNode $UsingDirective -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ExternAliasDirectiveSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax]$ExternAliasDirective
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Extern', [DevUtil.XLinqHelper]::NewAttribute('Identifier', $ExternAliasDirective.Identifier.ValueText));
        Set-SyntaxNodeContents -SyntaxNode $ExternAliasDirective -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-AttributeTargetSpecifierSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax]$AttributeTargetSpecifier
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Target', [DevUtil.XLinqHelper]::NewAttribute('Name', $AttributeTargetSpecifier.Identifier.ValueText));
        Set-SyntaxNodeContents -SyntaxNode $AttributeTargetSpecifier -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-AttributeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax]$Attribute
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Attribute', [DevUtil.XLinqHelper]::NewAttribute('Name', $Attribute.Name.ToString()));
        Set-SyntaxNodeContents -SyntaxNode $Attribute -Element $Element;
        if ($null -ne $Attribute.ArgumentList) {
            ($Attribute.ArgumentList | Import-AttributeArgumentListSyntax) | ForEach-Object { $Element.Add($_) }
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-AttributeArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax]$AttributeArgumentList
    )

    Process {
        if ($AttributeArgumentList.Arguments.Count -gt 0) {
            $Element = [DevUtil.XLinqHelper]::NewMdElement('Arguments');
            Set-SyntaxNodeContents -SyntaxNode $AttributeArgumentList -Element $Element;
            ($AttributeArgumentList.Arguments | Import-AttributeArgumentSyntax) | ForEach-Object { $Element.Add($_) }
            Write-Output -InputObject $Element -NoEnumerate;
        }
    }
}

Function Import-AttributeArgumentSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax]$AttributeArgument
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Argument');
        if ($null -ne $AttributeArgument.NameEquals) {
            [DevUtil.XLinqHelper]::SetAttribute($Element, 'Name', $AttributeArgument.NameEquals.Name.ToString());
        }
        Set-SyntaxNodeContents -SyntaxNode $AttributeArgument -Element $Element;
        (Import-ExpressionSyntax -Expression $AttributeArgument.Expression) | ForEach-Object { $Element.Add($_) }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax]$BaseList
    )

    Process {
        if ($BaseList.Types.Count -gt 0) {
            $Element = [DevUtil.XLinqHelper]::NewMdElement('BaseTypes');
            $BaseList.Types | Import-BaseTypeSyntax | ForEach-Object { $Element.Add($_) }
            Write-Output -InputObject $Element -NoEnumerate;
        }
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Import Functions

Function Import-SimpleBaseTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax]$SimpleBaseType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('SimpleBaseType');
        Set-BaseTypeSyntaxContents -BaseType $SimpleBaseType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-PrimaryConstructorBaseTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax]$PrimaryConstructorBaseType
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('PrimaryConstructorBaseType');
        Set-BaseTypeSyntaxContents -BaseType $PrimaryConstructorBaseType -Element $Element;
        if ($null -ne $PrimaryConstructorBaseType.ArgumentList) {
            (Import-ArgumentListSyntax -ArgumentList $PrimaryConstructorBaseType.ArgumentList) | ForEach-Object { $Element.Add($_) }
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax]$BaseType
    )

    Process {
        switch ($BaseType) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax] } { (Import-SimpleBaseTypeSyntax -SimpleBaseType $BaseType) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax] } { (Import-PrimaryConstructorBaseTypeSyntax -PrimaryConstructorBaseType $BaseType) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseTypeSyntax');
                Set-BaseTypeSyntaxContents -BaseType $BaseType -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseTypeSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax]$BaseType,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseType -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseType -Element $Element;
    }
    $Element.Add((Import-TypeSyntax -Type $BaseType.Type));
}

#endregion

#region Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax Import Functions

Function Import-ArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax]$ArgumentList
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('ArgumentList');
        Set-BaseArgumentListSyntaxContents -BaseArgumentList $ArgumentList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BracketedArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax]$BracketedArgumentList
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('BracketedArgumentList');
        Set-BaseArgumentListSyntaxContents -BaseArgumentList $BracketedArgumentList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax]$BaseArgumentList
    )

    Process {
        switch ($BaseArgumentList) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax] } { (Import-ArgumentListSyntax -ArgumentList $BaseArgumentList) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax] } { (Import-BracketedArgumentListSyntax -BracketedArgumentList $BaseArgumentList) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseArgumentListSyntax');
                Set-BaseArgumentListSyntaxContents -BaseArgumentList $BaseArgumentList -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseArgumentListSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax]$BaseArgumentList,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseArgumentList -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseArgumentList -Element $Element;
    }

    <#
        BaseType: Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax] Arguments
    #>
}

#endregion

Function Import-ArgumentSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax]$Argument
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Argument');
        (Import-ExpressionSyntax -Expression $Argument.Expression) | ForEach-Object { $Element.Add($_) }
        if ($null -ne $Argument.NameColon) {
            [DevUtil.XLinqHelper]::SetAttribute($Element, 'Name', $Argument.NameColon.Name.ToString());
        }
        $s = $Argument.RefOrOutKeyword.ToString();
        if ($s.Length -gt 0) {
            [DevUtil.XLinqHelper]::SetAttribute($Element, $s.Substring(0, 1) + $s.Substring(1), $true);
        }
        $s = $Argument.RefKindKeyword.ToString();
        if ($s.Length -gt 0) {
            [DevUtil.XLinqHelper]::SetAttribute($Element, 'Kind', $s);
        }
        Set-SyntaxNodeContents -SyntaxNode $Argument -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-TypeParameterListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax]$TypeParameterList
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TypeParameterList');
        Set-SyntaxNodeContents -SyntaxNode $TypeParameterList -Element $Element;
        if ($TypeParameterList.Parameters.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Parameters');
            $TypeParameterList.Parameters | Import-TypeParameterSyntax | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-TypeParameterConstraintClauseSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax]$TypeParameterConstraintClause
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TypeParameterConstraintClause', [DevUtil.XLinqHelper]::NewAttribute('Name', $TypeParameterConstraintClause.Name.ToString()));
        Set-SyntaxNodeContents -SyntaxNode $TypeParameterConstraintClause -Element $Element;
        if ($TypeParameterConstraintClause.Constraints.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Constraints');
            $TypeParameterConstraintClause.Constraints | Import-TypeParameterConstraintSyntax | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax Import Functions

Function Import-TypeConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax]$TypeConstraint
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TypeConstraint');
        Set-SyntaxNodeContents -SyntaxNode $TypeConstraint -Element $Element;
        if ($null -ne $TypeConstraint.Type) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Type');
            (Import-TypeSyntax -Type $TypeConstraint.Type) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ClassOrStructConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax]$ClassOrStructConstraint
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Constraint', [DevUtil.XLinqHelper]::NewAttribute('Type', $ClassOrStructConstraint.ClassOrStructKeyword.ToString()));
        Set-SyntaxNodeContents -SyntaxNode $ClassOrStructConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ConstructorConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]$ConstructorConstraint
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('EmptyConstructorConstraint');
        Set-SyntaxNodeContents -SyntaxNode $ConstructorConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-DefaultConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax]$DefaultConstraint
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('DefaultConstraint');
        Set-SyntaxNodeContents -SyntaxNode $DefaultConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-TypeParameterConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax]$TypeParameterConstraint
    )

    Process {
        switch ($TypeParameterConstraint) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax] } { (Import-ClassOrStructConstraintSyntax -ClassOrStructConstraint $TypeParameterConstraint) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax] } { (Import-ConstructorConstraintSyntax -ConstructorConstraint $TypeParameterConstraint) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax] } { (Import-TypeConstraintSyntax -TypeConstraint $TypeParameterConstraint) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax] } { (Import-DefaultConstraintSyntax -DefaultConstraint $TypeParameterConstraint) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownTypeParameterConstraintSyntax');
                Set-SyntaxNodeContents -SyntaxNode $TypeParameterConstraint -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

#endregion

Function Import-TypeParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax]$TypeParameter
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TypeParameter', [DevUtil.XLinqHelper]::NewAttribute('Name', $TypeParameter.Identifier.ValueText));
        $Text = $TypeParameter.VarianceKeyword.ValueText.Trim();
        if ($Text.Length -gt 0) {
            [DevUtil.XLinqHelper]::SetAttribute($Element, 'Name', $Text);
        }
        Set-SyntaxNodeContents -SyntaxNode $TypeParameter -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        if ($TypeParameter.AttributeLists.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('AttributeLists');
            $TypeParameter.AttributeLists | Import-AttributeListSyntax | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
    }
}

Function Import-TupleElementSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax]$TupleElement
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('TupleElement',
            ([DevUtil.XLinqHelper]::NewAttribute('Name', $TupleElement.Identifier.ValueText)));
        Set-SyntaxNodeContents -SyntaxNode $TupleElement -Element $Element;
        if ($null -ne $TupleElement.Type) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Type');
            (Import-TypeSyntax -Type $TupleElement.Type) | ForEach-Object { $e.Add($_) }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-ArrayRankSpecifierSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax]$ArrayRankSpecifier
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('ArrayRankSpecifier',
            [DevUtil.XLinqHelper]::NewAttribute('Rank', $ArrayRankSpecifier.Rank));
        Set-SyntaxNodeContents -SyntaxNode $ArrayRankSpecifier -Element $Element;
        if ($ArrayRankSpecifier.Sizes.Count -gt 0) {
            $e = [DevUtil.XLinqHelper]::NewMdElement('Sizes');
            $ArrayRankSpecifier.Sizes | ForEach-Object {
                $e.Add([DevUtil.XLinqHelper]::NewMdElement('Expression', [System.Xml.Linq.XCData]::new($_.ToString())));
            }
            $Element.Add($e);
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

#region Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax Import Functions

Function Import-ParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax]$Parameter
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('Parameter', [DevUtil.XLinqHelper]::NewAttribute('Name', $Parameter.Identifier.ValueText));
        Set-BaseParameterSyntaxContents -BaseParameter $Parameter -Element $Element;
        if ($null -ne $Parameter.Default) {
            $Element.Add([DevUtil.XLinqHelper]::NewMdElement('Default', [System.Xml.Linq.XCData]::new($Parameter.Default.Value.ToString())));
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-FunctionPointerParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax]$FunctionPointerParameter
    )

    Process {
        $Element = [DevUtil.XLinqHelper]::NewMdElement('FunctionPointerParameter');
        Set-BaseParameterSyntaxContents -BaseParameter $FunctionPointerParameter -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-BaseParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax]$BaseParameter
    )

    Process {
        switch ($BaseParameter) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax] } { (Import-ParameterSyntax -Parameter $BaseParameter) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax] } { (Import-FunctionPointerParameterSyntax -FunctionPointerParameter $BaseParameter) | Write-Output; break; }
            default {
                $Element = [DevUtil.XLinqHelper]::NewMdElement('UnknownBaseParameterSyntax');
                Set-BaseParameterSyntaxContents -BaseParameter $BaseParameter -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseParameterSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax]$BaseParameter,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseParameter -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseParameter -Element $Element;
    }

    if ($BaseParameter.Modifiers.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Modifiers');
        $BaseParameter.Modifiers | ForEach-Object { $e.Add([DevUtil.XLinqHelper]::NewMdElement('Modifier', $_.Value)) }
        $Element.Add($e);
    }

    if ($BaseParameter.AttributeLists.Count -gt 0) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('AttributeLists');
        $BaseParameter.AttributeLists | Import-AttributeListSyntax | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }

    if ($null -ne $BaseParameter.Type) {
        $e = [DevUtil.XLinqHelper]::NewMdElement('Type');
        (Import-TypeSyntax -Type $BaseParameter.Type) | ForEach-Object { $e.Add($_) }
        $Element.Add($e);
    }
}

#endregion

Function Import-SyntaxNode {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]$SyntaxNode
    )

    Process {
        [System.Xml.Linq.XElement]$XElement = $null;
        switch ($SyntaxNode) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax] } { (Import-ArgumentSyntax -Argument $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax] } { (Import-ArrayRankSpecifierSyntax -ArrayRankSpecifier $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax] } { (Import-AttributeSyntax -Attribute $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax] } { (Import-AttributeTargetSpecifierSyntax -AttributeTargetSpecifier $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax] } { (Import-CompilationUnitSyntax -CompilationUnit $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax] } { (Import-CrefParameterSyntax -CrefParameter $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax] } { (Import-AccessorDeclarationSyntax -AccessorDeclaration $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] } { (Import-StructuredTriviaSyntax -StructuredTrivia $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax] } { (Import-SubpatternSyntax -Subpattern $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax] } { (Import-UsingDirectiveSyntax -UsingDirective $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax] } { (Import-TypeArgumentListSyntax -TypeArgumentList $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax] } { (Import-FunctionPointerParameterListSyntax -FunctionPointerParameterList $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax] } { (Import-FunctionPointerCallingConventionSyntax -FunctionPointerCallingConvention $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionListSyntax] } { (Import-FunctionPointerUnmanagedCallingConventionListSyntax -FunctionPointerUnmanagedCallingConventionList $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionSyntax] } { (Import-FunctionPointerUnmanagedCallingConventionSyntax -FunctionPointerUnmanagedCallingConvention $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax] } { (Import-TupleElementSyntax -TupleElement $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax] } { (Import-ExpressionOrPatternSyntax -ExpressionOrPattern $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax] } { (Import-BaseArgumentListSyntax -BaseArgumentList $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionColonSyntax] } { (Import-BaseExpressionColonSyntax -BaseExpressionColon $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax] } { (Import-AnonymousObjectMemberDeclaratorSyntax -AnonymousObjectMemberDeclarator $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax] } { (Import-QueryClauseSyntax -QueryClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SelectOrGroupClauseSyntax] } { (Import-SelectOrGroupClauseSyntax -SelectOrGroupClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax] } { (Import-QueryBodySyntax -QueryBody $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.JoinIntoClauseSyntax] } { (Import-JoinIntoClauseSyntax -JoinIntoClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax] } { (Import-OrderingSyntax -Ordering $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryContinuationSyntax] } { (Import-QueryContinuationSyntax -QueryContinuation $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WhenClauseSyntax] } { (Import-WhenClauseSyntax -WhenClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PositionalPatternClauseSyntax] } { (Import-PositionalPatternClauseSyntax -PositionalPatternClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyPatternClauseSyntax] } { (Import-PropertyPatternClauseSyntax -PropertyPatternClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax] } { (Import-InterpolatedStringContentSyntax -InterpolatedStringContent $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationAlignmentClauseSyntax] } { (Import-InterpolationAlignmentClauseSyntax -InterpolationAlignmentClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax] } { (Import-InterpolationFormatClauseSyntax -InterpolationFormatClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax] } { (Import-StatementSyntax -Statement $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax] } { (Import-VariableDeclarationSyntax -VariableDeclaration $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax] } { (Import-VariableDeclaratorSyntax -VariableDeclarator $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax] } { (Import-EqualsValueClauseSyntax -EqualsValueClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDesignationSyntax] } { (Import-VariableDesignationSyntax -VariableDesignation $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElseClauseSyntax] } { (Import-ElseClauseSyntax -ElseClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax] } { (Import-SwitchSectionSyntax -SwitchSection $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax] } { (Import-SwitchLabelSyntax -SwitchLabel $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax] } { (Import-SwitchExpressionArmSyntax -SwitchExpressionArm $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax] } { (Import-CatchClauseSyntax -CatchClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchDeclarationSyntax] } { (Import-CatchDeclarationSyntax -CatchDeclaration $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax] } { (Import-CatchFilterClauseSyntax -CatchFilterClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FinallyClauseSyntax] } { (Import-FinallyClauseSyntax -FinallyClause $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax] } { (Import-ExternAliasDirectiveSyntax -ExternAliasDirective $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax] } { (Import-MemberDeclarationSyntax -MemberDeclaration $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] } { (Import-AttributeListSyntax -AttributeList $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax] } { (Import-AttributeArgumentListSyntax -AttributeArgumentList $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax] } { (Import-AttributeArgumentSyntax -AttributeArgument $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax] } { (Import-NameEqualsSyntax -NameEquals $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax] } { (Import-TypeParameterListSyntax -TypeParameterList $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax] } { (Import-TypeParameterSyntax -TypeParameter $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax] } { (Import-BaseListSyntax -BaseList $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax] } { (Import-BaseTypeSyntax -BaseType $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] } { (Import-TypeParameterConstraintClauseSyntax -TypeParameterConstraintClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax] } { (Import-TypeParameterConstraintSyntax -TypeParameterConstraint $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax] } { (Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax] } { (Import-ConstructorInitializerSyntax -ConstructorInitializer $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax] } { (Import-ArrowExpressionClauseSyntax -ArrowExpressionClause $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax] } { (Import-AccessorListSyntax -AccessorList $SyntaxNode) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax] } { (Import-BaseParameterSyntax -BaseParameter $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefSyntax] } { (Import-CrefSyntax -Cref $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseCrefParameterListSyntax] } { (Import-BaseCrefParameterListSyntax -BaseCrefParameterList $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax] } { (Import-XmlNodeSyntax -XmlNode $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementStartTagSyntax] } { (Import-XmlElementStartTagSyntax -XmlElementStartTag $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementEndTagSyntax] } { (Import-XmlElementEndTagSyntax -XmlElementEndTag $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameSyntax] } { (Import-XmlNameSyntax -XmlName $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlPrefixSyntax] } { (Import-XmlPrefixSyntax -XmlPrefix $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax] } { (Import-XmlAttributeSyntax -XmlAttribute $SyntaxNode) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LineDirectivePositionSyntax] } { (Import-LineDirectivePositionSyntax -LineDirectivePosition $SyntaxNode) | Write-Output; break; }
            default {
                $XElement = [DevUtil.XLinqHelper]::NewMdElement('UnknownSyntax');
                Set-SyntaxNodeContents -SyntaxNode $SyntaxNode -Element $XElement -IsUnknown;
                Write-Output -InputObject $XElement -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-SyntaxNodeContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]$SyntaxNode,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        $Element.Add([DevUtil.XLinqHelper]::NewAttribute('Kind', [Enum]::GetName([Microsoft.CodeAnalysis.CSharp.SyntaxKind], $SyntaxNode.Kind())));
        $Element.Add([DevUtil.XLinqHelper]::NewAttribute('RawKind', $SyntaxNode.RawKind));
        $Element.Add([DevUtil.XLinqHelper]::NewAttribute('TypeName', (ConvertTo-TypeSyntax -Type $SyntaxNode.GetType()).ToString()));
    }
    if ($SyntaxNode.IsMissing) { $Element.Add([DevUtil.XLinqHelper]::NewAttribute('IsMissing', $true)) }
    if ($SyntaxNode.HasLeadingTrivia) {
        $Nodes = @($SyntaxNode.GetLeadingTrivia() | ForEach-Object {
            if ($_.HasStructure) {
                $n = $_.GetStructure();
                if ($n -is [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                    Write-Output -InputObject $n -NoEnumerate;
                }
            }
        });
        if ($Nodes.Count -gt 0) {
            $XElement = [DevUtil.XLinqHelper]::NewMdElement('LeadingTrivia');
            $Element.Add($XElement);
            $Nodes | Import-SyntaxNode | ForEach-Object { $XElement.Add($_) }
        }
    }
    if ($SyntaxNode.HasTrailingTrivia) {
        $Nodes = @($SyntaxNode.GetTrailingTrivia() | Where-Object {
            if ($_.HasStructure) {
                $n = $_.GetStructure();
                if ($n -is [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                    Write-Output -InputObject $n -NoEnumerate;
                }
            }
        });
        if ($Nodes.Count -gt 0) {
            $XElement = [DevUtil.XLinqHelper]::NewMdElement('TrailingTrivia');
            $Element.Add($XElement);
            $Nodes | Import-SyntaxNode | ForEach-Object { $XElement.Add($_) }
        }
    }
}

#endregion

Function Import-SourceFile {
    [CmdletBinding(SupportsShouldProcess = $true, DefaultParameterSetName = 'WcPath')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'WcPath')]
        [SupportsWildcards()]
        [string[]]$Path,

        [Parameter(Mandatory = $true, ParameterSetName = "LiteralPath", ValueFromPipelineByPropertyName = $true)]
        [Alias("PSPath")]
        [string[]]$LiteralPath,

        [Parameter(Mandatory = $true)]
        [string]$BasePath,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $null -ne $_.Root -and $_.Root.Name.Equals([DevUtil.XLinqHelper]::GetMdName('ModelDefinitions')) })]
        [System.Xml.Linq.XDocument]$ModelDefinitions
    )

    Begin {
        $CurrentPathString = $ParentPathString = $null;
        if (Test-Path -LiteralPath $BasePath -PathType Container) {
            $PathInfo = Resolve-Path -LiteralPath $BasePath;
            $PathInfo | Push-Location;
            $CurrentPathString = ".$($PathInfo.Provider.ItemSeparator)";
            $ParentPathString = "..$($PathInfo.Provider.ItemSeparator)";
        } else {
            if (Test-Path -LiteralPath $BasePath) {
                Write-Error -Message "BasePath must refer to a subdirectory." -Category ObjectNotFound -ErrorId 'Import-SourceFile:InvalidBasePath' -TargetObject $BasePath -CategoryTargetName 'BasePath';
            } else {
                Write-Error -Message "BasePath does not exist." -Category ObjectNotFound -ErrorId 'Import-SourceFile:BasePathNotFound' -TargetObject $BasePath -CategoryTargetName 'BasePath';
            }
            continue;
        }
    }

    Process {
        if ($null -ne $CurrentPathString) {
            [string[]]$ResolvedPaths = $null;
            if ($PSCmdlet.ParameterSetName -eq 'LiteralPath') {
                $ResolvedPaths = @($LiteralPath | ForEach-Object {
                    if (Test-Path -LiteralPath $_ -PathType Leaf) {
                        $rp = Resolve-Path -LiteralPath $_ -Relative;
                        if ($rp.StartsWith($ParentPathString) -or (Split-Path -Path $rp -IsAbsolute)) {
                            Write-Error -Message "LiteralPath `"$_`" does not refer to a file contained by `"$BasePath`"" -Category InvalidArgument -ErrorId 'Import-SourceFile:InvalidPath' -TargetObject $_ -CategoryTargetName 'LiteralPath';
                        } else {
                            $rp | Write-Output;
                        }
                    } else {
                        if (Test-Path -LiteralPath $_) {
                            Write-Error -Message "LiteralPath `"$_`" does not refer to a file relative to `"$BasePath`"" -Category ObjectNotFound -ErrorId 'Import-SourceFile:InvalidPath' -TargetObject $_ -CategoryTargetName 'LiteralPath';
                        } else {
                            Write-Error -Message "LiteralPath `"$_`" does not refer to a file relative to `"$BasePath`"" -Category ObjectNotFound -ErrorId 'Import-SourceFile:PathNotFound' -TargetObject $_ -CategoryTargetName 'LiteralPath';
                        }
                    }
                } | Select-Object -Unique);
            } else {
                $ResolvedPaths = @($Path | ForEach-Object {
                    if (Test-Path -Path $_ -PathType Leaf) {
                        (Resolve-Path -Path $_ -Relative) | ForEach-Object {
                            if ($_.StartsWith($ParentPathString) -or (Split-Path -Path $_ -IsAbsolute)) {
                                Write-Error -Message "Path `"$_`" does not refer to a file contained by `"$BasePath`"" -Category InvalidArgument -ErrorId 'Import-SourceFile:InvalidPath' -TargetObject $_ -CategoryTargetName 'Path';
                            } else {
                                if (Test-Path -LiteralPath $_ -PathType Leaf) {
                                    $_ | Write-Output;
                                } else {
                                    Write-Error -Message "Path `"$_`" does not refer to a file relative to `"$BasePath`"" -Category ObjectNotFound -ErrorId 'Import-SourceFile:InvalidPath' -TargetObject $_ -CategoryTargetName 'Path';
                                }
                            }
                        }
                    } else {
                        if (Test-Path -Path $_) {
                            Write-Error -Message "Path `"$_`" does not refer to a file relative to `"$BasePath`"" -Category ObjectNotFound -ErrorId 'Import-SourceFile:InvalidPath' -TargetObject $_ -CategoryTargetName 'Path';
                        } else {
                            Write-Error -Message "Path `"$_`" does not refer to a file relative to `"$BasePath`"" -Category ObjectNotFound -ErrorId 'Import-SourceFile:PathNotFound' -TargetObject $_ -CategoryTargetName 'Path';
                        }
                    }
                } | Select-Object -Unique);
            }
            $ResolvedPaths | ForEach-Object {
                $SourcePath = $_;
                if ($SourcePath.StartsWith($CurrentPathString)) { $SourcePath = $SourcePath.Substring($CurrentPathString.Length) }
                $SouceCode = (Get-Content -LiteralPath $_) | Out-String;
                if ([string]::IsNullOrWhiteSpace($SouceCode)) {
                    Write-Warning -Message "File `"$SourcePath`" is empty.";
                } else {
                    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = $null;
                    try { $SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($SouceCode, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $SourcePath); }
                    catch {
                        $e = Get-ExceptionObject -InputObject $_;
                        if ($PSCmdlet.ParameterSetName -eq 'LiteralPath') {
                            Write-Error -Exception $e -Message 'Failed to read source code' -Category ParserError -ErrorId 'Import-SourceFile::ParseException' -TargetObject $SourcePath `
                                -CategoryReason '[Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText threw an exception' -CategoryActivity 'CSharpSyntaxTree.ParseText' `
                                -CategoryTargetName 'LiteralPath';
                        } else {
                            Write-Error -Exception $e -Message 'Failed to read source code' -Category ParserError -ErrorId 'Import-SourceFile::ParseException' -TargetObject $SourcePath `
                                -CategoryReason '[Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText threw an exception' -CategoryActivity 'CSharpSyntaxTree.ParseText' `
                                -CategoryTargetName 'Path';
                        }
                    }
                    if ($null -ne $SyntaxTree) {
                        if ($SyntaxTree.HasCompilationUnitRoot) {
                            if ($null -ne (Get-ModelDefinitionSourceElement -Path $SourcePath -Document $ModelDefinitions)) {
                                Write-Warning -Message "File `"$SourcePath`" is has already been imported.";
                            } else {
                                $Id = [Guid]::NewGuid();
                                $SourceElement = [DevUtil.XLinqHelper]::NewMdElement('Source',
                                    [DevUtil.XLinqHelper]::NewAttribute('Path', $SourcePath),
                                    [DevUtil.XLinqHelper]::NewAttribute('Id', $Id)
                                );
                                $XElement = $ModelDefinitions.Root.Element([DevUtil.XLinqHelper]::GetMdName('Sources'));
                                if ($null -eq $XElement) {
                                    if ($null -eq $XElement) {
                                        $ModelDefinitions.Root.AddFirst([DevUtil.XLinqHelper]::NewMdElement('Sources', $SourceElement));
                                    } else {
                                        $XElement.Add($SourceElement);
                                    }
                                } else {
                                    $XElement.Add($SourceElement);
                                }
                                [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax]$Syntax = $SyntaxTree.GetCompilationUnitRoot();
                                $Syntax.Externs | Import-ExternAliasDirectiveSyntax | ForEach-Object { $SourceElement.Add($_) }
                                $Syntax.Usings | Import-UsingDirectiveSyntax | ForEach-Object { $SourceElement.Add($_) }
                                $Syntax.AttributeLists | Import-AttributeListSyntax | ForEach-Object { $SourceElement.Add($_) }
                                $Syntax.Members | Import-MemberDeclarationSyntax | ForEach-Object { $SourceElement.Add($_) }
                            }
                        }
                    }
                }
            }
        }
    }

    End {
        if ($null -ne $CurrentPathString) { Pop-Location }
    }
}
