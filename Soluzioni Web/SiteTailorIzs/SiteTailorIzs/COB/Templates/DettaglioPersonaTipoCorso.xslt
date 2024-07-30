<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="CobCommon.xslt" />
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="root" />
  </xsl:template>

  <xsl:template match="root">
    
    <div class="container">
      <!-- tipo corso e regole -->
      <xsl:apply-templates select="tipocobbase" />
      <!-- situazione persona -->
      <xsl:apply-templates select="persona" />
      <div class="sezione">
        ► Elenco corsi frequentati o da frequentare
        <span style="font-size:12px">
          (sono escluse le partecipazioni come docente, relatore, tutor, moderatore)
        </span>
      </div>
      <table class="table" style="width:100%">
        <tr>
          <th>Tipo Corso</th>
          <th>Date</th>
          <th>Titolo</th>
          <th>Sede</th>
          <th>Interno/Esterno</th>
          <th>Stato presenza</th>
        </tr>
        <xsl:for-each select="cv">
          <tr>
            <td>
              <b>
                <xsl:value-of select="@tx_tipocobdett"/>
              </b>
            </td>
            <td>
              <b>
              <xsl:call-template name="dataDalAl_it">
                <xsl:with-param name="dataDal" select="@dt_inizio" />
                <xsl:with-param name="dataAl" select="@dt_fine" />
              </xsl:call-template>
              </b>
            </td>
            <td>
              <xsl:value-of select="@tx_titolo"/>
            </td>
            <td>
              <xsl:value-of select="@tx_sede"/>
            </td>
            <td>
              <xsl:value-of select="@tx_tipocorso"/>
            </td>
            <td>
              <b>
                <xsl:attribute name="style">
                  <xsl:value-of select="'color:'"/>
                  <xsl:value-of select="@ac_rgb"/>
                </xsl:attribute>
                <xsl:value-of select="@tx_status"/>
              </b>
              
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </div>
  </xsl:template>

  <xsl:template match="tipocobbase">
    <div class="sezione">
      ► Regole frequentazione
      <xsl:value-of select="@tx_tipocobbase"/>
    </div>
    <div class="regole">
      <xsl:choose>
        <xsl:when test="count(@ac_modalitacontrollo)=0">
          <div class="red">
            Per questa tipologia di corso non sono state definite regole.
          </div>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="regole" />
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="persona">
    <div class="sezione">
      ► Situazione di
      <xsl:value-of select="@tx_cognome"/>
      <xsl:value-of select="' '"/>
      <xsl:value-of select="@tx_nome"/>
    </div>
    <table class="table">
      <!-- data assunzione -->
      <tr>
        <td class="first">
          Data Assunzione
        </td>
        <td class="first">
          <b>
            <xsl:call-template name="dataDDMMYYYY">
              <xsl:with-param name="data" select="@dt_assunzione" />
            </xsl:call-template>
          </b>
        </td>
      </tr>
      <!-- nomina-->
      <xsl:if test="../tipocobbase/@fl_filtroruoli=1">
        <tr>
          <td>
            Data Nomina
          </td>
          <td>
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_nomina" />
              </xsl:call-template>
            </b>
          </td>
        </tr>
      </xsl:if>
      <!-- corso base -->
      <tr>
        <td>
          Stato frequentazione corso base
        </td>
        <td>
          <b>
            <xsl:attribute name="style">
              <xsl:value-of select="'color:'"/>
              <xsl:value-of select="@ac_rgbbase"/>
            </xsl:attribute>
            <xsl:value-of select="@tx_statusbase_plur"/>
          </b>
        </td>
      </tr>
      <xsl:if test="count(@dt_frequenzabase)&gt;0">
        <tr>
          <td>
            Data frequentazione corso base
          </td>
          <td>
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_frequenzabase" />
              </xsl:call-template>
            </b>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="count(@dt_limitebase)&gt;0">
        <tr>
          <td>
            Data limite frequentazione corso base
          </td>
          <td>
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_limitebase" />
              </xsl:call-template>
            </b>
          </td>
        </tr>
      </xsl:if>
      <!-- corso aggiornamento -->
      <tr>
        <td>
          Stato aggiornamento periodico
        </td>
        <td>
          <b>
            <xsl:attribute name="style">
              <xsl:value-of select="'color:'"/>
              <xsl:value-of select="@ac_rgbagg"/>
            </xsl:attribute>
            <xsl:value-of select="@tx_statusagg_plur"/>
          </b>
        </td>
      </tr>
      <xsl:if test="count(@dt_frequenzaagg)&gt;0">
        <tr>
          <td>
            Data frequentazione ultimo corso aggiornamento
          </td>
          <td>
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_frequenzaagg" />
              </xsl:call-template>
            </b>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="count(@dt_limiteagg)&gt;0">
        <tr>
          <td>
            Data limite frequentazione prossimo corso aggiornamento
          </td>
          <td>
            <b>
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_limiteagg" />
              </xsl:call-template>
            </b>
          </td>
        </tr>
      </xsl:if>
    </table>
  </xsl:template>
  
</xsl:stylesheet>