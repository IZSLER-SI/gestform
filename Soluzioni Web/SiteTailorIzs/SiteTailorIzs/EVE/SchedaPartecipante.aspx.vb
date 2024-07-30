'Imports Novacode
Imports Softailor.Web.UI.DbForms
Imports Softailor.Global.SqlUtils
Imports OfficeOpenXml
Imports Softailor.ReportEngine
Imports Microsoft.SharePoint.Client
Imports System.Configuration.ConfigurationManager
Imports Newtonsoft.Json.Linq
Imports System.Net.Mail


Public Class SchedaPartecipante
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Public isDipendente As Boolean = True
    Dim id_ISCRITTO As Integer = 0
    Dim id_PERSONA As Integer = 0
    Dim ac_CATEGORIAECM As String = ""

    Dim dbConn As SqlConnection

    Private Sub SchedaPartecipante_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            id_ISCRITTO = CInt(Request("id"))
        Catch ex As Exception
        End Try

        If id_ISCRITTO = 0 Then
            CloseMe("")
            Exit Sub
        End If

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'determino categoria ECM
        DeterminaCategoriaECM()

        'determino id_persona
        DeterminaIdPersona()

        If id_PERSONA = 0 Then
            CloseMe("")
            Exit Sub
        End If

        'titoli, se non siamo in postback
        If Not Page.IsPostBack Then
            Dim dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_eve_DatiEvento"
                .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
                .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            End With
            Dim dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()
            With dbRdr
                lblTitolo1.Text = .GetString(5)
                lblTitolo2.Text = .GetString(6) & ", " & Softailor.Global.DateUtils.DataDalAl(.GetDateTime(3), .GetDateTime(4))
            End With
            dbRdr.Close()
            dbCmd.Dispose()
        End If
    End Sub

    Private Sub CloseMe(value As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMe",
           "parent.stl_sel_done(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(value) & ");", True)
    End Sub

    Private Sub DeterminaCategoriaECM()
        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT ac_CATEGORIAECM FROM eve_ISCRITTI WHERE id_ISCRITTO=@id_ISCRITTO"
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
        End With
        ac_CATEGORIAECM = CStr(dbCmd.ExecuteScalar)
        dbCmd.Dispose()

    End Sub

    Private Sub DeterminaIdPersona()
        Dim dbCmd As SqlCommand

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT id_PERSONA FROM eve_ISCRITTI WHERE id_ISCRITTO=@id_ISCRITTO"
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
        End With
        id_PERSONA = CInt(dbCmd.ExecuteScalar)
        dbCmd.Dispose()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'valori di default
        sdsISCRITTI.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsISCRITTI.SelectParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString
        sdsISCRITTI.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME
        sdsISCRITTI.UpdateParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

        sdsQUOTE_g.SelectParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString
        sdsQUOTE_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsQUOTE_f.InsertParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString
        sdsQUOTE_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME

        sdsRecapiti_g.SelectParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRECAPITI_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsRECAPITI_f.InsertParameters("id_PERSONA").DefaultValue = id_PERSONA.ToString
        sdsRECAPITI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME

        sdsACCESSIISCRITTI_g.SelectParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString
        sdsACCESSIISCRITTI_f.InsertParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString

        sdsCOSTIRICAVI_ISCRITTI_g.SelectParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString
        sdsCOSTIRICAVI_ISCRITTI_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsCOSTIRICAVI_ISCRITTI_f.InsertParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString
        sdsCOSTIRICAVI_ISCRITTI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME

        sdsStorico.SelectParameters("id_ISCRITTO").DefaultValue = id_ISCRITTO.ToString

        sdsid_QUOTAISCRIZIONE.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

        'verifica is dipendente
        Dim dbCmd = dbConn.CreateCommand
        Dim prmRet As SqlParameter
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_IscrittoDipendente"
            .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_ISCRITTO
            prmRet = .Parameters.Add("@retValue", SqlDbType.Bit)
            prmRet.Direction = ParameterDirection.Output
        End With
        dbCmd.ExecuteNonQuery()
        isDipendente = CBool(prmRet.Value)
        dbCmd.Dispose()

        'eventuale selezione tab
        If Not Page.IsPostBack Then
            ' se è partecipante nascondi sezione Modulo Incarico
            If ac_CATEGORIAECM = "P" Then
                Me.sezioneIncarico.Visible = False
            End If
            If Request("goto") IsNot Nothing Then
                If Request("goto") = "part" Then
                    Me.tabContainer.ActiveTabIndex = 3
                End If

            End If
        End If



    End Sub

    Private Sub lnkClose_Click(sender As Object, e As EventArgs) Handles lnkClose.Click

        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Prima di chiudere devi salvare o annullare le modifiche effettuate.');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('REFRESH');", True)
        End If

    End Sub

    Private Sub SchedaPartecipante_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'dettaglio tempi
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_StatoPresenzaIscritto"
            .Parameters.Add("@id_iscritto", SqlDbType.Int).Value = id_ISCRITTO
        End With

        phdDettaglioTempi.Controls.Clear()
        Transformer.Transform(dbCmd, "Templates/SchedaPartecipante_StatoPresenza.xslt", phdDettaglioTempi)
        updDettaglioTempi.Update()
    End Sub

    Private Sub SchedaPartecipante_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub frmISCRITTI_ItemUpdated(sender As Object, e As System.Web.UI.WebControls.FormViewUpdatedEventArgs) Handles frmISCRITTI.ItemUpdated

        'aggiorno lo storico
        grdStorico.DataBind()
        updStorico.Update()

    End Sub

    Private Sub RemoveSection(ByRef document As Novacode.DocX, ByVal deleteCommand As String, ByVal deleteEndCommand As String)
        Try
            Dim deleteStart As Integer = 0
            Dim deleteEnd As Integer = 0



            Dim flag As Boolean = False
            Dim list1 As List(Of List(Of String)) = New List(Of List(Of String))()
            Dim list2 As List(Of String) = New List(Of String)()

            Dim list4 As List(Of Integer) = New List(Of Integer)()

            Dim p_i As Integer = 0
            For Each item As Novacode.Paragraph In document.Paragraphs
                'use this if you need whole text of a paragraph
                Dim paraText As String = item.Text
                Dim result = paraText.Split(" "c)
                Dim count As Integer = 0
                list2 = New List(Of String)()
                'use this if you need word by word

                For Each data As String In result
                    Dim word As String = data.ToString.ToLower
                    If word.Contains(deleteCommand) Then
                        flag = True
                        list4.Add(p_i)
                    End If
                    If word.Contains(deleteEndCommand) Then
                        flag = False
                        list2.Add(word)
                        list4.Add(p_i)
                    End If
                    If flag Then
                        If Not list4.Contains(p_i) Then
                            list4.Add(p_i)
                        End If

                        list2.Add(word)
                    End If
                    count = count + 1

                Next
                list1.Add(list2)
                p_i = p_i + 1
            Next
            For i As Integer = 0 To list1.Count() - 1
                Dim temp As String = ""

                For y As Integer = 0 To list1(i).Count() - 1

                    If y = 0 Then
                        temp = list1(i)(y)
                        Continue For
                    End If

                    temp += " " & list1(i)(y)
                Next

                If Not temp.Equals("") Then
                    'document.ReplaceText(temp, "")
                End If
            Next

            p_i = 0
            For Each paragraph In document.Paragraphs

                If list4.Contains(p_i) Then
                    paragraph.Remove(False)
                End If
                p_i = p_i + 1
            Next

        Catch ex As Exception

        End Try
    End Sub

    Private Sub RemoveTagSection(ByRef doc As Novacode.DocX, ByVal deleteCommand As String, ByVal deleteEndCommand As String)
        Try

            Dim deleteStart As Integer = 0
            Dim deleteEnd As Integer = 0

            doc.ReplaceText(deleteCommand.ToLower, "")
            doc.ReplaceText(deleteEndCommand.ToLower, "")
            doc.ReplaceText(deleteCommand.ToUpper, "")
            doc.ReplaceText(deleteEndCommand.ToUpper, "")

        Catch ex As Exception

        End Try
    End Sub

    Private Shared Function getLibreOfficePath() As String
        Select Case Environment.OSVersion.Platform
            Case PlatformID.Unix
                Return "/usr/bin/soffice"
            Case PlatformID.Win32NT
                Return My.Settings.SofficeExePath
            Case Else
                Throw New PlatformNotSupportedException("Your OS is not supported")
        End Select
        Return Nothing
    End Function



    ' necessaria per prendere il modello dal cloud
    Public Shared Function GetClientContext() As ClientContext

        'eseguo l'autenticazione se non ho il context nella request, altrimenti uso quello
        If System.Web.HttpContext.Current.Session("sharepoint_context") IsNot Nothing Then
            Return CType(System.Web.HttpContext.Current.Session("sharepoint_context"), ClientContext)
        Else
            Return LogonAndStore(System.Web.HttpContext.Current.Session)
        End If

    End Function

    Public Shared Function LogonAndStore(session As System.Web.SessionState.HttpSessionState) As ClientContext

        Dim context As ClientContext
        Dim username As String
        Dim password As String

        context = New ClientContext(AppSettings("onedrive_site_root"))
        username = AppSettings("onedrive_username")
        password = AppSettings("onedrive_password")
        Dim securePwd As New System.Security.SecureString()
        For Each c In password.ToCharArray
            securePwd.AppendChar(c)
        Next
        context.Credentials = New SharePointOnlineCredentials(username, securePwd)

        'aggancio root e carico le sottocartelle
        Dim web = context.Web
        context.Load(web)
        context.ExecuteQuery()

        If session IsNot Nothing Then
            session("sharepoint_context") = context
        End If

        Return context

    End Function


    Private Sub lnkAttestatoEcm_Click(sender As Object, e As System.EventArgs) Handles lnkAttestatoECM.Click
        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Prima di stampare l\'attestato devi salvare o annullare le modifiche effettuate.');", True)
        Else
            'OK ci siamo.... verifichiamo!
            Dim ac_STATOECM = ""
            Dim dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT ac_STATOECM FROM eve_ISCRITTI WHERE id_ISCRITTO=@id_ISCRITTO AND id_EVENTO=@id_EVENTO"
                .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            End With
            Dim dbRdr = dbCmd.ExecuteReader
            If dbRdr.Read Then
                ac_STATOECM = dbRdr.GetString(0)
            End If
            dbRdr.Close()
            dbCmd.Dispose()

            If ac_STATOECM = "C" Or ac_STATOECM = "COK" Then
                'generazione!!!
                Dim sSql = GetSql_AttestatiEcm()

                'generazione dataset
                Dim dst = AttestatiEcmHelper.getDatasetAttestatiEcm(dbConn, sSql)
                dbConn.Close()
                dbConn = Nothing

                'OK ci siamo
                CreatePdfAttestatiEcm(dst)
                Response.End()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "nonsipuo", "window.alert('Per poter stampare l\'attestato ECM il nominativo deve essere candidato al conseguimento dei crediti ECM o deve avere conseguito i crediti ECM.');", True)
            End If
        End If
    End Sub

    Private Sub CreatePdfAttestatiEcm(dst As dstAttestatiEcm)
        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("~/Reports/" & GecFinalContextHandler.NomeReportAttestatoECM), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        rpt.SetDataSource(dst)

        'esportazione report
        Dim rStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
        rpt.Close()
        rpt.Dispose()
        dst.Dispose()
        GC.Collect()

        Dim sReader As New IO.BinaryReader(rStream)
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;  filename=" & "AttestatiEcm_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss") & ".pdf")
        Response.BinaryWrite(sReader.ReadBytes(CInt(rStream.Length)))
        rStream.Dispose()
        Response.End()
    End Sub

    Private Sub CreatePdfAttestatiPart(dst As dstAttestatiEcm)
        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("~/Reports/rptAttestatoPartecipazione_" & My.Settings.CompanyName & ".rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        rpt.SetDataSource(dst)

        'esportazione report
        Dim rStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
        rpt.Close()
        rpt.Dispose()
        dst.Dispose()
        GC.Collect()

        Dim sReader As New IO.BinaryReader(rStream)
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;  filename=" & "AttestatiPartecipazione_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss") & ".pdf")
        Response.BinaryWrite(sReader.ReadBytes(CInt(rStream.Length)))
        rStream.Dispose()
        Response.End()
    End Sub

    Private Function GetSql_AttestatiEcm() As String

        Dim sOut = "SELECT * " &
           "FROM dbo.fn_eve_AttestatiECM (" & SQL_Int32(ContextHandler.ID_AZIEN) & ", " & SQL_Int32(GecFinalContextHandler.id_EVENTO) & ") " &
           "WHERE id_ISCRITTO = " & SQL_Int32(id_ISCRITTO)
        Return sOut

    End Function

    Private Sub lnkAttestatoPART_Click(sender As Object, e As EventArgs) Handles lnkAttestatoPART.Click
        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Prima di stampare l\'attestato devi salvare o annullare le modifiche effettuate.');", True)
        Else
            'OK ci siamo.... verifichiamo!
            Dim ac_STATOISCRIZIONE = ""
            Dim dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT ac_STATOISCRIZIONE FROM eve_ISCRITTI WHERE id_ISCRITTO=@id_ISCRITTO AND id_EVENTO=@id_EVENTO"
                .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
                .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            End With
            Dim dbRdr = dbCmd.ExecuteReader
            If dbRdr.Read Then
                ac_STATOISCRIZIONE = dbRdr.GetString(0)
            End If
            dbRdr.Close()
            dbCmd.Dispose()

            If ac_STATOISCRIZIONE = "P" Then
                'generazione!!!
                Dim sSql = GetSql_AttestatiEcm()

                'generazione dataset
                Dim dst = AttestatiPartHelper.getDatasetAttestatiPart(dbConn, sSql)
                dbConn.Close()
                dbConn = Nothing

                'OK ci siamo
                CreatePdfAttestatiPart(dst)
                Response.End()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "nonsipuo", "window.alert('Per poter stampare l\'attestato di partecipazione il nominativo deve essere presente.');", True)
            End If
        End If
    End Sub

    Private Sub sdsISCRITTI_updated(sender As Object, e As SqlDataSourceStatusEventArgs) Handles sdsISCRITTI.Updated
        Try
            Dim id_ISCRITTO_value As Integer = CInt(e.Command.Parameters("@id_ISCRITTO").Value)
            Dim id_EVENTO_value As Integer = CInt(e.Command.Parameters("@id_EVENTO").Value)
            Dim tx_NOME As String = e.Command.Parameters("@tx_NOME").Value.ToString
            Dim tx_COGNOME As String = e.Command.Parameters("@tx_COGNOME").Value.ToString
            Dim ac_CODICEFISCALE As String = e.Command.Parameters("@ac_CODICEFISCALE").Value.ToString
            Dim new_tx_EMAIL As String = e.Command.Parameters("@tx_EMAIL").Value.ToString
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            dbConn = DbConnectionHandler.GetOpenDataDbConn
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_log_getLastLogIscritto"
                .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
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

            Dim tx_EVENTO_TITOLO = String.Empty

            Dim txt As String = String.Empty
            Dim i As Integer = 0
            While dbRdr.Read
                data_operazione = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("dt_operazione")), dbRdr.GetDateTime(dbRdr.GetOrdinal("dt_operazione")), DateTime.Now)
                new_tx_MODIFICA = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("tx_OPERAZIONE")), dbRdr.GetString(dbRdr.GetOrdinal("tx_OPERAZIONE")), "n.d.")
                old_tx_PROFILO = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("old_tx_PROFILO")), dbRdr.GetString(dbRdr.GetOrdinal("old_tx_PROFILO")), "n.d.")
                new_tx_PROFILO = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_PROFILO")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_PROFILO")), "n.d.")
                old_tx_professione = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("old_tx_professione")), dbRdr.GetString(dbRdr.GetOrdinal("old_tx_professione")), "n.d.")
                new_tx_professione = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_professione")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_professione")), "n.d.")
                old_tx_disciplina = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("old_tx_disciplina")), dbRdr.GetString(dbRdr.GetOrdinal("old_tx_disciplina")), "n.d.")
                new_tx_disciplina = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("new_tx_disciplina")), dbRdr.GetString(dbRdr.GetOrdinal("new_tx_disciplina")), "n.d.")
                tx_EVENTO_TITOLO = If(Not dbRdr.IsDBNull(dbRdr.GetOrdinal("tx_TITOLO_EVENTO")), dbRdr.GetString(dbRdr.GetOrdinal("tx_TITOLO_EVENTO")), "n.d.")
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
                        sendEmail("FO", new_tx_EMAIL, txt, tx_NOME, tx_COGNOME, ac_CODICEFISCALE, data_operazione, new_tx_MODIFICA, tx_EVENTO_TITOLO)
                        ' Invio all'ufficio Formazione
                        sendEmail("BO", My.Settings.GenericMail_MailTo, txt, tx_NOME, tx_COGNOME, ac_CODICEFISCALE, data_operazione, new_tx_MODIFICA, tx_EVENTO_TITOLO)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Function sendEmail(tipo_destinatario As String, destinatario As String, tx_change As String, tx_NOME As String, tx_COGNOME As String, ac_CODICEFISCALE As String, data_operazione As DateTime, new_tx_MODIFICA As String, tx_EVENTO_TITOLO As String) As Boolean
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
                        "<tx_titolo_evento>" & tx_EVENTO_TITOLO & "</tx_titolo_evento>" &
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