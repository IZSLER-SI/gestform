Public Class VociSpesaRicavo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsVOCISPESARICAVO_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsVOCISPESARICAVO_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class