Param(
    [string]$ProjectPath = 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\FsInfoCat.Desktop.csproj'
)

enum ErrorIds {
    ValueIsNull;
    KeyInvalid;
    NameInvalid;
    LocalNameInvalid;
    NamespaceInvalid;
    InvalidDocumentElement;
}

enum XmlNCName {
    __any__;
    msb;
    Project;
    Condition;
    PropertyGroup;
    ItemGroup;
    ProjectGuid;
    Reference;
    ProjectReference;
    Include;
    Exclude;
    SpecificVersion;
    HintPath;
    RequiredTargetFramework;
    AutoGen;
    DesignTime;
    DependentUpon;
    DesignTimeSharedInput;
    Generator;
    LastGenOutput;
    SubType;
    Name;
}

Function Write-ThrownError {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [object]$Thrown,
        [Parameter(Mandatory = $true)]
        [ErrorIds]$Id,
        [Parameter(Mandatory = $true)]
        [System.Management.Automation.ErrorCategory]$Category,
        [string]$Message,
        [string]$CategoryActivity,
        [string]$CategoryReason,
        [string]$CategoryTargetName,
        [string]$CategoryTargetType,
        [string]$RecommendedAction,
        [object]$TargetObject
    )
    $Splat = @{ ErrorId = $ErrorId.ToString(); Category = $Category };
    if ($PSBoundParameters.ContainsKey('Message')) { $Splat['Message'] = $Message }
    if ($PSBoundParameters.ContainsKey('CategoryActivity')) { $Splat['CategoryActivity'] = $CategoryActivity }
    if ($PSBoundParameters.ContainsKey('CategoryReason')) { $Splat['CategoryReason'] = $CategoryReason }
    if ($PSBoundParameters.ContainsKey('CategoryTargetName')) { $Splat['CategoryTargetName'] = $CategoryTargetName }
    if ($PSBoundParameters.ContainsKey('CategoryTargetType')) { $Splat['CategoryTargetType'] = $CategoryTargetType }
    if ($PSBoundParameters.ContainsKey('RecommendedAction')) { $Splat['RecommendedAction'] = $RecommendedAction }
    if ($PSBoundParameters.ContainsKey('TargetObject')) { $Splat['TargetObject'] = $TargetObject }
    [System.Management.Automation.ErrorRecord]$ErrorRecord = $null;
    if ($Thrown -is [System.Exception]) {
        $Splat['Exception'] = $Thrown;
        if ($Source -is [System.Management.Automation.IContainsErrorRecord]) { $ErrorRecord = $Source.ErrorRecord }
    } else {
        if ($Thrown -is [string]) {
            if ($Thrown.Trim().Length -gt 0) {
                if ($PSBoundParameters.ContainsKey('Message')) {
                    $Splat['Message'] = "$Thrown`n$Message";
                } else {
                    $Splat['Message'] = $Thrown;
                }
            }
        } else {
            if ($Thrown -is [System.Management.Automation.ErrorRecord]) {
                $ErrorRecord = $Source;
                $Splat['Exception'] = $Thrown.Exception;
            } else {
                if (-not $Splat.ContainsKey('TargetObject')) { $Splat['TargetObject'] = $Thrown }
            }
        }
    }
    if ($null -ne $ErrorRecord) {
        if (-not ([string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.Activity) -or $PSBoundParameters.ContainsKey('CategoryActivity'))) {
            $Splat['CategoryActivity'] = $ErrorRecord.CategoryInfo.Activity;
        }
        if (-not ([string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.Reason) -or $PSBoundParameters.ContainsKey('CategoryReason'))) {
            $Splat['CategoryReason'] = $ErrorRecord.CategoryInfo.Reason;
        }
        if (-not ($PSBoundParameters.ContainsKey('CategoryTargetName') -or $PSBoundParameters.ContainsKey('CategoryTargetType') -or $PSBoundParameters.ContainsKey('TargetObject'))) {
            if ($ErrorRecord.CategoryInfo.TargetName.Trim().Length -gt 0) { $Splat['CategoryTargetName'] = $ErrorRecord.CategoryInfo.TargetName }
            if ($ErrorRecord.CategoryInfo.TargetType.Trim().Length -gt 0) { $Splat['CategoryTargetType'] = $ErrorRecord.CategoryInfo.TargetType }
            if ($null -ne $ErrorRecord.TargetObject) { $Splat['TargetObject'] = $ErrorRecord.TargetObject }
        }
        if (-not ($Splat.ContainsKey('Message') -or $null -eq $ErrorRecord.ErrorDetails)) {
            if (-not ([string]::IsNullOrWhiteSpace($ErrorRecord.ErrorDetails.RecommendedAction) -or $PSBoundParameters.ContainsKey('RecommendedAction'))) {
                $Splat['RecommendedAction'] = $ErrorRecord.ErrorDetails.RecommendedAction;
            }
            if (-not ([string]::IsNullOrWhiteSpace($ErrorRecord.ErrorDetails.Message) -or $Splat.ContainsKey('Message'))) {
                $Splat['Message'] = $ErrorRecord.ErrorDetails.Message;
            }
        }
    }
    Write-Error @Splat;
}

