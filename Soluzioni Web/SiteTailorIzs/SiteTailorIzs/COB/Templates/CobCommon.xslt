<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />

  <xsl:template name="regole">
    <div>
      Figure coinvolte:
      <b>
        <xsl:choose>
          <xsl:when test="@fl_filtroruoli=0">
            Tutti i dipendenti
          </xsl:when>
          <xsl:otherwise>
            <xsl:for-each select="ruolo">
              <xsl:value-of select="@tx_ruolo"/>
              <xsl:if test="position()!=last()">
                <xsl:value-of select="', '"/>
              </xsl:if>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>
      </b>
    </div>
    <xsl:choose>
      <xsl:when test="@ac_modalitacontrollo='DATA_NOM_A'">
        <div>
          Il corso base deve essere frequentato
          <b>
            <xsl:choose>
              <xsl:when test="@ni_offset1=0">
                entro la data di nomina
              </xsl:when>
              <xsl:when test="@ni_offset1&gt;0">
                entro <xsl:value-of select="@ni_offset1"/> giorni dalla data di nomina
              </xsl:when>
              <xsl:when test="@ni_offset1&lt;0">
                almeno <xsl:value-of select="-@ni_offset1"/> giorni prima della data di nomina
              </xsl:when>
            </xsl:choose>
          </b>
        </div>
        <div>
          <xsl:choose>
            <xsl:when test="@ni_mesivalidita_base=0">
              <b>Insieme al corso base</b> deve essere frequentato il primo corso di aggiornamento
            </xsl:when>
            <xsl:otherwise>
              Il corso base ha validità
              <b>
                <xsl:call-template name="mesianni">
                  <xsl:with-param name="mesi">
                    <xsl:value-of select="@ni_mesivalidita_base"/>
                  </xsl:with-param>
                </xsl:call-template>
              </b>
            </xsl:otherwise>
          </xsl:choose>
        </div>
        <div>
          I corsi di aggiornamento hanno validità
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_agg"/>
              </xsl:with-param>
            </xsl:call-template>
          </b>
        </div>
      </xsl:when>
      <xsl:when test="@ac_modalitacontrollo='DATA_NOM_B'">
        <div>
          Il corso base deve essere frequentato
          <b>
            <xsl:choose>
              <xsl:when test="@ni_offset1=0">
                entro la data di nomina
              </xsl:when>
              <xsl:when test="@ni_offset1&gt;0">
                entro <xsl:value-of select="@ni_offset1"/> giorni dalla data di nomina
              </xsl:when>
              <xsl:when test="@ni_offset1&lt;0">
                almeno <xsl:value-of select="-@ni_offset1"/> giorni prima della data di nomina
              </xsl:when>
            </xsl:choose>
          </b>
        </div>
        <div>
          I corsi di aggiornamento vengono organizzati <b>al bisogno</b>
        </div>
      </xsl:when>
      <xsl:when test="@ac_modalitacontrollo='DATA_ASS_A'">
        <div>
          I dipendenti assunti fino al
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data1" />
            </xsl:call-template>
          </b>
          sono in regola fino al
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data2" />
            </xsl:call-template>
          </b>
        </div>
        <div>
          I dipendenti assunti dopo il
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data1" />
            </xsl:call-template>
          </b>
          sono in regola per
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_base"/>
              </xsl:with-param>
            </xsl:call-template>
          </b> a partire dalla data di assunzione
        </div>
        <div>
          I corsi di aggiornamento hanno validità
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_agg"/>
              </xsl:with-param>
            </xsl:call-template>
          </b>
        </div>
      </xsl:when>
      <xsl:when test="@ac_modalitacontrollo='DATA_ASS_B'">
        <div>
          I dipendenti assunti fino al
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data1" />
            </xsl:call-template>
          </b>
          sono in regola fino al
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data2" />
            </xsl:call-template>
          </b>
        </div>
        <div>
          I dipendenti assunti dopo il
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data1" />
            </xsl:call-template>
          </b>
          devono frequentare il corso base
          <b>
            <xsl:choose>
              <xsl:when test="@ni_offset1=0">
                entro la data di assunzione.
              </xsl:when>
              <xsl:when test="@ni_offset1&gt;0">
                entro <xsl:value-of select="@ni_offset1"/> giorni dalla data di assunzione.
              </xsl:when>
              <xsl:when test="@ni_offset1&lt;0">
                almeno <xsl:value-of select="-@ni_offset1"/> giorni prima della data di assunzione.
              </xsl:when>
            </xsl:choose>
          </b> La validità del corso base
          La validità del corso base è di
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_base"/>
              </xsl:with-param>
            </xsl:call-template>
          </b>
        </div>
        <div>
          I corsi di aggiornamento hanno validità
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_agg"/>
              </xsl:with-param>
            </xsl:call-template>
          </b>
        </div>
      </xsl:when>
      <xsl:when test="@ac_modalitacontrollo='DATA_ASS_C'">
        <div>
          I dipendenti devono frequentare il corso base
          <b>
            <xsl:choose>
              <xsl:when test="@ni_offset1=0">
                entro la data di assunzione
              </xsl:when>
              <xsl:when test="@ni_offset1&gt;0">
                entro <xsl:value-of select="@ni_offset1"/> giorni dalla data di assunzione
              </xsl:when>
              <xsl:when test="@ni_offset1&lt;0">
                almeno <xsl:value-of select="-@ni_offset1"/> giorni prima della data di assunzione
              </xsl:when>
            </xsl:choose>
          </b>
        </div>
        <div>
          la validità del corso base è di
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_base"/>
              </xsl:with-param>
            </xsl:call-template>
          </b>
        </div>
        <div>
          I corsi di aggiornamento hanno validità
          <b>
            <xsl:call-template name="mesianni">
              <xsl:with-param name="mesi">
                <xsl:value-of select="@ni_mesivalidita_agg"/>
              </xsl:with-param>
            </xsl:call-template>
          </b>
        </div>
      </xsl:when>
      
      <xsl:when test="@ac_modalitacontrollo='DATA_ASS_D'">
        <div>
          Il corso base deve essere frequentato
          <b>
            <xsl:choose>
              <xsl:when test="@ni_offset1=0">
                entro la data di assunzione
              </xsl:when>
              <xsl:when test="@ni_offset1&gt;0">
                entro <xsl:value-of select="@ni_offset1"/> giorni dalla data di assunzione
              </xsl:when>
              <xsl:when test="@ni_offset1&lt;0">
                almeno <xsl:value-of select="-@ni_offset1"/> giorni prima della data di assunzione
              </xsl:when>
            </xsl:choose>
          </b>
        </div>
        <div>
          I corsi di aggiornamento vengono organizzati <b>al bisogno</b>
        </div>
      </xsl:when>
    
    </xsl:choose>

  </xsl:template>

  <xsl:template name="mesianni">
    <xsl:param name="mesi" />
    <xsl:variable name="m" select="$mesi mod 12" />
    <xsl:variable name="a" select="($mesi - ($mesi mod 12)) div 12" />
    <xsl:choose>
      <xsl:when test="$a=1">
        1 anno
      </xsl:when>
      <xsl:when test="$a&gt;1">
        <xsl:value-of select="$a"/> anni
      </xsl:when>
    </xsl:choose>
    <xsl:if test="$a&gt;0 and $m&gt;0">
      e
    </xsl:if>
    <xsl:choose>
      <xsl:when test="$m=1">
        1 mese
      </xsl:when>
      <xsl:when test="$m&gt;1">
        <xsl:value-of select="$a"/> mesi
      </xsl:when>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>