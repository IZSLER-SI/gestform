Imports Softailor.Global.SqlUtils

Public Class GestioneAttestatiPartecipazioneOnLine
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Private Sub GestioneAttestatiPartecipazioneOnLine_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'impostazione valori
        SetupData()

    End Sub

    Private Sub SetupData()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = <xml>
SELECT
	fl_ATTPARTONLINE_P_DIP,		--0
	fl_ATTPARTONLINE_P_EST,		--1
	fl_ATTPARTONLINE_DRMT_DIP,	--2
	fl_ATTPARTONLINE_DRMT_EST,	--3
    (SELECT COUNT(ID_ISCRITTO) FROM eve_ISCRITTI WHERE id_EVENTO = @id_evento AND ac_CATEGORIAECM='P' AND ac_STATOISCRIZIONE='P' AND ac_MATRICOLA is not null) as P_DIP,                         --4
    (SELECT COUNT(ID_ISCRITTO) FROM eve_ISCRITTI WHERE id_EVENTO = @id_evento AND ac_CATEGORIAECM='P' AND ac_STATOISCRIZIONE='P' AND ac_MATRICOLA is null) as P_EST,                             --5
    (SELECT COUNT(ID_ISCRITTO) FROM eve_ISCRITTI WHERE id_EVENTO = @id_evento AND ac_CATEGORIAECM IN ('D','R','M','T') AND ac_STATOISCRIZIONE='P' AND ac_MATRICOLA is not null) as DRMT_DIP,     --6
    (SELECT COUNT(ID_ISCRITTO) FROM eve_ISCRITTI WHERE id_EVENTO = @id_evento AND ac_CATEGORIAECM IN ('D','R','M','T') AND ac_STATOISCRIZIONE='P' AND ac_MATRICOLA is null) as DRMT_EST,         --7
    tx_ATTPART_HAPART,           --8
    fl_ATTPARTONLINE_T_DIP,      --9
    fl_ATTPARTONLINE_T_EST      --10
FROM
	eve_EVENTI
WHERE
	id_EVENTO = @id_evento
                           </xml>.Value
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        chkPI.Checked = dbRdr.GetBoolean(0)
        chkPE.Checked = dbRdr.GetBoolean(1)
        chkPIT.Checked = dbRdr.GetBoolean(9)
        chkPET.Checked = dbRdr.GetBoolean(10)
        chkDRMTI.Checked = dbRdr.GetBoolean(2)
        chkDRMTE.Checked = dbRdr.GetBoolean(3)

        lblNumPartecipantiInterni.Text = dbRdr.GetInt32(4).ToString
        lblNumPartecipantiEsterni.Text = dbRdr.GetInt32(5).ToString
        lblNumDRMTInterni.Text = dbRdr.GetInt32(6).ToString
        lblNumDRMTEsterni.Text = dbRdr.GetInt32(7).ToString

        If dbRdr.IsDBNull(8) Then
            txttx_ATTPART_HAPART.Text = String.Empty
        Else
            txttx_ATTPART_HAPART.Text = dbRdr.GetString(8)
        End If

        dbRdr.Close()
        dbCmd.Dispose()


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub GestioneAttestatiPartecipazioneOnLine_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'genero i riepiloghi solo se uno dei check è attivo
        If chkPI.Checked Or chkPE.Checked Or chkPIT.Checked Or chkPET.Checked Or chkDRMTI.Checked Or chkDRMTE.Checked Then

            GeneraRiepiloghi()

        End If

    End Sub

    Private Sub GestioneAttestatiPartecipazioneOnLine_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraRiepiloghi()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_part_StatoScaricamentoPART"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Transformer.Transform(dbCmd, "Templates/StatoScaricamentoPART.xslt", phdRiepiloghi)

    End Sub

    Private Sub lnkSalva_Click(sender As Object, e As EventArgs) Handles lnkSalva.Click

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = <xml>
UPDATE
    eve_EVENTI
