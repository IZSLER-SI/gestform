Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://schemas.softailor.com/gestioneformazione/GfWs")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class GfWs
    Inherits System.Web.Services.WebService

    Public Class AutenticazioneUtenteResult
        Public esito As Integer = 0
        Public id_persona As Integer = 0
        Public cognome As String = ""
        Public nome As String = ""
        Public email As String = ""
        Public codicefiscale As String = ""
    End Class

    <WebMethod()> _
    Public Function AutenticazioneUtente(username As String, password As String) As AutenticazioneUtenteResult

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'predisposizione risultato
        Dim result As New AutenticazioneUtenteResult

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("WsConnectionString").ConnectionString)
        dbConn.Open()

        'preparazione comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfwsfo_AuthenticateUser"
            .Parameters.Add("@ac_username", SqlDbType.NVarChar, 50).Value = Left(username, 50)
            .Parameters.Add("@tx_password", SqlDbType.NVarChar, 50).Value = Left(password, 50)
        End With

        'lettura risultati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        result.esito = dbRdr.GetInt32(0)
        result.id_persona = dbRdr.GetInt32(1)
        result.cognome = dbRdr.GetString(2)
        result.nome = dbRdr.GetString(3)
        result.email = dbRdr.GetString(4)

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

    <WebMethod()> _
    Public Function PermessoAccessoCorsoFAD(id_corso_atutor As Integer, id_persona As Integer) As Boolean

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'predisposizione risultato
        Dim result = False

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("WsConnectionString").ConnectionString)
        dbConn.Open()

        'preparazione comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_gfwsfo_UserAccessPermission"
            .Parameters.Add("@id_corso_atutor", SqlDbType.Int).Value = id_corso_atutor
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = id_persona
        End With

        'lettura risultati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        result = dbRdr.GetBoolean(0)

        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return result

    End Function

End Class