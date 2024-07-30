Imports Softailor.Global.SqlUtils
Imports OfficeOpenXml

Public Class ReportStandardEvento
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    'sub-controlli tabulato iscritti
    Protected WithEvents lnkTI_OIAll As LinkButton
    Protected WithEvents lnkTI_OINone As LinkButton
    Protected WithEvents cblTI_Origine As CheckBoxList
    Protected WithEvents lnkTI_CEAll As LinkButton
    Protected WithEvents lnkTI_CENone As LinkButton
    Protected WithEvents cblTI_CategoriaEcm As CheckBoxList
    Protected WithEvents lnkTI_SIAll As LinkButton
    Protected WithEvents lnkTI_SINone As LinkButton
    Protected WithEvents cblTI_StatoIscrizione As CheckBoxList
    Protected WithEvents lnkTI_SEAll As LinkButton
    Protected WithEvents lnkTI_SENone As LinkButton
    Protected WithEvents cblTI_StatoEcm As CheckBoxList
    Protected WithEvents lnkTI_SQAll As LinkButton
    Protected WithEvents lnkTI_SQNone As LinkButton
    Protected WithEvents cblTI_StatoQuestionario As CheckBoxList
    Protected WithEvents lnkTI_SPAll As LinkButton
    Protected WithEvents lnkTI_SPNone As LinkButton
    Protected WithEvents cblTI_StatoPresenza As CheckBoxList

    Protected WithEvents btnTI_Clear As Button
    Protected WithEvents btnTI_DoReport As Button

    'sub-controlli foglio firme
    Protected WithEvents lnkFF_CEAll As LinkButton
    Protected WithEvents lnkFF_CENone As LinkButton
    Protected WithEvents cblFF_CategoriaEcm As CheckBoxList
    Protected WithEvents lnkFF_SIAll As LinkButton
    Protected WithEvents lnkFF_SINone As LinkButton
    Protected WithEvents cblFF_StatoIscrizione As CheckBoxList
    Protected WithEvents lnkFF_SEAll As LinkButton
    Protected WithEvents lnkFF_SENone As LinkButton
    Protected WithEvents cblFF_StatoEcm As CheckBoxList
    Protected WithEvents txtFF_Intestazione As TextBox
    Protected WithEvents ddnFF_RigheBianche As DropDownList
    Protected WithEvents btnFF_Clear As Button
    Protected WithEvents btnFF_DoReport As Button

    'sub-controlli badge
    Protected WithEvents lnkBA_CEAll As LinkButton
    Protected WithEvents lnkBA_CENone As LinkButton
    Protected WithEvents cblBA_CategoriaEcm As CheckBoxList
    Protected WithEvents lnkBA_SIAll As LinkButton
    Protected WithEvents lnkBA_SINone As LinkButton
    Protected WithEvents cblBA_StatoIscrizione As CheckBoxList
    Protected WithEvents lnkBA_SEAll As LinkButton
    Protected WithEvents lnkBA_SENone As LinkButton
    Protected WithEvents cblBA_StatoEcm As CheckBoxList
    Protected WithEvents btnBA_Clear As Button
    Protected WithEvents btnBA_DoReport As Button
    Protected WithEvents btnBA_DoReportBC As Button

    'sub-controlli attestati ECM
    Protected WithEvents lnkAE_CEAll As LinkButton
    Protected WithEvents lnkAE_CENone As LinkButton
    Protected WithEvents cblAE_CategoriaEcm As CheckBoxList
    Protected WithEvents lnkAE_SIAll As LinkButton
    Protected WithEvents lnkAE_SINone As LinkButton
    Protected WithEvents cblAE_StatoIscrizione As CheckBoxList
    Protected WithEvents lnkAE_SEAll As LinkButton
    Protected WithEvents lnkAE_SENone As LinkButton
    Protected WithEvents cblAE_StatoEcm As CheckBoxList
    Protected WithEvents btnAE_Clear As Button
    Protected WithEvents btnAE_DoReport As Button

    Dim dbConn As SqlConnection

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione base XML
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_SearchIscrittiData"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim xReader = dbCmd.ExecuteXmlReader
        Dim filtriXDoc As New XmlDocument
        filtriXDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()



        'creazione controlli
        CreateControls_TabulatoIscritti(filtriXDoc)
        CreateControls_FoglioFirme(filtriXDoc)
        CreateControls_Badges(filtriXDoc)
        CreateControls_AttestatiEcm(filtriXDoc)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Tabulato Iscritti Excel"
    Private Sub CreateControls_TabulatoIscritti(filtriXDoc As XmlDocument)

        Dim sAspx = Transformer.Transform(filtriXDoc, "Templates/PrintIscrittiGF.xslt", "a", "a")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")
        Dim cCreato = Me.Page.ParseControl(sAspx)

        phdTabulatoIscritti.Controls.Clear()
        phdTabulatoIscritti.Controls.Add(cCreato)

        'aggancio controlli
        lnkTI_OIAll = CType(cCreato.FindControl("lnkTIOIAll"), LinkButton)
        lnkTI_OINone = CType(cCreato.FindControl("lnkTIOINone"), LinkButton)
        cblTI_Origine = CType(cCreato.FindControl("cblTIOrigine"), CheckBoxList)

        lnkTI_CEAll = CType(cCreato.FindControl("lnkTICEAll"), LinkButton)
        lnkTI_CENone = CType(cCreato.FindControl("lnkTICENone"), LinkButton)
        cblTI_CategoriaEcm = CType(cCreato.FindControl("cblTICategoriaEcm"), CheckBoxList)

        lnkTI_SIAll = CType(cCreato.FindControl("lnkTISIAll"), LinkButton)
        lnkTI_SINone = CType(cCreato.FindControl("lnkTISINone"), LinkButton)
        cblTI_StatoIscrizione = CType(cCreato.FindControl("cblTIStatoIscrizione"), CheckBoxList)

        lnkTI_SEAll = CType(cCreato.FindControl("lnkTISEAll"), LinkButton)
        lnkTI_SENone = CType(cCreato.FindControl("lnkTISENone"), LinkButton)
        cblTI_StatoEcm = CType(cCreato.FindControl("cblTIStatoEcm"), CheckBoxList)

        lnkTI_SPAll = CType(cCreato.FindControl("lnkTISPAll"), LinkButton)
        lnkTI_SPNone = CType(cCreato.FindControl("lnkTISPNone"), LinkButton)
        cblTI_StatoPresenza = CType(cCreato.FindControl("cblTIStatoPresenza"), CheckBoxList)

        lnkTI_SQAll = CType(cCreato.FindControl("lnkTISQAll"), LinkButton)
        lnkTI_SQNone = CType(cCreato.FindControl("lnkTISQNone"), LinkButton)
        cblTI_StatoQuestionario = CType(cCreato.FindControl("cblTIStatoQuestionario"), CheckBoxList)

        btnTI_DoReport = CType(cCreato.FindControl("btnTIDoReport"), Button)
        btnTI_Clear = CType(cCreato.FindControl("btnTIClear"), Button)

        AddHandler lnkTI_OIAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkTI_CEAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkTI_SIAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkTI_SEAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkTI_SPAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkTI_SQAll.Click, AddressOf lnkSelAll_Click

        AddHandler lnkTI_OINone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkTI_CENone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkTI_SINone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkTI_SENone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkTI_SPNone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkTI_SQNone.Click, AddressOf lnkSelNone_Click

    End Sub

    Private Sub btnTI_Clear_click(sender As Object, e As System.EventArgs) Handles btnTI_Clear.Click
        SelCb(cblTI_CategoriaEcm, True)
        SelCb(cblTI_Origine, True)
        SelCb(cblTI_StatoEcm, True)
        SelCb(cblTI_StatoIscrizione, True)
        SelCb(cblTI_StatoPresenza, True)
        SelCb(cblTI_StatoQuestionario, True)
    End Sub

    Private Function DataValid_TabulatoIscritti() As Boolean
        If Not SomeSelected(cblTI_CategoriaEcm) Or _
           Not SomeSelected(cblTI_Origine) Or _
           Not SomeSelected(cblTI_StatoEcm) Or _
           Not SomeSelected(cblTI_StatoPresenza) Or _
           Not SomeSelected(cblTI_StatoQuestionario) Or _
           Not SomeSelected(cblTI_StatoIscrizione) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert('Devi selezionare almeno un\'opzione in ogni riquadro.');", True)
            Return False
        End If
        Return True
    End Function

    Private Function GetSql_TabulatoIscritti() As String

        Dim sOut = <SQL>
