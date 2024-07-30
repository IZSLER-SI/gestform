Imports Softailor.ReportEngine

Public Class ReportMP
    Inherits System.Web.UI.MasterPage

    Private Class FiltroStd
        Public id_FILTRO As Integer
        Public tx_FILTRO As String
        Public xm_FILTRO As String
    End Class

    Dim rptDbConn As SqlConnection
    Public ac_FONTE As String
    Dim valoreFiltroBase As String
    Dim storageSubFolder As String
    Dim fonteXDoc As XmlDocument
    Dim myFonte As Fonte

    Dim rblOrdinamento As RadioButtonList
    Dim ddnOrdinamento1 As DropDownList
    Dim ddnOrdinamento2 As DropDownList
    Dim ddnOrdinamento3 As DropDownList
    Dim ddnOrdinamento4 As DropDownList
    Dim ddnOrdinamento5 As DropDownList

    Dim rbFiltriStd As Dictionary(Of Integer, RadioButton)
    Dim filtriStd As Dictionary(Of Integer, FiltroStd)

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        rptDbConn = DbConnectionHandler.GetOpenRptDbConn

        sdsREPORTS_g.SelectParameters("tx_SHAREPOINTBASE").DefaultValue = SharepointHelper.DocsGFBaseUrl & SharepointHelper.f_Modelli
        If Not Page.IsPostBack Then pnlGenerazione.Visible = False

        'recupero parametri dalla pagina
        CType(Me.Page, IReportGenerationPage).GetReportParameters(ac_FONTE, valoreFiltroBase, storageSubFolder)

        'carico i dati della fonte
        fonteXDoc = New XmlDocument
        fonteXDoc.Load(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'istanzio la fonte
        myFonte = ReportEngine.Fonte.FromXml(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'imposto parametri nel dataSource
        sdsREPORTS_g.SelectParameters("ac_FONTE").DefaultValue = ac_FONTE

        'script apertura editor filtro
        spanEditFiltro.Attributes.Add("onclick", "editFiltro(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_FONTE) & ");")

        'genero i controlli per le opzioni
        GeneraControlliOpzioni()

        'carico i possibili filtri
        CaricaPossibiliFiltri()

        'genero i controlli relativi ai filtri
        GeneraControlliFiltri()

    End Sub

    Private Sub SetupStampa(id_REPORT As Integer)

        Dim myModello As SharepointHelper.SharepointFile

        hidid_REPORT.Value = id_REPORT.ToString
        myModello = SharepointHelper.GetModello(rptDbConn, id_REPORT, False)

        txtNomeFile.Text = myModello.OutputFilePrefix.TrimEnd
        lblEstensione.Text = myModello.modelFileExtension

        'imposto l'ordinamento di default
        If rblOrdinamento IsNot Nothing Then
            If myModello.defaultOrdinamento <> "" Then
                rblOrdinamento.SelectedValue = myModello.defaultOrdinamento
            Else
                rblOrdinamento.SelectedIndex = 0
            End If
        End If

        'imposto il filtro di default
        If Not String.IsNullOrEmpty(myFonte.VistaCorpo) Then
            'deseleziono tutto
            rbFiltroPers.Checked = False
            rblFiltroNone.Checked = False
            For Each rb In rbFiltriStd.Values
                rb.Checked = False
            Next

            If myModello.defaultFiltro <> 0 Then
                rbFiltriStd(myModello.defaultFiltro).Checked = True
            Else
                rblFiltroNone.Checked = True
            End If

        End If

        pnlGenerazione.Visible = True
        updGenerazione.Update()

    End Sub

    Private Class GetFileResult
        Public file As IO.MemoryStream
        Public MIMEType As String
        Public OutputFilePrefix As String
        Public modelFileExtension As String
    End Class
    Private Function GetFile() As GetFileResult

        Dim result As GetFileResult

        Dim myModello As SharepointHelper.SharepointFile
        Dim myFile As IO.MemoryStream

        'ottengo il modello
        myModello = SharepointHelper.GetModello(rptDbConn, CInt(hidid_REPORT.Value), True)

        'filtri e ordinamenti
        Dim datiFO = New ReportEngine.Fonte.DatiFiltroOrdinamento

        'ordinamento
        If rblOrdinamento IsNot Nothing Then
            If rblOrdinamento.SelectedValue = "_CUSTOM_" Then
                datiFO.TipoOrdinamento = Fonte.DatiFiltroOrdinamento.TipiOrdinamento.Personalizzato
                datiFO.campoOrdPers1 = ddnOrdinamento1.SelectedValue
                datiFO.campoOrdPers2 = ddnOrdinamento2.SelectedValue
                datiFO.campoOrdPers3 = ddnOrdinamento3.SelectedValue
                datiFO.campoOrdPers4 = ddnOrdinamento4.SelectedValue
                datiFO.campoOrdPers5 = ddnOrdinamento5.SelectedValue
            Else
                datiFO.TipoOrdinamento = Fonte.DatiFiltroOrdinamento.TipiOrdinamento.Standard
                datiFO.codOrdinamentoStandard = rblOrdinamento.SelectedValue
            End If
        Else
            datiFO.TipoOrdinamento = Fonte.DatiFiltroOrdinamento.TipiOrdinamento.Nessuno
        End If

        'filtro
        If String.IsNullOrEmpty(myFonte.VistaCorpo) Then
            datiFO.filtro = Nothing
        Else
            'determino l'XML
            Dim filtroXml = ""
            If rbFiltroPers.Checked Then
                filtroXml = xmlFiltroPers.Text
            ElseIf rblFiltroNone.Checked Then
                filtroXml = ""
            Else
                For Each id_FILTRO In rbFiltriStd.Keys
                    If rbFiltriStd(id_FILTRO).Checked Then
                        filtroXml = filtriStd(id_FILTRO).xm_FILTRO
                        Exit For
                    End If
                Next
            End If

            If filtroXml = "" Then
                datiFO.filtro = Nothing
            Else
                datiFO.filtro = Filtro.FromXml(filtroXml, myFonte.CampiCorpo)
            End If

        End If
        Dim CheckFileMultipli As Boolean
        CheckFileMultipli = CheckFileMultipliButton.Checked

        'genero il file
        Select Case myModello.modelType
            Case "Word"
                myFile = myFonte.GeneraFile(rptDbConn, valoreFiltroBase, Fonte.TipoModello.Word, myModello.memoryStream, datiFO, CheckFileMultipli)
            Case "Excel"
                myFile = myFonte.GeneraFile(rptDbConn, valoreFiltroBase, Fonte.TipoModello.Excel, myModello.memoryStream, datiFO, False)
            Case Else
                Throw New NotImplementedException
        End Select

        result = New GetFileResult
        result.file = myFile
        result.MIMEType = myModello.MIMEType
        result.OutputFilePrefix = myModello.OutputFilePrefix
        result.modelFileExtension = myModello.modelFileExtension
        Return result

    End Function

    Private Sub lnkAnteprima_Click(sender As Object, e As EventArgs) Handles lnkAnteprima.Click

        'genero il file
        Dim getFileResult = GetFile()


        Dim CheckFileMultipli As Boolean
        CheckFileMultipli = CheckFileMultipliButton.Checked

        Response.Clear()
        Response.ContentType = getFileResult.MIMEType
        If (CheckFileMultipli And getFileResult.modelFileExtension = ".docx") Then
            Response.AddHeader("content-disposition", "attachment;  filename=" & getFileResult.OutputFilePrefix & " " & Date.Now.ToString("dd_MM_yyyy HH_mm") & ".zip")
        Else
            Response.AddHeader("content-disposition", "attachment;  filename=" & getFileResult.OutputFilePrefix & " " & Date.Now.ToString("dd_MM_yyyy HH_mm") & getFileResult.modelFileExtension)
        End If
        Response.BinaryWrite(getFileResult.file.ToArray)
        rptDbConn.Close()
        rptDbConn.Dispose()
        getFileResult.file.Dispose()
        Response.End()

    End Sub

    Private Sub lnkGeneraSalva_Click(sender As Object, e As EventArgs) Handles lnkGeneraSalva.Click

        txtNomeFile.Text = txtNomeFile.Text.Trim

        If txtNomeFile.Text = String.Empty Then
            errGeneraSalva.Text = "Immetti un nome file"
        Else
            If Not SharepointHelper.ValidFileName(txtNomeFile.Text) Then
                errGeneraSalva.Text = "Il nome file non può contenere i seguenti caratteri:  " & SharepointHelper.InvalidChars
            Else
                If SharepointHelper.FileExists(storageSubFolder,
                                                     txtNomeFile.Text & lblEstensione.Text) Then
                    errGeneraSalva.Text = "Il file esiste già"
                Else

                    'genero il file
                    Dim getFileResult = GetFile()

                    'salvo il file
                    Dim fileUrl = SharepointHelper.StoreFile(storageSubFolder,
                                        txtNomeFile.Text & lblEstensione.Text,
                                        getFileResult.file)

                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "done",
                                                        "if (confirm('Il report è stato generato e salvato in OneDrive. Vuoi aprirlo?')) { " &
                                                        "location.href=" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(SharepointHelper.SharepointBaseUrl & fileUrl) & ";" &
                                                        "}",
                                                        True)

                End If
            End If
        End If

    End Sub

    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not rptDbConn Is Nothing Then
            If rptDbConn.State = ConnectionState.Open Then rptDbConn.Close()
            rptDbConn.Dispose()
        End If
    End Sub

    Private Sub grdREPORTS_DataBound(sender As Object, e As EventArgs) Handles grdREPORTS.DataBound
        If Not Page.IsPostBack Then grdREPORTS.SelectedIndex = -1
    End Sub

    Private Sub grdREPORTS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdREPORTS.SelectedIndexChanged
        If grdREPORTS.SelectedIndex <> -1 Then
            SetupStampa(CInt(grdREPORTS.SelectedDataKey.Value))
        End If
    End Sub

    Private Sub GeneraControlliOpzioni()

        Dim sAspx As String
        Dim cCreato As Control

        sAspx = Transformer.Transform(fonteXDoc, "Templates/ControlliFonte.xslt", "dummy", "dummy")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)

        cCreato = Me.ParseControl(sAspx)
        phdControlliFonte.Controls.Clear()
        phdControlliFonte.Controls.Add(cCreato)

        'aggancio
        rblOrdinamento = CType(cCreato.FindControl("rblOrdinamento"), RadioButtonList)
        ddnOrdinamento1 = CType(cCreato.FindControl("ddnOrdinamento1"), DropDownList)
        ddnOrdinamento2 = CType(cCreato.FindControl("ddnOrdinamento2"), DropDownList)
        ddnOrdinamento3 = CType(cCreato.FindControl("ddnOrdinamento3"), DropDownList)
        ddnOrdinamento4 = CType(cCreato.FindControl("ddnOrdinamento4"), DropDownList)
        ddnOrdinamento5 = CType(cCreato.FindControl("ddnOrdinamento5"), DropDownList)

    End Sub

    Private Sub CaricaPossibiliFiltri()

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        filtriStd = New Dictionary(Of Integer, FiltroStd)

        dbCmd = rptDbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_rpt_GetFiltriFonte"
            .Parameters.Add("@ac_FONTE", SqlDbType.NVarChar, 20).Value = ac_FONTE
        End With
        dbRdr = dbCmd.ExecuteReader
        Do While dbRdr.Read
            filtriStd.Add(dbRdr.GetInt32(0), New FiltroStd With {.id_FILTRO = dbRdr.GetInt32(0), .tx_FILTRO = dbRdr.GetString(1), .xm_FILTRO = dbRdr.GetString(2)})
        Loop
        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Private Sub GeneraControlliFiltri()

        phdControlliFiltri.Controls.Clear()
        rbFiltriStd = New Dictionary(Of Integer, RadioButton)

        Dim filtro = False
        If myFonte.CampiCorpo IsNot Nothing Then
            For Each cCorpo In myFonte.CampiCorpo
                If cCorpo.Filtro Then
                    filtro = True
                    Exit For
                End If
            Next
        End If

        If Not filtro Then
            pnlFiltri.Visible = False
        Else
            pnlFiltri.Visible = True
            For Each filtroStd In filtriStd.Values
                phdControlliFiltri.Controls.Add(New LiteralControl("<div>"))
                Dim rb As New RadioButton
                rb.GroupName = "filtri"
                rb.ID = "rbFiltro_" & filtroStd.id_FILTRO
                rb.Text = filtroStd.tx_FILTRO
                phdControlliFiltri.Controls.Add(rb)

                phdControlliFiltri.Controls.Add(New LiteralControl(" "))

                Dim lb As New LinkButton
                lb.ID = "lbFiltro_" & filtroStd.id_FILTRO
                lb.CommandArgument = filtroStd.id_FILTRO.ToString
                lb.Text = "Personalizza"
                lb.CssClass = "classicA"
                phdControlliFiltri.Controls.Add(lb)
                AddHandler lb.Click, AddressOf lbFiltro_click

                phdControlliFiltri.Controls.Add(New LiteralControl("</div>"))
                rbFiltriStd.Add(filtroStd.id_FILTRO, rb)
            Next
        End If

    End Sub

    Private Sub lbFiltro_click(sender As Object, e As EventArgs)

        'deseleziono tutti i filtri
        For Each c As Control In phdControlliFiltri.Controls
            If TypeOf c Is RadioButton Then
                With CType(c, RadioButton)
                    If .GroupName = "filtri" Then
                        .Checked = False
                    End If
                End With
            End If
        Next

        'seleziono il filtro personalizzato
        rbFiltroPers.Checked = True

        'copio i dati del filtro
        Dim dbCmd = rptDbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.Text
            .CommandText = "SELECT xm_FILTRO FROM rpt_FILTRI WHERE id_FILTRO=@id_filtro"
            .Parameters.Add("@id_filtro", SqlDbType.Int).Value = CInt(CType(sender, LinkButton).CommandArgument)
        End With
        xmlFiltroPers.Text = CStr(dbCmd.ExecuteScalar)
        dbCmd.Dispose()

        'lancio la personalizzazione
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType,"editFiltroPersonalizzato",
                                                    "editFiltro(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_FONTE) & ");",
                                                    True)

    End Sub
End Class