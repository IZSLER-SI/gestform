Public Class Campo

    <XmlAttribute>
    Public Property NomeDb As String

    <XmlAttribute>
    Public Property Segnaposto As String

    <XmlAttribute>
    Public Property Descrizione As String

    <XmlAttribute>
    Public Property Tipo As TipoDato

    <XmlAttribute>
    Public Property Filtro As Boolean

    <XmlAttribute>
    Public Property Ordinamento As Boolean

    <XmlAttribute>
    Public Property Output As Boolean

    <XmlAttribute>
    Public Property TipoControllo As TipoControllo

    <XmlAttribute>
    Public Property QueryLista As String

    Public Function ExcelFormatString() As String

        Select Case Tipo
            Case TipoDato.Data
                Return "dd/MM/yyyy"
            Case TipoDato.DataOra
                Return "dd/MM/yyyy HH:mm:ss"
            Case TipoDato.Decimale
                Return "#,##0.00"
            Case TipoDato.Intero
                Return ""
            Case TipoDato.Ora
                Return "HH:mm:ss"
            Case TipoDato.Stringa
                Return ""
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
