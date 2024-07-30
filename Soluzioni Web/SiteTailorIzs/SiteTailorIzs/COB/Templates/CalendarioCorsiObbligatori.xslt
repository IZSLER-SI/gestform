<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="CobCommon.xslt" />
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="root" />
  </xsl:template>

  <xsl:template match="root">
    <div id="cob">
      <xsl:apply-templates select="tipobase" />
    </div>
  </xsl:template>

  <xsl:template match="tipobase">
    <div class="head" data-expanded="0">
      <xsl:attribute name="data-groupid">
        <xsl:value-of select="@ac_tipocobbase"/>
      </xsl:attribute>
      <div class="arrow">
        ►
      </div>
      <div class="title">
        <xsl:value-of select="@tx_tipocobbase"/>
      </div>
      <div class="next">
        <div>
          Persone coinvolte:
          <b>
            <xsl:value-of select="@ni_persone"/>
          </b>
        </div>
        <div>
          Prossimo corso:
          <b>
            <xsl:attribute name="class">
              <xsl:choose>
                <xsl:when test="count(@dt_prossimadata)=0">
                  <xsl:value-of select="'orange'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="@fl_prossimadatascaduta=1">
                      <xsl:value-of select="'red'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'green'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:choose>
              <xsl:when test="count(@dt_prossimadata)=0">
                Data non calcolabile
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="@fl_prossimadatascaduta=1">
                    Termini scaduti
                  </xsl:when>
                  <xsl:otherwise>
                    entro il
                    <xsl:call-template name="data_it">
                      <xsl:with-param name="data" select="@dt_prossimadata" />
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </b>
        </div>
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>
    <div class="body">
      <xsl:attribute name="data-groupid">
        <xsl:value-of select="@ac_tipocobbase"/>
      </xsl:attribute>
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
      <xsl:apply-templates select="tipodett[@fl_periodico=0]" />
      <div class="tipo">
        Stato frequentazione corso base
      </div>
      <table>
        <tr>
          <th>
            Situazione
          </th>
          <th>
            N.Persone
          </th>
        </tr>
        <xsl:for-each select="statusbase">
          <tr>
            <td>
              <b>
                <xsl:attribute name="style">
                  <xsl:value-of select="'color:'"/>
                  <xsl:value-of select="@ac_rgb"/>
                </xsl:attribute>
                <xsl:value-of select="@tx_statusbase_plur"/>
              </b>
            </td>
            <td style="text-align:right;font-weight:bold;">
              <xsl:value-of select="@ni_persone"/>
              <xsl:call-template name="detail">
                <xsl:with-param name="key" select="'sb'" />
                <xsl:with-param name="value" select="@ac_statusbase" />
              </xsl:call-template>
            </td>
          </tr>
        </xsl:for-each>
      </table>
      <xsl:apply-templates select="tipodett[@fl_periodico=1]" />
      <div class="tipo">
        Stato aggiornamento periodico
      </div>
      <table>
        <tr>
          <th>
            Situazione
          </th>
          <th>
            N.Persone
          </th>
        </tr>
        <xsl:for-each select="statusagg">
          <tr>
            <td>
              <b>
                <xsl:attribute name="style">
                  <xsl:value-of select="'color:'"/>
                  <xsl:value-of select="@ac_rgb"/>
                </xsl:attribute>
                <xsl:value-of select="@tx_statusagg_plur"/>
              </b>
            </td>
            <td style="text-align:right;font-weight:bold;">
              <xsl:value-of select="@ni_persone"/>
              <xsl:call-template name="detail">
                <xsl:with-param name="key" select="'sa'" />
                <xsl:with-param name="value" select="@ac_statusagg" />
              </xsl:call-template>
            </td>
          </tr>
        </xsl:for-each>
      </table>
      <xsl:if test="count(scadagg)&gt;0">
        <div class="tipo">
          Prossime scadenze aggiornamento periodico
        </div>
        <table>
          <tr>
            <th>
              Mese /anno
            </th>
            <th>
              N.Persone
            </th>
          </tr>
          <xsl:for-each select="scadagg">
            <tr>
              <td>
                <b>
                  <xsl:choose>
                    <xsl:when test="@ni_mese=1">Gen</xsl:when>
                    <xsl:when test="@ni_mese=2">Feb</xsl:when>
                    <xsl:when test="@ni_mese=3">Mar</xsl:when>
                    <xsl:when test="@ni_mese=4">Apr</xsl:when>
                    <xsl:when test="@ni_mese=5">Mag</xsl:when>
                    <xsl:when test="@ni_mese=6">Giu</xsl:when>
                    <xsl:when test="@ni_mese=7">Lug</xsl:when>
                    <xsl:when test="@ni_mese=8">Ago</xsl:when>
                    <xsl:when test="@ni_mese=9">Set</xsl:when>
                    <xsl:when test="@ni_mese=10">Ott</xsl:when>
                    <xsl:when test="@ni_mese=11">Nov</xsl:when>
                    <xsl:when test="@ni_mese=12">Dic</xsl:when>
                  </xsl:choose>
                  <xsl:value-of select="' '"/>
                  <xsl:value-of select="@ni_anno"/>
                </b>
              </td>
              <td style="text-align:right;font-weight:bold;">
                <xsl:value-of select="@ni_persone"/>
                <xsl:call-template name="detail">
                  <xsl:with-param name="key" select="'ma'" />
                  <xsl:with-param name="value">
                    <xsl:value-of select="@ni_mese"/>
                    <xsl:value-of select="'_'"/>
                    <xsl:value-of select="@ni_anno"/>
                  </xsl:with-param>
                </xsl:call-template>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </xsl:if>
    </div>
  </xsl:template>

  <xsl:template match="tipodett">
    <div class="tipo">
      Calendario Corsi <xsl:value-of select="@tx_tipocobdett"/>
    </div>
    <table style="width:100%;">
      <tr>
        <th>
          Date corso
        </th>
        <th>
          Titolo
        </th>
        <th>
          Sede
        </th>
        <th>
          Iscritti
        </th>
        <th>
          Presenti
        </th>
        <th>
          Validità dal-al
        </th>
      </tr>
      <xsl:apply-templates select="evento" />
    </table>
  </xsl:template>

  <xsl:template match="evento">
    <tr>
      <td>
        <xsl:call-template name="dataDalAl_it">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </td>
      <td>
        <xsl:value-of select="@tx_titolo"/>
        <xsl:if test="@tx_edizione!=''">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_edizione"/>
        </xsl:if>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@fl_fad=0">
            <xsl:value-of select="@tx_sede"/>
          </xsl:when>
          <xsl:otherwise>
            FAD
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="@ni_iscritti"/>
      </td>
      <td>
        <xsl:value-of select="@ni_presenti"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="../../@ac_modalitacontrollo!='DATA_NOM_B'">
            <xsl:choose>
              <xsl:when test="../@fl_periodico=0 and ../../@ni_mesivalidita_base = 0">
                -
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="dataDalAl_it">
                  <xsl:with-param name="dataDal" select="@dt_inizio" />
                  <xsl:with-param name="dataAl" select="@dt_fineval" />
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            -
          </xsl:otherwise>
        </xsl:choose>
      </td>
    </tr>
  </xsl:template>

  

  
  <xsl:template name="detail">
    <xsl:param name="key" />
    <xsl:param name="value" />
    <a class="detail">
      <xsl:attribute name="href">
        <xsl:text>javascript:openDetail('</xsl:text>
        <xsl:value-of select="../@ac_tipocobbase"/>
        <xsl:text>','</xsl:text>
        <xsl:value-of select="$key"/>
        <xsl:text>','</xsl:text>
        <xsl:value-of select="$value"/>
        <xsl:text>');</xsl:text>
      </xsl:attribute>
      <img src="../img/icoLens.gif" />
    </a>
    
  </xsl:template>
  
</xsl:stylesheet>