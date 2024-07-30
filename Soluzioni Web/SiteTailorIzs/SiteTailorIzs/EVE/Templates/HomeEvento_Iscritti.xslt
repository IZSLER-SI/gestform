<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="accesslevel" />
  
  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>
 
  <xsl:template match="evento">
    <div class="stl_dfo" style="position:absolute;top:305px;left:0px;width:480px;">
      <div class="title">
        Rs, Docenti, Partecipanti, Tutor, Moderatori
      </div>
    </div>
    <div class="stl_gen_box" style="position: absolute; width: 480px; top:330px; left: 0px;">
      <div class="content padall" style="height:320px">
        <div style="float:left;width:190px;">
          <div class="hdrGroupIsc">
            <b>Categorie</b>
          </div>
          <xsl:for-each select="categorieecm">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'CE'" />
            </xsl:call-template>
          </xsl:for-each>
          <div class="hdrGroupIsc">
            <b>Stati Iscrizioni</b>
          </div>
          <xsl:for-each select="statiiscrizione">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'SI'" />
            </xsl:call-template>
          </xsl:for-each>
        </div>
        <div style="float:left;width:15px;">
          <xsl:call-template name="nbsp" />
        </div>
        <div style="float:left;width:262px;">
          <div class="hdrGroupIsc">
            <b>Interni/Esterni</b>
          </div>
          <xsl:for-each select="categoriepersone">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'DE'" />
            </xsl:call-template>
          </xsl:for-each>
          <div class="hdrGroupIsc">
            <b>Origine Iscrizioni</b>
          </div>
          <xsl:for-each select="originiiscrizione">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'OI'" />
            </xsl:call-template>
          </xsl:for-each>
         
          <div class="hdrGroupIsc">
            <b>Presenza minima per ECM</b>
          </div>
          <xsl:for-each select="statipresenza">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'SP'" />
            </xsl:call-template>
          </xsl:for-each>
          <div class="hdrGroupIsc">
            <b>Questionario Apprendimento ECM</b>
          </div>
          <xsl:for-each select="statiquestionario">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'SQ'" />
            </xsl:call-template>
          </xsl:for-each>
          <div class="hdrGroupIsc">
            <b>Crediti ECM</b>
          </div>
          <xsl:for-each select="statiecm">
            <xsl:call-template name="elencoitem">
              <xsl:with-param name="prefix" select="'SE'" />
            </xsl:call-template>
          </xsl:for-each>
          
        </div>
        <div class="clear">
          <xsl:value-of select="''"/>
        </div>
      </div>
    </div>
    <div style="position:absolute;top:670px;left:0px;width:482px;text-align:right;">
			<xsl:if test="$accesslevel='1'">
				<asp:LinkButton ID="lnkdownload_" runat="server" CssClass="btnlink" style="font-family:Arial;font-weight:bold;font-size:14px;">Download Abstract CV</asp:LinkButton>
			</xsl:if>	
			<xsl:if test="$accesslevel='1'">
        <asp:LinkButton ID="lnkgoto_" runat="server" CssClass="btnlink" style="font-family:Arial;font-weight:bold;font-size:14px;">Gestione Partecipanti</asp:LinkButton>
      </xsl:if>
      <xsl:value-of select="''"/>
    </div>
  </xsl:template>

  <xsl:template name="elencoitem">
    <xsl:param name="prefix" />
    <table class="itemstable">
      <xsl:apply-templates select="item">
        <xsl:with-param name="prefix" select="$prefix" />
      </xsl:apply-templates>
    </table>
    
  </xsl:template>

  <xsl:template match="item">
    <xsl:param name="prefix" />
    <tr>
      <td align="left">
        <asp:Image runat="server" CssClass="tooltip"
                      ImageUrl="~/img/icoInfoSmall.gif">
          <xsl:attribute name="ToolTip">
            <xsl:value-of select="@expl"/>
          </xsl:attribute>
        </asp:Image>
        <xsl:value-of select="@desc"/>
      </td>
      <td align="right">
        <xsl:choose>
          <xsl:when test="@num=0">
            <xsl:value-of select="'-'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="$accesslevel='1'">
                <b>
                  <asp:LinkButton runat="server" CssClass="classicA">
                    <xsl:attribute name="ID">
                      <xsl:value-of select="'lnkgoto_'"/>
                      <xsl:value-of select="$prefix"/>
                      <xsl:value-of select="'_'"/>
                      <xsl:value-of select="@cod"/>
                    </xsl:attribute>
                    <xsl:value-of select="@num"/>
                  </asp:LinkButton>
                </b>
              </xsl:when>
              <xsl:otherwise>
                <b>
                  <xsl:value-of select="@num"/>
                </b>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>