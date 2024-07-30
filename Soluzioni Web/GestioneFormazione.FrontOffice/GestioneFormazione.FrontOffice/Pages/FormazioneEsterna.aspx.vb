Imports Softailor.Global.ValidationUtils
Imports Softailor.Global.XmlParser
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Microsoft.SharePoint.Client
Imports System.Configuration.ConfigurationManager
Imports System.Net.Mail



Public Class FormazioneEsterna
    Inherits System.Web.UI.Page
    Implements IFOPage

    Const errRequired = "Campo obbligatorio"
    Const errInvalidDate = "Data non valida. Utilizza gg/mm/aaaa"
    Const errInvalidDateRange = "Data non valida. Deve essere successiva o uguale alla data di oggi"
    Const errInvalidNumber = "Numero non valido"
    Const errNegativeNumber = "Devi inserire un numero maggiore di zero"
    Const errMoney = "L'importo non ha un formato corretto: Inserire solo numeri e la virgola come separatore dei centesimi, es. 301,17"

    Dim fpd As FOPageData
    Dim fl_PROFILOECM As Boolean = False
    Const EMPTY_ROW = "                                      "

#Region "Inizializzazione pagina"

    Public Function CheckAccess() As Boolean Implements IFOPage.CheckAccess
        Return ContextHandler.Region = ContextHandler.Regions.LoggedIn
    End Function

    Public Function GetMainMenuNodeKey() As String Implements IFOPage.GetMainMenuNodeKey
        Return "formazione-esterna"
    End Function

    Public Function GetSubTitle() As String Implements IFOPage.GetSubTitle
        Return "formazione-esterna"
    End Function

    Public Sub HandleRegionChange() Implements IFOPage.HandleRegionChange
        'vado alla home se sono uscito
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Response.Redirect("/", True)
        End If
    End Sub

    Public Function IsCompleteProfilePage() As Boolean Implements IFOPage.IsCompleteProfilePage

        Return False

    End Function

    Public Sub SetFOPageData(fpd As FOPageData) Implements IFOPage.SetFOPageData

        Me.fpd = fpd

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'determino se il profilo è ECM - verificare se necessario
        fl_PROFILOECM = ContextHandler.ProfiloECM(fpd.dbConn)
        'generazione lista
        If (Page.IsPostBack) Then
            'get a reference to ScriptManager And check if we have a partial postback
            If (ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack) Then
                RefreshListaPending()
                'updPending.Update()
                updForm.Update()

            Else
                'RefreshListaPending()
                GeneraListaEventi()
                'regular full page postback occured
                'custom logic accordingly                
            End If
        Else
            GeneraListaEventi()
            RefreshListaPending()
            updPending.Update()
        End If
    End Sub

#End Region

#Region "Ricerca eventi esistenti"

    Private Sub lnkCerca_Click(sender As Object, e As EventArgs) Handles lnkCerca.Click
        'esecuzione ricerca
        GeneraListaEventi()
    End Sub

    Private Sub lnkPulisci_Click(sender As Object, e As EventArgs) Handles lnkPulisci.Click

        'pulizia completa
        acTIPOLOGIAEVENTO.SelectedIndex = -1
        txtSede.Text = ""
        txtOrganizzatore.Text = ""
        txtTitolo.Text = ""

        GeneraListaEventi()

    End Sub

    Private Sub GeneraListaEventi()
        Dim xDoc As New XmlDocument
        If fl_filter.Value = "0" Then
            phdEventi.Controls.Clear()
            xDoc.LoadXml("<eventi/>")
            fl_filter.Value = "1"
        Else
            Dim dbCmd = fpd.dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ext_UltimiEventiCreatiBOFOXML"
                .Parameters.Add("@ac_TIPOLOGIAEVENTO", SqlDbType.VarChar).Value = acTIPOLOGIAEVENTO.SelectedValue.Replace("\0", "").Replace("\1", "")
                .Parameters.Add("@tx_sede", SqlDbType.VarChar).Value = txtSede.Text()
                .Parameters.Add("@tx_organizzatore", SqlDbType.VarChar).Value = txtOrganizzatore.Text()
                .Parameters.Add("@tx_titolo", SqlDbType.VarChar).Value = txtTitolo.Text()
            End With
            Dim xReader = dbCmd.ExecuteXmlReader

            xDoc.Load(xReader)
            xReader.Close()
            dbCmd.Dispose()
        End If
        phdEventi.Controls.Clear()
        Dim sAspx = Transformer.Transform(xDoc,
                                        "~/Templates/" & My.Settings.CompanyKey & "/Ext_EventList.xslt",
                                        "fl_profiloecm", If(fl_PROFILOECM, "1", "1"))
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        Dim cCreato = Me.ParseControl(sAspx)
        phdEventi.Controls.Add(cCreato)

        updEventi.Update()
    End Sub

#End Region

#Region "Richieste partecipazioni pendenti"

    Private Sub RefreshListaPending()
        Dim dbCmd As SqlCommand

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_PartecipazioniMFSPending"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@min_data", SqlDbType.Date).Value = Today.AddDays(-360)
        End With

        phdPending.Controls.Clear()
        Dim sAspx = Transformer.Transform(dbCmd,
                                        "~/Templates/" & My.Settings.CompanyKey & "/MFS_Eventi.xslt",
                                        "fl_profiloecm", If(fl_PROFILOECM, "1", "0"))
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        Dim cCreato = Me.ParseControl(sAspx)
        phdPending.Controls.Add(cCreato)

    End Sub

#End Region

