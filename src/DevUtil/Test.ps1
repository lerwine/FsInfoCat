Function Import-Expression {
    # [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.StackAllocArrayCreationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.TupleExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.PrefixUnaryExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.PostfixUnaryExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.MemberBindingExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ElementBindingExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.RangeExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitElementAccessSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.InstanceExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.MakeRefExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.RefValueExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.CheckedExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.SizeOfExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.RefExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.InitializerExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.BaseObjectCreationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.WithExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitArrayCreationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitStackAllocArrayCreationExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedArraySizeExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.IsPatternExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ThrowExpressionSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionSyntax], []
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax] } { Import-AnonymousFunctionExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StackAllocArrayCreationExpressionSyntax] } { Import-StackAllocArrayCreationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax] } { Import-Type -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax] } { Import-ParenthesizedExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleExpressionSyntax] } { Import-TupleExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PrefixUnaryExpressionSyntax] } { Import-PrefixUnaryExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax] } { Import-AwaitExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PostfixUnaryExpressionSyntax] } { Import-PostfixUnaryExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax] } { Import-MemberAccessExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax] } { Import-ConditionalAccessExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberBindingExpressionSyntax] } { Import-MemberBindingExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElementBindingExpressionSyntax] } { Import-ElementBindingExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RangeExpressionSyntax] } { Import-RangeExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitElementAccessSyntax] } { Import-ImplicitElementAccess -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax] } { Import-BinaryExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax] } { Import-AssignmentExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalExpressionSyntax] } { Import-ConditionalExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InstanceExpressionSyntax] } { Import-InstanceExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax] } { Import-LiteralExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MakeRefExpressionSyntax] } { Import-MakeRefExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeExpressionSyntax] } { Import-RefTypeExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefValueExpressionSyntax] } { Import-RefValueExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CheckedExpressionSyntax] } { Import-CheckedExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax] } { Import-DefaultExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax] } { Import-TypeOfExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SizeOfExpressionSyntax] } { Import-SizeOfExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax] } { Import-InvocationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax] } { Import-ElementAccessExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationExpressionSyntax] } { Import-DeclarationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax] } { Import-CastExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefExpressionSyntax] } { Import-RefExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InitializerExpressionSyntax] } { Import-InitializerExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseObjectCreationExpressionSyntax] } { Import-BaseObjectCreationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WithExpressionSyntax] } { Import-WithExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax] } { Import-AnonymousObjectCreationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax] } { Import-ArrayCreationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitArrayCreationExpressionSyntax] } { Import-ImplicitArrayCreationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitStackAllocArrayCreationExpressionSyntax] } { Import-ImplicitStackAllocArrayCreationExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax] } { Import-QueryExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedArraySizeExpressionSyntax] } { Import-OmittedArraySizeExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax] } { Import-InterpolatedStringExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IsPatternExpressionSyntax] } { Import-IsPatternExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ThrowExpressionSyntax] } { Import-ThrowExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionSyntax] } { Import-SwitchExpression -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-Expression -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownExpression'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-SyntaxNode -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-SyntaxNode -Syntax $Syntax -MemberElement $MemberElement;
            }
        }
    }
}

Function Import-FieldDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Field'));

        Import-BaseFieldDeclaration -Syntax $Syntax -MemberElement $MemberElement;
    }
}

Function Import-EventFieldDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('EventField'));

        Import-BaseFieldDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.EventKeywordPropertyName -ParentElement $MemberElement;
    }
}

Function Import-BaseFieldDeclaration {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax] } { Import-FieldDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax] } { Import-EventFieldDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-BaseFieldDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownBaseField'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax]$Declaration;
            # Import-Type -Syntax $Syntax.Declaration -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxToken]$SemicolonToken;
            # Import-Type -Syntax $Syntax.SemicolonToken -ParentElement $MemberElement;
        }
    }
}

Function Import-ConstructorDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Constructor'));

        Import-BaseMethodDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax]
        # Import-Type -Syntax $Syntax.InitializerPropertyName -ParentElement $MemberElement;
    }
}

Function Import-ConversionOperatorDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('ConversionOperator'));

        Import-BaseMethodDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.ImplicitOrExplicitKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]
        # Import-Type -Syntax $Syntax.ExplicitInterfaceSpecifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.OperatorKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]
        # Import-Type -Syntax $Syntax.TypePropertyName -ParentElement $MemberElement;
    }
}

