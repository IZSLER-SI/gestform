Imports Softailor.Web.UI.DbForms

Public Class ChiusuraListeAttesa
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    Private Sub ChiusuraListeAttesa_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione status
        GeneraStatus()

        'preimpostazione datasource
        sdsI.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsLAP.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsLAS.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsNA.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub GeneraStatus()

        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_StatoListeAttesa"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@dt_dataoggi", SqlDbType.DateTime).Value = Date.Today
        End With

        Transformer.Transform(dbCmd, "Templates/ChiusuraListeAttesa.xslt", phdStatus)
        updStatus.Update()

    End Sub

    Private Sub ChiusuraListeAttesa_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub


    Private Sub ddnOrdinamento_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddnOrdinamento.SelectedIndexChanged
        grdI.SelectedIndex = -1
        grdI.DataBind()
        updI.Update()
        grdNA.SelectedIndex = -1
        grdNA.DataBind()
        updNA.Update()
        grdLAP.SelectedIndex = -1
        grdLAP.DataBind()
        updLAP.Update()
        grdLAS.SelectedIndex = -1
        grdLAS.DataBind()
        updLAS.Update()
    End Sub

    Private Sub grdI_DataBound(sender As Object, e As System.EventArgs) Handles grdI.DataBound
        If Not Page.IsPostBack Then grdI.SelectedIndex = -1
    End Sub

    Private Sub grdLAP_DataBound(sender As Object, e As System.EventArgs) Handles grdLAP.DataBound
        If Not Page.IsPostBack Then grdLAP.SelectedIndex = -1
    End Sub

    Private Sub grdLAS_DataBound(sender As Object, e As System.EventArgs) Handles grdLAS.DataBound
        If Not Page.IsPostBack Then grdLAS.SelectedIndex = -1
    End Sub

    Private Sub grdNA_DataBound(sender As Object, e As System.EventArgs) Handles grdNA.DataBound
        If Not Page.IsPostBack Then grdNA.SelectedIndex = -1
    End Sub



    Private Sub grdI_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdI.RowCommand
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

    Private Sub grdLAP_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLAP.RowCommand
        Dim index As Integer
        Dim ac_origine = "LAP"
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

    Private Sub grdLAS_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLAS.RowCommand
        Dim index As Integer
        Dim ac_origine = "LAS"
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

    Private Sub grdNA_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdNA.RowCommand
        Dim index As Integer
        Dim ac_origine = "NA"
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
        Return (s = "I" Or s = "NA" Or s = "LAP" Or s = "LAS")
    End Function

    Private Sub MoveSingle(id_ISCRITTO As Integer, ac_origine As String, ac_destinazione As String)

        'flag modifica da lista d'attesa a iscritto o da non accettato a iscritto
        Dim fl_ACCETTAZIONEINCHIUSURA As Boolean = False
        If ac_destinazione = "I" And (ac_origine = "LAP" Or ac_origine = "LAS" Or ac_origine = "NA") Then
            fl_ACCETTAZIONEINCHIUSURA = True
        End If

        'eseguo lo spostamento
        Dim dbCmd As SqlCommand = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "UPDATE eve_ISCRITTI SET fl_ACCETTAZIONEINCHIUSURA = @fl_ACCETTAZIONEINCHIUSURA, ac_STATOISCRIZIONE=@ac_STATOISCRIZIONE, dt_MODIFICA=GETDATE(), tx_MODIFICA=@tx_MODIFICA WHERE id_EVENTO=@id_EVENTO AND id_ISCRITTO=@id_ISCRITTO"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_ISCRITTO", SqlDbType.Int).Value = id_ISCRITTO
            .Parameters.Add("@ac_STATOISCRIZIONE", SqlDbType.NVarChar, 8).Value = ac_destinazione
            .Parameters.Add("@fl_ACCETTAZIONEINCHIUSURA", SqlDbType.Bit).Value = fl_ACCETTAZIONEINCHIUSURA
            .Parameters.Add("@tx_MODIFICA", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
        End With
        dbCmd.ExecuteNonQuery()
        dbCmd.Dispose()

        Dim grdOrigine As StlGridView = Nothing
        Select Case ac_origine
            Case "I" : grdOrigine = grdI
            Case "NA" : grdOrigine = grdNA
            Case "LAP" : grdOrigine = grdLAP
            Case "LAS" : grdOrigine = grdLAS
        End Select
        Dim grdDestinazione As StlGridView = Nothing
        Select Case ac_destinazione
            Case "I" : grdDestinazione = grdI
            Case "NA" : grdDestinazione = grdNA
            Case "LAP" : grdDestinazione = grdLAP
            Case "LAS" : grdDestinazione = grdLAS
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

        'per finire rigenero lo status
        GeneraStatus()

    End Sub

End Class