#Region "Genera Documento"

    ' Handle del pulsante di generazione e invio modulo fuori sede
    Private Sub lnkGeneraDoc_Click(sender As Object, e As EventArgs) Handles lnkGeneraDoc.Click
        Dim id_PARTECIPAZIONE_print As String = id_PARTECIPAZIONE_stampa.Value
        Dim id_PARTECIPAZIONE_stampa_in As Integer = Integer.Parse(id_PARTECIPAZIONE_print)
        GeneraDocAndSend(id_PARTECIPAZIONE_stampa_in, True, False)
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

    'Genera documento e scarica/invia
    'Usa la Libre Office per la conversione da docx a pdf
    Private Sub GeneraDocAndSend(id_PARTECIPAZIONE_to_print As Integer, fl_download As Boolean, fl_mailsend As Boolean)
        Dim id_PARTECIPAZIONE As String = ""
        ' Prende i dati della partecipazione tramite id_PARTECIPAZIONE
        Dim dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_DatiPartecipazione_MFS"
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE_to_print
        End With
        ' Crea e inizializza una DataTable per ospitare i campi
        Dim dtPartecipazione As DataTable = Nothing
        dtPartecipazione = getDbDataInDataTable(dbCmd)
        If dtPartecipazione.IsInitialized Then
            For Each row As DataRow In dtPartecipazione.Rows
                id_PARTECIPAZIONE = row.Item("id_PARTECIPAZIONE").ToString
            Next
        End If
        dbCmd.Dispose()

        'OK ci siamo
        If (id_PARTECIPAZIONE <> "") Then
            Dim mStreamDest As IO.MemoryStream = CreateModuloFuoriSede(id_PARTECIPAZIONE, dtPartecipazione)
            If Not mStreamDest Is Nothing Then
                Try
                    ' Genera PDF da docx
                    Dim filename_in As String
                    Dim filename_out As String
                    Dim basePath As String

                    'impostazione dei path
                    basePath = My.Settings.BinariesTmpBasePath
                    filename_in = basePath + "\" + "modulo_formazione_esterna_" + id_PARTECIPAZIONE + ".docx"
                    filename_out = Replace(filename_in, ".docx", ".pdf")

                    'crea il docx
                    If Not IO.Directory.Exists(basePath) Then IO.Directory.CreateDirectory(basePath)
                    Dim fileStream = IO.File.Create(filename_in)
                    fileStream.Write(mStreamDest.ToArray, 0, CInt(mStreamDest.Length))
                    fileStream.Close()
                    fileStream.Dispose()

                    'converte il docx in pdf e lo salva in basepath
                    Dim libreOfficePath As String = getLibreOfficePath()

                    Dim procStartInfo As ProcessStartInfo = New ProcessStartInfo(libreOfficePath, "--convert-to pdf --headless " & filename_in & " --outdir " & basePath)

                    procStartInfo.RedirectStandardOutput = True
                    procStartInfo.UseShellExecute = False
                    procStartInfo.CreateNoWindow = True
                    procStartInfo.WorkingDirectory = Environment.CurrentDirectory
                    Dim process As Process = New Process() With {
                        .StartInfo = procStartInfo
                    }
                    process.Start()
                    process.WaitForExit()
                    If process Is Nothing And Not process.HasExited Then
                        process.Kill()
                    End If

                    'se il flag email è TRUE
                    If fl_mailsend Then
                        ' Invio email
                        Dim emailFrom As String = My.Settings.GenericMail_MailFrom
                        Dim emailTo As String = ContextHandler.tx_EMAIL

                        ' se il debug è attivo usa gli indirizzi email di test
                        If My.Settings.DEBUG_FLG = "1" Then
                            emailFrom = My.Settings.DEBUG_EMAIL_FROM
                            emailTo = My.Settings.DEBUG_EMAIL_TO
                        End If

                        Dim mailBody As String
                        mailBody = Transformer.Transform(New XmlDocument,
                                             "~/Templates/" & My.Settings.CompanyKey & "/Mail/NotificaModuloFormazioneEsterna.xslt",
                                             "baseurl", My.Settings.FrontOfficeUrl)

                        'spedizione mail
                        Dim smtp As New SmtpClient
                        Dim msg As New MailMessage
                        With msg
                            .From = New MailAddress(emailFrom)
                            .Subject = "Modulo richiesta formazione esterna" & " - " & Date.Today.ToString("d MMMM yyyy", Softailor.Global.Cultures.CulturaItalian)
                            .BodyEncoding = System.Text.Encoding.UTF8
                            .IsBodyHtml = True
                            .Body = mailBody
                            .To.Add(New MailAddress(emailTo))
                            Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(filename_out)
                            data.Name = IO.Path.GetFileName(filename_out)
                            .Attachments.Add(data)
                        End With
                        Try
                            smtp.Send(msg)
                            msg.Dispose()
                            smtp.Dispose()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "moduloIviato", "window.alert('Il modulo di richiesta è stato inviato all\'indirizzo " + emailTo + ".');", True)
                        Catch ex As SmtpException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "moduloIviato", "window.alert('Non è stato possibile inviare il modulo di richiesta all\'indirizzo " + emailTo + ".');", True)
                        End Try
                    End If

                    'se il flag download è TRUE
                    If fl_download Then
                        ' Download
                        Dim modello_pdf = New IO.MemoryStream(IO.File.ReadAllBytes(filename_out))
                        Response.Clear()
                        Response.ContentType = "application/pdf"
                        Response.AddHeader("content-disposition", "attachment;  filename=" & "RichiestaPartecipazione_" & Date.Now.ToString("dd_MM_yyyy HH_mm") & ".pdf")
                        Response.BinaryWrite(modello_pdf.ToArray())
                    End If

                    ' Dispose
                    mStreamDest.Dispose()
                    mStreamDest = Nothing
                    IO.File.Delete(filename_out)
                    IO.File.Delete(filename_in)


                Catch Exception As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "errlettinc", "window.alert('Errore nella generazione del documento.');", True)
                Finally
                    ' Nel caso di download viene chiusa la risposta
                    If fl_download Then
                        Response.End()
                    End If
                End Try
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "errlettinc", "window.alert('Errore nella generazione del documento.');", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "nonsipuolettinc", "window.alert('Per poter stampare la lettera di incarico il nominativo deve aver compilato il modulo di incarico.');", True)
        End If
    End Sub

    ' crea il modulo in PDF
    Private Function CreateModuloFuoriSede(id_PARTECIPAZIONE As String, dtPartecipazione As DataTable) As IO.MemoryStream
        Dim filepath As String
        Try
            Dim mStreamDest As IO.MemoryStream
            Dim docDest As Novacode.DocX
            Dim modello As IO.MemoryStream
            modello = New IO.MemoryStream
            Dim context = GetClientContext()
            ' sceglie il nome del file template in base ad id_tipo_incarico
            Dim tipoPart = dtPartecipazione.Rows.Item(0)("ac_TIPOPARTECIPAZIONE").ToString()
            Select Case dtPartecipazione.Rows.Item(0)("ac_TIPOPARTECIPAZIONE").ToString()
                Case "PG56_B"
                    filepath = System.Configuration.ConfigurationManager.AppSettings("onedrive_site_relativebase") & My.Settings.formazione_ext_path_56B_002
                Case "PG56_C"
                    filepath = System.Configuration.ConfigurationManager.AppSettings("onedrive_site_relativebase") & My.Settings.formazione_ext_path_56C_002
                Case "PG56_D"
                    filepath = System.Configuration.ConfigurationManager.AppSettings("onedrive_site_relativebase") & My.Settings.formazione_ext_path_56D_002
            End Select
            Using respStream = File.OpenBinaryDirect(context, filepath).Stream
                respStream.CopyTo(modello)
            End Using
            modello.Position = 0
            Dim docOrig As Novacode.DocX
            docOrig = Novacode.DocX.Load(modello)
            mStreamDest = New IO.MemoryStream
            modello.Position = 0
            modello.CopyTo(mStreamDest)
            mStreamDest.Position = 0
            docDest = Novacode.DocX.Load(mStreamDest)
            modello.Position = 0
            docOrig = Novacode.DocX.Load(modello)

            For Each row As DataRow In dtPartecipazione.Rows
                Console.WriteLine(String.Format("{0}", row))
                ReplaceTIPALL(dtPartecipazione, docDest)
            Next
            docOrig.Dispose()
            docDest.Save()
            modello.Dispose()
            modello = Nothing
            Return mStreamDest
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Private Function ReplaceTIPALL(dt As DataTable, docDest As Novacode.DocX) As Novacode.DocX
        For Each row As DataRow In dt.Rows
            Dim num_ext_part As String = (row.Item("ac_TIPOPARTECIPAZIONE").ToString()) & "-" & (row.Item("ni_NUMERO").ToString()) & "/" & (row.Item("ni_ANNO").ToString())
            docDest.ReplaceText("%%NUM_EXT_PART%%", num_ext_part, False, RegexOptions.None)
            If String.IsNullOrEmpty(row.Item("NOMERESP").ToString()) = False Then
                docDest.ReplaceText("%%NOME_D%%", row.Item("NOMERESP").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%NOME_D%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("COGNOMERESP").ToString()) = False Then
                docDest.ReplaceText("%%COGNOME_D%%", row.Item("COGNOMERESP").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%COGNOME_D%%", "", False, RegexOptions.None)
            End If
            If row.Item("ac_RESPONSABILE").ToString() = "SCENT" Then
                docDest.ReplaceText("%%FL_RESP%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_RESP_UO%%", "", False, RegexOptions.None)
            ElseIf row.Item("ac_RESPONSABILE").ToString() = "UOPER" Then
                docDest.ReplaceText("%%FL_RESP%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_RESP_UO%%", "X", False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%FL_RESP%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_RESP_UO%%", "", False, RegexOptions.None)
            End If
            If row.Item("ac_PROGETTOATTIVITA").ToString() = "PROGRIC" Then
                docDest.ReplaceText("%%FL_PROG%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ATT%%", "", False, RegexOptions.None)
                If String.IsNullOrEmpty(row.Item("ac_CODICEPRAF").ToString()) = False Then
                    docDest.ReplaceText("%%CODICE_PROG%%", row.Item("ac_CODICEPRAF").ToString(), False, RegexOptions.None)
                    docDest.ReplaceText("%%CODICE_ATT%%", "", False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%CODICE_PROG%%", "", False, RegexOptions.None)
                    docDest.ReplaceText("%%CODICE_ATT%%", "", False, RegexOptions.None)
                End If
                If String.IsNullOrEmpty(row.Item("ac_CODICECUP").ToString()) = False Then
                    docDest.ReplaceText("%%CODICE_CUP%%", row.Item("ac_CODICECUP").ToString(), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%CODICE_CUP%%", "", False, RegexOptions.None)
                End If
            ElseIf row.Item("ac_PROGETTOATTIVITA").ToString() = "ATTFIN" Then
                docDest.ReplaceText("%%FL_PROG%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ATT%%", "X", False, RegexOptions.None)
                If String.IsNullOrEmpty(row.Item("ac_CODICEPRAF").ToString()) = False Then
                    docDest.ReplaceText("%%CODICE_PROG%%", "", False, RegexOptions.None)
                    docDest.ReplaceText("%%CODICE_ATT%%", row.Item("ac_CODICEPRAF").ToString(), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%CODICE_PROG%%", "", False, RegexOptions.None)
                    docDest.ReplaceText("%%CODICE_ATT%%", "", False, RegexOptions.None)
                End If
                docDest.ReplaceText("%%CODICE_CUP%%", "", False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%FL_PROG%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ATT%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%CODICE_PROG%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%CODICE_ATT%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%CODICE_CUP%%", "", False, RegexOptions.None)
            End If
            If row.Item("ac_TIPOLOGIAEVENTO").ToString <> "FAD" Then
                If row.Item("dt_INIZIO").ToString <> row.Item("dt_Fine").ToString Then
                    docDest.ReplaceText("%%DATA_EVENTO%%", row.Item("dt_INIZIO").ToString.Substring(0, 10) & " - " & row.Item("dt_FINE").ToString.Substring(0, 10), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%DATA_EVENTO%%", row.Item("dt_INIZIO").ToString.Substring(0, 10), False, RegexOptions.None)
                End If
            Else
                If row.Item("dt_INIZIO").ToString <> row.Item("dt_Fine").ToString Then
                    docDest.ReplaceText("%%DATA_EVENTO%%", row.Item("dt_INIZIOFRUIZIONE").ToString.Substring(0, 10) & " - " & row.Item("dt_FINEFRUIZIONE").ToString.Substring(0, 10), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%DATA_EVENTO%%", row.Item("dt_INIZIOFRUIZIONE").ToString.Substring(0, 10), False, RegexOptions.None)
                End If
                docDest.ReplaceText("%%GIORNI_VIAGGIO%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_VIAGGIO%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%SPESE_VIAGGIO%%", "", False, RegexOptions.None)
            End If
            If row.Item("ac_TIPOLOGIAEVENTO").ToString() = "NAZ" Then
                docDest.ReplaceText("%%FL_ITALIA%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ESTERO%%", "", False, RegexOptions.None)
                If String.IsNullOrEmpty(row.Item("tx_SEDE").ToString()) = False Then
                    docDest.ReplaceText("%%CITTA%%", row.Item("tx_SEDE").ToString(), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%CITTA%%", "", False, RegexOptions.None)
                End If
                docDest.ReplaceText("%%NAZIONE%%", "", False, RegexOptions.None)
            ElseIf row.Item("ac_TIPOLOGIAEVENTO").ToString() = "INTL" Then
                docDest.ReplaceText("%%FL_ITALIA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ESTERO%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%CITTA%%", "", False, RegexOptions.None)
                If String.IsNullOrEmpty(row.Item("tx_SEDE").ToString()) = False Then
                    docDest.ReplaceText("%%NAZIONE%%", row.Item("tx_SEDE").ToString(), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%NAZIONE%%", "", False, RegexOptions.None)
                End If
            ElseIf row.Item("ac_TIPOLOGIAEVENTO").ToString() = "FAD" Then
                docDest.ReplaceText("%%FL_ITALIA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ESTERO%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%CITTA%%", "ONLINE", False, RegexOptions.None)
                docDest.ReplaceText("%%NAZIONE%%", "", False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%FL_ITALIA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_ESTERO%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%CITTA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%NAZIONE%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("tx_TITOLO").ToString()) = False Then
                docDest.ReplaceText("%%TITOLO%%", row.Item("tx_TITOLO").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%TITOLO%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("tx_ORGANIZZATORE").ToString()) = False Then
                docDest.ReplaceText("%%ENTE_ORGANIZZATORE%%", row.Item("tx_ORGANIZZATORE").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%ENTE_ORGANIZZATORE%%", "", False, RegexOptions.None)
            End If
            If row.Item("ac_NORMATIVAECM").ToString() = "2011" Then
                docDest.ReplaceText("%%FL_ECM%%", "X", False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%FL_ECM%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("nd_QUOTAISCRIZIONE_PREV").ToString()) Then
                docDest.ReplaceText("%%FL_NO_QUOTA%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_QUOTA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%QUOTA_ISCRIZIONE%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%QUOTA_ISCRIZIONE_VALUTA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%QUOTA_SCADENZA%%", "", False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%FL_NO_QUOTA%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_QUOTA%%", "X", False, RegexOptions.None)
                Dim nd_QUOTAISCRIZIONE_PREV_PARSED = ParseItalianDecimal(row.Item("nd_QUOTAISCRIZIONE_PREV").ToString())
                docDest.ReplaceText("%%QUOTA_ISCRIZIONE%%", Math.Round(nd_QUOTAISCRIZIONE_PREV_PARSED, 2).ToString(), False, RegexOptions.None)
                docDest.ReplaceText("%%QUOTA_ISCRIZIONE_VALUTA%%", row.Item("nd_QUOTAISCRIZIONE_PREV_VALUTA").ToString.Substring(0, 3), False, RegexOptions.None)
                If String.IsNullOrEmpty(row.Item("dt_PAGARSIENTRO").ToString()) = False Then
                    Dim dt_PAGARSIENTRO_PARSED = DateTime.Parse(row.Item("dt_PAGARSIENTRO").ToString())
                    docDest.ReplaceText("%%QUOTA_SCADENZA%%", dt_PAGARSIENTRO_PARSED.ToShortDateString(), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%QUOTA_SCADENZA%%", "", False, RegexOptions.None)
                End If
            End If

            If String.IsNullOrEmpty(row.Item("tx_NOME").ToString()) = False Then
                docDest.ReplaceText("%%NOME%%", row.Item("tx_NOME").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%NOME%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("tx_COGNOME").ToString()) = False Then
                docDest.ReplaceText("%%COGNOME%%", row.Item("tx_COGNOME").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%COGNOME%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("ac_MATRICOLA").ToString()) = False Then
                docDest.ReplaceText("%%MATRICOLA%%", row.Item("ac_MATRICOLA").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%MATRICOLA%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("tx_EMAIL").ToString()) = False Then
                docDest.ReplaceText("%%EMAIL%%", row.Item("tx_EMAIL").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%EMAIL%%", "", False, RegexOptions.None)
            End If
            If row.Item("js_EXTEND").ToString <> "" Then
                Dim json_partecipazione As JObject = Nothing
                json_partecipazione = JObject.Parse(row.Item("js_EXTEND").ToString)
                If CStr(json_partecipazione("tx_TELEFONO_INTERNO")) <> "" Then
                    docDest.ReplaceText("%%TELEFONO_INT%%", CStr(json_partecipazione("tx_TELEFONO_INTERNO")), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%TELEFONO_INT%%", EMPTY_ROW, False, RegexOptions.None)
                End If
            End If
            If String.IsNullOrEmpty(row.Item("tx_UNITAOPERATIVA").ToString()) = False Then
                docDest.ReplaceText("%%UO%%", row.Item("tx_UNITAOPERATIVA").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%UO%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("tx_CENTROCOSTO").ToString()) = False Then
                docDest.ReplaceText("%%CENTRO_DI_COSTO%%", row.Item("tx_CENTROCOSTO").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%CENTRO_DI_COSTO%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("tx_CENTROCOSTO_COMMESSA").ToString()) = False Then
                docDest.ReplaceText("%%COMMESSA%%", row.Item("tx_CENTROCOSTO_COMMESSA").ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%COMMESSA%%", "", False, RegexOptions.None)
            End If
            If row.Item("ac_CONTRATTO").ToString() = "DIPEN_TI" Then
                docDest.ReplaceText("%%FL_DIPEN_TI%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_PERS_RIC%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_DIPEN_TD%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_BORS%%", "", False, RegexOptions.None)
            ElseIf row.Item("ac_CONTRATTO").ToString() = "PERS_RIC" Then
                docDest.ReplaceText("%%FL_DIPEN_TI%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_PERS_RIC%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_DIPEN_TD%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_BORS%%", "", False, RegexOptions.None)
            ElseIf row.Item("ac_CONTRATTO").ToString() = "DIPEN_TD" Then
                docDest.ReplaceText("%%FL_DIPEN_TI%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_PERS_RIC%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_DIPEN_TD%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_BORS%%", "", False, RegexOptions.None)
            ElseIf row.Item("ac_CONTRATTO").ToString() = "BORS" Then
                docDest.ReplaceText("%%FL_DIPEN_TI%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_PERS_RIC%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_DIPEN_TD%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_BORS%%", "X", False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%FL_DIP%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_BORS%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%FL_CONT%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("nd_COSTOVIAGGIO_PREV").ToString()) = False Then
                Dim temp = ParseItalianDecimal(row.Item("nd_COSTOVIAGGIO_PREV").ToString())
                docDest.ReplaceText("%%FL_VIAGGIO%%", "X", False, RegexOptions.None)
                docDest.ReplaceText("%%SPESE_VIAGGIO%%", Math.Round(temp, 2).ToString(), False, RegexOptions.None)
                If row.Item("nd_GIORNIVIAGGIO").ToString <> "" Then
                    docDest.ReplaceText("%%GIORNI_VIAGGIO%%", row.Item("nd_GIORNIVIAGGIO").ToString.Replace(","c, ""), False, RegexOptions.None)
                Else
                    docDest.ReplaceText("%%GIORNI_VIAGGIO%%", "    ", False, RegexOptions.None)
                End If
            Else
                docDest.ReplaceText("%%FL_VIAGGIO%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%SPESE_VIAGGIO%%", "", False, RegexOptions.None)
                docDest.ReplaceText("%%GIORNI_VIAGGIO%%", "    ", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("nd_GIORNIFORMAZIONE").ToString()) = False Then
                Dim temp = Convert.ToInt32(Decimal.Parse(row.Item("nd_GIORNIFORMAZIONE").ToString()))
                docDest.ReplaceText("%%GIORNI_DIRIG%%", temp.ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%GIORNI_DIRIG%%", "", False, RegexOptions.None)
            End If
            If String.IsNullOrEmpty(row.Item("nd_OREFORMAZIONE").ToString()) = False Then
                Dim temp = Convert.ToInt32(Decimal.Parse(row.Item("nd_OREFORMAZIONE").ToString()))
                docDest.ReplaceText("%%ORE_AGG%%", temp.ToString(), False, RegexOptions.None)
            Else
                docDest.ReplaceText("%%ORE_AGG%%", "", False, RegexOptions.None)
            End If
            docDest.ReplaceText("%%DATA%%", DateTime.Now.ToShortDateString(), False, RegexOptions.None)
            docDest.ReplaceText("%%GIORNO%%", DateTime.Now.Day.ToString("00"), False, RegexOptions.None)
            docDest.ReplaceText("%%MESE%%", DateTime.Now.Month.ToString("00"), False, RegexOptions.None)
            docDest.ReplaceText("%%ANNO%%", DateTime.Now.Year.ToString(), False, RegexOptions.None)
        Next
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

    ' prende i dati dal SqlDataReader e li inserisce in Datatable
    Private Function getDbDataInDataTable(dbCmd As SqlCommand) As DataTable
        Try
            Dim dbRdr As SqlDataReader
            Dim dt As DataTable = Nothing
            dbRdr = dbCmd.ExecuteReader
            If dbRdr.HasRows Then
                dt = New DataTable("table")
                dt.Load(dbRdr)
            End If
            dbRdr.Close()
            dbCmd.Dispose()
            Return dt
        Catch ex As Exception

        End Try

    End Function

    ' Autenticazione Sharepoint
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
        Try
            context.ExecuteQuery()
        Catch ex As Exception
            Return Nothing
        End Try

        If session IsNot Nothing Then
            session("sharepoint_context") = context
        End If

        Return context

    End Function

#End Region

#Region "FORM/POPUP"

    ' Viene richiamata quando viene selezionato un evento esistente
    Private Sub lnkOpenPreFilledForm_Click(sender As Object, e As EventArgs) Handles lnkOpenPreFilledForm.Click
        Try
            ' prende l'id dell'evento selzionato
            Dim id_evento As String = id_EVENTO_in.Value

            ' Verifica che esista già una richiesta di partecipazione per l'evento selezionato
            Dim dbRdr As SqlDataReader
            Dim dbCmd = fpd.dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT TOP 1 * FROM ext_PARTECIPAZIONI WHERE id_EVENTO=" & id_EVENTO_in.Value & " AND id_PERSONA = " & ContextHandler.id_PERSONA.ToString
            End With
            dbRdr = dbCmd.ExecuteReader
            ' Se esiste già una richiesta di partecipazione non apre il popup e notifica all'utente l'esistenza
            ' altrimenti apre il popup
            If dbRdr.HasRows Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "partEsistente", "window.alert('Esiste già una richiesta per questo evento.');", True)
                dbRdr.Close()
                dbCmd.Dispose()
            Else
                dbRdr.Close()
                dbCmd.Dispose()
                SetupFormRichiesta(0, CInt(id_evento))
            End If
        Catch ex As Exception

        End Try

    End Sub

    ' Funzione che fa il setup del form sia nel caso di evento eisstente sia nel caso di nuovo evento
    Private Sub SetupFormRichiesta(id_PARTECIPAZIONE_open As Integer, id_EVENTO_open As Integer)

        'visibilità pannelli
        pnlDataEntry.Visible = True
        pnlVerifyData.Visible = False
        pnlPrintForm.Visible = False

        'limite input giorni
        nd_GIORNIVIAGGIO.Attributes.Add("min", "0")
        nd_GIORNIVIAGGIO.Attributes.Add("max", "10")

        'modifica radiobutton Ruolo lasciando solo partecipante
        ac_CATEGORIAECM.Items.Remove(ac_CATEGORIAECM.Items.FindByValue("D"))
        ac_CATEGORIAECM.Items.Remove(ac_CATEGORIAECM.Items.FindByValue("T"))

        'svuoto tutti i controlli e rendo invisibili i pannelli che sono visibili in funzione di altri
        ClearForm()

        If id_PARTECIPAZIONE_open = 0 Then
            If id_EVENTO_open = 0 Then
                'titolo
                lblPopupTitle.Text = "Nuova richiesta"
                lblStep1Title.Text = "Passo 1 di 3: Caricamento Dati"
            Else
                'titolo
                lblPopupTitle.Text = "Nuova richiesta per evento esistente"
                lblStep1Title.Text = "Passo 1 di 3: Caricamento Dati"
                'riempi form con dati evento esistente
                If Not ScriviDatiEvento(id_EVENTO_open) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "notOpenPopup", "alert('Impossibile recuperare i dati dell\'evento selezionato.');", True)
                    Return
                End If
            End If
        Else
            'titolo
            lblPopupTitle.Text = "Modifica dati richiesta"
            lblStep1Title.Text = "Passo 1 di 3: Modifica Dati"
            'ID - scrittura nel controllo hidden
            id_PARTECIPAZIONE_in.Value = id_PARTECIPAZIONE_open.ToString
            'Riempi form con dati partecipazione
            ScriviDatiPartecipazioneEsistente(id_PARTECIPAZIONE_open)
        End If

        'scrivo dati persona
        ScriviDatiPersona()

        'refresh del popup
        updForm.Update()

        'registra lo script js che rende visibile il popup
        ScriptManager.RegisterStartupScript(updForm, updForm.GetType, "openPopup2", "showAutocertificazionePopup(true);", True)

    End Sub

    Private Function ScriviDatiEvento(id_EVENTO_open As Integer) As Boolean
        Dim dbCmd As SqlCommand
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim pNode As XmlNode

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_GetEvento"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = id_EVENTO_open
        End With
        xReader = dbCmd.ExecuteXmlReader

        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        pNode = xDoc.SelectSingleNode("/evento")
        If Not pNode Is Nothing Then
            'titolo
            tx_TITOLO.Text = ParseXmlString(pNode, "tx_titolo")
            'tipologia
            ac_TIPOLOGIAEVENTO.SelectedValue = ParseXmlString(pNode, "ac_tipologiaevento") & "\" & ParseXmlString(pNode, "fl_fad")
            ac_TIPOLOGIAEVENTO_SelectedIndexChanged(Nothing, Nothing)
            ac_TIPOLOGIAEVENTO.Enabled = False
            Select Case ParseXmlString(pNode, "fl_fad")
                Case "0"
                    'non fad
                    tx_SEDE.Text = ParseXmlString(pNode, "tx_sede")
                    dt_INIZIO.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_inizio"))
                    dt_FINE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_fine"))
                    tx_SEDE.Enabled = False
                    dt_INIZIO.Enabled = False
                    dt_FINE.Enabled = False
                Case "1"
                    dt_INIZIOFRUIZIONE.Enabled = True
                    dt_FINEFRUIZIONE.Enabled = True
            End Select
            tx_ORGANIZZATORE.Text = ParseXmlString(pNode, "tx_organizzatore")
            dbCmd.Dispose()
            Return True
        End If
        Return False
    End Function

    Private Sub ScriviDatiPersona()
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ModuloFuoriSede"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        WriteText(dbRdr, "ac_CODICEFISCALE", tx_CODICE_FISCALE)
        WriteText(dbRdr, "tx_COGNOME", tx_COGNOME)
        WriteText(dbRdr, "tx_NOME", tx_NOME)
        WriteText(dbRdr, "ac_MATRICOLA", ac_MATRICOLA)
        WriteText(dbRdr, "tx_TELEFONO_INTERNO", tx_TELEFONO_INTERNO)
        tx_EMAIL.Text() = ContextHandler.tx_EMAIL
        'ruolo
        ac_RUOLO.DataBind()
        WriteDdnString(dbRdr, "ac_RUOLO", ac_RUOLO)
        'profilo
        ac_PROFILO.DataBind()
        WriteDdnString(dbRdr, "ac_PROFILO", ac_PROFILO)
        ' unità operativa
        ac_UNITAOPERATIVA.DataBind()
        WriteDdnString(dbRdr, "ac_UNITAOPERATIVA", ac_UNITAOPERATIVA)
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Sub ScriviDatiPartecipazioneEsistente(id_PARTECIPAZIONE_open As Integer)

        Dim dbCmd As SqlCommand
        Dim xDoc As XmlDocument
        Dim xReader As XmlReader
        Dim pNode As XmlNode

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_ext_AutocertificazionePending"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA
            .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int).Value = id_PARTECIPAZIONE_open
        End With
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        pNode = xDoc.SelectSingleNode("/autocertificazione")

        'ruolo
        ac_CATEGORIAECM.SelectedValue = ParseXmlString(pNode, "ac_categoriaecm")

        'titolo
        tx_TITOLO.Text = ParseXmlString(pNode, "tx_titolo")

        'tipologia
        ac_TIPOLOGIAEVENTO.SelectedValue = ParseXmlString(pNode, "ac_tipologiaevento") & "\" & ParseXmlString(pNode, "fl_fad")
        ac_TIPOLOGIAEVENTO_SelectedIndexChanged(Nothing, Nothing)

        Select Case ParseXmlString(pNode, "fl_fad")
            Case "0"
                'non fad
                tx_SEDE.Text = ParseXmlString(pNode, "tx_sede")
                dt_INIZIO.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_inizio"))
                dt_FINE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_fine"))
            Case "1"
                dt_INIZIOFRUIZIONE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_iniziofruizione"))
                dt_FINEFRUIZIONE.Text = FormatItalianDateY4(ParseXmlDateOnly(pNode, "dt_finefruizione"))
        End Select

    End Sub

    Private Sub ClearForm()

        pnlDatiFAD.Visible = False
        pnlDatiNonFAD.Visible = False

        ac_CATEGORIAECM.SelectedValue = "P"
        tx_TITOLO.Text = String.Empty
        tx_ORGANIZZATORE.Text = String.Empty
        ac_TIPOLOGIAEVENTO.SelectedIndex = -1
        ac_TIPOLOGIAEVENTO.Enabled = True

        dt_INIZIOFRUIZIONE.Text = String.Empty
        dt_INIZIOFRUIZIONE.Enabled = True
        dt_FINEFRUIZIONE.Text = String.Empty
        dt_FINEFRUIZIONE.Enabled = True

        tx_SEDE.Text = String.Empty
        dt_INIZIO.Text = String.Empty
        dt_INIZIO.Enabled = True
        dt_FINE.Text = String.Empty
        dt_FINE.Enabled = True

        tx_TELEFONO_INTERNO.Text = String.Empty

        ac_NORMATIVAECM.SelectedIndex = -1

        id_PARTECIPAZIONE.Value = String.Empty
        ni_ANNO.Value = String.Empty
        ni_NUMERO.Value = String.Empty

        ' azzera valori hidden
        id_EVENTO_in.Value = String.Empty
        id_PARTECIPAZIONE_in.Value = String.Empty

        ac_TIPOPARTECIPAZIONE.SelectedIndex = -1

        ClearFormDatiPartecipazione()

    End Sub


    Private Sub ClearFormDatiPartecipazione()
        nd_GIORNIFORMAZIONE.Text = String.Empty
        nd_OREFORMAZIONE.Text = String.Empty
        ac_CODICEPR.Text = String.Empty
        ac_CODICEAF.Text = String.Empty
        ac_CODICECUP.Text = String.Empty
        DropDownListDirigenti.SelectedIndex = -1
        ac_RESPONSABILE.SelectedIndex = -1
        ac_PROGETTOATTIVITA.SelectedIndex = -1
        ac_CONTRATTO.SelectedIndex = -1
        ac_CODICECUP.Text = String.Empty

        tx_CENTROCOSTO.Text = String.Empty
        tx_CENTROCOSTO_COMMESSA.Text = String.Empty
        nd_QUOTAISCRIZIONE_PREV.Text = String.Empty
        nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedValue = "EUR"
        dt_PAGARSIENTRO.Text = String.Empty
        nd_GIORNIVIAGGIO.Text = String.Empty
        nd_GIORNIFORMAZIONE.Text = String.Empty
        nd_OREFORMAZIONE.Text = String.Empty

    End Sub

    Private Sub ac_TIPOLOGIAEVENTO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_TIPOLOGIAEVENTO.SelectedIndexChanged

        tx_SEDE.Text = String.Empty
        dt_INIZIO.Text = String.Empty
        dt_FINE.Text = String.Empty
        dt_INIZIOFRUIZIONE.Text = String.Empty
        dt_FINEFRUIZIONE.Text = String.Empty

        pnlDatiFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1")
        pnlDatiNonFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\0")


    End Sub

    Private Sub ac_PROGETTOATTIVITA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ac_PROGETTOATTIVITA.SelectedIndexChanged
        container_ac_CODICECUP.Visible = False
        ac_CODICECUP.Text = String.Empty
        container_ac_CODICEPR.Visible = False
        ac_CODICEPR.Text = String.Empty
        container_ac_CODICEAF.Visible = False
        ac_CODICEAF.Text = String.Empty
        If ac_PROGETTOATTIVITA.SelectedValue = "PROGRIC" Then
            container_ac_CODICEPR.Visible = True
            container_ac_CODICECUP.Visible = True
        ElseIf ac_PROGETTOATTIVITA.SelectedValue = "ATTFIN" Then
            container_ac_CODICEAF.Visible = True
        End If
    End Sub


    Private Sub lnkNext1_Click(sender As Object, e As EventArgs) Handles lnkNext1.Click
        If ValidateStep1() Then

            WriteSummary()

            pnlDataEntry.Visible = False
            pnlVerifyData.Visible = True

        End If

    End Sub

    Private Function ValidateStep1() As Boolean
        Dim allValid = True

        'pulizie
        tx_TITOLO.Text = tx_TITOLO.Text.Trim.ToUpper
        ac_CODICEPR.Text = ac_CODICEPR.Text.Trim
        ac_CODICEAF.Text = ac_CODICEAF.Text.Trim
        ac_CODICECUP.Text = ac_CODICECUP.Text.Trim

        If tx_EMAIL.Text = String.Empty Then
            allValid = False
            err_tx_EMAIL.Text = "Campo obbligatorio"
        Else
            If Not Softailor.Global.ValidationUtils.ValidateEmail(tx_EMAIL.Text) Then
                allValid = False
                err_tx_EMAIL.Text = "Indirizzo e-mail non valido"
            End If
        End If

        If String.IsNullOrEmpty(tx_ORGANIZZATORE.Text) Then
            allValid = False
            error_tx_ORGANIZZATORE.Text = "Campo obbligatorio"
        End If

        'autorizzazioni
        If ac_TIPOPARTECIPAZIONE.SelectedIndex = -1 Then
            allValid = False
            err_ac_TIPOPARTECIPAZIONE.Text = errRequired
        End If

        'ruolo
        If ac_CATEGORIAECM.SelectedIndex = -1 Then
            allValid = False
            err_ac_CATEGORIAECM.Text = errRequired
        End If

        'titolo
        If tx_TITOLO.Text = String.Empty Then
            allValid = False
            err_tx_TITOLO.Text = errRequired
        End If

        'tipologia
        If ac_TIPOLOGIAEVENTO.SelectedIndex = -1 Then
            allValid = False
            err_ac_TIPOLOGIAEVENTO.Text = errRequired
        Else
            If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then
                allValid = allValid And ValidateStep1_FAD()
            Else
                allValid = allValid And ValidateStep1_NonFAD()
            End If
        End If

        If ac_NORMATIVAECM.SelectedIndex = -1 Then
            allValid = False
            err_ac_NORMATIVAECM.Text = errRequired
        End If

        'costo quota
        If nd_QUOTAISCRIZIONE_PREV.Text <> String.Empty Then
            Try
                If ValidateItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text) = False Or ParseItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text) < 0 Then
                    allValid = False
                    errnd_QUOTAISCRIZIONE_PREV.Text = errMoney
                Else
                    Dim temp = ParseItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text)
                    If temp > 0 And dt_PAGARSIENTRO.Text = "__/__/____" Then
                        allValid = False
                        errdt_PAGARSIENTRO.Text = errRequired
                    End If
                End If
                If nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedIndex = -1 Then
                    allValid = False
                    errnd_QUOTAISCRIZIONE_PREV_VALUTA.Text = errRequired
                End If
            Catch ex As Exception
                allValid = False
                errnd_QUOTAISCRIZIONE_PREV.Text = errMoney
            End Try
        End If


        If nd_COSTOVIAGGIO_PREV.Text <> String.Empty Then
            Try
                If Not ValidateItalianDecimal(nd_COSTOVIAGGIO_PREV.Text) Then
                    allValid = False
                    errnd_COSTOVIAGGIO_PREV.Text = errMoney
                End If
            Catch ex As Exception
                allValid = False
                errnd_COSTOVIAGGIO_PREV.Text = errMoney
            End Try
        End If

        If ac_TIPOPARTECIPAZIONE.SelectedValue = "PG56_C" Then
            If nd_GIORNIFORMAZIONE.Text <> String.Empty And nd_OREFORMAZIONE.Text <> String.Empty Then
                allValid = False
                errnd_GIORNIFORMAZIONE.Text = "Compila i giorni o le ore ma non entrambi."
            Else
                If nd_GIORNIFORMAZIONE.Text <> String.Empty Then
                    If Not ValidateItalianDecimal(nd_GIORNIFORMAZIONE.Text) Then
                        allValid = False
                        errnd_GIORNIFORMAZIONE.Text = errInvalidNumber
                    End If
                End If
                If nd_OREFORMAZIONE.Text <> String.Empty Then
                    If Not ValidateItalianDecimal(nd_OREFORMAZIONE.Text) Then
                        allValid = False
                        errnd_OREFORMAZIONE.Text = errInvalidNumber
                    End If
                End If
            End If
        End If

        If ac_TIPOPARTECIPAZIONE.SelectedValue = "PG56_D" Then
            If ac_RESPONSABILE.SelectedIndex = -1 Then
                allValid = False
                err_ac_RESPONSABILE.Text = errRequired
            End If
            If ac_PROGETTOATTIVITA.SelectedIndex = -1 Then
                allValid = False
                err_ac_PROGETTOATTIVITA.Text = errRequired
            End If
            If ac_PROGETTOATTIVITA.SelectedValue = "PROGRIC" Then
                If ac_CODICEPR.Text = String.Empty Then
                    allValid = False
                    errac_CODICEPR.Text = errRequired
                End If
                If ac_CODICECUP.Text = String.Empty Then
                    allValid = False
                    errac_CODICECUP.Text = errRequired
                End If
            End If
        End If

        If ac_CONTRATTO.SelectedIndex = -1 Then
            allValid = False
            err_ac_CONTRATTO.Text = errRequired
        End If



        If DropDownListDirigenti.SelectedValue.Length = 0 Then
            allValid = False
            err_DropDownListDirigenti.Text = errRequired
        End If

        If tx_CENTROCOSTO.Text = String.Empty Then
            allValid = False
            errtx_CENTROCOSTO.Text = errRequired
        End If
        'Return True
        Return allValid
    End Function

    Private Function ValidateStep1_FAD() As Boolean

        'validazione dati FAD
        Dim allValid = True
        Dim iniOk = False
        Dim finOk = False

        'pulizie
        dt_INIZIOFRUIZIONE.Text = dt_INIZIOFRUIZIONE.Text.Trim
        dt_FINEFRUIZIONE.Text = dt_FINEFRUIZIONE.Text.Trim

        'data inizio fruizione
        If dt_INIZIOFRUIZIONE.Text = String.Empty Then
            allValid = False
            err_dt_INIZIOFRUIZIONE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_INIZIOFRUIZIONE.Text) Then
                allValid = False
                err_dt_INIZIOFRUIZIONE.Text = errInvalidDate
            Else
                If ParseItalianDate(dt_INIZIOFRUIZIONE.Text) >= Now.Date Then
                    iniOk = True
                Else
                    allValid = False
                    err_dt_INIZIOFRUIZIONE.Text = errInvalidDateRange
                End If
            End If
        End If

        'data fine fruizione
        If dt_FINEFRUIZIONE.Text = String.Empty Then
            allValid = False
            err_dt_FINEFRUIZIONE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_FINEFRUIZIONE.Text) Then
                allValid = False
                err_dt_FINEFRUIZIONE.Text = errInvalidDate
            Else
                If ParseItalianDate(dt_FINEFRUIZIONE.Text) >= Now.Date Then
                    finOk = True
                Else
                    allValid = False
                    err_dt_FINEFRUIZIONE.Text = errInvalidDateRange
                End If
            End If
        End If

        'ordine date
        If iniOk And finOk Then
            If ParseItalianDate(dt_INIZIOFRUIZIONE.Text) > ParseItalianDate(dt_FINEFRUIZIONE.Text) Then
                allValid = False
                err_dt_FINEFRUIZIONE.Text = "La data di fine precede la data di inizio."
            End If
        End If

        Return allValid

    End Function

    Private Function ValidateStep1_NonFAD() As Boolean

        'validazione dati NON-FAD
        Dim allValid = True
        Dim iniOk = False
        Dim finOk = False

        'pulizie
        tx_SEDE.Text = tx_SEDE.Text.Trim.ToUpper
        dt_INIZIO.Text = dt_INIZIO.Text.Trim
        dt_FINE.Text = dt_FINE.Text.Trim
        nd_COSTOVIAGGIO_PREV.Text = nd_COSTOVIAGGIO_PREV.Text.Trim
        nd_QUOTAISCRIZIONE_PREV.Text = nd_QUOTAISCRIZIONE_PREV.Text.Trim
        tx_ORGANIZZATORE.Text = tx_ORGANIZZATORE.Text.Trim.ToUpper
        nd_GIORNIFORMAZIONE.Text = nd_GIORNIFORMAZIONE.Text.Trim
        nd_OREFORMAZIONE.Text = nd_OREFORMAZIONE.Text.Trim

        'sede
        If tx_SEDE.Text = String.Empty Then
            allValid = False
            err_tx_SEDE.Text = errRequired
        End If

        'data inizio
        If dt_INIZIO.Text = String.Empty Then
            allValid = False
            err_dt_INIZIO.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_INIZIO.Text) Then
                allValid = False
                err_dt_INIZIO.Text = errInvalidDate
            Else
                If ParseItalianDate(dt_INIZIO.Text) >= Now.Date Then
                    iniOk = True
                Else
                    allValid = False
                    err_dt_INIZIO.Text = errInvalidDateRange
                End If
            End If
        End If

        'data fine
        If dt_FINE.Text = String.Empty Then
            allValid = False
            err_dt_FINE.Text = errRequired
        Else
            If Not ValidateItalianDate(dt_FINE.Text) Then
                allValid = False
                err_dt_FINE.Text = errInvalidDate
            Else
                If ParseItalianDate(dt_FINE.Text) >= Now.Date Then
                    finOk = True
                Else
                    allValid = False
                    err_dt_FINE.Text = errInvalidDateRange
                End If
            End If
        End If

        'ordine date
        If iniOk And finOk Then
            If ParseItalianDate(dt_INIZIO.Text) > ParseItalianDate(dt_FINE.Text) Then
                allValid = False
                err_dt_FINE.Text = "La data di fine precede la data di inizio."
            End If
        End If

        Return allValid

    End Function

    Private Sub lnkNext2_Click(sender As Object, e As EventArgs) Handles lnkNext2.Click

        'creazione nel DB o modifica
        If id_EVENTO_in.Value = "0" Then
            CreaEventoRichiesto()
        Else
            ' Modifica (attualmente usata la stessa funzione)
            CreaEventoRichiesto()
        End If
        'Return
        'setup pannello successivo
        If id_PARTECIPAZIONE_in.Value = "-1" Then
            Return
        End If

        ' Genera documento e invia tramite mail
        GeneraDocAndSend(CInt(id_PARTECIPAZIONE.Value), False, True)

        lblPrintIstruzioni.Text = "E' stata generata la richiesta di partecipazione numero <b>" &
                ac_TIPOPARTECIPAZIONE.Text & "-" & ni_NUMERO.Value & "/" & ni_ANNO.Value & "</b>.<br/>A breve riceverai all'indirizzo email da te indicato il modulo di richiesta compilato."

        ''Refresh del form per evitare che rimangano dati "vecchi" se la pagina non viene ricaricata
        ClearForm()
        updForm.Update()

        ''Refresh della lista delle richieste pendenti
        RefreshListaPending()
        updPending.Update()

        ''Mostra il pannello
        pnlVerifyData.Visible = False
        pnlPrintForm.Visible = True
    End Sub

    ' Pulsante di salvataggio dei dati
    Private Sub lnkPrevious2_Click(sender As Object, e As EventArgs) Handles lnkPrevious2.Click
        pnlVerifyData.Visible = False
        pnlDataEntry.Visible = True
    End Sub


    Private Sub clear_summary()
        r_tx_TELEFONO_INTERNO.Text = String.Empty
        r_tx_EMAIL.Text = String.Empty
        r_tx_TITOLO.Text = String.Empty
        r_tx_TIPOLOGIAEVENTO.Text = String.Empty
        r_ac_NORMATIVAECM.Text = String.Empty
        r_tx_ORGANIZZATORE.Text = String.Empty
        r_pnlDatiFAD.Visible = False
        r_pnlDatiNonFAD.Visible = False

        r_dt_INIZIOFRUIZIONE.Text = String.Empty
        r_dt_FINEFRUIZIONE.Text = String.Empty
        row_r_nd_GIORNIVIAGGIO.Visible = False
        row_r_nd_COSTOVIAGGIO_PREV.Visible = False

        r_tx_SEDE.Text = String.Empty
        r_dt_INIZIO.Text = String.Empty
        r_dt_FINE.Text = String.Empty
        row_r_nd_GIORNIVIAGGIO.Visible = False
        r_nd_GIORNIVIAGGIO.Text = String.Empty
        row_r_nd_COSTOVIAGGIO_PREV.Visible = False
        r_nd_COSTOVIAGGIO_PREV.Text = String.Empty

        r_tx_CATEGORIAECM.Text = String.Empty
        r_ac_TIPOPARTECIPAZIONE.Text = String.Empty

        row_r_tx_NOMINATIVO.Visible = False
        r_tx_NOMINATIVO.Text = DropDownListDirigenti.SelectedItem.Text

        row_r_ac_RESPONSABILE.Visible = False
        r_ac_RESPONSABILE.Text = String.Empty

        row_r_ac_PROGETTOATTIVITA.Visible = False
        r_ac_PROGETTOATTIVITA.Text = String.Empty

        row_r_ac_CODICEPRAF.Visible = False
        r_ac_CODICEPRAF.Text = String.Empty

        row_r_ac_CODICECUP.Visible = False
        r_ac_CODICECUP.Text = String.Empty


        row_r_ac_CONTRATTO.Visible = False
        r_ac_CONTRATTO.Text = String.Empty

        row_r_ac_RESPONSABILE.Visible = False
        row_r_ac_PROGETTOATTIVITA.Visible = False
        row_r_ac_CODICEPRAF.Visible = False
        row_r_ac_CODICECUP.Visible = False
        row_r_ac_CONTRATTO.Visible = False

        row_r_tx_CENTROCOSTO.Visible = False
        r_tx_CENTROCOSTO.Text = String.Empty
        row_r_tx_CENTROCOSTO_COMMESSA.Visible = False
        r_tx_CENTROCOSTO_COMMESSA.Text = String.Empty

        row_r_nd_QUOTAISCRIZIONE_PREV.Visible = False
        r_nd_QUOTAISCRIZIONE_PREV.Text = String.Empty
        r_nd_QUOTAISCRIZIONE_PREV_VALUTA.Text = String.Empty
        row_r_dt_PAGARSIENTRO.Visible = False
        r_dt_PAGARSIENTRO.Text = String.Empty

        row_r_dt_PAGARSIENTRO.Visible = False

        row_r_nd_GIORNIFORMAZIONE.Visible = False
        r_nd_GIORNIFORMAZIONE.Text = String.Empty
        row_r_nd_OREFORMAZIONE.Visible = False
        r_nd_OREFORMAZIONE.Text = String.Empty

        row_r_nd_GIORNIFORMAZIONE.Visible = False
        row_r_nd_OREFORMAZIONE.Visible = False

    End Sub


    ' SUB per scrivere il riepilogo dei dati inseriti
    Private Sub WriteSummary()
        clear_summary()

        r_tx_TELEFONO_INTERNO.Text = tx_TELEFONO_INTERNO.Text
        r_tx_EMAIL.Text = tx_EMAIL.Text
        r_tx_TITOLO.Text = tx_TITOLO.Text
        r_tx_TIPOLOGIAEVENTO.Text = ac_TIPOLOGIAEVENTO.SelectedItem.Text
        r_ac_NORMATIVAECM.Text = If(ac_NORMATIVAECM.SelectedIndex = 0, "Si", "No")
        r_tx_ORGANIZZATORE.Text = tx_ORGANIZZATORE.Text
        r_pnlDatiFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1")
        r_pnlDatiNonFAD.Visible = ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\0")
        If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then ' FAD
            r_dt_INIZIOFRUIZIONE.Text = dt_INIZIOFRUIZIONE.Text
            r_dt_FINEFRUIZIONE.Text = dt_FINEFRUIZIONE.Text
            row_r_nd_GIORNIVIAGGIO.Visible = False
            row_r_nd_COSTOVIAGGIO_PREV.Visible = False
        Else ' NON FAD
            r_tx_SEDE.Text = tx_SEDE.Text
            r_dt_INIZIO.Text = dt_INIZIO.Text
            r_dt_FINE.Text = dt_FINE.Text
            row_r_nd_GIORNIVIAGGIO.Visible = True
            r_nd_GIORNIVIAGGIO.Text = nd_GIORNIVIAGGIO.Text
            row_r_nd_COSTOVIAGGIO_PREV.Visible = True
            r_nd_COSTOVIAGGIO_PREV.Text = nd_COSTOVIAGGIO_PREV.Text
        End If

        r_tx_CATEGORIAECM.Text = ac_CATEGORIAECM.SelectedItem.Text
        r_ac_TIPOPARTECIPAZIONE.Text = ac_TIPOPARTECIPAZIONE.SelectedItem.Text

        row_r_tx_NOMINATIVO.Visible = True
        r_tx_NOMINATIVO.Text = DropDownListDirigenti.SelectedItem.Text

        row_r_ac_CONTRATTO.Visible = True
        r_ac_CONTRATTO.Text = ac_CONTRATTO.SelectedItem.Text

        If (ac_TIPOPARTECIPAZIONE.SelectedValue = "PG56_D") Then
            row_r_ac_RESPONSABILE.Visible = True
            r_ac_RESPONSABILE.Text = ac_RESPONSABILE.SelectedItem.Text

            row_r_ac_PROGETTOATTIVITA.Visible = True
            r_ac_PROGETTOATTIVITA.Text = ac_PROGETTOATTIVITA.SelectedItem.Text

            row_r_ac_CODICEPRAF.Visible = True
            'r_ac_CODICEPRAF.Text = ac_CODICEPRAF.Text

            If ac_PROGETTOATTIVITA.SelectedValue = "PROGRIC" Then
                r_ac_CODICEPRAF.Text = ac_CODICEPR.Text
                row_r_ac_CODICECUP.Visible = True
                r_ac_CODICECUP.Text = ac_CODICECUP.Text
            Else
                r_ac_CODICEPRAF.Text = ac_CODICEAF.Text
                row_r_ac_CODICECUP.Visible = False
                r_ac_CODICECUP.Text = String.Empty
            End If

        Else
            row_r_ac_RESPONSABILE.Visible = False
            row_r_ac_PROGETTOATTIVITA.Visible = False
        End If

        row_r_tx_CENTROCOSTO.Visible = True
        r_tx_CENTROCOSTO.Text = tx_CENTROCOSTO.Text

        row_r_tx_CENTROCOSTO_COMMESSA.Visible = True
        r_tx_CENTROCOSTO_COMMESSA.Text = tx_CENTROCOSTO_COMMESSA.Text

        If Not nd_QUOTAISCRIZIONE_PREV.Text = String.Empty Then
            If (ParseItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text) > 0) Then
                row_r_nd_QUOTAISCRIZIONE_PREV.Visible = True
                r_nd_QUOTAISCRIZIONE_PREV.Text = nd_QUOTAISCRIZIONE_PREV.Text
                r_nd_QUOTAISCRIZIONE_PREV_VALUTA.Text = nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedItem.Text
                row_r_dt_PAGARSIENTRO.Visible = True
                r_dt_PAGARSIENTRO.Text = dt_PAGARSIENTRO.Text
            Else
                row_r_nd_QUOTAISCRIZIONE_PREV.Visible = False
                row_r_dt_PAGARSIENTRO.Visible = False
            End If
        End If


        If (ac_TIPOPARTECIPAZIONE.SelectedValue = "PG56_C") Then
            row_r_nd_GIORNIFORMAZIONE.Visible = True
            r_nd_GIORNIFORMAZIONE.Text = nd_GIORNIFORMAZIONE.Text
            row_r_nd_OREFORMAZIONE.Visible = True
            r_nd_OREFORMAZIONE.Text = nd_OREFORMAZIONE.Text
        Else
            row_r_nd_GIORNIFORMAZIONE.Visible = False
            row_r_nd_OREFORMAZIONE.Visible = False
        End If

    End Sub

    ' Funzione che registra la richiesta di partecipazione ad un evento nuovo/esistente
    ' se l'evento è nuovo 
    Private Sub CreaEventoRichiesto()
        Dim dbCmd As SqlCommand
        Dim prmid_PARTECIPAZIONE As SqlParameter
        Dim prmni_ANNO As SqlParameter
        Dim prmni_NUMERO As SqlParameter
        Dim ac_GRUPPONUMERAZIONE As SqlParameter

        ' prepara json
        Dim sb As New StringBuilder()
        Dim sw As New IO.StringWriter(sb)
        ' json dati_personali
        Using writer As JsonWriter = New JsonTextWriter(sw)
            writer.Formatting = Formatting.Indented
            writer.WriteStartObject()
            writer.WritePropertyName("tx_TELEFONO_INTERNO")
            writer.WriteValue(tx_TELEFONO_INTERNO.Text)
            writer.WriteEndObject()
        End Using

        dbCmd = fpd.dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_CreaEvento_MFS"

            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = ContextHandler.id_PERSONA

            .Parameters.Add("@ac_TIPOPARTECIPAZIONE", SqlDbType.NVarChar, 8).Value = ac_TIPOPARTECIPAZIONE.SelectedValue

            .Parameters.Add("@ac_CATEGORIAECM", SqlDbType.NVarChar, 8).Value = ac_CATEGORIAECM.SelectedValue
            If String.IsNullOrEmpty(id_EVENTO_in.Value) = False Then
                .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = id_EVENTO_in.Value
            Else
                .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = 0
            End If
            If String.IsNullOrEmpty(DropDownListDirigenti.SelectedValue) = False Then
                .Parameters.Add("@id_PERSONA_RESPONSABILE", SqlDbType.Int).Value = DropDownListDirigenti.SelectedValue
            End If
            .Parameters.Add("@tx_TITOLO", SqlDbType.NVarChar, 600).Value = tx_TITOLO.Text
            .Parameters.Add("@ac_TIPOLOGIAEVENTO", SqlDbType.NVarChar, 8).Value = ac_TIPOLOGIAEVENTO.SelectedValue.Replace("\0", "").Replace("\1", "")

            .Parameters.Add("@dt_DATA", SqlDbType.Date).Value = Today
            .Parameters.Add("@tx_ORGANIZZATORE", SqlDbType.NVarChar, 300).Value = tx_ORGANIZZATORE.Text

            .Parameters.Add("@ac_MATRICOLA", SqlDbType.NVarChar, 16).Value = ac_MATRICOLA.Text

            .Parameters.Add("@ac_STATOVERIFICAAPPRENDIMENTO", SqlDbType.NVarChar, 2).Value = "NP"

            If nd_QUOTAISCRIZIONE_PREV.Text <> "" Then
                .Parameters.Add("@nd_QUOTAISCRIZIONE_PREV", SqlDbType.Money).Value = ParseItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text)
                .Parameters.Add("@nd_QUOTAISCRIZIONE_PREV_VALUTA", SqlDbType.NVarChar, 3).Value = nd_QUOTAISCRIZIONE_PREV_VALUTA.SelectedValue
            End If

            .Parameters.Add("@js_EXTEND", SqlDbType.Text).Value = sw.ToString()

            If ac_TIPOLOGIAEVENTO.SelectedValue.EndsWith("\1") Then
                'FAD
                .Parameters.Add("@dt_INIZIOFRUIZIONE", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIOFRUIZIONE.Text)
                .Parameters.Add("@dt_FINEFRUIZIONE", SqlDbType.Date).Value = ParseItalianDate(dt_FINEFRUIZIONE.Text)
                .Parameters.Add("@nd_GIORNIVIAGGIO", SqlDbType.Money).Value = 0
            Else
                'NON-FAD
                .Parameters.Add("@tx_SEDE", SqlDbType.NVarChar, 300).Value = tx_SEDE.Text
                .Parameters.Add("@dt_INIZIO", SqlDbType.Date).Value = ParseItalianDate(dt_INIZIO.Text)
                .Parameters.Add("@dt_FINE", SqlDbType.Date).Value = ParseItalianDate(dt_FINE.Text)
                If nd_GIORNIVIAGGIO.Text <> "" Then
                    .Parameters.Add("@nd_GIORNIVIAGGIO", SqlDbType.Money).Value = Convert.ToDecimal(nd_GIORNIVIAGGIO.Text)
                End If
            End If
            'ECM
            .Parameters.Add("@ac_NORMATIVAECM", SqlDbType.NVarChar, 16).Value = ac_NORMATIVAECM.SelectedValue
            If ac_NORMATIVAECM.SelectedValue = "NONE" Then
                'evento non accreditato
                .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = "NC"
            Else
                'evento accreditato
                'stato ECM
                .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = "C"
            End If

            If nd_COSTOVIAGGIO_PREV.Text <> String.Empty Then .Parameters.Add("@nd_COSTOVIAGGIO_PREV", SqlDbType.Money).Value = ParseItalianDecimal(nd_COSTOVIAGGIO_PREV.Text)

            If String.IsNullOrEmpty(tx_CENTROCOSTO.Text) = False Then .Parameters.Add("@tx_CENTROCOSTO", SqlDbType.NVarChar, 50).Value = tx_CENTROCOSTO.Text
            If String.IsNullOrEmpty(tx_CENTROCOSTO_COMMESSA.Text) = False Then .Parameters.Add("@tx_CENTROCOSTO_COMMESSA", SqlDbType.NVarChar, 50).Value = tx_CENTROCOSTO_COMMESSA.Text

            If String.IsNullOrEmpty(dt_PAGARSIENTRO.Text) = False And ValidateItalianDate(dt_PAGARSIENTRO.Text) Then .Parameters.Add("@dt_PAGARSIENTRO", SqlDbType.Date).Value = ParseItalianDate(dt_PAGARSIENTRO.Text)

            If ac_TIPOPARTECIPAZIONE.Text = "PG56_C" Or ac_TIPOPARTECIPAZIONE.Text = "PG56_D" Then
                If nd_OREFORMAZIONE.Text <> String.Empty Then .Parameters.Add("@nd_OREFORMAZIONE", SqlDbType.Money).Value = ParseItalianDecimal(nd_OREFORMAZIONE.Text)
                If nd_GIORNIFORMAZIONE.Text <> String.Empty Then .Parameters.Add("@nd_GIORNIFORMAZIONE", SqlDbType.Money).Value = ParseItalianDecimal(nd_GIORNIFORMAZIONE.Text)
            End If

            If String.IsNullOrEmpty(ac_PROGETTOATTIVITA.SelectedValue) = False Then .Parameters.Add("@ac_PROGETTOATTIVITA", SqlDbType.NVarChar, 8).Value = ac_PROGETTOATTIVITA.SelectedValue

            If ac_PROGETTOATTIVITA.SelectedValue = "PROGRIC" Then
                If ac_CODICEPR.Text <> String.Empty Then .Parameters.Add("@ac_CODICEPRAF", SqlDbType.NVarChar, 50).Value = ac_CODICEPR.Text
                If ac_CODICECUP.Text <> String.Empty Then .Parameters.Add("@ac_CODICECUP", SqlDbType.NVarChar, 50).Value = ac_CODICECUP.Text
            ElseIf ac_PROGETTOATTIVITA.SelectedValue = "ATTFIN" Then
                If ac_CODICEAF.Text <> String.Empty Then .Parameters.Add("@ac_CODICEPRAF", SqlDbType.NVarChar, 50).Value = ac_CODICEAF.Text
            End If

            If String.IsNullOrEmpty(ac_RESPONSABILE.SelectedValue) = False Then .Parameters.Add("@ac_RESPONSABILE", SqlDbType.NVarChar, 8).Value = ac_RESPONSABILE.SelectedValue

            If String.IsNullOrEmpty(ac_CONTRATTO.SelectedValue) = False Then .Parameters.Add("@ac_CONTRATTO", SqlDbType.NVarChar, 8).Value = ac_CONTRATTO.SelectedValue

            'parametri in uscita
            prmid_PARTECIPAZIONE = .Parameters.Add("@id_PARTECIPAZIONE", SqlDbType.Int)
            prmid_PARTECIPAZIONE.Direction = ParameterDirection.Output

            prmni_ANNO = .Parameters.Add("@ni_ANNO", SqlDbType.Int)
            prmni_ANNO.Direction = ParameterDirection.Output

            prmni_NUMERO = .Parameters.Add("@ni_NUMERO", SqlDbType.Int)
            prmni_NUMERO.Direction = ParameterDirection.Output

            ac_GRUPPONUMERAZIONE = .Parameters.Add("@ac_GRUPPONUMERAZIONE", SqlDbType.NVarChar, 8)
            ac_GRUPPONUMERAZIONE.Direction = ParameterDirection.Output

        End With

        Try
            dbCmd.ExecuteNonQuery()
        Catch ex As Exception
            id_PARTECIPAZIONE.Value = "-1"
        End Try
        dbCmd.Dispose()

        id_PARTECIPAZIONE.Value = CInt(prmid_PARTECIPAZIONE.Value).ToString
        ni_ANNO.Value = CInt(prmni_ANNO.Value).ToString
        ni_NUMERO.Value = CInt(prmni_NUMERO.Value).ToString

    End Sub


#End Region

#Region "Utility"

    Private Sub Ac_TIPOPARTECIPAZIONE_SelectedIndexChanges(sender As Object, e As EventArgs) Handles ac_TIPOPARTECIPAZIONE.SelectedIndexChanged
        ClearFormDatiPartecipazione()

    End Sub

    Private Sub nd_QUOTAISCRIZIONE_PREV_TextChanged(sender As Object, e As EventArgs) Handles nd_QUOTAISCRIZIONE_PREV.TextChanged
        If ValidateItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text) Then
            Dim temp = ParseItalianDecimal(nd_QUOTAISCRIZIONE_PREV.Text)
            If temp > 0 Then
                dt_PAGARSIENTRO.Enabled = True
                If ValidateItalianDate(dt_INIZIO.Text) And Not ValidateItalianDate(dt_PAGARSIENTRO.Text) Then
                    dt_PAGARSIENTRO.Text = dt_INIZIO.Text
                End If
                If ValidateItalianDate(dt_INIZIOFRUIZIONE.Text) And Not ValidateItalianDate(dt_PAGARSIENTRO.Text) Then
                    dt_PAGARSIENTRO.Text = dt_INIZIOFRUIZIONE.Text
                End If
            Else
                dt_PAGARSIENTRO.Enabled = False
                dt_PAGARSIENTRO.Text = String.Empty
            End If
        Else
            nd_QUOTAISCRIZIONE_PREV.Text = String.Empty
            dt_PAGARSIENTRO.Enabled = False
            dt_PAGARSIENTRO.Text = String.Empty
            errnd_QUOTAISCRIZIONE_PREV.Text = "Formato importo non valido: deve essere del tipo 1234,12"
        End If
    End Sub

    Private Sub dt_INIZIOFRUIZIONE_TextChanged(sender As Object, e As EventArgs) Handles dt_INIZIOFRUIZIONE.TextChanged
        If Not ValidateItalianDate(dt_PAGARSIENTRO.Text) AndAlso nd_QUOTAISCRIZIONE_PREV.Text <> String.Empty And ValidateItalianDate(dt_INIZIOFRUIZIONE.Text) Then
            dt_PAGARSIENTRO.Text = dt_INIZIOFRUIZIONE.Text
        End If
    End Sub

    Private Sub dt_INIZIO_TextChanged(sender As Object, e As EventArgs) Handles dt_INIZIO.TextChanged
        If Not ValidateItalianDate(dt_PAGARSIENTRO.Text) AndAlso nd_QUOTAISCRIZIONE_PREV.Text <> String.Empty And ValidateItalianDate(dt_INIZIO.Text) Then
            dt_PAGARSIENTRO.Text = dt_INIZIO.Text
        End If
    End Sub

    Private Sub WriteText(dbRdr As SqlDataReader, field As String, txt As TextBox)

        If IsDBNull(dbRdr(field)) Then
            txt.Text = ""
        Else
            txt.Text = CStr(dbRdr(field))
        End If

    End Sub

    Private Sub WriteDate(dbRdr As DataRow, field As String, txt As TextBox)

        If IsDBNull(dbRdr(field)) Then
            txt.Text = String.Empty
        Else
            txt.Text = FormatItalianDateY4(CDate(dbRdr(field)))
        End If

    End Sub

    Private Sub WriteDdnString(dbRdr As SqlDataReader, field As String, ddn As DropDownList)
        If IsDBNull(dbRdr(field)) Then
            Try
                ddn.SelectedValue = String.Empty
            Catch ex As Exception
            End Try
        Else
            Try
                ddn.SelectedValue = CStr(dbRdr(field))
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub WriteDdnInt(dbRdr As DataRow, field As String, ddn As DropDownList)
        If IsDBNull(dbRdr(field)) Then
            Try
                ddn.SelectedValue = String.Empty
            Catch ex As Exception
            End Try
        Else
            Try
                ddn.SelectedValue = CInt(dbRdr(field)).ToString
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Function ValidaOreMinuti(ddnOre As DropDownList, ddnMinuti As DropDownList, errLbl As Label) As Boolean

        If ddnOre.SelectedValue = "" Or ddnMinuti.SelectedValue = "" Then
            errLbl.Text = "Entrambi i campi sono obbligatori"
            Return False
        Else
            Return True
        End If

    End Function

    Private Function OreMinuti(ddnOre As DropDownList, ddnMinuti As DropDownList) As String

        Return ddnOre.SelectedValue & ":" & ddnMinuti.SelectedItem.Text

    End Function

    Private Function OreMinuti2Ore(ddnOre As DropDownList, ddnMinuti As DropDownList) As Integer

        Return 60 * CInt(ddnOre.SelectedValue) + CInt(ddnMinuti.SelectedValue)

    End Function
#End Region

End Class