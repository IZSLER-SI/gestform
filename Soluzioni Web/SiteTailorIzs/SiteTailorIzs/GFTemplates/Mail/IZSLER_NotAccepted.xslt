<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../FormatCommon.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="baseurl" />
  <xsl:template match="/">
    <xsl:apply-templates select="eventi/evento" />
  </xsl:template>

  <xsl:template match="evento">
    <div>
      <xsl:value-of select="@tx_vocativo"/>
      <xsl:value-of select="' '"/>
      <b>
        <xsl:value-of select="@tx_nome"/>
        <xsl:value-of select="' '"/>
        <xsl:value-of select="@tx_cognome"/>
      </b>
      <xsl:value-of select="','"/>
    </div>
    <div>
      <br/>
    </div>
    <!-- con la presente...-->
    <div>
      Siamo spiacenti di informarti che la tua richiesta di iscrizione al seguente evento formativo, 
      che era stata inserita in lista d'attesa, <b>non è stata accettata a causa dell'esaurimento dei posti disponibili:</b>
    </div>
    <div>
      <br/>
    </div>
    <!-- dati evento -->
    <xsl:call-template name="dati_evento" />
    <xsl:if test="@ht_notemailnonaccettazione!=''">
      <div>
        <br/>
      </div>
      <div>
        <xsl:value-of select="@ht_notemailnonaccettazione" disable-output-escaping="yes" />
      </div>
    </xsl:if>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div>
      Distinti Saluti
    </div>
  </xsl:template>

  <xsl:template name="dati_evento">
    <div>
      <b>
        <xsl:value-of select="@tx_titolo"/>
        <xsl:if test="@tx_edizione!=''">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_edizione"/>
        </xsl:if>
      </b>
    </div>
    <div>
      Data:
      <b>
        <xsl:call-template name="dataDalAl_it">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </b>
    </div>
    <div>
      Sede:
      <b>
        <xsl:value-of select="@tx_sede"/>
      </b>
    </div>
    <xsl:if test="@tx_iol_note!=''">
      <div style="color:red;">
        <xsl:value-of select="@tx_iol_note"/>
      </div>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
