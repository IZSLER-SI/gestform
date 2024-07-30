<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="region" />
  <xsl:param name="companyname" />
  <xsl:param name="flspid" />

  <xsl:template match="/">
    <div class="title blue bottom10">
      ISCRIZIONE
    </div>
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <!-- se ho un testo particolare per l'iscrizione, riporto quello e basta -->
    <xsl:choose>
      <xsl:when test="count(@iol_ht_testoiscr_iolnonpreviste)&gt;0">
        <xsl:value-of select="@iol_ht_testoiscr_iolnonpreviste" disable-output-escaping="yes" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$region='LoggedOut'">
            <xsl:call-template name="evento_loggedout" />
          </xsl:when>
          <xsl:when test="$region='LoggedIn'">
            <xsl:call-template name="evento_loggedin" />
          </xsl:when>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template name="evento_loggedout">
    <!-- non loggato -->
    <xsl:choose>
      <xsl:when test="@ac_statoiscrizionifo='NON_PREV'">
        Per questo evento non sono previste le iscrizioni on line.
      </xsl:when>
      <xsl:when test="@ac_statoiscrizionifo='APRIRANNO'">
        Le iscrizioni apriranno il
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@iol_dt_aperturaiscrizioni" />
          </xsl:call-template>
        </b>
        e chiuderanno il 
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
          </xsl:call-template>
        </b>.
      </xsl:when>
      <xsl:when test="@ac_statoiscrizionifo='CHIUSE'">
        Le iscrizioni si sono chiuse il
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
          </xsl:call-template>
        </b>.
      </xsl:when>
      <xsl:when test="@ac_statoiscrizionifo='APERTE'">
        <div>
          E' possibile iscriversi fino al
          <b>
            <xsl:call-template name="data_it_mmmm">
              <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
            </xsl:call-template>
          </b>.
          <br/>
          Per informazioni dettagliate sui requisiti per l'iscrizione consulta l'ultima
          sezione della colonna di sinistra.
        </div>
        <xsl:call-template name="statopartecipanti" />
        <div class="boxiscr_sez">
          <span class="classica" style="font-weight:bold;" onclick="openRegistration();">Accedi al portale</span>
          per iscriverti.
        </div>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="evento_loggedin">
    <xsl:choose>
      <xsl:when test="count(iscritto)&gt;0">
        <xsl:call-template name="evento_loggedin_iscritto" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="evento_loggedin_noniscritto" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

    
   
    
  <xsl:template name="evento_loggedin_iscritto">
    <xsl:choose>
      <xsl:when test="@fl_fad=1">
        <xsl:choose>
          <xsl:when test="iscritto/@ac_categoriaecm='P'">
            <!-- partecipante FAD -->
              <xsl:choose>
              <xsl:when test="$flspid='0'">
                  <b class="green">Sei iscritto al corso FAD.</b><br/>
                  Per fruire del corso, collegati a
                  <a target="_blank" href="https://fad.izsler.it/" class="classica">fad.izsler.it</a>
                  ed effettua l'accesso usando le stesse credenziali che hai usato per accedere a questo portale.
              </xsl:when>
              <xsl:otherwise>
                  <b class="green">Sei iscritto al corso FAD.</b><br/>
                  Per fruire del corso, clicca su:<br />
                  <a href="#" onclick="linksso()" class="classica">ACCEDI ALLA PIATTAFORMA E-LEARNING</a>
              </xsl:otherwise>
              </xsl:choose>         
          </xsl:when>
          <xsl:otherwise>
            Sei iscritto a questo evento in qualità di
            <b>
              <xsl:value-of select="iscritto/@tx_categoriaecm"/>
            </b>.
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="iscritto/@ac_categoriaecm='P'">
            <xsl:call-template name="evento_loggedin_iscritto_partecipante" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="evento_loggedin_iscritto_drt" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="evento_loggedin_iscritto_partecipante">
    <xsl:choose>
      <xsl:when test="@ac_statoiscrizionifo='APERTE'">
        <!-- sei iscritto e puoi cancellarti -->
        <div>
          <xsl:choose>
            <xsl:when test="iscritto/@ac_statoiscrizione='I'">
                <b class="green">Sei iscritto all'evento.</b>
            </xsl:when>
            <xsl:when test="iscritto/@ac_statoiscrizione='LAP'">
              <b class="orange">
                Sei iscritto all'evento
                <xsl:choose>
                  <xsl:when test="@fl_q2='1'">
                    in lista d'attesa prioritaria
                  </xsl:when>
                  <xsl:otherwise>
                    in lista d'attesa
                  </xsl:otherwise>
                </xsl:choose>
                (posizione n.<xsl:value-of select="iscritto/@ni_posizione"/>).
              </b>
              <br/>
              Verrai <b>avvisato via e-mail</b> nel caso in cui la tua iscrizione venisse accettata.
            </xsl:when>
            <xsl:when test="iscritto/@ac_statoiscrizione='LAS'">
              <b class="orange">
                Sei iscritto all'evento in lista d'attesa secondaria
                (posizione n.<xsl:value-of select="iscritto/@ni_posizione"/>).
              </b>
              <br/>
              Verrai avvisato via e-mail 
              dopo il
              <b>
                <xsl:call-template name="data_it_mmmm">
                  <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                </xsl:call-template>
              </b>
              nel caso in cui la tua iscrizione venisse accettata.
            </xsl:when>
          </xsl:choose>
          <br/>
          Se lo desideri, puoi cancellare la tua iscrizione entro il
          <b>
            <xsl:call-template name="data_it_mmmm">
              <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
            </xsl:call-template>
          </b>.
        </div>
        <xsl:call-template name="statopartecipanti" />
        <div class="boxiscr_sez">
          <span class="btnlink btnlink_blue" style="font-weight:bold;font-size:14px;" onclick="showRegistrationPopup(true);">CANCELLA LA TUA ISCRIZIONE</span>  
        </div>
      </xsl:when>
      <xsl:otherwise>
        <!-- sei iscritto e basta... -->
        <div>
          <xsl:choose>
            <xsl:when test="iscritto/@ac_statoiscrizione='I'">
              <b class="green">Sei iscritto all'evento.</b>
              <br/>
              Se hai la necessità di cancellare la tua iscrizione, ti preghiamo di contattare
              la segreteria organizzativa.
            </xsl:when>
            <xsl:when test="iscritto/@ac_statoiscrizione='LAP'">
              <b class="orange">
                Sei iscritto all'evento
                <xsl:choose>
                  <xsl:when test="@fl_q2='1'">
                    in lista d'attesa prioritaria
                  </xsl:when>
                  <xsl:otherwise>
                    in lista d'attesa
                  </xsl:otherwise>
                </xsl:choose>
                (posizione n.<xsl:value-of select="iscritto/@ni_posizione"/>).
              </b>
              <br/>
              Ti comunicheremo a breve se potrai o non potrai partecipare all'evento.
              <br/>
              Se hai la necessità di cancellare la tua iscrizione, ti preghiamo di contattare
              la segreteria organizzativa.
            </xsl:when>
            <xsl:when test="iscritto/@ac_statoiscrizione='LAS'">
              <b class="orange">
                Sei iscritto all'evento in lista d'attesa secondaria
                (posizione n.<xsl:value-of select="iscritto/@ni_posizione"/>).
              </b>
              <br/>
              Ti comunicheremo a breve se potrai o non potrai partecipare all'evento.
              <br/>
              Se hai la necessità di cancellare la tua iscrizione, ti preghiamo di contattare
              la segreteria organizzativa.
            </xsl:when>
            <xsl:when test="iscritto/@ac_statoiscrizione='NA'">
              <b class="red">
                Siamo spiacenti, la tua richiesta di iscrizione non è stata accettata
                per esaurimento dei posti disponibili.
              </b>
            </xsl:when>
            
          </xsl:choose>
        </div>
      </xsl:otherwise>
    </xsl:choose>
    
  </xsl:template>

  <xsl:template name="evento_loggedin_iscritto_drt">
    <div>
      Sei iscritto a questo evento in qualità di
      <b>
        <xsl:value-of select="iscritto/@tx_categoriaecm"/>
      </b>.
    </div>
  </xsl:template>

  <xsl:template name="evento_loggedin_noniscritto">
    <xsl:choose>
      <xsl:when test="@ac_statoiscrizionifo='NON_PREV'">
        Per questo evento non sono previste le iscrizioni on line.
      </xsl:when>
      <xsl:when test="@ac_statoiscrizionifo='APRIRANNO'">
        Le iscrizioni apriranno il
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@iol_dt_aperturaiscrizioni" />
          </xsl:call-template>
        </b>
        e chiuderanno il
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
          </xsl:call-template>
        </b>.
      </xsl:when>
      <xsl:when test="@ac_statoiscrizionifo='CHIUSE'">
        Le iscrizioni si sono chiuse il
        <b>
          <xsl:call-template name="data_it_mmmm">
            <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
          </xsl:call-template>
        </b>.
      </xsl:when>
      <xsl:when test="@ac_statoiscrizionifo='APERTE'">
        <div>
          E' possibile iscriversi fino al
          <b>
            <xsl:call-template name="data_it_mmmm">
              <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
            </xsl:call-template>
          </b>.
          <br/>
          Per informazioni dettagliate sui requisiti per l'iscrizione consulta l'ultima
          sezione della colonna di sinistra.
        </div>
        <xsl:call-template name="statopartecipanti" />
        <div class="boxiscr_sez">
          <xsl:choose>
            <xsl:when test="@ac_destinazioneaccesso='ACC_Q1' or @ac_destinazioneaccesso='Q2'">
              <span class="btnlink btnlink_blue" style="font-weight:bold;font-size:14px;" onclick="showRegistrationPopup(true);">ISCRIVITI ALL'EVENTO</span>
            </xsl:when>
            <xsl:otherwise>
              <b class="red">Non puoi iscriverti</b> a questo evento perchè non rispetti i requisiti per l'accesso.
            </xsl:otherwise>
          </xsl:choose>
        </div>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="statopartecipanti">
    <xsl:if test="count(@iol_ni_maxpartecipanti)&gt;0">
      <div class="boxiscr_sez">
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
              <br />Lista d'attesa secondaria:
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
                  <br />Lista d'attesa:
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
                  <br />Lista d'attesa prioritaria:
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
                  <br />Lista d'attesa secondaria:
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
  </xsl:template>
</xsl:stylesheet>
