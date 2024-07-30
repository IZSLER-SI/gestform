<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <div>
      <div class="stl_cus_grd_hdr">
        <table>
          <tr>
            <td class="l">
              <b>Persone da includere</b>
              <span>
                <xsl:choose>
                  <xsl:when test="count(root/iscritto)=1">
                    <xsl:value-of select="'(numero totale: 1)'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'(numero totale: '"/>
                    <xsl:value-of select="count(root/iscritto)"/>
                    <xsl:value-of select="')'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </span>
            </td>
          </tr>
        </table>
      </div>
      <table class="stl_cus_grd">
        <tr class="ph">
          <th>
            <xsl:call-template name="nbsp" />
          </th>
        </tr>
        <tr class="h">
          <th scope="col">Selezione</th>
          <th scope="col">Categoria</th>
          <th scope="col">Cognome</th>
          <th scope="col">Nome</th>
          <th scope="col">Matricola</th>
        </tr>
        <xsl:apply-templates select="root/iscritto" />
      </table>
    </div>
  </xsl:template>

  <xsl:template match="iscritto">
    <tr class="rnc">
      <td style="text-align:center;">
        <asp:CheckBox runat="server" CssClass="selezcb">
          <xsl:attribute name="ID">
            <xsl:value-of select="'chkIscritto_'"/>
            <xsl:value-of select="@id_iscritto"/>
          </xsl:attribute>
        </asp:CheckBox>
      </td>
      <td>
        <xsl:value-of select="@tx_categoriaecm"/>
      </td>
      <td>
        <b>
          <xsl:value-of select="@tx_cognome"/>
        </b>
      </td>
      <td>
        <b>
          <xsl:value-of select="@tx_nome"/>
        </b>
      </td>
      <td>
        <xsl:value-of select="@ac_matricola"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>