Public Class SpeseRicaviEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sdsCOSTIRICAVI_EVENTO_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsCOSTIRICAVI_EVENTO_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsCOSTIRICAVI_EVENTO_f.InsertParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsCOSTIRICAVI_EVENTO_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME
    End Sub

End Class