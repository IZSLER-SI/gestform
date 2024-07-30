Imports System.Configuration.ConfigurationManager
Imports Softailor.Global.XmlParser

Public Class MailAttestatiOnLine
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

    Private Sub MailAttestatiOnLineGF_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub SetupPage()

        Dim fl_ATTECMONLINE As Boolean
        Dim dt_INVIOMAILATTECM As SqlDateTime

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim i As Integer

        'pulizie
        lblNumPartecipanti.Text = ""
        lblNumRelatori.Text = ""
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

        'lettura numero relatori
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOECM='COK' AND ac_CATEGORIAECM<>'P'"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumRelatori.Text = "Nessuno"
            lblNumRelatori.ForeColor = Drawing.Color.DarkRed
        Else
            lblNumRelatori.Text = i.ToString
            lblNumRelatori.ForeColor = Drawing.Color.DarkGreen
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura numero partecipanti
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOECM='COK' AND ac_CATEGORIAECM='P'"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumPartecipanti.Text = "Nessuno"
            lblNumPartecipanti.ForeColor = Drawing.Color.DarkRed
        Else
            lblNumPartecipanti.Text = i.ToString
            lblNumPartecipanti.ForeColor = Drawing.Color.DarkGreen
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura stato invio e stato attivazione
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT fl_ATTECMONLINE, dt_INVIOMAILATTECM FROM eve_EVENTI WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        fl_ATTECMONLINE = dbRdr.GetBoolean(0)
        dt_INVIOMAILATTECM = dbRdr.GetSqlDateTime(1)
        dbRdr.Close()
        dbCmd.Dispose()

        '3 possibilità
        If Not dt_INVIOMAILATTECM.IsNull Then
            'mail già inviate
            lblStatoInvio.Text = "Ultimo invio mail di notifica disponibilità attestati ECM effettuato in data <b>" & dt_INVIOMAILATTECM.Value.ToString("dd/MM/yyyy HH:mm") & "</b>."
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
            If fl_ATTECMONLINE Then
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
                lblRisultatoInvio.Text = "Impossibile inviare le e-mail: lo scaricamento dell'attestato ECM on line non è stato attivato."
                lblRisultatoInvio.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Private Sub GeneraRiepiloghiToDo()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ecm_StatoInvioMailAttestatiECM"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Transformer.Transform(dbCmd, "Templates/StatoInvioMailAttestatiECM.xslt", phdRiepiloghiToDo, "mode", "todo")

    End Sub

    Private Sub GeneraRiepiloghiDone()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ecm_StatoInvioMailAttestatiECM"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Dim sAspx = Transformer.Transform(dbCmd, "Templates/StatoInvioMailAttestatiECM.xslt", "mode", "done")
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
            .CommandText = "sp_ecm_StatoInvioMailAttestatiECM"
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
                If ParseXmlBoolean01(pNode, "fl_inviomailattecmok") = False Then

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

                    subject = "Notifica disponibilità attestato ECM per " & tx_NOME & " " & tx_COGNOME

                    If tx_EMAIL = "" Then
                        sResult = SendingResult.mailnotpresent
                        'come se non fosse riuscito...
                        dbCmd = dbConn.CreateCommand
                        With dbCmd
                            .CommandType = CommandType.Text
                            .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILATTECM=@dt_INVIOMAILATTECM, fl_INVIOMAILATTECMOK=0, tx_INVIOMAILATTECM=@tx_INVIOMAILATTECM WHERE id_ISCRITTO=@id_ISCRITTO"
                            .Parameters.Add("@dt_INVIOMAILATTECM", SqlDbType.DateTime).Value = Date.Now
                            .Parameters.Add("@tx_INVIOMAILATTECM", SqlDbType.NVarChar, 150).Value = DBNull.Value
                            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                        End With
                        dbCmd.ExecuteNonQuery()
                        dbCmd.Dispose()
                    Else
                        'OK c'è una mail

                        'generazione del corpo
                        Dim sBody = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/AttestatoDisponibile.xslt", _
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
                                    .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILATTECM=@dt_INVIOMAILATTECM, fl_INVIOMAILATTECMOK=1, tx_INVIOMAILATTECM=@tx_INVIOMAILATTECM WHERE id_ISCRITTO=@id_ISCRITTO"
                                    .Parameters.Add("@dt_INVIOMAILATTECM", SqlDbType.DateTime).Value = Date.Now
                                    .Parameters.Add("@tx_INVIOMAILATTECM", SqlDbType.NVarChar, 150).Value = tx_EMAIL
                                    .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                                End With
                                dbCmd.ExecuteNonQuery()
                                dbCmd.Dispose()
                            Case SendingResult.errorsending
                                dbCmd = dbConn.CreateCommand
                                With dbCmd
                                    .CommandType = CommandType.Text
                                    .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILATTECM=@dt_INVIOMAILATTECM, fl_INVIOMAILATTECMOK=0, tx_INVIOMAILATTECM=@tx_INVIOMAILATTECM WHERE id_ISCRITTO=@id_ISCRITTO"
                                    .Parameters.Add("@dt_INVIOMAILATTECM", SqlDbType.DateTime).Value = Date.Now
                                    .Parameters.Add("@tx_INVIOMAILATTECM", SqlDbType.NVarChar, 150).Value = tx_EMAIL
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
            .CommandText = "UPDATE eve_EVENTI SET dt_INVIOMAILATTECM=@dt_INVIOMAILATTECM WHERE id_EVENTO=@id_EVENTO"
            .Parameters.Add("@dt_INVIOMAILATTECM", SqlDbType.DateTime).Value = Date.Now
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