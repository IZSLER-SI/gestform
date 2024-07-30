Imports System.Net.Mail
Imports Softailor.Global.ValidationUtils

Public Class EditProfile
    Inherits System.Web.UI.Page
    Implements IFOPage

    Private Const errRequired = "Campo obbligatorio"

    Dim fpd As FOPageData

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            FillData()
            GotoStep(1)
        End If

    End Sub

#Region "Scrittura Dati Iniziali"

    Private Sub FillData()
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim ac_profilo_here = ""
        Dim ac_codiceesterno_here = ""

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_EditProfile_GetData"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        WriteText(dbRdr, "ac_CODICEFISCALE", ac_CODICEFISCALE)

        WriteDdnString(dbRdr, "ac_TITOLO", ac_TITOLO)

        WriteText(dbRdr, "tx_COGNOME", tx_COGNOME)
        WriteText(dbRdr, "tx_NOME", tx_NOME)

        WriteDate(dbRdr, "dt_NASCITA", dt_NASCITA)
		
        'provincia nascita
        ac_PROVINCIA_nascita.DataBind()
        WriteDdnString(dbRdr, "ac_PROVINCIANASCITA", ac_PROVINCIA_nascita)
        'luogo nascita
        ac_LUOGO_nascita.DataBind()
        WriteDdnString(dbRdr, "ac_COMUNENASCITA", ac_LUOGO_nascita)
				'genere
				WriteDdnString(dbRdr, "ac_GENERE", ac_GENERE)
				WriteText(dbRdr, "tx_INDIRIZZO_res", tx_INDIRIZZO_res)
				'provincia res
				ac_PROVINCIA_res.DataBind()
        WriteDdnString(dbRdr, "ac_PROVINCIA_res", ac_PROVINCIA_res)
        'comune res
        ac_COMUNE_res.DataBind()
        WriteDdnString(dbRdr, "ac_COMUNE_res", ac_COMUNE_res)
        'cap res
        ac_CAP_res.DataBind()
        WriteDdnString(dbRdr, "ac_CAP_res", ac_CAP_res)

        WriteText(dbRdr, "tx_TELEFONO_res", tx_TELEFONO_res)
        WriteText(dbRdr, "tx_FAX_res", tx_FAX_res)
        WriteText(dbRdr, "tx_CELLULARE_res", tx_CELLULARE_res)
        WriteText(dbRdr, "tx_EMAILPEC_res", tx_EMAILPEC)

        'cat lavorativa
        ac_CATEGORIALAVORATIVA.DataBind()
        WriteDdnString(dbRdr, "ac_CATEGORIALAVORATIVA", ac_CATEGORIALAVORATIVA)
        'ruolo
        ac_RUOLO.DataBind()
        WriteDdnString(dbRdr, "ac_RUOLO", ac_RUOLO)
        'profilo
        ac_PROFILO.DataBind()
        WriteDdnString(dbRdr, "ac_PROFILO", ac_PROFILO)
        If (IsDipendente()) Then
            ac_CATEGORIALAVORATIVA.Attributes.Add("disabled", "disabled")
            ac_RUOLO.Attributes.Add("disabled", "disabled")
            ac_PROFILO.Attributes.Add("disabled", "disabled")
        End If
        'disciplina
        id_DISCIPLINA.DataBind()
        WriteDdnInt(dbRdr, "id_DISCIPLINA", id_DISCIPLINA)

        WriteText(dbRdr, "tx_ENTE_lav", tx_ENTE_lav)
        WriteText(dbRdr, "tx_INDIRIZZO_lav", tx_INDIRIZZO_lav)
        'provincia lav
        ac_PROVINCIA_lav.DataBind()
        WriteDdnString(dbRdr, "ac_PROVINCIA_lav", ac_PROVINCIA_lav)
        'comune lav
        ac_COMUNE_lav.DataBind()
        WriteDdnString(dbRdr, "ac_COMUNE_lav", ac_COMUNE_lav)
        'cap lav
        ac_CAP_lav.DataBind()
        WriteDdnString(dbRdr, "ac_CAP_lav", ac_CAP_lav)

        'lettura profilo e codice esterno
        If Not IsDBNull(dbRdr("ac_PROFILO")) Then ac_profilo_here = CStr(dbRdr("ac_PROFILO"))
        If Not IsDBNull(dbRdr("ac_CODICEESTERNO")) Then ac_codiceesterno_here = CStr(dbRdr("ac_CODICEESTERNO"))

        dbRdr.Close()
        dbCmd.Dispose()

        'gestione del codice esterno
        Dim cep As New CodiceEsternoProfilo(fpd.dbConn, ac_profilo_here)
        cep.SetupControlli(pnlac_CODICEESTERNO, lbl_ac_CODICEESTERNO)
        ac_CODICEESTERNO.Text = ac_codiceesterno_here

    End Sub

    Private Sub WriteText(dbRdr As SqlDataReader, field As String, txt As TextBox)

        If IsDBNull(dbRdr(field)) Then
            txt.Text = ""
        Else
            txt.Text = CStr(dbRdr(field))
        End If

    End Sub

    Private Sub WriteDate(dbRdr As SqlDataReader, field As String, txt As TextBox)

        If IsDBNull(dbRdr(field)) Then
            txt.Text = ""
        Else
            txt.Text = FormatItalianDateY4(CDate(dbRdr(field)))
        End If

    End Sub

    Private Sub WriteDdnString(dbRdr As SqlDataReader, field As String, ddn As DropDownList)
        If IsDBNull(dbRdr(field)) Then
            Try
                ddn.SelectedValue = ""
            Catch ex As Exception
            End Try
        Else
            Try
                ddn.SelectedValue = CStr(dbRdr(field))
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub WriteDdnInt(dbRdr As SqlDataReader, field As String, ddn As DropDownList)
        If IsDBNull(dbRdr(field)) Then
            Try
                ddn.SelectedValue = ""
            Catch ex As Exception
            End Try
        Else
            Try
                ddn.SelectedValue = CInt(dbRdr(field)).ToString
            Catch ex As Exception
            End Try
        End If
    End Sub


