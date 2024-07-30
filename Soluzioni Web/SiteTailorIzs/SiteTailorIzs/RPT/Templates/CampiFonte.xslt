<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:c="http://schemas.softailor.com/ReportEngine/Fonte"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                exclude-result-prefixes="c"
                >
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <xsl:apply-templates select="c:Fonte" />
  </xsl:template>

  <xsl:template match="c:Fonte">
    <div class="title">
      <b>
        <xsl:value-of select="@Descrizione"/>
      </b> - elenco campi
    </div>
    <!-- intestazione: c'è sempre -->
    <div class="section">Campi Intestazione</div>
    <xsl:call-template name="campo">
      <xsl:with-param name="d" select="'Data di stampa (gg/mm/aaaa)'" />
      <xsl:with-param name="p" select="'DATA_STAMPA_NUM'" />
    </xsl:call-template>
    <xsl:call-template name="campo">
      <xsl:with-param name="d" select="'Data di stampa (per esteso)'" />
      <xsl:with-param name="p" select="'DATA_STAMPA_TESTO'" />
    </xsl:call-template>
    <xsl:call-template name="campo">
      <xsl:with-param name="d" select="'Ora di stampa'" />
      <xsl:with-param name="p" select="'ORA_STAMPA'" />
    </xsl:call-template>
    <!-- elenco campi veri e propri -->
    <xsl:for-each select="c:CampiIntestazione/c:Campo[@Output='true']">
      <xsl:call-template name="campo">
        <xsl:with-param name="d" select="@Descrizione" />
        <xsl:with-param name="p" select="@Segnaposto" />
      </xsl:call-template>
    </xsl:for-each>
    <!-- corpo: potrebbe non esserci -->
    <xsl:if test="count(c:CampiCorpo/c:Campo[@Output='true'])&gt;0">
      <div class="section">Campi Corpo</div>
      <xsl:call-template name="campo">
        <xsl:with-param name="d" select="'Numero Riga'" />
        <xsl:with-param name="p" select="'N_RIGA'" />
      </xsl:call-template>
      <xsl:for-each select="c:CampiCorpo/c:Campo[@Output='true']">
        <xsl:call-template name="campo">
          <xsl:with-param name="d" select="@Descrizione" />
          <xsl:with-param name="p" select="@Segnaposto" />
        </xsl:call-template>
      </xsl:for-each>
    </xsl:if>



  </xsl:template>

  <xsl:template name="campo">
    <xsl:param name="d" />
    <xsl:param name="p" />
    <div class="campo">
      <span class="d">
        <xsl:attribute name="onclick">
          <xsl:text>CopyValue('</xsl:text>
          <xsl:value-of select="'%%'"/>
          <xsl:value-of select="$p"/>
          <xsl:value-of select="'%%'"/>
          <xsl:text>');</xsl:text>
        </xsl:attribute>
        <xsl:value-of select="$d"/>
      </span>
      <span class="p">
        <xsl:value-of select="'%%'"/>
        <xsl:value-of select="$p"/>
        <xsl:value-of select="'%%'"/>
      </span>
      
    </div>
  </xsl:template>
  
</xsl:stylesheet>
