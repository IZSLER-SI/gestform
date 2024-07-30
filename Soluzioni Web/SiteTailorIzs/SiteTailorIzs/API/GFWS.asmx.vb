Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://schemas.softailor.com/GfWs")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class GFWS
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function StoreParticipantQualitySurveyOpenAnswer(username As String, password As String, id_evento As Integer, id_domanda As Integer, tx_risposta As String, tx_domanda As String, dt_compilazione As String) As Boolean

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_SetParticipantQualitySurveysOpenAnswer"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.NVarChar, 50).Value = id_evento
            .Parameters.Add("@id_domanda", SqlDbType.NVarChar, 50).Value = id_domanda
            .Parameters.Add("@tx_domanda", SqlDbType.NVarChar, 4000).Value = tx_domanda
            .Parameters.Add("@tx_risposta", SqlDbType.NVarChar, 4000).Value = tx_risposta
            .Parameters.Add("@dt_compilazione", SqlDbType.DateTime).Value = New SqlDateTime(Date.ParseExact(dt_compilazione, "yyyyMMddHHmmss", Softailor.Global.Cultures.CulturaItalian))

            retPrm = .Parameters.Add("@retValue", SqlDbType.Int)
            retPrm.Direction = ParameterDirection.ReturnValue
        End With
        dbCmd.ExecuteNonQuery()
        StoreParticipantQualitySurveyOpenAnswer = CInt(retPrm.Value) = 1
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Function



    <WebMethod()>
    Public Function DeactivateEvent(username As String, password As String, id_evento As Integer) As Boolean

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_SuspendEventValutazioneWeb"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.NVarChar, 50).Value = id_evento


            retPrm = .Parameters.Add("@retValue", SqlDbType.Int)
            retPrm.Direction = ParameterDirection.ReturnValue
        End With
        dbCmd.ExecuteNonQuery()
        DeactivateEvent = CInt(retPrm.Value) = 1
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Function







    <WebMethod()> _
    Public Function AuthenticateUser(username As String, password As String) As Boolean

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_AuthenticateUser"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            retPrm = .Parameters.Add("@retValue", SqlDbType.Int)
            retPrm.Direction = ParameterDirection.ReturnValue
        End With
        dbCmd.ExecuteNonQuery()
        AuthenticateUser = CInt(retPrm.Value) = 1
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Function

    <WebMethod()> _
    Public Function GetCoursesList(username As String, password As String, daysOffset As Integer) As CoursesList

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_GetCoursesList"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@day_offs", SqlDbType.Int).Value = daysOffset
        End With

        'istanzio variabili
        Dim result As New CoursesList
        Dim dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim c As New Course
            c.id_EVENTO = dbRdr.GetInt32(0)
            c.tx_TITOLO = dbRdr.GetString(1)
            c.dt_INIZIO = dbRdr.GetDateTime(2)
            c.dt_FINE = dbRdr.GetDateTime(3)
            result.Add(c)
        Loop

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

    <WebMethod()> _
    Public Function GetConfirmedParticipantsList(username As String, password As String, id_evento As Integer) As ParticipantsList

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_GetConfirmedParticipantsList"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
        End With

        'istanzio variabili
        Dim result As New ParticipantsList
        Dim dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim p As New Participant
            p.id_ISCRITTO = dbRdr.GetInt32(0)
            p.tx_TITOLO = dbRdr.GetString(1)
            p.tx_COGNOME = dbRdr.GetString(2)
            p.tx_NOME = dbRdr.GetString(3)
            p.tx_ORDINE = dbRdr.GetString(4)
            p.ac_ISCRIZIONE_ORDINE = dbRdr.GetString(5)
            p.ac_CODICEFISCALE = dbRdr.GetString(6)
            p.ac_BARCODE = Softailor.Global.EpsonItfBarcode.CongressBC2of5_Number(p.id_ISCRITTO, GecFinalContextHandler.BarcodeStarter)
            result.Add(p)
        Loop

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

    <WebMethod()> _
    Public Function GetConfirmedParticipantFromBarcode(username As String, password As String, id_evento As Integer, barcode As String) As ParticipantsList

        'risultato
        Dim result As New ParticipantsList

        'se ho qualcosa...
        If barcode <> String.Empty Then

            'variabili per chiamata
            Dim id_ISCRITTOSqlInt32 As SqlInt32
            Dim ac_CODICEFISCALESqlString As SqlString

            'vediamo se è un barcode valido...
            Dim readid_ISCRITTO = Softailor.Global.EpsonItfBarcode.IdFromBarcode(barcode.Trim, GecFinalContextHandler.BarcodeStarter)

            If readid_ISCRITTO > 0 Then
                'OK è un barcode valido > si va sull'ID
                id_ISCRITTOSqlInt32 = New SqlInt32(readid_ISCRITTO)
                ac_CODICEFISCALESqlString = SqlString.Null
            Else
                'non è un barcode valido > al limite è un CF
                id_ISCRITTOSqlInt32 = SqlInt32.Null
                ac_CODICEFISCALESqlString = Left(barcode.Trim, 16)
            End If

            'apertura connessione
            Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

            'esecuzione
            Dim dbCmd = dbConn.CreateCommand

            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_gfws_GetConfirmedParticipantFromIdOrFiscalCode"
                .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
                .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
                .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
                .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_ISCRITTOSqlInt32
                .Parameters.Add("@ac_codicefiscale", SqlDbType.NVarChar, 16).Value = ac_CODICEFISCALESqlString
            End With

            'istanzio variabili
            Dim dbRdr = dbCmd.ExecuteReader
            Do While dbRdr.Read
                Dim p As New Participant
                p.id_ISCRITTO = dbRdr.GetInt32(0)
                p.tx_TITOLO = dbRdr.GetString(1)
                p.tx_COGNOME = dbRdr.GetString(2)
                p.tx_NOME = dbRdr.GetString(3)
                p.tx_ORDINE = dbRdr.GetString(4)
                p.ac_ISCRIZIONE_ORDINE = dbRdr.GetString(5)
                p.ac_CODICEFISCALE = dbRdr.GetString(6)
                p.ac_BARCODE = Softailor.Global.EpsonItfBarcode.CongressBC2of5_Number(p.id_ISCRITTO, GecFinalContextHandler.BarcodeStarter)
                result.Add(p)
            Loop

            dbRdr.Close()
            dbCmd.Dispose()

            'chiusura connessione
            dbConn.Close()
            dbConn.Dispose()

        End If




        Return result

    End Function

    <WebMethod()> _
    Public Function StoreSurveyStructure(username As String, password As String, id_evento As Integer, questionCount As Integer, minimumRightAnswers As Integer, correctAnswersSequence As String) As Boolean
        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_SetSurveyStructure"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
            .Parameters.Add("@ni_risposte", SqlDbType.Int).Value = questionCount
            .Parameters.Add("@ni_minimorisposte", SqlDbType.Int).Value = minimumRightAnswers
            .Parameters.Add("@ac_sequenzarisposte", SqlDbType.NVarChar, 200).Value = Left(correctAnswersSequence, 200)
            retPrm = .Parameters.Add("@retValue", SqlDbType.Int)
            retPrm.Direction = ParameterDirection.ReturnValue
        End With
        dbCmd.ExecuteNonQuery()
        StoreSurveyStructure = CInt(retPrm.Value) = 1
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()
    End Function

    <WebMethod()> _
    Public Function StoreParticipantAnswers(username As String,
                                            password As String,
                                            id_evento As Integer,
                                            id_iscritto As Integer,
                                            dataOraCompilazioneYYYYMMDDHHMMSS As String,
                                            answersSequence As String) As Boolean
        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_SetParticipantAnswers"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
            .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_iscritto
            .Parameters.Add("@dt_compilazione", SqlDbType.DateTime).Value = Date.ParseExact(dataOraCompilazioneYYYYMMDDHHMMSS, "yyyyMMddHHmmss", Softailor.Global.Cultures.CulturaItalian)
            .Parameters.Add("@ac_sequenzarisposte", SqlDbType.NVarChar, 200).Value = Left(answersSequence, 200)
            retPrm = .Parameters.Add("@retValue", SqlDbType.Int)
            retPrm.Direction = ParameterDirection.ReturnValue
        End With
        dbCmd.ExecuteNonQuery()
        StoreParticipantAnswers = CInt(retPrm.Value) = 1
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Function

    <WebMethod()> _
    Public Function StoreParticipantQualitySurvey(username As String,
                                                  password As String,
                                                  id_evento As Integer,
                                                  id_iscritto As Integer,
                                                  dataOraCompilazioneYYYYMMDDHHMMSS As String,
                                                  answersSequence As String) As Boolean
        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_SetParticipantQualitySurveys"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
            .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_iscritto
            .Parameters.Add("@dt_compilazione", SqlDbType.DateTime).Value = Date.ParseExact(dataOraCompilazioneYYYYMMDDHHMMSS, "yyyyMMddHHmmss", Softailor.Global.Cultures.CulturaItalian)
            .Parameters.Add("@ac_sequenzarisposte", SqlDbType.NVarChar, 200).Value = Left(answersSequence, 200)
            retPrm = .Parameters.Add("@retValue", SqlDbType.Int)
            retPrm.Direction = ParameterDirection.ReturnValue
        End With
        dbCmd.ExecuteNonQuery()
        StoreParticipantQualitySurvey = CInt(retPrm.Value) = 1
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()
    End Function

    <WebMethod()> _
    Public Function StoreParticipantEntryExitList(username As String, password As String, id_evento As Integer, participantEntryExitList As ParticipantEntryExitList) As String

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand
        Dim retPrm As SqlParameter

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfws_StoreParticipantEntryExitList"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
            .Parameters.Add("@list", SqlDbType.Structured).Value = participantEntryExitList.GetDataTable
            retPrm = .Parameters.Add("@retValue", SqlDbType.NVarChar, -1)
            retPrm.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        StoreParticipantEntryExitList = CStr(retPrm.Value)
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()
    End Function

