Public Class PianiFormativi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsPIANIFORMATIVI_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsPIANIFORMATIVI_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

        sdsBUDGET_PIANIFORMATIVI_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsBUDGET_PIANIFORMATIVI_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class