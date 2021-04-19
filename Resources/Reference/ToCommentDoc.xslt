<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:r="http://git.erwinefamily.net/FsInfoCat/V1/References.xsd"
                xmlns:b="http://schemas.openxmlformats.org/officeDocument/2006/bibliography" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns="" extension-element-prefixes="msxsl">

    <xsl:output encoding="utf-8" omit-xml-declaration="yes" method="xml" />

    <!-- Semicolon-separated list of values from /b:Sources/b:Source/b:Tag elements of the sources to be included in the reference listing. Empty string emits all references -->
    <xsl:param name="tagNames" select="''"/>
    <xsl:variable name="UcLetters" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
    <xsl:variable name="LcLetters" select="'abcdefghijklmnopqrstuvwxyz'" />
    <xsl:variable name="AllLetters" select="concat($UcLetters, $LcLetters)" />
    <xsl:variable name="Digits" select="'01234567890'" />
    <xsl:variable name="AlphaNum" select="concat($AllLetters, $Digits)" />
    <xsl:variable name="Sources" select="document('Sources.xml')/b:Sources" />
    <xsl:template match="/">
        <xsl:element name="DocComments">
            <xsl:apply-templates select="/r:References/r:Types/r:Type" />
            <xsl:apply-templates select="/r:References/r:Types/r:Namespace" />
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:Namespace">
        <xsl:param name="parentNs" select="''" />
        <xsl:variable name="ns">
            <xsl:choose>
                <xsl:when test="string-length(normalize-space($parentNs))=0">
                    <xsl:value-of select="@Name"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="concat(normalize-space($parentNs), '.', @Name)"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:apply-templates select="r:Type">
            <xsl:with-param name="parentNs" select="$ns" />
        </xsl:apply-templates>
        <xsl:apply-templates select="r:Namespace">
            <xsl:with-param name="parentNs" select="$ns" />
        </xsl:apply-templates>
    </xsl:template>
    <xsl:template match="r:Type">
        <xsl:param name="parentNs" select="''" />
        <xsl:param name="parentClass" select="''" />
        <xsl:variable name="cn">
            <xsl:choose>
                <xsl:when test="string-length(normalize-space($parentClass))=0">
                    <xsl:value-of select="@Name"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="concat(normalize-space($parentClass), '.', @Name)"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:text>
    </xsl:text>
        <xsl:element name="DocComment">
            <xsl:attribute name="Name">
                <xsl:value-of select="$cn"/>
            </xsl:attribute>
            <xsl:attribute name="Namespace">
                <xsl:value-of select="$parentNs"/>
            </xsl:attribute>
            <xsl:text>
    /// References
    /// </xsl:text>
            <xsl:element name="list">
                <xsl:attribute name="type">
                    <xsl:text>bullet</xsl:text>
                </xsl:attribute>
                <xsl:for-each select="r:ReferenceTag">
                    <xsl:variable name="tag" select="normalize-space(.)"/>
                    <xsl:apply-templates select="msxsl:node-set($Sources)/b:Source[b:Tag=$tag]" mode="Reference" />
                </xsl:for-each>
                <xsl:text>
    /// </xsl:text>
            </xsl:element>
            <xsl:text>
    </xsl:text>
        </xsl:element>
        <xsl:text>