#End Region

    Protected Function IsDipendente() As Boolean
        Return ContextHandler.fl_DIPENDENTE
    End Function

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey

        Return "modifica-profilo"

    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return "Modifica Profilo"

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
            WriteRiep(tx_EMAILPEC, r_tx_EMAILPEC)

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

            GotoStep(2)
        End If

    End Sub

    Private Function ValidateStep1() As Boolean

        'pulizie
        TrimNoUpper(dt_NASCITA)
        TrimUpper(tx_INDIRIZZO_res)
        TrimNoUpper(tx_TELEFONO_res)
        TrimNoUpper(tx_FAX_res)
        TrimNoUpper(tx_CELLULARE_res)
        TrimLower(tx_EMAILPEC)
        TrimUpper(tx_ENTE_lav)
        TrimUpper(tx_INDIRIZZO_lav)


        Dim valid = True

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

        'PEC
        If tx_EMAILPEC.Text <> String.Empty Then
            If Not Softailor.Global.ValidationUtils.ValidateEmail(tx_EMAILPEC.Text) Then
                valid = False
                err_tx_EMAILPEC.Text = "Indirizzo PEC non valido"
            End If
        End If

        '------------------------------------
        If ac_CATEGORIALAVORATIVA.SelectedValue = "" Then
            valid = False
            err_ac_CATEGORIALAVORATIVA.Text = errRequired
        End If

        If (Not IsDipendente()) Then
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
            errStep1.Text = "Dati mancanti o non validi. Controlla i messaggi in rosso."
        End If

        Return valid

    End Function

#End Region

#Region "pulizia campi"

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

    Private Sub lnkPrevious2_Click(sender As Object, e As EventArgs) Handles lnkPrevious2.Click

        GotoStep(1)

    End Sub