Function Import-DestructorDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Destructor'));

        Import-BaseMethodDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.TildeTokenPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;
    }
}

Function Import-MethodDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Method'));

        Import-BaseMethodDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [System.Int32]
        # Import-Type -Syntax $Syntax.ArityPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]
        # Import-Type -Syntax $Syntax.ReturnTypePropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]
        # Import-Type -Syntax $Syntax.ExplicitInterfaceSpecifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax]
        # Import-Type -Syntax $Syntax.TypeParameterListPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]
        # Import-Type -Syntax $Syntax.ConstraintClausesPropertyName -ParentElement $MemberElement;
    }
}

Function Import-OperatorDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Operator'));

        Import-BaseMethodDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]
        # Import-Type -Syntax $Syntax.ReturnTypePropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]
        # Import-Type -Syntax $Syntax.ExplicitInterfaceSpecifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.OperatorKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.OperatorTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-BaseMethodDeclaration {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] } { Import-ConstructorDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } { Import-ConversionOperatorDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax] } { Import-DestructorDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } { Import-MethodDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } { Import-OperatorDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-BaseMethodDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownBaseMethod'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax]$ParameterList;
            # Import-Type -Syntax $Syntax.ParameterList -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax]$Body;
            # Import-Type -Syntax $Syntax.Body -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax]$ExpressionBody;
            # Import-Type -Syntax $Syntax.ExpressionBody -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxToken]$SemicolonToken;
            # Import-Type -Syntax $Syntax.SemicolonToken -ParentElement $MemberElement;
        }
    }
}

Function Import-EventDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Event'));

        Import-BasePropertyDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.EventKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-IndexerDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Indexer'));

        Import-BasePropertyDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.ThisKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax]
        # Import-Type -Syntax $Syntax.ParameterListPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax]
        # Import-Type -Syntax $Syntax.ExpressionBodyPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-PropertyDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Property'));

        Import-BasePropertyDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax]
        # Import-Type -Syntax $Syntax.ExpressionBodyPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax]
        # Import-Type -Syntax $Syntax.InitializerPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-BasePropertyDeclaration {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] } { Import-EventDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } { Import-IndexerDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } { Import-PropertyDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-BasePropertyDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownBaseProperty'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type;
            # Import-Type -Syntax $Syntax.Type -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax]$ExplicitInterfaceSpecifier;
            # Import-Type -Syntax $Syntax.ExplicitInterfaceSpecifier -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax]$AccessorList;
            # Import-Type -Syntax $Syntax.AccessorList -ParentElement $MemberElement;
        }
    }
}

Function Import-DelegateDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Delegate'));

        Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [System.Int32]
        # Import-Type -Syntax $Syntax.ArityPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.DelegateKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]
        # Import-Type -Syntax $Syntax.ReturnTypePropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax]
        # Import-Type -Syntax $Syntax.TypeParameterListPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax]
        # Import-Type -Syntax $Syntax.ParameterListPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]
        # Import-Type -Syntax $Syntax.ConstraintClausesPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-EnumMemberDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('EnumMember'));

        Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.IdentifierPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax]
        # Import-Type -Syntax $Syntax.EqualsValuePropertyName -ParentElement $MemberElement;
    }
}

Function Import-GlobalStatement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('GlobalStatement'));

        Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax]
        # Import-Type -Syntax $Syntax.StatementPropertyName -ParentElement $MemberElement;
    }
}

Function Import-NamespaceDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Namespace'));

        Import-BaseNamespaceDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.OpenBraceTokenPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.CloseBraceTokenPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-FileScopedNamespaceDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('FileScopedNamespace'));

        Import-BaseNamespaceDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.SemicolonTokenPropertyName -ParentElement $MemberElement;
    }
}

Function Import-BaseNamespaceDeclaration {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } { Import-NamespaceDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax] } { Import-FileScopedNamespaceDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-BaseNamespaceDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownBaseNamespace'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [Microsoft.CodeAnalysis.SyntaxToken]$NamespaceKeyword;
            # Import-Type -Syntax $Syntax.NamespaceKeyword -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$Name;
            # Import-Type -Syntax $Syntax.Name -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]$Externs;
            # Import-Type -Syntax $Syntax.Externs -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]$Usings;
            # Import-Type -Syntax $Syntax.Usings -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]$Members;
            # Import-Type -Syntax $Syntax.Members -ParentElement $MemberElement;
        }
    }
}

