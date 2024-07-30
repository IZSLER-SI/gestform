<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:import href="../../Common.xslt"/>
    <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
    <xsl:param name="baseurl" />
    <xsl:template match="/">
        <xsl:apply-templates select="evento" />
    </xsl:template>

    <xsl:template match="evento">
        <div>
            <xsl:value-of select="iscritto/@tx_vocativo"/>
            <xsl:value-of select="' '"/>
            <b>
                <xsl:value-of select="iscritto/@tx_nome"/>
                <xsl:value-of select="' '"/>
                <xsl:value-of select="iscritto/@tx_cognome"/>
            </b>
            <xsl:value-of select="','"/>
        </div>
        <div>
            <br/>
        </div>
        <!-- con la presente...-->
        <div>
            <xsl:choose>
                <xsl:when test="iscritto/@ac_statoiscrizione='I'">
                    Con la presente ti <b>confermiamo che è stata accettata la tua richiesta di iscrizione</b> al seguente evento formativo:
                </xsl:when>
                <xsl:when test="iscritto/@ac_statoiscrizione='LAP'">
                    Con la presente ti
                    <b>
                        confermiamo l'avvenuta iscrizione in
                        <xsl:choose>
                            <xsl:when test="@fl_q2='1'">
                                lista d'attesa prioritaria
                            </xsl:when>
                            <xsl:otherwise>
                                lista d'attesa
                            </xsl:otherwise>
                        </xsl:choose>
                    </b>
                    (posizione n. <b>
                        <xsl:value-of select="iscritto/@ni_posizione" />
                    </b>)
                    per il seguente evento formativo:
                </xsl:when>
                <xsl:when test="iscritto/@ac_statoiscrizione='LAS'">
                    Con la presente ti
                    <b>
                        confermiamo l'avvenuta iscrizione in lista d'attesa secondaria
                    </b>
                    (posizione n. <b>
                        <xsl:value-of select="iscritto/@ni_posizione" />
                    </b>)
                    per il seguente evento formativo:
                </xsl:when>
            </xsl:choose>
        </div>
        <div>
            <br/>
        </div>
        <!-- dati evento -->
        <xsl:call-template name="dati_evento" />
        <div>
            <br/>
        </div>
        <!-- ulteriori informazioni -->
        <div>
            <xsl:choose>
                <xsl:when test="iscritto/@ac_statoiscrizione='I'">
                    <b style="color:red;">
                        Se sei iscritto ad un corso residenziale o ad un webinar:<br /><br />
                        La presente mail non garantisce la partecipazione effettiva al corso.<br/>
                        Attendere la conferma da parte del nostro Staff tramite mail automatica alla chiusura delle iscrizioni.<br/>
                        <br/>
                    </b>
                    <b>Se decidi di rinunciare alla partecipazione</b>, ti raccomandiamo di cancellare la tua iscrizione
                    mediante il portale entro il
                    <xsl:call-template name="data_it_mmmm">
                        <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>
                    per agevolare l'organizzazione dell'evento e per cedere il tuo posto ad eventuali partecipanti in lista d'attesa.<br/><br />
                    <b style="color:red;">
                        Se sei iscritto ad un corso FAD ASINCRONO:<br /><br />
                        La presente mail garantisce automaticamente la partecipazione effettiva al corso.
                    </b>
                </xsl:when>
                <xsl:when test="iscritto/@ac_statoiscrizione='LAP'">
                    Se, prima della chiusura delle iscrizioni (prevista per il
                    <xsl:call-template name="data_it_mmmm">
                        <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>) la lista d'attesa verrà smaltita e la tua iscrizione verrà accettata, riceverai una mail di notifica.
                    In caso contrario riceverai una mail di mancata accettazione dopo la chiusura delle iscrizioni.<br/>
                    <b>Se decidi di rinunciare alla partecipazione</b>, ti raccomandiamo di cancellare la tua iscrizione
                    mediante il portale entro il
                    <xsl:call-template name="data_it_mmmm">
                        <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>
                    per agevolare l'organizzazione dell'evento e per cedere il tuo posto ad eventuali altri partecipanti in lista d'attesa.
                </xsl:when>
                <xsl:when test="iscritto/@ac_statoiscrizione='LAS'">
                    Dopo la chiusura delle iscrizioni (prevista per il
                    <xsl:call-template name="data_it_mmmm">
                        <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>) ti verrà comunicato via e-mail se la tua iscrizione è stata accettata o rifiutata.<br/>
                    <b>Se decidi di rinunciare alla partecipazione</b>, ti raccomandiamo di cancellare la tua iscrizione
                    mediante il portale entro il
                    <xsl:call-template name="data_it_mmmm">
                        <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>
                    per agevolare l'organizzazione dell'evento e per cedere il tuo posto ad eventuali altri partecipanti in lista d'attesa.
                </xsl:when>
            </xsl:choose>
        </div>
        <div>
            <br/>
        </div>
        <div>
            Ti ricordiamo che il portale è disponibile all'indirizzo<br/>
            <a>
                <xsl:attribute name="href">
                    <xsl:value-of select="$baseurl"/>
                </xsl:attribute>
                <xsl:value-of select="$baseurl"/>
            </a>
        </div>
        <div style="background-color: #FBD603;">
            Per restare aggiornati sui nostri corsi, sulle scadenze delle iscrizioni e dei test seguici su twitter <a href="https://twitter.com/fizsler" target="_blank">@fizsler</a>,<strong>#izsler</strong>, <strong>#formazioneizsler</strong>
        </div>
    </xsl:template>

    <xsl:template name="dati_evento">
        <div>
            <b>
                <xsl:value-of select="@tx_titolo"/>
                <xsl:if test="@tx_edizione!=''">
                    <xsl:value-of select="' - '"/>
                    <xsl:value-of select="@tx_edizione"/>
                </xsl:if>
            </b>
        </div>
        <div>
            Data/date:
            <b>
                <xsl:call-template name="dataDalAl_it_mmmm">
                    <xsl:with-param name="dataDal" select="@dt_inizio" />
                    <xsl:with-param name="dataAl" select="@dt_fine" />
                </xsl:call-template>
            </b>
        </div>
        <div>
            Sede:
            <b>
                <xsl:value-of select="@tx_sede"/>
                <xsl:if test="@tx_dettaglisede!=''">
                    <xsl:value-of select="' - '"/>
                    <xsl:value-of select="@tx_dettaglisede"/>
                </xsl:if>
            </b>
            <br/>
            <xsl:value-of select="@tx_indirizzosede_br" disable-output-escaping="yes" />
        </div>
    </xsl:template>

</xsl:stylesheet>
