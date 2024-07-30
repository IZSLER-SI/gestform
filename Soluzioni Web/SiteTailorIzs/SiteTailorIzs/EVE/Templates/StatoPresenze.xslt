<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="display" />
  <xsl:template match="/">
    <h1>
      <xsl:choose>
        <xsl:when test="$display='TUTTI'">
          Partecipanti presenti ed assenti in questo istante
        </xsl:when>
        <xsl:when test="$display='PRESENTI'">
          Partecipanti presenti in questo istante
        </xsl:when>
        <xsl:when test="$display='ASSENTI'">
          Partecipanti assenti in questo istante
        </xsl:when>
      </xsl:choose>
    </h1>
    <table width="100%">
      <tr>
        <th>
          Cognome, Nome, Matricola
        </th>
        <th>
          Stato
        </th>
        <th>
          Orario ingresso
        </th>
      </tr>
      <xsl:choose>
        <xsl:when test="$display='TUTTI'">
          <xsl:apply-templates select="persone/persona" />
        </xsl:when>
        <xsl:when test="$display='PRESENTI'">
          <xsl:apply-templates select="persone/persona[@fl_presente='1']" />
        </xsl:when>
        <xsl:when test="$display='ASSENTI'">
          <xsl:apply-templates select="persone/persona[@fl_presente='0']" />
        </xsl:when>
      </xsl:choose>
    </table>
  </xsl:template>

  <xsl:template match="persona">
    <tr>
      <td>
        <xsl:value-of select="@cognommat" disable-output-escaping="yes"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@fl_presente='1'">
            Presente
          </xsl:when>
          <xsl:when test="@fl_presente='0'">
            Assente
          </xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="@tm_inizio"/>
        <xsl:value-of select="'&amp;nbsp;'" disable-output-escaping="yes"/>
      </td>
    </tr>
    
  </xsl:template>

</xsl:stylesheet>