Public Class FilesEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'parametri standard
        sdsFILES_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString


        sdsFILES_f.InsertParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString
        sdsFILES_f.InsertParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsFILES_f.UpdateParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

        sdsFILES_f.SelectParameters("tx_BASEURL").DefaultValue = ConfigurationManager.AppSettings("GF_FrontofficeBasePath_FromWeb") & "DocEve/"
    End Sub

    Public Function GetFilePreamble() As String
        Return "Evento " & GecFinalContextHandler.id_EVENTO.ToString & " - "
    End Function

End Class