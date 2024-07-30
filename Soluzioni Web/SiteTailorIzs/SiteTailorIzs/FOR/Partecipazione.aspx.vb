Imports Softailor.Global.ValidationUtils

Public Class Partecipazione
    Inherits System.Web.UI.Page

    Private Enum Modes
        Add
        Edit
        Unknown
    End Enum

    Dim dbConn As SqlConnection
    Dim myMode As Modes
    
    Private Property id_PARTECIPAZIONE As Integer
        Get
            Return CInt(hid_id_PARTECIPAZIONE.Value)
        End Get
        Set(value As Integer)
            hid_id_PARTECIPAZIONE.Value = value.ToString
        End Set
    End Property

    Private Property ac_TIPOPARTECIPAZIONE As String
        Get
            Return hid_ac_TIPOPARTECIPAZIONE.Value
        End Get
        Set(value As String)
            hid_ac_TIPOPARTECIPAZIONE.Value = value
        End Set
    End Property

    Private Property fl_GIORNIORE As Boolean
        Get
            Return hid_fl_GIORNIORE.Value = "1"
        End Get
        Set(value As Boolean)
            hid_fl_GIORNIORE.Value = If(value, "1", "0")
        End Set
    End Property

    Private Sub Partecipazione_Init(sender As Object, e As EventArgs) Handles Me.Init

        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'dati in input
        id_PARTECIPAZIONE = 0
        ac_TIPOPARTECIPAZIONE = ""
        myMode = Modes.Unknown

        If Request("id") IsNot Nothing Then
            If Request("id") <> String.Empty Then
                id_PARTECIPAZIONE = CInt(Request("id"))
                myMode = Modes.Edit
            End If
        End If
        If id_PARTECIPAZIONE = 0 Then
            If Request("type") IsNot Nothing Then
                ac_TIPOPARTECIPAZIONE = Request("type")
                myMode = Modes.Add
            End If
        End If

        If myMode = Modes.Unknown Then
            CloseMe()
            Exit Sub
        End If

        'descrizione
        id_ELEME.DefaultDescriptionPreamble = "Allegato part " & Date.Now.ToString("dd/MM/yy HH:mm", Softailor.Global.Cultures.CulturaItalian)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'preparazione form
        If Not Page.IsPostBack Then
            Select Case myMode
                Case Modes.Add
                    If Not FirstSetup_Add() Then
                        CloseMe()
                        Exit Sub
                    End If
                Case Modes.Edit
                    If Not FirstSetup_Edit() Then
                        CloseMe()
                        Exit Sub
                    End If
            End Select
        End If

        If Not Page.IsPostBack Then
            Dim sScript = "function clickPickPersona(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkPickPersona, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            sScript &= "function clickPickResponsabile(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkPickResponsabile, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            sScript &= "function clickPickEvento(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkPickEvento, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf


            ltrScripts.Text = sScript
        End If

    End Sub

    Private Sub Partecipazione_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub CloseMe()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMe",
           "parent.stl_sel_done('');", True)
    End Sub

    Private Function FirstSetup_Add() As Boolean

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        FirstSetup_Add = False

        trCreazioneModifica.Visible = False

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_DatiPartecipazione_New"
            .Parameters.Add("@ac_TIPOPARTECIPAZIONE", SqlDbType.NVarChar, 8).Value = ac_TIPOPARTECIPAZIONE
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then

            'dati globali
            FirstSetup_Add = True
            id_PARTECIPAZIONE = 0

            'titolo
            lblTitle.Text = "Nuova " & dbRdr.GetString(1).ToLower

            'dati validazione
            pnlValdazione.Visible = False

            'dati
            tx_TIPOPARTECIPAZIONE.Text = dbRdr.GetString(1)
            lblNumeroData.Text = "Data"
            ni_PARTECIPAZIONE.Visible = False
            lblSpazioNumeroData.Text = ""
            dt_PARTECIPAZIONE.Text = FormatItalianDateY4(Date.Today)
            dt_PARTECIPAZIONE.Enabled = True

            'partecipante
            id_PERSONA.Text = ""
            'reazione alla selezione > dati ECM invisibili
            lnkPickPersona_Click(Nothing, Nothing)

            'quota di iscrizione
            fl_ANTICIPOQUOTAISCRIZIONE.Checked = False
            nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedValue = "EUR"
            'reazione
            fl_ANTICIPOQUOTAISCRIZIONE_CheckedChanged(Nothing, Nothing)

            'giorni e ore
            fl_GIORNIORE = dbRdr.GetBoolean(2)
            tr_GIORNIFORMAZIONE.Visible = fl_GIORNIORE
            tr_OREFORMAZIONE.Visible = fl_GIORNIORE

        End If
        dbRdr.Close()
        dbCmd.Dispose()


    End Function

    Private Function FirstSetup_Edit() As Boolean

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        FirstSetup_Edit = False

        trCreazioneModifica.Visible = True

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_DatiPartecipazione_Existing"
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then

            'dati globali
            FirstSetup_Edit = True
            ac_TIPOPARTECIPAZIONE = dbRdr.GetString(1).ToLower

            'titolo
            lblTitle.Text = "Modifica " & dbRdr.GetString(2)

            'dati
            tx_TIPOPARTECIPAZIONE.Text = dbRdr.GetString(2)
            lblNumeroData.Text = "Numero e Data"
            ni_PARTECIPAZIONE.Visible = True
            lblSpazioNumeroData.Text = " - "
            ni_PARTECIPAZIONE.Text = dbRdr.GetString(3) & dbRdr.GetInt32(5).ToString & "/" & dbRdr.GetInt32(4).ToString
            dt_PARTECIPAZIONE.Text = FormatItalianDateY4(dbRdr.GetDateTime(6))
            dt_PARTECIPAZIONE.Enabled = False

            'responsabile
            If Not dbRdr.IsDBNull(7) Then
                id_PERSONA_RESPONSABILE.Text = dbRdr.GetInt32(7).ToString
                tx_PERSONA_RESPONSABILE.Text = dbRdr.GetString(8)
            End If

            If Not dbRdr.IsDBNull(45) Then
                ac_UNITAOPERATIVA.SelectedValue = dbRdr.GetString(45)
            End If

            If Not dbRdr.IsDBNull(46) Then
                tx_CENTROCOSTO.Text = dbRdr.GetString(46)
            End If

            If Not dbRdr.IsDBNull(51) Then
                tx_CENTROCOSTO_COMMESSA.Text = dbRdr.GetString(51)
            End If

            If Not dbRdr.IsDBNull(47) Then
                dt_PAGARSIENTRO.Text = FormatItalianDateY4(dbRdr.GetDateTime(47))
            End If

            'partecipante
            If Not dbRdr.IsDBNull(9) Then
                id_PERSONA.Text = dbRdr.GetInt32(9).ToString
            End If
            'reazione alla selezione
            lnkPickPersona_Click(Nothing, Nothing)

            'dati del corso
            If Not dbRdr.IsDBNull(11) Then tx_TITOLO.Text = dbRdr.GetString(11)
            If Not dbRdr.IsDBNull(12) Then ac_TIPOLOGIAEVENTO.SelectedValue = dbRdr.GetString(12)
            If Not dbRdr.IsDBNull(44) Then ac_TIPOCOBDETT.SelectedValue = dbRdr.GetString(44)
            If Not dbRdr.IsDBNull(13) Then tx_SEDE.Text = dbRdr.GetString(13)
            If Not dbRdr.IsDBNull(14) Then ac_NAZIONE.SelectedValue = dbRdr.GetString(14)
            If Not dbRdr.IsDBNull(15) Then tx_ORGANIZZATORE.Text = dbRdr.GetString(15)
            If Not dbRdr.IsDBNull(30) Then dt_INIZIO.Text = FormatItalianDateY4(dbRdr.GetDateTime(30))
            If Not dbRdr.IsDBNull(31) Then dt_FINE.Text = FormatItalianDateY4(dbRdr.GetDateTime(31))
            If Not dbRdr.IsDBNull(18) Then tx_NOTEDATE.Text = dbRdr.GetString(18)

            'giorni e ore
            If Not dbRdr.IsDBNull(16) Then nd_GIORNIFORMAZIONE.Text = FormatItalianDecimal(dbRdr.GetDecimal(16))
            If Not dbRdr.IsDBNull(17) Then nd_OREFORMAZIONE.Text = FormatItalianDecimal(dbRdr.GetDecimal(17))

            'quota di iscrizione
            If Not dbRdr.IsDBNull(20) Then nd_QUOTAISCRIZIONE_PREV.Text = FormatItalianCurrency(dbRdr.GetDecimal(20))
            If Not dbRdr.IsDBNull(52) Then
                nd_QUOTAISCRIZIONE_PREV_VALUTA.Text = dbRdr.GetString(52)
            Else
                nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedValue = "EUR"
            End If

            'si richiede anticipo
            If dbRdr.IsDBNull(19) Then
                fl_ANTICIPOQUOTAISCRIZIONE.Checked = False
            Else
                fl_ANTICIPOQUOTAISCRIZIONE.Checked = dbRdr.GetBoolean(19)
            End If
            'reazione
            fl_ANTICIPOQUOTAISCRIZIONE_CheckedChanged(Nothing, Nothing)
            'valore
            If fl_ANTICIPOQUOTAISCRIZIONE.Checked Then
                If Not dbRdr.IsDBNull(22) Then ac_CIGQUOTAISCRIZIONE.Text = dbRdr.GetString(22)
            End If

            'dati viaggio
            If Not dbRdr.IsDBNull(23) Then nd_GIORNIVIAGGIO.Text = FormatItalianDecimal(dbRdr.GetDecimal(23))
            If Not dbRdr.IsDBNull(24) Then nd_COSTOVIAGGIO_PREV.Text = FormatItalianCurrency(dbRdr.GetDecimal(24))

            'dati validazione
            pnlValdazione.Visible = True

            'status
            ac_STATOPARTECIPAZIONE.DataBind()
            ac_STATOPARTECIPAZIONE.SelectedValue = dbRdr.GetString(37) & "\" & If(dbRdr.GetBoolean(33), "1", "0")
            'reazione allo status
            ac_STATOPARTECIPAZIONE_SelectedIndexChanged(Nothing, Nothing)

            'evento accreditato ECM
            If fl_PROFILOECM.Text = "1" Then
                fl_EVENTOECM.Checked = dbRdr.GetString(26) <> "NONE"
            Else
                fl_EVENTOECM.Checked = False
            End If
            'reazione
            fl_EVENTOECM_CheckedChanged(Nothing, Nothing)

            'gestione dati ECM nella parte di validazione
            If fl_PROFILOECM.Text = "1" And fl_EVENTOECM.Checked Then
                If dbRdr.GetBoolean(33) Then    'fl_OK
                    If dbRdr.GetString(27) = "COK" Then
                        If Not dbRdr.IsDBNull(28) Then
                            nd_CREDITIECM.Text = FormatItalianDecimal(dbRdr.GetDecimal(28))
                        End If
                        If Not dbRdr.IsDBNull(36) Then
                            dt_OTTENIMENTOCREDITIECM.Text = FormatItalianDateY4(dbRdr.GetDateTime(36))
                        End If
                    End If
                End If
            End If

            'altri dati validazione
            If dbRdr.GetBoolean(33) Then    'fl_OK
                If Not dbRdr.IsDBNull(21) Then
                    nd_QUOTAISCRIZIONE_CONS.Text = FormatItalianCurrency(dbRdr.GetDecimal(21))
                End If
                If Not dbRdr.IsDBNull(25) Then
                    nd_COSTOVIAGGIO_CONS.Text = FormatItalianCurrency(dbRdr.GetDecimal(25))
                End If
            End If

            'giorni e ore
            fl_GIORNIORE = dbRdr.GetBoolean(38)
            tr_GIORNIFORMAZIONE.Visible = fl_GIORNIORE
            tr_OREFORMAZIONE.Visible = fl_GIORNIORE

            If fl_GIORNIORE Then
                If Not dbRdr.IsDBNull(16) Then nd_GIORNIFORMAZIONE.Text = FormatItalianDecimal(dbRdr.GetDecimal(16))
                If Not dbRdr.IsDBNull(17) Then nd_OREFORMAZIONE.Text = FormatItalianDecimal(dbRdr.GetDecimal(17))
            End If

            'allegato
            If Not dbRdr.IsDBNull(29) Then
                id_ELEME.Value = dbRdr.GetInt32(29).ToString
            End If

            If Not dbRdr.IsDBNull(48) Then
                ac_RESPONSABILE.SelectedValue = dbRdr.GetString(48)
            End If
            If Not dbRdr.IsDBNull(49) Then
                ac_PROGETTOATTIVITA.SelectedValue = dbRdr.GetString(49)
            End If
            If Not dbRdr.IsDBNull(50) Then
                ac_CONTRATTO.SelectedValue = dbRdr.GetString(50)
            End If

            'creazione e modifica
            Dim sCM As String = ""

            sCM = "Creata da <b>" & dbRdr.GetString(40) & "</b> il <b>" & dbRdr.GetDateTime(39).ToString("dd/MM/yyyy HH:mm") & "</b>"

            If (Not dbRdr.IsDBNull(41)) And (Not dbRdr.IsDBNull(42)) Then
                sCM &= " - Ultima modifica: <b>" & dbRdr.GetString(42) & "</b> il <b>" & dbRdr.GetDateTime(41).ToString("dd/MM/yyyy HH:mm") & "</b>"
            End If

            lblCreazioneModifica.Text = sCM

            'codice progetto ricerca o attività finanziata
            If Not dbRdr.IsDBNull(43) Then ac_CODICEPRAF.Text = dbRdr.GetString(43)

            'codice CUP
            If Not dbRdr.IsDBNull(53) Then ac_CODICECUP.Text = dbRdr.GetString(53)

        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'warning
        SetWarning()

    End Function

    Private Sub lnkPickPersona_Click(sender As Object, e As EventArgs) Handles lnkPickPersona.Click

        If id_PERSONA.Text = "" Then
            'tutto vuoto
            id_PERSONA.Text = ""
            tx_PERSONA.Text = ""
            fl_PROFILOECM.Text = ""

            'svuoto i campi ECM
            fl_EVENTOECM.Checked = False

            'nascondo tutti i dati ECM
            tr_EVENTOECM.Visible = False
            tr_CREDITIECM.Visible = False
            tr_OTTENIMENTOCREDITIECM.Visible = False

        Else
            'recipero i dati della persona
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = DbConnectionHandler.GetOpenDataDbConn

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ext_DatiPersona"
                .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = CInt(id_PERSONA.Text)
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()

            id_PERSONA.Text = dbRdr.GetInt32(0).ToString
            tx_PERSONA.Text = dbRdr.GetString(1)
            fl_PROFILOECM.Text = If(dbRdr.GetBoolean(2), "1", "0")
            dbRdr.Close()
            dbCmd.Dispose()
            dbConn.Close()
            dbConn.Dispose()

            'dati ECM?

            tr_EVENTOECM.Visible = fl_PROFILOECM.Text = "1"
            If fl_PROFILOECM.Text = "0" Then fl_EVENTOECM.Checked = False
            fl_EVENTOECM_CheckedChanged(Nothing, Nothing)

        End If

        'warning
        SetWarning()

    End Sub

    Private Sub fl_ANTICIPOQUOTAISCRIZIONE_CheckedChanged(sender As Object, e As EventArgs) Handles fl_ANTICIPOQUOTAISCRIZIONE.CheckedChanged

        tr_CIGQUOTAISCRIZIONE.Visible = fl_ANTICIPOQUOTAISCRIZIONE.Checked

    End Sub

    Private Sub ac_STATOPARTECIPAZIONE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_STATOPARTECIPAZIONE.SelectedIndexChanged

        tblDatiConsuntivo.Visible = ac_STATOPARTECIPAZIONE.SelectedValue.EndsWith("\1")

        If Not tblDatiConsuntivo.Visible Then
        End If

    End Sub

    Private Sub fl_EVENTOECM_CheckedChanged(sender As Object, e As EventArgs) Handles fl_EVENTOECM.CheckedChanged

        'mostro o nascondo tutti i dati
        tr_CREDITIECM.Visible = fl_EVENTOECM.Checked
        tr_OTTENIMENTOCREDITIECM.Visible = fl_EVENTOECM.Checked

        If Not fl_EVENTOECM.Checked Then
        End If

    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click

        Dim valid As Boolean = False
        Dim result As AddEditMeResult
        Dim msg As String
        Dim script As String

        Select Case myMode
            Case Modes.Add
                valid = ValidateMe_Add()
            Case Modes.Edit
                valid = ValidateMe_Add() And ValidateMe_Edit()
        End Select

        If valid Then
            Select Case myMode
                Case Modes.Add
                    result = AddMe()

                    'messaggio e chiusura
                    msg = "E' stata creata la partecipazione numero " &
                        result.ac_GRUPPONUMERAZIONE & result.ni_NUMERO.ToString & "/" & result.ni_ANNO.ToString & "."

                    script = "window.alert(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(msg) & ");" &
                        "parent.stl_sel_done('" & result.id_PARTECIPAZIONE.ToString & "');"

                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMeAdded",
                        script, True)

                Case Modes.Edit
                    result = EditMe()

                    script = "parent.stl_sel_done('" & result.id_PARTECIPAZIONE.ToString & "');"

                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMeEdited",
                        script, True)

            End Select
        End If


    End Sub

