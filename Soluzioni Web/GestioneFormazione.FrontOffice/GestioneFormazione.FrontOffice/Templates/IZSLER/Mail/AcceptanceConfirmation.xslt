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
      Con la presente siamo lieti di informarti che la tua richiesta di iscrizione al seguente evento formativo,
      che era stata inserita in lista d'attesa, è stata <b>accettata</b> in quanto si sono liberati dei posti:
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
      <b style="color:red;">
        La presente mail non garantisce la partecipazione effettiva al corso.<br/>
        Attendere la conferma da parte del nostro Staff tramite mail automatica alla chiusura delle iscrizioni.<br/>
        <br/>
      </b>
      <b>Qualora, nonostante questa conferma, decidessi di rinunciare alla partecipazione</b>, ti raccomandiamo di cancellare la tua iscrizione
      mediante il portale prima del
      <xsl:call-template name="data_it_mmmm">
        <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
      </xsl:call-template>
      per agevolare l'organizzazione dell'evento e per cedere il tuo posto ad eventuali altri partecipanti in lista d'attesa.
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
