Imports System.Web
Imports System.Web.Services

Public Class DOC_GEN
    Implements System.Web.IHttpHandler

    Private Const codcateg = "DOC_GEN"

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim file As String = ""
        Dim desel_fs As String = ""
        Dim exte_get As String = ""

        If context.Request("file") Is Nothing Then
            Throw404(context)
            Exit Sub
        End If

        Try
            file = context.Request("file")
            desel_fs = Left(IO.Path.GetFileNameWithoutExtension(file), 200)
            exte_get = Left(IO.Path.GetExtension(file), 16)
        Catch ex As Exception
            Throw404(context)
            Exit Sub
        End Try

        If desel_fs = String.Empty Or exte_get = String.Empty Then
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

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        dbConn.Open()

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_bin_GetBinaryFileData"
            .Parameters.Add("@desel_fs", SqlDbType.NVarChar, 200).Value = desel_fs
            .Parameters.Add("@codcateg", SqlDbType.NVarChar, 16).Value = codcateg
            .Parameters.Add("@exte_get", SqlDbType.NVarChar, 16).Value = exte_get
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then
            mimetype = dbRdr.GetString(0)
            filename = dbRdr.GetString(1)
        End If
        dbRdr.Close()
        dbCmd.Dispose()
        dbConn.Close()

        If filename = String.Empty Then
            Throw404(context)
            Exit Sub
        End If

        context.Response.ContentType = mimetype
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