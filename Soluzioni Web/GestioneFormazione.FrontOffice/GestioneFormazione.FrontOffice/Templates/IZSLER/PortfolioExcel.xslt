<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes"/>
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <xsl:apply-templates select="eventi" />
  </xsl:template>

  <xsl:template match="eventi">
    <eventi>
      <xsl:attribute name="tx_cognome">
        <xsl:value-of select="@tx_cognome"/>
      </xsl:attribute>
      <xsl:attribute name="tx_nome">
        <xsl:value-of select="@tx_nome"/>
      </xsl:attribute>
      <xsl:attribute name="ac_matricola">
        <xsl:value-of select="@ac_matricola"/>
      </xsl:attribute>
      <xsl:attribute name="fl_profiloecm">
        <xsl:value-of select="@fl_profiloecm"/>
      </xsl:attribute>
      <xsl:apply-templates select="evento" />
    </eventi>
  </xsl:template>


  <xsl:template match="evento">
    <evento>
      <!-- date -->
      <xsl:attribute name="date">
        <xsl:call-template name="dataDalAl_it_mmm">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </xsl:attribute>
      <!-- tipo partecipazione -->
      <xsl:attribute name="tipopartecipazione">
        <xsl:choose>
          <xsl:when test="@ac_tipoglob='EVE_INT'">
            <xsl:value-of select="'Evento organizzato da '" />
            <xsl:value-of select="$companyname"/>
          </xsl:when>
          <xsl:when test="@ac_tipoglob='IND_EXT'">
            <xsl:value-of select="'Formazione individuale esterna'" />
          </xsl:when>
          <xsl:when test="@ac_tipoglob='FOR_IND'">
            <xsl:value-of select="@tx_tipopartecipazione"/>
          </xsl:when>
        </xsl:choose>
      </xsl:attribute>
      <!-- tipo evento -->
      <xsl:attribute name="tipologiaevento">
        <xsl:value-of select="@tx_tipologiaevento"/>
      </xsl:attribute>
      <!-- titolo evento -->
      <xsl:attribute name="titoloevento">
        <xsl:value-of select="@tx_titoloevento"/>
        <xsl:if test="@tx_edizione!=''">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_edizione"/>
        </xsl:if>
      </xsl:attribute>
      <!-- sede evento -->
      <xsl:attribute name="sedeevento">
        <xsl:if test="@tx_sede!='' and @fl_fad=0">
          <xsl:value-of select="@tx_sede"/>
          <xsl:if test="@tx_dettaglisede!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_dettaglisede"/>
          </xsl:if>
        </xsl:if>
      </xsl:attribute>
      <!-- ruolo -->
      <xsl:attribute name="ruolo">
        <xsl:value-of select="@tx_categoriaecm"/>
      </xsl:attribute>
      <!-- crediti ecm -->
      <xsl:attribute name="creditiecm">
        <xsl:if test="../@fl_profiloecm=1">
          <xsl:choose>
            <xsl:when test="@ac_normativaecm='NONE'">
              <xsl:value-of select="''"/>
            </xsl:when>
            <xsl:otherwise>
              <!-- evento accreditato ECM -->
              <xsl:choose>
                <xsl:when test="@ac_statoecm='C'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="@ac_statoecm='NC'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="@ac_statoecm='COK'">
                  <xsl:choose>
                    <xsl:when test="@ac_categoriaecm='P'">
                      <xsl:value-of select="@nd_creditiecm_evento"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="@nd_creditiecm_iscritto"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test="@ac_statoecm='CKO'">
                  <xsl:value-of select="''"/>
                </xsl:when>
              </xsl:choose>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </xsl:attribute>
    </evento>
  </xsl:template>
</xsl:stylesheet>
