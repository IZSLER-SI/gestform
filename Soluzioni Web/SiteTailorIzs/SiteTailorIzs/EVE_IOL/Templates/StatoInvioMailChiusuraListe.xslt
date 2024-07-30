<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="mode" />
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="$mode='todo'">
        <table class="mytable">
          <tr>
            <td class="mylabel">
              Alle seguenti persone <b>VERRÀ INVIATA</b> una mail di notifica:
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(eventi/evento[count(@tx_email)=1])&gt;0">
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='A' and count(@tx_email)=1])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='A' and count(@tx_email)=1]" mode="todo" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='B' and count(@tx_email)=1])&gt;0">
                      <div class="mygroup">
                        Notifica accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='B' and count(@tx_email)=1]" mode="todo" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='C' and count(@tx_email)=1])&gt;0">
                      <div class="mygroup">
                        Notifica mancata accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='C' and count(@tx_email)=1]" mode="todo" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='D' and count(@tx_email)=1])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione docenti / relatori / moderatori
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='D' and count(@tx_email)=1]" mode="todo" />
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <div>[nessuno]</div>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </td>
          </tr>
        </table>
        <table class="mytable">
          <tr>
            <td class="mylabel">
              Alle seguenti persone <b>NON VERRÀ INVIATA</b> una mail di notifica in quanto in anagrafica non è presente un indirizzo e-mail:
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(eventi/evento[count(@tx_email)=0])&gt;0">
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='A' and count(@tx_email)=0])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='A' and count(@tx_email)=0]" mode="todo" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='B' and count(@tx_email)=0])&gt;0">
                      <div class="mygroup">
                        Notifica accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='B' and count(@tx_email)=0]" mode="todo" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='C' and count(@tx_email)=0])&gt;0">
                      <div class="mygroup">
                        Notifica mancata accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='C' and count(@tx_email)=0]" mode="todo" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='D' and count(@tx_email)=0])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione docenti / relatori / moderatori
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='D' and count(@tx_email)=0]" mode="todo" />
                    </xsl:if>
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
        <table class="mytable">
          <tr>
            <td class="mylabel">
              Alle seguenti persone <b>È STATA INVIATA </b> la mail di notifica.<br/>
              Se desideri <b>effettuare il reinvio della mail ad una o più persone</b>, fai clic sul link "non inviata"
              accanto al/ai nominativi desiderati, e in seguito utilizza il pulsante "invia le mail delle tipologie selezionate" in fondo alla pagina.
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(eventi/evento[@fl_inviomailchiusuralisteok='1'])&gt;0">
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='A' and @fl_inviomailchiusuralisteok='1'])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='A' and @fl_inviomailchiusuralisteok='1']" mode="done" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='B' and @fl_inviomailchiusuralisteok='1'])&gt;0">
                      <div class="mygroup">
                        Notifica accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='B' and @fl_inviomailchiusuralisteok='1']" mode="done" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='C' and @fl_inviomailchiusuralisteok='1'])&gt;0">
                      <div class="mygroup">
                        Notifica mancata accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='C' and @fl_inviomailchiusuralisteok='1']" mode="done" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='D' and @fl_inviomailchiusuralisteok='1'])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione docenti / relatori / moderatori
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='D' and @fl_inviomailchiusuralisteok='1']" mode="done" />
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <div>[nessuno]</div>
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </td>
          </tr>
        </table>
        <table class="mytable">
          <tr>
            <td class="mylabel">
              Alle seguenti persone <b>NON È STATA INVIATA</b> la mail di notifica.<br/>
              Possibili motivazioni:
              <ol style="margin-top:0px;margin-botom:0px;list-style-type: lower-alpha;padding-left:30px;">
                <li>la persona è stata iscritta dopo l'ultimo invio delle mail</li>
                <li>in anagrafica non è presente un indirizzo e-mail</li>
                <li>l'indirizzo e-mail presente in anagrafica non è valido</li>
                <li>la tipologia di e-mail della persona è stata esclusa dall'invio</li>
              </ol>
            </td>
            <td class="mydata">
              <div class="mylist">
                <xsl:choose>
                  <xsl:when test="count(eventi/evento[@fl_inviomailchiusuralisteok='0'])&gt;0">
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='A' and @fl_inviomailchiusuralisteok='0'])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='A' and @fl_inviomailchiusuralisteok='0']" mode="done" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='B' and @fl_inviomailchiusuralisteok='0'])&gt;0">
                      <div class="mygroup">
                        Notifica accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='B' and @fl_inviomailchiusuralisteok='0']" mode="done" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='C' and @fl_inviomailchiusuralisteok='0'])&gt;0">
                      <div class="mygroup">
                        Notifica mancata accettazione discenti
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='C' and @fl_inviomailchiusuralisteok='0']" mode="done" />
                    </xsl:if>
                    <xsl:if test="count(eventi/evento[@ac_inviomailchiusuraliste='D' and @fl_inviomailchiusuralisteok='0'])&gt;0">
                      <div class="mygroup">
                        Promemoria partecipazione docenti / relatori / moderatori
                      </div>
                      <xsl:apply-templates select="eventi/evento[@ac_inviomailchiusuraliste='D' and @fl_inviomailchiusuralisteok='0']" mode="done" />
                    </xsl:if>
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
    </xsl:choose>

  </xsl:template>

  <xsl:template match="evento" mode="todo">
    <div class="myitem">
      <b>
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="', '"/>
        <xsl:value-of select="@tx_nome"/>
      </b>
      <xsl:if test="@ac_matricola!=''">
        <xsl:value-of select="' (matr.'"/>
        <xsl:value-of select="@ac_matricola"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:if test="@tx_email!=''">
        <span class="mymail">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_email"/>
        </span>
      </xsl:if>
      <xsl:if test="@ac_categoriaecm!='P'">
        <xsl:value-of select="' - '"/>
        <xsl:value-of select="@tx_categoriaecm"/>
      </xsl:if>
    </div>
  </xsl:template>

  <xsl:template match="evento" mode="done">
    <div class="myitem">
      <b>
        <xsl:value-of select="@tx_cognome"/>
        <xsl:value-of select="', '"/>
        <xsl:value-of select="@tx_nome"/>
      </b>
      <xsl:if test="@ac_matricola!=''">
        <xsl:value-of select="' (matr.'"/>
        <xsl:value-of select="@ac_matricola"/>
        <xsl:value-of select="')'"/>
      </xsl:if>
      <xsl:if test="@tx_email!=''">
        <span class="mymail">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_email"/>
        </span>
      </xsl:if>
      <xsl:if test="@ac_categoriaecm!='P'">
        <xsl:value-of select="' - '"/>
        <xsl:value-of select="@tx_categoriaecm"/>
      </xsl:if>
      <xsl:if test="@fl_inviomailchiusuralisteok='1'">
        <xsl:value-of select="' '"/>
        <asp:LinkButton runat="server" CssClass="classicA" Font-Bold="true">
          <xsl:attribute name="ID">
            <xsl:value-of select="'lnkReinviaMail_'"/>
            <xsl:value-of select="@id_iscritto"/>
          </xsl:attribute>
          <xsl:value-of select="'Non inviata'"/>
        </asp:LinkButton>
      </xsl:if>
    </div>
  </xsl:template>
</xsl:stylesheet>