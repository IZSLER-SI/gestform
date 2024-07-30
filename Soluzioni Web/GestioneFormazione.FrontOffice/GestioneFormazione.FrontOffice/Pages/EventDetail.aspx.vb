Imports System.Net
Imports Softailor.Global.XmlParser

Public Class EventDetail
    Inherits System.Web.UI.Page
    Implements IFOPage

    Dim fpd As FOPageData
    Dim id_EVENTO As Integer
    Dim tx_TITOLO As String
    Dim eventoXDoc As XmlDocument

    Protected WithEvents lnkIscrizione As LinkButton
    Protected WithEvents lnkDisiscrizione As LinkButton

    Private Sub EventDetail_Init(sender As Object, e As EventArgs) Handles Me.Init

        'verifica ID evento
        If Not RouteData.Values.ContainsKey("id") Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        Try
            id_EVENTO = CInt(RouteData.Values("id"))
        Catch ex As Exception
            id_EVENTO = 0
        End Try

        If id_EVENTO <= 0 Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        'genero la scheda evento. La routine di generazione effettua un redirect alla home
        'se necessario.
        GeneraSchedaEvento()

        'genero i dati dell'iscrizione
        GeneraBoxIscrizione()

        'genero il popup
        GeneraPopupIscrizione()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle

        Return tx_TITOLO

    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "eventi"
    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData
        Me.fpd = fpd
    End Sub

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange

        'rigenero la scheda evento. 
        'se l'utente non è più autorizzato, viene fatto un redirect.
        GeneraSchedaEvento()

        'rigenero anche il box iscrizione
        GeneraBoxIscrizione()

        '... e anche il popup
        GeneraPopupIscrizione()

    End Sub

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess

        'restituisco sempre TRUE. Eventuali redirect vengono gestiti dalle routine di generazione scheda.
        Return True

    End Function

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage
        Return False
    End Function

    Private Sub GeneraSchedaEvento()

        'lettura documento XML
        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_EventoVisibileOfferta"
            .Parameters.Add("@dt_DATAOGGI", SqlDbType.DateTime).Value = Date.Today
            With .Parameters.Add("@id_PERSONA", SqlDbType.Int)
                If ContextHandler.Region = ContextHandler.Regions.LoggedIn Then
                    .Value = ContextHandler.id_PERSONA
                Else
                    .Value = DBNull.Value
                End If
            End With
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = id_EVENTO
        End With
        Dim xReader = dbCmd.ExecuteXmlReader
        eventoXDoc = New XmlDocument
        eventoXDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        'abbiamo un evento?
        If eventoXDoc.SelectNodes("/evento").Count = 0 Then
            Response.Redirect("/", True)
            Exit Sub
        End If

        'titolo
        tx_TITOLO = eventoXDoc.SelectSingleNode("/evento").Attributes("tx_titolo").Value

        'generazione contenuto
        phdEvento.Controls.Clear()
        Transformer.Transform(eventoXDoc, fpd.baseTemplates & "EventDetail.xslt",
                              phdEvento,
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short)

        updEvento.Update()

    End Sub

    Private Sub GeneraBoxIscrizione()

        phdIscrizione.Controls.Clear()
        Transformer.Transform(eventoXDoc, fpd.baseTemplates & "EventRegistration.xslt",
                              phdIscrizione,
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short, "flspid", CType(IIf(ContextHandler.fl_SPID = True, "1", "0"), String))

        updIscrizioneEvento.Update()

    End Sub

    Private Sub GeneraPopupIscrizione()


        Dim sAspx = Transformer.Transform(eventoXDoc, fpd.baseTemplates & "EventRegistrationPopup.xslt",
                              "region", ContextHandler.RegionString,
                              "companyname", My.Settings.CompanyName_Short)

        Softailor.Global.AspxCleaner.CleanAspx(sAspx)

        Dim cCreato = Me.ParseControl(sAspx)
        phdPopupIscrizione.Controls.Clear()
        phdPopupIscrizione.Controls.Add(cCreato)
        lnkDisiscrizione = CType(cCreato.FindControl("lnkDisiscrizione"), LinkButton)
        lnkIscrizione = CType(cCreato.FindControl("lnkIscrizione"), LinkButton)
        updPopupIscrizioneEvento.Update()

    End Sub

    Private Sub GeneraPopupIscrizioneDone()

        phdPopupIscrizione.Controls.Clear()

        Transformer.Transform(eventoXDoc, fpd.baseTemplates & "EventRegistrationPopupDone.xslt",
                      phdPopupIscrizione,
                      "region", ContextHandler.RegionString,
                      "companyname", My.Settings.CompanyName_Short)

        updPopupIscrizioneEvento.Update()

    End Sub

    Private Sub lnkIscrizione_Click(sender As Object, e As EventArgs) Handles lnkIscrizione.Click

        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then Exit Sub


        Dim dbCmd As SqlCommand
        Dim prm_id_ISCRITTO As SqlParameter
        Dim done As Boolean

        'esecuzione
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_eve_IscriviPartecipante"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_EVENTO
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            prm_id_ISCRITTO = .Parameters.Add("@id_iscritto", SqlDbType.Int)
            prm_id_ISCRITTO.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        done = Not IsDBNull(prm_id_ISCRITTO.Value)
        dbCmd.Dispose()

        If done Then
            'rigenero tutto
            GeneraSchedaEvento()
            GeneraBoxIscrizione()
            GeneraPopupIscrizioneDone()

            'invio la mail
            SendRegistrationConfirmationMail()

        End If

    End Sub

    Private Sub lnkDisiscrizione_Click(sender As Object, e As EventArgs) Handles lnkDisiscrizione.Click

        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then Exit Sub

        Dim dbCmd As SqlCommand
        Dim prm_result As SqlParameter
        Dim prm_id_persona_scalata As SqlParameter
        Dim done As Boolean
        Dim id_persona_scalata As Integer = 0

        'esecuzione
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_eve_DisiscriviPartecipante"
            .Parameters.Add("@id_persona_in", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_evento_in", SqlDbType.Int).Value = id_EVENTO

            prm_result = .Parameters.Add("@result", SqlDbType.Int)
            prm_result.Direction = ParameterDirection.Output

            prm_id_persona_scalata = .Parameters.Add("@id_persona_scalata", SqlDbType.Int)
            prm_id_persona_scalata.Direction = ParameterDirection.Output

        End With
        dbCmd.ExecuteNonQuery()
        done = CInt(prm_result.Value) > 0
        If CInt(prm_result.Value) = 2 Then id_persona_scalata = CInt(prm_id_persona_scalata.Value)
        dbCmd.Dispose()

        If done Then
            'rigenero tutto
            GeneraSchedaEvento()
            GeneraBoxIscrizione()
            GeneraPopupIscrizioneDone()

            If id_persona_scalata > 0 Then
                'invio la mail all'altra persona
                SendAcceptanceConfirmationMail(id_persona_scalata)
            End If

        End If

    End Sub

    Private Sub SendRegistrationConfirmationMail()

        Dim iscrittoNode As XmlNode
        Dim tx_EMAIL As String

        iscrittoNode = eventoXDoc.SelectSingleNode("/evento/iscritto")
        tx_EMAIL = ParseXmlString(iscrittoNode, "tx_email")

        If tx_EMAIL <> String.Empty Then
            If Softailor.Global.ValidationUtils.ValidateEmail(tx_EMAIL) Then
                'ok, ci siamo

                Dim mailSubject As String = ""

                'determinazione del subject
                Dim ac_STATOISCRIZIONE = ParseXmlString(iscrittoNode, "ac_statoiscrizione")
                Select Case ac_STATOISCRIZIONE
                    Case "I"
                        mailSubject = "Conferma iscrizione ad evento formativo"
                    Case "LAP", "LAS"
                        mailSubject = "Conferma iscrizione in lista d'attesa ad evento formativo"
                End Select

                Dim mailBody = Transformer.Transform(eventoXDoc,
                                      "~/Templates/" & My.Settings.CompanyKey & "/Mail/RegistrationConfirmation.xslt",
                                      "baseurl", My.Settings.FrontOfficeUrl)

                'spedizione e-mail
                GFMailHandler.SendMail(tx_EMAIL, mailSubject, mailBody)

            End If
        End If


    End Sub

    Private Sub SendAcceptanceConfirmationMail(id_persona_scalata As Integer)

        Dim otherXDoc As XmlDocument
        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_EventoVisibileOfferta"
            .Parameters.Add("@dt_DATAOGGI", SqlDbType.DateTime).Value = Date.Today
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_persona_scalata
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = id_EVENTO
        End With
        xReader = dbCmd.ExecuteXmlReader
        otherXDoc = New XmlDocument
        otherXDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        Dim iscrittoNode As XmlNode
        Dim tx_EMAIL As String

        iscrittoNode = otherXDoc.SelectSingleNode("/evento/iscritto")
        tx_EMAIL = ParseXmlString(iscrittoNode, "tx_email")

        If tx_EMAIL <> String.Empty Then
            If Softailor.Global.ValidationUtils.ValidateEmail(tx_EMAIL) Then
                'ok, ci siamo

                Dim mailSubject As String = "Accettazione iscrizione ad evento formativo"

                Dim mailBody = Transformer.Transform(otherXDoc,
                                      "~/Templates/" & My.Settings.CompanyKey & "/Mail/AcceptanceConfirmation.xslt",
                                      "baseurl", My.Settings.FrontOfficeUrl)

                'spedizione e-mail
                GFMailHandler.SendMail(tx_EMAIL, mailSubject, mailBody)

            End If
        End If


    End Sub

End Class