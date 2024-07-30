Public Class SiteTailorPopupMP
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

        'specifico per GecFinalContextHandler
        'riprocesso i menu
        For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
            If TypeOf iCustomContextHandler Is GecFinalContextHandler Then
                If GecFinalContextHandler.id_EVENTOset Then
                    i_d_e_v_e.InnerText = GecFinalContextHandler.id_EVENTO.ToString
                Else
                    i_d_e_v_e.InnerText = ""
                End If
            End If
        Next

        'gestione raw url, per URL rewriting
        form1.Action = Request.RawUrl

    End Sub

    Friend Sub SetTitle(ByVal title As String)
        Page.Title = ContextHandler.DESAPPLI & " - " & title
    End Sub

End Class