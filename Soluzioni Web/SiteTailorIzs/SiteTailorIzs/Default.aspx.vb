Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'scadenza
        Response.Expires = 0
        Response.Cache.SetNoStore()

        'non controllo autorizzazione perchè sono Default!

        'redirect a seconda di evento o meno...

        'gestione del redirect per i vari contexthandler
        Dim redirectUrl As String = ""

        For Each iCustomContextHandler In CustomContextHandlerHelper.GetApplicationCustomContextHandlerList
            redirectUrl = iCustomContextHandler.DefaultPageRedirectUrl()
        Next
        If redirectUrl <> "" Then Response.Redirect(redirectUrl, True)

    End Sub

End Class