<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:c="http://schemas.softailor.com/ReportEngine/Fonte"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:asp="remove"
                exclude-result-prefixes="c"
                >
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <xsl:apply-templates select="c:Fonte" />
  </xsl:template>

  <xsl:template match="c:Fonte">
    <!-- ordinamento: se ce n'è almeno uno -->
    <xsl:choose>
      <xsl:when test="count(c:Ordinamenti/c:Ordinamento)&gt;0">
        <table class="datatable">
          <td class="lbl" style="width:80px;padding-top:4px;">
            <b>Ordinamento</b>
          </td>
          <td>
            <asp:RadioButtonList runat="server" ID="rblOrdinamento">
              <xsl:for-each select="c:Ordinamenti/c:Ordinamento">
                <asp:ListItem>
                  <xsl:attribute name="Value">
                    <xsl:value-of select="@Descrizione"/>
                  </xsl:attribute>
                </asp:ListItem>
              </xsl:for-each>
              <!-- personalizzato -->
              <xsl:if test="count(c:CampiCorpo/c:Campo[@Ordinamento='true'])&gt;0">
                <asp:ListItem Text="Personalizzato" Value="_CUSTOM_" />  
              </xsl:if>
            </asp:RadioButtonList>
            <!-- ordinamento personalizzato -->
            <xsl:if test="count(c:CampiCorpo/c:Campo[@Ordinamento='true'])&gt;0">
              <xsl:call-template name="ddnOrdinamento">
                <xsl:with-param name="num" select="'1'" />
              </xsl:call-template>
              <xsl:call-template name="ddnOrdinamento">
                <xsl:with-param name="num" select="'2'" />
              </xsl:call-template>
              <xsl:call-template name="ddnOrdinamento">
                <xsl:with-param name="num" select="'3'" />
              </xsl:call-template>
              <xsl:call-template name="ddnOrdinamento">
                <xsl:with-param name="num" select="'4'" />
              </xsl:call-template>
              <xsl:call-template name="ddnOrdinamento">
                <xsl:with-param name="num" select="'5'" />
              </xsl:call-template>
            </xsl:if>
          </td>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <div style="padding-left:6px;">Nessuna opzione disponibile per questo tipo di modello.</div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="ddnOrdinamento">
    <xsl:param name="num" />
    <div style="padding-left:20px;">
      <asp:DropDownList runat="server" CssClass="ddn" Width="400px">
        <xsl:attribute name="ID">
          <xsl:value-of select="'ddnOrdinamento'"/>
          <xsl:value-of select="$num"/>
        </xsl:attribute>
        <asp:ListItem Text="" Value="" />
        <xsl:for-each select="c:CampiCorpo/c:Campo[@Ordinamento='true']">
          <asp:ListItem>
            <xsl:attribute name="Text">
              <xsl:value-of select="@Descrizione"/>
            </xsl:attribute>
            <xsl:attribute name="Value">
              <xsl:value-of select="@NomeDb"/>
            </xsl:attribute>

          </asp:ListItem>            
        </xsl:for-each>
        
      </asp:DropDownList>
    </div>
  </xsl:template>
</xsl:stylesheet>
