Imports Softailor.Global.XmlParser
Imports OfficeOpenXml

Public Class ReportPresenzeEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Private Sub ReportPresenzeEvento_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'riempimento date
        RiempimentoDate()

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub btnDoReport_Click(sender As Object, e As EventArgs) Handles btnDoReport.Click

        'lettura doc Xml
        Dim xDoc = GeneraXmlReport()

        'generazione Excel
        GeneraXlsxReport(xDoc)

    End Sub

    Private Sub RiempimentoDate()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        'pulizia e primo valore
        rblData.Items.Clear()
        rblData.Items.Add(New ListItem("Tutte le date", ""))
        rblData.SelectedValue = ""

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT DISTINCT CAST(dt_DATA as datetime) as dt_DATA FROM eve_CALENDARIO WHERE id_EVENTO=@id_EVENTO ORDER BY dt_DATA"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            rblData.Items.Add(New ListItem(dbRdr.GetDateTime(0).ToString("dd/MM/yyyy"), dbRdr.GetDateTime(0).ToString("yyyyMMdd")))
        Loop
        dbRdr.Close()
        dbCmd.Dispose()
    End Sub

    Private Function GeneraXmlReport() As XmlDocument

        Dim dbCmd As SqlCommand
        Dim xReader As XmlReader
        Dim xDoc As XmlDocument

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_ReportPresenzeXml"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@ac_DIPEXT", SqlDbType.NVarChar, 6).Value = rblDipExt.SelectedValue
            With .Parameters.Add("@dt_DATA", SqlDbType.DateTime)
                If rblData.SelectedValue = "" Then
                    .Value = DBNull.Value
                Else
                    .Value = Date.ParseExact(rblData.SelectedValue, "yyyyMMdd", Softailor.Global.Cultures.CulturaEnglish)
                End If
            End With
        End With
        xReader = dbCmd.ExecuteXmlReader
        xDoc = New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        xReader.Dispose()
        dbCmd.Dispose()

        Return xDoc

    End Function

