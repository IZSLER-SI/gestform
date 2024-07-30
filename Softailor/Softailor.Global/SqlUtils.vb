Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes

Public Class SqlUtils

    Public Shared Function CulturaSQL() As System.Globalization.CultureInfo
        Dim cSQL As New System.Globalization.CultureInfo("en-US", False)
        cSQL.DateTimeFormat.DateSeparator = "/"
        cSQL.DateTimeFormat.TimeSeparator = ":"
        Return cSQL
    End Function

    Public Overloads Shared Function SQL_String(ByVal V As SqlString) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return "N'" & Replace(V.ToString, "'", "''") & "'"
        End If
    End Function
    Public Overloads Shared Function SQL_String(ByVal V As String) As String
        If V = "" Then
            Return "N''"
        Else
            Return "N'" & Replace(V.ToString, "'", "''") & "'"
        End If
    End Function
    Public Overloads Shared Function SQL_Int16(ByVal V As SqlInt16) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.ToString
        End If
    End Function
    Public Overloads Shared Function SQL_Int16(ByVal V As Short) As String
        Return V.ToString
    End Function
    Public Overloads Shared Function SQL_Int32(ByVal V As SqlInt32) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.ToString
        End If
    End Function
    Public Overloads Shared Function SQL_Int32(ByVal V As Integer) As String
        Return V.ToString
    End Function
    Public Overloads Shared Function SQL_Byte(ByVal V As SqlByte) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.ToString
        End If
    End Function
    Public Overloads Shared Function SQL_Byte(ByVal V As Byte) As String
        Return V.ToString
    End Function
    Public Overloads Shared Function SQL_Boolean(ByVal V As SqlBoolean) As String
        If V.IsNull Then
            Return "Null"
        Else
            If V.Value Then Return "1" Else Return "0"
        End If
    End Function
    Public Overloads Shared Function SQL_Boolean(ByVal V As Boolean) As String
        If V Then Return "1" Else Return "0"
    End Function
    Public Overloads Shared Function SQL_Decimal(ByVal V As SqlDecimal) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.Value.ToString(CulturaSQL)
        End If
    End Function
    Public Overloads Shared Function SQL_Decimal(ByVal V As Decimal) As String
        Return V.ToString(CulturaSQL)
    End Function
    Public Overloads Shared Function SQL_Money(ByVal V As SqlMoney) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.Value.ToString(CulturaSQL)
        End If
    End Function
    Public Overloads Shared Function SQL_Money(ByVal V As Decimal) As String
        Return V.ToString(CulturaSQL)
    End Function
    Public Overloads Shared Function SQL_Double(ByVal V As SqlDouble) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.Value.ToString(CulturaSQL)
        End If
    End Function
    Public Overloads Shared Function SQL_Double(ByVal V As Double) As String
        Return V.ToString(CulturaSQL)
    End Function
    Public Overloads Shared Function SQL_Single(ByVal V As SqlSingle) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return V.Value.ToString(CulturaSQL)
        End If
    End Function
    Public Overloads Shared Function SQL_Single(ByVal V As Single) As String
        Return V.ToString(CulturaSQL)
    End Function

    Public Overloads Shared Function SQL_DateTime(ByVal V As SqlDateTime) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return "'" & V.Value.ToString("yyyyMMdd HH:mm:ss:fff", CulturaSQL) & "'"
        End If
    End Function
    Public Overloads Shared Function SQL_DateTime(ByVal V As DateTime) As String
        'usiamo il formato '20030112 12:10:00:000'
        Return "'" & V.ToString("yyyyMMdd HH:mm:ss:fff", CulturaSQL) & "'"
    End Function

    Public Overloads Shared Function SQL_Date(ByVal V As SqlDateTime) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return "'" & V.Value.ToString("yyyyMMdd", CulturaSQL) & "'"
        End If
    End Function
    Public Overloads Shared Function SQL_Date(ByVal V As DateTime) As String
        'usiamo il formato '20030112 12:10:00:000'
        Return "'" & V.ToString("yyyyMMdd", CulturaSQL) & "'"
    End Function

    Public Overloads Shared Function SQL_Time(ByVal V As SqlDateTime) As String
        If V.IsNull Then
            Return "Null"
        Else
            Return "'" & V.Value.ToString("18991230 HH:mm:ss", CulturaSQL) & "'"
        End If
    End Function
    Public Overloads Shared Function SQL_Time(ByVal V As DateTime) As String
        'usiamo il formato '20030112 12:10:00:000'
        Return "'" & V.ToString("18991230 HH:mm:ss", CulturaSQL) & "'"
    End Function


    Public Function SQL_FromAnyType(ByVal V As Object) As String
        If TypeOf V Is SqlString Then
            Return SQL_String(CType(V, SqlString))
        ElseIf TypeOf V Is String Then
            Return SQL_String(CType(V, String))
        ElseIf TypeOf V Is SqlInt16 Then
            Return SQL_Int16(CType(V, SqlInt16))
        ElseIf TypeOf V Is Short Then
            Return SQL_Int16(CType(V, Short))
        ElseIf TypeOf V Is SqlInt32 Then
            Return SQL_Int32(CType(V, SqlInt32))
        ElseIf TypeOf V Is Integer Then
            Return SQL_Int32(CType(V, Integer))
        ElseIf TypeOf V Is SqlByte Then
            Return SQL_Byte(CType(V, SqlByte))
        ElseIf TypeOf V Is Byte Then
            Return SQL_Byte(CType(V, Byte))
        ElseIf TypeOf V Is SqlBoolean Then
            Return SQL_Boolean(CType(V, SqlBoolean))
        ElseIf TypeOf V Is Boolean Then
            Return SQL_Boolean(CType(V, Boolean))
        ElseIf TypeOf V Is SqlMoney Then
            Return SQL_Money(CType(V, SqlMoney))
        ElseIf TypeOf V Is SqlDecimal Then
            Return SQL_Decimal(CType(V, SqlDecimal))
        ElseIf TypeOf V Is Decimal Then
            Return SQL_Decimal(CType(V, Decimal))
        ElseIf TypeOf V Is SqlDouble Then
            Return SQL_Double(CType(V, SqlDouble))
        ElseIf TypeOf V Is Double Then
            Return SQL_Double(CType(V, Double))
        ElseIf TypeOf V Is SqlSingle Then
            Return SQL_Single(CType(V, SqlSingle))
        ElseIf TypeOf V Is Single Then
            Return SQL_Single(CType(V, Single))
        ElseIf TypeOf V Is SqlDateTime Then
            Return SQL_DateTime(CType(V, SqlDateTime))
        ElseIf TypeOf V Is DateTime Then
            Return SQL_DateTime(CType(V, DateTime))
        Else
            Throw New Exception("Invalid datatype (Sql_FromAnyType)")
        End If
    End Function

    Public Overloads Shared Function Nz(ByVal value As SqlString, Optional ByVal valueIfNull As String = "") As String
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function
    Public Overloads Shared Function Nz(ByVal value As SqlInt32, Optional ByVal valueIfNull As Int32 = 0) As Int32
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function
    Public Overloads Shared Function Nz(ByVal value As SqlInt16, Optional ByVal valueIfNull As Int16 = 0) As Int16
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function
    Public Overloads Shared Function Nz(ByVal value As SqlMoney, Optional ByVal valueIfNull As Decimal = 0D) As Decimal
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function
    Public Overloads Shared Function Nz(ByVal value As SqlDateTime, ByVal valueIfNull As Date) As Date
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function
    Public Overloads Shared Function Nz(ByVal value As SqlSingle, Optional ByVal valueIfNull As Single = 0) As Single
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function
    Public Overloads Shared Function Nz(ByVal value As SqlBoolean, Optional ByVal valueIfNull As Boolean = False) As Boolean
        If value.IsNull Then
            Return valueIfNull
        Else
            Return value.Value
        End If
    End Function

    Public Shared Function EnumerateFields(ByVal dbConn As SqlConnection, ByVal query As String) As List(Of String)

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim sTable As DataTable
        Dim fieldList As New List(Of String)

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SET FMTONLY ON;" & query & ";SET FMTONLY OFF"
        End With
        dbRdr = dbCmd.ExecuteReader
        sTable = dbRdr.GetSchemaTable()
        dbRdr.Close()
        dbCmd.Dispose()

        For Each row As DataRow In sTable.Rows
            fieldList.Add(CStr(row.Item(0)))
        Next

        sTable.Dispose()

        Return fieldList

    End Function

    Public Shared Function EmptyToDbNull(ByVal s As String) As SqlString
        If s = "" Then Return SqlString.Null Else Return New SqlString(s)
    End Function

    Public Overloads Shared Function ZeroToDbNull(ByVal i As Integer) As SqlInt32
        If i = 0 Then Return SqlInt32.Null Else Return New SqlInt32(i)
    End Function

    Public Overloads Shared Function ZeroToDbNull(ByVal d As Decimal) As SqlMoney
        If d = 0 Then Return SqlMoney.Null Else Return New SqlMoney(d)
    End Function

    Public Shared Function ZeroToEmptyString(ByVal i As Integer) As String
        If i = 0 Then Return "" Else Return i.ToString
    End Function

End Class
