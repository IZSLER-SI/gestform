<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="node" />
  <xsl:param name="region" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="count(evento/sezione/file[@fl_visibile=1])=0">
        <div style="padding:25px;">
          <div class="bottom20">Per questo evento non è disponibile alcun materiale.</div>
          <div style="font-weight:bold;">
            <span class="btnlink btnlink_blue" onclick="showMaterialPopup(false);">Chiudi</span>
          </div>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="mat_popup_button">
          <span class="btnlink btnlink_blue" onclick="showMaterialPopup(false);">Chiudi</span>
        </div>
        <div class="mat_popup_mat">
          <xsl:apply-templates select="evento/sezione" />
        </div>
      </xsl:otherwise>
    </xsl:choose>  
  </xsl:template>

  <xsl:template match="sezione">
    <xsl:if test="count(file[@fl_visibile=1])&gt;0">
      <div class="section">
        <xsl:choose>
          <xsl:when test="count(file[@fl_visibile=1])=1">
            <xsl:value-of select="@tx_sezionedocevento_sing"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="@tx_sezionedocevento_plur"/>
          </xsl:otherwise>
        </xsl:choose>
      </div>
      <div class="sectiondata">
        <xsl:apply-templates select="file[@fl_visibile=1]" />
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="file">
    <a class="dl_item">
      <xsl:attribute name="href">
        <xsl:value-of select="'/DocEve/'"/>
        <xsl:value-of select="@id_file_evento"/>
        <xsl:value-of select="@ext"/>
      </xsl:attribute>
      <div class="img">
        <img>
          <xsl:attribute name="src">
            <xsl:value-of select="'/DocEveThumbs/'"/>
            <xsl:value-of select="@thumbnail"/>
          </xsl:attribute>
        </img>
      </div>
      <div class="text">
        <div class="name">
          <xsl:value-of select="@tx_descrizione"/>
        </div>
        <xsl:if test="count(@ht_note)&gt;0">
          <div class="userhtml">
            <xsl:value-of select="@ht_note" disable-output-escaping="yes" />
          </div>
        </xsl:if>
      </div>
      <div class="clear">
        <xsl:value-of select="''" />
      </div>
    </a>
  </xsl:template>


</xsl:stylesheet>