#Region "ValutazioneWeb"
    <WebMethod()> _
    Public Function ValutazioneWeb_GetCoursesList(username As String, password As String, daysOffset As Integer) As CoursesList

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfwsvweb_GetCoursesList"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@day_offs", SqlDbType.Int).Value = daysOffset
        End With

        'istanzio variabili
        Dim result As New CoursesList
        Dim dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim c As New Course
            c.id_EVENTO = dbRdr.GetInt32(0)
            c.tx_TITOLO = dbRdr.GetString(1)
            c.dt_INIZIO = dbRdr.GetDateTime(2)
            c.dt_FINE = dbRdr.GetDateTime(3)
            result.Add(c)
        Loop

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

    <WebMethod()> _
    Public Function ValutazioneWeb_GetPresentParticipantsList(username As String, password As String, id_evento As Integer) As ValutazioneWebParticipantsList

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfwsvweb_GetPresentParticipantsList"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
        End With

        'istanzio variabili
        Dim result As New ValutazioneWebParticipantsList
        Dim dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim p As New ValutazioneWebParticipant
            p.id_ISCRITTO = dbRdr.GetInt32(0)
            p.ac_CODICEFISCALE = dbRdr.GetString(1)
            p.tx_COGNOME = dbRdr.GetString(2)
            p.tx_NOME = dbRdr.GetString(3)
            p.tx_EMAIL = dbRdr.GetString(4)
            result.Add(p)
        Loop

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

    <WebMethod()> _
    Public Function ValutazioneWeb_GetSpeakersList(username As String, password As String, id_evento As Integer) As ValutazioneWebSpeakersList

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfwsvweb_GetSpeakersList"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
        End With

        'istanzio variabili
        Dim result As New ValutazioneWebSpeakersList
        Dim dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim p As New ValutazioneWebSpeaker
            p.ni_ORDINE = dbRdr.GetInt32(0)
            p.tx_COGNOME = dbRdr.GetString(1)
            p.tx_NOME = dbRdr.GetString(2)
            result.Add(p)
        Loop

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

    <WebMethod()> _
    Public Sub ValutazioneWeb_SetSurveyTimeRange(username As String, password As String, id_evento As Integer, dataOraInizio As String, dataOraFine As String, apprendimento As String, qualita As String)

        Dim sqlDataOraInizio As SqlDateTime = SqlDateTime.Null
        Dim sqlDataOraFine As SqlDateTime = SqlDateTime.Null

        If dataOraInizio <> "" Then
            sqlDataOraInizio = New SqlDateTime(Date.ParseExact(dataOraInizio, "yyyyMMddHHmm", Softailor.Global.Cultures.CulturaItalian))
        End If
        If dataOraFine <> "" Then
            sqlDataOraFine = New SqlDateTime(Date.ParseExact(dataOraFine, "yyyyMMddHHmm", Softailor.Global.Cultures.CulturaItalian))
        End If

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenWsDbConn

        'esecuzione
        Dim dbCmd = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfwsvweb_SetSurveyTimeRange"
            .Parameters.Add("@username", SqlDbType.NVarChar, 50).Value = username
            .Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_evento
            .Parameters.Add("@vweb_dt_inizio", SqlDbType.DateTime).Value = sqlDataOraInizio
            .Parameters.Add("@vweb_dt_fine", SqlDbType.DateTime).Value = sqlDataOraFine
            .Parameters.Add("@vweb_fl_apprendimento", SqlDbType.Bit).Value = apprendimento = "1"
            .Parameters.Add("@vweb_fl_qualita", SqlDbType.Bit).Value = qualita = "1"
        End With
        dbCmd.ExecuteNonQuery()

        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Sub
