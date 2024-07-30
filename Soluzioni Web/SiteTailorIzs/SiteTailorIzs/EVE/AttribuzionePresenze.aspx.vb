Imports Softailor.Web.UI.DbForms

Public Class AttribuzionePresenze
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Private Sub AttribuzionePresenze_Init(sender As Object, e As EventArgs) Handles Me.Init
        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        sdsIscritti.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsPresenti.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsPresentiParziali.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsAssentiIngiustificati.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsAssentiGiustificati.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub AttribuzionePresenze_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub grdIscritti_DataBound(sender As Object, e As System.EventArgs) Handles grdIscritti.DataBound
        If Not Page.IsPostBack Then grdIscritti.SelectedIndex = -1
    End Sub

    Private Sub grdPresenti_DataBound(sender As Object, e As System.EventArgs) Handles grdPresenti.DataBound
        If Not Page.IsPostBack Then grdPresenti.SelectedIndex = -1
    End Sub

    Private Sub grdPresentiParziali_DataBound(sender As Object, e As EventArgs) Handles grdPresentiParziali.DataBound
        If Not Page.IsPostBack Then grdPresentiParziali.SelectedIndex = -1
    End Sub

    Private Sub grdAssentiGiustificati_DataBound(sender As Object, e As System.EventArgs) Handles grdAssentiGiustificati.DataBound
        If Not Page.IsPostBack Then grdAssentiGiustificati.SelectedIndex = -1
    End Sub

    Private Sub grdAssentiIngiustificati_DataBound(sender As Object, e As System.EventArgs) Handles grdAssentiIngiustificati.DataBound
        If Not Page.IsPostBack Then grdAssentiIngiustificati.SelectedIndex = -1
    End Sub

    Private Sub grdIscritti_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdIscritti.RowCommand

        Dim index As Integer
        Dim ac_origine = "I"
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

    Private Sub grdPresenti_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPresenti.RowCommand
        Dim index As Integer
        Dim ac_origine = "P"
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

    Private Sub grdPresentiParziali_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdPresentiParziali.RowCommand
        Dim index As Integer
        Dim ac_origine = "PP"
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

    Private Sub grdAssentiGiustificati_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAssentiGiustificati.RowCommand
        Dim index As Integer
        Dim ac_origine = "AG"
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

    Private Sub grdAssentiIngiustificati_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAssentiIngiustificati.RowCommand
        Dim index As Integer
        Dim ac_origine = "AI"
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

    Private Sub MoveSingle(id_ISCRITTO As Integer, ac_origine As String, ac_destinazione As String)

        'eseguo lo spostamento
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_ISCRITTI SET ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE, dt_MODIFICA=GETDATE(), tx_MODIFICA=@tx_MODIFICA WHERE id_EVENTO=@id_EVENTO AND id_ISCRITTO=@id_ISCRITTO"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@ac_STATOISCRIZIONE", SqlDbType.NVarChar, 8).Value = ac_destinazione
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        Dim grdOrigine As StlGridView = Nothing
        Select Case ac_origine
            Case "I" : grdOrigine = grdIscritti
            Case "P" : grdOrigine = grdPresenti
            Case "PP" : grdOrigine = grdPresentiParziali
            Case "AI" : grdOrigine = grdAssentiIngiustificati
            Case "AG" : grdOrigine = grdAssentiGiustificati
        End Select
        Dim grdDestinazione As StlGridView = Nothing
        Select Case ac_destinazione
            Case "I" : grdDestinazione = grdIscritti
            Case "P" : grdDestinazione = grdPresenti
            Case "PP" : grdDestinazione = grdPresentiParziali
            Case "AI" : grdDestinazione = grdAssentiIngiustificati
            Case "AG" : grdDestinazione = grdAssentiGiustificati
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

    Private Function ValidDestinazione(s As String) As Boolean
        Return (s = "I" Or s = "P" Or s = "AI" Or s = "AG" Or s = "PP")
    End Function

    Private Sub lnkMoveAll_Click(sender As Object, e As System.EventArgs) Handles lnkMoveAll.Click

        Dim ac_STATOISCRIZIONEORIG = ddnMoveAll_Orig.SelectedValue
        Dim ac_STATOISCRIZIONEDEST = ddnMoveAll_Dest.SelectedValue

        If (ac_STATOISCRIZIONEORIG = ac_STATOISCRIZIONEDEST) Or (ac_STATOISCRIZIONEORIG = "I_BC" And ac_STATOISCRIZIONEDEST = "I") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "whattadoing", "window.alert('Hai selezionato lo stesso stato iscrizione di origine e destinazione.');", True)
            Exit Sub
        End If

        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            If ac_STATOISCRIZIONEORIG = "I_BC" Then
                .CommandText = "UPDATE eve_ISCRITTI " & _
                               "SET eve_ISCRITTI.ac_STATOISCRIZIONE=@ac_STATOISCRIZIONEDEST, " & _
                               "    eve_ISCRITTI.dt_MODIFICA=GETDATE(), eve_ISCRITTI.tx_MODIFICA=@tx_MODIFICA " & _
                               "FROM eve_ISCRITTI INNER JOIN eve_ACCESSIISCRITTI ON eve_ISCRITTI.id_ISCRITTO=eve_ACCESSIISCRITTI.id_ISCRITTO " & _
                               "WHERE eve_ISCRITTI.id_EVENTO=@id_EVENTO AND eve_ISCRITTI.ac_STATOISCRIZIONE=@ac_STATOISCRIZIONEORIG AND eve_ISCRITTI.ac_CATEGORIAECM<>'RS'"
                .Parameters.Add("@ac_STATOISCRIZIONEORIG", SqlDbType.NVarChar, 8).Value = "I"
            Else
                .CommandText = "UPDATE eve_ISCRITTI SET ac_STATOISCRIZIONE=@ac_STATOISCRIZIONEDEST, dt_MODIFICA=GETDATE(), tx_MODIFICA=@tx_MODIFICA WHERE id_EVENTO=@id_EVENTO AND ac_STATOISCRIZIONE=@ac_STATOISCRIZIONEORIG AND ac_CATEGORIAECM<>'RS'"
                .Parameters.Add("@ac_STATOISCRIZIONEORIG", SqlDbType.NVarChar, 8).Value = ac_STATOISCRIZIONEORIG
            End If
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@ac_STATOISCRIZIONEDEST", SqlDbType.NVarChar, 8).Value = ac_STATOISCRIZIONEDEST
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        Dim grdOrigine As StlGridView = Nothing
        Select Case ac_STATOISCRIZIONEORIG
            Case "I", "I_BC" : grdOrigine = grdIscritti
            Case "P" : grdOrigine = grdPresenti
            Case "PP" : grdOrigine = grdPresentiParziali
            Case "AI" : grdOrigine = grdAssentiIngiustificati
            Case "AG" : grdOrigine = grdAssentiGiustificati
        End Select
        Dim grdDestinazione As StlGridView = Nothing
        Select Case ac_STATOISCRIZIONEDEST
            Case "I" : grdDestinazione = grdIscritti
            Case "P" : grdDestinazione = grdPresenti
            Case "PP" : grdDestinazione = grdPresentiParziali
            Case "AI" : grdDestinazione = grdAssentiIngiustificati
            Case "AG" : grdDestinazione = grdAssentiGiustificati
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