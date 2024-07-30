Imports Softailor.Global.ValidationUtils
Imports Softailor.Global.XmlParser

Public Class FormazioneIndividuale
    Inherits System.Web.UI.Page
    Implements IFOPage

    Const errRequired = "Campo obbligatorio"
    Const errInvalidDate = "Data non valida. Utilizza gg/mm/aaaa"
    Const errInvalidNumber = "Numero non valido"
    Const errNegativeNumber = "Devi inserire un numero maggiore di zero"

    Dim fpd As FOPageData
    Dim fl_PROFILOECM As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'determino se il profilo è ECM
        fl_PROFILOECM = ContextHandler.ProfiloECM(fpd.dbConn)

        'generazione lista
        RefreshListaPending()
    End Sub

    Private Sub RefreshListaPending()

        Dim dbCmd As SqlCommand

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_AutocertificazioniPending"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With

        phdPending.Controls.Clear()
        Dim sAspx = Transformer.Transform(dbCmd,
                                        "~/Templates/" & My.Settings.CompanyKey & "/Autocertificazioni_Formazione_Individuale.xslt",
                                        "fl_profiloecm", If(fl_PROFILOECM, "1", "0"))
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        Dim cCreato = Me.ParseControl(sAspx)
        phdPending.Controls.Add(cCreato)

        For Each c As Control In cCreato.Controls
            If TypeOf c Is LinkButton Then
                With CType(c, LinkButton)
                    If .ID Like "lnkEliminaAutoCertificazione_*" Then
                        AddHandler .Click, AddressOf lnkEliminaAutoCertificazione_Click
                    ElseIf .ID Like "lnkModificaAutoCertificazione_*" Then
                        AddHandler .Click, AddressOf lnkModificaAutoCertificazione_Click
                    End If
                End With
            End If
        Next

        updPending.Update()
    End Sub

    Private Sub lnkEliminaAutoCertificazione_Click(sender As Object, e As EventArgs)

        Dim id_PARTECIPAZIONE = CInt(CType(sender, LinkButton).CommandArgument)
        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_EliminaAutocertificazionePending"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        RefreshListaPending()

    End Sub

    Private Sub lnkModificaAutoCertificazione_Click(sender As Object, e As EventArgs)

        Dim id_PARTECIPAZIONE = CInt(CType(sender, LinkButton).CommandArgument)

        SetupFormAutocertificazione(id_PARTECIPAZIONE)

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess
        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn And ContextHandler.fl_DIPENDENTE
    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "formazione-individuale"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "Formazione Individuale"
    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange
        'vado alla home se sono uscito
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Response.Redirect("/", True)
        End If
    End Sub

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage

        Return False

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

    Private Sub lnkOpenEmptyForm_Click(sender As Object, e As EventArgs) Handles lnkOpenEmptyForm.Click

        SetupFormAutocertificazione(0)

    End Sub

    Private Sub SetupFormAutocertificazione(id_PARTECIPAZIONE_open As Integer)

        'titolo
        If id_PARTECIPAZIONE_open = 0 Then
            lblPopupTitle.Text = "Caricamento nuova auto-certificazione"
            lblStep1Title.Text = "Passo 1 di 3: Caricamento Dati"
        Else
            lblPopupTitle.Text = "Modifica dati auto-certificazione"
            lblStep1Title.Text = "Passo 1 di 3: Modifica Dati"
        End If

        'ID - scrittura nel controllo hidden
        id_PARTECIPAZIONE_in.Value = id_PARTECIPAZIONE_open.ToString

        'visibilità pannelli
        pnlDataEntry.Visible = True
        pnlVerifyData.Visible = False
        pnlPrintForm.Visible = False

        'svuoto tutti i controlli e rendo invisibili i pannelli che sono visibili in funzione di altri
        'rendo inoltre visibile o invisibile il pannello relativo ai crediti ECM
        ClearForm()

        'scrittura dati
        If id_PARTECIPAZIONE_open <> 0 Then
            ScriviDatiAutocertificazioneEsistente(id_PARTECIPAZIONE_open)
        End If

        updForm.Update()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "openPopup", "showAutocertificazionePopup(true);", True)

    End Sub

    Private Sub ScriviDatiAutocertificazioneEsistente(id_PARTECIPAZIONE_open As Integer)

        Dim dbCmd As SqlCommand
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim pNode As XmlNode

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_AutocertificazionePending"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE_open
        End With
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        pNode = xDoc.SelectSingleNode("/autocertificazione")

        'ruolo
        ac_CATEGORIAECM.SelectedValue = ParseXmlString(pNode, "ac_categoriaecm")

        'titolo
        tx_TITOLO.Text = ParseXmlString(pNode, "tx_titolo")

        'tipologia
        ac_TIPOLOGIAEVENTO.SelectedValue = ParseXmlString(pNode, "ac_tipologiaevento") & "\" & ParseXmlString(pNode, "fl_fad")
        ac_TIPOLOGIAEVENTO_SelectedIndexChanged(Nothing, Nothing)

        Select Case ParseXmlString(pNode, "fl_fad")
            Case "0"
                'non fad
                tx_SEDE.Text = ParseXmlString(pNode, "tx_sede")
                dt_INIZIO.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_inizio"))
                dt_FINE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_fine"))
                ni_ORE_RES.SelectedValue = ParseXmlString(pNode, "ni_ore")
                ni_MINUTI_RES.SelectedValue = ParseXmlString(pNode, "ni_minuti")
            Case "1"
                dt_INIZIOFRUIZIONE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_iniziofruizione"))
                dt_FINEFRUIZIONE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_finefruizione"))
                ni_ORE_FAD.SelectedValue = ParseXmlString(pNode, "ni_ore")
                ni_MINUTI_FAD.SelectedValue = ParseXmlString(pNode, "ni_minuti")
        End Select

        'esame di verifica
        ac_STATOVERIFICAAPPRENDIMENTO.SelectedValue = ParseXmlString(pNode, "ac_statoverificaapprendimento")

        'dati ECM
        If fl_PROFILOECM Then
            ac_NORMATIVAECM.SelectedValue = ParseXmlString(pNode, "ac_normativaecm")
            ac_NORMATIVAECM_SelectedIndexChanged(Nothing, Nothing)
            If ac_NORMATIVAECM.SelectedValue <> "NONE" Then
                ac_STATOECM.SelectedValue = ParseXmlString(pNode, "ac_statoecm")
                ac_STATOECM_SelectedIndexChanged(Nothing, Nothing)
                If ac_STATOECM.SelectedValue = "COK" Then
                    nd_CREDITIECM.Text = FormatItalianDecimal(ParseXmlDecimal(pNode, "nd_creditiecm"))
                    dt_OTTENIMENTOCREDITIECM.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_ottenimentocreditiecm"))
                End If
            End If
        End If
    End Sub

    Private Sub ClearForm()

        pnlDatiFAD.Visible = False
        pnlDatiNonFAD.Visible = False
        pnlCreditiConseguiti.Visible = False
        pnlNumeroCrediti.Visible = False
        pnlDataCrediti.Visible = False

        pnlDatiEcm.Visible = fl_PROFILOECM

        ac_CATEGORIAECM.SelectedIndex = -1
        tx_TITOLO.Text = ""
        ac_TIPOLOGIAEVENTO.SelectedIndex = -1

        dt_INIZIOFRUIZIONE.Text = ""
        dt_FINEFRUIZIONE.Text = ""
        ni_ORE_FAD.SelectedValue = ""
        ni_MINUTI_FAD.SelectedValue = ""

        tx_SEDE.Text = ""
        dt_INIZIO.Text = ""
        dt_FINE.Text = ""
        ni_ORE_RES.SelectedValue = ""
        ni_MINUTI_RES.SelectedValue = ""

        ac_STATOVERIFICAAPPRENDIMENTO.SelectedIndex = -1

        ac_NORMATIVAECM.SelectedIndex = -1
        ac_STATOECM.SelectedIndex = -1
        nd_CREDITIECM.Text = ""
        dt_OTTENIMENTOCREDITIECM.Text = ""

        id_PARTECIPAZIONE.Value = ""
        ni_ANNO.Value = ""
        ni_NUMERO.Value = ""

    End Sub

    Private Sub ac_TIPOLOGIAEVENTO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_TIPOLOGIAEVENTO.SelectedIndexChanged

        pnlDatiFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1")
        pnlDatiNonFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\0")

    End Sub

    Private Sub ac_NORMATIVAECM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_NORMATIVAECM.SelectedIndexChanged

        pnlCreditiConseguiti.Visible = ac_NORMATIVAECM.SelectedValue = "2011"
        pnlNumeroCrediti.Visible = ac_STATOECM.SelectedValue = "COK" And ac_NORMATIVAECM.SelectedValue = "2011"
        pnlDataCrediti.Visible = ac_STATOECM.SelectedValue = "COK" And ac_NORMATIVAECM.SelectedValue = "2011"

    End Sub

    Private Sub ac_STATOECM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_STATOECM.SelectedIndexChanged

        pnlNumeroCrediti.Visible = ac_STATOECM.SelectedValue = "COK"
        pnlDataCrediti.Visible = ac_STATOECM.SelectedValue = "COK"

    End Sub

    Private Sub lnkNext1_Click(sender As Object, e As EventArgs) Handles lnkNext1.Click

        If ValidateStep1() Then

            WriteSummary()

            pnlDataEntry.Visible = False
            pnlVerifyData.Visible = True

        End If

    End Sub

    Private Function ValidateStep1() As Boolean
        Dim allValid = True

        'pulizie
        tx_TITOLO.Text = tx_TITOLO.Text.Trim.ToUpper
        nd_CREDITIECM.Text = nd_CREDITIECM.Text.Trim
        dt_OTTENIMENTOCREDITIECM.Text = dt_OTTENIMENTOCREDITIECM.Text.Trim

        'ruolo
        If ac_CATEGORIAECM.SelectedIndex = -1 Then
            allValid = False
            err_ac_CATEGORIAECM.Text = errRequired
        End If

        'titolo
        If tx_TITOLO.Text = String.Empty Then
            allValid = False
            err_tx_TITOLO.Text = errRequired
        End If

        'tipologia
        If ac_TIPOLOGIAEVENTO.SelectedIndex = -1 Then
            allValid = False
            err_ac_TIPOLOGIAEVENTO.Text = errRequired
        Else
            If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then
                allValid = allValid And ValidateStep1_FAD()
            Else
                allValid = allValid And ValidateStep1_NonFAD()
            End If
        End If

        'esame verifica
        If ac_STATOVERIFICAAPPRENDIMENTO.SelectedIndex = -1 Then
            allValid = False
            err_ac_STATOVERIFICAAPPRENDIMENTO.Text = errRequired
        End If

        'ECM?
        If fl_PROFILOECM Then
            If ac_NORMATIVAECM.SelectedIndex = -1 Then
                allValid = False
                err_ac_NORMATIVAECM.Text = errRequired
            Else
                If ac_NORMATIVAECM.SelectedValue = "2011" Then
                    'accreditato
                    If ac_STATOECM.SelectedIndex = -1 Then
                        allValid = False
                        err_ac_STATOECM.Text = errRequired
                    Else
                        If ac_STATOECM.SelectedValue = "COK" Then
                            'crediti conseguiti
                            If nd_CREDITIECM.Text = String.Empty Then
                                allValid = False
                                err_nd_CREDITIECM.Text = errRequired
                            Else
                                If Not ValidateItalianDecimal(nd_CREDITIECM.Text) Then
                                    allValid = False
                                    err_nd_CREDITIECM.Text = errInvalidNumber
                                Else
                                    If ParseItalianDecimal(nd_CREDITIECM.Text) <= 0D Then
                                        allValid = False
                                        err_nd_CREDITIECM.Text = errNegativeNumber
                                    End If
                                End If
                            End If
                            'data crediti
                            If dt_OTTENIMENTOCREDITIECM.Text = String.Empty Then
                                allValid = False
                                err_dt_OTTENIMENTOCREDITIECM.Text = errRequired
                            Else
                                If Not ValidateItalianDate(dt_OTTENIMENTOCREDITIECM.Text) Then
                                    allValid = False
                                    err_dt_OTTENIMENTOCREDITIECM.Text = errInvalidDate
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return allValid
    End Function

    Private Function ValidateStep1_FAD() As Boolean

        'validazione dati FAD
        Dim allValid = True
        Dim iniOk = False
        Dim finOk = False

        'pulizie
        dt_INIZIOFRUIZIONE.Text = dt_INIZIOFRUIZIONE.Text.Trim
        dt_FINEFRUIZIONE.Text = dt_FINEFRUIZIONE.Text.Trim

        'data inizio fruizione
        If dt_INIZIOFRUIZIONE.Text = String.Empty Then
            allValid = False
            err_dt_INIZIOFRUIZIONE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_INIZIOFRUIZIONE.Text) Then
                allValid = False
                err_dt_INIZIOFRUIZIONE.Text = errInvalidDate
            Else
                iniOk = True
            End If
        End If

        'data fine fruizione
        If dt_FINEFRUIZIONE.Text = String.Empty Then
            allValid = False
            err_dt_FINEFRUIZIONE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_FINEFRUIZIONE.Text) Then
                allValid = False
                err_dt_FINEFRUIZIONE.Text = errInvalidDate
            Else
                finOk = True
            End If
        End If

        'ordine date
        If iniOk And finOk Then
            If ParseItalianDate(dt_INIZIOFRUIZIONE.Text) > ParseItalianDate(dt_FINEFRUIZIONE.Text) Then
                allValid = False
                err_dt_FINEFRUIZIONE.Text = "La data di fine precede la data di inizio."
            End If
        End If

        'durata totale
        allValid = allValid And ValidaOreMinuti(ni_ORE_FAD, ni_MINUTI_FAD, err_ni_ORE_FAD)

        Return allValid

    End Function

    Private Function ValidateStep1_NonFAD() As Boolean

        'validazione dati NON-FAD
        Dim allValid = True
        Dim iniOk = False
        Dim finOk = False

        'pulizie
        tx_SEDE.Text = tx_SEDE.Text.Trim.ToUpper
        dt_INIZIO.Text = dt_INIZIO.Text.Trim
        dt_FINE.Text = dt_FINE.Text.Trim

        'sede
        If tx_SEDE.Text = String.Empty Then
            allValid = False
            err_tx_SEDE.Text = errRequired
        End If

        'data inizio
        If dt_INIZIO.Text = String.Empty Then
            allValid = False
            err_dt_INIZIO.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_INIZIO.Text) Then
                allValid = False
                err_dt_INIZIO.Text = errInvalidDate
            Else
                iniOk = True
            End If
        End If

        'data fine
        If dt_FINE.Text = String.Empty Then
            allValid = False
            err_dt_FINE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_FINE.Text) Then
                allValid = False
                err_dt_FINE.Text = errInvalidDate
            Else
                finOk = True
            End If
        End If

        'ordine date
        If iniOk And finOk Then
            If ParseItalianDate(dt_INIZIO.Text) > ParseItalianDate(dt_FINE.Text) Then
                allValid = False
                err_dt_FINE.Text = "La data di fine precede la data di inizio."
            End If
        End If

        'durata totale
        allValid = allValid And ValidaOreMinuti(ni_ORE_RES, ni_MINUTI_RES, err_ni_ORE_RES)

        Return allValid

    End Function

    Private Function ValidaOreMinuti(ddnOre As DropDownList, ddnMinuti As DropDownList, errLbl As Label) As Boolean

        If ddnOre.SelectedValue = "" Or ddnMinuti.SelectedValue = "" Then
            errLbl.Text = "Entrambi i campi sono obbligatori"
            Return False
        Else
            If ddnOre.SelectedValue = "0" And ddnMinuti.SelectedValue = "0" Then
                errLbl.Text = "La durata non può essere pari a zero"
                Return False
            Else
                Return True
            End If

        End If

    End Function

    Private Function OreMinuti(ddnOre As DropDownList, ddnMinuti As DropDownList) As String

        Return ddnOre.SelectedValue & ":" & ddnMinuti.SelectedItem.Text

    End Function

    Private Function OreMinuti2Ore(ddnOre As DropDownList, ddnMinuti As DropDownList) As Integer

        Return 60 * CInt(ddnOre.SelectedValue) + CInt(ddnMinuti.SelectedValue)

    End Function

    Private Sub WriteSummary()

        r_tx_CATEGORIAECM.Text = ac_CATEGORIAECM.SelectedItem.Text
        r_tx_TITOLO.Text = tx_TITOLO.Text
        r_tx_TIPOLOGIAEVENTO.Text = ac_TIPOLOGIAEVENTO.SelectedItem.Text


        r_pnlDatiFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1")
        r_pnlDatiNonFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\0")
        If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then
            r_dt_INIZIOFRUIZIONE.Text = dt_INIZIOFRUIZIONE.Text
            r_dt_FINEFRUIZIONE.Text = dt_FINEFRUIZIONE.Text
            r_ni_MINUTI_FAD.Text = OreMinuti(ni_ORE_FAD, ni_MINUTI_FAD)
        Else
            r_tx_SEDE.Text = tx_SEDE.Text
            r_dt_INIZIO.Text = dt_INIZIO.Text
            r_dt_FINE.Text = dt_FINE.Text
            r_ni_MINUTI_RES.Text = OreMinuti(ni_ORE_RES, ni_MINUTI_RES)
        End If

        r_tx_STATOVERIFICAAPPRENDIMENTO.Text = ac_STATOVERIFICAAPPRENDIMENTO.SelectedItem.Text

        If fl_PROFILOECM Then
            r_pnlDatiEcm.Visible = True
            r_tx_NORMATIVAECM.Text = ac_NORMATIVAECM.SelectedItem.Text
            If ac_NORMATIVAECM.SelectedValue = "NONE" Then
                'non accreditato
                r_pnlCreditiConseguiti.Visible = False
                r_pnlNumeroCrediti.Visible = False
                r_pnlDataCrediti.Visible = False
            Else
                'accreditato
                r_pnlCreditiConseguiti.Visible = True
                r_tx_STATOECM.Text = ac_STATOECM.SelectedItem.Text
                If ac_STATOECM.SelectedValue = "COK" Then
                    r_pnlNumeroCrediti.Visible = True
                    r_pnlDataCrediti.Visible = True
                    r_nd_CREDITIECM.Text = nd_CREDITIECM.Text
                    r_dt_OTTENIMENTOCREDITIECM.Text = dt_OTTENIMENTOCREDITIECM.Text
                Else
                    r_pnlNumeroCrediti.Visible = False
                    r_pnlDataCrediti.Visible = False
                End If
            End If
        Else
            r_pnlDatiEcm.Visible = False
        End If

    End Sub

    Private Sub lnkNext2_Click(sender As Object, e As EventArgs) Handles lnkNext2.Click

        'creazione nel DB o modifica
        If id_PARTECIPAZIONE_in.Value = "0" Then
            CreaAutocertificazione()
        Else
            AggiornaAutocertificazione(CInt(id_PARTECIPAZIONE_in.Value))
        End If

        'setup pannello successivo
        If id_PARTECIPAZIONE_in.Value = "0" Then
            lblPrintIstruzioni.Text = "E' stata creata l'autocertificazione numero <b>" &
                ni_NUMERO.Value & "/" & ni_ANNO.Value & "</b>."
        Else
            lblPrintIstruzioni.Text = "Sono stati aggiornati i dati dell'autocertificazione numero <b>" &
                ni_NUMERO.Value & "/" & ni_ANNO.Value & "</b>."
        End If

        lnkStampaAutocertificazione.NavigateUrl = "/stampa-auto-certificazione/" & id_PARTECIPAZIONE.Value

        'update pending
        RefreshListaPending()

        'mostro il pannello
        pnlVerifyData.Visible = False
        pnlPrintForm.Visible = True
    End Sub

    Private Sub lnkPrevious2_Click(sender As Object, e As EventArgs) Handles lnkPrevious2.Click
        pnlVerifyData.Visible = False
        pnlDataEntry.Visible = True
    End Sub

    Private Sub CreaAutocertificazione()

        Dim dbCmd As SqlCommand
        Dim prmid_PARTECIPAZIONE As SqlParameter
        Dim prmni_ANNO As SqlParameter
        Dim prmni_NUMERO As SqlParameter

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_CreaAutocertificazione"

            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM.SelectedValue
            .Parameters.Add("@tx_TITOLO", SqlDbType.NVarChar, 600).Value = tx_TITOLO.Text
            .Parameters.Add("@ac_TIPOLOGIAEVENTO", SqlDbType.NVarChar, 8).Value = ac_TIPOLOGIAEVENTO.SelectedValue.Replace("\0", "").Replace("\1", "")

            If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then
                'FAD
                .Parameters.Add("@dt_INIZIOFRUIZIONE", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIOFRUIZIONE.Text)
                .Parameters.Add("@dt_FINEFRUIZIONE", SqlDbType.Date).Value = ParseItalianDate(dt_FINEFRUIZIONE.Text)
                .Parameters.Add("@ni_MINUTIFORMAZIONE", SqlDbType.Int).Value = OreMinuti2Ore(ni_ORE_FAD, ni_MINUTI_FAD)
            Else
                'NON-FAD
                .Parameters.Add("@tx_SEDE", SqlDbType.NVarChar, 300).Value = tx_SEDE.Text
                .Parameters.Add("@dt_INIZIO", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIO.Text)
                .Parameters.Add("@dt_FINE", SqlDbType.Date).Value = ParseItalianDate(dt_FINE.Text)
                .Parameters.Add("@ni_MINUTIFORMAZIONE", SqlDbType.Int).Value = OreMinuti2Ore(ni_ORE_RES, ni_MINUTI_RES)
            End If

            .Parameters.Add("@ac_STATOVERIFICAAPPRENDIMENTO", SqlDbType.NVarChar, 8).Value = ac_STATOVERIFICAAPPRENDIMENTO.SelectedValue

            If fl_PROFILOECM Then
                'ECM
                .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16).Value = ac_NORMATIVAECM.SelectedValue
                If ac_NORMATIVAECM.SelectedValue = "NONE" Then
                    'evento non accreditato
                    .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = "NC"
                Else
                    'evento accreditato
                    'stato ECM
                    .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = ac_STATOECM.SelectedValue
                    If ac_STATOECM.SelectedValue = "COK" Then
                        'crediti conseguiti
                        .Parameters.Add("@nd_CREDITIECM", SqlDbType.Money).Value = ParseItalianDecimal(nd_CREDITIECM.Text)
                        .Parameters.Add("@dt_OTTENIMENTOCREDITIECM", SqlDbType.Date).Value = ParseItalianDate(dt_OTTENIMENTOCREDITIECM.Text)
                    End If
                End If
            Else
                'non ECM
                .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16).Value = "NONE"
                .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = "NC"
            End If

            'parametri in uscita
            prmid_PARTECIPAZIONE = .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int)
            prmid_PARTECIPAZIONE.Direction = ParameterDirection.Output

            prmni_ANNO = .Parameters.Add("@ni_ANNO", SqlDbType.Int)
            prmni_ANNO.Direction = ParameterDirection.Output

            prmni_NUMERO = .Parameters.Add("@ni_NUMERO", SqlDbType.Int)
            prmni_NUMERO.Direction = ParameterDirection.Output

        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        id_PARTECIPAZIONE.Value = CInt(prmid_PARTECIPAZIONE.Value).ToString
        ni_ANNO.Value = CInt(prmni_ANNO.Value).ToString
        ni_NUMERO.Value = CInt(prmni_NUMERO.Value).ToString

    End Sub

    Private Sub AggiornaAutocertificazione(id_AUTOCERTIFICAZIONE_in As Integer)

        Dim dbCmd As SqlCommand
        Dim prmni_ANNO As SqlParameter
        Dim prmni_NUMERO As SqlParameter

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_AggiornaAutocertificazione"

            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_AUTOCERTIFICAZIONE_in

            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM.SelectedValue
            .Parameters.Add("@tx_TITOLO", SqlDbType.NVarChar, 600).Value = tx_TITOLO.Text
            .Parameters.Add("@ac_TIPOLOGIAEVENTO", SqlDbType.NVarChar, 8).Value = ac_TIPOLOGIAEVENTO.SelectedValue.Replace("\0", "").Replace("\1", "")

            If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then
                'FAD
                .Parameters.Add("@dt_INIZIOFRUIZIONE", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIOFRUIZIONE.Text)
                .Parameters.Add("@dt_FINEFRUIZIONE", SqlDbType.Date).Value = ParseItalianDate(dt_FINEFRUIZIONE.Text)
                .Parameters.Add("@ni_MINUTIFORMAZIONE", SqlDbType.Int).Value = OreMinuti2Ore(ni_ORE_FAD, ni_MINUTI_FAD)
            Else
                'NON-FAD
                .Parameters.Add("@tx_SEDE", SqlDbType.NVarChar, 300).Value = tx_SEDE.Text
                .Parameters.Add("@dt_INIZIO", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIO.Text)
                .Parameters.Add("@dt_FINE", SqlDbType.Date).Value = ParseItalianDate(dt_FINE.Text)
                .Parameters.Add("@ni_MINUTIFORMAZIONE", SqlDbType.Int).Value = OreMinuti2Ore(ni_ORE_RES, ni_MINUTI_RES)
            End If

            .Parameters.Add("@ac_STATOVERIFICAAPPRENDIMENTO", SqlDbType.NVarChar, 8).Value = ac_STATOVERIFICAAPPRENDIMENTO.SelectedValue

            If fl_PROFILOECM Then
                'ECM
                .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16).Value = ac_NORMATIVAECM.SelectedValue
                If ac_NORMATIVAECM.SelectedValue = "NONE" Then
                    'evento non accreditato
                    .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = "NC"
                Else
                    'evento accreditato
                    'stato ECM
                    .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = ac_STATOECM.SelectedValue
                    If ac_STATOECM.SelectedValue = "COK" Then
                        'crediti conseguiti
                        .Parameters.Add("@nd_CREDITIECM", SqlDbType.Money).Value = ParseItalianDecimal(nd_CREDITIECM.Text)
                        .Parameters.Add("@dt_OTTENIMENTOCREDITIECM", SqlDbType.Date).Value = ParseItalianDate(dt_OTTENIMENTOCREDITIECM.Text)
                    End If
                End If
            Else
                'non ECM
                .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16).Value = "NONE"
                .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = "NC"
            End If

            'parametri in uscita
            prmni_ANNO = .Parameters.Add("@ni_ANNO", SqlDbType.Int)
            prmni_ANNO.Direction = ParameterDirection.Output

            prmni_NUMERO = .Parameters.Add("@ni_NUMERO", SqlDbType.Int)
            prmni_NUMERO.Direction = ParameterDirection.Output

        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        id_PARTECIPAZIONE.Value = id_PARTECIPAZIONE_in.ToString
        ni_ANNO.Value = CInt(prmni_ANNO.Value).ToString
        ni_NUMERO.Value = CInt(prmni_NUMERO.Value).ToString

    End Sub

End Class