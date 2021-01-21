Import-Module -Name 'C:\Users\lerwi\Git\FsInfoCat\dev\bin\DevHelper' -ErrorAction Stop;

$Script:MamlNamespaces = @{
    msh = "http://msh";
    maml = "http://schemas.microsoft.com/maml/2004/10";
    command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
    dev = "http://schemas.microsoft.com/maml/dev/2004/10";
};
[Xml]$Xml = '<helpItems xmlns="http://msh" schema="maml" />';
$Path = 'C:\Users\lerwi\Git\FsInfoCat\dev\src\DevHelper\DevHelperLib.dll-help.xml';
$Xml.Load($Path);
$Nsmgr = [System.Xml.XmlNamespaceManager]::new($Xml.NameTable);
$Script:MamlNamespaces.Keys | ForEach-Object { $Nsmgr.AddNamespace($_, $Script:MamlNamespaces[$_]) }
foreach ($n in @($Xml.SelectNodes('//command:parameter[not(count(@aliases)=0)]', $Nsmgr))) {
    $n.PSBase.Attributes.Remove($n.PSBase.SelectSingleNode("@aliases")) | Out-Null;
}
$settings = [System.Xml.XmlWriterSettings]::new();
$settings.Indent = $true;
$settings.Encoding = [System.Text.UTF8Encoding]::new($false, $true);
$writer = [System.Xml.XmlWriter]::Create($Path, $settings);
try {
    $Xml.WriteTo($writer);
    $Writer.Flush();
} finally { $Writer.Close() }


Export-Xml -Xml $Xml -Path "$Path.Tmp" -ErrorAction Stop;

Function Test-MshNode {
    [CmdletBinding(DefaultParameterSetName = 'Any')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [System.Xml.XmlNode]$InputNode,

        [Parameter(ParameterSetName = 'Any')]
        # Test for and element in any PS help document namespace.
        [switch]$Any,

        [Parameter(Mandatory = $true, ParameterSetName = 'msh')]
        # Test for the msh document qualified name ({ "http://msh", "helpItems" }).
        [switch]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'command')]
        # Test for an element in the command namespace (http://schemas.microsoft.com/maml/dev/command/2004/10).
        [switch]$Command,

        [Parameter(Mandatory = $true, ParameterSetName = 'maml')]
        # Test for an element in the maml namespace (http://schemas.microsoft.com/maml/2004/10).
        [switch]$Maml,

        [Parameter(Mandatory = $true, ParameterSetName = 'dev')]
        # Test for an element in the dev namespace (http://schemas.microsoft.com/maml/dev/2004/10).
        [switch]$Dev,
        
        [Parameter(ParameterSetName = 'Any')]
        [Parameter(ParameterSetName = 'Command')]
        [Parameter(ParameterSetName = 'Maml')]
        [Parameter(ParameterSetName = 'Dev')]
        # Optionally test for one or more element local names.
        [string[]]$Name
    )

    Begin { $Success = $true }

    Process {
        if ($Success) {
            if ($null -eq $InputNode) {
                $Success = $false;
                Write-Debug -Message "Failed due to null value";
            } else {
                $n = $null;
                if ($InputNode -is [System.Xml.XmlDocument]) {
                    if ($null -eq $InputNode.DocumentElement) {
                        $Success = $false;
                        Write-Debug -Message "Failed due to Xml Document lacking a document element";
                    } else {
                        $n = $InputNode.DocumentElement;
                    }
                } else {
                    if ($InputNode -isnot [System.Xml.XmlElement]) {
                        $Success = $false;
                        Write-Debug -Message "Failed because Xml node was neither an xml element nor a document";
                    } else {
                        $n = $InputNode;
                    }
                }
                if ($null -ne $n) {
                    $LocalNames = @($Name);
                    if ($Script:MamlNamespaces.ContainsKey($PSCmdlet.ParameterSetName)) {
                        $uri = $Script:MamlNamespaces[$PSCmdlet.ParameterSetName];
                        if ($n.NamespaceURI -ne $uri) {
                            $Success = $false;
                            Write-Debug -Message "Namespace mismatch. Expected: `"$uri`"; Actual: `"$($n.NamespaceURI)`"";
                        } else {
                            if ($Document.IsPresent) { $LocalNames = @('helpItems') }
                        }
                    } else {
                        if (@($Script:MamlNamespaces.Values) -cnotcontains $n.NamespaceURI) {
                            $Success = $false;
                            $d = @($Script:MamlNamespaces.Values) -join '" | "';
                            Write-Debug -Message "Namespace mismatch. Expected one of: (`"$d`"); Actual: `"$($n.NamespaceURI)`"";
                        }
                    }
                    if ($Success -and $LocalNames.Count -gt 0 -and $LocalNames -cnotcontains $n.LocalName) {
                        $Success = $false;
                        if ($LocalNames.Count -eq 1) {
                            Write-Debug -Message "Local name mismatch. Expected: `"$($LocalNames[0])`"; Actual: `"$($n.LocalName)`"";
                        } else {
                            $d = @($LocalNames) -join '" | "';
                            Write-Debug -Message "Local name mismatch. Expected one of: (`"$d`"); Actual: `"$($n.LocalName)`"";
                        }
                    }
                }
            }
        }
    }

    End { $Success | Write-Output }
}

