Imports Softailor.SiteTailor.Binaries
Imports System.Xml.Serialization
Imports System.IO

Partial Public Class ElementPreview
    Inherits System.Web.UI.Page

    Private Sub ElementPreview_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
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
                        'ok si può vedere.

                        'titolo pagina
                        Page.Title = .DESEL_TX

                        'serializzo e trasformo
                        Dim sWriter As New StringWriter
                        Dim xSer As XmlSerializer = New XmlSerializer(.GetType)
                        xSer.Serialize(sWriter, userBinaryElement)
                        Dim xDoc As New XmlDocument
                        xDoc.LoadXml(sWriter.ToString)
                        sWriter.Close()

                        'rimuovo tutti i namespaces
                        xDoc.LastChild.Attributes.RemoveAll()

                        Transformer.Transform(xDoc, "Templates/ElementPreview.xslt", phdContent, _
                                              "bothumb_width", ThumbnailDefaults.LarghezzaThumbnailBackOffice.ToString, _
                                              "bothumb_height", ThumbnailDefaults.AltezzaThumbnailBackOffice.ToString, _
                                              "bothumb_mimetype", ThumbnailDefaults.MimeTypeThumbnailBackOffice)

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