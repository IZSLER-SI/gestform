<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="region" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  
    <xsl:template match="evento">
      <div class="title green">
        <xsl:value-of select="@tx_titolo"/>
        <xsl:if test="@tx_edizione!=''">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_edizione"/>
        </xsl:if>
      </div>
      <div class="title">
        <xsl:call-template name="dataDalAl_it_mmmm">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </div>

      <!-- sede (solo se non FAD) -->
      <xsl:if test="@fl_fad!=1">
        <div class="section">SEDE</div>
        <div class="sectiondata">
          <!-- URL per link -->
          <xsl:variable name="mapurl">
            <xsl:value-of select="'http://maps.google.com/maps?q='" />
            <!-- indirizzo -->
            <xsl:value-of select="@tx_indirizzosede_tratt"/>
            <!-- lingua -->
            <xsl:value-of select="'&amp;hl=it'"/>
            <!-- tipo e zoom -->
            <xsl:value-of select="'&amp;=m&amp;z=14&amp;vpsrc=0&amp;iwloc=A'"/>
          </xsl:variable>
          <xsl:variable name="imageurl">
            <xsl:value-of select="'http://maps.googleapis.com/maps/api/staticmap?center='" />
            <!-- indirizzo centratura -->
            <xsl:value-of select="@tx_indirizzosede_tratt"/>
            <!-- indirizzo marker -->
            <xsl:value-of select="'&amp;markers=color:blue|'" />
            <xsl:value-of select="@tx_indirizzosede_tratt"/>
            <!-- altro -->
            <xsl:value-of select="'&amp;sensor=false&amp;size=250x140&amp;maptype=roadmap&amp;zoom=14'"/>
          </xsl:variable>
          <div style="float:right;">
            <a target="_blank">
              <xsl:attribute name="href">
                <xsl:value-of select="$mapurl"/>
              </xsl:attribute>
              <img border="0" width="180" height="140">
                <xsl:attribute name="src">
                  <xsl:value-of select="'../img/map.png'"/>
                </xsl:attribute>
              </img>
            </a>
            <div style="text-align:center;font-weight:bold;font-size:11px;">
              clicca per visualizzare la mappa
            </div>
          </div>
          <b>
            <xsl:value-of select="@tx_sede"/>
            <xsl:if test="@tx_dettaglisede!=''">
              <xsl:value-of select="' - '"/>
              <xsl:value-of select="@tx_dettaglisede"/>
            </xsl:if>
          </b>
          <br/>
          <xsl:value-of select="@tx_indirizzosede_br" disable-output-escaping="yes" />
          <div class="clear"></div>
        </div>
      </xsl:if>
      <!-- organizzatore -->
    <div class="section">ORGANIZZATORE</div>
    <div class="sectiondata">
      <b>
        <xsl:value-of select="@tx_organizzatore"/>
      </b>
    </div>
    <!-- SO -->
    <div class="section">SEGRETERIA ORGANIZZATIVA</div>
    <div class="sectiondata">
      <b>
        <xsl:value-of select="@tx_segreteriaorganizzativa"/>
      </b>
      <br/>
      <xsl:value-of select="@tx_indirizzoso_br" disable-output-escaping="yes" />
      <xsl:if test="@tx_telefonoso!=''">
        Telefono: <xsl:value-of select="@tx_telefonoso"/><br/>
      </xsl:if>
      <xsl:if test="@tx_faxso!=''">
        Fax: <xsl:value-of select="@tx_faxso"/><br/>
      </xsl:if>
      <xsl:if test="@tx_emailso!=''">
        <a class="classica_nu">
          <xsl:attribute name="href">
            <xsl:value-of select="'mailto:'"/>
            <xsl:value-of select="@tx_emailso"/>
          </xsl:attribute>
          <xsl:value-of select="@tx_emailso"/>
        </a>
        <br/>
      </xsl:if>
    </div>
    <!-- ECM -->
    <div class="section">EDUCAZIONE CONTINUA IN MEDICINA (ECM)</div>
    <div class="sectiondata">
      <xsl:choose>
        <xsl:when test="@ac_normativaecm='NONE'">
          <b>Evento non accreditato ECM</b>
        </xsl:when>
        <xsl:otherwise>
          <b>Evento accreditato ECM</b>
          <br/>
          <xsl:choose>
            <xsl:when test="count(@ecm2_num_cred)=0">
              Numero di crediti non disponibile
            </xsl:when>
            <xsl:otherwise>
              <b>
                <xsl:call-template name="number_xdec">
                  <xsl:with-param name="number" select="@ecm2_num_cred" />
                </xsl:call-template>
              </b>
              <xsl:choose>
                <xsl:when test="@ecm2_num_cred=1">
                  credito formativo
                </xsl:when>
                <xsl:otherwise>
                  crediti formativi
                </xsl:otherwise>
              </xsl:choose>
            </xsl:otherwise>
          </xsl:choose>
          <br/>
          Professioni/discipline:
          <ul>
            <xsl:for-each select="professionedisciplina">
              <li>
                <xsl:value-of select="@tx_professione"/>
                -
                <xsl:value-of select="@tx_disciplina"/>
              </li>
            </xsl:for-each>
          </ul>
        </xsl:otherwise>
      </xsl:choose>
    </div>
    <!-- files -->
    <xsl:apply-templates select="sezione" />
    <!-- criteri per l'accesso -->
    <div class="section">REQUISITI PER L'ISCRIZIONE</div>
    <div class="sectiondata">
      <div>
        <xsl:choose>
          <xsl:when test="count(@iol_ni_maxpartecipanti)=0">
            Questo evento non prevede un numero massimo di partecipanti.
          </xsl:when>
          <xsl:otherwise>
            Numero massimo di partecipanti:
            <b>
              <xsl:value-of select="@iol_ni_maxpartecipanti"/>
            </b>
          </xsl:otherwise>
        </xsl:choose>
      </div>
      <div>
        <xsl:choose>
          <xsl:when test="count(criterio)=0">
            <b>Al momento non sono disponibili i requisiti per l'iscrizione all'evento.</b>
          </xsl:when>
          <xsl:when test="count(criterio)=1">
            <b>
              Puoi iscriverti all'evento se rispetti i seguenti requisiti:
            </b>
          </xsl:when>
          <xsl:otherwise>
            <b>
              Puoi iscriverti all'evento se rientri in uno dei seguenti gruppi:
            </b>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:if test="count(criterio)&gt;0">
          <xsl:variable name="criterimultipli">
            <xsl:choose>
              <xsl:when test="count(criterio)=1">
                <xsl:value-of select="'0'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'1'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:variable name="q2">
            <xsl:choose>
              <xsl:when test="count(criterio[@ac_destinazione='Q2'])=1">
                <xsl:value-of select="'1'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'0'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$criterimultipli='1'">
              <ul>
                <xsl:for-each select="criterio">
                  <li>
                    <b>
                      GRUPPO <xsl:value-of select="@ni_criterio"/>
                    </b>
                    <xsl:apply-templates select=".">
                      <xsl:with-param name="criterimultipli" select="$criterimultipli" />
                      <xsl:with-param name="q2" select="$q2" />
                    </xsl:apply-templates>
                  </li>
                </xsl:for-each>
              </ul>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="criterio">
                <xsl:with-param name="criterimultipli" select="$criterimultipli" />
                <xsl:with-param name="q2" select="$q2" />
              </xsl:apply-templates>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="criterio">
    <xsl:param name="criterimultipli" />
    <xsl:param name="q2" />
    <ul>
      <!-- dip / ext -->
      <li>
        <xsl:choose>
          <xsl:when test="@ac_dipext='EXT'">
            NON sei dipendente <xsl:value-of select="$companyname"/>
          </xsl:when>
          <xsl:when test="@ac_dipext='DIP'">
            Sei dipendente <xsl:value-of select="$companyname"/>
          </xsl:when>
          <xsl:when test="@ac_dipext='ALL'">
            Sei o non sei dipendente <xsl:value-of select="$companyname"/>
          </xsl:when>
          <xsl:when test="@ac_dipext='DIP_UO'">
            Sei dipendente <xsl:value-of select="$companyname"/> e lavori in uno dei seguenti reparti / U.O.:
            <ul>
              <xsl:for-each select="uo">
                <li>
                  <xsl:value-of select="@tx_unitaoperativa"/>
                </li>
              </xsl:for-each>
            </ul>
          </xsl:when>
        </xsl:choose>
      </li>
      <!-- profilo -->
      <xsl:if test="@fl_profili=1">
        <li>
          Il tuo profilo professionale è uno dei seguenti:
          <ul>
            <xsl:for-each select="profilo">
              <li>
                <xsl:value-of select="@tx_profilo"/>
              </li>
            </xsl:for-each>
          </ul>
        </li>
      </xsl:if>
      <!-- professione / disciplina ECM -->
      <xsl:if test="../@ac_normativaecm!='NONE'">
        <xsl:choose>
          <xsl:when test="@fl_prodiscecmaccr='1'">
            <li>
              La tua professione/disciplina ti consente di ottenere crediti ECM per questo evento
            </li>
          </xsl:when>
          <xsl:when test="@fl_prodiscecmaccr='0'">
            <li>
              La tua professione/disciplina NON ti consente di ottenere crediti ECM per questo evento
            </li>

          </xsl:when>
        </xsl:choose>
      </xsl:if>
    </ul>
    <!-- testo finale - solo se le iscrizioni sono aperte -->
    <xsl:if test="count(../@iol_ni_maxpartecipanti)&gt;0 and count(../@iol_dt_aperturaiscrizioni)&gt;0">
      <div class="green">
        <xsl:choose>
          <xsl:when test="@ac_destinazione='Q2'">
            <!-- lista d'attesa secondaria: non sarà mai su criteri singoli -->
            Se rientri in questo gruppo, la tua iscrizione sarà posizionata in una <b>
              lista d'attesa
              secondaria.
            </b><br/>
            Potrai partecipare all'evento solo se, dopo la chiusura delle iscrizioni, saranno
            ancora disponibili posti.
          </xsl:when>
          <xsl:otherwise>
            <!-- lista d'attesa -->
            <xsl:choose>
              <xsl:when test="$criterimultipli='1'">
                Se rientri in questo gruppo, la tua iscrizione sarà <b>immediatamente accettata</b> fino
                al raggiungimento del numero di posti disponibili.<br/>
                In alternativa essa sarà posizionata in una
                <xsl:choose>
                  <xsl:when test="$q2='1'">
                    <b>lista d'attesa prioritaria</b>.
                  </xsl:when>
                  <xsl:otherwise>
                    <b>lista d'attesa</b>.
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <!-- criterio singolo -->
                La tua iscrizione sarà <b>immediatamente accettata</b> fino al raggiungimento del numero di posti disponibili.<br/>
                in alternativa essa sarà posizionata in una <b>lista d'attesa</b>.
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>

      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="sezione">
    <xsl:if test="count(file[@fl_visibile=1])&gt;0">
      <div class="section">
        <xsl:choose>
          <xsl:when test="count(file[@fl_visibile=1])=1">
            <xsl:value-of select="@tx_sezionedocevento_sing"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="@tx_sezionedocevento_plur"/>
          </xsl:otherwise>
        </xsl:choose>
      </div>
      <div class="sectiondata">
        <xsl:apply-templates select="file[@fl_visibile=1]" />
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="file">
    <a class="dl_item">
      <xsl:attribute name="href">
        <xsl:value-of select="'/DocEve/'"/>
        <xsl:value-of select="@id_file_evento"/>
        <xsl:value-of select="@ext"/>
      </xsl:attribute>
      <div class="img">
        <img>
          <xsl:attribute name="src">
            <xsl:value-of select="'/DocEveThumbs/'"/>
            <xsl:value-of select="@thumbnail"/>
          </xsl:attribute>
        </img>
      </div>
      <div class="text">
        <div class="name">
          <xsl:value-of select="@tx_descrizione"/>
        </div>
        <xsl:if test="count(@ht_note)&gt;0">
          <div class="userhtml">
            <xsl:value-of select="@ht_note" disable-output-escaping="yes" />
          </div>
        </xsl:if>
      </div>
      <div class="clear">
        <xsl:value-of select="''" />
      </div>
    </a>
  </xsl:template>

</xsl:stylesheet>