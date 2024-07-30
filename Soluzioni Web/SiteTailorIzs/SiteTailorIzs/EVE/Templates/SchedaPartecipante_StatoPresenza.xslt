<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">

  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <div style="position:absolute;top:26px;left:430px;">
      <xsl:apply-templates select="root" />
    </div>
  </xsl:template>

  <xsl:template match="root">
    <table style="font-family:Verdana, Arial;font-size:11px;">
      <tr>
        <td>
          Durata complessiva dell'evento (minuti)
        </td>
        <td style="padding-left:20px;text-align:right;font-weight:bold;">
          <xsl:value-of select="@durataevento"/>          
        </td>
      </tr>
      <tr>
        <td>
          Presenza minima richiesta (minuti)
        </td>
        <td style="padding-left:20px;text-align:right;font-weight:bold;">
          <xsl:value-of select="@minimominuti"/>
        </td>
      </tr>
      <tr>
        <td>
          Presenza accumulata dal partecipante (minuti)
        </td>
        <td style="padding-left:20px;text-align:right;font-weight:bold;">
          <span>
            <xsl:attribute name="style">
              <xsl:choose>
                <xsl:when test="@presenzaok='1'">
                  <xsl:value-of select="'color:#00aa00'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'color:#ff0000'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="@minutiiscritto"/>
          </span>
          
        </td>
      </tr>
    </table>
  
  </xsl:template>
</xsl:stylesheet>

