<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Templates/Common.xslt" />
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="lblid_event_text" />
  <xsl:param name="lblnomevent_text" />
  <xsl:param name="lblsededataevent_text" />
  <xsl:param name="lnkchangeevent_text" />
  <xsl:param name="lnkchangeevent_navigateurl" />
  <xsl:param name="lnkeventhome_visible" />
  <xsl:param name="lnkeventhome_navigateurl" />
  <xsl:param name="lnkeventdocs_visible" />
  <xsl:param name="lnkeventdocs_navigateurl" />
  <xsl:param name="lnkeventprog_visible" />
  <xsl:param name="lnkeventprog_navigateurl" />
  <xsl:param name="lnkglobalprog_visible" />
  <xsl:param name="lnkglobalprog_navigateurl" />
  <xsl:param name="lnkglobalprog_imageurl" />

  <xsl:template match="/">
    <td class="tdi">
      <!-- ID evento -->
      <span id="i_d_e_v_e" style="display:none;">
        <xsl:value-of select="$lblid_event_text"/>
      </span>
      <!-- calendario eventi: solo se attivo -->
      <xsl:if test="$lnkglobalprog_visible='true'">
        <div style="float:left;margin-right:5px;">
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="$lnkglobalprog_navigateurl"/>
            </xsl:attribute>
            <img title="Calendario Scadenze" style="border-width:0px;">
              <xsl:attribute name="src">
                <xsl:value-of select="$lnkglobalprog_imageurl"/>
              </xsl:attribute>
            </img>
          </a>
        </div>
      </xsl:if>
      <div>
        <xsl:if test="$lnkglobalprog_visible='true'">
          <xsl:attribute name="style">
            <xsl:choose>
              <xsl:when test="$lnkeventhome_visible='true'">
                <xsl:text>float:left;padding-top:4px;</xsl:text>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>float:left;padding-top:9px;</xsl:text>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:if>
        <span style="color:#ffffff;">Evento:</span>
        <xsl:call-template name="nbsp" />
        <xsl:value-of select="$lblnomevent_text" />
        <xsl:call-template name="nbsp" />
        <xsl:call-template name="nbsp" />
        <!-- link seleziona o cambia / nuovo -->
        <a class="ast_a">
          <xsl:attribute name="href">
            <xsl:value-of select="$lnkchangeevent_navigateurl"/>
          </xsl:attribute>
          <xsl:value-of select="$lnkchangeevent_text"/>
        </a>
        <!-- link documenti e scheda programmazione: li mostro se siamo abilitati anche alla programmazione eventi -->
        <xsl:if test="$lnkeventdocs_visible='true' and $lnkeventprog_visible='true'">
          <xsl:call-template name="nbsp" />
          <xsl:call-template name="nbsp" />
          <a class="ast_a" target="_blank">
            <xsl:attribute name="href">
              <xsl:value-of select="$lnkeventdocs_navigateurl"/>
            </xsl:attribute>
            <xsl:value-of select="'Docs'"/>
          </a>
        </xsl:if>
        <xsl:if test="$lnkeventprog_visible='true'">
          <xsl:call-template name="nbsp" />
          <xsl:call-template name="nbsp" />
          <a class="ast_a">
            <xsl:attribute name="href">
              <xsl:text>javascript:wopen('</xsl:text>
              <xsl:value-of select="$lnkeventprog_navigateurl"/>
              <xsl:text>','schedaprogevento',900,700,1,0,0,1,1);</xsl:text>
            </xsl:attribute>
            <xsl:value-of select="'Prog'"/>
          </a>
        </xsl:if>
        <!-- link home -->
        <xsl:if test="$lnkeventhome_visible='true'">
          <xsl:call-template name="nbsp" />
          <xsl:call-template name="nbsp" />
          <a class="ast_a">
            <xsl:attribute name="href">
              <xsl:value-of select="$lnkeventhome_navigateurl"/>
            </xsl:attribute>
            <xsl:value-of select="'Home'"/>
          </a>
        </xsl:if>
        <!-- sede e date -->
        <xsl:if test="$lnkeventhome_visible='true'">
          <br/>
          <xsl:value-of select="$lblsededataevent_text"/>
        </xsl:if>
      </div>
      <div style="clear:both;"></div>
    </td>
    <td class="tdf">
      <xsl:call-template name="nbsp" />
    </td>
  </xsl:template>
</xsl:stylesheet>
