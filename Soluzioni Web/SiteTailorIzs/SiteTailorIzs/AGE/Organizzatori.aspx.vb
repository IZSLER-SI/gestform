Public Class Organizzatori
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsORGANIZZATORI_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsORGANIZZATORI_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

        sdsRAPPRLEGALI_ORGANIZZATORI_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsRAPPRLEGALI_ORGANIZZATORI_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class