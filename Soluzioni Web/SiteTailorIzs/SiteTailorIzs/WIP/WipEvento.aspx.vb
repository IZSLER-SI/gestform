Imports System.Web.Services

Public Class WipEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private Sub WipEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'autorizzazione
        If Not Softailor.SiteTailor.ACL.AclHelper.AclInitForPagesWithoutMasterPage(Server, Request, Response, True) Then Exit Sub


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'generazione controlli
        GeneraControlli()

    End Sub

    Private Sub GeneraControlli()

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim schedaXDoc As XmlDocument
        Dim schedaNode As XmlNode
        Dim datiXDoc As XmlDocument
        Dim datiNode As XmlNode

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'lettura dati scheda
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_WE_DatiScheda"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTOSqlInt32
            .Parameters.Add("@dt_dataoggi", SqlDbType.Date).Value = Date.Today
        End With
        xReader = dbCmd.ExecuteXmlReader
        schedaXDoc = New XmlDocument
        schedaXDoc.Load(xReader)
        xReader.Close()
        xReader.Dispose()
        dbCmd.Dispose()

        'esiste una scheda?
        If schedaXDoc.ChildNodes.Count = 0 Then
            dbConn.Close()
            dbConn.Dispose()
            phdContent.Controls.Add(New LiteralControl("<b style=""color:#ff0000;"">Per questo evento non è stata predisposta una scheda.</b>"))
            Exit Sub
        End If

        'lettura dati evento
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_WE_DatiEvento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTOSqlInt32
        End With
        xReader = dbCmd.ExecuteXmlReader
        datiXDoc = New XmlDocument
        datiXDoc.Load(xReader)
        xReader.Close()
        xReader.Dispose()
        dbCmd.Dispose()

        'trapianto
        datiNode = datiXDoc.LastChild
        schedaNode = schedaXDoc.ImportNode(datiNode, True)
        schedaXDoc.LastChild.AppendChild(schedaNode)

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        'trasfomazione
        Transformer.Transform(schedaXDoc, "WipEvento_Controls.xslt", phdContent)

    End Sub

