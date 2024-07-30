Imports System.Data.SqlClient
Imports System.Web.HttpContext
Imports System.Web.HttpUtility

Public Interface IGFNeedsEventId
    'per contrassegnare le pagine che devono funzionare solo con un evento!!!
End Interface

Public Class GecFinalContextHandler
    Implements ICustomContextHandler

    Public Const idEventoRequestKey = "i_d_e_v_e"
    Public Const BarcodeStarter = "7747"

    Public Shared ReadOnly Property id_EVENTOset() As Boolean
        Get
            If Current.Request.QueryString(idEventoRequestKey) Is Nothing Then
                Return False
            Else
                If Current.Request.QueryString(idEventoRequestKey) = "0" Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Get
    End Property

    Public Shared ReadOnly Property id_EVENTO() As Integer
        Get
            If Current.Request.QueryString(idEventoRequestKey) Is Nothing Then
                Throw New NullReferenceException
            Else
                If Current.Request.QueryString(idEventoRequestKey) = "0" Then
                    Throw New NullReferenceException
                Else
                    Return CInt(Current.Request.QueryString(idEventoRequestKey))
                End If
            End If
        End Get
    End Property

    Public Shared ReadOnly Property id_EVENTOSqlInt32() As SqlInt32
        Get
            If Current.Request.QueryString(idEventoRequestKey) Is Nothing Then
                Return SqlInt32.Null
            Else
                If CInt(Current.Request.QueryString(idEventoRequestKey)) = 0 Then
                    Return SqlInt32.Null
                Else
                    Return New SqlInt32(CInt(Current.Request.QueryString(idEventoRequestKey)))
                End If
            End If
        End Get
    End Property

    Public Shared ReadOnly Property tx_MAILFROM() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("GF_MailFrom")
        End Get
    End Property

    Public Shared ReadOnly Property NomeReportAttestatoECM() As String
        Get
            If System.Configuration.ConfigurationManager.AppSettings("GF_NomeReportAttestatoECM") Is Nothing Then
                Return "rptAttestatoECM.rpt"
            Else
                Return System.Configuration.ConfigurationManager.AppSettings("GF_NomeReportAttestatoECM")

            End If
        End Get
    End Property

    Public Shared Sub SelectEvento(ByVal id_EVENTO As Integer, ByVal LogIt As Boolean)

        'selezione di un evento e recupero dati
        Dim dbConn As SqlConnection = DbConnectionHandler.GetOpenDataDbConn
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_SelezioneEvento"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_EVENTO
            .Parameters.Add("@logit", SqlDbType.Bit).Value = LogIt
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        With dbRdr
            'do nothing
        End With
        dbRdr.Close()
        dbCmd.Dispose()
        dbConn.Close()

        'se devo loggare, creo la cartella
        If LogIt Then
            Softailor.ReportEngine.SharepointHelper.EnsureEventFolderCreated(id_EVENTO)
        End If
    End Sub

    Public Function GetPanelTd(ByVal page As System.Web.UI.Page, ByVal functionAuthorization As SiteTailorFunctionAuthorization) As String Implements ICustomContextHandler.GetPanelTd

        'generazione testo pannello
        Dim xArgs As New Xsl.XsltArgumentList

        'abilitazioni
        Dim wipEvKey = page.Server.MapPath("~/WIP/WipEvento.aspx").ToLower
        Dim wipGlKey = page.Server.MapPath("~/WIP/WipEventi.aspx").ToLower

        Dim wipEvEnabled = ContextHandler.USERFUNC.ContainsKey(wipEvKey)
        Dim wipGlEnabled = ContextHandler.USERFUNC.ContainsKey(wipGlKey)

        'WIP globale eventi: solo se autorizzato
        If wipGlEnabled Then
            xArgs.AddParam("lnkglobalprog_visible", "", "true")
            If GecFinalContextHandler.id_EVENTOset Then
                xArgs.AddParam("lnkglobalprog_navigateurl", "", page.ResolveUrl("~/" & GecFinalContextHandler.id_EVENTO.ToString & "/WIP/WipEventi.aspx"))
            Else
                xArgs.AddParam("lnkglobalprog_navigateurl", "", page.ResolveUrl("~/0/WIP/WipEventi.aspx"))
            End If
            xArgs.AddParam("lnkglobalprog_imageurl", "", page.ResolveUrl("~/img/calendar.png"))
        Else
            xArgs.AddParam("lnkglobalprog_visible", "", "false")
            xArgs.AddParam("lnkglobalprog_navigateurl", "", "")
            xArgs.AddParam("lnkglobalprog_imageurl", "", "")
        End If

        If GecFinalContextHandler.id_EVENTOset Then
            'c'è un evento

            'variabili per lettura da db
            Dim sNomEvent As String
            Dim sSedeEvent As String
            Dim dDataInizio As Date
            Dim dDataFine As Date
            Dim sDateEvent As String
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            'lettura dei dati da DB
            dbConn = DbConnectionHandler.GetOpenDataDbConn
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_eve_DatiEvento"
                .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
                .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()
            With dbRdr
                dDataInizio = .GetDateTime(3)
                dDataFine = .GetDateTime(4)
                sNomEvent = .GetString(5)
                sSedeEvent = .GetString(6)
                sDateEvent = Softailor.Global.DateUtils.DataDalAl(dDataInizio, dDataFine)
            End With
            dbRdr.Close()
            dbCmd.Dispose()
            dbConn.Close()
            dbConn.Dispose()


            'manipolazione testi
            If sNomEvent.Length > 35 Then
                sNomEvent = Left(sNomEvent, 35) & "..."
            End If
            If sSedeEvent.Length > 35 Then
                sSedeEvent = Left(sSedeEvent, 35) & "..."
            End If
            xArgs.AddParam("lblid_event_text", "", GecFinalContextHandler.id_EVENTO.ToString)
            xArgs.AddParam("lblnomevent_text", "", sNomEvent)
            xArgs.AddParam("lblsededataevent_text", "", sSedeEvent & ", " & sDateEvent)
            xArgs.AddParam("lnkchangeevent_text", "", "Cambia/Crea")
            xArgs.AddParam("lnkchangeevent_navigateurl", "", page.ResolveUrl("~/" & GecFinalContextHandler.id_EVENTO.ToString & "/EVE/SelezioneEvento.aspx") & "?GoToUrl=" & UrlEncode(functionAuthorization.PathFromRoot.Substring(2)))
            xArgs.AddParam("lnkeventhome_visible", "", "true")
            xArgs.AddParam("lnkeventhome_navigateurl", "", page.ResolveUrl("~/" & GecFinalContextHandler.id_EVENTO.ToString & "/EVE/HomeEvento.aspx"))
            xArgs.AddParam("lnkeventdocs_visible", "", "true")
            xArgs.AddParam("lnkeventdocs_navigateurl", "", Softailor.ReportEngine.SharepointHelper.EventDocsUrl(GecFinalContextHandler.id_EVENTO))
            'programmazione evento: solo se autorizzato
            If wipEvEnabled Then
                xArgs.AddParam("lnkeventprog_visible", "", "true")
                xArgs.AddParam("lnkeventprog_navigateurl", "", page.ResolveUrl("~/" & GecFinalContextHandler.id_EVENTO.ToString & "/WIP/WipEvento.aspx"))
            Else
                xArgs.AddParam("lnkeventprog_visible", "", "false")
                xArgs.AddParam("lnkeventprog_navigateurl", "", "")
            End If
        Else
            'non c'è un evento
            xArgs.AddParam("lblid_event_text", "", "")
            xArgs.AddParam("lblnomevent_text", "", "Nessun evento selezionato")
            xArgs.AddParam("lblsededataevent_text", "", "")
            xArgs.AddParam("lnkchangeevent_text", "", "Seleziona/Crea")
            xArgs.AddParam("lnkchangeevent_navigateurl", "", page.ResolveUrl("~/0/EVE/SelezioneEvento.aspx") & "?GoToUrl=" & UrlEncode(functionAuthorization.PathFromRoot.Substring(2)))
            xArgs.AddParam("lnkeventhome_visible", "", "false")
            xArgs.AddParam("lnkeventhome_navigateurl", "", "")
            xArgs.AddParam("lnkeventdocs_visible", "", "false")
            xArgs.AddParam("lnkeventdocs_navigateurl", "", "")
            xArgs.AddParam("lnkeventprog_visible", "", "false")
            xArgs.AddParam("lnkeventprog_navigateurl", "", "")

        End If

        'trasformazione
        Return Transformer.Transform(New XmlDocument, "~/CustomContextHandlers/GecFinalContextHandlerTdPanels.xslt", xArgs)

    End Function

    Public Sub ClearSession() Implements ICustomContextHandler.ClearSession
        'GecFinalContextHandler.UnSelectEvento()
    End Sub

    Public Function DefaultPageRedirectUrl() As String Implements ICustomContextHandler.DefaultPageRedirectUrl
        If GecFinalContextHandler.id_EVENTOset Then
            Return "~/" & GecFinalContextHandler.id_EVENTO.ToString & "/EVE/HomeEvento.aspx"
        Else
            Return "~/0/EVE/SelezioneEvento.aspx"
        End If
    End Function

    Public Function MasterPageRedirectUrl(ByVal page As System.Web.UI.Page, ByVal functionAuthorization As SiteTailorFunctionAuthorization) As String Implements ICustomContextHandler.MasterPageRedirectUrl

        MasterPageRedirectUrl = ""
        If TypeOf page Is IGFNeedsEventId Then
            If Not GecFinalContextHandler.id_EVENTOset Then
                Return "~/0/EVE/SelezioneEvento.aspx?GoToUrl=" & UrlEncode(functionAuthorization.PathFromRoot.Substring(2))
            End If
        End If

    End Function

    Public Sub ProcessFunctionsAndMenus(dataDbConn As System.Data.SqlClient.SqlConnection, ByRef funzColl As System.Collections.Generic.Dictionary(Of String, SiteTailorFunctionAuthorization), userMenuXDocOriginal As System.Xml.XmlDocument, ByRef userMenuXDocProcessed As System.Xml.XmlDocument) Implements ICustomContextHandler.ProcessFunctionsAndMenus

        Dim fpAttr As XmlAttribute
        Dim fpOrig As String
        Dim fpDest As String
        Dim pos As Integer

        'reset iniziale
        userMenuXDocProcessed = CType(userMenuXDocOriginal.Clone, XmlDocument)

        'modifica degli URL,
        For Each menuNode As XmlElement In userMenuXDocProcessed.SelectNodes("/menu/l1/l2")
            fpAttr = menuNode.Attributes("fullpath")
            fpOrig = fpAttr.Value
            fpDest = ""
            pos = 1
            For Each pathPart In fpOrig.Split("/"c)
                If pos = 2 Then
                    If GecFinalContextHandler.id_EVENTOset Then
                        fpDest &= "/" & GecFinalContextHandler.id_EVENTO.ToString
                    Else
                        fpDest &= "/0"
                    End If
                End If
                If fpDest <> String.Empty Then fpDest &= "/"
                fpDest &= pathPart
                pos += 1
            Next
            fpAttr.Value = fpDest
        Next

    End Sub

    Public Function Get_dyn_id_CLIENTE() As Integer Implements ICustomContextHandler.Get_dyn_id_CLIENTE
        Return ContextHandler.ID_AZIEN
    End Function

    Public Function Get_dyn_id_EDIZIONE() As Integer Implements ICustomContextHandler.Get_dyn_id_EDIZIONE
        Return GecFinalContextHandler.id_EVENTO
    End Function

    Public Function Get_dyn_ExtensionList() As String Implements ICustomContextHandler.Get_dyn_ExtensionList
        Return ".doc .docx .pdf"
    End Function

    Public Function Get_dyn_MaxFileSizeKb() As Integer Implements ICustomContextHandler.Get_dyn_MaxFileSizeKb
        Return 2048
    End Function

End Class
