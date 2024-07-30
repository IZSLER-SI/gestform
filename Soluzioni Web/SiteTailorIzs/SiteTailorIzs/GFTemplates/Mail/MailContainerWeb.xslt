<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="ht_testo" />
  <xsl:param name="baseurl" />
  <xsl:param name="mailfrom_name" />
  <xsl:param name="mailfrom_address" />
  <xsl:param name="mailto" />
  <xsl:param name="subject" />
  <xsl:param name="ragionesociale" />
  <xsl:param name="indirizzocompleto" />
  <xsl:param name="tel" />
  <xsl:param name="fax" />
  <xsl:param name="email" />
  
  <xsl:template match="/">
    <div class="mailhead">
      Mittente:
      <b>
        <xsl:value-of select="$mailfrom_name"/>
      </b>
      <br/>
      <xsl:if test="$mailto!=''">
        Destinatario:
        <b>
          <xsl:value-of select="$mailto" />
        </b>
        <br/>
      </xsl:if>
      Oggetto:
      <b>
        <xsl:value-of select="$subject"/>
      </b>
    </div>
        <div class="body">
          <div align="center">
            <table cellspacing="0" cellpadding="0" border="0" width="630">
              <tr>
                <td class="bodyhdr">
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
                <td class="bodytd">
                  <div>
                    <xsl:value-of select="$ht_testo" disable-output-escaping="yes"/>
                  </div>
                </td>
              </tr>
              <tr>
                <td class="footertd">
                  <b><xsl:value-of select="$ragionesociale"/></b><br />
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
        
  </xsl:template>
  
</xsl:stylesheet>