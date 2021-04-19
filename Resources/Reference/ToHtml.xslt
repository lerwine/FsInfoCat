<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:r="http://git.erwinefamily.net/FsInfoCat/V1/References.xsd"
                xmlns:b="http://schemas.openxmlformats.org/officeDocument/2006/bibliography" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:cs="uri:erwinefamily.net:regex"
                extension-element-prefixes="msxsl cs b r">
    <xsl:output encoding="utf-8" method="html" indent="yes" version="1.0" />
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
        <html lang="en">
            <head>
                <meta charset="UTF-8"/>
                <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
                <meta name="viewport" content="width=1024, initial-scale=1.0"/>
                <title>References</title>
                <style type="text/css">
                    body {
                        font-family:Arial,Helvetica,sans-serif;
                        font-size:12px;
                    }
                    table {
                        border-collapse: collapse;
                    }
                    table caption {
                        text-align: left;
                        font-weight: bold;
                        font-size:14px;
                    }
                    table, th, td {
                        border: 1px solid black;
                    }
                    thead th {
                        text-align: left;
                        white-space: nowrap;
                    }
                    tbody th {
                        text-align: right;
                        white-space: nowrap;
                    }
                    thead th, tbody td {
                        vertical-align: bottom;
                    }
                    tbody th, tbody td {
                        vertical-align: top;
                    }
                    dl {
                        margin-top:0px;
                    }
                    dl dt {
                        font-weight: bold;
                    }
                </style>
            </head>
            <body>
                <ul>
                    <xsl:apply-templates select="msxsl:node-set($Categorized)/r:Category" mode="Index" />
                </ul>
                <xsl:if test="not(count(msxsl:node-set($Uncategorized))=0)">
                    <h1>Uncategorized</h1>
                    <xsl:apply-templates select="msxsl:node-set($Uncategorized)" mode="Links" />
                </xsl:if>
                <xsl:apply-templates select="msxsl:node-set($Categorized)/r:Category" mode="Links" />
            </body>
        </html>
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
        <xsl:element name="li">
            <xsl:element name="a">
                <xsl:attribute name="href">
                    <xsl:value-of select="concat('#cat:', cs:ToIdentifier(@Name))"/>
                </xsl:attribute>
                <xsl:value-of select="@Name"/>
            </xsl:element>
            <xsl:if test="not(count(r:Categories/r:Category)=0)">
                <xsl:element name="ul">
                    <xsl:apply-templates select="r:Categories/r:Category" mode="Index"/>
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:Category" mode="Links">
        <xsl:param name="heading" select="1" />
        <xsl:element name="{concat('h', $heading)}">
            <xsl:attribute name="id">
                <xsl:value-of select="concat('cat:', cs:ToIdentifier(@Name))"/>
            </xsl:attribute>
            <xsl:value-of select="@Name"/>
        </xsl:element>
        <xsl:if test="not(count(b:Source[b:SourceType='InternetSite'])=0)">
            <ul>
                <xsl:for-each select="b:Source[b:SourceType='InternetSite']">
                    <xsl:variable name="description" select="normalize-space(b:Comments)"/>
                    <xsl:element name="li">
                        <xsl:element name="a">
                            <xsl:attribute name="href">
                                <xsl:value-of select="concat('#ref:', normalize-space(b:Tag))"/>
                            </xsl:attribute>
                            <xsl:call-template name="GetTitle"/>
                        </xsl:element>
                        <xsl:if test="not(string-length(normalize-space(b:Comments))=0)">
                            <xsl:text> - </xsl:text>
                            <xsl:value-of select="b:Comments"/>
                        </xsl:if>
                    </xsl:element>
                </xsl:for-each>
            </ul>
            <xsl:apply-templates select="b:Source[b:SourceType='InternetSite']" mode="Links"/>
        </xsl:if>
        <xsl:apply-templates select="r:Categories/r:Category" mode="Links">
            <xsl:with-param name="heading" select="$heading + 1"/>
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="b:Source" mode="Links">
        <xsl:element name="table">
            <xsl:attribute name="id">
                <xsl:value-of select="concat('ref:', normalize-space(b:Tag))"/>
            </xsl:attribute>
            <xsl:element name="caption">
                <xsl:call-template name="GetTitle"/>
            </xsl:element>
            <xsl:element name="thead">
                <xsl:element name="tr">
                    <xsl:element name="th">
                        <xsl:attribute name="scope">
                            <xsl:text>row</xsl:text>
                        </xsl:attribute>
                        <xsl:text>Name</xsl:text>
                    </xsl:element>
                    <xsl:element name="th">
                        <xsl:attribute name="scope">
                            <xsl:text>row</xsl:text>
                        </xsl:attribute>
                        <xsl:text>Value</xsl:text>
                    </xsl:element>
                </xsl:element>
            </xsl:element>
            <xsl:element name="tbody">
                <xsl:for-each select="b:*">
                    <xsl:element name="tr">
                        <xsl:element name="th">
                            <xsl:attribute name="scope">
                                <xsl:text>row</xsl:text>
                            </xsl:attribute>
                            <xsl:choose>
                                <xsl:when test="local-name()='AbbreviatedCaseNumber'">
                                    <xsl:text>Abbreviated Case Number:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='AlbumTitle'">
                                    <xsl:text>Album Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='BookTitle'">
                                    <xsl:text>Book Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='BroadcastTitle'">
                                    <xsl:text>Broadcast Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='CaseNumber'">
                                    <xsl:text>Cas eNumber:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='ChapterNumber'">
                                    <xsl:text>Chapter Number:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='ConferenceName'">
                                    <xsl:text>Conference Name:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='CountryRegion'">
                                    <xsl:text>Country/Region:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='DayAccessed'">
                                    <xsl:text>Day Accessed:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='InternetSiteTitle'">
                                    <xsl:text>Internet Site Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='JournalName'">
                                    <xsl:text>Journal Name:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='MonthAccessed'">
                                    <xsl:text>Month Accessed:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='NumberVolumes'">
                                    <xsl:text>Number of Volumes:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='PatentNumber'">
                                    <xsl:text>Patent Number:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='PeriodicalTitle'">
                                    <xsl:text>Periodical Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='ProductionCompany'">
                                    <xsl:text>Production Company:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='PublicationTitle'">
                                    <xsl:text>Publication Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='RecordingNumber'">
                                    <xsl:text>Recording Number:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='RefOrder'">
                                    <xsl:text>Ref Order:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='ShortTitle'">
                                    <xsl:text>Short Title:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='StandardNumber'">
                                    <xsl:text>Standard Number:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='StateProvince'">
                                    <xsl:text>State/Province:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='ThesisType'">
                                    <xsl:text>Thesis Type:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='YearAccessed'">
                                    <xsl:text>Year Accessed:</xsl:text>
                                </xsl:when>
                                <xsl:when test="local-name()='SourceType'">
                                    <xsl:text>Source Type:</xsl:text>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="concat(local-name(), ':')"/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:element>
                        <xsl:element name="td">
                            <xsl:choose>
                                <xsl:when test="local-name()='URL'">
                                    <xsl:element name="a">
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="normalize-space(.)"/>
                                        </xsl:attribute>
                                        <xsl:attribute name="target">
                                            <xsl:text>_blank</xsl:text>
                                        </xsl:attribute>
                                        <xsl:value-of select="."/>
                                    </xsl:element>
                                </xsl:when>
                                <xsl:when test="local-name()='Author'">
                                    <xsl:if test="not(string-length(normalize-space(.))=0)">
                                        <dl>
                                            <xsl:apply-templates select="b:*" mode="Author" />
                                        </dl>
                                    </xsl:if>
                                </xsl:when>
                                <xsl:when test="local-name()='SourceType'">
                                    <xsl:choose>
                                        <xsl:when test=".='ArticleInAPeriodical'">
                                            <xsl:text>Article in a Periodical</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='BookSection'">
                                            <xsl:text>Book Section</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='JournalArticle'">
                                            <xsl:text>Journal Article</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='ConferenceProceedings'">
                                            <xsl:text>Conference Proceedings</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='SoundRecording'">
                                            <xsl:text>Sound Recording</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='DocumentFromInternetSite'">
                                            <xsl:text>Document from Internet Site</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='InternetSite'">
                                            <xsl:text>Internet Site</xsl:text>
                                        </xsl:when>
                                        <xsl:when test=".='ElectronicSource'">
                                            <xsl:text>Electronic Source</xsl:text>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:value-of select="."/>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="."/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:element>
                    </xsl:element>
                </xsl:for-each>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="b:Author" mode="Author">
        <xsl:apply-templates select="b:NameList/b:Person" mode="Author" />
        <xsl:apply-templates select="b:Corporate" mode="Author" />
    </xsl:template>
    <xsl:template match="b:Performer" mode="Author">
        <xsl:apply-templates select="b:NameList/b:Person" mode="Person">
            <xsl:with-param name="typeName" select="'Peformer'" />
        </xsl:apply-templates>
        <xsl:apply-templates select="b:Corporate" mode="Author">
            <xsl:with-param name="typeName" select="'Peformer'" />
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="b:BookAuthor" mode="Author">
        <xsl:apply-templates select="b:NameList/b:Person" mode="Person">
            <xsl:with-param name="typeName" select="'Book Author'" />
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="b:ProducerName" mode="Author">
        <xsl:apply-templates select="b:NameList/b:Person" mode="Person">
            <xsl:with-param name="typeName" select="'Producer Name'" />
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="b:Corporate" mode="Author">
        <xsl:param name="typeName" select="'Author'" />
        <xsl:element name="dt">
            <xsl:value-of select="concat('Corporate ', $typeName)"/>
        </xsl:element>
        <xsl:element name="dd">
            <xsl:value-of select="."/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="b:Person" mode="Author">
        <xsl:param name="typeName" select="'Author'" />
        <xsl:element name="dt">
            <xsl:value-of select="$typeName"/>
        </xsl:element>
        <xsl:choose>
            <xsl:when test="string-length(normalize-space(b:Last))=0">
                <xsl:choose>
                    <xsl:when test="string-length(normalize-space(b:Middle))=0">
                        <xsl:value-of select="b:First"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="concat(b:First, ' ', b:Middle)"/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="string-length(normalize-space(b:Middle))=0">
                <xsl:choose>
                    <xsl:when test="string-length(normalize-space(b:First))=0">
                        <xsl:value-of select="b:Last"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="concat(normalize-space(b:Last), ', ', b:First)"/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="string-length(normalize-space(b:First))=0">
                <xsl:value-of select="concat(normalize-space(b:Last), ', ', b:Middle)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="concat(normalize-space(b:Last), ', ', b:First, ' ', b:Middle)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="b:*" mode="Author">
        <xsl:apply-templates select="b:NameList/b:Person" mode="Person">
            <xsl:with-param name="typeName" select="local-name()" />
        </xsl:apply-templates>
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
    <xsl:template name="GetTitle">
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
    </xsl:template>
</xsl:stylesheet>
