<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove" xmlns:ajaxToolkit="remove">

  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <!-- rilevanza -->
    <div class="qst">
      <div class="num">
        1.
      </div>
      <div class="question">
        Rilevanza degli argomenti trattati
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_RILEVANZA">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_rilevanza"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_RILEVANZA" runat="server"
          TargetControlID="ni_RILEVANZA"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>

    <!-- qualità -->
    <div class="qst">
      <div class="num">
        2.
      </div>
      <div class="question">
        Qualità educativa
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_QUALITA">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_qualita"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_QUALITA" runat="server"
          TargetControlID="ni_QUALITA"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- utilità -->
    <div class="qst">
      <div class="num">
        3.
      </div>
      <div class="question">
        Utilità per la formazione / aggiornamento
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_UTILITA">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_utilita"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_UTILITA" runat="server"
          TargetControlID="ni_UTILITA"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- influenza dello sponsor -->
    <div class="qst">
      <div class="num">
        4.
      </div>
      <div class="question">
        Influenza sponsor/interessi commerciali
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_INFLUENZASPONSOR">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_influenzasponsor"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_INFLUENZASPONSOR" runat="server"
          TargetControlID="ni_INFLUENZASPONSOR"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>

    <!-- esempi influenza dello sponsor -->
    <div class="qst">
      <div class="num">
        <xsl:call-template name="nbsp" />
      </div>
      <div class="question">
        Eventuali esempi influenza sponsor
      </div>
      <div class="answerw">
        <asp:TextBox runat="server" CssClass="tmultiline" ID="tx_INFLUENZASPONSOR" TextMode="multiline">
          <xsl:attribute name="Text">
            <xsl:value-of select="@tx_influenzasponsor"/>
          </xsl:attribute>
        </asp:TextBox>
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>
 
    <!-- docenti (intro) -->
    <div class="qst">
      <div class="num">
        5.
      </div>
      <div class="questionw">
        Capacità espositiva docenti
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>

    <!-- docenti -->
    <xsl:for-each select="docente">
      <div class="qst">
        <div class="question_ind">
          <xsl:value-of select="@tx_cognome"/>
          <xsl:value-of select="' '"/>
          <xsl:value-of select="@tx_nome"/>
        </div>
        <div class="answern">
          <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1">
            <xsl:attribute name="ID">
              <xsl:value-of select="'ni_CAPACITAESPOSIZIONE_'"/>
              <xsl:value-of select="@id_persona"/>
            </xsl:attribute>
            <xsl:attribute name="Text">
              <xsl:value-of select="@ni_capacitaesposizione"/>
            </xsl:attribute>
          </asp:TextBox>
          <ajaxToolkit:FilteredTextBoxExtender  runat="server"
           FilterType="Custom"
           ValidChars="12345 ">
            <xsl:attribute name="ID">
              <xsl:value-of select="'f_ni_CAPACITAESPOSIZIONE_'"/>
              <xsl:value-of select="@id_persona"/>
            </xsl:attribute>
            <xsl:attribute name="TargetControlID">
              <xsl:value-of select="'ni_CAPACITAESPOSIZIONE_'"/>
              <xsl:value-of select="@id_persona"/>
            </xsl:attribute>
          </ajaxToolkit:FilteredTextBoxExtender>
        </div>
        <div class="suggestion">
          1-5. Spazio = non risponde
        </div>
        <div class="clear">
          <xsl:value-of select="''"/>
        </div>
      </div>
    </xsl:for-each>

    <!-- soddisfazione argomenti -->
    <div class="qst">
      <div class="num">
        6.
      </div>
      <div class="question">
        Soddisfazione aspettative argomenti trattati
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_SODDISFAZIONE">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_soddisfazione"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_SODDISFAZIONE" runat="server"
          TargetControlID="ni_SODDISFAZIONE"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- materiale di supporto -->
    <div class="qst">
      <div class="num">
        7.
      </div>
      <div class="question">
        Valutazione materiale di supporto
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_MATERIALE">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_materiale"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_MATERIALE" runat="server"
          TargetControlID="ni_MATERIALE"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- infrastrutture -->
    <div class="qst">
      <div class="num">
        8.
      </div>
      <div class="question">
        Idoneità e funzionalità infrastrutture
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_INFRASTRUTTURE">
          <xsl:attribute name="Text">
            <xsl:value-of select="@ni_infrastrutture"/>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_INFRASTRUTTURE" runat="server"
          TargetControlID="ni_INFRASTRUTTURE"
          FilterType="Custom"
          ValidChars="12345 " />
      </div>
      <div class="suggestion">
        1-5. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>
   
    <!-- consiglierebbe il corso -->
    <div class="qst">
      <div class="num">
        9.
      </div>
      <div class="question">
        Consiglierebbe il corso a colleghi
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_CONSIGLIACOLLEGHI">
          <xsl:attribute name="Text">
            <xsl:choose>
              <xsl:when test="@ni_consigliacolleghi=1">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="@ni_consigliacolleghi=0">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_CONSIGLIACOLLEGHI" runat="server"
          TargetControlID="ni_CONSIGLIACOLLEGHI"
          FilterType="Custom"
          ValidChars="sSnN " />
      </div>
      <div class="suggestion">
        S/N. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>

    <!-- problemi orario -->
    <div class="qst">
      <div class="num">
        10.
      </div>
      <div class="question">
        Problemi causati da durata e orario
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_PROBLEMIORARIO">
          <xsl:attribute name="Text">
            <xsl:choose>
              <xsl:when test="@ni_problemiorario=1">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="@ni_problemiorario=0">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_PROBLEMIORARIO" runat="server"
           TargetControlID="ni_PROBLEMIORARIO"
           FilterType="Custom"
           ValidChars="sSnN " />
      </div>
      <div class="suggestion">
        S/N. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- frequenterebbe altri corsi -->
    <div class="qst">
      <div class="num">
        11.
      </div>
      <div class="question">
        Frequenterebbe altri corsi
      </div>
      <div class="answern">
        <asp:TextBox runat="server" CssClass="tnarrow" MaxLength="1" ID="ni_FREQUENTAALTRICORSI">
          <xsl:attribute name="Text">
            <xsl:choose>
              <xsl:when test="@ni_frequentaaltricorsi=1">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="@ni_frequentaaltricorsi=0">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="f_ni_FREQUENTAALTRICORSI" runat="server"
          TargetControlID="ni_FREQUENTAALTRICORSI"
          FilterType="Custom"
          ValidChars="sSnN " />
      </div>
      <div class="suggestion">
        S/N. Spazio = non risponde
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- argomenti di interesse -->
    <div class="qst">
      <div class="num">
        <xsl:call-template name="nbsp" />
      </div>
      <div class="question">
        Argomenti di interesse
      </div>
      <div class="answerw">
        <asp:TextBox runat="server" CssClass="tmultiline" ID="tx_FREQUENTAALTRICORSI" TextMode="multiline">
          <xsl:attribute name="Text">
            <xsl:value-of select="@tx_frequentaaltricorsi"/>
          </xsl:attribute>
        </asp:TextBox>
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

    <!-- suggerimenti -->
    <div class="qst">
      <div class="num">
        <xsl:call-template name="nbsp" />
      </div>
      <div class="question">
        Suggerimenti, commenti, proposte
      </div>
      <div class="answerw">
        <asp:TextBox runat="server" CssClass="tmultiline" ID="tx_COMMENTI" TextMode="multiline">
          <xsl:attribute name="Text">
            <xsl:value-of select="@tx_commenti"/>
          </xsl:attribute>
        </asp:TextBox>
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>

    <!-- nominativo -->
    <div class="qst">
      <div class="num">
        <xsl:call-template name="nbsp" />
      </div>
      <div class="question">
        Nominativo
      </div>
      <div class="answerw">
        <asp:DropDownList runat="server" ID="id_ISCRITTO" CssClass="tddn">
          <asp:ListItem Text="[Anonimo]" Value="" />
          <asp:ListItem Text="[Firma Illeggibile]" Value="-1">
            <xsl:if test="@id_iscritto='-1'">
              <xsl:attribute name="Selected">
                <xsl:value-of select="'true'" />
              </xsl:attribute>
            </xsl:if>
          </asp:ListItem>
          <xsl:for-each select="iscritto">
            <asp:ListItem>
              <xsl:attribute name="Value">
                <xsl:value-of select="@id_iscritto"/>
              </xsl:attribute>
              <xsl:attribute name="Text">
                <xsl:value-of select="@tx_cognome"/>
                <xsl:value-of select="' '"/>
                <xsl:value-of select="@tx_nome"/>
              </xsl:attribute>
              <xsl:if test="@id_iscritto=../@id_iscritto">
                <xsl:attribute name="Selected">
                  <xsl:value-of select="'true'" />
                </xsl:attribute>
              </xsl:if>
            </asp:ListItem>

          </xsl:for-each>

        </asp:DropDownList>
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>

    </div>

  </xsl:template>


  <xsl:template name="nbsp">
    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
  </xsl:template>

</xsl:stylesheet>

