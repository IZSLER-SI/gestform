<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Templates/Common.xslt"/>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <div class="stl_dfo" style="position:absolute;top:305px;left:500px;width:470px;">
      <div class="title">
        Scadenze <span style="font-weight:normal;font-size:17px;">(prossimi 
          <xsl:value-of select="scheda/@ni_giorni_max"/>
          <xsl:value-of select="noscheda/@ni_giorni_max"/>
          giorni o scadute)</span>
      </div>
    </div>
    <div class="stl_gen_box" style="position: absolute; width: 470px; top:335px; left: 500px;">
      <div class="content padall" style="height:345px">
        <xsl:apply-templates select="noscheda" />
        <xsl:apply-templates select="scheda" />
      </div>
    </div>
    
    
    
  </xsl:template>
  <xsl:template match="noscheda">
    <div>Per questo evento non è stata creata una scheda di programmazione.</div>
  </xsl:template>
  <xsl:template match="scheda">
    <xsl:choose>
      <xsl:when test="count(data)=0">
        Non ci sono scadenze nei prossimi
        <xsl:value-of select="@ni_giorni_max"/>
        giorni per questo evento.
      </xsl:when>
      <xsl:otherwise>
        <div class="wipevento">
          <ul>
            <xsl:apply-templates select="data" />
          </ul>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="data">
    <li>
      <xsl:attribute name="class">
        <xsl:choose>
          <xsl:when test="@ni_giorniscadenza&lt;0">
            <xsl:value-of select="'expired'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'active'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <b>
        <xsl:call-template name="dataDDMMYYYY">
          <xsl:with-param name="data" select="@dt_scadenza" />
        </xsl:call-template>
      </b>
      <xsl:choose>
        <xsl:when test="@ni_giorniscadenza&lt;-1">
          (<xsl:value-of select="-@ni_giorniscadenza"/> giorni fa)
        </xsl:when>
        <xsl:when test="@ni_giorniscadenza=-1">
          (ieri)
        </xsl:when>
        <xsl:when test="@ni_giorniscadenza=0">
          (oggi)
        </xsl:when>
        <xsl:when test="@ni_giorniscadenza=1">
          (domani)
        </xsl:when>
        <xsl:when test="@ni_giorniscadenza&gt;1">
          (tra <xsl:value-of select="@ni_giorniscadenza"/> giorni)
        </xsl:when>
      </xsl:choose>
      <ul>
        <xsl:apply-templates select="scadparent" />
        <xsl:apply-templates select="scadchild" />
      </ul>
    </li>
  </xsl:template>
  <xsl:template match="scadparent">
    <li class="normal">
      <xsl:value-of select="@tx_label"/>
    </li>
  </xsl:template>
  <xsl:template match="scadchild">
    <li class="normal">
      <xsl:value-of select="@tx_label"/> per:
      <ul>
        <xsl:for-each select="child">
          <li class="normal">
            <xsl:value-of select="@tx_cognome"/>
            <xsl:value-of select="' '"/>
            <xsl:value-of select="@tx_nome"/>
            <xsl:value-of select="' ('"/>
            <xsl:value-of select="@ac_categoriaecm"/>
            <xsl:value-of select="')'"/>
          </li>
        </xsl:for-each>
      </ul>
    </li>
  </xsl:template>
</xsl:stylesheet>