Function Import-TypeDeclaration {
    # [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax], [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax], []
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] } { Import-RecordDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] } { Import-ClassDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] } { Import-StructDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] } { Import-InterfaceDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-TypeDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownType'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-BaseTypeDeclaration -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-BaseTypeDeclaration -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [System.Int32]$Arity;
            # Import-Type -Syntax $Syntax.Arity -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxToken]$Keyword;
            # Import-Type -Syntax $Syntax.Keyword -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax]$TypeParameterList;
            # Import-Type -Syntax $Syntax.TypeParameterList -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]$ConstraintClauses;
            # Import-Type -Syntax $Syntax.ConstraintClauses -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]$Members;
            # Import-Type -Syntax $Syntax.Members -ParentElement $MemberElement;
        }
    }
}

Function Import-EnumDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('Enum'));

        Import-BaseTypeDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.SyntaxToken]
        # Import-Type -Syntax $Syntax.EnumKeywordPropertyName -ParentElement $MemberElement;

        # [Microsoft.CodeAnalysis.SeparatedSyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]
        # Import-Type -Syntax $Syntax.MembersPropertyName -ParentElement $MemberElement;
    }
}

Function Import-BaseTypeDeclaration {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax] } { Import-TypeDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] } { Import-EnumDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-BaseTypeDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownBaseType'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [Microsoft.CodeAnalysis.SyntaxToken]$Identifier;
            # Import-Type -Syntax $Syntax.Identifier -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax]$BaseList;
            # Import-Type -Syntax $Syntax.BaseList -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxToken]$OpenBraceToken;
            # Import-Type -Syntax $Syntax.OpenBraceToken -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxToken]$CloseBraceToken;
            # Import-Type -Syntax $Syntax.CloseBraceToken -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxToken]$SemicolonToken;
            # Import-Type -Syntax $Syntax.SemicolonToken -ParentElement $MemberElement;
        }
    }
}

Function Import-IncompleteMember {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax]$Syntax,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin { $OwnerDocument = $ParentElement.OwnerDocument }

    Process {
        $MemberElement = $ParentElement.AppendChild($OwnerDocument.CreateElement('IncompleteMember'));

        Import-MemberDeclaration -Syntax $Syntax -MemberElement $MemberElement;

        # [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]
        # Import-Type -Syntax $Syntax.TypePropertyName -ParentElement $MemberElement;
    }
}

Function Import-MemberDeclaration {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax] } { Import-BaseFieldDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } { Import-BaseMethodDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } { Import-BasePropertyDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax] } { Import-DelegateDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax] } { Import-EnumMemberDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax] } { Import-GlobalStatement -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } { Import-BaseNamespaceDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] } { Import-BaseTypeDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax] } { Import-IncompleteMember -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-MemberDeclaration -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownMember'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                Import-SyntaxNode -Syntax $Syntax -MemberElement $MemberElement -IsUnknown;
            } else {
                Import-SyntaxNode -Syntax $Syntax -MemberElement $MemberElement;
            }

            # [Microsoft.CodeAnalysis.SyntaxList`1[[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax, Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]]$AttributeLists;
            # Import-Type -Syntax $Syntax.AttributeLists -ParentElement $MemberElement;

            # [Microsoft.CodeAnalysis.SyntaxTokenList]$Modifiers;
            # Import-Type -Syntax $Syntax.Modifiers -ParentElement $MemberElement;
        }
    }
}

