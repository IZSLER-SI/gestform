Imports OfficeOpenXml

Public Class PortfolioExcel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Clear()
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=Portfolio " & CType(Session("portfolio_nomeCognome"), String) & " " & Date.Today.ToString("d MMM yyyy") & ".xlsx")

        Dim excel_binary As ExcelPackage
        excel_binary = CType(Session("portfolio_binaryData"), ExcelPackage)
        Response.BinaryWrite(excel_binary.GetAsByteArray())
        Response.End()
        Response.Redirect("Persone.aspx")

    End Sub

End Class