Function Import-MamlParagraphs {
    Param(
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [AllowEmptyCollection()]
        [object[]]$Paragraphs,
        
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,
        
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-MshDocument })]
        [System.Xml.XmlElement]$ParentElement,
        
        [Parameter(Mandatory = $true)]
        [string]$EmptyCommentText,

        [switch]$Force
    )
    $ParagraphLines = @();
    if ($null -ne $Paragraphs) {
        $ParagraphLines = @(@($Paragraphs) | ForEach-Object {
            if ($null -eq $_ -or $_ -is [string]) { return $_ }
            return $_.Text;
        } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) });
    }
    if ($ParagraphLines.Count -eq 0) {
        if ($ParentElement.IsEmpty -or [string]::IsNullOrWhiteSpace($ParentElement.InnerText)) {
            $DescriptionElement.RemoveAll();
            $XmlElement = $DescriptionElement.PSBase.AppendChild($OwnerDocument.CreateElement('para', $Script:MamlNamespaces['maml']));
            $XmlElement.PSBase.AppendChild($OwnerDocument.CreateComment('')) | Out-Null;
        }
    } else {
        if ($null -eq $DescriptionElement) {
            $DescriptionElement = $DetailsElement.PSBase.AppendChild($OwnerDocument.CreateElement('description', $Script:MamlNamespaces['maml']));
            $Paragraphs | ForEach-Object {
                $DescriptionElement.PSBase.AppendChild($OwnerDocument.CreateElement('para', $Script:MamlNamespaces['maml'])).InnerText = $_;
            }
        } else {
            if ($Force.IsPresent -or [string]::IsNullOrWhiteSpace($DescriptionElement.InnerText)) {
                $DescriptionElement.RemoveAll();
                $Paragraphs | ForEach-Object {
                    $DescriptionElement.PSBase.AppendChild($OwnerDocument.CreateElement('para', $Script:MamlNamespaces['maml'])).InnerText = $_;
                }
            }
        }
    }
}

Function Confirm-ChildElement {
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-MshDocument })]
        [System.Xml.XmlElement]$ParentElement,
        
        [ValidateSet('msh', 'maml', 'command', 'dev')]
        [string]$Namespace,
        
        [Parameter(Mandatory = $true)]
        [string]$LocalName,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr
    )
    $XmlElement = $ParentElement.PSBase.SelectSingleNode("$($Namespace):$LocalName");
    if ($null -eq $XmlElement) {
        $DetailsElement.PSBase.AppendChild($OwnerDocument.CreateElement($LocalName, $Script:MamlNamespaces[$Namespace])) | Write-Output;
    } else {
        $XmlElement | Write-Output;
    }
}

