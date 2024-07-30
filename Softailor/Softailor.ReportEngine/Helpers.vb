Imports Softailor.Global.SqlUtils
Imports System.Data.SqlTypes
Imports System.Globalization

Public Class Helpers

    Public Shared Function SqlValue(valore As String, tipo As TipoDato) As String

        Select Case tipo
            Case TipoDato.Intero
                Return SQL_Int32(New SqlInt32(CInt(valore)))
            Case TipoDato.Stringa
                Return SQL_String(New SqlString(valore))
            Case Else
                Throw New NotImplementedException

        End Select

    End Function

    Public Overloads Shared Function TextValue_MailMerge(value As Object, campo As Campo, cIta As CultureInfo) As String

        Return TextValue_MailMerge(value, campo.Tipo, cIta)

    End Function

    Public Overloads Shared Function TextValue_MailMerge(value As Object, tipoDato As TipoDato, cIta As CultureInfo) As String

        If IsDBNull(value) Then
            Return ""
        Else
            Select Case tipoDato
                Case tipoDato.Stringa
                    Return value.ToString
                Case tipoDato.Intero
                    Return CInt(value).ToString
                Case tipoDato.Decimale
                    Return CDec(value).ToString("#,##0.00", cIta)
                Case tipoDato.Data
                    Return CDate(value).ToString("dd/MM/yyyy", cIta)
                Case tipoDato.DataOra
                    Return CDate(value).ToString("dd/MM/yyyy HH:mm:ss", cIta)
                Case tipoDato.Ora
                    Return CDate(value).ToString("HH:mm:ss", cIta)
                Case Else
                    Throw New NotImplementedException
            End Select
        End If

    End Function

    Public Shared Function TextValue_EditorFiltro(value As Object, campo As Campo, cIta As CultureInfo) As String

        If IsDBNull(value) Then
            Return ""
        Else
            Select Case campo.Tipo
                Case TipoDato.Stringa
                    Return value.ToString
                Case TipoDato.Intero
                    Return CInt(value).ToString
                Case TipoDato.Decimale
                    Return CDec(value).ToString("#0.####", cIta)
                Case TipoDato.Data
                    Return CDate(value).ToString("dd/MM/yyyy", cIta)
                Case TipoDato.DataOra
                    Return CDate(value).ToString("dd/MM/yyyy HH:mm:ss", cIta)
                Case TipoDato.Ora
                    Return CDate(value).ToString("HH:mm:ss", cIta)
                Case Else
                    Throw New NotImplementedException
            End Select
        End If

    End Function

    Public Shared Function XmlValue(s As String, campo As Campo, cIta As CultureInfo, cSql As CultureInfo) As String

        Select Case campo.Tipo
            Case TipoDato.Stringa
                Return s
            Case TipoDato.Intero
                Return Integer.Parse(s).ToString
            Case TipoDato.Decimale
                Return Decimal.Parse(s,
                                     Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowLeadingSign,
                                     cIta).ToString("#0.####", cSQL)
            Case TipoDato.Data
                Return Date.ParseExact(s, "dd/MM/yyyy", cIta, DateTimeStyles.None).ToString("yyyyMMdd", cSql)
            Case TipoDato.DataOra
                Return Date.ParseExact(s, "dd/MM/yyyy HH:mm:ss", cIta, DateTimeStyles.None).ToString("yyyyMMddHHmmss", cSql)
            Case TipoDato.Ora
                Return Date.ParseExact(s, "HH:mm:ss", cIta, DateTimeStyles.None).ToString("HHmmss", cSql)
            Case Else
                Throw New NotImplementedException
        End Select

        Return ""

    End Function

    Public Shared Function XmlToTypedValue(s As String, campo As Campo, cSql As CultureInfo) As Object

        Select Case campo.Tipo
            Case TipoDato.Stringa
                Return s
            Case TipoDato.Intero
                Return Integer.Parse(s)
            Case TipoDato.Decimale
                Return Decimal.Parse(s,
                                     Globalization.NumberStyles.AllowDecimalPoint Or Globalization.NumberStyles.AllowLeadingSign,
                                     cSql)
            Case TipoDato.Data
                Return Date.ParseExact(s, "yyyyMMdd", cSql, DateTimeStyles.None)
            Case TipoDato.DataOra
                Return Date.ParseExact(s, "yyyyMMddHHmmss", cSql, DateTimeStyles.None)
            Case TipoDato.Ora
                Return Date.ParseExact(s, "HHmmss", cSql, DateTimeStyles.None)
            Case Else
                Throw New NotImplementedException
        End Select

        Return ""

    End Function

    Public Shared Function XmlToText(s As String, campo As Campo, cIta As CultureInfo, cSql As CultureInfo) As String

        Return TextValue_EditorFiltro(XmlToTypedValue(s, campo, cSql), campo, cIta)

    End Function
End Class
