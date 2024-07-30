Imports Softailor.Web.UI.DbForms
Imports Softailor.Global.SqlUtils

Public Class SetupAttestatoECM
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.sdsEVENTI.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsEVENTI.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME.ToString

    End Sub

    Private Sub lnkAttestatoTest_Click(sender As Object, e As System.EventArgs) Handles lnkAttestatoTest.Click
        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Per poter ottenere l\'anteprima dell\'attestato devi prima salvare o annullare le modifiche effettuate.');", True)
        Else
            'OK ci siamo
            Dim sSql = GetSql_AttestatiEcm("P")

            'generazione dataset
            Dim dbConn = DbConnectionHandler.GetOpenDataDbConn
            Dim dst = AttestatiEcmHelper.getDatasetAttestatiEcm(dbConn, sSql)
            dbConn.Close()
            dbConn.Dispose()

            'OK ci siamo
            CreatePdfAttestatiEcm(dst)
        End If
    End Sub

    Private Sub lnkAttestatoTestDocente_Click(sender As Object, e As System.EventArgs) Handles lnkAttestatoTestDocente.Click
        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Per poter ottenere l\'anteprima dell\'attestato devi prima salvare o annullare le modifiche effettuate.');", True)
        Else
            'OK ci siamo
            Dim sSql = GetSql_AttestatiEcm("D")

            'generazione dataset
            Dim dbConn = DbConnectionHandler.GetOpenDataDbConn
            Dim dst = AttestatiEcmHelper.getDatasetAttestatiEcm(dbConn, sSql)
            dbConn.Close()
            dbConn.Dispose()

            'OK ci siamo
            CreatePdfAttestatiEcm(dst)
        End If
    End Sub

    Private Sub CreatePdfAttestatiEcm(dst As dstAttestatiEcm)

        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("~/Reports/" & GecFinalContextHandler.NomeReportAttestatoECM), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
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
        Response.AddHeader("content-disposition", "attachment;  filename=" & "AttestatiEcm_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss") & ".pdf")
        Response.BinaryWrite(sReader.ReadBytes(CInt(rStream.Length)))
        rStream.Dispose()
        Response.End()

    End Sub

    Private Function GetSql_AttestatiEcm(tipo As String) As String

        Dim sOut = "SELECT * " & _
            "FROM dbo.fn_eve_AttestatiECM_Preview (" & SQL_Int32(ContextHandler.ID_AZIEN) & ", " & SQL_Int32(GecFinalContextHandler.id_EVENTO) & ", " & SQL_String(tipo) & ")"
        Return sOut

    End Function

End Class