SELECT 

--dati personali
tx_TITOLO as [Titolo],
tx_COGNOME as [Cognome],
tx_NOME as [Nome],
ac_GENERE as [Genere],
dt_NASCITA as [Data Nascita],
tx_LUOGONASCITA as [Luogo Nascita],
ac_CODICEFISCALE as [Codice Fiscale],
tx_EMAIL as [E-Mail],

--dati iscrizione
dt_CREAZIONE as [Data/Ora Iscrizione],
tx_ORIGINEISCRIZIONE as [Origine Iscrizione],
tx_CATEGORIAECM as [Ruolo Iscritto],
tx_STATOISCRIZIONE as [Stato Iscrizione],

tx_STATOPRESENZA as [Raggiungimento Presenza Minima per ECM],
ni_TOTALEMINUTI as [Presenza Totalizzata (minuti)],
ni_MINIMOMINUTIEVENTO as [Presenza Minima per ECM (minuti)],

tx_STATOQUESTIONARIO as [Questionario Apprendimento],
ni_RISPOSTEOK as [Risposte OK],
ni_RISPOSTEKO as [Risposte OK],
ni_RISPOSTEND as [Risposte ND],

tx_STATOECM as [Crediti ECM],
--ni_TOTALEMINUTI as [],
--dt_OTTENIMENTOCREDITIECM as [],

