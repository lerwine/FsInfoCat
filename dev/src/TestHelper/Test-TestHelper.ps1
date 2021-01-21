Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '..\..\bin\TestHelper') -ErrorAction Stop;

Function Test-InputSet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.InputSet]$InputSet,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $InputSet) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    if ($null -eq $InputSet.Description) {
        Write-Warning -Message "Expected $Description.Description='$($Source.Description)'; Actual: null";
        return $false;
    }
    if ($InputSet.Description -cne $Source.Description) {
        Write-Warning -Message "Expected $Description.Description='$($Source.Description)'; Actual: '$($InputSet.Description)'";
        return $false;
    }
    $XmlNodeList = @($Source.SelectNodes('Roots/Root'));
    if ($InputSet.Roots.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Files.Count=$($XmlNodeList.Count); Actual: $($InputSet.Files.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-RootDefinition -Source $XmlNodeList[$i] -RootDefinition $InputSet.Roots[$i] -Description "$Description.Roots[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Roots[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Roots[$i]";
            return $false;
        }
    }
    $XmlNodeList = @($Source.SelectNodes('Tests/Test'));
    if ($InputSet.Tests.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Tests.Count=$($XmlNodeList.Count); Actual: $($InputSet.Tests.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-TestDefinition -Source $XmlNodeList[$i] -TestDefinition $InputSet.Tests[$i] -Description "$Description.Tests[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Tests[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Tests[$i]";
            return $false;
        }
    }
    return $true;
}

Function Test-RootDefinition {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.RootDefinition]$RootDefinition,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $RootDefinition) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    $Value = [System.Xml.XmlConvert]::ToInt32($Source.ID);
    if ($RootDefinition.ID -ne $Value) {
        Write-Warning -Message "Expected $Description.ID=$Value; Actual: $($RootDefinition.ID)";
        return $false;
    }
    if ($null -eq $RootDefinition.Description) {
        Write-Warning -Message "Expected $Description.Description='$($Source.Description)'; Actual: null";
        return $false;
    }
    if ($RootDefinition.Description -cne $Source.Description) {
        Write-Warning -Message "Expected $Description.Description='$($Source.Description)'; Actual: '$($RootDefinition.Description)'";
        return $false;
    }
    $XmlNodeList = @($Source.SelectNodes('TemplateRef'));
    if ($RootDefinition.TemplateRefs.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Tests.Count=$($XmlNodeList.Count); Actual: $($RootDefinition.TemplateRefs.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        $Expected = $XmlNodeList[$i].InnerText;
        $Actual = $RootDefinition.TemplateRefs[$i];
        if ($null -eq $Actual) {
            Write-Warning -Message "Expected $Description.TemplateRefs[$i]='$Expected'; Actual: null";
            return $false;
        }
        if ($Actual -cne $Expected) {
            Write-Warning -Message "Expected $Description.TemplateRefs[$i]='$Expected'; Actual: '$Actual'";
            return $false;
        }
    }
    return $true;
}

Function Test-TestDefinition {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.TestDefinition]$TestDefinition,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $TestDefinition) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    if ($null -eq $Source.MaxDepth) {
        if ($null -ne $TestDefinition.MaxDepth) {
            Write-Warning -Message "Expected $Description.MaxDepth=null; Actual: $($TestDefinition.MaxDepth)";
            return $false;
        }
    } else {
        $MaxDepth = [System.Xml.XmlConvert]::ToInt32($Source.MaxDepth);
        if ($null -eq $TestDefinition.MaxDepth) {
            Write-Warning -Message "Expected $Description.MaxDepth=$MaxDepth; Actual: null";
            return $false;
        }
        if ($TestDefinition.MaxDepth -ne $MaxDepth) {
            Write-Warning -Message "Expected $Description.MaxDepth=$MaxDepth; Actual: $($TestDefinition.MaxDepth)";
            return $false;
        }
    }
    if ($null -eq $Source.MaxItems) {
        if ($null -ne $TestDefinition.MaxItems) {
            Write-Warning -Message "Expected $Description.MaxItems=null; Actual: $($TestDefinition.MaxItems)";
            return $false;
        }
    } else {
        $MaxItems = [System.Xml.XmlConvert]::ToInt32($Source.MaxItems);
        if ($null -eq $TestDefinition.MaxItems) {
            Write-Warning -Message "Expected $Description.MaxItems=$MaxItems; Actual: null";
            return $false;
        }
        if ($TestDefinition.MaxItems -ne $MaxItems) {
            Write-Warning -Message "Expected $Description.MaxItems=$MaxItems; Actual: $($TestDefinition.MaxItems)";
            return $false;
        }
    }
    $XmlNodeList = @($Source.SelectNodes('Expected'));
    if ($TestDefinition.Expected.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Expected.Count=$($XmlNodeList.Count); Actual: $($TestDefinition.Expected.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-ExpectedResult -Source $XmlNodeList[$i] -ExpectedResult $TestDefinition.Expected[$i] -Description "$Description.Expected[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Expected[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Expected[$i]";
            return $false;
        }
    }
    return $true;
}

