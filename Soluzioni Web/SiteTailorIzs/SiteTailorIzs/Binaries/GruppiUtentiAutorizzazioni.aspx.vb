Public Partial Class GruppiUtentiAutorizzazioni
    Inherits System.Web.UI.Page

    Private Sub GruppiUtentiAutorizzazioni_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        sdsGRUPPI_g.SelectParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsGRUPPI_f.InsertParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsUTEGRP_f.InsertParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsAUTGRP_g.SelectParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsAUTGRP_f.InsertParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsAUTGRP_ID_CATEG.SelectParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
        sdsUTEGRP_ID_UTENT.SelectParameters("ID_AZIEN").DefaultValue = ContextHandler.ID_AZIEN.ToString
    End Sub
End Class