--dati rapporto lavorativo
tx_RUOLO as [Ruolo],
tx_PROFILO as [Profilo],
ac_DIPEXT as [Tipo],
ac_MATRICOLA as [Matricola],
tx_CATEGORIALAVORATIVA as [Categoria Lavorativa],
tx_UNITAOPERATIVA as [Unità Operativa],
tx_TIPOCONTRATTO as [Tipo Contratto],
tx_CATEGORIACONTRATTO as [Categoria Contratto],
tx_FASCIACONTRATTO as [Fascia],
tx_PROFESSIONE as [Professione],
tx_DISCIPLINA as [Disciplina],
tx_ALBO_LONG as [Albo],
ac_ISCRIZIONEALBO as [Iscrizione Albo],


tx_INDIRIZZO_res as [RES - Indirizzo],
tx_CODICEPOSTALE_res as [RES - Cap],
tx_LOCALITA_res as [RES - Località],
tx_CITTA_res as [RES - Comune],
tx_PROVINCIA_res as [RES - Provincia],
tx_NAZIONE_res as [RES - Nazione],
tx_TELEFONO_res as [RES - Telefono],
tx_FAX_res as [RES - Fax],
tx_CELLULARE_res as [RES - Cellulare],

tx_INDIRIZZO_dom as [DOM - Indirizzo],
tx_CODICEPOSTALE_dom as [DOM - Cap],
tx_LOCALITA_dom as [DOM - Località],
tx_CITTA_dom as [DOM - Comune],
tx_PROVINCIA_dom as [DOM - Provincia],
tx_NAZIONE_dom as [DOM - Nazione],
tx_TELEFONO_dom as [DOM - Telefono],
tx_FAX_dom as [DOM - Fax],
tx_CELLULARE_dom as [DOM - Cellulare],

tx_ENTE_lav as [LAV - Ente],
tx_INDIRIZZO_lav as [LAV - Indirizzo],
tx_CODICEPOSTALE_lav as [LAV - Cap],
tx_LOCALITA_lav as [LAV - Località],
tx_CITTA_lav as [LAV - Comune],
tx_PROVINCIA_lav as [LAV - Provincia],
tx_NAZIONE_lav as [LAV - Nazione],
tx_TELEFONO_lav as [LAV - Telefono],
tx_FAX_lav as [LAV - Fax],
tx_CELLULARE_lav as [LAV - Cellulare]


