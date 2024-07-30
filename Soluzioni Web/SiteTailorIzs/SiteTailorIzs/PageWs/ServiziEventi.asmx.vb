Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://schemas.softailor.com/ServiziEventi/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ServiziEventi
    Inherits System.Web.Services.WebService

    Public Class DatiIscrizione
        Public iscrizionePossibile As Boolean = False
        Public mostraWarning As Boolean = False
        Public ac_destinazione As String = ""
        Public testoErroreWarning As String = ""
        Public testoRichiestaConferma As String = ""
    End Class

    <WebMethod()> _
    Public Function VerificaIscrizionePersonaEsistente(id_EVENTO As Integer, id_PERSONA As Integer, ac_CATEGORIAECM As String) As DatiIscrizione

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim prmac_DESTINAZIONE As SqlParameter
        Dim datiIscrizione As New DatiIscrizione
        Dim ac_DESTINAZIONE As String

        dbConn = DbConnectionHandler.GetOpenDataDbConn

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_DestinazioneAccesso_PaxEsistente"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = id_EVENTO
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_PERSONA
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM
            prmac_DESTINAZIONE = .Parameters.Add("@ac_DESTINAZIONE", SqlDbType.NVarChar, 10)
            prmac_DESTINAZIONE.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        ac_DESTINAZIONE = prmac_DESTINAZIONE.Value.ToString
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

        Select Case ac_DESTINAZIONE
            Case "ACC_Q1", "Q2"
                'passato quando ci sono criteri e il nominativo matcha oppure quando il nominativo è docente/relatore
                With datiIscrizione
                    .iscrizionePossibile = True
                    .mostraWarning = False
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = ""
                    .testoRichiestaConferma = ""
                End With
            Case "GIA_PRES"
                'passato quando il nominativo è già presente
                With datiIscrizione
                    .iscrizionePossibile = False
                    .mostraWarning = False
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = "Nominativo già presente nell'elenco dei partecipanti con il ruolo specificato."
                    .testoRichiestaConferma = ""
                End With
            Case "NEIN"
                With datiIscrizione
                    .iscrizionePossibile = True
                    .mostraWarning = True
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = "ATTENZIONE: In base ai criteri di accesso all'evento, questo nominativo non potrebbe iscriversi"
                    .testoRichiestaConferma = "Confermi comunque l'aggiunta del nominativo?"
                End With
            Case "NO_CRIT"
                With datiIscrizione
                    .iscrizionePossibile = True
                    .mostraWarning = True
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = "ATTENZIONE: Non sono stati definiti criteri di accesso all'evento e non è pertanto possibile stabilire se il nominativo può o non può accedere."
                    .testoRichiestaConferma = "Confermi comunque l'aggiunta del nominativo?"
                End With
        End Select

        Return datiIscrizione

    End Function

    <WebMethod()> _
    Public Function VerificaIscrizionePersonaNuova(id_EVENTO As Integer, ac_PROFILO As String, ac_CATEGORIAECM As String) As DatiIscrizione

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim prmac_DESTINAZIONE As SqlParameter
        Dim datiIscrizione As New DatiIscrizione
        Dim ac_DESTINAZIONE As String

        dbConn = DbConnectionHandler.GetOpenDataDbConn

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_DestinazioneAccesso_PaxNuova"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = id_EVENTO
            .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = ac_PROFILO
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM
            prmac_DESTINAZIONE = .Parameters.Add("@ac_DESTINAZIONE", SqlDbType.NVarChar, 10)
            prmac_DESTINAZIONE.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        ac_DESTINAZIONE = prmac_DESTINAZIONE.Value.ToString
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

        Select Case ac_DESTINAZIONE
            Case "ACC_Q1", "Q2"
                'passato quando ci sono criteri e il nominativo matcha oppure quando il nominativo è docente/relatore
                With datiIscrizione
                    .iscrizionePossibile = True
                    .mostraWarning = False
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = ""
                    .testoRichiestaConferma = ""
                End With
            Case "NEIN"
                With datiIscrizione
                    .iscrizionePossibile = True
                    .mostraWarning = True
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = "ATTENZIONE: In base ai criteri di accesso all'evento, questo nominativo non potrebbe iscriversi"
                    .testoRichiestaConferma = "Confermi comunque l'aggiunta del nominativo?"
                End With
            Case "NO_CRIT"
                With datiIscrizione
                    .iscrizionePossibile = True
                    .mostraWarning = True
                    .ac_destinazione = ac_DESTINAZIONE
                    .testoErroreWarning = "ATTENZIONE: Non sono stati definiti criteri di accesso all'evento e non è pertanto possibile stabilire se il nominativo può o non può accedere."
                    .testoRichiestaConferma = "Confermi comunque l'aggiunta del nominativo?"
                End With
        End Select

        Return datiIscrizione

    End Function

End Class