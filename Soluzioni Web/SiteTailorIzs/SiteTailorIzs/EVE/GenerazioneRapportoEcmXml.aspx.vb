Public Class GenerazioneRapportoEcmXml
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection
    Private totabilitati As Integer = 0
    Private totok As Integer = 0
    Private totko As Integer = 0

    Private Sub GenerazioneRapportoEcmXml_Init(sender As Object, e As EventArgs) Handles Me.Init
        Response.Buffer = True

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione riepilogo e popolamento variabili
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_ecm_RiassuntoValidazione"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        dbrdr.Read()
        totabilitati = dbrdr.GetInt32(0)
        totok = dbrdr.GetInt32(1)
        totko = dbrdr.GetInt32(2)
        dbrdr.Close()
        dbCmd.Dispose()

        Transformer.Transform(New XmlDocument, "Templates/EcmXml_Totali.xslt", phdRiassunto, "totabilitati", totabilitati.ToString, "totok", totok.ToString, "totko", totko.ToString)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'aggancio record evento
        If Not Page.IsPostBack Then
            Me.frmEVENTI.BoundStlSqlDataSource.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        End If
        Me.sdsEVENTI.UpdateParameters("tx_MODIFICA").DefaultValue = ContextHandler.USERNAME.ToString
        Me.sdsTFORM_EVE_SEC.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString

    End Sub

    Private Sub GenerazioneRapportoEcmXml_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub lnkGenera_Click(sender As Object, e As System.EventArgs) Handles lnkGenera.Click

        'verifico il salvataggio
        Dim saved = True

        With frmEVENTI
            Select Case .CurrentMode
                Case FormViewMode.Edit
                    .UpdateItem(False)
                    'se siamo ancora in edit il salvataggio non è riuscito
                    If .CurrentMode = FormViewMode.Edit Then saved = False
                    'aggiorno il pannello per riflettere l'errore o il salvataggio ok
                    .UpdateParentPanel()
                Case FormViewMode.Insert
                    .InsertItem(False)
                    'se siamo ancora in edit il salvataggio non è riuscito
                    If .CurrentMode = FormViewMode.Insert Then saved = False
                    'aggiorno il pannello per riflettere l'errore o il salvataggio ok
                    .UpdateParentPanel()
            End Select
        End With

        If Not saved Then Exit Sub 'il messaggio viene dal form

        'verifica che ci siano tutti i dati a livello di evento        
        Dim datiOk = True
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT id_EVENTO FROM vw_eve_EVENTI_DatiEcmXml WHERE id_EVENTO=@id_evento AND (" &
                "(ecm2_COD_ACCR is null) OR (ecm2_COD_EVE is null) OR (ecm2_COD_EDI is null) OR (ac_ACCREDITAMENTO is null) OR (ecm2_NUM_CRED is null) OR (ecm2_NUM_ORE is null) OR (id_TIPOLOGIAECMEVENTO is null) OR (ecm2_TIPO_EVE is null) OR (ecm2_COD_OBI is null) OR (ecm2_DATA_ACQ is null) OR (ecm2_TFORM_EVE is null))"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        If dbrdr.Read Then datiOk = False
        dbrdr.Close()
        dbCmd.Dispose()

        'verifica che siano specificate più tipologie formative per i corsi blended
        Dim fl_blended = False
        Dim blendedOk = False
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT id_TIPOLOGIAECMEVENTO FROM eve_EVENTI WHERE id_EVENTO = @id_evento AND id_TIPOLOGIAECMEVENTO = @id_tipologiaecmevento_blended"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@id_tipologiaecmevento_blended", SqlDbType.Int).Value = 5
        End With
        dbrdr = dbCmd.ExecuteReader
        If dbrdr.Read Then
            fl_blended = True
        End If
        dbrdr.Close()
        dbCmd.Dispose()

        If fl_blended Then
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM eve_EVENTI_TIPOLOGIEECM WHERE id_EVENTO = @id_evento"
                .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            End With
            dbrdr = dbCmd.ExecuteReader
            If dbrdr.Read Then
                blendedOk = True
            Else
                blendedOk = False
            End If
            dbrdr.Close()
            dbCmd.Dispose()
        Else
            blendedOk = True
        End If

        If Not datiOk Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "noData", "window.alert('Tutti i dati nel riquadro \'Intestazione rapporto\' sono obbligatori. Verifica i dati immessi.');", True)
            Exit Sub
        End If

        If Not blendedOk Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "noBlended", "window.alert('Per i corsi blended è necessario specificare le tipologie formative secondarie. Controlla i dati dell\'evento.');", True)
            Exit Sub
        End If

        'verifica totali
        If totabilitati = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "noPerson", "window.alert('Nessun nominativo ha ottenuto i crediti ECM. Impossibile generare il rapporto.');", True)
            Exit Sub
        End If

        'verifica scorretti
        If totko > 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "someKo", "window.alert('Esistono nominativi con dati ECM non validi. Clicca su \'Verifica dati ECM\' nel riquadro \'Riepilogo Nominativi\' per correggere le anomalie.');", True)
            Exit Sub
        End If

        'OK ci siamo
        DoReport()

    End Sub

    Private Sub DoReport()

        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_RapportoEcmXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim xReader = dbCmd.ExecuteXmlReader
        Dim xDoc As New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        Response.Clear()
        Response.ContentType = "text/xml"
        Response.AddHeader("Content-Disposition", "attachment; filename=RapportoEcmXml_" & Date.Now.ToString("dd_MM_yyyy_HH_mm_ss") & ".xml")
        'Response.Write("<?xml version=""1.0"" encoding=""utf-8""?>" & vbCrLf & xDoc.OuterXml)
        Dim isoEncoding = Encoding.GetEncoding("iso8859-1")
        Response.Write(isoEncoding.GetString(CreateXml8859_1(xDoc)))
        dbConn.Close()
        Response.End()

    End Sub

    Private Function CreateXml8859_1(xDoc As XmlDocument) As Byte()
        Dim xmlDecl = xDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", String.Empty)
        xDoc.PrependChild(xmlDecl)
        Dim mStream As New IO.MemoryStream
        Dim xWriter As New XmlTextWriter(mStream, System.Text.Encoding.GetEncoding("iso8859-1"))
        xDoc.Save(xWriter)
        xWriter.Close()
        Return mStream.ToArray
    End Function


End Class