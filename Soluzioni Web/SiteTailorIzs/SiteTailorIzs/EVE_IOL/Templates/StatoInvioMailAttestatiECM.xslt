<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove" xmlns:ajaxToolkit="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="mode" />
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="$mode='todo'">
        <table cellspacing="0" cellpadding="0" border="0" class="mytable">
          <tr>
            <td class="mylabel">
              Le seguenti persone <b>RICEVERANNO</b> la mail di notifica:
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(persone/persona[count(@tx_email)=1])&gt;0">
                    <xsl:apply-templates select="persone/persona[count(@tx_email)=1]" mode="todo" />    
                  </xsl:when>
                  <xsl:otherwise>
                    <div>[nessuno]</div>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </td>
          </tr>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" class="mytable">
          <tr>
            <td class="mylabel">
              Le seguenti persone <b>NON RICEVERANNO</b> la mail di notifica in quanto in anagrafica non è presente un indirizzo e-mail:
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(persone/persona[count(@tx_email)=0])&gt;0">
                    <xsl:apply-templates select="persone/persona[count(@tx_email)=0]" mode="todo" />
                  </xsl:when>
                  <xsl:otherwise>
                    <div>[nessuno]</div>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </td>
          </tr>
        </table>
      </xsl:when>
      <xsl:when test="$mode='done'">
        <table cellspacing="0" cellpadding="0" border="0" class="mytable">
          <tr>
            <td class="mylabel">
              Le seguenti persone <b>HANNO RICEVUTO</b> la mail di notifica:
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(persone/persona[@fl_inviomailattecmok='1'])&gt;0">
                    <xsl:apply-templates select="persone/persona[@fl_inviomailattecmok='1']" mode="done" />
                  </xsl:when>
                  <xsl:otherwise>
                    <div>[nessuno]</div>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </td>
          </tr>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" class="mytable">
          <tr>
            <td class="mylabel">
              Le seguenti persone <b>NON HANNO RICEVUTO</b> la mail di notifica:
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(persone/persona[@fl_inviomailattecmok='0'])&gt;0">
                    <xsl:apply-templates select="persone/persona[@fl_inviomailattecmok='0']" mode="done" />
                  </xsl:when>
                  <xsl:otherwise>
                    <div>[nessuno]</div>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
              <div style="padding-top:3px;">
                <asp:Button runat="server" ID="lnkInvioUlteriore" Text="Effettua invio ulteriore per questi nominativi" CssClass="btnlink">
                  <xsl:attribute name="enabled">
                    <xsl:choose>
                      <xsl:when test="count(persone/persona[@fl_inviomailattecmok='0'])">
                        <xsl:value-of select="'True'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="'False'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </asp:Button>
                <ajaxToolkit:ConfirmButtonExtender runat="server" ID="cnfInvioUlteriore" TargetControlID="lnkInvioUlteriore"
                    ConfirmText="Confermi l'invio delle mail?" />
                <xsl:if test="count(persone/persona[@fl_inviomailattecmok='0'])">
                  <div style="padding-top:3px;">
                    <b>ATTENZIONE:</b> L'invio delle mail può richiedere parecchio tempo (anche qualche
                    minuto). Fai clic una volta sola sul pulsante e attendi il termine dell'operazione.
                  </div>
                </xsl:if>
              
              </div>
              
            </td>
          </tr>
        </table>

      </xsl:when>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="persona" mode="todo">
    <div>
      <b>
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="', '"/>
        <xsl:value-of select="@tx_nome"/>
      </b>
      <xsl:if test="@ac_matricola!=''">
        <xsl:value-of select="' ('"/>
        <xsl:value-of select="@ac_matricola"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:if test="@tx_email!=''">
        <span class="mymail">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_email"/>
        </span>
      </xsl:if>
    </div>
  </xsl:template>

  <xsl:template match="persona" mode="done">
    <div>
      <b>
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="', '"/>
        <xsl:value-of select="@tx_nome"/>
      </b>
      <xsl:if test="@ac_matricola!=''">
        <xsl:value-of select="' ('"/>
        <xsl:value-of select="@ac_matricola"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:if test="@tx_email!=''">
        <span class="mymail">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_email"/>
        </span>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="@fl_inviomailattecmok='1'">
          <!-- mail inviata -->
          <span class="mymail">
            <xsl:value-of select="' - invio: '"/>
            <xsl:call-template name="dataoraDDMMYYHHMM">
              <xsl:with-param name="dataora" select="@dt_inviomailattecm" />
            </xsl:call-template>
          </span>
        </xsl:when>
        <xsl:otherwise>
          <!-- mail non inviata -->
          <xsl:choose>
            <xsl:when test="@dt_inviomailattecm!=''">
              <!-- non inviata per problemi -->
              <xsl:choose>
                <xsl:when test="@tx_email!=''">
                  <xsl:value-of select="' - '"/>
                  <span style="color:#cc0000">Indirizzo e-mail non valido</span>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="' - '"/>
                  <span style="color:#cc0000">Indirizzo e-mail mancante</span>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <!-- non inviata perchè non ancora inviata :) -->
              <xsl:value-of select="' - '"/>
              <span style="color:#ff6600">Crediti acquisiti dopo ultimo invio</span>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>

    </div>
  </xsl:template>
</xsl:stylesheet>