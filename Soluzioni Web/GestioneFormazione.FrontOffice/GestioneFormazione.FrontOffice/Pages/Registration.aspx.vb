Imports Softailor.Global.ValidationUtils

Public Class Registration
    Inherits System.Web.UI.Page
    Implements IFOPage

    Private Const errRequired = "Campo obbligatorio"

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            GotoStep(1)
        End If

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'accesso: possibile solo se siamo fuori
        Return ContextHandler.Region = ContextHandler.Regions.LoggedOut

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "registrazione"

    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Registrazione"

    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'me ne vado in ogni caso
        Response.Redirect("/", True)

    End Sub

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage

        Return False

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

#Region "Cambi di step"
    Private Sub GotoStep(stepNo As Integer)
        pnlStep1.Visible = stepNo = 1
        pnlStep2.Visible = stepNo = 2
        pnlStep3.Visible = stepNo = 3
        pnlStep4.Visible = stepNo = 4
        pnlStep5.Visible = stepNo = 5
        pnlStep6.Visible = stepNo = 6
    End Sub
#End Region

#Region "Scrittura Dati Riepilogo"
    Public Overloads Sub WriteRiep(t As TextBox, l As Label)

        If t.Text = "" Then
            l.Text = "-"
        Else
            l.Text = t.Text
        End If

    End Sub

    Public Overloads Sub WriteRiep(d As DropDownList, l As Label)

        If d.SelectedValue = "" Then
            l.Text = "-"
        Else
            l.Text = d.SelectedItem.Text
        End If

    End Sub
#End Region

#Region "Step 1"
    Private Sub lnkNext1_Click(sender As Object, e As EventArgs) Handles lnkNext1.Click

        If ValidateStep1() Then

            'scrittura dati del riepilogo
            WriteRiep(ac_CODICEFISCALE, r_ac_CODICEFISCALE)

            GotoStep(2)
        End If

    End Sub
    Private Function ValidateStep1() As Boolean

        Dim valid = True

        'pulizia
        TrimUpper(ac_CODICEFISCALE)

        'verifica del codice fiscale
        If ac_CODICEFISCALE.Text = String.Empty Then
            err_ac_CODICEFISCALE.Text = errRequired
            valid = False
        Else
            If EsisteCodiceFiscale(ac_CODICEFISCALE.Text) Then
                err_ac_CODICEFISCALE.Text = "Il codice fiscale inserito risulta già registrato. Utilizza la funzione di reimpostazione password."
                valid = False
            Else
                If Not ValidateCodiceFiscaleItaliano(ac_CODICEFISCALE.Text) Then
                    err_ac_CODICEFISCALE.Text = "Codice fiscale formalmente errato"
                    valid = False
                End If
            End If
        End If

        If Not valid Then
            errStep1.Text = "Dati mancanti o non validi. Controlla i messaggi in rosso."
        End If

        Return valid

    End Function

    Private Function EsisteCodiceFiscale(cf As String) As Boolean

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_reg_CheckCodiceFiscale"
            .Parameters.Add("@ac_codicefiscale", SqlDbType.NVarChar, 16).Value = cf
        End With
        dbRdr = dbCmd.ExecuteReader
        EsisteCodiceFiscale = dbRdr.Read
        dbRdr.Close()
        dbCmd.Dispose()

    End Function
#End Region


