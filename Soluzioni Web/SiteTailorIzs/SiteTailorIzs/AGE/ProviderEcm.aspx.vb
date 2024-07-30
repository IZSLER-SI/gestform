Public Class ProviderEcm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsPROVIDERECM_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsPROVIDERECM_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

        sdsRAPPRLEGALI_PROVIDERECM_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsRAPPRLEGALI_PROVIDERECM_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

    End Sub

End Class