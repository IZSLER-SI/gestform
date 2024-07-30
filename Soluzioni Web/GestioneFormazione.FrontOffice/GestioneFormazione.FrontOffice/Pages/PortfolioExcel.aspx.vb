Imports OfficeOpenXml
Imports System.IO
Imports Softailor.Global.XmlParser

Public Class PortfolioExcel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Clear()
        Response.Expires = 0
        Response.Cache.SetNoStore()

        'verifica
        If ContextHandler.Region <> ContextHandler.Regions.LoggedIn Then
            Response.StatusCode = 403
            Response.End()
        End If

        'se siamo qui > generazione Excel

        GeneraOutput()


    End Sub

    Private Sub GeneraOutput()

        Dim profiloecm As Boolean
        Dim xlp As ExcelPackage
        Dim xlw As ExcelWorksheet

        'lettura documento
        Dim xDoc = LeggiDati()

        'profilo ECM?
        profiloecm = ParseXmlBoolean01(xDoc.LastChild, "fl_profiloecm")

        'istanzio Excel
        If profiloecm Then
            xlp = New ExcelPackage(New FileInfo(Server.MapPath("/Files/BasePortfolioExcel_ECM.xlsx")))
        Else
            xlp = New ExcelPackage(New FileInfo(Server.MapPath("/Files/BasePortfolioExcel_NOECM.xlsx")))
        End If

        xlw = xlp.Workbook.Worksheets(1)

        'riempimento Excel
        Dim cognomeNome = RiempiExcel(profiloecm, xlw, xDoc)

        'esportazione
        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=Portfolio " & cognomeNome & " " & Date.Today.ToString("d MMM yyyy") & ".xlsx")
        Response.BinaryWrite(xlp.GetAsByteArray())

        xlp.Dispose()
        xlp = Nothing
        Response.End()
        
    End Sub

    Private Function LeggiDati() As XmlDocument

        Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim sXml As String
        Dim xDoc As XmlDocument

        'apertura connessione
        dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        dbConn.Open()

        'preparazione comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_fo_PortfolioExcel"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = ContextHandler.id_PERSONA
        End With
        sXml = Transformer.Transform(dbCmd, "~/Templates/" & My.Settings.CompanyKey & "/PortfolioExcel.xslt",
                              "companyname", My.Settings.CompanyName_Short)
        xDoc = New XmlDocument
        xDoc.LoadXml(sXml)

        'chiusura connessione
        dbConn.Close()
        dbConn.Dispose()

        Return xDoc

    End Function

    Private Function RiempiExcel(profiloecm As Boolean, xlw As ExcelWorksheet, xDoc As XmlDocument) As String

        Dim rootNode As XmlNode
        Dim eventNode As XmlNode
        Dim cy As Integer
        Dim lastcol As Integer
        Dim cell As ExcelRange
        Dim ert As OfficeOpenXml.Style.ExcelRichText

        rootNode = xDoc.LastChild

        'restituzione cognome e nome
        RiempiExcel = ParseXmlString(rootNode, "tx_cognome") & " " & ParseXmlString(rootNode, "tx_nome")

        'ultima colonna
        If profiloecm Then lastcol = 4 Else lastcol = 3

        'dati testata
        xlw.Cells(1, 1).Value = ParseXmlString(rootNode, "tx_cognome") & " " & ParseXmlString(rootNode, "tx_nome")
        xlw.Cells(1, lastcol).Value = "Matricola " & ParseXmlString(rootNode, "ac_matricola")
        xlw.Cells(3, lastcol).Value = "Aggiornato al " & Date.Today.ToString("dd/MM/yyyy")

        'eventi
        cy = 7
        For Each eventNode In rootNode.SelectNodes("evento")


            'tipologia e data
            cell = xlw.Cells(cy, 1)
            cell.IsRichText = True
            With cell.RichText
                ert = .Add(ParseXmlString(eventNode, "date") & vbCrLf)
                ert.Bold = True

                ert = .Add(ParseXmlString(eventNode, "tipopartecipazione"))
                ert.Bold = False

            End With

            'tipo evento, titolo, sede
            cell = xlw.Cells(cy, 2)
            cell.IsRichText = True
            With cell.RichText
                ert = .Add(ParseXmlString(eventNode, "tipologiaevento") & vbCrLf)
                ert.Bold = False

                ert = .Add(ParseXmlString(eventNode, "titoloevento"))
                ert.Bold = True

                If ParseXmlString(eventNode, "sedeevento") <> String.Empty Then
                    ert = .Add(vbCrLf & ParseXmlString(eventNode, "sedeevento"))
                    ert.Bold = False
                End If

            End With

            'ruolo
            cell = xlw.Cells(cy, 3)
            cell.Value = ParseXmlString(eventNode, "ruolo")

            'crediti ECM
            If profiloecm Then
                If ParseXmlString(eventNode, "creditiecm") <> String.Empty Then
                    cell = xlw.Cells(cy, 4)
                    cell.Value = ParseXmlDecimal(eventNode, "creditiecm")
                    cell.Style.Font.Bold = True
                End If
            End If

            'bordo inferiore
            If profiloecm Then
                cell = xlw.Cells(cy, 1, cy, 4)
            Else
                cell = xlw.Cells(cy, 1, cy, 3)
            End If
            cell.Style.Border.Bottom.Style = Style.ExcelBorderStyle.Hair

            cy += 1

        Next


    End Function


End Class