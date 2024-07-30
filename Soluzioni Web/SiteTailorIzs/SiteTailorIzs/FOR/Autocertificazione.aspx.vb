Imports Softailor.Web.UI.DbForms

Public Class Autocertificazione
    Inherits System.Web.UI.Page

    Dim id_PARTECIPAZIONE As Integer

    Private Sub Autocertificazione_Init(sender As Object, e As EventArgs) Handles Me.Init

        id_PARTECIPAZIONE = CInt(Request("id"))

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'valori di default
        sdsPARTECIPAZIONI_f.SelectParameters("id_PARTECIPAZIONE").DefaultValue = id_PARTECIPAZIONE.ToString
        sdsPARTECIPAZIONI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME.ToString

    End Sub

    Private Sub CloseMe()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "closeMe",
           "parent.stl_sel_done('');", True)
    End Sub

    Private Sub lnkClose_Click(sender As Object, e As EventArgs) Handles lnkClose.Click

        If StlFormView.SomeDirtyOnPage(Me) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mustsave", "window.alert('Prima di chiudere devi salvare o annullare le modifiche effettuate.');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "orachiudo", "parent.stl_sel_done('REFRESH');", True)
        End If

    End Sub

End Class