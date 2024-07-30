<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <div class="title green">
      <xsl:value-of select="@tx_titoloevento"/>
      <xsl:if test="@tx_edizione!=''">
        <xsl:value-of select="' - '"/>
        <xsl:value-of select="@tx_edizione"/>
      </xsl:if>
    </div>
    <div style="padding-top:5px;padding-bottom:20px;">
      <b>
        <xsl:value-of select="@tx_sede"/>
        <xsl:value-of select="', '"/>
        <xsl:call-template name="dataDalAl_it_mmmm">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </b>
    </div>
    <div class="infodiv">
      <div>
        La registrazione delle date di svolgimento, degli orari e delle presenze deve essere completata entro il
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@dt_fineinserimento" />
          </xsl:call-template>
        </b>.
      </div>
      <xsl:if test="count(altrapersona)&gt;0">
        <div>
          <xsl:choose>
            <xsl:when test="count(altrapersona)=1">
              Oltre a te, è abilitato all'inserimento dei dati anche
            </xsl:when>
            <xsl:when test="count(altrapersona)&gt;1">
              Oltre a te, sono abilitati all'inserimento dei dati anche
            </xsl:when>
          </xsl:choose>
          <b>
            <xsl:for-each select="altrapersona">
              <xsl:value-of select="@tx_nome"/>
              <xsl:value-of select="' '"/>
              <xsl:value-of select="@tx_cognome"/>
              <xsl:if test="position()!=last()">
                <xsl:value-of select="', '"/>
              </xsl:if>
            </xsl:for-each>
          </b>.
        </div>
      </xsl:if>
      <div>
        <xsl:choose>
          <xsl:when test="count(data)&gt;0">
            Ad oggi risultano già inseriti gli orari di svolgimento e le presenze per le seguenti date:
            <ul>
              <xsl:for-each select="data">
                <li>
                  <b>
                    <xsl:call-template name="data_it_mmmm">
                      <xsl:with-param name="data" select="@dt_data" />
                    </xsl:call-template>
                  </b>
                  <xsl:value-of select="' (orari: '"/>
                  <xsl:for-each select="range">
                    <xsl:call-template name="oraHHMM_sqltime">
                      <xsl:with-param name="ora" select="@tm_inizio" />
                    </xsl:call-template>
                    <xsl:value-of select="'-'"/>
                    <xsl:call-template name="oraHHMM_sqltime">
                      <xsl:with-param name="ora" select="@tm_fine" />
                    </xsl:call-template>
                    <xsl:if test="position()!=last()">
                      <xsl:value-of select="', '"/>
                    </xsl:if>
                  </xsl:for-each>

                  <xsl:value-of select="')'"/>
                </li>
              </xsl:for-each>
            </ul>
          </xsl:when>
          <xsl:otherwise>
            Ad oggi non risulta inserito nessun dato.
          </xsl:otherwise>
        </xsl:choose>
      </div>
    </div>

    <div class="datagroup" style="margin-top:30px;">
      <div class="row">
        <div class="label" style="width:240px;padding-bottom:20px;padding-right:20px;">
          <b>Data di svolgimento</b>
        </div>
        <div class="data" style="width:420px;padding-bottom:20px;">
          <asp:DropDownList ID="ddnData" runat="server" EnableViewState="false" CssClass="ddn ddnnarrow">
            <asp:ListItem Text="" value="" />
            <xsl:for-each select="dataevento">
              <asp:ListItem>
                <xsl:attribute name="Text">
                  <xsl:call-template name="data_it_mmmm">
                    <xsl:with-param name="data" select="@dt_data" />
                  </xsl:call-template>
                </xsl:attribute>
                <xsl:attribute name="Value">
                  <xsl:call-template name="dataYYYYMMDD">
                    <xsl:with-param name="data" select="@dt_data" />
                  </xsl:call-template>
                </xsl:attribute>
              </asp:ListItem>
            </xsl:for-each>
          </asp:DropDownList>
        </div>
        <div class="error" style="width:220px;padding-bottom:20px;">
          <asp:Label ID="errData" runat="server" EnableViewState="false" />
        </div>
      </div>
      <div class="row">
        <div class="label" style="width:240px;padding-bottom:20px;padding-right:20px;">
          <b>Orari di svolgimento</b>
          <div style="font-size:12px;">
            Inserisci gli orari escludendo eventuali pause. Ad esempio, 
            se l'attività è iniziata alle 14.00 ed è terminata alle 18.30 con una pausa tra
            le 16.00 e le 16.30, inserisci nella prima riga <em>14.00 - 16.00</em>
            e nella seconda riga <em>16.30 - 18.30</em>
          </div>
        </div>
        <div class="data" style="padding-bottom:20px;">
          <xsl:call-template name="range">
            <xsl:with-param name="no" select="1" />
          </xsl:call-template>
          <xsl:call-template name="range">
            <xsl:with-param name="no" select="2" />
          </xsl:call-template>
          <xsl:call-template name="range">
            <xsl:with-param name="no" select="3" />
          </xsl:call-template>
          <xsl:call-template name="range">
            <xsl:with-param name="no" select="4" />
          </xsl:call-template>
          <xsl:call-template name="range">
            <xsl:with-param name="no" select="5" />
          </xsl:call-template>
        </div>
        <div class="error" style="padding-bottom:20px;">
          <asp:Label ID="errOrari" runat="server" EnableViewState="false" />
        </div>
      </div>
      <div class="row">
        <div class="label" style="padding-bottom:20px;">
          <div style="padding-bottom:3px;">
            <b>Docenti/Moderatori/Tutor presenti</b>
          </div>
          <span class="btnlink btnlink_green" onclick="cblSel('cblDRMT',true);">Tutti</span>
          <xsl:value-of select="' '"/>
          <span class="btnlink btnlink_green" onclick="cblSel('cblDRMT',false);">Nessuno</span>
        </div>
        <div class="data" style="padding-bottom:20px;">
          <asp:CheckBoxList ID="cblDRMT" runat="server" ClientIDMode="Static">
            <xsl:for-each select="drmt">
              <xsl:call-template name="cbp">
                <xsl:with-param name="ruolo" select="'1'" />
              </xsl:call-template>
            </xsl:for-each>
          </asp:CheckBoxList>
        </div>
        <div class="error" style="padding-bottom:20px;">
          <asp:Label ID="errDRMT" runat="server" EnableViewState="false" />
        </div>
      </div>
      <div class="row">
        <div class="label" style="padding-bottom:20px;">
          <div style="padding-bottom:3px;">
            <b>Partecipanti presenti</b>
          </div>
          <span class="btnlink btnlink_green" onclick="cblSel('cblP',true);">Tutti</span>
          <xsl:value-of select="' '"/>
          <span class="btnlink btnlink_green" onclick="cblSel('cblP',false);">Nessuno</span>
        </div>
        <div class="data" style="padding-bottom:20px;">
          <asp:CheckBoxList ID="cblP" runat="server" ClientIDMode="Static">
            <xsl:for-each select="p">
              <xsl:call-template name="cbp">
                <xsl:with-param name="ruolo" select="'0'" />
              </xsl:call-template>
            </xsl:for-each>
          </asp:CheckBoxList>
        </div>
        <div class="error" style="padding-bottom:20px;">
          <asp:Label ID="errP" runat="server" EnableViewState="false" />
        </div>
      </div>
    </div>

    <div class="top10">
      <asp:LinkButton runat="server" CssClass="btnlink btnlink_blue" ID="lnkSave" Font-Bold="true" onClientClick="return(confirmSave());">Salva Dati</asp:LinkButton>
      <xsl:call-template name="nbsp" />
      <span class="btnlink btnlink_blue" onclick="confirmCancel();">Annulla</span>
      <xsl:call-template name="nbsp" />
      <xsl:call-template name="nbsp" />
      <xsl:call-template name="nbsp" />
      <asp:Label ID="errGlobal" runat="server" ForeColor="#ff0000" Font-Bold="True" EnableViewState="false"></asp:Label>
    </div>
    
  </xsl:template>

  <xsl:template name="range">
    <xsl:param name="no" />
    <div>
      Dalle ore
      <asp:TextBox runat="server" EnableViewState="false" CssClass="txt stl_dt_ora_hhmm" width="50px">
        <xsl:attribute name="ID">
          <xsl:value-of select="'txtDalle_'"/>
          <xsl:value-of select="$no"/>
        </xsl:attribute>
      </asp:TextBox>
      alle ore
      <asp:TextBox runat="server" EnableViewState="false" CssClass="txt stl_dt_ora_hhmm" width="50px">
      <xsl:attribute name="ID">
        <xsl:value-of select="'txtAlle_'"/>
        <xsl:value-of select="$no"/>
      </xsl:attribute>
    </asp:TextBox>
    </div>
  </xsl:template>

  <xsl:template name="cbp">
    <xsl:param name="ruolo" />
    <asp:ListItem>
      <xsl:attribute name="Value">
        <xsl:value-of select="@id_iscritto"/>
      </xsl:attribute>
      <xsl:attribute name="Text">
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="', '"/>
        <xsl:value-of select="@tx_nome"/>
        <xsl:if test="count(@ac_matricola)&gt;0">
          <xsl:value-of select="' ('"/>
          <xsl:value-of select="@ac_matricola"/>
          <xsl:value-of select="')'"/>
        </xsl:if>
        <xsl:if test="$ruolo='1'">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_categoriaecm"/>
        </xsl:if>
      </xsl:attribute>
    </asp:ListItem>
  </xsl:template>

</xsl:stylesheet>
