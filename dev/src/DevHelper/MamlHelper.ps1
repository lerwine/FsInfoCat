class PsHelpXml : System.Xml.XmlDocument {
}

$Script:PsHelpNamespaces = @{
    msh = "http://msh";
    maml = "http://schemas.microsoft.com/maml/2004/10";
    command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
    dev = "http://schemas.microsoft.com/maml/dev/2004/10";
    MSHelp = "http://msdn.microsoft.com/mshelp";
};
$Script:SchemaLocations = @{
    msh = [Uri]::new(($PSScriptRoot | Join-Path -ChildPath 'Msh.xsd')).AbsoluteUri;
    maml = [Uri]::new(($PSScriptRoot | Join-Path -ChildPath 'PSMaml/Maml.xsd')).AbsoluteUri;
    command = [Uri]::new(($PSScriptRoot | Join-Path -ChildPath 'PSMaml/developerCommand.xsd')).AbsoluteUri;
    dev = [Uri]::new(($PSScriptRoot | Join-Path -ChildPath 'PSMaml/developer.xsd')).AbsoluteUri;
};

Function Test-NCName {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$Value
    )

    Process {
        if ($null -eq $Value) {
            Write-Debug -Message 'Value was null';
            $false | Write-Output;
        } else {
            $s = '';
            if ($Value -is [string]) {
                $s = $Value;
            } else {
                if ($null -ne $Value.Text) {
                    $s = '' + $Value.Text;
                } else {
                    $s = '' + $Value;
                }
            }
            if ($s.Trim().Length -eq 0) {
                Write-Debug -Message 'Empty value';
                $false | Write-Output;
            } else {
                $a = $s.ToCharArray();
                $success = $true;
                $index = 0;
                if ([System.Xml.XmlConvert]::IsStartNCNameChar($a[0])) {
                    while (++$index -lt $a.Length) {
                        if (-not [System.Xml.XmlConvert]::IsNCNameChar($a[$index])) { break }
                    }
                }
                if ($index -lt $a.Length) {
                    Write-Debug -Message "Invalid NCName char at index $index";
                    $false | Write-Output;
                } else {
                    $true | Write-Output;
                }
            }
        }
    }
}