FROM
	vw_eve_ISCRITTI_RECAPITI

                   </SQL>.Value

        sOut &= " WHERE id_EVENTO=" & SQL_Int32(GecFinalContextHandler.id_EVENTO)

        'filtro evento


        sOut &= filterAtom("ac_ORIGINEISCRIZIONE", EnqueueValues(cblTI_Origine))
        sOut &= filterAtom("ac_CATEGORIAECM", EnqueueValues(cblTI_CategoriaEcm))
        sOut &= filterAtom("ac_STATOISCRIZIONE", EnqueueValues(cblTI_StatoIscrizione))
        sOut &= filterAtom("ac_STATOECM", EnqueueValues(cblTI_StatoEcm))
        sOut &= filterAtom("ac_STATOPRESENZA", EnqueueValues(cblTI_StatoPresenza))
        sOut &= filterAtom("ac_STATOQUESTIONARIO", EnqueueValues(cblTI_StatoQuestionario))

        sOut &= " ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO"

        Return sOut

    End Function

    Private Sub BtnTI_DoReport_Click(sender As Object, e As System.EventArgs) Handles btnTI_DoReport.Click
        'validazione
        If Not DataValid_TabulatoIscritti() Then Exit Sub

        'OK ci siamo
        Dim sSql = GetSql_TabulatoIscritti()

        'generazione dataset
        Dim dt = getDataTableIscritti(sSql)

        If dt.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errEmpty", "window.alert('Nessun nominativo corrisponde ai filtri impostati.');", True)
            dt.Dispose()
            Exit Sub
        End If

        'OK ci siamo
        CreateExcelTabulatoIscritti(dt)

    End Sub

    Private Sub CreateExcelTabulatoIscritti(DT As DataTable)

        Dim rng As ExcelRange
        Dim xlp As New ExcelPackage
        Dim xlws = xlp.Workbook.Worksheets.Add("Tabulato")
        xlws.Cells("A1").LoadFromDataTable(DT, True)

        'formattazione delle colonne
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Columns.Count - 1
                If DT.Columns(i).DataType.Equals(GetType(System.DateTime)) Then
                    rng = xlws.Cells(2, i + 1, 2 + DT.Rows.Count - 1, i + 1)
                    rng.Style.Numberformat.Format = "dd/MM/yyyy"
                ElseIf DT.Columns(i).DataType.Equals(GetType(System.Decimal)) Then
                    rng = xlws.Cells(2, i + 1, 2 + DT.Rows.Count - 1, i + 1)
                    rng.Style.Numberformat.Format = "#,##0.00"
                ElseIf DT.Columns(i).DataType.Equals(GetType(System.TimeSpan)) Then
                    rng = xlws.Cells(2, i + 1, 2 + DT.Rows.Count - 1, i + 1)
                    rng.Style.Numberformat.Format = "HH:mm"
                End If
            Next
        End If

        'intestazioni
        rng = xlws.Cells(1, 1, 1, DT.Columns.Count)
        rng.Style.Font.Bold = True
        rng.Style.Font.Color.SetColor(System.Drawing.Color.White)
        rng.Style.Fill.PatternType = Style.ExcelFillStyle.Solid
        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(31, 73, 125))

        'font e filtro
        rng = xlws.Cells(1, 1, DT.Rows.Count + 1, DT.Columns.Count)
        rng.Style.Font.Size = 10
        rng.AutoFitColumns()
        rng.AutoFilter = True

        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;  filename=TabulatoIscritti_" & Date.Now.ToString("dd_MM_yyyy_HH_mm") & ".xlsx")
        Response.BinaryWrite(xlp.GetAsByteArray())
        xlp.Dispose()
        xlp = Nothing

        dbConn.Close()
        dbConn.Dispose()
        Response.End()

    End Sub
#End Region

#Region "Foglio Firme PDF"
    Private Sub CreateControls_FoglioFirme(filtriXDoc As XmlDocument)

        Dim sAspx = Transformer.Transform(filtriXDoc, "Templates/PrintFoglioFirmeGF.xslt", "a", "a")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")
        Dim cCreato = Me.Page.ParseControl(sAspx)

        phdFoglioFirme.Controls.Clear()
        phdFoglioFirme.Controls.Add(cCreato)

        'aggancio controlli
        lnkFF_CEAll = CType(cCreato.FindControl("lnkFFCEAll"), LinkButton)
        lnkFF_CENone = CType(cCreato.FindControl("lnkFFCENone"), LinkButton)
        cblFF_CategoriaEcm = CType(cCreato.FindControl("cblFFCategoriaEcm"), CheckBoxList)
        lnkFF_SIAll = CType(cCreato.FindControl("lnkFFSIAll"), LinkButton)
        lnkFF_SINone = CType(cCreato.FindControl("lnkFFSINone"), LinkButton)
        cblFF_StatoIscrizione = CType(cCreato.FindControl("cblFFStatoIscrizione"), CheckBoxList)
        lnkFF_SEAll = CType(cCreato.FindControl("lnkFFSEAll"), LinkButton)
        lnkFF_SENone = CType(cCreato.FindControl("lnkFFSENone"), LinkButton)
        cblFF_StatoEcm = CType(cCreato.FindControl("cblFFStatoEcm"), CheckBoxList)
        txtFF_Intestazione = CType(cCreato.FindControl("txtFFIntestazione"), TextBox)
        ddnFF_RigheBianche = CType(cCreato.FindControl("ddnFFRigheBianche"), DropDownList)
        btnFF_DoReport = CType(cCreato.FindControl("btnFFDoReport"), Button)
        btnFF_Clear = CType(cCreato.FindControl("btnFFClear"), Button)

        AddHandler lnkFF_CEAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkFF_SIAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkFF_SEAll.Click, AddressOf lnkSelAll_Click

        AddHandler lnkFF_CENone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkFF_SINone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkFF_SENone.Click, AddressOf lnkSelNone_Click

    End Sub

    Private Sub btnFF_Clear_click(sender As Object, e As System.EventArgs) Handles btnFF_Clear.Click
        SelCb(cblFF_CategoriaEcm, True)
        SelCb(cblFF_StatoEcm, True)
        SelCb(cblFF_StatoIscrizione, True)
        txtFF_Intestazione.Text = ""
    End Sub

    Private Function DataValid_FoglioFirme() As Boolean
        If Not SomeSelected(cblFF_CategoriaEcm) Or _
           Not SomeSelected(cblFF_StatoEcm) Or _
           Not SomeSelected(cblFF_StatoIscrizione) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert('Devi selezionare almeno un\'opzione in ogni riquadro.');", True)
            Return False
        End If
        Return True
    End Function

    Private Function GetSql_FoglioFirme() As String

        Dim sOut = "SELECT * " & _
            "FROM vw_eve_ISCRITTI_RECAPITI WHERE id_EVENTO=" & SQL_Int32(GecFinalContextHandler.id_EVENTO)

        sOut &= filterAtom("ac_CATEGORIAECM", EnqueueValues(cblFF_CategoriaEcm))
        sOut &= filterAtom("ac_STATOISCRIZIONE", EnqueueValues(cblFF_StatoIscrizione))
        sOut &= filterAtom("ac_STATOECM", EnqueueValues(cblFF_StatoEcm))

        sOut &= " ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO"

        Return sOut

    End Function

    Private Sub Btnff_DoReport_Click(sender As Object, e As System.EventArgs) Handles btnFF_DoReport.Click
        'validazione
        If Not DataValid_FoglioFirme() Then Exit Sub

        'OK ci siamo
        Dim sSql = GetSql_FoglioFirme()

        'generazione dataset
        Dim dst = getDatasetIscritti(sSql)

        If dst.vw_eve_ISCRITTI_RECAPITI.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errEmpty", "window.alert('Nessun nominativo corrisponde ai filtri impostati.');", True)
            dst.Dispose()
            Exit Sub
        End If

        Dim righeAgg = CInt(ddnFF_RigheBianche.SelectedValue)
        For i = 1 To righeAgg
            Dim newrow = dst.vw_eve_ISCRITTI_RECAPITI.Newvw_eve_ISCRITTI_RECAPITIRow
            dst.vw_eve_ISCRITTI_RECAPITI.Addvw_eve_ISCRITTI_RECAPITIRow(newrow)
        Next

        'OK ci siamo
        Dim title = txtFF_Intestazione.Text.Trim
        CreatePdfFoglioFirme(dst, title)

    End Sub

    Private Sub CreatePdfFoglioFirme(dst As dstIscrittiEvento, title As String)
        'OK ci siamo
        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("rptFogliFirme.rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        rpt.SetDataSource(dst)
        rpt.SetParameterValue("intestazione", title)
        'stampa report
        rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "FogliFirme_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss"))
        GC.Collect()
        rpt.Close()
        rpt.Dispose()
        dst.Dispose()
    End Sub

