Public Class DettaglioTipoCorso
    Inherits System.Web.UI.Page

    Dim ac_TIPOCOBBASE As String = ""
    Dim ac_STATUSBASE As String = ""
    Dim ac_STATUSAGG As String = ""
    Dim ni_MESE As Integer = 0
    Dim ni_ANNO As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'layout ricerca
        srcPERSONE.AddNewLineAfter("ac_STATUSAGG")

        'lettura parametri dalla request
        ac_TIPOCOBBASE = Request("tcb")
        If Request("sb") IsNot Nothing Then ac_STATUSBASE = Request("sb")
        If Request("sa") IsNot Nothing Then ac_STATUSAGG = Request("sa")
        If Request("ma") IsNot Nothing Then
            ni_MESE = CInt(Request("ma").Split("_"c)(0))
            ni_ANNO = CInt(Request("ma").Split("_"c)(1))
        End If

        'parametri ricerca
        srcPERSONE.SearchParameters.Add(0, ac_TIPOCOBBASE)
        srcPERSONE.SearchParameters.Add(1, Date.Today.ToString("yyyyMMdd"))

        'titolo
        If Not Page.IsPostBack Then
            Dim dbConn As SqlConnection
            Dim dbCmd As SqlCommand
            Dim dbRdr As SqlDataReader

            lblTitolo.Text = ""
            dbConn = DbConnectionHandler.GetOpenDataDbConn

            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT tx_TIPOCOBBASE FROM cob_TIPICORSOBASE WHERE ac_TIPOCOBBASE = @ac_tipocobbase"
                .Parameters.Add("@ac_tipocobbase", SqlDbType.NVarChar, 8).Value = ac_TIPOCOBBASE
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()
            lblTitolo.Text = dbRdr.GetString(0)
            dbRdr.Close()
            dbCmd.Dispose()
            dbConn.Close()
            dbConn.Dispose()

        End If
    End Sub

    Private Sub DettaglioTipoCorso_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

        'preimpostazione valori
        If Not Page.IsPostBack Then
            If ac_STATUSBASE <> "" Then srcPERSONE.SetVisibleValue("ac_STATUSBASE", ac_STATUSBASE)
            If ac_STATUSAGG <> "" Then srcPERSONE.SetVisibleValue("ac_STATUSAGG", ac_STATUSAGG)
            If ni_MESE <> 0 Then srcPERSONE.SetVisibleValue("ni_MESE", ni_MESE.ToString)
            If ni_ANNO <> 0 Then srcPERSONE.SetVisibleValue("ni_ANNO", ni_ANNO.ToString)

            srcPERSONE.ForceSearch(True)

        End If

    End Sub

    Private Sub grdPERSONE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdPERSONE.SelectedIndexChanged

        If grdPERSONE.SelectedIndex <> -1 Then

            Dim id_PERSONA As String = grdPERSONE.SelectedDataKey.Value.ToString

            'script apertura
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupPersona",
                    "dettaglioPersona('" & ac_TIPOCOBBASE & "','" & id_PERSONA.ToString & "');", True)

        End If

    End Sub
End Class