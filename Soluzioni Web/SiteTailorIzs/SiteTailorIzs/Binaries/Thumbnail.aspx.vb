Imports Softailor.SiteTailor.Binaries

Partial Public Class Thumbnail
    Inherits System.Web.UI.Page

    Private Sub Thumbnail_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        'visualizza i thumbnail frontoffice di un elemento
        'riceve dalla request il parametro id (ID_ELEME) e il numero di thumbnail (1,2,3)

        Dim ID_ELEME As Integer
        Dim num As Integer
        Dim found As Boolean = False
        Dim authorized As Boolean = False

        Try
            ID_ELEME = CInt(Request("id"))
            num = CInt(Request("num"))
        Catch ex As Exception
            ID_ELEME = 0
            num = 0
        End Try

        If ID_ELEME <> 0 And num <> 0 Then
            Dim dbConn As SqlConnection = DbConnectionHandler.GetOpenDataDbConn
            Dim userBinaryElement As New UserBinaryElement(dbConn, ContextHandler.ID_AZIEN, ContextHandler.ID_UTENT, ID_ELEME, ContextHandler.BinariesBasePath)
            dbConn.Close()
            With userBinaryElement
                If .Exists Then
                    If .HasThumbnail(num) Then
                        found = True
                        If .CAN_VIEW Then
                            authorized = True
                            With .GetThumbnail(num)
                                Response.Clear()
                                Response.ContentType = .MimeType
                                Response.WriteFile(.FileName)
                                Response.End()
                            End With
                        End If
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