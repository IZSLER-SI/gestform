<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="region" />
  <xsl:param name="nodekey" />
  <xsl:param name="titolo" />
  <xsl:param name="cognome" />
  <xsl:param name="nome" />
  <xsl:param name="matricola" />
  <xsl:param name="email" />
  <xsl:template match="/">

    <xsl:choose>
      <xsl:when test="$region='LoggedOut'">
        <div id="navright_nouser">
          <!-- chiave -->
          <div class="key">
            <img src="/CIMG/key.png" />
          </div>
          <!-- accedi -->
          <xsl:call-template name="item">
            <xsl:with-param name="active">
              <xsl:choose>
                <xsl:when test="$nodekey='accesso'">1</xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="enabled" select="'1'" />
            <xsl:with-param name="row1" select="'ACCEDI'" />
            <xsl:with-param name="row2" select="'AL PORTALE'" />
            <xsl:with-param name="url" select="''" />
            <xsl:with-param name="id" select="'mnulogin'" />
          </xsl:call-template>
          <!-- registrati al portale -->
          <xsl:call-template name="item">
            <xsl:with-param name="active">
              <xsl:choose>
                <xsl:when test="$nodekey='registrazione'">1</xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="enabled" select="'1'" />
            <xsl:with-param name="row1" select="'REGISTRATI'" />
            <xsl:with-param name="row2" select="'AL PORTALE'" />
            <xsl:with-param name="url" select="'/registrazione'" />
          </xsl:call-template>
          <!-- password smarrita -->
          <xsl:call-template name="item">
            <xsl:with-param name="active">
              <xsl:choose>
                <xsl:when test="$nodekey='password-smarrita'">1</xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="enabled" select="'1'" />
            <xsl:with-param name="row1" select="'PASSWORD'" />
            <xsl:with-param name="row2" select="'SMARRITA?'" />
            <xsl:with-param name="url" select="'/password-smarrita'" />
            <xsl:with-param name="last" select="'1'" />
          </xsl:call-template>
        </div>
      </xsl:when>
      <xsl:when test="$region='LoggedIn' or $region='CompleteProfile'">
        <div id="navright_user">
          <div class="user">
            <div class="picture">
              <img src="/CImg/User.png" />
            </div>
            <div class="data">
              <div>
                <strong>
                  <xsl:if test="$titolo!=''">
                    <xsl:value-of select="$titolo"/>
                    <xsl:value-of select="' '"/>
                  </xsl:if>
                  <xsl:value-of select="$nome"/>
                  <xsl:value-of select="' '"/>
                  <xsl:value-of select="$cognome"/>
                </strong>
              </div>
              <xsl:if test="$matricola!=''">
                <div class="matricola">
                  Matricola: <xsl:value-of select="$matricola"/>
                </div>
              </xsl:if>
              <div>
                <xsl:value-of select="$email"/>
              </div>
            </div>
            <div class="logout">
              <asp:LinkButton ID="lnkLogout" runat="server" EnableViewState="False">
                <img src="/Cimg/logout.png" title="Logout" />
              </asp:LinkButton>
            </div>
          </div>
          <div class="commands">
            <!-- cambio mail (sempre enabled tranne in post-login) -->
            <xsl:call-template name="item_auth">
              <xsl:with-param name="active">
                <xsl:choose>
                  <xsl:when test="$nodekey='cambio-mail'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:with-param>
              <xsl:with-param name="enabled">
                <xsl:choose>
                  <xsl:when test="$region='LoggedIn'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:with-param>
              <xsl:with-param name="row1" select="'MODIFICA'" />
              <xsl:with-param name="row2" select="'E-MAIL'" />
              <xsl:with-param name="url" select="'/cambio-mail'" />
            </xsl:call-template>
            <!-- cambio password (sempre enabled tranne in post-login) -->
            <xsl:call-template name="item_auth">
              <xsl:with-param name="active">
                <xsl:choose>
                  <xsl:when test="$nodekey='cambio-password'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:with-param>
              <xsl:with-param name="enabled">
                <xsl:choose>
                  <xsl:when test="$region='LoggedIn'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:with-param>
              <xsl:with-param name="row1" select="'CAMBIA'" />
              <xsl:with-param name="row2" select="'PASSWORD'" />
              <xsl:with-param name="url" select="'/cambio-password'" />
            </xsl:call-template>
            <!-- modifica profilo (solo esterni) -->
            <xsl:call-template name="item_auth">
              <xsl:with-param name="active">
                <xsl:choose>
                  <xsl:when test="$nodekey='modifica-profilo'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:with-param>
              <xsl:with-param name="row1" select="'MODIFICA'" />
              <xsl:with-param name="row2" select="'PROFILO'" />
              <xsl:with-param name="url" select="'/modifica-profilo'" />
              <xsl:with-param name="last" select="'1'" />
            </xsl:call-template>
          </div>
        </div>
      </xsl:when>
    </xsl:choose>
    
  </xsl:template>

  <xsl:template name="item">
    <xsl:param name="last" />
    <xsl:param name="active" />
    <xsl:param name="enabled" />
    <xsl:param name="row1" />
    <xsl:param name="row2" />
    <xsl:param name="url" />
    <xsl:param name="id" />
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
        <xsl:choose>
          <xsl:when test="$url=''">
            <!-- genero uno SPAN -->
            <!-- abilitato -->
            <span>
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
              <xsl:attribute name="id">
                <xsl:value-of select="$id"/>
              </xsl:attribute>
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
        
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="item_auth">
    <xsl:param name="last" />
    <xsl:param name="active" />
    <xsl:param name="enabled" />
    <xsl:param name="row1" />
    <xsl:param name="row2" />
    <xsl:param name="url" />
    <xsl:param name="id" />
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
        <xsl:choose>
          <xsl:when test="$url=''">
            <!-- genero uno SPAN -->
            <!-- abilitato -->
            <span>
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
              <xsl:attribute name="id">
                <xsl:value-of select="$id"/>
              </xsl:attribute>
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

      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
