<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />
  <xsl:param name="node" />
  <xsl:param name="searchactive" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <div class="title blue">Nuovo modulo di richiesta</div>
    <p>
      <i>Inserisci un nuovo evento</i>
    </p>
    <p>
      <a class="btnlink btnlink_blue" href="javascript:SubmitBtn_Click(0)">
        Nuovo Evento
      </a>
    </p>
    <div class="">
      <xsl:choose>
        <xsl:when test="count(eventi/evento)=0">
          <xsl:apply-templates select="eventi" />
          Non ci sono eventi esistenti con i parametri di ricerca impostati.
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="eventi" />
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="eventi">
    <p>
      <i>oppure ricerca un evento esistente utilizzando la maschera di ricerca a destra e seleziona l'evento desiderato.</i>
    </p>
    <table class="pf_table">
      <tr>
        <th>ID</th>
        <th>Date</th>
        <th>Sede</th>
        <th>Titolo evento</th>
        <th>Tipologia evento</th>
        <th>Comandi</th>
      </tr>
      <xsl:apply-templates select="evento" />
    </table>
  </xsl:template>

  <xsl:template match="evento">
    <tr class="everow">
      <td style="text-align:center;font-weight:bold;">
        <xsl:value-of select="@id_evento"/>
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
      <td>
        <xsl:value-of select="@tx_sede"/>
      </td>
      <td style="font-weight:bold;">
        <xsl:value-of select="@tx_titolo"/>
      </td>
      <td>
        <xsl:value-of select="@tx_tipologiaevento"/>
      </td>
      <td style="font-weight:bold;">
        <a class="classica_nu" href="javascript:SubmitBtn_Click({@id_evento})">
          Seleziona
        </a>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
