Imports Softailor.Global.MiscUtils
Imports Softailor.Global.ValidationUtils
Imports Softailor.Global.ControlsParser
Imports System

Public Class NuovoEvento
    Inherits System.Web.UI.Page

    Private Const errRequired = "Campo richiesto"
    Private Const errInvalidDate = "Data non valida"
    Private bgErr As Drawing.Color = Drawing.Color.Yellow
    Private bgOk As Drawing.Color = Drawing.Color.Empty


    Dim dbConn As SqlConnection

    Private Sub NuovoEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'riempimento drop downs e altri
        FillControls()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub NuovoEvento_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
            dbConn = Nothing
        End If
    End Sub

    Private Sub FillControls()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'tipologie evento
        FillDropDown(id_TIPOLOGIAEVENTO, dbConn, "SELECT id_TIPOLOGIAEVENTO, tx_TIPOLOGIAEVENTO FROM age_TIPOLOGIEEVENTI ORDER BY tx_TIPOLOGIAEVENTO", False, True, True, True)

        'categoria (corsi obbligatori)
        FillDropDown(ac_TIPOCOBDETT, dbConn, "SELECT ac_TIPOCOBDETT, tx_TIPOCOBDETT FROM vw_cob_TIPICORSODETT ORDER BY sort1, sort2 desc", True, True, True, True)

        'edizione
        FillDropDown(ac_EDIZIONE, dbConn, "SELECT ac_EDIZIONE, tx_EDIZIONE FROM eve_EDIZIONI ORDER BY ni_ORDINE", True, True, True, True)

        'sede
        FillDropDown(id_SEDE, dbConn, "SELECT id_SEDE, tx_SEDE FROM age_SEDI ORDER BY fl_ABITUALE desc, tx_SEDE", False, True, True, True)

        'organizzatori (prima il default; seleziono il primo)
        FillDropDown(id_ORGANIZZATORE, dbConn, "SELECT id_ORGANIZZATORE, tx_ORGANIZZATORE FROM age_ORGANIZZATORI ORDER BY fl_DEFAULT desc, tx_ORGANIZZATORE", False, True, True, True)
        If id_ORGANIZZATORE.Items.Count > 1 And Not Page.IsPostBack Then
            id_ORGANIZZATORE.SelectedIndex = 1
            id_ORGANIZZATORE_SelectedIndexChanged(Nothing, Nothing)
        End If

        'non riempio i rappr legali organizzatore

        'segreterie organizzative (prima il default; seleziono il primo)
        FillDropDown(id_SEGRETERIAORGANIZZATIVA, dbConn, "SELECT id_SEGRETERIAORGANIZZATIVA, tx_SEGRETERIAORGANIZZATIVA FROM age_SEGRETERIEORGANIZZATIVE ORDER BY fl_DEFAULT desc, tx_SEGRETERIAORGANIZZATIVA", False, True, True, True)
        If id_SEGRETERIAORGANIZZATIVA.Items.Count > 1 And Not Page.IsPostBack Then
            id_SEGRETERIAORGANIZZATIVA.SelectedIndex = 1
        End If

        'centri di referenza (se ce n'è uno solo seleziono il primo)
        FillDropDown(id_CENTROREFERENZA, dbConn, "SELECT id_CENTROREFERENZA, tx_CENTROREFERENZA FROM age_CENTRIREFERENZA ORDER BY tx_CENTROREFERENZA", False, True, True, True)
        If id_CENTROREFERENZA.Items.Count = 2 And Not Page.IsPostBack Then
            id_CENTROREFERENZA.SelectedIndex = 1
        End If

        'centri di costo
        FillDropDown(ac_CDC, dbConn, "SELECT ac_CDC, tx_CDC FROM vw_age_CDC ORDER BY dt_INIZIO desc", True, True, True, True)

        'piani formativi
        FillDropDown(id_PIANOFORMATIVO, dbConn, "SELECT id_PIANOFORMATIVO, tx_PIANOFORMATIVO FROM vw_age_PIANIFORMATIVI ORDER BY dt_INIZIO desc", False, True, True, True)

        'tipo formazione (non-2010)
        FillDropDown(id_TIPOLOGIAECMEVENTO, dbConn, "SELECT id_TIPOLOGIAECMEVENTO, tx_TIPOLOGIAECMEVENTO FROM age_TIPOLOGIEECMEVENTI WHERE ac_NORMATIVAECM<>'2010' ORDER BY tx_TIPOLOGIAECMEVENTO", False, True, True, True)

        'tipologia formativa
        FillDropDown(ecm2_TFORM_EVE, dbConn, "SELECT CODELEME, DESELEME FROM ut_ECMELE WHERE CODLISTA='COD_TIPOLOGIA_FORM' ORDER BY DESELEME", True, True, True, True)

        'tipologie formative secondarie (se blended)
        ecm2_TFORM_EVE_SEC.Items.Clear()
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT CODELEME, DESELEME FROM ut_ECMELE WHERE CODLISTA='COD_TIPOLOGIA_FORM' ORDER BY DESELEME"
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            ecm2_TFORM_EVE_SEC.Items.Add(New ListItem(dbRdr.GetString(1), dbRdr.GetString(0)))
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        'tipo evento 
        FillDropDown(ecm2_TIPO_EVE, dbConn, "SELECT CODELEME, DESELEME FROM ut_ECMELE WHERE CODLISTA='TIPO_EVE' ORDER BY DESELEME", True, True, True, True)

        'ambito / obbiettivo formativo
        FillDropDown(ecm2_COD_OBI, dbConn, "SELECT	CODELEME, '(' + CODELEME + ') ' + DESELEME AS DESELEME
            FROM	ut_ECMELE 
            WHERE	CODLISTA='COD_OBI'
            ORDER BY fl_DISATTIVATO ASC, CASE WHEN ISNUMERIC(CODELEME)=1 THEN CAST(CODELEME AS INT) ELSE 9999 END, DESELEME", True, True, True, True)

        'ente accreditante
        FillDropDown(ecm2_COD_ACCR, dbConn, "SELECT CODELEME, DESELEME FROM ut_ECMELE WHERE CODLISTA='COD_ACCR' ORDER BY CODELEME", True, True, True, True)

        'provider ECM (non-2010)
        FillDropDown(id_PROVIDERECM, dbConn, "SELECT id_PROVIDERECM, tx_PROVIDERECM FROM age_PROVIDERECM WHERE ac_NORMATIVAECM<>'2010' ORDER BY tx_PROVIDERECM", False, True, True, True)
        If id_PROVIDERECM.Items.Count = 2 And Not Page.IsPostBack Then
            id_PROVIDERECM.SelectedIndex = 1
            id_PROVIDERECM_SelectedIndexChanged(Nothing, Nothing)
        End If
        'non riempio il rappresentante legale del provider

        'professioni e discipline
        id_DISCIPLINAs.Items.Clear()

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ecm_ProfessioniDisciplineAttive"
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            id_DISCIPLINAs.Items.Add(New ListItem(dbRdr.GetString(1), dbRdr.GetInt32(0).ToString))
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

        'normative ECM
        ac_NORMATIVAECM.Items.Clear()
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT ac_NORMATIVAECM, tx_NORMATIVAECM FROM age_NORMATIVEECM WHERE ac_NORMATIVAECM<>'2010' ORDER BY ac_NORMATIVAECM desc"
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            ac_NORMATIVAECM.Items.Add(New ListItem(dbRdr.GetString(1), dbRdr.GetString(0)))
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Private Sub id_ORGANIZZATORE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles id_ORGANIZZATORE.SelectedIndexChanged

        If id_ORGANIZZATORE.SelectedValue = "" Then
            id_RAPPRLEGALE_ORGANIZZATORE.SelectedValue = ""
            id_RAPPRLEGALE_ORGANIZZATORE.Items.Clear()
        Else
            FillDropDown(id_RAPPRLEGALE_ORGANIZZATORE, dbConn, "SELECT id_RAPPRLEGALE_ORGANIZZATORE, tx_RAPPRLEGALE " &
                                                               "FROM vw_age_RAPPRLEGALI_ORGANIZZATORI_grid " &
                                                               "WHERE id_ORGANIZZATORE=" & CInt(id_ORGANIZZATORE.SelectedValue).ToString & " " &
                                                               "ORDER BY id_RAPPRLEGALE_ORGANIZZATORE", False, True, True, True)
            If id_RAPPRLEGALE_ORGANIZZATORE.Items.Count = 2 Then
                id_RAPPRLEGALE_ORGANIZZATORE.SelectedIndex = 1
            End If
        End If

    End Sub

    Private Sub id_PROVIDERECM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles id_PROVIDERECM.SelectedIndexChanged
        If id_PROVIDERECM.SelectedValue = "" Then
            id_RAPPRLEGALE_PROVIDERECM.SelectedValue = ""
            id_RAPPRLEGALE_PROVIDERECM.Items.Clear()
        Else
            FillDropDown(id_RAPPRLEGALE_PROVIDERECM, dbConn, "SELECT id_RAPPRLEGALE_PROVIDERECM, tx_RAPPRLEGALE " &
                                                   "FROM vw_age_RAPPRLEGALI_PROVIDERECM_grid " &
                                                   "WHERE id_PROVIDERECM=" & CInt(id_PROVIDERECM.SelectedValue).ToString & " " &
                                                   "ORDER BY id_RAPPRLEGALE_PROVIDERECM", False, True, True, True)
            If id_RAPPRLEGALE_PROVIDERECM.Items.Count = 2 Then
                id_RAPPRLEGALE_PROVIDERECM.SelectedIndex = 1
            End If
        End If
    End Sub

    Private Sub ac_NORMATIVAECM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_NORMATIVAECM.SelectedIndexChanged
        pnlDatiEcm.Visible = ac_NORMATIVAECM.SelectedValue <> "NONE"
    End Sub

    Private Sub id_TIPOLOGIAECMEVENTO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles id_TIPOLOGIAECMEVENTO.SelectedIndexChanged
        row_ecm2_TFORM_EVE_SEC.Visible = id_TIPOLOGIAECMEVENTO.SelectedValue = "5" 'visibile solo se BLENDED (ID = 5)
    End Sub

    Private Sub ecm2_TFORM_EVE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ecm2_TFORM_EVE.SelectedIndexChanged
        'tipologie formative secondarie (se blended)
        'aggiorna la checkboxlist eliminando l'elemento selezionato nel dropdown ecm2_TFORM_EVE
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        ecm2_TFORM_EVE_SEC.Items.Clear()
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT CODELEME, DESELEME FROM ut_ECMELE WHERE " & If(ecm2_TFORM_EVE.SelectedValue <> "", " CODELEME <> " & ecm2_TFORM_EVE.SelectedValue & " AND ", "") & " CODLISTA='COD_TIPOLOGIA_FORM' ORDER BY DESELEME"
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            ecm2_TFORM_EVE_SEC.Items.Add(New ListItem(dbRdr.GetString(1), dbRdr.GetString(0)))
        Loop
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Function ValidateMe() As Boolean

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim allValid = True

        'trim iniziali
        tx_CODINT.Text = tx_CODINT.Text.Trim
        tx_TITOLO.Text = tx_TITOLO.Text.Trim
        tx_DETTAGLISEDE.Text = tx_DETTAGLISEDE.Text.Trim
        ecm2_COD_EVE.Text = ecm2_COD_EVE.Text.Trim
        ecm2_NUM_CRED.Text = ecm2_NUM_CRED.Text.Trim
        ecm2_CREDITILETTERE.Text = ecm2_CREDITILETTERE.Text.Trim
        tx_OBBIETTIVI.Text = tx_OBBIETTIVI.Text.Trim

        'pulizie (non pulisco le label perchè sono tutte con viewstate = false)
        id_TIPOLOGIAEVENTO.BackColor = bgOk
        tx_TITOLO.BackColor = bgOk
        id_SEDE.BackColor = bgOk
        tx_DETTAGLISEDE.BackColor = bgOk
        dt_INIZIO.BackColor = bgOk
        dt_FINE.BackColor = bgOk
        id_ORGANIZZATORE.BackColor = bgOk
        id_RAPPRLEGALE_ORGANIZZATORE.BackColor = bgOk
        id_SEGRETERIAORGANIZZATIVA.BackColor = bgOk
        id_CENTROREFERENZA.BackColor = bgOk
        ac_CDC.BackColor = bgOk
        id_PIANOFORMATIVO.BackColor = bgOk
        ac_NORMATIVAECM.BackColor = bgOk
        id_TIPOLOGIAECMEVENTO.BackColor = bgOk
        id_PROVIDERECM.BackColor = bgOk
        id_RAPPRLEGALE_PROVIDERECM.BackColor = bgOk
        id_DISCIPLINAs.BackColor = bgOk
        ecm2_NUM_CRED.BackColor = bgOk
        tx_OBBIETTIVI.BackColor = bgOk

        'tipologia evento
        If id_TIPOLOGIAEVENTO.SelectedValue = "" Then
            allValid = False
            id_TIPOLOGIAEVENTO.BackColor = bgErr
            errid_TIPOLOGIAEVENTO.Text = errRequired
        End If

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

        'organizzatore
        If id_ORGANIZZATORE.SelectedValue = "" Then
            allValid = False
            id_ORGANIZZATORE.BackColor = bgErr
            errid_ORGANIZZATORE.Text = errRequired
        End If

        'segreteria organizzativa
        If id_SEGRETERIAORGANIZZATIVA.SelectedValue = "" Then
            allValid = False
            id_SEGRETERIAORGANIZZATIVA.BackColor = bgErr
            errid_SEGRETERIAORGANIZZATIVA.Text = errRequired
        End If

        'CDC
        If ac_CDC.SelectedValue = "" Then
            allValid = False
            ac_CDC.BackColor = bgErr
            errac_CDC.Text = errRequired
        End If

        'Accreditamento ECM
        If ac_NORMATIVAECM.SelectedValue = "" Then
            allValid = False
            ac_NORMATIVAECM.BackColor = bgErr
            errac_NORMATIVAECM.Text = errRequired
        End If

        'se ECM...
        If ac_NORMATIVAECM.SelectedValue <> "NONE" Then

            'tipo formazione
            If id_TIPOLOGIAECMEVENTO.SelectedValue = "" Then
                allValid = False
                id_TIPOLOGIAECMEVENTO.BackColor = bgErr
                errid_TIPOLOGIAECMEVENTO.Text = errRequired
            ElseIf id_TIPOLOGIAECMEVENTO.SelectedValue = "5" Then ' se BLENDED (5)
                If ecm2_TFORM_EVE.SelectedValue = "" Then
                    allValid = False
                    ecm2_TFORM_EVE.BackColor = bgErr
                    errecm2_TFORM_EVE.Text = errRequired
                Else
                    If ecm2_TFORM_EVE_SEC.SelectedValue = "" Then
                        allValid = False
                        ecm2_TFORM_EVE_SEC.BackColor = bgErr
                        errecm2_TFORM_EVE_SEC.Text = errRequired
                    Else
                        Dim fl_ok As Boolean = False
                        For Each lItem As ListItem In ecm2_TFORM_EVE_SEC.Items
                            If lItem.Selected Then
                                Dim test As String = lItem.Text
                                Dim m1 As Match = Regex.Match(test, "\[\w+\]", RegexOptions.Multiline)

                                Dim m2 As Match = Regex.Match(ecm2_TFORM_EVE.SelectedItem.Text, "\[\w+\]", RegexOptions.Multiline)
                                If m1.Value <> m2.Value Then
                                    fl_ok = True
                                End If

                            End If
                        Next
                        If Not fl_ok Then
                            allValid = False
                            ecm2_TFORM_EVE_SEC.BackColor = bgErr
                            errecm2_TFORM_EVE_SEC.Text = "Seleziona almeno una tipologia formativa diversa dal tipo " & Regex.Match(ecm2_TFORM_EVE.SelectedItem.Text, "\[\w+\]", RegexOptions.Multiline).Value
                        End If
                    End If
                End If
            End If

            'provider
            If id_PROVIDERECM.SelectedValue = "" Then
                allValid = False
                id_PROVIDERECM.BackColor = bgErr
                errid_PROVIDERECM.Text = errRequired
            End If

            'prof/disc
            Dim nItems = 0
            For Each lItem As ListItem In id_DISCIPLINAs.Items
                If lItem.Selected Then
                    nItems += 1
                    Exit For
                End If
            Next
            If nItems = 0 Then
                allValid = False
                id_DISCIPLINAs.BackColor = bgErr
                errid_DISCIPLINAs.Text = "Seleziona almeno una professione/disciplina"
            End If

            'crediti
            If ecm2_NUM_CRED.Text <> String.Empty Then
                If Not ValidateItalianDecimal(ecm2_NUM_CRED.Text) Then
                    allValid = False
                    ecm2_NUM_CRED.BackColor = bgErr
                    errecm2_NUM_CRED.Text = "Numero non valido"
                End If
            End If

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

                'lettura dati da CDC
                If ac_CDC.SelectedValue <> "" Then
                    dbCmd = dbConn.CreateCommand
                    With dbCmd
                        .CommandType = CommandType.Text
                        .CommandText = "SELECT CAST(dt_INIZIO as datetime) as dt_INIZIO, CAST(dt_FINE as datetime) as dt_FINE FROM age_CDC WHERE ac_CDC=@ac_CDC"
                        .Parameters.Add("@ac_CDC", SqlDbType.NVarChar, 16).Value = ac_CDC.SelectedValue
                    End With
                    dbRdr = dbCmd.ExecuteReader
                    dbRdr.Read()
                    If Not dbRdr.IsDBNull(0) Then cdcIni = dbRdr.GetDateTime(0)
                    If Not dbRdr.IsDBNull(1) Then cdcFin = dbRdr.GetDateTime(1)
                    dbRdr.Close()
                    dbCmd.Dispose()
                End If

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
                    errdt_INIZIO.Text = "Data non coerente con il piano formativo e/o il centro di costo"
                End If

                'data di fine
                If fin < cdcIni Or fin > cdcFin Or fin < pfIni Or fin > pfFin Then
                    allValid = False
                    dt_FINE.BackColor = bgErr
                    errdt_FINE.Text = "Data non coerente con il piano formativo e/o il centro di costo"
                End If

            End If
        End If

        Return allValid

    End Function

    Private Sub lnkCreate_Click(sender As Object, e As EventArgs) Handles lnkCreate.Click
        If ValidateMe() Then
            Dim id_EVENTO = SaveMe()

            If id_EVENTO > 0 Then
                'selezione evento
                GecFinalContextHandler.SelectEvento(id_EVENTO, True)

                'redirect
                dbConn.Close()
                dbConn.Dispose()
                Response.Redirect(ResolveUrl("~/" & id_EVENTO.ToString & "/EVE/HomeEvento.aspx"))
            End If

        Else
            lblGlobalError.Text = "Dati mancanti o non validi."
        End If
    End Sub

    Private Function SaveMe() As Integer

        Dim dbCmd As SqlCommand
        Dim prmid_EVENTO As SqlParameter
        Dim listaDiscipline As Softailor.Global.StructuredUtils.GenericIntList
        Dim listaTipologieECMSec As Softailor.Global.StructuredUtils.GenericIntList

        Try
            listaDiscipline = New Softailor.Global.StructuredUtils.GenericIntList
            listaTipologieECMSec = New Softailor.Global.StructuredUtils.GenericIntList
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_eve_CreaEvento"
                .Parameters.Add("@dt_CREAZIONE", SqlDbType.DateTime).Value = Date.Now
                .Parameters.Add("@tx_CREAZIONE", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
                .Parameters.Add("@id_SEDE", SqlDbType.Int).Value = DropDownSqlInt32(id_SEDE)
                .Parameters.Add("@tx_DETTAGLISEDE", SqlDbType.NVarChar, 200).Value = TextBoxSqlString(tx_DETTAGLISEDE)
                .Parameters.Add("@id_SEGRETERIAORGANIZZATIVA", SqlDbType.Int).Value = DropDownSqlInt32(id_SEGRETERIAORGANIZZATIVA)
                .Parameters.Add("@id_TIPOLOGIAEVENTO", SqlDbType.Int).Value = DropDownSqlInt32(id_TIPOLOGIAEVENTO)
                .Parameters.Add("@ac_TIPOCOBDETT", SqlDbType.NVarChar, 8).Value = DropDownSqlString(ac_TIPOCOBDETT)
                .Parameters.Add("@id_ORGANIZZATORE", SqlDbType.Int).Value = DropDownSqlInt32(id_ORGANIZZATORE)
                .Parameters.Add("@id_RAPPRLEGALE_ORGANIZZATORE", SqlDbType.Int).Value = DropDownSqlInt32(id_RAPPRLEGALE_ORGANIZZATORE)
                .Parameters.Add("@id_CENTROREFERENZA", SqlDbType.Int).Value = DropDownSqlInt32(id_CENTROREFERENZA)
                .Parameters.Add("@ac_CDC", SqlDbType.NVarChar, 16).Value = DropDownSqlString(ac_CDC)
                .Parameters.Add("@id_PIANOFORMATIVO", SqlDbType.Int).Value = DropDownSqlInt32(id_PIANOFORMATIVO)
                .Parameters.Add("@fl_NUOVOINPF", SqlDbType.Bit).Value = fl_NUOVOINPF.Checked
                .Parameters.Add("@dt_INIZIO", SqlDbType.DateTime).Value = TextBoxSqlDateTimeDDMMYYYY(dt_INIZIO)
                .Parameters.Add("@dt_FINE", SqlDbType.DateTime).Value = TextBoxSqlDateTimeDDMMYYYY(dt_FINE)
                .Parameters.Add("@tx_TITOLO", SqlDbType.NVarChar, 600).Value = TextBoxSqlString(tx_TITOLO)
                .Parameters.Add("@ac_EDIZIONE", SqlDbType.NVarChar, 2).Value = DropDownSqlString(ac_EDIZIONE)
                .Parameters.Add("@tx_OBBIETTIVI", SqlDbType.NVarChar, -1).Value = TextBoxSqlString(tx_OBBIETTIVI)
                .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16).Value = ac_NORMATIVAECM.SelectedValue
                .Parameters.Add("@tx_CODINT", SqlDbType.NVarChar, 32).Value = TextBoxSqlString(tx_CODINT)

                'dal-al
                .Parameters.Add("@tx_DALAL", SqlDbType.NVarChar, 200).Value = Softailor.Global.DateUtils.DataDalAlEstesa(ParseItalianDate(dt_INIZIO.Text), ParseItalianDate(dt_FINE.Text))

                If ac_NORMATIVAECM.SelectedValue <> "NONE" Then
                    .Parameters.Add("@id_TIPOLOGIAECMEVENTO", SqlDbType.Int).Value = DropDownSqlInt32(id_TIPOLOGIAECMEVENTO)
                    'riempio le tipologie secondarie se il corso è BLENDED
                    For Each li As ListItem In ecm2_TFORM_EVE_SEC.Items
                        If li.Selected Then
                            listaTipologieECMSec.Add(CInt(li.Value))
                        End If
                    Next
                    .Parameters.Add("@id_PROVIDERECM", SqlDbType.Int).Value = DropDownSqlInt32(id_PROVIDERECM)
                    .Parameters.Add("@id_RAPPRLEGALE_PROVIDERECM", SqlDbType.Int).Value = DropDownSqlInt32(id_RAPPRLEGALE_PROVIDERECM)
                    .Parameters.Add("@ecm2_COD_EVE", SqlDbType.NVarChar, 20).Value = TextBoxSqlString(ecm2_COD_EVE)
                    If ecm2_NUM_CRED.Text = String.Empty Then
                        .Parameters.Add("@ecm2_NUM_CRED", SqlDbType.Money).Value = DBNull.Value
                    Else
                        .Parameters.Add("@ecm2_NUM_CRED", SqlDbType.Money).Value = ParseItalianDecimal(ecm2_NUM_CRED.Text)
                    End If
                    .Parameters.Add("@ecm2_CREDITILETTERE", SqlDbType.NVarChar, 100).Value = TextBoxSqlString(ecm2_CREDITILETTERE)
                    .Parameters.Add("@ecm2_TIPO_EVE", SqlDbType.NVarChar, 10).Value = DropDownSqlString(ecm2_TIPO_EVE)
                    .Parameters.Add("@ecm2_COD_OBI", SqlDbType.NVarChar, 10).Value = DropDownSqlString(ecm2_COD_OBI)
                    .Parameters.Add("@ecm2_TFORM_EVE", SqlDbType.NVarChar, 10).Value = DropDownSqlString(ecm2_TFORM_EVE)
                    .Parameters.Add("@ecm2_COD_ACCR", SqlDbType.NVarChar, 10).Value = DropDownSqlString(ecm2_COD_ACCR)

                    'riempio le discipline
                    For Each li As ListItem In id_DISCIPLINAs.Items
                        If li.Selected Then
                            listaDiscipline.Add(CInt(li.Value))
                        End If
                    Next
                Else
                    .Parameters.Add("@id_TIPOLOGIAECMEVENTO", SqlDbType.Int).Value = DBNull.Value
                    .Parameters.Add("@id_PROVIDERECM", SqlDbType.Int).Value = DBNull.Value
                    .Parameters.Add("@id_RAPPRLEGALE_PROVIDERECM", SqlDbType.Int).Value = DBNull.Value

                    .Parameters.Add("@ecm2_COD_EVE", SqlDbType.NVarChar, 20).Value = DBNull.Value
                    .Parameters.Add("@ecm2_NUM_CRED", SqlDbType.Money).Value = DBNull.Value
                    .Parameters.Add("@ecm2_CREDITILETTERE", SqlDbType.NVarChar, 100).Value = DBNull.Value
                    .Parameters.Add("@ecm2_TIPO_EVE", SqlDbType.NVarChar, 10).Value = DBNull.Value
                    .Parameters.Add("@ecm2_COD_OBI", SqlDbType.NVarChar, 10).Value = DBNull.Value
                    .Parameters.Add("@ecm2_TFORM_EVE", SqlDbType.NVarChar, 10).Value = DBNull.Value
                    .Parameters.Add("@ecm2_COD_ACCR", SqlDbType.NVarChar, 10).Value = DBNull.Value


                    'NON riempio le discipline
                End If

                .Parameters.Add("@ecm2_TFORM_EVEs", SqlDbType.Structured).Value = listaTipologieECMSec.GetTable

                .Parameters.Add("@id_DISCIPLINAs", SqlDbType.Structured).Value = listaDiscipline.GetTable

                prmid_EVENTO = .Parameters.Add("@id_EVENTO", SqlDbType.Int)
                prmid_EVENTO.Direction = ParameterDirection.Output
            End With
            dbCmd.ExecuteNonQuery()
            SaveMe = CInt(prmid_EVENTO.Value)
            dbCmd.Dispose()
        Catch ex As Exception
            lblGlobalError.Text = "Si è verificato un errore durante la creazione dell'evento. Contatta l'assistenza."
            SaveMe = 0
        End Try


    End Function
End Class