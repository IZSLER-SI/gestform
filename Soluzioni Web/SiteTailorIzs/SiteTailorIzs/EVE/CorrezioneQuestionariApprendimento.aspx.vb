Imports Softailor.Web.UI.DbForms

Public Class CorrezioneQuestionariApprendimento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Private Sub CorrezioneQuestionariApprendimento_Init(sender As Object, e As EventArgs) Handles Me.Init
        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        Me.sdsNonCorretti.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsCorrettiOK.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsCorrettiKO.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'gestione degli script
        If Not Page.IsPostBack Then
            Dim sScript = "function editQuestionario_callback(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkReposition, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf
            Me.ltrRepositioning.Text = sScript
        End If
    End Sub

    Private Sub CorrezioneQuestionariApprendimento_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub grdNonCorretti_DataBound(sender As Object, e As System.EventArgs) Handles grdNonCorretti.DataBound
        If Not Page.IsPostBack Then grdNonCorretti.SelectedIndex = -1
    End Sub

    Private Sub grdCorrettiOK_DataBound(sender As Object, e As System.EventArgs) Handles grdCorrettiOK.DataBound
        If Not Page.IsPostBack Then grdCorrettiOK.SelectedIndex = -1
    End Sub

    Private Sub grdCorrettiKO_DataBound(sender As Object, e As System.EventArgs) Handles grdCorrettiKO.DataBound
        If Not Page.IsPostBack Then grdCorrettiKO.SelectedIndex = -1
    End Sub


    Private Sub grdNonCorretti_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdNonCorretti.RowCommand
        Dim index As Integer
        Dim ac_origine = "C"
        Dim ac_destinazione = ""
        Dim id_ISCRITTO As Integer
        With CType(sender, StlGridView)
            index = Convert.ToInt32(e.CommandArgument)
            id_ISCRITTO = CInt(.DataKeys(index).Values(0))
        End With

        Select Case e.CommandName
            Case "EDIT"
                TryEditQuestionario(id_ISCRITTO)
            Case "CLEAR"
                TryClearQuestionario(id_ISCRITTO)
        End Select

    End Sub

    Private Sub grdCorrettiOK_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdCorrettiOK.RowCommand
        Dim index As Integer
        Dim ac_origine = "COK"
        Dim ac_destinazione = ""
        Dim id_ISCRITTO As Integer
        With CType(sender, StlGridView)
            index = Convert.ToInt32(e.CommandArgument)
            id_ISCRITTO = CInt(.DataKeys(index).Values(0))
        End With

        Select Case e.CommandName
            Case "EDIT"
                TryEditQuestionario(id_ISCRITTO)
            Case "CLEAR"
                TryClearQuestionario(id_ISCRITTO)
        End Select

    End Sub

    Private Sub grdCorrettiKO_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdCorrettiKO.RowCommand
        Dim index As Integer
        Dim ac_origine = "CKO"
        Dim ac_destinazione = ""
        Dim id_ISCRITTO As Integer
        With CType(sender, StlGridView)
            index = Convert.ToInt32(e.CommandArgument)
            id_ISCRITTO = CInt(.DataKeys(index).Values(0))
        End With

        Select Case e.CommandName
            Case "EDIT"
                TryEditQuestionario(id_ISCRITTO)
            Case "CLEAR"
                TryClearQuestionario(id_ISCRITTO)
        End Select

    End Sub

    Private Function QuestionarioInserito() As Boolean

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim retValue = False

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_GetQuestionario"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()

        If Not dbRdr.IsDBNull(0) Then
            retValue = True
        End If

        dbRdr.Close()
        dbCmd.Dispose()

        Return retValue
    End Function

    Private Sub TryEditQuestionario(id_ISCRITTO As Integer)

        If Not QuestionarioInserito() Then
            LaunchQuestionarioNonPresenteError()
            Exit Sub
        End If

        'OK apertura
        Dim sScript = "stl_sel_display_high('CorrezioneQuestionarioApprendimento.aspx?id=" & id_ISCRITTO & "',editQuestionario_callback);"
        'scrivo il valore
        txtReposition.Text = id_ISCRITTO.ToString
        updHiddenCtls.Update()

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "editquest", sScript, True)

    End Sub

    Private Sub TryClearQuestionario(id_ISCRITTO As Integer)
        If Not QuestionarioInserito() Then
            LaunchQuestionarioNonPresenteError()
            Exit Sub
        End If

        'ok ci siamo
        'salvataggio di "vuoto" :)
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_AddUpdateQuestionarioIscritto"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@ac_SEQUENZARISPOSTE", SqlDbType.NVarChar, 200).Value = DBNull.Value
            .Parameters.Add("@dt_CORREZIONEQUESTIONARIO", SqlDbType.DateTime).Value = Date.Now
            .Parameters.Add("@tx_UTENTECORREZIONEQUESTIONARIO", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        'rilocazione...
        txtReposition.Text = id_ISCRITTO.ToString
        lnkReposition_Click(Nothing, Nothing)

    End Sub

    Private Sub LaunchQuestionarioNonPresenteError()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "whattadoing", "window.alert('I dati del questionario ECM (numero di risposte, risposte esatte, etc) non sono stati inseriti. Impossibile proseguire.');", True)
    End Sub

    Private Sub lnkReposition_Click(sender As Object, e As System.EventArgs) Handles lnkReposition.Click
        'deseleziono ambetre :)
        grdNonCorretti.SelectedIndex = -1
        grdCorrettiOK.SelectedIndex = -1
        grdCorrettiKO.SelectedIndex = -1

        'forzo ricerca
        grdNonCorretti.DataBind()
        grdNonCorretti.UpdateParentPanel()
        grdCorrettiOK.DataBind()
        grdCorrettiOK.UpdateParentPanel()
        grdCorrettiKO.DataBind()
        grdCorrettiKO.UpdateParentPanel()

        'riposiziono
        Dim sIdx_NC As Integer = -1
        Dim sIdx_OK As Integer = -1
        Dim sIdx_KO As Integer = -1
        Dim cIdx As Integer

        For cIdx = 0 To grdNonCorretti.DataKeys.Count - 1
            If grdNonCorretti.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx_NC = cIdx
                Exit For
            End If
        Next
        For cIdx = 0 To grdCorrettiOK.DataKeys.Count - 1
            If grdCorrettiOK.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx_OK = cIdx
                Exit For
            End If
        Next
        For cIdx = 0 To grdCorrettiKO.DataKeys.Count - 1
            If grdCorrettiKO.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx_KO = cIdx
                Exit For
            End If
        Next

        If sIdx_NC >= 0 Or sIdx_OK >= 0 Or sIdx_KO >= 0 Then
            If sIdx_NC >= 0 Then
                grdNonCorretti.SelectedIndex = sIdx_NC
                grdNonCorretti.EnsureSelectedRowVisible()
            End If
            If sIdx_OK >= 0 Then
                grdCorrettiOK.SelectedIndex = sIdx_OK
                grdCorrettiOK.EnsureSelectedRowVisible()
            End If
            If sIdx_KO >= 0 Then
                grdCorrettiKO.SelectedIndex = sIdx_KO
                grdCorrettiKO.EnsureSelectedRowVisible()
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, il nominativo non ha più diritto ai crediti ECM e non è pertanto più visibile.');", True)
        End If
    End Sub
End Class