Public Class AspxCleaner
    Public Shared Sub CleanAspx(ByRef sAspx As String)
        'nel framework 4.0, i controlli ajaxtoolkit sono con prefisso asp:
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
    End Sub
End Class
