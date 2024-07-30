Imports Novacode
Imports System.Data.SqlClient
Imports OfficeOpenXml
Imports Softailor.Global.SqlUtils
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core

<XmlRoot(Namespace:="http://schemas.softailor.com/ReportEngine/Fonte")>
Public Class Fonte

#Region "enums e costanti"

    Public Enum TipoModello
        Word
        Excel
    End Enum

    Private Const phpre = "%%"
    Private Const phpost = "%%"
    Private Const phd_data_stampa_num = "%%DATA_STAMPA_NUM%%"
    Private Const phd_data_stampa_testo = "%%DATA_STAMPA_TESTO%%"
    Private Const phd_ora_stampa = "%%ORA_STAMPA%%"
    Private Const phd_n_riga = "%%N_RIGA%%"

#End Region

#Region "Attributi Serializzabili"
    <XmlAttribute>
    Public Property Codice As String

    <XmlAttribute>
    Public Property Descrizione As String

    <XmlAttribute>
    Public Property VistaIntestazione As String

    <XmlAttribute>
    Public Property VistaCorpo As String

    <XmlAttribute>
    Public Property ChiaveIntestazione As String

    <XmlAttribute>
    Public Property TipoChiaveIntestazione As TipoDato

    <XmlAttribute>
    Public Property ChiaveCorpo As String

    <XmlAttribute>
    Public Property TipoChiaveCorpo As TipoDato

    <XmlAttribute>
    Public Property CampoFiltroBase As String

    <XmlAttribute>
    Public Property CampoMail As String

    <XmlAttribute>
    Public Property TipoDatoFiltroBase As TipoDato

    Public Property CampiLista As List(Of CampoLista)

    Public Property CampiIntestazione As List(Of Campo)

    Public Property CampiCorpo As List(Of Campo)

    Public Property Ordinamenti As List(Of Ordinamento)

#End Region

#Region "Variabili interne"

    Private queryIntestazione As String
    Private queryCorpo As String
    Private queryLista As String

    Private dstIntestazione As DataSet
    Private dstCorpo As DataSet
    Private dstLista As DataSet
    Private xmlLista As System.Xml.XmlDocument

    Private CampiIntestazionePresenti As List(Of Campo)
    Private CampiCorpoPresenti As List(Of Campo)

#End Region

#Region "Classi di servizio"
    Public Class DatiFiltroOrdinamento

        Public Enum TipiOrdinamento
            Nessuno
            Standard
            Personalizzato
        End Enum

        Public TipoOrdinamento As TipiOrdinamento
        Public codOrdinamentoStandard As String
        Public campoOrdPers1 As String
        Public campoOrdPers2 As String
        Public campoOrdPers3 As String
        Public campoOrdPers4 As String
        Public campoOrdPers5 As String
        Public filtro As Filtro

    End Class
#End Region

#Region "Costruttori"
    Public Sub New()
        Me.CampiLista = New List(Of CampoLista)
        Me.CampiIntestazione = New List(Of Campo)
        Me.CampiCorpo = New List(Of Campo)
        Me.Ordinamenti = New List(Of Ordinamento)
    End Sub
#End Region

#Region "Verifica correttezza campi"

    Public Function SovrapposizioneCampi() As Boolean

        Dim listaHdr As New List(Of String)
        Dim listaBdy As New List(Of String)

        If CampiIntestazione IsNot Nothing Then
            For Each Campo In CampiIntestazione
                listaHdr.Add(Campo.Segnaposto)
            Next
        End If

        If CampiCorpo IsNot Nothing Then
            For Each Campo In CampiCorpo
                listaBdy.Add(Campo.Segnaposto)
            Next
        End If

        Return listaHdr.Intersect(listaBdy).Count > 0

    End Function

#End Region

