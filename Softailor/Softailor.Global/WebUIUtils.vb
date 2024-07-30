Imports System.Web.UI

Public Class WebUIUtils

    Public Shared Function FindControl(ByVal rootControl As Control, ByVal controlId As String) As Control
        Dim controlFound As Control
        Dim control As Control

        If Not rootControl Is Nothing Then
            controlFound = rootControl.FindControl(controlId)
            If Not controlFound Is Nothing Then
                Return controlFound
            End If
            For Each control In rootControl.Controls
                controlFound = FindControl(control, controlId)
                If Not controlFound Is Nothing Then
                    Return controlFound
                End If
            Next
        End If
        Return Nothing
    End Function

    Public Shared Function TextAreaToHtml(ByVal text As String) As String
        Dim sText As String = text
        sText = sText.Replace(vbCrLf, vbCr)
        sText = sText.Replace(vbLf, vbCr)
        sText = System.Web.HttpUtility.HtmlEncode(sText)
        sText = sText.Replace(vbCr, "<br/>")
        Return sText
    End Function

    Public Shared Function HtmlToTextArea(ByVal html As String) As String
        Dim sText As String = html
        sText = sText.Replace("<br/>", vbCrLf)
        sText = System.Web.HttpUtility.HtmlDecode(sText)
        Return sText
    End Function

End Class
