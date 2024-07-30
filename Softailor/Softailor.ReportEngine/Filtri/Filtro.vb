<XmlRoot(Namespace:="http://schemas.softailor.com/ReportEngine/Filtro")>
Public Class Filtro

#Region "Attributi Serializzabili"

    Public Property Condizioni() As List(Of Condizione)

#End Region

#Region "Costruttori"
    Public Sub New()
        Condizioni = New List(Of Condizione)
        campiCorpo = New Dictionary(Of String, Campo)
    End Sub
#End Region

#Region "Variabili Interne"
    Private campiCorpo As Dictionary(Of String, Campo)
#End Region

#Region "Generazione Query"

    Public Function GetFilter() As String


        'se non ho condizioni > niente filtro
        If Condizioni.Count = 0 Then
            Return ""
        End If

        Dim filtro As String
        Dim cCondizione As Integer
        Dim condizione As Condizione

        filtro = "("

        For cCondizione = 0 To Condizioni.Count - 1

            condizione = Condizioni(cCondizione)

            If cCondizione > 0 Then
                filtro &= vbCrLf & vbTab & "AND"
            End If
            filtro &= condizione.GetFilter(campiCorpo(condizione.NomeCampoDb))

        Next

        filtro &= vbCrLf & ")"

        Return filtro

    End Function

#End Region

#Region "Serializzazione"
    Public Function GetXml() As String

        'serializzazione del dato
        Dim sWriter As New IO.StringWriter
        Dim xSer As XmlSerializer = New XmlSerializer(GetType(Filtro))
        xSer.Serialize(sWriter, Me)
        GetXml = sWriter.ToString
        sWriter.Close()
        sWriter.Dispose()

    End Function
#End Region

#Region "Membri condivisi"
    Public Shared Function FromXml(xmlString As String, ListaCampiCorpo As List(Of Campo)) As Filtro

        Dim newFiltro As Filtro

        'Using fReader = New IO.FileStream(fullPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
        '    Dim xSer As New XmlSerializer(GetType(Filtro))
        '    newFiltro = CType(xSer.Deserialize(fReader), Filtro)
        'End Using

        Using sReader = New IO.StringReader(xmlString)
            Dim xSer As New XmlSerializer(GetType(Filtro))
            newFiltro = CType(xSer.Deserialize(sReader), Filtro)
        End Using

        'filtro
        For Each Campo In ListaCampiCorpo
            newFiltro.campiCorpo.Add(Campo.NomeDb, Campo)
        Next

        'ritorno il filtro
        Return newFiltro

    End Function
#End Region

End Class
