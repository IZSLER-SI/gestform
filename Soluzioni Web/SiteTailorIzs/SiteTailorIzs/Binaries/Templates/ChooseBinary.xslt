<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <xsl:apply-templates select="Categories" />
  </xsl:template>

  <xsl:template match="Categories">
    <xsl:apply-templates select="Category" />
  </xsl:template>

  <xsl:template match="Category">
    <xsl:if test="count(Element)&gt;0">
      <div class="category">
        <xsl:value-of select="@descateg"/>
      </div>
      <xsl:apply-templates select="Element" />
    </xsl:if>
  </xsl:template>
  <xsl:template match="Element">
    <div class="element">
      <table cellspacing="0" border="0">
        <td class="imgtd">
          <a>
            <xsl:attribute name="href">
              <xsl:text>javascript:wopen('ElementPreview.aspx?id=</xsl:text>
              <xsl:value-of select="@id_eleme"/>
              <xsl:text>','binarypreview',980,700,1,0,0,1,1);</xsl:text>
            </xsl:attribute>
            <img class="thumb_i">
              <xsl:attribute name="src">
                <xsl:value-of select="'BOThumbnail.aspx?id='"/>
                <xsl:value-of select="@id_eleme"/>
              </xsl:attribute>
            </img>
          </a>
        </td>
        <td class="txttd">
          <a class="chooseA">
            <xsl:attribute name="href">
              <xsl:value-of select="'javascript:chooseElement('"/>
              <xsl:value-of select="@id_eleme"/>
              <xsl:value-of select="');'"/>
            </xsl:attribute>
            <div class="desc">
              <xsl:value-of select="@desel_tx"/>
            </div>
            <div class="fmt">
              <xsl:value-of select="@desforma"/>
            </div>
          </a>
        </td>
      </table>
    </div>
    
  </xsl:template>
</xsl:stylesheet>
