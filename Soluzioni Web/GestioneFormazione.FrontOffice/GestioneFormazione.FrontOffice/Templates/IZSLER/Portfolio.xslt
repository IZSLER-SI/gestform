<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="node" />
  <xsl:param name="region" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="count(eventi/evento)=0">
        <div>
          <b>
            Al momento non ci risulta che tu abbia partecipato ad alcun evento formativo.
          </b>
          <br/>
          Se hai partecipato ad un evento che è terminato da pochi giorni, ti preghiamo di provare a ricollegarti a breve.
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="bottom20">
          Nella lista sottostante sono visualizzati tutti gli eventi formativi ai quali hai partecipato, in ordine cronologico decrescente.
          <br/>
          Cliccando su "visualizza materiale" puoi accedere all'eventuale materiale didattico disponibile per l'evento.
        </div>
        <xsl:if test="count(eventi/@ac_matricola)&gt;0">
          <div class="bottom20">
            <strong>
              Per scaricare il tuo portfolio in formato Microsoft Excel <a class="classica" href="/portfolio-excel">clicca qui</a>.  
            </strong><br/>
            <em>Nota: il file include solo gli eventi la cui frequentazione è stata confermata/validata.</em>
          </div>
        </xsl:if>
        <xsl:apply-templates select="eventi" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="eventi">
    <table class="pf_table">
      <tr>
        <th>Data/date</th>
        <th>Tipo partecipazione</th>
        <th>Tipo, titolo e sede evento</th>
        <th>Ruolo</th>
        <xsl:if test="@fl_profiloecm=1">
          <th>ECM</th>  
        </xsl:if>
        <th>Stato Partecipazione</th>
        <th>Materiale</th>
      </tr>
      <xsl:apply-templates select="evento" />
    </table>
    
    
  </xsl:template>
  
  
  <xsl:template match="evento">
    <tr class="everow">
      <!-- data / date: sempre presenti -->
      <td>
        <b>
          <xsl:call-template name="dataDalAl_it_mmm">
            <xsl:with-param name="dataDal" select="@dt_inizio" />
            <xsl:with-param name="dataAl" select="@dt_fine" />
          </xsl:call-template>
        </b>
      </td>
      <!-- tipo -->
      <td>
        <xsl:choose>
          <xsl:when test="@ac_tipoglob='EVE_INT'">
            Evento organizzato da <xsl:value-of select="$companyname"/>
          </xsl:when>
          <xsl:when test="@ac_tipoglob='IND_EXT'">
            Formazione individuale esterna
          </xsl:when>
          <xsl:when test="@ac_tipoglob='FOR_IND'">
            <xsl:value-of select="@tx_tipopartecipazione"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <!-- tipo, titolo e sede -->
      <td>
        <xsl:value-of select="@tx_tipologiaevento"/>
        <br/>
        <b>
          <xsl:value-of select="@tx_titoloevento"/>
          <xsl:if test="@tx_edizione!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_edizione"/>
          </xsl:if>
        </b>
        <xsl:if test="@tx_sede!='' and @fl_fad=0">
          <br/>
          <xsl:value-of select="@tx_sede"/>
          <xsl:if test="@tx_dettaglisede!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_dettaglisede"/>
          </xsl:if>
        </xsl:if>
      </td>
      <!-- ruolo -->
      <td>
        <xsl:value-of select="@tx_categoriaecm"/>
      </td>
      <!-- ECM: solo se il profilo lo prevede -->
      <xsl:if test="../@fl_profiloecm=1">
        <td>
        <xsl:choose>
          <xsl:when test="@ac_normativaecm='NONE'">
            Evento non accreditato ECM, oppure informazioni sui crediti non disponibili
          </xsl:when>
          <xsl:otherwise>
            <!-- evento accreditato ECM -->
            <xsl:choose>
              <xsl:when test="@ac_statoecm='C'">
                Informazioni sui crediti attualmente non disponibili
              </xsl:when>
              <xsl:when test="@ac_statoecm='NC'">
                Non rispetti i requisiti per l'ottenimento dei crediti ECM
              </xsl:when>
              <xsl:when test="@ac_statoecm='COK'">
                <div>
                  <b>
                    <xsl:choose>
                      <xsl:when test="@ac_categoriaecm='P'">
                        <xsl:choose>
                          <xsl:when test="@nd_creditiecm_evento=1">
                            1 credito conseguito
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:call-template name="number_xdec">
                              <xsl:with-param name="number" select="@nd_creditiecm_evento" />
                            </xsl:call-template> crediti conseguiti
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test="@nd_creditiecm_iscritto=1">
                            1 credito conseguito
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:call-template name="number_xdec">
                              <xsl:with-param name="number" select="@nd_creditiecm_iscritto" />
                            </xsl:call-template> crediti conseguiti
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </b>
                </div>
                <!-- attestato on line: solo per EVE_INT -->
                <xsl:if test="@ac_tipoglob='EVE_INT'">
                  <div>
                  <xsl:choose>
                    <xsl:when test="@fl_attecmonline=1">
                      <a class="classica" style="font-weight:bold">
                        <xsl:attribute name="href">
                          <xsl:value-of select="'/attestato-ecm/'"/>
                          <xsl:value-of select="@id_iscritto"/>
                        </xsl:attribute>
                        Scarica l'attestato
                      </a>
                    </xsl:when>
                    <xsl:otherwise>
                      Attestato ECM non disponibile per lo scaricamento
                    </xsl:otherwise>
                  </xsl:choose>
                </div>
                </xsl:if>
              </xsl:when>
              <xsl:when test="@ac_statoecm='CKO'">
                Non hai conseguito i crediti ECM
              </xsl:when>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </td>
      </xsl:if>
      <!-- stato ed eventuale attestato -->
      <td>
        <xsl:choose>
          <xsl:when test="@ac_tipoglob='EVE_INT'">
            <xsl:choose>
              <xsl:when test="@ac_statoiscrizione='PP'">
                <b style="color:#ff6600">
                  Presenza parziale
                </b>
              </xsl:when>
              <xsl:otherwise>
                <b style="color:#00aa00">
                  <xsl:choose>
                    <xsl:when test="@fl_fad=1">
                      Partecipazione confermata
                    </xsl:when>
                    <xsl:otherwise>
                      Presenza confermata
                    </xsl:otherwise>
                  </xsl:choose>
                </b>
              </xsl:otherwise>
            </xsl:choose>
            
            <xsl:if test="@fl_attpartonline='1'">
              <br/>
              <a class="classica" style="font-weight:bold">
                <xsl:attribute name="href">
                  <xsl:value-of select="'/attestato-partecipazione/'"/>
                  <xsl:value-of select="@id_iscritto"/>
                </xsl:attribute>
                Scarica attestato di partecipazione
              </a>
            </xsl:if>
          </xsl:when>
          <xsl:when test="@ac_tipoglob='IND_EXT' or @ac_tipoglob='FOR_IND'">
            <b>
              <xsl:attribute name="style">
                <xsl:value-of select="'color:'"/>
                <xsl:value-of select="@ac_rgb"/>
              </xsl:attribute>
              <xsl:value-of select="@tx_statopartecipazione"/>
            </b>
            <xsl:if test="@fl_ko=1 and @tx_noteavanzamento!=''">
              <br />
              <xsl:value-of select="@tx_noteavanzamento" />
            </xsl:if>
          </xsl:when>
        </xsl:choose>
      </td>
      <!-- materiale -->
      <td>
        <xsl:choose>
          <xsl:when test="@ac_tipoglob='EVE_INT'">
            <b>
              <asp:LinkButton runat="server" CssClass="classica">
                <xsl:attribute name="ID">
                  <xsl:value-of select="'lnkMateriale_'"/>
                  <xsl:value-of select="@id_iscritto"/>
                </xsl:attribute>
                <xsl:attribute name="CommandArgument">
                  <xsl:value-of select="@id_evento"/>
                </xsl:attribute>
                Visualizza Materiale
              </asp:LinkButton>
            </b>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="nbsp" />
          </xsl:otherwise>
        </xsl:choose>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
