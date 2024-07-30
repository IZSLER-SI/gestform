Public Class StampaAutoCertificazione
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim id_PARTECIPAZIONE As Integer
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim xDoc As XmlDocument

        Response.Expires = 0
        Response.Cache.SetNoStore()

        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Throw404()
            Exit Sub
        End If

        If Not ContextHandler.fl_DIPENDENTE Then
            Throw404()
            Exit Sub
        End If

        'verifica ID autocertificazione
        If Not RouteData.Values.ContainsKey("id") Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        Try
            id_PARTECIPAZIONE = CInt(RouteData.Values("id"))
        Catch ex As Exception
            id_PARTECIPAZIONE = 0
        End Try

        If id_PARTECIPAZIONE <= 0 Then
            Throw404()
            Exit Sub
        End If

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        dbConn.Open()

        'caricamento doc XML
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_AutocertificazionePending"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE
        End With
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        dbConn.Close()
        dbConn.Dispose()

        'trasformo oppure 404
        If xDoc.SelectNodes("/autocertificazione").Count = 0 Then
            Throw404()
            Exit Sub
        Else
            Transformer.Transform(xDoc,
                                  "~/Templates/" & My.Settings.CompanyKey & "/Autocertificazione_Formazione_Individuale.xslt",
                                  phdContent)
        End If

    End Sub

    Private Sub Throw404()
        Response.StatusCode = 404
        Response.End()
    End Sub

End Class