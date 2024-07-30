Imports Softailor.SiteTailor.Binaries

Partial Public Class ElementForPlayer
    Inherits System.Web.UI.Page

    Private Sub ElementForPlayer_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'scrive i files di tipo FLV / SWF
        'riceve dalla request il parametro id (ID_ELEME)

        Dim ID_ELEME As Integer
        Dim found As Boolean = False
        Dim authorized As Boolean = False

        Try
            ID_ELEME = CInt(Request("id"))
        Catch ex As Exception
            ID_ELEME = 0
        End Try

        If ID_ELEME <> 0 Then
            Dim dbConn As SqlConnection = DbConnectionHandler.GetOpenDataDbConn
            Dim userBinaryElement As New UserBinaryElement(dbConn, ContextHandler.ID_AZIEN, ContextHandler.ID_UTENT, ID_ELEME, ContextHandler.BinariesBasePath)
            dbConn.Close()
            With userBinaryElement
                If .Exists And .ElementFileName <> "" Then
                    found = True
                    If .CAN_VIEW Then
                        authorized = True
                        Response.Clear()
                        Response.ContentType = .MIMETYPE
                        Response.WriteFile(.ElementFileName)
                        Response.End()
                    End If
                End If
            End With
        End If

        If Not found Then
            Response.StatusCode = 404 'non trovato
            Exit Sub
        End If
        If Not authorized Then
            Response.StatusCode = 403 'non autorizzato
            Exit Sub
        End If
    End Sub
End Class