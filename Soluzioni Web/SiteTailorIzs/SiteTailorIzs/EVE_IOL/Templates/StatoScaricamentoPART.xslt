<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <table cellspacing="0" cellpadding="0" border="0" class="mytable">
      <tr>
        <td class="mylabel">
          Persone presenti, per le quali lo scaricamento dell'attestato è stato attivato, che <b>NON HANNO</b> scaricato l'attestato di partecipazione ad oggi<br/>
          <em>Clicca sul nome per visualizzare l'attestato</em>
        </td>
        <td class="mydata">
          <div class="mylist">
            <xsl:apply-templates select="persone/persona[@fl_downloadattpart=0]" />
          </div>
        </td>
      </tr>
    </table>
    <table cellspacing="0" cellpadding="0" border="0" class="mytable">
      <tr>
        <td class="mylabel">
          Persone presenti, per le quali lo scaricamento dell'attestato è stato attivato, che <b>HANNO</b> scaricato l'attestato di partecipazione ad oggi<br/>
          <em>Clicca sul nome per visualizzare l'attestato</em>
        </td>
        <td class="mydata">
          <div class="mylist">
            <xsl:apply-templates select="persone/persona[@fl_downloadattpart=1]" />
          </div>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="persona">
    <div>
      <a class="mya">
        <xsl:attribute name="href">
          <xsl:value-of select="'../EVE/'"/>
          <xsl:value-of select="'AttestatoPart.aspx?i='"/>
          <xsl:value-of select="@id_iscritto"/>
        </xsl:attribute>
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="', '"/>
        <xsl:value-of select="@tx_nome"/>
      </a>
      <xsl:if test="@ac_matricola!=''">
        <xsl:value-of select="' ('"/>
        <xsl:value-of select="@ac_matricola"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:value-of select="' - '"/>
      <xsl:value-of select="@tx_categoriaecm"/>
      <xsl:if test="@dt_downloadattpart!=''">
        <span class="mydatascar">
          <xsl:value-of select="' - scaricato il '"/>
          <xsl:call-template name="dataDDMMYY">
            <xsl:with-param name="data" select="@dt_downloadattpart" />
          </xsl:call-template>
        </span>
      </xsl:if>
    </div>
  </xsl:template>

</xsl:stylesheet>