Function Test-ExpectedResult {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.ExpectedResult]$ExpectedResult,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $ExpectedResult) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    $Value = [System.Xml.XmlConvert]::ToInt32($Source.RootID);
    if ($ExpectedResult.RootID -ne $Value) {
        Write-Warning -Message "Expected $Description.RootID=$Value; Actual: $($ExpectedResult.RootID)";
        return $false;
    }
    $Value = [System.Xml.XmlConvert]::ToInt32($Source.FileCount);
    if ($ExpectedResult.FileCount -ne $Value) {
        Write-Warning -Message "Expected $Description.FileCount=$Value; Actual: $($ExpectedResult.FileCount)";
        return $false;
    }
    $Value = [System.Xml.XmlConvert]::ToInt32($Source.FolderCount);
    if ($ExpectedResult.FolderCount -ne $Value) {
        Write-Warning -Message "Expected $Description.FolderCount=$Value; Actual: $($ExpectedResult.FolderCount)";
        return $false;
    }
    $XmlNodeList = @($Source.SelectNodes('FileRef|FolderRef'));
    if ($ExpectedResult.Items.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Items.Count=$($XmlNodeList.Count); Actual: $($ExpectedResult.Items.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-ContentRef -Source $XmlNodeList[$i] -ContentRef $ExpectedResult.Items[$i] -Description "$Description.Items[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Items[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Items[$i]";
            return $false;
        }
    }
    return $true;
}

Function Test-ContentRef {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.ContentRef]$ContentRef,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $ContentRef) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    if ($Source.LocalName -eq 'FileRef') {
        if ($ContentRef -isnot [TestHelper.FileRef]) {
            Write-Warning -Message "Expected typeof($Description)=$([TestHelper.FileRef].FullName); Actual: $($ContentRef.GetType().FullName)";
            return $false;
        }
        $FileID = [System.Xml.XmlConvert]::ToInt32($Source.FileID);
        if ($ContentRef.FileID -ne $FileID) {
            Write-Warning -Message "Expected $Description.FileID=$($Source.FileID); Actual: $($ContentRef.FileID)";
            return $false;
        }
    } else {
        if ($ContentRef -isnot [TestHelper.FolderRef]) {
            Write-Warning -Message "Expected typeof($Description)=$([TestHelper.FileRef].FolderRef); Actual: $($ContentRef.GetType().FullName)";
            return $false;
        }
        $FolderID = [System.Xml.XmlConvert]::ToInt32($Source.FolderID);
        if ($ContentRef.FolderID -ne $FolderID) {
            Write-Warning -Message "Expected $Description.FolderID=$($Source.FolderID); Actual: $($ContentRef.FolderID)";
            return $false;
        }
    }
    if ($null -eq $ContentRef.Path) {
        Write-Warning -Message "Expected $Description.Path='$($Source.InnerText)'; Actual: null";
        return $false;
    }
    if ($ContentRef.Path -cne $Source.InnerText) {
        Write-Warning -Message "Expected $Description.Path='$($Source.InnerText)'; Actual: '$($ContentRef.Path)'";
        return $false;
    }
    return $true;
}