Function Import-SyntaxItem {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [PSObject]$Item,
        
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,
        
        [ValidateScript({ $_ | Test-MshDocument -Command -Name 'syntax' })]
        [System.Xml.XmlElement]$ParentElement,
        
        [switch]$Force
    )

    Begin {
        $ImportedItems = @();
        $ExistingSyntaxItems = @(
            @($ParentElement.PSBase.SelectNodes('command:syntaxItem', $Nsmgr)) | ForEach-Object {
                $ParameterObjects = @(($_.PSBase.SelectNodes('command:parameter', $Nsmgr)) | ForEach-Object {
                    $n = $_.PSBase.SelectSingleNode('maml:name', $Nsmgr);
                    $t = $_.PSBase.SelectSingleNode('command:parameterValue', $Nsmgr);
                    if ($null -eq $t -or $t.IsEmpty) { $t = $_.PSBase.SelectSingleNode('dev:type/maml:name', $Nsmgr) }
                    $s = '';
                    if ($null -ne $n -and -not $n.IsEmpty) { $s = $n.PSBase.InnerText }
                    $pv = 0;
                    $ps = ('' + $_.position).Trim();
                    if ($ps.Length -eq 0 -or $ps -eq 'Named' -or -not [int]::TryParse($ps, [ref]$pv)) { $pv = [int]::MinValue }
                    $r = ('' + $_.required) -eq $true;
                    $v = '';
                    if ($null -ne $t -and -not $t.IsEmpty) { $v = $t.PSBase.InnerText.Trim() }
                    New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                        Name = $s;
                        Type = $v;
                        Required = ('' + $_.required) -eq $true;
                        Position = $pv;
                        Element = $_;
                    };
                });
                if ($ParameterObjects.Count -gt 1) {
                    $Index = -1;
                    ($ParameterObjects | Where-Object { $_.Position -gt [int]::MinValue } | Sort-Object -Property 'Position') | ForEach-Object {
                        $_ | Add-Member -MemberType NoteProperty -Name 'Order' -Value (++$Index);
                    }
                    $ParameterObjects | Where-Object { $_.Position -eq [int]::MinValue } | ForEach-Object {
                        $_ | Add-Member -MemberType NoteProperty -Name 'Order' -Value (++$Index);
                    }
                    $ParameterObjects = @($ParameterObjects | Sort-Object -Property 'Order');
                }
                $ParameterObjects = @(@($ParameterObjects | Sort-Object -Property 'Order') | ForEach-Object {
                    if ($_.Required) {
                        if ($_.Postion -gt [int]::MinValue) {
                            $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "[-$($_.Name)] <$($_.Type)>" -PassThru;
                        } else {
                            $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "-$($_.Name) <$($_.Type)>" -PassThru;
                        }
                    } else {
                        if ($_.Postion -gt [int]::MinValue) {
                            $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "[[-$($_.Name)] <$($_.Type)>]" -PassThru;
                        } else {
                            $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "[-$($_.Name) <$($_.Type)>]" -PassThru;
                        }
                    }
                });
                New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                    ParameterElements = $ParameterObjects | Select-Object -ExpandProperty 'Element';
                    Signature = @($ParameterObjects | Select-Object -ExpandProperty 'Signature') -join ', ';
                    Element = $_;
                };
            }
        );
    }

    Process {
        $ParameterObjects = @($Item.parameter | ForEach-Object {
            $s = ('' + $_.name).trim();
            $t = ('' + $_.parameterValue).trim();
            $pv = 0;
            $ps = ('' + $_.position).Trim();
            if ($ps.Length -eq 0 -or $ps -eq 'Named' -or -not [int]::TryParse($ps, [ref]$pv)) { $pv = [int]::MinValue }
            $r = ('' + $_.required) -eq 'true';
            New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                Name = $s;
                Type = $t;
                Required = ('' + $_.required) -eq $true;
                Position = $pv;
                Element = $_;
            };
        });
        if ($ParameterObjects.Count -gt 1) {
            $Index = -1;
            ($ParameterObjects | Where-Object { $_.Position -gt [int]::MinValue } | Sort-Object -Property 'Position') | ForEach-Object {
                $_ | Add-Member -MemberType NoteProperty -Name 'Order' -Value (++$Index);
            }
            $ParameterObjects | Where-Object { $_.Position -eq [int]::MinValue } | ForEach-Object {
                $_ | Add-Member -MemberType NoteProperty -Name 'Order' -Value (++$Index);
            }
            $ParameterObjects = @($ParameterObjects | Sort-Object -Property 'Order');
        }
        $ParameterObjects = @(@($ParameterObjects | Sort-Object -Property 'Order') | ForEach-Object {
            if ($_.Required) {
                if ($_.Postion -gt [int]::MinValue) {
                    $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "[-$($_.Name)] <$($_.Type)>" -PassThru;
                } else {
                    $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "-$($_.Name) <$($_.Type)>" -PassThru;
                }
            } else {
                if ($_.Postion -gt [int]::MinValue) {
                    $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "[[-$($_.Name)] <$($_.Type)>]" -PassThru;
                } else {
                    $_ | Add-Member -MemberType NoteProperty -Name 'Signature' -Value "[-$($_.Name) <$($_.Type)>]" -PassThru;
                }
            }
        });
        $ImportedItems += @(
            New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                Signature = @($ParameterObjects | Select-Object -ExpandProperty 'Signature') -join ', ';
                SyntaxItem = $Item;
                Matching = @($ExistingSyntaxItems | Where-Object { $_.Signature -eq $Signature });
            }
        );
    }

    End {
        $ItemElement = Confirm-ChildElement -ParentElement $SyntaxElement -Namespace 'command' -LocalName 'syntaxItem';
        (Confirm-ChildElement -ParentElement $ItemElement -Namespace 'maml' -LocalName 'name').PSBase.InnerText = $Name;
        $ParameterElement = Confirm-ChildElement -ParentElement $ItemElement -Namespace 'command' -LocalName 'parameter';
        $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('required')).Value = 'true';
        $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('variableLength')).Value = 'true';
        $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('globbing')).Value = 'false';
        $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('pipelineInput')).Value = 'False';
        $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('position')).Value = '0';
        (Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'maml' -LocalName 'name').PSBase.AppendChild(
            $OwnerDocument.CreateCDataSection(' Parameter name goes here ')
        ) | Out-Null;
        (Confirm-ChildElement -ParentElement (
            Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'maml' -LocalName 'description'
        ) -Namespace 'maml' -LocalName 'para').PSBase.AppendChild(
            $OwnerDocument.CreateCDataSection(' Parameter description goes here ')
        ) | Out-Null;
        $XmlElement = Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'command' -LocalName 'parameterValue';
        $XmlElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('required')).Value = 'true';
        $XmlElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('variableLength')).Value = 'false';
        $XmlElement.PSBase.AppendChild(
            $OwnerDocument.CreateCDataSection(' Parameter type goes here ')
        ) | Out-Null;
        $XmlElement = Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'dev' -LocalName 'type';
        (Confirm-ChildElement -ParentElement $XmlElement -Namespace 'maml' -LocalName 'name').PSBase.AppendChild(
            $OwnerDocument.CreateCDataSection(' Parameter type name goes here ')
        ) | Out-Null;
        (Confirm-ChildElement -ParentElement $XmlElement -Namespace 'maml' -LocalName 'uri') | Out-Null;
        (Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'dev' -LocalName 'defaultValue').PSBase.InnerText = 'None';
    }

}

