<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="node" />
  <xsl:param name="region" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <xsl:if test="root/@fl_vet=1">
      <div class="onecol" style="background-color:#f5f5f5;border-radius:5px;padding:10px;margin-bottom:10px;margin-top:20px;margin-left:25px;margin-right:25px;">

        <a class="classica" target="_blank" href="Files/Linee_guida_valutazione_idoneità_trasporto_suini.pdf">
          <img src="/img/icoPDF.gif" style="vertical-align:middle;margin-right:5px;" />
          Linee guida per la valutazione dell’idoneità al trasporto dei  suini
        </a>
        
      </div>
    </xsl:if>
    
    
    <xsl:if test="count(root/iscrizione)&gt;0 or count(root/attestato)&gt;0 or count(root/attestatopart)&gt;0 or count(root/valutazioneweb)&gt;0 or count(root/registrazionepresenze)&gt;0 or count(root/richiestavalidazionepg)&gt;0">
      <div class="onecol">
        <div class="title orange">
          <img src="/img/important.png" style="margin-right:2px;vertical-align:middle;line-height:25px;margin-top:-3px;" />
          PROMEMORIA
        </div>
				<xsl:if test="count(root/iscrizione_docente[@fl_modinc_compilato=2])&gt;0">
						<div class="top20">
								<b class="orange">
										Devi compilare il modulo di incarico per							
										<xsl:choose>		
												<xsl:when test="count(root/iscrizione_docente[@fl_modinc_compilato=2])=1">
														1 evento formativo.
												</xsl:when>
												<xsl:otherwise>
														<xsl:value-of select="count(root/iscrizione_docente[@fl_modinc_compilato='2'])" /> eventi formativi.
												</xsl:otherwise>
										</xsl:choose>
								</b>
								Accedi all'area
								<a class="classica" href="/iscrizioni-attive" style="font-weight:bold;">"Gli eventi a cui sono iscritto"</a>
								nella sezione "Moduli di incarico" per visualizzare i dettagli ed il link per compilare il modulo di incarico (ove richiesto).
						</div>
				</xsl:if>
        <xsl:if test="count(root/iscrizione)&gt;0">
          <div class="top20">
            <b class="orange">
              Sei iscritto a
              <xsl:choose>
              <xsl:when test="count(root/iscrizione)=1">
                1 evento formativo.
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="count(root/iscrizione)" /> eventi formativi.
              </xsl:otherwise>
            </xsl:choose>
            </b>
            Accedi all'area
            <a class="classica" href="/iscrizioni-attive" style="font-weight:bold;">"Gli eventi a cui sono iscritto"</a>
            per visualizzare i dettagli ed il materiale o se intendi cancellare la tua iscrizione (ove possibile).
          </div>
        </xsl:if>
        <xsl:if test="count(root/valutazioneweb)&gt;0">
          <xsl:for-each select="root/valutazioneweb">
            <div class="top20">
              <!-- devi -->
              <div class="orange">
                Devi compilare entro il
                <b>
                  <xsl:call-template name="data_it_mmmm">
                    <xsl:with-param name="data" select="@vweb_dt_fine" />
                  </xsl:call-template>
                </b>
                alle ore
                <b>
                  <xsl:call-template name="oraHHMM">
                    <xsl:with-param name="ora" select="@vweb_dt_fine" />
                  </xsl:call-template>
                </b>
                <xsl:choose>
                  <xsl:when test="@fl_qualita=1 and @fl_apprendimento=1">
                    i questionari di
                    <b>verifica apprendimento</b>
                    e di
                    <b>valutazione qualità percepita</b>
                  </xsl:when>
                  <xsl:when test="@fl_qualita=1 and @fl_apprendimento=0">
                    il questionario di
                    <b>valutazione qualità percepita</b>
                  </xsl:when>
                  <xsl:when test="@fl_qualita=0 and @fl_apprendimento=1">
                    il questionario di
                    <b>verifica apprendimento</b>
                  </xsl:when>
                  <xsl:otherwise>
                    -
                  </xsl:otherwise>
                </xsl:choose>
                per il seguente evento formativo:
              </div>
              <div>
                <b>
                  <xsl:value-of select="@tx_titoloevento"/>  
                  <xsl:if test="@tx_edizione!=''">
                    <xsl:value-of select="' - '"/>
                    <xsl:value-of select="@tx_edizione"/>
                  </xsl:if>
                </b>
              </div>
              <div>
                <xsl:value-of select="@tx_sede"/>
                <xsl:value-of select="', '"/>
                <xsl:call-template name="dataDalAl_it_mmmm">
                  <xsl:with-param name="dataDal" select="@dt_inizio" />
                  <xsl:with-param name="dataAl" select="@dt_fine" />
                </xsl:call-template>
              </div>
              <div>
                <a class="classica" target="_blank" style="font-weight:bold;">
                  <xsl:attribute name="href">
                    <xsl:value-of select="@url"/>
                  </xsl:attribute>
                  <xsl:choose>
                    <xsl:when test="@fl_qualita=1 and @fl_apprendimento=1">
                      Clicca qui per compilare i questionari
                    </xsl:when>
                    <xsl:otherwise>
                      Clicca qui per compilare il questionario
                    </xsl:otherwise>
                  </xsl:choose>
                </a>
              </div>
            </div>
          </xsl:for-each>
        </xsl:if>
        <xsl:if test="count(root/attestato)&gt;0">
          <div class="top20">
            <b class="orange">
              Devi scaricare l'attestato ECM per
              <xsl:choose>
                <xsl:when test="count(root/attestato)=1">
                  il seguente evento formativo:
                </xsl:when>
                <xsl:otherwise>
                  i seguenti eventi formativi:
                </xsl:otherwise>
              </xsl:choose>
            </b>
            <ul style="padding-left:20px;">
              <xsl:for-each select="root/attestato">
                <li style="padding-top:10px;">
                  <b>
                    <xsl:call-template name="dataDalAl_it_mmmm">
                      <xsl:with-param name="dataDal" select="@dt_inizio" />
                      <xsl:with-param name="dataAl" select="@dt_fine" />
                    </xsl:call-template>
                  </b>
                  -
                  <xsl:value-of select="@tx_sede"/>
                  <xsl:if test="@tx_dettaglisede!=''">
                    <xsl:value-of select="' - '"/>
                    <xsl:value-of select="@tx_dettaglisede"/>
                  </xsl:if>
                  <br/>
                  <b>
                    <xsl:value-of select="@tx_titoloevento"/>
                    <xsl:if test="@tx_edizione!=''">
                      <xsl:value-of select="' - '"/>
                      <xsl:value-of select="@tx_edizione"/>
                    </xsl:if>
                  </b>
                  <br/>
                  <a class="classica" style="font-weight:bold;">
                    <xsl:attribute name="href">
                      <xsl:value-of select="'/attestato-ecm/'"/>
                      <xsl:value-of select="@id_iscritto"/>
                    </xsl:attribute>  
                    Scarica l'attestato ECM
                  </a>
                </li>
              </xsl:for-each>
            </ul>
          </div>
        </xsl:if>
        <xsl:if test="count(root/attestatopart)&gt;0">
          <div class="top20">
            <b class="orange">
              Devi scaricare l'attestato di partecipazione per
              <xsl:choose>
                <xsl:when test="count(root/attestatopart)=1">
                  il seguente evento formativo:
                </xsl:when>
                <xsl:otherwise>
                  i seguenti eventi formativi:
                </xsl:otherwise>
              </xsl:choose>
            </b>
            <ul style="padding-left:20px;">
              <xsl:for-each select="root/attestatopart">
                <li style="padding-top:10px;">
                  <b>
                    <xsl:call-template name="dataDalAl_it_mmmm">
                      <xsl:with-param name="dataDal" select="@dt_inizio" />
                      <xsl:with-param name="dataAl" select="@dt_fine" />
                    </xsl:call-template>
                  </b>
                  -
                  <xsl:value-of select="@tx_sede"/>
                  <xsl:if test="@tx_dettaglisede!=''">
                    <xsl:value-of select="' - '"/>
                    <xsl:value-of select="@tx_dettaglisede"/>
                  </xsl:if>
                  <br/>
                  <b>
                    <xsl:value-of select="@tx_titoloevento"/>
                    <xsl:if test="@tx_edizione!=''">
                      <xsl:value-of select="' - '"/>
                      <xsl:value-of select="@tx_edizione"/>
                    </xsl:if>
                  </b>
                  <br/>
                  <a class="classica" style="font-weight:bold;">
                    <xsl:attribute name="href">
                      <xsl:value-of select="'/attestato-partecipazione/'"/>
                      <xsl:value-of select="@id_iscritto"/>
                    </xsl:attribute>
                    Scarica l'attestato di partecipazione
                  </a>
                </li>
              </xsl:for-each>
            </ul>
          </div>
        </xsl:if>
        <xsl:if test="count(root/registrazionepresenze)&gt;0">
          <div class="top20">
            <b class="orange">
              Devi caricare le date di svoglimento, gli orari e le presenze per
              <xsl:choose>
                <xsl:when test="count(root/registrazionepresenze)=1">
                  il seguente evento formativo:
                </xsl:when>
                <xsl:otherwise>
                  i seguenti eventi formativi:
                </xsl:otherwise>
              </xsl:choose>
            </b>
            <ul style="padding-left:20px;">
              <xsl:for-each select="root/registrazionepresenze">
                <li style="padding-top:10px;">
                  <b>
                    <xsl:value-of select="@tx_titoloevento"/>
                    <xsl:if test="@tx_edizione!=''">
                      <xsl:value-of select="' - '"/>
                      <xsl:value-of select="@tx_edizione"/>
                    </xsl:if>
                  </b>
                  <br/>
                  <xsl:value-of select="@tx_sede"/>
                  <xsl:if test="@tx_dettaglisede!=''">
                    <xsl:value-of select="' - '"/>
                    <xsl:value-of select="@tx_dettaglisede"/>
                  </xsl:if>
                  <xsl:value-of select="', '"/>
                  <xsl:call-template name="dataDalAl_it_mmmm">
                    <xsl:with-param name="dataDal" select="@dt_inizio" />
                    <xsl:with-param name="dataAl" select="@dt_fine" />
                  </xsl:call-template>
                  <br/>
                  
                  L'inserimento dei dati deve essere terminato entro il
                  <b>
                    <xsl:call-template name="data_it_mmmm">
                      <xsl:with-param name="data" select="@dt_fineinserimento" />
                    </xsl:call-template>
                  </b>
                  <br/>
                  <a class="classica" style="font-weight:bold;">
                    <xsl:attribute name="href">
                      <xsl:value-of select="'/inserimento-presenze/'"/>
                      <xsl:value-of select="@id_evento"/>
                    </xsl:attribute>
                    Inserimento dati
                  </a>
                </li>
              </xsl:for-each>
            </ul>
          </div>
        </xsl:if>
        <!-- Richiesta validazione PG-->
        <xsl:if test="count(root/richiestavalidazionepg)&gt;0">
          <div class="top20">
              <hr style="height:1px; solid; border:1px; color:#eaeaea; background-color:#eaeaea;" />           
              <!-- Testo generale -->
              <span class="orange" style="padding-top:10px">E' necessario procedere alla validazione della partecipazione agli eventi esterni.</span>
              <br />
              <span>
                Per ogni evento devi confermare, consapevole delle responsabilità penali e degli effetti amministrativi derivanti dalla falsità in atti e dalle dichiarazioni mendaci:
                <ul>
                  <li>di avere effettivamente preso parte all'evento e di avere ottenuto l'eventuale attestato di partecipazione</li>
                  <li>di aver inviato agli uffici preposti (U.O.Gestione del Personale e Ufficio Formazione) tutta la documentazione necessaria alla registrazione.</li>
                </ul>
                <a class='classica' href="/validazione-pg">
                  <strong>Procedi alla validazione degli eventi.</strong>
                </a>
              </span>
          </div>      
        </xsl:if>
      </div>

    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
