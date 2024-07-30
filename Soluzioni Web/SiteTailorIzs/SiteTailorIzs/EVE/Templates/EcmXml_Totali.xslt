<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="livelloaccesso" />
  <xsl:param name="totabilitati" />
  <xsl:param name="totok" />
  <xsl:param name="totko" />
  <xsl:template match="/">
    <div style="position:absolute;top:425px;left:340px;width:457px;" class="stl_dfo">
      <div class="title">
        Riepilogo Nominativi
      </div>
    </div>
    <div class="stl_gen_box" style="position: absolute; width: 438px; top:455px; left: 340px;">
      <div class="content padall" style="height:150px;">
        <table style="width:100%;">
          <tr>
            <td align="left" valign="middle" style="border-bottom:solid 1px #c0c0c0;font-size:12px;padding-bottom:5px;padding-top:5px;">
              <b>Nominativi aventi diritto ai crediti ECM</b>
            </td>
            <td align="right" valign="middle" style="border-bottom:solid 1px #c0c0c0;font-size:25px;padding-bottom:5px;padding-top:5px;">
              <xsl:choose>
                <xsl:when test="$totabilitati='0'">
                  <span style="color:red;">Nessuno</span>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$totabilitati"/>
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
          <tr>
            <td align="left" valign="middle" style="border-bottom:solid 1px #c0c0c0;font-size:12px;padding-bottom:5px;padding-top:5px;padding-left:20px;">
              Nominativi con dati ECM validi
            </td>
            <td align="right" valign="middle" style="border-bottom:solid 1px #c0c0c0;font-size:25px;padding-bottom:5px;padding-top:5px;">
              <xsl:choose>
                <xsl:when test="$totok='0'">
                  Nessuno
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$totok"/>
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
          <tr>
            <td align="left" valign="middle" style="border-bottom:solid 1px #c0c0c0;font-size:12px;padding-bottom:5px;padding-top:5px;padding-left:20px;">
              Nominativi con dati ECM NON validi
            </td>
            <td align="right" valign="middle" style="border-bottom:solid 1px #c0c0c0;font-size:25px;padding-bottom:5px;padding-top:5px;">
              <xsl:choose>
                <xsl:when test="$totko='0'">
                  <span>Nessuno</span>
                </xsl:when>
                <xsl:otherwise>
                  <div style="color:red;">
                    <xsl:value-of select="$totko"/>
                  </div>
                  <div style="font-size:12px">
                    <a class="classicA" href="ValidazioneDatiECM.aspx">
                      Verifica dati ECM
                    </a>
                  </div>
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
        </table>
      </div>
    </div>
  </xsl:template>

</xsl:stylesheet>