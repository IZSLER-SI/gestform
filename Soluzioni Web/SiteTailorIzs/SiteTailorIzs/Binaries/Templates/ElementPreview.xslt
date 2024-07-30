<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../../Templates/Common.xslt"/>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />

  <xsl:param name="bothumb_width" />
  <xsl:param name="bothumb_height" />
  <xsl:param name="bothumb_mimetype" />


  <xsl:template match="/">
    <xsl:apply-templates select="UserBinaryElement"/>
  </xsl:template>

  <xsl:template match="UserBinaryElement">
    <div class="title">
      <xsl:value-of select="DESEL_TX"/>
    </div>
    <div class="container">
      <div class="left">
        <div class="title2">Contenuto</div>
        <div class="content2">
          <xsl:call-template name="content" />
          <br/>
          <xsl:call-template name="contentinfo" />
        </div>
      </div>
      <div class="right">
        <div class="title2">Anteprima backoffice</div>
        <div class="content2">
          <img class="img">
            <xsl:attribute name="src">
              <xsl:value-of select="'BOThumbnail.aspx?id='"/>
              <xsl:value-of select="ID_ELEME"/>
            </xsl:attribute>
          </img>
          <div class="fl">
            <xsl:value-of select="$bothumb_mimetype"/>
            <br/>
            <b>
              <xsl:value-of select="$bothumb_width"/>
              <xsl:value-of select="' x '"/>
              <xsl:value-of select="$bothumb_height"/>
            </b>
          </div>
          <div class="clr">
            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
          </div>
        </div>
        <xsl:apply-templates select="Thumbnail1" />
        <xsl:apply-templates select="Thumbnail2" />
        <xsl:apply-templates select="Thumbnail3" />
        <xsl:apply-templates select="Thumbnail4" />
        <xsl:apply-templates select="Thumbnail5" />
      </div>
      <div class="clr">
        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="Thumbnail1">
    <xsl:call-template name="Thumbnail">
      <xsl:with-param name="n" select="1" />
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="Thumbnail2">
    <xsl:call-template name="Thumbnail">
      <xsl:with-param name="n" select="2" />
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="Thumbnail3">
    <xsl:call-template name="Thumbnail">
      <xsl:with-param name="n" select="3" />
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="Thumbnail4">
    <xsl:call-template name="Thumbnail">
      <xsl:with-param name="n" select="4" />
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="Thumbnail5">
    <xsl:call-template name="Thumbnail">
      <xsl:with-param name="n" select="5" />
    </xsl:call-template>
  </xsl:template>


  <xsl:template name="Thumbnail">
    <xsl:param name="n" />
    <div class="title2">
      Anteprima <xsl:value-of select="$n"/>
    </div>
    <div class="content2">
      <img class="img">
        <xsl:attribute name="src">
          <xsl:value-of select="'Thumbnail.aspx?id='"/>
          <xsl:value-of select="../ID_ELEME"/>
          <xsl:value-of select="'&amp;num='"/>
          <xsl:value-of select="$n"/>
        </xsl:attribute>
      </img>
      <div class="fl">
        <xsl:value-of select="MimeType"/>
        <br/>
        <b>
          <xsl:choose>
            <xsl:when test="Width=0">?</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="Width"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:value-of select="' x '"/>
          <xsl:choose>
            <xsl:when test="Height=0">?</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="Height"/>
            </xsl:otherwise>
          </xsl:choose>
        </b>
      </div>
      <div class="clr">
        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
      </div>
    </div>
  </xsl:template>

  <xsl:template name="content">
    <xsl:choose>
      <xsl:when test="CodicePlayer='Direct'">
        <!-- contenuto immagine o allegato -->
        <xsl:choose>
          <xsl:when test="ModalitaGestioneAnteprima='Auto'">
            <!-- immagine -->
            <img class="boxc">
              <xsl:attribute name="src">
                <xsl:value-of select="'Element.aspx?id='"/>
                <xsl:value-of select="ID_ELEME"/>
              </xsl:attribute>
            </img>
          </xsl:when>
          <xsl:when test="CODFORMA='PDF'">
            <!-- PDF -->
            <iframe class="boxpdf" style="width:750px;height:560px;">
              <xsl:attribute name="src">
                <xsl:value-of select="'Element.aspx?id='"/>
                <xsl:value-of select="ID_ELEME"/>
              </xsl:attribute>
              IFRAME
            </iframe>
          </xsl:when>
          <xsl:otherwise>
            <!-- word, excel, powerpoint -->
            <a class="externalA">
              <xsl:attribute name="href">
                <xsl:value-of select="'Element.aspx?id='"/>
                <xsl:value-of select="ID_ELEME"/>
              </xsl:attribute>
              Fai clic per visualizzare l'elemento
            </a>
            <br/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="CodicePlayer='Swf'">
        <!-- contenuto SWF -->
        <div id="swfplayer">
          <a href="http://www.macromedia.com/go/getflashplayer/">Scarica Flash Player per visualizzare correttamente l'animazione</a>.
        </div>
        <script type="text/javascript">
          <xsl:choose>
            <xsl:when test="IS_LOCAL='true'">
              var so = new SWFObject("../Binaries/ElementForPlayer.aspx?id=<xsl:value-of select="ID_ELEME" />", "IDswf", "<xsl:value-of select="ELE_WIDT" />", "<xsl:value-of select="ELE_HEIG" />", "8", "#000000", "high");
            </xsl:when>
            <xsl:otherwise>
              var so = new SWFObject("<xsl:value-of select="URL_EXTE" />", "IDswf", "<xsl:value-of select="ELE_WIDT" />", "<xsl:value-of select="ELE_HEIG" />", "8", "#000000", "high");
            </xsl:otherwise>
          </xsl:choose>
          so.write("swfplayer");
        </script>
      </xsl:when>
      <xsl:when test="CodicePlayer='Flv'">
        <!-- contenuto FLV -->
        <div id="videoplayer">
          <a href="http://www.macromedia.com/go/getflashplayer/">Scarica Flash Player per visualizzare correttamente il filmato</a>.
        </div>
        <script type="text/javascript">
          var so = new SWFObject("../Flash/mediaplayer.swf", "IDvideoplayer", "<xsl:value-of select="ELE_WIDT" />", "<xsl:value-of select="ELE_HEIG + 20" />", "8", "#000000", "high");
          <xsl:choose>
            <xsl:when test="IS_LOCAL='true'">
              so.addVariable("file", encodeURIComponent("../Binaries/ElementForPlayer.aspx?id=<xsl:value-of select="ID_ELEME" />"));
            </xsl:when>
            <xsl:otherwise>
              so.addVariable("file", encodeURIComponent("<xsl:value-of select="URL_EXTE" />"));
            </xsl:otherwise>
          </xsl:choose>
          so.addVariable("type", "flv");
          so.addVariable("width", "<xsl:value-of select="ELE_WIDT" />");
          so.addVariable("height", "<xsl:value-of select="ELE_HEIG + 20" />");
          so.addVariable("showeq", "false");
          so.addVariable("autostart", "true");
          so.addVariable("overstretch", "none");
          so.addParam("allowfullscreen", "true");
          so.addParam("quality", "high");
          so.addParam("menu", "false");
          so.write("videoplayer");
        </script>
      </xsl:when>
      <xsl:when test="CodicePlayer='Url'">
        <!-- link ad url esterno -->
        <a class="externalA" target="_blank">
          <xsl:attribute name="href">
            <xsl:value-of select="URL_EXTE"/>
          </xsl:attribute>
          <xsl:value-of select="URL_EXTE"/>
        </a>
        <br/>
        <br/>
        <iframe style="width:750px;height:560px;">
          <xsl:attribute name="src">
            <xsl:value-of select="URL_EXTE"/>
          </xsl:attribute>
          IFRAME
        </iframe>
      </xsl:when>

    </xsl:choose>

  </xsl:template>
  <xsl:template name="contentinfo">
    <xsl:value-of select="DESFORMA"/>
    <xsl:if test="ELE_WIDT!=0">
      <xsl:if test="DESFORMA!=''">
        <xsl:value-of select="' - '" />
      </xsl:if>
      <b>
        <xsl:value-of select="ELE_WIDT"/>
        <xsl:value-of select="' x '"/>
        <xsl:value-of select="ELE_HEIG"/>
      </b>
    </xsl:if>
    <br />
    Caricamento:
    <b>
      <xsl:value-of select="INS_NOM_COG"/>
    </b>
    <xsl:value-of select="' - '"/>
    <b>
      <xsl:call-template name="dataoraDDMMYYHHMM">
        <xsl:with-param name="dataora" select="DATA_INS" />
      </xsl:call-template>
    </b>
    <xsl:if test="USER_UPD!=0">
      <br/>
      Utima modifica:
      <b>
        <xsl:value-of select="UPD_NOM_COG"/>
      </b>
      <xsl:value-of select="' - '"/>
      <b>
        <xsl:call-template name="dataoraDDMMYYHHMM">
          <xsl:with-param name="dataora" select="DATA_UPD" />
        </xsl:call-template>
      </b>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
