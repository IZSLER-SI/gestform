<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="fl_profiloecm" />

  <xsl:template match="/">
    <div class="title blue top20">auto-certificazioni in attesa di validazione</div>
    <div class="top20">
      <xsl:choose>
        <xsl:when test="count(autocertificazioni/autocertificazione)=0">
          Al momento non ci sono auto-certificazioni in attesa di validazione da parte dell'Ufficio Formazione.
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="autocertificazioni" />
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="autocertificazioni">
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
      <xsl:apply-templates select="autocertificazione" />
    </table>
  </xsl:template>
  
  <xsl:template match="autocertificazione">
    <tr class="everow">
      <td style="text-align:center;font-weight:bold;">
        <xsl:value-of select="@ni_numero"/>/<xsl:value-of select="@ni_anno"/>
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
          <a target="_blank" class="classica_nu">
            <xsl:attribute name="href">
              <xsl:value-of select="'/stampa-auto-certificazione/'" />
              <xsl:value-of select="@id_partecipazione"/>
            </xsl:attribute>
            Stampa
          </a>
        </div>
        <div>
          <asp:LinkButton runat="server" CssClass="classica_nu">
            <xsl:attribute name="ID">
              <xsl:value-of select="'lnkModificaAutoCertificazione_'"/>
              <xsl:value-of select="@id_partecipazione"/>
            </xsl:attribute>
            <xsl:attribute name="CommandArgument">
              <xsl:value-of select="@id_partecipazione"/>
            </xsl:attribute>
            Modifica
          </asp:LinkButton>
        </div>
        <div>
          <asp:LinkButton runat="server" CssClass="classica_nu" OnClientClick="return(confirm('Confermi l\'eliminazione dell\'auto-certificazione?'));">
            <xsl:attribute name="ID">
              <xsl:value-of select="'lnkEliminaAutoCertificazione_'"/>
              <xsl:value-of select="@id_partecipazione"/>
            </xsl:attribute>
            <xsl:attribute name="CommandArgument">
              <xsl:value-of select="@id_partecipazione"/>
            </xsl:attribute>
            Elimina
          </asp:LinkButton>
        </div>
      </td>
    </tr>
  </xsl:template>
  
</xsl:stylesheet>
