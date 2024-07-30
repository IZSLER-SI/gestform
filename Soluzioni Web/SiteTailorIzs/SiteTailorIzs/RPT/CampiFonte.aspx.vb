Public Class CampiFonte
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not AclHelper.AclInitForPagesWithoutMasterPage(Server, Request, Response, True) Then Exit Sub

        Dim ac_FONTE As String = ""

        If Request("f") IsNot Nothing Then
            ac_FONTE = Request("f")
        End If

        If Request("f") <> "" Then
            Dim fonteXDoc As New XmlDocument
            fonteXDoc.Load(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))
            Transformer.Transform(fonteXDoc, "Templates/CampiFonte.xslt", phdContent)
        End If

    End Sub

End Class