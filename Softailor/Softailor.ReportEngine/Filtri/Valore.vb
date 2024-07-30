Imports Softailor.Global.ValidationUtils
Imports Softailor.Global.SqlUtils

Public Class Valore

    <XmlAttribute>
    Public Property v1 As String

    <XmlAttribute>
    Public Property v2 As String

    Public Function SqlValue(tipo As TipoDato) As String

        Select Case tipo

            Case TipoDato.Data
                'formato: yyyyMMdd
                Dim vData = Date.ParseExact(v1, "yyyyMMdd", CulturaSQL)
                Return "'" & vData.ToString("yyyyMMdd", CulturaSQL) & "'"
            Case TipoDato.Ora
                'formato: HHmmss
                Dim vOra = Date.ParseExact(v1, "HHmmss", CulturaSQL)
                Return "'" & vOra.ToString("HH:mm:ss", CulturaSQL) & "'"
            Case TipoDato.Intero
                Dim vIntero = Int32.Parse(v1)
                Return vIntero.ToString
            Case TipoDato.Stringa
                Return "'" & Replace(v1, "'", "''") & "'"
            Case TipoDato.Decimale
                'con punto, senza sep migliaia
                Dim vDecimal = Decimal.Parse(v1, CulturaSQL)
                Return vDecimal.ToString(CulturaSQL)
            Case TipoDato.DataOra
                'formato: yyyyMMddHHmmss
                Dim vDataOra = Date.ParseExact(v1, "yyyyMMddHHmmss", CulturaSQL)
                Return "'" & vDataOra.ToString("yyyyMMdd HH:mm:ss", CulturaSQL) & "'"
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

    Public Function SqlValue2(tipo As TipoDato) As String

        Select Case tipo

            Case TipoDato.Data
                'formato: yyyyMMdd
                Dim vData = Date.ParseExact(v2, "yyyyMMdd", CulturaSQL)
                Return "'" & vData.ToString("yyyyMMdd", CulturaSQL) & "'"
            Case TipoDato.Ora
                'formato: HHmmss
                Dim vOra = Date.ParseExact(v2, "HHmmss", CulturaSQL)
                Return "'" & vOra.ToString("HH:mm:ss", CulturaSQL) & "'"
            Case TipoDato.Intero
                Dim vIntero = Int32.Parse(v2)
                Return vIntero.ToString
            Case TipoDato.Stringa
                Return "'" & Replace(v2, "'", "''") & "'"
            Case TipoDato.Decimale
                'con punto, senza sep migliaia
                Dim vDecimal = Decimal.Parse(v2, CulturaSQL)
                Return vDecimal.ToString(CulturaSQL)
            Case TipoDato.DataOra
                'formato: yyyyMMddHHmmss
                Dim vDataOra = Date.ParseExact(v2, "yyyyMMddHHmmss", CulturaSQL)
                Return "'" & vDataOra.ToString("yyyyMMdd HH:mm:ss", CulturaSQL) & "'"
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

End Class
