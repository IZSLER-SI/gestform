<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />
  <xsl:param name="nodekey" />
  <xsl:param name="dipendente" />
	<xsl:param name="pUrl" />

  <xsl:template match="/">
    
    <!-- home page -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='homepage'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled" select="'1'" />
      <xsl:with-param name="row1" select="'HOME'" />
      <xsl:with-param name="row2" select="'PAGE'" />
      <xsl:with-param name="url" select="'/'" />
    </xsl:call-template>
    
    <!-- offerta formativa -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='eventi'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled" select="'1'" />
      <xsl:with-param name="row1" select="'OFFERTA'" />
      <xsl:with-param name="row2" select="'FORMATIVA'" />
      <xsl:with-param name="url" select="'/eventi'" />
    </xsl:call-template>

    <!-- gli eventi a cui sono iscritto (solo loggedin) -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='iscrizioni-attive'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled">
        <xsl:choose>
          <xsl:when test="$region='LoggedIn'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="row1" select="'GLI EVENTI A CUI'" />
      <xsl:with-param name="row2" select="'SONO ISCRITTO'" />
      <xsl:with-param name="url" select="'/iscrizioni-attive'" />
    </xsl:call-template>

    <!-- portfolio (solo logged in) -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='portfolio'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled">
        <xsl:choose>
          <xsl:when test="$region='LoggedIn'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="row1" select="'PORTFOLIO'" />
      <xsl:with-param name="row2" select="'FORMATIVO'" />
      <xsl:with-param name="url" select="'/portfolio'" />
    </xsl:call-template>

     <!-- formazione esterna (solo ente-sigma 57) -->
      <xsl:call-template name="item">
          <xsl:with-param name="active">
              <xsl:choose>
                  <xsl:when test="$nodekey='formazione-esterna'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
          </xsl:with-param>
          <xsl:with-param name="enabled">
              <xsl:choose>
                  <xsl:when test="$region='LoggedIn' and $dipendente='1'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
          </xsl:with-param>
          <xsl:with-param name="row1" select="'FORMAZIONE'" />
          <xsl:with-param name="row2" select="'ESTERNA'" />
          <xsl:with-param name="url" select="'/formazione-esterna'" />
      </xsl:call-template>
    
    <!-- formazione individuale (solo logged in INT) -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='formazione-individuale'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled">
        <xsl:choose>
          <xsl:when test="$region='LoggedIn' and $dipendente='1'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="row1" select="'FORMAZIONE'" />
      <xsl:with-param name="row2" select="'INDIVIDUALE'" />
      <xsl:with-param name="url" select="'/formazione-individuale'" />
    </xsl:call-template>

    <!-- area news -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='news'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled" select="'1'" />
      <xsl:with-param name="row1" select="'AREA'" />
      <xsl:with-param name="row2" select="'NOTIZIE'" />
      <xsl:with-param name="url" select="'/news'" />
    </xsl:call-template>

    <!-- area download -->
    <xsl:call-template name="item">
      <xsl:with-param name="active">
        <xsl:choose>
          <xsl:when test="$nodekey='download'">1</xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
      <xsl:with-param name="enabled" select="'1'" />
      <xsl:with-param name="row1" select="'AREA'" />
      <xsl:with-param name="row2" select="'DOWNLOAD'" />
      <xsl:with-param name="url" select="'/download'" />
      <xsl:with-param name="last" select="'0'" />
    </xsl:call-template>
			
    <!-- chiusura -->
    <div class="clear">
      <xsl:value-of select="''"/>
    </div>
    
  </xsl:template>
  
  <xsl:template name="item">
    <xsl:param name="last" />
    <xsl:param name="active" />
    <xsl:param name="enabled" />
    <xsl:param name="row1" />
    <xsl:param name="row2" />
    <xsl:param name="url" />
    <xsl:choose>
      <xsl:when test="$enabled='0'">
        <!-- non abilitato -->
        <span class="item disab">
          <xsl:if test="$last='1'">
            <xsl:attribute name="style">
              <xsl:value-of select="'border-right-width:0px;'"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$row1"/>
          <br/>
          <xsl:value-of select="$row2"/>
        </span>
      </xsl:when>
      <xsl:otherwise>
        <!-- abilitato -->
        <a>
          <xsl:attribute name="class">
            <xsl:choose>
              <xsl:when test="$active='1'">
                <xsl:value-of select="'item act'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'item enab'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="href">
            <xsl:value-of select="$url"/>
          </xsl:attribute>
          <xsl:if test="$last='1'">
            <xsl:attribute name="style">
              <xsl:value-of select="'border-right-width:0px;'"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:value-of select="$row1"/>
          <br/>
          <xsl:value-of select="$row2"/>
        </a>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
