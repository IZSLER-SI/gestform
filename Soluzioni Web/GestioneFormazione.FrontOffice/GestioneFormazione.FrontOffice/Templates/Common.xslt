<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="no" omit-xml-declaration="yes" />

  <!-- definizione per formattazione euro -->
  <xsl:decimal-format	name="euro"	decimal-separator="," grouping-separator="." />
  <xsl:decimal-format	name="quantita"	decimal-separator="," grouping-separator="." />

  <xsl:template name="dataDDMMYY">
    <xsl:param name="data" />
    <xsl:if test="$data!=''">
      <xsl:value-of select="substring($data,9,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($data,6,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($data,3,2)" />
    </xsl:if>
  </xsl:template>

  <xsl:template name="dataDDMMYYYY">
    <xsl:param name="data" />
    <xsl:if test="$data!=''">
      <xsl:value-of select="substring($data,9,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($data,6,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($data,1,4)" />
    </xsl:if>
  </xsl:template>

  <xsl:template name="dataYYYYMMDD">
    <xsl:param name="data" />
    <xsl:if test="$data!=''">
      <xsl:value-of select="substring($data,1,4)" />
      <xsl:value-of select="substring($data,6,2)" />
      <xsl:value-of select="substring($data,9,2)" />
    </xsl:if>
  </xsl:template>

  <xsl:template name="dataoraDDMMYYHHMM">
    <xsl:param name="dataora" />
    <xsl:if test="$dataora!=''">
      <xsl:value-of select="substring($dataora,9,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($dataora,6,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($dataora,3,2)" />
      <xsl:value-of select="' '" />
      <xsl:value-of select="substring($dataora,12,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($dataora,15,2)" />
    </xsl:if>
  </xsl:template>
  
  <xsl:template name="dataoraDDMMYYHHMMSS">
    <xsl:param name="dataora" />
    <xsl:value-of select="$dataora"/>
    <xsl:if test="$dataora!=''">
      <xsl:value-of select="substring($dataora,9,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($dataora,6,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($dataora,3,2)" />
      <xsl:value-of select="' '" />
      <xsl:value-of select="substring($dataora,12,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($dataora,15,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($dataora,18,2)" />
    </xsl:if>
  </xsl:template>
  
  <xsl:template name="dataoraDDMMYYYYHHMMSS">
    <xsl:param name="dataora" />
    <xsl:if test="$dataora!=''">
      <xsl:value-of select="substring($dataora,9,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($dataora,6,2)" />
      <xsl:value-of select="'/'" />
      <xsl:value-of select="substring($dataora,1,4)" />
      <xsl:value-of select="' '" />
      <xsl:value-of select="substring($dataora,12,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($dataora,15,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($dataora,18,2)" />
    </xsl:if>
  </xsl:template>

  <xsl:template name="oraHHMM">
    <xsl:param name="ora" />
    <xsl:if test="$ora!=''">
      <xsl:value-of select="substring($ora,12,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($ora,15,2)" />
    </xsl:if>
  </xsl:template>

  <xsl:template name="oraHHMM_sqltime">
    <xsl:param name="ora" />
    <xsl:if test="$ora!=''">
      <xsl:value-of select="substring($ora,1,2)" />
      <xsl:value-of select="'.'" />
      <xsl:value-of select="substring($ora,4,2)" />
    </xsl:if>
  </xsl:template>


  <xsl:template name="dataDalAl_it_mmm">
    <xsl:param name="dataDal" />
    <xsl:param name="dataAl" />
    <xsl:variable name="AI" select="substring($dataDal,1,4)" />
    <xsl:variable name="AF" select="substring($dataAl,1,4)" />
    <xsl:variable name="MIn" select="substring($dataDal,6,2)" />
    <xsl:variable name="MFn" select="substring($dataAl,6,2)" />
    <xsl:variable name="MI">
      <xsl:choose>
        <xsl:when test="$MIn='01'">Gen</xsl:when>
        <xsl:when test="$MIn='02'">Feb</xsl:when>
        <xsl:when test="$MIn='03'">Mar</xsl:when>
        <xsl:when test="$MIn='04'">Apr</xsl:when>
        <xsl:when test="$MIn='05'">Mag</xsl:when>
        <xsl:when test="$MIn='06'">Giu</xsl:when>
        <xsl:when test="$MIn='07'">Lug</xsl:when>
        <xsl:when test="$MIn='08'">Ago</xsl:when>
        <xsl:when test="$MIn='09'">Set</xsl:when>
        <xsl:when test="$MIn='10'">Ott</xsl:when>
        <xsl:when test="$MIn='11'">Nov</xsl:when>
        <xsl:when test="$MIn='12'">Dic</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="MF">
      <xsl:choose>
        <xsl:when test="$MFn='01'">Gen</xsl:when>
        <xsl:when test="$MFn='02'">Feb</xsl:when>
        <xsl:when test="$MFn='03'">Mar</xsl:when>
        <xsl:when test="$MFn='04'">Apr</xsl:when>
        <xsl:when test="$MFn='05'">Mag</xsl:when>
        <xsl:when test="$MFn='06'">Giu</xsl:when>
        <xsl:when test="$MFn='07'">Lug</xsl:when>
        <xsl:when test="$MFn='08'">Ago</xsl:when>
        <xsl:when test="$MFn='09'">Set</xsl:when>
        <xsl:when test="$MFn='10'">Ott</xsl:when>
        <xsl:when test="$MFn='11'">Nov</xsl:when>
        <xsl:when test="$MFn='12'">Dic</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="GI">
      <xsl:choose>
        <xsl:when test="substring($dataDal,9,1)='0'">
          <xsl:value-of select="substring($dataDal,10,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($dataDal,9,2)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="GF">
      <xsl:choose>
        <xsl:when test="substring($dataAl,9,1)='0'">
          <xsl:value-of select="substring($dataAl,10,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($dataAl,9,2)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="$GI=$GF and $MI=$MF and $AI=$AF">
        <xsl:value-of select="$GI" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$MI" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$AI" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$GI" />
        <xsl:if test="$MI!=$MF or $AI!=$AF">
          <xsl:value-of select="' '" />
          <xsl:value-of select="$MI" />
        </xsl:if>
        <xsl:if test="$AI!=$AF">
          <xsl:value-of select="' '" />
          <xsl:value-of select="$AI" />
        </xsl:if>
        <xsl:value-of select="'-'" />
        <xsl:value-of select="$GF" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$MF" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$AF" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="dataDalAl_it_mmmm">
    <xsl:param name="dataDal" />
    <xsl:param name="dataAl" />
    <xsl:variable name="AI" select="substring($dataDal,1,4)" />
    <xsl:variable name="AF" select="substring($dataAl,1,4)" />
    <xsl:variable name="MIn" select="substring($dataDal,6,2)" />
    <xsl:variable name="MFn" select="substring($dataAl,6,2)" />
    <xsl:variable name="MI">
      <xsl:choose>
        <xsl:when test="$MIn='01'">Gennaio</xsl:when>
        <xsl:when test="$MIn='02'">Febbraio</xsl:when>
        <xsl:when test="$MIn='03'">Marzo</xsl:when>
        <xsl:when test="$MIn='04'">Aprile</xsl:when>
        <xsl:when test="$MIn='05'">Maggio</xsl:when>
        <xsl:when test="$MIn='06'">Giugno</xsl:when>
        <xsl:when test="$MIn='07'">Luglio</xsl:when>
        <xsl:when test="$MIn='08'">Agosto</xsl:when>
        <xsl:when test="$MIn='09'">Settembre</xsl:when>
        <xsl:when test="$MIn='10'">Ottobre</xsl:when>
        <xsl:when test="$MIn='11'">Novembre</xsl:when>
        <xsl:when test="$MIn='12'">Dicembre</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="MF">
      <xsl:choose>
        <xsl:when test="$MFn='01'">Gennaio</xsl:when>
        <xsl:when test="$MFn='02'">Febbraio</xsl:when>
        <xsl:when test="$MFn='03'">Marzo</xsl:when>
        <xsl:when test="$MFn='04'">Aprile</xsl:when>
        <xsl:when test="$MFn='05'">Maggio</xsl:when>
        <xsl:when test="$MFn='06'">Giugno</xsl:when>
        <xsl:when test="$MFn='07'">Luglio</xsl:when>
        <xsl:when test="$MFn='08'">Agosto</xsl:when>
        <xsl:when test="$MFn='09'">Settembre</xsl:when>
        <xsl:when test="$MFn='10'">Ottobre</xsl:when>
        <xsl:when test="$MFn='11'">Novembre</xsl:when>
        <xsl:when test="$MFn='12'">Dicembre</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="GI">
      <xsl:choose>
        <xsl:when test="substring($dataDal,9,1)='0'">
          <xsl:value-of select="substring($dataDal,10,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($dataDal,9,2)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="GF">
      <xsl:choose>
        <xsl:when test="substring($dataAl,9,1)='0'">
          <xsl:value-of select="substring($dataAl,10,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($dataAl,9,2)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="$GI=$GF and $MI=$MF and $AI=$AF">
        <xsl:value-of select="$GI" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$MI" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$AI" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$GI" />
        <xsl:if test="$MI!=$MF or $AI!=$AF">
          <xsl:value-of select="' '" />
          <xsl:value-of select="$MI" />
        </xsl:if>
        <xsl:if test="$AI!=$AF">
          <xsl:value-of select="' '" />
          <xsl:value-of select="$AI" />
        </xsl:if>
        <xsl:value-of select="'-'" />
        <xsl:value-of select="$GF" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$MF" />
        <xsl:value-of select="' '" />
        <xsl:value-of select="$AF" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="data_it_mmm">
    <xsl:param name="data" />
    <xsl:variable name="A" select="substring($data,1,4)" />
    <xsl:variable name="MIn" select="substring($data,6,2)" />
    <xsl:variable name="M">
      <xsl:choose>
        <xsl:when test="$MIn='01'">Gen</xsl:when>
        <xsl:when test="$MIn='02'">Feb</xsl:when>
        <xsl:when test="$MIn='03'">Mar</xsl:when>
        <xsl:when test="$MIn='04'">Apr</xsl:when>
        <xsl:when test="$MIn='05'">Mag</xsl:when>
        <xsl:when test="$MIn='06'">Giu</xsl:when>
        <xsl:when test="$MIn='07'">Lug</xsl:when>
        <xsl:when test="$MIn='08'">Ago</xsl:when>
        <xsl:when test="$MIn='09'">Set</xsl:when>
        <xsl:when test="$MIn='10'">Ott</xsl:when>
        <xsl:when test="$MIn='11'">Nov</xsl:when>
        <xsl:when test="$MIn='12'">Dic</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="G">
      <xsl:choose>
        <xsl:when test="substring($data,9,1)='0'">
          <xsl:value-of select="substring($data,10,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($data,9,2)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="$G" />
    <xsl:value-of select="' '" />
    <xsl:value-of select="$M" />
    <xsl:value-of select="' '" />
    <xsl:value-of select="$A" />
  </xsl:template>

  <xsl:template name="data_it_mmmm">
    <xsl:param name="data" />
    <xsl:variable name="A" select="substring($data,1,4)" />
    <xsl:variable name="MIn" select="substring($data,6,2)" />
    <xsl:variable name="M">
      <xsl:choose>
        <xsl:when test="$MIn='01'">Gennaio</xsl:when>
        <xsl:when test="$MIn='02'">Febbraio</xsl:when>
        <xsl:when test="$MIn='03'">Marzo</xsl:when>
        <xsl:when test="$MIn='04'">Aprile</xsl:when>
        <xsl:when test="$MIn='05'">Maggio</xsl:when>
        <xsl:when test="$MIn='06'">Giugno</xsl:when>
        <xsl:when test="$MIn='07'">Luglio</xsl:when>
        <xsl:when test="$MIn='08'">Agosto</xsl:when>
        <xsl:when test="$MIn='09'">Settembre</xsl:when>
        <xsl:when test="$MIn='10'">Ottobre</xsl:when>
        <xsl:when test="$MIn='11'">Novembre</xsl:when>
        <xsl:when test="$MIn='12'">Dicembre</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="G">
      <xsl:choose>
        <xsl:when test="substring($data,9,1)='0'">
          <xsl:value-of select="substring($data,10,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($data,9,2)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="$G" />
    <xsl:value-of select="' '" />
    <xsl:value-of select="$M" />
    <xsl:value-of select="' '" />
    <xsl:value-of select="$A" />
  </xsl:template>

  <xsl:template name="nbsp">
    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
  </xsl:template>

  <xsl:template name="number_xdec">
    <xsl:param name="number" />
    <!-- formattazione di un numero con X decimali (vengono inseriti solo gli esistenti - scrive 0 se stringa vuota) -->
    <xsl:choose>
      <xsl:when test="$number!=''">
        <xsl:value-of select="format-number($number,'#.##0,####','quantita')" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'0'"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template name="number_euro">
    <xsl:param name="number" />
    <!-- formattazione di un numero con € e 2 decimali -->
    <xsl:value-of select="'€ '"/>
    <xsl:value-of select="format-number($number,'#.##0,00','euro')" />
  </xsl:template>
  
  <xsl:template name="number_2dec">
    <xsl:param name="number" />
    <!-- formattazione di un numero con 2 decimali -->
    <xsl:value-of select="format-number($number,'#.##0,00','euro')" />
  </xsl:template>
</xsl:stylesheet>
