Imports Softailor.ReportEngine
Imports System.Net.Mail
Imports System.Configuration.ConfigurationManager

Public Class SpedizioneMailStep2
    Inherits System.Web.UI.Page

    'variabili per generazione
    Dim rptDbConn As SqlConnection
    Dim ac_FONTE As String
    Dim valoreFiltroBase As String
    Dim fonteXDoc As XmlDocument
    Dim myFonte As Fonte
    Dim chks As Dictionary(Of String, CheckBox)

    Private Sub SpedizioneMailStep2_Init(sender As Object, e As EventArgs) Handles Me.Init

        'timeout
        ScriptManager.GetCurrent(Me).AsyncPostBackTimeout = 6000

        'recupero campi dal parent
        If Not Page.IsPostBack Then
            With Request.Unvalidated.Form
                sd_ac_FONTE.Text = .Item("sd2_ac_FONTE")
                sd_id_MAILREPORT.Text = .Item("sd2_id_MAILREPORT")
                sd_tx_VALOREFILTROBASE.Text = .Item("sd2_tx_VALOREFILTROBASE")
                sd_xm_FILTRO.Text = .Item("sd2_xm_FILTRO")
                sd_ac_ORDINAMENTO.Text = .Item("sd2_ac_ORDINAMENTO")
                sd_tx_ORDINAMENTO1.Text = .Item("sd2_tx_ORDINAMENTO1")
                sd_tx_ORDINAMENTO2.Text = .Item("sd2_tx_ORDINAMENTO2")
                sd_tx_ORDINAMENTO3.Text = .Item("sd2_tx_ORDINAMENTO3")
                sd_tx_ORDINAMENTO4.Text = .Item("sd2_tx_ORDINAMENTO4")
                sd_tx_ORDINAMENTO5.Text = .Item("sd2_tx_ORDINAMENTO5")
                sd_tx_OGGETTO.Text = .Item("sd2_tx_OGGETTO")
                sd_ht_CORPO.Text = .Item("sd2_ht_CORPO")
                sd_ragionesociale.Text = .Item("sd2_ragionesociale")
                sd_indirizzocompleto.Text = .Item("sd2_indirizzocompleto")
                sd_tel.Text = .Item("sd2_tel")
                sd_fax.Text = .Item("sd2_fax")
                sd_email.Text = .Item("sd2_email")
            End With
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'apertura connessione
        rptDbConn = DbConnectionHandler.GetOpenRptDbConn

        'preparazione variabili per reportistica
        ac_FONTE = sd_ac_FONTE.Text
        valoreFiltroBase = sd_tx_VALOREFILTROBASE.Text
        fonteXDoc = New XmlDocument
        fonteXDoc.Load(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))
        myFonte = ReportEngine.Fonte.FromXml(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'generazione lista destinatari
        GeneraListaDestinatari()

    End Sub

    Private Sub SpedizioneMailStep2_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not rptDbConn Is Nothing Then
            If rptDbConn.State = ConnectionState.Open Then rptDbConn.Close()
            rptDbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraListaDestinatari()

        'dati filtro e ordinamento
        Dim datiFO = GetDatiFiltroOrdinamento()

        'generazione XML lista
        myFonte.GeneraXmlLista(rptDbConn, datiFO, valoreFiltroBase)

        'generazione lista a video
        Dim sAspx = Transformer.Transform(myFonte.GetXmlLista, "Templates/ListaDestinatari.xslt", "dummy", "dummy")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)

        Dim cCreato = Me.ParseControl(sAspx)

        'aggancio
        chks = New Dictionary(Of String, CheckBox)
        For Each c As Control In cCreato.Controls
            If TypeOf c Is CheckBox Then
                Dim cb = CType(c, CheckBox)
                If cb.ID Like "chksel_*" Then
                    chks.Add(Mid(cb.ID, 8), cb)
                End If
            End If
        Next

        phdLista.Controls.Clear()
        phdLista.Controls.Add(cCreato)

    End Sub

    Private Function GetDatiFiltroOrdinamento() As Fonte.DatiFiltroOrdinamento

        Dim datiFO As New Fonte.DatiFiltroOrdinamento

        'ordinamento
        If sd_ac_ORDINAMENTO.Text = String.Empty And
            sd_tx_ORDINAMENTO1.Text = String.Empty And
            sd_tx_ORDINAMENTO2.Text = String.Empty And
            sd_tx_ORDINAMENTO3.Text = String.Empty And
            sd_tx_ORDINAMENTO4.Text = String.Empty And
            sd_tx_ORDINAMENTO5.Text = String.Empty Then

            datiFO.TipoOrdinamento = Fonte.DatiFiltroOrdinamento.TipiOrdinamento.Nessuno

        Else
            If sd_ac_ORDINAMENTO.Text <> String.Empty Then
                datiFO.TipoOrdinamento = Fonte.DatiFiltroOrdinamento.TipiOrdinamento.Standard
                datiFO.codOrdinamentoStandard = sd_ac_ORDINAMENTO.Text
            Else
                datiFO.TipoOrdinamento = Fonte.DatiFiltroOrdinamento.TipiOrdinamento.Personalizzato
                datiFO.campoOrdPers1 = sd_tx_ORDINAMENTO1.Text
                datiFO.campoOrdPers2 = sd_tx_ORDINAMENTO2.Text
                datiFO.campoOrdPers3 = sd_tx_ORDINAMENTO3.Text
                datiFO.campoOrdPers4 = sd_tx_ORDINAMENTO4.Text
                datiFO.campoOrdPers5 = sd_tx_ORDINAMENTO5.Text
            End If
        End If

        'filtro
        If String.IsNullOrEmpty(myFonte.VistaCorpo) Then
            datiFO.filtro = Nothing
        Else
            'determino l'XML
            Dim filtroXml = sd_xm_FILTRO.Text

            If filtroXml = "" Then
                datiFO.filtro = Nothing
            Else
                datiFO.filtro = Filtro.FromXml(filtroXml, myFonte.CampiCorpo)
            End If

        End If

        Return datiFO

    End Function

    Private Sub lnkSpedizione_Click(sender As Object, e As EventArgs) Handles lnkSpedizione.Click

        Dim valoriChiave As New List(Of String)
        Dim key As String
        Dim mailMessage As MailMessage
        Dim smtpClient As New SmtpClient
        Dim email_from As String = GecFinalContextHandler.tx_MAILFROM
        Dim ragionesociale = sd_ragionesociale.Text
        Dim indirizzocompleto = sd_indirizzocompleto.Text
        Dim tel = sd_tel.Text
        Dim fax = sd_fax.Text
        Dim email = sd_email.Text
        Dim baseUrl = AppSettings("GF_FrontofficeBasePath_FromWeb")

        For Each key In chks.Keys
            If chks(key).Checked Then valoriChiave.Add(key)
        Next

        Dim mails2Send = myFonte.GeneraMails(rptDbConn, valoreFiltroBase, valoriChiave, sd_tx_OGGETTO.Text, sd_ht_CORPO.Text)

        For Each mail2Send In mails2Send

            Try
                mailMessage = New MailMessage

                With mailMessage
                    .From = New MailAddress(email_from, ragionesociale)
                    .Subject = mail2Send.subject
                    .IsBodyHtml = True
                    .Body = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainer.xslt", _
                                                    "ht_testo", mail2Send.body, _
                                                    "baseurl", baseUrl, _
                                                    "subject", mail2Send.subject, _
                                                    "ragionesociale", ragionesociale, _
                                                    "indirizzocompleto", indirizzocompleto, _
                                                    "tel", tel, _
                                                    "fax", fax, _
                                                    "email", email)
                    .To.Add(New MailAddress(mail2Send.recipient))
                    If My.Settings.BccMail <> "" Then .Bcc.Add(New MailAddress(My.Settings.BccMail))
                End With
                smtpClient.Send(mailMessage)
            Catch ex As Exception

            End Try
            'attesa
            Threading.Thread.Sleep(100)

        Next

        smtpClient.Dispose()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "doneall",
                                            "alert('La spedizione delle e-mail è stata effettuata correttamente.');parent.stl_sel_done('');",
                                            True)

    End Sub
End Class