#End Region

End Class


#Region "Classi di servizio"

Public Class CoursesList
    Inherits List(Of Course)
End Class

Public Class Course
    Public id_EVENTO As Integer
    Public tx_TITOLO As String
    Public dt_INIZIO As Date
    Public dt_FINE As Date
End Class

Public Class ParticipantsList
    Inherits List(Of Participant)
End Class

Public Class Participant
    Public id_ISCRITTO As Integer
    Public tx_TITOLO As String
    Public tx_COGNOME As String
    Public tx_NOME As String
    Public tx_ORDINE As String
    Public ac_ISCRIZIONE_ORDINE As String
    Public ac_CODICEFISCALE As String
    Public ac_BARCODE As String
End Class

Public Class ValutazioneWebParticipantsList
    Inherits List(Of ValutazioneWebParticipant)
End Class

Public Class ValutazioneWebSpeakersList
    Inherits List(Of ValutazioneWebSpeaker)
End Class

Public Class ValutazioneWebParticipant
    Public id_ISCRITTO As Integer
    Public ac_CODICEFISCALE As String
    Public tx_COGNOME As String
    Public tx_NOME As String
    Public tx_EMAIL As String
End Class

Public Class ValutazioneWebSpeaker
    Public ni_ORDINE As Integer
    Public tx_COGNOME As String
    Public tx_NOME As String
