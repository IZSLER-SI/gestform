Imports Softailor.Web.UI.DbForms
Imports Softailor.Global.SqlUtils
Imports System.Threading.Tasks
Imports System.Net.Mail
Imports System.IO
Imports System.Configuration.ConfigurationManager

Public Class Persona
    Inherits System.Web.UI.Page

    Dim id_PERSONA As Integer
    Public isDipendente As Boolean = True

    Dim dbConn As SqlConnection

    Const tipoAutocertificazione = "IND_EXT"
    Const tipoPartecipazione = "FOR_IND"
    Const tipoEvento = "EVE_INT"

    Private Sub Persona_Init(sender As Object, e As EventArgs) Handles Me.Init

        id_PERSONA = CInt(Request("id"))

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'verifica is dipendente
        Dim dbCmd = dbConn.CreateCommand
        Dim prmRet As SqlParameter
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_age_PersonaDipendente"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_PERSONA
            prmRet = .Parameters.Add("@retValue", SqlDbType.Bit)
            prmRet.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        isDipendente = CBool(prmRet.Value)
        dbCmd.Dispose()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sScript As String = ""

        'testi vari
        grdSTORICOLAVORATIVO.Title = "Storico rapporto lavorativo con " & My.Settings.CompanyName

        'valori di default
        sdsPERSONE_f.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsPERSONE_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME.ToString
        sdsSTORICOLAVORATIVO_g.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRECAPITI_g.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRECAPITI_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsRECAPITI_f.InsertParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRECAPITI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME
        sdsPORTFOLIO_g.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsPORTFOLIO_g.SelectParameters("tx_NOMEENTE").DefaultValue = My.Settings.CompanyName

        sdsPORTFOLIODOCENTE_g.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsPORTFOLIODOCENTE_g.SelectParameters("tx_NOMEENTE").DefaultValue = My.Settings.CompanyName

        sdsRUOLI_g.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRUOLI_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsRUOLI_f.InsertParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRUOLI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME


        'gestione degli script
        If Not Page.IsPostBack Then

            'autocertificazione
            sScript &= "function editAutocertificazione_callback(codice) { if(codice!='') {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkRepositionAutocertificazione, "").Replace("javascript:", "")
            sScript &= "}}" & vbCrLf

            sScript &= "function editPartecipazione_callback(codice) { if(codice!='') {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkRepositionPartecipazione, "").Replace("javascript:", "")
            sScript &= "}}" & vbCrLf

            sScript &= "function editIscritto_callback(codice) { if(codice!='') {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkRepositionIscritto, "").Replace("javascript:", "")
            sScript &= "}}" & vbCrLf

            Me.ltrRepositioning.Text = sScript
        End If

    End Sub

    Private Sub CloseMe()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMe",
           "parent.stl_sel_done('');", True)
    End Sub

    Private Sub lnkClose_Click(sender As Object, e As EventArgs) Handles lnkClose.Click

        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Prima di chiudere devi salvare o annullare le modifiche effettuate.');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('REFRESH');", True)
        End If

    End Sub

    Private Sub Persona_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub grdPORTFOLIO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdPORTFOLIO.SelectedIndexChanged

        If grdPORTFOLIO.SelectedIndex <> -1 Then

            Dim ac_ITEM As String = grdPORTFOLIO.SelectedDataKey.Value.ToString
            Dim idItem As Integer
            Dim idEvento As Integer
            Dim tipoItem = Mid(ac_ITEM, 1, 7)

            Select Case tipoItem
                Case tipoAutocertificazione
                    idItem = CInt(Mid(ac_ITEM, 9))
                    txtReposition.Text = idItem.ToString
                    txtRepositionEvento.Text = ""

                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupAutocertificazione",
                    "schedaAutocertificazione('" & idItem.ToString & "');", True)
                Case tipoPartecipazione
                    idItem = CInt(Mid(ac_ITEM, 9))
                    txtReposition.Text = idItem.ToString
                    txtRepositionEvento.Text = ""
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupPartecipazione",
                    "schedaPartecipazione('" & idItem.ToString & "');", True)
                Case tipoEvento
                    idItem = CInt(Mid(ac_ITEM, 9).Split("_"c)(0))
                    idEvento = CInt(Mid(ac_ITEM, 9).Split("_"c)(1))
                    txtReposition.Text = idItem.ToString
                    txtRepositionEvento.Text = idEvento.ToString
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupIscritto",
                    "schedaIscritto('" & idItem.ToString & "','" & idEvento.ToString & "');", True)
            End Select

            'per memorizzazione del valore
            updHiddenCtls.Update()


        End If

    End Sub

		Private Sub grdPORTFOLIODOCENTE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdPORTFOLIODOCENTE.SelectedIndexChanged

				If grdPORTFOLIODOCENTE.SelectedIndex <> -1 Then

						Dim ac_ITEM As String = grdPORTFOLIODOCENTE.SelectedDataKey.Value.ToString
						Dim idEvento As Integer
						Dim tipoItem = Mid(ac_ITEM, 1, 7)

						idEvento = CInt(Mid(ac_ITEM, 9).Split("_"c)(1))

						ScriptManager.RegisterStartupScript(Me, Me.GetType, "RedirectQuestionario",
					"schedaQuestionario('" & idEvento.ToString & "');", True)

						'per memorizzazione del valore
						updHiddenCtls.Update()

				End If

		End Sub

		Private Sub lnkRepositionAutocertificazione_Click(sender As Object, e As EventArgs) Handles lnkRepositionAutocertificazione.Click

        Dim findKey = tipoAutocertificazione & "_" & txtReposition.Text

        'deseleziono
        grdPORTFOLIO.SelectedIndex = -1
        'forzo databind
        grdPORTFOLIO.DataBind()
        grdPORTFOLIO.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdPORTFOLIO.DataKeys.Count - 1
            If grdPORTFOLIO.DataKeys(cIdx).Value.ToString = findKey Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdPORTFOLIO.SelectedIndex = sIdx
            grdPORTFOLIO.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)
        End If
    End Sub

    Private Sub lnkRepositionPartecipazione_Click(sender As Object, e As EventArgs) Handles lnkRepositionPartecipazione.Click

        Dim findKey = tipoPartecipazione & "_" & txtReposition.Text

        'deseleziono
        grdPORTFOLIO.SelectedIndex = -1
        'forzo databind
        grdPORTFOLIO.DataBind()
        grdPORTFOLIO.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdPORTFOLIO.DataKeys.Count - 1
            If grdPORTFOLIO.DataKeys(cIdx).Value.ToString = findKey Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdPORTFOLIO.SelectedIndex = sIdx
            grdPORTFOLIO.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)
        End If
    End Sub

    Private Sub lnkRepositionIscritto_Click(sender As Object, e As EventArgs) Handles lnkRepositionIscritto.Click

        Dim findKey = tipoEvento & "_" & txtReposition.Text & "_" & txtRepositionEvento.Text

        'deseleziono
        grdPORTFOLIO.SelectedIndex = -1
        'forzo databind
        grdPORTFOLIO.DataBind()
        grdPORTFOLIO.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdPORTFOLIO.DataKeys.Count - 1
            If grdPORTFOLIO.DataKeys(cIdx).Value.ToString = findKey Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdPORTFOLIO.SelectedIndex = sIdx
            grdPORTFOLIO.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)
        End If
    End Sub

    Private Sub lnkPasswordReset_Click(sender As Object, e As EventArgs) Handles lnkPasswordReset.Click

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bo_PasswordReset"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_PERSONA
            .Parameters.Add("@tx_PASSWORD", SqlDbType.NVarChar, 50).Value = RandomPassword()
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        txtPasswordResetResult.Text = "La password è stata reimpostata." & vbCrLf &
            "I nuovi dati per l'accesso sono i seguenti:" & vbCrLf &
            "Codice Fiscale: " & dbRdr.GetString(0) & vbCrLf &
            "Password: " & dbRdr.GetString(1) & vbCrLf &
            "Prestare attenzione alla distinzione maiuscole/minuscole nella password."

        dbRdr.Close()
        dbCmd.Dispose()

        txtPasswordResetResult.Visible = True

    End Sub

    Private Function RandomPassword() As String

        Dim rg As New Random()
        Dim sout As String = ""

        'lettera maiuscola
        sout &= Chr(Asc("A") + rg.Next(26))

        '3 lettere minuscole
        For i = 1 To 3
            sout &= Chr(Asc("a") + rg.Next(26))
        Next

        '4 cifre
        For i = 1 To 4
            sout &= Chr(Asc("0") + rg.Next(10))
        Next

        Return sout



    End Function

    Private Sub sdsPERSONE_f_updated(sender As Object, e As SqlDataSourceStatusEventArgs) Handles sdsPERSONE_f.Updated
        Dim id_PERSONA As Integer = CInt(e.Command.Parameters("@id_PERSONA").Value)
        Dim tx_NOME As String = e.Command.Parameters("@tx_NOME").Value.ToString
        Dim tx_COGNOME As String = e.Command.Parameters("@tx_COGNOME").Value.ToString
        Dim ac_CODICEFISCALE As String = e.Command.Parameters("@ac_CODICEFISCALE").Value.ToString
        Try
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            dbConn = DbConnectionHandler.GetOpenDataDbConn
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_log_getLastLogPersona"
                .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_PERSONA
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
                If data_operazione > DateTime.Now.AddSeconds(-10) And new_tx_MODIFICA = "BO" Then
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
                        sendEmail("FO", new_tx_EMAIL, txt, tx_NOME, tx_COGNOME, ac_CODICEFISCALE, data_operazione, new_tx_MODIFICA)
                        ' Invio all'ufficio Formazione
                        sendEmail("BO", My.Settings.GenericMail_MailTo, txt, tx_NOME, tx_COGNOME, ac_CODICEFISCALE, data_operazione, new_tx_MODIFICA)
                    End If
                End If
            End If


        Catch ex As Exception
        End Try

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

            'ottengo il body della mail
            'lettura dati globali
            Dim tx_BASEURL As String
            tx_BASEURL = AppSettings("GF_FrontofficeBasePath_FromWeb")

            'spedizione mail
            Dim smtp As New SmtpClient
            Dim msg As New MailMessage

            mailBody = Transformer.Transform(xDoc,
                                      "~/GFTemplates/Mail/" & My.Settings.CompanyName & "_NotificaModProfiloUfficio.xslt",
                                      "baseurl", tx_BASEURL)
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

End Class