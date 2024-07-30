Public Class OrariEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Private Sub OrariEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        sdsCALENDARIO_g.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

        sdsCALENDARIO_f.InsertParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsCALENDARIO_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsCALENDARIO_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME

        sdsDateEvento.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        sdsAuleEvento.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub OrariEvento_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'durata totale
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT ISNULL(ni_TOTALEMINUTI,0) as TotaleAggr, ISNULL(ni_TOTALEMINUTI_NONRAGGR,0) as TotaleNonAggr FROM eve_EVENTI WHERE id_EVENTO=@id_evento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        dbrdr.Read()
        lblDurataTotaleAggr.Text = OreMinuti(dbrdr.GetInt32(0))
        lblDurataTotaleDisaggr.Text = OreMinuti(dbrdr.GetInt32(1))
        dbrdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Function OreMinuti(minuti As Integer) As String

        Return ((minuti - minuti Mod 60) / 60).ToString("00") & ":" &
            (minuti Mod 60).ToString("00")

    End Function

    Private Sub OrariEvento_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub
End Class