End Class

Public Class ParticipantEntryExit
    Public id_ISCRITTO As Integer
    Public tx_COGNOME As String
    Public tx_NOME As String
    Public dt_DATA As Date
    Public tm_INIZIO As Date
    Public tm_FINE As Date
End Class

Public Class ParticipantEntryExitList
    Inherits List(Of ParticipantEntryExit)

    Public Function GetDataTable() As DataTable

        Dim dtDYN As New DataTable

        Dim dcDYN As DataColumn

        ' Creazione datatable corrispondente al TableDataType di SQL 
        dcDYN = New DataColumn()
        dcDYN.ColumnName = "id_ISCRITTO"
        dcDYN.AllowDBNull = False
        dcDYN.DataType = Type.GetType("System.Int32")
        dtDYN.Columns.Add(dcDYN)

        dcDYN = New DataColumn()
        dcDYN.ColumnName = "tx_COGNOME"
        dcDYN.AllowDBNull = False
        dcDYN.DataType = Type.GetType("System.String")
        dtDYN.Columns.Add(dcDYN)

        dcDYN = New DataColumn()
        dcDYN.ColumnName = "tx_NOME"
        dcDYN.AllowDBNull = False
        dcDYN.DataType = Type.GetType("System.String")
        dtDYN.Columns.Add(dcDYN)

        dcDYN = New DataColumn()
        dcDYN.ColumnName = "dt_DATA"
        dcDYN.AllowDBNull = False
        dcDYN.DataType = GetType(Date)
        dtDYN.Columns.Add(dcDYN)

        dcDYN = New DataColumn()
        dcDYN.ColumnName = "tm_INIZIO"
        dcDYN.AllowDBNull = False
        dcDYN.DataType = GetType(Date)
        dtDYN.Columns.Add(dcDYN)

        dcDYN = New DataColumn()
        dcDYN.ColumnName = "tm_FINE"
        dcDYN.AllowDBNull = False
        dcDYN.DataType = GetType(Date)
        dtDYN.Columns.Add(dcDYN)

        'riempimento della tabella
        For Each pee In Me

            Dim newRow = dtDYN.NewRow
            With pee
                newRow("id_ISCRITTO") = pee.id_ISCRITTO
                newRow("tx_COGNOME") = Left(pee.tx_COGNOME, 100)
                newRow("tx_NOME") = Left(pee.tx_NOME, 100)
                newRow("dt_DATA") = pee.dt_DATA
                newRow("tm_INIZIO") = pee.tm_INIZIO
                newRow("tm_FINE") = pee.tm_FINE
            End With
            dtDYN.Rows.Add(newRow)

        Next

        Return dtDYN

    End Function

End Class

#End Region