#Region "Salvataggio"

    Private Class AddEditMeResult
        Public id_PARTECIPAZIONE As Integer = 0
        Public ac_GRUPPONUMERAZIONE As String = ""
        Public ni_ANNO As Integer = 0
        Public ni_NUMERO As Integer = 0
    End Class

    Private Function AddMe() As AddEditMeResult

        Dim result As New AddEditMeResult

        Dim dbCmd As SqlCommand
        Dim prmac_GRUPPONUMERAZIONE As SqlParameter
        Dim prmid_PARTECIPAZIONE As SqlParameter
        Dim prmni_ANNO As SqlParameter
        Dim prmni_NUMERO As SqlParameter

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_CreaPartecipazione"

            'parametri in comune
            AddParameters_Common(dbCmd)

            'parametri specifici NEW
            AddParameters_New(dbCmd)

            'parametri in uscita
            prmac_GRUPPONUMERAZIONE = .Parameters.Add("@ac_GRUPPONUMERAZIONE", SqlDbType.NVarChar, 8)
            prmac_GRUPPONUMERAZIONE.Direction = ParameterDirection.Output

            prmid_PARTECIPAZIONE = .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int)
            prmid_PARTECIPAZIONE.Direction = ParameterDirection.Output

            prmni_ANNO = .Parameters.Add("@ni_ANNO", SqlDbType.Int)
            prmni_ANNO.Direction = ParameterDirection.Output

            prmni_NUMERO = .Parameters.Add("@ni_NUMERO", SqlDbType.Int)
            prmni_NUMERO.Direction = ParameterDirection.Output

        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        result.ac_GRUPPONUMERAZIONE = CStr(prmac_GRUPPONUMERAZIONE.Value)
        result.id_PARTECIPAZIONE = CInt(prmid_PARTECIPAZIONE.Value)
        result.ni_ANNO = CInt(prmni_ANNO.Value)
        result.ni_NUMERO = CInt(prmni_NUMERO.Value)

        Return result

    End Function

    Private Function EditMe() As AddEditMeResult

        Dim result As New AddEditMeResult

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_AggiornaPartecipazione"

            'parametri in comune
            AddParameters_Common(dbCmd)

            'parametri specifici EDIT
            AddParameters_Update(dbCmd)

        End With

        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        result.id_PARTECIPAZIONE = Me.id_PARTECIPAZIONE

        Return result

    End Function

    Private Sub AddParameters_Common(dbCmd As SqlCommand)

        With dbCmd

            .Parameters.Add("@id_UTENT", SqlDbType.Int).Value = ContextHandler.ID_UTENT

            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = CInt(id_PERSONA.Text)
            If id_PERSONA_RESPONSABILE.Text <> String.Empty Then .Parameters.Add("@id_PERSONA_RESPONSABILE", SqlDbType.Int).Value = CInt(id_PERSONA_RESPONSABILE.Text)

            If fl_GIORNIORE Then
                If nd_OREFORMAZIONE.Text <> String.Empty Then .Parameters.Add("@nd_OREFORMAZIONE", SqlDbType.Money).Value = ParseItalianDecimal(nd_OREFORMAZIONE.Text)
                If nd_GIORNIFORMAZIONE.Text <> String.Empty Then .Parameters.Add("@nd_GIORNIFORMAZIONE", SqlDbType.Money).Value = ParseItalianDecimal(nd_GIORNIFORMAZIONE.Text)
            End If

            If tx_NOTEDATE.Text <> String.Empty Then .Parameters.Add("@tx_NOTEDATE", SqlDbType.NVarChar, -1).Value = tx_NOTEDATE.Text

            With .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16)
                If fl_PROFILOECM.Text = "1" Then
                    If fl_EVENTOECM.Checked Then
                        .Value = "2011"
                    Else
                        .Value = "NONE"
                    End If
                Else
                    .Value = "NONE"
                End If
            End With

            With .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8)
                If fl_PROFILOECM.Text = "1" Then
                    If fl_EVENTOECM.Checked Then
                        If ac_STATOPARTECIPAZIONE.SelectedValue.EndsWith("\1") Then
                            'validato
                            If nd_CREDITIECM.Text <> String.Empty Then
                                .Value = "COK"
                            Else
                                .Value = "CKO"
                            End If
                        Else
                            'non validato
                            .Value = "C"
                        End If
                    Else
                        .Value = "NC"
                    End If
                Else
                    .Value = "NC"
                End If
            End With

            If nd_QUOTAISCRIZIONE_PREV.Text <> "" Then
                .Parameters.Add("@nd_QUOTAISCRIZIONE_PREV", SqlDbType.Money).Value = ParseItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text)
                If nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedIndex <> -1 Then
                    .Parameters.Add("@nd_QUOTAISCRIZIONE_PREV_VALUTA", SqlDbType.NVarChar, 3).Value = nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedValue
                End If
            End If

            .Parameters.Add("@fl_ANTICIPOQUOTAISCRIZIONE", SqlDbType.Bit).Value = fl_ANTICIPOQUOTAISCRIZIONE.Checked

            If fl_ANTICIPOQUOTAISCRIZIONE.Checked And ac_CIGQUOTAISCRIZIONE.Text <> String.Empty Then
                .Parameters.Add("@ac_CIGQUOTAISCRIZIONE", SqlDbType.NVarChar, 20).Value = ac_CIGQUOTAISCRIZIONE.Text
            End If

            If nd_GIORNIVIAGGIO.Text <> String.Empty Then .Parameters.Add("@nd_GIORNIVIAGGIO", SqlDbType.Money).Value = ParseItalianDecimal(nd_GIORNIVIAGGIO.Text)
            If nd_COSTOVIAGGIO_PREV.Text <> String.Empty Then .Parameters.Add("@nd_COSTOVIAGGIO_PREV", SqlDbType.Money).Value = ParseItalianDecimal(nd_COSTOVIAGGIO_PREV.Text)

            If id_ELEME.Value <> String.Empty Then .Parameters.Add("@id_ELEME", SqlDbType.Int).Value = CInt(id_ELEME.Value)

            If tx_TITOLO.Text <> String.Empty Then .Parameters.Add("@tx_TITOLO", SqlDbType.NVarChar, 600).Value = tx_TITOLO.Text
            If ac_TIPOLOGIAEVENTO.SelectedValue <> String.Empty Then .Parameters.Add("@ac_TIPOLOGIAEVENTO", SqlDbType.NVarChar, 8).Value = ac_TIPOLOGIAEVENTO.SelectedValue
            If ac_TIPOCOBDETT.SelectedValue <> String.Empty Then .Parameters.Add("@ac_TIPOCOBDETT", SqlDbType.NVarChar, 8).Value = ac_TIPOCOBDETT.SelectedValue

            If dt_INIZIO.Text <> String.Empty Then .Parameters.Add("@dt_INIZIO", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIO.Text)
            If dt_FINE.Text <> String.Empty Then .Parameters.Add("@dt_FINE", SqlDbType.Date).Value = ParseItalianDate(dt_FINE.Text)
            If tx_SEDE.Text <> String.Empty Then .Parameters.Add("@tx_SEDE", SqlDbType.NVarChar, 300).Value = tx_SEDE.Text

            If ac_NAZIONE.SelectedValue <> String.Empty Then .Parameters.Add("@ac_NAZIONE", SqlDbType.NVarChar, 4).Value = ac_NAZIONE.SelectedValue

            If tx_ORGANIZZATORE.Text <> String.Empty Then .Parameters.Add("@tx_ORGANIZZATORE", SqlDbType.NVarChar, 300).Value = tx_ORGANIZZATORE.Text

            If ac_CODICEPRAF.Text <> String.Empty Then .Parameters.Add("@ac_CODICEPRAF", SqlDbType.NVarChar, 50).Value = ac_CODICEPRAF.Text

            If ac_CODICECUP.Text <> String.Empty Then .Parameters.Add("@ac_CODICECUP", SqlDbType.NVarChar, 50).Value = ac_CODICECUP.Text

            If tx_CENTROCOSTO.Text <> String.Empty Then .Parameters.Add("@tx_CENTROCOSTO", SqlDbType.NVarChar, 50).Value = tx_CENTROCOSTO.Text

            If tx_CENTROCOSTO_COMMESSA.Text <> String.Empty Then .Parameters.Add("@tx_CENTROCOSTO_COMMESSA", SqlDbType.NVarChar, 50).Value = tx_CENTROCOSTO_COMMESSA.Text

            If String.IsNullOrEmpty(dt_PAGARSIENTRO.Text) = False Then .Parameters.Add("@dt_PAGARSIENTRO", SqlDbType.Date).Value = ParseItalianDate(dt_PAGARSIENTRO.Text)

            If String.IsNullOrEmpty(ac_RESPONSABILE.SelectedValue) = False Then .Parameters.Add("@ac_RESPONSABILE", SqlDbType.NVarChar, 8).Value = ac_RESPONSABILE.SelectedValue

            If String.IsNullOrEmpty(ac_PROGETTOATTIVITA.SelectedValue) = False Then .Parameters.Add("@ac_PROGETTOATTIVITA", SqlDbType.NVarChar, 8).Value = ac_PROGETTOATTIVITA.SelectedValue

            If String.IsNullOrEmpty(ac_CONTRATTO.SelectedValue) = False Then .Parameters.Add("@ac_CONTRATTO", SqlDbType.NVarChar, 8).Value = ac_CONTRATTO.SelectedValue

        End With


    End Sub

    Private Sub AddParameters_New(dbCmd As SqlCommand)

        With dbCmd
            .Parameters.Add("@dt_DATA", SqlDbType.DateTime).Value = ParseItalianDate(dt_PARTECIPAZIONE.Text)
            .Parameters.Add("@ac_TIPOPARTECIPAZIONE", SqlDbType.NVarChar, 8).Value = ac_TIPOPARTECIPAZIONE
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = "P"
            .Parameters.Add("@ac_STATOVERIFICAAPPRENDIMENTO", SqlDbType.NVarChar, 8).Value = "NP"
        End With

    End Sub

    Private Sub AddParameters_Update(dbCmd As SqlCommand)

        With dbCmd

            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE
            .Parameters.Add("@ac_STATOPARTECIPAZIONE", SqlDbType.NVarChar, 8).Value = ac_STATOPARTECIPAZIONE.SelectedValue.Replace("\0", "").Replace("\1", "")

            If ac_STATOPARTECIPAZIONE.SelectedValue.EndsWith("\1") Then
                If nd_QUOTAISCRIZIONE_CONS.Text <> String.Empty Then .Parameters.Add("@nd_QUOTAISCRIZIONE_CONS", SqlDbType.Money).Value = ParseItalianDecimal(nd_QUOTAISCRIZIONE_CONS.Text)
                If nd_COSTOVIAGGIO_CONS.Text <> String.Empty Then .Parameters.Add("@nd_COSTOVIAGGIO_CONS", SqlDbType.Money).Value = ParseItalianDecimal(nd_COSTOVIAGGIO_CONS.Text)
                If fl_PROFILOECM.Text = "1" And fl_EVENTOECM.Checked Then
                    If nd_CREDITIECM.Text <> String.Empty Then .Parameters.Add("@nd_CREDITIECM", SqlDbType.Money).Value = ParseItalianDecimal(nd_CREDITIECM.Text)
                    If dt_OTTENIMENTOCREDITIECM.Text <> String.Empty Then .Parameters.Add("@dt_OTTENIMENTOCREDITIECM", SqlDbType.DateTime).Value = ParseItalianDate(dt_OTTENIMENTOCREDITIECM.Text)
                End If
            End If

        End With

    End Sub