Function Test-ContentFile {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.ContentFile]$ContentFile,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $ContentFile) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    if ($null -eq $ContentFile.Name) {
        Write-Warning -Message "Expected $Description.Name='$($Source.Name)'; Actual: null";
        return $false;
    }
    if ($ContentFile.Name -cne $Source.Name) {
        Write-Warning -Message "Expected $Description.Name='$($Source.Name)'; Actual: '$($ContentFile.Name)'";
        return $false;
    }
    $Length = [System.Xml.XmlConvert]::ToInt64($Source.Length);
    if ($ContentFile.Length -ne $Length) {
        Write-Warning -Message "Expected $Description.Length=$Length; Actual: $($ContentFile.Length)";
        return $false;
    }
    $DateTime = [System.Xml.XmlConvert]::ToDateTime($Source.CreationTime, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
    if ($ContentFile.CreationTime -ne $DateTime) {
        Write-Warning -Message "Expected $Description.CreationTime=$DateTime; Actual: $($ContentFile.CreationTime)";
        return $false;
    }
    $DateTime = [System.Xml.XmlConvert]::ToDateTime($Source.LastWriteTime, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
    if ($ContentFile.LastWriteTime -ne $DateTime) {
        Write-Warning -Message "Expected $Description.LastWriteTime=$DateTime; Actual: $($ContentFile.LastWriteTime)";
        return $false;
    }
    $Attributes = [System.Xml.XmlConvert]::ToInt32($Source.Attributes);
    if ($ContentFile.Attributes -ne $Attributes) {
        Write-Warning -Message "Expected $Description.Attributes=$Attributes; Actual: $($ContentFile.Attributes)";
        return $false;
    }
    $Depth = [System.Xml.XmlConvert]::ToInt32($Source.Depth);
    if ($ContentFile.Depth -ne $Depth) {
        Write-Warning -Message "Expected $Description.Depth=$Depth; Actual: $($ContentFile.Depth)";
        return $false;
    }
    return $true;
}

Function Test-ContentFolder {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.ContentFolder]$ContentFolder,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $ContentFolder) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    if ($null -eq $ContentFolder.Name) {
        Write-Warning -Message "Expected $Description.Name='$($Source.Name)'; Actual: null";
        return $false;
    }
    if ($ContentFolder.Name -cne $Source.Name) {
        Write-Warning -Message "Expected $Description.Name='$($Source.Name)'; Actual: '$($ContentFolder.Name)'";
        return $false;
    }
    $DateTime = [System.Xml.XmlConvert]::ToDateTime($Source.CreationTime, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
    if ($ContentFolder.CreationTime -ne $DateTime) {
        Write-Warning -Message "Expected $Description.CreationTime=$DateTime; Actual: $($ContentFolder.CreationTime)";
        return $false;
    }
    $DateTime = [System.Xml.XmlConvert]::ToDateTime($Source.LastWriteTime, [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind);
    if ($ContentFolder.LastWriteTime -ne $DateTime) {
        Write-Warning -Message "Expected $Description.LastWriteTime=$DateTime; Actual: $($ContentFolder.LastWriteTime)";
        return $false;
    }
    $Attributes = [System.Xml.XmlConvert]::ToInt32($Source.Attributes);
    if ($ContentFolder.Attributes -ne $Attributes) {
        Write-Warning -Message "Expected $Description.Attributes=$Attributes; Actual: $($ContentFolder.Attributes)";
        return $false;
    }
    $Depth = [System.Xml.XmlConvert]::ToInt32($Source.Depth);
    if ($ContentFolder.Depth -ne $Depth) {
        Write-Warning -Message "Expected $Description.Depth=$Depth; Actual: $($ContentFolder.Depth)";
        return $false;
    }
    $XmlNodeList = @($Source.SelectNodes('Files/File'));
    if ($ContentFolder.Files.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Files.Count=$($XmlNodeList.Count); Actual: $($ContentFolder.Files.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-ContentFile -Source $XmlNodeList[$i] -ContentFile $ContentFolder.Files[$i] -Description "$Description.Files[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Files[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Files[$i]";
            return $false;
        }
    }
    $XmlNodeList = @($Source.SelectNodes('Folders/Folder'));
    if ($ContentFolder.Folders.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Folders.Count=$($XmlNodeList.Count); Actual: $($ContentFolder.Folders.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-ContentFolder -Source $XmlNodeList[$i] -ContentFolder $ContentFolder.Folders[$i] -Description "$Description.Folders[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Folders[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Folders[$i]";
            return $false;
        }
    }
    return $true;
}

Function Test-ContentTemplate {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$Source,
        [Parameter(Mandatory = $true)]
        [AllowNull()]
        [TestHelper.ContentTemplate]$ContentTemplate,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )
    if ($null -eq $ContentTemplate) {
        Write-Warning -Message "$Description was null";
        return $false;
    }
    if ($null -eq $ContentTemplate.FileName) {
        Write-Warning -Message "Expected $Description.FileName='$($Source.FileName)'; Actual: null";
        return $false;
    }
    if ($ContentTemplate.FileName -cne $Source.FileName) {
        Write-Warning -Message "Expected $Description.FileName='$($Source.FileName)'; Actual: '$($ContentTemplate.FileName)'";
        return $false;
    }
    $MaxDepth = [System.Xml.XmlConvert]::ToInt32($Source.MaxDepth);
    if ($ContentTemplate.MaxDepth -ne $MaxDepth) {
        Write-Warning -Message "Expected $Description.MaxDepth=$MaxDepth; Actual: $($ContentTemplate.MaxDepth)";
        return $false;
    }
    $XmlNodeList = @($Source.SelectNodes('Files/File'));
    if ($ContentTemplate.Files.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Files.Count=$($XmlNodeList.Count); Actual: $($ContentTemplate.Files.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-ContentFile -Source $XmlNodeList[$i] -ContentFile $ContentTemplate.Files[$i] -Description "$Description.Files[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Files[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Files[$i]";
            return $false;
        }
    }
    $XmlNodeList = @($Source.SelectNodes('Folders/Folder'));
    if ($ContentTemplate.Folders.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected $Description.Folders.Count=$($XmlNodeList.Count); Actual: $($ContentTemplate.Folders.Count)";
        return $false;
    }
    for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
        if (Test-ContentFolder -Source $XmlNodeList[$i] -ContentFolder $ContentTemplate.Folders[$i] -Description "$Description.Folders[$i]") {
            Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "$Description.Folders[$i]";
        } else {
            Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "$Description.Folders[$i]";
            return $false;
        }
    }
    return $true;
}