#End Region

#Region "Badges PDF"
    Private Sub CreateControls_Badges(filtriXDoc As XmlDocument)

        'lettura dati

        Dim sAspx = Transformer.Transform(filtriXDoc, "Templates/PrintBadgesGF.xslt", "a", "a")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")
        Dim cCreato = Me.Page.ParseControl(sAspx)

        PhdBadges.Controls.Clear()
        PhdBadges.Controls.Add(cCreato)

        'aggancio controlli
        lnkBA_CEAll = CType(cCreato.FindControl("lnkBACEAll"), LinkButton)
        lnkBA_CENone = CType(cCreato.FindControl("lnkBACENone"), LinkButton)
        cblBA_CategoriaEcm = CType(cCreato.FindControl("cblBACategoriaEcm"), CheckBoxList)
        lnkBA_SIAll = CType(cCreato.FindControl("lnkBASIAll"), LinkButton)
        lnkBA_SINone = CType(cCreato.FindControl("lnkBASINone"), LinkButton)
        cblBA_StatoIscrizione = CType(cCreato.FindControl("cblBAStatoIscrizione"), CheckBoxList)
        lnkBA_SEAll = CType(cCreato.FindControl("lnkBASEAll"), LinkButton)
        lnkBA_SENone = CType(cCreato.FindControl("lnkBASENone"), LinkButton)
        cblBA_StatoEcm = CType(cCreato.FindControl("cblBAStatoEcm"), CheckBoxList)

        btnBA_DoReport = CType(cCreato.FindControl("btnBADoReport"), Button)
        btnBA_DoReportBC = CType(cCreato.FindControl("btnBADoReportBC"), Button)
        btnBA_Clear = CType(cCreato.FindControl("btnBAClear"), Button)

        AddHandler lnkBA_CEAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkBA_SIAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkBA_SEAll.Click, AddressOf lnkSelAll_Click

        AddHandler lnkBA_CENone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkBA_SINone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkBA_SENone.Click, AddressOf lnkSelNone_Click

    End Sub

    Private Sub btnBA_Clear_click(sender As Object, e As System.EventArgs) Handles btnBA_Clear.Click
        SelCb(cblBA_CategoriaEcm, True)
        SelCb(cblBA_StatoEcm, True)
        SelCb(cblBA_StatoIscrizione, True)
    End Sub

    Private Function DataValid_Badges() As Boolean
        If Not SomeSelected(cblBA_CategoriaEcm) Or _
           Not SomeSelected(cblBA_StatoEcm) Or _
           Not SomeSelected(cblBA_StatoIscrizione) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert('Devi selezionare almeno un\'opzione in ogni riquadro.');", True)
            Return False
        End If
        Return True
    End Function

    Private Function GetSql_Badges() As String

        Dim sOut = "SELECT * " & _
            "FROM vw_eve_ISCRITTI_RECAPITI WHERE id_EVENTO=" & SQL_Int32(GecFinalContextHandler.id_EVENTO)

        sOut &= filterAtom("ac_CATEGORIAECM", EnqueueValues(cblBA_CategoriaEcm))
        sOut &= filterAtom("ac_STATOISCRIZIONE", EnqueueValues(cblBA_StatoIscrizione))
        sOut &= filterAtom("ac_STATOECM", EnqueueValues(cblBA_StatoEcm))

        sOut &= " ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO"

        Return sOut

    End Function

    Private Sub BtnBA_DoReport_Click(sender As Object, e As System.EventArgs) Handles btnBA_DoReport.Click

        'validazione
        If Not DataValid_Badges() Then Exit Sub

        'OK ci siamo
        Dim sSql = GetSql_Badges()

        'generazione dataset
        Dim dst = getDatasetIscritti(sSql)

        If dst.vw_eve_ISCRITTI_RECAPITI.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errEmpty", "window.alert('Nessun nominativo corrisponde ai filtri impostati.');", True)
            dst.Dispose()
            Exit Sub
        End If

        'OK ci siamo
        CreatePdfBadge(dst, False)

    End Sub

    Private Sub btnBA_DoReportBC_Click(sender As Object, e As System.EventArgs) Handles btnBA_DoReportBC.Click
        'validazione
        If Not DataValid_Badges() Then Exit Sub

        'OK ci siamo
        Dim sSql = GetSql_Badges()

        'generazione dataset
        Dim dst = getDatasetIscritti(sSql)

        If dst.vw_eve_ISCRITTI_RECAPITI.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errEmpty", "window.alert('Nessun nominativo corrisponde ai filtri impostati.');", True)
            dst.Dispose()
            Exit Sub
        End If

        'OK ci siamo
        CreatePdfBadge(dst, True)
    End Sub

    Private Sub CreatePdfBadge(dst As dstIscrittiEvento, withBarcode As Boolean)
        'OK ci siamo

        'aggiunta del barcode se necessario
        If withBarcode Then
            Dim bcStarter = GecFinalContextHandler.BarcodeStarter
            For Each row As dstIscrittiEvento.vw_eve_ISCRITTI_RECAPITIRow In dst.vw_eve_ISCRITTI_RECAPITI.Rows
                row.ac_BARCODE = Softailor.Global.EpsonItfBarcode.CongressBC2of5(row.id_ISCRITTO, bcStarter)
            Next
        End If

        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        If withBarcode Then
            rpt.Load(Server.MapPath("rptBadgesBarcode.rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        Else
            rpt.Load(Server.MapPath("rptBadges.rpt"), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        End If

        rpt.SetDataSource(dst)

        'stampa report
        rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "Badge_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss"))
        GC.Collect()
        rpt.Close()
        rpt.Dispose()
        dst.Dispose()
    End Sub

#End Region

#Region "Attestati ECM PDF"
    Private Sub CreateControls_AttestatiEcm(filtriXDoc As XmlDocument)

        'lettura dati

        Dim sAspx = Transformer.Transform(filtriXDoc, "Templates/PrintAttestatiEcmGF.xslt", "dummy", "dummy")
        sAspx = sAspx.Replace("xmlns:asp=""remove""", "")
        sAspx = sAspx.Replace("xmlns:ajaxToolkit=""remove""", "")
        Dim cCreato = Me.Page.ParseControl(sAspx)

        phdAttestatiEcm.Controls.Clear()
        phdAttestatiEcm.Controls.Add(cCreato)

        'aggancio controlli
        lnkAE_CEAll = CType(cCreato.FindControl("lnkAECEAll"), LinkButton)
        lnkAE_CENone = CType(cCreato.FindControl("lnkAECENone"), LinkButton)
        cblAE_CategoriaEcm = CType(cCreato.FindControl("cblAECategoriaEcm"), CheckBoxList)
        lnkAE_SIAll = CType(cCreato.FindControl("lnkAESIAll"), LinkButton)
        lnkAE_SINone = CType(cCreato.FindControl("lnkAESINone"), LinkButton)
        cblAE_StatoIscrizione = CType(cCreato.FindControl("cblAEStatoIscrizione"), CheckBoxList)
        lnkAE_SEAll = CType(cCreato.FindControl("lnkAESEAll"), LinkButton)
        lnkAE_SENone = CType(cCreato.FindControl("lnkAESENone"), LinkButton)
        cblAE_StatoEcm = CType(cCreato.FindControl("cblAEStatoEcm"), CheckBoxList)

        btnAE_DoReport = CType(cCreato.FindControl("btnAEDoReport"), Button)
        btnAE_Clear = CType(cCreato.FindControl("btnAEClear"), Button)

        AddHandler lnkAE_CEAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkAE_SIAll.Click, AddressOf lnkSelAll_Click
        AddHandler lnkAE_SEAll.Click, AddressOf lnkSelAll_Click

        AddHandler lnkAE_CENone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkAE_SINone.Click, AddressOf lnkSelNone_Click
        AddHandler lnkAE_SENone.Click, AddressOf lnkSelNone_Click

    End Sub

    Private Sub btnAE_Clear_click(sender As Object, e As System.EventArgs) Handles btnAE_Clear.Click
        SelCb(cblAE_CategoriaEcm, True)
        SelCb(cblAE_StatoEcm, True)
        SelCb(cblAE_StatoIscrizione, True)
    End Sub

    Private Function DataValid_AttestatiECM() As Boolean
        If Not SomeSelected(cblAE_CategoriaEcm) Or _
           Not SomeSelected(cblAE_StatoEcm) Or _
           Not SomeSelected(cblAE_StatoIscrizione) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errCreation", "window.alert('Devi selezionare almeno un\'opzione in ogni riquadro.');", True)
            Return False
        End If
        Return True
    End Function

    Private Function GetSql_AttestatiEcm() As String

        Dim sOut = "SELECT * " & _
            "FROM dbo.fn_eve_AttestatiECM (" & SQL_Int32(ContextHandler.ID_AZIEN) & ", " & SQL_Int32(GecFinalContextHandler.id_EVENTO) & ") WHERE (1=1) "

        sOut &= filterAtom("ac_CATEGORIAECM", EnqueueValues(cblAE_CategoriaEcm))
        sOut &= filterAtom("ac_STATOISCRIZIONE", EnqueueValues(cblAE_StatoIscrizione))
        sOut &= filterAtom("ac_STATOECM", EnqueueValues(cblAE_StatoEcm))
        
        sOut &= " ORDER BY tx_COGNOME, tx_NOME, id_ISCRITTO"

        Return sOut

    End Function

    Private Sub BtnAE_DoReport_Click(sender As Object, e As System.EventArgs) Handles btnAE_DoReport.Click
        'validazione
        If Not DataValid_AttestatiECM() Then Exit Sub

        'OK ci siamo
        Dim sSql = GetSql_AttestatiEcm()

        'generazione dataset
        Dim dst = AttestatiEcmHelper.getDatasetAttestatiEcm(dbConn, sSql)

        If dst.vw_gf_AttestatiECM.Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "errEmpty", "window.alert('Nessun nominativo corrisponde ai filtri impostati.');", True)
            dst.Dispose()
            Exit Sub
        End If

        'OK ci siamo
        CreatePdfAttestatiEcm(dst)

    End Sub

    Private Sub CreatePdfAttestatiEcm(dst As dstAttestatiEcm)
        'OK ci siamo
        'generazione report
        Dim rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        rpt.Load(Server.MapPath("~/Reports/" & GecFinalContextHandler.NomeReportAttestatoECM), CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault)
        rpt.SetDataSource(dst)

        'stampa report
        rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "AttestatiEcm_" & Date.Now.ToString("yyyy_MM_dd_HH_mm_ss"))
        GC.Collect()
        rpt.Close()
        rpt.Dispose()
        dst.Dispose()
    End Sub

