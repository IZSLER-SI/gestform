Public Class WipEventi
    Inherits System.Web.UI.Page

    Dim dbConn As SqlConnection

    Private Sub WipEventi_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione controlli
        GeneraControlli()

    End Sub


    Private Sub WipEventi_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraControlli()

        Dim dbCmd As SqlCommand = dbConn.CreateCommand

        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_WE_WipEventiXml"
        End With

        Dim sAspx = Transformer.Transform(dbCmd, "WipEventi_Controls.xslt")
        Dim cCreato = Me.ParseControl(sAspx)

        phdContent.Controls.Clear()
        phdContent.Controls.Add(cCreato)

        For Each c In cCreato.Controls
            If TypeOf c Is LinkButton Then
                With CType(c, LinkButton)
                    If .ID Like "lnkSelectEvento_*" Then
                        AddHandler .Click, AddressOf lnkSelectEvento_Click
                    End If
                End With
            End If
        Next

    End Sub

    Private Sub lnkSelectEvento_Click(sender As Object, e As EventArgs)

        Dim id_EVENTO = CInt(CType(sender, LinkButton).CommandArgument)

        'selezione evento
        GecFinalContextHandler.SelectEvento(id_EVENTO, True)

        'vado alla home
        Response.Redirect("~/" & id_EVENTO.ToString & "/EVE/HomeEvento.aspx", True)

    End Sub
End Class