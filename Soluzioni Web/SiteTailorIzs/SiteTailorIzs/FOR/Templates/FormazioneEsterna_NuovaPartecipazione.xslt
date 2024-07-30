<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt"/>
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />
  
  <xsl:template match="/">
    <xsl:for-each select="tipi/tipo">
      <span class="btnlink">
        <xsl:attribute name="onclick">
          <xsl:text>nuovaPartecipazione('</xsl:text>
          <xsl:value-of select="@ac_tipopartecipazione"/>
          <xsl:text>');</xsl:text>
        </xsl:attribute>
        Nuova
          <xsl:value-of select="@tx_tipopartecipazione"/>
      </span>
      <xsl:call-template name="nbsp" />
      
      
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>