Function Import-SyntaxNode {
    [CmdletBinding(DefaultParameterSetName = 'ToParent')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]$Syntax,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToParent')]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'ToMember')]
        [System.Xml.XmlElement]$MemberElement,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$IsUnknown,

        [Parameter(ParameterSetName = 'ToMember')]
        [switch]$PassThru
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'ToParent') {
            switch ($Syntax) {
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax] } { Import-Argument -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax] } { Import-ArrayRankSpecifier -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax] } { Import-Attribute -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax] } { Import-AttributeTargetSpecifier -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax] } { Import-CompilationUnit -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax] } { Import-CrefParameter -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax] } { Import-AccessorDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] } { Import-StructuredTrivia -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax] } { Import-Subpattern -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax] } { Import-UsingDirective -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax] } { Import-TypeArgumentList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax] } { Import-FunctionPointerParameterList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax] } { Import-FunctionPointerCallingConvention -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionListSyntax] } { Import-FunctionPointerUnmanagedCallingConventionList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionSyntax] } { Import-FunctionPointerUnmanagedCallingConvention -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax] } { Import-TupleElement -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax] } { Import-ExpressionOrPattern -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax] } { Import-BaseArgumentList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionColonSyntax] } { Import-BaseExpressionColon -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax] } { Import-AnonymousObjectMemberDeclarator -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax] } { Import-QueryClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SelectOrGroupClauseSyntax] } { Import-SelectOrGroupClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax] } { Import-QueryBody -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.JoinIntoClauseSyntax] } { Import-JoinIntoClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax] } { Import-Ordering -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryContinuationSyntax] } { Import-QueryContinuation -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WhenClauseSyntax] } { Import-WhenClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PositionalPatternClauseSyntax] } { Import-PositionalPatternClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyPatternClauseSyntax] } { Import-PropertyPatternClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax] } { Import-InterpolatedStringContent -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationAlignmentClauseSyntax] } { Import-InterpolationAlignmentClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax] } { Import-InterpolationFormatClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax] } { Import-Statement -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax] } { Import-VariableDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax] } { Import-VariableDeclarator -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax] } { Import-EqualsValueClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDesignationSyntax] } { Import-VariableDesignation -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElseClauseSyntax] } { Import-ElseClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax] } { Import-SwitchSection -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax] } { Import-SwitchLabel -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax] } { Import-SwitchExpressionArm -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax] } { Import-CatchClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchDeclarationSyntax] } { Import-CatchDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax] } { Import-CatchFilterClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FinallyClauseSyntax] } { Import-FinallyClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax] } { Import-ExternAliasDirective -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax] } { Import-MemberDeclaration -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] } { Import-AttributeList -Syntax $Syntax -ParentElement $ParentElement; break; }
                { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax] } { Import-AttributeArgumentList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax] } { Import-AttributeArgument -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax] } { Import-NameEquals -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax] } { Import-TypeParameterList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax] } { Import-TypeParameter -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax] } { Import-BaseList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax] } { Import-BaseType -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] } { Import-TypeParameterConstraintClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax] } { Import-TypeParameterConstraint -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax] } { Import-ExplicitInterfaceSpecifier -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax] } { Import-ConstructorInitializer -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax] } { Import-ArrowExpressionClause -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax] } { Import-AccessorList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax] } { Import-BaseParameterList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax] } { Import-BaseParameter -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefSyntax] } { Import-Cref -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseCrefParameterListSyntax] } { Import-BaseCrefParameterList -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax] } { Import-XmlNode -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementStartTagSyntax] } { Import-XmlElementStartTag -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementEndTagSyntax] } { Import-XmlElementEndTag -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameSyntax] } { Import-XmlName -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlPrefixSyntax] } { Import-XmlPrefix -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax] } { Import-XmlAttribute -Syntax $Syntax -ParentElement $ParentElement; break; }
                #{ $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LineDirectivePositionSyntax] } { Import-LineDirectivePosition -Syntax $Syntax -ParentElement $ParentElement; break; }
                default {
                    Import-SyntaxNode -Syntax $Syntax -MemberElement ($ParentElement.AppendChild($ParentElement.OwnerDocument.CreateElement('UnknownSyntaxNode'))) -IsUnknown;
                    break;
                }
            }
        } else {
            if ($IsUnknown.IsPresent) {
                $XmlElement.Attributes.Append($ParentElement.OwnerDocument.CreateElement('Kind')).Value = [Enum]::GetName([Microsoft.CodeAnalysis.CSharp.SyntaxKind], $Syntax.Kind());
                $XmlElement.Attributes.Append($ParentElement.OwnerDocument.CreateElement('RawKind')).Value = [System.Xml.XmlConvert]::ToString($Syntax.RawKind);
            }
            if ($Syntax.HasLeadingTrivia) {

            }
            if ($Syntax.HasTrailingTrivia) {

            }
            if ($PassThru.IsPresent) { Write-Output -InputObject $MemberElement -NoEnumerate }
        }
    }
}