#Region "Costruzione Query"

    Private Sub GeneraQueryIntestazione(valoreFiltroBase As String)

        If String.IsNullOrEmpty(VistaIntestazione) Then
            queryIntestazione = ""
            Exit Sub
        End If

        Dim listaSelect As String
        Dim isFirst As Boolean

        'lista select
        listaSelect = ""
        isFirst = True
        For Each c As Campo In CampiIntestazione
            If c.Output Then
                If isFirst Then
                    isFirst = False
                Else
                    listaSelect &= ", "
                End If
                listaSelect &= c.NomeDb
                isFirst = False
            End If
        Next

        queryIntestazione = "SELECT " &
            listaSelect &
            " FROM " &
            VistaIntestazione

        If Not String.IsNullOrEmpty(CampoFiltroBase) Then

            queryIntestazione &= " WHERE " &
                CampoFiltroBase &
                " = " &
                Helpers.SqlValue(valoreFiltroBase, TipoDatoFiltroBase)

        End If

    End Sub

    Private Sub GeneraQueryCorpo(datiFO As DatiFiltroOrdinamento, valoreFiltroBase As String)

        If String.IsNullOrEmpty(VistaCorpo) Then
            queryCorpo = ""
            Exit Sub
        End If

        Dim listaSelect As String
        Dim isFirst As Boolean

        'lista select
        listaSelect = ""
        isFirst = True
        For Each c As Campo In CampiCorpo
            If c.Output Then
                If isFirst Then
                    isFirst = False
                Else
                    listaSelect &= ", "
                End If
                listaSelect &= c.NomeDb
                isFirst = False
            End If
        Next

        queryCorpo = "SELECT " &
            listaSelect &
            " FROM " &
            VistaCorpo

        If Not String.IsNullOrEmpty(CampoFiltroBase) Then
            queryCorpo &= " WHERE " &
                CampoFiltroBase &
                " = " &
                Helpers.SqlValue(valoreFiltroBase, TipoDatoFiltroBase)
            If datiFO.filtro IsNot Nothing Then
                If datiFO.filtro.Condizioni.Count > 0 Then
                    queryCorpo &= " AND " & datiFO.filtro.GetFilter
                End If
            End If
        Else
            If datiFO.filtro IsNot Nothing Then
                If datiFO.filtro.Condizioni.Count > 0 Then
                    queryCorpo &= " WHERE " & datiFO.filtro.GetFilter
                End If
            End If
        End If

        'ordinamento
        Select Case datiFO.TipoOrdinamento
            Case DatiFiltroOrdinamento.TipiOrdinamento.Standard
                For Each ord In Ordinamenti
                    If ord.Descrizione = datiFO.codOrdinamentoStandard Then
                        queryCorpo &= " ORDER BY " & ord.Sql
                        Exit For
                    End If
                Next
            Case DatiFiltroOrdinamento.TipiOrdinamento.Personalizzato
                Dim listaOrder = ""
                If datiFO.campoOrdPers1 <> String.Empty Then listaOrder &= datiFO.campoOrdPers1 & ", "
                If datiFO.campoOrdPers2 <> String.Empty Then listaOrder &= datiFO.campoOrdPers2 & ", "
                If datiFO.campoOrdPers3 <> String.Empty Then listaOrder &= datiFO.campoOrdPers3 & ", "
                If datiFO.campoOrdPers4 <> String.Empty Then listaOrder &= datiFO.campoOrdPers4 & ", "
                If datiFO.campoOrdPers5 <> String.Empty Then listaOrder &= datiFO.campoOrdPers5 & ", "
                If listaOrder <> String.Empty Then
                    queryCorpo &= " ORDER BY " & Mid(listaOrder, 1, Len(listaOrder) - 2)
                End If
        End Select
    End Sub

    Private Sub GeneraQueryCorpo_ItemSpecifici(valoreFiltroBase As String, valoriChiave As List(Of String))

        If String.IsNullOrEmpty(VistaCorpo) Then
            queryCorpo = ""
            Exit Sub
        End If

        Dim listaSelect As String
        Dim isFirst As Boolean
        Dim listaChiavi As String

        'lista select
        listaSelect = ""
        isFirst = True
        For Each c As Campo In CampiCorpo
            If c.Output Then
                If isFirst Then
                    isFirst = False
                Else
                    listaSelect &= ", "
                End If
                listaSelect &= c.NomeDb
                isFirst = False
            End If
        Next

        queryCorpo = "SELECT " &
            listaSelect &
            " FROM " &
            VistaCorpo &
            " WHERE " &
            ChiaveCorpo &
            " IN ("

        listaChiavi = ""
        Select Case TipoChiaveCorpo
            Case TipoDato.Intero
                For Each valoreChiave In valoriChiave
                    listaChiavi &= SQL_Int32(CInt(valoreChiave)) & ", "
                Next
            Case TipoDato.Stringa
                For Each valoreChiave In valoriChiave
                    listaChiavi &= SQL_String(valoreChiave) & ", "
                Next
            Case Else
                Throw New NotImplementedException
        End Select

        queryCorpo &= Left(listaChiavi, Len(listaChiavi) - 2) & ")"

    End Sub

    Private Sub GeneraQueryLista(datiFO As DatiFiltroOrdinamento, valoreFiltroBase As String)

        If String.IsNullOrEmpty(VistaCorpo) Then
            queryLista = ""
            Exit Sub
        End If

        Dim listaSelect As String

        'lista select
        listaSelect = ChiaveCorpo & ", " & CampoMail

        For Each c As CampoLista In CampiLista
            listaSelect &= ", " & c.NomeDb
        Next

        queryLista = "SELECT " &
            listaSelect &
            " FROM " &
            VistaCorpo

        If Not String.IsNullOrEmpty(CampoFiltroBase) Then
            queryLista &= " WHERE " &
                CampoFiltroBase &
                " = " &
                Helpers.SqlValue(valoreFiltroBase, TipoDatoFiltroBase)
            If datiFO.filtro IsNot Nothing Then
                If datiFO.filtro.Condizioni.Count > 0 Then
                    queryLista &= " AND " & datiFO.filtro.GetFilter
                End If
            End If
        Else
            If datiFO.filtro IsNot Nothing Then
                If datiFO.filtro.Condizioni.Count > 0 Then
                    queryLista &= " WHERE " & datiFO.filtro.GetFilter
                End If
            End If
        End If

        'ordinamento
        Select Case datiFO.TipoOrdinamento
            Case DatiFiltroOrdinamento.TipiOrdinamento.Standard
                For Each ord In Ordinamenti
                    If ord.Descrizione = datiFO.codOrdinamentoStandard Then
                        queryLista &= " ORDER BY " & ord.Sql
                        Exit For
                    End If
                Next
            Case DatiFiltroOrdinamento.TipiOrdinamento.Personalizzato
                Dim listaOrder = ""
                'misura di sicurezza per evitare SQL injections
                If datiFO.campoOrdPers1 <> String.Empty Then If EsisteCampoCorpo(datiFO.campoOrdPers1) Then listaOrder &= datiFO.campoOrdPers1 & ", "
                If datiFO.campoOrdPers2 <> String.Empty Then If EsisteCampoCorpo(datiFO.campoOrdPers2) Then listaOrder &= datiFO.campoOrdPers2 & ", "
                If datiFO.campoOrdPers3 <> String.Empty Then If EsisteCampoCorpo(datiFO.campoOrdPers3) Then listaOrder &= datiFO.campoOrdPers3 & ", "
                If datiFO.campoOrdPers4 <> String.Empty Then If EsisteCampoCorpo(datiFO.campoOrdPers4) Then listaOrder &= datiFO.campoOrdPers4 & ", "
                If datiFO.campoOrdPers5 <> String.Empty Then If EsisteCampoCorpo(datiFO.campoOrdPers5) Then listaOrder &= datiFO.campoOrdPers5 & ", "
                If listaOrder <> String.Empty Then
                    queryLista &= " ORDER BY " & Mid(listaOrder, 1, Len(listaOrder) - 2)
                End If
        End Select
    End Sub

    Private Function EsisteCampoCorpo(nomeDb As String) As Boolean

        Dim found = False
        For Each c In CampiCorpo
            If c.NomeDb = nomeDb Then
                found = True
                Exit For
            End If
        Next

        Return found

    End Function

#End Region

