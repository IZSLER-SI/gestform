Imports Softailor.ReportEngine

Public Class MailReportMP
    Inherits System.Web.UI.MasterPage

    Private Class FiltroStd
        Public id_FILTRO As Integer
        Public tx_FILTRO As String
        Public xm_FILTRO As String
    End Class

    Dim rptDbConn As SqlConnection
    Dim ac_FONTE As String
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

        If Not Page.IsPostBack Then pnlGenerazione.Visible = False

        'recupero parametri dalla pagina
        CType(Me.Page, IReportGenerationPage).GetReportParameters(ac_FONTE, valoreFiltroBase, storageSubFolder)

        'recupero parametri specifici mail dalla pagina
        Dim prms = CType(Me.Page, IMailReportGenerationPage).GetMailParameters()
        sd_ragionesociale.Text = prms.ragionesociale
        sd_indirizzocompleto.Text = prms.indirizzocompleto
        sd_tel.Text = prms.tel
        sd_fax.Text = prms.fax
        sd_email.Text = prms.email

        'carico i dati della fonte
        fonteXDoc = New XmlDocument
        fonteXDoc.Load(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'istanzio la fonte
        myFonte = ReportEngine.Fonte.FromXml(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'imposto parametri nel dataSource
        sdsMAILREPORTS_g.SelectParameters("ac_FONTE").DefaultValue = ac_FONTE

        'script apertura editor filtro
        spanEditFiltro.Attributes.Add("onclick", "editFiltro(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_FONTE) & ");")

        'genero i controlli per le opzioni
        GeneraControlliOpzioni()

        'carico i possibili filtri
        CaricaPossibiliFiltri()

        'genero i controlli relativi ai filtri
        GeneraControlliFiltri()

    End Sub

    Private Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not rptDbConn Is Nothing Then
            If rptDbConn.State = ConnectionState.Open Then rptDbConn.Close()
            rptDbConn.Dispose()
        End If
    End Sub

    Private Sub grdREPORTS_DataBound(sender As Object, e As EventArgs) Handles grdMAILREPORTS.DataBound
        If Not Page.IsPostBack Then grdMAILREPORTS.SelectedIndex = -1
    End Sub

    Private Sub grdREPORTS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdMAILREPORTS.SelectedIndexChanged
        If grdMAILREPORTS.SelectedIndex <> -1 Then
            SetupStampa(CInt(grdMAILREPORTS.SelectedDataKey.Value))
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
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "editFiltroPersonalizzato",
                                                    "editFiltro(" & Softailor.Global.JSUtils.EncodeJsStringWithQuotes(ac_FONTE) & ");",
                                                    True)

    End Sub

    Private Sub SetupStampa(id_MAILREPORT As Integer)

        Dim myModello As SharepointHelper.SharepointFile

        hidid_MAILREPORT.Value = id_MAILREPORT.ToString

        myModello = SharepointHelper.GetModelloMail(rptDbConn, id_MAILREPORT)

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

    Private Sub lnkIniziaSpedizione_Click(sender As Object, e As EventArgs) Handles lnkIniziaSpedizione.Click

        'pulizia dei campi nascosti
        sd_ac_FONTE.Text = String.Empty
        sd_tx_VALOREFILTROBASE.Text = String.Empty
        sd_id_MAILREPORT.Text = String.Empty
        sd_xm_FILTRO.Text = String.Empty
        sd_ac_ORDINAMENTO.Text = String.Empty
        sd_tx_ORDINAMENTO1.Text = String.Empty
        sd_tx_ORDINAMENTO2.Text = String.Empty
        sd_tx_ORDINAMENTO3.Text = String.Empty
        sd_tx_ORDINAMENTO4.Text = String.Empty
        sd_tx_ORDINAMENTO5.Text = String.Empty

        'fonte, report, campo chiave
        sd_ac_FONTE.Text = Me.ac_FONTE
        sd_tx_VALOREFILTROBASE.Text = valoreFiltroBase
        sd_id_MAILREPORT.Text = hidid_MAILREPORT.Value

        'filtro
        If String.IsNullOrEmpty(myFonte.VistaCorpo) Then
            sd_xm_FILTRO.Text = String.Empty
        Else
            'determino l'XML
            If rbFiltroPers.Checked Then
                sd_xm_FILTRO.Text = xmlFiltroPers.Text
            ElseIf rblFiltroNone.Checked Then
                sd_xm_FILTRO.Text = ""
            Else
                For Each id_FILTRO In rbFiltriStd.Keys
                    If rbFiltriStd(id_FILTRO).Checked Then
                        sd_xm_FILTRO.Text = filtriStd(id_FILTRO).xm_FILTRO
                        Exit For
                    End If
                Next
            End If
        End If

        'ordinamento
        If rblOrdinamento IsNot Nothing Then
            If rblOrdinamento.SelectedValue = "_CUSTOM_" Then
                sd_tx_ORDINAMENTO1.Text = ddnOrdinamento1.SelectedValue
                sd_tx_ORDINAMENTO2.Text = ddnOrdinamento2.SelectedValue
                sd_tx_ORDINAMENTO3.Text = ddnOrdinamento3.SelectedValue
                sd_tx_ORDINAMENTO4.Text = ddnOrdinamento4.SelectedValue
                sd_tx_ORDINAMENTO5.Text = ddnOrdinamento5.SelectedValue
            Else
                sd_ac_ORDINAMENTO.Text = rblOrdinamento.SelectedValue
            End If
        End If

        'lancio js
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "launchSpedizione", "IniziaSpedizione();", True)
    End Sub
End Class