Function Test-PsTypeName {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Value
    )

    Begin {
        $NameTokenRegex = New-Object -TypeName 'System.Text.RegularExpressions.Regex' -ArgumentList '(?<n>[a-z_][a-z_\d]*(\.[a-z_][a-z\d]*)*)|(?<o>\[ *(?<a>(,[ ,]*)?\](\[[, ]*\])*)?)|(?<s> *, *)|(?<c> *\])', ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]::Compiled));
    }

    Process {
        if ($null -eq $Value -or ($Value = $Value.Trim()).Length -eq 0) {
            $false | Write-Output;
        } else {
            $OriginalValue = $Value;
            $mode = 0; # match expected: 0=n; 1=a|o|s|c; 2=s|c; 3=a|s|c
            $m = $NameTokenRegex.Match($Value);
            $Passed = $m.Success -and $m.Index -eq 0;
            $Level = 0;
            $Position = 0;
            $Iteration = 0;
            while ($Passed) {
                Write-Information -Message "Matched $($m.Length) characters";
                $Iteration++;
                Write-Information -Message "Checking mode $mode";
                switch ($mode) {
                    0 {
                        $Passed = $m.Groups['n'].Success;
                        break;
                    }
                    1 {
                        $Passed = -not $m.Groups['n'].Success;
                        break;
                    }
                    2 {
                        $Passed = $m.Groups['s'].Success -or $m.Groups['c'].Success;
                        break;
                    }
                    default {
                        $Passed = -not ($m.Groups['n'].Success -or $m.Groups['o'].Success);
                        break;
                    }
                }
                if (-not $Passed) { break; }
                Write-Information -Message "Mode $mode passed";
                if ($m.Groups['n'].Success) {
                    Write-Information -Message "Name matched change to mode 1";
                    $Mode = 1;
                } else {
                    if ($m.Groups['a'].Success) {
                        Write-Information -Message "Array matched change to mode 2";
                        $Mode = 2;
                    } else {
                        if ($m.Groups['c'].Success) {
                            Write-Information -Message "Close matched checking level";
                            $Passed = $Level -gt 0;
                            if (-not $Passed) { break }
                            Write-Information -Message "Close Level $Level passed; decrementing";
                            $Level--;
                            Write-Information -Message "Change to mode 3";
                            $Mode = 3;
                        } else {
                            if ($m.Groups['o'].Success) {  Write-Information -Message "Incrementing from level $Level"; $Level++ }
                            Write-Information -Message "Change to mode 0";
                            $Mode = 0;
                        }
                    }
                }
                $Position += $m.Length;
                Write-Information -Message "Moved to position $Position";
                if ($m.Length -eq $Value.Length) { break }
                $Value = $Value.Substring($m.Length);
                $m = $NameTokenRegex.Match($Value);
                $Passed = $m.Success -and $m.Index -eq 0;
            }

            if ($Passed -and $Level -eq 0) {
                $true | Write-Output;
            } else {
                Write-Warning -Message "Failed at Iteration $Iteration; Position $Position; Level $Level; Mode $Mode (`"$($OriginalValue.Substring($Position))`" of `"$OriginalValue`"";
                $false | Write-Output;
            }
        }
    }
}
Function Add-TextElement {
    [CmdletBinding(DefaultParameterSetName = 'NoEmpty')]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true)]
        [ValidateRange('maml', 'command', 'dev')]
        [string]$NS,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$Value,

        [Parameter(Mandatory = $true, ParameterSetName = 'CommentIfEmpty')]
        [string]$CommentIfEmpty,

        [Parameter(Mandatory = $true, ParameterSetName = 'TextIfEmpty')]
        [string]$TextIfEmpty,

        [Parameter(ParameterSetName = 'NoEmpty')]
        [switch]$NoEmpty,

        [switch]$PassThru
    )

    if ($null -ne $Value) {
        $s = $null;
        if ($Value -is [string]) {
            $s = $Value;
        } else {
            if ($null -eq $Value.Text) {
                $s = '' + $Value;
            } else {
                $s = '' + $Value.Text;
            }
        }
        if (-not [string]::IsNullOrWhiteSpace($s)) {
            $XmlElement = $ParentElement.PSBase.AppendChild(
                $ParentElement.PSBase.OwnerDocument.CreateElement($Name, $Script:PsHelpNamespaces[$NS])
            );
            $XmlElement.PSBase.InnerText = $s;
            if ($PassThru) { return $XmlElement }
            return;
        }
    }
    if ($NoEmpty.IsPresent) { return }
    $XmlElement = $ParentElement.PSBase.AppendChild(
        $ParentElement.PSBase.OwnerDocument.CreateElement($Name, $Script:PsHelpNamespaces[$NS])
    );
    if ($PSBoundParameters.ContainsKey('TextIfEmpty')) {
        $ParentElement.InnerText = $TextIfEmpty;
    } else {
        if ($PSBoundParameters.ContainsKey('CommentIfEmpty')) {
            $ParentElement.AppendChild($ParentElement.PSBase.OwnerDocument.CreateComment($CommentIfEmpty)) | Out-Null;
        }
    }
    if ($PassThru) { return $XmlElement }
}

