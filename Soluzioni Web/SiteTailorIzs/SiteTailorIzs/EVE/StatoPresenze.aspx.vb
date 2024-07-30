Public Class StatoPresenze
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'chiamata per gestione ACL: verifica le autorizzazioni di accesso alla pagina e imposta expires=0
        If Not AclHelper.AclInitForPagesWithoutMasterPage(Server, Request, Response, True) Then
            Response.End()
            Exit Sub
        End If

        'gec final
        If Not GecFinalContextHandler.id_EVENTOset Then
            Response.End()
            Exit Sub
        End If

        'apertura connessione
        Dim dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_presid_StatoPresenze"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@dt_data", SqlDbType.Date).Value = Date.Today
        End With

        'trasformo
        Transformer.Transform(dbCmd, "Templates/StatoPresenze.xslt", phdContent, "display", Request("show").ToUpper)


        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Sub

End Class