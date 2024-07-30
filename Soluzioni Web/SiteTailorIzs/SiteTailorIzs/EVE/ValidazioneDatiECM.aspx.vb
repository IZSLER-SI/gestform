Public Class ValidazioneDatiECM
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Sub ValidazioneDatiECM_Init(sender As Object, e As EventArgs) Handles Me.Init

        Me.sdsISCRITTIKO_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsISCRITTIOK_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'gestione degli script
        If Not Page.IsPostBack Then
            Dim sScript = "function editIscritto_callback(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkReposition, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf
            Me.ltrRepositioning.Text = sScript
        End If
    End Sub

    Private Sub grdIscrittiKO_DataBound(sender As Object, e As System.EventArgs) Handles grdIscrittiKO.DataBound
        If Not Page.IsPostBack Then grdIscrittiKO.SelectedIndex = -1
    End Sub


    Private Sub grdIscrittiKO_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdIscrittiKO.SelectedIndexChanged
        If grdIscrittiKO.SelectedIndex <> -1 Then

            Dim id_ISCRITTO As String = grdIscrittiKO.SelectedDataKey.Value.ToString
            Dim sScript = "stl_sel_display_wh('SchedaPartecipante.aspx?id=" & id_ISCRITTO & "', 880, 780, editIscritto_callback);"

            'scrivo il valore
            txtReposition.Text = id_ISCRITTO
            updHiddenCtls.Update()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)

        End If
    End Sub

    Private Sub grdIscrittiOK_DataBound(sender As Object, e As System.EventArgs) Handles grdIscrittiOK.DataBound
        If Not Page.IsPostBack Then grdIscrittiOK.SelectedIndex = -1
    End Sub

    Private Sub grdIscrittiOK_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles grdIscrittiOK.SelectedIndexChanged
        If grdIscrittiOK.SelectedIndex <> -1 Then

            Dim id_ISCRITTO As String = grdIscrittiOK.SelectedDataKey.Value.ToString
            Dim sScript = "stl_sel_display_wh('SchedaPartecipante.aspx?id=" & id_ISCRITTO & "', 880, 780, editIscritto_callback);"

            'scrivo il valore
            txtReposition.Text = id_ISCRITTO
            updHiddenCtls.Update()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)

        End If
    End Sub

    Private Sub lnkReposition_Click(sender As Object, e As System.EventArgs) Handles lnkReposition.Click
        'deseleziono entrambe
        grdIscrittiOK.SelectedIndex = -1
        grdIscrittiKO.SelectedIndex = -1

        'forzo ricerca
        grdIscrittiOK.DataBind()
        grdIscrittiOK.UpdateParentPanel()
        grdIscrittiKO.DataBind()
        grdIscrittiKO.UpdateParentPanel()

        'riposiziono
        Dim sIdx_OK As Integer = -1
        Dim sIdx_KO As Integer = -1
        Dim cIdx As Integer

        For cIdx = 0 To grdIscrittiOK.DataKeys.Count - 1
            If grdIscrittiOK.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx_OK = cIdx
                Exit For
            End If
        Next
        For cIdx = 0 To grdIscrittiKO.DataKeys.Count - 1
            If grdIscrittiKO.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx_KO = cIdx
                Exit For
            End If
        Next

        If sIdx_OK >= 0 Or sIdx_KO >= 0 Then
            If sIdx_OK >= 0 Then
                grdIscrittiOK.SelectedIndex = sIdx_OK
                grdIscrittiOK.EnsureSelectedRowVisible()
            End If
            If sIdx_KO >= 0 Then
                grdIscrittiKO.SelectedIndex = sIdx_KO
                grdIscrittiKO.EnsureSelectedRowVisible()
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, il nominativo non ha più diritto ai crediti ECM e non è pertanto più visibile.');", True)
        End If
    End Sub

End Class