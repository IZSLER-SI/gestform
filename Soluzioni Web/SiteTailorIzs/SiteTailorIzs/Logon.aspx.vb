Imports Microsoft.SharePoint.Client

Partial Public Class Logon
    Inherits System.Web.UI.Page

    Private Sub Logon_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'questa pagina scade
        Response.Expires = 0
        Response.Cache.SetNoStore()

        'titolo
        imgTitle.ImageUrl = "~/img/appTitle/" & ContextHandler.DESAPPLI & ".gif"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'eseguo un sign out per chiudere in ogni caso
        'l'autenticazione all'accesso a questa pagina
        FormsAuthentication.SignOut()
        Login1.Focus()
        Page.Title = ContextHandler.DESAPPLI & " - Log In"
    End Sub

    Private Sub Login1_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        'chiamata personalizzata
        With CType(sender, Login)
            e.Authenticated = ContextHandler.LogonUser(.UserName, .Password, _
                    "~/Default.aspx", "Home Page",
                    "~/Binaries/AddBinary.aspx", "Aggiunta immagine/allegato", _
                    "~/Binaries/ChooseBinary.aspx", "Selezione immagine/allegato")

        End With
    End Sub

    Private Sub Login1_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoggedIn
        'pulizie sessione per tutti i customcontexthandler
        For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
            iCustomContextHandler.ClearSession()
        Next

        'qualunque cosa succeda vado a default.aspx
        Response.Redirect("Default.aspx")
    End Sub

End Class