<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:asp="remove">
  <xsl:import href="../../Templates/Common.xslt" />
  <xsl:output method="xml" indent="no" omit-xml-declaration="yes" />
  <xsl:param name="accesslevel" />
  <xsl:template match="/">
    <xsl:for-each select="root">
      <xsl:for-each select="data">
        <div style="border-bottom: 1px solid #c0c0c0;padding-bottom:3px;padding-top:10px;margin-bottom:3px;">
          <b>
            <xsl:call-template name="data_it">
              <xsl:with-param name="data" select="@dt_data" />
            </xsl:call-template>
          </b>
        </div>
        <div style="padding-left:10px;">
          <xsl:for-each select="range">
            <div>
              <asp:CheckBox runat="server">
                <xsl:attribute name="ID">
                  <xsl:value-of select="'chkOrario_'"/>
                  <xsl:value-of select="@id_calendario"/>
                </xsl:attribute>
                <xsl:attribute name="Text">
                  <xsl:call-template name="oraHHMM_SqlTime">
                    <xsl:with-param name="ora" select="@tm_inizio" />
                  </xsl:call-template>
                  -
                  <xsl:call-template name="oraHHMM_SqlTime">
                    <xsl:with-param name="ora" select="@tm_fine" />
                  </xsl:call-template>
                </xsl:attribute>
              </asp:CheckBox>
            </div>
          </xsl:for-each>
        </div>
      </xsl:for-each>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>