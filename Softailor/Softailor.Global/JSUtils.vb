Imports System.Text

Public Class JSUtils
    Public Shared Function EncodeJsStringWithQuotes(ByVal s As String) As String
        If ((s Is Nothing) OrElse (s.Length = 0)) Then
            Return """"""
        End If
        Dim len As Integer = s.Length
        Dim sb As New StringBuilder((len + 4))
        sb.Append(""""c)
        Dim i As Integer
        For i = 0 To len - 1
            Dim c As Char = s.Chars(i)
            Select Case c
                Case "\"c, """"c, ">"c
                    sb.Append("\"c)
                    sb.Append(c)
                    Exit Select
                Case ChrW(8)
                    sb.Append("\b")
                    Exit Select
                Case ChrW(9)
                    sb.Append("\t")
                    Exit Select
                Case ChrW(10)
                    sb.Append("\n")
                    Exit Select
                Case ChrW(12)
                    sb.Append("\f")
                    Exit Select
                Case ChrW(13)
                    sb.Append("\r")
                    Exit Select
                Case Else
                    If (c < " "c) Then
                        Dim tmp As New String(c, 1)
                        Dim t As String = ("000" & Integer.Parse(tmp, System.Globalization.NumberStyles.HexNumber))
                        sb.Append(("\u" & t.Substring((t.Length - 4))))
                    Else
                        sb.Append(c)
                    End If
                    Exit Select
            End Select
        Next i
        sb.Append(""""c)
        Return sb.ToString
    End Function

End Class