$HelpObject = Get-Help -Name 'Get-ChildItem' -Full

$HelpObject.Syntax.syntaxItem[0].parameter[0].required
<#
$HelpObject.Syntax.syntaxItem[0] | Get-Member


TypeName: ExtendedCmdletHelpInfo#syntaxItem

Name                     MemberType   Definition
----                     ----------   ----------
CommonParameters         NoteProperty bool CommonParameters=True
name                     NoteProperty string name=Export-Xml
parameter                NoteProperty Object[] parameter=System.Object[]
WorkflowCommonParameters NoteProperty bool WorkflowCommonParameters=False
#>

$HelpObject = Get-Help 'Select-Object' -Full;
<#
$HelpObject.Syntax.syntaxItem[0].parameter[0].parameterValue


TypeName: ExtendedCmdletHelpInfo#parameter

Name             MemberType   Definition
----             ----------   
aliases          NoteProperty string aliases=None
description      NoteProperty string description=Export the XML to a file at the specified path. If the parent directory does not exist, it will not be created a...
isDynamic        NoteProperty string isDynamic=false
name             NoteProperty string name=Path
parameterSetName NoteProperty string parameterSetName=Single:Path:Object, Multi:Path:Object
parameterValue   NoteProperty System.String parameterValue=string
pipelineInput    NoteProperty string pipelineInput=false
position         NoteProperty string position=0
required         NoteProperty string required=true
type             NoteProperty ExtendedCmdletHelpInfo#type type=@{name=string}
#>

<#
$HelpObject.Syntax.syntaxItem[0].parameter[0].type | Get-Member


TypeName: ExtendedCmdletHelpInfo#type

