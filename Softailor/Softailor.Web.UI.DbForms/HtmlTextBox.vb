Imports System.Web.HttpUtility

Public Class HtmlTextBox
    Inherits TextBox

    Public Property HtmlText() As String
        'qui usiamo <br> e non <br/> per quel cazzone del signor Crystal Reports... :)
        Get
            'quando leggo il valore, lo trasformo in HTML
            Dim sText As String = MyBase.Text
            sText = sText.Replace(vbCrLf, vbCr)
            sText = sText.Replace(vbLf, vbCr)
            sText = HtmlEncode(sText)
            sText = sText.Replace(vbCr, "<br>")
            Return sText
        End Get
        Set(ByVal value As String)
            'quando scrivo il valore, lo trasformo in TESTO
            Dim sText As String = value
            sText = sText.Replace("<br>", vbCrLf)
            sText = HtmlDecode(sText)
            MyBase.Text = sText
        End Set
    End Property
End Class