Function Test-NamespaceURI {
    [OutputType([bool])]
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [string]$URI
    )

    Begin { $Success = $true }

    Process {
        if ($Success -and $URI.Length -gt 0 -and -not [Uri]::IsWellFormedUriString($URI, [UriKind]::Absolute)) {
            $Success = $false;
        }
    }

    End { $Success | Write-Output }
}

class XmlQualifiedName {
    hidden static [string]$DefaultNamespaceURI = 'http://schemas.microsoft.com/developer/msbuild/2003';
    hidden static [System.Text.RegularExpressions.Regex]$QNameRegex = [System.Text.RegularExpressions.Regex]::new(
        '^\{\s*"(?<ns>[^"]+)"\s*,\s*"(?<ln>[^"]+)?"\s*\}$',
        [System.Text.RegularExpressions.RegexOptions]::Compiled
    );

    hidden [string]$LocalName;
    hidden [string]$NamespaceURI;

    XmlQualifiedName([string]$Name) {
        if ($null -eq $Name) {
            Write-Error -Message 'Name cannot be null' -Category InvalidArgument -ErrorId ([ErrorIds]::LocalNameInvalid) -TargetObject $Name -ErrorAction Stop;
        }
        $m = [XmlQualifiedName]::QNameRegex.Match($Name);
        if ($m.Success) {
            $this.LocalName = $m.Groups['ln'].Value;
            if ($m.Groups['ns'].Success) {
                $this.NamespaceURI = $m.Groups['ns'].Value;
            } else {
                $this.NamespaceURI = '';
            }
        } else {
            $this.LocalName = $Name;
            $this.NamespaceURI = '';
        }
        try { [System.Xml.XmlConvert]::VerifyNCName($this.LocalName) | Out-Null }
        catch { Write-ThrownError -Thrown $_ -Message 'Local name is invalid' -Category InvalidArgument -ErrorId LocalNameInvalid -TargetObject $Name -ErrorAction Stop; }
        if (-not ($this.NamespaceURI | Test-NamespaceURI)) {
            Write-Error -Message 'Invalid namespace URI' -Category InvalidArgument -ErrorId ([ErrorIds]::NamespaceInvalid) -TargetObject $Name -ErrorAction Stop;
        }
    }

    XmlQualifiedName([string]$LocalName, [string]$NamespaceURI) {
        try { [System.Xml.XmlConvert]::VerifyNCName($LocalName) | Out-Null }
        catch { Write-ThrownError -Thrown $_ -Message 'Local name is invalid' -Category InvalidArgument -ErrorId LocalNameInvalid -TargetObject $LocalName -ErrorAction Stop; }
        if ($null -eq $NamespaceURI) {
            $this.NamespaceURI = [XmlQualifiedName]::DefaultNamespaceURI;
        } else {
            if (-not ($NamespaceURI | Test-NamespaceURI)) {
                Write-Error -Message 'Invalid namespace URI' -Category InvalidArgument -ErrorId ([ErrorIds]::NamespaceInvalid) -TargetObject $NamespaceURI -ErrorAction Stop;
            }
            $this.LocalName = $LocalName;
            $this.NamespaceURI = $NamespaceURI;
        }
    }

