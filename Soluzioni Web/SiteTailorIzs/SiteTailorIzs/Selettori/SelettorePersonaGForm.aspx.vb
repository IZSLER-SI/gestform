Public Class SelettorePersonaGForm
    Inherits System.Web.UI.Page

    Dim soloDipendenti As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        soloDipendenti = Request("diponly") = "1"

        'titolo e parametri ricerca
        If soloDipendenti Then
            ltrTitolo.Text = "Selezione Dipendente"
            srcPERSONE.SearchParameters.Add(0, " AND ac_MATRICOLA is not null")
        Else
            ltrTitolo.Text = "Selezione Persona"
            srcPERSONE.SearchParameters.Add(0, "")
        End If

        If Not Page.IsPostBack Then srcPERSONE.Focus()

    End Sub

    Private Sub grdPERSONE_RowSelected(dataKeyName As String, dataKeyValue As String) Handles grdPERSONE.RowSelected

        If dataKeyValue <> "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "done", "parent.stl_sel_done('" & dataKeyValue & "');", True)
        End If

    End Sub
End Class