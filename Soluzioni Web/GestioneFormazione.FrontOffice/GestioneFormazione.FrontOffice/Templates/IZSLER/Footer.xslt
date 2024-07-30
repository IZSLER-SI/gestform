<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:template match="/">

    <div class="footerinner">
      <div style="float:left;">
        <div>
          <b>Istituto Zooprofilattico Sperimentale della Lombardia e dell'Emilia Romagna "Bruno Ubertini"
            <br/>
            Ufficio Formazione
          </b>
        </div>
        <div>
          Via Bianchi, 9 - 25124 Brescia - Tel.030 2290379-330-333 - Fax 030 2290616 - Email
          <a class="ftra_u" href="mailto:formazione@izsler.it">formazione@izsler.it</a>
          - PEC
          <a class="ftra_u" href="mailto:formazione@cert.izsler.it">formazione@cert.izsler.it</a>
        </div>
        <div>
          C.F. - P.IVA 00284840170 -
          N. REA CCIAA DI BRESCIA 88834
        </div>
        <div>
          <em>
            Questo sito utilizza esclusivamente cookies tecnici non persistenti.
          </em>
        </div>
      </div>
      <div style="float:right;text-align:right;">
        <div>
          <a class="ftra" href="/NoteLegali">note legali</a>
          |
          <a class="ftra" href="/Privacy">privacy</a>
        </div>
        <div style="padding-top:25px;">
          <a href="http://www.invisiblefarm.it" target="_blank">
            <img src="/img/IZSLER/logo_invisiblefarm.png" alt="Powered By Invisiblefarm" />
          </a>
        </div>
      </div>
      <div class="clear">
        <xsl:value-of select="''"/>
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>