#Region "Chiamate AJAX"

    Public Class SetValueReturnData
        Public value As String = ""
        Public updatedon As String = ""
        Public updatedby As String = ""
    End Class

    <WebMethod(True)> _
    Public Shared Function SetValue(id_SCHEDA As Integer,
                                    id_ITEM As Integer,
                                    ac_TIPODATO As String,
                                    xx_DATO As String) As SetValueReturnData

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim output As SetValueReturnData

        If Not AmIAuthorized() Then
            'non siamo autorizzati
            HttpContext.Current.Response.StatusCode = 403
            HttpContext.Current.Response.End()
            Return New SetValueReturnData
        End If

        'TRIM iniziale
        xx_DATO = xx_DATO.Trim

        'preparazione risultato
        output = New SetValueReturnData

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_SetValoreItemScheda"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_scheda", SqlDbType.Int).Value = id_SCHEDA
            .Parameters.Add("@id_item", SqlDbType.Int).Value = id_ITEM
            .Parameters.Add("@ac_tipodato", SqlDbType.NVarChar, 10).Value = ac_TIPODATO.ToLower

            Select Case ac_TIPODATO
                Case "string"
                    With .Parameters.Add("@tx_dato", SqlDbType.NVarChar, 400)
                        If xx_DATO = String.Empty Then
                            .Value = DBNull.Value
                        Else
                            .Value = xx_DATO
                        End If
                    End With
                    'restituzione valore
                    output.value = xx_DATO
                Case "int"
                    With .Parameters.Add("@ni_dato", SqlDbType.Int)
                        If xx_DATO = String.Empty Then
                            .Value = DBNull.Value
                        Else
                            .Value = CInt(xx_DATO)
                        End If
                    End With
                    'restituzione valore
                    If xx_DATO = String.Empty Then
                        output.value = ""
                    Else
                        output.value = CInt(xx_DATO).ToString
                    End If
                Case "boolean"
                    With .Parameters.Add("@fl_dato", SqlDbType.Bit)
                        If xx_DATO = String.Empty Then
                            .Value = DBNull.Value
                        Else
                            .Value = xx_DATO = "1"
                        End If
                    End With
                    'restituzione valore (0 o 1)
                    output.value = xx_DATO
                Case "date"
                    With .Parameters.Add("@dt_dato", SqlDbType.Date)
                        If xx_DATO = String.Empty Then
                            .Value = DBNull.Value
                        Else
                            .Value = Softailor.Global.ValidationUtils.ParseItalianDate(xx_DATO)
                        End If
                    End With
                    'restituzione valore
                    If xx_DATO = String.Empty Then
                        output.value = ""
                    Else
                        output.value = Softailor.Global.ValidationUtils.FormatItalianDateY4(Softailor.Global.ValidationUtils.ParseItalianDate(xx_DATO))
                    End If
                Case "money"
                    With .Parameters.Add("@mo_dato", SqlDbType.Money)
                        If xx_DATO = String.Empty Then
                            .Value = DBNull.Value
                        Else
                            .Value = Softailor.Global.ValidationUtils.ParseItalianDecimalConSeparatoreMigliaia(xx_DATO)
                        End If
                    End With
                    'restituzione valore
                    If xx_DATO = String.Empty Then
                        output.value = ""
                    Else
                        output.value = Softailor.Global.ValidationUtils.FormatItalianCurrencyConSeparatoreMigliaia(Softailor.Global.ValidationUtils.ParseItalianDecimalConSeparatoreMigliaia(xx_DATO))
                    End If
            End Select

        End With
        'esecuzione e recupero dati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        If dbRdr.IsDBNull(0) Then
            output.updatedon = ""
        Else
            output.updatedon = dbRdr.GetDateTime(0).ToString("dd/MM/yyyy HH:mm:ss", Softailor.Global.Cultures.CulturaItalian)
        End If
        If dbRdr.IsDBNull(1) Then
            output.updatedby = ""
        Else
            output.updatedby = dbRdr.GetString(1)
        End If
        dbRdr.Close()
        dbCmd.Dispose()


        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return output

    End Function

    Public Class SetWipReturnData
        Public statuscode As String = ""
        Public expiry As String = ""
        Public daystoexpiry As String = ""
        Public updatedon As String = ""
        Public updatedby As String = ""
    End Class

    <WebMethod(True)> _
    Public Shared Function SetWipOK(id_SCHEDA As Integer,
                                    id_ITEM As Integer,
                                    id_ITEM_FIGLIO As Integer) As SetWipReturnData

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim output As SetWipReturnData

        If Not AmIAuthorized() Then
            'non siamo autorizzati
            HttpContext.Current.Response.StatusCode = 403
            HttpContext.Current.Response.End()
            Return New SetWipReturnData
        End If

        'preparazione risultato
        output = New SetWipReturnData

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_SetWipOKScheda"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_scheda", SqlDbType.Int).Value = id_SCHEDA
            .Parameters.Add("@id_item", SqlDbType.Int).Value = IIf(id_ITEM = 0, DBNull.Value, id_ITEM)
            .Parameters.Add("@id_item_figlio", SqlDbType.Int).Value = IIf(id_ITEM_FIGLIO = 0, DBNull.Value, id_ITEM_FIGLIO)
        End With
        'esecuzione e recupero dati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        If dbRdr.IsDBNull(0) Then
            output.updatedon = ""
        Else
            output.updatedon = dbRdr.GetDateTime(0).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        End If
        If dbRdr.IsDBNull(1) Then
            output.updatedby = ""
        Else
            output.updatedby = dbRdr.GetString(1)
        End If
        output.expiry = dbRdr.GetDateTime(2).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        output.daystoexpiry = dbRdr.GetInt32(3).ToString
        output.statuscode = dbRdr.GetString(4)
        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return output

    End Function

    <WebMethod(True)> _
    Public Shared Function SetWipNN(id_SCHEDA As Integer,
                                    id_ITEM As Integer,
                                    id_ITEM_FIGLIO As Integer) As SetWipReturnData

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim output As SetWipReturnData

        If Not AmIAuthorized() Then
            'non siamo autorizzati
            HttpContext.Current.Response.StatusCode = 403
            HttpContext.Current.Response.End()
            Return New SetWipReturnData
        End If

        'preparazione risultato
        output = New SetWipReturnData

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_SetWipNNScheda"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_scheda", SqlDbType.Int).Value = id_SCHEDA
            .Parameters.Add("@id_item", SqlDbType.Int).Value = IIf(id_ITEM = 0, DBNull.Value, id_ITEM)
            .Parameters.Add("@id_item_figlio", SqlDbType.Int).Value = IIf(id_ITEM_FIGLIO = 0, DBNull.Value, id_ITEM_FIGLIO)
        End With
        'esecuzione e recupero dati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        If dbRdr.IsDBNull(0) Then
            output.updatedon = ""
        Else
            output.updatedon = dbRdr.GetDateTime(0).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        End If
        If dbRdr.IsDBNull(1) Then
            output.updatedby = ""
        Else
            output.updatedby = dbRdr.GetString(1)
        End If
        output.expiry = dbRdr.GetDateTime(2).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        output.daystoexpiry = dbRdr.GetInt32(3).ToString
        output.statuscode = dbRdr.GetString(4)
        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return output

    End Function

    <WebMethod(True)> _
    Public Shared Function SetWipTBC(id_SCHEDA As Integer,
                                    id_ITEM As Integer,
                                    id_ITEM_FIGLIO As Integer) As SetWipReturnData

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim output As SetWipReturnData

        If Not AmIAuthorized() Then
            'non siamo autorizzati
            HttpContext.Current.Response.StatusCode = 403
            HttpContext.Current.Response.End()
            Return New SetWipReturnData
        End If

        'preparazione risultato
        output = New SetWipReturnData

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_SetWipTBCScheda"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_scheda", SqlDbType.Int).Value = id_SCHEDA
            .Parameters.Add("@id_item", SqlDbType.Int).Value = IIf(id_ITEM = 0, DBNull.Value, id_ITEM)
            .Parameters.Add("@id_item_figlio", SqlDbType.Int).Value = IIf(id_ITEM_FIGLIO = 0, DBNull.Value, id_ITEM_FIGLIO)
        End With
        'esecuzione e recupero dati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        If dbRdr.IsDBNull(0) Then
            output.updatedon = ""
        Else
            output.updatedon = dbRdr.GetDateTime(0).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        End If
        If dbRdr.IsDBNull(1) Then
            output.updatedby = ""
        Else
            output.updatedby = dbRdr.GetString(1)
        End If
        output.expiry = dbRdr.GetDateTime(2).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        output.daystoexpiry = dbRdr.GetInt32(3).ToString
        output.statuscode = dbRdr.GetString(4)
        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return output

    End Function

    <WebMethod(True)> _
    Public Shared Function SetWipDeadline(id_SCHEDA As Integer,
                                    id_ITEM As Integer,
                                    id_ITEM_FIGLIO As Integer,
                                    dt_SCADENZA As String) As SetWipReturnData

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader
        Dim output As SetWipReturnData

        If Not AmIAuthorized() Then
            'non siamo autorizzati
            HttpContext.Current.Response.StatusCode = 403
            HttpContext.Current.Response.End()
            Return New SetWipReturnData
        End If

        'preparazione risultato
        output = New SetWipReturnData

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_SetWipDeadlineScheda"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_scheda", SqlDbType.Int).Value = id_SCHEDA
            .Parameters.Add("@id_item", SqlDbType.Int).Value = IIf(id_ITEM = 0, DBNull.Value, id_ITEM)
            .Parameters.Add("@id_item_figlio", SqlDbType.Int).Value = IIf(id_ITEM_FIGLIO = 0, DBNull.Value, id_ITEM_FIGLIO)
            .Parameters.Add("@dt_scadenza", SqlDbType.Date).Value = Softailor.Global.ValidationUtils.ParseItalianDate(dt_SCADENZA)
        End With
        'esecuzione e recupero dati
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        If dbRdr.IsDBNull(0) Then
            output.updatedon = ""
        Else
            output.updatedon = dbRdr.GetDateTime(0).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        End If
        If dbRdr.IsDBNull(1) Then
            output.updatedby = ""
        Else
            output.updatedby = dbRdr.GetString(1)
        End If
        output.expiry = dbRdr.GetDateTime(2).ToString("dd/MM/yyyy", Softailor.Global.Cultures.CulturaItalian)
        output.daystoexpiry = dbRdr.GetInt32(3).ToString
        output.statuscode = dbRdr.GetString(4)
        dbRdr.Close()
        dbCmd.Dispose()

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return output

    End Function

    Private Shared Function AmIAuthorized() As Boolean

        Dim _functionAuthorization As SiteTailorFunctionAuthorization

        'ottengo il percorso su disco della pagina corrente
        Dim myPath As String = System.Web.HttpContext.Current.Server.MapPath("~/WIP/WipEvento.aspx").ToLower

        'verifico se l'utente è autorizzato
        Try
            _functionAuthorization = ContextHandler.USERFUNC(myPath)
        Catch ex As Exception
            _functionAuthorization = Nothing
        End Try

        If _functionAuthorization IsNot Nothing Then
            If Not _functionAuthorization.Disabled Then
                Return True
            End If
        End If

        Return False

    End Function
#End Region

#Region "Determinazione scadenze"
    <WebMethod(True)> _
    Public Shared Function GetWipEvento(id_EVENTO As Integer) As String

        If Not AmIAuthorized() Then
            'non siamo autorizzati
            HttpContext.Current.Response.StatusCode = 403
            HttpContext.Current.Response.End()
            Return ""
        End If

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_wii_we_WipEventoXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = id_EVENTO
        End With

        'trasformazione
        GetWipEvento = Transformer.Transform(dbCmd, "WipEvento_Summary.xslt")

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

    End Function

#End Region

End Class