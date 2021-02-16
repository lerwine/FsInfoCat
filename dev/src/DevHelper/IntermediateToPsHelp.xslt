<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:int="urn:Erwine:Leonard:T:PsHelpIntermediate.xsd" xmlns:command="http://schemas.microsoft.com/maml/dev/command/2004/10" xmlns:maml="http://schemas.microsoft.com/maml/2004/10" xmlns:dev="http://schemas.microsoft.com/maml/dev/2004/10" version="1.0" exclude-result-prefixes="int xsl msxsl">
    <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" standalone="yes" />
    <xsl:strip-space elements="int:*"/>
    <xsl:preserve-space elements="code"/>
    <xsl:param name="version">
        <xsl:text>0.1</xsl:text>
    </xsl:param>
    <xsl:param name="copyright">
        <xsl:text>Copyright © Leonard Thomas Erwine 2021</xsl:text>
    </xsl:param>
    <xsl:variable name="allParameterSets">
        <xsl:text>_ALLPARAMETERSETS</xsl:text>
    </xsl:variable>

    <xsl:template name="RequireDescription">
        <xsl:param name="source">
            <xsl:copy-of select="int:description/int:*" />
        </xsl:param>
        <xsl:element name="maml:description">
            <xsl:choose>
                <xsl:when test="count(msxsl:node-set($source)/int:*)=0">
                <xsl:element name="maml:para">
                    <xsl:comment>
                        <xsl:text>Description goes here.</xsl:text>
                    </xsl:comment>
                </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select="msxsl:node-set($source)/int:*" />
                </xsl:otherwise>
            </xsl:choose>
        </xsl:element>
    </xsl:template>
    <xsl:template name="ApplyError">
        <xsl:choose>
            <xsl:when test="count(int:type)=0">
                <xsl:element name="dev:type">
                    <xsl:element name="maml:name">
                        <xsl:comment>
                            <xsl:text>Type name goes here</xsl:text>
                        </xsl:comment>
                    </xsl:element>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="int:type" />
            </xsl:otherwise>
        </xsl:choose>
        <xsl:if test="not(count(int:description/int:*)=0)">
            <xsl:apply-templates select="int:description" />
        </xsl:if>
        <xsl:if test="not(count(@category)=0)">
            <xsl:element name="command:category">
                <xsl:value-of select="@category" />
            </xsl:element>
        </xsl:if>
        <xsl:if test="not(count(@errorID)=0)">
            <xsl:element name="command:errorID">
                <xsl:value-of select="@errorID" />
            </xsl:element>
        </xsl:if>
        <xsl:if test="not(count(int:recommendedAction/int:*)=0)">
            <xsl:element name="command:recommendedAction">
                <xsl:apply-templates select="int:*" />
            </xsl:element>
        </xsl:if>
        <xsl:if test="not(count(int:targetObjectType)=0)">
            <xsl:element name="command:targetObjectType">
                <xsl:call-template name="ApplyType" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template name="ApplyType">
        <xsl:element name="maml:name">
            <xsl:value-of select="@name" />
        </xsl:element>
        <xsl:if test="not(count(@uri)=0)">
            <xsl:element name="maml:uri">
                <xsl:value-of select="@uri" />
            </xsl:element>
        </xsl:if>
        <xsl:if test="not(count(int:*)=0)">
            <xsl:element name="maml:description">
                <xsl:apply-templates select="int:*" />
            </xsl:element>
        </xsl:if>
    </xsl:template>

    <xsl:template match="/">
        <xsl:apply-templates select="int:command" mode="Command" />
    </xsl:template>

    <xsl:template match="int:example" mode="Command">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='exampleType']
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='exampleTypeRestricted']
            urn:xsd:command:doc(developerCommand):/xs:schema/xs:complexType[@name='exampleType']#L239
            urn:xsd:command:doc(developerCommand):/xs:schema/xs:complexType[@name='exampleTypeRestricted']#L248
        -->
        <xsl:element name="command:example">
            <xsl:apply-templates select="int:*" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:parameterValue">
        <xsl:param name="required" />
        <xsl:param name="variableLength" />
        <!-- doc("developerCommand.xsd")/schema/element[@name="parameterValueType"] -->
        <xsl:element name="command:parameterValue">
            <xsl:attribute name="required">
                <xsl:value-of select="$required" />
            </xsl:attribute>
            <xsl:attribute name="variableLength">
                <xsl:value-of select="$variableLength" />
            </xsl:attribute>
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>

    <xsl:template match="int:command" mode="Command">
        <xsl:variable name="commandName">
            <xsl:choose>
                <xsl:when test="normalize-space(@verb)=''">
                    <xsl:value-of select="@noun" />
                </xsl:when>
                <xsl:when test="normalize-space(@noun)=''">
                    <xsl:value-of select="@verb" />
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="concat(@verb,'-',@noun)" />
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <command:command xmlns:command="http://schemas.microsoft.com/maml/dev/command/2004/10" xmlns:maml="http://schemas.microsoft.com/maml/2004/10" xmlns:dev="http://schemas.microsoft.com/maml/dev/2004/10">
            <xsl:element name="command:details">
                <xsl:element name="command:name">
                    <xsl:value-of select="$commandName" />
                </xsl:element>
                <xsl:element name="maml:description">
                    <xsl:element name="maml:para">
                        <xsl:choose>
                            <xsl:when test="normalize-space(int:synopsis)=''">
                                <xsl:comment>
                                    <xsl:text>Synopsis goes here</xsl:text>
                                </xsl:comment>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="int:synopsis" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:element>
                </xsl:element>
                <xsl:if test="not(count(int:aliases/int:alias)=0)">
                    <xsl:element name="command:synonyms">
                        <xsl:for-each select="int:aliases/int:alias">
                            <xsl:element name="maml:synonym">
                                <xsl:value-of select="." />
                            </xsl:element>
                        </xsl:for-each>
                    </xsl:element>
                </xsl:if>
                <xsl:element name="maml:copyright">
                    <xsl:element name="maml:para">
                        <xsl:value-of select="$copyright" />
                    </xsl:element>
                </xsl:element>
                <xsl:element name="command:verb">
                    <xsl:value-of select="@verb" />
                </xsl:element>
                <xsl:element name="command:noun">
                    <xsl:value-of select="@noun" />
                </xsl:element>
                <xsl:element name="dev:version">
                    <xsl:value-of select="$version" />
                </xsl:element>
            </xsl:element>
            <xsl:call-template name="RequireDescription">
                <xsl:with-param name="source">
                    <xsl:copy-of select="int:details/int:*" />
                </xsl:with-param>
            </xsl:call-template>
            <xsl:element name="command:syntax">
                <xsl:variable name="syntaxItems">
                    <xsl:element name="int:syntax">
                        <xsl:for-each select="int:parameters/int:parameter">
                            <xsl:choose>
                                <xsl:when test="count(int:syntaxItem)=0">
                                    <xsl:element name="int:parameter">
                                        <xsl:attribute name="parameterSetName">
                                            <xsl:value-of select="$allParameterSets"></xsl:value-of>
                                        </xsl:attribute>
                                        <xsl:attribute name="required">
                                            <xsl:text>false</xsl:text>
                                        </xsl:attribute>
                                        <xsl:attribute name="globbing">
                                            <xsl:text>false</xsl:text>
                                        </xsl:attribute>
                                        <xsl:attribute name="pipelineInput">
                                            <xsl:text>False</xsl:text>
                                        </xsl:attribute>
                                        <xsl:attribute name="position">
                                            <xsl:text>named</xsl:text>
                                        </xsl:attribute>
                                        <xsl:attribute name="name">
                                            <xsl:value-of select="@name" />
                                        </xsl:attribute>
                                        <xsl:attribute name="variableLength">
                                            <xsl:choose>
                                                <xsl:when test="count(@variableLength)=0">
                                                    <xsl:text>false</xsl:text>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                    <xsl:value-of select="@variableLength" />
                                                </xsl:otherwise>
                                            </xsl:choose>
                                        </xsl:attribute>
                                        <xsl:copy-of select="@requiresTrustedData" />
                                        <xsl:copy-of select="int:*" />
                                    </xsl:element>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:variable name="variableLength">
                                        <xsl:choose>
                                            <xsl:when test="count(../@variableLength)=0">
                                                <xsl:text>false</xsl:text>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:value-of select="../@variableLength" />
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </xsl:variable>
                                    <xsl:for-each select="int:syntaxItem">
                                        <xsl:element name="int:parameter">
                                            <xsl:attribute name="name">
                                                <xsl:value-of select="../@name" />
                                            </xsl:attribute>
                                            <xsl:attribute name="variableLength">
                                                <xsl:value-of select="$variableLength" />
                                            </xsl:attribute>
                                            <xsl:copy-of select="../@requiresTrustedData" />
                                            <xsl:attribute name="parameterSetName">
                                                <xsl:choose>
                                                    <xsl:when test="count(@parameterSetName)=0">
                                                        <xsl:value-of select="$allParameterSets"></xsl:value-of>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="translate(@parameterSetName,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:attribute>
                                            <xsl:copy-of select="@required" />
                                            <xsl:copy-of select="@globbing" />
                                            <xsl:copy-of select="@pipelineInput" />
                                            <xsl:copy-of select="@position" />
                                            <xsl:copy-of select="../int:description" />
                                            <xsl:copy-of select="../int:parameterValue" />
                                            <xsl:copy-of select="../int:type" />
                                            <xsl:copy-of select="../int:defaultValue" />
                                            <xsl:copy-of select="../int:possibleValues" />
                                            <xsl:copy-of select="../int:validation" />
                                        </xsl:element>
                                    </xsl:for-each>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:for-each>
                    </xsl:element>
                </xsl:variable>
                <xsl:for-each select="msxsl:node-set($syntaxItems)/int:syntax/int:parameter">
                    <xsl:variable name="parameterSetName">
                        <xsl:value-of select="normalize-space(@parameterSetName)" />
                    </xsl:variable>
                    <xsl:if test="count(preceding-sibling::int:parameter[@parameterSetName=$parameterSetName])=0">
                        <xsl:element name="command:syntaxItem">
                            <xsl:if test="not($parameterSetName=$allParameterSets)">
                                <xsl:attribute name="address">
                                    <xsl:value-of select="concat('ParameterSetName_x003D_', $parameterSetName)" />
                                </xsl:attribute>
                            </xsl:if>
                            <xsl:element name="maml:name">
                                <xsl:value-of select="$commandName" />
                            </xsl:element>
                            <xsl:apply-templates select="msxsl:node-set($syntaxItems)/int:syntax/int:parameter[@parameterSetName=$parameterSetName]">
                            <xsl:with-param name="isSyntax">
                                <xsl:text>true</xsl:text>
                            </xsl:with-param>
                        </xsl:apply-templates>
                        </xsl:element>
                    </xsl:if>
                </xsl:for-each>
                <!-- <xsl:copy-of select="$syntaxItems" /> -->
            </xsl:element>
            <xsl:if test="not(count(int:parameters/int:parameter)=0)">
                <xsl:element name="command:parameters">
                    <xsl:apply-templates select="int:parameters/int:parameter">
                        <xsl:with-param name="isSyntax">
                            <xsl:text>false</xsl:text>
                        </xsl:with-param>
                    </xsl:apply-templates>
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(int:inputTypes/int:inputType)=0)">
                <xsl:element name="command:inputTypes">
                    <xsl:apply-templates select="int:inputTypes/int:inputType" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(int:returnValues/int:returnValue)=0)">
                <xsl:element name="command:returnValues">
                    <xsl:apply-templates select="int:returnValues/int:returnValue" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(int:errors/int:error[not(@nonTerminating='true')])=0)">
                <xsl:element name="command:terminatingErrors">
                    <xsl:for-each select="int:errors/int:error[not(@nonTerminating='true')]">
                        <xsl:element name="terminatingError">
                            <xsl:call-template name="ApplyError" />
                        </xsl:element>
                    </xsl:for-each>
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(int:errors/int:error[@nonTerminating='true'])=0)">
                <xsl:element name="command:nonTerminatingErrors">
                    <xsl:for-each select="int:errors/int:error[@nonTerminating='true']">
                        <xsl:element name="nonTerminatingError">
                            <xsl:call-template name="ApplyError" />
                        </xsl:element>
                    </xsl:for-each>
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="int:alertSet" />
            <xsl:if test="not(count(int:examples/int:example)=0)">
                <xsl:element name="command:examples">
                    <xsl:apply-templates select="int:examples/int:example" mode="Command" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="int:relatedLinks" />
        </command:command>
    </xsl:template>
    <xsl:template match="int:inputType">
        <xsl:element name="command:inputType">
            <xsl:if test="not(count(@requiresTrustedData)=0)">
                <xsl:attribute name="command:requiresTrustedData">
                    <xsl:value-of select="@requiresTrustedData" />
                </xsl:attribute>
            </xsl:if>
            <xsl:choose>
                <xsl:when test="count(int:type)=0">
                    <xsl:element name="dev:type">
                        <xsl:element name="maml:name">
                            <xsl:comment>
                                <xsl:text>Type name goes here</xsl:text>
                            </xsl:comment>
                        </xsl:element>
                    </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select="int:type" />
                </xsl:otherwise>
            </xsl:choose>
            <xsl:if test="not(count(int:description/int:*)=0)">
                <xsl:apply-templates select="int:description" />
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:returnValue">
        <xsl:element name="command:returnValue">
            <xsl:if test="not(count(@isTrustedData)=0)">
                <xsl:attribute name="command:isTrustedData">
                    <xsl:value-of select="@isTrustedData" />
                </xsl:attribute>
            </xsl:if>
            <xsl:choose>
                <xsl:when test="count(int:type)=0">
                    <xsl:element name="dev:type">
                        <xsl:element name="maml:name">
                            <xsl:comment>
                                <xsl:text>Type name goes here</xsl:text>
                            </xsl:comment>
                        </xsl:element>
                    </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select="int:type" />
                </xsl:otherwise>
            </xsl:choose>
            <xsl:if test="not(count(int:description/int:*)=0)">
                <xsl:apply-templates select="int:description" />
            </xsl:if>
            <xsl:if test="not(count(int:possibleValues/int:possibleValue)=0)">
                <xsl:element name="dev:possibleValues">
                    <xsl:apply-templates select="int:possibleValues/int:possibleValue" />
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:alertSet">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='alertSetType']
            urn:xsd:maml:doc(structureList):/xs:schema/xs:element[@name='alertSet']#L72
        -->
        <xsl:element name="maml:alertSet">
            <xsl:if test="not(count(@class)=0)">
                <xsl:attribute name="class">
                    <xsl:value-of select="@class"></xsl:value-of>
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@expandCollapse)=0)">
                <xsl:attribute name="expandCollapse">
                    <xsl:value-of select="@expandCollapse"></xsl:value-of>
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(int:title)=0)">
                <xsl:element name="maml:title">
                    <xsl:apply-templates select="int:*|text()" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="int:alert" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:alert">
        <xsl:element name="maml:alert">
            <xsl:apply-templates select="int:*" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:relatedLinks">
        <xsl:if test="not(count(int:navigationLink)=0)">
            <xsl:element name="maml:relatedLinks">
                <xsl:if test="not(count(@type)=0)">
                    <xsl:attribute name="type">
                        <xsl:value-of select="@type"></xsl:value-of>
                    </xsl:attribute>
                </xsl:if>
                <xsl:if test="not(count(int:title)=0)">
                    <xsl:element name="maml:title">
                        <xsl:apply-templates select="int:*|text()" />
                    </xsl:element>
                </xsl:if>
                <xsl:apply-templates select="int:navigationLink" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="int:type">
        <!-- doc("developer.xsd")/schema/element[@name="type"] -->
        <xsl:element name="dev:type">
            <xsl:call-template name="ApplyType" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:parameter">
        <!-- doc(developerCommand.xsd/xs:schema/xs:complexType[@name='parameterType'] -->
        <xsl:param name="isSyntax" />
        <xsl:element name="command:parameter">
            <xsl:attribute name="required">
                <xsl:choose>
                    <xsl:when test="$isSyntax='true'">
                        <xsl:choose>
                            <xsl:when test="count(@required)=0">
                                <xsl:text>false</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="@required" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:choose>
                            <xsl:when test="not(count(int:syntaxItem)=0) and count(int:syntaxItem[count(@required)=0 or @required='false'])=0">
                                <xsl:text>true</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:text>false</xsl:text>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="variableLength">
                <xsl:choose>
                    <xsl:when test="count(@variableLength)=0">
                        <xsl:text>false</xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="@variableLength" />
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="globbing">
                <xsl:choose>
                    <xsl:when test="$isSyntax='true'">
                        <xsl:choose>
                            <xsl:when test="count(@globbing)=0">
                                <xsl:text>false</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="@globbing" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:choose>
                            <xsl:when test="count(int:syntaxItem[@globbing='true'])=0">
                                <xsl:text>false</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:text>true</xsl:text>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="pipelineInput">
                <xsl:choose>
                    <xsl:when test="$isSyntax='true'">
                        <xsl:choose>
                            <xsl:when test="count(@pipelineInput)=0">
                                <xsl:text>False</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="@pipelineInput" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:choose>
                            <xsl:when test="count(int:syntaxItem[not(count(@pipelineInput)=0 or @pipelineInput='False')])=0">
                                <xsl:text>false</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:text>true</xsl:text>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="position">
                <xsl:choose>
                    <xsl:when test="$isSyntax='true'">
                        <xsl:choose>
                            <xsl:when test="count(@position)=0">
                                <xsl:text>named</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="@position" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:choose>
                            <xsl:when test="count(int/syntaxItem[not(count(@position)=0 or @position='named')])=0">
                                <xsl:text>named</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="int/syntaxItem[not(count(@position)=0 or @position='named')][1]/@position" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:if test="not(count(@requiresTrustedData)=0)">
                <xsl:attribute name="command:requiresTrustedData">
                    <xsl:value-of select="@requiresTrustedData" />
                </xsl:attribute>
            </xsl:if>
            <xsl:element name="maml:name">
                <xsl:value-of select="@name" />
            </xsl:element>
            <xsl:call-template name="RequireDescription" />
            <xsl:apply-templates select="int:parameterValue">
                <xsl:with-param name="required">
                    <xsl:value-of select="@required" />
                </xsl:with-param>
                <xsl:with-param name="variableLength">
                    <xsl:value-of select="@variableLength" />
                </xsl:with-param>
            </xsl:apply-templates>
            <xsl:choose>
                <xsl:when test="count(int:type)=0">
                    <xsl:element name="dev:type">
                        <xsl:element name="maml:name">
                            <xsl:comment>
                                <xsl:text>Type name goes here</xsl:text>
                            </xsl:comment>
                        </xsl:element>
                    </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select="int:type" />
                </xsl:otherwise>
            </xsl:choose>
            <xsl:apply-templates select="int:defaultValue" />
            <xsl:if test="not(count(int:possibleValues/int:possibleValue)=0)">
                <xsl:element name="dev:possibleValues">
                    <xsl:apply-templates select="int:possibleValues/int:possibleValue" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="int:validation" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:defaultValue">
        <!-- doc("developer.xsd")/schema/element[@name="defaultValue"] -->
        <xsl:element name="dev:defaultValue">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:possibleValue">
        <!-- doc("developer.xsd")/schema/element[@name="possibleValueType"] -->
        <xsl:element name="dev:possibleValue">
            <xsl:if test="not(count(@default)=0)">
                <xsl:attribute name="default">
                    <xsl:value-of select="@default" />
                </xsl:attribute>
            </xsl:if>
            <xsl:element name="dev:value">
                <xsl:value-of select="@value" />
            </xsl:element>
            <xsl:if test="not(count(int:*)=0)">
                <xsl:element name="maml:description">
                    <xsl:apply-templates select="int:*" />
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:validation">
        <!-- doc("developerCommand.xsd")/schema/element[@name="validationType"] -->
        <xsl:element name="command:validation">
            <xsl:if test="not(count(@minCount)=0)">
                <xsl:element name="command:minCount">
                    <xsl:value-of select="@minCount" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(@maxCount)=0)">
                <xsl:element name="command:maxCount">
                    <xsl:value-of select="@maxCount" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(@minLength)=0)">
                <xsl:element name="command:minLength">
                    <xsl:value-of select="@minLength" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(@maxLength)=0)">
                <xsl:element name="command:maxLength">
                    <xsl:value-of select="@maxLength" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(@minRange)=0)">
                <xsl:element name="command:minRange">
                    <xsl:value-of select="@minRange" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(@maxRange)=0)">
                <xsl:element name="command:maxRange">
                    <xsl:value-of select="@maxRange" />
                </xsl:element>
            </xsl:if>
            <xsl:if test="not(count(@pattern)=0)">
                <xsl:element name="command:pattern">
                    <xsl:value-of select="@pattern" />
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:leadInPhrase">
        <xsl:element name="maml:leadInPhrase">
            <xsl:if test="not(count(@class)=0)">
                <xsl:attribute name="class">
                    <xsl:value-of select="@class" />
                </xsl:attribute>
                <xsl:value-of select="." />
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:code">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='codeType']
            urn:xsd:dev:doc(developer):/xs:schema/xs:complexType[@name='codeType']#L103
        -->
        <xsl:element name="dev:code">
            <xsl:if test="not(count(@language)=0)">
                <xsl:attribute name="language">
                    <xsl:value-of select="@language" />
                </xsl:attribute>
            </xsl:if>
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:commandLines">
        <xsl:if test="not(count(int:commandLine/int:commandText)=0)">
            <xsl:element name="command:commandLines">
                <xsl:apply-templates select="int:commandLine" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="int:commandLine">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='commandLineType']
            urn:xsd:command:doc(developerCommand):/xs:schema/xs:complexType[@name='commandLineType']#L275
        -->
        <xsl:if test="not(count(int:commandText)=0)">
            <xsl:element name="command:commandLine">
                <xsl:apply-templates select="int:commandText" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="int:commandText">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='commandTextType']
            urn:xsd:command:doc(developerCommand):/xs:schema/xs:complexType[@name='commandTextType']#L283
        -->
        <xsl:element name="command:commandText">
            <xsl:if test="not(count(@input)=0)">
                <xsl:attribute name="input">
                    <xsl:value-of select="@input" />
                </xsl:attribute>
            </xsl:if>
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:list">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='listType']
            urn:xsd:maml:doc(structureList):/xs:schema/xs:element[@name='list']#L17
        -->
        <xsl:if test="not(count(int:listItem/int:*)=0)">
            <xsl:element name="maml:list">
                <xsl:if test="not(count(@class)=0)">
                    <xsl:attribute name="class">
                        <xsl:value-of select="@class" />
                    </xsl:attribute>
                </xsl:if>
                <xsl:apply-templates select="int:listItem" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="int:listItem">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='listType']/xs:sequence/xs:element[@name='listItem']
            urn:xsd:maml:doc(structureList):/xs:schema/xs:element[@name='listItem']#L41
        -->
        <xsl:if test="not(count(int:*)=0)">
            <xsl:element name="maml:listItem">
                <xsl:if test="not(count(@selectionDefault)=0)">
                    <xsl:attribute name="selectionDefault">
                        <xsl:value-of select="@selectionDefault" />
                    </xsl:attribute>
                </xsl:if>
                <xsl:apply-templates select="int:*" />
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="int:tableHeader|tableFooter">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='tableBodyType']
            urn:xsd:maml:doc(structureTable):/xs:schema/xs:complexType[@name='tableBodyType']
        -->
        <xsl:element name="maml:{local-name()}">
            <xsl:apply-templates select="int:row" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:headerEntry|int:entry|int:footerEntry">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='entryType']
            urn:xsd:maml:doc(structureTable):/xs:schema/xs:complexType[@name='entryType']#L34
        -->
        <xsl:element name="maml:{local-name()}">
            <xsl:if test="not(count(@rowSpan)=0)">
                <xsl:attribute name="rowSpan">
                    <xsl:value-of select="@rowSpan" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@columnSpan)=0)">
                <xsl:attribute name="columnSpan">
                    <xsl:value-of select="@columnSpan" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="int:*" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:code">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='codeType']
            urn:xsd:dev:doc(developer):/xs:schema/xs:complexType[@name='codeType']#L103
        -->
        <xsl:element name="maml:code">
            <xsl:if test="not(count(@language)=0)">
                <xsl:attribute name="language">
                    <xsl:value-of select="@language" />
                </xsl:attribute>
            </xsl:if>
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:definitionList">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='definitionListType']
            urn:xsd:maml:doc(structureTable):/xs:schema/xs:complexType[@name='definitionListType']#L114
        -->
        <xsl:element name="maml:definitionList">
            <xsl:apply-templates select="int:definitionListItem" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:definitionListItem">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='definitionListItemType']
            urn:xsd:maml:doc(structureTable):/xs:schema/xs:complexType[@name='definitionListItemType']#L107
        -->
        <xsl:element name="maml:definitionListItem">
            <xsl:choose>
                <xsl:when test="count(maml:term)=0">
                    <xsl:element name="maml:term">
                        <xsl:comment>
                            <xsl:text>Term goes here</xsl:text>
                        </xsl:comment>
                    </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select="int:term[1]" />
                </xsl:otherwise>
            </xsl:choose>
            <xsl:choose>
                <xsl:when test="count(maml:definition)=0">
                    <xsl:element name="maml:definition">
                        <xsl:comment>
                            <xsl:text>Definition goes here</xsl:text>
                        </xsl:comment>
                    </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select="int:definition[1]" />
                </xsl:otherwise>
            </xsl:choose>
            <xsl:apply-templates select="int:uri[1]" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:term">
        <!--
            urn:xsd:int:doc(PsHelpIntermediate):/xs:schema/xs:complexType[@name='definitionListType']
            urn:xsd:maml:doc(structureTable):/xs:schema/xs:complexType[@name='definitionListType']#L114
        -->
        <xsl:element name="maml:term">
            <xsl:if test="not(count(@termClass)=0)">
                <xsl:attribute name="termClass">
                    <xsl:value-of select="@termClass" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@partOfSpeech)=0)">
                <xsl:attribute name="partOfSpeech">
                    <xsl:value-of select="@partOfSpeech" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@geographicalUsage)=0)">
                <xsl:attribute name="geographicalUsage">
                    <xsl:value-of select="@geographicalUsage" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@language)=0)">
                <xsl:attribute name="language">
                    <xsl:value-of select="@language" />
                </xsl:attribute>
            </xsl:if>
            <xsl:if test="not(count(@address)=0)">
                <xsl:attribute name="address">
                    <xsl:value-of select="@address" />
                </xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="int:*|text()" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:navigationLink">
        <xsl:element name="maml:navigationLink">
            <xsl:if test="not(count(@targetVerification)=0)">
                <xsl:attribute name="targetVerification">
                    <xsl:value-of select="@targetVerification" />
                </xsl:attribute>
            </xsl:if>
            <xsl:element name="linkText">
                <xsl:value-of select="."></xsl:value-of>
            </xsl:element>
            <xsl:if test="not(count(@uri)=0)">
                <xsl:element name="uri">
                    <xsl:value-of select="@uri" />
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:para|int:conditionalInline|int:title|int:quote">
        <xsl:element name="maml:{local-name()}">
            <xsl:apply-templates select="int:*|text()" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:description|int:introduction|int:buildInstructions|int:robustProgramming|int:security|int:remarks|int:table|int:row|int:definition">
        <xsl:element name="maml:{local-name()}">
            <xsl:apply-templates select="int:*" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="int:*">
        <xsl:element name="maml:{local-name()}">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>