Name        MemberType   Definition
----        ----------   ----------
name        NoteProperty string name=string
#>


Function Import-HelpInfo {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,
        
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,
        
        [ValidateScript({ $_ | Test-MshDocument -Document })]
        [System.Xml.XmlNode]$Msh,
        
        [switch]$Force
    )

    [xml]$MshDocument = @'
<?xml version="1.0" encoding="utf-8" ?>
<helpItems xmlns="http://msh" schema="maml"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://schemas.microsoft.com/maml/2004/10 file:///C:/Users/lerwi/Git/PowerShell/src/Schemas/PSMaml/Maml.xsd http://schemas.microsoft.com/maml/dev/command/2004/10 C:\Users\lerwi\Git\PowerShell\src\Schemas\PSMaml\developerCommand.xsd http://schemas.microsoft.com/maml/dev/2004/10 C:\Users\lerwi\Git\PowerShell\src\Schemas\PSMaml\developer.xsd">
    <command:command xmlns:maml="http://schemas.microsoft.com/maml/2004/10"
        xmlns:command="http://schemas.microsoft.com/maml/dev/command/2004/10"
        xmlns:dev="http://schemas.microsoft.com/maml/dev/2004/10" />
</helpItems>
'@;
    $HelpObject = Get-Help -Name 'Export-Xml' -Full;
    if ($null -eq $HelpObject) {
        Write-Warning -Message "Command $Name not found.";
        return;
    }

    [Xml]$OwnerDocument = $Msh.OwnerDocument;
    [System.Xml.XmlElement]$HelpItemsElement = $null;
    if ($Msh -is [System.Xml.XmlElement]) { $HelpItemsElement = $Msh } else { $HelpItemsElement = $Msh.DocumentElement }
    $Name = $HelpObject.Name;
    $Noun = "$($HelpObject.details.noun)";
    $Verb = "$(HelpObject.details.verb)";
    if ([string]::IsNullOrWhiteSpace($Name)) {
        $Name = $HelpObject.details.name;
        if ([string]::IsNullOrWhiteSpace($Name)) {
            $Name = "$($Verb)-$($Noun)";
            if ($Name.EndsWith('-')) {
                $Name = $Name.Substring(0, $Name.Length - 1);
            } else {
                if ($Name.StartsWith('-')) { $Name = $Name.Substring(1) }
            }
        }
    }
    if ([string]::IsNullOrWhiteSpace($Verb) -and [string]::IsNullOrWhiteSpace($Noun)) {
        ($Verb, $Noun) = $Name.Split('-', 2);
        if ([string]::IsNullOrWhiteSpace($Noun)) {
            $Noun = $Verb;
            $Verb = '';
        }
    }
    $CommandElement = $null;
    $DetailsElement = @($HelpItemsElement.PSBase.SelectNodes("command:command/command:details/command:name", $Nsmgr)) | Where-Object {
        $_.PSBase.InnerText -eq $HelpObject.Name
    } | Select-Object -First 1;
    if ($null -eq $DetailsElement -and $null -eq ($DetailsElement = @($HelpItemsElement.PSBase.SelectNodes("command:command/command:details", $Nsmgr)) | Where-Object {
            $v = @($_.PSBase.SelectNodes('command:verb', $Nsmbr)) | % { $_.InnerText } | ? { -not [string]::IsNullOrEmpty($_) } | select -First 1;
            if ($v -eq $Verb) {
                $v = @($_.PSBase.SelectNodes('command:noun', $Nsmbr)) | % { $_.InnerText } | ? { -not [string]::IsNullOrEmpty($_) } | select -First 1;
                ($v -eq $Noun) | Write-Output;
            } else {
                $false | Write-Output;
            }
        } | Select-Object -First 1)) {
        $CommandElement = $HelpItemsElement.PSBase.AppendChild($OwnerDocument.CreateElement('command', 'command', $Script:MamlNamespaces['command']));
        $DetailsElement = $CommandElement.PSBase.AppendChild($OwnerDocument.CreateElement('details', $Script:MamlNamespaces['command']));
    } else {
        if ($DetailsElement.PSBase.LocalName -ne 'details') { $DetailsElement = $DetailsElement.PSBase.ParentNode }
        $CommandElement = $DetailsElement.PSBase.ParentNode;
    }
    (Confirm-ChildElement -ParentElement $DetailsElement -Namespace 'command' -LocalName 'name').PSBase.InnerText = $Name;
    (Confirm-ChildElement -ParentElement $DetailsElement -Namespace 'command' -LocalName 'verb').PSBase.InnerText = $Verb;
    (Confirm-ChildElement -ParentElement $DetailsElement -Namespace 'command' -LocalName 'noun').PSBase.InnerText = $Noun;
    $XmlElement = Confirm-ChildElement -ParentElement $DetailsElement -Namespace 'maml' -LocalName 'description';
    if ($Force.IsPresent) {
        Import-MamlParagraphs -Paragraphs $HelpObject.details.description -Nsmgr $Nsmgr -ParentElement $XmlElement -EmptyCommentText ' Put summary here ' -Force;
    } else {
        Import-MamlParagraphs -Paragraphs $HelpObject.details.description -Nsmgr $Nsmgr -ParentElement $XmlElement -EmptyCommentText ' Put summary here ';
    }
    $XmlElement = Confirm-ChildElement -ParentElement $CommandElement -Namespace 'maml' -LocalName 'description';
    if ($Force.IsPresent) {
        Import-MamlParagraphs -Paragraphs $HelpObject.description -Nsmgr $Nsmgr -ParentElement $XmlElement -EmptyCommentText ' Put detailed description here ' -Force;
    } else {
        Import-MamlParagraphs -Paragraphs $HelpObject.description -Nsmgr $Nsmgr -ParentElement $XmlElement -EmptyCommentText ' Put detailed description here ';
    }
    $SyntaxElement = Confirm-ChildElement -ParentElement $CommandElement -Namespace 'command' -LocalName 'syntax';
    $SyntaxItems = @();
    if ($null -ne $HelpObject.Syntax) {
        if ($null -ne $HelpObject.Syntax.syntaxItem) {
            $SyntaxItems = @($HelpObject.Syntax.syntaxItem);
        } else {
            $SyntaxItems = @($HelpObject.Syntax);
        }
    }
    if ($SyntaxItems.Count -gt 0) {
        if ($Force.IsPresent) {
            $SyntaxItems | Import-SyntaxItem -Nsmgr $Nsmgr -ParentElement $SyntaxElement -Force;
        } else {
            $SyntaxItems | Import-SyntaxItem -Nsmgr $Nsmgr -ParentElement $SyntaxElement -Force;
        }
    } else {
        if ($null -eq $XmlElement.PSBase.SelectSingleNode('command:syntaxItem', $Nsmgr)) {
            $ItemElement = Confirm-ChildElement -ParentElement $SyntaxElement -Namespace 'command' -LocalName 'syntaxItem';
            (Confirm-ChildElement -ParentElement $ItemElement -Namespace 'maml' -LocalName 'name').PSBase.InnerText = $Name;
            $ParameterElement = Confirm-ChildElement -ParentElement $ItemElement -Namespace 'command' -LocalName 'parameter';
            $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('required')).Value = 'true';
            $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('variableLength')).Value = 'true';
            $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('globbing')).Value = 'false';
            $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('pipelineInput')).Value = 'False';
            $ParameterElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('position')).Value = '0';
            (Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'maml' -LocalName 'name').PSBase.AppendChild(
                $OwnerDocument.CreateCDataSection(' Parameter name goes here ')
            ) | Out-Null;
            (Confirm-ChildElement -ParentElement (
                Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'maml' -LocalName 'description'
            ) -Namespace 'maml' -LocalName 'para').PSBase.AppendChild(
                $OwnerDocument.CreateCDataSection(' Parameter description goes here ')
            ) | Out-Null;
            $XmlElement = Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'command' -LocalName 'parameterValue';
            $XmlElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('required')).Value = 'true';
            $XmlElement.PSBase.Attributes.Append($OwnerDocument.CreateAttribute('variableLength')).Value = 'false';
            $XmlElement.PSBase.AppendChild(
                $OwnerDocument.CreateCDataSection(' Parameter type goes here ')
            ) | Out-Null;
            $XmlElement = Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'dev' -LocalName 'type';
            (Confirm-ChildElement -ParentElement $XmlElement -Namespace 'maml' -LocalName 'name').PSBase.AppendChild(
                $OwnerDocument.CreateCDataSection(' Parameter type name goes here ')
            ) | Out-Null;
            (Confirm-ChildElement -ParentElement $XmlElement -Namespace 'maml' -LocalName 'uri') | Out-Null;
            (Confirm-ChildElement -ParentElement $ParameterElement -Namespace 'dev' -LocalName 'defaultValue').PSBase.InnerText = 'None';
        }
    }
}