#End Region

#Region "Validazione"

    Const errRequired = "Campo obbligatorio"
    Const errRequired2 = "Campi obbligatori"
    Const errInvalidDate = "Data non valida"
    Const errInvalidNumber = "Numero non valido"

    Private Function ValidateMe_Add() As Boolean

        Dim valid = True
        Dim iniOk = False
        Dim finOk = False

        'pulizie
        tx_TITOLO.Text = tx_TITOLO.Text.Trim.ToUpper
        tx_SEDE.Text = tx_SEDE.Text.Trim.ToUpper
        tx_ORGANIZZATORE.Text = tx_ORGANIZZATORE.Text.Trim.ToUpper
        dt_INIZIO.Text = dt_INIZIO.Text.Trim
        If dt_INIZIO.Text = "__/__/____" Then dt_INIZIO.Text = ""
        dt_FINE.Text = dt_FINE.Text.Trim
        If dt_FINE.Text = "__/__/____" Then dt_FINE.Text = ""
        nd_GIORNIFORMAZIONE.Text = nd_GIORNIFORMAZIONE.Text.Trim
        nd_OREFORMAZIONE.Text = nd_OREFORMAZIONE.Text.Trim
        nd_QUOTAISCRIZIONE_PREV.Text = nd_QUOTAISCRIZIONE_PREV.Text.Trim
        ac_CIGQUOTAISCRIZIONE.Text = ac_CIGQUOTAISCRIZIONE.Text.Trim
        nd_GIORNIVIAGGIO.Text = nd_GIORNIVIAGGIO.Text.Trim
        nd_COSTOVIAGGIO_PREV.Text = nd_COSTOVIAGGIO_PREV.Text.Trim
        ac_CODICEPRAF.Text = ac_CODICEPRAF.Text.Trim
        ac_CODICECUP.Text = ac_CODICECUP.Text.Trim
        dt_PAGARSIENTRO.Text = dt_PAGARSIENTRO.Text.Trim
        If dt_PAGARSIENTRO.Text = "__/__/____" Then dt_PAGARSIENTRO.Text = ""

        tx_CENTROCOSTO.Text = tx_CENTROCOSTO.Text.Trim.ToUpper
        tx_CENTROCOSTO_COMMESSA.Text = tx_CENTROCOSTO_COMMESSA.Text.Trim.ToUpper


        'riporto la data di fine
        If dt_FINE.Text = "" Then dt_FINE.Text = dt_INIZIO.Text

        'partecipante
        If id_PERSONA.Text = String.Empty Then
            valid = False
            errtx_PERSONA.Text = errRequired
        End If

        If String.IsNullOrEmpty(dt_PAGARSIENTRO.Text) = False Then
            If Not ValidateItalianDate(dt_PAGARSIENTRO.Text) Then
                valid = False
                err_dt_PAGARSIENTRO.Text = errInvalidNumber
            End If
        End If

        'titolo
        If tx_TITOLO.Text = String.Empty Then
            valid = False
            errtx_TITOLO.Text = errRequired
        End If

        'tipologia
        If ac_TIPOLOGIAEVENTO.SelectedValue = String.Empty Then
            valid = False
            errac_TIPOLOGIAEVENTO.Text = errRequired
        End If

        'città
        If tx_SEDE.Text = String.Empty Then
            valid = False
            errtx_SEDE.Text = errRequired
        End If

        'organizzatore
        If tx_ORGANIZZATORE.Text = String.Empty Then
            valid = False
            errtx_ORGANIZZATORE.Text = errRequired
        End If

        'data inizio
        If dt_INIZIO.Text = String.Empty Then
            valid = False
            errdt_INIZIOFINE.Text = errRequired2
        Else
            If Not ValidateItalianDate(dt_INIZIO.Text) Then
                valid = False
                errdt_INIZIOFINE.Text = errInvalidDate
            Else
                iniOk = True
            End If
        End If

        'data fine
        If dt_FINE.Text = String.Empty Then
            valid = False
            errdt_INIZIOFINE.Text = errRequired2
        Else
            If Not ValidateItalianDate(dt_FINE.Text) Then
                valid = False
                errdt_INIZIOFINE.Text = errInvalidDate
            Else
                finOk = True
            End If
        End If

        'ordine date
        If iniOk And finOk Then
            If ParseItalianDate(dt_INIZIO.Text) > ParseItalianDate(dt_FINE.Text) Then
                valid = False
                errdt_INIZIOFINE.Text = "La data di fine precede la data di inizio."
            End If
        End If

        'giorni e ore
        If fl_GIORNIORE Then
            If nd_GIORNIFORMAZIONE.Text <> String.Empty And nd_OREFORMAZIONE.Text <> String.Empty Then
                valid = False
                errnd_GIORNIFORMAZIONE.Text = "Compila i giorni o le ore ma non entrambi."
            Else
                If nd_GIORNIFORMAZIONE.Text <> String.Empty Then
                    If Not ValidateItalianDecimal(nd_GIORNIFORMAZIONE.Text) Then
                        valid = False
                        errnd_GIORNIFORMAZIONE.Text = errInvalidNumber
                    End If
                End If
                If nd_OREFORMAZIONE.Text <> String.Empty Then
                    If Not ValidateItalianDecimal(nd_OREFORMAZIONE.Text) Then
                        valid = False
                        errnd_OREFORMAZIONE.Text = errInvalidNumber
                    End If
                End If
            End If
        End If

        'costo quota
        If nd_QUOTAISCRIZIONE_PREV.Text <> String.Empty Then
            If Not ValidateItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text) Then
                valid = False
                errnd_QUOTAISCRIZIONE_PREV.Text = errInvalidNumber
            End If
            If nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedIndex = -1 Then
                valid = False
                errnd_QUOTAISCRIZIONE_PREV_VALUTA.Text = errRequired
            End If
        End If

        If fl_ANTICIPOQUOTAISCRIZIONE.Checked Then
            If nd_QUOTAISCRIZIONE_PREV.Text = String.Empty Then
                valid = False
                errnd_QUOTAISCRIZIONE_PREV.Text = "Campo obbligatorio (in quanto è richiesto l'anticipo)"
            End If
            If ac_CIGQUOTAISCRIZIONE.Text = String.Empty Then
                valid = False
                errac_CIGQUOTAISCRIZIONE.Text = "Campo obbligatorio (in quanto è richiesto l'anticipo)"
            End If
        End If


        'giorni viaggio e costo presunto
        If nd_GIORNIVIAGGIO.Text <> String.Empty Then
            If Not ValidateItalianDecimal(nd_GIORNIVIAGGIO.Text) Then
                valid = False
                errnd_GIORNIVIAGGIO.Text = errInvalidNumber
            End If
        End If
        If nd_COSTOVIAGGIO_PREV.Text <> String.Empty Then
            If Not ValidateItalianDecimal(nd_COSTOVIAGGIO_PREV.Text) Then
                valid = False
                errnd_COSTOVIAGGIO_PREV.Text = errInvalidNumber
            End If
        End If
        Return valid
    End Function

    Private Function ValidateMe_Edit() As Boolean

        Dim valid = True

        'pulizie
        nd_QUOTAISCRIZIONE_CONS.Text = nd_QUOTAISCRIZIONE_CONS.Text.Trim
        nd_COSTOVIAGGIO_CONS.Text = nd_COSTOVIAGGIO_CONS.Text.Trim
        nd_CREDITIECM.Text = nd_CREDITIECM.Text.Trim
        dt_OTTENIMENTOCREDITIECM.Text = dt_OTTENIMENTOCREDITIECM.Text.Trim
        If dt_OTTENIMENTOCREDITIECM.Text = "__/__/____" Then dt_OTTENIMENTOCREDITIECM.Text = ""
        ac_CODICEPRAF.Text = ac_CODICEPRAF.Text.Trim
        ac_CODICECUP.Text = ac_CODICECUP.Text.Trim

        If ac_STATOPARTECIPAZIONE.SelectedValue.EndsWith("\1") Then
            'validato

            'consuntivi
            If nd_QUOTAISCRIZIONE_CONS.Text <> String.Empty Then
                If Not ValidateItalianDecimal(nd_QUOTAISCRIZIONE_CONS.Text) Then
                    valid = False
                    errnd_QUOTAISCRIZIONE_CONS.Text = errInvalidNumber
                End If
            End If

            If nd_COSTOVIAGGIO_CONS.Text <> String.Empty Then
                If Not ValidateItalianDecimal(nd_COSTOVIAGGIO_CONS.Text) Then
                    valid = False
                    errnd_COSTOVIAGGIO_CONS.Text = errInvalidNumber
                End If
            End If

            'crediti ECM
            If fl_PROFILOECM.Text = "1" And fl_EVENTOECM.Checked Then
                If nd_CREDITIECM.Text <> String.Empty Xor dt_OTTENIMENTOCREDITIECM.Text <> String.Empty Then
                    errnd_CREDITIECM.Text = "Compila n.crediti e data conseguimento o nessuno dei 2"
                Else

                    If nd_CREDITIECM.Text <> String.Empty Then
                        If Not ValidateItalianDecimal(nd_CREDITIECM.Text) Then
                            valid = False
                            errnd_CREDITIECM.Text = errInvalidNumber
                        End If
                    End If

                    If dt_OTTENIMENTOCREDITIECM.Text <> String.Empty Then
                        If Not ValidateItalianDate(dt_OTTENIMENTOCREDITIECM.Text) Then
                            valid = False
                            errdt_OTTENIMENTOCREDITIECM.Text = errInvalidNumber
                        End If
                    End If

                End If

            End If

        End If

        Return valid

    End Function

