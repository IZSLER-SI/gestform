Public Class SpedizioneMailProxy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ltrJquery.Text = "<script src=""" & Page.ResolveUrl("~/Scripts/jquery-1.7.1.min.js") & """></script>"

    End Sub

End Class