#Region "Lettura Dati da DB"

    Private Sub GeneraDatasetIntestazioneCorpo(dbConn As SqlConnection, datiFO As DatiFiltroOrdinamento, valoreFiltroBase As String)

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        'generazione query
        GeneraQueryIntestazione(valoreFiltroBase)
        GeneraQueryCorpo(datiFO, valoreFiltroBase)

        'intestazione (se c'è)
        If Not String.IsNullOrEmpty(queryIntestazione) Then
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = queryIntestazione
            End With
            dbDad = New SqlDataAdapter(dbCmd)
            dstIntestazione = New DataSet
            dbDad.Fill(dstIntestazione)
            dbDad.Dispose()
            dbCmd.Dispose()
        End If

        'corpo (se c'è)
        If Not String.IsNullOrEmpty(queryCorpo) Then
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = queryCorpo
            End With
            dbDad = New SqlDataAdapter(dbCmd)
            dstCorpo = New DataSet
            dbDad.Fill(dstCorpo)
            dbDad.Dispose()
            dbCmd.Dispose()
        End If

    End Sub

    Private Sub GeneraDatasetIntestazioneCorpo_ItemSpecifici(dbConn As SqlConnection, valoreFiltroBase As String, valoriChiave As List(Of String))

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        'generazione query
        GeneraQueryIntestazione(valoreFiltroBase)
        GeneraQueryCorpo_ItemSpecifici(valoreFiltroBase, valoriChiave)

        'intestazione (se c'è)
        If Not String.IsNullOrEmpty(queryIntestazione) Then
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = queryIntestazione
            End With
            dbDad = New SqlDataAdapter(dbCmd)
            dstIntestazione = New DataSet
            dbDad.Fill(dstIntestazione)
            dbDad.Dispose()
            dbCmd.Dispose()
        End If

        'corpo (se c'è)
        If Not String.IsNullOrEmpty(queryCorpo) Then
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = queryCorpo
            End With
            dbDad = New SqlDataAdapter(dbCmd)
            dstCorpo = New DataSet
            dbDad.Fill(dstCorpo)
            dbDad.Dispose()
            dbCmd.Dispose()
        End If

    End Sub

    Private Sub GeneraDatasetLista(dbConn As SqlConnection, datiFO As DatiFiltroOrdinamento, valoreFiltroBase As String)

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        'generazione query
        GeneraQueryLista(datiFO, valoreFiltroBase)

        'lista
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = queryLista
        End With
        dbDad = New SqlDataAdapter(dbCmd)
        dstLista = New DataSet
        dbDad.Fill(dstLista)
        dbDad.Dispose()
        dbCmd.Dispose()

    End Sub

#End Region

#Region "Generazione XML Lista"

    Public Sub GeneraXmlLista(dbConn As SqlConnection,
                              datiFO As DatiFiltroOrdinamento,
                              valoreFiltroBase As String)

        Dim row As DataRow
        Dim cIta = Softailor.Global.Cultures.CulturaItalian

        'genero il dataset
        GeneraDatasetLista(dbConn, datiFO, valoreFiltroBase)

        'generazione documento XML
        Using mStream = New IO.MemoryStream
            Using xWriter = New XmlTextWriter(mStream, System.Text.Encoding.UTF8)
                xWriter.WriteStartDocument()
                xWriter.WriteStartElement("root")

                'campi
                For Each campo As CampoLista In Me.CampiLista
                    xWriter.WriteStartElement("field")
                    xWriter.WriteAttributeString("name", campo.NomeDb)
                    xWriter.WriteAttributeString("desc", campo.Descrizione)
                    xWriter.WriteEndElement()
                Next

                'contenuto
                For Each row In dstLista.Tables(0).Rows
                    xWriter.WriteStartElement("item")

                    'identificatore
                    Select Case TipoChiaveCorpo
                        Case TipoDato.Intero, TipoDato.Stringa
                            xWriter.WriteAttributeString("key", row(ChiaveCorpo).ToString)
                        Case Else
                            Throw New NotImplementedException
                    End Select

                    'e-mail
                    If Not row.IsNull(CampoMail) Then xWriter.WriteAttributeString("mail", row(CampoMail).ToString)

                    'campi
                    For Each campo As CampoLista In Me.CampiLista
                        If Not row.IsNull(campo.NomeDb) Then
                            xWriter.WriteAttributeString(campo.NomeDb, Helpers.TextValue_MailMerge(row(campo.NomeDb), campo.Tipo, cIta))
                        End If
                    Next

                    xWriter.WriteEndElement()
                Next

                xWriter.WriteEndElement()
                xWriter.WriteEndDocument()
                xWriter.Flush()
                mStream.Position = 0
                xmlLista = New System.Xml.XmlDocument
                xmlLista.Load(mStream)

            End Using

        End Using

    End Sub

    Public Function GetXmlLista() As System.Xml.XmlDocument

        Return xmlLista

    End Function

#End Region

