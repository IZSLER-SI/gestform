<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />
  <xsl:param name="selectedid" />

  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="count(news/news)=0">
        <div class="gcontent">
          <div class="title green">
            NOTIZIE
          </div>
          <div>Al momento non è disponibile nessuna notizia.</div>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="folders">
          <div class="folders_left">
            <xsl:apply-templates select="news/news" mode="listitem" />
          </div>
          <div class="folders_right">
            <xsl:choose>
              <xsl:when test="$selectedid=''">
                <xsl:apply-templates select="news/news[1]" mode="content" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="news/news[@id_news=$selectedid]" mode="content" />
              </xsl:otherwise>
            </xsl:choose>
            
          </div>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="news" mode="listitem">
    <xsl:choose>
      <xsl:when test="(position()=1 and $selectedid='') or ($selectedid=@id_news)">
        <div class="active">
          <xsl:attribute name="href">
            <xsl:value-of select="'/News/'"/>
            <xsl:value-of select="@id_news"/>
          </xsl:attribute>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@dt_creazione" />
          </xsl:call-template>
          <br/>
          <b>
            <xsl:value-of select="@tx_descrizione"/>
          </b>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <a class="enab">
          <xsl:attribute name="href">
            <xsl:value-of select="'/News/'"/>
            <xsl:value-of select="@id_news"/>
          </xsl:attribute>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@dt_creazione" />
          </xsl:call-template>
          <br/>
          <b>
            <xsl:value-of select="@tx_descrizione"/>
          </b>
        </a>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="news" mode="content">
    <div style="padding-bottom:5px;">
      <strong>
        <xsl:call-template name="data_it_mmmm">
          <xsl:with-param name="data" select="@dt_creazione" />
        </xsl:call-template>
      </strong>
    </div>
    <div class="title green bottom20">
      <xsl:value-of select="@tx_descrizione"/>
    </div>
    <div class="userhtml">
      <xsl:value-of select="@ht_note" disable-output-escaping="yes" />
      <xsl:value-of select="''"/>
    </div>
  </xsl:template>
</xsl:stylesheet>
