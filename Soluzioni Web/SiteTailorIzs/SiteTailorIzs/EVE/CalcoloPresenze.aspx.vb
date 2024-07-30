Imports Softailor.Web.UI.DbForms

Public Class CalcoloPresenze
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Private Sub CalcoloPresenze_Init(sender As Object, e As EventArgs) Handles Me.Init


        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        sdsORARIEVENTO_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

        sdsEVENTO_f.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

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

    Private Sub grdIscrittiOK_DataBound(sender As Object, e As System.EventArgs) Handles grdIscrittiOK.DataBound
        If Not Page.IsPostBack Then grdIscrittiOK.SelectedIndex = -1
    End Sub

    Private Sub grdIscrittiKO_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdIscrittiKO.RowCommand

        If e.CommandName = "E" Then

            Dim index As Integer

            Dim id_ISCRITTO As Integer
            With CType(sender, StlGridView)
                index = Convert.ToInt32(e.CommandArgument)
                id_ISCRITTO = CInt(.DataKeys(index).Values(0))
            End With

            Dim sScript = "stl_sel_display_wh('SchedaPartecipante.aspx?goto=part&id=" & id_ISCRITTO.ToString & "',880,780,editIscritto_callback);"

            'scrivo il valore
            txtReposition.Text = id_ISCRITTO.ToString
            updHiddenCtls.Update()

            ScriptManager.RegisterStartupScript(Me, Me.GetType, "editiscr", sScript, True)
        End If

    End Sub

    Private Sub grdIscrittiOK_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdIscrittiOK.RowCommand

        If e.CommandName = "E" Then

            Dim index As Integer

            Dim id_ISCRITTO As Integer
            With CType(sender, StlGridView)
                index = Convert.ToInt32(e.CommandArgument)
                id_ISCRITTO = CInt(.DataKeys(index).Values(0))
            End With

            Dim sScript = "stl_sel_display_wh('SchedaPartecipante.aspx?goto=part&id=" & id_ISCRITTO.ToString & "',880,780,editIscritto_callback);"

            'scrivo il valore
            txtReposition.Text = id_ISCRITTO.ToString
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
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, il nominativo non è più visibile.');", True)
        End If
    End Sub



    Private Sub CalcoloPresenze_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'durata totale
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT ISNULL(ni_TOTALEMINUTI,0) as Totale FROM eve_EVENTI WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        dbrdr.Read()
        lblDurataTotale.Text = dbrdr.GetInt32(0).ToString
        dbrdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Sub CalcoloPresenze_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub RefreshLists()
        grdIscrittiKO.DataBind()
        updIscrittiKO_g.Update()
        grdIscrittiOK.DataBind()
        updIscrittiOK_g.Update()
    End Sub

    Private Sub frmEVENTO_ItemUpdated(sender As Object, e As System.Web.UI.WebControls.FormViewUpdatedEventArgs) Handles frmEVENTO.ItemUpdated
        RefreshLists()
    End Sub
End Class