#End Region

    Private Sub lnkPickResponsabile_Click(sender As Object, e As EventArgs) Handles lnkPickResponsabile.Click

        If id_PERSONA_RESPONSABILE.Text = "" Then
            'tutto vuoto
            id_PERSONA_RESPONSABILE.Text = ""
            tx_PERSONA_RESPONSABILE.Text = ""
        Else
            'recipero i dati della persona
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            dbConn = DbConnectionHandler.GetOpenDataDbConn

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ext_DatiPersona"
                .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = CInt(id_PERSONA_RESPONSABILE.Text)
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()

            id_PERSONA_RESPONSABILE.Text = dbRdr.GetInt32(0).ToString
            tx_PERSONA_RESPONSABILE.Text = dbRdr.GetString(1).ToString
            dbRdr.Close()
            dbCmd.Dispose()
            dbConn.Close()
            dbConn.Dispose()

        End If

    End Sub

#Region "Warning"
    Public Sub SetWarning()

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'tutto invisibile
        pnlTipoContrattoKo.Visible = False
        pnlPartParallele.Visible = False

        'c'è una persona?
        If id_PERSONA.Text.Trim = String.Empty Then Exit Sub

        'OK, qualcosa
        dbConn = DbConnectionHandler.GetOpenDataDbConn
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_DiscrepanzePersona"

            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = CInt(id_PERSONA.Text)

            Select Case myMode
                Case Modes.Add
                    .Parameters.Add("@ac_TIPOPARTECIPAZIONE", SqlDbType.NVarChar, 8).Value = ac_TIPOPARTECIPAZIONE
                Case Modes.Edit
                    .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE
            End Select

            If dt_INIZIO.Text <> String.Empty And dt_INIZIO.Text <> "__/__/____" Then
                If ValidateItalianDate(dt_INIZIO.Text) Then
                    .Parameters.Add("@dt_INIZIO", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIO.Text)
                End If
            End If

            If dt_FINE.Text = String.Empty Or dt_FINE.Text = String.Empty Then
                If dt_INIZIO.Text <> String.Empty And dt_INIZIO.Text <> "__/__/____" Then
                    If ValidateItalianDate(dt_INIZIO.Text) Then
                        .Parameters.Add("@dt_FINE", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIO.Text)
                    End If
                End If
            Else
                If ValidateItalianDate(dt_FINE.Text) Then
                    .Parameters.Add("@dt_FINE", SqlDbType.Date).Value = ParseItalianDate(dt_FINE.Text)
                End If
            End If
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        pnlTipoContrattoKo.Visible = dbRdr.GetBoolean(0)
        pnlPartParallele.Visible = dbRdr.GetBoolean(1)

        dbRdr.Close()
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

    End Sub