#Region "Generazione Excel"

    Public Sub GeneraXlsxReport(xDoc As XmlDocument)

        Dim xlp As New ExcelPackage
        Dim xlws = xlp.Workbook.Worksheets.Add("Report")

        GeneraReport(xDoc, xlws)

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=ReportPresenze_" & Date.Now.ToString("dd_MM_yyyy_HH_mm_ss") & ".xlsx")
        Response.BinaryWrite(xlp.GetAsByteArray())
        xlp.Dispose()
        xlp = Nothing

        dbConn.Close()
        dbConn.Dispose()
        Response.End()

    End Sub

    Private Function ExcelSize(s As Double) As Double
        Return s * CDbl(20) / CDbl(19.29)
    End Function

    Private Sub GeneraReport(xDoc As XmlDocument, ByRef ws As ExcelWorksheet)

        Dim curRow = 1
        GeneraIntestazione(xDoc, ws, curRow)
        GeneraCalendarioEvento(xDoc, ws, curRow)
        GeneraDatiIscritti(xDoc, ws, curRow)

        'colonne
        ws.Column(1).Width = ExcelSize(14.71)
        ws.Column(2).Width = ExcelSize(15.43)
        ws.Column(3).Width = ExcelSize(15.43)
        ws.Column(4).Width = ExcelSize(15.43)
        ws.Column(5).Width = ExcelSize(11.43)
        ws.Column(6).Width = ExcelSize(11)
        ws.Column(7).Width = ExcelSize(9.57)
        ws.Column(8).Width = ExcelSize(7.14)
        ws.Column(9).Width = ExcelSize(7.14)
        ws.Column(10).Width = ExcelSize(7.14)

        'stampa
        ws.PrinterSettings.TopMargin = 0.4D
        ws.PrinterSettings.BottomMargin = 0.4D
        ws.PrinterSettings.HeaderMargin = 0.4D
        ws.PrinterSettings.FooterMargin = 0.4D

        ws.PrinterSettings.LeftMargin = 0.4D
        ws.PrinterSettings.RightMargin = 0.4D

        ws.PrinterSettings.PaperSize = ePaperSize.A4
        ws.PrinterSettings.Orientation = eOrientation.Portrait
        ws.PrinterSettings.HorizontalCentered = True
        ws.PrinterSettings.Scale = 79

    End Sub

    Private Sub GeneraIntestazione(xDoc As XmlDocument, ByRef ws As ExcelWorksheet, ByRef curRow As Integer)

        Dim eveNode As XmlNode = xDoc.SelectSingleNode("/evento")

        ws.Cells(1, 1).Value = "N° ID corso"
        ws.Cells(2, 1).Value = "Titolo del corso"
        ws.Cells(3, 1).Value = "Edizione"
        ws.Cells(4, 1).Value = "Sede"
        ws.Cells(5, 1).Value = "Data inizio"
        ws.Cells(6, 1).Value = "Data fine"

        ws.Cells(1, 2).Value = ParseXmlInteger(eveNode, "id_evento")
        ws.Cells(1, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left

        ws.Cells(2, 2).Value = ParseXmlString(eveNode, "tx_titolo")
        ws.Cells(3, 2).Value = ParseXmlString(eveNode, "tx_edizione")
        ws.Cells(4, 2).Value = ParseXmlString(eveNode, "tx_sede")

        ws.Cells(5, 2).Value = ParseXmlDateOnly(eveNode, "dt_inizio")
        ws.Cells(5, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
        ws.Cells(5, 2).Style.Numberformat.Format = "dd/MM/yyyy"

        ws.Cells(6, 2).Value = ParseXmlDateOnly(eveNode, "dt_fine")
        ws.Cells(6, 2).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
        ws.Cells(6, 2).Style.Numberformat.Format = "dd/MM/yyyy"

        ws.Cells(1, 2, 6, 2).Style.Font.Bold = True

        ws.Cells(6, 1, 6, 10).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(6, 1, 6, 10).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black)

        curRow = 8


    End Sub

    Private Sub GeneraCalendarioEvento(xDoc As XmlDocument, ByRef ws As ExcelWorksheet, ByRef curRow As Integer)

        Dim startCal As Integer
        Dim endCal As Integer
        Dim rowMin As Integer
        Dim totMin As Integer = 0

        ws.Cells(curRow, 1).Value = "Data"
        ws.Cells(curRow, 2).Value = "Dalle ore"
        ws.Cells(curRow, 3).Value = "Alle ore"
        ws.Cells(curRow, 4).Value = "Durata"
        ws.Cells(curRow, 1, curRow, 4).Style.Font.Bold = True
        ws.Cells(curRow, 1, curRow, 4).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(curRow, 1, curRow, 4).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black)

        curRow += 1
        startCal = curRow
        For Each calendarioNode As XmlNode In xDoc.SelectNodes("evento/calendario")
            endCal = curRow
            ws.Cells(curRow, 1).Value = ParseXmlDateOnly(calendarioNode, "dt_data")
            ws.Cells(curRow, 2).Value = ParseXmlTimeOnly(calendarioNode, "tm_inizio")
            ws.Cells(curRow, 3).Value = ParseXmlTimeOnly(calendarioNode, "tm_fine")
            rowMin = ParseXmlInteger(calendarioNode, "ni_minuti")
            totMin += rowMin
            ws.Cells(curRow, 4).Value = New TimeSpan(0, rowMin, 0)
            curRow += 1
        Next

        'formati
        ws.Cells(startCal, 1, endCal, 1).Style.Numberformat.Format = "dd/MM/yyyy"
        ws.Cells(startCal, 2, endCal, 4).Style.Numberformat.Format = "HH:mm"

        'chiudo la riga
        ws.Cells(endCal, 1, endCal, 4).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(endCal, 1, endCal, 4).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black)

        'totale
        ws.Cells(curRow, 1).Value = "Durata Totale"
        ws.Cells(curRow, 4).Value = (totMin \ 60).ToString("#00") & ":" & (totMin Mod 60).ToString("00")
        ws.Cells(curRow, 1, curRow, 4).Style.Font.Bold = True
        ws.Cells(curRow, 1, curRow, 4).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right

        curRow += 2

    End Sub

    Private Sub GeneraDatiIscritti(xDoc As XmlDocument, ByRef ws As ExcelWorksheet, ByRef curRow As Integer)

        'intestazioni colonne

        ws.Cells(curRow, 1).Value = "Ruolo"
        ws.Cells(curRow, 2).Value = "Matricola"
        ws.Cells(curRow, 3).Value = "Cognome"
        ws.Cells(curRow, 4).Value = "Nome"
        ws.Cells(curRow, 5).Value = "Categoria"
        ws.Cells(curRow, 6).Value = "Data"
        ws.Cells(curRow, 7).Value = "Durata tot"
        ws.Cells(curRow, 8).Value = "Entrata"
        ws.Cells(curRow, 9).Value = "Uscita"
        ws.Cells(curRow, 10).Value = "Durata"
        ws.Cells(curRow, 1, curRow, 10).Style.Font.Bold = True
        ws.Cells(curRow, 1, curRow, 10).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
        ws.Cells(curRow, 1, curRow, 10).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black)

        curRow += 1
        'ciclo
        For Each partecipanteNode As XmlNode In xDoc.SelectNodes("evento/partecipante")
            GeneraDatiIscritto(partecipanteNode, ws, curRow)
        Next

    End Sub

    Private Sub GeneraDatiIscritto(xNode As XmlNode, ByRef ws As ExcelWorksheet, ByRef curRow As Integer)

        Dim s As String
        Dim minuti As Integer
        Dim minutiData As Integer
        Dim startData As Integer

        'ruolo
        s = ParseXmlString(xNode, "tx_ruolo")
        If s <> String.Empty Then ws.Cells(curRow, 1).Value = s

        'matricola
        s = ParseXmlString(xNode, "ac_matricola")
        If s <> String.Empty Then ws.Cells(curRow, 2).Value = s

        'cognome
        s = ParseXmlString(xNode, "tx_cognome")
        If s <> String.Empty Then ws.Cells(curRow, 3).Value = s

        'nome
        s = ParseXmlString(xNode, "tx_nome")
        If s <> String.Empty Then ws.Cells(curRow, 4).Value = s

        'categoria
        s = ParseXmlString(xNode, "tx_categoriaecm")
        If s <> String.Empty Then ws.Cells(curRow, 5).Value = s



        'ciclo sulle date
        For Each dataNode As XmlNode In xNode.SelectNodes("data")

            'riga di partenza
            startData = curRow

            'scrittura data
            With ws.Cells(curRow, 6)
                .Value = ParseXmlDateOnly(dataNode, "dt_data")
                .Style.Numberformat.Format = "dd/MM/yyyy"
            End With

            'ciclo sugli in - out e calcolo della durata totale
            minutiData = 0

            For Each inOutNode As XmlNode In dataNode.SelectNodes("inout")
                ws.Cells(curRow, 8).Value = ParseXmlTimeOnly(inOutNode, "tm_inizio")
                ws.Cells(curRow, 9).Value = ParseXmlTimeOnly(inOutNode, "tm_fine")
                ws.Cells(curRow, 8, curRow, 9).Style.Numberformat.Format = "HH:mm"
                minuti = ParseXmlInteger(inOutNode, "ni_minuti")
                ws.Cells(curRow, 10).Value = (minuti \ 60).ToString("#00") & ":" & (minuti Mod 60).ToString("00")
                ws.Cells(curRow, 10).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right
                minutiData += minuti
                curRow += 1
            Next

            'durata totale
            ws.Cells(startData, 7).Value = (minutiData \ 60).ToString("#00") & ":" & (minutiData Mod 60).ToString("00")
            ws.Cells(startData, 7).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right


            'linea di chiusura relativa alla data e alla durata totale
            With ws.Cells(curRow - 1, 6, curRow - 1, 10).Style.Border.Bottom
                .Style = Style.ExcelBorderStyle.Thin
                .Color.SetColor(System.Drawing.Color.Black)
            End With

        Next


        'linea di chiusura relativa alla persona
        With ws.Cells(curRow - 1, 1, curRow - 1, 5).Style.Border.Bottom
            .Style = Style.ExcelBorderStyle.Thin
            .Color.SetColor(System.Drawing.Color.Black)
        End With

    End Sub


#End Region


End Class