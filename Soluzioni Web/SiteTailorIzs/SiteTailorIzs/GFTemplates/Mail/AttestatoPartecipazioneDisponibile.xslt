<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../FormatCommon.xslt"/>
  <xsl:output method="xml" indent="yes" />
  <xsl:param name="baseurl" />
  <xsl:param name="tx_VOCATIVO" />
  <xsl:param name="tx_COGNOME" />
  <xsl:param name="tx_NOME" />
  <xsl:param name="tx_TITOLOEVENTO" />
  <xsl:param name="tx_SEDE" />
  <xsl:param name="dt_INIZIO" />
  <xsl:param name="dt_FINE" />


  <xsl:template match="/">
    <div>
      <xsl:value-of select="$tx_VOCATIVO"/>
      <xsl:value-of select="' '"/>
      <b>
        <xsl:value-of select="$tx_NOME"/>
        <xsl:value-of select="' '"/>
        <xsl:value-of select="$tx_COGNOME"/>
      </b>
      <xsl:value-of select="','"/>
    </div>
    <div>
      <br/>
    </div>
    <!-- con la presente...-->
    <div>
      Con la presente ti informiamo che <b>è disponibile per lo scaricamento il tuo attestato di partecipazione</b> relativo al seguente evento formativo:
    </div>
    <div>
      <br/>
    </div>
    <!-- dati evento -->
    <xsl:call-template name="dati_evento" />
    <div>
      <br/>
    </div>
    <div>
      Per scaricare l'attestato, collegati al Portale Formazione al seguente indirizzo:<br/>
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="$baseurl"/>
        </xsl:attribute>
        <xsl:value-of select="$baseurl"/>
      </a>
    </div>
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
        <xsl:value-of select="$tx_TITOLOEVENTO"/>
      </b>
    </div>
    <div>
      Data/date:
      <b>
        <xsl:call-template name="dataDalAl_it">
          <xsl:with-param name="dataDal" select="$dt_INIZIO" />
          <xsl:with-param name="dataAl" select="$dt_FINE" />
        </xsl:call-template>
      </b>
    </div>
    <div>
      Sede:
      <b>
        <xsl:value-of select="$tx_SEDE"/>
      </b>
    </div>
  </xsl:template>
</xsl:stylesheet>
