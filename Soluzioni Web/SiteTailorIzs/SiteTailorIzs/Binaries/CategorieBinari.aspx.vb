Public Partial Class CategorieBinari
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sdsCATEGO_g.SelectParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsCATEGO_f.InsertParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
    End Sub

End Class