Public Class Albi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsALBI_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsALBI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME
    End Sub

End Class