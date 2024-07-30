Public Class PickEvento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand

        dbConn = DbConnectionHandler.GetOpenDataDbConn

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_UltimiEventiCreatiBOXML"
        End With

        Transformer.Transform(dbCmd, "Templates/PickEvento.xslt", phdContent)

        dbCmd.Dispose()



        dbConn.Close()
        dbConn.Dispose()

    End Sub

End Class