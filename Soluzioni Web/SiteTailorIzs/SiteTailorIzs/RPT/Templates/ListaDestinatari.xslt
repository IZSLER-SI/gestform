<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:asp="remove"
                >
  <xsl:import href="../../Templates/Common.xslt"/>
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <div>
      <div class="stl_cus_grd_hdr">
        <table>
          <tr>
            <td class="l">
              <b>Selezione Destinatari</b>
              <span>
                <xsl:choose>
                  <xsl:when test="count(root/item)=1">
                    <xsl:value-of select="'(numero totale: 1)'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'(numero totale: '"/>
                    <xsl:value-of select="count(root/item)"/>
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
          <th scope="col">N°</th>
          <th scope="col">Selezione</th>
          <th scope="col">Anteprima</th>
          <xsl:for-each select="root/field">
            <th scope="col">
              <xsl:value-of select="@desc"/>
            </th>
          </xsl:for-each>
          <th scope="col">E-mail</th>
          
        </tr>
        <xsl:choose>
          <xsl:when test="count(root/item)=0">
            <tr class="rnc">
              <td>
                <xsl:attribute name="colspan">
                  <xsl:value-of select="count(root/field) + 4"/>
                </xsl:attribute>
                Nessun destinatario corrisponde ai criteri di ricerca impostati.
              </td>
            </tr>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="root/item" />
          </xsl:otherwise>
        </xsl:choose>
      </table>
    </div>
  </xsl:template>

  <xsl:template match="item">
    <xsl:variable name="thisitem" select="." />
    <tr class="rnc">
      <td style="text-align:center;font-weight:bold;">
        <xsl:value-of select="position()"/>
      </td>
      <td style="text-align:center;">
        <xsl:if test="count(@mail)&gt;0">
          <asp:CheckBox runat="server" CssClass="selezcb">
            <xsl:attribute name="ID">
              <xsl:value-of select="'chksel_'"/>
              <xsl:value-of select="@key"/>
            </xsl:attribute>
          </asp:CheckBox>  
        </xsl:if>
        
      </td>
      <td style="text-align:center;">
        <img src="../../img/icoLens.gif" class="previewimg" title="Anteprima e-mail">
          <xsl:attribute name="onclick">
            <xsl:value-of select="''"/>
            <xsl:text>MailPreview('</xsl:text>
            <xsl:value-of select="@key"/>
            <xsl:text>');</xsl:text>
          </xsl:attribute>
        </img>
      </td>
      <xsl:for-each select="../field">
        <xsl:variable name="fieldname" select="@name" />
        <td>
          <xsl:if test="position()=1">
            <xsl:attribute name="style">
              <xsl:value-of select="'font-weight:bold;'"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$thisitem/@*[name()=$fieldname]"/>
        </td>
      </xsl:for-each>
      <td>
        <xsl:choose>
          <xsl:when test="count(@mail)&gt;0">
            <xsl:value-of select="@mail"/>
          </xsl:when>
          <xsl:otherwise>
            <span style="color:red;">Mancante</span>
          </xsl:otherwise>
        </xsl:choose>

      </td>
      
    </tr>
  </xsl:template>
</xsl:stylesheet>