Function Add-MamlParagraphs {
    [CmdletBinding(DefaultParameterSetName = 'CommentIfEmpty')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$ParaObj,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(ParameterSetName = 'CommentIfEmpty')]
        [string]$CommentIfEmpty,

        [Parameter(Mandatory = $true, ParameterSetName = 'TextIfEmpty')]
        [string[]]$TextIfEmpty
    )

    Begin { $NoContent = $true }
    Process {
        if ($null -ne (Add-TextElement -Value $ParaObj -ParentElement $ParentElement -NS 'maml' -Name 'para' -NoEmpty -PassThru)) { $NoContent = $false }
    }
    End {
        if ($NoContent) {
            if ($PSBoundParameters.ContainsKey('CommentIfEmpty')) {
                $ParentElement.PSBase.AppendChild(
                    $ParentElement.PSBase.OwnerDocument.CreateElement('para', $Script:PsHelpNamespaces['maml'])
                ).AppendChild($ParentElement.OwnerDocument.CreateComment($CommentIfEmpty)) | Out-Null;
            } else {
                if ($PSBoundParameters.ContainsKey('TextIfEmpty')) {
                    $TextIfEmpty | ForEach-Object {
                        $ParentElement.PSBase.AppendChild(
                            $ParentElement.PSBase.OwnerDocument.CreateElement('para', $Script:PsHelpNamespaces['maml'])
                        ).InnerText = $_;
                    }
                }
            }
        }
    }
}

Function New-PsHelpNamespaceManager {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Target
    )

    $Nsmgr = [System.Xml.XmlNamespaceManager]::new($Xml.NameTable);
    $Script:PsHelpNamespaces.Keys | ForEach-Object { $Nsmgr.AddNamespace($_, $Script:PsHelpNamespaces[$_]) }
    Write-Output -InputObject $Nsmgr -NoEnumerate;
}

Function New-PsHelpXml {
    [CmdletBinding()]
    Param([switch]$IncludeSchemaLocation)

    $XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
    $XmlElement =$XmlDocument.AppendChild($XmlDocument.CreateElement('', 'helpItems', $Script:PsHelpNamespaces['msh']));
    if ($IncludeSchemaLocation) {
        $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xsi', 'schemaLocation', 'http://www.w3.org/2001/XMLSchema-instance')).Value = `
            @($Script:SchemaLocations.Keys | ForEach-Object { $Script:PsHelpNamespaces[$_]; $Script:SchemaLocations[$_] }) -join ' ';
    }
    Write-Output -InputObject $XmlDocument -NoEnumerate;
}

