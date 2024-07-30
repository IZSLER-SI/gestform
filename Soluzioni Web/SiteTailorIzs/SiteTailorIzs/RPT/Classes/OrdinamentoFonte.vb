Imports Softailor.Global.XmlParser

Public Class OrdinamentoFonte
    Public Property ac_ORDINAMENTO As String
    Public Property tx_CAMPIDB As String
End Class

Public Class OrdinamentoFonteData
    Public Sub New()

    End Sub

    Public Function GetOrdinamentoFonte(ac_FONTE As String) As List(Of OrdinamentoFonte)

        'apro il file Xml
        Dim mDefRelPath = "~/RPT/Fonti/" & ac_FONTE & ".xml"
        Dim xDoc As New XmlDocument
        Dim output As New List(Of OrdinamentoFonte)

        If Not String.IsNullOrEmpty(ac_FONTE) Then
            xDoc.Load(HttpContext.Current.Server.MapPath(mDefRelPath))
            Dim nsMgr As New XmlNamespaceManager(xDoc.NameTable)
            nsMgr.AddNamespace("ns", "http://schemas.softailor.com/ReportEngine/Fonte")

            For Each ordinamentoNode As XmlNode In xDoc.SelectNodes("/ns:Fonte/ns:Ordinamenti/ns:Ordinamento", nsMgr)
                output.Add(New OrdinamentoFonte With {
                           .ac_ORDINAMENTO = ParseXmlString(ordinamentoNode, "Descrizione"),
                           .tx_CAMPIDB = ParseXmlString(ordinamentoNode, "Sql")
                       })
            Next
        End If

        Return output
    End Function
End Class
