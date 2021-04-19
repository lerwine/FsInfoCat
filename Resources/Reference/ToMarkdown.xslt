<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:r="http://git.erwinefamily.net/FsInfoCat/V1/References.xsd"
                xmlns:b="http://schemas.openxmlformats.org/officeDocument/2006/bibliography" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:cs="uri:erwinefamily.net:regex"
                extension-element-prefixes="msxsl cs">
    <xsl:output encoding="utf-8" method="text" />
    <xsl:variable name="UcLetters" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
    <xsl:variable name="LcLetters" select="'abcdefghijklmnopqrstuvwxyz'" />
    <xsl:variable name="AllLetters" select="concat($UcLetters, $LcLetters)" />
    <xsl:variable name="Digits" select="'01234567890'" />
    <xsl:variable name="AlphaNum" select="concat($AllLetters, $Digits)" />
    <xsl:variable name="NewLine"><xsl:text><![CDATA[
]]></xsl:text></xsl:variable>
    <msxsl:script language="c#" implements-prefix="cs">
        <![CDATA[
        public string ToIdentifier(string value)
        {
            return Regex.Replace(value.Trim().ToLower(), @"[^a-z\d]+", "-");
        }
    ]]></msxsl:script>
    <xsl:variable name="Sources" select="document('Sources.xml')/b:Sources" />
    <xsl:template match="/">
        <xsl:variable name="Categorized">
            <xsl:apply-templates select="/r:References/r:Categories/r:Category" mode="Categorize"/>
        </xsl:variable>
        <xsl:variable name="Uncategorized">
            <xsl:apply-templates select="msxsl:node-set($Sources)/b:Source" mode="Categorize">
                <xsl:with-param name="AllTags" select="//r:Link/@Tag"/>
            </xsl:apply-templates>
        </xsl:variable>
        <xsl:apply-templates select="msxsl:node-set($Categorized)/r:Category" mode="Index" />
        <xsl:if test="not(count(msxsl:node-set($Uncategorized)/b:*)=0)">
            <xsl:value-of select="concat($NewLine, '## Uncategorized', $NewLine, $NewLine)"/>
            <xsl:apply-templates select="msxsl:node-set($Uncategorized)" mode="Links" />
        </xsl:if>
        <xsl:apply-templates select="msxsl:node-set($Categorized)/r:Category" mode="Links" />
    </xsl:template>
    <xsl:template match="b:Source" mode="Categorize">
        <xsl:param name="AllTags" />
        <xsl:variable name="tag" select="normalize-space(b:Tag)" />
        <xsl:if test="count(msxsl:node-set($AllTags)[.=$tag])=0">
            <xsl:copy-of select="."/>
        </xsl:if>
    </xsl:template>
    <xsl:template match="r:Categories" mode="Categorize">
        <xsl:element name="r:Categories">
            <xsl:apply-templates select="r:Category" mode="Categorize"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:Category" mode="Categorize">
        <xsl:element name="r:Category">
            <xsl:copy-of select="@*"/>
            <xsl:apply-templates select="r:*" mode="Categorize"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:Link" mode="Categorize">
        <xsl:variable name="tag" select="normalize-space(@Tag)" />
        <xsl:copy-of select="msxsl:node-set($Sources)/b:Source[b:Tag=$tag]"/>
    </xsl:template>
    <xsl:template match="r:Category" mode="Index">
        <xsl:param name="indent" select="''" />
        <xsl:value-of select="concat($indent, '- [', @Name, '](#', cs:ToIdentifier(@Name), ')', $NewLine)"/>
        <xsl:apply-templates select="r:Categories/r:Category" mode="Index">
            <xsl:with-param name="indent" select="concat('  ', $indent)"/>
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="r:Category" mode="Links">
        <xsl:param name="heading" select="'##'" />
        <xsl:value-of select="concat($NewLine, $heading, ' ', @Name, $NewLine, $NewLine)"/>
        <xsl:apply-templates select="b:Source[b:SourceType='InternetSite']" mode="Links" />
        <xsl:apply-templates select="r:Categories/r:Category" mode="Links">
            <xsl:with-param name="heading" select="concat('#', $heading)"/>
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="b:Source" mode="Links">
        <xsl:variable name="title">
            <xsl:variable name="title">
                <xsl:variable name="titleNodes">
                    <xsl:copy-of select="b:ShortTitle"/>
                    <xsl:copy-of select="b:Title"/>
                    <xsl:copy-of select="b:BookTitle"/>
                    <xsl:copy-of select="b:PublicationTitle"/>
                    <xsl:copy-of select="b:PeriodicalTitle"/>
                    <xsl:copy-of select="b:CaseNumber"/>
                </xsl:variable>
                <xsl:variable name="title" select="normalize-space(msxsl:node-set($titleNodes)[not(string-length(normalize-space(.))=0)][1])" />
                <xsl:variable name="internetSiteTitle">
                    <xsl:variable name="internetSiteTitle" select="normalize-space(b:InternetSiteTitle)" />
                    <xsl:choose>
                        <xsl:when test="not(string-length($internetSiteTitle)=0)">
                            <xsl:value-of select="$internetSiteTitle"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:variable name="person" select="normalize-space(b:Author/b:Author/b:NameList/b:Person/b:Last)"/>
                            <xsl:choose>
                                <xsl:when test="string-length($person)=0">
                                    <xsl:value-of select="normalize-space(b:Author/b:Corporate)"/>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:variable name="firstMi" select="normalize-space(concat(b:Author/b:Author/b:NameList/b:Person/b:First, ' ', b:Author/b:Author/b:NameList/b:Person/b:Middle))" />
                                    <xsl:choose>
                                        <xsl:when test="string-length($firstMi)=0">
                                            <xsl:value-of select="$person"/>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:value-of select="concat($person, ', ', $firstMi)"/>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:variable>
                <xsl:choose>
                    <xsl:when test="string-length($internetSiteTitle)=0">
                        <xsl:value-of select="$title"/>
                    </xsl:when>
                    <xsl:when test="string-length($title)=0 or $internetSiteTitle=$title">
                        <xsl:value-of select="$internetSiteTitle"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="concat($title, ' | ', $internetSiteTitle)"/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:choose>
                <xsl:when test="not(string-length($title)=0)">
                    <xsl:value-of select="$title"/>
                </xsl:when>
                <xsl:when test="not(string-length(normalize-space(b:Tag))=0)">
                    <xsl:value-of select="normalize-space(b:Tag)"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="normalize-space(b:Guid)"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="description" select="normalize-space(b:Comments)"/>
        <xsl:choose>
            <xsl:when test="string-length($description)=0">
                <xsl:value-of select="concat('- [', $title, '](', b:URL, ')', $NewLine)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="concat('- [', $title, '](', b:URL, ')', $NewLine, ' ', $description, $NewLine)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="ToLowerCase">
        <xsl:param name="text" select="''" />
        <xsl:param name="normalize" select="false()" />
        <xsl:choose>
            <xsl:when test="$normalize=true()">
                <xsl:value-of select="translate(normalize-space($text), $UcLetters, $LcLetters)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="translate($text, $UcLetters, $LcLetters)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
