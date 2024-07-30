Public Class StlFormViewHyperLink
    Inherits HyperLink

    Private Sub StlFormViewHyperLink_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.EnableViewState = False
    End Sub
End Class