Function New-PsCommandHelpXml {
    [CmdletBinding(DefaultParameterSetName = 'Common')]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $null -ne $_.DocumentElement -and $_.LocalName -eq 'helpItems' -and $_.NamespaceURI -eq $Script:PsHelpNamespaces['msh'] })]
        [System.Xml.XmlDocument]$PsHelpXml,

        [Parameter(Mandatory = $true, ParameterSetName = 'Common')]
        [System.Management.Automation.VerbsCommon]$CommonVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Communications')]
        [System.Management.Automation.VerbsCommunications]$CommunicationsVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Data')]
        [System.Management.Automation.VerbsData]$DataVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Diagnostic')]
        [System.Management.Automation.VerbsDiagnostic]$DiagnosticVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Lifecycle')]
        [System.Management.Automation.VerbsLifecycle]$LifecycleVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Security')]
        [System.Management.Automation.VerbsSecurity]$SecurityVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Other')]
        [System.Management.Automation.VerbsOther]$OtherVerb,

        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][a-z\d]*$')]
        [string]$Noun,

        [string]$Synopsis = '',

        [Version]$Version = [version]::new(0, 1),

        [string[]]$Description
    )

    $commandElement = $PsHelpXml.DocumentElement.AppendChild($PsHelpXml.CreateElement('command', 'command', $Script:PsHelpNamespaces['command']));
    $commandElement.Attributes.Append($PsHelpXml.CreateAttribute('xmlns', 'maml', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['maml'];
    $commandElement.Attributes.Append($PsHelpXml.CreateAttribute('xmlns', 'dev', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['dev'];
    $detailsElement = $commandElement.AppendChild($PsHelpXml.CreateElement('command', 'details', $Script:PsHelpNamespaces['command']));
    $Verb = '';
    switch ($PSCmdlet.ParameterSetName) {
        'Common' { $Verb = $CommonVerb.ToString('F'); break; }
        'Communications' { $Verb = $CommunicationsVerb.ToString('F'); break; }
        'Data' { $Verb = $DataVerb.ToString('F'); break; }
        'Diagnostic' { $Verb = $DiagnosticVerb.ToString('F'); break; }
        'Lifecycle' { $Verb = $LifecycleVerb.ToString('F'); break; }
        'Security' { $Verb = $SecurityVerb.ToString('F'); break; }
        default { $Verb = $OtherVerb.ToString('F'); break; }
    }
    Add-TextElement -ParentElement $detailsElement -NS 'command' -Name 'name' -Value "$Verb-$Noun";
    $descriptionElement = $detailsElement.AppendChild($PsHelpXml.CreateElement('maml', 'description', $Script:PsHelpNamespaces['maml']));
    Add-MamlParagraphs -ParentElement $descriptionElement -ParaObj $Summary -CommentIfEmpty 'Summary goes here';
    $copyrightElement = $detailsElement.AppendChild($PsHelpXml.CreateElement('maml', 'copyright', $Script:PsHelpNamespaces['maml']));
    Add-MamlParagraphs -ParentElement $copyrightElement -ParaObj "Copyright © Leonard Thomas Erwine $([DateTime]::Now.ToString('yyyy'))";
    Add-TextElement -ParentElement $detailsElement -NS 'command' -Name 'verb' -Value $Verb;
    Add-TextElement -ParentElement $detailsElement -NS 'command' -Name 'noun' -Value $Noun;
    Add-TextElement -ParentElement $detailsElement -NS 'dev' -Name 'version' -Value $Version;
    $descriptionElement = $commandElement.AppendChild($PsHelpXml.CreateElement('maml', 'description', $Script:PsHelpNamespaces['maml']));
    Add-MamlParagraphs -ParentElement $descriptionElement -ParaObj $Description -CommentIfEmpty 'Detailed description goes here';
    Write-Output -InputObject $commandElement -NoEnumerate;
}

Function New-CommandParameter {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_.LocalName -eq 'command' -and $_.NamespaceURI -eq $Script:PsHelpNamespaces['command'] })]
        [System.Xml.XmlElement]$CommandElement,

        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][a-z\d]*$')]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsTypeName })]
        [string]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,

        [string[]]$Description,

        [ValidateRange(1, [int]::MaxValue)]
        [int]$Position,

        [string]$DefaultValue = 'None',

        [switch]$PipelineByValue,

        [switch]$PipelineByPropertyName,

        # Array, collection, etc
        [switch]$VariableLength,

        [switch]$Required,

        # Can include wildcard characters.
        [switch]$Globbing,

        [switch]$PassThru
    )

    $PsHelpXml = $CommandElement.OwnerDocument;
    $parametersElement = $CommandElement.SelectSingleNode('command:parameters', $Nsmgr);
    if ($null -eq $parametersElement) {
        $parametersElement = $CommandElement.AppendChild($PsHelpXml.CreateElement('command', 'parameters', $Script:PsHelpNamespaces['command']));
    } else {
        if ($null -ne (@($CommandElement.SelectNodes("command:parameters/command:parameter", $Nsmgr)) | Where-Object { $_.name -ieq $Name } | Select-Object -First 1)) {
            Write-Warning -Message 'A parameter with that name already exists';
            break;
        }
    }
    $parameterElement = $parametersElement.AppendChild($PsHelpXml.CreateElement('command', 'parameter', $Script:PsHelpNamespaces['command']));
    $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('required')).Value = $Required.IsPresent.ToString().ToLower();
    $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('variableLength')).Value = $VariableLength.IsPresent.ToString().ToLower();
    $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('globbing')).Value = $Globbing.IsPresent.ToString().ToLower();
    if ($PipelineByValue.IsPresent) {
        if ($PipelineByPropertyName.IsPresent) {
            $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('pipelineInput')).Value = 'True (ByValue, ByPropertyName)';
        } else {
            $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('pipelineInput')).Value = 'True (ByValue)';
        }
    } else {
        if ($PipelineByPropertyName.IsPresent) {
            $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('pipelineInput')).Value = 'True (ByPropertyName)';
        } else {
            $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('pipelineInput')).Value = 'False';
        }
    }
    if ($PSBoundParameters.ContainsKey('Position')) {
        $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('position')).Value = $Position.ToString();
    } else {
        $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('position')).Value = 'named';
    }
    Add-TextElement -ParentElement $parameterElement -NS 'maml' -Name 'name' -Value $Name;
    $descriptionElement = $parameterElement.AppendChild($PsHelpXml.CreateElement('maml', 'description', $Script:PsHelpNamespaces['maml']));
    Add-MamlParagraphs -ParentElement $descriptionElement -ParaObj $Description -CommentIfEmpty 'Detailed description goes here';
    $parameterValueElement = Add-TextElement -ParentElement $parameterElement -NS 'command' -Name 'parameterValue' -Value $Type -PassThru;
    $parameterValueElement.Attributes.Append($PsHelpXml.CreateAttribute('required')).Value = $Required.IsPresent.ToString().ToLower();
    $parameterValueElement.Attributes.Append($PsHelpXml.CreateAttribute('variableLength')).Value = $VariableLength.IsPresent.ToString().ToLower();
    $typeElement = $parameterElement.AppendChild($PsHelpXml.CreateElement('dev', 'type', $Script:PsHelpNamespaces['maml']));
    Add-TextElement -ParentElement $typeElement -NS 'maml' -Name 'name' -Value $Type;
    $typeElement.AppendChild($PsHelpXml.CreateElement('maml', 'uri', $Script:PsHelpNamespaces['maml'])).IsEmpty = $true;
    Add-TextElement -ParentElement $parameterElement -NS 'dev' -Name 'defaultValue' -Value $DefaultValue;
    if ($PassThru.IsPresent) { $parameterElement | Write-Output }
}

