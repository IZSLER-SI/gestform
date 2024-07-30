<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="no"/>
  <xsl:param name="gcontentkey" />
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="$gcontentkey='notelegali'">Note Legali</xsl:when>
      <xsl:when test="$gcontentkey='privacy'">Privacy</xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