#Region "Salvataggio"
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

    Private Overloads Function BitVal(b As Boolean) As Object
        If b = True Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Function AggiornaProfilo() As Boolean

        Dim dbCmd As SqlCommand
        AggiornaProfilo = True

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_EditProfile_SaveData"

            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA

            .Parameters.Add("@ac_TITOLO", SqlDbType.NVarChar, 10).Value = StringVal(ac_TITOLO)
            .Parameters.Add("@ac_GENERE", SqlDbType.NVarChar, 1).Value = StringVal(ac_GENERE)
            .Parameters.Add("@dt_NASCITA", SqlDbType.DateTime).Value = DateVal(dt_NASCITA)
            .Parameters.Add("@ac_COMUNENASCITA", SqlDbType.NVarChar, 4).Value = StringVal(ac_LUOGO_nascita)
            .Parameters.Add("@ac_CATEGORIALAVORATIVA", SqlDbType.NVarChar, 8).Value = StringVal(ac_CATEGORIALAVORATIVA)
            .Parameters.Add("@id_DISCIPLINA", SqlDbType.Int).Value = IntVal(id_DISCIPLINA)
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

            .Parameters.Add("@is_DIPENDENTE", SqlDbType.Bit).Value = BitVal(IsDipendente())
        End With
        Try
            dbCmd.ExecuteNonQuery()
        Catch ex As Exception
            AggiornaProfilo = False
        End Try
        dbCmd.Dispose()


    End Function
