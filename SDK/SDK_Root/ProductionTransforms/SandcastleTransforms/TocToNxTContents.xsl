<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1">

  <xsl:output indent="yes" encoding="UTF-8" />

  <xsl:param name="includeIds" />
  <xsl:param name="htmlDir" select="'html'"/>
  
  <xsl:template match="/">
    <HelpTOC xmlns="http://schemas.nasutek.com/2013/Help5/Help42Extensions">
      <xsl:apply-templates select="/topics" />
    </HelpTOC>
  </xsl:template>

  <xsl:template match="topic">
    <HelpTOCNode>
      <xsl:if test="boolean($includeIds)">
        <xsl:attribute name="Id">
          <xsl:value-of select="@id"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="Url">
        <xsl:value-of select="concat('html\',@file,'.htm')" />
      </xsl:attribute>
      <xsl:attribute name="Title">
        <xsl:choose>
          <xsl:when test="@title">
            <xsl:value-of select="@title"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="document(concat($htmlDir,'\',@file,'.htm'))/html/head/title"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates />
    </HelpTOCNode>
  </xsl:template>
</xsl:stylesheet>
