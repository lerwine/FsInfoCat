Param(
    [Type[]]$Type = @([Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax])
)

Set-Variable -Name 'XNamespaces' -Option Constant -Value(&{
    [System.Collections.Generic.Dictionary[string]]
});

Function Test-SchemaConvertibleType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [switch]$AllowBasic
    )

    Process {
        if ($AllowBasic.IsPresent) {
            if ($Type.IsSpecialName -or $Type.IsPointer -or $Type.IsByRef -or -not ($Type.IsEnum -or $Type.IsPrimitive -or $Type -eq [string] -or $Type -eq [decimal] -or $Type -eq [TimeSpan] -or $Type -eq [DateTime] -or $Type -eq [Uri] -or $Type.IsClass)) {
                $false | Write-Output;
                continue;
            }
        } else {
            if ($Type.IsSpecialName -or $Type.IsPointer -or $Type.IsByRef -or $Type.Assembly -eq [string].Assembly -or $Type -eq [Uri] -or -not $Type.IsClass) {
                $false | Write-Output;
                continue;
            }
        }
        if ($Type.IsConstructedGenericType) {
            $g = $Type.GetGenericArguments();
            if ($g.Length -ne 1 -or (-not (Test-SchemaConvertibleType -Type $g[0] -AllowBasic)) -or $Type.GetInterfaces() -cnotcontains [System.Collections.Generic.IEnumerable`1].MakeGenericType($g[0])) {
                $false | Write-Output;
                continue;
            }
        } else {
            if ($Type.IsArray) {
                $e = $Type.GetElementType();
                if ($Type.GetArrayRank() -ne 1 -or -not (Test-SchemaConvertibleType -Type $e -AllowBasic)) {
                    $false | Write-Output;
                    continue;
                }
            } else {
                if ($Type.IsGenericType) {
                    $false | Write-Output;
                    continue;
                }
            }
        }
    }

    End { $true | Write-Output }
}

class SchemaConversionContext {
    [string]$Prefix;
    [Type]$BaseType;
    [System.Xml.Linq.XDocument]$Document;
    [System.Xml.Linq.XNamespace]$Namespace;

    SchemaConversionContext([System.Xml.Linq.XDocument]$Document, [Type]$BaseType) {
        $this.Document = $Document;
        $a = $Document.Root.Attribute('targetNamespace');
        if ($null -eq $a) { throw 'Document does not have a targetNamespace attribute' }
        $this.Namespace = [System.Xml.Linq.XNamespace]::Get($a.Value);
        $a = $Document.Root.Attributes() | Where-Object { $_.Name.Namespace -eq [System.Xml.Linq.XNamespace]::Xmlns -and $_.Value -eq $a.Value }
        if ($null -eq $a) { throw 'Document does not have a prefix for the targetNamespace URI' }
        $this.Prefix = $a.Name.LocalName;
    }

    SchemaConversionContext([string]$Prefix, [System.Xml.Linq.XNamespace]$Namespace, [Type]$BaseType) {
        if ($null -eq $Namespace) { throw 'Namespace cannot be null' }
        if ($null -eq $BaseType) { throw 'BaseType cannot be null' }
        if ($BaseType.IsSealed -or $BaseType.IsSpecialName -or $BaseType.IsPointer -or $BaseType.IsByRef -or $BaseType.IsArray -or $BaseType.Assembly -eq [string].Assembly -or $BaseType -eq [Uri] -or -not $BaseType.IsClass) {
            throw 'Base type must be inheritable';
        }
        if (('xs', 'xmlns', 'xml') -ccontains [System.Xml.XmlConvert]::VerifyNCName($Prefix)) { throw 'Invalid prefix' }
        $this.Prefix = $Prefix;
        $this.Namespace = $Namespace;
        $this.BaseType = $BaseType;
        $this.Document = [System.Xml.Linq.XDocument]::Parse(@"
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:$Prefix="$($Namespace.NamespaceName)" targetNamespace="$($Namespace.NamespaceName)">
    <xs:simpleType name="GuidType">
        <xs:restriction base="xs:NCName">
            <xs:pattern value="[a-f\d]{8}(-[a-f\d]{4}){4}[a-f\d]{8}" />
        </xs:restriction>
    </xs:simpleType>
</xs:schema>
"@);
    }

    [Type] GetElementType([Type]$Type) {
        if ($this.IsSchemaConvertible($Type)) {
            if ($Type.IsArray) { return $Type.GetElementType() }
            if ($Type.IsConstructedGenericType) { return $Type.GetGenericArguments()[0] }
        }
        return $null;
    }

    [bool] IsSchemaConvertible([Type]$Type) {
        if ($null -eq $Type -or $Type.IsSpecialName -or $Type.IsPointer -or $Type.IsByRef -or $Type.Assembly -eq [string].Assembly -or $Type -eq [Uri] -or -not $Type.IsClass) { return $false }
        if ($Type.IsConstructedGenericType) {
            $a = $Type.GetGenericArguments();
            if ($a.Length -ne 1 -or $Type.GetInterfaces() -cnotcontains [System.Collections.Generic.IEnumerable`1].MakeGenericType($a[0]) -or $a[0].IsGenericType -or $a[0].IsArray) { return $false }
            return $a[0].IsClass -and  $this.IsSchemaConvertible($a[0]);
        }
        if ($Type.IsArray) {
            if ($Type.GetArrayRank() -ne 1) { return $false }
            $e = $Type.GetElementType();
            if ($e.IsArray -or $e.IsGenericType) { return $false }
            return $this.IsSchemaConvertible($e);
        }
        return $true;
    }

    [string] GetTypeName([Type]$Type) {
        if ($null -eq $Type -or $Type.IsSpecialName -or $Type.IsPointer -or $Type.IsByRef -or $Type.Assembly -eq [string].Assembly -or $Type -eq [Uri] -or -not $Type.IsClass) { return $null }
        if ($Type.IsConstructedGenericType) {
            $a = $Type.GetGenericArguments();
            if ($a.Length -ne 1 -or $Type.GetInterfaces() -cnotcontains [System.Collections.Generic.IEnumerable`1].MakeGenericType($a[0])) { return $null }
            $e = $this.GetElementName($a[0]);
            if ($null -ne $e) { return "$e$($Type.Name -replace '`\d+$', '')List"; }
            return $null;
        }
        if ($Type.IsArray) {
            if ($Type.GetArrayRank() -ne 1) { return $null }
            $e = $this.GetElementName($Type.GetElementType());
            if ($null -ne $e) { return "$($e)Array" }
            return $null;
        }
        return "$($Type.Name)Type";
    }

    [string] GetReferenceName([Type]$Type) {
        switch ($Type) {
            { $_ -eq [string] } { return "xs:string"; }
            { $_ -eq [byte] } { return "xs:unsignedByte"; }
            { $_ -eq [sbyte] } { return "xs:byte"; }
            { $_ -eq [short] } { return "xs:short"; }
            { $_ -eq [ushort] } { return "xs:unsignedshort"; }
            { $_ -eq [int] } { return "xs:int"; }
            { $_ -eq [uint] } { return "xs:unsignedInt"; }
            { $_ -eq [long] } { return "xs:long"; }
            { $_ -eq [ulong] } { return "xs:unsignedLong"; }
            { $_ -eq [float] } { return "xs:float"; }
            { $_ -eq [double] } { return "xs:double"; }
            { $_ -eq [bool] } { return "xs:boolean"; }
            { $_ -eq [decimal] } { return "xs:decimal"; }
            { $_ -eq [DateTime] } { return "xs:dateTime"; }
            { $_ -eq [TimeSpan] } { return "xs:duration"; }
            { $_ -eq [Guid] } { return "$($this.Prefix):GuidType"; }
            { $_ -eq [Uri] } { return "xs:anyUri"; }
            { $_ -eq [byte[]] } { return "xs:base64Binary"; }
            default {
                $n = $this.GetTypeName($Type);
                if ($null -ne $n) { return "$($this.Prefix):$n" }
                break;
            }
        }
        return $null;
    }

    [string] GetElementName([Type]$Type) {
        if ($null -eq $Type -or $Type.IsConstructedGenericType -or $Type.IsArray -or $Type.IsSpecialName -or $Type.IsPointer -or $Type.IsByRef -or $Type.Assembly -eq [string].Assembly -or $Type -eq [Uri] -or -not $Type.IsClass) { return $null }
        return $Type.Name;
    }
}

Function Get-XsdComplexType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$TypeName,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$Document
    )

    Begin {
        $ElementName = [XLinqHelper]::GetXsdName('complexType');
        $AttributeName = [XLinqHelper]::GetName('name');
    }
    Process {
        ($Document.Root | Select-XElement -ElementName $ElementName -AttributeName $AttributeName -Equals $TypeName) | Write-Output;
    }
}