#Region "Generazione Files"

    Public Function GeneraFile(dbConn As SqlConnection,
                                   valoreFiltroBase As String,
                                   tipoModello As TipoModello,
                                   modello As IO.MemoryStream,
                                   datiFO As DatiFiltroOrdinamento,
                                   CheckFileMultipli As Boolean
                                   ) As IO.MemoryStream

        Select Case tipoModello
            Case Fonte.TipoModello.Word
                If CheckFileMultipli Then
                    Return GeneraFileWordSplit(dbConn, valoreFiltroBase, modello, datiFO)
                End If
                If Not CheckFileMultipli Then
                    Return GeneraFileWord(dbConn, valoreFiltroBase, modello, datiFO)
                End If
            Case Fonte.TipoModello.Excel
                Return GeneraFileExcel(dbConn, valoreFiltroBase, modello, datiFO)
            Case Else
                Throw New NotImplementedException
        End Select

    End Function

    Private Function GeneraFileWord(
                                   dbConn As SqlConnection,
                                   valoreFiltroBase As String,
                                   modello As IO.MemoryStream,
                                   datiFO As DatiFiltroOrdinamento) As IO.MemoryStream

        Dim valoriIntestazione As Dictionary(Of String, String)
        Dim valoriCorpo As Dictionary(Of String, String)
        Dim rigaIntestazione As DataRow
        Dim rigaCorpo As DataRow
        Dim campo As Campo
        Dim cIta = Softailor.Global.Cultures.CulturaItalian
        Dim docOrig As DocX
        Dim docDest As DocX
        Dim mStreamDest As IO.MemoryStream
        Dim i As Integer
        Dim curRecord As Integer
        Dim totRecord As Integer

        'generazione dataset
        GeneraDatasetIntestazioneCorpo(dbConn, datiFO, valoreFiltroBase)

        'prima apertura del documento di origine per determinare quali campi sono presenti
        modello.Position = 0
        docOrig = DocX.Load(modello)

        CampiIntestazionePresenti = New List(Of Campo)
        CampiCorpoPresenti = New List(Of Campo)

        If Not String.IsNullOrEmpty(queryIntestazione) Then
            For Each campo In CampiIntestazione
                If campo.Output Then
                    If docOrig.FindAll(phpre & campo.Segnaposto & phpost).Count > 0 Then
                        CampiIntestazionePresenti.Add(campo)
                    End If
                End If
            Next
        End If
        If Not String.IsNullOrEmpty(queryCorpo) Then
            For Each campo In CampiCorpo
                If campo.Output Then
                    If docOrig.FindAll(phpre & campo.Segnaposto & phpost).Count > 0 Then
                        CampiCorpoPresenti.Add(campo)
                    End If
                End If
            Next
        End If

        docOrig.Dispose()

        'istanzio il documento di destinazione
        'creando una copia esatta di quello di origine
        mStreamDest = New IO.MemoryStream
        modello.Position = 0
        modello.CopyTo(mStreamDest)
        mStreamDest.Position = 0
        docDest = DocX.Load(mStreamDest)

        'pulisco tutti i paragrafi e le tabelle nel documento di destinazione
        i = 0
        Do While (docDest.Tables.Count > 0) And i < 500
            Dim tbl = docDest.Tables(0)
            tbl.Remove()
            i += 1
        Loop
        i = 0
        Do While (docDest.Paragraphs.Count > 0) And i < 500
            Dim prg = docDest.Paragraphs(0)
            prg.Remove(False)
            i += 1
        Loop

        'preparo i valori dell'intestazione
        valoriIntestazione = New Dictionary(Of String, String)
        If Not String.IsNullOrEmpty(queryIntestazione) Then
            rigaIntestazione = dstIntestazione.Tables(0).Rows(0)
            'aggiunta campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
            For Each campo In CampiIntestazionePresenti
                valoriIntestazione.Add(
                    phpre & campo.Segnaposto & phpost,
                    Helpers.TextValue_MailMerge(rigaIntestazione(campo.NomeDb), campo, cIta))
            Next
        Else
            'aggiungo SOLO campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
        End If

        'ciclo sul corpo
        If Not String.IsNullOrEmpty(queryCorpo) Then
            curRecord = 1
            totRecord = dstCorpo.Tables(0).Rows.Count
            valoriCorpo = New Dictionary(Of String, String)
            For Each rigaCorpo In dstCorpo.Tables(0).Rows
                valoriCorpo.Clear()
                'numero di riga
                valoriCorpo.Add(phd_n_riga, curRecord.ToString)
                For Each campo In CampiCorpoPresenti
                    valoriCorpo.Add(
                        phpre & campo.Segnaposto & phpost,
                        Helpers.TextValue_MailMerge(rigaCorpo(campo.NomeDb), campo, cIta))
                Next

                'ri-apro il doc di origine
                modello.Position = 0
                docOrig = DocX.Load(modello)

                'sostituzione segnaposto nel doc di origine
                For Each segnaposto In valoriIntestazione.Keys
                    docOrig.ReplaceText(segnaposto, valoriIntestazione(segnaposto), False, Text.RegularExpressions.RegexOptions.None)
                Next

                For Each segnaposto In valoriCorpo.Keys
                    docOrig.ReplaceText(segnaposto, valoriCorpo(segnaposto), False, Text.RegularExpressions.RegexOptions.None)
                Next

                'salto pagina
                If curRecord < totRecord Then
                    Dim padded = docOrig.InsertParagraph("")
                    padded.InsertPageBreakAfterSelf()
                End If


                'accodo il documento e il salto pagina
                docDest.InsertDocument(docOrig)

                docOrig.Dispose()
                curRecord += 1
            Next
        End If

        docDest.Save()

        Return mStreamDest

    End Function

    Private Function GeneraFileExcel(dbConn As SqlConnection,
                                   valoreFiltroBase As String,
                                   modello As IO.MemoryStream,
                                   datiFO As DatiFiltroOrdinamento) As IO.MemoryStream

        Const maxCols = 100
        Const maxRows = 200
        Dim bodyStartRow As Integer
        Dim bodyEndRow As Integer
        Dim bodyStartCol As Integer
        Dim bodyEndCol As Integer
        Dim bodyRows As Integer

        Dim valoriIntestazione As Dictionary(Of String, String)
        Dim valoriIntestazioneObj As Dictionary(Of String, Object)
        Dim formatiIntestazione As Dictionary(Of String, String)

        Dim valoriCorpo As Dictionary(Of String, String)
        Dim valoriCorpoObj As Dictionary(Of String, Object)
        Dim formatiCorpo As Dictionary(Of String, String)

        Dim rigaIntestazione As DataRow
        Dim rigaCorpo As DataRow
        Dim campo As Campo
        Dim cIta = Softailor.Global.Cultures.CulturaItalian
        Dim mStreamDest As IO.MemoryStream
        Dim curRecord As Integer
        Dim totRecord As Integer
        Dim xlp As ExcelPackage
        Dim xlw As ExcelWorksheet
        Dim row As Integer
        Dim col As Integer
        Dim value As Object
        Dim sValue As String
        Dim oValue As Object
        Dim fValue As String
        Dim cell As ExcelRange

        'generazione dataset
        GeneraDatasetIntestazioneCorpo(dbConn, datiFO, valoreFiltroBase)

        'prima apertura del documento di origine per determinare quali campi sono presenti
        'e le posizioni dei campi corpo
        modello.Position = 0
        xlp = New ExcelPackage(modello)
        xlw = xlp.Workbook.Worksheets(1)

        CampiIntestazionePresenti = New List(Of Campo)
        CampiCorpoPresenti = New List(Of Campo)

        'ciclo su righe e colonne
        bodyStartRow = 0
        bodyEndRow = 0
        bodyStartCol = 0
        bodyEndCol = 0
        With xlw
            For row = 1 To maxRows
                For col = 1 To maxCols
                    value = .Cells(row, col).Value
                    If value IsNot Nothing Then
                        If TypeOf value Is String Then
                            'OK, contiene una stringa
                            sValue = CStr(value)

                            'cerco i campi testata
                            If Not String.IsNullOrEmpty(queryIntestazione) Then
                                For Each campo In CampiIntestazione
                                    If campo.Output Then
                                        If sValue.Contains(phpre & campo.Segnaposto & phpost) Then
                                            If Not CampiIntestazionePresenti.Contains(campo) Then
                                                CampiIntestazionePresenti.Add(campo)
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                            'ricerca del numero di riga
                            If sValue.Contains(phd_n_riga) Then
                                'riga di inizio corpo: sto procedendo dall'alto verso il basso quindi la valorizzo appena la trovo
                                If bodyStartRow = 0 Then bodyStartRow = row
                                'riga di fine: eventualmente aumento
                                If row > bodyEndRow Then bodyEndRow = row
                                'colonne
                                If bodyStartCol = 0 Then
                                    bodyStartCol = col
                                Else
                                    If col < bodyStartCol Then bodyStartCol = col
                                End If
                                If bodyEndCol = 0 Then
                                    bodyEndCol = col
                                Else
                                    If col > bodyEndCol Then bodyEndCol = col
                                End If
                            End If

                            For Each campo In CampiCorpo
                                If campo.Output Then
                                    If sValue.Contains(phpre & campo.Segnaposto & phpost) Then
                                        'riga di inizio corpo: sto procedendo dall'alto verso il basso quindi la valorizzo appena la trovo
                                        If bodyStartRow = 0 Then bodyStartRow = row
                                        'riga di fine: eventualmente aumento
                                        If row > bodyEndRow Then bodyEndRow = row
                                        'colonne
                                        If bodyStartCol = 0 Then
                                            bodyStartCol = col
                                        Else
                                            If col < bodyStartCol Then bodyStartCol = col
                                        End If
                                        If bodyEndCol = 0 Then
                                            bodyEndCol = col
                                        Else
                                            If col > bodyEndCol Then bodyEndCol = col
                                        End If
                                        'campi presenti
                                        If Not CampiCorpoPresenti.Contains(campo) Then
                                            CampiCorpoPresenti.Add(campo)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next
            Next
        End With

        'preparo i valori dell'intestazione
        valoriIntestazione = New Dictionary(Of String, String)
        valoriIntestazioneObj = New Dictionary(Of String, Object)
        formatiIntestazione = New Dictionary(Of String, String)

        If Not String.IsNullOrEmpty(queryIntestazione) Then
            rigaIntestazione = dstIntestazione.Tables(0).Rows(0)
            'aggiunta campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
            valoriIntestazioneObj.Add(phd_data_stampa_num, Date.Today)
            valoriIntestazioneObj.Add(phd_data_stampa_testo, Date.Today)
            valoriIntestazioneObj.Add(phd_ora_stampa, Date.Now)
            formatiIntestazione.Add(phd_data_stampa_num, "dd/MM/yyyy")
            formatiIntestazione.Add(phd_data_stampa_testo, "d MMMM yyyy")
            formatiIntestazione.Add(phd_ora_stampa, "HH:mm")
            For Each campo In CampiIntestazionePresenti
                valoriIntestazione.Add(
                    phpre & campo.Segnaposto & phpost,
                    Helpers.TextValue_MailMerge(rigaIntestazione(campo.NomeDb), campo, cIta))
                valoriIntestazioneObj.Add(
                    phpre & campo.Segnaposto & phpost,
                    rigaIntestazione(campo.NomeDb))
                formatiIntestazione.Add(phpre & campo.Segnaposto & phpost, campo.ExcelFormatString())
            Next
        Else
            'solo campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
            valoriIntestazioneObj.Add(phd_data_stampa_num, Date.Today)
            valoriIntestazioneObj.Add(phd_data_stampa_testo, Date.Today)
            valoriIntestazioneObj.Add(phd_ora_stampa, Date.Now)
            formatiIntestazione.Add(phd_data_stampa_num, "dd/MM/yyyy")
            formatiIntestazione.Add(phd_data_stampa_testo, "d MMMM yyyy")
            formatiIntestazione.Add(phd_ora_stampa, "HH:mm")
        End If

        'sostituzione valori dell'intestazione: ovunque, ANCHE nelle righe del corpo
        'in questo modo quando viene clonata la riga il sistema ripete i campi "da solo"
        'lo faccio anche se NON ho una query intestazione, per data/ora stampa
        With xlw
            For row = 1 To maxRows
                For col = 1 To maxCols
                    cell = .Cells(row, col)
                    value = cell.Value
                    If value IsNot Nothing Then
                        If TypeOf value Is String Then
                            'OK, contiene una stringa
                            sValue = CStr(value)
                            oValue = DBNull.Value
                            fValue = ""

                            Dim sostHeader = False
                            'PRIMA provo a sostituire il valore IN TOTO
                            For Each segnaposto In valoriIntestazione.Keys
                                If sValue = segnaposto Then
                                    oValue = valoriIntestazioneObj(segnaposto)
                                    fValue = formatiIntestazione(segnaposto)
                                    sostHeader = True
                                    Exit For
                                End If
                            Next

                            'ALTRIMENTI provo con i valori concatenati
                            If Not sostHeader Then
                                For Each segnaposto In valoriIntestazione.Keys
                                    sValue = sValue.Replace(segnaposto, valoriIntestazione(segnaposto))
                                Next
                                oValue = sValue
                            End If

                            If Not IsDBNull(oValue) Then
                                cell.Value = oValue
                            Else
                                cell.Value = Nothing
                            End If
                            If fValue <> String.Empty Then cell.Style.Numberformat.Format = fValue

                        End If
                    End If
                Next
            Next
        End With

        'corpo
        If Not String.IsNullOrEmpty(queryCorpo) Then
            totRecord = dstCorpo.Tables(0).Rows.Count
            If totRecord > 0 And bodyStartRow > 0 Then
                'OK, processiamo il corpo.
                bodyRows = 1 + bodyEndRow - bodyStartRow
                With xlw
                    'aggiunta N righe bianche dopo l'ultima
                    .InsertRow(bodyEndRow + 1, (totRecord - 1) * bodyRows)

                    'altezza righe
                    For curRecord = 2 To totRecord
                        For rowOffset = 0 To bodyRows - 1
                            .Row(bodyStartRow + (curRecord - 1) * bodyRows + rowOffset).Height =
                            .Row(bodyStartRow + rowOffset).Height
                        Next
                    Next

                    'duplicazione modello
                    For curRecord = 2 To totRecord
                        .Cells(bodyStartRow.ToString & ":" & bodyEndRow.ToString).Copy(
                        .Cells(
                            (bodyStartRow + (curRecord - 1) * bodyRows).ToString &
                            ":" &
                            (bodyStartRow + curRecord * bodyRows - 1).ToString
                        ))
                    Next

                    'sostituzione segnaposto sul corpo
                    valoriCorpo = New Dictionary(Of String, String)
                    valoriCorpoObj = New Dictionary(Of String, Object)
                    formatiCorpo = New Dictionary(Of String, String)
                    curRecord = 1

                    For Each rigaCorpo In dstCorpo.Tables(0).Rows

                        'preparazione valori
                        valoriCorpo.Clear()
                        valoriCorpoObj.Clear()
                        formatiCorpo.Clear()
                        valoriCorpo.Add(phd_n_riga, curRecord.ToString)
                        valoriCorpoObj.Add(phd_n_riga, curRecord)
                        formatiCorpo.Add(phd_n_riga, "")
                        For Each campo In CampiCorpoPresenti
                            valoriCorpo.Add(
                                phpre & campo.Segnaposto & phpost,
                                Helpers.TextValue_MailMerge(rigaCorpo(campo.NomeDb), campo, cIta))
                            valoriCorpoObj.Add(phpre & campo.Segnaposto & phpost,
                                               rigaCorpo(campo.NomeDb))
                            formatiCorpo.Add(phpre & campo.Segnaposto & phpost, campo.ExcelFormatString())
                        Next

                        'sostituzione
                        For row = bodyStartRow + (curRecord - 1) * bodyRows To bodyStartRow + curRecord * bodyRows - 1
                            For col = bodyStartCol To bodyEndCol
                                cell = .Cells(row, col)
                                value = cell.Value
                                If value IsNot Nothing Then
                                    If TypeOf value Is String Then

                                        'OK, contiene una stringa
                                        sValue = CStr(value)
                                        oValue = DBNull.Value
                                        fValue = ""

                                        Dim sostHeader = False
                                        'PRIMA provo a sostituire il valore IN TOTO
                                        For Each segnaposto In valoriCorpo.Keys
                                            If sValue = segnaposto Then
                                                oValue = valoriCorpoObj(segnaposto)
                                                fValue = formatiCorpo(segnaposto)
                                                sostHeader = True
                                                Exit For
                                            End If
                                        Next

                                        'ALTRIMENTI provo con i valori concatenati
                                        If Not sostHeader Then
                                            For Each segnaposto In valoriCorpo.Keys
                                                sValue = sValue.Replace(segnaposto, valoriCorpo(segnaposto))
                                            Next
                                            oValue = sValue
                                        End If

                                        If Not IsDBNull(oValue) Then
                                            cell.Value = oValue
                                        Else
                                            cell.Value = Nothing
                                        End If
                                        If fValue <> String.Empty Then cell.Style.Numberformat.Format = fValue

                                    End If
                                End If
                            Next
                        Next

                        curRecord += 1
                    Next

                End With
            End If
        End If

        'salvataggio
        mStreamDest = New IO.MemoryStream
        xlp.SaveAs(mStreamDest)
        xlp.Dispose()
        modello.Dispose()
        Return mStreamDest

    End Function


    Private Function GeneraFileWordSplit(
                                   dbConn As SqlConnection,
                                   valoreFiltroBase As String,
                                   modello As IO.MemoryStream,
                                   datiFO As DatiFiltroOrdinamento) As IO.MemoryStream

        Dim valoriIntestazione As Dictionary(Of String, String)
        Dim valoriCorpo As Dictionary(Of String, String)
        Dim rigaIntestazione As DataRow
        Dim rigaCorpo As DataRow
        Dim campo As Campo
        Dim cIta = Softailor.Global.Cultures.CulturaItalian
        Dim docOrig As DocX
        Dim docDest As DocX
        Dim mStreamDest As IO.MemoryStream
        Dim i As Integer
        Dim curRecord As Integer
        Dim totRecord As Integer

        'Dim listFile As List(Of IO.MemoryStream)
        Dim listFile As IList(Of IO.MemoryStream)
        listFile = New List(Of IO.MemoryStream)

        Dim listFile1 As Dictionary(Of String, IO.MemoryStream)
        listFile1 = New Dictionary(Of String, IO.MemoryStream)
        Dim ret As IO.MemoryStream
        Dim nome As String
        Dim cognome As String
        Dim nominativo As String

        'generazione dataset
        GeneraDatasetIntestazioneCorpo(dbConn, datiFO, valoreFiltroBase)

        'prima apertura del documento di origine per determinare quali campi sono presenti
        modello.Position = 0
        docOrig = DocX.Load(modello)

        CampiIntestazionePresenti = New List(Of Campo)
        CampiCorpoPresenti = New List(Of Campo)

        If Not String.IsNullOrEmpty(queryIntestazione) Then
            For Each campo In CampiIntestazione
                If campo.Output Then
                    If docOrig.FindAll(phpre & campo.Segnaposto & phpost).Count > 0 Then
                        CampiIntestazionePresenti.Add(campo)
                    End If
                End If
            Next
        End If
        If Not String.IsNullOrEmpty(queryCorpo) Then
            For Each campo In CampiCorpo
                If campo.Output Then
                    If docOrig.FindAll(phpre & campo.Segnaposto & phpost).Count > 0 Then
                        CampiCorpoPresenti.Add(campo)
                    End If
                End If
            Next
        End If

        docOrig.Dispose()

        'istanzio il documento di destinazione
        'creando una copia esatta di quello di origine
        mStreamDest = New IO.MemoryStream



        modello.Position = 0
        modello.CopyTo(mStreamDest)
        mStreamDest.Position = 0
        docDest = DocX.Load(mStreamDest)

        'pulisco tutti i paragrafi e le tabelle nel documento di destinazione
        i = 0
        Do While (docDest.Tables.Count > 0) And i < 500
            Dim tbl = docDest.Tables(0)
            tbl.Remove()
            i += 1
        Loop
        i = 0
        Do While (docDest.Paragraphs.Count > 0) And i < 500
            Dim prg = docDest.Paragraphs(0)
            prg.Remove(False)
            i += 1
        Loop

        'preparo i valori dell'intestazione
        valoriIntestazione = New Dictionary(Of String, String)
        If Not String.IsNullOrEmpty(queryIntestazione) Then
            rigaIntestazione = dstIntestazione.Tables(0).Rows(0)
            'aggiunta campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
            For Each campo In CampiIntestazionePresenti
                valoriIntestazione.Add(
                    phpre & campo.Segnaposto & phpost,
                    Helpers.TextValue_MailMerge(rigaIntestazione(campo.NomeDb), campo, cIta))
            Next
        Else
            'aggiungo SOLO campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
        End If

        'ciclo sul corpo
        If Not String.IsNullOrEmpty(queryCorpo) Then
            curRecord = 1
            totRecord = dstCorpo.Tables(0).Rows.Count
            valoriCorpo = New Dictionary(Of String, String)
            Dim contatore As Integer = 0
            For Each rigaCorpo In dstCorpo.Tables(0).Rows

                valoriCorpo.Clear()
                'numero di riga
                valoriCorpo.Add(phd_n_riga, curRecord.ToString)
                For Each campo In CampiCorpoPresenti
                    valoriCorpo.Add(
                        phpre & campo.Segnaposto & phpost,
                        Helpers.TextValue_MailMerge(rigaCorpo(campo.NomeDb), campo, cIta))
                Next

                'ri-apro il doc di origine
                modello.Position = 0
                docOrig = DocX.Load(modello)

                'sostituzione segnaposto nel doc di origine
                For Each segnaposto In valoriIntestazione.Keys
                    docOrig.ReplaceText(segnaposto, valoriIntestazione(segnaposto), False, Text.RegularExpressions.RegexOptions.None)
                Next

                For Each segnaposto In valoriCorpo.Keys
                    If (segnaposto = "%%NOME%%") Then
                        nome = valoriCorpo(segnaposto)
                    End If
                    If (segnaposto = "%%COGNOME%%") Then
                        cognome = valoriCorpo(segnaposto)
                    End If
                    docOrig.ReplaceText(segnaposto, valoriCorpo(segnaposto), False, Text.RegularExpressions.RegexOptions.None)
                Next

                nominativo = nome & "_" & cognome
                'salto pagina
                'If curRecord < totRecord Then
                '    Dim padded = docOrig.InsertParagraph("")
                '    padded.InsertPageBreakAfterSelf()
                'End If

                docDest.InsertDocument(docOrig)

                docDest.Save()

                Dim tmp As IO.MemoryStream
                tmp = New IO.MemoryStream

                docOrig.SaveAs(tmp)
                docOrig.Dispose()

                'Dim tmp As IO.MemoryStream
                'tmp = New IO.MemoryStream
                'mStreamDest.CopyTo(tmp)
                nominativo = nominativo.Replace("'", "")
                listFile.Add(tmp)
                listFile1.Add(contatore & "_" & nominativo, tmp)
                contatore = contatore + 1
                curRecord += 1
            Next
        End If

        ret = CreateToMemoryStream(listFile1)
        Return ret
        'Return mStreamDest

    End Function


    Public Function CreateToMemoryStream(listFile As Dictionary(Of String, IO.MemoryStream)) As IO.MemoryStream

        Dim outputMemStream As New IO.MemoryStream()
        Dim zipStream As New ZipOutputStream(outputMemStream)


        Dim i As Integer
        i = 0
        For Each fileX As KeyValuePair(Of String, IO.MemoryStream) In listFile
            Dim nome_file As String = fileX.Key
            Dim mem_stream As IO.MemoryStream = fileX.Value

            Dim newEntry As New ZipEntry(nome_file & ".docx")
            'fileX_1 = New IO.MemoryStream(mem_stream.ToArray())

            mem_stream.Position = 0
            zipStream.SetLevel(3)       '0-9, 9 being the highest level of compression
            newEntry.DateTime = DateTime.Now
            'zipStream.UseZip64 = UseZip64.Off
            zipStream.PutNextEntry(newEntry)

            StreamUtils.Copy(mem_stream, zipStream, New Byte(4095) {})

            mem_stream.Close()
            zipStream.CloseEntry()
            i = i + 1
        Next

        zipStream.IsStreamOwner = False     'False stops the Close also Closing the underlying stream.
        zipStream.Close()                   'Must finish the ZipOutputStream before using outputMemStream.
        outputMemStream.Position = 0
        Return outputMemStream

    End Function

    Public Function CreateToMemoryStream2(memStreamIn As IO.MemoryStream, zipEntryName As String) As IO.MemoryStream



        memStreamIn.Position = 0
        Dim outputMemStream As New IO.MemoryStream()
        Dim zipStream As New ZipOutputStream(outputMemStream)

        zipStream.SetLevel(3)       '0-9, 9 being the highest level of compression
        Dim newEntry As New ZipEntry(zipEntryName)
        newEntry.DateTime = DateTime.Now
        'zipStream.UseZip64 = UseZip64.Off
        zipStream.PutNextEntry(newEntry)

        StreamUtils.Copy(memStreamIn, zipStream, New Byte(4095) {})

        memStreamIn.Close()
        zipStream.CloseEntry()

        zipStream.IsStreamOwner = False     ' False stops the Close also Closing the underlying stream.
        zipStream.Close()           ' Must finish the ZipOutputStream before using outputMemStream.
        outputMemStream.Position = 0
        Return outputMemStream

    End Function


