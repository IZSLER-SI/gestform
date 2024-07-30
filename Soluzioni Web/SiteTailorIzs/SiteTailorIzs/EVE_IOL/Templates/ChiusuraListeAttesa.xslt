<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />
  <xsl:template match="/">

    <div class="stl_gen_box" style="position: absolute; width: 830px; top:0px;left: 0px;">
      <div class="content padall" style="height:63px;">
        <xsl:apply-templates select="evento" mode="riepilogo" />
      </div>
    </div>

    <div class="stl_gen_box" style="position: absolute; width: 390px; top:0px;left: 836px;">
      <div class="content padall" style="height:87px;">
        <xsl:apply-templates select="evento" mode="numeri" />
      </div>
    </div>
  </xsl:template>

  <xsl:template match="evento" mode="riepilogo">
    <xsl:choose>
      <xsl:when test="count(@iol_dt_aperturaiscrizioni)=0">
        Date apertura/chiusura iscrizioni on line:
        <b>Non configurate</b>
      </xsl:when>
      <xsl:otherwise>
        <div>
          Iscrizioni on line aperte dal
          <b>
            <xsl:call-template name="dataDDMMYYYY">
              <xsl:with-param name="data" select="@iol_dt_aperturaiscrizioni" />
            </xsl:call-template>
          </b>
          al
          <b>
            <xsl:call-template name="dataDDMMYYYY">
              <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
            </xsl:call-template>
          </b>
          - stato attuale:
          <xsl:choose>
            <xsl:when test="@ac_statoaperturaiscrizioni='PREOPEN'">
              <b style="color:#ff6600">Non ancora aperte</b>
            </xsl:when>
            <xsl:when test="@ac_statoaperturaiscrizioni='OPEN'">
              <b style="color:#ff0000">Aperte</b>
            </xsl:when>
            <xsl:when test="@ac_statoaperturaiscrizioni='CLOSED'">
              <b style="color:#009900">Chiuse</b>
            </xsl:when>
          </xsl:choose>
        </div>
      </xsl:otherwise>
    </xsl:choose>
    <div>
      Invio mail chiusura liste d'attesa:
      <xsl:choose>
        <xsl:when test="@iol_dt_chiusuraliste!=''">
          <b style="color:#ff0000">
            Già effettuato in data
            <xsl:call-template name="dataDDMMYYYY">
              <xsl:with-param name="data" select="@iol_dt_chiusuraliste" />
            </xsl:call-template>
          </b>
        </xsl:when>
        <xsl:otherwise>
          <b style="color:#009900">Non ancora effettuato</b>
        </xsl:otherwise>
      </xsl:choose>
    </div>
    <div>
      <xsl:choose>
        <xsl:when test="count(@iol_ni_maxpartecipanti)&gt;0">
          Evento a numero chiuso (massimo <b>
            <xsl:value-of select="@iol_ni_maxpartecipanti"/>
          </b> partecipanti)
        </xsl:when>
        <xsl:otherwise>
          Evento <b>non</b> a numero chiuso.
        </xsl:otherwise>
      </xsl:choose>
    </div>
    <div>
      <b>
        <a class="classicA" href="CriteriAccesso.aspx">
          Visualizza/modifica date e criteri per l'accesso
        </a>
      </b>
    </div>
  </xsl:template>

  <xsl:template match="evento" mode="numeri">
    <table class="numtable">
        <xsl:choose>
          <xsl:when test="count(@iol_ni_maxpartecipanti)&gt;0">
            <xsl:call-template name="maxiscritti_iscritti_residui" />
            <tr>
              <td class="int">Lista Attesa Prioritaria</td>
              <td class="num">
                <xsl:value-of select="@ni_listaattesaprioritaria"/>
              </td>
              <td class="not">
                <xsl:call-template name="nbsp" />
              </td>
            </tr>
            <tr>
              <td class="int">Lista Attesa Secondaria</td>
              <td class="num">
                <xsl:value-of select="@ni_listaattesasecondaria"/>
              </td>
              <td class="not">
                <xsl:call-template name="nbsp" />
              </td>
            </tr>
          </xsl:when>
          <xsl:otherwise>
            <tr>
              <td class="int">Numero Iscritti</td>
              <td class="num">
                <xsl:value-of select="@ni_iscritti"/>
              </td>
              <td class="not">
                <xsl:call-template name="nbsp" />
              </td>
            </tr>
          </xsl:otherwise>
        </xsl:choose>
    </table>
  </xsl:template>

  <xsl:template name="maxiscritti_iscritti_residui">
    <tr>
      <td class="int">Numero Massimo Partecipanti</td>
      <td class="num">
        <xsl:value-of select="@iol_ni_maxpartecipanti"/>
      </td>
      <td class="not">
        <xsl:call-template name="nbsp" />
      </td>
    </tr>
    <tr>
      <td class="int">Numero Iscritti</td>
      <td class="num">
        <xsl:value-of select="@ni_iscritti"/>
      </td>
      <td class="not">
        <xsl:call-template name="nbsp" />
      </td>
    </tr>
    <tr>
      <td class="int">Posti disponibili</td>
      <td class="num">
        <xsl:choose>
          <xsl:when test="@ni_rimanenti&gt;0">
            <span style="color:#009900">
              <xsl:value-of select="@ni_rimanenti"/>
            </span>
          </xsl:when>
          <xsl:when test="@ni_rimanenti=0">
            <span style="color:#ff6600">
              <xsl:value-of select="@ni_rimanenti"/>
            </span>
          </xsl:when>
          <xsl:otherwise>
            <span style="color:#ff0000">
              <xsl:value-of select="'0'"/>
            </span>
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td class="not">
        <xsl:choose>
          <xsl:when test="@ni_rimanenti=-1">
            <span style="color:#ff0000">
              <b>1</b> esubero
            </span>
          </xsl:when>
          <xsl:when test="@ni_rimanenti&lt;-1">
            <span style="color:#ff0000">
              <b>
                <xsl:value-of select="-@ni_rimanenti"/>
              </b>
              esuberi
            </span>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="nbsp" />
          </xsl:otherwise>
        </xsl:choose>

      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>