    [string] ToString() {
        if ($this.NamespaceURI.Length -eq 0) { return $this.LocalName }
        return "{`"$($this.NamespaceURI)`", `"$($this.LocalName)`"}";
    }
    
    [bool] Equals([object]$obj) {
        if ($null -eq $obj) { return $false }
        if ($obj -is [string]) { return $this.ToString() -ceq $obj }
        return $obj -is [XmlQualifiedName] -and $this.LocalName -ceq $obj.LocalName -and $this.NamespaceURI -ceq $obj.NamespaceURI;
    }
    
    [bool] IsMsb() { return $this.NamespaceURI -eq [XmlQualifiedName]::DefaultNamespaceURI }
    
    [bool] IsMsb([string]$LocalName) { return $this.NamespaceURI -eq [XmlQualifiedName]::DefaultNamespaceURI -and $LocalName -ceq $this.LocalName }
    
    static [bool] IsMsb([System.Xml.XmlNode]$Node, [System.Xml.XmlDocument]$OwnerDocument, [string]$LocalName) {
        return [XmlQualifiedName]::IsMsb($Node, $OwnerDocument) -and $Node.LocalName -ceq $LocalName;
    }

    static [bool] IsMsb([System.Xml.XmlNode]$Node, [System.Xml.XmlDocument]$OwnerDocument) {
        return [XmlQualifiedName]::IsMsb($Node) -and [Object]::ReferenceEquals($OwnerDocument, $Node.OwnerDocument);
    }

    static [bool] IsMsb([System.Xml.XmlNode]$Node, [string]$LocalName) {
        return [XmlQualifiedName]::IsMsb($Node) -and $Node.LocalName -ceq $LocalName;
    }

    static [bool] IsMsb([System.Xml.XmlNode]$Node) {
        return $null -ne $Node -and $Node.NamespaceURI -ceq [XmlQualifiedName]::DefaultNamespaceURI;
    }

    static [XmlQualifiedName] Create([System.Xml.XmlNode]$Node) {
        if ($null -eq $Node) {
            Write-Error -Message 'Node cannot be null' -Category InvalidArgument -ErrorId ([ErrorIds]::ValueIsNull) -ErrorAction Stop;
        }
        return (New-Object -TypeName 'XmlQualifiedName' -ArgumentList $Node.LocalName, $Node.NamespaceURI -ErrorAction Stop);
    }
    
    static [System.Xml.XmlElement] CreateMsbElement([System.Xml.XmlDocument]$OwnerDocument, [XmlNCName]$LocalName) {
        if ($LocalName -eq [XmlNCName]::__any__) {
            Write-Error -Message 'Invalid name' -Category InvalidArgument -ErrorId ([ErrorIds]::LocalNameInvalid) -ErrorAction Stop;
        }
        return $OwnerDocument.CreateElement($LocalName, [XmlQualifiedName]::DefaultNamespaceURI);
    }

    static [System.Xml.XmlElement] CreateMsbElement([System.Xml.XmlDocument]$OwnerDocument, [string]$LocalName) {
        return $OwnerDocument.CreateElement($LocalName, [XmlQualifiedName]::DefaultNamespaceURI);
    }

    static [System.Collections.ObjectModel.Collection[System.Xml.XmlElement]] GetMsbElements([System.Xml.XmlNode]$ParentNode) {
        $MsbElements = [System.Collections.ObjectModel.Collection[System.Xml.XmlElement]]::new();
        if ($ParentNode -is [System.Xml.XmlDocument]) { $ParentNode = $ParentNode.DocumentElement }
        if ($null -ne $ParentNode) {
            $nsmgr = $null;
            if ($ParentNode.OwnerDocument -is [MsbProject]) {
                $nsmgr = $ParentNode.OwnerDocument.nsmgr;
            } else {
                $nsmgr = [System.Xml.XmlNamespaceManager]::new($ParentNode.OwnerDocument.NameTable);
                $nsmgr.AddNamespace([XmlNCName]::msb, [XmlQualifiedName]::DefaultNamespaceURI);
            }
            @($ParentNode.SelectNodes("$([XmlNCName]::msb):*", $nsmgr)) | ForEach-Object { $MsbElements.Add($_) }
        }
        return $MsbElements;
    }

    static [System.Collections.ObjectModel.Collection[System.Xml.XmlElement]] GetMsbElements([System.Xml.XmlNode]$ParentNode, [XmlNCName[]]$Name) {
        $MsbElements = [System.Collections.ObjectModel.Collection[System.Xml.XmlElement]]::new();
        if ($ParentNode -is [System.Xml.XmlDocument]) { $ParentNode = $ParentNode.DocumentElement }
        if ($null -ne $ParentNode) {
            $nsmgr = $null;
            if ($ParentNode.OwnerDocument -is [MsbProject]) {
                $nsmgr = $ParentNode.OwnerDocument.nsmgr;
            } else {
                $nsmgr = [System.Xml.XmlNamespaceManager]::new($ParentNode.OwnerDocument.NameTable);
                $nsmgr.AddNamespace([XmlNCName]::msb, [XmlQualifiedName]::DefaultNamespaceURI);
            }
            if ($null -eq $Name -or $Name.Length -eq 0) {
                @($ParentNode.SelectNodes("$([XmlNCName]::msb):*", $nsmgr)) | ForEach-Object { $MsbElements.Add($_) }
            } else {
                @($ParentNode.SelectNodes((($Name | ForEach-Object {
                    if ($_ -eq [XmlNCName]::__any__) { "$([XmlNCName]::msb):*" } else { "$([XmlNCName]::msb):$_" }
                }) -join '/'), $nsmgr)) | ForEach-Object { $MsbElements.Add($_) }
            }
        }
        return $MsbElements;
    }

    static [System.Xml.XmlElement] GetFirstMsbElement([System.Xml.XmlNode]$ParentNode, [XmlNCName]$Name) {
        return [XmlQualifiedName]::GetFirstMsbElement($ParentNode, [XmlNCName[]]@($Name));
    }

    static [System.Xml.XmlElement] GetFirstMsbElement([System.Xml.XmlNode]$ParentNode, [XmlNCName[]]$Name) {
        if ($ParentNode -is [System.Xml.XmlDocument]) { $ParentNode = $ParentNode.DocumentElement }
        if ($null -eq $ParentNode) { return $null }
        $nsmgr = $null;
        if ($ParentNode.OwnerDocument -is [MsbProject]) {
            $nsmgr = $ParentNode.OwnerDocument.nsmgr;
        } else {
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($ParentNode.OwnerDocument.NameTable);
            $nsmgr.AddNamespace([XmlNCName]::msb, [XmlQualifiedName]::DefaultNamespaceURI);
        }
        if ($null -eq $Name -or $Name.Length -eq 0) {
            return $ParentNode.SelectSingleNode("$([XmlNCName]::msb):*", $nsmgr);
        }
        return $ParentNode.SelectSingleNode((($Name | ForEach-Object {
            if ($_ -eq [XmlNCName]::__any__) { "$([XmlNCName]::msb):*" } else { "$([XmlNCName]::msb):$_" }
        }) -join '/'), $nsmgr);
    }
}

