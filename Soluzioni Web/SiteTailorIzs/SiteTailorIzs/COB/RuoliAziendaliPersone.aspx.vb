Public Class RuoliAziendaliPersone
    Inherits System.Web.UI.Page

    Dim myac_RUOLO As String = ""
    Dim myac_TIPOVIS As String = ""
    Dim dbConn As SqlConnection

    Private Sub RuoliAziendaliPersone_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim dbCmd As SqlCommand


        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'determino i parametri dalla request
        If Request("r") IsNot Nothing Then
            myac_RUOLO = Request("r")
        End If
        If Request("v") IsNot Nothing Then
            myac_TIPOVIS = Request("v")
        End If

        'riempimento drop down ruoli
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT ac_RUOLO, tx_RUOLO FROM cob_RUOLI ORDER BY tx_RUOLO"
        End With
        Softailor.Global.MiscUtils.FillDropDown(ddnac_RUOLO, dbCmd, True, True, True, False)

        'se non ho un tipo visualizzazione > predefinito
        If myac_TIPOVIS = String.Empty Then
            myac_TIPOVIS = "ATT"
        End If

        'se non ho un ruolo > primo della lista
        If myac_RUOLO = String.Empty Then
            myac_RUOLO = ddnac_RUOLO.Items(0).Value
        End If

        'scrivo i parametri nelle drop down
        ddnac_TIPOVIS.SelectedValue = myac_TIPOVIS
        ddnac_RUOLO.SelectedValue = myac_RUOLO

        'imposto i dati
        sdsRUOLI_g.SelectParameters("ac_RUOLO").DefaultValue = myac_RUOLO
        sdsRUOLI_g.SelectParameters("ac_TIPOVIS").DefaultValue = myac_TIPOVIS

        sdsRUOLI_f.InsertParameters("tx_CREAZIONE").DefaultValue = ContextHandler.USERNAME
        sdsRUOLI_f.InsertParameters("ac_RUOLO").DefaultValue = myac_RUOLO
        sdsRUOLI_f.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub RuoliAziendaliPersone_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub ddnac_RUOLO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddnac_RUOLO.SelectedIndexChanged
        ReloadFilters()
    End Sub

    Private Sub ddnac_TIPOVIS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddnac_TIPOVIS.SelectedIndexChanged
        ReloadFilters()
    End Sub

    Private Sub ReloadFilters()
        Response.Redirect("RuoliAziendaliPersone.aspx?r=" & Server.UrlEncode(ddnac_RUOLO.SelectedValue) & "&v=" & Server.UrlEncode(ddnac_TIPOVIS.SelectedValue))
    End Sub
End Class