<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="region" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <xsl:choose>
      <xsl:when test="count(iscritto)=0">
        <!-- CANCELLAZIONE ISCRIZIONE -->
        <div class="title blue">
          CANCELLAZIONE DELL'ISCRIZIONE
        </div>
        <div class="top20 green">
          <b>La tua iscrizione è stata correttamente cancellata.</b>
        </div>
        <div class="top20" style="font-size:14px;">
          <span class="btnlink btnlink_blue" onclick="location.reload();">Chiudi</span>
        </div>
      </xsl:when>
      <!-- iscrizione effettuata -->
      <xsl:otherwise>
        <div class="title blue bottom20">
          ISCRIZIONE ALL'EVENTO
        </div>
        <div class="top20">
            <!-- sei iscritto e puoi cancellarti -->
            <div>
              <xsl:choose>
                <xsl:when test="iscritto/@ac_statoiscrizione='I'">
                  <b class="green">La tua iscrizione è stata accettata.</b>
                  <br/>
                  Ti abbiamo inviato una e-mail di conferma.
                </xsl:when>
                <xsl:when test="iscritto/@ac_statoiscrizione='LAP'">
                  <b class="orange">
                    La tua iscrizione è stata inserita
                    <xsl:choose>
                      <xsl:when test="@fl_q2='1'">
                        in lista d'attesa prioritaria
                      </xsl:when>
                      <xsl:otherwise>
                        in lista d'attesa
                      </xsl:otherwise>
                    </xsl:choose>
                    (posizione n.<xsl:value-of select="iscritto/@ni_posizione"/>).
                  </b>
                  <br/>
                  Ti abbiamo inviato una e-mail di conferma inserimento in lista d'attesa.
                  <br/>
                  Verrai <b>avvisato via e-mail</b> nel caso in cui la tua iscrizione venisse accettata.
                </xsl:when>
                <xsl:when test="iscritto/@ac_statoiscrizione='LAS'">
                  <b class="orange">
                    La tua iscrizione è stata inserita in lista d'attesa secondaria
                    (posizione n.<xsl:value-of select="iscritto/@ni_posizione"/>).
                  </b>
                  <br/>
                  Ti abbiamo inviato una e-mail di conferma inserimento in lista d'attesa.
                  <br/>
                  Verrai avvisato via e-mail
                  dopo il
                  <b>
                    <xsl:call-template name="data_it_mmmm">
                      <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                    </xsl:call-template>
                  </b>
                  nel caso in cui la tua iscrizione venisse accettata.
                </xsl:when>
              </xsl:choose>
              <br/>
              <br/>
              Se lo desideri, puoi cancellare la tua iscrizione entro il
              <b>
                <xsl:call-template name="data_it_mmmm">
                  <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
                </xsl:call-template>
              </b>.
            </div>
        </div>
        <div class="top20">
          <span class="btnlink btnlink_blue" onclick="location.reload();">Chiudi</span>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
