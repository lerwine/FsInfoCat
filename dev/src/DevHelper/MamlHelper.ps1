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

Function Add-TextElement {
    [CmdletBinding(DefaultParameterSetName = 'NoEmpty')]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement,
        
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$Value,

        [Parameter(Mandatory = $true)]
        [ValidateRange('maml', 'command', 'dev')]
        [string]$NS,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-NCName })]
        [string]$Name,

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
    $commandElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('command', 'command', $Script:PsHelpNamespaces['command']));
    $commandElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'maml', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['maml'];
    $commandElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'dev', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['dev'];
    Write-Output -InputObject $XmlDocument -NoEnumerate;
}

Function New-PsCommandHelpXml {
    [CmdletBinding()]
    Param(
    )
    
    $XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
    $XmlElement =$XmlDocument.AppendChild($XmlDocument.CreateElement('', 'helpItems', $Script:PsHelpNamespaces['msh']));
    $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xsi', 'schemaLocation', 'http://www.w3.org/2001/XMLSchema-instance')).Value = 'http://schemas.microsoft.com/maml/2004/10 file:///C:/Users/lerwi/Git/PowerShell/src/Schemas/PSMaml/Maml.xsd http://schemas.microsoft.com/maml/dev/command/2004/10 C:\Users\lerwi\Git\PowerShell\src\Schemas\PSMaml\developerCommand.xsd http://schemas.microsoft.com/maml/dev/2004/10 C:\Users\lerwi\Git\PowerShell\src\Schemas\PSMaml\developer.xsd';
    $Nsmgr = [System.Xml.XmlNamespaceManager]::new($Xml.NameTable);
    $Script:PsHelpNamespaces.Keys | ForEach-Object { $Nsmgr.AddNamespace($_, $Script:PsHelpNamespaces[$_]) }
    $commandElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('command', 'command', $Script:PsHelpNamespaces['command']));
    $commandElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'maml', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['maml'];
    $commandElement.Attributes.Append($XmlDocument.CreateAttribute('xmlns', 'dev', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['dev'];
    Write-Output -InputObject $XmlDocument -NoEnumerate;
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