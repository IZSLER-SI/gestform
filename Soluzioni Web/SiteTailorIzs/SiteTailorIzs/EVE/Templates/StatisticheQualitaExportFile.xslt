<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="statistica" />
  </xsl:template>


  <xsl:template match="statistica">
    <table cellspacing="0" cellpadding="0" border="0" class="rtable">

      <tr>
        <td class="lbl">
          Numero iscritti
        </td>
        <td class="val">
          <xsl:call-template name="ni">
            <xsl:with-param name="n" select="@totiscritti" />
          </xsl:call-template>
        </td>
        <td class="exp">
          esclusi relatori, docenti, etc.
        </td>
      </tr>

      <tr>
        <td class="lbl">
          Numero questionari inseriti
        </td>
        <td class="val">
          <xsl:call-template name="ni">
            <xsl:with-param name="n" select="@totquestionari" />
          </xsl:call-template>
        </td>

      </tr>

      <tr>
        <td class="lbl">
          Rilevanza degli argomenti trattati
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_rilevanza" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>

      <tr>
        <td class="lbl">
          Qualità educativa dell'evento
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_qualita" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>

      <tr>
        <td class="lbl">
          Utilità dell'evento
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_utilita" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>

      <xsl:if test="@tx_sponsor!=''">
        <tr>
          <td class="lbl">
            Influenza dello sponsor
          </td>
          <td class="val">
            <xsl:call-template name="nd">
              <xsl:with-param name="n" select="@nd_influenzasponsor" />
            </xsl:call-template>
          </td>
          <td class="exp">
            Media (min 1, max 5)
          </td>
        </tr>
      </xsl:if>

      <tr>
        <td class="lbl">
          Capacità espositiva dei docenti
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_capacitaesposizione" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>
      <xsl:for-each select="relatore">
        <tr>
          <td class="lbl indent">
            <xsl:value-of select="@tx_cognome"/>
            <xsl:value-of select="' '"/>
            <xsl:value-of select="@tx_nome"/>
          </td>
          <td class="val" style="font-weight:normal;">
            <xsl:call-template name="nd">
              <xsl:with-param name="n" select="@nd_capacitaesposizione" />
            </xsl:call-template>
          </td>

        </tr>
      </xsl:for-each>

      <tr>
        <td class="lbl">
          Soddisfazione aspettative argomenti trattati
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_soddisfazione" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>

      <tr>
        <td class="lbl">
          Valutazione materiale di supporto
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_materiale" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>

      <tr>
        <td class="lbl">
          Idoneità e funzionalità delle infrastrutture
        </td>
        <td class="val">
          <xsl:call-template name="nd">
            <xsl:with-param name="n" select="@nd_infrastrutture" />
          </xsl:call-template>
        </td>
        <td class="exp">
          Media (min 1, max 5)
        </td>
      </tr>


      <tr>
        <td class="lbl">
          Consiglierebbe il corso ad altri colleghi
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          Sì
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_consigliacolleghi_si" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          No
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_consigliacolleghi_no" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          Non risponde
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_consigliacolleghi_na" />
          </xsl:call-template>
        </td>

      </tr>


      <tr>
        <td class="lbl">
          Problemi causati da durata o orari
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          Sì
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_problemiorario_si" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          No
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_problemiorario_no" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          Non risponde
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_problemiorario_na" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl">
          Frequenterebbe altri corsi
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          Sì
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_frequentaaltricorsi_si" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          No
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_frequentaaltricorsi_no" />
          </xsl:call-template>
        </td>

      </tr>
      <tr>
        <td class="lbl indent">
          Non risponde
        </td>
        <td class="val">
          <xsl:call-template name="np">
            <xsl:with-param name="n" select="@nd_frequentaaltricorsi_na" />
          </xsl:call-template>
        </td>

      </tr>
    </table>
  </xsl:template>

  <xsl:template name="ni">
    <xsl:param name="n" />
    <xsl:choose>
      <xsl:when test="$n!=''">
        <xsl:call-template name="number_xdec">
          <xsl:with-param name="number" select="$n" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'-'"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="nd">
    <xsl:param name="n" />
    <xsl:choose>
      <xsl:when test="$n!=''">
        <xsl:call-template name="number_2dec">
          <xsl:with-param name="number" select="$n" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'-'"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="np">
    <xsl:param name="n" />
    <xsl:choose>
      <xsl:when test="$n!=''">
        <xsl:call-template name="number_2dec">
          <xsl:with-param name="number" select="$n" />
        </xsl:call-template>
        <xsl:value-of select="' %'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'-'"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>