#End Region

#Region "Generazione e-mail"

    Public Class EmailResult
        Public subject As String
        Public body As String
        Public recipient As String
    End Class

    Public Function GeneraMails(dbConn As SqlConnection, valoreFiltroBase As String, valoriChiave As List(Of String), tx_OGGETTO As String, ht_CORPO As String) As List(Of EmailResult)

        Dim rigaIntestazione As DataRow
        Dim rigaCorpo As DataRow
        Dim valoriIntestazione As Dictionary(Of String, String)
        Dim valoriIntestazioneObj As Dictionary(Of String, Object)
        Dim formatiIntestazione As Dictionary(Of String, String)

        Dim valoriCorpo As Dictionary(Of String, String)
        Dim valoriCorpoObj As Dictionary(Of String, Object)
        Dim formatiCorpo As Dictionary(Of String, String)

        Dim campo As Campo
        Dim cIta = Softailor.Global.Cultures.CulturaItalian
        Dim curRecord As Integer
        Dim emailOut As New List(Of EmailResult)
        Dim emailResult As EmailResult

        'ricerca dei campi presenti
        CampiIntestazionePresenti = New List(Of Campo)
        CampiCorpoPresenti = New List(Of Campo)

        For Each campo In CampiIntestazione
            If campo.Output Then
                If tx_OGGETTO.Contains(phpre & campo.Segnaposto & phpost) Or ht_CORPO.Contains(phpre & campo.Segnaposto & phpost) Then
                    If Not CampiIntestazionePresenti.Contains(campo) Then
                        CampiIntestazionePresenti.Add(campo)
                    End If
                End If
            End If
        Next

        For Each campo In CampiCorpo
            If campo.Output Then
                If tx_OGGETTO.Contains(phpre & campo.Segnaposto & phpost) Or ht_CORPO.Contains(phpre & campo.Segnaposto & phpost) Then
                    If Not CampiCorpoPresenti.Contains(campo) Then
                        CampiCorpoPresenti.Add(campo)
                    End If
                End If
            End If
        Next

        'generazione dataset
        GeneraDatasetIntestazioneCorpo_ItemSpecifici(dbConn, valoreFiltroBase, valoriChiave)

        'preparo gli array
        valoriIntestazione = New Dictionary(Of String, String)
        valoriIntestazioneObj = New Dictionary(Of String, Object)
        formatiIntestazione = New Dictionary(Of String, String)
        valoriCorpo = New Dictionary(Of String, String)
        valoriCorpoObj = New Dictionary(Of String, Object)
        formatiCorpo = New Dictionary(Of String, String)

        'preparo i valori dell'intestazione (una sola volta)
        If Not String.IsNullOrEmpty(queryIntestazione) Then
            rigaIntestazione = dstIntestazione.Tables(0).Rows(0)
            'aggiunta campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
            valoriIntestazioneObj.Add(phd_data_stampa_num, Date.Today)
            valoriIntestazioneObj.Add(phd_data_stampa_testo, Date.Today)
            valoriIntestazioneObj.Add(phd_ora_stampa, Date.Now)
            formatiIntestazione.Add(phd_data_stampa_num, "dd/MM/yyyy")
            formatiIntestazione.Add(phd_data_stampa_testo, "d MMMM yyyy")
            formatiIntestazione.Add(phd_ora_stampa, "HH:mm")
            For Each campo In CampiIntestazionePresenti
                valoriIntestazione.Add(
                    phpre & campo.Segnaposto & phpost,
                    Helpers.TextValue_MailMerge(rigaIntestazione(campo.NomeDb), campo, cIta))
                valoriIntestazioneObj.Add(
                    phpre & campo.Segnaposto & phpost,
                    rigaIntestazione(campo.NomeDb))
                formatiIntestazione.Add(phpre & campo.Segnaposto & phpost, campo.ExcelFormatString())
            Next
        Else
            'solo campi standard
            valoriIntestazione.Add(phd_data_stampa_num, Date.Today.ToString("dd/MM/yyyy", cIta))
            valoriIntestazione.Add(phd_data_stampa_testo, Date.Today.ToString("d MMMM yyyy", cIta))
            valoriIntestazione.Add(phd_ora_stampa, Date.Now.ToString("HH:mm", cIta))
            valoriIntestazioneObj.Add(phd_data_stampa_num, Date.Today)
            valoriIntestazioneObj.Add(phd_data_stampa_testo, Date.Today)
            valoriIntestazioneObj.Add(phd_ora_stampa, Date.Now)
            formatiIntestazione.Add(phd_data_stampa_num, "dd/MM/yyyy")
            formatiIntestazione.Add(phd_data_stampa_testo, "d MMMM yyyy")
            formatiIntestazione.Add(phd_ora_stampa, "HH:mm")
        End If

        'ciclo sui record del corpo

        curRecord = 1
        For Each rigaCorpo In dstCorpo.Tables(0).Rows

            'preparazione valori
            valoriCorpo.Clear()
            valoriCorpoObj.Clear()
            formatiCorpo.Clear()
            valoriCorpo.Add(phd_n_riga, curRecord.ToString)
            valoriCorpoObj.Add(phd_n_riga, curRecord)
            formatiCorpo.Add(phd_n_riga, "")
            For Each campo In CampiCorpoPresenti
                valoriCorpo.Add(
                    phpre & campo.Segnaposto & phpost,
                    Helpers.TextValue_MailMerge(rigaCorpo(campo.NomeDb), campo, cIta))
                valoriCorpoObj.Add(phpre & campo.Segnaposto & phpost,
                                   rigaCorpo(campo.NomeDb))
                formatiCorpo.Add(phpre & campo.Segnaposto & phpost, campo.ExcelFormatString())
            Next

            'preparazione messaggio
            emailResult = New EmailResult

            'destinatario
            If rigaCorpo.IsNull(Me.CampoMail) Then
                emailResult.recipient = ""
            Else
                emailResult.recipient = rigaCorpo(Me.CampoMail).ToString
            End If

            'preparo intestazione e corpo
            emailResult.subject = tx_OGGETTO
            emailResult.body = ht_CORPO

            'sostituzione dati intestazione
            For Each segnaposto In valoriIntestazione.Keys
                emailResult.subject = emailResult.subject.Replace(segnaposto, valoriIntestazione(segnaposto))
                emailResult.body = emailResult.body.Replace(segnaposto, valoriIntestazione(segnaposto))
            Next

            'sostituzione dati corpo
            For Each segnaposto In valoriCorpo.Keys
                emailResult.subject = emailResult.subject.Replace(segnaposto, valoriCorpo(segnaposto))
                emailResult.body = emailResult.body.Replace(segnaposto, valoriCorpo(segnaposto))
            Next

            'appendo
            emailOut.Add(emailResult)

            curRecord += 1

        Next

        Return emailOut

    End Function


#End Region

#Region "Membri condivisi"
    Public Shared Function FromXml(fullPath As String) As Fonte

        Dim newFonte As Fonte

        Using fReader = New IO.FileStream(fullPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            Dim xSer As New XmlSerializer(GetType(Fonte))
            newFonte = CType(xSer.Deserialize(fReader), Fonte)
        End Using

        If newFonte.SovrapposizioneCampi Then
            Throw New Exception("Esistono segnaposto che compaiono sia nell'intestazione sia nel corpo.")
        Else
            ''filtro
            'newFonte.valoreFiltroBase = valoreFiltroBase
            ''connessione
            'newFonte.dbConn = dbConn
            'ritorno la fonte
            Return newFonte
        End If

    End Function
#End Region

End Class
