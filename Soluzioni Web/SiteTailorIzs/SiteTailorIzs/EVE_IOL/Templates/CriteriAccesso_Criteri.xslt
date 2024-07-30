<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:asp="remove"
                xmlns:ajaxToolkit="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="companyname" />
  <xsl:template match="/">
    <xsl:apply-templates select="evento" />
  </xsl:template>

  <xsl:template match="evento">

    <table class="critable">
      <tr>
        <th class="numero">N.</th>
        <th class="dipext">Rapporto lavorativo con <xsl:value-of select="$companyname"/></th>
        <th class="profilo">Profilo</th>
        <!-- prof/disc: solo se è ECM -->
        <xsl:if test="@fl_ecm='1'">
          <th class="profdiscecm">Professioni/Discipline ECM</th>
        </xsl:if>
        <!-- destinazione: solo se c'è numero chiuso -->
        <xsl:if test="@iol_ni_maxpartecipanti!=''">
          <th class="destinazione">Destinazione</th>
        </xsl:if>
        <th class="comandi">Comandi</th>
      </tr>
      
      <xsl:apply-templates select="criterio" />
    </table>
    <div style="padding-top:10px;font-size:15px;font-weight:bold;font-family:Arial;">
      <span class="btnlink" onclick="addCriterio();">Aggiungi Criterio</span>
    </div>
    
  </xsl:template>

  <xsl:template match="criterio">
    <tr class="datatr">
      <td class="numero">
        <xsl:value-of select="@ni_ordine"/>
      </td>
      <td class="dipext">
        <xsl:choose>
          <xsl:when test="@ac_dipext='ALL'">
            Dipendente o Esterno
          </xsl:when>
          <xsl:when test="@ac_dipext='EXT'">
            Esterno
          </xsl:when>
          <xsl:when test="@ac_dipext='DIP'">
            Dipendente appartenente a qualsiasi unità operativa
          </xsl:when>
          <xsl:when test="@ac_dipext='DIP_UO'">
            Dipendente appartenente ad una delle seguenti U.O.:
            <ul class="criul">
              <xsl:for-each select="uo">
                <li>
                  <xsl:value-of select="@tx_unitaoperativa"/>
                </li>
              </xsl:for-each>
            </ul>
          </xsl:when>
        </xsl:choose>
      </td>
      <td class="profilo">
        <xsl:choose>
          <xsl:when test="@fl_profili='0'">
            Qualsiasi profilo
          </xsl:when>
          <xsl:when test="@fl_profili='1'">
            Uno dei seguenti:
            <ul class="criul">
              <xsl:for-each select="profilo">
                <li>
                  <xsl:value-of select="@tx_profilo"/>
                </li>
              </xsl:for-each>
            </ul>
          </xsl:when>
        </xsl:choose>
      </td>
      <xsl:if test="../@fl_ecm='1'">
        <td>
          <xsl:choose>
            <xsl:when test="count(@fl_prodiscecmaccr)=0">
              Qualsiasi
            </xsl:when>
            <xsl:when test="@fl_prodiscecmaccr='0'">
              Professione/discplina non presente o non accreditata per l'evento
            </xsl:when>
            <xsl:when test="@fl_prodiscecmaccr='1'">
              Solo le professioni/discipline per le quali l'evento è accreditato:
              <ul class="criul">
                <xsl:for-each select="../professionedisciplina">
                  <li>
                    <xsl:value-of select="@tx_professionedisciplina"/>
                  </li>
                </xsl:for-each>
              </ul>
            </xsl:when>
          </xsl:choose>
        </td>
      </xsl:if>
      <xsl:if test="../@iol_ni_maxpartecipanti!=''">
        <td>
          <xsl:choose>
            <xsl:when test="@ac_destinazione='ACC_Q1'">
              Accettato immediatamente o in lista d'attesa prioritaria/unica
            </xsl:when>
            <xsl:when test="@ac_destinazione='Q2'">
              Lista d'attesa secondaria
            </xsl:when>
          </xsl:choose>
        </td>
      </xsl:if>
      <!-- comandi -->
      <td class="comandi">
        <div>
          <span class="btnlink_narrow">
            <xsl:attribute name="onclick">
              <xsl:text>editCriterio('</xsl:text>
              <xsl:value-of select="@id_criterio"/>
              <xsl:text>');</xsl:text>
            </xsl:attribute>
            Modifica
          </span>
        </div>
        <div>
          <asp:LinkButton runat="server" CssClass="btnlink_narrow">
            <xsl:attribute name="ID">
              <xsl:value-of select="'lnkEliminaCriterio_'"/>
              <xsl:value-of select="@id_criterio"/>
            </xsl:attribute>
            <xsl:attribute name="CommandArgument">
              <xsl:value-of select="@id_criterio"/>
            </xsl:attribute>
            <xsl:value-of select="'Elimina'"/>
          </asp:LinkButton>
          <ajaxToolkit:ConfirmButtonExtender runat="server" ConfirmText="Confermi l'eliminazione del criterio?">
            <xsl:attribute name="ID">
              <xsl:value-of select="'cnfEliminaCriterio_'"/>
              <xsl:value-of select="@id_criterio"/>
            </xsl:attribute>
            <xsl:attribute name="TargetControlID">
              <xsl:value-of select="'lnkEliminaCriterio_'"/>
              <xsl:value-of select="@id_criterio"/>
            </xsl:attribute>
          </ajaxToolkit:ConfirmButtonExtender>
        </div>
        <xsl:if test="position()!=1 or position()!=last()">
          <div>
            <xsl:if test="position()!=1">
              <asp:LinkButton runat="server" CssClass="btnlink_narrow">
                <xsl:attribute name="ID">
                  <xsl:value-of select="'lnkMoveUpCriterio_'"/>
                  <xsl:value-of select="@id_criterio"/>
                </xsl:attribute>
                <xsl:attribute name="CommandArgument">
                  <xsl:value-of select="@id_criterio"/>
                </xsl:attribute>
                <xsl:value-of select="'Su'"/>
              </asp:LinkButton>
            </xsl:if>
            <xsl:value-of select="' '"/>
            <xsl:if test="position()!=last()">
              <asp:LinkButton runat="server" CssClass="btnlink_narrow">
                <xsl:attribute name="ID">
                  <xsl:value-of select="'lnkMoveDnCriterio_'"/>
                  <xsl:value-of select="@id_criterio"/>
                </xsl:attribute>
                <xsl:attribute name="CommandArgument">
                  <xsl:value-of select="@id_criterio"/>
                </xsl:attribute>
                <xsl:value-of select="'Giù'"/>
              </asp:LinkButton>
            </xsl:if>
          </div>
        </xsl:if>
        
        


      </td>
    </tr>
    <xsl:if test="count(conflitto)&gt;0">
      <tr>
        
        <td class="conflitto">
          <xsl:attribute name="colspan">
            <xsl:choose>
              <xsl:when test="../@fl_ecm='1' and ../@iol_ni_maxpartecipanti!=''">6</xsl:when>
              <xsl:when test="../@fl_ecm='0' and ../@iol_ni_maxpartecipanti!=''">5</xsl:when>
              <xsl:when test="../@fl_ecm='1' and count(../@iol_ni_maxpartecipanti)=0">5</xsl:when>
              <xsl:otherwise>4</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:choose>
            <xsl:when test="count(conflitto)=1">
              Criterio <xsl:value-of select="@ni_ordine"/> in conflitto con il criterio
              <xsl:value-of select="conflitto/@ni_ordine"/>
            </xsl:when>
            <xsl:otherwise>
              Criterio <xsl:value-of select="@ni_ordine"/> in in conflitto con i criteri
              <xsl:for-each select="conflitto">
                <xsl:value-of select="@ni_ordine"/>
                <xsl:if test="position()!=last()">
                  <xsl:value-of select="' / '"/>
                </xsl:if>
              </xsl:for-each>


            </xsl:otherwise>
          </xsl:choose>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>