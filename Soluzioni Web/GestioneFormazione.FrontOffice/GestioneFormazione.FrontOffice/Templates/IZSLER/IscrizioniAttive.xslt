<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../Common.xslt"/>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="node" />
  <xsl:param name="region" />
  <xsl:param name="companyname" />

  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="count(root/iscrizione)=0">
        <div>
          <b>
            Attualmente non sei iscritto a nessun evento formativo.
          </b>
          <br/>
          Consulta
          l'<a class="classica" href="/eventi">offerta formativa</a> se vuoi iscriverti ad un evento.
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="bottom20">
          <b>Attualmente sei iscritto ai seguenti eventi formativi.</b>
          <br/>
          Clicca sul nome di un evento per visualizzare i dettagli ed il materiale o se intendi cancellare la tua iscrizione (ove possibile).
        </div>
        <xsl:apply-templates select="root/iscrizione" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
			
	<xsl:template match="/">
		<div class="title green bottom20">
				
    </div>
		<xsl:call-template name="iscrizioni_all" />
			<br/>
		<div class="title green bottom20">
				Moduli di incarico
    </div>
		<xsl:call-template name="iscrizioni_docente" />	
		
	</xsl:template>
	
	<xsl:template name="iscrizioni_all">
		<xsl:for-each select="root/iscrizione">
				<xsl:choose>
						<xsl:when test="@fl_visibilefo='1'">
						<a class="list_evento">
								<xsl:attribute name="href">
								<xsl:value-of select="'/eventi/'"/>
								<xsl:value-of select="@id_evento"/>
								</xsl:attribute>
								<xsl:call-template name="datievento" />
						</a>
						</xsl:when>
						<xsl:otherwise>
						<span class="list_evento_nc">
								<xsl:call-template name="datievento" />
						</span>
					
						</xsl:otherwise>
				</xsl:choose>
		</xsl:for-each>
  </xsl:template>
	
	<xsl:template name="iscrizioni_docente">
			<xsl:choose>
					<xsl:when test="count(root/iscrizione_docente) = (count(root/iscrizione_docente[@fl_modinc_compilato=0])+(count(root/iscrizione_docente[@fl_modinc_compilato=1]))+(count(root/iscrizione_docente[@fl_modinc_compilato=4])))">
							<div>
									<b>
											Attualmente non hai moduli di incarico da compilare.
									</b>
							</div>
					</xsl:when>
					<xsl:otherwise>
							<xsl:for-each select="root/iscrizione_docente">
									<xsl:choose>
											<xsl:when test="@fl_visibilefo='1' and @fl_modinc_compilato != 0 and @fl_modinc_compilato = 0 and @fl_modinc_compilato != 4">
													<a class="list_evento">
															<xsl:attribute name="href">
																	<xsl:value-of select="'/eventi/'"/>
																	<xsl:value-of select="@id_evento"/>
															</xsl:attribute>
															<xsl:call-template name="datievento" />
													</a>
											</xsl:when>
											<xsl:otherwise>
													<xsl:choose>
															<xsl:when test="@fl_modinc_compilato != 0 and @fl_modinc_compilato != 1 and @fl_modinc_compilato != 4">
																	<span class="list_evento_nc">
																			<xsl:call-template name="datievento_docente" />
																	</span>
															</xsl:when>
													</xsl:choose>
											</xsl:otherwise>
									</xsl:choose>
							</xsl:for-each>
					</xsl:otherwise>
			</xsl:choose>
	</xsl:template>
				
						
								
  
  <xsl:template name="datievento">
    <!-- date e sede -->
    <div>
      <b>
        <xsl:call-template name="dataDalAl_it_mmmm">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </b>
      -
      <xsl:choose>
        <xsl:when test="@fl_fad='0'">
          <xsl:value-of select="@tx_sede"/>
          <xsl:if test="@tx_dettaglisede!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_dettaglisede"/>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          Formazione a Distanza
        </xsl:otherwise>
      </xsl:choose>
    </div>
    <!-- titolo -->
    <div>
      <b>
        <xsl:value-of select="@tx_titoloevento"/>
        <xsl:if test="@tx_edizione!=''">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_edizione"/>
        </xsl:if>
      </b>
    </div>
    <!-- stato iscrizione -->
    <xsl:choose>
      <xsl:when test="@ac_statoiscrizione='I'">
        <div class="green">
          <b>Iscrizione confermata</b>
        </div>
      </xsl:when>
      <xsl:when test="@ac_statoiscrizione='LAP'">
        <div class="orange">
          <b>
            Iscrizione in lista d'attesa
            <xsl:if test="@fl_q2=1">
              prioritaria
            </xsl:if>
          </b>
          (posizione n. <xsl:value-of select="@ni_posizione"/>)
        </div>
      </xsl:when>
      <xsl:when test="@ac_statoiscrizione='LAS'">
        <div class="orange">
          <b>
            Iscrizione in lista d'attesa secondaria
          </b>
          (posizione n. <xsl:value-of select="@ni_posizione"/>)
        </div>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


	<xsl:template name="datievento_docente">
    <!-- date e sede -->
    <div>
      <b>
        <xsl:call-template name="dataDalAl_it_mmmm">
          <xsl:with-param name="dataDal" select="@dt_inizio" />
          <xsl:with-param name="dataAl" select="@dt_fine" />
        </xsl:call-template>
      </b>
      -
      <xsl:choose>
        <xsl:when test="@fl_fad='0'">
          <xsl:value-of select="@tx_sede"/>
          <xsl:if test="@tx_dettaglisede!=''">
            <xsl:value-of select="' - '"/>
            <xsl:value-of select="@tx_dettaglisede"/>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          Formazione a Distanza
        </xsl:otherwise>
      </xsl:choose>
    </div>
    <!-- titolo -->
    <div>
      <b>
        <xsl:value-of select="@tx_titoloevento"/>
        <xsl:if test="@tx_edizione!=''">
          <xsl:value-of select="' - '"/>
          <xsl:value-of select="@tx_edizione"/>
        </xsl:if>
      </b>
    </div>

			

			<div class="green">
					<b><xsl:value-of select="@tx_categoriaecm"/></b>
					<span>: </span>
					
			<xsl:choose>
				<xsl:when test="@fl_modinc_compilato='2'">
						<a class="classica">
								<xsl:attribute name="href">
										<xsl:value-of select="'/modulo-incarico/'"/>
										<xsl:value-of select="@id_iscritto"/>
								</xsl:attribute>
								Compila modulo incarico

						</a>
				</xsl:when>
				<xsl:when test="@fl_modinc_compilato='3'">
						Modulo incarico compilato (
						<a class="classica">
								<xsl:attribute name="href">
										<xsl:value-of select="'/modulo-incarico/'"/>
										<xsl:value-of select="@id_iscritto"/>
								</xsl:attribute>
								Modifica
							</a>
                            <xsl:value-of select="' | '"/>
                            <a class="classica">
                                <xsl:attribute name="href">
                                    <xsl:value-of select="'/modulo-incarico/'"/>
                                    <xsl:value-of select="@id_iscritto"/>
                                    <xsl:value-of select="'?download=1'"/>
                                </xsl:attribute>
                                Download
                            </a>)	
			  </xsl:when>
				<xsl:when test="@fl_modinc_compilato='4'">
						Modulo incarico non modificabile
				</xsl:when>
					
		
    </xsl:choose>
	
			</div>
  
  </xsl:template>


</xsl:stylesheet>
