<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:r="http://git.erwinefamily.net/FsInfoCat/V1/References.xsd"
                xmlns:b="http://schemas.openxmlformats.org/officeDocument/2006/bibliography" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                extension-element-prefixes="msxsl">
    <xsl:output encoding="utf-8" omit-xml-declaration="yes" method="xml" indent="yes" standalone="yes" />
    <xsl:variable name="Sources" select="document('Sources.xml')/b:Sources" />
    <xsl:template match="/">
        <xsl:element name="r:Categories">
            <xsl:apply-templates select="/r:References/r:Categories/r:Category" mode="Categorize"/>
            <xsl:variable name="Uncategorized">
                <xsl:apply-templates select="msxsl:node-set($Sources)/b:Source" mode="Categorize">
                    <xsl:with-param name="AllTags" select="//r:Link/@Tag"/>
                </xsl:apply-templates>
            </xsl:variable>
            <xsl:if test="not(count(msxsl:node-set($Uncategorized))=0)">
                <xsl:element name="Uncategorized">
                    <xsl:copy-of select="msxsl:node-set($Uncategorized)"/>
                </xsl:element>
            </xsl:if>
        </xsl:element>
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
</xsl:stylesheet>
