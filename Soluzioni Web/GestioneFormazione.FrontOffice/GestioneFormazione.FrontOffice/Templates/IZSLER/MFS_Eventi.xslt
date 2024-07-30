<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="fl_profiloecm" />

  <xsl:template match="/">
    <div class="title blue top20">Richieste in attesa di validazione</div>
    <div class="top20">
      <xsl:choose>
        <xsl:when test="count(eventi/evento)=0">
          Al momento non ci sono richieste in attesa di validazione da parte dell'Ufficio Formazione.
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="eventi" />
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="eventi">
    <table class="pf_table">
      <tr>
        <th>Numero</th>
        <th>Data</th>
        <th>Ruolo</th>
        <th>Titolo evento</th>
        <th>Tipologia evento</th>
        <th>Date</th>
        <th>Comandi</th>
      </tr>
      <xsl:apply-templates select="evento" />
    </table>
  </xsl:template>

  <xsl:template match="evento">
    <tr class="everow">
      <td style="text-align:center;font-weight:bold;">
          <xsl:value-of select="@ac_tipopartecipazione"/>-<xsl:value-of select="@ni_numero"/>/<xsl:value-of select="@ni_anno"/>
      </td>
      <td>
        <xsl:call-template name="dataDDMMYYYY">
          <xsl:with-param name="data" select="@dt_data" />
        </xsl:call-template>
      </td>
      <td>
        <xsl:value-of select="@tx_categoriaecm"/>
      </td>
      <td style="font-weight:bold;">
        <xsl:value-of select="@tx_titolo"/>
      </td>
      <td>
        <xsl:value-of select="@tx_tipologiaevento"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@fl_fad='1'">
            <xsl:call-template name="dataDalAl_it_mmm">
              <xsl:with-param name="dataDal" select="@dt_iniziofruizione" />
              <xsl:with-param name="dataAl" select="@dt_finefruizione" />
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="dataDalAl_it_mmm">
              <xsl:with-param name="dataDal" select="@dt_inizio" />
              <xsl:with-param name="dataAl" select="@dt_fine" />
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td style="font-weight:bold;">
        <div>
          <a class="classica_nu" href="javascript:StampaBtn_Click({@id_partecipazione})">
            Download
          </a>
        </div>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
