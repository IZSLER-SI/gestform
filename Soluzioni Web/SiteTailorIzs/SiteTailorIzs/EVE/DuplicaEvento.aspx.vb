Imports Softailor.Global.MiscUtils
Imports Softailor.Global.ValidationUtils
Imports Softailor.Global.ControlsParser

Public Class DuplicaEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Const errRequired = "Campo richiesto"
    Private Const errInvalidDate = "Data non valida"
    Private bgErr As Drawing.Color = Drawing.Color.Yellow
    Private bgOk As Drawing.Color = Drawing.Color.Empty

    Dim dbConn As SqlConnection

    Private Sub DuplicaEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'riempimento drop downs e altri
        FillControls()

        'scrittura dati vecchio evento e nuovo evento (se non siamo in postback)
        ScriviDatiVecchioNuovo()

    End Sub


    Private Sub DuplicaEvento_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
            dbConn = Nothing
        End If
    End Sub

    Private Sub FillControls()

        'edizione
        FillDropDown(ac_EDIZIONE, dbConn, "SELECT ac_EDIZIONE, tx_EDIZIONE FROM eve_EDIZIONI ORDER BY ni_ORDINE", True, True, True, True)

        'sede
        FillDropDown(id_SEDE, dbConn, "SELECT id_SEDE, tx_SEDE FROM age_SEDI ORDER BY fl_ABITUALE desc, tx_SEDE", False, True, True, True)

        'piani formativi
        FillDropDown(id_PIANOFORMATIVO, dbConn, "SELECT id_PIANOFORMATIVO, tx_PIANOFORMATIVO FROM vw_age_PIANIFORMATIVI ORDER BY dt_INIZIO desc", False, True, True, True)

    End Sub

    Private Sub ScriviDatiVecchioNuovo()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_duplicazione_DatiOrigine"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()


        With dbRdr
            'scrittura dati vecchio evento (sempre)

            lbltx_TITOLO.Text = .GetString(0)
            If .IsDBNull(2) Then lbltx_EDIZIONE.Text = "" Else lbltx_EDIZIONE.Text = .GetString(2)
            If .IsDBNull(4) Then lbltx_SEDE.Text = "" Else lbltx_SEDE.Text = .GetString(4)
            If .IsDBNull(5) Then lbltx_DETTAGLISEDE.Text = "" Else lbltx_DETTAGLISEDE.Text = .GetString(5)
            lbldt_INIZIO.Text = FormatItalianDateY4(.GetDateTime(6))
            lbldt_FINE.Text = FormatItalianDateY4(.GetDateTime(7))
            If .IsDBNull(9) Then lbltx_PIANOFORMATIVO.Text = "" Else lbltx_PIANOFORMATIVO.Text = .GetString(9)
            orig_fl_NUOVOINPF.Checked = .GetBoolean(10)

            'scrittura dati nuovo evento (se non siamo in postback)
            If Not Page.IsPostBack Then
                tx_TITOLO.Text = .GetString(0)
                If .IsDBNull(1) Then ac_EDIZIONE.SelectedValue = "" Else ac_EDIZIONE.SelectedValue = .GetString(1)
                If .IsDBNull(3) Then id_SEDE.SelectedValue = "" Else id_SEDE.SelectedValue = .GetInt32(3).ToString
                If .IsDBNull(5) Then tx_DETTAGLISEDE.Text = "" Else tx_DETTAGLISEDE.Text = .GetString(5)
                dt_INIZIO.Text = FormatItalianDateY4(.GetDateTime(6))
                dt_FINE.Text = FormatItalianDateY4(.GetDateTime(7))
                If .IsDBNull(8) Then id_PIANOFORMATIVO.SelectedValue = "" Else id_PIANOFORMATIVO.SelectedValue = .GetInt32(8).ToString
                fl_NUOVOINPF.Checked = .GetBoolean(10)
            End If

        End With

        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Private Sub lnkDuplica_Click(sender As Object, e As EventArgs) Handles lnkDuplica.Click
        If ValidateMe() Then
            Dim id_EVENTO = DuplicateMe()

            Select Case id_EVENTO
                Case 0  'errore tecnico
                    lblGlobalError.Text = "Si è verificato un errore durante la creazione dell'evento. Contatta l'assistenza."
                Case -1 'durata errata
                    lblGlobalError.Text = "La durata in giorni del nuovo evento è inferiore a quella dell'evento attuale."
                Case Else
                    'selezione evento
                    GecFinalContextHandler.SelectEvento(id_EVENTO, True)
                    'redirect
                    dbConn.Close()
                    dbConn.Dispose()
                    Response.Redirect(ResolveUrl("~/" & id_EVENTO.ToString & "/EVE/HomeEvento.aspx"))
            End Select
        Else
            lblGlobalError.Text = "Dati mancanti o non validi."
        End If
    End Sub

    Private Function DuplicateMe() As Integer

        Dim dbCmd As SqlCommand
        Dim prmid_EVENTO As SqlParameter
        Dim listaDiscipline As Softailor.Global.StructuredUtils.GenericIntList

        Try
            listaDiscipline = New Softailor.Global.StructuredUtils.GenericIntList
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_eve_DuplicaEvento"
                .Parameters.Add("@id_EVENTO_origine", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
                .Parameters.Add("@dt_CREAZIONE", SqlDbType.DateTime).Value = Date.Now
                .Parameters.Add("@tx_CREAZIONE", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
                .Parameters.Add("@id_SEDE", SqlDbType.Int).Value = DropDownSqlInt32(id_SEDE)
                .Parameters.Add("@tx_DETTAGLISEDE", SqlDbType.NVarChar, 200).Value = TextBoxSqlString(tx_DETTAGLISEDE)
                .Parameters.Add("@id_PIANOFORMATIVO", SqlDbType.Int).Value = DropDownSqlInt32(id_PIANOFORMATIVO)
                .Parameters.Add("@fl_NUOVOINPF", SqlDbType.Bit).Value = fl_NUOVOINPF.Checked
                .Parameters.Add("@dt_INIZIO", SqlDbType.DateTime).Value = TextBoxSqlDateTimeDDMMYYYY(dt_INIZIO)
                .Parameters.Add("@dt_FINE", SqlDbType.DateTime).Value = TextBoxSqlDateTimeDDMMYYYY(dt_FINE)
                .Parameters.Add("@tx_TITOLO", SqlDbType.NVarChar, 600).Value = TextBoxSqlString(tx_TITOLO)
                .Parameters.Add("@ac_EDIZIONE", SqlDbType.NVarChar, 2).Value = DropDownSqlString(ac_EDIZIONE)

                'dal-al
                .Parameters.Add("@tx_DALAL", SqlDbType.NVarChar, 200).Value = Softailor.Global.DateUtils.DataDalAlEstesa(ParseItalianDate(dt_INIZIO.Text), ParseItalianDate(dt_FINE.Text))

                'opzioni
                .Parameters.Add("@fl_COPIASPESERICAVIEVENTO", SqlDbType.Bit).Value = fl_COPIASPESERICAVIEVENTO.Checked
                .Parameters.Add("@fl_COPIASPESERICAVIPERSONE", SqlDbType.Bit).Value = fl_COPIASPESERICAVIPERSONE.Checked

                prmid_EVENTO = .Parameters.Add("@id_EVENTO_destinazione", SqlDbType.Int)
                prmid_EVENTO.Direction = ParameterDirection.Output
            End With
            dbCmd.ExecuteNonQuery()
            DuplicateMe = CInt(prmid_EVENTO.Value)
            dbCmd.Dispose()
        Catch ex As Exception
            DuplicateMe = 0
        End Try

    End Function

    Private Function ValidateMe() As Boolean

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim allValid = True

        'trim iniziali
        tx_TITOLO.Text = tx_TITOLO.Text.Trim
        tx_DETTAGLISEDE.Text = tx_DETTAGLISEDE.Text.Trim

        'pulizie (non pulisco le label perchè sono tutte con viewstate = false)
        tx_TITOLO.BackColor = bgOk
        id_SEDE.BackColor = bgOk
        tx_DETTAGLISEDE.BackColor = bgOk
        dt_INIZIO.BackColor = bgOk
        dt_FINE.BackColor = bgOk
        id_PIANOFORMATIVO.BackColor = bgOk

        'titolo evento
        If tx_TITOLO.Text = String.Empty Then
            allValid = False
            tx_TITOLO.BackColor = bgErr
            errtx_TITOLO.Text = errRequired
        End If

        'sede evento
        If id_SEDE.SelectedValue = "" Then
            allValid = False
            id_SEDE.BackColor = bgErr
            errid_SEDE.Text = errRequired
        End If

        'verifica delle date

        Dim iniOk As Boolean = True
        Dim finOk As Boolean = True

        If dt_INIZIO.Text = "" Then
            iniOk = False
            dt_INIZIO.BackColor = bgErr
            errdt_INIZIO.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_INIZIO.Text) Then
                iniOk = False
                dt_INIZIO.BackColor = bgErr
                errdt_INIZIO.Text = errInvalidDate
            End If
        End If

        If dt_FINE.Text = "" Then
            finOk = False
            dt_FINE.BackColor = bgErr
            errdt_FINE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_FINE.Text) Then
                finOk = False
                dt_FINE.BackColor = bgErr
                errdt_FINE.Text = errInvalidDate
            End If
        End If

        'se manca una data > tutto non valido
        If iniOk = False Or finOk = False Then
            allValid = False
        Else
            'OK, abbiamo le 2 date
            'le leggo
            Dim ini = ParseItalianDate(dt_INIZIO.Text)
            Dim fin = ParseItalianDate(dt_FINE.Text)

            'confronto inizio-fine
            If ini > fin Then
                allValid = False
                dt_INIZIO.BackColor = bgErr
                dt_FINE.BackColor = bgErr
                errdt_FINE.Text = "Data precedente alla data di inizio"
            Else
                'ok sequenza temporale corretta

                Dim cdcIni As Date = New Date(1900, 1, 1)
                Dim cdcFin As Date = New Date(2100, 1, 1)
                Dim pfIni As Date = New Date(1900, 1, 1)
                Dim pfFin As Date = New Date(2100, 1, 1)

                'lettura dati da piano formativo
                If id_PIANOFORMATIVO.SelectedValue <> "" Then
                    dbCmd = dbConn.CreateCommand
                    With dbCmd
                        .CommandType = CommandType.Text
                        .CommandText = "SELECT CAST(dt_INIZIO as datetime) as dt_INIZIO, CAST(dt_FINE as datetime) as dt_FINE FROM age_PIANIFORMATIVI WHERE id_PIANOFORMATIVO=@id_PIANOFORMATIVO"
                        .Parameters.Add("@id_PIANOFORMATIVO", SqlDbType.Int).Value = CInt(id_PIANOFORMATIVO.SelectedValue)
                    End With
                    dbRdr = dbCmd.ExecuteReader
                    dbRdr.Read()
                    If Not dbRdr.IsDBNull(0) Then pfIni = dbRdr.GetDateTime(0)
                    If Not dbRdr.IsDBNull(1) Then pfFin = dbRdr.GetDateTime(1)
                    dbRdr.Close()
                    dbCmd.Dispose()
                End If

                'data di inizio
                If ini < cdcIni Or ini > cdcFin Or ini < pfIni Or ini > pfFin Then
                    allValid = False
                    dt_INIZIO.BackColor = bgErr
                    errdt_INIZIO.Text = "Data non coerente con il piano formativo"
                End If

                'data di fine
                If fin < cdcIni Or fin > cdcFin Or fin < pfIni Or fin > pfFin Then
                    allValid = False
                    dt_FINE.BackColor = bgErr
                    errdt_FINE.Text = "Data non coerente con il piano formativo"
                End If

            End If
        End If

        Return allValid

    End Function
End Class