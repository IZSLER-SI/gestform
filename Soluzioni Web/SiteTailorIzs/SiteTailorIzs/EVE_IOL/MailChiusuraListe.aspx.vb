Imports System.Configuration.ConfigurationManager
Imports Softailor.Global.XmlParser

Public Class MailChiusuraListe
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'timeout
        ScriptManager.GetCurrent(Me).AsyncPostBackTimeout = 6000

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        SetupPage()
    End Sub

    Private Sub MailChiusuraListe_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub SetupPage()

        Dim iol_dt_CHIUSURALISTE As SqlDateTime
        Dim fl_STATINONVALIDI As Boolean = False
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim i As Integer

        'pulizie
        lblNumIscrittiConfermaImmediata.Text = ""
        lblNumAccettatiChiusuraListe.Text = ""
        lblNumNonAccettati.Text = ""
        lblNumDRM.Text = ""

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

        'lettura numero iscritti con conferma immediata
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOISCRIZIONE IN ('I','P','AG','AI') AND ac_CATEGORIAECM='P' AND fl_ACCETTAZIONEINCHIUSURA=0"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumIscrittiConfermaImmediata.Text = "Nessuno"
        Else
            lblNumIscrittiConfermaImmediata.Text = i.ToString
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura numero accettati in fase di chiusura liste
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOISCRIZIONE IN ('I','P','AG','AI') AND ac_CATEGORIAECM='P' AND fl_ACCETTAZIONEINCHIUSURA=1"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumAccettatiChiusuraListe.Text = "Nessuno"
        Else
            lblNumAccettatiChiusuraListe.Text = i.ToString
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura numero non accettati
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOISCRIZIONE='NA' AND ac_CATEGORIAECM='P'"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumNonAccettati.Text = "Nessuno"
        Else
            lblNumNonAccettati.Text = i.ToString
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura numero D/R/M
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT COUNT(id_ISCRITTO) as Quanti FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento AND ac_STATOISCRIZIONE IN ('I','P','AG','AI') AND ac_CATEGORIAECM IN ('D','R','M')"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        i = dbRdr.GetInt32(0)
        If i = 0 Then
            lblNumDRM.Text = "Nessuno"
        Else
            lblNumDRM.Text = i.ToString
        End If
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura stato invio
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT CAST(iol_dt_CHIUSURALISTE as datetime) as iol_dt_CHIUSURALISTE FROM eve_EVENTI WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        iol_dt_CHIUSURALISTE = dbRdr.GetSqlDateTime(0)
        dbRdr.Close()
        dbCmd.Dispose()

        'verifichiamo se ci sono stati non validi
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT TOP 1 id_ISCRITTO FROM eve_ISCRITTI WHERE id_EVENTO=@id_evento and ac_CATEGORIAECM ='P' AND ac_STATOISCRIZIONE IN ('LAP','LAS')"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read() Then fl_STATINONVALIDI = True
        dbRdr.Close()
        dbCmd.Dispose()

        '3 possibilità
        If Not iol_dt_CHIUSURALISTE.IsNull Then
            'mail già inviate
            lblStatoInvio.Text = "Ultimo invio e-mail di chiusura liste effettuato in data <b>" & iol_dt_CHIUSURALISTE.Value.ToString("dd/MM/yyyy") & "</b>."
            lblStatoInvio.ForeColor = Drawing.Color.DarkGreen
            pnlSpacer1.Visible = False
            pnlSpacer2.Visible = False
            pnlSpacer3.Visible = False
            pnlSpacer4.Visible = False
            pnlPrimoInvio.Visible = False
            tblReinvio.Visible = True
            GeneraRiepiloghiDone()
        Else
            'mail da inviare
            tblReinvio.Visible = False
            lblStatoInvio.Text = "E-mail di chiusura liste d'attesa non ancora inviate."
            lblStatoInvio.ForeColor = Drawing.Color.Empty
            If fl_STATINONVALIDI = False Then
                'OK si può fare
                pnlWarningInvia.Visible = False
                pnlPrimoInvio.Visible = True
                lnkInvia.Visible = True
                lnkInvia.Enabled = True
                pnlWarning2invia.Visible = True
                GeneraRiepiloghiToDo()
            Else
                'modifica 3-6-2015: si può fare lo stesso
                pnlWarningInvia.Visible = False
                pnlPrimoInvio.Visible = True
                lnkInvia.Visible = True
                lnkInvia.Enabled = True
                pnlWarning2invia.Visible = True
                GeneraRiepiloghiToDo()

                lblRisultatoInvio.Text = "ATTENZIONE: Esistono iscritti in lista d'attesa. E' comunque possibile inviare le e-mail, ma tali iscritti non riceveranno nessuna comunicazione."
                lblRisultatoInvio.ForeColor = Drawing.Color.Red

                'pnlWarningInvia.Visible = False
                'pnlPrimoInvio.Visible = False
                'lnkInvia.Visible = False
                'lnkInvia.Enabled = False
                'pnlWarning2invia.Visible = False
                'pnlSpacer1.Visible = False
                'pnlSpacer2.Visible = False
                'pnlSpacer3.Visible = False
                'pnlSpacer4.Visible = True
                
            End If
        End If
    End Sub

    Private Sub GeneraRiepiloghiToDo()

        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_StatoInvioMailChiusuraListe"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        Transformer.Transform(dbCmd, "Templates/StatoInvioMailChiusuraListe.xslt", phdRiepiloghiToDo, "mode", "todo")

    End Sub

    Private Sub GeneraRiepiloghiDone()

        Dim dbCmd As SqlCommand
        Dim sAspx As String
        Dim cCreato As Control

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_StatoInvioMailChiusuraListe"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        sAspx = Transformer.Transform(dbCmd, "Templates/StatoInvioMailChiusuraListe.xslt", "mode", "done")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        cCreato = ParseControl(sAspx)
        phdRiepiloghiDone.Controls.Clear()
        phdRiepiloghiDone.Controls.Add(cCreato)

        For Each c In cCreato.Controls
            If TypeOf c Is LinkButton Then
                With CType(c, LinkButton)
                    If .ID Like "lnkReinviaMail_*" Then
                        AddHandler .Click, AddressOf lnkReinviaMail_Click
                    End If
                End With

            End If
        Next

    End Sub

    Private Sub lnkReinviaMail_Click(sender As Object, e As System.EventArgs)

        Dim id_ISCRITTO As Integer
        Dim dbCmd As SqlCommand

        id_ISCRITTO = CInt(CType(sender, LinkButton).ID.Split("_"c)(1))

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILCHIUSURALISTE=null, fl_INVIOMAILCHIUSURALISTEOK=0, tx_INVIOMAILCHIUSURALISTE=null, ac_INVIOMAILCHIUSURALISTE=null WHERE id_ISCRITTO=@id_ISCRITTO"
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        GeneraRiepiloghiDone()

    End Sub

    Private Sub lnkInvia_Click(sender As Object, e As System.EventArgs) Handles lnkInvia.Click

        'validazione
        If Not (chkInviaA.Checked Or chkInviaB.Checked Or chkInviaC.Checked Or chkInviaD.Checked) Then
            'messaggio
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "atleastoneinvia", "window.alert('Seleziona almeno una tipologia di e-mail.');", True)
            Exit Sub
        End If


        'invio vero e proprio
        SendMailsNotAlreadySent(chkInviaA.Checked,
                                chkInviaB.Checked,
                                chkInviaC.Checked,
                                chkInviaD.Checked)

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

    Private Sub SendMailsNotAlreadySent(sendA As Boolean, sendB As Boolean, sendC As Boolean, sendD As Boolean)

        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim myXDoc As XmlDocument

        'dati globali
        Dim tx_BASEURL As String
        Dim subject As String
        Dim type As String

        'dati per generazione mail
        Dim id_ISCRITTO As Integer
        Dim tx_EMAIL As String
        Dim ac_STATOISCRIZIONE As String
        Dim ac_CATEGORIAECM As String
        Dim fl_ACCETTAZIONEINCHIUSURA As Boolean
        Dim fl_INVIOMAILCHIUSURALISTEOK As Boolean

        'Dim smtpServer As String
        Dim sResult As SendingResult

        'lettura dati globali
        tx_BASEURL = AppSettings("GF_FrontofficeBasePath_FromWeb")
        'smtpServer = ContextHandler.SmtpServer

        'lettura doc XML
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_StatoInvioMailChiusuraListe"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        xReader = dbCmd.ExecuteXmlReader
        myXDoc = New XmlDocument
        myXDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        'ciclo sui nodi
        If myXDoc.SelectNodes("/eventi").Count > 0 Then

            For Each pNode As XmlNode In myXDoc.SelectNodes("/eventi/evento")

                fl_INVIOMAILCHIUSURALISTEOK = ParseXmlBoolean01(pNode, "fl_inviomailchiusuralisteok")
                id_ISCRITTO = ParseXmlInteger(pNode, "id_iscritto")
                tx_EMAIL = ParseXmlString(pNode, "tx_email")
                ac_CATEGORIAECM = ParseXmlString(pNode, "ac_categoriaecm")
                ac_STATOISCRIZIONE = ParseXmlString(pNode, "ac_statoiscrizione")
                fl_ACCETTAZIONEINCHIUSURA = ParseXmlBoolean01(pNode, "fl_accettazioneinchiusura")
                type = ParseXmlString(pNode, "ac_inviomailchiusuraliste")

                'mando la mail SOLO se non è già stata mandata e se il tipo coincide
                If fl_INVIOMAILCHIUSURALISTEOK = False Then

                    If (type = "A" And sendA = True) Or
                       (type = "B" And sendB = True) Or
                       (type = "C" And sendC = True) Or
                       (type = "D" And sendD = True) Then


                        sResult = SendingResult.unknown

                        'determinazione subject
                        subject = MailSubjectHelperGF.GetSubject(ac_CATEGORIAECM, ac_STATOISCRIZIONE, fl_ACCETTAZIONEINCHIUSURA)

                        If tx_EMAIL = "" Then
                            sResult = SendingResult.mailnotpresent
                        Else
                            'OK c'è una mail

                            'generazione del documento
                            Dim personaXDoc As New XmlDocument
                            Dim eventiNode = personaXDoc.CreateElement("eventi")
                            Dim eventoNode = personaXDoc.ImportNode(pNode, True)
                            eventiNode.AppendChild(eventoNode)
                            personaXDoc.AppendChild(eventiNode)

                            'generazione del corpo
                            Dim basepath = MapPath("~/GFTemplates/Mail/")
                            Dim sbody = MailSubjectHelperGF.GetBody(personaXDoc, basepath, ac_CATEGORIAECM, ac_STATOISCRIZIONE, fl_ACCETTAZIONEINCHIUSURA, tx_BASEURL)

                            If GFMailHandler.SendMail(dbConn, GecFinalContextHandler.id_EVENTO, tx_BASEURL, tx_EMAIL, subject, sbody) Then
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
                                        .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILCHIUSURALISTE=@dt_INVIOMAILCHIUSURALISTE, fl_INVIOMAILCHIUSURALISTEOK=1, tx_INVIOMAILCHIUSURALISTE=@tx_INVIOMAILCHIUSURALISTE, ac_INVIOMAILCHIUSURALISTE=@ac_INVIOMAILCHIUSURALISTE WHERE id_ISCRITTO=@id_ISCRITTO"
                                        .Parameters.Add("@dt_INVIOMAILCHIUSURALISTE", SqlDbType.DateTime).Value = Date.Now
                                        .Parameters.Add("@tx_INVIOMAILCHIUSURALISTE", SqlDbType.NVarChar, 150).Value = tx_EMAIL
                                        .Parameters.Add("@ac_INVIOMAILCHIUSURALISTE", SqlDbType.NVarChar, 1).Value = type
                                        .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                                    End With
                                    dbCmd.ExecuteNonQuery()
                                    dbCmd.Dispose()
                                Case SendingResult.errorsending
                                    dbCmd = dbConn.CreateCommand
                                    With dbCmd
                                        .CommandType = CommandType.Text
                                        .CommandText = "UPDATE eve_ISCRITTI SET dt_INVIOMAILCHIUSURALISTE=@dt_INVIOMAILCHIUSURALISTE, fl_INVIOMAILCHIUSURALISTEOK=0, tx_INVIOMAILCHIUSURALISTE=@tx_INVIOMAILCHIUSURALISTE, ac_INVIOMAILCHIUSURALISTE=@ac_INVIOMAILCHIUSURALISTE WHERE id_ISCRITTO=@id_ISCRITTO"
                                        .Parameters.Add("@dt_INVIOMAILCHIUSURALISTE", SqlDbType.DateTime).Value = Date.Now
                                        .Parameters.Add("@tx_INVIOMAILCHIUSURALISTE", SqlDbType.NVarChar, 150).Value = tx_EMAIL
                                        .Parameters.Add("@ac_INVIOMAILCHIUSURALISTE", SqlDbType.NVarChar, 1).Value = type
                                        .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                                    End With
                                    dbCmd.ExecuteNonQuery()
                                    dbCmd.Dispose()
                            End Select

                            'attesa
                            Threading.Thread.Sleep(100)
                        End If

                    End If
                End If

            Next

        End If

    End Sub

    Private Sub MarkEventAsSent()
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_EVENTI SET iol_dt_CHIUSURALISTE=@iol_dt_CHIUSURALISTE WHERE id_EVENTO=@id_EVENTO"
            .Parameters.Add("@iol_dt_CHIUSURALISTE", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()
    End Sub

    Private Sub lnkReinvia_Click(sender As Object, e As EventArgs) Handles lnkReinvia.Click

        'validazione
        If Not (chkReInviaA.Checked Or chkReInviaB.Checked Or chkReInviaC.Checked Or chkReInviaD.Checked) Then
            'messaggio
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "atleastoneinvia", "window.alert('Seleziona almeno una tipologia di e-mail.');", True)
            Exit Sub
        End If

        'invio vero e proprio
        SendMailsNotAlreadySent(chkReInviaA.Checked,
                                chkReInviaB.Checked,
                                chkReInviaC.Checked,
                                chkReInviaD.Checked)

        'marco di nuovo come spedito (il testo è "ultima spedizione")
        MarkEventAsSent()

        'refresh dati
        SetupPage()

        'messaggio
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "doneall", "window.alert('La spedizione delle mail è stata correttamente effettuata. Controlla i risultati a video.');", True)

    End Sub
End Class