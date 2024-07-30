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
      Con la presente ti ricordiamo che sei <b>iscritto al seguente evento formativo</b>:
    </div>
    <div>
      <br/>
    </div>
    <!-- dati evento -->
    <xsl:call-template name="dati_evento" />
    <div>
      <br/>
    </div>
    <!-- ulteriori informazioni -->
    <div>
      <b>Qualora decidessi di rinunciare alla partecipazione</b>, ti raccomandiamo
      di cancellare la tua iscrizione contattando la Segreteria Organizzativa
      per cedere il tuo posto ad eventuali altri partecipanti in
      lista d'attesa.
    </div>
    <xsl:if test="@ht_notemailpromemoria!=''">
      <div>
        <br/>
      </div>
      <div>
        <xsl:value-of select="@ht_notemailpromemoria" disable-output-escaping="yes" />
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