class MsbProject : System.Xml.XmlDocument {
    hidden [System.Xml.XmlNamespaceManager]$Nsmgr;
    hidden [System.IO.FileInfo]$FileInfo

    MsbProject([string]$Path) {
        $this.FileInfo = New-Object -TypeName 'System.IO.FileInfo' -ArgumentList $Path -ErrorAction Stop;
        if ($this.FileInfo.Exists) {
            $XmlReader = [System.Xml.XmlReader]::Create($Path);
            try { $this.Load($XmlReader) }
            catch {
                Write-ThrownError -Thrown $_ -Id ReadError -Category ([System.Management.Automation.ErrorCategory]::ReadError) -TargetObject -ErrorAction Stop;
            } finally {
                $XmlReader.Close();
            }
            if (-not [XmlQualifiedName]::IsMsb($this.DocumentElement)) {
                Write-Error -Message 'Not a project file' -Category InvalidData -ErrorId ([ErrorIds]::InvalidDocumentElement) -TargetObject $Path -ErrorAction Stop;
            }
        } else {
            if (-not $this.FileInfo.Directory.Exists) {
                Write-Error -Message 'Parent path not found' -Category ObjectNotFound -ErrorId ([ErrorIds]::ReadError) -TargetObject $Path -ErrorAction Stop;
            }
            $this.AppendChild([XmlQualifiedName]::CreateMsbElement($this, [XmlNCName]::Project)).Attributes.Append('ToolsVersion').Value = '14.0';
            $this.DocumentElement.Attributes.Append('DefaultTargets').Value = 'Build';
        }
        $this.Nsmgr = [System.Xml.XmlNamespaceManager]::new($this.NameTable);
        $this.Nsmgr.AddNamespace([XmlNCName]::msb, [XmlQualifiedName]::DefaultNamespaceURI);
    }
}

