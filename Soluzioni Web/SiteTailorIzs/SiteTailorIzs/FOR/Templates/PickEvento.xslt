<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt"/>
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />
  
  <xsl:template match="/">
    <xsl:for-each select="eventi/evento">
      <div class="evento">
        <xsl:attribute name="onclick">
          <xsl:text>parent.stl_sel_done('</xsl:text>
          <xsl:value-of select="@id_evento"/>
          <xsl:text>');</xsl:text>
        </xsl:attribute>
        <div>
          <xsl:value-of select="@tx_tipologiaevento"/>
        </div>
        <div>
          <b>
            <xsl:value-of select="@tx_titolo"/>
          </b>
        </div>
        <div>
          <b>
            <xsl:call-template name="dataDalAl_it">
              <xsl:with-param name="dataDal" select="@dt_inizio" />
              <xsl:with-param name="dataAl" select="@dt_fine" />
            </xsl:call-template>
          </b>
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_sede"/>
          <xsl:if test="@ac_nazione!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_nazione"/>
          </xsl:if>
        </div>
        <div>
          <em>Organizzatore: </em>
          <xsl:value-of select="@tx_organizzatore"/>
        </div>
      </div>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>