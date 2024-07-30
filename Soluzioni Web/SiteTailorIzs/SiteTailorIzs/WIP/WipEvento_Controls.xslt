<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Templates/Common.xslt"/>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />

  <xsl:variable name="gx" select="30" />
  <xsl:variable name="gy" select="28" />
  <xsl:variable name="delta" select="3" />
  <xsl:variable name="fs" select="'14'" />

  <xsl:template match="/">
    <xsl:apply-templates select="scheda" />
  </xsl:template>

  <xsl:template match="scheda">
    <!-- stile -->
    <style type="text/css">
      #wip {
        font-size:<xsl:value-of select="$fs"/>px;
        line-height:<xsl:value-of select="$gy"/>px;
      }
      .field
      {
        line-height:<xsl:value-of select="$gy - 8"/>px;
        font-size:<xsl:value-of select="$fs"/>px;
      }
      .value
      {
        line-height:<xsl:value-of select="$gy - 4"/>px;
      }
      .inputtext
      {
        font-size:<xsl:value-of select="$fs"/>px;
      }
    </style>
    <!-- contenitore -->
    <div id="wip">
      <xsl:attribute name="data-formid">
        <xsl:value-of select="@id_scheda"/>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:call-template name="wh">
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="@h" />
        </xsl:call-template>
      </xsl:attribute>

      <!-- box -->
      <xsl:apply-templates select="box" mode="parent" />
      <!-- testi -->
      <xsl:apply-templates select="text" mode="parent" />
      <!-- valori -->
      <xsl:apply-templates select="value" mode="parent" />
      <!-- campi input utente -->
      <xsl:apply-templates select="field" mode="parent" />
      <!-- wip -->
      <xsl:for-each select="wip">
        <xsl:apply-templates select=".">
          <xsl:with-param name="y" select="@y" />
          <xsl:with-param name="fl_child" select="0" />
        </xsl:apply-templates>
      </xsl:for-each>

      <!-- variabile start figli -->
      <xsl:variable name="syf" select="@syf" />
    
      <!-- ciclo sui figli -->
      <xsl:apply-templates select="child">
        <xsl:with-param name="syf" select="$syf" />
      </xsl:apply-templates>
    </div>
  </xsl:template>

  <xsl:template match="box" mode="parent">
    <div class="box">
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="@y" />
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="@h" />
          <xsl:with-param name="dt" select="0" />
          <xsl:with-param name="dr" select="0" />
          <xsl:with-param name="dl" select="0" />
          <xsl:with-param name="db" select="0" />
        </xsl:call-template>
        <xsl:if test="count(@bgcolor)&gt;0">
          <xsl:value-of select="'background-color:'"/>
          <xsl:value-of select="@bgcolor"/>
        </xsl:if>
      </xsl:attribute>
    </div>

  </xsl:template>

  <xsl:template match="text" mode="parent">
    <div class="text">
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="@y" />
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="@h" />
          <xsl:with-param name="dt" select="0" />
          <xsl:with-param name="dr" select="2" />
          <xsl:with-param name="dl" select="2" />
          <xsl:with-param name="db" select="0" />
        </xsl:call-template>
        <!-- altezza linea: pari all'altezza -->
        <xsl:value-of select="'line-height:'"/>
        <xsl:value-of select="format-number(@h * $gy, '0')" />
        <xsl:value-of select="'px;'"/>
        <xsl:if test="count(@color)&gt;0">
          <xsl:value-of select="'color:'"/>
          <xsl:value-of select="@color"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
        <xsl:if test="count(@fontsize)&gt;0">
          <xsl:value-of select="'font-size:'"/>
          <xsl:value-of select="@fontsize"/>
          <xsl:value-of select="'px;'"/>
        </xsl:if>
        <xsl:if test="count(@fontweight)&gt;0">
          <xsl:value-of select="'font-weight:'"/>
          <xsl:value-of select="@fontweight"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
        <xsl:if test="count(@textalign)&gt;0">
          <xsl:value-of select="'text-align:'"/>
          <xsl:value-of select="@textalign"/>
          <xsl:value-of select="';'"/>
        </xsl:if>

      </xsl:attribute>
      <xsl:value-of select="@label"/>
    </div>
    
  </xsl:template>

  <xsl:template match="value" mode="parent">
    <div class="value">
      <xsl:attribute name="class">
        <xsl:value-of select="'value'"/>
        <xsl:if test="@ac_tipodato='boolean'">
          <xsl:value-of select="' boolvalue'"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="@y" />
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="@h" />
          <xsl:with-param name="dt" select="2" />
          <xsl:with-param name="dr" select="2" />
          <xsl:with-param name="dl" select="2" />
          <xsl:with-param name="db" select="2" />
        </xsl:call-template>
        <xsl:if test="count(@fontsize)&gt;0">
          <xsl:value-of select="'font-size:'"/>
          <xsl:value-of select="@fontsize"/>
          <xsl:value-of select="'px;'"/>
        </xsl:if>
        <xsl:if test="count(@fontweight)&gt;0">
          <xsl:value-of select="'font-weight:'"/>
          <xsl:value-of select="@fontweight"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
        <xsl:if test="count(@textalign)&gt;0">
          <xsl:value-of select="'text-align:'"/>
          <xsl:value-of select="@textalign"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:variable name="ac_datodb" select="@ac_datodb" />
      <xsl:variable name="dato" select="../specificdata/maindata/@*[name()=$ac_datodb]" />
      <!-- testo -->
      <xsl:choose>
        <xsl:when test="count($dato)=0">
          <xsl:call-template name="nbsp" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="@ac_tipodato='string'">
              <xsl:value-of select="$dato"/>
            </xsl:when>
            <xsl:when test="@ac_tipodato='int'">
              <xsl:value-of select="$dato"/>
            </xsl:when>
            <xsl:when test="@ac_tipodato='date'">
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="$dato" />
              </xsl:call-template> 
            </xsl:when>
            <xsl:when test="@ac_tipodato='money'">
              <xsl:call-template name="number_xdec">
                <xsl:with-param name="number" select="$dato" />
              </xsl:call-template>
            </xsl:when>
            <xsl:when test="@ac_tipodato='boolean'">
              <xsl:choose>
                <xsl:when test="$dato=1">
                  X
                </xsl:when>
                <xsl:otherwise>
                  <xsl:call-template name="nbsp" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </div>

  </xsl:template>

  <xsl:template match="field" mode="parent">
    <div data-controltype="parentfield">
      <xsl:attribute name="data-datatype"> 
        <xsl:value-of select="@ac_tipodato" />
      </xsl:attribute>
      <xsl:attribute name="data-itemid">
        <xsl:value-of select="@id_item" />
      </xsl:attribute>
      <xsl:attribute name="data-value">
        <xsl:choose>
          <xsl:when test="@ac_tipodato='boolean'">
            <xsl:value-of select="@fl_dato"/>
          </xsl:when>
          <xsl:when test="@ac_tipodato='string'">
            <xsl:value-of select="@tx_dato"/>
          </xsl:when>
          <xsl:when test="@ac_tipodato='int'">
            <xsl:value-of select="@ni_dato"/>
          </xsl:when>
          <xsl:when test="@ac_tipodato='date'">
            <xsl:if test="count(@dt_dato)=1">
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_dato" />
              </xsl:call-template>
            </xsl:if>
          </xsl:when>
          <xsl:when test="@ac_tipodato='money'">
            <xsl:if test="count(@mo_dato)=1">
              <xsl:call-template name="number_2dec">
                <xsl:with-param name="number" select="@mo_dato" />
              </xsl:call-template>
            </xsl:if>
          </xsl:when>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="data-label">
        <xsl:value-of select="@tx_label"/>
      </xsl:attribute>
      <xsl:attribute name="data-updatedon">
        <xsl:if test="count(@dt_aggiornamento)=1">
          <xsl:call-template name="dataoraDDMMYYYYHHMMSS">
            <xsl:with-param name="dataora" select="@dt_aggiornamento" />
          </xsl:call-template>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="data-updatedby">
        <xsl:if test="count(@tx_aggiornamento)=1">
          <xsl:value-of select="@tx_aggiornamento"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:choose>
          <xsl:when test="@ac_tipodato='boolean'">
            <xsl:value-of select="'field boolfield inactive'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'field inactive'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="@y" />
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="@h" />
          <xsl:with-param name="dt" select="2" />
          <xsl:with-param name="dr" select="2" />
          <xsl:with-param name="dl" select="2" />
          <xsl:with-param name="db" select="2" />
        </xsl:call-template>
        <xsl:if test="count(@fontsize)&gt;0">
          <xsl:value-of select="'font-size:'"/>
          <xsl:value-of select="@fontsize"/>
          <xsl:value-of select="'px;'"/>
        </xsl:if>
        <xsl:if test="count(@fontweight)&gt;0">
          <xsl:value-of select="'font-weight:'"/>
          <xsl:value-of select="@fontweight"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
        <xsl:if test="count(@textalign)&gt;0">
          <xsl:value-of select="'text-align:'"/>
          <xsl:value-of select="@textalign"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
      </xsl:attribute>
      <!-- valore -->
      <xsl:choose>
        <xsl:when test="@ac_tipodato='boolean'">
          <xsl:choose>
            <xsl:when test="@fl_dato=1">
              X
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="nbsp" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@ac_tipodato='string'">
          <xsl:choose>
            <xsl:when test="count(@tx_dato)=1">
              <xsl:value-of select="@tx_dato"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="nbsp" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@ac_tipodato='int'">
          <xsl:choose>
            <xsl:when test="count(@ni_dato)=1">
              <xsl:value-of select="@ni_dato"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="nbsp" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@ac_tipodato='date'">
          <xsl:choose>
            <xsl:when test="count(@dt_dato)=1">
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="@dt_dato" />
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="nbsp" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@ac_tipodato='money'">
          <xsl:choose>
            <xsl:when test="count(@mo_dato)=1">
              <xsl:call-template name="number_2dec">
                <xsl:with-param name="number" select="@mo_dato" />
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="nbsp" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
      </xsl:choose>
    </div>
  </xsl:template>

 
  <xsl:template match="child">
    <xsl:param name="syf" />
    
    <xsl:variable name="id_elemento_figlio" select="@id_elemento_figlio" />
    <xsl:variable name="childdata" select="../specificdata/childdata[@id_elemento_figlio=$id_elemento_figlio]" />
    
    <!-- proseguo solo se ho childdata -->
    <xsl:if test="count($childdata)&gt;0">
      <xsl:variable name="y" select="$syf + $childdata/@ni_ordine - 1" />
      <!-- box -->
      <xsl:apply-templates select="box" mode="child">
        <xsl:with-param name="y" select="$y" />
      </xsl:apply-templates>
      <!-- value -->
      <xsl:apply-templates select="value" mode="child">
        <xsl:with-param name="y" select="$y" />
        <xsl:with-param name="childdata" select="$childdata" />
      </xsl:apply-templates>
      <!-- wip -->
      <xsl:apply-templates select="wip">
        <xsl:with-param name="y" select="$y" />
        <xsl:with-param name="fl_child" select="1" />
        <xsl:with-param name="childdata" select="$childdata" />
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>

  <xsl:template match="box" mode="child">
    <xsl:param name="y" />
    <div class="box">
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="$y" />
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="1" />
          <xsl:with-param name="dt" select="0" />
          <xsl:with-param name="dr" select="0" />
          <xsl:with-param name="dl" select="0" />
          <xsl:with-param name="db" select="0" />
        </xsl:call-template>
        <xsl:if test="count(@bgcolor)&gt;0">
          <xsl:value-of select="'background-color:'"/>
          <xsl:value-of select="@bgcolor"/>
        </xsl:if>
      </xsl:attribute>
    </div>
  </xsl:template>

  <xsl:template match="value" mode="child">
    <xsl:param name="y" />
    <xsl:param name="childdata" />
    <div class="value">
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="$y" />
          <xsl:with-param name="w" select="@w" />
          <xsl:with-param name="h" select="1" />
          <xsl:with-param name="dt" select="0" />
          <xsl:with-param name="dr" select="0" />
          <xsl:with-param name="dl" select="0" />
          <xsl:with-param name="db" select="0" />
        </xsl:call-template>
        <xsl:if test="count(@fontsize)&gt;0">
          <xsl:value-of select="'font-size:'"/>
          <xsl:value-of select="@fontsize"/>
          <xsl:value-of select="'px;'"/>
        </xsl:if>
        <xsl:if test="count(@fontweight)&gt;0">
          <xsl:value-of select="'font-weight:'"/>
          <xsl:value-of select="@fontweight"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
        <xsl:if test="count(@textalign)&gt;0">
          <xsl:value-of select="'text-align:'"/>
          <xsl:value-of select="@textalign"/>
          <xsl:value-of select="';'"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:variable name="ac_datodb" select="@ac_datodb" />
      <xsl:variable name="dato" select="$childdata/@*[name()=$ac_datodb]" />
      <!-- testo -->
      <xsl:choose>
        <xsl:when test="count($dato)=0">
          <xsl:call-template name="nbsp" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="@ac_tipodato='string'">
              <xsl:value-of select="$dato"/>
            </xsl:when>
            <xsl:when test="@ac_tipodato='int'">
              <xsl:value-of select="$dato"/>
            </xsl:when>
            <xsl:when test="@ac_tipodato='date'">
              <xsl:call-template name="dataDDMMYYYY">
                <xsl:with-param name="data" select="$dato" />
              </xsl:call-template>
            </xsl:when>
            <xsl:when test="@ac_tipodato='money'">
              <xsl:call-template name="number_xdec">
                <xsl:with-param name="number" select="$dato" />
              </xsl:call-template>
            </xsl:when>
            <xsl:when test="@ac_tipodato='boolean'">
              <div class="boolvalue">
                <xsl:choose>
                  <xsl:when test="$dato=1">
                    X
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="nbsp" />
                  </xsl:otherwise>
                </xsl:choose>
              </div>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="wip">
    <xsl:param name="fl_child" />
    <xsl:param name="y" />
    <xsl:param name="childdata" />
    <div data-controltype="wip">
      <xsl:choose>
        <xsl:when test="$fl_child=0">
          <xsl:attribute name="data-itemid">
            <xsl:value-of select="@id_item"/>
          </xsl:attribute>
          <xsl:attribute name="data-childitemid">
            <xsl:value-of select="0"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="data-itemid">
            <xsl:value-of select="0"/>
          </xsl:attribute>
          <xsl:attribute name="data-childitemid">
            <xsl:value-of select="@id_item_figlio"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:attribute name="data-statuscode">
        <xsl:value-of select="@ac_status"/>
      </xsl:attribute>
      <xsl:attribute name="data-label">
        <xsl:value-of select="@tx_label"/>
      </xsl:attribute>
      <xsl:if test="$fl_child=1">
        <xsl:attribute name="data-childlabel">
          <xsl:value-of select="$childdata/@tx_figlio"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="data-expiry">
        <xsl:call-template name="dataDDMMYYYY">
          <xsl:with-param name="data" select="@dt_scadenza" />
        </xsl:call-template>
      </xsl:attribute>
      <xsl:attribute name="data-daystoexpiry">
        <xsl:value-of select="@ni_giorniscadenza"/>
      </xsl:attribute>
      <xsl:attribute name="data-updatedon">
        <xsl:if test="count(@dt_aggiornamento)=1">
          <xsl:call-template name="dataDDMMYYYY">
            <xsl:with-param name="data" select="@dt_aggiornamento" />
          </xsl:call-template>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="data-updatedby">
        <xsl:if test="count(@tx_aggiornamento)=1">
          <xsl:value-of select="@tx_aggiornamento"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:value-of select="'wip inactive wip_'"/>
        <xsl:value-of select="@ac_status"/>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:call-template name="xywh">
          <xsl:with-param name="x" select="@x" />
          <xsl:with-param name="y" select="$y" />
          <xsl:with-param name="w" select="1" />
          <xsl:with-param name="h" select="1" />
          <xsl:with-param name="dt" select="2" />
          <xsl:with-param name="dr" select="2" />
          <xsl:with-param name="dl" select="2" />
          <xsl:with-param name="db" select="2" />
        </xsl:call-template>
      </xsl:attribute>
      <!-- testo: sempre spazio -->
      <xsl:call-template name="nbsp" />
    </div>
  </xsl:template>


  <xsl:template name="wh">
    <xsl:param name="w" />
    <xsl:param name="h" />
    <xsl:value-of select="'width:'"/>
    <xsl:value-of select="format-number($w * $gx, '0')" />
    <xsl:value-of select="'px;'"/>
    <xsl:value-of select="'height:'"/>
    <xsl:value-of select="format-number($h * $gy, '0')" />
    <xsl:value-of select="'px;'"/>
  </xsl:template>
  <xsl:template name="xywh">
    <xsl:param name="x" />
    <xsl:param name="y" />
    <xsl:param name="w" />
    <xsl:param name="h" />
    <xsl:param name="dt" />
    <xsl:param name="dr" />
    <xsl:param name="dl" />
    <xsl:param name="db" />
    <xsl:value-of select="'top:'"/>
    <xsl:value-of select="format-number($y * $gy + $dt, '0')" />
    <xsl:value-of select="'px;'"/>
    <xsl:value-of select="'left:'"/>
    <xsl:value-of select="format-number($x * $gx + $dl, '0')" />
    <xsl:value-of select="'px;'"/>
    <xsl:value-of select="'width:'"/>
    <xsl:value-of select="format-number($w * $gx - $dl - $dr, '0')" />
    <xsl:value-of select="'px;'"/>
    <xsl:value-of select="'height:'"/>
    <xsl:value-of select="format-number($h * $gy - $dt - $db, '0')" />
    <xsl:value-of select="'px;'"/>
  </xsl:template>

  

</xsl:stylesheet>
