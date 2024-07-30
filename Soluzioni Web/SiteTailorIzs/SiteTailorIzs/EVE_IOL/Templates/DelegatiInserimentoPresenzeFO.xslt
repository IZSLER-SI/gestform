<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">
    <table class="fieldtable">
      <tr>
        <td class="labelcol bordertop">
          Numero di giorni dopo il termine
          dell'evento entro il quale deve essere completato il caricamento dei fogli firme
        </td>
        <td class="datacol bordertop">
          <asp:DropDownList ID="ddn_ni_GIORNIDOPOFINEEVE_INSPRESFO" runat="server" CssClass="ddn ddnnarrow">
            <asp:ListItem Text="1" Value="1">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=1">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="2" Value="2">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=2">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="3" Value="3">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=3">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="4" Value="4">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=4">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="5" Value="5">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=5">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="6" Value="6">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=6">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="7" Value="7">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=7">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="8" Value="8">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=8">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="9" Value="9">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=9">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="10" Value="10">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=10">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="11" Value="11">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=11">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="12" Value="12">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=12">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="13" Value="13">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=13">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
            <asp:ListItem Text="14" Value="14">
              <xsl:if test="@ni_giornidopofineeve_inspresfo=14">
                <xsl:attribute name="Selected">true</xsl:attribute>
              </xsl:if>
            </asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="errorcol bordertop">
          <xsl:call-template name="nbsp" />
        </td>
      </tr>
      <tr>
        <td class="labelcol">
          RS / Docenti / Relatori abilitati al caricamento dei fogli firme
        </td>
        <td class="datacol">
          <xsl:for-each select="iscritto">
            <div>
              <asp:CheckBox runat="server" CssClass="chk">
                <xsl:attribute name="ID">
                  <xsl:value-of select="'chkIscritto_'"/>
                  <xsl:value-of select="@id_persona"/>
                </xsl:attribute>
                <xsl:attribute name="Text">
                  <xsl:value-of select="@tx_persona"/>
                </xsl:attribute>
                <xsl:if test="@fl_attivo=1">
                  <xsl:attribute name="Checked">true</xsl:attribute>
                </xsl:if>
              </asp:CheckBox>
            </div>
          </xsl:for-each>
        </td>
        <td class="errorcol">
          <xsl:call-template name="nbsp" />
        </td>
      </tr>
      <tr>
        <td class="labelcol">
          Altre persone abilitate al caricamento dei fogli firme
        </td>
        <td class="datacol">
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="1" />
            <xsl:with-param name="val" select="noniscritto[1]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="2" />
            <xsl:with-param name="val" select="noniscritto[2]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="3" />
            <xsl:with-param name="val" select="noniscritto[3]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="4" />
            <xsl:with-param name="val" select="noniscritto[4]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="5" />
            <xsl:with-param name="val" select="noniscritto[5]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="6" />
            <xsl:with-param name="val" select="noniscritto[6]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="7" />
            <xsl:with-param name="val" select="noniscritto[7]" />
          </xsl:call-template>
          <xsl:call-template name="noniscritto">
            <xsl:with-param name="num" select="8" />
            <xsl:with-param name="val" select="noniscritto[8]" />
          </xsl:call-template>

        </td>
        <td class="errorcol">
          <xsl:call-template name="nbsp" />
        </td>
      </tr>
    </table>

  </xsl:template>

  <xsl:template name="noniscritto">
    <xsl:param name="num" />
    <xsl:param name="val" />
    <div>

      <span style="display: none;">
        <asp:TextBox runat="server" ClientIDMode="Static">
          <xsl:attribute name="ID">
            <xsl:value-of select="'noniscrittoid_'"/>
            <xsl:value-of select="$num"/>
          </xsl:attribute>
          <xsl:if test="count($val/@id_persona)=1">
            <xsl:attribute name="Text">
              <xsl:value-of select="$val/@id_persona"/>
            </xsl:attribute>
          </xsl:if>
        </asp:TextBox>
      </span>
      <asp:TextBox runat="server" EnableViewState="False" ReadOnly="True" CssClass="txt" Width="365px" ClientIDMode="Static">
        <xsl:attribute name="ID">
          <xsl:value-of select="'noniscrittonome_'"/>
          <xsl:value-of select="$num"/>
        </xsl:attribute>
      </asp:TextBox>
      <img src="../Img/icoLens.gif" class="btnicon" title="Seleziona Persona">
        <xsl:attribute name="onclick">
          <xsl:value-of select="'selectPartecipante('" />
          <xsl:value-of select="$num" />
          <xsl:value-of select="');'" />
        </xsl:attribute>
      </img>
      <img src="../Img/icoDelete.gif" class="btnicon" title="Rimuovi Persona">
        <xsl:attribute name="onclick">
          <xsl:value-of select="'clearPartecipante('" />
          <xsl:value-of select="$num" />
          <xsl:value-of select="');'" />
        </xsl:attribute>

      </img>
      
      
      
      
    </div>
  </xsl:template>
</xsl:stylesheet>
