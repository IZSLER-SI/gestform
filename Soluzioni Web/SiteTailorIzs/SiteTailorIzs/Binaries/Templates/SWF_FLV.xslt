<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="conttype" />
  <xsl:param name="filetype" />
  <xsl:param name="id_eleme" />
  <xsl:param name="url_exte" />
  <xsl:param name="ele_widt" />
  <xsl:param name="ele_heig" />
  <xsl:template match="/">

    <xsl:choose>
      <xsl:when test="$conttype='swf'">
        <!-- scrittura SWF -->
        <div id="swfplayer">
            <a href="http://www.macromedia.com/go/getflashplayer/">Scarica Flash Player per visualizzare correttamente l'animazione</a>.
        </div>
        <script type="text/javascript">
          <xsl:choose>
            <xsl:when test="$filetype='internal'">
              var so = new SWFObject("../Binaries/ElementForPlayer.aspx?id=<xsl:value-of select="$id_eleme" />", "IDswf", "<xsl:value-of select="$ele_widt" />", "<xsl:value-of select="$ele_heig" />", "8", "#000000", "high");
            </xsl:when>
            <xsl:otherwise>
              var so = new SWFObject("<xsl:value-of select="$url_exte" />", "IDswf", "<xsl:value-of select="$ele_widt" />", "<xsl:value-of select="$ele_heig" />", "8", "#000000", "high");
            </xsl:otherwise>
          </xsl:choose>
          so.write("swfplayer");
        </script>
        
        
      </xsl:when>
      <xsl:when test="$conttype='flv'">
        <!-- scrittura video player -->
        <div id="videoplayer">
          <a href="http://www.macromedia.com/go/getflashplayer/">Scarica Flash Player per visualizzare correttamente il filmato</a>.
        </div>
        <script type="text/javascript">
          var so = new SWFObject("../Flash/mediaplayer.swf", "IDvideoplayer", "<xsl:value-of select="$ele_widt" />", "<xsl:value-of select="$ele_heig + 20" />", "8", "#000000", "high");
          <xsl:choose>
            <xsl:when test="$filetype='internal'">
              so.addVariable("file", encodeURIComponent("../Binaries/ElementForPlayer.aspx?id=<xsl:value-of select="$id_eleme" />"));
            </xsl:when>
            <xsl:otherwise>
              so.addVariable("file", encodeURIComponent("<xsl:value-of select="$url_exte" />"));
            </xsl:otherwise>
          </xsl:choose>          
          so.addVariable("type", "flv");
          so.addVariable("width", "<xsl:value-of select="$ele_widt" />");
          so.addVariable("height", "<xsl:value-of select="$ele_heig + 20" />");
          so.addVariable("showeq", "false");
          so.addVariable("autostart", "true");
          so.addVariable("overstretch", "none");
          so.addParam("allowfullscreen", "true");
          so.addParam("quality", "high");
          so.addParam("menu", "false");
          so.write("videoplayer");
        </script>
        
      </xsl:when>
    </xsl:choose>
  
  
  
  
  </xsl:template>
</xsl:stylesheet>
