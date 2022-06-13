if ($null -eq $Script:ModelNamespace) {
    Set-Variable -Name 'ModelNamespace' -Scope 'Script' -Option Constant -Value ([System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd'));
}
$Script:ModelDocument = [System.Xml.Linq.XDocument]::new([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName("ModelDefinitions")));
$Script:SolutionDir = [System.IO.DirectoryInfo]::new($PSScriptRoot).Parent;

Function Import-DocumentationComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.SyntaxNode]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$ParentElement
    )

    if ($Syntax.HasLeadingTrivia) {
        foreach ($DocumentationSyntax in ($Syntax.GetLeadingTrivia() | Where-Object { $_.HasStructure } | ForEach-Object { $_.GetStructure() } | Where-Object {
            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax]
        })) {
            $Text = (($DocumentationSyntax.ToString() -split '\r\n?|\n') | ForEach-Object { $_ -replace '^\s*///\s?', '' } | Out-String).Trim();
            if (-not [string]::IsNullOrWhiteSpace($Text)) {
                $DocumentationElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Documentation'));
                try {
                    $Element = [System.Xml.Linq.XElement]::Parse("<Documentation xmlns=`"$($Script:ModelNamespace.NamespaceName)`">$($Text)</Documentation>", [System.Xml.Linq.LoadOptions]::PreserveWhitespace);
                    foreach ($e in $Element.Elements()) {
                        $DocumentationElement.Add($e);
                    }
                    if (-not $DocumentationElement.IsEmpty) { $ParentElement.Add($DocumentationElement) }
                } catch {
                    $DocumentationElement.Add([System.Xml.Linq.XCData]::new($Text));
                    $ParentElement.Add($DocumentationElement);
                }
            }
        }
    }
}

Function Import-TypeSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$ParentElement
    )

    Process {
        if ($null -ne $Syntax) {
            [System.Xml.Linq.XElement]$TypeElement = $null;
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$NameSyntax = $Syntax;
                    switch ($NameSyntax) {
                        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax] } {
                            #[Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax]$AliasQualifiedNameSyntax = $NameSyntax;
                            #[Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax]$Alias = $AliasQualifiedNameSyntax.Alias;
                            #[Microsoft.CodeAnalysis.SyntaxToken]$ColonColonToken = $AliasQualifiedNameSyntax.ColonColonToken;
                            #[Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$Name = $AliasQualifiedNameSyntax.Name;
                            $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('AliasQualifiedName'), [System.Xml.Linq.XText]::new($NameSyntax.ToString().Trim()));
                            break;
                        }
                        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax] } {
                            #[Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax]$QualifiedNameSyntax = $NameSyntax;
                            #[Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$Left = $QualifiedNameSyntax.Left;
                            #[Microsoft.CodeAnalysis.SyntaxToken]$DotToken = $QualifiedNameSyntax.DotToken;
                            #[Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$Right = $QualifiedNameSyntax.Right;
                            $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('QualifiedName'), [System.Xml.Linq.XText]::new($NameSyntax.ToString().Trim()));
                            break;
                        }
                        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax] } {
                            [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$SimpleNameSyntax = $NameSyntax;
                            switch ($SimpleNameSyntax) {
                                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax] } {
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax]$GenericNameSyntax = $SimpleNameSyntax;
                                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericName'), [System.Xml.Linq.XAttribute]::new('Name', $SimpleNameSyntax.Identifier.ValueText.Trim()));
                                    if ($GenericNameSyntax.IsUnboundGenericName) {
                                        $TypeElement.Add([System.Xml.Linq.XAttribute]::new('IsUnbound', $true));
                                    }
                                    if ($null -ne $GenericNameSyntax.TypeArgumentList -and $GenericNameSyntax.TypeArgumentList.Arguments.Count -gt 0) {
                                        $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('TypeArguments'));
                                        $TypeElement.Add($XElement);
                                        $GenericNameSyntax.TypeArgumentList.Arguments | Import-TypeSyntax -ParentElement $XElement;
                                    }
                                    break;
                                }
                                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax] } {
                                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Identifier'), [System.Xml.Linq.XAttribute]::new('Name', $SimpleNameSyntax.Identifier.ValueText.Trim()));
                                    break;
                                }
                                default {
                                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownSimpleName'), [System.Xml.Linq.XAttribute]::new('Name', $SimpleNameSyntax.Identifier.ValueText.Trim()));
                                    break;
                                }
                            }
                            break;
                        }
                        default {
                            $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownName'), [System.Xml.Linq.XCData]::new($NameSyntax.ToString().Trim()));
                            break;
                        }
                    }
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax]$RefTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Ref'));
                    Import-TypeSyntax -Syntax $RefTypeSyntax.Type -ParentElement $TypeElement;
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax]$PredefinedTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Predefined'), [System.Xml.Linq.XText]::new($PredefinedTypeSyntax.Keyword.ToString().Trim()));
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax]$ArrayTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Array'));
                    if ($null -ne $ArrayTypeSyntax.RankSpecifiers -and $ArrayTypeSyntax.RankSpecifiers.Count -gt 0) {
                        $TypeElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Rank'), $_.Rank));
                    }
                    Import-TypeSyntax -Syntax $ArrayTypeSyntax.ElementType -ParentElement $TypeElement;
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax]$PointerTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Pointer'));
                    Import-TypeSyntax -Syntax $PointerTypeSyntax.ElementType -ParentElement $TypeElement;
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax] } {
                    #[Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax]$FunctionPointerTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('FunctionPointer'), [System.Xml.Linq.XText]::new($Syntax.ToString().Trim()));
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax]$NullableTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Nullable'));
                    Import-TypeSyntax -Syntax $NullableTypeSyntax.ElementType -ParentElement $TypeElement;
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax] } {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax]$TupleTypeSyntax = $Syntax;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Tuple'));
                    $TupleTypeSyntax.Elements | ForEach-Object {
                        $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Element'), [System.Xml.Linq.XAttribute]::new('Name', $_.Identifier.ValueText.Trim()));
                        $TypeElement.Add($XElement);
                        Import-TypeSyntax -Syntax $_.Type -ParentElement $XElement;
                    }
                    break;
                }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax] } {
                    #[Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax]$OmittedTypeArgumentSyntax = $Syntax;
                    #[Microsoft.CodeAnalysis.SyntaxToken]$OmittedTypeArgumentToken = $OmittedTypeArgumentSyntax.OmittedTypeArgumentToken;
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('OmittedType'), [System.Xml.Linq.XText]::new($Syntax.ToString().Trim()));
                    break;
                }
                default {
                    $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownType'), [System.Xml.Linq.XCData]::new($Syntax.ToString().Trim()));
                    break;
                }
            }
            $ParentElement.Add($TypeElement);
        }
    }
}

Function Import-ParameterSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$ParentElement
    )

    Process {
        switch ($Syntax) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax] } {
                [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax]$ParameterSyntax = $Syntax;
                $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XAttribute]::new('Name', $ParameterSyntax.Identifier.ValueText.Trim()));
                Import-TypeSyntax -Syntax $ParameterSyntax.Type -ParentElement $TypeElement;
                if ($null -ne $ParameterSyntax.Default) {
                    $TypeElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Default'), [System.Xml.Linq.XText]::new($ParameterSyntax.Default.Value.ToString().Trim())));
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax] } {
                $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('FunctionPointerParameter'), [System.Xml.Linq.XText]::new($Syntax.ToString().Trim()));
                break;
            }
            default {
                $TypeElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownBaseParameter'), [System.Xml.Linq.XText]::new($Syntax.ToString().Trim()));
                break;
            }
        }
        $ParentElement.Add($TypeElement);
    }
}

<#
$Type = [Microsoft.CodeAnalysis.SyntaxToken];
@([Microsoft.CodeAnalysis.SyntaxToken].Assembly.GetTypes()) + @([Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax].Assembly.GetTypes()) | ? { $_.BaseType -eq $Type } | % { $_.FullName }

$Type = [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax];
$BaseProperties = $Type.GetProperties() | % { $_.ToString() }
#$Type.GetProperties() | % { "        [$($_.PropertyType.FullName)]`$$($_.Name) = `$$($Type.Name).$($_.Name);" }
$Type.Assembly.GetTypes() | ? { $_.BaseType -eq $Type } | % {
    $VarName = "`$$($_.Name)";
    $Name = $_.Name;
    if ($Name -ne 'ParameterSyntax') { $Name = $Name -replace '(Parameter)?Syntax$', '' }
    @"
    { `$_ -is [$($_.FullName)] } {
        [$($_.FullName)]$VarName = `$$($Type.Name);
"@
    $_.GetProperties() | ? { $BaseProperties -notcontains $_.ToString() } | % { "        [$($_.PropertyType.FullName)]`$$($_.Name) = $VarName.$($_.Name);" }
    @"
        `$TypeElement = [System.Xml.Linq.XElement]::new(`$Script:ModelNamespace.GetName('$Name'));
        break;
    }
"@
}
#>

Function Import-MemberSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$ParentElement
    )

    Process {
        $MemberElement = $null;
        switch ($Syntax) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax] } {
                $BaseFieldSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$Syntax;
                [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax]$Declaration = $BaseFieldSyntax.Declaration;
                switch ($BaseFieldSyntax) {
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax] } {
                        $FieldSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]$BaseFieldSyntax;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Field'));
                        Import-DocumentationComments -Syntax $FieldSyntax -ParentElement $MemberElement;
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax] } {
                        $EventFieldSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]$BaseFieldSyntax;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('EventField'));
                        Import-DocumentationComments -Syntax $EventFieldSyntax -ParentElement $MemberElement;
                        break;
                    }
                    default {
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownBaseField'));
                        Import-DocumentationComments -Syntax $BaseFieldSyntax -ParentElement $MemberElement;
                        break;
                    }
                }
                Import-TypeSyntax -Syntax $Declaration.Type -ParentElement $MemberElement;
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } {
                $BaseMethodSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$Syntax;
                switch ($BaseMethodSyntax) {
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] } {
                        $ConstructorSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]$BaseMethodSyntax;
                        [Microsoft.CodeAnalysis.SyntaxToken]$Identifier = $ConstructorSyntax.Identifier;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax]$Initializer = $ConstructorSyntax.Initializer;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Constructor'));
                        Import-DocumentationComments -Syntax $ConstructorSyntax -ParentElement $MemberElement;
                        if ($null -ne $BaseMethodSyntax.ParameterList -and $BaseMethodSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $BaseMethodSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } {
                        $ConversionOperatorSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]$BaseMethodSyntax;
                        [Microsoft.CodeAnalysis.SyntaxToken]$ImplicitOrExplicitKeyword = $ConversionOperatorSyntax.ImplicitOrExplicitKeyword;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]$ExplicitInterfaceSpecifier = $ConversionOperatorSyntax.ExplicitInterfaceSpecifier;
                        [Microsoft.CodeAnalysis.SyntaxToken]$OperatorKeyword = $ConversionOperatorSyntax.OperatorKeyword;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type = $ConversionOperatorSyntax.Type;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('ConversionOperator'));
                        Import-DocumentationComments -Syntax $ConversionOperatorSyntax -ParentElement $MemberElement;
                        if ($null -ne $BaseMethodSyntax.ParameterList -and $BaseMethodSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $BaseMethodSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        Import-TypeSyntax -Syntax $Type -ParentElement $MemberElement;
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax] } {
                        $DestructorSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]$BaseMethodSyntax;
                        [Microsoft.CodeAnalysis.SyntaxToken]$Identifier = $DestructorSyntax.Identifier;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Destructor'));
                        Import-DocumentationComments -Syntax $DestructorSyntax -ParentElement $MemberElement;
                        if ($null -ne $BaseMethodSyntax.ParameterList -and $BaseMethodSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $BaseMethodSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } {
                        $MethodSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]$BaseMethodSyntax;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$ReturnType = $MethodSyntax.ReturnType;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]$ExplicitInterfaceSpecifier = $MethodSyntax.ExplicitInterfaceSpecifier;
                        [Microsoft.CodeAnalysis.SyntaxToken]$Identifier = $MethodSyntax.Identifier;
                        [Microsoft.CodeAnalysis.SyntaxList[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax]]$ConstraintClauses = $MethodSyntax.ConstraintClauses;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Method'));
                        Import-DocumentationComments -Syntax $MethodSyntax -ParentElement $MemberElement;
                        if ($null -ne $MethodSyntax.TypeParameterList -and $MethodSyntax.TypeParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                            $MemberElement.Add($XElement);
                            $MethodSyntax.TypeParameterList.Parameters | ForEach-Object {
                                $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                            }
                        }
                        if ($null -ne $BaseMethodSyntax.ParameterList -and $BaseMethodSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $BaseMethodSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } {
                        $OperatorSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]$BaseMethodSyntax;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$ReturnType = $OperatorSyntax.ReturnType;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]$ExplicitInterfaceSpecifier = $OperatorSyntax.ExplicitInterfaceSpecifier;
                        [Microsoft.CodeAnalysis.SyntaxToken]$OperatorKeyword = $OperatorSyntax.OperatorKeyword;
                        [Microsoft.CodeAnalysis.SyntaxToken]$OperatorToken = $OperatorSyntax.OperatorToken;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Operator'));
                        Import-DocumentationComments -Syntax $OperatorSyntax -ParentElement $MemberElement;
                        if ($null -ne $BaseMethodSyntax.ParameterList -and $BaseMethodSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $BaseMethodSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        break;
                    }
                    default {
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownBaseMethod'));
                        Import-DocumentationComments -Syntax $BaseMethodSyntax -ParentElement $MemberElement;
                        if ($null -ne $BaseMethodSyntax.ParameterList -and $BaseMethodSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $BaseMethodSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        break;
                    }
                }
                <#
                if ($null -ne $BaseMethodSyntax.ExpressionBody) {
                    $MemberElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('ExpressionBody'), [System.Xml.Linq.XCData]::new($BaseMethodSyntax.ExpressionBody.ToString())));
                } else {
                    if ($null -ne $BaseMethodSyntax.Body) {
                        $MemberElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Body'), [System.Xml.Linq.XCData]::new($BaseMethodSyntax.Body.ToString())));
                    }
                }
                #>
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } {
                $BasePropertySyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$Syntax;
                [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type = $BasePropertySyntax.Type;
                [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]$ExplicitInterfaceSpecifier = $BasePropertySyntax.ExplicitInterfaceSpecifier;
                switch ($BasePropertySyntax) {
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] } {
                        $EventSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]$BasePropertySyntax;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('EventProperty'), [System.Xml.Linq.XAttribute]::new('Name', $EventSyntax.Identifier.ValueText.Trim()));
                        Import-DocumentationComments -Syntax $EventSyntax -ParentElement $MemberElement;
                        $EventSyntax.AccessorList.Accessors | ForEach-Object {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Accessor'), [System.Xml.Linq.XAttribute]::new('Keyword', $_.Keyword.ValueText.Trim()));
                            $MemberElement.Add($XElement);
                            <#
                            if ($null -ne $_.ExpressionBody) {
                                $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('ExpressionBody')), [System.Xml.Linq.XCData]::new($_.ExpressionBody.ToString()));
                            } else {
                                if ($null -ne $_.Body) {
                                    $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Body')), [System.Xml.Linq.XCData]::new($_.Body.ToString()));
                                }
                            }
                            #>
                        }
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } {
                        $IndexerSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]$BasePropertySyntax;
                        # [Microsoft.CodeAnalysis.SyntaxToken]$ThisKeyword = $IndexerSyntax.ThisKeyword;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Indexer'));
                        Import-DocumentationComments -Syntax $IndexerSyntax -ParentElement $MemberElement;
                        Import-TypeSyntax -Syntax $IndexerSyntax.Type -ParentElement $MemberElement;
                        if ($null -ne $IndexerSyntax.ParameterList -and $IndexerSyntax.ParameterList.Parameters.Count -gt 0) {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                            $MemberElement.Add($XElement);
                            $IndexerSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                        }
                        $IndexerSyntax.AccessorList.Accessors | ForEach-Object {
                            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Accessor'), [System.Xml.Linq.XAttribute]::new('Keyword', $_.Keyword.ValueText.Trim()));
                            $MemberElement.Add($XElement);
                            <#
                            if ($null -ne $_.ExpressionBody) {
                                $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('ExpressionBody')), [System.Xml.Linq.XCData]::new($_.ExpressionBody.ToString()));
                            } else {
                                if ($null -ne $_.Body) {
                                    $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Body')), [System.Xml.Linq.XCData]::new($_.Body.ToString()));
                                }
                            }
                            #>
                        }
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } {
                        $PropertySyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]$BasePropertySyntax;
                        [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax]$Initializer = $PropertySyntax.Initializer;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Property'), [System.Xml.Linq.XAttribute]::new('Name', $PropertySyntax.Identifier.ValueText.Trim()));
                        Import-DocumentationComments -Syntax $PropertySyntax -ParentElement $MemberElement;
                        Import-TypeSyntax -Syntax $PropertySyntax.Type -ParentElement $MemberElement;
                        if ($null -ne $PropertySyntax.AccessorList) {
                            $PropertySyntax.AccessorList.Accessors | ForEach-Object {
                                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Accessor'), [System.Xml.Linq.XAttribute]::new('Keyword', $_.Keyword.ValueText.Trim()));
                                $MemberElement.Add($XElement);
                                <#
                                if ($null -ne $_.ExpressionBody) {
                                    $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('ExpressionBody')), [System.Xml.Linq.XCData]::new($_.ExpressionBody.ToString()));
                                } else {
                                    if ($null -ne $_.Body) {
                                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Body')), [System.Xml.Linq.XCData]::new($_.Body.ToString()));
                                    }
                                }
                                #>
                            }
                        }
                        break;
                    }
                    default {
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownBaseProperty'));
                        Import-DocumentationComments -Syntax $BasePropertySyntax -ParentElement $MemberElement;
                        break;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax] } {
                $DelegateSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Syntax;
                [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$ReturnType = $DelegateSyntax.ReturnType;
                [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax]$ParameterList = $Syntax.ParameterList;
                [Microsoft.CodeAnalysis.SyntaxList[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax]]$ConstraintClauses = $DelegateSyntax.ConstraintClauses;
                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Delegate'), [System.Xml.Linq.XAttribute]::new('Name', $DelegateSyntax.Identifier.ValueText.Trim()));
                Import-DocumentationComments -Syntax $DelegateSyntax -ParentElement $MemberElement;
                if ($null -ne $DelegateSyntax.TypeParameterList -and $DelegateSyntax.TypeParameterList.Parameters.Count -gt 0) {
                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                    $MemberElement.Add($XElement);
                    $DelegateSyntax.TypeParameterList.Parameters | ForEach-Object {
                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                    }
                }
                if ($null -ne $DelegateSyntax.ParameterList -and $DelegateSyntax.ParameterList.Parameters.Count -gt 0) {
                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                    $MemberElement.Add($XElement);
                    $DelegateSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax] } {
                $EnumMemberSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax]$Syntax;
                [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax]$EqualsValue = $EnumMemberSyntax.EqualsValue;
                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Enum'), [System.Xml.Linq.XAttribute]::new('Name', $DelegateSyntax.Identifier.ValueText.Trim()));
                Import-DocumentationComments -Syntax $EnumMemberSyntax -ParentElement $MemberElement;
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax] } {
                $GlobalStatementSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax]$Syntax;
                [Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax]$Statement = $GlobalStatementSyntax.Statement;
                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GlobalStatement'));
                Import-DocumentationComments -Syntax $GlobalStatementSyntax -ParentElement $MemberElement;
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } {
                $NamespaceSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$Syntax;
                #[Microsoft.CodeAnalysis.SyntaxList[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax]]$Externs = $NamespaceSyntax.Externs;
                [Microsoft.CodeAnalysis.SyntaxList[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]]$Members = $NamespaceSyntax.Members;
                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Namespace'), [System.Xml.Linq.XAttribute]::new('Name', $NamespaceSyntax.Name.ToString().Trim()));
                if ($NamespaceSyntax -is [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax]) {
                    $MemberElement.Add([System.Xml.Linq.XAttribute]::new('FileScoped', $true));
                }
                Import-DocumentationComments -Syntax $NamespaceSyntax -ParentElement $MemberElement;
                if ($null -ne $NamespaceSyntax.Usings) {
                    foreach ($UsingDirective in $NamespaceSyntax.Usings) {
                        $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Using'), [System.Xml.Linq.XAttribute]::new('Name', $UsingDirective.Name.GetText().ToString().Trim()));
                        $MemberElement.Add($XElement);
                        if ($null -ne $UsingDirective.Alias) {
                            $XElement.Add([System.Xml.Linq.XAttribute]::new('Alias', $UsingDirective.Alias.Name.Identifier.ValueText.Trim()));
                        }
                        $Text = $UsingDirective.StaticKeyword.ValueText.Trim();
                        if (-not [string]::IsNullOrEmpty($Text)) {
                            $XElement.Add([System.Xml.Linq.XAttribute]::new('Static', $true));
                        }
                    }
                }
                if ($null -ne $NamespaceSyntax.Members) {
                    Write-Host -Object $NamespaceSyntax.Name.ToString() -ForegroundColor Cyan;
                    $NamespaceSyntax.Members | Import-MemberSyntax -ParentElement $MemberElement;
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] } {
                $BaseTypeSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]$Syntax;
                switch ($BaseTypeSyntax) {
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax] } {
                        $TypeSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]$BaseTypeSyntax;
                        [Microsoft.CodeAnalysis.SyntaxList[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax]]$ConstraintClauses = $TypeSyntax.ConstraintClauses;
                        switch ($TypeSyntax) {
                            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] } {
                                $RecordSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]$TypeSyntax;
                                [Microsoft.CodeAnalysis.SyntaxToken]$ClassOrStructKeyword = $RecordSyntax.ClassOrStructKeyword;
                                [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax]$ParameterList = $RecordSyntax.ParameterList;
                                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Record'), [System.Xml.Linq.XAttribute]::new('Name', $RecordSyntax.Identifier.ValueText.Trim()));
                                Import-DocumentationComments -Syntax $RecordSyntax -ParentElement $MemberElement;
                                if ($null -ne $TypeSyntax.TypeParameterList -and $TypeSyntax.TypeParameterList.Parameters.Count -gt 0) {
                                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                                    $MemberElement.Add($XElement);
                                    $TypeSyntax.TypeParameterList.Parameters | ForEach-Object {
                                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                                    }
                                }
                                if ($null -ne $RecordSyntax.ParameterList -and $RecordSyntax.ParameterList.Parameters.Count -gt 0) {
                                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameters'));
                                    $MemberElement.Add($XElement);
                                    $RecordSyntax.ParameterList.Parameters | Import-ParameterSyntax -ParentElement $XElement;
                                }
                                break;
                            }
                            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] } {
                                $ClassSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$TypeSyntax;
                                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Class'), [System.Xml.Linq.XAttribute]::new('Name', $ClassSyntax.Identifier.ValueText.Trim()));
                                Import-DocumentationComments -Syntax $ClassSyntax -ParentElement $MemberElement;
                                if ($null -ne $TypeSyntax.TypeParameterList -and $TypeSyntax.TypeParameterList.Parameters.Count -gt 0) {
                                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                                    $MemberElement.Add($XElement);
                                    $TypeSyntax.TypeParameterList.Parameters | ForEach-Object {
                                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                                    }
                                }
                                break;
                            }
                            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] } {
                                $StructSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]$TypeSyntax;
                                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Struct'), [System.Xml.Linq.XAttribute]::new('Name', $StructSyntax.Identifier.ValueText.Trim()));
                                Import-DocumentationComments -Syntax $StructSyntax -ParentElement $MemberElement;
                                if ($null -ne $TypeSyntax.TypeParameterList -and $TypeSyntax.TypeParameterList.Parameters.Count -gt 0) {
                                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                                    $MemberElement.Add($XElement);
                                    $TypeSyntax.TypeParameterList.Parameters | ForEach-Object {
                                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                                    }
                                }
                                break;
                            }
                            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] } {
                                $InterfaceSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$TypeSyntax;
                                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Interface'), [System.Xml.Linq.XAttribute]::new('Name', $InterfaceSyntax.Identifier.ValueText.Trim()));
                                Import-DocumentationComments -Syntax $InterfaceSyntax -ParentElement $MemberElement;
                                if ($null -ne $TypeSyntax.TypeParameterList -and $TypeSyntax.TypeParameterList.Parameters.Count -gt 0) {
                                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                                    $MemberElement.Add($XElement);
                                    $TypeSyntax.TypeParameterList.Parameters | ForEach-Object {
                                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                                    }
                                }
                                break;
                            }
                            default {
                                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownType'));
                                Import-DocumentationComments -Syntax $TypeSyntax -ParentElement $MemberElement;
                                if ($null -ne $TypeSyntax.TypeParameterList -and $TypeSyntax.TypeParameterList.Parameters.Count -gt 0) {
                                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('GenericArguments'));
                                    $MemberElement.Add($XElement);
                                    $TypeSyntax.TypeParameterList.Parameters | ForEach-Object {
                                        $XElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Parameter'), [System.Xml.Linq.XText]::new($_.ToString().Trim())));
                                    }
                                }
                                $MemberElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Syntax'), [System.Xml.Linq.XText]::new($TypeSyntax.ToString().Trim())));
                                break;
                            }
                        }
                        if ($null -ne $TypeSyntax.Members) {
                            $TypeSyntax.Members | Import-MemberSyntax -ParentElement $MemberElement;
                        }
                        break;
                    }
                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] } {
                        $EnumSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$BaseTypeSyntax;
                        [Microsoft.CodeAnalysis.SeparatedSyntaxList[Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax]]$Members = $EnumSyntax.Members;
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Enum'), [System.Xml.Linq.XAttribute]::new('Name', $EnumSyntax.Identifier.ValueText.Trim()));
                        Import-DocumentationComments -Syntax $EnumSyntax -ParentElement $MemberElement;
                        break;
                    }
                    default {
                        $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownBaseType'), [System.Xml.Linq.XAttribute]::new('Name', $BaseTypeSyntax.Identifier.ValueText.Trim()));
                        Import-DocumentationComments -Syntax $BaseTypeSyntax -ParentElement $MemberElement;
                        $MemberElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Syntax'), [System.Xml.Linq.XText]::new($BaseTypeSyntax.ToString().Trim())));
                        break;
                    }
                }
                if ($null -ne $BaseTypeSyntax.BaseList -and $BaseTypeSyntax.BaseList.Types.Count -gt 0) {
                    $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('BaseTypes'));
                    $MemberElement.Add($XElement);
                    $BaseTypeSyntax.BaseList.Types | ForEach-Object { $_.Type } | Import-TypeSyntax -ParentElement $XElement;
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax] } {
                $IncompleteMemberSyntax = [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax]$Syntax;
                [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type = $IncompleteMemberSyntax.Type;
                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('IncompleteMember'));
                Import-DocumentationComments -Syntax $IncompleteMemberSyntax -ParentElement $EnumSyntax;
                break;
            }
            default {
                $MemberElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('UnknownMember'));
                Import-DocumentationComments -Syntax $Syntax -ParentElement $EnumSyntax;
                $MemberElement.Add([System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Syntax'), [System.Xml.Linq.XText]::new($Syntax.ToString().Trim())));
                break;
            }
        }
        $ParentElement.Add($MemberElement);
    }
}
Function Import-SourceFile {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$LiteralPath
    )

    Process {
        $Text = '';
        Write-Host -Object $LiteralPath -ForegroundColor Cyan;
        $FileInfo = [System.IO.FileInfo]::new($LiteralPath);
        $StreamReader = $FileInfo.OpenText();
        try { $Text = $StreamReader.ReadToEnd(); }
        finally { $StreamReader.Close(); }
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
        $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
        $FileElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('File'), [System.Xml.Linq.XAttribute]::new('Name', [System.IO.Path]::GetFileNameWithoutExtension($FileInfo.Name)));
        $Script:ModelDocument.Root.Add($FileElement);
        if ($null -ne $CompilationUnitRoot.Usings) {
            foreach ($UsingDirective in $CompilationUnitRoot.Usings) {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelNamespace.GetName('Using'), [System.Xml.Linq.XAttribute]::new('Name', $UsingDirective.Name.GetText().ToString().Trim()));
                $FileElement.Add($XElement);
                if ($null -ne $UsingDirective.Alias) {
                    $XElement.Add([System.Xml.Linq.XAttribute]::new('Alias', $UsingDirective.Alias.Name.Identifier.ToString().Trim()));
                }
                $Text = $UsingDirective.StaticKeyword.ValueText.Trim();
                if (-not [string]::IsNullOrEmpty($Text)) {
                    $XElement.Add([System.Xml.Linq.XAttribute]::new('Static', $true));
                }
            }
        }
        $CompilationUnitRoot.Members | Import-MemberSyntax -ParentElement $FileElement;
    }
}

#'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat\Model\IMediaPropertiesListItem.cs' | Import-SourceFile;
@('FsInfoCat\Model', 'FsInfoCat\Local', 'FsInfoCat\Upstream') | ForEach-Object {
    [System.IO.Directory]::GetFiles([System.IO.Path]::Combine($PSSCriptRoot, '..', $_), '*.cs') | Import-SourceFile -ErrorAction Stop;
}

$Writer = [System.Xml.XmlWriter]::Create(($PSScriptRoot | Join-Path -ChildPath 'ModelDefinitions.xml'), [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
});
try {
    $Script:ModelDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}
<#
[System.IO.Directory]::GetFiles([System.IO.Path]::Combine($PSSCriptRoot, '..\FsInfoCat\Model'), '*.cs') | Import-SourceFile;
#>
