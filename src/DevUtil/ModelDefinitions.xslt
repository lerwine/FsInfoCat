<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:md="http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd" xmlns="http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd"
    xsi:schemaLocation="http://www.w3.org/1999/XSL/Transform ../../../PowerShell-Modules/src/XmlUtility/Schemas/Xslt.xsd">
    <xsl:output method="xml" indent="yes" />
    <xsl:template match="/">
        <xsl:element name="ModelDefinitions">
            <xsl:apply-templates select="ModelDefinitions/Sources/Source[not(@Path='Model\DbEntity.DbValidationContext.cs' or @Path='Model\DbEntity.cs' or @Path='Model\BaseDbContext.cs')]" mode="Root" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Source" mode="Root">
        <xsl:element name="Source">
            <xsl:attribute name="Path">
                <xsl:value-of select="@Path" />
            </xsl:attribute>
            <xsl:attribute name="Id">
                <xsl:value-of select="@Id" />
            </xsl:attribute>
            <xsl:apply-templates select="Using" mode="Root" />
            <xsl:apply-templates select="Namespace" mode="Root" />
            <xsl:apply-templates select="*[not(local-name()='Using' or local-name()='Namespace')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Using" mode="Root">
        <xsl:element name="Using">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:if test="not(count(@Alias)=0)">
                <xsl:attribute name="Alias">
                    <xsl:value-of select="@Alias" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@Static)=0)">
                <xsl:attribute name="Static">
                    <xsl:value-of select="@Static" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@Global)=0)">
                <xsl:attribute name="Global">
                    <xsl:value-of select="@Global" />
                </xsl:attribute>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Argument" mode="Display">
        <xsl:choose>
            <xsl:when test="count(Expression)=0">
                <xsl:text>TODO: Need argument transformation for </xsl:text>
                <xsl:value-of select="local-name(*[position()=1])" />
            </xsl:when>
            <xsl:otherwise>
                <xsl:choose>
                    <xsl:when test="count(Expression[@Type='Invocation'])=0">
                        <xsl:text>TODO: Need argument transformation for Expression[@Type='</xsl:text>
                        <xsl:value-of select="@Type" />
                        <xsl:text>']</xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:variable name="property" select="substring-before(substring-after(normalize-space(Expression), 'nameof(Properties.Resources.'), ')')" />
                        <xsl:choose>
                            <xsl:when test="string-length($property)=0">
                                <xsl:text>TODO: Need argument transformation for Expression[@Type='Invocation' and .='</xsl:text>
                                <xsl:value-of select="Expression" />
                                <xsl:text>']</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="$property" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="Attribute[@Name='Display']" mode="Root">
        <xsl:param name="name" />
        <xsl:element name="Display">
            <xsl:attribute name="Name">
                <xsl:choose>
                    <xsl:when test="not(count(Arguments/Argument[@Name='Name'])=0)">
                        <xsl:apply-templates select="Arguments/Argument[@Name='Name']" mode="Display" />
                    </xsl:when>
                    <xsl:when test="not(count(Arguments/Argument[@Name='ShortName'])=0)">
                        <xsl:apply-templates select="Arguments/Argument[@Name='ShortName']" mode="Display" />
                    </xsl:when>
                    <xsl:when test="count(Arguments/Argument[@Name='Description'])=0">
                    <xsl:value-of select="concat('DisplayName_', $name)" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:apply-templates select="Arguments/Argument[@Name='Description']" mode="Display" />
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="ShortName">
                <xsl:choose>
                    <xsl:when test="not(count(Arguments/Argument[@Name='ShortName'])=0)">
                        <xsl:apply-templates select="Arguments/Argument[@Name='ShortName']" mode="Display" />
                    </xsl:when>
                    <xsl:when test="not(count(Arguments/Argument[@Name='Name'])=0)">
                        <xsl:apply-templates select="Arguments/Argument[@Name='Name']" mode="Display" />
                    </xsl:when>
                    <xsl:when test="count(Arguments/Argument[@Name='Description'])=0">
                    <xsl:value-of select="concat('ShortName_', $name)" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:apply-templates select="Arguments/Argument[@Name='Description']" mode="Display" />
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="Description">
                <xsl:choose>
                    <xsl:when test="not(count(Arguments/Argument[@Name='Description'])=0)">
                        <xsl:apply-templates select="Arguments/Argument[@Name='Description']" mode="Display" />
                    </xsl:when>
                    <xsl:when test="not(count(Arguments/Argument[@Name='Name'])=0)">
                        <xsl:apply-templates select="Arguments/Argument[@Name='Name']" mode="Display" />
                    </xsl:when>
                    <xsl:when test="count(Arguments/Argument[@Name='ShortName'])=0">
                    <xsl:value-of select="concat('Description_', $name)" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:apply-templates select="Arguments/Argument[@Name='ShortName']" mode="Display" />
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:for-each select="Arguments/Argument[not(@Name='Name' or @Name='ShortName' or @Name='Description' or @Name='ResourceType')]">
                <xsl:comment>
                    <xsl:text>TODO: Need attribute for Arguments/Argument[@Name='</xsl:text>
                    <xsl:value-of select="@Name" />
                    <xsl:text>']</xsl:text>
                </xsl:comment>
            </xsl:for-each>
            <xsl:for-each select="*[not(local-name()='Arguments')]">
                <xsl:comment>
                    <xsl:text><![CDATA[TODO: Need <xsl:apply-templates select="]]></xsl:text>
                    <xsl:value-of select="local-name()" />
                    <xsl:text><![CDATA[" mode="Root" />]]></xsl:text>
                </xsl:comment>
            </xsl:for-each>
        </xsl:element>
    </xsl:template>
    <xsl:template match="LiteralExpression" mode="Expression">
        <xsl:element name="Literal">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Argument" mode="Attribute">
        <xsl:element name="Argument">
            <xsl:if test="not(count(@Name)=0)">
                <xsl:attribute name="Name">
                    <xsl:value-of select="@Name" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="@*[not(local-name()='Name')]" mode="Unknown" />
            <xsl:apply-templates select="LiteralExpression" mode="Expression" />
            <xsl:apply-templates select="*[not(local-name()='LiteralExpression')]" mode="Unknown">
                <xsl:with-param name="mode">
                    <xsl:text>Types</xsl:text>
                </xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Attribute[@Name='SuppressMessage']" mode="Root">
        <xsl:element name="SuppressMessage">
            <xsl:if test="not(count(Arguments/Argument)=0)">
                <xsl:element name="Arguments">
                    <xsl:apply-templates select="Arguments/Argument" mode="Attribute" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="*[not(local-name()='Arguments')]|@*[not(local-name()='Name')]" mode="Root" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Attribute" mode="Root">
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:template match="Attribute[@Name=']]></xsl:text>
            <xsl:value-of select="@Name" />
            <xsl:text><![CDATA[']" mode="Root" />]]></xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="AttributeList" mode="Root">
        <xsl:param name="name" />
        <xsl:apply-templates select="Attribute[@Name='Display']" mode="Root">
            <xsl:with-param name="name" select="$name" />
        </xsl:apply-templates>
        <xsl:apply-templates select="Attribute[@Name='SuppressMessage']" mode="Root">
            <xsl:with-param name="name" select="$name" />
        </xsl:apply-templates>
        <xsl:apply-templates select="Attribute[not(@Name='DisallowNull' or @Name='Required' or @Name='NotNull' or @Name='SuppressMessage' or @Name='Display' or @Name='Flags' or @Name='MessageCode' or @Name='StatusMessageLevel' or @Name='ErrorCode')]" mode="Root" />
    </xsl:template>
    <xsl:template match="Modifiers" mode="Modifiers">
        <xsl:choose>
            <xsl:when test="not(count(Modifier[.='public'])=0)">
                <xsl:attribute name="Access">
                    <xsl:text>public</xsl:text>
                </xsl:attribute>
            </xsl:when>
            <xsl:when test="not(count(Modifier[.='private'])=0)">
                <xsl:attribute name="Access">
                    <xsl:text>private</xsl:text>
                </xsl:attribute>
            </xsl:when>
            <xsl:when test="not(count(Modifier[.='protected'])=0)">
                <xsl:attribute name="Access">
                    <xsl:choose>
                        <xsl:when test="not(count(Modifier[.='internal'])=0)">
                            <xsl:text>protected internal</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:text>protected</xsl:text>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:attribute>
            </xsl:when>
            <xsl:when test="not(count(Modifier[.='internal'])=0)">
                <xsl:attribute name="Access">
                    <xsl:text>internal</xsl:text>
                </xsl:attribute>
            </xsl:when>
        </xsl:choose>
        <xsl:if test="not(count(Modifier[.='abstract'])=0)">
            <xsl:attribute name="IsAbstract">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='static'])=0)">
            <xsl:attribute name="IsStatic">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='virtual'])=0)">
            <xsl:attribute name="IsVirtual">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='override'])=0)">
            <xsl:attribute name="IsOverride">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='async'])=0)">
            <xsl:attribute name="IsAsync">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='sealed'])=0)">
            <xsl:attribute name="IsSealed">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='readonly'])=0)">
            <xsl:attribute name="IsReadOnly">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(count(Modifier[.='const'])=0)">
            <xsl:attribute name="IsConstant">
                <xsl:text>true</xsl:text>
            </xsl:attribute>
        </xsl:if>
        <xsl:variable name="other">
            <xsl:value-of select="normalize-space(Modifier[not(.='public' or .='protected' or .='internal' or .='private' or .='static' or .='abstract' or .='virtual' or .='async' or .='sealed' or .='readonly' or .='const' or .='override')])" />
        </xsl:variable>
        <xsl:if test="not(string-length($other)=0)">
            <xsl:attribute name="OTHER">
                <xsl:value-of select="concat(&quot;TODO: Need translation for Modifier[.='&quot;, $other, &quot;']&quot;)" />
            </xsl:attribute>
        </xsl:if>
    </xsl:template>
    <xsl:template match="para" mode="Documentation">
        <xsl:element name="para">
            <xsl:apply-templates select="@*|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="paramref" mode="Documentation">
        <xsl:element name="paramref">
            <xsl:if test="not(count(@name)=0)">
                <xsl:attribute name="name">
                    <xsl:value-of select="@name" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="@*[not(local-name()='name')]|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="see" mode="Documentation">
        <xsl:element name="see">
            <xsl:choose>
                <xsl:when test="not(count(@cref)=0)">
                    <xsl:attribute name="cref">
                        <xsl:value-of select="@cref" />
                    </xsl:attribute>
                </xsl:when>
                <xsl:when test="not(count(@langword)=0)">
                    <xsl:attribute name="langword">
                        <xsl:value-of select="@langword" />
                    </xsl:attribute>
                </xsl:when>
                <xsl:when test="not(count(@href)=0)">
                    <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                    </xsl:attribute>
                </xsl:when>
            </xsl:choose>
            <xsl:apply-templates select="@*[not(local-name()='cref' or local-name()='langword' or local-name()='href')]|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="param" mode="DocumentationRoot">
        <xsl:element name="param">
            <xsl:attribute name="name">
                <xsl:value-of select="@name" />
            </xsl:attribute>
            <xsl:apply-templates select="@*[not(local-name()='name')]|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="seealso" mode="DocumentationRoot">
        <xsl:element name="seealso">
            <xsl:if test="not(count(@cref)=0)">
                <xsl:attribute name="cref">
                    <xsl:value-of select="@cref" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="@*[not(local-name()='cref')]|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="summary" mode="DocumentationRoot">
        <xsl:element name="summary">
            <xsl:apply-templates select="@*|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="returns" mode="DocumentationRoot">
        <xsl:element name="returns">
            <xsl:apply-templates select="@*|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="value" mode="DocumentationRoot">
        <xsl:element name="value">
            <xsl:apply-templates select="@*|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="exception" mode="DocumentationRoot">
        <xsl:element name="exception">
            <xsl:if test="not(count(@cref)=0)">
                <xsl:attribute name="cref">
                    <xsl:value-of select="@cref" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="@*[not(local-name()='cref')]|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="remarks" mode="DocumentationRoot">
        <xsl:element name="remarks">
            <xsl:apply-templates select="@*|*|text()" mode="Documentation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="DocumentationComment" mode="LeadingTrivia">
        <xsl:element name="Documentation">
            <xsl:apply-templates select="@*" mode="Unknown" />
            <xsl:apply-templates select="summary" mode="DocumentationRoot" />
            <xsl:apply-templates select="param" mode="DocumentationRoot" />
            <xsl:apply-templates select="returns" mode="DocumentationRoot" />
            <xsl:apply-templates select="value" mode="DocumentationRoot" />
            <xsl:apply-templates select="exception" mode="DocumentationRoot" />
            <xsl:apply-templates select="remarks" mode="DocumentationRoot" />
            <xsl:apply-templates select="seealso" mode="DocumentationRoot" />
            <xsl:apply-templates select="*[not(local-name()='value' or local-name()='exception' or local-name()='returns' or local-name()='summary' or local-name()='param' or local-name()='seealso' or local-name()='remarks')]" mode="Unknown">
                <xsl:with-param name="mode">DocumentationRoot</xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="LeadingTrivia" mode="Root">
        <xsl:apply-templates select="DocumentationComment" mode="LeadingTrivia" />
        <xsl:if test="not(count(@*)=0 and count(*[not(local-name()='DocumentationComment' or local-name()='UnknownStructuredTriviaSyntax')])=0)">
            <xsl:element name="LeadingTrivia">
                <xsl:apply-templates select="@*" mode="Unknown" />
                <xsl:apply-templates select="*[not(local-name()='DocumentationComment' or local-name()='UnknownStructuredTriviaSyntax')]" mode="Unknown" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="SimpleBaseType" mode="BaseTypes">
        <xsl:element name="SimpleBaseType">
            <xsl:choose>
                <xsl:when test="count(IdentifierName/@Name)=0 or not(count(@*)=0)">
                    <xsl:apply-templates select="@*" mode="Unknown" />
                    <xsl:apply-templates select="*[not(local-name()='LeadingTrivia')]" mode="Unknown">
                        <xsl:with-param name="mode">BaseTypes</xsl:with-param>
                    </xsl:apply-templates>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:attribute name="Name">
                        <xsl:value-of select="IdentifierName/@Name" />
                    </xsl:attribute>
                    <xsl:apply-templates select="*[not(local-name()='IdentifierName' or local-name()='LeadingTrivia')]" mode="Unknown">
                        <xsl:with-param name="mode">BaseTypes</xsl:with-param>
                    </xsl:apply-templates>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:element>
    </xsl:template>
    <xsl:template match="IdentifierName" mode="Types">
        <xsl:element name="Identifier">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="@*[not(local-name()='Name' or local-name()='Arity')]" mode="Unknown" />
            <xsl:apply-templates select="*[not(local-name()='LeadingTrivia')]" mode="Unknown">
                <xsl:with-param name="mode">
                    <xsl:text>Types</xsl:text>
                </xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="PredefinedType" mode="Types">
        <xsl:element name="PredefinedType">
            <xsl:attribute name="Keyword">
                <xsl:value-of select="@Keyword" />
            </xsl:attribute>
            <xsl:apply-templates select="@*[not(local-name()='Keyword')]" mode="Unknown" />
            <xsl:apply-templates select="*[not(local-name()='LeadingTrivia')]" mode="Unknown">
                <xsl:with-param name="mode">
                    <xsl:text>Types</xsl:text>
                </xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="GenericName" mode="Types">
        <xsl:element name="Generic">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="@*[not(local-name()='Name' or local-name()='Arity')]" mode="Unknown" />
            <xsl:apply-templates select="PredefinedType|IdentifierName|GenericName" mode="Types" />
            <xsl:if test="not(count(Arguments/*)=0)">
                <xsl:element name="Arguments">
                    <xsl:apply-templates select="Arguments/*" mode="Unknown">
                        <xsl:with-param name="mode">
                            <xsl:text>Types</xsl:text>
                        </xsl:with-param>
                    </xsl:apply-templates>
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="*[not(local-name()='PredefinedType' or local-name()='GenericName' or local-name()='IdentifierName' or local-name()='Arguments' or local-name()='LeadingTrivia')]" mode="Unknown">
                <xsl:with-param name="mode">
                    <xsl:text>Types</xsl:text>
                </xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Type" mode="MemberType">
        <xsl:element name="Type">
            <xsl:apply-templates select="@*" mode="Unknown" />
            <xsl:apply-templates select="PredefinedType|IdentifierName|GenericName" mode="Types" />
            <xsl:apply-templates select="*[not(local-name()='PredefinedType' or local-name()='GenericName' or local-name()='IdentifierName' or local-name()='Arguments' or local-name()='LeadingTrivia')]" mode="Unknown">
                <xsl:with-param name="mode">
                    <xsl:text>Types</xsl:text>
                </xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="ReturnType" mode="MemberType">
        <xsl:element name="ReturnType">
            <xsl:apply-templates select="@*" mode="Unknown" />
            <xsl:apply-templates select="PredefinedType|IdentifierName|GenericName" mode="Types" />
            <xsl:apply-templates select="*[not(local-name()='PredefinedType' or local-name()='GenericName' or local-name()='IdentifierName' or local-name()='Arguments' or local-name()='LeadingTrivia')]" mode="Unknown">
                <xsl:with-param name="mode">
                    <xsl:text>Types</xsl:text>
                </xsl:with-param>
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Parameter" mode="Root">
        <xsl:element name="Parameter">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:if test="not(count(AttributeLists/AttributeList/Attribute[@Name='NotNull'])=0 and count(AttributeLists/AttributeList/Attribute[@Name='DisallowNull'])=0)">
                <xsl:attribute name="NotNull">
                    <xsl:text>true</xsl:text>
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:apply-templates select="Type" mode="MemberType" />
            <xsl:apply-templates select="@*[not(local-name()='Name')]|*[not(local-name()='Type' or local-name()='AttributeLists')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Explicit" mode="MemberType">
        <xsl:if test="not(count(@*)=0 and count(*)=0)">
            <xsl:element name="Explicit">
                <xsl:apply-templates select="@*" mode="Unknown" />
                <xsl:apply-templates select="*" mode="Unknown">
                    <xsl:with-param name="mode">
                        <xsl:text>Types</xsl:text>
                    </xsl:with-param>
                </xsl:apply-templates>
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="Field" mode="Members">
        <xsl:element name="Field">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="Modifiers" mode="Modifiers" />
            <xsl:apply-templates select="@*[not(local-name()='Name')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:apply-templates select="PredefinedType|IdentifierName|GenericName" mode="Types" />
            <xsl:apply-templates select="*[not(local-name()='IdentifierName' or local-name()='GenericName' or local-name()='PredefinedType' or local-name()='Modifiers' or local-name()='LeadingTrivia' or local-name()='AttributeLists')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Constructor" mode="Members">
        <xsl:element name="Constructor">
            <xsl:apply-templates select="Modifiers" mode="Modifiers" />
            <xsl:apply-templates select="@*[not(local-name()='Name')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:if test="not(count(Parameters/Parameter)=0)">
                <xsl:element name="Parameters">
                    <xsl:apply-templates select="Parameters/Parameter" mode="Root" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="*[not(local-name()='Parameters' or local-name()='Modifiers' or local-name()='LeadingTrivia' or local-name()='AttributeLists')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Method" mode="Members">
        <xsl:element name="Method">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="Modifiers" mode="Modifiers" />
            <xsl:apply-templates select="@*[not(local-name()='Name' or local-name()='Arity')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:if test="not(count(Parameters/Parameter)=0)">
                <xsl:element name="Parameters">
                    <xsl:apply-templates select="Parameters/Parameter" mode="Root" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="ReturnType" mode="MemberType" />
            <xsl:apply-templates select="*[not(local-name()='Parameters' or local-name()='ReturnType' or local-name()='Modifiers' or local-name()='LeadingTrivia' or local-name()='AttributeLists')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Property" mode="Members">
        <xsl:element name="Property">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:if test="not(count(AttributeLists/AttributeList/Attribute[@Name='Required'])=0)">
                <xsl:attribute name="Required">
                    <xsl:text>true</xsl:text>
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="@*[not(local-name()='Name')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:apply-templates select="Type|Explicit" mode="MemberType" />
            <xsl:apply-templates select="*[not(local-name()='LeadingTrivia' or local-name()='AttributeLists' or local-name()='Type' or local-name()='Explicit')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Member" mode="Enum">
        <xsl:param name="name" />
        <xsl:element name="Field">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:if test="not(count(Value/LiteralExpression)=0)">
                <xsl:attribute name="Value">
                    <xsl:value-of select="normalize-space(Value/LiteralExpression)" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="concat($name, '_', @Name)" />
            </xsl:apply-templates>
            <xsl:apply-templates select="*[not(local-name()='LeadingTrivia' or local-name()='Value' or local-name()='AttributeLists')]" mode="Unknown" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="Enum" mode="Root">
        <xsl:element name="Enum">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:if test="not(count(BaseTypes/SimpleBaseType/PredefinedType/@Keyword)=0)">
                <xsl:attribute name="BaseType">
                    <xsl:value-of select="normalize-space(BaseTypes/SimpleBaseType/PredefinedType/@Keyword)" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(AttributeLists/AttributeList/Attribute[@Name='MessageCode']/Arguments/Argument/Expression)=0)">
                <xsl:attribute name="MessageCode">
                    <xsl:value-of select="normalize-space(AttributeLists/AttributeList/Attribute[@Name='MessageCode']/Arguments/Argument/Expression)" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(AttributeLists/AttributeList/Attribute[@Name='StatusMessageLevel']/Arguments/Argument/Expression)=0)">
                <xsl:attribute name="StatusMessageLevel">
                    <xsl:value-of select="normalize-space(AttributeLists/AttributeList/Attribute[@Name='StatusMessageLevel']/Arguments/Argument/Expression)" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(AttributeLists/AttributeList/Attribute[@Name='ErrorCode']/Arguments/Argument/Expression)=0)">
                <xsl:attribute name="ErrorCode">
                    <xsl:value-of select="normalize-space(AttributeLists/AttributeList/Attribute[@Name='ErrorCode']/Arguments/Argument/Expression)" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(AttributeLists/AttributeList/Attribute[@Name='Flags'])=0)">
                <xsl:attribute name="IsFlags">
                    <xsl:text>true</xsl:text>
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="Modifiers" mode="Modifiers" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:apply-templates select="*[not(local-name()='BaseTypes' or local-name()='Modifiers' or local-name()='Member' or local-name()='AttributeLists' or local-name()='LeadingTrivia')]" mode="Unknown" />
            <xsl:apply-templates select="Member" mode="Enum">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Class" mode="Root">
        <xsl:element name="Class">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="Modifiers" mode="Modifiers" />
            <xsl:apply-templates select="@*[not(local-name()='Name' or local-name()='Arity')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:if test="not(count(BaseTypes/*)=0)">
                <xsl:element name="Implements">
                    <xsl:apply-templates select="BaseTypes/SimpleBaseType" mode="BaseTypes" />
                    <xsl:apply-templates select="BaseTypes/*[not(local-name()='SimpleBaseType')]" mode="Unknown">
                        <xsl:with-param name="mode">BaseTypes</xsl:with-param>
                    </xsl:apply-templates>
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="*[not(local-name()='Members' or local-name()='Modifiers' or local-name()='AttributeLists' or local-name()='LeadingTrivia' or local-name()='BaseTypes')]" mode="Unknown" />
            <xsl:apply-templates select="Members/Field" mode="Members" />
            <xsl:apply-templates select="Members/Property" mode="Members" />
            <xsl:apply-templates select="Members/Constructor" mode="Members" />
            <xsl:apply-templates select="Members/Method" mode="Members" />
            <xsl:if test="not(count(Members/*[not(local-name()='Field' or local-name()='Property' or local-name()='Constructor' or local-name()='Method')])=0)">
                <xsl:element name="Members">
                    <xsl:apply-templates select="Members/*[not(local-name()='Field' or local-name()='Property' or local-name()='Constructor' or local-name()='Method')]" mode="Unknown">
                        <xsl:with-param name="mode">
                            <xsl:text>Members</xsl:text>
                        </xsl:with-param>
                    </xsl:apply-templates>
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Interface" mode="Root">
        <xsl:element name="Interface">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="Modifiers" mode="Modifiers" />
            <xsl:apply-templates select="@*[not(local-name()='Name' or local-name()='Arity')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="AttributeLists/AttributeList" mode="Root">
                <xsl:with-param name="name" select="@Name" />
            </xsl:apply-templates>
            <xsl:if test="not(count(BaseTypes/*)=0)">
                <xsl:element name="Implements">
                    <xsl:apply-templates select="BaseTypes/SimpleBaseType" mode="BaseTypes" />
                    <xsl:apply-templates select="BaseTypes/*[not(local-name()='SimpleBaseType')]" mode="Unknown">
                        <xsl:with-param name="mode">BaseTypes</xsl:with-param>
                    </xsl:apply-templates>
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="*[not(local-name()='Members' or local-name()='Modifiers' or local-name()='AttributeLists' or local-name()='LeadingTrivia' or local-name()='BaseTypes')]" mode="Unknown" />
            <xsl:apply-templates select="Members/Property" mode="Members" />
            <xsl:apply-templates select="Members/Method" mode="Members" />
            <xsl:if test="not(count(Members/*[not(local-name()='Property' or local-name()='Method')])=0)">
                <xsl:element name="Members">
                    <xsl:apply-templates select="Members/*[not(local-name()='Property' or local-name()='Method')]" mode="Unknown">
                        <xsl:with-param name="mode">
                            <xsl:text>Members</xsl:text>
                        </xsl:with-param>
                    </xsl:apply-templates>
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Namespace" mode="Root">
        <xsl:element name="Namespace">
            <xsl:attribute name="Name">
                <xsl:value-of select="@Name" />
            </xsl:attribute>
            <xsl:apply-templates select="@*[not(local-name()='Name')]" mode="Unknown" />
            <xsl:apply-templates select="LeadingTrivia" mode="Root" />
            <xsl:apply-templates select="Members/Enum" mode="Root" />
            <xsl:apply-templates select="Members/Interface" mode="Root" />
            <xsl:apply-templates select="Members/Class" mode="Root" />
            <xsl:apply-templates select="*[not(local-name()='Members' or local-name()='LeadingTrivia')]" mode="Unknown" />
            <xsl:if test="not(count(Members/*[not(local-name()='Enum' or local-name()='Interface' or local-name()='Class')])=0)">
                <xsl:element name="Members">
                    <xsl:apply-templates select="Members/*[not(local-name()='Enum' or local-name()='Interface' or local-name()='Class')]" mode="Unknown" />
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="*" mode="Root">
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:template match="]]></xsl:text>
            <xsl:value-of select="local-name()" />
            <xsl:text><![CDATA[" mode="Root"></xsl:template>]]></xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="@*" mode="Root">
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:if test="not(count(@]]></xsl:text>
            <xsl:value-of select="local-name()" />
            <xsl:text><![CDATA[)=0)"></xsl:if>]]></xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="*" mode="Documentation">
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:template match="]]></xsl:text>
            <xsl:value-of select="local-name()" />
            <xsl:text><![CDATA[" mode="Root"></xsl:template>]]></xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="@*" mode="Documentation">
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:if test="not(count(@]]></xsl:text>
            <xsl:value-of select="local-name()" />
            <xsl:text><![CDATA[)=0)"></xsl:if>]]></xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="text()" mode="Documentation">
        <xsl:copy />
    </xsl:template>
    <xsl:template match="*" mode="Unknown">
        <xsl:param name="mode">
            <xsl:text>Root</xsl:text>
        </xsl:param>
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:template match="]]></xsl:text>
            <xsl:value-of select="local-name()" />
            <xsl:text><![CDATA[" mode="]]></xsl:text>
            <xsl:value-of select="$mode" />
            <xsl:text><![CDATA["></xsl:template>]]></xsl:text>
        </xsl:comment>
    </xsl:template>
    <xsl:template match="@*" mode="Unknown">
        <xsl:comment>
            <xsl:text><![CDATA[TODO: Need <xsl:if test="not(count(@]]></xsl:text>
            <xsl:value-of select="local-name()" />
            <xsl:text><![CDATA[)=0)"></xsl:if>]]></xsl:text>
        </xsl:comment>
    </xsl:template>
</xsl:stylesheet>