#Region "Step 2"
    Private Sub lnkNext2_Click(sender As Object, e As EventArgs) Handles lnkNext2.Click

        If ValidateStep2() Then

            'scrittura dati del riepilogo
            WriteRiep(ac_TITOLO, r_tx_TITOLO)
            WriteRiep(tx_COGNOME, r_tx_COGNOME)
            WriteRiep(tx_NOME, r_tx_NOME)
            WriteRiep(dt_NASCITA, r_dt_NASCITA)
            WriteRiep(ac_PROVINCIA_nascita, r_ac_PROVINCIA_nascita)
            WriteRiep(ac_LUOGO_nascita, r_ac_LUOGO_nascita)
            WriteRiep(ac_GENERE, r_tx_GENERE)

            WriteRiep(tx_INDIRIZZO_res, r_tx_INDIRIZZO_res)
            WriteRiep(ac_CAP_res, r_tx_CAP_res)
            WriteRiep(ac_COMUNE_res, r_tx_COMUNE_res)
            WriteRiep(ac_PROVINCIA_res, r_tx_PROVINCIA_res)

            WriteRiep(tx_TELEFONO_res, r_tx_TELEFONO_res)
            WriteRiep(tx_FAX_res, r_tx_FAX_res)
            WriteRiep(tx_CELLULARE_res, r_tx_CELLULARE_res)
            WriteRiep(tx_EMAIL, r_tx_EMAIL)
            WriteRiep(tx_EMAILPEC, r_tx_EMAILPEC)

            GotoStep(3)
        End If

    End Sub

    Private Sub lnkPrevious2_Click(sender As Object, e As EventArgs) Handles lnkPrevious2.Click

        GotoStep(1)

    End Sub

    Private Function ValidateStep2() As Boolean

        Dim valid = True

        'pulizie
        PulisciCognomeNome(tx_COGNOME)
        PulisciCognomeNome(tx_NOME)
        TrimNoUpper(dt_NASCITA)
        TrimUpper(tx_INDIRIZZO_res)
        TrimNoUpper(tx_TELEFONO_res)
        TrimNoUpper(tx_FAX_res)
        TrimNoUpper(tx_CELLULARE_res)
        TrimLower(tx_EMAIL)
        TrimLower(tx_EMAIL_conf)
        TrimLower(tx_EMAILPEC)

        'validazione
        If ac_TITOLO.SelectedValue = "" Then
            valid = False
            err_ac_TITOLO.Text = errRequired
        End If

        If tx_COGNOME.Text = "" Then
            valid = False
            err_tx_COGNOME.Text = errRequired
        End If

        If tx_NOME.Text = "" Then
            valid = False
            err_tx_NOME.Text = errRequired
        End If

        If dt_NASCITA.Text = "" Then
            valid = False
            err_dt_NASCITA.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_NASCITA.Text) Then
                valid = False
                err_dt_NASCITA.Text = "Data non valida. Utilizza gg/mm/aaaa"
            End If
        End If

        If ac_PROVINCIA_nascita.SelectedValue = "" Then
            valid = False
            err_ac_PROVINCIA_nascita.Text = errRequired
        End If

        If ac_LUOGO_nascita.SelectedValue = "" Then
            valid = False
            err_ac_LUOGO_nascita.Text = errRequired
        End If

        If ac_GENERE.SelectedValue = "" Then
            valid = False
            err_ac_GENERE.Text = errRequired
        End If

        If tx_INDIRIZZO_res.Text = "" Then
            valid = False
            err_tx_INDIRIZZO_res.Text = errRequired
        End If

        If ac_PROVINCIA_res.SelectedValue = "" Then
            valid = False
            err_ac_PROVINCIA_res.Text = errRequired
        End If

        If ac_COMUNE_res.SelectedValue = "" Then
            valid = False
            err_ac_COMUNE_res.Text = errRequired
        End If

        If ac_CAP_res.SelectedValue = "" Then
            valid = False
            err_ac_CAP_res.Text = errRequired
        End If

        If tx_CELLULARE_res.Text = "" Then
            valid = False
            err_tx_CELLULARE_res.Text = errRequired
        End If

        'e-mail
        If tx_EMAIL.Text = String.Empty Then
            valid = False
            err_tx_EMAIL.Text = errRequired
        Else
            If Not Softailor.Global.ValidationUtils.ValidateEmail(tx_EMAIL.Text) Then
                valid = False
                err_tx_EMAIL.Text = "Indirizzo e-mail non valido"
            Else
                If String.Compare(tx_EMAIL.Text, tx_EMAIL_conf.Text, True) <> 0 Then
                    valid = False
                    err_tx_EMAIL_conf.Text = "I due indirizzi e-mail non coincidono"
                End If
            End If
        End If

        'PEC
        If tx_EMAILPEC.Text <> String.Empty Then
            If Not Softailor.Global.ValidationUtils.ValidateEmail(tx_EMAILPEC.Text) Then
                valid = False
                err_tx_EMAILPEC.Text = "Indirizzo PEC non valido"
            End If
        End If

        If Not valid Then
            errStep2.Text = "Dati mancanti o non validi. Controlla i messaggi in rosso."
        End If

        Return valid


    End Function

    
#End Region

