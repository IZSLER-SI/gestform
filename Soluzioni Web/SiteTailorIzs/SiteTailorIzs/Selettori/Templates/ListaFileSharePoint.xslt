<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="baseurl" />


  <xsl:template match="/">
    <xsl:apply-templates select="files" />
  </xsl:template>

  <xsl:template match="files">
    <xsl:apply-templates select="file" />
  </xsl:template>
  <xsl:template match="file">
      <div class="file">
        <xsl:attribute name="onclick">
          <xsl:text>parent.stl_sel_done('</xsl:text>
          <xsl:value-of select="@name"/>
          <xsl:text>');</xsl:text>
        </xsl:attribute>
        <img>
          <xsl:attribute name="src">
            <xsl:value-of select="'Img/'"/>
            <xsl:value-of select="@ext"/>
            <xsl:value-of select="'.png'"/>
          </xsl:attribute>
        </img>
        <xsl:value-of select="@name"/>
      </div>
  </xsl:template>
                
</xsl:stylesheet>