Public Class CalendarioCorsiObbligatori
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand


        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_cob_CalendarioCorsi"
            .Parameters.Add("@dt_dataoggi", SqlDbType.Date).Value = Date.Today
        End With

        'generazione report
        Transformer.Transform(dbCmd, "Templates/CalendarioCorsiObbligatori.xslt", phdReport)

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()


    End Sub

End Class