#Region "Step 3"
    Private Sub lnkNext3_Click(sender As Object, e As EventArgs) Handles lnkNext3.Click

        If ValidateStep3() Then

            'scrittura dati del riepilogo
            WriteRiep(ac_CATEGORIALAVORATIVA, r_tx_CATEGORIALAVORATIVA)
            WriteRiep(ac_RUOLO, r_tx_RUOLO)
            WriteRiep(ac_PROFILO, r_tx_PROFILO)
            WriteRiep(id_DISCIPLINA, r_tx_DISCIPLINA)

            WriteRiep(tx_ENTE_lav, r_tx_ENTE_lav)
            WriteRiep(tx_INDIRIZZO_lav, r_tx_INDIRIZZO_lav)
            WriteRiep(ac_CAP_lav, r_tx_CAP_lav)
            WriteRiep(ac_COMUNE_lav, r_tx_COMUNE_lav)
            WriteRiep(ac_PROVINCIA_lav, r_tx_PROVINCIA_lav)

            'gestione del codice esterno
            Dim cep As New CodiceEsternoProfilo(fpd.dbConn, ac_PROFILO.SelectedValue)
            If cep.tx_CODICEESTERNO_DESC = "" Then
                pnl_r_ac_CODICEESTERNO.Visible = False
            Else
                pnl_r_ac_CODICEESTERNO.Visible = True
                lbl_r_ac_CODICEESTERNO.Text = cep.tx_CODICEESTERNO_DESC
                r_ac_CODICEESTERNO.Text = ac_CODICEESTERNO.Text
            End If

            GotoStep(4)
        End If

    End Sub

    Private Sub lnkPrevious3_Click(sender As Object, e As EventArgs) Handles lnkPrevious3.Click

        GotoStep(2)

    End Sub

    Private Function ValidateStep3() As Boolean

        Dim valid = True

        'pulizie
        TrimUpper(tx_ENTE_lav)
        TrimUpper(tx_INDIRIZZO_lav)

        If ac_CATEGORIALAVORATIVA.SelectedValue = "" Then
            valid = False
            err_ac_CATEGORIALAVORATIVA.Text = errRequired
        End If

        If ac_RUOLO.SelectedValue = "" Then
            valid = False
            err_ac_RUOLO.Text = errRequired
        End If

        If ac_PROFILO.SelectedValue = "" Then
            valid = False
            err_ac_PROFILO.Text = errRequired
        Else
            Dim cep As New CodiceEsternoProfilo(fpd.dbConn, ac_PROFILO.SelectedValue)
            valid = valid And cep.Validate(ac_CODICEESTERNO, err_ac_CODICEESTERNO, errRequired)
        End If

        If id_DISCIPLINA.SelectedValue = "" And id_DISCIPLINA.Items.Count > 1 Then
            valid = False
            err_id_DISCIPLINA.Text = errRequired
        End If

        If tx_ENTE_lav.Text <> "" Or tx_INDIRIZZO_lav.Text <> "" Or ac_PROVINCIA_lav.SelectedValue <> "" Or ac_COMUNE_lav.SelectedValue <> "" Or ac_CAP_lav.SelectedValue <> "" Then
            If tx_ENTE_lav.Text = "" Or tx_INDIRIZZO_lav.Text = "" Or ac_PROVINCIA_lav.SelectedValue = "" Or ac_COMUNE_lav.SelectedValue = "" Or ac_CAP_lav.SelectedValue = "" Then
                valid = False
                err_tx_ENTE_lav.Text = "Specifica tutti i dati dell'ente ove lavori oppure lascia vuoti tutti i campi."
            End If
        End If

        If Not valid Then
            errStep3.Text = "Dati mancanti o non validi. Controlla i messaggi in rosso."
        End If

        Return valid

    End Function

#End Region

#Region "Step 4"
    Private Sub lnkNext4_Click(sender As Object, e As EventArgs) Handles lnkNext4.Click

        If ValidateStep4() Then

            'scrivo la password nel campo hidden
            hidPassword.Value = tx_PASSWORD.Text
            GotoStep(5)
        End If

    End Sub

    Private Sub lnkPrevious4_Click(sender As Object, e As EventArgs) Handles lnkPrevious4.Click

        'pulizia password
        hidPassword.Value = ""
        GotoStep(3)

    End Sub

    Private Function ValidateStep4() As Boolean

        Dim valid = True

        'validazione nuova password
        If tx_PASSWORD.Text.Trim = String.Empty Then
            valid = False
            err_tx_PASSWORD.Text = errRequired
        Else
            If Len(tx_PASSWORD.Text) < 8 Then
                valid = False
                err_tx_PASSWORD.Text = "Lunghezza minima 8 caratteri"
            Else
                If String.Compare(tx_PASSWORD.Text, tx_PASSWORD_conf.Text, False) <> 0 Then
                    valid = False
                    err_tx_PASSWORD_conf.Text = "Le due password non coincidono"
                End If
            End If
        End If

        If Not valid Then
            errStep4.Text = "Dati mancanti o non validi. Controlla i messaggi in rosso."
        End If

        Return valid

    End Function