$XmlText = @'
<Contents xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocationNoLocation="ContentInfo.xsd">
    <Templates>
        <Template FileName="SingleFile.zip" MaxDepth="12" ID="0">
            <Files>
                <File Name="Lorem Ipsum.docx" Length="15163" Attributes="32" CreationTime="2021-01-14T00:32:58-05:00" LastWriteTime="2021-01-14T00:32:58-05:00" Depth="0" ID="1" />
                <File Name="Lorem Ipsum.pdf" Length="61559" Attributes="32" CreationTime="2021-01-14T00:34:30-05:00" LastWriteTime="2021-01-14T00:34:30-05:00" Depth="0" ID="2" />
            </Files>
            <Folders>
                <Folder Name="Graphics" Attributes="16" CreationTime="2021-01-14T00:38:18.6723781-05:00" LastWriteTime="2021-01-14T00:38:33.2018893-05:00" Depth="1" ID="0">
                    <Files>
                        <File Name="button-1.svg" Length="4052" Attributes="32" CreationTime="2020-10-01T23:00:55.8975058-04:00" LastWriteTime="2020-10-01T23:00:55.8985023-04:00" Depth="2" ID="3" />
                        <File Name="drawing.svg" Length="1851" Attributes="32" CreationTime="2020-10-01T15:51:14.6385521-04:00" LastWriteTime="2020-10-01T15:56:32.0796876-04:00" Depth="2" ID="4" />
                    </Files>
                    <Folders>
                        <Folder Name="Pictures" Attributes="16" CreationTime="2021-01-14T00:35:51.5493482-05:00" LastWriteTime="2021-01-14T00:36:32.144156-05:00" Depth="2" ID="1" />
                    </Folders>
                </Folder>
                <Folder Name="Drawings" Attributes="16" CreationTime="2021-01-14T00:38:03.9541683-05:00" LastWriteTime="2021-01-14T00:39:30.1920359-05:00" Depth="2" ID="2" />
            </Folders>
        </Template>
        <Template FileName="MultiFile.zip" MaxDepth="12" ID="1" />
    </Templates>
    <InputSets>
        <InputSet Description="@(MultiFile) + @(ThreeDeep)">
            <Roots>
                <Root Description="@(MultiFile) + @(ThreeDeep)" ID="3">
                    <TemplateRef>0</TemplateRef>
                    <TemplateRef>1</TemplateRef>
                </Root>
            </Roots>
            <Tests>
                <Test>
                    <Expected RootID="3" FileCount="8" FolderCount="3">
                        <FileRef FileID="0">Lorem Ipsum.docx</FileRef>
                        <FolderRef FolderID="0">Graphics</FolderRef>
                        <FileRef FileID="1">Graphics/Drawings/button-1.svg</FileRef>
                        <FolderRef FolderID="1">Graphics/Pictures</FolderRef>
                    </Expected>
                </Test>
                <Test MaxDepth="2" />
                <Test MaxItems="8" />
                <Test MaxItems="15" MaxDepth="14" />
            </Tests>
        </InputSet>
        <InputSet Description="SevenDeep" />
    </InputSets>