class CustomTypeMapper {

}
Function Format-XsdTypeName {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type
    )

    Process {
        [System.Xml.XmlQualifiedName]::new()
    }
}
Function Convert-TypeToSchema {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [SchemaConversionContext]$Context
    )

    Process {
        $TypeName = $Context.GetTypeName($Type);
        if ($null -ne $TypeName) {
            $ComplexTypeElement = Get-XsdComplexType -TypeName $TypeName -Document $Context.Document
            if ($null -eq $ComplexTypeElement) {
                $Extends = $null;
                if ($Context.BaseType.IsAssignableFrom($Type.BaseType)) {
                    $Extends = $Context.GetReferenceName($Type.BaseType)
                }
                $ParentElement = $null;
                if ($null -eq $Extends) {
                    $ParentElement = $ComplexTypeElement = New-XsdComplexType -Name $TypeName;
                } else {
                    Convert-TypeToSchema -Type $Type.BaseType -Context $Context;
                    $ComplexTypeElement = New-XsdComplexType -Name $TypeName -Extends $Extends;
                    $ParentElement = $ComplexTypeElement.Element([XLinqHelper]::GetXsdName('complexContent')).Element([XLinqHelper]::GetXsdName('extension'));
                }
            }
        }
    }

    End {
        if (-not $PSBoundParameters.ContainsKey('Document')) { $Schema | Write-Output }
    }
}

