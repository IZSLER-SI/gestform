Public Class UIHelpers
    Public Shared Function Thumbnail(ByVal value As Object, ByVal page As System.Web.UI.Page) As String
        'generazione HTML thumbnail immagine per griglie
        Return If(TypeOf value Is System.DBNull, _
                "", _
                "<a target=""_blank"" href=""" & _
                page.ResolveUrl("~/Binaries/ElementPreview.aspx") & _
                "?id=" & value.ToString & _
                """>" & _
                "<img class=""thumb_i"" src=""" & page.ResolveUrl("~/Binaries/BOThumbnail.aspx") & _
                "?id=" & value.ToString & _
                """ /></a>")
    End Function
End Class
