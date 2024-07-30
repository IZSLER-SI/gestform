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
        <!-- ISCRIZIONE -->
        <div class="title blue">
          ISCRIZIONE ALL'EVENTO
        </div>
        <!-- se l'evento è ECM ma il profilo non lo è > warning -->
        <xsl:if test="@ac_normativaecm!='NONE' and @fl_profiloecm=0">
        </xsl:if>
        <div class="top20">
          <xsl:choose>
            <xsl:when test="@ac_destinazioneaccesso='ACC_Q1'">
              <xsl:choose>
                <xsl:when test="count(@iol_ni_maxpartecipanti) = 0">
                  In base ai requisiti per l'accesso,
                  la tua iscrizione verrà
                  <b class="green">
                    immediatamente accettata.
                  </b>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="@iol_ni_postidisponibili &gt; 0">
                      In base ai requisiti per l'accesso ed ai posti attualmente disponibili,
                      la tua iscrizione verrà
                      <b class="green">
                        immediatamente accettata.
                      </b>
                    </xsl:when>
                    <xsl:otherwise>
                      In base ai requisiti per l'accesso ed ai posti attualmente disponibili,
                      la tua iscrizione verrà inserita
                      <b class="orange">
                        <xsl:choose>
                          <xsl:when test="@fl_q2='1'">
                            in lista d'attesa prioritaria.
                          </xsl:when>
                          <xsl:otherwise>
                            in lista d'attesa.
                          </xsl:otherwise>
                        </xsl:choose>
                      </b>
                      <br/>
                      Verrai avvisato via e-mail qualora si liberasse un numero sufficiente di posti.
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:when test="@ac_destinazioneaccesso='Q2'">
              In base ai requisiti per l'accesso ed ai posti attualmente disponibili,
              la tua iscrizione verrà inserita
              <b class="orange">
                in lista d'attesa secondaria.
              </b>
              <br/>
              Dopo la chiusura delle iscrizioni
              (<xsl:call-template name="data_it_mmmm">
                <xsl:with-param name="data" select="@iol_dt_chiusuraiscrizioni" />
              </xsl:call-template>)
              ti comunicheremo via e-mail se la tua iscrizione è stata accettata o rifiutata.
            </xsl:when>
          </xsl:choose>
        </div>
        <div class="top20" style="font-size:14px;">
          <asp:LinkButton runat="server" ID="lnkIscrizione" CssClass="btnlink btnlink_blue" Font-Bold="true">
            <xsl:choose>
              <xsl:when test="@ac_destinazioneaccesso='ACC_Q1'">
                <xsl:choose>
                  <xsl:when test="count(@iol_ni_maxpartecipanti) = 0">
                    CONFERMA L'ISCRIZIONE
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="@iol_ni_postidisponibili &gt; 0">
                        CONFERMA L'ISCRIZIONE
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test="@fl_q2='1'">
                            CONFERMA L'ISCRIZIONE (in lista d'attesa prioritaria)
                          </xsl:when>
                          <xsl:otherwise>
                            CONFERMA L'ISCRIZIONE (in lista d'attesa)
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="@ac_destinazioneaccesso='Q2'">
                CONFERMA L'ISCRIZIONE (in lista d'attesa secondaria)
              </xsl:when>
            </xsl:choose>
          </asp:LinkButton>
          <xsl:call-template name="nbsp" />
          <xsl:call-template name="nbsp" />
          <span class="btnlink btnlink_blue" onclick="showRegistrationPopup(false);">ANNULLA</span>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="title blue bottom20">
          CANCELLAZIONE DELL'ISCRIZIONE
        </div>
        <div class="top20">
          Per confermare la cancellazione della tua iscrizione, fai clic su "CANCELLA L'ISCRIZIONE". Se invece non intendi
          cancellare la tua iscrizione, fai clic su "ANNULLA".
        </div>
        <div class="top20">
          <asp:LinkButton runat="server" ID="lnkDisiscrizione" CssClass="btnlink btnlink_blue" Font-Bold="true">
            CANCELLA L'ISCRIZIONE
          </asp:LinkButton>
          <xsl:call-template name="nbsp" />
          <xsl:call-template name="nbsp" />
          <span class="btnlink btnlink_blue" onclick="showRegistrationPopup(false);">ANNULLA</span>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
