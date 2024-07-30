Imports System.Data.SqlClient
Imports System.Web.UI.WebControls

Public Class MiscUtils

    Public Shared Function InizialiNome(ByVal nome As String) As String
        Dim i As Integer
        Dim SP As Boolean
        Dim O As String = ""
        Dim s As String = ""

        If nome.Trim = "" Then
            Return ""
        Else
            s = nome.ToUpper
        End If
        'pulizia
        s = s.Replace(".", " ")
        Do While s.Contains("  ")
            s = s.Replace("  ", " ")
        Loop
        s = " " & s.Trim

        SP = False
        For i = 1 To Len(s)
            If SP Then
                If Mid(s, i, 1) <> " " Then
                    O = O & UCase(Mid(s, i, 1)) & ". "
                    SP = False
                End If
            Else
                If Mid(s, i, 1) = " " Then SP = True
            End If
        Next i
        If Len(O) > 10 Then O = Left(O, 10)
        InizialiNome = O.Trim
    End Function

    Public Overloads Shared Sub FillDropDown(ByVal ddn As DropDownList, ByVal dbConn As SqlConnection, ByVal query As String, ByVal ValueIsString As Boolean, ByVal TextIsString As Boolean, ByVal ClearFirst As Boolean, ByVal AddEmpty As Boolean)

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'pulizia iniziale se richiesto
        If ClearFirst Then ddn.Items.Clear()

        'aggiunta item vuota se richiesto
        If AddEmpty Then ddn.Items.Add(New ListItem("", ""))

        'lettura
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = query
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim LI As New ListItem
            With dbRdr
                If ValueIsString Then
                    LI.Value = dbRdr.GetString(0)
                Else
                    LI.Value = dbRdr.GetInt32(0).ToString
                End If
                If TextIsString Then
                    LI.Text = dbRdr.GetString(1)
                Else
                    LI.Text = dbRdr.GetInt32(1).ToString
                End If
            End With
            ddn.Items.Add(LI)
        Loop
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Public Overloads Shared Sub FillDropDown(ByVal ddn As DropDownList, dbCmd As SqlCommand, ByVal ValueIsString As Boolean, ByVal TextIsString As Boolean, ByVal ClearFirst As Boolean, ByVal AddEmpty As Boolean)

        Dim dbRdr As SqlDataReader

        'pulizia iniziale se richiesto
        If ClearFirst Then ddn.Items.Clear()

        'aggiunta item vuota se richiesto
        If AddEmpty Then ddn.Items.Add(New ListItem("", ""))

        'lettura
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            Dim LI As New ListItem
            With dbRdr
                If ValueIsString Then
                    LI.Value = dbRdr.GetString(0)
                Else
                    LI.Value = dbRdr.GetInt32(0).ToString
                End If
                If TextIsString Then
                    LI.Text = dbRdr.GetString(1)
                Else
                    LI.Text = dbRdr.GetInt32(1).ToString
                End If
            End With
            ddn.Items.Add(LI)
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Public Overloads Shared Sub FillDropDown(ByVal ddn As DropDownList, ByVal list As Dictionary(Of Integer, String), ByVal ClearFirst As Boolean, ByVal AddEmpty As Boolean)

        'pulizia iniziale se richiesto
        If ClearFirst Then ddn.Items.Clear()

        'aggiunta item vuota se richiesto
        If AddEmpty Then ddn.Items.Add(New ListItem("", ""))

        For Each item As KeyValuePair(Of Integer, String) In list
            ddn.Items.Add(New ListItem(item.Value, item.Key.ToString))
        Next
    End Sub

    Public Overloads Shared Sub FillDropDown(ByVal ddn As DropDownList, ByVal list As Dictionary(Of String, String), ByVal ClearFirst As Boolean, ByVal AddEmpty As Boolean)

        'pulizia iniziale se richiesto
        If ClearFirst Then ddn.Items.Clear()

        'aggiunta item vuota se richiesto
        If AddEmpty Then ddn.Items.Add(New ListItem("", ""))

        For Each item As KeyValuePair(Of String, String) In list
            ddn.Items.Add(New ListItem(item.Value, item.Key))
        Next
    End Sub
End Class