Function New-SyntaxItem {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_.LocalName -eq 'command' -and $_.NamespaceURI -eq $Script:PsHelpNamespaces['command'] })]
        [System.Xml.XmlElement]$CommandElement,

        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidatePattern('^[A-Z][a-z\d]*$')]
        [string[]]$Name,

        [switch]$PassThru
    )

    Begin {
        $Elements = @();
        $Success = $true;
    }

    Process {
        if ($Success) {
            foreach ($n in $Name) {
                $parameterElement = @($CommandElement.SelectNodes("command:parameters/command:parameter", $Nsmgr)) | Where-Object { $_.name -ieq $n } | Select-Object -First 1;
                $Success = $null -ne $parameterElement;
                if ($Success) {
                    if (@($Elements | Where-Object { [object]::ReferenceEquals($_, $ParentElement) }).Count -eq 0) {
                        $Elements += @($parameterElement)
                    }
                } else {
                    Write-Warning -Message "Parameter named `"$n`" not found.";
                    break;
                }
            }
        }
    }

    End {
        if ($success) {
            $PsHelpXml = $CommandElement.OwnerDocument;
            $syntaxElement = $CommandElement.SelectSingleNode('command:syntax', $Nsmgr);
            if ($null -eq $syntaxElement) {
                $syntaxElement = $CommandElement.AppendChild($PsHelpXml.CreateElement('command', 'syntax', $Script:PsHelpNamespaces['command']));
            }
            $syntaxItemElement = $syntaxElement.AppendChild($PsHelpXml.CreateElement('command', 'syntaxItem', $Script:PsHelpNamespaces['command']));
            Add-TextElement -ParentElement $syntaxItemElement -NS 'maml' -Name 'name' -Value ($CommandElement.SelectSingleNode('command:details/command:name', $Nsmgr).InnerText);
            foreach ($e in $Elements) {
                $parameterElement = $syntaxItemElement.AppendChild($PsHelpXml.CreateElement('command', 'parameter', $Script:PsHelpNamespaces['command']));
                $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('required')).Value = $e.SelectSingleNode('@required').Value;
                $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('position')).Value = $e.SelectSingleNode('@position').Value;
                $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('pipelineInput')).Value = $e.SelectSingleNode('@pipelineInput').Value;
                $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('variableLength')).Value = $e.SelectSingleNode('@variableLength').Value;
                $parameterElement.Attributes.Append($PsHelpXml.CreateAttribute('globbing')).Value = $e.SelectSingleNode('@globbing').Value;
                $parameterElement.AppendChild($e.SelectSingleNode('maml:name', $nsmgr).Clone($true)) | Out-Null;
                $parameterElement.AppendChild($e.SelectSingleNode('maml:description', $nsmgr).Clone($true)) | Out-Null;
                $parameterElement.AppendChild($e.SelectSingleNode('command:parameterValue', $nsmgr).Clone($true)) | Out-Null;
                $parameterElement.AppendChild($e.SelectSingleNode('dev:type', $nsmgr).Clone($true)) | Out-Null;
                $parameterElement.AppendChild($e.SelectSingleNode('dev:defaultValue', $nsmgr).Clone($true)) | Out-Null;
                if ($PassThru.IsPresent) { $syntaxItemElement | Write-Output }
            }
        }
    }
}

Function Import-MamlHelpInfo {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [HelpInfo]$MamlHelpInfo,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlDocument]$PsHelpXml
    )

    $detailsElement = $commandElement.AppendChild($XmlDocument.CreateElement('details', $Script:PsHelpNamespaces['command']));
    $CommandName = $MamlHelpInfo.details.name;
    if ([string]::IsNullOrWhiteSpace($CommandName)) { $CommandName = $MamlHelpInfo.Name }
    $Verb = $MamlHelpInfo.details.verb;
    $Noun = $MamlHelpInfo.details.noun;
    if ([string]::IsNullOrWhiteSpace($Verb) -and [string]::IsNullOrWhiteSpace($Noun)) {
        ($Verb, $Noun) = $CommandName.Split('-', 2);
        if ($null -eq $Noun) {
            $Noun = $Verb;
            $Verb = '';
        }
    } else {
        if ([string]::IsNullOrWhiteSpace($CommandName)) { $CommandName = @(($Verb, $Noun) | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) -join '-' }
    }
    $copyright = @();
    if ($null -ne $MamlHelpInfo.details.copyright) { $copyright = @($MamlHelpInfo.details.copyright) }
    Add-TextElement -ParentElement $detailsElement -Value $CommandName -NS 'command' -Name 'name' -CommentIfEmpty ' Command name goes here ';
    $MamlHelpInfo.details | Add-MamlParagraphs -ParentElement $descriptionElement -CommentIfEmpty ' Add synopsis here ';
    $descriptionElement = $detailsElement.AppendChild($XmlDocument.CreateElement('description', $Script:PsHelpNamespaces['maml']));
    $Text = @();
    if ($null -ne $MamlHelpInfo.details.description) { $Text = @($MamlHelpInfo.details.description) }
    $Text | Add-MamlParagraphs -ParentElement $descriptionElement -CommentIfEmpty ' Add synopsis here ';
    $copyrightElement = $detailsElement.AppendChild($XmlDocument.CreateElement('copyright', $Script:PsHelpNamespaces['maml']));
    $Copyright | Add-MamlParagraphs -ParentElement $detailsElement -TextIfEmpty "Copyright © Leonard Thomas Erwine $([DateTime]::Now.ToString('yyyy'))";
    $detailsElement.AppendChild($XmlDocument.CreateElement('verb', $Script:PsHelpNamespaces['command'])).InnerText = $Verb;
    $detailsElement.AppendChild($XmlDocument.CreateElement('noun', $Script:PsHelpNamespaces['command'])).InnerText = $Noun;
    $MamlHelpInfo.details.version
    [Xml]$XmlDocument = @'
    <command:details>
      <command:name><!--Add command name here --></command:name>
      <maml:description>
        <maml:para><!--Add synopsis --></maml:para>
      </maml:description>
      <maml:copyright>
        <maml:para>Copyright © Leonard Thomas Erwine 2021</maml:para>
      </maml:copyright>
      <command:verb><!--Add verb --></command:verb>
      <command:noun><!--Add noun --></command:noun>
      <dev:version>0.1</dev:version>
    </command:details>

    <maml:description>
      <maml:para><!--Add detailed description here --></maml:para>
    </maml:description>
    <command:syntax>
      <command:syntaxItem>
        <maml:name><!--Add command name here--></maml:name>
        <command:parameter required="true" position="0" pipelineInput="False" variableLength="true" globbing="false">
          <maml:name><!--Add parameter name here--></maml:name>
          <maml:description>
            <maml:para><!--Add parameter information here--></maml:para>
          </maml:description>
          <command:parameterValue required="true" variableLength="false"><!--Add parameter type description here--></command:parameterValue>
          <dev:type>
            <maml:name><!--Add parameter type name--></maml:name>
            <maml:uri />
          </dev:type>
          <dev:defaultValue>None</dev:defaultValue>
        </command:parameter>
      </command:syntaxItem>
    </command:syntax>
    <command:parameters>
      <command:parameter required="false" variableLength="true" globbing="true" pipelineInput="False" position="0">
        <maml:name><!--Add parameter name here--></maml:name>
        <maml:description>
          <maml:para><!--Add parameter information here--></maml:para>
        </maml:description>
        <command:parameterValue required="true" variableLength="false"><!--Add parameter type description here--></command:parameterValue>
        <dev:type>
          <maml:name><!--Add parameter type name--></maml:name>
          <maml:uri />
        </dev:type>
        <dev:defaultValue>None</dev:defaultValue>
      </command:parameter>
    </command:parameters>
    <command:inputTypes>
      <command:inputType>
        <dev:type>
          <maml:name><!--Add input type name here--></maml:name>
          <maml:uri />
        </dev:type>
        <maml:description>
          <maml:para><!--Add input type information here--></maml:para>
        </maml:description>
      </command:inputType>
    </command:inputTypes>
    <command:returnValues>
      <command:returnValue>
        <dev:type>
          <maml:name><!--Add return value type here--></maml:name>
          <maml:uri />
        </dev:type>
        <maml:description>
          <maml:para><!--Add return value information here--></maml:para>
        </maml:description>
      </command:returnValue>
    </command:returnValues>
    <command:terminatingErrors />
    <command:nonTerminatingErrors />
    <maml:alertSet>
      <maml:alert>
        <maml:para><!--Add Note information here--></maml:para>
      </maml:alert>
    </maml:alertSet>
    <command:examples>
      <command:example>
        <maml:title><!--Add example title here--></maml:title>
        <dev:code><!--Add cmdlet examples here-->*</dev:code>
        <dev:remarks>
          <maml:para><!--Add example remarks here--></maml:para>
        </dev:remarks>
      </command:example>

    </command:examples>
    <maml:relatedLinks>
      <maml:navigationLink>
        <maml:linkText>Online Version:</maml:linkText>
        <maml:uri>https://docs.microsoft.com/en-us/powershell/module/az.storage/get-azstorageblob</maml:uri>
      </maml:navigationLink>
      <!--Add links to related content here-->
    </maml:relatedLinks>
  </command:command>
</helpItems>
'@
    $MamlHelpInfo.Name
}
$MamlHelpInfo = Get-Help -Name 'Get-ChildItem';
$MamlHelpInfo.GetType().FullName