#End Region

#Region "Step 5"
    Private Sub lnkRegister_Click(sender As Object, e As EventArgs) Handles lnkRegister.Click

        If ValidateStep5() Then
            If CreaPersona() Then
                GotoStep(6)
            Else
                errStep5.Text = "Si è verificato un errore durante il completamento della registrazione."
            End If

        End If

    End Sub

    Private Sub lnkPrevious5_Click(sender As Object, e As EventArgs) Handles lnkPrevious5.Click

        GotoStep(4)

    End Sub

    Private Function ValidateStep5() As Boolean

        Dim valid = True

        'pulizia
        chkConsensoPrivacy.BackColor = Drawing.Color.Empty
        chkConsensoCopyright.BackColor = Drawing.Color.Empty

        If Not chkConsensoPrivacy.Checked Then
            valid = False
            chkConsensoPrivacy.BackColor = Drawing.Color.Yellow
        End If

        If Not chkConsensoCopyright.Checked Then
            valid = False
            chkConsensoCopyright.BackColor = Drawing.Color.Yellow
        End If

        If Not valid Then
            errStep5.Text = "Dati mancanti. Controlla i campi in giallo."
        End If

        Return valid

    End Function

#End Region

#Region "pulizia campi"
    Private Sub PulisciCognomeNome(t As TextBox)
        t.Text = t.Text.Trim.Replace("à", "A'").Replace("è", "E'").Replace("é", "E'").Replace("ì", "I'").Replace("ò", "O'").Replace("ù", "U'").ToUpper
        t.Text = t.Text.Replace("À", "A'").Replace("È", "E'").Replace("É", "E'").Replace("Ì", "I'").Replace("Ò", "O'").Replace("Ù", "U'")

    End Sub

    Private Sub TrimNoUpper(t As TextBox)
        t.Text = t.Text.Trim
    End Sub

    Private Sub TrimUpper(t As TextBox)
        t.Text = t.Text.Trim.ToUpper
    End Sub

    Private Sub TrimLower(t As TextBox)
        t.Text = t.Text.Trim.ToLower
    End Sub

