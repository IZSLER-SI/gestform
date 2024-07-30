<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="companyname" />
  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <div class="stl_gen_box" style="position:absolute;top:15px;left:15px;width:623px;">
      <div class="title">
        Rapporto lavorativo con <xsl:value-of select="$companyname"/>
      </div>
      <div class="content padall" style="height:550px;">
        <xsl:call-template name="rapportolavorativo" />
      </div>
    </div>

    <div class="stl_gen_box" style="position:absolute;top:15px;left:655px;width:430px;">
      <div class="title">
        Profilo
      </div>
      <div class="content padall" style="height:550px;">
        <xsl:call-template name="profilo" />
      </div>
    </div>

    <!-- prof/disc: solo se è ECM -->
    <xsl:if test="@fl_ecm='1'">
      <div class="stl_gen_box" style="position:absolute;top:615px;left:15px;width:623px;">
        <div class="title">
          Professioni/Discipline ECM
        </div>
        <div class="content padall" style="height:150px;">
          <xsl:call-template name="professionidisciplineecm" />
        </div>
      </div>
    </xsl:if>

    <!-- destinazione: solo se c'è numero chiuso -->
    <xsl:if test="@iol_ni_maxpartecipanti!=''">
      <div class="stl_gen_box" style="position:absolute;top:615px;left:655px;width:430px;">
        <div class="title">
          Destinazione Iscrizione
        </div>
        <div class="content padall" style="height:115px;">
          <xsl:call-template name="destinazione" />
        </div>
      </div>
      <th>Destinazione</th>
    </xsl:if>
    
  </xsl:template>

  <xsl:template name="rapportolavorativo">
    <asp:RadioButtonList ID="rblac_DIPEXT" runat="server" EnableViewState="false" ClientIdMode="Static">
      <asp:ListItem Text="Dipendente o Esterno" Value="ALL">
        <xsl:if test="criterio/@ac_dipext='ALL' or count(criterio)=0">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Esterno" Value="EXT">
        <xsl:if test="criterio/@ac_dipext='EXT'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Dipendente appartenente a qualsiasi unità operativa" Value="DIP">
        <xsl:if test="criterio/@ac_dipext='DIP'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Dipendente appartenente ad una o più unità operative specifiche" Value="DIP_UO">
        <xsl:if test="criterio/@ac_dipext='DIP_UO'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
    </asp:RadioButtonList>
    <div id="uocontainer">
      <xsl:attribute name="style">
        <xsl:choose>
          <xsl:when test="criterio/@ac_dipext='DIP_UO'">
            <xsl:value-of select="'display:block;'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'display:none;'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <asp:CheckBoxList ID="cblac_UNITAOPERATIVA" runat="server" EnableViewState="false">
        <xsl:for-each select="uo">
          <asp:ListItem>
            <xsl:attribute name="Text">
              <xsl:value-of select="@tx_unitaoperativa"/>
            </xsl:attribute>
            <xsl:attribute name="Value">
              <xsl:value-of select="@ac_unitaoperativa"/>
            </xsl:attribute>
            <xsl:if test="@fl_selezionata='1'">
              <xsl:attribute name="Selected">
                <xsl:value-of select="'true'"/>
              </xsl:attribute>
            </xsl:if>
          </asp:ListItem>
        </xsl:for-each>
      </asp:CheckBoxList>
    </div>
  </xsl:template>

  <xsl:template name="profilo">
    <asp:RadioButtonList ID="rblfl_PROFILI" runat="server" EnableViewState="false" ClientIdMode="Static">
      <asp:ListItem Text="Qualsiasi profilo" Value="0">
        <xsl:if test="criterio/@fl_profili='0' or count(criterio)=0">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Uno o più profili specifici" Value="1">
        <xsl:if test="criterio/@fl_profili='1'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
    </asp:RadioButtonList>
    <div id="profilocontainer">
      <xsl:attribute name="style">
        <xsl:choose>
          <xsl:when test="criterio/@fl_profili='1'">
            <xsl:value-of select="'display:block;'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'display:none;'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <asp:CheckBoxList ID="cblac_PROFILO" runat="server" EnableViewState="false" ClientIdMode="Static">
        <xsl:for-each select="profilo">
          <asp:ListItem>
            <xsl:attribute name="Text">
              <xsl:value-of select="@tx_profilo"/>
            </xsl:attribute>
            <xsl:attribute name="Value">
              <xsl:value-of select="@ac_profilo"/>
            </xsl:attribute>
            <xsl:if test="@fl_selezionato='1'">
              <xsl:attribute name="Selected">
                <xsl:value-of select="'true'"/>
              </xsl:attribute>
            </xsl:if>
          </asp:ListItem>
        </xsl:for-each>
      </asp:CheckBoxList>
    </div>
  </xsl:template>

  <xsl:template name="professionidisciplineecm">
    <asp:RadioButtonList ID="rblfl_PRODISCECMACCR" runat="server" EnableViewState="false">
      <asp:ListItem Text="Qualsiasi" Value="">
        <xsl:if test="count(criterio/@fl_prodiscecmaccr)=0 or count(criterio)=0">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Professione/discplina non presente o non accreditata per l'evento" Value="0">
        <xsl:if test="criterio/@fl_prodiscecmaccr='0'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Solo le professioni/discipline per le quali l'evento è accreditato:" Value="1">
        <xsl:if test="criterio/@fl_prodiscecmaccr='1'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
    </asp:RadioButtonList>
    <ul style="margin:0px;">
      <xsl:for-each select="professionedisciplina">
        <li>
          <xsl:value-of select="@tx_professionedisciplina"/>
        </li>
      </xsl:for-each>
    </ul>
  </xsl:template>

  <xsl:template name="destinazione">
    <asp:RadioButtonList ID="rblac_DESTINAZIONE" runat="server" EnableViewState="false">
      <asp:ListItem Text="Accettato immediatamente o in lista d'attesa prioritaria/unica" Value="ACC_Q1">
        <xsl:if test="criterio/@ac_destinazione='ACC_Q1' or count(criterio)=0">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
      <asp:ListItem Text="Lista d'attesa secondaria" Value="Q2">
        <xsl:if test="criterio/@ac_destinazione='Q2'">
          <xsl:attribute name="Selected">
            <xsl:value-of select="'true'"/>
          </xsl:attribute>
        </xsl:if>
      </asp:ListItem>
    </asp:RadioButtonList>
  </xsl:template>
</xsl:stylesheet>