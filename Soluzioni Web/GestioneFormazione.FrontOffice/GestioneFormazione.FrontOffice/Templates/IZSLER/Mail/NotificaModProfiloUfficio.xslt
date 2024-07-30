<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:import href="../../Common.xslt"/>
    <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
    <xsl:param name="baseurl" />
    <xsl:variable name="apos">'</xsl:variable>
    <xsl:template match="/">
        <xsl:apply-templates select="notifica" />
    </xsl:template>

    <xsl:template match="notifica">
        <div>
            <xsl:choose>
                <xsl:when test="tipo_destinatario='FO'">

                    <xsl:value-of select="'Gentile utente'"/>
                    <xsl:value-of select="' '"/>
                    <b>
                        <xsl:value-of select="tx_nome"/>
                        <xsl:value-of select="' '"/>
                        <xsl:value-of select="tx_cognome"/>
                    </b>
                    <xsl:value-of select="','"/>
                    <br/>
                    <br/>
                    sono state apportate modifiche ai suoi dati
                    <xsl:value-of select="' '"/>
                    <xsl:choose>
                        <xsl:when test="new_tx_modifica='FO'">
                            dal Portale della Formazione
                        </xsl:when>
                        <xsl:otherwise>
                            dall'Ufficio della Formazione
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:when test="tipo_destinatario='BO'">
                    I dati dell'utente:
                    <br/>
                    <br/><b>Nome: </b><xsl:value-of select="tx_nome"/>
                    <br/><b>Cognome: </b><xsl:value-of select="tx_cognome"/>
                    <br/><b>Codice Fiscale: </b><xsl:value-of select="tx_codicefiscale"/>
                    <br/>
                    <br/>
                    sono stati modificati
                    <xsl:value-of select="' '"/>
                    <xsl:choose>
                        <xsl:when test="new_tx_modifica='FO'">
                            dal Portale della Formazione
                        </xsl:when>
                        <xsl:otherwise>
                            dall'Ufficio della Formazione
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                    I dati dell'utente:
                    <br/>
                    <br/><b>Nome: </b><xsl:value-of select="tx_nome"/>
                    <br/><b>Cognome: </b><xsl:value-of select="tx_cognome"/>
                    <br/><b>Codice Fiscale: </b><xsl:value-of select="tx_codicefiscale"/>
                    <br/>
                    <br/>
                    sono stati modificati
                    <xsl:value-of select="' '"/>
                    <xsl:choose>
                        <xsl:when test="new_tx_modifica='FO'">
                            dal Portale della Formazione
                        </xsl:when>
                        <xsl:otherwise>
                            dall'Ufficio della Formazione
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:otherwise>
            </xsl:choose>
            <xsl:value-of select="' in data '"/>
            <xsl:value-of select="tx_data"/>
            <xsl:value-of select="'.'"/>
            <br/><br/>
            Riepilogo modifiche:
            <br/>
            <xsl:value-of select="tx_change" disable-output-escaping="yes"/>
            <xsl:choose>
                <xsl:when test="tipo_destinatario='FO'">
                    <br/>
                    <br/>
                    <xsl:value-of select="'Distinti Saluti'"/>
                </xsl:when>
            </xsl:choose>
            <br/>
        </div>
    </xsl:template>

</xsl:stylesheet>