SET
	fl_ATTPARTONLINE_P_DIP = @fl_ATTPARTONLINE_P_DIP,
	fl_ATTPARTONLINE_P_EST = @fl_ATTPARTONLINE_P_EST,
	fl_ATTPARTONLINE_DRMT_DIP = @fl_ATTPARTONLINE_DRMT_DIP,
	fl_ATTPARTONLINE_DRMT_EST = @fl_ATTPARTONLINE_DRMT_EST,
    fl_ATTPARTONLINE_T_DIP = @fl_ATTPARTONLINE_T_DIP,
	fl_ATTPARTONLINE_T_EST = @fl_ATTPARTONLINE_T_EST,
    dt_ATTPARTONLINE = GETDATE()
FROM
	eve_EVENTI
WHERE
	id_EVENTO = @id_evento
                           </xml>.Value

            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@fl_ATTPARTONLINE_P_DIP", SqlDbType.Bit).Value = chkPI.Checked
            .Parameters.Add("@fl_ATTPARTONLINE_P_EST", SqlDbType.Bit).Value = chkPE.Checked
            .Parameters.Add("@fl_ATTPARTONLINE_DRMT_DIP", SqlDbType.Bit).Value = chkDRMTI.Checked
            .Parameters.Add("@fl_ATTPARTONLINE_DRMT_EST", SqlDbType.Bit).Value = chkDRMTE.Checked
            .Parameters.Add("@fl_ATTPARTONLINE_T_DIP", SqlDbType.Bit).Value = chkPIT.Checked
            .Parameters.Add("@fl_ATTPARTONLINE_T_EST", SqlDbType.Bit).Value = chkPET.Checked

        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        'non rigenero i riepiloghi - viene fatto nel prerender


    End Sub

    Private Sub lnkAnteprimaPartecipante_Click(sender As Object, e As EventArgs) Handles lnkAnteprimaPartecipante.Click
        'generazione!!!
        Dim sSql = GetSql_AttestatiEcm("P")

        'generazione dataset
        Dim dst = AttestatiPartHelper.getDatasetAttestatiPart(dbConn, sSql)
        dbConn.Close()
        dbConn = Nothing

        'OK ci siamo
        CreatePdfAttestatiPart(dst)
        Response.End()
    End Sub

    Private Sub lnkAnteprimaDRMT_Click(sender As Object, e As EventArgs) Handles lnkAnteprimaDRMT.Click
        'generazione!!!
        Dim sSql = GetSql_AttestatiEcm("D")

        'generazione dataset
        Dim dst = AttestatiPartHelper.getDatasetAttestatiPart(dbConn, sSql)
        dbConn.Close()
        dbConn = Nothing

        'OK ci siamo
        CreatePdfAttestatiPart(dst)
        Response.End()
    End Sub


    Private Function GetSql_AttestatiEcm(ac_CATEGORIAECM As String) As String

        Dim sOut = "SELECT * " & _
           "FROM dbo.fn_eve_AttestatiECM_Preview (" &
           SQL_Int32(ContextHandler.ID_AZIEN) & ", " &
           SQL_Int32(GecFinalContextHandler.id_EVENTO) & ", " &
           SQL_String(ac_CATEGORIAECM) & ")"

        Return sOut

    End Function

    Private Sub CreatePdfAttestatiPart(dst As dstAttestatiEcm)
        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("~/Reports/rptAttestatoPartecipazione_" & My.Settings.CompanyName & ".rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
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
        Response.AddHeader("content-disposition", "attachment;  filename=" & "AttestatiPartecipazione_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss") & ".pdf")
        Response.BinaryWrite(sReader.ReadBytes(CInt(rStream.Length)))
        rStream.Dispose()
        Response.End()
    End Sub

    Private Sub lnkSavetxttx_ATTPART_HAPART_Click(sender As Object, e As EventArgs) Handles lnkSavetxttx_ATTPART_HAPART.Click

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = <xml>
UPDATE  eve_EVENTI
SET     tx_ATTPART_HAPART = @tx_ATTPART_HAPART
FROM	eve_EVENTI
WHERE	id_EVENTO = @id_evento
                           </xml>.Value

            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            If txttx_ATTPART_HAPART.Text.Trim = String.Empty Then
                .Parameters.Add("@tx_ATTPART_HAPART", SqlDbType.NVarChar, 500).Value = DBNull.Value
            Else
                .Parameters.Add("@tx_ATTPART_HAPART", SqlDbType.NVarChar, 500).Value = txttx_ATTPART_HAPART.Text.Trim
            End If
        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()


    End Sub
End Class