#End Region

    Private Sub dt_INIZIO_TextChanged(sender As Object, e As EventArgs) Handles dt_INIZIO.TextChanged
        SetWarning()
        dt_FINE.Focus()
    End Sub

    Private Sub dt_FINE_TextChanged(sender As Object, e As EventArgs) Handles dt_FINE.TextChanged
        SetWarning()
        tx_PERSONA_RESPONSABILE.Focus()
    End Sub

    Private Sub lnkPickEvento_Click(sender As Object, e As EventArgs) Handles lnkPickEvento.Click

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = <xml>
SELECT
	tx_TITOLO,				--0
	ac_TIPOLOGIAEVENTO,		--1
	CAST(dt_INIZIO as datetime) as dt_INIZIO,		--2
	CAST(dt_FINE as datetime) as dt_FINE,		    --3
	tx_SEDE,				--4
	ac_NAZIONE,				--5
	tx_ORGANIZZATORE,		--6
    ac_TIPOCOBDETT          --7
FROM
	ext_EVENTI
WHERE
	id_EVENTO=@id_EVENTO
                           </xml>.Value
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = CInt(id_EVENTO.Text)
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then
            'pulizia
            tx_TITOLO.Text = ""
            ac_TIPOLOGIAEVENTO.SelectedValue = ""
            dt_INIZIO.Text = ""
            dt_FINE.Text = ""
            tx_SEDE.Text = ""
            ac_NAZIONE.SelectedValue = ""
            tx_ORGANIZZATORE.Text = ""
            ac_TIPOCOBDETT.SelectedValue = ""

            If Not dbRdr.IsDBNull(0) Then tx_TITOLO.Text = dbRdr.GetString(0)
            If Not dbRdr.IsDBNull(1) Then ac_TIPOLOGIAEVENTO.SelectedValue = dbRdr.GetString(1)
            If Not dbRdr.IsDBNull(2) Then dt_INIZIO.Text = FormatItalianDateY4(dbRdr.GetDateTime(2))
            If Not dbRdr.IsDBNull(3) Then dt_FINE.Text = FormatItalianDateY4(dbRdr.GetDateTime(3))
            If Not dbRdr.IsDBNull(4) Then tx_SEDE.Text = dbRdr.GetString(4)
            If Not dbRdr.IsDBNull(5) Then ac_NAZIONE.SelectedValue = dbRdr.GetString(5)
            If Not dbRdr.IsDBNull(6) Then tx_ORGANIZZATORE.Text = dbRdr.GetString(6)
            If Not dbRdr.IsDBNull(7) Then ac_TIPOCOBDETT.SelectedValue = dbRdr.GetString(7)

        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'rigenero il promemoria date
        SetWarning()

    End Sub
End Class