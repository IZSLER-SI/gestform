Public Class CodiciIva
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsIVA_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsIVA_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class