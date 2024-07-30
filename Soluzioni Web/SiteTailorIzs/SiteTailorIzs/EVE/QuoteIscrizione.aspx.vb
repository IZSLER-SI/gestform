Public Class QuoteIscrizione
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsQUOTE_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

        sdsQUOTE_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsQUOTE_f.InsertParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsQUOTE_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class