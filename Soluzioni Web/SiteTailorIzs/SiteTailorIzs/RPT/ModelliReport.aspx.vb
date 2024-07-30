Imports Softailor.ReportEngine

Public Class ModelliReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsFONTI_g.SelectParameters("tx_SHAREPOINTBASE").DefaultValue = SharepointHelper.DocsGFBaseUrl & SharepointHelper.f_Modelli
        sdsFONTI_g.SelectParameters("tx_SHAREPOINTRELATIVEBASE").DefaultValue = SharepointHelper.f_Modelli

        sdsREPORTS_g.SelectParameters("tx_SHAREPOINTBASE").DefaultValue = SharepointHelper.DocsGFBaseUrl & SharepointHelper.f_Modelli
    End Sub

    Private Sub grdFONTI_DataBound(sender As Object, e As EventArgs) Handles grdFONTI.DataBound
        If Not Page.IsPostBack Then
            If grdFONTI.DataKeys.Count > 0 Then
                txtModelsRelativeUrl.Text = grdFONTI.DataKeys(0).Values(1).ToString
            End If
        End If
    End Sub

    Private Sub grdFONTI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdFONTI.SelectedIndexChanged
        txtModelsRelativeUrl.Text = grdFONTI.SelectedDataKey.Values(1).ToString
    End Sub

End Class