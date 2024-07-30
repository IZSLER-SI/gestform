<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <div class="title green">
      <xsl:value-of select="@tx_titoloevento"/>
      <xsl:if test="@tx_edizione!=''">
        <xsl:value-of select="' - '"/>
        <xsl:value-of select="@tx_edizione"/>
      </xsl:if>
    </div>
    <div style="padding-top:5px;padding-bottom:20px;">
      <b>
        <xsl:value-of select="@tx_sede"/>
        <xsl:value-of select="', '"/>
        <xsl:call-template name="dataDalAl_it_mmmm">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </b>
    </div>

    <div class="green">
      <b>
        I dati sono stati inseriti correttamente.
      </b>
    </div>
    <div>
      <br/>
    </div>
    <div>
      <span class="btnlink btnlink_blue" onclick="gotoHome();">Torna alla home page</span>
    </div>
    
  </xsl:template>

</xsl:stylesheet>