<#
(,$HelpObject) | Get-Member


TypeName: ExtendedCmdletHelpInfo#FullView

Name                     MemberType   Definition
----                     ----------   ----------
alertSet                 NoteProperty object alertSet=null
aliases                  NoteProperty string aliases=None...
Category                 NoteProperty string Category=Cmdlet
CommonParameters         NoteProperty bool CommonParameters=True
Component                NoteProperty object Component=null
description              NoteProperty psobject[] description=System.Management.Automation.PSObject[]
details                  NoteProperty ExtendedCmdletHelpInfo#details details=@{name=Export-Xml; noun=Xml; verb=Export}
examples                 NoteProperty object examples=null
Functionality            NoteProperty object Functionality=null
inputTypes               NoteProperty ExtendedCmdletHelpInfo#inputTypes inputTypes=@{inputType=}
ModuleName               NoteProperty string ModuleName=DevHelper
Name                     NoteProperty string Name=Export-Xml
nonTerminatingErrors     NoteProperty string nonTerminatingErrors=
parameters               NoteProperty ExtendedCmdletHelpInfo#parameters parameters=@{parameter=System.Object[]}
PSSnapIn                 NoteProperty object PSSnapIn=null
remarks                  NoteProperty string remarks=None
returnValues             NoteProperty ExtendedCmdletHelpInfo#returnValues returnValues=@{returnValue=}
Role                     NoteProperty object Role=null
Synopsis                 NoteProperty string Synopsis=...
Syntax                   NoteProperty ExtendedCmdletHelpInfo#syntax Syntax=@{syntaxItem=System.Object[]}
WorkflowCommonParameters NoteProperty bool WorkflowCommonParameters=False
xmlns:command            NoteProperty string xmlns:command=http://schemas.microsoft.com/maml/dev/command/2004/10
xmlns:dev                NoteProperty string xmlns:dev=http://schemas.microsoft.com/maml/dev/2004/10
xmlns:maml               NoteProperty string xmlns:maml=http://schemas.microsoft.com/maml/2004/10
#>

