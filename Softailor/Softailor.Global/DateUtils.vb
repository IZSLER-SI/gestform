Public Class DateUtils

    Public Shared Function DataDalAl(ByVal data1 As Date, ByVal data2 As Date, Optional ByVal cultureName As String = "it-IT") As String

        Dim ci As New System.Globalization.CultureInfo(cultureName)
        Dim g1 As String, g2 As String
        Dim m1 As String, m2 As String
        Dim a1 As String, a2 As String
        Dim sOut As String

        g1 = data1.Day.ToString
        g2 = data2.Day.ToString

        m1 = data1.ToString("MMM", ci)
        m2 = data2.ToString("MMM", ci)

        a1 = data1.Year.ToString
        a2 = data2.Year.ToString

        If g1 = g2 And m1 = m2 And a1 = a2 Then
            sOut = g1 & " " & m1 & " " & a1
        Else
            sOut = g1
            If m1 <> m2 Or a1 <> a2 Then
                sOut &= " " & m1
            End If
            If a1 <> a2 Then
                sOut &= " " & a1
            End If

            sOut &= " - " & g2 & " " & m2 & " " & a2

        End If

        Return sOut

    End Function

    Public Shared Function DataDalAlEstesa(ByVal data1 As Date, ByVal data2 As Date, Optional ByVal cultureName As String = "it-IT") As String

        Dim ci As New System.Globalization.CultureInfo(cultureName)
        Dim g1 As String, g2 As String
        Dim m1 As String, m2 As String
        Dim a1 As String, a2 As String
        Dim sOut As String

        g1 = data1.Day.ToString
        g2 = data2.Day.ToString

        m1 = data1.ToString("MMMM", ci)
        m2 = data2.ToString("MMMM", ci)

        a1 = data1.Year.ToString
        a2 = data2.Year.ToString

        If g1 = g2 And m1 = m2 And a1 = a2 Then
            Select Case data1.Day
                Case 1, 8, 11
                    sOut = "l'" & g1 & " " & m1 & " " & a1
                Case Else
                    sOut = "il " & g1 & " " & m1 & " " & a1
            End Select
        Else
            Select Case data1.Day
                Case 1, 8, 11
                    sOut = "dall'" & g1
                Case Else
                    sOut = "dal " & g1
            End Select
            If m1 <> m2 Or a1 <> a2 Then
                sOut &= " " & m1
            End If
            If a1 <> a2 Then
                sOut &= " " & a1
            End If
            Select Case data2.Day
                Case 1, 8, 11
                    sOut &= " all'" & g2 & " " & m2 & " " & a2
                Case Else
                    sOut &= " al " & g2 & " " & m2 & " " & a2
            End Select
        End If

        Return sOut

    End Function

End Class
