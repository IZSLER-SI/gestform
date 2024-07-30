Imports System.Web
Imports System.Web.Services

Public Class DOC_EVE
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim id_FILE_EVENTO As Integer = 0
       
        If context.Request("file") Is Nothing Then
            Throw404(context)
            Exit Sub
        End If

        Try
            id_FILE_EVENTO = CInt(IO.Path.GetFileNameWithoutExtension(context.Request("file")))
        Catch ex As Exception
            Throw404(context)
            Exit Sub
        End Try

        If id_FILE_EVENTO <= 0 Then
            Throw404(context)
            Exit Sub
        End If

        'ok abbiamo i dati
        '-------------------------------------
        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim filename As String = ""
        Dim mimetype As String = ""
        Dim userfilename As String = ""

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        dbConn.Open()

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_bin_GetEventFileData"
            .Parameters.Add("@id_file_evento", SqlDbType.Int).Value = id_FILE_EVENTO
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = "DOC_EVE"
            With .Parameters.Add("@id_persona", SqlDbType.Int)
                If ContextHandler.Region = ContextHandler.Regions.LoggedIn Then
                    .Value = ContextHandler.id_PERSONA
                Else
                    .Value = DBNull.Value
                End If
            End With
            .Parameters.Add("@dt_dataoggi", SqlDbType.DateTime).Value = Date.Today
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then
            mimetype = dbRdr.GetString(0)
            filename = dbRdr.GetString(1)
            userfilename = dbRdr.GetString(2)
        End If
        dbRdr.Close()
        dbCmd.Dispose()
        dbConn.Close()

        If filename = String.Empty Then
            Throw404(context)
            Exit Sub
        End If

        context.Response.ContentType = mimetype
        context.Response.AddHeader("Content-Disposition", "attachment; filename=" & userfilename)
        context.Response.TransmitFile(IO.Path.Combine(My.Settings.BinariesBasePath, filename))

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Sub Throw404(context As HttpContext)
        context.Response.StatusCode = 404
        context.Response.End()
    End Sub

End Class