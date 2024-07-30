Imports Softailor.Global.SqlUtils

Public Class AttestatoPART
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim id_ISCRITTO As Integer
        Dim dbConn As SqlConnection

        Response.Expires = 0
        Response.Cache.SetNoStore()


        'region
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Throw404()
            Exit Sub
        End If

        'verifica ID iscritto
        If Not RouteData.Values.ContainsKey("id") Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        Try
            id_ISCRITTO = CInt(RouteData.Values("id"))
        Catch ex As Exception
            id_ISCRITTO = 0
        End Try

        If id_ISCRITTO <= 0 Then
            Throw404()
            Exit Sub
        End If

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        dbConn.Open()

        'verifico che si possa scaricare l'attestato
        If Not DownloadPossibile(dbConn, id_ISCRITTO) Then
            dbConn.Close()
            dbConn.Dispose()
            Throw404()
        End If

        'generazione dell'attestato
        'generazione!!!
        Dim sSql = GetSql_AttestatiPart(id_ISCRITTO)

        'generazione dataset
        Dim dst = getDatasetAttestatiPart(dbConn, sSql)

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        'OK ci siamo
        CreatePdfAttestatiPart(dst)
        Response.End()

    End Sub


    Private Sub CreatePdfAttestatiPart(dst As dstAttestatiEcm)
        'generazione report
        Dim path As String = My.Settings.ReportsBasePath
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(IO.Path.Combine(path, "rptAttestatoPartecipazione_" & My.Settings.CompanyKey & ".rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        rpt.SetDataSource(dst)

        'esportazione report
        Dim rStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
        rpt.Close()
        rpt.Dispose()
        dst.Dispose()
        GC.Collect()

        Dim sReader As New IO.BinaryReader(rStream)
        Response.Clear()
        Response.ContentType = "application/pdf"
        Dim filename = "AttestatoPartecipazione_"
        With CType(dst.vw_gf_AttestatiECM.Rows(0), dstAttestatiEcm.vw_gf_AttestatiECMRow)
            filename &= .tx_COGNOME & "_" & .tx_NOME & "_" & .id_ISCRITTO.ToString
        End With
        filename &= ".pdf"

        Response.AddHeader("content-disposition", "attachment;  filename=" & filename)
        Response.BinaryWrite(sReader.ReadBytes(CInt(rStream.Length)))
        rStream.Dispose()
        Response.End()
    End Sub

    Private Function GetSql_AttestatiPart(id_ISCRITTO As Integer) As String

        Dim sOut = "EXEC dbo.sp_eve_fo_AttestatoECM " & SQL_Int32(id_ISCRITTO)
        Return sOut

    End Function

    Private Sub Throw404()
        Response.StatusCode = 404
        Response.End()
    End Sub

    Private Function DownloadPossibile(dbConn As SqlConnection, id_ISCRITTO As Integer) As Boolean

        Dim dbCmd As SqlCommand
        Dim prmResult As SqlParameter

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_GetAndMarkAttestatoPART"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
            prmResult = .Parameters.Add("@fl_OK", SqlDbType.Bit)
            prmResult.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        DownloadPossibile = CBool(prmResult.Value)
        dbCmd.Dispose()

    End Function

    Public Function getDatasetAttestatiPart(dbConn As SqlConnection, sql As String) As dstAttestatiEcm

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        Dim dst As New dstAttestatiEcm
        dst.EnforceConstraints = False

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = sql
        End With
        dbDad = New SqlDataAdapter(dbCmd)

        dbDad.Fill(dst.vw_gf_AttestatiECM)

        dbDad.Dispose()
        dbCmd.Dispose()

        'eventuale caricamento dei loghi - se ci sono righe
        If dst.vw_gf_AttestatiECM.Rows.Count > 0 Then

            Dim attRow As dstAttestatiEcm.vw_gf_AttestatiECMRow = CType(dst.vw_gf_AttestatiECM.Rows(0), dstAttestatiEcm.vw_gf_AttestatiECMRow)
            Dim newRow = dst.loghi.NewloghiRow

            newRow.id_EVENTO = attRow.id_EVENTO

            If Not attRow.Istx_NOMEFILELOGOORGNull Then
                newRow.LogoOrganizzatore = GetFile(attRow.tx_NOMEFILELOGOORG)
            End If
            If Not attRow.Istx_NOMEFILELOGOPRONull Then
                newRow.LogoProvider = GetFile(attRow.tx_NOMEFILELOGOPRO)
            End If
            If Not attRow.Istx_NOMEFILEFIRMANull Then
                newRow.Firma = GetFile(attRow.tx_NOMEFILEFIRMA)
            End If
            If Not attRow.Istx_NOMEFILEFIRMA2Null Then
                newRow.Firma2 = GetFile(attRow.tx_NOMEFILEFIRMA2)
            End If

            dst.loghi.Rows.Add(newRow)

        End If

        Return dst

    End Function

    Private Function GetFile(baseName As String) As Byte()

        ' Recupero il file di logo su FS
        Dim basePath As String = My.Settings.BinariesBasePath_WoAzi
        Dim filePath = IO.Path.Combine(basePath, baseName)

        Dim abLogo As Byte() = Nothing

        If IO.File.Exists(filePath) Then
            Dim fsLogo As New IO.FileStream(filePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Dim brLogo As New IO.BinaryReader(fsLogo)

            abLogo = brLogo.ReadBytes(CInt(fsLogo.Length))
            brLogo.Close()
            fsLogo.Close()
        End If

        Return abLogo

    End Function

End Class