$Prefix = 'md';
$Namespace = [System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd');
$Schema = [System.Xml.Linq.XDocument]::Parse(@"
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:$Prefix="$($Namespace.NamespaceName)" targetNamespace="$($Namespace.NamespaceName)">
    <xs:simpleType name="GuidType">
        <xs:restriction base="xs:NCName">
            <xs:pattern value="[a-f\d]{8}(-[a-f\d]{4}){4}[a-f\d]{8}" />
        </xs:restriction>
    </xs:simpleType>
</xs:schema>
"@);
$CSharpAssembly = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].Assembly;

$Writer = [System.IO.StringWriter]::new();
foreach ($CurrentType in $Type) {
    $InheritingTypes = @();
    if (-not $CurrentType.IsSealed) {
        $InheritingTypes = @($CurrentType.Assembly.GetTypes() | Where-Object { $_.BaseType -eq $CurrentType });
        if ($CurrentType.Assembly.FullName -cne $CSharpAssembly.FullName) {
            $InheritingTypes = @($InheritingTypes + @($CSharpAssembly.GetTypes() | Where-Object { $_.BaseType -eq $CurrentType }));
        }
    }
    $BasePropertyNames = @($CurrentType.BaseType.GetProperties() | Where-Object {
        -not $_.GetGetMethod().IsStatic
    } | ForEach-Object { $_.Name });
    $CurrentProperties = @($CurrentType.GetProperties() | Where-Object { $BasePropertyNames -cnotcontains $_.Name -and -not $_.GetGetMethod().IsStatic });
    $NounName = $CurrentType.Name -Replace 'Syntax$', '';
    $ElementName = $NounName -replace 'Declaration$', '';
    $ParentNounName = $CurrentType.BaseType.Name -Replace 'Syntax$', '';
    $Writer.WriteLine('');
    $Writer.Write('Function Import-');
    $Writer.Write($NounName);
    $Writer.WriteLine(' {');
    if ($InheritingTypes.Count -gt 0) {
        $Writer.Write('    # [');
        $Writer.Write($InheritingTypes[0].FullName);
        ($InheritingTypes | Select-Object -Skip 1) | ForEach-Object {
            $Writer.Write('], [');
            $Writer.Write($_.FullName);
        }
        $Writer.WriteLine(']');
        $Writer.WriteLine('    [CmdletBinding(DefaultParameterSetName = ''ToParent'')]');
        $Writer.WriteLine('    Param(');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
        $Writer.Write('        [');
        $Writer.Write($CurrentType.FullName);
        $Writer.WriteLine(']$Syntax,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ParameterSetName = ''ToParent'')]');
        $Writer.WriteLine('        [System.Xml.XmlElement]$ParentElement,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ParameterSetName = ''ToMember'')]');
        $Writer.WriteLine('        [System.Xml.XmlElement]$MemberElement,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(ParameterSetName = ''ToParent'')]');
        $Writer.WriteLine('        [string]$ElementName,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(ParameterSetName = ''ToMember'')]');
        $Writer.WriteLine('        [switch]$IsUnknown');
        $Writer.WriteLine('    )');
        $Writer.WriteLine('');
        $Writer.WriteLine('    Process {');
        $Writer.WriteLine('        if ($PSCmdlet.ParameterSetName -eq ''ToParent'') {');
        $Writer.WriteLine('            switch ($Syntax) {');
        $InheritingTypes | ForEach-Object {
                $Writer.Write('                # { $_ -is [');
            $Writer.Write($_.FullName);
            $Writer.WriteLine('] } {');
            $Writer.WriteLine('                #     if ($PSBoundParameters.ContainsKey(''ElementName'')) {');
                $Writer.Write('                #         Import-');
            $Writer.Write(($_.Name -Replace 'Syntax$', ''));
            $Writer.WriteLine(' -Syntax $Syntax -ParentElement $ParentElement -ElementName $ElementName;');
            $Writer.WriteLine('                #     } else {');
                $Writer.Write('                #         Import-');
            $Writer.Write(($_.Name -Replace 'Syntax$', ''));
            $Writer.WriteLine(' -Syntax $Syntax -ParentElement $ParentElement;');
            $Writer.WriteLine('                #     }');
            $Writer.WriteLine('                #     break;');
            $Writer.WriteLine('                # }');
        }
        $Writer.WriteLine('                default {');
        $Writer.WriteLine('                    if ($PSBoundParameters.ContainsKey(''ElementName'')) {');
        $Writer.Write('                        Import-');
        $Writer.Write($NounName);
        $Writer.Write(' -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement($ElementName)).AppendChild($ParentElement.OwnerDocument.CreateElement(''Unknown');
        $Writer.Write($ElementName);
        $Writer.WriteLine('''))) -IsUnknown;');
        $Writer.WriteLine('                    } else {');
        $Writer.Write('                        Import-');
        $Writer.Write($NounName);
        $Writer.Write(' -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement(''Unknown');
        $Writer.Write($ElementName);
        $Writer.WriteLine('''))) -IsUnknown;');
        $Writer.WriteLine('                    }');
        $Writer.WriteLine('                    break;');
        $Writer.WriteLine('                }');
        $Writer.WriteLine('            }');
        $Writer.WriteLine('        } else {');
        $Writer.WriteLine('            if ($IsUnknown.IsPresent) {');
        $Writer.Write('                Import-');
        $Writer.Write($ParentNounName);
        $Writer.WriteLine(' -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;');
        $Writer.WriteLine('            } else {');
        $Writer.Write('                Import-');
        $Writer.Write($ParentNounName);
        $Writer.WriteLine(' -Syntax $Syntax -MemberElement $MemberElement;');
        $Writer.WriteLine('            }');
        $CurrentProperties | ForEach-Object {
            $Writer.WriteLine('');
            $Writer.Write('            # [');
            $Writer.Write($_.PropertyType.FullName);
            $Writer.Write(']$');
            $Writer.Write($_.Name);
            $Writer.Write('; # IsValueType = ');
            $Writer.WriteLine($_.PropertyType.IsValueType.ToString());
            $Writer.Write('            # Import-');
            $Writer.Write(($_.PropertyType.Name -replace 'Syntax$', ''));
            $Writer.Write(' -Syntax $Syntax.');
            $Writer.Write($_.Name);
            $Writer.WriteLine(' -ParentElement $MemberElement;');
        }
        $Writer.WriteLine('        }');
    } else {
        $Writer.WriteLine('    [CmdletBinding()]');
        $Writer.WriteLine('    Param(');
        $Writer.WriteLine('        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]');
        $Writer.Write('        [');
        $Writer.Write($CurrentType.FullName);
        $Writer.WriteLine(']$Syntax,');
        $Writer.WriteLine('');
        $Writer.WriteLine('        [Parameter(Mandatory = $true)]');
        $Writer.WriteLine('        [System.Xml.XmlElement]$ParentElement,');
        $Writer.WriteLine('');
        $Writer.Write('        [string]$ElementName = ''');
        $Writer.Write($ElementName);
        $Writer.WriteLine('''');
        $Writer.WriteLine('    )');
        $Writer.WriteLine('');
        $Writer.WriteLine('    Begin { $OwnerDocument = $ParentElement.OwnerDocument }');
        $Writer.WriteLine('');
        $Writer.WriteLine('    Process {');
        $Writer.WriteLine('        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement($ElementName));');
        $Writer.WriteLine('');
        $Writer.Write('        Import-');
        $Writer.Write($ParentNounName);
        $Writer.WriteLine(' -Syntax $Syntax -MemberElement $MemberElement;');
        $CurrentProperties | ForEach-Object {
            $Writer.WriteLine('');
            $Writer.Write('        # [');
            $Writer.Write($_.PropertyType.FullName);
            $Writer.Write(']$');
            $Writer.Write($_.Name);
            $Writer.Write('; # IsValueType = ');
            $Writer.WriteLine($_.PropertyType.IsValueType.ToString());
            $Writer.Write('        # Import-');
            $Writer.Write(($_.PropertyType.Name -replace 'Syntax$', ''));
            $Writer.Write(' -Syntax $Syntax.');
            $Writer.Write($_.Name);
            $Writer.WriteLine(' -ParentElement $MemberElement;');
        }
    }
    $Writer.WriteLine('    }');
    $Writer.WriteLine('}');
}
[System.IO.File]::WriteAllText(($PSScriptRoot | Join-Path -ChildPath 'Temp.txt'), $Writer.ToString().TrimEnd());