#End Region


    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Overloads Sub SelCb(lb As LinkButton, sel As Boolean)

        Dim cbl As CheckBoxList = Nothing

        Dim sezKey As String
        Dim cblKey As String

        sezKey = Mid(lb.ID, 4, 2)
        cblKey = Mid(lb.ID, 6, 2)
        Select Case sezKey
            Case "TI"
                Select Case cblKey
                    Case "CE" : cbl = cblTI_CategoriaEcm
                    Case "OI" : cbl = cblTI_Origine
                    Case "SI" : cbl = cblTI_StatoIscrizione
                    Case "SE" : cbl = cblTI_StatoEcm
                    Case "SP" : cbl = cblTI_StatoPresenza
                    Case "SQ" : cbl = cblTI_StatoQuestionario
                        'Case "OA" : cbl = cblTI_OrdineAppartenenza
                End Select
            Case "FF"
                Select Case cblKey
                    Case "CE" : cbl = cblFF_CategoriaEcm
                        'Case "OI" : cbl = cblff_Origine
                    Case "SI" : cbl = cblFF_StatoIscrizione
                    Case "SE" : cbl = cblFF_StatoEcm
                        'Case "OA" : cbl = cblTI_OrdineAppartenenza
                End Select
            Case "BA"
                Select Case cblKey
                    Case "CE" : cbl = cblBA_CategoriaEcm
                        'Case "OI" : cbl = cblff_Origine
                    Case "SI" : cbl = cblBA_StatoIscrizione
                    Case "SE" : cbl = cblBA_StatoEcm
                        'Case "OA" : cbl = cblBA_OrdineAppartenenza
                End Select
            Case "AE"
                Select Case cblKey
                    Case "CE" : cbl = cblAE_CategoriaEcm
                        'Case "OI" : cbl = cblff_Origine
                    Case "SI" : cbl = cblAE_StatoIscrizione
                    Case "SE" : cbl = cblAE_StatoEcm
                        'Case "OA" : cbl = cblAE_OrdineAppartenenza
                End Select
        End Select

        For Each li As ListItem In cbl.Items
            li.Selected = sel
        Next
    End Sub

    Private Overloads Sub SelCb(cbl As CheckBoxList, sel As Boolean)

        For Each li As ListItem In cbl.Items
            li.Selected = sel
        Next
    End Sub

    Private Function SomeSelected(cbl As CheckBoxList) As Boolean
        SomeSelected = False
        For Each li As ListItem In cbl.Items
            If li.Selected Then
                SomeSelected = True
                Exit For
            End If
        Next
    End Function

    Private Sub lnkSelAll_Click(sender As Object, e As System.EventArgs)
        SelCb(CType(sender, LinkButton), True)
    End Sub

    Private Sub lnkSelNone_Click(sender As Object, e As System.EventArgs)
        SelCb(CType(sender, LinkButton), False)
    End Sub

    Private Function getDatasetIscritti(sql As String) As dstIscrittiEvento

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        Dim dst As New dstIscrittiEvento

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = sql
        End With
        dbDad = New SqlDataAdapter(dbCmd)

        dbDad.Fill(dst.vw_eve_ISCRITTI_RECAPITI)

        dbDad.Dispose()
        dbCmd.Dispose()

        'correzione delle date
        For Each r As dstIscrittiEvento.vw_eve_ISCRITTI_RECAPITIRow In dst.vw_eve_ISCRITTI_RECAPITI.Rows
            r.tx_DATAINIZIOFINE = Softailor.Global.DateUtils.DataDalAl(r.dt_INIZIO, r.dt_FINE)
        Next

        Return dst

    End Function

    Private Function getDataTableIscritti(sql As String) As DataTable

        Dim dbCmd As SqlCommand
        Dim dbDad As SqlDataAdapter

        Dim dt As New DataTable

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = sql
        End With
        dbDad = New SqlDataAdapter(dbCmd)

        dbDad.Fill(dt)

        dbDad.Dispose()
        dbCmd.Dispose()

        Return dt

    End Function

    Private Function filterAtom(fieldName As String, hText As String) As String

        If hText = "" Then
            'nessuno selezionato
            Return " AND (1=0)"
        Else
            Dim sOut = " AND " & fieldName & " IN ("
            For Each s As String In hText.Split("\"c)
                If s <> String.Empty Then
                    sOut &= SQL_String(s) & ", "
                End If
            Next
            sOut = Mid(sOut, 1, Len(sOut) - 2) & ")"
            Return sOut
        End If

    End Function

    Private Function filterAtomInt(fieldName As String, hText As String) As String

        If hText = "" Then
            'nessuno selezionato
            Return " AND (1=0)"
        Else
            Dim sOut = " AND ISNULL(" & fieldName & ",-1) IN ("
            For Each s As String In hText.Split("\"c)
                If s <> String.Empty Then
                    sOut &= CInt(s).ToString & ", "
                End If
            Next
            sOut = Mid(sOut, 1, Len(sOut) - 2) & ")"
            Return sOut
        End If

    End Function

    Private Function EnqueueValues(cbl As CheckBoxList) As String

        Dim nTot As Integer = cbl.Items.Count
        Dim sSel As String = ""

        For Each li As ListItem In cbl.Items
            If li.Selected Then
                sSel += "\" & li.Value
            End If
        Next

        Return sSel

    End Function

End Class