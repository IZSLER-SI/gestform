<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />

  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="count(news/news)=0">
        <div class="title blue bottom20">
          ULTIME NOTIZIE
        </div>
        <div>Al momento non è disponibile nessuna notizia.</div>
      </xsl:when>
      <xsl:otherwise>
        <a class="title blue bottom20" href="/News">
          ULTIME NOTIZIE
        </a>
        <xsl:apply-templates select="news/news" />
        <div>
          <a class="classica" href="/news" style="font-weight:bold;">Tutte le notizie</a>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="news">
    <a class="hp_news">
      <xsl:attribute name="href">
        <xsl:value-of select="'/news/'"/>
        <xsl:value-of select="@id_news"/>
      </xsl:attribute>
      <xsl:if test="@fl_hilite=1">
        <img src="/img/important.png" style="float:left;margin-right:5px;" />
      </xsl:if>
      <xsl:call-template name="data_it_mmmm">
        <xsl:with-param name="data" select="@dt_creazione" />
      </xsl:call-template>
      <br/>
      <b class="blue">
        <xsl:value-of select="@tx_descrizione"/>
      </b>
    </a>
    
  </xsl:template>
</xsl:stylesheet>
