Imports OfficeOpenXml
Imports System.IO
Imports Softailor.Global.XmlParser
Public Class Persone
    Inherits System.Web.UI.Page


    Dim dbConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'script
        'gestione degli script
        If Not Page.IsPostBack Then
            Dim sScript = "function editPersona_callback(codice) {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkReposition, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf
            Me.ltrRepositioning.Text = sScript
        End If

        srcPERSONE.AddNewLineAfter("ac_PROFILO")

    End Sub

    Private Sub lnkReposition_Click(sender As Object, e As EventArgs) Handles lnkReposition.Click
        'deseleziono
        grdPERSONE.SelectedIndex = -1
        'forzo ricerca
        grdPERSONE.DataBind()
        grdPERSONE.UpdateParentPanel()

        'riposiziono
        Dim sIdx As Integer = -1
        Dim cIdx As Integer
        For cIdx = 0 To grdPERSONE.DataKeys.Count - 1
            If grdPERSONE.DataKeys(cIdx).Value.ToString = txtReposition.Text Then
                sIdx = cIdx
                Exit For
            End If
        Next

        If sIdx >= 0 Then
            grdPERSONE.SelectedIndex = sIdx
            grdPERSONE.EnsureSelectedRowVisible()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "disappeared", "window.alert('A causa delle modifiche effettuate, l\'elemento selezionato non rispetta i filtri impostati e non è pertanto più visibile.');", True)
        End If
    End Sub

    Private Sub grdPERSONE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdPERSONE.SelectedIndexChanged

        If grdPERSONE.SelectedIndex <> -1 Then

            Dim id_PERSONA As String = grdPERSONE.SelectedDataKey.Value.ToString

            'scrivo il valore
            txtReposition.Text = id_PERSONA
            updHiddenCtls.Update()

            'script apertura
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupPersona",
                    "schedaPersona('" & id_PERSONA.ToString & "');", True)

        End If

    End Sub

    Private Sub srcPERSONE_CreateNew(dataDbConn As SqlConnection, sqlValues As Dictionary(Of String, Object), ByRef errorEncountered As Boolean, ByRef errorMessage As String, ByRef gotoNewKey As Boolean, ByRef newKeyFieldName As String, ByRef newKeyValue As String) Handles srcPERSONE.CreateNew

        Dim id_PERSONA As New SqlInt32
        Dim errorMsg As String = ""

        With sqlValues
            CreaNuovo_PERSONE(dataDbConn,
                               ContextHandler.ID_UTENT,
                               CType(.Item("ac_TITOLO"), SqlString),
                               CType(.Item("tx_COGNOME"), SqlString),
                               CType(.Item("tx_NOME"), SqlString),
                               CType(.Item("ac_PROFILO"), SqlString),
                               CType(.Item("ac_CODICEFISCALE"), SqlString),
                               id_PERSONA,
                               errorMsg)

        End With

        If id_PERSONA.IsNull Then
            errorEncountered = True
            errorMessage = errorMsg
        Else
            errorEncountered = False
            gotoNewKey = True
            newKeyFieldName = "id_PERSONA"
            newKeyValue = id_PERSONA.Value.ToString

            'scrivo il valore
            txtReposition.Text = id_PERSONA.Value.ToString
            updHiddenCtls.Update()

            'script apertura
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPopupPersona",
                    "schedaPersona('" & id_PERSONA.Value.ToString & "');", True)

        End If

    End Sub

    Friend Sub CreaNuovo_PERSONE(ByVal dbConn As SqlConnection,
                                  ByVal id_UTENT As Integer,
                                  ByVal ac_TITOLO As SqlString,
                                  ByVal tx_COGNOME As SqlString,
                                  ByVal tx_NOME As SqlString,
                                  ByVal ac_PROFILO As SqlString,
                                  ByVal ac_CODICEFISCALE As SqlString,
                                  ByRef id_PERSONA As SqlInt32,
                                  ByRef errorMsg As String)

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader


        id_PERSONA = SqlInt32.Null
        errorMsg = ""

        If Not ac_CODICEFISCALE.IsNull Then
            If Not Softailor.Global.ValidationUtils.ValidateCodiceFiscaleItaliano(ac_CODICEFISCALE.Value) Then
                errorMsg &= vbCrLf & "- codice fiscale non valido"
            Else
                'verifico presenza CF in archivio
                dbCmd = dbConn.CreateCommand
                With dbCmd
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT TOP 1 id_PERSONA, tx_COGNOME, tx_NOME FROM age_PERSONE WHERE ac_CODICEFISCALE=@ac_CODICEFISCALE"
                    .Parameters.Add("@ac_CODICEFISCALE", SqlDbType.NVarChar, 16).Value = ac_CODICEFISCALE
                End With
                dbRdr = dbCmd.ExecuteReader
                If dbRdr.Read Then
                    errorMsg &= vbCrLf & "- codice fiscale già presente in archivio (" & dbRdr.GetString(1) & " " & dbRdr.GetString(2) & ")"
                End If
                dbRdr.Close()
                dbCmd.Dispose()
            End If
        End If

        If Not ac_PROFILO.IsNull Then
            'verifico che il profilo sia valido per gli esterni
            dbCmd = dbConn.CreateCommand
            With dbCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT fl_ESTERNO FROM age_PROFILI WHERE ac_PROFILO=@ac_PROFILO"
                .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = ac_PROFILO
            End With
            dbRdr = dbCmd.ExecuteReader
            dbRdr.Read()
            If Not dbRdr.GetBoolean(0) Then
                errorMsg &= vbCrLf & "- il profilo selezionato non è utilizzabile per persone non dipendenti " & My.Settings.CompanyName
            End If
            dbRdr.Close()
            dbCmd.Dispose()
        End If

        'se tutto va bene, crea persona maiuscolizzando cognome, nome, cf
        'dal profilo determina il ruolo, la professione, la disciplina
        'se c'è un titolo determina il sesso
        'dal CF determina sesso, data di nascita, luogo di nascita

        If errorMsg <> "" Then
            errorMsg = "Impossibile creare la persona per i seguenti errori:" & vbCrLf & errorMsg
        Else

            'maiuscolizzo i dati presenti
            If Not tx_COGNOME.IsNull Then tx_COGNOME = New SqlString(tx_COGNOME.Value.ToUpper)
            If Not tx_NOME.IsNull Then tx_NOME = New SqlString(tx_NOME.Value.ToUpper)
            If Not ac_CODICEFISCALE.IsNull Then ac_CODICEFISCALE = New SqlString(ac_CODICEFISCALE.Value.ToUpper)

            Dim datiCF As New Softailor.Global.ValidationUtils.DatiCodiceFiscale

            'dati da codice fiscale
            If Not ac_CODICEFISCALE.IsNull Then
                datiCF = Softailor.Global.ValidationUtils.DatiDaCodiceFiscaleValido(ac_CODICEFISCALE.Value)
            End If

            'creazione effettiva

            dbCmd = dbConn.CreateCommand()
            With dbCmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_age_PERSONE_insert"
                .Parameters.Add("@tx_CREAZIONE", SqlDbType.NVarChar, 50).Value = ContextHandler.USERNAME
                .Parameters.Add("@ac_TITOLO", SqlDbType.NVarChar, 10).Value = ac_TITOLO
                .Parameters.Add("@tx_COGNOME", SqlDbType.NVarChar, 100).Value = tx_COGNOME
                .Parameters.Add("@tx_NOME", SqlDbType.NVarChar, 100).Value = tx_NOME
                .Parameters.Add("@ac_PROFILO", SqlDbType.NVarChar, 20).Value = ac_PROFILO
                .Parameters.Add("@ac_CODICEFISCALE", SqlDbType.NVarChar, 16).Value = ac_CODICEFISCALE
                .Parameters.Add("@dt_NASCITA", SqlDbType.DateTime).Value = datiCF.dataNascita
                .Parameters.Add("@ac_COMUNENASCITA", SqlDbType.NVarChar, 4).Value = datiCF.belfioreNascita
                .Parameters.Add("@ac_GENERE", SqlDbType.NVarChar, 1).Value = datiCF.genereMF
                .Parameters.Add("@ac_CATEGORIALAVORATIVA", SqlDbType.NVarChar, 8).Value = DBNull.Value
                With .Parameters.Add("@id_PERSONA", SqlDbType.Int)
                    .Direction = ParameterDirection.Output
                End With
                Try
                    .ExecuteNonQuery()
                    id_PERSONA = CType(.Parameters("@id_PERSONA").SqlValue, SqlInt32)
                Catch ex As Exception
                    errorMsg = "Impossibile creare la delibera. Errore restituito:" & ex.ToString
                End Try
                dbCmd.Dispose()
            End With
        End If

    End Sub

    Private Sub Persone_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        'chiusura connessione
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn = Nothing
        End If
    End Sub

    Protected Sub grdPERSONE_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If (e.CommandName = "genera_portfolio") Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim id_persona As Integer = Convert.ToInt32(grdPERSONE.DataKeys(index).Value)


            Response.Clear()
            Response.Expires = 0
            Response.Cache.SetNoStore()


            Dim profiloecm As Boolean

            Dim xlp As ExcelPackage
            Dim xlw As ExcelWorksheet

            'Dim myWebClient As New WebClient()
            'lettura documento
            Dim xDoc = LeggiDati(id_persona)

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
            'Response.Buffer = True
            'Response.Clear()
            'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.ContentType = "application/octet-stream"

            'Response.AddHeader("content-disposition", "attachment;  filename=Portfolio " & cognomeNome & " " & Date.Today.ToString("d MMM yyyy") & ".xlsx")
            Session("portfolio_binaryData") = xlp
            Session("portfolio_nomeCognome") = cognomeNome
            Response.Redirect("PortfolioExcel.aspx")
            'Response.BinaryWrite(xlp.GetAsByteArray())
            'Response.OutputStream.Write(xlp.GetAsByteArray(), 0, xlp.GetAsByteArray().Length)


        End If

    End Sub
    Private Function LeggiDati(id_persona As Int32) As XmlDocument

        'Dim dbConn As SqlConnection
        Dim dbCmd As SqlCommand
        Dim sXml As String
        Dim xDoc As XmlDocument

        'apertura connessione
        'dbConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        'dbConn.Open()

        'preparazione comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_bo_PortfolioExcel"
            .Parameters.Add("@id_persona", SqlDbType.Int).Value = id_persona
        End With
        sXml = Transformer.Transform(dbCmd, "..\Files\PortfolioExcel.xslt",
                              "companyname", "Izsler")
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