<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <div style="padding:5px 15px 5px 15px;">
      <div class="stl_dfo">
        <div class="title">
          Badge
        </div>
      </div>
      <xsl:apply-templates select="root" />
    </div>
  </xsl:template>

  <xsl:template match="root">
    <div class="stl_gen_box" style="width:923px;">
      <div class="title">
        Eventuali filtri
      </div>
      <div class="content" style="padding:6px 6px 10px 6px">
        <div class="coldiv" style="width:200px;margin-right:20px;">
          <div class="searchLabel">
            Categoria
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkBACEAll" runat="server" CssClass="classicA_nu">Tutte</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkBACENone" runat="server" CssClass="classicA_nu">Nessuna</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblBACategoriaEcm" EnableViewState="false" runat="server" CssClass="searchCbl" CellPadding="0" CellSpacing="0">
            <xsl:apply-templates select="categorieecm/item" />
          </asp:CheckBoxList>
        </div>
        <div class="coldiv" style="width:200px;margin-right:20px;">
          <div class="searchLabel">
            Stato Iscrizione
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkBASIAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkBASINone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblBAStatoIscrizione" EnableViewState="false" runat="server" CssClass="searchCbl" CellPadding="0" CellSpacing="0">
            <xsl:apply-templates select="statiiscrizione/item[@cod!='P' and @cod!='A' and @cod!='NA' and @cod!='AI' and @cod!='AG']" />
          </asp:CheckBoxList>
        </div>
        <div class="coldiv" style="width:250px;margin-right:20px;">
          <div class="searchLabel">
            Stato ECM
          </div>
          <div class="searchCmds">
            <asp:LinkButton ID="lnkBASEAll" runat="server" CssClass="classicA_nu">Tutti</asp:LinkButton>
            <xsl:call-template name="nbsp" />
            <xsl:call-template name="nbsp" />
            <asp:LinkButton ID="lnkBASENone" runat="server" CssClass="classicA_nu">Nessuno</asp:LinkButton>
          </div>
          <div class="clear">
            <xsl:value-of select="''"/>
          </div>
          <asp:CheckBoxList ID="cblBAStatoEcm" EnableViewState="false" runat="server" CssClass="searchCbl" CellPadding="0" CellSpacing="0">
            <xsl:apply-templates select="statiecm/item[@cod!='COK' and @cod!='CKO']" />
          </asp:CheckBoxList>
        </div>
        <div class="clear">
          <xsl:value-of select="''"/>
        </div>
      </div>
      <div class="commands">
        <asp:Button ID="btnBAClear" runat="server" Text="Pulisci" CssClass="command_nobold" />
        <asp:Button ID="btnBADoReport" runat="server" Text="Genera Badge (senza barcode)" CssClass="command" Width="232px" />
        <asp:Button ID="btnBADoReportBC" runat="server" Text="Genera Badge (con barcode)" CssClass="command" Width="232px" />
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