class ProjectElementProxy {
    hidden [System.Xml.XmlElement]$ItemElement;
    [string]$Condition;
    ProjectElementProxy([System.Xml.XmlElement]$ItemElement) {
        if (-not [XmlQualifiedName]::IsMsb($ItemElement)) {
            Write-Error -Message 'Invalid element' -Category InvalidArgument -ErrorId ([ErrorIds]::InvalidDocumentElement) -TargetObject $ItemElement -ErrorAction Stop;
        }
        $this.ItemElement = $ItemElement;
        $this.Type = $ItemElement.LocalName;
        $a = $ItemElement.SelectSingleNode('@' + [XmlNCName]::Condition);
        if ($null -eq $a) { $this.Condition = '' } else { $this.Condition = $a.Value }
    }
    
    [System.Xml.XmlElement] GetXmlElement() { return $this.ItemElement }
    static [Nullable[bool]] ToBoolean([string]$Value) {
        if ($null -eq $Value -or ($Value = $Value.Trim()).ToLower().Length -eq 0) { return $null }
        $b = $null;
        try {
            $b = [System.Xml.XmlConvert]::ToBoolean($Value);
        } catch { }
        if ($null -eq $b) {
            try {
                $b = [System.Xml.XmlConvert]::ToInt32($s) -ne 0;
            } catch { $b = $null}
        }
        return $b;
    }
}

class ItemElementProxy : ProjectElementProxy {
    hidden [System.Xml.XmlElement]$ItemElement;
    [string]$Type;
    [string]$Include;
    [string]$Exclude;
    ItemElementProxy([System.Xml.XmlElement]$ItemElement) : base($ItemElement) {
        if (-not [XmlQualifiedName]::IsMsb($ItemElement.ParentNode, [XmlNCName]::ItemGroup)) {
            Write-Error -Message 'Invalid element' -Category InvalidArgument -ErrorId ([ErrorIds]::InvalidDocumentElement) -TargetObject $ItemElement -ErrorAction Stop;
        }
        $this.ItemElement = $ItemElement;
        $this.Type = $ItemElement.LocalName;
        $a = $ItemElement.SelectSingleNode('@' + [XmlNCName]::Include);
        if ($null -eq $a) { $this.Include = '' } else { $this.Include = $a.Value }
        $a = $ItemElement.SelectSingleNode('@' + [XmlNCName]::Exclude);
        if ($null -eq $a) { $this.Exclude = '' } else { $this.Exclude = $a.Value }
    }
    
    [System.Xml.XmlElement] GetGroup() { return $this.ItemElement.ParentNode }

    static [System.Xml.XmlElement] Create([System.Xml.XmlElement]$ItemElement) {
        if ($null -ne $ItemElement) {
            switch ($ItemElement.PSBase.LocalName) {
                { $_ -ceq [XmlNCName]::ProjectReference } { return New-Object -TypeName 'ProjectReferenceElementProxy' -ArgumentList $ItemElement -ErrorAction Stop; }
                { $_ -ceq [XmlNCName]::Reference } { return New-Object -TypeName 'ReferenceElementProxy' -ArgumentList $ItemElement -ErrorAction Stop; }
                default { break; }
            }
        }
        return New-Object -TypeName 'ProjectReferenceElementProxy' -ArgumentList $ItemElement -ErrorAction Stop;
    }
}