</xsl:text>
        <xsl:apply-templates select="r:Type">
            <xsl:with-param name="parentNs" select="$parentNs" />
            <xsl:with-param name="parentClass" select="$cn" />
        </xsl:apply-templates>
    </xsl:template>
    <!--*2889#-->
    <xsl:template match="b:Source" mode="Reference">
        <xsl:variable name="primaryContributors">
            <xsl:apply-templates select="." mode="GetPrimaryContributors" />
        </xsl:variable>
        <xsl:variable name="title">
            <xsl:variable name="title" select="normalize-space(b:Title)"/>
            <xsl:choose>
                <xsl:when test="string-length($title)=0">
                    <xsl:apply-templates select="." mode="GetEffectiveShortTitle" />
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="$title"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="shortTitle">
            <xsl:variable name="shortTitle">
                <xsl:apply-templates select="." mode="GetEffectiveShortTitle" />
            </xsl:variable>
            <xsl:if test="not($shortTitle=$title)">
                <xsl:value-of select="$shortTitle"/>
            </xsl:if>
        </xsl:variable>
        <xsl:variable name="effectiveOrg">
            <xsl:apply-templates select="." mode="GetEffectiveOrg" />
        </xsl:variable>
        <xsl:variable name="pubDate">
            <xsl:call-template name="GetNormalizedDate">
                <xsl:with-param name="year" select="b:Year" />
                <xsl:with-param name="month" select="b:Month" />
                <xsl:with-param name="day" select="b:Day" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="accessDate">
            <xsl:call-template name="GetNormalizedDate">
                <xsl:with-param name="year" select="b:YearAccessed" />
                <xsl:with-param name="month" select="b:MonthAccessed" />
                <xsl:with-param name="day" select="b:DayAccessed" />
                <xsl:with-param name="noNd" select="true()" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="url">
            <xsl:variable name="url" select="normalize-space(b:URL)" />
            <xsl:if test="not($url=$title)">
                <xsl:value-of select="$url"/>
            </xsl:if>
        </xsl:variable>
        <xsl:variable name="doi">
            <xsl:variable name="doi" select="normalize-space(b:DOI)" />
            <xsl:if test="not(string-length($doi)=0 or $doi=$title)">
                <xsl:value-of select="concat('doi: ', $doi)"/>
            </xsl:if>
        </xsl:variable>

        <xsl:choose>
            <xsl:when test="not(string-length($primaryContributors)=0)">
                <xsl:choose>
                    <xsl:when test="not(string-length($effectiveOrg)=0)">
                        <xsl:choose>
                            <xsl:when test="not(string-length($accessDate)=0)">
                                <xsl:choose>
                                    <xsl:when test="not(string-length($url)=0)">
                                        <xsl:choose>
                                            <xsl:when test="not(string-length($shortTitle)=0)">
                                                <xsl:call-template name="RenderItemElement">
                                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                                    <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from ', $shortTitle, ': ', $url)" />
                                                    <xsl:with-param name="line3" select="$doi" />
                                                </xsl:call-template>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:call-template name="RenderItemElement">
                                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                                    <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from: ', $url)" />
                                                    <xsl:with-param name="line3" select="$doi" />
                                                </xsl:call-template>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </xsl:when>
                                    <xsl:when test="not(string-length($shortTitle)=0)">
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from ', $shortTitle)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:when>
                            <xsl:when test="not(string-length($url)=0)">
                                <xsl:choose>
                                    <xsl:when test="not(string-length($shortTitle)=0)">
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                            <xsl:with-param name="line2" select="concat('Retrieved from ', $shortTitle, ': ', $url)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                            <xsl:with-param name="line2" select="concat('url: ', $url)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:when>
                            <xsl:when test="not(string-length($shortTitle)=0)">
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                    <xsl:with-param name="line2" select="concat('Retrieved from ', $shortTitle)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                                    <xsl:with-param name="line2" select="$doi" />
                                </xsl:call-template>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="not(string-length($accessDate)=0)">
                        <xsl:choose>
                            <xsl:when test="not(string-length($url)=0)">
                                <xsl:choose>
                                    <xsl:when test="not(string-length($shortTitle)=0)">
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="$title" />
                                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from ', $shortTitle, ': ', $url)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="$title" />
                                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from: ', $url)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:when>
                            <xsl:when test="not(string-length($shortTitle)=0)">
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$title" />
                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from ', $shortTitle)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$title" />
                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="not(string-length($url)=0)">
                        <xsl:choose>
                            <xsl:when test="not(string-length($shortTitle)=0)">
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$title" />
                                    <xsl:with-param name="line2" select="concat('Retrieved from ', $shortTitle, ': ', $url)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$title" />
                                    <xsl:with-param name="line2" select="concat('url: ', $url)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="not(string-length($shortTitle)=0)">
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="$title" />
                            <xsl:with-param name="line2" select="concat('Retrieved from ', $shortTitle)" />
                            <xsl:with-param name="line3" select="$doi" />
                        </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="$title" />
                            <xsl:with-param name="line2" select="$doi" />
                        </xsl:call-template>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="not(string-length($effectiveOrg)=0)">
                <xsl:choose>
                    <xsl:when test="not(string-length($accessDate)=0)">
                        <xsl:choose>
                            <xsl:when test="not(string-length($url)=0)">
                                <xsl:choose>
                                    <xsl:when test="not(string-length($shortTitle)=0)">
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="$effectiveOrg" />
                                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from ', $shortTitle, ': ', $url)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:call-template name="RenderItemElement">
                                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                            <xsl:with-param name="line1Right" select="$effectiveOrg" />
                                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from: ', $url)" />
                                            <xsl:with-param name="line3" select="$doi" />
                                        </xsl:call-template>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:when>
                            <xsl:when test="not(string-length($shortTitle)=0)">
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$effectiveOrg" />
                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from ', $shortTitle)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$effectiveOrg" />
                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="not(string-length($url)=0)">
                        <xsl:choose>
                            <xsl:when test="not(string-length($shortTitle)=0)">
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$effectiveOrg" />
                                    <xsl:with-param name="line2" select="concat('Retrieved from ', $shortTitle, ': ', $url)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$effectiveOrg" />
                                    <xsl:with-param name="line2" select="concat('url: ', $url)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="not(string-length($shortTitle)=0)">
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="$effectiveOrg" />
                            <xsl:with-param name="line2" select="concat('Retrieved from ', $shortTitle)" />
                            <xsl:with-param name="line3" select="$doi" />
                        </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($primaryContributors, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="concat($title, ' (', $effectiveOrg, ')')" />
                            <xsl:with-param name="line2" select="$doi" />
                        </xsl:call-template>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="not(string-length($accessDate)=0)">
                <xsl:choose>
                    <xsl:when test="not(string-length($url)=0)">
                        <xsl:choose>
                            <xsl:when test="not(string-length($shortTitle)=0)">
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="$shortTitle" />
                                    <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate, ', from: ', $url)" />
                                    <xsl:with-param name="line3" select="$doi" />
                                </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:call-template name="RenderItemElement">
                                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                                    <xsl:with-param name="line1Right" select="concat('Retrieved ', $accessDate, ', from: ', $url)" />
                                    <xsl:with-param name="line2" select="$doi" />
                                </xsl:call-template>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="not(string-length($shortTitle)=0)">
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="$shortTitle" />
                            <xsl:with-param name="line2" select="concat('Retrieved ', $accessDate)" />
                            <xsl:with-param name="line3" select="$doi" />
                        </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="concat('Retrieved ', $accessDate)" />
                            <xsl:with-param name="line3" select="$doi" />
                        </xsl:call-template>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="not(string-length($url)=0)">
                <xsl:choose>
                    <xsl:when test="not(string-length($shortTitle)=0)">
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="$shortTitle" />
                            <xsl:with-param name="line2" select="concat('Retrieved from : ', $url)" />
                            <xsl:with-param name="line3" select="$doi" />
                        </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="RenderItemElement">
                            <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                            <xsl:with-param name="line1Right" select="$url" />
                            <xsl:with-param name="line3" select="$doi" />
                        </xsl:call-template>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="not(string-length($shortTitle)=0)">
                <xsl:call-template name="RenderItemElement">
                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                    <xsl:with-param name="line1Right" select="$shortTitle" />
                    <xsl:with-param name="line2" select="$doi" />
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="RenderItemElement">
                    <xsl:with-param name="line1Left" select="concat($title, ' (', $pubDate, ')')" />
                    <xsl:with-param name="line1Right" select="$doi" />
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="b:Source" mode="GetEffectiveOrg">
        <xsl:variable name="orgElements">
            <xsl:choose>
                <xsl:when test="./b:SourceType='Book' or ./b:SourceType='BookSection' or ./b:SourceType='Report'">
                    <xsl:element name="b:Institution">
                        <xsl:value-of select="normalize-space(b:Publisher)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='JournalArticle' or ./b:SourceType='ArticleInAPeriodical'">
                    <xsl:element name="b:Institution">
                        <xsl:value-of select="normalize-space(b:JournalName)"/>
                    </xsl:element>
                    <xsl:element name="b:Institution">
                        <xsl:value-of select="normalize-space(b:Publisher)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='ConferenceProceedings'">
                    <xsl:element name="b:Institution">
                        <xsl:value-of select="normalize-space(b:ConferenceName)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='Performance'">
                    <xsl:element name="b:Institution">
                        <xsl:value-of select="normalize-space(b:Theater)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='Art'">
                    <xsl:element name="b:Institution">
                        <xsl:value-of select="normalize-space(b:Institution)"/>
                    </xsl:element>
                </xsl:when>
            </xsl:choose>
            <xsl:element name="b:Institution">
                <xsl:value-of select="normalize-space(b:ProductionCompany)"/>
            </xsl:element>
            <xsl:element name="b:Institution">
                <xsl:value-of select="normalize-space(b:Institution)"/>
            </xsl:element>
        </xsl:variable>
        <xsl:value-of select="normalize-space(msxsl:node-set($orgElements)/b:Institution[not(string-length(.)=0)][1])"/>
    </xsl:template>
    <xsl:template match="b:Source" mode="GetEffectiveShortTitle">
        <xsl:variable name="titleElements">
            <xsl:element name="b:ShortTitle">
                <xsl:value-of select="normalize-space(b:ShortTitle)"/>
            </xsl:element>
            <xsl:choose>
                <xsl:when test="./b:SourceType='Book' or ./b:SourceType='BookSection'">
                    <xsl:element name="b:ShortTitle">
                        <xsl:value-of select="normalize-space(b:BookTitle)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='ArticleInAPeriodical'">
                    <xsl:element name="b:ShortTitle">
                        <xsl:value-of select="normalize-space(b:PeriodicalTitle)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='InternetSite' or ./b:SourceType='DocumentFromInternetSite'">
                    <xsl:element name="b:ShortTitle">
                        <xsl:value-of select="normalize-space(b:InternetSiteTitle)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='ElectronicSource' or ./b:SourceType='Art' or ./b:SourceType='Misc'">
                    <xsl:element name="b:ShortTitle">
                        <xsl:value-of select="normalize-space(b:PublicationTitle)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='SoundRecording'">
                    <xsl:element name="b:ShortTitle">
                        <xsl:value-of select="normalize-space(b:AlbumTitle)"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="./b:SourceType='Interview'">
                    <xsl:element name="b:ShortTitle">
                        <xsl:value-of select="normalize-space(b:BroadcastTitle)"/>
                    </xsl:element>
                </xsl:when>
            </xsl:choose>
        </xsl:variable>
        <xsl:value-of select="normalize-space(msxsl:node-set($titleElements)/b:ShortTitle[not(string-length(.)=0)][1])"/>
    </xsl:template>
    <xsl:template match="b:Source" mode="GetPrimaryContributors">
        <xsl:variable name="contributorNames">
            <xsl:variable name="normalizedNameLists">
                <xsl:choose>
                    <xsl:when test="./b:SourceType='SoundRecording'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Composer/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Performer/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Performer/b:Corporate" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Artist/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Author/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Author/b:Corporate" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Conductor/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:ProducerName/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:when test="./b:SourceType='Performance'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Writer/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Director/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Performer/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Performer/b:Corporate" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Artist/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:when test="./b:SourceType='Art'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Artist/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:when test="./b:SourceType='Film'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:ProducerName/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Writer/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Director/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:when test="./b:SourceType='Interview'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Interviewee/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Interviewer/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Editor/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Translator/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:when test="./b:SourceType='Patent'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Inventor/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:when test="./b:SourceType='Book' or ./b:SourceType='BookSection'">
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Inventor/b:NameList/b:BookAuthor" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Author/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Author/b:Corporate" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Editor/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Translator/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Author/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Author/b:Corporate" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Inventor/b:NameList/b:BookAuthor" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Editor/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:Translator/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                        <xsl:element name="b:NameList">
                            <xsl:apply-templates select="./b:Author/b:ProducerName/b:NameList/b:Person" mode="Normalize" />
                        </xsl:element>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:apply-templates select="msxsl:node-set($normalizedNameLists)/b:NameList[not(string-length(normalize-space(.))=0)][1]/b:Person" mode="ToNameElement" />
        </xsl:variable>
        <xsl:variable name="count" select="count(msxsl:node-set($contributorNames)/Name)" />
        <xsl:choose>
            <xsl:when test="$count=1">
                <xsl:value-of select="msxsl:node-set($contributorNames)/Name[1]"/>
            </xsl:when>
            <xsl:when test="$count=2">
                <xsl:value-of select="concat(msxsl:node-set($contributorNames)/Name[1], ' &amp; ', msxsl:node-set($contributorNames)/Name[2])"/>
            </xsl:when>
            <xsl:when test="$count&gt;7">
                <xsl:for-each select="msxsl:node-set($contributorNames)/Name[position()&lt;7]">
                    <xsl:value-of select="concat(normalize-space(.), ', ')"/>
                </xsl:for-each>
                <xsl:value-of select="concat(' &#x2026;, ', string(msxsl:node-set($contributorNames)/Name[$count]))"/>
            </xsl:when>
            <xsl:when test="$count&gt;2">
                <xsl:for-each select="msxsl:node-set($contributorNames)/Name[position()&lt;$count]">
                    <xsl:value-of select="concat(normalize-space(.), ', ')"/>
                </xsl:for-each>
                <xsl:value-of select="concat(' &amp; ', string(msxsl:node-set($contributorNames)/Name[$count]))"/>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="b:Corporate" mode="Normalize">
        <xsl:variable name="name" select="normalize-space(.)" />
        <xsl:if test="not(string-length($name)=0)">
            <xsl:element name="b:Person">
                <xsl:element name="b:Last">
                    <xsl:value-of select="string($name)"/>
                </xsl:element>
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="b:Person" mode="Normalize">
        <xsl:variable name="first" select="normalize-space(b:First)" />
        <xsl:variable name="last" select="normalize-space(b:Last)" />
        <xsl:variable name="middle" select="normalize-space(b:Middle)" />
        <xsl:choose>
            <xsl:when test="not(string-length($last)=0)">
                <xsl:element name="b:Person">
                    <xsl:element name="b:Last">
                        <xsl:value-of select="string($last)"/>
                    </xsl:element>
                    <xsl:variable name="fi">
                        <xsl:call-template name="ToInitial">
                            <xsl:with-param name="name" select="$first" />
                        </xsl:call-template>
                    </xsl:variable>
                    <xsl:variable name="mi">
                        <xsl:call-template name="ToInitial">
                            <xsl:with-param name="name" select="$middle" />
                        </xsl:call-template>
                    </xsl:variable>
                    <xsl:choose>
                        <xsl:when test="not(string-length(normalize-space($fi))=0)">
                            <xsl:element name="b:First">
                                <xsl:value-of select="string($fi)"/>
                            </xsl:element>
                            <xsl:if test="not(string-length(normalize-space($mi))=0)">
                                <xsl:element name="b:Middle">
                                    <xsl:value-of select="string($mi)"/>
                                </xsl:element>
                            </xsl:if>
                        </xsl:when>
                        <xsl:when test="not(string-length(normalize-space($mi))=0)">
                            <xsl:element name="b:First">
                                <xsl:value-of select="string($mi)"/>
                            </xsl:element>
                        </xsl:when>
                    </xsl:choose>
                </xsl:element>
            </xsl:when>
            <xsl:when test="not(string-length($first)=0)">
                <xsl:element name="b:Person">
                    <xsl:element name="b:Last">
                        <xsl:value-of select="string($first)"/>
                    </xsl:element>
                    <xsl:variable name="mi">
                        <xsl:call-template name="ToInitial">
                            <xsl:with-param name="name" select="$middle" />
                        </xsl:call-template>
                    </xsl:variable>
                    <xsl:if test="not(string-length(normalize-space($mi))=0)">
                        <xsl:element name="b:First">
                            <xsl:value-of select="string($mi)"/>
                        </xsl:element>
                    </xsl:if>
                </xsl:element>
            </xsl:when>
            <xsl:when test="not(string-length($middle)=0)">
                <xsl:element name="b:Person">
                    <xsl:element name="b:Last">
                        <xsl:value-of select="string($middle)"/>
                    </xsl:element>
                </xsl:element>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="b:Person" mode="ToNameElement">
        <xsl:variable name="first" select="normalize-space(b:First)" />
        <xsl:variable name="last" select="normalize-space(b:Last)" />
        <xsl:variable name="middle" select="normalize-space(b:Middle)" />
        <xsl:choose>
            <xsl:when test="not(string-length($middle)=0)">
                <xsl:element name="Name">
                    <xsl:value-of select="concat($last, ', ', $first, ' ', $middle)"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="not(string-length($first)=0)">
                <xsl:element name="Name">
                    <xsl:value-of select="concat($last, ', ', $first)"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="not(string-length($last)=0)">
                <xsl:element name="Name">
                    <xsl:value-of select="$last"/>
                </xsl:element>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="b:Source" mode="ShortTitle">
        <xsl:variable name="titleNodes">
            <xsl:copy-of select="b:ShortTitle"/>
            <xsl:copy-of select="b:Title"/>
            <xsl:copy-of select="b:BookTitle"/>
            <xsl:copy-of select="b:PublicationTitle"/>
            <xsl:copy-of select="b:PeriodicalTitle"/>
            <xsl:copy-of select="b:InternetSiteTitle"/>
            <xsl:copy-of select="b:CaseNumber"/>
        </xsl:variable>
        <xsl:variable name="title" select="normalize-space(msxsl:node-set($titleNodes)[not(string-length(normalize-space(.))=0)][1])" />
        <xsl:choose>
            <xsl:when test="not(string-length($title)=0)">
                <xsl:apply-templates select="." mode="Tag">
                    <xsl:with-param name="isLabel" select="true()" />
                </xsl:apply-templates>
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
    <xsl:template match="b:Source" mode="Tag">
        <xsl:param name="isLabel" select="false()" />
        <xsl:if test="not(string-length(normalize-space(b:Tag))=0)">
            <xsl:choose>
                <xsl:when test="$isLabel=true()">
                    <xsl:value-of select="concat(normalize-space(b:Tag), ': ')"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="normalize-space(b:Tag)"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:if>
    </xsl:template>
    <xsl:template match="b:Source" mode="ID">
        <xsl:param name="fragment" select="false()" />
        <xsl:choose>
            <xsl:when test="$fragment=true()">
                <xsl:call-template name="ToLowerCase">
                    <xsl:with-param name="text" select="concat('#', translate(normalize-space(b:Guid), '{}-', ''))"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="ToLowerCase">
                    <xsl:with-param name="text" select="translate(normalize-space(b:Guid), '{}-', '')"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="RemoveZeroPad">
        <xsl:param name="value" select="''" />
        <xsl:choose>
            <xsl:when test="string-length($value)&gt;1 and starts-with($value, '0') and contains($Digits, substring($value, 2, 1))">
                <xsl:call-template name="RemoveZeroPad">
                    <xsl:with-param name="value" select="substring($value, 2)" />
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$value"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="GetNormalizedYear">
        <xsl:param name="value" select="''" />
        <xsl:variable name="normalized" select="normalize-space($value)" />
        <xsl:if test="string-length($normalized)&gt;0 and contains($Digits, substring($normalized, 1, 1))">
            <xsl:variable name="y" select="number($normalized)" />
            <xsl:choose>
                <xsl:when test="$y&gt;999 and $y &lt;10000">
                    <xsl:value-of select="string($y)"/>
                </xsl:when>
                <xsl:when test="string-length(string($y))=2">
                    <xsl:choose>
                        <xsl:when test="$y&lt;40">
                            <xsl:value-of select="concat('20', string($y))"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:value-of select="concat('19', string($y))"/>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
            </xsl:choose>
        </xsl:if>
    </xsl:template>
    <xsl:template name="GetNormalizedMonth">
        <xsl:param name="value" select="''" />
        <xsl:variable name="lc">
            <xsl:call-template name="ToLowerCase">
                <xsl:with-param name="text" select="translate($value, '.', ' ')" />
                <xsl:with-param name="normalize" select="true()" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
            <xsl:when test="$lc='january' or $lc='jan'">
                <xsl:text>January</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='february' or $lc='feb'">
                <xsl:text>February</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='march' or $lc='mar'">
                <xsl:text>March</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='april' or $lc='apr'">
                <xsl:text>April</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='may'">
                <xsl:text>May</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='june' or $lc='jun'">
                <xsl:text>June</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='july' or $lc='jul'">
                <xsl:text>July</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='august' or $lc='aug'">
                <xsl:text>August</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='september' or $lc='sep' or $lc='sept'">
                <xsl:text>September</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='october' or $lc='oct'">
                <xsl:text>October</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='november' or $lc='nov'">
                <xsl:text>November</xsl:text>
            </xsl:when>
            <xsl:when test="$lc='december' or $lc='dec'">
                <xsl:text>December</xsl:text>
            </xsl:when>
            <xsl:when test="string-length($lc)&gt;0 and contains($Digits, substring($lc, 1, 1))">
                <xsl:variable name="m" select="number($lc)" />
                <xsl:choose>
                    <xsl:when test="$m=1">
                        <xsl:text>January</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=2">
                        <xsl:text>February</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=3">
                        <xsl:text>March</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=4">
                        <xsl:text>April</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=5">
                        <xsl:text>May</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=6">
                        <xsl:text>June</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=7">
                        <xsl:text>July</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=8">
                        <xsl:text>August</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=9">
                        <xsl:text>September</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=10">
                        <xsl:text>October</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=11">
                        <xsl:text>November</xsl:text>
                    </xsl:when>
                    <xsl:when test="$m=12">
                        <xsl:text>December</xsl:text>
                    </xsl:when>
                </xsl:choose>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="GetMonthNumber">
        <xsl:param name="value" select="''" />
        <xsl:variable name="lc">
            <xsl:call-template name="ToLowerCase">
                <xsl:with-param name="text" select="translate($value, '.', ' ')" />
                <xsl:with-param name="normalize" select="true()" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
            <xsl:when test="$lc='january' or $lc='jan'">
                <xsl:value-of select="1"/>
            </xsl:when>
            <xsl:when test="$lc='february' or $lc='feb'">
                <xsl:value-of select="2"/>
            </xsl:when>
            <xsl:when test="$lc='march' or $lc='mar'">
                <xsl:value-of select="3"/>
            </xsl:when>
            <xsl:when test="$lc='april' or $lc='apr'">
                <xsl:value-of select="4"/>
            </xsl:when>
            <xsl:when test="$lc='may'">
                <xsl:value-of select="5"/>
            </xsl:when>
            <xsl:when test="$lc='june' or $lc='jun'">
                <xsl:value-of select="6"/>
            </xsl:when>
            <xsl:when test="$lc='july' or $lc='jul'">
                <xsl:value-of select="7"/>
            </xsl:when>
            <xsl:when test="$lc='august' or $lc='aug'">
                <xsl:value-of select="8"/>
            </xsl:when>
            <xsl:when test="$lc='september' or $lc='sep' or $lc='sept'">
                <xsl:value-of select="9"/>
            </xsl:when>
            <xsl:when test="$lc='october' or $lc='oct'">
                <xsl:value-of select="10"/>
            </xsl:when>
            <xsl:when test="$lc='november' or $lc='nov'">
                <xsl:value-of select="11"/>
            </xsl:when>
            <xsl:when test="$lc='december' or $lc='dec'">
                <xsl:value-of select="12"/>
            </xsl:when>
            <xsl:when test="string-length($lc)&gt;0 and contains($Digits, substring($lc, 1, 1))">
                <xsl:variable name="m" select="number($lc)" />
                <xsl:if test="$m&gt;0 and $m&lt;13">
                    <xsl:value-of select="$m"/>
                </xsl:if>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="GetNormalizedDay">
        <xsl:param name="value" select="''" />
        <xsl:param name="month" select="''" />
        <xsl:variable name="normalized" select="normalize-space($value)" />
        <xsl:if test="string-length($normalized)&gt;0 and contains($Digits, substring($normalized, 1, 1))">
            <xsl:variable name="d" select="number($normalized)" />
            <xsl:variable name="m">
                <xsl:call-template name="GetMonthNumber">
                    <xsl:with-param name="value" select="$month" />
                </xsl:call-template>
            </xsl:variable>
            <xsl:choose>
                <xsl:when test="$d&gt;0 and $d&lt;10">
                    <xsl:value-of select="concat('0', string($d))"/>
                </xsl:when>
                <xsl:when test="$d&gt;9 and $d&lt;30">
                    <xsl:value-of select="string($d)"/>
                </xsl:when>
                <xsl:when test="string-length($m)=0">
                    <xsl:if test="$d=30 or $d=31">
                        <xsl:value-of select="string($d)"/>
                    </xsl:if>
                </xsl:when>
                <xsl:when test="$m=4 or $m=6 or $m=9 or $m=11">
                    <xsl:if test="$d=30">
                        <xsl:value-of select="string($d)"/>
                    </xsl:if>
                </xsl:when>
                <xsl:when test="not($m=2)">
                    <xsl:value-of select="string($d)"/>
                </xsl:when>
            </xsl:choose>
        </xsl:if>
    </xsl:template>
    <xsl:template name="GetNormalizedDate">
        <xsl:param name="year" select="''" />
        <xsl:param name="month" select="''" />
        <xsl:param name="day" select="''" />
        <xsl:param name="noNd" select="false()" />
        <xsl:variable name="normalizedYear">
            <xsl:call-template name="GetNormalizedYear">
                <xsl:with-param name="value" select="$year" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="normalizedMonth">
            <xsl:call-template name="GetNormalizedMonth">
                <xsl:with-param name="value" select="$month" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="normalizedDay">
            <xsl:call-template name="GetNormalizedDay">
                <xsl:with-param name="value" select="$day" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
            <xsl:when test="string-length($normalizedYear)=0">
                <xsl:if test="not($noNd)">
                    <xsl:text>n.d.</xsl:text>
                </xsl:if>
            </xsl:when>
            <xsl:when test="string-length($normalizedMonth)=0">
                <xsl:value-of select="$normalizedYear"/>
            </xsl:when>
            <xsl:when test="string-length($normalizedDay)=0">
                <xsl:value-of select="concat($normalizedYear, ', ', $normalizedMonth)"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="concat($normalizedYear, ', ', $normalizedMonth, ' ', $normalizedDay)"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="ToInitial">
        <xsl:param name="name" select="''" />
        <xsl:variable name="normalizedName" select="normalize-space($name)" />
        <xsl:if test="not(string-length($normalizedName)=0)">
            <xsl:variable name="initial" select="substring($normalizedName, 1, 1)" />
            <xsl:choose>
                <xsl:when test="contains($AlphaNum, $initial)">
                    <xsl:value-of select="concat($initial, '.')"/>
                </xsl:when>
                <xsl:when test="not(string-length($normalizedName)=0)">
                    <xsl:call-template name="ToInitial">
                        <xsl:with-param name="name" select="substring($normalizedName, 2)" />
                    </xsl:call-template>
                </xsl:when>
            </xsl:choose>
        </xsl:if>
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
    <xsl:template name="ToUcFirst">
        <xsl:param name="text" select="''" />
        <xsl:choose>
            <xsl:when test="string-length($text)=1">
                <xsl:choose>
                    <xsl:when test="contains($LcLetters, $text)">
                        <xsl:value-of select="translate($text, $LcLetters, $UcLetters)"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="$text"/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            <xsl:when test="string-length($text)&gt;1">
                <xsl:variable name="c" select="substring($text, 1, 1)" />
                <xsl:choose>
                    <xsl:when test="contains($LcLetters, $c)">
                        <xsl:value-of select="concat(translate($c, $LcLetters, $UcLetters), substring($text, 2))"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="$text"/>
                    </xsl:otherwise>
                </xsl:choose>
                <xsl:value-of select="concat(translate(normalize-space($text), $LcLetters, $UcLetters), substring($text, 2))"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$text"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="RenderItemElement">
        <xsl:param name="line1Left" select="''" />
        <xsl:param name="line1Right" select="''" />
        <xsl:param name="line2" select="''" />
        <xsl:param name="line3" select="''" />
        <xsl:choose>
            <xsl:when test="string-length(normalize-space($line2))=0">
                <xsl:text>
    ///     </xsl:text>
                <xsl:element name="item">
                    <xsl:element name="term">
                        <xsl:value-of select="$line1Left"/>
                    </xsl:element>
                    <xsl:text>
    ///         </xsl:text>
                    <xsl:element name="description">
                        <xsl:value-of select="$line1Right"/>
                    </xsl:element>
                    <xsl:text>
    ///     </xsl:text>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:text>
    ///     </xsl:text>
                <xsl:element name="item">
                    <xsl:element name="term">
                        <xsl:value-of select="$line1Left"/>
                    </xsl:element>
                    <xsl:text>
    ///         </xsl:text>
                    <xsl:element name="description">
                        <xsl:value-of select="$line1Right"/>
                        <xsl:text>
    ///         </xsl:text>
                        <xsl:element name="para">
                            <xsl:value-of select="$line2"/>
                        </xsl:element>
                        <xsl:if test="not(string-length(normalize-space($line3))=0)">
                            <xsl:text>
    ///         </xsl:text>
                            <xsl:element name="para">
                                <xsl:value-of select="$line3"/>
                            </xsl:element>
                        </xsl:if>
                    </xsl:element>
                    <xsl:text>
    ///     </xsl:text>
                </xsl:element>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