<#
$HelpObject.description[0] | get-member


TypeName: MamlParaTextItem

Name        MemberType   Definition
----        ----------   ----------
Text        NoteProperty string Text=The Get-Command cmdlet gets all commands that are installed on the computer, including cmdlets, aliases, functions, workflow...
#>

<#
$HelpObject.Synopsis

Export-Xml [-Path] <string> -InputNode <XmlNode[]> [-Settings <XmlWriterSettings>] [-Force] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml [-Xml] <XmlNode> [-Path] <string> [-Settings <XmlWriterSettings>] [-Force] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml [-Xml] <XmlNode> -FileInfo <FileInfo> [-Settings <XmlWriterSettings>] [-Force] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml [-Xml] <XmlNode> -AsSingleString [-Settings <XmlWriterSettings>] [-Force] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml [-Xml] <XmlNode> -Stream <Stream> [-Settings <XmlWriterSettings>] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-CloseOutput] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml [-Xml] <XmlNode> -TextWriter <TextWriter> [-Settings <XmlWriterSettings>] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-CloseOutput] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml -Xml <XmlNode> -XmlWriter <XmlWriter> [<CommonParameters>]

Export-Xml -InputNode <XmlNode[]> -Stream <Stream> [-Settings <XmlWriterSettings>] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-CloseOutput] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml -InputNode <XmlNode[]> -TextWriter <TextWriter> [-Settings <XmlWriterSettings>] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-CloseOutput] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml -InputNode <XmlNode[]> -AsSingleString [-Settings <XmlWriterSettings>] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml -InputNode <XmlNode[]> -PathFactory <scriptblock> [-Settings <XmlWriterSettings>] [-Force] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml -InputNode <XmlNode[]> -FileInfo <FileInfo> [-Settings <XmlWriterSettings>] [-Force] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

