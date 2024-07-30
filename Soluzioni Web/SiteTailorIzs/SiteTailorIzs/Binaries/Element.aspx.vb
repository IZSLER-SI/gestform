Imports Softailor.SiteTailor.Binaries

Partial Public Class Element
    Inherits System.Web.UI.Page

    Private Sub Element_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
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
                        'ok si può vedere...
                        Select Case .CodicePlayer
                            Case CodiciPlayer.Direct
                                Response.Clear()
                                Response.ContentType = .MIMETYPE
                                Response.WriteFile(.ElementFileName)
                                Response.End()
                            Case CodiciPlayer.Url
                                'redirect
                                'funzionerà in un IFRAME?
                                Response.Redirect(.URL_EXTE)
                            Case CodiciPlayer.Swf
                                'titolo pagina
                                Page.Title = .DESEL_TX
                                'contenuto pagina
                                If .IS_LOCAL Then   'SWF locale
                                    Transformer.Transform(New XmlDocument, "Templates/SWF_FLV.xslt", phdContent, _
                                                          "conttype", "swf", _
                                                          "filetype", "internal", _
                                                          "id_eleme", .ID_ELEME.ToString, _
                                                          "url_exte", "", _
                                                          "ele_widt", .ELE_WIDT.ToString, _
                                                          "ele_heig", .ELE_HEIG.ToString)
                                Else    'SWF esterno
                                    Transformer.Transform(New XmlDocument, "Templates/SWF_FLV.xslt", phdContent, _
                                                          "conttype", "swf", _
                                                          "filetype", "external", _
                                                          "id_eleme", "", _
                                                          "url_exte", .URL_EXTE, _
                                                          "ele_widt", .ELE_WIDT.ToString, _
                                                          "ele_heig", .ELE_HEIG.ToString)
                                End If
                            Case CodiciPlayer.Flv
                                Page.Title = .DESEL_TX
                                'contenuto pagina
                                If .IS_LOCAL Then   'FLV locale
                                    Transformer.Transform(New XmlDocument, "Templates/SWF_FLV.xslt", phdContent, _
                                                          "conttype", "flv", _
                                                          "filetype", "internal", _
                                                          "id_eleme", .ID_ELEME.ToString, _
                                                          "url_exte", "", _
                                                          "ele_widt", .ELE_WIDT.ToString, _
                                                          "ele_heig", .ELE_HEIG.ToString)
                                Else    'SWF esterno
                                    Transformer.Transform(New XmlDocument, "Templates/SWF_FLV.xslt", phdContent, _
                                                          "conttype", "flv", _
                                                          "filetype", "external", _
                                                          "id_eleme", "", _
                                                          "url_exte", .URL_EXTE, _
                                                          "ele_widt", .ELE_WIDT.ToString, _
                                                          "ele_heig", .ELE_HEIG.ToString)
                                End If
                        End Select
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