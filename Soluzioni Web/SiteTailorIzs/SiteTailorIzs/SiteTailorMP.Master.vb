Public Class SiteTailorMP
    Inherits System.Web.UI.MasterPage

    Private _functionAuthorization As SiteTailorFunctionAuthorization

    Public Property FunctionAuthorization() As SiteTailorFunctionAuthorization
        Get
            Return _functionAuthorization
        End Get
        Set(ByVal value As SiteTailorFunctionAuthorization)
            _functionAuthorization = value
        End Set
    End Property


    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'scadenza
        Response.Expires = 0
        Response.Cache.SetNoStore()

        'script pageWs
        ltrPageWs.Text = "var stlPageWsBase = """ & Page.ResolveUrl("~/PageWs/") & """;" & vbCrLf

        'imageUrl titolo
        defaultAspxHyperLink.ImageUrl = "~/img/appTitle/" & System.Configuration.ConfigurationManager.AppSettings("sitetailor_DESAPPLI") & ".gif"

        'ottengo il percorso su disco della pagina corrente
        Dim myPath As String = Server.MapPath(HttpContext.Current.Request.FilePath).ToLower

        'verifico se l'utente è autorizzato

        Try
            _functionAuthorization = ContextHandler.USERFUNC(myPath)
        Catch ex As Exception
            _functionAuthorization = Nothing
        End Try

        If Not _functionAuthorization Is Nothing Then
            If Not _functionAuthorization.Disabled Then
                'ok, siamo autorizzati

                'vediamo se qualcuno dei customcontexthandlers richiedono una redirezione
                Dim redirectUrl As String = ""

                For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
                    redirectUrl = iCustomContextHandler.MasterPageRedirectUrl(Me.Page, FunctionAuthorization)
                Next
                If redirectUrl <> "" Then
                    Response.Redirect(redirectUrl, True)
                    Exit Sub
                End If
                SetTitle(_functionAuthorization.Description)
            Else
                'non siamo autorizzati
                Response.StatusCode = 403
                Response.End()
            End If
        Else
            'non siamo autorizzati
            Response.StatusCode = 403
            Response.End()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'gestione raw url, per URL rewriting
        Form1.Action = Request.RawUrl

        'costruzione del menu: solo se non siamo in un postback parziale
        If Not Me.ScriptManager1.IsInAsyncPostBack Then

            'riprocesso i menu
            For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
                iCustomContextHandler.ProcessFunctionsAndMenus(Nothing, ContextHandler.USERFUNC, ContextHandler.USERMENUOriginal, ContextHandler.USERMENU)
            Next

            Dim dsXml As XmlDataSource
            Dim dataBinding2 As New MenuItemBinding()
            Dim dataBinding1 As New MenuItemBinding()

            dsXml = New XmlDataSource()
            dsXml.EnableCaching = False
            dsXml.Data = ContextHandler.USERMENU.OuterXml
            dsXml.XPath = "/*/*"

            dataBinding1.DataMember = "l1"
            dataBinding1.Selectable = False
            dataBinding1.TextField = "desmenub"
            dataBinding1.ValueField = "desmenub"

            dataBinding2.DataMember = "l2"
            dataBinding2.TextField = "desmenui"
            dataBinding2.ValueField = "desmenui"
            dataBinding2.NavigateUrlField = "fullpath"

            applicationMenu.Items.Clear()
            applicationMenu.DataBindings.Add(dataBinding1)
            applicationMenu.DataBindings.Add(dataBinding2)
            applicationMenu.DataSource = dsXml
            applicationMenu.DataBind()

            'dati utente
            Dim userData As String
            If ContextHandler.TX_UNIT <> "" Then
                userData = ContextHandler.NOMEUTEN & " " & ContextHandler.COGNUTEN & " - " & ContextHandler.TX_UNIT
            Else
                userData = ContextHandler.NOMEUTEN & " " & ContextHandler.COGNUTEN & " - " & ContextHandler.RAGSOCIA
            End If
            userDataLabel.Text = userData
        End If

        'scrittura dei dati dei pannelli
        If Not Me.ScriptManager1.IsInAsyncPostBack Then
            For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
                phdContextHandlerPanels.Controls.Add(New LiteralControl(iCustomContextHandler.GetPanelTd(Me.Page, FunctionAuthorization)))
            Next
        End If
    End Sub

    Friend Sub SetTitle(ByVal title As String)
        Page.Title = ContextHandler.DESAPPLI & " - " & title
    End Sub

    Friend Sub RewriteCustomContextHandlerPanels()
        'scrittura dei dati dei pannelli
        phdContextHandlerPanels.Controls.Clear()
        For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
            phdContextHandlerPanels.Controls.Add(New LiteralControl(iCustomContextHandler.GetPanelTd(Me.Page, FunctionAuthorization)))
        Next
        updContextHandlerPanels.Update()
    End Sub

End Class