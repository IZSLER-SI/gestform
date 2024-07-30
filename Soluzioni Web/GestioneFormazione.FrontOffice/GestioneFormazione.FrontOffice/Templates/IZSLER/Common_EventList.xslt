<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="region" />
  <xsl:param name="node" />
  <xsl:param name="searchactive" />
  <xsl:param name="companyname" />
  <xsl:param name="filtertype" />

  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="$node='homepage'">
        <!-- FUNZIONAMENTO HOME PAGE (loggato o non loggato -->
        <xsl:choose>
          <xsl:when test="$region='LoggedOut'">
            <div class="title green bottom20">
              I prossimi eventi formativi ai quali è possibile iscriversi
            </div>
            <xsl:choose>
              <xsl:when test="count(eventi/evento[@fl_match=1])&gt;0">
                <xsl:apply-templates select="eventi/evento[@fl_match=1]" />
              </xsl:when>
              <xsl:otherwise>
                <div>
                  <em>Al momento non ci sono eventi formativi in calendario ai quali è possibile iscriversi on line.</em>
                </div>
                <div>
                  <br/>
                </div>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$region='LoggedIn'">
            <div class="title green bottom20">
              I prossimi eventi formativi ai quali puoi iscriverti
            </div>
            <xsl:choose>
              <xsl:when test="count(eventi/evento[@fl_match=1])&gt;0">
                <xsl:apply-templates select="eventi/evento[@fl_match=1]" />
              </xsl:when>
              <xsl:otherwise>
                <div>
                  <em>Al momento non ci sono eventi formativi in calendario ai quali puoi iscriverti in base al tuo profilo.</em>
                </div>
                <div>
                  <br/>
                </div>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
        <div>
          <a class="classica" href="/eventi" style="font-weight:bold;">Offerta formativa completa</a>
        </div>
      </xsl:when>
      <xsl:when test="$node='eventi'">
        <!-- titolo -->
        <div class="title green bottom20">
          <xsl:choose>
            <xsl:when test="$region='LoggedIn'">
              <!--i prossimi--> Eventi formativi ai quali puoi partecipare
            </xsl:when>
          </xsl:choose>
        </div>
            <!-- menu -->
            <div id="navleft_submenu">
              <xsl:call-template name="item">
                <xsl:with-param name="active">
                  <xsl:choose>
                    <xsl:when test="$filtertype='nxt'">1</xsl:when>
                    <xsl:otherwise>0</xsl:otherwise>
                  </xsl:choose>
                </xsl:with-param>
                <xsl:with-param name="enabled" select="'1'" />
                <xsl:with-param name="row1" select="'PROSSIMI EVENTI RESIDENZIALI'" />
                <xsl:with-param name="url" select="'/eventit'" />
                <xsl:with-param name="filtertype" select="'nxt'" />
              </xsl:call-template>

              <xsl:call-template name="item">
                <xsl:with-param name="active">
                  <xsl:choose>
                    <xsl:when test="$filtertype='fad'">1</xsl:when>
                    <xsl:otherwise>0</xsl:otherwise>
                  </xsl:choose>
                </xsl:with-param>
                <xsl:with-param name="enabled" select="'1'" />
                <xsl:with-param name="row1" select="'CORSI FAD APERTI'" />
                <xsl:with-param name="url" select="'/eventit'" />
                <xsl:with-param name="filtertype" select="'fad'" />
              </xsl:call-template>

              <xsl:call-template name="item">
                <xsl:with-param name="active">
                  <xsl:choose>
                    <xsl:when test="$filtertype='all'">1</xsl:when>
                    <xsl:otherwise>0</xsl:otherwise>
                  </xsl:choose>
                </xsl:with-param>
                <xsl:with-param name="enabled" select="'1'" />
                <xsl:with-param name="row1" select="'TUTTI GLI EVENTI'" />
                <xsl:with-param name="url" select="'/eventit'" />
                <xsl:with-param name="filtertype" select="'all'" />
                <xsl:with-param name="last" select="'1'" />
              </xsl:call-template>
            </div>

            <!-- chiusura -->
            <div class="clear">
              <xsl:value-of select="''"/>
            </div>
        
            <!-- elenco -->
            <xsl:choose>
              <xsl:when test="count(eventi/evento[@fl_match=1])&gt;0">
                <xsl:apply-templates select="eventi/evento[@fl_match=1]" />
              </xsl:when>
              <xsl:otherwise>
                <div>
                  <em>
                    <xsl:choose>
                      <xsl:when test="$searchactive=1">
                        Siamo spiacenti, la tua ricerca non ha prodotto risultati.
                      </xsl:when>
                      <xsl:otherwise>
                        Siamo spiacenti, al momento non ci sono eventi formativi in calendario ai quali puoi partecipare in base al tuo profilo.
                      </xsl:otherwise>
                    </xsl:choose>
                    </em>
                </div>
                <div>
                  <br/>
                </div>
              </xsl:otherwise>
            </xsl:choose>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="evento">
    <a class="list_evento">
      <xsl:attribute name="href">
        <xsl:value-of select="'/eventi/'"/>
        <xsl:value-of select="@id_evento"/>
      </xsl:attribute>
      <!-- date e sede -->
      <div>
        <b>
          <xsl:call-template name="dataDalAl_it_mmmm">
            <xsl:with-param name="dataDal" select="@dt_inizio" />
            <xsl:with-param name="dataAl" select="@dt_fine" />
          </xsl:call-template>
        </b>
        <xsl:if test="@fl_fad!=1">
          -
          <xsl:value-of select="@tx_sede"/>
          <xsl:if test="@tx_dettaglisede!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_dettaglisede"/>
          </xsl:if>
        </xsl:if>
      </div>
      <!-- titolo -->
      <div class="green">
        <b>
          <xsl:value-of select="@tx_titolo"/>
        </b>
        <xsl:if test="@tx_edizione!=''">
          <b class="black">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_edizione"/>
          </b>
        </xsl:if>
      </div>
      <div class="small">
        <!-- ECM e criteri di accesso -->
        <div>
          <xsl:choose>
            <xsl:when test="@fl_dip=1 and @fl_ext=1">
              Evento aperto a dipendenti <xsl:value-of select="$companyname"/> e a non dipendenti
            </xsl:when>
            <xsl:when test="@fl_dip=1 and @fl_ext=0">
              Evento riservato ai dipendenti <xsl:value-of select="$companyname"/>
            </xsl:when>
            <xsl:when test="@fl_dip=0 and @fl_ext=1">
              Evento riservato ai NON dipendenti <xsl:value-of select="$companyname"/>
            </xsl:when>
          </xsl:choose>
          -
          <xsl:choose>
            <xsl:when test="@ac_normativaecm='NONE'">
              non accreditato ECM
            </xsl:when>
            <xsl:otherwise>
              <xsl:choose>
                <xsl:when test="count(@ecm2_num_cred)=0">
                  accreditato ECM - numero di crediti non disponibile
                </xsl:when>
                <xsl:otherwise>
                  <b>
                    <xsl:call-template name="number_xdec">
                      <xsl:with-param name="number" select="@ecm2_num_cred" />
                    </xsl:call-template>
                  </b>
                  <xsl:choose>
                    <xsl:when test="@ecm2_num_cred=1">
                      credito ECM
                    </xsl:when>
                    <xsl:otherwise>
                      crediti ECM
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:otherwise>
          </xsl:choose>
        </div>
        <!-- dati profili -->
        <div>
          <xsl:choose>
            <xsl:when test="@fl_profili=1">
              <div>
                Rivolto a:
                <xsl:for-each select="profilo">
                  <xsl:value-of select="@tx_profilo"/>
                  <xsl:if test="position()!=last()">
                    <xsl:value-of select="', '"/>
                  </xsl:if>
                </xsl:for-each>
              </div>
            </xsl:when>
            <xsl:otherwise>
              Rivolto a tutti i profili professionali
            </xsl:otherwise>
          </xsl:choose>
        </div>
        <!-- dati iscrizioni-->
        <div>
          <xsl:choose>
            <xsl:when test="@ac_statoiscrizioni='APRIRANNO'">
              Apertura iscrizioni il
              <b>
                <xsl:call-template name="data_it_mmmm">
                  <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                </xsl:call-template>
              </b>
            </xsl:when>
            <xsl:when test="@ac_statoiscrizioni='APERTE'">
              Iscrizioni aperte fino al
              <b>
                <xsl:call-template name="data_it_mmmm">
                  <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                </xsl:call-template>
              </b>
            </xsl:when>
            <xsl:when test="@ac_statoiscrizioni='CHIUSE'">
              <b class="red">Iscrizioni chiuse</b>
            </xsl:when>
          </xsl:choose>
        </div>
      
        <!-- posti disponibili (solo se iscrizioni aperte e se esiste max part) -->
        <xsl:if test="@ac_statoiscrizioni='APERTE'">
          <xsl:if test="count(@iol_ni_maxpartecipanti)&gt;0">
            <div>
              <!-- posti disponibili -->
              <xsl:choose>
                <xsl:when test="@iol_ni_postidisponibili&gt;0">
                  <b>
                    <xsl:value-of select="@iol_ni_postidisponibili"/>
                  </b>
                  <xsl:choose>
                    <xsl:when test="@iol_ni_postidisponibili=1">
                      posto disponibile
                    </xsl:when>
                    <xsl:otherwise>
                      posti disponibili
                    </xsl:otherwise>
                  </xsl:choose>
                  <!-- se c'è qualcuno in lista d'attesa secondaria, lo scrivo -->
                  <xsl:if test="@iol_ni_listaattesasecondaria&gt;0">
                    - lista d'attesa secondaria:
                    <b>
                      <xsl:value-of select="@iol_ni_listaattesasecondaria"/>
                    </b>
                    <xsl:choose>
                      <xsl:when test="@iol_ni_listaattesasecondaria=1">
                        persona
                      </xsl:when>
                      <xsl:otherwise>
                        persone
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>
                </xsl:when>
                <xsl:otherwise>
                  <b class="red">Posti esauriti</b>
                  <!-- dati liste d'attesa -->
                  <xsl:choose>
                    <xsl:when test="@fl_q2=0">
                      <!-- non esiste lista d'attesa secondaria -->
                      <xsl:if test="@iol_ni_istaattesaprioritaria&gt;0">
                        - lista d'attesa:
                        <b>
                          <xsl:value-of select="@iol_ni_istaattesaprioritaria"/>
                        </b>
                        <xsl:choose>
                          <xsl:when test="@iol_ni_istaattesaprioritaria=1">
                            persona
                          </xsl:when>
                          <xsl:otherwise>
                            persone
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </xsl:when>
                    <xsl:otherwise>
                      <!-- esiste lista d'attesa secondaria -->
                      <xsl:if test="@iol_ni_istaattesaprioritaria&gt;0">
                        - lista d'attesa prioritaria:
                        <b>
                          <xsl:value-of select="@iol_ni_istaattesaprioritaria"/>
                        </b>
                        <xsl:choose>
                          <xsl:when test="@iol_ni_istaattesaprioritaria=1">
                            persona
                          </xsl:when>
                          <xsl:otherwise>
                            persone
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                      <xsl:if test="@iol_ni_listaattesasecondaria&gt;0">
                        - lista d'attesa secondaria:
                        <b>
                          <xsl:value-of select="@iol_ni_listaattesasecondaria"/>
                        </b>
                        <xsl:choose>
                          <xsl:when test="@iol_ni_listaattesasecondaria=1">
                            persona
                          </xsl:when>
                          <xsl:otherwise>
                            persone
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </div>
          </xsl:if>
        </xsl:if>
      </div>
    </a>
  </xsl:template>

  <xsl:template name="item">
    <xsl:param name="last" />
    <xsl:param name="active" />
    <xsl:param name="enabled" />
    <xsl:param name="row1" />
    <xsl:param name="url" />
    <xsl:param name="filtertype" />
    <xsl:choose>
      <xsl:when test="$enabled='0'">
        <!-- non abilitato -->
        <span class="item disab">
          <xsl:if test="$last='1'">
            <xsl:attribute name="style">
              <xsl:value-of select="'border-right-width:0px;'"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$row1"/>
        </span>
      </xsl:when>
      <xsl:otherwise>
        <!-- abilitato -->
        <a>
          <xsl:attribute name="class">
            <xsl:choose>
              <xsl:when test="$active='1'">
                <xsl:value-of select="'item act'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'item enab'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="href">
            <xsl:value-of select="$url"/>/<xsl:value-of select="$filtertype"/>
          </xsl:attribute>
          <xsl:if test="$last='1'">
            <xsl:attribute name="style">
              <xsl:value-of select="'border-right-width:0px;'"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$row1"/>
        </a>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
