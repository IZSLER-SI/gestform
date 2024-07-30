Imports Softailor.Global.SqlUtils

Public Class AttestatoECM
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim id_ISCRITTO As Integer

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'recupero id iscritto
        id_ISCRITTO = CInt(Request("i"))

        'generazione!!!
        Dim sSql = GetSql_AttestatiEcm(id_ISCRITTO)

        'generazione dataset
        Dim dst = AttestatiEcmHelper.getDatasetAttestatiEcm(dbConn, sSql)
        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        'OK ci siamo
        CreatePdfAttestatiEcm(dst)
        Response.End()

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

    Private Function GetSql_AttestatiEcm(id_ISCRITTO As Integer) As String

        Dim sOut = "SELECT * " & _
           "FROM dbo.fn_eve_AttestatiECM (" & SQL_Int32(ContextHandler.ID_AZIEN) & ", " & SQL_Int32(GecFinalContextHandler.id_EVENTO) & ") " &
           "WHERE id_ISCRITTO = " & SQL_Int32(id_ISCRITTO)
        Return sOut

    End Function

End Class