Export-Xml -InputNode <XmlNode[]> -AsStrings [-Settings <XmlWriterSettings>] [-OmitXmlDeclaration] [-NewLineHandling <NewLineHandling>] [-NewLineChars <string>] [-OmitDuplicateNamespaces] [-Encoding <Encoding>] [-DoNotEscapeUriAttributes] [-ConformanceLevel <ConformanceLevel>] [-NoCheckCharacters] [-NoAutoCloseTag] [-Indent] [<CommonParameters>]

#>

<#
$HelpObject.details | Get-Member
$ho.details

TypeName: ExtendedCmdletHelpInfo#details

Name        MemberType   Definition
----        ----------   ----------
name        NoteProperty string name=Export-Xml
noun        NoteProperty string noun=Xml
verb        NoteProperty string verb=Export
#>

<#
$HelpObject.inputTypes | Get-Member


TypeName: ExtendedCmdletHelpInfo#inputTypes

Name        MemberType   Definition
----        ----------   ----------
inputType   NoteProperty ExtendedCmdletHelpInfo#inputType inputType=@{type=}
#>

<#
$HelpObject.inputTypes.inputType | Get-Member


TypeName: ExtendedCmdletHelpInfo#inputType

Name        MemberType   Definition
----        ----------   ----------
type        NoteProperty ExtendedCmdletHelpInfo#type type=@{name=None...
#>

<#
$HelpObject.inputTypes.inputType.type | Get-Member


TypeName: ExtendedCmdletHelpInfo#type

Name        MemberType   Definition
----        ----------   ----------
name        NoteProperty string name=None...
#>

<#
$HelpObject.parameters | Get-Member


TypeName: ExtendedCmdletHelpInfo#parameters

Name        MemberType   Definition
----        ----------   ----------
parameter   NoteProperty Object[] parameter=System.Object[]
#>

<#
$HelpObject.parameters.parameter[0] | Get-Member


TypeName: ExtendedCmdletHelpInfo#parameter

Name             MemberType   Definition
----             ----------   ----------
aliases          NoteProperty string aliases=None
description      NoteProperty string description=Return the exported XML as a string. If multiple XML nodes are provided, the resulting string will be an xml fra...
isDynamic        NoteProperty string isDynamic=false
name             NoteProperty string name=AsSingleString
parameterSetName NoteProperty string parameterSetName=Single:String:Object, Multi:String:Object
pipelineInput    NoteProperty string pipelineInput=false
position         NoteProperty string position=Named
required         NoteProperty string required=true
type             NoteProperty ExtendedCmdletHelpInfo#type type=@{name=switch}
#>

<#
$HelpObject.parameters.parameter[0].type | Get-Member


TypeName: ExtendedCmdletHelpInfo#type

Name        MemberType   Definition
----        ----------   ----------
name        NoteProperty string name=switch
#>

<#
$HelpObject.returnValues | Get-Member


TypeName: ExtendedCmdletHelpInfo#returnValues

Name        MemberType   Definition
----        ----------   ----------
Equals      Method       bool Equals(System.Object obj)
GetHashCode Method       int GetHashCode()
GetType     Method       type GetType()
ToString    Method       string ToString()
returnValue NoteProperty ExtendedCmdletHelpInfo#returnValue returnValue=@{type=}
#>

<#
$HelpObject.returnValues.returnValue | Get-Member

Name        MemberType   Definition
----        ----------   ----------
Equals      Method       bool Equals(System.Object obj)
GetHashCode Method       int GetHashCode()
GetType     Method       type GetType()
ToString    Method       string ToString()
type        NoteProperty ExtendedCmdletHelpInfo#type type=@{name=System.Collections.Hashtable[]...
#>

<#
$HelpObject.returnValues.returnValue.type | Get-Member

Name        MemberType   Definition
----        ----------   ----------
Equals      Method       bool Equals(System.Object obj)
GetHashCode Method       int GetHashCode()
GetType     Method       type GetType()
ToString    Method       string ToString()
name        NoteProperty string name=System.Collections.Hashtable[...
#>

<#
$HelpObject.Syntax | Get-Member


TypeName: ExtendedCmdletHelpInfo#syntax

Name        MemberType   Definition
----        ----------   ----------
syntaxItem  NoteProperty Object[] syntaxItem=System.Object[]
#>