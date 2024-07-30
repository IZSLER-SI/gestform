Imports Softailor.SiteTailor.Binaries

Partial Public Class BOThumbnail
    Inherits System.Web.UI.Page

    Private Sub BOThumbnail_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'visualizza il thumbnail backoffice di un elemento
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
                If .Exists Then
                    found = True
                    If .CAN_VIEW Then
                        authorized = True
                        Response.Clear()
                        Response.ContentType = .BackofficeThumbnail.MimeType
                        'la scrittura del file è in un try-catch per evitare un'eccezione se il file non esiste.
                        Try
                            Response.WriteFile(.BackofficeThumbnail.FileName)
                        Catch ex As Exception

                        End Try
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