</Contents>
'@
[Xml]$XmlDocument = $XmlText;
$StringReader = [System.IO.StringReader]::new($XmlText);
$XmlReader = [System.Xml.XmlReader]::Create($StringReader);
[TestHelper.FsCrawlJobTestData]$FsCrawlJobTestData = $null;
try {
    $XmlSerializer = New-Object -TypeName 'System.Xml.Serialization.XmlSerializer' -ArgumentList ([TestHelper.FsCrawlJobTestData]) -ErrorAction Stop;
    $FsCrawlJobTestData = $XmlSerializer.Deserialize($XmlReader);
} catch {
    $e = $null;
    if ($_ -is [System.Exception]) {
        $e = $_;
    } else {
        $_ | Write-Host -ForegroundColor Cyan;
        $e = $_.Exception;
    }
    while ($null -ne $e) {
        $e | Write-Host -ForegroundColor Cyan;
        $e = $e.InnerException;
    }
    return;
} finally { $XmlReader.Close() }

if ($null -eq $FsCrawlJobTestData) {
    Write-Warning -Message 'Nothing deserialized';
    return;
}
if ($FsCrawlJobTestData -isnot [TestHelper.FsCrawlJobTestData]) {
    Write-Warning -Message "Expected type: $([TestHelper.FsCrawlJobTestData].AssemblyQualifiedName); Actual type: $($FsCrawlJobTestData.GetType().AssemblyQualifiedName)";
    return;
}

$XmlNodeList = @($XmlDocument.DocumentElement.SelectNodes('Templates/Template'));
if ($FsCrawlJobTestData.Templates.Count -ne $XmlNodeList.Count) {
    Write-Warning -Message "Expected Templates.Count: $($XmlNodeList.Count); Actual: $($FsCrawlJobTestData.Templates.Count)";
    return;
}
$passed = $true;
for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
    if (Test-ContentTemplate -Source $XmlNodeList[$i] -ContentTemplate $FsCrawlJobTestData.Templates[$i] -Description "Templates[$i]") {
        Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "Templates[$i]";
    } else {
        Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "Templates[$i]";
        $passed = $false;
        break;
    }
}
if ($passed) {
    $XmlNodeList = @($XmlDocument.DocumentElement.SelectNodes('InputSets/InputSet'));
    if ($FsCrawlJobTestData.InputSets.Count -ne $XmlNodeList.Count) {
        Write-Warning -Message "Expected InputSets.Count: $($XmlNodeList.Count); Actual: $($FsCrawlJobTestData.InputSets.Count)";
        $passed = $false;
    } else {
        for ($i = 0; $i -lt $XmlNodeList.Count; $i++) {
            if (Test-InputSet -Source $XmlNodeList[$i] -InputSet $FsCrawlJobTestData.InputSets[$i] -Description "InputSets[$i]") {
                Write-Progress -Activity "Running tests" -Status "Passed" -CurrentOperation "InputSets[$i]";
            } else {
                Write-Progress -Activity "Running tests" -Status "Failed" -CurrentOperation "Templates[$i]";
                $passed = $false;
                break;
            }
        }
    }
}

if ($passed) {
    Write-Progress -Activity "Running tests" -Status "All tests passed" -Completed;
    Write-Information -MessageData "All tests passed" -InformationAction Continue;
} else {
    Write-Progress -Activity "Running tests" -Status "Test failed" -Completed;
}