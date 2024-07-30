Imports OfficeOpenXml

Public Class ReportRimborsiDocenze
    Inherits System.Web.UI.Page

    Dim dbConn As SqlConnection

    Private Sub ReportRimborsiDocenze_Init(sender As Object, e As EventArgs) Handles Me.Init

        dbConn = DbConnectionHandler.GetOpenRptDbConn

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ddnAnno.SelectedValue = Date.Today.Year.ToString
        End If
    End Sub

    Private Sub ReportRimborsiDocenze_Unload(sender As Object, e As EventArgs) Handles Me.Unload

        If dbConn IsNot Nothing Then
            If dbConn.State = ConnectionState.Open Then
                dbConn.Close()
            End If
            dbConn.Dispose()
        End If

    End Sub

    Private Sub lnkGenera_Click(sender As Object, e As EventArgs) Handles lnkGenera.Click

        Dim dbCmd As SqlCommand
        Dim dt As DataTable
        Dim dbDad As SqlDataAdapter


        'lettura dati
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_statcosti_DocenzeRimborsi"
            .Parameters.Add("@ni_anno", SqlDbType.Int).Value = CInt(ddnAnno.SelectedValue)
            .Parameters.Add("@ni_trimestre", SqlDbType.Int).Value = CInt(ddnTrimestre.SelectedValue)
            .Parameters.Add("@fl_ripetievento", SqlDbType.Bit).Value = ddnRipetiEvento.SelectedValue = "1"
        End With
        dbDad = New SqlDataAdapter(dbCmd)
        dt = New DataTable
        dbDad.Fill(dt)
        dbDad.Dispose()
        dbCmd.Dispose()

        ProduciExcel(dt, "RimborsiDocenze", CInt(ddnAnno.SelectedValue))

    End Sub

    Private Sub ProduciExcel(dt As DataTable, fileName As String, anno As Integer)

        Dim rng As ExcelRange
        Dim xlp As New ExcelPackage

        'inserimento dati
        Dim xlws = xlp.Workbook.Worksheets.Add("Tabulato")
        xlws.Cells("A1").LoadFromDataTable(dt, True)
        dt.Dispose()

        'formattazione delle colonne
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).DataType.Equals(GetType(System.DateTime)) Then
                    rng = xlws.Cells(2, i + 1, 2 + dt.Rows.Count - 1, i + 1)
                    rng.Style.Numberformat.Format = "dd/MM/yyyy"
                ElseIf dt.Columns(i).DataType.Equals(GetType(System.Decimal)) Then
                    rng = xlws.Cells(2, i + 1, 2 + dt.Rows.Count - 1, i + 1)
                    rng.Style.Numberformat.Format = "#,##0.00"
                ElseIf dt.Columns(i).DataType.Equals(GetType(System.TimeSpan)) Then
                    rng = xlws.Cells(2, i + 1, 2 + dt.Rows.Count - 1, i + 1)
                    rng.Style.Numberformat.Format = "HH:mm"
                End If
            Next
        End If

        'intestazioni
        rng = xlws.Cells(1, 1, 1, dt.Columns.Count)
        rng.Style.Font.Bold = True
        rng.Style.Font.Color.SetColor(System.Drawing.Color.White)
        rng.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(31, 73, 125))

        'font
        rng = xlws.Cells(1, 1, dt.Rows.Count + 1, dt.Columns.Count)
        rng.Style.Font.Size = 10
        rng.AutoFitColumns()
        'rng.AutoFilter = True

        'cella titolo
        xlws.Column(2).Width = 45
        If dt.Rows.Count > 0 Then
            xlws.Cells(2, 2, 2 + dt.Rows.Count - 1, 2).Style.WrapText = True
        End If

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=" & fileName & "_" & anno.ToString & ".xlsx")
        Response.BinaryWrite(xlp.GetAsByteArray())
        xlp.Dispose()
        xlp = Nothing

        dbConn.Close()
        dbConn.Dispose()
        Response.End()

    End Sub
End Class