#End Region

    Private Sub lnkConfirm_Click(sender As Object, e As EventArgs) Handles lnkConfirm.Click
        If AggiornaProfilo() Then
            Try
                Dim id_PERSONA_value As Integer = ContextHandler.id_PERSONA
                Dim tx_NOME_value As String = tx_NOME.Text
                Dim tx_COGNOME_value As String = tx_COGNOME.Text
                Dim ac_CODICEFISCALE_value As String = ac_CODICEFISCALE.Text
                Dim dbCmd As SqlCommand
                dbCmd = fpd.dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "sp_log_getLastLogPersona"
                    .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_PERSONA_value
                End With
                Dim dbRdr As SqlDataReader = dbCmd.ExecuteReader

                Dim data_operazione As DateTime = Nothing
                Dim new_tx_MODIFICA As String = Nothing

                Dim old_tx_PROFILO As String = String.Empty
                Dim new_tx_PROFILO As String = String.Empty

                Dim old_tx_professione As String = String.Empty
                Dim new_tx_professione As String = String.Empty

                Dim old_tx_disciplina As String = String.Empty
                Dim new_tx_disciplina As String = String.Empty
                Dim new_tx_EMAIL As String = String.Empty

                Dim txt As String = String.Empty
                Dim i As Integer = 0
                While dbRdr.Read
                    data_operazione = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("data_operazione")), dbRdr.GetDateTime(dbRdr.GetOrdinal("data_operazione")), DateTime.Now)
                    new_tx_MODIFICA = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_MODIFICA")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_MODIFICA")), "n.d.")
                    new_tx_EMAIL = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_EMAIL")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_EMAIL")), "n.d.")
                    old_tx_PROFILO = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("old_tx_PROFILO")), dbRdr.GetString(dbRdr.GetOrdinal("old_tx_PROFILO")), "n.d.")
                    new_tx_PROFILO = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_PROFILO")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_PROFILO")), "n.d.")
                    old_tx_professione = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("old_tx_professione")), dbRdr.GetString(dbRdr.GetOrdinal("old_tx_professione")), "n.d.")
                    new_tx_professione = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_professione")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_professione")), "n.d.")
                    old_tx_disciplina = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("old_tx_disciplina")), dbRdr.GetString(dbRdr.GetOrdinal("old_tx_disciplina")), "n.d.")
                    new_tx_disciplina = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_disciplina")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_disciplina")), "n.d.")
                    i = i + 1
                End While
                dbRdr.Close()
                dbCmd.Dispose()
                If i > 0 Then
                    If new_tx_MODIFICA = "FrontOffice" Then
                        new_tx_MODIFICA = "FO"
                    Else
                        new_tx_MODIFICA = "BO"
                    End If
                    If data_operazione > DateTime.Now.AddSeconds(-10) And new_tx_MODIFICA = "FO" Then
                        If old_tx_PROFILO <> new_tx_PROFILO Then
                            txt += "<b> - Profilo precedente:</b> " + old_tx_PROFILO + "<br/>"
                            txt += "<b> - Profilo attuale:</b> " + new_tx_PROFILO + "<br/>"
                        End If
                        If old_tx_professione <> new_tx_professione Then
                            txt += "<b> - Professione precedente:</b> " + old_tx_professione + "<br/>"
                            txt += "<b> - Professione attuale:</b> " + new_tx_professione + "<br/>"
                        End If
                        If old_tx_disciplina <> new_tx_disciplina Then
                            txt += "<b> - Disciplina precedente:</b> " + old_tx_disciplina + "<br/>"
                            txt += "<b> - Disciplina attuale:</b> " + new_tx_disciplina + "<br/>"
                        End If
                        If txt.Length > 0 Then
                            ' Invio all'utente
                            sendEmail("FO", new_tx_EMAIL, txt, tx_NOME_value, tx_COGNOME_value, ac_CODICEFISCALE_value, data_operazione, new_tx_MODIFICA)
                            ' Invio all'ufficio Formazione
                            sendEmail("BO", My.Settings.GenericMail_MailFrom, txt, tx_NOME_value, tx_COGNOME_value, ac_CODICEFISCALE_value, data_operazione, new_tx_MODIFICA)
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
            GotoStep(3)
        Else
            errStep5.Text = "Si è verificato un errore durante il salvataggio dei dati."
        End If
    End Sub

    Private Function sendEmail(tipo_destinatario As String, destinatario As String, tx_change As String, tx_NOME As String, tx_COGNOME As String, ac_CODICEFISCALE As String, data_operazione As DateTime, new_tx_MODIFICA As String) As Boolean
        'generazione documento Xml
        Dim xDoc As XmlDocument
        Dim mailBody As String
        Try
            xDoc = New XmlDocument
            xDoc.LoadXml("<notifica>" &
                        "<tx_nome>" & tx_NOME & "</tx_nome>" &
                        "<tx_cognome>" & tx_COGNOME & "</tx_cognome>" &
                        "<tx_codicefiscale>" & ac_CODICEFISCALE & "</tx_codicefiscale>" &
                        "<tx_data>" & data_operazione.ToString("dd/MM/yyyy alle ore HH:mm") & "</tx_data>" &
                        "<tx_change><![CDATA[" & tx_change & "]]></tx_change>" &
                        "<tipo_destinatario>" & tipo_destinatario & "</tipo_destinatario>" &
                        "<new_tx_modifica>" & new_tx_MODIFICA & "</new_tx_modifica>" &
                        "<tx_titolo_evento></tx_titolo_evento>" &
                        "</notifica>")

            'spedizione mail
            Dim smtp As New SmtpClient
            Dim msg As New MailMessage
            mailBody = Transformer.Transform(xDoc,
                                             "~/Templates/" & My.Settings.CompanyKey & "/Mail/NotificaModProfiloUfficio.xslt",
                                             "baseurl", My.Settings.FrontOfficeUrl)
            With msg
                .From = New MailAddress(My.Settings.GenericMail_MailFrom)
                .Subject = "Conferma di modifica professione/disciplina nel profilo utente"
                .BodyEncoding = Encoding.UTF8
                .IsBodyHtml = True
                .Body = mailBody
                .To.Add(New MailAddress(destinatario))
            End With
            smtp.Send(msg)
            msg.Dispose()
            smtp.Dispose()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub ac_PROFILO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_PROFILO.SelectedIndexChanged

        Dim cep = New CodiceEsternoProfilo(fpd.dbConn, ac_PROFILO.SelectedValue)
        cep.SetupControlli(pnlac_CODICEESTERNO, lbl_ac_CODICEESTERNO)

    End Sub
End Class