Public Class CentriReferenza
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsCENTRIREFERENZA_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsCENTRIREFERENZA_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class