class ReferenceElementProxy : ItemElementProxy {
    [Nullable[bool]]$SpecificVersion;
    [string]$HintPath;
    [string]$RequiredTargetFramework;
    ReferenceElementProxy([System.Xml.XmlElement]$ItemElement) : base($ItemElement) {
        if ($this.Type -cne [XmlNCName]::Reference) {
            Write-Error -Message 'Invalid element' -Category InvalidArgument -ErrorId ([ErrorIds]::InvalidDocumentElement) -TargetObject $ItemElement -ErrorAction Stop;
        }
        $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::SpecificVersion);
        if ($null -eq $e -or $e.IsEmpty) {
            $this.SpecificVersion = $null;
        } else {
            $this.SpecificVersion = [ProjectElementProxy]::ToBoolean($e.InnerText);
        }
        $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::HintPath);
        if ($null -eq $e -or $e.IsEmpty) { $this.HintPath = '' } else { $this.HintPath = $e.InnerText }
        $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::RequiredTargetFramework);
        if ($null -eq $e -or $e.IsEmpty) { $this.RequiredTargetFramework = '' } else { $this.RequiredTargetFramework = $e.InnerText }
    }
}

class ProjectReferenceElementProxy : ItemElementProxy {
    [Nullable[Guid]]$Project;
    [string]$Name;
    ProjectReferenceElementProxy([System.Xml.XmlElement]$ItemElement) : base($ItemElement) {
        if ($this.Type -cne [XmlNCName]::Reference) {
            Write-Error -Message 'Invalid element' -Category InvalidArgument -ErrorId ([ErrorIds]::InvalidDocumentElement) -TargetObject $ItemElement -ErrorAction Stop;
        }
        $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::SpecificVersion);
        if ($null -eq $e -or $e.IsEmpty) {
            $this.Project = $null;
        } else {
            $Guid = [Guid]::Empty;
            if ([Guid]::TryParse($e.InnerText.Trim(), [ref]$Guid)) {
                $this.Project = $Guid;
            } else {
                $this.Project = $null;
            }
        }
        $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::Name);
        if ($null -eq $e -or $e.IsEmpty) { $this.Name = '' } else { $this.Name = $e.InnerText }
    }
}

class FileElementProxy : ItemElementProxy {
    [Nullable[bool]]$AutoGen;
    [Nullable[bool]]$DesignTime;
    [string]$DependentUpon;
    [string]$DesignTimeSharedInput;
    [string]$Generator;
    [string]$LastGenOutput;
    [string]$SubType;
    FileElementProxy([System.Xml.XmlElement]$ItemElement) : base($ItemElement) {
        switch ($ItemElement.PSBase.LocalName) {
            { $_ -ceq [XmlNCName]::ProjectReference } { break; }
            { $_ -ceq [XmlNCName]::Reference } { break; }
            default {
                $this.ItemElement = $ItemElement;
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::AutoGen);
                if ($null -eq $e -or $e.IsEmpty) {
                    $this.AutoGen = $null;
                } else {
                    $this.AutoGen = [ProjectElementProxy]::ToBoolean($e.InnerText);
                }
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::DesignTime);
                if ($null -eq $e -or $e.IsEmpty) {
                    $this.DesignTime = $null;
                } else {
                    $this.DesignTime = [ProjectElementProxy]::ToBoolean($e.InnerText);
                }
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::DependentUpon);
                if ($null -eq $e -or $e.IsEmpty) { $this.DependentUpon = '' } else { $this.DependentUpon = $e.InnerText }
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::DesignTimeSharedInput);
                if ($null -eq $e -or $e.IsEmpty) { $this.DesignTimeSharedInput = '' } else { $this.DesignTimeSharedInput = $e.InnerText }
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::Generator);
                if ($null -eq $e -or $e.IsEmpty) { $this.Generator = '' } else { $this.Generator = $e.InnerText }
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::LastGenOutput);
                if ($null -eq $e -or $e.IsEmpty) { $this.LastGenOutput = '' } else { $this.LastGenOutput = $e.InnerText }
                $e = [XmlQualifiedName]::GetFirstMsbElement($ItemElement, [XmlNCName]::SubType);
                if ($null -eq $e -or $e.IsEmpty) { $this.SubType = '' } else { $this.SubType = $e.InnerText }
                return;
            }
        }
        Write-Error -Message 'Invalid file item element name' -Category InvalidArgument -ErrorId ([ErrorIds]::InvalidDocumentElement) -TargetObject $ItemElement -ErrorAction Stop;
    }
    
}

