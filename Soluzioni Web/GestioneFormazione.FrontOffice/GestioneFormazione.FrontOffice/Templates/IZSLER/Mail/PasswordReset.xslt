<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../../Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="baseurl" />
  <xsl:param name="reseturl" />
  <xsl:template match="/">
    <div>
      Gentile Utente,<br/>
      <br/>
      Utilizza il link sottostante per reimpostare la tua password di accesso al Portale Formazione:<br/>
      <br/>
      <a target="_blank">
        <xsl:attribute name="href">
          <xsl:value-of select="$reseturl"/>
        </xsl:attribute>
        <xsl:value-of select="$reseturl"/>
      </a><br/>
      <br/>
      Ti ricordiamo che questo link è valido per 24 ore dalla data di ricezione della presente e-mail.
    </div>
  </xsl:template>
  
</xsl:stylesheet>
