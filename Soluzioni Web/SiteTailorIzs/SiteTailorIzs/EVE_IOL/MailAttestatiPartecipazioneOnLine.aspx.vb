Imports System.Configuration.ConfigurationManager
Imports Softailor.Global.XmlParser

Public Class MailAttestatiPartecipazioneOnLine
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Protected WithEvents btnInvioUlteriore As Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'timeout
        ScriptManager.GetCurrent(Me).AsyncPostBackTimeout = 6000

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        SetupPage()

    End Sub

    Private Sub MailAttestatiPartecipazioneOnLine_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub SetupPage()

        Dim fl_ATTPARTONLINE As Boolean
        Dim dt_INVIOMAILATTPART As SqlDateTime

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        
        'pulizie
        lblNumPartecipantiInterni.Text = ""
        lblNumPartecipantiEsterni.Text = ""
        lblNumPartecipantiInterniTest.Text = ""
        lblNumPartecipantiEsterniTest.Text = ""
        lblNumDRMTEsterni.Text = ""
        lblNumDRMTInterni.Text = ""
        lblListaAttivi.Text = ""
        lblStatoInvio.Text = ""
        pnlWarningInvia.Visible = False
        lnkInvia.Enabled = False
        lnkInvia.Visible = False
        pnlWarning2invia.Visible = False
        lblRisultatoInvio.Text = ""
        pnlSpacer1.Visible = True
        pnlSpacer2.Visible = True
        pnlSpacer3.Visible = True
        pnlSpacer4.Visible = True

        phdRiepiloghiToDo.Controls.Clear()
        phdRiepiloghiDone.Controls.Clear()

        'lettura dati
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_part_DettagliAttivazionePART"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        'labels
        lblListaAttivi.Text = dbRdr.GetString(0)
        lblNumPartecipantiInterni.Text = dbRdr.GetInt32(1).ToString
        lblNumPartecipantiEsterni.Text = dbRdr.GetInt32(2).ToString
        lblNumDRMTInterni.Text = dbRdr.GetInt32(3).ToString
        lblNumDRMTEsterni.Text = dbRdr.GetInt32(4).ToString
        lblNumPartecipantiInterniTest.Text = dbRdr.GetInt32(7).ToString
        lblNumPartecipantiEsterniTest.Text = dbRdr.GetInt32(8).ToString

        'flag
        fl_ATTPARTONLINE = dbRdr.GetBoolean(5)
        dt_INVIOMAILATTPART = dbRdr.GetSqlDateTime(6)

        dbRdr.Close()
        dbCmd.Dispose()

        '3 possibilità
        If Not dt_INVIOMAILATTPART.IsNull Then
            'mail già inviate
            lblStatoInvio.Text = "Ultimo invio mail di notifica disponibilità attestati di partecipazione effettuato in data <b>" & dt_INVIOMAILATTPART.Value.ToString("dd/MM/yyyy HH:mm") & "</b>."
            lblStatoInvio.ForeColor = Drawing.Color.DarkGreen
            pnlSpacer1.Visible = False
            pnlSpacer2.Visible = False
            pnlSpacer3.Visible = False
            pnlSpacer4.Visible = False
            GeneraRiepiloghiDone()
        Else
            'mail da inviare
            lblStatoInvio.Text = "Mail di notifica disponibilità attestati non ancora inviate."
            lblStatoInvio.ForeColor = Drawing.Color.Empty
            If fl_ATTPARTONLINE Then
                'OK si può fare
                pnlWarningInvia.Visible = True
                lnkInvia.Visible = True
                lnkInvia.Enabled = True
                pnlWarning2invia.Visible = True
                GeneraRiepiloghiToDo()
            Else
                pnlWarningInvia.Visible = True
                lnkInvia.Visible = True
                lnkInvia.Enabled = False
                pnlWarning2invia.Visible = True
                lblRisultatoInvio.Text = "Impossibile inviare le e-mail: lo scaricamento dell'attestato di partecipazione on line non è stato attivato per nessuna categoria."
                lblRisultatoInvio.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Private Sub GeneraRiepiloghiToDo()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_part_StatoInvioMailAttestatiPART"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Transformer.Transform(dbCmd, "Templates/StatoInvioMailAttestatiPART.xslt", phdRiepiloghiToDo, "mode", "todo")

    End Sub

    Private Sub GeneraRiepiloghiDone()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_part_StatoInvioMailAttestatiPART"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Dim sAspx = Transformer.Transform(dbCmd, "Templates/StatoInvioMailAttestatiPART.xslt", "mode", "done")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        Dim cCreato = Me.Page.ParseControl(sAspx)

        phdRiepiloghiDone.Controls.Clear()
        phdRiepiloghiDone.Controls.Add(cCreato)

        btnInvioUlteriore = CType(cCreato.FindControl("lnkInvioUlteriore"), Button)

    End Sub


    Private Sub lnkInvia_Click(sender As Object, e As System.EventArgs) Handles lnkInvia.Click

        'invio vero e proprio
        SendMails()

        'marco come spedito
        MarkEventAsSent()

        'refresh dati
        SetupPage()

        'messaggio
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "doneall", "window.alert('La spedizione delle mail è stata correttamente effettuata. Controlla i risultati a video.');", True)

    End Sub

    Private Enum SendingResult
        unknown
        sent
        mailnotpresent
        errorsending
    End Enum

    Private Sub SendMails()

        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim myXDoc As XmlDocument

        'dati globali
        Dim tx_BASEURL As String
        Dim subject As String

        'dati per generazione mail
        Dim id_ISCRITTO As Integer
        Dim tx_EMAIL As String
        Dim tx_VOCATIVO As String
        Dim tx_COGNOME As String
        Dim tx_NOME As String
        Dim tx_TITOLOEVENTO As String
        Dim tx_SEDE As String
        Dim dt_INIZIO As String
        Dim dt_FINE As String
        'Dim smtpServer As String
        Dim sResult As SendingResult

        'lettura dati globali
        tx_BASEURL = AppSettings("GF_FrontofficeBasePath_FromWeb")
        'smtpServer = ContextHandler.SmtpServer

        'lettura doc XML
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_part_StatoInvioMailAttestatiPART"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        xReader = dbCmd.ExecuteXmlReader
        myXDoc = New XmlDocument
        myXDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        'ciclo sui nodi
        If myXDoc.SelectNodes("/persone").Count > 0 Then

            For Each pNode As XmlNode In myXDoc.SelectNodes("/persone/persona")

                'l'invio viene tentato SOLO se il flag corrispondente è a ZERO!
                If ParseXmlBoolean01(pNode, "fl_inviomailattpartok") = False Then

                    id_ISCRITTO = ParseXmlInteger(pNode, "id_iscritto")
                    tx_EMAIL = ParseXmlString(pNode, "tx_email")
                    tx_VOCATIVO = ParseXmlString(pNode, "tx_vocativo")
                    tx_COGNOME = ParseXmlString(pNode, "tx_cognome")
                    tx_NOME = ParseXmlString(pNode, "tx_nome")
                    tx_TITOLOEVENTO = ParseXmlString(pNode, "tx_titoloevento")
                    tx_SEDE = ParseXmlString(pNode, "tx_sede")
                    dt_INIZIO = ParseXmlString(pNode, "dt_inizio")
                    dt_FINE = ParseXmlString(pNode, "dt_fine")

                    sResult = SendingResult.unknown

                    subject = "Notifica disponibilità attestato di partecipazione per " & tx_NOME & " " & tx_COGNOME

                    If tx_EMAIL = "" Then
                        sResult = SendingResult.mailnotpresent
                        'come se non fosse riuscito...
                        dbCmd = dbConn.CreateCommand
                        With dbCmd
                            .CommandType = CommandType.Text
                            .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILATTPART=@dt_INVIOMAILATTPART, fl_INVIOMAILATTPARTOK=0, tx_INVIOMAILATTPART=@tx_INVIOMAILATTPART WHERE id_ISCRITTO=@id_ISCRITTO"
                            .Parameters.Add("@dt_INVIOMAILATTPART", SqlDbType.DateTime).Value = Date.Now
                            .Parameters.Add("@tx_INVIOMAILATTPART", SqlDbType.NVarChar, 150).Value = DBNull.Value
                            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                        End With
                        dbCmd.ExecuteNonQuery()
                        dbCmd.Dispose()
                    Else
                        'OK c'è una mail

                        'generazione del corpo
                        Dim sBody = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/AttestatoPartecipazioneDisponibile.xslt", _
                                                        "tx_VOCATIVO", tx_VOCATIVO, _
                                                        "tx_COGNOME", tx_COGNOME, _
                                                        "tx_NOME", tx_NOME, _
                                                        "tx_TITOLOEVENTO", tx_TITOLOEVENTO, _
                                                        "tx_SEDE", tx_SEDE, _
                                                        "dt_INIZIO", dt_INIZIO, _
                                                        "dt_FINE", dt_FINE, _
                                                        "baseurl", tx_BASEURL)

                        If GFMailHandler.SendMail(dbConn, GecFinalContextHandler.id_EVENTO, tx_BASEURL, tx_EMAIL, subject, sBody) Then
                            sResult = SendingResult.sent
                        Else
                            sResult = SendingResult.errorsending
                        End If

                        'scriviamo i dettagli dell'invio
                        Select Case sResult
                            Case SendingResult.sent
                                dbCmd = dbConn.CreateCommand
                                With dbCmd
                                    .CommandType = CommandType.Text
                                    .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILATTPART=@dt_INVIOMAILATTPART, fl_INVIOMAILATTPARTOK=1, tx_INVIOMAILATTPART=@tx_INVIOMAILATTPART WHERE id_ISCRITTO=@id_ISCRITTO"
                                    .Parameters.Add("@dt_INVIOMAILATTPART", SqlDbType.DateTime).Value = Date.Now
                                    .Parameters.Add("@tx_INVIOMAILATTPART", SqlDbType.NVarChar, 150).Value = tx_EMAIL
                                    .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                                End With
                                dbCmd.ExecuteNonQuery()
                                dbCmd.Dispose()
                            Case SendingResult.errorsending
                                dbCmd = dbConn.CreateCommand
                                With dbCmd
                                    .CommandType = CommandType.Text
                                    .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILATTPART=@dt_INVIOMAILATTPART, fl_INVIOMAILATTPARTOK=0, tx_INVIOMAILATTPART=@tx_INVIOMAILATTPART WHERE id_ISCRITTO=@id_ISCRITTO"
                                    .Parameters.Add("@dt_INVIOMAILATTPART", SqlDbType.DateTime).Value = Date.Now
                                    .Parameters.Add("@tx_INVIOMAILATTPART", SqlDbType.NVarChar, 150).Value = tx_EMAIL
                                    .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                                End With
                                dbCmd.ExecuteNonQuery()
                                dbCmd.Dispose()
                        End Select


                        'attesa
                        Threading.Thread.Sleep(100)

                    End If
                End If

            Next

        End If

    End Sub

    Private Sub MarkEventAsSent()
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_EVENTI SET dt_INVIOMAILATTPART=@dt_INVIOMAILATTPART WHERE id_EVENTO=@id_EVENTO"
            .Parameters.Add("@dt_INVIOMAILATTPART", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
    End Sub

    Private Sub btnInvioUlteriore_Click(sender As Object, e As System.EventArgs) Handles btnInvioUlteriore.Click

        'spedizione ulteriore
        'invio vero e proprio
        SendMails()

        'marco come spedito
        MarkEventAsSent()

        'refresh dati
        SetupPage()

        'messaggio
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "doneall", "window.alert('La spedizione delle mail è stata correttamente effettuata. Controlla i risultati a video.');", True)

    End Sub

End Class