$NamespaceIds = [System.Collections.Generic.Dictionary[string,[Guid]]]::new();
$CoreEnums = [System.Collections.Generic.Dictionary[string,ParsedEnum]]::new();
$CoreEntities = [System.Collections.Generic.Dictionary[string,ParsedInterface]]::new();
$LocalEnums = [System.Collections.Generic.Dictionary[string,ParsedEnum]]::new();
$LocalEntities = [System.Collections.Generic.Dictionary[string,ParsedInterface]]::new();
$UpstreamEntities = [System.Collections.Generic.Dictionary[string,ParsedInterface]]::new();
$UpstreamEnums = [System.Collections.Generic.Dictionary[string,ParsedEnum]]::new();
[Xml]$XmlDocument = '<EntityData/>';
[System.IO.Directory]::GetFiles('C:\Users\lerwi\source\repos\FsInfoCat\src\FsInfoCat\Model', '*.cs') | ForEach-Object {
    $FileInfo = [System.IO.FileInfo]::new($_);
    $StreamReader = $FileInfo.OpenText();
    try { $Text = $StreamReader.ReadToEnd(); }
    finally { $StreamReader.Close(); }
    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
    $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
    $CompilationUnitRoot.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } | ForEach-Object {
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$NamespaceDeclaration = $_;
        $Namespace = $NamespaceDeclaration.Name.ToString();
        $NsId = [Guid]::Empty;
        if ($NamespaceIds.ContainsKey($Namespace)) {
            $NsId = $NamespaceIds[$Namespace];
        } else {
            $NsId = [Guid]::NewGuid();
            $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('Namespace'));
            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('ID')).Value = [System.Xml.XmlConvert]::ToString($NsId);
        }
        $NamespaceDeclaration.Members | ForEach-Object {
            if ($_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]) {
                $ParsedInterface = [ParsedInterface]::new($NamespaceDeclaration, $_);
                $XmlElement
                $CoreEntities.Add($ParsedInterface.Name, $ParsedInterface);
            } else {
                if ($_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) {
                    $ParsedEnum = [ParsedEnum]::new($NamespaceDeclaration, $_);
                    $CoreEnums.Add($ParsedEnum.Name, $ParsedEnum);
                }
            }

            #if ($null -ne $InterfaceDeclarationSyntax.BaseList) {
            #    ([Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$_).BaseList.Types | ForEach-Object { $_.GetType().FullName }
            #}
            #([Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$_).Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] }
        }
    }
}

[System.IO.Directory]::GetFiles('C:\Users\lerwi\source\repos\FsInfoCat\src\FsInfoCat\Local', '*.cs') | ForEach-Object {
    $FileInfo = [System.IO.FileInfo]::new($_);
    $StreamReader = $FileInfo.OpenText();
    try { $Text = $StreamReader.ReadToEnd(); }
    finally { $StreamReader.Close(); }
    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
    $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
    $CompilationUnitRoot.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } | ForEach-Object {
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$NamespaceDeclaration = $_;
        $NsId = [Guid]::Empty;
        if ($NamespaceIds.ContainsKey($Namespace)) {
            $NsId = $NamespaceIds[$Namespace];
        } else {
            $NsId = [Guid]::NewGuid();
            $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('Namespace'));
            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('ID')).Value = [System.Xml.XmlConvert]::ToString($NsId);
        }
        $NamespaceDeclaration.Members | ForEach-Object {
            if ($_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]) {
                $ParsedInterface = [ParsedInterface]::new($NamespaceDeclaration, $_);
                $LocalEntities.Add($ParsedInterface.Name, $ParsedInterface);
            } else {
                if ($_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) {
                    $ParsedEnum = [ParsedEnum]::new($NamespaceDeclaration, $_);
                    $LocalEnums.Add($ParsedEnum.Name, $ParsedEnum);
                }
            }
        }
    }
}

[System.IO.Directory]::GetFiles('C:\Users\lerwi\source\repos\FsInfoCat\src\FsInfoCat\Upstream', '*.cs') | ForEach-Object {
    $FileInfo = [System.IO.FileInfo]::new($_);
    $StreamReader = $FileInfo.OpenText();
    try { $Text = $StreamReader.ReadToEnd(); }
    finally { $StreamReader.Close(); }
    [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $FileInfo.FullName);
    $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot();
    $CompilationUnitRoot.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } | ForEach-Object {
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$NamespaceDeclaration = $_;
        $Namespace = $NamespaceDeclaration.Name.ToString();
        $NsId = [Guid]::Empty;
        if ($NamespaceIds.ContainsKey($Namespace)) {
            $NsId = $NamespaceIds[$Namespace];
        } else {
            $NsId = [Guid]::NewGuid();
            $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('Namespace'));
            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('ID')).Value = [System.Xml.XmlConvert]::ToString($NsId);
        }
        $NamespaceDeclaration.Members | ForEach-Object {
            if ($_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]) {
                $ParsedInterface = [ParsedInterface]::new($NamespaceDeclaration, $_);
                $UpstreamEntities.Add($ParsedInterface.Name, $ParsedInterface);
            } else {
                if ($_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) {
                    $ParsedEnum = [ParsedEnum]::new($NamespaceDeclaration, $_);
                    $UpstreamEnums.Add($ParsedEnum.Name, $ParsedEnum);
                }
            }
        }
    }
}
