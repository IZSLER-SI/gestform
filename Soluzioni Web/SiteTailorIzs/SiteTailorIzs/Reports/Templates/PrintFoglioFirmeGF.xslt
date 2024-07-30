<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <div style="padding:5px 15px 5px 15px;">
      <div class="stl_dfo">
        <div class="title">
          Fogli Firme
        </div>
      </div>
      <xsl:apply-templates select="root" />
    </div>
  </xsl:template>

  <xsl:template match="root">

    <div class="stl_gen_box" style="width:923px;">
      <div class="title">
        Eventuali filtri e parametri
      </div>
      <div class="content" style="padding:6px 6px 10px 6px">
        <div class="coldiv" style="width:200px;margin-right:20px;">
          <div class="searchLabel">
            Categoria
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkFFCEAll" runat="server" CssClass="classicA_nu">Tutte</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkFFCENone" runat="server" CssClass="classicA_nu">Nessuna</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblFFCategoriaEcm" EnableViewState="false" runat="server" CssClass="searchCbl" CellPadding="0" CellSpacing="0">
            <xsl:apply-templates select="categorieecm/item" />
          </asp:CheckBoxList>
        </div>
        <div class="coldiv" style="width:200px;margin-right:20px;">
          <div class="searchLabel">
            Stato Iscrizione
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkFFSIAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkFFSINone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblFFStatoIscrizione" EnableViewState="false" runat="server" CssClass="searchCbl" CellPadding="0" CellSpacing="0">
            <xsl:apply-templates select="statiiscrizione/item[@cod!='P' and @cod!='A' and @cod!='NA' and @cod!='AI' and @cod!='AG']" />
          </asp:CheckBoxList>
        </div>
        <div class="coldiv" style="width:250px;margin-right:20px;">
          <div class="searchLabel">
            Stato ECM
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkFFSEAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkFFSENone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblFFStatoEcm" EnableViewState="false" runat="server" CssClass="searchCbl" CellPadding="0" CellSpacing="0">
            <xsl:apply-templates select="statiecm/item[@cod!='COK' and @cod!='CKO']" />
          </asp:CheckBoxList>
        </div>
        <div class="clear">
          <xsl:value-of select="''"/>
        </div>
        <div>
          <br/>
        </div>
        <div style="padding-bottom:3px;">
          <b>
            Eventuale testo da inserire nell'intestazione (es: data, mattino o pomeriggio...)<br/>
          </b>
        </div>
        <div>
          <asp:TextBox runat="server" MaxLength="200" ID="txtFFIntestazione" Width="645px" CssClass="txt" />
        </div>
        <div style="padding-bottom:3px;padding-top:3px;">
          <b>
            Numero di righe bianche da inserire<br/>
          </b>
        </div>
        <div>
          <asp:DropDownList runat="server" ID="ddnFFRigheBianche" CssClass="ddn">
            <asp:ListItem Text="0" Value="0" />
            <asp:ListItem Text="5" Value="5" />
            <asp:ListItem Text="10" Value="10" />
            <asp:ListItem Text="20" Value="20" />
            <asp:ListItem Text="30" Value="30" />
            <asp:ListItem Text="40" Value="40" />
            <asp:ListItem Text="50" Value="50" />
          </asp:DropDownList>
        </div>
      </div>
      <div class="commands">
        <asp:Button ID="btnFFClear" runat="server" Text="Pulisci" CssClass="command_nobold" />
        <asp:Button ID="btnFFDoReport" runat="server" Text="Genera Foglio Firme" CssClass="command" />
      </div>
    </div>
  </xsl:template>

  <xsl:template match="item">
    <asp:ListItem Selected="true">
      <xsl:attribute name="text">
        <xsl:value-of select="@desc"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="@cod"/>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:value-of select="'color:'"/>
        <xsl:value-of select="@rgb" />
      </xsl:attribute>
    </asp:ListItem>
  </xsl:template>
</xsl:stylesheet>