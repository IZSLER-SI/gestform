<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove"  xmlns:ajaxToolkit="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="accesslevel" />
  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>
  <xsl:template match="evento">

    <style type="text/css">
      .numtable
      {
      font-size:11px;
      width:100%;
      }
      .numtable td
      {
      border-bottom:solid 1px #c0c0c0;
      empty-cells:show;
      }
      .numtable .int
      {
      padding:1px 2px 1px 2px;
      text-align:left;
      }
      .numtable .num
      {
      text-align:right;
      font-weight:bold;
      padding:1px 5px 1px 5px;
      background-color:#f0f0f0;
      min-width:30px;
      }
      .numtable .not
      {
      font-style:oblique;
      padding:1px 2px 1px 2px;
      }
    </style>

    <div class="stl_gen_box" style="position: absolute; width: 970px; top: 0px; left: 0px;">
      <div class="content padall" style="height:268px;">
        <div style="font-size:15px;line-height:17px;padding-bottom:3px; margin-bottom:3px; border-bottom:dotted 1px #666666;">
          <strong style="color:#ff6600;">
            <xsl:value-of select="@tx_titolo"/>
          </strong>
          <xsl:if test="@tx_edizione!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_edizione"/>
          </xsl:if>
          <br/>
          <strong>
          <xsl:call-template name="dataDalAl_it">
            <xsl:with-param name="dataDal" select="@dt_inizio" />
            <xsl:with-param name="dataAl" select="@dt_fine" />
          </xsl:call-template>
          </strong>
          <xsl:choose>
            <xsl:when test="@fl_fad=0">
              <xsl:value-of select="', '"/>
              <xsl:value-of select="@tx_sede"/>
              <xsl:value-of select="', '"/>
              <xsl:value-of select="@tx_indirizzosede"/>
            </xsl:when>
            <xsl:otherwise>
              - FAD
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="count(@ac_tipocobdett)&gt;0">
            <div style="font-size:11px;color:#336699;">
              <em>
                Corso Obbligatorio - Tipologia:
                <b>
                  <xsl:value-of select="@tx_tipocobbase"/>
                  <xsl:value-of select="' - '"/>
                  <xsl:value-of select="@tx_tipocobdett"/>
                </b>
              </em>
            </div>
          </xsl:if>          
        </div>
        <div style="line-height:14px;">
          <div style="display:inline-block;vertical-align:top;width:400px;padding-right:5px;border-right:1px dotted #666666;min-height:210px;">
            <xsl:if test="count(@ac_evento)=1">
              <div>
                Codice Interno: <b>
                  <xsl:value-of select="@ac_evento"/>
                </b>
              </div>
            </xsl:if>
            <div>
              ID: <b>
                <span id="spanIdEvento"><xsl:value-of select="@id_evento" /></span>
              </b>
            </div>
						<div>
								NOME: <b>
										<span id="spanTitoloEvento">
												<xsl:value-of select="@tx_titolo" />
										</span>
								</b>
						</div>
            <div>
              Tipologia: <b>
                <xsl:value-of select="@tx_tipologiaevento" />
              </b>
            </div>
            <div>
              Organizzatore: <b>
                <xsl:value-of select="@tx_organizzatore" />
              </b>
            </div>
            <div>
              Responsabile Scientifico: <b>
                <xsl:choose>
                  <xsl:when test="count(@tx_cognomtit_rs)&gt;0">
                    <xsl:value-of select="@tx_cognomtit_rs" />
                  </xsl:when>
                  <xsl:otherwise>
                    Non inserito
                  </xsl:otherwise>
                </xsl:choose>
                
              </b>
            </div>
            <div>
              Segreteria Organizzativa: <b>
                <xsl:value-of select="@tx_segreteriaorganizzativa" />
              </b>
            </div>
            <div>
              Centro di Referenza: <b>
                <xsl:value-of select="@tx_centroreferenza" />
              </b>
            </div>
            <div>
              Centro di Costo: <b>
                <xsl:value-of select="@tx_cdc" />
              </b>
            </div>
            <div>
              Piano Formativo: <b>
                <xsl:value-of select="@tx_pianoformativo" />
                <xsl:choose>
                  <xsl:when test="@fl_nuovoinpf=0">
                    (presente originariamente nel PF)
                  </xsl:when>
                  <xsl:otherwise>
                    (aggiunto in seguito)
                  </xsl:otherwise>
                </xsl:choose>
              </b>
            </div>
            <div>
              Numero massimo partecipanti: <b>
                <xsl:choose>
                  <xsl:when test="@iol_ni_maxpartecipanti!=''">
                    <xsl:value-of select="@iol_ni_maxpartecipanti"/>
                  </xsl:when>
                  <xsl:otherwise>
                    Illimitato
                  </xsl:otherwise>
                </xsl:choose>
              </b>
            </div>
            <div>
              Inizio visibilità su portale pubbico: <b>
                <xsl:choose>
                  <xsl:when test="@iol_dt_iniziovisibilita!=''">
                    <xsl:call-template name="data_it">
                      <xsl:with-param name="data" select="@iol_dt_iniziovisibilita" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    -
                  </xsl:otherwise>
                </xsl:choose>
              </b>
            </div>
            <div>
              Date apertura iscrizioni su portale pubbico: <b>
                <xsl:choose>
                  <xsl:when test="@iol_dt_aperturaiscrizioni!=''">
                    <xsl:call-template name="dataDalAl_it">
                      <xsl:with-param name="dataDal" select="@iol_dt_aperturaiscrizioni" />
                      <xsl:with-param name="dataAl" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    -
                  </xsl:otherwise>
                </xsl:choose>
              </b>
            </div>
            <div>
              Valutazione Web:
              <xsl:choose>
                <xsl:when test="@vweb_dt_inizio!=''">
                  Attiva dal
                  <b>
                    <xsl:call-template name="dataoraDDMMYYHHMM">
                      <xsl:with-param name="dataora" select="@vweb_dt_inizio" />
                    </xsl:call-template>
                  </b>
                  al
                  <b>
                    <xsl:call-template name="dataoraDDMMYYHHMM">
                      <xsl:with-param name="dataora" select="@vweb_dt_fine" />
                    </xsl:call-template>
                  </b>
                  - Apprendimento:
                  <b>
                    <xsl:choose>
                      <xsl:when test="@vweb_fl_apprendimento='1'">
                        Attivo
                      </xsl:when>
                      <xsl:otherwise>
                        Non attivo
                      </xsl:otherwise>
                    </xsl:choose>
                  </b>
                  - Qualità:
                  <b>
                    <xsl:choose>
                      <xsl:when test="@vweb_fl_qualita='1'">
                        Attiva
                      </xsl:when>
                      <xsl:otherwise>
                        Non attiva
                      </xsl:otherwise>
                    </xsl:choose>
                  </b>
                </xsl:when>
                <xsl:otherwise>
                  <b>Non attivata</b>
                </xsl:otherwise>
              </xsl:choose>
            </div>

          </div>
          <div style="display:inline-block;vertical-align:top;width:510px;padding-left:8px;padding-right:5px;overflow:auto;">
            <div>
              ECM: <b>
                <xsl:value-of select="@tx_normativaecm"/>
              </b>
              - N° Crediti:
              <b>
                <xsl:choose>
                  <xsl:when test="count(@ecm2_num_cred)&gt;0">
                    <xsl:call-template name="number_xdec">
                      <xsl:with-param name="number" select="@ecm2_num_cred" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    Non inserito
                  </xsl:otherwise>
                </xsl:choose>
              </b>
            </div>
            <xsl:if test="@ac_normativaecm!='NONE'">
              <div>
                Codice evento: <b>
                  <xsl:value-of select="@ecm2_cod_eve"/>
                </b>
                - Codice edizione: <b>
                  <xsl:value-of select="@ecm2_cod_edi"/>
                </b>
              </div>
              <div>
                Tipo Evento: <b>
                  <xsl:value-of select="@ecm2_tipo_evento"/>
                </b>
              </div>
              <div>
                Tipo Formazione: <b>
                  <xsl:value-of select="@tx_tipologiaecmevento"/>
                </b>
              </div>
              <div>
                Provider: <b>
                  <xsl:value-of select="@tx_providerecm"/>
                </b>
              </div>
              <div>
                Professioni/Discipline abilitate:
                <ul style="font-weight:bold;margin:0px 0px 0px 0px;">
                  <xsl:for-each select="professionedisciplina">
                    <li>
                      <xsl:value-of select="@tx_professionedisciplina" />
                    </li>
                  </xsl:for-each>
                  <xsl:value-of select="''"/>
                </ul>
              </div>
            </xsl:if>
          </div>
        </div>
      </div>
      <div class="commands">
        <xsl:if test="$accesslevel='1'">
          <asp:LinkButton ID="lnkEliminaEvento" runat="server" CssClass="command" Font-Bold="False">Elimina Evento</asp:LinkButton>
          <ajaxToolkit:ConfirmButtonExtender ID="enfEliminaEvento" runat="server" TargetControlID="lnkEliminaEvento" ConfirmText="Confermi l'eliminazione dell'evento e di tutti i dati ad esso associati? ATTENZIONE: l'operazione è irreversibile!" />
          <a href="DuplicaEvento.aspx" class="command" style="font-weight:normal;">Duplica Evento</a>
          <a href="ModificaDatiEvento.aspx" class="command">Modifica Dati</a>
        </xsl:if>
        <xsl:value-of select="''"/>
      </div>
    </div>

    <xsl:for-each select="statoliste">
      <div class="stl_gen_box" style="position: absolute; width: 240px; top: 0px; left: 985px;">
        <div class="content padall">
          <table class="numtable">
            <xsl:choose>
              <xsl:when test="count(@iol_ni_maxpartecipanti)&gt;0">
                <xsl:call-template name="maxiscritti_iscritti_residui" />
                <tr>
                  <td class="int">Lista Attesa Prioritaria</td>
                  <td class="num">
                    <xsl:value-of select="@ni_listaattesaprioritaria"/>
                  </td>
                  <td class="not">
                    <xsl:call-template name="nbsp" />
                  </td>
                </tr>
                <tr>
                  <td class="int">Lista Attesa Secondaria</td>
                  <td class="num">
                    <xsl:value-of select="@ni_listaattesasecondaria"/>
                  </td>
                  <td class="not">
                    <xsl:call-template name="nbsp" />
                  </td>
                </tr>
              </xsl:when>
              <xsl:otherwise>
                <tr>
                  <td class="int">Numero Iscritti</td>
                  <td class="num">
                    <xsl:value-of select="@ni_iscritti"/>
                  </td>
                  <td class="not">
                    <xsl:call-template name="nbsp" />
                  </td>
                </tr>
              </xsl:otherwise>
            </xsl:choose>
          </table>
        </div>
      </div>
    </xsl:for-each>
    
  </xsl:template>

  <xsl:template name="maxiscritti_iscritti_residui">
    <tr>
      <td class="int">Numero Max Partecipanti</td>
      <td class="num">
        <xsl:value-of select="@iol_ni_maxpartecipanti"/>
      </td>
      <td class="not">
        <xsl:call-template name="nbsp" />
      </td>
    </tr>
    <tr>
      <td class="int">Numero Iscritti</td>
      <td class="num">
        <xsl:value-of select="@ni_iscritti"/>
      </td>
      <td class="not">
        <xsl:call-template name="nbsp" />
      </td>
    </tr>
    <tr>
      <td class="int">Posti disponibili</td>
      <td class="num">
        <xsl:choose>
          <xsl:when test="@ni_rimanenti&gt;0">
            <span style="color:#009900">
              <xsl:value-of select="@ni_rimanenti"/>
            </span>
          </xsl:when>
          <xsl:when test="@ni_rimanenti=0">
            <span style="color:#ff6600">
              <xsl:value-of select="@ni_rimanenti"/>
            </span>
          </xsl:when>
          <xsl:otherwise>
            <span style="color:#ff0000">
              <xsl:value-of select="'0'"/>
            </span>
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td class="not">
        <xsl:choose>
          <xsl:when test="@ni_rimanenti=-1">
            <span style="color:#ff0000">
              <b>1</b> esubero
            </span>
          </xsl:when>
          <xsl:when test="@ni_rimanenti&lt;-1">
            <span style="color:#ff0000">
              <b>
                <xsl:value-of select="-@ni_rimanenti"/>
              </b>
              esuberi
            </span>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="nbsp" />
          </xsl:otherwise>
        </xsl:choose>

      </td>
    </tr>
  </xsl:template>


</xsl:stylesheet>