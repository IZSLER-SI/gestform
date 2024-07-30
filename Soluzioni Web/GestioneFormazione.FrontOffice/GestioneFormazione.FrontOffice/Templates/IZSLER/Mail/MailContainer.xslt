<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" />
  <xsl:param name="ht_testo" />
  <xsl:param name="baseurl" />
  <xsl:param name="ragionesociale" />
  <xsl:param name="indirizzocompleto" />
  <xsl:param name="tel" />
  <xsl:param name="fax" />
  <xsl:param name="email" />

  <xsl:template match="/">
    <html>
      <head>
        <title>e-mail</title>
        <xsl:call-template name="css" />
      </head>
      <body>
        <div style="margin:5px 5px 5px 5px;background-color:#f0f0f0;color:#000000;">
          <div align="center" style="font-family: Arial, Helvetica, Sans-Serif;font-size:12px;">
            <table cellspacing="0" cellpadding="0" border="0" width="630">
              <tr>
                <td style="padding-bottom:10px;padding-top:10px;font-size:12px;text-align:left;">
                  <img width="630" height="105">
                    <xsl:attribute name="alt">
                      <xsl:value-of select="$ragionesociale"/>
                    </xsl:attribute>
                    <xsl:attribute name="src">
                      <xsl:value-of select="$baseurl"/>
                      <xsl:value-of select="'CImg/mail/LogoHdr.png'"/>
                    </xsl:attribute>
                  </img>
                </td>
              </tr>
              <tr>
                <td style="border:3px solid #0668a7;padding:11px;background-color:#ffffff;font-size:14px;line-height:18px;text-align:left;font-family: Arial, Helvetica, Sans-Serif;">
                  <div>
                    <xsl:value-of select="$ht_testo" disable-output-escaping="yes"/>
                  </div>
                </td>
              </tr>
              <tr>
                <td style="padding-top:10px;padding-bottom:10px;font-size:12px;line-height:15px;text-align:left;font-family: Arial, Helvetica, Sans-Serif;">
                  <b>
                    <xsl:value-of select="$ragionesociale"/>
                  </b><br />
                  <xsl:value-of select="$indirizzocompleto"/>
                  <xsl:value-of select="' - Tel. '"/>
                  <xsl:value-of select="$tel" />
                  <xsl:value-of select="' - Fax '"/>
                  <xsl:value-of select="$fax"/><br/>
                  E-mail:
                  <a>
                    <xsl:attribute name="href">
                      <xsl:value-of select="'mailto:'"/>
                      <xsl:value-of select="$email" />
                    </xsl:attribute>
                    <xsl:value-of select="$email" />
                  </a>
                </td>
              </tr>
            </table>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
  <xsl:template name="css">
    <style type="text/css">
      body
      {
      margin:0px;
      background-color:#f0f0f0;
      }
      a
      {
      color:#336699;
      text-decoration:underline;
      }
      a:hover
      {
      color:#ff6600;
      }
    </style>

  </xsl:template>

</xsl:stylesheet>