Imports System.Configuration.ConfigurationManager
Imports Softailor.Global.XmlParser
Public Class PartecipantiEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Sub PartecipantiEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

        With Me.searchIscritti
            .id_EVENTO = GecFinalContextHandler.id_EVENTO
        End With

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'eventuale prima lettura della request
        If Not Page.IsPostBack Then
            searchIscritti.SetRoamingData(New GFRoamingData(Request))
            searchIscritti.DoSearch()
        End If

        'parametri std
        Me.sdsISCRITTI_g.DeleteParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsISCRITTI_g.DeleteParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

        'gestione degli script
        If Not Page.IsPostBack Then
            Dim sScript = "function editIscritto_callback(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkReposition, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            sScript &= "function addIscritto_callback(codice) { if(codice!='') {" & vbCrLf
            sScript &= "$get('" & txtAfterNew.ClientID & "').value=codice;" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkAfterNew, "").Replace("javascript:", "")
            sScript &= "}}" & vbCrLf
            Me.ltrRepositioning.Text = sScript
        End If

    End Sub

    Private Sub lnkReposition_Click(sender As Object, e As EventArgs) Handles lnkReposition.Click
        'deseleziono
        grdIscritti.SelectedIndex = -1
        'forzo ricerca
        grdIscritti.DataBind()
        grdIscritti.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdIscritti.DataKeys.Count - 1
            If grdIscritti.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdIscritti.SelectedIndex = sIdx
            grdIscritti.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)

        End If
    End Sub

    Private Sub lnkAfterNew_Click(sender As Object, e As EventArgs) Handles lnkAfterNew.Click

        'valori
        Dim newid_ISCRITTO As Integer = CInt(txtAfterNew.Text.Split("_"c)(0))
        Dim newOPERAZIONE As String = txtAfterNew.Text.Split("_"c)(1)
        Dim positioned = False


        'deseleziono
        grdIscritti.SelectedIndex = -1
        'forzo ricerca
        grdIscritti.DataBind()
        grdIscritti.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdIscritti.DataKeys.Count - 1
            If grdIscritti.DataKeys(cIdx).Value.ToString = newid_ISCRITTO.ToString Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdIscritti.SelectedIndex = sIdx
            grdIscritti.EnsureSelectedRowVisible()
            positioned = True
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappearednew", "window.alert('Il nominativo appena inserito non rispetta i criteri di filtro impostati, e non è pertanto visualizzato.');", True)
        End If

        'TODO eventuale apertura scheda
        If positioned And newOPERAZIONE = "EDIT" Then
            Dim sScript = "schedaPartecipante('" & newid_ISCRITTO.ToString & "');"

            'scrivo il valore
            txtReposition.Text = newid_ISCRITTO.ToString
            updHiddenCtls.Update()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)
        End If
    End Sub

    Private Sub grdIscritti_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdIscritti.SelectedIndexChanged

        If grdIscritti.SelectedIndex <> -1 Then

            Dim id_ISCRITTO As String = grdIscritti.SelectedDataKey.Value.ToString
            Dim sScript = "schedaPartecipante('" & id_ISCRITTO & "');"

            'scrivo il valore
            txtReposition.Text = id_ISCRITTO
            updHiddenCtls.Update()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)

        End If

    End Sub











    Public Function mailAttEcm_Click(sender As Object, e As EventArgs) As Boolean
        Dim gvRow As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        Dim index As Integer = gvRow.RowIndex
        'recupero l'id iscritto dal commandArgument del button
        Dim lbn As LinkButton = CType(sender, LinkButton)
        Dim id_ISCRITTO As String = lbn.CommandArgument

        SendMailsAttEcm(id_ISCRITTO)
        'refrescio la gridview
        grdIscritti.SelectedIndex = -1
        grdIscritti.DataBind()
        grdIscritti.UpdateParentPanel()
    End Function

    Public Function mailAttPart_Click(sender As Object, e As EventArgs) As Boolean
        Dim gvRow As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        Dim index As Integer = gvRow.RowIndex

        'recupero l'id iscritto dal commandArgument del button
        Dim lbn As LinkButton = CType(sender, LinkButton)
        Dim id_ISCRITTO As String = lbn.CommandArgument
        SendMailsAttPart(id_ISCRITTO)

        'refrescio la gridview
        grdIscritti.SelectedIndex = -1
        grdIscritti.DataBind()
        grdIscritti.UpdateParentPanel()

    End Function

    Private Enum SendingResult
        unknown
        sent
        mailnotpresent
        errorsending
    End Enum


    Private dbConn As SqlConnection

    Private Sub SendMailsAttEcm(id_ISCRITTO As String)
        dbConn = DbConnectionHandler.GetOpenDataDbConn
        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim myXDoc As XmlDocument
        Dim dbRdr As SqlDataReader

        'dati globali
        Dim tx_BASEURL As String
        Dim subject As String

        'dati per generazione mail
        Dim tx_EMAIL As String
        Dim tx_VOCATIVO As String
        Dim tx_COGNOME As String
        Dim tx_NOME As String
        Dim tx_TITOLOEVENTO As String
        Dim tx_SEDE As String
        Dim dt_INIZIO As String
        Dim dt_FINE As String
        Dim smtpServer As String
        Dim sResult As SendingResult

        'lettura dati globali
        'smtpServer = ContextHandler.SmtpServer

        tx_VOCATIVO = ""

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "select tx_email, tx_cognome, tx_nome from eve_ISCRITTI iscr join age_PERSONE pers on iscr.id_PERSONA=pers.id_PERSONA where id_iscritto=@id_ISCRITTO"
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        tx_EMAIL = dbRdr.GetString(0)
        tx_COGNOME = dbRdr.GetString(1)
        tx_NOME = dbRdr.GetString(2)

        dbRdr.Close()
        dbCmd.Dispose()

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "select tx_TITOLO, tx_SEDE, dt_INIZIO, dt_FINE from eve_EVENTI eve join age_SEDI sedi on sedi.id_sede=eve.id_sede where id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        tx_TITOLOEVENTO = dbRdr.GetString(0)
        tx_SEDE = dbRdr.GetString(1)
        dt_INIZIO = dbRdr.GetDateTime(2).ToString("yyyy-MM-dd")
        dt_FINE = dbRdr.GetDateTime(3).ToString("yyyy-MM-dd")
        dbRdr.Close()
        dbCmd.Dispose()

        'lettura dati globali
        tx_BASEURL = AppSettings("GF_FrontofficeBasePath_FromWeb")

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
            Dim sBody = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/AttestatoDisponibile.xslt",
                                            "tx_VOCATIVO", tx_VOCATIVO,
                                            "tx_COGNOME", tx_COGNOME,
                                            "tx_NOME", tx_NOME,
                                            "tx_TITOLOEVENTO", tx_TITOLOEVENTO,
                                            "tx_SEDE", tx_SEDE,
                                            "dt_INIZIO", dt_INIZIO,
                                            "dt_FINE", dt_FINE,
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

    End Sub

    Private Sub SendMailsAttPart(id_ISCRITTO As String)
        dbConn = DbConnectionHandler.GetOpenDataDbConn
        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim myXDoc As XmlDocument
        Dim dbRdr As SqlDataReader

        'dati globali
        Dim tx_BASEURL As String
        Dim subject As String

        'dati per generazione mail
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


        tx_VOCATIVO = ""

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "select tx_email, tx_cognome, tx_nome from eve_ISCRITTI iscr join age_PERSONE pers on iscr.id_PERSONA=pers.id_PERSONA where id_iscritto=@id_ISCRITTO"
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        tx_EMAIL = dbRdr.GetString(0)
        tx_COGNOME = dbRdr.GetString(1)
        tx_NOME = dbRdr.GetString(2)

        dbRdr.Close()
        dbCmd.Dispose()

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "select tx_TITOLO, tx_SEDE, dt_INIZIO, dt_FINE from eve_EVENTI eve join age_SEDI sedi on sedi.id_sede=eve.id_sede where id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        tx_TITOLOEVENTO = dbRdr.GetString(0)
        tx_SEDE = dbRdr.GetString(1)
        dt_INIZIO = dbRdr.GetDateTime(2).ToString("yyyy-MM-dd")
        dt_FINE = dbRdr.GetDateTime(3).ToString("yyyy-MM-dd")
        dbRdr.Close()
        dbCmd.Dispose()

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
            Dim sBody = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/AttestatoPartecipazioneDisponibile.xslt",
                                            "tx_VOCATIVO", tx_VOCATIVO,
                                            "tx_COGNOME", tx_COGNOME,
                                            "tx_NOME", tx_NOME,
                                            "tx_TITOLOEVENTO", tx_TITOLOEVENTO,
                                            "tx_SEDE", tx_SEDE,
                                            "dt_INIZIO", dt_INIZIO,
                                            "dt_FINE", dt_FINE,
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

    End Sub




End Class