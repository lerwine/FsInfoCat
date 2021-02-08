<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
        xmlns:r="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" version="1.0">
    <xsl:output method="html" media-type="text/html" omit-xml-declaration="yes" doctype-public="html" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" />
    <xsl:param name="notPassedOnlyThreshold" select="100" />
    <xsl:template name="FindReplaceString">
        <xsl:param name="originalString" />
        <xsl:param name="stringToBeReplaced" />
        <xsl:param name="stringReplacement" />
        <xsl:choose>
            <xsl:when test="contains($originalString,$stringToBeReplaced)">
                <xsl:value-of select="concat(substring-before($originalString,$stringToBeReplaced),$stringReplacement)" />
                <xsl:call-template name="FindReplaceString">
                    <xsl:with-param name="originalString" select="substring-after($originalString,$stringToBeReplaced)" />
                    <xsl:with-param name="stringToBeReplaced" select="$stringToBeReplaced" />
                    <xsl:with-param name="stringReplacement" select="$stringReplacement" />
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$originalString" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="@*" mode="Counts">
        <xsl:element name="li">
            <xsl:attribute name="class">
                <xsl:choose>
                    <xsl:when test="number(.)='0'">outcome-other</xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="OutcomeToClassName"><xsl:with-param name="outcome" select="local-name()" /></xsl:call-template>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:attribute>
            <xsl:if test="position()&gt;1">
                <xsl:text>; </xsl:text>
            </xsl:if>
            <xsl:element name="span">
                <xsl:call-template name="OutcomeToDislayText"><xsl:with-param name="outcome" select="local-name()" /></xsl:call-template>
                <xsl:text>: </xsl:text>
            </xsl:element>
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="/">
        <xsl:element name="html">
            <xsl:element name="head">
                <xsl:element name="meta">
                    <xsl:attribute name="charset">utf-8</xsl:attribute>
                </xsl:element>
                <xsl:element name="meta">
                    <xsl:attribute name="http-equiv">X-UA-Compatible</xsl:attribute>
                    <xsl:attribute name="content">IE=edge</xsl:attribute>
                </xsl:element>
                <xsl:element name="title">
                    <xsl:value-of select="r:TestRun/@name" />
                </xsl:element>
                <xsl:element name="meta">
                    <xsl:attribute name="name">description</xsl:attribute>
                    <xsl:attribute name="content">
                        <xsl:value-of select="concat('NUnit test started on ',msxsl:format-date(r:TestRun/r:Times/@start,'yyyy-MM-dd HH:mm:ss'),' and finished on ',msxsl:format-date(r:TestRun/r:Times/@finish,'yyyy-MM-dd HH:mm:ss'))" />
                    </xsl:attribute>
                </xsl:element>
                <xsl:element name="meta">
                    <xsl:attribute name="name">viewport</xsl:attribute>
                    <xsl:attribute name="content">width=device-width, initial-scale=1</xsl:attribute>
                </xsl:element>
                <xsl:element name="style">
                    <xsl:attribute name="type">text/css</xsl:attribute>
                    <xsl:text disable-output-escaping="yes"><![CDATA[
body {
    font-family: Arial, Helvetica, sans-serif;
    background-color: white;
    color:black;
}

ul.counts {
    list-style-type: none;
}

.outcome-passed {
    color:darkgreen;
}

.outcome-failed {
    color:maroon;
}

.outcome-warning {
    color:darkmagenta;
}

.outcome-inconclusive {
    color:dimgray;
}

.outcome-other {
    color:black;
}

section.summary li {
    display: inline-block;
    white-space: nowrap;
}

section.summary li span {
    font-weight: bold;
}

section.outputs h3, section.outputs h4 {
    margin-bottom:0px;
}

section.outputs pre {
    margin-top:0px;
}
]]></xsl:text>
                </xsl:element>
            </xsl:element>
            <xsl:element name="body">
                <xsl:variable name="resultItems">
                    <xsl:element name="Results">
                        <xsl:choose>
                            <xsl:when test="number(/r:TestRun/r:ResultSummary/r:Counters/@total)&gt;$notPassedOnlyThreshold and not(/r:TestRun/r:ResultSummary/r:Counters/@total=/r:TestRun/r:ResultSummary/r:Counters/@passed)">
                                <xsl:apply-templates select="/r:TestRun/r:Results/r:UnitTestResult[not(@outcome='Passed')]" mode="CompileResultItem" />
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:apply-templates select="/r:TestRun/r:Results/r:UnitTestResult" mode="CompileResultItem" />
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:element>
                </xsl:variable>
                <xsl:element name="section">
                    <xsl:attribute name="class">summary</xsl:attribute>
                    <xsl:element name="h1">
                        <xsl:attribute name="id">summary</xsl:attribute>
                        <xsl:value-of select="concat('NUnit Test &quot;',r:TestRun/r:TestSettings/@name,'&quot; Summary')" />
                    </xsl:element>
                    <xsl:element name="table">
                        <xsl:element name="tbody">
                            <xsl:element name="tr">
                                <xsl:element name="th">
                                    <xsl:attribute name="scope">row</xsl:attribute>
                                    <xsl:text>Outcome: </xsl:text>
                                </xsl:element>
                                <xsl:element name="td">
                                    <xsl:attribute name="class">
                                        <xsl:call-template name="OutcomeToClassName">
                                            <xsl:with-param name="outcome" select="r:TestRun/r:ResultSummary/@outcome" />
                                        </xsl:call-template>
                                    </xsl:attribute>
                                    <xsl:call-template name="OutcomeToDislayText">
                                        <xsl:with-param name="outcome" select="r:TestRun/r:ResultSummary/@outcome" />
                                    </xsl:call-template>
                                </xsl:element>
                            </xsl:element>
                            <xsl:element name="tr">
                                <xsl:element name="th">
                                    <xsl:text>Started: </xsl:text>
                                </xsl:element>
                                <xsl:element name="td">
                                    <xsl:value-of select="msxsl:format-date(r:TestRun/r:Times/@start,'M/d/yyyy')" />
                                    <xsl:text> at </xsl:text>
                                    <xsl:value-of select="msxsl:format-date(r:TestRun/r:Times/@start,'h:mm tt')" />
                                </xsl:element>
                            </xsl:element>
                            <xsl:element name="tr">
                                <xsl:element name="th">
                                    <xsl:text>Finished: </xsl:text>
                                </xsl:element>
                                <xsl:element name="td">
                                    <xsl:value-of select="msxsl:format-date(r:TestRun/r:Times/@finish,'M/d/yyyy')" />
                                    <xsl:text> at </xsl:text>
                                    <xsl:value-of select="msxsl:format-date(r:TestRun/r:Times/@finish,'h:mm tt')" />
                                </xsl:element>
                            </xsl:element>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="h2">
                        <xsl:text>Counts</xsl:text>
                    </xsl:element>
                    <xsl:element name="ul">
                        <xsl:apply-templates select="r:TestRun/r:ResultSummary/r:Counters/@*[position()&gt;1]" mode="Counts" />
                    </xsl:element>
                </xsl:element>
                <xsl:element name="section">
                    <xsl:attribute name="class">testResults</xsl:attribute>
                    <xsl:element name="h1">
                        <xsl:choose>
                            <xsl:when test="number(/r:TestRun/r:ResultSummary/r:Counters/@total)&gt;$notPassedOnlyThreshold and not(/r:TestRun/r:ResultSummary/r:Counters/@total=/r:TestRun/r:ResultSummary/r:Counters/@passed)">
                                <xsl:text>Test Results (not passed only)</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:text>Test Results</xsl:text>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:element>
                    <xsl:element name="ul">
                        <xsl:for-each select="msxsl:node-set($resultItems)/Results/UnitTestResult">
                            <xsl:sort select="@className"></xsl:sort>
                            <xsl:variable name="className"><xsl:value-of select="@className" /></xsl:variable>
                            <xsl:if test="count(preceding-sibling::UnitTestResult[@className=$className])=0">
                                <xsl:element name="li">
                                    <xsl:choose>
                                        <xsl:when test="count(../UnitTestResult[@className=$className and not(count(r:*))=0])=0">
                                            <xsl:value-of select="@className" />
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:element name="a">
                                                <xsl:attribute name="href">
                                                    <xsl:value-of select="concat('#c:', $className)" />
                                                </xsl:attribute>
                                                <xsl:value-of select="@className" />
                                            </xsl:element>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:element name="table">
                                        <xsl:element name="thead">
                                            <xsl:element name="th">Outcome</xsl:element>
                                            <xsl:element name="th">Duration</xsl:element>
                                            <xsl:element name="th">Name</xsl:element>
                                            <xsl:element name="th">Priority</xsl:element>
                                        </xsl:element>
                                        <xsl:element name="tbody">
                                            <xsl:for-each select="msxsl:node-set($resultItems)/Results/UnitTestResult[@className=$className]">
                                                <xsl:sort select="@testName"></xsl:sort>
                                                <xsl:element name="tr">
                                                    <xsl:attribute name="class">
                                                        <xsl:call-template name="OutcomeToClassName">
                                                            <xsl:with-param name="outcome" select="@outcome" />
                                                        </xsl:call-template>
                                                    </xsl:attribute>
                                                    <xsl:element name="td">
                                                        <xsl:call-template name="OutcomeToDislayText"><xsl:with-param name="outcome" select="@outcome" /></xsl:call-template>
                                                    </xsl:element>
                                                    <xsl:element name="td">
                                                        <xsl:value-of select="@duration" />
                                                    </xsl:element>
                                                    <xsl:element name="td">
                                                        <xsl:choose>
                                                            <xsl:when test="count(r:*)=0">
                                                                <xsl:value-of select="@testName" />
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:element name="a">
                                                                    <xsl:attribute name="href">
                                                                        <xsl:value-of select="concat('#t:', @id)" />
                                                                    </xsl:attribute>
                                                                    <xsl:value-of select="@testName" />
                                                                </xsl:element>
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </xsl:element>
                                                    <xsl:element name="td">
                                                        <xsl:value-of select="@priority" />
                                                    </xsl:element>
                                                </xsl:element>
                                            </xsl:for-each>
                                        </xsl:element>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:if>
                        </xsl:for-each>
                    </xsl:element>
                </xsl:element>
                <xsl:for-each select="msxsl:node-set($resultItems)/Results/UnitTestResult">
                    <xsl:sort select="@className"></xsl:sort>
                    <xsl:variable name="className"><xsl:value-of select="@className" /></xsl:variable>
                    <xsl:if test="count(preceding-sibling::UnitTestResult[@className=$className and not(count(../UnitTestResult[@className=$className and not(count(r:*)=0)])=0)])=0">
                        <xsl:element name="section">
                            <xsl:attribute name="class">outputs</xsl:attribute>
                            <xsl:element name="h1">
                                <xsl:attribute name="id">
                                    <xsl:value-of select="concat('c:', $className)" />
                                </xsl:attribute>
                                <xsl:value-of select="concat('Test class ', @className, ' outputs')" />
                            </xsl:element>
                            <xsl:apply-templates select="msxsl:node-set($resultItems)/Results/UnitTestResult[@className=$className]" mode="Details">
                                <xsl:sort select="@testName"></xsl:sort>
                            </xsl:apply-templates>
                        </xsl:element>
                    </xsl:if>
                </xsl:for-each>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="UnitTestResult" mode="Details">
        <xsl:element name="h2">
            <xsl:attribute name="id">
                <xsl:value-of select="concat('t:', @id)" />
            </xsl:attribute>
            <xsl:value-of select="@testName" />
        </xsl:element>
        <xsl:element name="table">
            <xsl:element name="tbody">
                <xsl:element name="tr">
                    <xsl:element name="th">
                        <xsl:attribute name="scope">row</xsl:attribute>
                        <xsl:text>Outcome: </xsl:text>
                    </xsl:element>
                    <xsl:element name="td">
                        <xsl:attribute name="class">
                            <xsl:call-template name="OutcomeToClassName">
                                <xsl:with-param name="outcome" select="@outcome" />
                            </xsl:call-template>
                        </xsl:attribute>
                        <xsl:call-template name="OutcomeToDislayText"><xsl:with-param name="outcome" select="@outcome" /></xsl:call-template>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="tr">
                    <xsl:element name="th">
                        <xsl:text>Start Time: </xsl:text>
                    </xsl:element>
                    <xsl:element name="td">
                        <xsl:value-of select="msxsl:format-date(@startTime,'M/d/yyyy')" />
                        <xsl:text> at </xsl:text>
                        <xsl:value-of select="msxsl:format-date(@startTime,'h:mm tt')" />
                    </xsl:element>
                </xsl:element>
                <xsl:element name="tr">
                    <xsl:element name="th">
                        <xsl:text>End Time: </xsl:text>
                    </xsl:element>
                    <xsl:element name="td">
                        <xsl:value-of select="msxsl:format-date(@endTime,'M/d/yyyy')" />
                        <xsl:text> at </xsl:text>
                        <xsl:value-of select="msxsl:format-date(@endTime,'h:mm tt')" />
                    </xsl:element>
                </xsl:element>
                <xsl:element name="tr">
                    <xsl:element name="th">
                        <xsl:text>Duration: </xsl:text>
                    </xsl:element>
                    <xsl:element name="td">
                        <xsl:value-of select="@duration" />
                    </xsl:element>
                </xsl:element>
            </xsl:element>
        </xsl:element>
        <xsl:apply-templates select="r:*" mode="Outputs" />
    </xsl:template>
    <xsl:template match="r:ErrorInfo" mode="Outputs">
        <xsl:apply-templates select="r:*" mode="ErrorInfo" />
    </xsl:template>
    <xsl:template match="r:*" mode="Outputs">
        <xsl:element name="h3">Other output</xsl:element>
        <xsl:element name="pre">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:Message" mode="ErrorInfo">
        <xsl:element name="h3">Message</xsl:element>
        <xsl:element name="pre">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:StackTrace" mode="ErrorInfo">
        <xsl:element name="h4">Stack Trace</xsl:element>
        <xsl:element name="pre">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:*" mode="ErrorInfo">
        <xsl:element name="h3">Other error info</xsl:element>
        <xsl:element name="pre">
            <xsl:value-of select="." />
        </xsl:element>
    </xsl:template>
    <xsl:template match="r:UnitTestResult" mode="CompileResultItem">
        <xsl:variable name="testId"><xsl:value-of select="@testId" /></xsl:variable>
        <xsl:element name="UnitTestResult">
            <xsl:attribute name="id">
                <xsl:value-of select="translate(@testId,'-', '')" />
            </xsl:attribute>
            <xsl:attribute name="testId">
                <xsl:value-of select="@testId" />
            </xsl:attribute>
            <xsl:attribute name="testName">
                <xsl:value-of select="@testName" />
            </xsl:attribute>
            <xsl:attribute name="outcome">
                <xsl:value-of select="@outcome" />
            </xsl:attribute>
            <xsl:attribute name="duration">
                <xsl:value-of select="@duration" />
            </xsl:attribute>
            <xsl:attribute name="startTime">
                <xsl:value-of select="@startTime" />
            </xsl:attribute>
            <xsl:attribute name="endTime">
                <xsl:value-of select="@endTime" />
            </xsl:attribute>
            <xsl:attribute name="priority">
                <xsl:value-of select="/r:TestRun/r:TestDefinitions/r:UnitTest[@id=$testId]/@priority" />
            </xsl:attribute>
            <xsl:attribute name="className">
                <xsl:value-of select="/r:TestRun/r:TestDefinitions/r:UnitTest[@id=$testId]/r:TestMethod/@className" />
            </xsl:attribute>
            <xsl:copy-of select="r:Output/r:*"></xsl:copy-of>
        </xsl:element>
    </xsl:template>
    <xsl:template name="OutcomeToClassName">
        <xsl:param name="outcome" />
        <xsl:variable name="uc"><xsl:value-of select="translate($outcome,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" /></xsl:variable>
        <xsl:choose>
            <xsl:when test="$uc='PASSEDBUTRUNABORTED' or $uc='PASSED (RUN ABORTED)' or $uc='PASSED'">outcome-passed</xsl:when>
            <xsl:when test="$uc='TIMEOUT' or $uc='ERROR' or $uc='FAILED'">outcome-failed</xsl:when>
            <xsl:when test="$uc='ABORTED' or $uc='NOTRUNNABLE' or $uc='NOT RUNNABLE' or $uc='DISCONNECTED' or $uc='WARNING'">outcome-warning</xsl:when>
            <xsl:when test="$uc='NOTEXECUTED' or $uc='NOT EXECUTED' or $uc='INCONCLUSIVE'">outcome-inconclusive</xsl:when>
            <xsl:otherwise><xsl:text>outcome-other</xsl:text></xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template name="OutcomeToDislayText">
        <xsl:param name="outcome" />
        <xsl:variable name="uc"><xsl:value-of select="translate($outcome,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" /></xsl:variable>
        <xsl:choose>
            <xsl:when test="$uc='PASSEDBUTRUNABORTED'">Passed (run aborted)</xsl:when>
            <xsl:when test="$uc='NOTEXECUTED'">Not executed</xsl:when>
            <xsl:when test="$uc='INPROGRESS'">In progress</xsl:when>
            <xsl:when test="$uc='NOTRUNNABLE'">Not runnable</xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="concat(substring($uc,1,1),substring($outcome,2))" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