$ProjectXml = [MsbProject]::new($ProjectPath);
$ProjectDirectory = $ProjectXml.FileInfo.Directory;
$ProjectXml.FileInfo.Exists
$SubIdx = $ProjectDirectory.FullName.Length + 1;

$FileElements = [XmlQualifiedName]::GetMsbElements($ProjectXml, ([XmlNCName[]]@([XmlNCName]::ItemGroup, [XmlNCName]::__any__)));
$ActualFiles = @(($ProjectDirectory.GetFiles('*', [System.IO.SearchOption]::AllDirectories) | Select-Object -Property 'Name', @{ Label = 'DirectoryName'; Expression = {
    if ($_.DirectoryName.Length -gt $SubIdx) { $_.DirectoryName.Substring($SubIdx) } else { '' }
} }, @{ Label = 'Type'; Expression = {
    # C:\Users\lerwi\Git\PowerShell-Modules\WPF\WPF.csproj
    switch ($_.Extension) {
        '.cs' { 'Compile'; break; }
        '.md' { 'None'; break; }
        '' { 'None'; break; }
        default { 'Content'; break; }
    }
} }) | Where-Object { $_.DirectoryName -ne 'obj' -and $_.DirectoryName -ne 'bin' -and -not ($_.DirectoryName.StartsWith('obj\') -or $_.DirectoryName.StartsWith('bin\') -or $_.Name.EndsWith('.csproj')) });

$FileElements | ForEach-Object {
    [System.Xml.XmlElement]$ParentNode = $_.Element.ParentNode;
    $ParentNode.RemoveChild($_.Element) | Out-Null;
    if ($ParentNode.IsEmpty -or $ParentNode.InnerXml.Trim().Length -eq 0) { $ParentNode.ParentNode.RemoveChild($ParentNode) | Out-Null }
}
$ActualFiles = @($ActualFiles | ForEach-Object {
    $e = $_;
    $r = $FileElements | Where-Object { $_.Name -ieq $e.Name -and $_.Name -ieq $e.Name } | Select-Object -First 1;
    if ($null -eq $r) {
        $x = $ProjectXml.CreateElement($_.Type, 'http://schemas.microsoft.com/developer/msbuild/2003');
        if ([string]::IsNullOrEmpty($e.DirectoryName)) {
            $x.Attributes.Append($ProjectXml.CreateAttribute('Include')).Value = $e.Name;
        } else {
            $x.Attributes.Append($ProjectXml.CreateAttribute('Include')).Value = $e.DirectoryName + '\' + $e.Name;
        }
        $e | Add-Member -MemberType NoteProperty -Name 'Element' -Value $x -PassThru;
    } else {
        if ([string]::IsNullOrEmpty($e.DirectoryName)) {
            $r.Element.Include = $e.Name;
        } else {
            $r.Element.Include = $e.DirectoryName + '\' + $e.Name;
        }
        $r
    }
})
$RefElement = @($ProjectXml.DocumentElement.SelectNodes('msb:ItemGroup', $XmlNamespaceManager)) | Select-Object -Last 1;
if ($null -eq $RefElement) { $RefElement = @($ProjectXml.DocumentElement.SelectNodes('msb:PropertyGroup', $XmlNamespaceManager)) | Select-Object -Last 1 }
foreach ($Group in (($ActualFiles | Group-Object -Property 'Type') | Sort-Object -Property 'Name')) {
    $ItemGroup = $ProjectXml.CreateElement('ItemGroup', 'http://schemas.microsoft.com/developer/msbuild/2003');
    $ProjectXml.DocumentElement.InsertAfter($ItemGroup, $RefElement) | Out-Null;
    $RefElement = $ItemGroup;
    ($Group.Group | Sort-Object -Property 'DirectoryName', 'Name') | ForEach-Object { $ItemGroup.AppendChild($_.Element) | Out-Null }
} 
$XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
$XmlWriterSettings.Indent = $true;
$XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $false);
$XmlWriter = [System.Xml.XmlWriter]::Create($ProjectFile.FullName, $XmlWriterSettings);
try {
    $ProjectXml.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
#>