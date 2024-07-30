Public Class FiltriStandard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub grdFONTI_DataBound(sender As Object, e As EventArgs) Handles grdFONTI.DataBound
        If Not Page.IsPostBack Then
            If grdFONTI.DataKeys.Count > 0 Then
                txtFonte.Text = grdFONTI.DataKeys(0).Values(0).ToString
            End If
        End If
    End Sub

    Private Sub grdFONTI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdFONTI.SelectedIndexChanged
        txtFonte.Text = grdFONTI.SelectedDataKey.Values(0).ToString
    End Sub

End Class