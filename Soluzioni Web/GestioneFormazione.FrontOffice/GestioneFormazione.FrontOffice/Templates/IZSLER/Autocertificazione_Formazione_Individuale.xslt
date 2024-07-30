<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />

  <xsl:template match="/">
    <xsl:apply-templates select="autocertificazione" />
  </xsl:template>
  
  <xsl:template match="autocertificazione">
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div style="text-align:center;">
      <b>
        DICHIARAZIONE SOSTITUTIVA DI ATTO NOTORIO
      </b>
    </div>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div>
      <xsl:choose>
        <xsl:when test="@ac_genere='F'">
          Io sottoscritta
        </xsl:when>
        <xsl:otherwise>
          Io sottoscritto
        </xsl:otherwise>
      </xsl:choose>
      <b>
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="' '"/>
        <xsl:value-of select="@tx_nome"/>
      </b>
      <xsl:choose>
        <xsl:when test="@ac_genere='F'">
          nata a
        </xsl:when>
        <xsl:otherwise>
          nato a
        </xsl:otherwise>
      </xsl:choose>
      <xsl:value-of select="@tx_comune_nascita"/>
      <xsl:if test="@ac_provincia_nascita!='EE'">
        <xsl:value-of select="' ('"/>
        <xsl:value-of select="@ac_provincia_nascita"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:value-of select="', '"/>
      residente a
      <xsl:value-of select="@tx_comune_residenza"/>
      <xsl:if test="@ac_provincia_residenza!='EE'">
        <xsl:value-of select="' ('"/>
        <xsl:value-of select="@ac_provincia_residenza"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:value-of select="', '"/>
      consapevole delle sanzioni penali, nel caso di dichiarazioni non veritiere, di formazione o uso
      di atti falsi, richiamate dall'art. 76 del D.P.R. 445 del 28.12.2000, sotto la mia responsabilità,
    </div>
    <div>
      <br/>
    </div>
    <div style="text-align:center;font-style:italic;">
      DICHIARO
    </div>
    <div>
      <br/>
    </div>
    <div>
      Che i dati da me inseriti nella procedura informatica, relativa alla formazione, come di seguito elencati sono
      veritieri e corrispondono alla documentazione autentica in mio possesso:
    </div>
    <ul>
      <li>
        Ruolo: 
        <b>
          <xsl:value-of select="@tx_categoriaecm"/>
        </b>
      </li>
      <li>
        Titolo dell'evento:
        <b>
          <xsl:value-of select="@tx_titolo"/>
        </b>
      </li>
      <li>
        Tipologia:
        <b>
          <xsl:value-of select="@tx_tipologiaevento"/>
        </b>
      </li>
      <xsl:choose>
        <xsl:when test="@fl_fad='1'">
          <li>
            Data inizio fruizione:
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_iniziofruizione" />
              </xsl:call-template>
            </b>
          </li>
          <li>
            Data fine fruizione:
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_finefruizione" />
              </xsl:call-template>
            </b>
          </li>
          <li>
            Durata totale (HH:MM):
            <b>
              <xsl:value-of select="@tx_oreminuti"/>
            </b>
          </li>
        </xsl:when>
        <xsl:otherwise>
          <li>
            Sede:
            <b>
              <xsl:value-of select="@tx_sede"/>
            </b>
          </li>
          <li>
            Data inizio:
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_inizio" />
              </xsl:call-template>
            </b>
          </li>
          <li>
            Data fine:
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_fine" />
              </xsl:call-template>
            </b>
          </li>
          <li>
            Durata totale (HH:MM):
            <b>
              <xsl:value-of select="@tx_oreminuti"/>
            </b>
          </li>
        </xsl:otherwise>
      </xsl:choose>
      
      <xsl:if test="@fl_creditiecm='1'">
        <li>
          Accreditamento ECM:
          <b>
            <xsl:choose>
              <xsl:when test="@ac_normativaecm='NONE'">
                Evento non accreditato ECM
              </xsl:when>
              <xsl:otherwise>
                Evento accreditato ECM
              </xsl:otherwise>
            </xsl:choose>
          </b>
        </li>
        <xsl:if test="@ac_normativaecm!='NONE'">
          <li>
            Crediti ECM conseguiti:
            <b>
              <xsl:choose>
                <xsl:when test="@ac_statoecm='COK'">
                  Sì
                </xsl:when>
                <xsl:when test="@ac_statoecm='CKO'">
                  No
                </xsl:when>
              </xsl:choose>
            </b>
          </li>
          <xsl:if test="@ac_statoecm='COK'">
            <li>
              Numero crediti conseguiti:
              <b>
                <xsl:call-template name="number_xdec">
                  <xsl:with-param name="number" select="@nd_creditiecm" />
                </xsl:call-template>
              </b>
            </li>
            <li>
              Data conseguimento crediti:
              <b>
                <xsl:call-template name="dataDDMMYYYY">
                  <xsl:with-param name="data" select="@dt_ottenimentocreditiecm" />
                </xsl:call-template>
              </b>
            </li>
          </xsl:if>
        </xsl:if>
        
      </xsl:if>
    
    </ul>
    <div>
      M'impegno, inoltre, a fornire la documentazione autentica qualora mi fosse richiesta dall'Ufficio Formazione.
    </div>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div>
      Luogo __________________________ Data ____/____/________
    </div>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div style="text-align:right;">
      Firma _____________________________________________
    </div>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div>
      <br/>
    </div>
    <div>
      Rif.
      <xsl:value-of select="@ni_numero"/>/<xsl:value-of select="@ni_anno"/>
    </div>
  </xsl:template>
  
</xsl:stylesheet>
