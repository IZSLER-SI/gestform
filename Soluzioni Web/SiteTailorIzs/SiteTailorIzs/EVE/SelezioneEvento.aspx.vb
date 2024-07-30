Public Class SelezioneEvento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'parametri default
        sdsRecent.SelectParameters("id_UTENT").DefaultValue = ContextHandler.ID_UTENT.ToString

        'pulsante crea
        pnlNuovoEvento.Visible = Not CType(Me.Master, SiteTailorMP).FunctionAuthorization.AccessLevel = 0

        'Softailor.Web.UI.DbForms.StlSearchForm.




    End Sub

    Private Sub grdRecent_DataBound(sender As Object, e As EventArgs) Handles grdRecent.DataBound
        If Not Page.IsPostBack Then grdRecent.SelectedIndex = -1
    End Sub

    Private Sub grdRecent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRecent.SelectedIndexChanged
        SelezionaEvento(CInt(grdRecent.SelectedDataKey.Value.ToString))
    End Sub

    Private Sub grdEVENTI_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdEVENTI.SelectedIndexChanged
        SelezionaEvento(CInt(grdEVENTI.SelectedDataKey.Value.ToString))
    End Sub

    Private Sub SelezionaEvento(ByVal id_EVENTO As Integer)
        'selezione
        GecFinalContextHandler.SelectEvento(id_EVENTO, True)

        'redirect
        If Request("GoToUrl") = "" Then
            'nessun goto URL > home page evento
            Response.Redirect("~/" & id_EVENTO.ToString & "/EVE/HomeEvento.aspx")
        Else
            Response.Redirect("~/" & id_EVENTO.ToString & "/" & Request("GoToUrl"))
        End If

        'redirect: vado alla home page evento!

    End Sub

End Class