<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />

  <xsl:template match="/">
    <div class="onecol">
      <div class="title green bottom20">
        DOWNLOAD
      </div>
      <xsl:choose>
        <xsl:when test="count(files/file)=0">
            <div>Al momento non è disponibile nessun download.</div>
        </xsl:when>
        <xsl:otherwise>
          <div class="download">
            <xsl:apply-templates select="files/file" />
          </div>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>
  
  <xsl:template match="file">
    <a class="dl_item" target="_blank">
      <xsl:attribute name="href">
        <xsl:value-of select="'/Docs/'"/>
        <xsl:value-of select="@file"/>
      </xsl:attribute>
      <div class="img">
        <img>
          <xsl:attribute name="src">
            <xsl:value-of select="'/DocThumbs/'"/>
            <xsl:value-of select="@thumbnail"/>
          </xsl:attribute>
        </img>
      </div>
      <div class="text">
        <div class="name">
          <xsl:value-of select="@tx_descrizione"/>
        </div>
        <xsl:if test="count(@ht_note)&gt;0">
          <div class="userhtml">
            <xsl:value-of select="@ht_note" disable-output-escaping="yes" />
          </div>
        </xsl:if>
      </div>
      <div class="clear">
        <xsl:value-of select="''" />
      </div>
    </a>
  </xsl:template>
  
</xsl:stylesheet>
