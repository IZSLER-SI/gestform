Imports Softailor.ReportEngine

Public Class SelettoreFileSharePoint
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'recupero il folder
        Dim folder As String
        Dim ext As String

        folder = Request("folder")
        ext = Request("ext")

        If folder Is Nothing Then
            Response.End()
            Exit Sub
        End If

        If folder = String.Empty Then
            Response.End()
            Exit Sub
        End If


        If folder.Contains(":") Then
            Response.End()
            Exit Sub
        End If

        If ext Is Nothing Then ext = ""

        Dim filesXDoc = SharepointHelper.GetFolderFiles(folder, ext.Split(";"c))

        Transformer.Transform(filesXDoc, "Templates/ListaFileSharePoint.xslt", phdContent, "baseurl", SharepointHelper.SharepointBaseUrl)

    End Sub

End Class