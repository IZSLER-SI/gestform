Imports Softailor.Global.XmlParser

Public Class DettaglioPersonaTipoCorso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim xDoc As XmlDocument
        Dim ac_TIPOCOBBASE As String
        Dim id_PERSONA As Integer
        Dim pNode As XmlNode
        Dim cNode As XmlNode

        'lettura parametri
        ac_TIPOCOBBASE = Request("tcb")
        id_PERSONA = CInt(Request("id"))

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_cob_dettaglioPersonaTipoCorso"
            .Parameters.Add("@ac_tipocobbase", SqlDbType.NVarChar, 8).Value = ac_TIPOCOBBASE
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = id_PERSONA
            .Parameters.Add("@dt_dataoggi", SqlDbType.Date).Value = Date.Today
        End With

        'lettura dati
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        xReader.Dispose()
        dbConn.Close()

        'titolo
        pNode = xDoc.SelectSingleNode("/root/persona")
        lblTitolo1.Text = ParseXmlString(pNode, "tx_cognome") & " " &
            ParseXmlString(pNode, "tx_nome") & " <span style=""font-weight:normal;"">(matricola:" &
            ParseXmlString(pNode, "ac_matricola") & ")"
        cNode = xDoc.SelectSingleNode("/root/tipocobbase")
        lblTitolo2.Text = ParseXmlString(cNode, "tx_tipocobbase")

        'contenuto
        Transformer.Transform(xDoc, "Templates/DettaglioPersonaTipoCorso.xslt", phdContent)

    End Sub

End Class