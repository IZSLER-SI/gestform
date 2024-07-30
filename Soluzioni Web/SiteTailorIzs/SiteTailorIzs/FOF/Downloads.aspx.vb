Public Class Downloads
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'parametri standard
        sdsFILES_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsFILES_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
    End Sub

End Class