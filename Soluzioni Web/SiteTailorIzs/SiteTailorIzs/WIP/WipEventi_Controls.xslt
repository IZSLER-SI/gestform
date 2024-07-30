<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../Templates/Common.xslt"/>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <xsl:apply-templates select="root" />
  </xsl:template>

  <xsl:template match="root">
    <div class="stl_dfo">
      <div class="title" style="padding-bottom:20px;">
        Operazioni da effettuare nei prossimi <xsl:value-of select="@ni_giorni_max"/> giorni o scadute
      </div>
    </div>
    <xsl:choose>
      <xsl:when test="count(data)=0">
        <div style="color:#009900;font-size:15px;font-weight:bold;">
          Nessuna operazione in scadenza o scaduta.
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="wipevento">
          <ul>
            <xsl:apply-templates select="data" />
          </ul>
        </div>
      </xsl:otherwise>
      
    </xsl:choose>
  </xsl:template>

  <xsl:template match="data">
    <li>
      
      <span>
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="@ni_giorniscadenza&lt;0">
              <xsl:value-of select="'expired'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'active'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <b>
          <xsl:call-template name="dataDDMMYYYY">
            <xsl:with-param name="data" select="@dt_scadenza" />
          </xsl:call-template>
        </b>
        <xsl:choose>
          <xsl:when test="@ni_giorniscadenza&lt;-1">
            (<xsl:value-of select="-@ni_giorniscadenza"/> giorni fa)
          </xsl:when>
          <xsl:when test="@ni_giorniscadenza=-1">
            (ieri)
          </xsl:when>
          <xsl:when test="@ni_giorniscadenza=0">
            (oggi)
          </xsl:when>
          <xsl:when test="@ni_giorniscadenza=1">
            (domani)
          </xsl:when>
          <xsl:when test="@ni_giorniscadenza&gt;1">
            (tra <xsl:value-of select="@ni_giorniscadenza"/> giorni)
          </xsl:when>
        </xsl:choose>
      </span>
      
      <ul>
        <xsl:apply-templates select="evento">
          <xsl:with-param name="number" select="position()" />
        </xsl:apply-templates>
      </ul>
    </li>
  </xsl:template>

  <xsl:template match="evento">
    <xsl:param name="number" />
    <li>
      <asp:LinkButton runat="server" CssClass="evea">
        <xsl:attribute name="id">
          <xsl:value-of select="'lnkSelectEvento_'"/>
          <xsl:value-of select="$number"/>
          <xsl:value-of select="'_'"/>
          <xsl:value-of select="position()"/>
        </xsl:attribute>
        <xsl:attribute name="CommandArgument">
          <xsl:value-of select="@id_evento"/>
        </xsl:attribute>
        <b>
          <xsl:value-of select="@tx_titolo"/>
        </b>
        -
        <xsl:call-template name="dataDalAl_it">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </asp:LinkButton>
      <ul>
        <xsl:apply-templates select="scadparent" />
        <xsl:apply-templates select="scadchild" />
      </ul>
    </li>
  </xsl:template>

  <xsl:template match="scadparent">
    <li>
      <xsl:value-of select="@tx_label"/>
    </li>
  </xsl:template>

  <xsl:template match="scadchild">
    <li>
      <xsl:value-of select="@tx_label"/> per:
      <ul>
        <xsl:for-each select="child">
          <li class="normal">
            <xsl:value-of select="@tx_cognome"/>
            <xsl:value-of select="' '"/>
            <xsl:value-of select="@tx_nome"/>
            <xsl:value-of select="' ('"/>
            <xsl:value-of select="@ac_categoriaecm"/>
            <xsl:value-of select="')'"/>
          </li>
        </xsl:for-each>
      </ul>
    </li>
  </xsl:template>
</xsl:stylesheet>
