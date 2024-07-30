Imports OfficeOpenXml
Imports System.IO
Imports Softailor.Global.XmlParser


Public Class GestioneQuestionariQualita
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'parametri std
        Me.sdsQUESTIONARIQUALITA_g.DeleteParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsQUESTIONARIQUALITA_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

        'gestione degli script
        If Not Page.IsPostBack Then
            Dim sScript = "function editQuestionario_callback(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkReposition, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            sScript &= "function addQuestionario_callback(codice) { if(codice!='') {" & vbCrLf
            sScript &= "$get('" & txtAfterNew.ClientID & "').value=codice;" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkAfterNew, "").Replace("javascript:", "")
            sScript &= "}}" & vbCrLf
            Me.ltrRepositioning.Text = sScript
        End If

        If Not Page.IsPostBack Then
            'generazione risultati
            GeneraRisultati()
        End If

    End Sub

    Private Sub grdQUESTIONARIQUALITA_RowDeleted(sender As Object, e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles grdQUESTIONARIQUALITA.RowDeleted

        'generazione risultati
        GeneraRisultati()

    End Sub

    Private Sub grdQUESTIONARIQUALITA_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdQUESTIONARIQUALITA.SelectedIndexChanged

        If grdQUESTIONARIQUALITA.SelectedIndex <> -1 Then

            Dim id_QUESTIONARIOQUALITA As String = grdQUESTIONARIQUALITA.SelectedDataKey.Value.ToString
            Dim sScript = "OpenQuestionario('" & id_QUESTIONARIOQUALITA & "');"

            'scrivo il valore
            txtReposition.Text = id_QUESTIONARIOQUALITA
            updHiddenCtls.Update()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editquest", sScript, True)

        End If

    End Sub

    Private Sub lnkReposition_Click(sender As Object, e As System.EventArgs) Handles lnkReposition.Click
        'deseleziono
        grdQUESTIONARIQUALITA.SelectedIndex = -1
        'forzo ricerca
        grdQUESTIONARIQUALITA.DataBind()
        grdQUESTIONARIQUALITA.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdQUESTIONARIQUALITA.DataKeys.Count - 1
            If grdQUESTIONARIQUALITA.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdQUESTIONARIQUALITA.SelectedIndex = sIdx
            grdQUESTIONARIQUALITA.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)
        End If

        'generazione risultati
        GeneraRisultati()

    End Sub

    Private Sub lnkAfterNew_Click(sender As Object, e As System.EventArgs) Handles lnkAfterNew.Click

        'valori
        Dim newid_QUESTIONARIOQUALITA As Integer = CInt(txtAfterNew.Text)
        Dim positioned = False


        'deseleziono
        grdQUESTIONARIQUALITA.SelectedIndex = -1
        'forzo ricerca
        grdQUESTIONARIQUALITA.DataBind()
        grdQUESTIONARIQUALITA.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdQUESTIONARIQUALITA.DataKeys.Count - 1
            If grdQUESTIONARIQUALITA.DataKeys(cIdx).Value.ToString = newid_QUESTIONARIOQUALITA.ToString Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdQUESTIONARIQUALITA.SelectedIndex = sIdx
            grdQUESTIONARIQUALITA.EnsureSelectedRowVisible()
            positioned = True
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappearednew", "window.alert('Il nominativo appena inserito non rispetta i criteri di filtro impostati, e non è pertanto visualizzato.');", True)
        End If

        'generazione risultati
        GeneraRisultati()

    End Sub

    Private Sub GeneraRisultati()

        Dim dbConn = DbConnectionHandler.GetOpenDataDbConn

        Dim dbcmd = dbConn.CreateCommand
        With dbcmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_StatisticheQualita"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Transformer.Transform(dbcmd, "Templates/StatisticheQualita.xslt", phdRisultati)
        updRisultati.Update()

        dbConn.Close()
        dbConn.Dispose()

    End Sub

    Private Sub lnkStampaPdf_Click(sender As Object, e As System.EventArgs) Handles lnkStampaPdf.Click

        'generazione dataset
        Dim dbConn = DbConnectionHandler.GetOpenDataDbConn
        Dim dbDad As SqlDataAdapter
        Dim dbcmd = dbConn.CreateCommand
        With dbcmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_GetQuestionarioQualitaDataset"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_azien", SqlDbType.Int).Value = ContextHandler.ID_AZIEN
        End With

        dbDad = New SqlDataAdapter(dbcmd)
        dbDad.TableMappings.Add("Table", "evento")
        dbDad.TableMappings.Add("Table1", "relatori")

        Dim dstValutazioneQualita As New dstValutazioneQualita
        dstValutazioneQualita.EnforceConstraints = False
        dbDad.Fill(dstValutazioneQualita)

        dbConn.Close()
        dbConn.Dispose()

        'gestione date
        If dstValutazioneQualita.evento.Rows.Count = 1 Then
            With CType(dstValutazioneQualita.evento.Rows(0), dstValutazioneQualita.eventoRow)
                .tx_INIZIOFINE = Softailor.Global.DateUtils.DataDalAl(.dt_INIZIO, .dt_FINE)
                If Not .Istx_NOMEFILELOGOORGNull Then
                    Dim fileLogo = IO.Path.Combine(ContextHandler.BinariesBasePath, .tx_NOMEFILELOGOORG)
                    If IO.File.Exists(fileLogo) Then
                        Dim fsFile As New IO.FileStream(fileLogo, IO.FileMode.Open, IO.FileAccess.Read)
                        Dim brFile As New IO.BinaryReader(fsFile)
                        .logoOrganizzatore = brFile.ReadBytes(CInt(fsFile.Length))
                        brFile.Close()
                        fsFile.Close()
                        fsFile.Dispose()
                    End If
                End If
            End With
        End If

        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("~/Reports/rptValutazioneQualita.rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        rpt.SetDataSource(dstValutazioneQualita)

        'stampa report
        rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "ValutazioneQualita_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss"))
        GC.Collect()
        rpt.Close()
        rpt.Dispose()
        dstValutazioneQualita.Dispose()


    End Sub

    Private Sub risultati_completi(sender As Object, e As System.EventArgs) Handles uxSearch.Click
        'generazione dataset

        Response.Clear()
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Dim dbConn = DbConnectionHandler.GetOpenDataDbConn

        'Dim xDoc = LeggiDati(GecFinalContextHandler.id_EVENTO)
        Dim dbCmd As SqlCommand

        Dim dbRdr As SqlDataReader


        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_StatisticheQualitaExport"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        'sXml = Transformer.Transform(dbCmd, "Templates/StatisticheQualitaExportFile.xslt", "companyname", "Izsler")
        dbRdr = dbCmd.ExecuteReader
        Dim xlp As ExcelPackage
        Dim xlw As ExcelWorksheet
        xlp = New ExcelPackage(New FileInfo(Server.MapPath("/Files/QuestionarioResult.xlsx")))
        xlw = xlp.Workbook.Worksheets(1)

        Do While dbRdr.Read
            xlw.Cells(1, 1).Value = dbRdr.GetString(22) & " Data inizio: " & dbRdr.GetDateTime(23).ToString("dd-MM-yyyy")
            '------------------------------------------------------------------------------------------
            xlw.Cells(3, 1).Value = "Numero iscritti"
            xlw.Cells(3, 2).Value = dbRdr.GetInt32(3).ToString
            xlw.Cells(3, 3).Value = "(esclusi relatori, docenti, etc.)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(4, 1).Value = "Numero questionari inseriti"
            xlw.Cells(4, 2).Value = dbRdr.GetInt32(4).ToString

            '------------------------------------------------------------------------------------------
            xlw.Cells(5, 1).Value = "Rilevanza degli argomenti trattati"
            If dbRdr.IsDBNull(5) Then
                xlw.Cells(5, 2).Value = "0"
            Else
                xlw.Cells(5, 2).Value = dbRdr.GetSqlMoney(5).ToString
            End If
            xlw.Cells(5, 3).Value = "Media(min 1, max 5)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(6, 1).Value = "Qualità educativa dell'evento"
            If dbRdr.IsDBNull(5) Then
                xlw.Cells(6, 2).Value = "0"
            Else
                xlw.Cells(6, 2).Value = dbRdr.GetSqlMoney(6).ToString
            End If
            xlw.Cells(6, 3).Value = "Media(min 1, max 5)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(7, 1).Value = "Utilità dell'evento"
            If dbRdr.IsDBNull(6) Then
                xlw.Cells(7, 2).Value = "0"
            Else
                xlw.Cells(7, 2).Value = dbRdr.GetSqlMoney(7).ToString
            End If
            xlw.Cells(7, 3).Value = "Media(min 1, max 5)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(8, 1).Value = "Influenza dello sponsor"
            If dbRdr.IsDBNull(7) Then
                xlw.Cells(8, 2).Value = "0"
            Else
                xlw.Cells(8, 2).Value = dbRdr.GetSqlMoney(8).ToString
            End If
            xlw.Cells(8, 3).Value = "Media(min 1, max 5)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(9, 1).Value = "Valutazione materiale di supporto"
            If dbRdr.IsDBNull(8) Then
                xlw.Cells(9, 2).Value = "0"
            Else
                xlw.Cells(9, 2).Value = dbRdr.GetSqlMoney(9).ToString
            End If
            xlw.Cells(9, 3).Value = "Media(min 1, max 5)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(10, 1).Value = "Soddisfazione aspettative argomenti trattati"
            If dbRdr.IsDBNull(9) Then
                xlw.Cells(10, 2).Value = "0"
            Else
                xlw.Cells(10, 2).Value = dbRdr.GetSqlMoney(10).ToString
            End If
            xlw.Cells(10, 3).Value = "Media(min 1, max 5)"

            '------------------------------------------------------------------------------------------
            xlw.Cells(11, 1).Value = "Consiglierebbe il corso ad altri colleghi"
            xlw.Cells(12, 1).Value = "Si"
            xlw.Cells(13, 1).Value = "No"
            xlw.Cells(14, 1).Value = "Non risponde"
            If dbRdr.IsDBNull(15) Then
                xlw.Cells(12, 2).Value = "0"
            Else
                xlw.Cells(12, 2).Value = dbRdr.GetSqlMoney(15).ToString
            End If
            If dbRdr.IsDBNull(16) Then
                xlw.Cells(13, 2).Value = "0"
            Else
                xlw.Cells(13, 2).Value = dbRdr.GetSqlMoney(16).ToString
            End If
            If dbRdr.IsDBNull(17) Then
                xlw.Cells(14, 2).Value = "0"
            Else
                xlw.Cells(14, 2).Value = dbRdr.GetSqlMoney(17).ToString
            End If
            '------------------------------------------------------------------------------------------
            xlw.Cells(15, 1).Value = "Problemi causati da durata o orari"
            xlw.Cells(16, 1).Value = "Si"
            xlw.Cells(17, 1).Value = "No"
            xlw.Cells(18, 1).Value = "Non risponde"
            If dbRdr.IsDBNull(11) Then
                xlw.Cells(16, 2).Value = "0"
            Else
                xlw.Cells(16, 2).Value = dbRdr.GetSqlMoney(11).ToString
            End If
            If dbRdr.IsDBNull(12) Then
                xlw.Cells(17, 2).Value = "0"
            Else
                xlw.Cells(17, 2).Value = dbRdr.GetSqlMoney(12).ToString
            End If
            If dbRdr.IsDBNull(13) Then
                xlw.Cells(18, 2).Value = "0"
            Else
                xlw.Cells(18, 2).Value = dbRdr.GetSqlMoney(13).ToString
            End If
            '------------------------------------------------------------------------------------------
            xlw.Cells(19, 1).Value = "Frequenterebbe altri corsi"
            xlw.Cells(20, 1).Value = "Si"
            xlw.Cells(21, 1).Value = "No"
            xlw.Cells(22, 1).Value = "Non risponde"
            If dbRdr.IsDBNull(18) Then
                xlw.Cells(20, 2).Value = "0"
            Else
                xlw.Cells(20, 2).Value = dbRdr.GetSqlMoney(18).ToString
            End If
            If dbRdr.IsDBNull(19) Then
                xlw.Cells(21, 2).Value = "0"
            Else
                xlw.Cells(21, 2).Value = dbRdr.GetSqlMoney(19).ToString
            End If
            If dbRdr.IsDBNull(20) Then
                xlw.Cells(22, 2).Value = "0"
            Else
                xlw.Cells(22, 2).Value = dbRdr.GetSqlMoney(20).ToString
            End If

            '------------------------------------------------------------------------------------------
            xlw.Cells(23, 1).Value = "Idoneità e funzioni delle infrastrutture"
            If dbRdr.IsDBNull(14) Then
                xlw.Cells(23, 2).Value = "0"
            Else
                xlw.Cells(23, 2).Value = dbRdr.GetSqlMoney(14).ToString
            End If
            xlw.Cells(23, 3).Value = "Media(min 1, max 5)"
            xlw.Cells(25, 1).Value = "Capacità espositiva dei docenti"
            If dbRdr.IsDBNull(21) Then
                xlw.Cells(25, 2).Value = "0"
            Else
                xlw.Cells(25, 2).Value = dbRdr.GetSqlMoney(21).ToString
            End If
            xlw.Cells(25, 3).Value = "Media(min 1, max 5)"

        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        '-------------docenti------------------
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_StatisticheQualitaDocentiExport"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        Dim i As Int32 = 26
        Do While dbRdr.Read
            '------------------------------------------------------------------------------------------
            xlw.Cells(i, 1).Value = dbRdr.GetString(1) & " " & dbRdr.GetString(2)
            xlw.Cells(i, 2).Value = dbRdr.GetSqlMoney(3)
            i = i + 1
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        '-------------risposte a domande aperte------------------
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_StatisticheQualitaOpenanswerExport"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        dbRdr = dbCmd.ExecuteReader
        xlw.Cells(i + 1, 1).Value = "Risposte aperte"
        xlw.Cells(i + 1, 1).Style.Font.Bold = True
        xlw.Cells(i + 1, 1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center

        Dim k As Int32 = i + 2
        Dim z As Int32 = i + 2

        Dim ans As String
        ans = "****************"
        Do While dbRdr.Read
            If z = k Then
                xlw.Cells(k, 1).Value = dbRdr.GetString(3)
            End If
            If ans <> dbRdr.GetString(3) Then
                ans = dbRdr.GetString(3)
                xlw.Cells(k, 1).Value = dbRdr.GetString(3)
            End If

            xlw.Cells(k, 2).Value = dbRdr.GetString(4)
            k = k + 1
        Loop
        dbRdr.Close()
        dbCmd.Dispose()
        '---------------------------------------------------------   


        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=Questionari_idevento_" & GecFinalContextHandler.id_EVENTO & "_" & Date.Today.ToString("d MMM yyyy") & ".xlsx")
        'Response.Output.Write(xDoc.InnerXml)
        Dim excel_binary As ExcelPackage
        excel_binary = CType(xlp, ExcelPackage)
        Response.BinaryWrite(excel_binary.GetAsByteArray())
        Response.End()

    End Sub

End Class