#End Region

    Private Function CreaPersona() As Boolean

        Dim dbCmd As SqlCommand
        CreaPersona = True

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_reg_CreaPersona"
            .Parameters.Add("@ac_TITOLO", SqlDbType.NVarChar, 10).Value = StringVal(ac_TITOLO)
            .Parameters.Add("@tx_COGNOME", SqlDbType.NVarChar, 100).Value = StringVal(tx_COGNOME)
            .Parameters.Add("@tx_NOME", SqlDbType.NVarChar, 100).Value = StringVal(tx_NOME)
            .Parameters.Add("@ac_CODICEFISCALE", SqlDbType.NVarChar, 16).Value = StringVal(ac_CODICEFISCALE)
            .Parameters.Add("@ac_GENERE", SqlDbType.NVarChar, 1).Value = StringVal(ac_GENERE)
            .Parameters.Add("@dt_NASCITA", SqlDbType.DateTime).Value = DateVal(dt_NASCITA)
            .Parameters.Add("@ac_COMUNENASCITA", SqlDbType.NVarChar, 4).Value = StringVal(ac_LUOGO_nascita)
            .Parameters.Add("@ac_CATEGORIALAVORATIVA", SqlDbType.NVarChar, 8).Value = StringVal(ac_CATEGORIALAVORATIVA)
            .Parameters.Add("@id_DISCIPLINA", SqlDbType.Int).Value = IntVal(id_DISCIPLINA)
            .Parameters.Add("@tx_EMAIL", SqlDbType.NVarChar, 150).Value = StringVal(tx_EMAIL)
            .Parameters.Add("@tx_PASSWORD", SqlDbType.NVarChar, 50).Value = StringVal(hidPassword)
            .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = StringVal(ac_PROFILO)
            .Parameters.Add("@ac_RUOLO", SqlDbType.NVarChar, 4).Value = StringVal(ac_RUOLO)
            .Parameters.Add("@ac_CODICEESTERNO", SqlDbType.NVarChar, 16).Value = StringVal(ac_CODICEESTERNO)

            .Parameters.Add("@tx_INDIRIZZO_res", SqlDbType.NVarChar, 300).Value = StringVal(tx_INDIRIZZO_res)
            .Parameters.Add("@ac_CAP_res", SqlDbType.NVarChar, 5).Value = StringVal(ac_CAP_res)
            .Parameters.Add("@ac_COMUNE_res", SqlDbType.NVarChar, 4).Value = StringVal(ac_COMUNE_res)
            .Parameters.Add("@tx_TELEFONO_res", SqlDbType.NVarChar, 20).Value = StringVal(tx_TELEFONO_res)
            .Parameters.Add("@tx_FAX_res", SqlDbType.NVarChar, 20).Value = StringVal(tx_FAX_res)
            .Parameters.Add("@tx_CELLULARE_res", SqlDbType.NVarChar, 20).Value = StringVal(tx_CELLULARE_res)
            .Parameters.Add("@tx_EMAILPEC_res", SqlDbType.NVarChar, 20).Value = StringVal(tx_EMAILPEC)

            .Parameters.Add("@tx_ENTE_lav", SqlDbType.NVarChar, 300).Value = StringVal(tx_ENTE_lav)
            .Parameters.Add("@tx_INDIRIZZO_lav", SqlDbType.NVarChar, 300).Value = StringVal(tx_INDIRIZZO_lav)
            .Parameters.Add("@ac_CAP_lav", SqlDbType.NVarChar, 5).Value = StringVal(ac_CAP_lav)
            .Parameters.Add("@ac_COMUNE_lav", SqlDbType.NVarChar, 4).Value = StringVal(ac_COMUNE_lav)
        End With
        Try
            dbCmd.ExecuteNonQuery()
        Catch ex As Exception
            Try
                Dim smtpClient As New System.Net.Mail.SmtpClient()
                Dim mailMsg As New System.Net.Mail.MailMessage
                With mailMsg
                    .From = New System.Net.Mail.MailAddress(My.Settings.ErrorReportMailFrom)
                    .Subject = My.Settings.ErrorReportMailSubject
                    .IsBodyHtml = True
                    .Body = "Errore nella procedura di registrazione: " & ex.ToString
                    For Each address As String In My.Settings.ErrorReportMailTo.Split(";"c)
                        If address.Trim <> String.Empty Then
                            .To.Add(New System.Net.Mail.MailAddress(address.Trim))
                        End If
                    Next
                End With
                smtpClient.Send(mailMsg)
            Catch ex2 As Exception

            End Try
            CreaPersona = False
        End Try
        dbCmd.Dispose()


    End Function

    Private Overloads Function DateVal(t As TextBox) As Object
        If t.Text = String.Empty Then
            Return DBNull.Value
        Else
            Return ParseItalianDate(t.Text)
        End If
    End Function

    Private Overloads Function StringVal(t As TextBox) As Object
        If t.Text = String.Empty Then
            Return DBNull.Value
        Else
            Return t.Text
        End If
    End Function

    Private Overloads Function StringVal(d As DropDownList) As Object
        If d.SelectedValue = String.Empty Then
            Return DBNull.Value
        Else
            Return d.SelectedValue
        End If
    End Function

    Private Overloads Function StringVal(h As HiddenField) As Object
        If h.Value = String.Empty Then
            Return DBNull.Value
        Else
            Return h.Value
        End If
    End Function

    Private Overloads Function IntVal(d As DropDownList) As Object
        If d.SelectedValue = String.Empty Then
            Return DBNull.Value
        Else
            Return CInt(d.SelectedValue)
        End If
    End Function

    Private Sub ac_PROFILO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_PROFILO.SelectedIndexChanged

        Dim cep = New CodiceEsternoProfilo(fpd.dbConn, ac_PROFILO.SelectedValue)
        cep.SetupControlli(pnlac_CODICEESTERNO, lbl_ac_CODICEESTERNO)

    End Sub

End Class