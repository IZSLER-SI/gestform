<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <xsl:apply-templates select="root" />
  </xsl:template>

  <xsl:template match="root">
    <table class="stl_sft" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td class="title">Filtra/Ordina Elenco</td>
      </tr>
      <tr>
        <td class="controls" style="padding:6px;">
          <div class="searchLabelCtl" style="width:60px;">Cognome</div>
          <asp:TextBox ID="txtCognome" runat="server" EnableViewState="false" CssClass="txt" MaxLength="35" width="174px" />
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <div class="searchLabelCtl" style="width:60px;">Nome</div>
          <asp:TextBox ID="txtNome" runat="server" EnableViewState="false" CssClass="txt" MaxLength="30" width="174px" />
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <div>
            <br/>
          </div>
          <div class="searchLabel">
            Tipologia
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkDEAll" runat="server" CssClass="classicA_nu">Tutte</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkDENone" runat="server" CssClass="classicA_nu">Nessuna</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblDipendenteEsterno" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="categoriepersone/item" />
          </asp:CheckBoxList>
          <div class="searchLabel">
            Origine Iscrizione
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkOIAll" runat="server" CssClass="classicA_nu">Tutte</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkOINone" runat="server" CssClass="classicA_nu">Nessuna</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblOrigine" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="originiiscrizione/item" />
          </asp:CheckBoxList>
          <div class="searchLabel">
            Categoria
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkCEAll" runat="server" CssClass="classicA_nu">Tutte</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkCENone" runat="server" CssClass="classicA_nu">Nessuna</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblCategoriaEcm" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="categorieecm/item" />
          </asp:CheckBoxList>
          <div class="searchLabel">
            Stato Iscrizione
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkSIAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkSINone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblStatoIscrizione" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="statiiscrizione/item" />
          </asp:CheckBoxList>
          <div class="searchLabel">
            Presenza minima per ECM
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkSPAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkSPNone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblStatoPresenza" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="statipresenza/item" />
          </asp:CheckBoxList>
          
          <div class="searchLabel">
            Stato Questionario
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkSQAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkSQNone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblStatoQuestionario" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="statiquestionario/item" />
          </asp:CheckBoxList>
          <div class="searchLabel">
            Stato ECM
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkSEAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkSENone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblStatoEcm" EnableViewState="false" runat="server" CssClass="searchCbl" RepeatLayout="Flow">
            <xsl:apply-templates select="statiecm/item" />
          </asp:CheckBoxList>
          <div>
            <br/>
          </div>
          <div class="searchLabelCtl" style="width:80px;">
            Ordina per
          </div>
          <asp:DropDownList runat="server" ID="ddnOrder" EnableViewState="false" CssClass="ddn" Width="158px">
            <asp:ListItem Selected="true" Text="Cognome e nome" Value="CN" />
            <asp:ListItem Text="Data/Ora Iscrizione" Value="DI" />
            <asp:ListItem Text="Origine Iscrizione" Value="OI" />
            <asp:ListItem Text="Categoria" Value="CE" />
            <asp:ListItem Text="Stato Iscrizione" Value="SI" />
            <asp:ListItem Text="Stato ECM" Value="SE" />
          </asp:DropDownList>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>

        </td>
      </tr>
      <tr>
        <td class="commands">
          <asp:Button ID="btnClear" runat="server" Text="Pulisci" CssClass="btnNewClear" />
          <asp:Button ID="btnFilter" runat="server" Text="Filtra/Ordina" CssClass="btnSearch" />
        </td>
      </tr>
    </table>
    <!-- controlli nascosti -->
    <asp:HiddenField runat="server" ID="hidCognome" />
    <asp:HiddenField runat="server" ID="hidNome" />
    <asp:HiddenField runat="server" ID="hidDipendenteEsterno" />
    <asp:HiddenField runat="server" ID="hidOrigine" />
    <asp:HiddenField runat="server" ID="hidCategoriaEcm" />
    <asp:HiddenField runat="server" ID="hidStatoIscrizione" />
    <asp:HiddenField runat="server" ID="hidStatoEcm" />
    <asp:HiddenField runat="server" ID="hidStatoQuestionario" />
    <asp:HiddenField runat="server" ID="hidStatoPresenza" />
    <asp:HiddenField runat="server" ID="hidOrder" />
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