Public Class ControlloAccessi
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Const nRighe As Integer = 5
    Private dbConn As SqlConnection


    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'riempimento delle drop down
        FillDropDowns()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtBarcode.Focus()
        End If
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_presid_Statistiche"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Dim dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        lblIscritti.Text = dbRdr.GetInt32(0).ToString
        lblPresenti.Text = dbRdr.GetInt32(1).ToString
        lblAssenti.Text = dbRdr.GetInt32(2).ToString
        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub FillDropDowns()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'drop down iscritti non annullati
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_presid_Iscritti"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        ddnCognomeNome.Items.Clear()
        ddnCognomeNome.Items.Add(New ListItem("", ""))
        ddnCognomeNomeManuale.Items.Clear()
        ddnCognomeNomeManuale.Items.Add(New ListItem("", ""))

        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim LI1 As New ListItem(dbRdr.GetString(1), dbRdr.GetInt32(0).ToString)
            Dim LI2 As New ListItem(dbRdr.GetString(1), dbRdr.GetInt32(0).ToString)
            ddnCognomeNome.Items.Add(LI1)
            ddnCognomeNomeManuale.Items.Add(LI2)
        Loop

        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Sub EseguiLettura(ac_BARCODE As String, id_ISCRITTO As Integer)

        If id_ISCRITTO > 0 Then
            'proveniamo dalla drop down iscritti
            EseguiLetturaDB("", id_ISCRITTO)
        Else
            'proveniamo dal barcode

            'vediamo se è un barcode valido...
            Dim readid_ISCRITTO = Softailor.Global.EpsonItfBarcode.IdFromBarcode(ac_BARCODE, GecFinalContextHandler.BarcodeStarter)

            If readid_ISCRITTO > 0 Then
                'OK è un barcode valido
                EseguiLetturaDB("", readid_ISCRITTO)
            Else
                EseguiLetturaDB(ac_BARCODE, 0)
            End If

        End If

    End Sub

    Private Sub btnBarcode_Click(sender As Object, e As System.EventArgs) Handles btnBarcode.Click
        If txtBarcode.Text.Trim.ToUpper <> String.Empty Then
            EseguiLettura(txtBarcode.Text.Trim.ToUpper, 0)
        End If
        'reset
        txtBarcode.Text = ""
        'txtBarcode.Focus()
    End Sub

    Private Sub ddnCognomeNome_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddnCognomeNome.SelectedIndexChanged
        If ddnCognomeNome.SelectedValue <> "" Then
            EseguiLettura("", CInt(ddnCognomeNome.SelectedValue))
        End If
        'reset
        ddnCognomeNome.SelectedValue = ""
        'txtBarcode.Focus()
    End Sub

    Private Sub EseguiLetturaDB(ac_BARCODE As String, id_ISCRITTO As Integer)

        Dim dbCmd As SqlCommand
        Dim prmstatus_out As SqlParameter
        Dim prmid_iscritto_out As SqlParameter
        Dim prmid_accessoiscritto_out As SqlParameter
        Dim prmtx_iscritto_out As SqlParameter
        Dim prmorario_out As SqlParameter

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_presid_Lettura"
            'parametri in ingresso
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            With .Parameters.Add("@id_iscritto", SqlDbType.Int)
                If id_ISCRITTO = 0 Then
                    .Value = DBNull.Value
                Else
                    .Value = id_ISCRITTO
                End If
            End With
            With .Parameters.Add("@ac_codicefiscale", SqlDbType.NVarChar, 40)
                If ac_BARCODE = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = ac_BARCODE
                End If
            End With
            'parametri in uscita

            prmstatus_out = .Parameters.Add("@status_out", SqlDbType.Int)
            prmstatus_out.Direction = ParameterDirection.Output

            prmid_iscritto_out = .Parameters.Add("@id_iscritto_out", SqlDbType.Int)
            prmid_iscritto_out.Direction = ParameterDirection.Output

            prmid_accessoiscritto_out = .Parameters.Add("@id_accessoiscritto_out", SqlDbType.Int)
            prmid_accessoiscritto_out.Direction = ParameterDirection.Output

            prmtx_iscritto_out = .Parameters.Add("@tx_iscritto_out", SqlDbType.NVarChar, 400)
            prmtx_iscritto_out.Direction = ParameterDirection.Output

            prmorario_out = .Parameters.Add("@orario_out", SqlDbType.Time)
            prmorario_out.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()

        Dim result As New EseguiLetturaDbResult
        result.status = CInt(prmstatus_out.Value)
        result.id_iscritto = CType(prmid_iscritto_out.SqlValue, SqlInt32)
        result.id_accessoiscritto = CType(prmid_accessoiscritto_out.SqlValue, SqlInt32)
        result.tx_iscritto = CType(prmtx_iscritto_out.SqlValue, SqlString)
        result.orario = CType(prmorario_out.SqlValue, TimeSpan)

        dbCmd.Dispose()

        'scrittura
        WriteResult(result)

        'suono
        Dim script = "play"
        Select Case result.status
            Case 0 : script &= "no"
            Case 1 : script &= "in"
            Case 2 : script &= "out"
            Case 3 : script &= "no"
        End Select
        script &= "();"

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "suona", script, True)

        'aggiorno updatepanel letture
        updLetture.Update()


    End Sub

    Private Class EseguiLetturaDbResult
        Public status As Integer
        Public id_iscritto As SqlInt32
        Public id_accessoiscritto As SqlInt32
        Public tx_iscritto As SqlString
        Public orario As TimeSpan
    End Class

    Private Sub WriteResult(r As EseguiLetturaDbResult)

        'scroll down
        For i As Integer = nRighe To 2 Step -1

            rowId_accesso(i).Value = rowId_accesso(i - 1).Value
            rowId_iscritto(i).Value = rowId_iscritto(i - 1).Value

            rowName(i).Text = rowName(i - 1).Text
            rowName(i).CssClass = rowName(i - 1).CssClass

            rowInOut(i).Text = rowInOut(i - 1).Text
            rowInOut(i).CssClass = rowInOut(i - 1).CssClass

            rowOrario(i).Text = rowOrario(i - 1).Text
            rowOrario(i).CssClass = rowOrario(i - 1).CssClass

            rowDelete(i).Visible = rowDelete(i - 1).Visible
            rowDetail(i).Visible = rowDetail(i - 1).Visible

        Next

        'scrittura
        Select Case r.status
            Case 0  'errore
                l1id_accesso.Value = ""
                l1id_iscritto.Value = ""

                l1name.Text = "<b>*** Nominativo non riconosciuto ***</b>"
                l1name.CssClass = "llet nome active_ko"

                l1inout.Text = ""
                l1inout.CssClass = "llet inout active_ko"

                l1orario.Text = ""
                l1orario.CssClass = "llet orario active_ko"

                l1delete.Visible = False
                l1detail.Visible = False

            Case 3  'nominativo presente più volte
                l1id_accesso.Value = ""
                l1id_iscritto.Value = ""

                l1name.Text = "<b>*** Nominativo con più ruoli ***</b>"
                l1name.CssClass = "llet nome active_ko"

                l1inout.Text = ""
                l1inout.CssClass = "llet inout active_ko"

                l1orario.Text = ""
                l1orario.CssClass = "llet orario active_ko"

                l1delete.Visible = False
                l1detail.Visible = False

            Case 1, 2  'ingresso o uscita
                l1id_accesso.Value = r.id_accessoiscritto.Value.ToString
                l1id_iscritto.Value = r.id_iscritto.Value.ToString

                l1name.Text = r.tx_iscritto.Value
                l1name.CssClass = "llet nome active_ok"

                l1inout.Text = If(r.status = 1, "IN", "OUT")
                l1inout.CssClass = "llet inout active_ok"

                l1orario.Text = r.orario.Hours.ToString("00") & ":" & r.orario.Minutes.ToString("00")
                l1orario.CssClass = "llet orario active_ok"

                l1delete.Visible = True
                l1detail.Visible = True

            Case 99  'uscita generale
                l1id_accesso.Value = ""
                l1id_iscritto.Value = ""

                l1name.Text = "<b>*** Uscita generale *** </b>"
                l1name.CssClass = "llet nome active_ok"

                l1inout.Text = "OUT"
                l1inout.CssClass = "llet inout active_ok"

                l1orario.Text = r.orario.Hours.ToString("00") & ":" & r.orario.Minutes.ToString("00")
                l1orario.CssClass = "llet orario active_ok"

                l1delete.Visible = False
                l1detail.Visible = False

        End Select

    End Sub

    Private Function rowId_accesso(i As Integer) As HiddenField
        Return CType(pnlLetture.FindControl("l" & i.ToString & "id_accesso"), HiddenField)
    End Function

    Private Function rowId_iscritto(i As Integer) As HiddenField
        Return CType(pnlLetture.FindControl("l" & i.ToString & "id_iscritto"), HiddenField)
    End Function

    Private Function rowName(i As Integer) As Label
        Return CType(pnlLetture.FindControl("l" & i.ToString & "name"), Label)
    End Function

    Private Function rowInOut(i As Integer) As Label
        Return CType(pnlLetture.FindControl("l" & i.ToString & "inout"), Label)
    End Function

    Private Function rowOrario(i As Integer) As Label
        Return CType(pnlLetture.FindControl("l" & i.ToString & "orario"), Label)
    End Function

    Private Function rowDelete(i As Integer) As LinkButton
        Return CType(pnlLetture.FindControl("l" & i.ToString & "delete"), LinkButton)
    End Function

    Private Function rowDetail(i As Integer) As LinkButton
        Return CType(pnlLetture.FindControl("l" & i.ToString & "detail"), LinkButton)
    End Function

    Private Sub l1detail_Click(sender As Object, e As System.EventArgs) Handles l1detail.Click
        OpenDetails(1)
    End Sub
    Private Sub l1detai2_Click(sender As Object, e As System.EventArgs) Handles l2detail.Click
        OpenDetails(2)
    End Sub
    Private Sub l1detai3_Click(sender As Object, e As System.EventArgs) Handles l3detail.Click
        OpenDetails(3)
    End Sub
    Private Sub l1detai4_Click(sender As Object, e As System.EventArgs) Handles l4detail.Click
        OpenDetails(4)
    End Sub
    Private Sub l1detai5_Click(sender As Object, e As System.EventArgs) Handles l5detail.Click
        OpenDetails(5)
    End Sub

    Private Sub l1delete_Click(sender As Object, e As System.EventArgs) Handles l1delete.Click
        DeleteRegistration(1)
    End Sub
    Private Sub l2delete_Click(sender As Object, e As System.EventArgs) Handles l2delete.Click
        DeleteRegistration(2)
    End Sub
    Private Sub l3delete_Click(sender As Object, e As System.EventArgs) Handles l3delete.Click
        DeleteRegistration(3)
    End Sub
    Private Sub l4delete_Click(sender As Object, e As System.EventArgs) Handles l4delete.Click
        DeleteRegistration(4)
    End Sub
    Private Sub l5delete_Click(sender As Object, e As System.EventArgs) Handles l5delete.Click
        DeleteRegistration(5)
    End Sub

    Private Sub OpenDetails(row As Integer)

        Dim id_ISCRITTO = CInt(rowId_iscritto(row).Value)
        Dim sScript = "stl_sel_display_wh('SchedaPartecipante.aspx?goto=part&id=" & id_ISCRITTO.ToString & "',880, 780, editIscritto_callback);"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)

    End Sub

    Private Sub DeleteRegistration(row As Integer)

        Dim id_ISCRITTO = CInt(rowId_iscritto(row).Value)
        Dim id_ACCESSOISCRITTO = CInt(rowId_accesso(row).Value)
        Dim fl_USCITA = rowInOut(row).Text = "OUT"

        Dim dbcmd = dbConn.CreateCommand
        With dbcmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_presid_AnnullamentoLettura"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@id_accessoiscritto", SqlDbType.Int).Value = id_ACCESSOISCRITTO
            .Parameters.Add("@fl_uscita", SqlDbType.Bit).Value = fl_USCITA
        End With
        dbcmd.ExecuteNonQuery()
        dbcmd.Dispose()

        'scrittura annullamento

        rowId_accesso(row).Value = ""
        rowId_iscritto(row).Value = ""

        rowName(row).Text = "<b>*** cancellato ***</b>"
        rowName(row).CssClass = "llet nome inactive"

        rowInOut(row).Text = ""
        rowInOut(row).CssClass = "llet inout inactive"

        rowOrario(row).Text = ""
        rowOrario(row).CssClass = "llet orario inactive"

        rowDelete(row).Visible = False
        rowDetail(row).Visible = False

    End Sub


    Private Sub ddnCognomeNomeManuale_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddnCognomeNomeManuale.SelectedIndexChanged

        'apertura
        If ddnCognomeNomeManuale.SelectedValue <> "" Then
            Dim id_ISCRITTO = CInt(ddnCognomeNomeManuale.SelectedValue)
            Dim sScript = "stl_sel_display_wh('SchedaPartecipante.aspx?goto=part&id=" & id_ISCRITTO.ToString & "',880, 780, editIscritto_callback);"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)
            'reset
            ddnCognomeNomeManuale.SelectedValue = ""

        End If
    End Sub

    Private Sub btnRegistra_Click(sender As Object, e As System.EventArgs) Handles btnRegistra.Click
        'registrazione uscita generale

        Dim allOk = True

        Dim dt_DATA As Date
        Dim dt_FINE As Date
        Dim tm_FINE As TimeSpan

        Try
            dt_DATA = Date.ParseExact(txtUscitaGeneraleData.Text, "dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        Catch ex As Exception
            allOk = False
            txtUscitaGeneraleData.BackColor = Drawing.Color.Yellow
        End Try

        Try
            dt_FINE = Date.ParseExact(txtUscitaGeneraleOra.Text.Replace(".", ":"), "HH:mm:ss", Softailor.Global.Cultures.CulturaEnglish)
            tm_FINE = New TimeSpan(dt_FINE.Hour, dt_FINE.Minute, 0)
        Catch ex As Exception
            allOk = False
            txtUscitaGeneraleOra.BackColor = Drawing.Color.Yellow
        End Try

        If allOk Then

            'TODO registrazione
            Dim dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_eve_presid_RegistraUscitaGenerale"
                .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
                .Parameters.Add("@dt_data", SqlDbType.Date).Value = dt_DATA
                .Parameters.Add("@tm_fine", SqlDbType.Time).Value = tm_FINE
            End With

            dbCmd.ExecuteNonQuery()
            dbCmd.Dispose()

            'scrittura
            Dim r As New EseguiLetturaDbResult
            r.status = 99
            r.orario = tm_FINE

            WriteResult(r)
            txtUscitaGeneraleData.Text = ""
            txtUscitaGeneraleOra.Text = ""

            updLetture.Update()

            'suono
            Dim script = "playout();"

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "suonaug", script, True)

            'txtBarcode.Focus()

        End If

    End Sub
End Class