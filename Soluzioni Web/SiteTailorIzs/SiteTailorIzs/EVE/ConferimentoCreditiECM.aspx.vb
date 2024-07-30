Imports Softailor.Web.UI.DbForms

Public Class ConferimentoCreditiECM
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Private Sub ConferimentoCreditiECM_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        Me.sdsCandidati.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsCreditiOK.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        Me.sdsCreditiKO.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ConferimentoCreditiECM_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub grdCandidati_DataBound(sender As Object, e As System.EventArgs) Handles grdCandidati.DataBound
        If Not Page.IsPostBack Then grdCandidati.SelectedIndex = -1
    End Sub

    Private Sub grdCreditiOK_DataBound(sender As Object, e As System.EventArgs) Handles grdCreditiOK.DataBound
        If Not Page.IsPostBack Then grdCreditiOK.SelectedIndex = -1
    End Sub

    Private Sub grdCreditiKO_DataBound(sender As Object, e As System.EventArgs) Handles grdCreditiKO.DataBound
        If Not Page.IsPostBack Then grdCreditiKO.SelectedIndex = -1
    End Sub


    Private Sub grdCandidati_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdCandidati.RowCommand
        Dim index As Integer
        Dim ac_origine = "C"
        Dim ac_destinazione = ""
        Dim id_ISCRITTO As Integer
        With CType(sender, StlGridView)
            index = Convert.ToInt32(e.CommandArgument)
            id_ISCRITTO = CInt(.DataKeys(index).Values(0))
        End With
        ac_destinazione = e.CommandName

        If ValidDestinazione(ac_destinazione) And id_ISCRITTO > 0 Then
            MoveSingle(id_ISCRITTO, ac_origine, ac_destinazione)
        End If
    End Sub

    Private Sub grdCreditiOK_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdCreditiOK.RowCommand
        Dim index As Integer
        Dim ac_origine = "COK"
        Dim ac_destinazione = ""
        Dim id_ISCRITTO As Integer
        With CType(sender, StlGridView)
            index = Convert.ToInt32(e.CommandArgument)
            id_ISCRITTO = CInt(.DataKeys(index).Values(0))
        End With
        ac_destinazione = e.CommandName

        If ValidDestinazione(ac_destinazione) And id_ISCRITTO > 0 Then
            MoveSingle(id_ISCRITTO, ac_origine, ac_destinazione)
        End If
    End Sub

    Private Sub grdCreditiKO_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdCreditiKO.RowCommand
        Dim index As Integer
        Dim ac_origine = "CKO"
        Dim ac_destinazione = ""
        Dim id_ISCRITTO As Integer
        With CType(sender, StlGridView)
            index = Convert.ToInt32(e.CommandArgument)
            id_ISCRITTO = CInt(.DataKeys(index).Values(0))
        End With
        ac_destinazione = e.CommandName

        If ValidDestinazione(ac_destinazione) And id_ISCRITTO > 0 Then
            MoveSingle(id_ISCRITTO, ac_origine, ac_destinazione)
        End If
    End Sub

    Private Function ValidDestinazione(s As String) As Boolean
        Return (s = "C" Or s = "COK" Or s = "CKO")
    End Function

    Private Sub MoveSingle(id_ISCRITTO As Integer, ac_origine As String, ac_destinazione As String)

        'eseguo lo spostamento
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_ISCRITTI SET ac_STATOECM=@ac_STATOECM, dt_MODIFICA=GETDATE(), tx_MODIFICA=@tx_MODIFICA WHERE id_EVENTO=@id_EVENTO AND id_ISCRITTO=@id_ISCRITTO"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@ac_STATOECM", SqlDbType.NVarChar, 8).Value = ac_destinazione
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        Dim grdOrigine As StlGridView = Nothing
        Select Case ac_origine
            Case "C" : grdOrigine = grdCandidati
            Case "COK" : grdOrigine = grdCreditiOK
            Case "CKO" : grdOrigine = grdCreditiKO
        End Select
        Dim grdDestinazione As StlGridView = Nothing
        Select Case ac_destinazione
            Case "C" : grdDestinazione = grdCandidati
            Case "COK" : grdDestinazione = grdCreditiOK
            Case "CKO" : grdDestinazione = grdCreditiKO
        End Select

        'griglia origine: refresh senza selezione
        grdOrigine.SelectedIndex = -1
        grdOrigine.DataBind()
        grdOrigine.UpdateParentPanel()

        'griglia destinazione: refresh e selezione
        grdDestinazione.SelectedIndex = -1
        grdDestinazione.DataBind()
        grdDestinazione.UpdateParentPanel()

        Dim sIdx As Integer = -1
        Dim cIdx As Integer

        For cIdx = 0 To grdDestinazione.DataKeys.Count - 1
            If grdDestinazione.DataKeys(cIdx).Value.ToString = id_ISCRITTO.ToString Then
                sIdx = cIdx
                Exit For
            End If
        Next
        If sIdx >= 0 Then
            grdDestinazione.SelectedIndex = sIdx
            grdDestinazione.EnsureSelectedRowVisible()
        End If
    End Sub

    Private Sub lnkMoveAll_Click(sender As Object, e As System.EventArgs) Handles lnkMoveAll.Click

        Dim ac_STATOECMORIG As String
        Dim ac_STATOQUESTIONARIOORIG As String
        Dim ac_STATOPRESENZAORIG As String
        Dim ac_STATOECMDEST As String

        ac_STATOECMORIG = ddnMoveAll_Orig.SelectedValue
        ac_STATOQUESTIONARIOORIG = ddnMoveAll_Orig_Quest.SelectedValue
        ac_STATOPRESENZAORIG = ddnMoveAll_Orig_Presenza.SelectedValue
        ac_STATOECMDEST = ddnMoveAll_Dest.SelectedValue

        If ac_STATOECMORIG = ac_STATOECMDEST Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "whattadoing", "window.alert('Hai selezionato lo stesso stato crediti ECM di origine e destinazione.');", True)
            Exit Sub
        End If

        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE vw_eve_ISCRITTI SET ac_STATOECM=@ac_STATOECMDEST, dt_MODIFICA=GETDATE(), tx_MODIFICA=@tx_MODIFICA " & _
                           "WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE='P' AND ac_STATOECM=@ac_STATOECMORIG AND " & _
                           "ISNULL(@ac_STATOQUESTIONARIOORIG, ac_STATOQUESTIONARIO)=ac_STATOQUESTIONARIO AND " & _
                           "ISNULL(@ac_STATOPRESENZAORIG, ac_STATOPRESENZA)=ac_STATOPRESENZA"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@ac_STATOECMORIG", SqlDbType.NVarChar, 8).Value = ac_STATOECMORIG
            With .Parameters.Add("@ac_STATOQUESTIONARIOORIG", SqlDbType.NVarChar, 8)
                If ac_STATOQUESTIONARIOORIG = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = ac_STATOQUESTIONARIOORIG
                End If
            End With
            With .Parameters.Add("@ac_STATOPRESENZAORIG", SqlDbType.NVarChar, 8)
                If ac_STATOPRESENZAORIG = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = ac_STATOPRESENZAORIG
                End If
            End With

            .Parameters.Add("@ac_STATOECMDEST", SqlDbType.NVarChar, 8).Value = ac_STATOECMDEST
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME

        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        Dim grdOrigine As StlGridView = Nothing
        Select Case ac_STATOECMORIG
            Case "C" : grdOrigine = grdCandidati
            Case "COK" : grdOrigine = grdCreditiOK
            Case "CKO" : grdOrigine = grdCreditiKO
        End Select
        Dim grdDestinazione As StlGridView = Nothing
        Select Case ac_STATOECMDEST
            Case "C" : grdDestinazione = grdCandidati
            Case "COK" : grdDestinazione = grdCreditiOK
            Case "CKO" : grdDestinazione = grdCreditiKO
        End Select

        'griglia origine: refresh senza selezione
        grdOrigine.SelectedIndex = -1
        grdOrigine.DataBind()
        grdOrigine.UpdateParentPanel()

        'griglia destinazione: refresh senza selezione
        grdDestinazione.SelectedIndex = -1
        grdDestinazione.DataBind()
        grdDestinazione.UpdateParentPanel()

    End Sub
End Class