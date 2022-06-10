Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath 'bin/Debug/net6.0/DevHelper') -ErrorAction Stop;

$Process = [System.Diagnostics.Process]::GetCurrentProcess();
Read-Host -Prompt "Press enter after debugger is attached to $($Process.ProcessName) (PID: $($Process.Id))";

$BuildWorkspace = New-BuildWorkspace -LoadMetadataForReferencedProjects -SkipUnrecognizedProjects;
if ($null -eq $BuildWorkspace) {
    throw 'New-BuildWorkspace returned null';
    return;
}
$OpenSolutionJob = Start-OpenSolutionJob;
if ($null -eq $OpenSolutionJob) {
    throw 'Start-OpenSolutionJob returned null';
    return;
}
$Solution = $OpenSolutionJob | Receive-Job -Wait;
if ($null -eq $Solution) {
    throw 'Receive-Job returned null';
    return;
}
@"
Solution ID: $($Solution.SolutionId)
    FilePath: "$($Solution.FilePath)"
"@
$Projects = @(Get-SolutionProject -Solution $Solution);
if ($Project.Count -eq 0) {
    throw 'No projects returned';
    return;
}
foreach ($p in $Projects) {
    @"
Project Id: $($p.ProjectId)
    Name: "$($p.Name)"
    Language: "$($p.Language)"
    FilePath: "$($p.FilePath)"
    OutputFilePath: "$($p.OutputFilePath)"
    OutputRefFilePath: "$($p.OutputRefFilePath)"
    DefaultNamespace: "$($p.DefaultNamespace)"
    AssemblyName: "$($p.AssemblyName)"
"@
}
<#
Function Get-DirectlyImplementingInterfaces {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $Type.Assembly.GetTypes() | ? {
            $_ -ne $Type -and $_.IsInterface -and $Type.IsAssignableFrom($_) -and @($_.GetInterfaces() | ? { $_ -ne $Type -and $Type.IsAssignableFrom($_) }).Count -eq 0;
        } | % { "    /// <seealso cref=`"$($_ | ConvertTo-SimleTypeName -Namespace $Namespace)`" />"} | Sort-Object;
    }
}
Function Get-PropertyReferences {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $e = @([System.Collections.Generic.ICollection`1], [System.Collections.Generic.IEnumerable`1], [System.Collections.Generic.ISet`1],
        [System.Collections.Generic.IReadOnlyCollection`1], [System.Collections.Generic.IReadOnlySet`1]) | % { $_.MakeGenericType($Type) }
        $Type.Assembly.GetTypes() | % { $_.GetProperties() } | ? {
            $_.ReflectedType -eq $_.DeclaringType -and ($_.PropertyType -eq $Type -or ($_.PropertyType.IsGenericType -and @($_.PropertyType.GetGenericArguments() | ? { $_ -eq $Type }).Count -gt 0))
        } | % { "    /// <seealso cref=`"$($_.DeclaringType | ConvertTo-SimleTypeName -Namespace $Namespace).$($_.Name)`" />" } | Sort-Object;
    }
}
Function Get-InterfaceBaseTypes {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $AllInterfaces = $Type.GetInterfaces();
        $AllInterfaces | ? {
            $t = $_;
            @($AllInterfaces | ? {
                $t -ne $_ -and $t.IsAssignableFrom($_)
            }).Count -eq 0;
        } | % { "    /// <seealso cref=`"$($_ | ConvertTo-SimleTypeName -Namespace $Namespace)`" />"} | Sort-Object;
    }
}
Function Get-SeeAlsoElements {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $Type | Get-InterfaceBaseTypes -Namespace $Namespace;
        $Type | Get-DirectlyImplementingInterfaces -Namespace $Namespace;
        $Type | Get-PropertyReferences -Namespace $Namespace;
    }
}
[FsInfoCat.IAccessError] | Get-SeeAlsoElements -Namespace 'FsInfoCat';
#>
<#
$XmlDocument = [System.Xml.XmlDocument]::new();
$Xml = $XmlDocument.CreateDocumentFragment();
[FsInfoCat.IDbEntity] | Add-SeeAlsoElements -Xml $Xml -UsingNamespace 'FsInfoCat', 'System', 'System.Collections.Generic' -ErrorAction Stop;
$Xml.ChildNodes | % { "    /// $($_.OuterXml)" }
#>

# $Xml.RemoveAll(); [FsInfoCat.IAccessError] | Add-SeeAlsoElements -Xml $Xml -UsingNamespace 'FsInfoCat', 'System', 'System.Collections.Generic'; $Xml.ChildNodes | % { "    /// $($_.OuterXml)" }

